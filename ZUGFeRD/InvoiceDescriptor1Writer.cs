/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor1Writer : IInvoiceDescriptorWriter
    {
        private ProfileAwareXmlTextWriter _Writer;
        private InvoiceDescriptor _Descriptor;
        private readonly Profile PROFILE_COMFORT_EXTENDED_XRECHNUNG = Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung;


        /// <summary>
        /// Saves the given invoice to the given stream.
        /// Make sure that the stream is open and writeable. Otherwise, an IllegalStreamException will be thron.
        /// </summary>
        /// <param name="descriptor">The invoice object that should be saved</param>
        /// <param name="stream">The target stream for saving the invoice</param>
        /// <param name="format">Format of the target file</param>
        /// <param name="options">Optional `InvoiceFormatOptions` for custom formatting of invoice file</param>
        public override void Save(InvoiceDescriptor descriptor, Stream stream, ZUGFeRDFormats format = ZUGFeRDFormats.CII, InvoiceFormatOptions options = null)
        {
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new IllegalStreamException("Cannot write to stream");
            }

            if (format == ZUGFeRDFormats.UBL)
            {
                throw new UnsupportedException("UBL format is not supported for ZUGFeRD 1");
            }

            // validate data
            if ((descriptor.Profile == Profile.BasicWL) || (descriptor.Profile == Profile.Minimum))
            {
                throw new UnsupportedException("Invalid profile used for ZUGFeRD 1.x invoice.");
            }

            // write data
            long streamPosition = stream.Position;

            this._Descriptor = descriptor;
            this._Writer = new ProfileAwareXmlTextWriter(stream, descriptor.Profile, options?.AutomaticallyCleanInvalidCharacters ?? false);
            this._Writer.SetNamespaces(new Dictionary<string, string>()
            {
                { "xsi", "http://www.w3.org/2001/XMLSchema-instance" },
                { "rsm", "urn:ferd:CrossIndustryDocument:invoice:1p0" },
                { "ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:12" },
                { "udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:15" }
            });
            _Writer.WriteStartDocument();
            _WriteHeaderComments(_Writer, options);

            #region Kopfbereich
            _Writer.WriteStartElement("rsm", "CrossIndustryDocument");
            _Writer.WriteAttributeString("xmlns", "xsi", "http://www.w3.org/2001/XMLSchema-instance");
            _Writer.WriteAttributeString("xmlns", "rsm", "urn:ferd:CrossIndustryDocument:invoice:1p0");
            _Writer.WriteAttributeString("xmlns", "ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:12");
            _Writer.WriteAttributeString("xmlns", "udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:15");
            #endregion

            #region SpecifiedExchangedDocumentContext
            _Writer.WriteStartElement("rsm", "SpecifiedExchangedDocumentContext");
            _Writer.WriteStartElement("ram", "TestIndicator");
            _Writer.WriteElementString("udt", "Indicator", this._Descriptor.IsTest ? "true" : "false");
            _Writer.WriteEndElement(); // !ram:TestIndicator

            if (!String.IsNullOrWhiteSpace(this._Descriptor.BusinessProcess))
            {
                _Writer.WriteStartElement("ram", "BusinessProcessSpecifiedDocumentContextParameter", Profile.Extended);
                _Writer.WriteElementString("ram", "ID", this._Descriptor.BusinessProcess, Profile.Extended);
                _Writer.WriteEndElement(); // !ram:BusinessProcessSpecifiedDocumentContextParameter
            }

            _Writer.WriteStartElement("ram", "GuidelineSpecifiedDocumentContextParameter");
            _Writer.WriteElementString("ram", "ID", this._Descriptor.Profile.EnumToString(ZUGFeRDVersion.Version1));
            _Writer.WriteEndElement(); // !ram:GuidelineSpecifiedDocumentContextParameter
            _Writer.WriteEndElement(); // !rsm:SpecifiedExchangedDocumentContext

            _Writer.WriteStartElement("rsm", "HeaderExchangedDocument");
            _Writer.WriteElementString("ram", "ID", this._Descriptor.InvoiceNo);
            _Writer.WriteElementString("ram", "Name", this._Descriptor.Name);
            _Writer.WriteElementString("ram", "TypeCode", String.Format("{0}", EnumExtensions.EnumToString<InvoiceType>(this._Descriptor.Type))); //Code für den Rechnungstyp

            if (this._Descriptor.InvoiceDate.HasValue)
            {
                _Writer.WriteStartElement("ram", "IssueDateTime");
                _Writer.WriteStartElement("udt", "DateTimeString");
                _Writer.WriteAttributeString("format", "102");
                _Writer.WriteValue(_formatDate(this._Descriptor.InvoiceDate.Value));
                _Writer.WriteEndElement(); // !udt:DateTimeString
                _Writer.WriteEndElement(); // !IssueDateTime
            }
            _writeNotes(_Writer, this._Descriptor.Notes);
            _Writer.WriteEndElement(); // !rsm:HeaderExchangedDocument
            #endregion

            #region SpecifiedSupplyChainTradeTransaction
            _Writer.WriteStartElement("rsm", "SpecifiedSupplyChainTradeTransaction");

            _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableHeaderTradeAgreementComment);
            _Writer.WriteStartElement("ram", "ApplicableSupplyChainTradeAgreement");
            if (!String.IsNullOrWhiteSpace(this._Descriptor.ReferenceOrderNo))
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerReferenceComment);
                _Writer.WriteElementString("ram", "BuyerReference", this._Descriptor.ReferenceOrderNo);
            }

            _WriteComment(_Writer, options, InvoiceCommentConstants.SellerTradePartyComment);
            _writeOptionalParty(_Writer, "ram", "SellerTradeParty", this._Descriptor.Seller, this._Descriptor.SellerContact, TaxRegistrations: this._Descriptor.SellerTaxRegistration);
            _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerTradePartyComment);
            _writeOptionalParty(_Writer, "ram", "BuyerTradeParty", this._Descriptor.Buyer, this._Descriptor.BuyerContact, TaxRegistrations: this._Descriptor.BuyerTaxRegistration);

            if (!String.IsNullOrWhiteSpace(this._Descriptor.OrderNo))
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerOrderReferencedDocumentComment);
                _Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument");
                if (this._Descriptor.OrderDate.HasValue)
                {
                    _Writer.WriteStartElement("ram", "IssueDateTime");
                    //_Writer.WriteStartElement("udt:DateTimeString");
                    //_Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(this._Descriptor.OrderDate.Value, false));
                    //_Writer.WriteEndElement(); // !udt:DateTimeString
                    _Writer.WriteEndElement(); // !IssueDateTime()
                }

                _Writer.WriteElementString("ram", "ID", this._Descriptor.OrderNo);
                _Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }


            foreach (AdditionalReferencedDocument document in this._Descriptor.AdditionalReferencedDocuments)
            {
                _Writer.WriteStartElement("ram", "AdditionalReferencedDocument");
                if (document.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "IssueDateTime");
                    //_Writer.WriteStartElement("udt", "DateTimeString");
                    //_Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(document.IssueDateTime.Value, false));
                    //_Writer.WriteEndElement(); // !udt:DateTimeString
                    _Writer.WriteEndElement(); // !IssueDateTime()
                }

                if (document.TypeCode.HasValue)
                {
                    _Writer.WriteElementString("ram", "TypeCode", document.TypeCode.Value.EnumToString());
                }

                if (document.ReferenceTypeCode.HasValue)
                {
                    _Writer.WriteElementString("ram", "ReferenceTypeCode", document.ReferenceTypeCode.Value.EnumToString());
                }

                _Writer.WriteElementString("ram", "ID", document.ID);
                _Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
            } // !foreach(document)

            _Writer.WriteEndElement(); // !ApplicableSupplyChainTradeAgreement

            _Writer.WriteStartElement("ram", "ApplicableSupplyChainTradeDelivery"); // Pflichteintrag

            //RelatedSupplyChainConsignment --> SpecifiedLogisticsTransportMovement --> ModeCode // Only in extended profile
            if (this._Descriptor.TransportMode != null)
            {
                _Writer.WriteStartElement("ram", "RelatedSupplyChainConsignment", Profile.Extended); // BG-X-24
                _Writer.WriteStartElement("ram", "SpecifiedLogisticsTransportMovement", Profile.Extended); // BT-X-152-00
                _Writer.WriteElementString("ram", "ModeCode", EnumExtensions.EnumToString<TransportModeCodes>(this._Descriptor.TransportMode)); // BT-X-152
                _Writer.WriteEndElement(); // !ram:SpecifiedLogisticsTransportMovement 
                _Writer.WriteEndElement(); // !ram:RelatedSupplyChainConsignment
            }

            if (_Descriptor.Profile == Profile.Extended)
            {
                _writeOptionalParty(_Writer, "ram", "ShipToTradeParty", this._Descriptor.ShipTo);
                _writeOptionalParty(_Writer, "ram", "ShipFromTradeParty", this._Descriptor.ShipFrom);
            }

            _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableHeaderTradeDeliveryComment);
            if (this._Descriptor.ActualDeliveryDate.HasValue)
            {
                _Writer.WriteStartElement("ram", "ActualDeliverySupplyChainEvent");
                _Writer.WriteStartElement("ram", "OccurrenceDateTime");
                _Writer.WriteStartElement("udt", "DateTimeString");
                _Writer.WriteAttributeString("format", "102");
                _Writer.WriteValue(_formatDate(this._Descriptor.ActualDeliveryDate.Value));
                _Writer.WriteEndElement(); // !udt:DateTimeString
                _Writer.WriteEndElement(); // !OccurrenceDateTime()
                _Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
            }

            if (this._Descriptor.DeliveryNoteReferencedDocument != null)
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.DespatchAdviceReferencedDocumentComment);
                _Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument");

                if (this._Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "IssueDateTime");
                    _Writer.WriteValue(_formatDate(this._Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.Value, false));
                    _Writer.WriteEndElement(); // !IssueDateTime
                }

                _Writer.WriteElementString("ram", "ID", this._Descriptor.DeliveryNoteReferencedDocument.ID);
                _Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }

            _Writer.WriteEndElement(); // !ApplicableSupplyChainTradeDelivery

            _Writer.WriteStartElement("ram", "ApplicableSupplyChainTradeSettlement");
            _Writer.WriteElementString("ram", "InvoiceCurrencyCode", this._Descriptor.Currency.EnumToString());

            if (_Descriptor.Profile != Profile.Basic)
            {
                _writeOptionalParty(_Writer, "ram", "InvoiceeTradeParty", this._Descriptor.Invoicee);
            }
            if (_Descriptor.Profile == Profile.Extended)
            {
                _writeOptionalParty(_Writer, "ram", "PayeeTradeParty", this._Descriptor.Payee);
            }

            _Writer.WriteOptionalElementString("ram", "PaymentReference", this._Descriptor.PaymentReference);

            if (!this._Descriptor.AnyCreditorFinancialAccount() && ! this._Descriptor.AnyDebitorFinancialAccount())
            {
                if (this._Descriptor.PaymentMeans != null)
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementPaymentMeansComment);
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans.SEPAMandateReference))
                        {
                            _Writer.WriteStartElement("ram", "ID");
                            _Writer.WriteAttributeString("schemeAgencyID", this._Descriptor.PaymentMeans.SEPACreditorIdentifier);
                            _Writer.WriteValue(this._Descriptor.PaymentMeans.SEPAMandateReference);
                            _Writer.WriteEndElement(); // !ram:ID
                        }
                    }
                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }
            else
            {
                foreach (BankAccount creditorBankAccount in this._Descriptor.GetCreditorFinancialAccounts())
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementPaymentMeansComment);
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans.SEPAMandateReference))
                        {
                            _Writer.WriteStartElement("ram", "ID");
                            _Writer.WriteAttributeString("schemeAgencyID", this._Descriptor.PaymentMeans.SEPACreditorIdentifier);
                            _Writer.WriteValue(this._Descriptor.PaymentMeans.SEPAMandateReference);
                            _Writer.WriteEndElement(); // !ram:ID
                        }
                    }

                    _Writer.WriteStartElement("ram", "PayeePartyCreditorFinancialAccount");
                    _Writer.WriteElementString("ram", "IBANID", creditorBankAccount.IBAN);
                    if (!String.IsNullOrWhiteSpace(creditorBankAccount.Name))
                    {
                        _Writer.WriteOptionalElementString("ram", "AccountName", creditorBankAccount.Name);
                    }
                    _Writer.WriteOptionalElementString("ram", "ProprietaryID", creditorBankAccount.ID);
                    _Writer.WriteEndElement(); // !PayeePartyCreditorFinancialAccount

                    _Writer.WriteStartElement("ram", "PayeeSpecifiedCreditorFinancialInstitution");
                    _Writer.WriteElementString("ram", "BICID", creditorBankAccount.BIC);
                    _Writer.WriteOptionalElementString("ram", "GermanBankleitzahlID", creditorBankAccount.Bankleitzahl);
                    _Writer.WriteOptionalElementString("ram", "Name", creditorBankAccount.BankName);
                    _Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }

                foreach (BankAccount debitorBankAccount in this._Descriptor.GetDebitorFinancialAccounts())
                {
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans.SEPAMandateReference))
                        {
                            _Writer.WriteStartElement("ram", "ID");
                            _Writer.WriteAttributeString("schemeAgencyID", this._Descriptor.PaymentMeans.SEPACreditorIdentifier);
                            _Writer.WriteValue(this._Descriptor.PaymentMeans.SEPAMandateReference);
                            _Writer.WriteEndElement(); // !ram:ID
                        }
                    }

                    _Writer.WriteStartElement("ram", "PayerPartyDebtorFinancialAccount");
                    _Writer.WriteElementString("ram", "IBANID", debitorBankAccount.IBAN);
                    _Writer.WriteOptionalElementString("ram", "ProprietaryID", debitorBankAccount.ID);
                    _Writer.WriteEndElement(); // !PayerPartyDebtorFinancialAccount

                    _Writer.WriteStartElement("ram", "PayerSpecifiedDebtorFinancialInstitution");
                    _Writer.WriteElementString("ram", "BICID", debitorBankAccount.BIC);
                    _Writer.WriteOptionalElementString("ram", "GermanBankleitzahlID", debitorBankAccount.Bankleitzahl);
                    _Writer.WriteOptionalElementString("ram", "Name", debitorBankAccount.BankName);
                    _Writer.WriteEndElement(); // !PayerSpecifiedDebtorFinancialInstitution
                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }

            _writeOptionalTaxes(_Writer, options);

            foreach (TradeAllowance tradeAllowance in this._Descriptor.GetTradeAllowances())
            {
                _WriteDocumentLevelSpecifiedTradeAllowanceCharge(_Writer, tradeAllowance);
            }

            foreach (TradeCharge tradeCharge in this._Descriptor.GetTradeCharges())
            {
                _WriteDocumentLevelSpecifiedTradeAllowanceCharge(_Writer, tradeCharge);
            }

            foreach (ServiceCharge serviceCharge in this._Descriptor.GetLogisticsServiceCharges())
            {
                _Writer.WriteStartElement("ram", "SpecifiedLogisticsServiceCharge");
                _Writer.WriteOptionalElementString("ram", "Description", serviceCharge.Description, Profile.Comfort | Profile.Extended);
                _Writer.WriteElementString("ram", "AppliedAmount", _formatDecimal(serviceCharge.Amount), Profile.Comfort | Profile.Extended);
                if (serviceCharge.Tax != null)
                {
                    _Writer.WriteStartElement("ram", "AppliedTradeTax");

                    if (serviceCharge.Tax.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", serviceCharge.Tax.TypeCode.EnumToString(), Profile.Comfort | Profile.Extended);
                    }

                    if (serviceCharge.Tax.CategoryCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "CategoryCode", serviceCharge.Tax.CategoryCode.EnumToString(), Profile.Comfort | Profile.Extended);
                    }
                    
                    _Writer.WriteElementString("ram", "ApplicablePercent", _formatDecimal(serviceCharge.Tax.Percent), Profile.Comfort | Profile.Extended);
                    _Writer.WriteEndElement();
                }
                _Writer.WriteEndElement();
            }

            //  The cardinality depends on the profile.
            switch (_Descriptor.Profile)
            {
                case Profile.Unknown:
                case Profile.Minimum:
                    break;
                case Profile.Extended:
                    foreach (PaymentTerms paymentTerms in this._Descriptor.GetTradePaymentTerms())
                    {
                        _Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        _Writer.WriteOptionalElementString("ram", "Description", paymentTerms.Description);
                        if (paymentTerms.DueDate.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(paymentTerms.DueDate.Value));
                            _Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        _Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", _Descriptor.PaymentMeans?.SEPAMandateReference);
                        if (paymentTerms.PaymentTermsType.HasValue)
                        {
                            if (paymentTerms.PaymentTermsType == PaymentTermsType.Skonto)
                            {
                                _Writer.WriteStartElement("ram", "ApplicableTradePaymentDiscountTerms");
                                _writeOptionalAmount(_Writer, "ram", "BasisAmount", paymentTerms.BaseAmount);
                                _Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                _writeOptionalAmount(_Writer, "ram", "ActualDiscountAmount", paymentTerms.ActualAmount);
                                _Writer.WriteEndElement(); // !ram:ApplicableTradePaymentDiscountTerms
                            }
                            if (paymentTerms.PaymentTermsType == PaymentTermsType.Verzug)
                            {
                                _Writer.WriteStartElement("ram", "ApplicableTradePaymentPenaltyTerms");
                                _writeOptionalAmount(_Writer, "ram", "BasisAmount", paymentTerms.BaseAmount);
                                _Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                _writeOptionalAmount(_Writer, "ram", "ActualPenaltyAmount", paymentTerms.ActualAmount);
                                _Writer.WriteEndElement(); // !ram:ApplicableTradePaymentPenaltyTerms
                            }
                        }
                        _Writer.WriteEndElement();
                    }
                    if (this._Descriptor.GetTradePaymentTerms().Count == 0 && !string.IsNullOrWhiteSpace(_Descriptor.PaymentMeans?.SEPAMandateReference))
                    {
                        _Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        _Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", _Descriptor.PaymentMeans?.SEPAMandateReference);
                        _Writer.WriteEndElement();
                    }
                    break;
                default:
                    if (_Descriptor.GetTradePaymentTerms().Count > 0)
                    {
                        _Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        var sbPaymentNotes = new StringBuilder();
                        DateTime? dueDate = null;
                        foreach (PaymentTerms paymentTerms in this._Descriptor.GetTradePaymentTerms())
                        {
                            sbPaymentNotes.AppendLine(paymentTerms.Description);
                            dueDate = dueDate ?? paymentTerms.DueDate;
                        }
                        _Writer.WriteOptionalElementString("ram", "Description", sbPaymentNotes.ToString().TrimEnd());
                        if (dueDate.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(dueDate.Value));
                            _Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        _Writer.WriteEndElement();
                    }
                    break;
            }

            _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementHeaderMonetarySummationComment);
            _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementMonetarySummation");
            _writeOptionalAmount(_Writer, "ram", "LineTotalAmount", this._Descriptor.LineTotalAmount);
            _writeOptionalAmount(_Writer, "ram", "ChargeTotalAmount", this._Descriptor.ChargeTotalAmount ?? 0m); // must occur exactly 1 time
            _writeOptionalAmount(_Writer, "ram", "AllowanceTotalAmount", this._Descriptor.AllowanceTotalAmount ?? 0m); // must occur exactly 1 time
            _writeOptionalAmount(_Writer, "ram", "TaxBasisTotalAmount", this._Descriptor.TaxBasisAmount);
            _writeOptionalAmount(_Writer, "ram", "TaxTotalAmount", this._Descriptor.TaxTotalAmount);
            _writeOptionalAmount(_Writer, "ram", "GrandTotalAmount", this._Descriptor.GrandTotalAmount);
            _writeOptionalAmount(_Writer, "ram", "TotalPrepaidAmount", this._Descriptor.TotalPrepaidAmount);
            _writeOptionalAmount(_Writer, "ram", "DuePayableAmount", this._Descriptor.DuePayableAmount);
            _Writer.WriteEndElement(); // !ram:SpecifiedTradeSettlementMonetarySummation

            _Writer.WriteEndElement(); // !ram:ApplicableSupplyChainTradeSettlement

            foreach (TradeLineItem tradeLineItem in this._Descriptor.GetTradeLineItems())
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.IncludedSupplyChainTradeLineItemComment);
                _Writer.WriteStartElement("ram", "IncludedSupplyChainTradeLineItem");

                if (tradeLineItem.AssociatedDocument != null)
                {
                    _Writer.WriteStartElement("ram", "AssociatedDocumentLineDocument");
                    _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.AssociatedDocument.LineID);
                    _writeNotes(_Writer, tradeLineItem.AssociatedDocument.Notes);
                    _Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument
                }

                // handelt es sich um einen Kommentar?
                if ((tradeLineItem.AssociatedDocument?.Notes.Count > 0) && (tradeLineItem.BilledQuantity == 0) && (String.IsNullOrWhiteSpace(tradeLineItem.Description)))
                {
                    _Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItemComment
                    continue;
                }

                if (_Descriptor.Profile != Profile.Basic)
                {
                    _Writer.WriteStartElement("ram", "SpecifiedSupplyChainTradeAgreement");

                    if (tradeLineItem.BuyerOrderReferencedDocument != null)
                    {
                        _Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument");
                        if (tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "IssueDateTime");
                            _Writer.WriteValue(_formatDate(tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.Value, false));
                            _Writer.WriteEndElement(); // !ram:IssueDateTime
                        }
                        _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.BuyerOrderReferencedDocument.LineID);
                        _Writer.WriteOptionalElementString("ram", "ID", tradeLineItem.BuyerOrderReferencedDocument.ID);
                        _Writer.WriteEndElement(); // !ram:BuyerOrderReferencedDocument
                    }

                    if (tradeLineItem.ContractReferencedDocument != null)
                    {
                        _Writer.WriteStartElement("ram", "ContractReferencedDocument");
                        if (tradeLineItem.ContractReferencedDocument.IssueDateTime.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "IssueDateTime");
                            _Writer.WriteValue(_formatDate(tradeLineItem.ContractReferencedDocument.IssueDateTime.Value, false));
                            _Writer.WriteEndElement(); // !ram:IssueDateTime
                        }
                        _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.ContractReferencedDocument.LineID);
                        _Writer.WriteOptionalElementString("ram", "ID", tradeLineItem.ContractReferencedDocument.ID);
                        _Writer.WriteEndElement(); // !ram:ContractReferencedDocument
                    }

                    if (tradeLineItem.AdditionalReferencedDocuments != null)
                    {
                        foreach (AdditionalReferencedDocument document in tradeLineItem.AdditionalReferencedDocuments)
                        {
                            _Writer.WriteStartElement("ram", "AdditionalReferencedDocument");
                            if (document.IssueDateTime.HasValue)
                            {
                                _Writer.WriteStartElement("ram", "IssueDateTime");
                                _Writer.WriteValue(_formatDate(document.IssueDateTime.Value, false));
                                _Writer.WriteEndElement(); // !ram:IssueDateTime
                            }

                            _Writer.WriteElementString("ram", "LineID", String.Format("{0}", tradeLineItem.AssociatedDocument?.LineID));
                            _Writer.WriteOptionalElementString("ram", "ID", document.ID);

                            if (document.ReferenceTypeCode.HasValue)
                            {
                                _Writer.WriteElementString("ram", "ReferenceTypeCode", document.ReferenceTypeCode.Value.EnumToString());
                            }

                            _Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                        }
                    }

                    _Writer.WriteStartElement("ram", "GrossPriceProductTradePrice");
                    _writeOptionalAdaptiveAmount(_Writer, "ram", "ChargeAmount", tradeLineItem.GrossUnitPrice, 2, 4, true);
                    if (tradeLineItem.GrossQuantity.HasValue)
                    {
                        _writeElementWithAttribute(_Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.GrossQuantity.Value, 4));
                    }

                    foreach (AbstractTradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.GetTradeAllowanceCharges())
                    {
                        _Writer.WriteStartElement("ram", "AppliedTradeAllowanceCharge");

                        _Writer.WriteStartElement("ram", "ChargeIndicator", Profile.Comfort | Profile.Extended);
                        _Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                        _Writer.WriteEndElement(); // !ram:ChargeIndicator

                        if (tradeAllowanceCharge.BasisAmount.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "BasisAmount", Profile.Extended);
                            _Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                            _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value, 4));
                            _Writer.WriteEndElement();
                        }
                        _Writer.WriteStartElement("ram", "ActualAmount", Profile.Comfort | Profile.Extended);
                        _Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                        _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 4));
                        _Writer.WriteEndElement();

                        _Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason, Profile.Comfort | Profile.Extended);

                        _Writer.WriteEndElement(); // !AppliedTradeAllowanceCharge
                    }

                    _Writer.WriteEndElement(); // ram:GrossPriceProductTradePrice

                    _WriteComment(_Writer, options, InvoiceCommentConstants.NetPriceProductTradePriceComment);
                    _Writer.WriteStartElement("ram", "NetPriceProductTradePrice");
                    _writeOptionalAdaptiveAmount(_Writer, "ram", "ChargeAmount", tradeLineItem.NetUnitPrice, 2, 4, true);

                    if (tradeLineItem.NetQuantity.HasValue)
                    {
                        _writeElementWithAttribute(_Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.NetQuantity.Value, 4));
                    }
                    _Writer.WriteEndElement(); // ram:NetPriceProductTradePrice

                    _Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeAgreement
                }

                if (_Descriptor.Profile != Profile.Basic)
                {
                    _Writer.WriteStartElement("ram", "SpecifiedSupplyChainTradeDelivery");
                    _writeElementWithAttribute(_Writer, "ram", "BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));

                    if (tradeLineItem.DeliveryNoteReferencedDocument != null)
                    {
                        _Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument");
                        if (tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "IssueDateTime");
                            _Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value, false));
                            _Writer.WriteEndElement(); // !ram:IssueDateTime
                        }
                        _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.DeliveryNoteReferencedDocument.LineID);
                        _Writer.WriteOptionalElementString("ram", "ID", tradeLineItem.DeliveryNoteReferencedDocument.ID);
                        _Writer.WriteEndElement(); // !ram:DeliveryNoteReferencedDocument
                    }

                    if (tradeLineItem.ActualDeliveryDate.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "ActualDeliverySupplyChainEvent");
                        _Writer.WriteStartElement("ram", "OccurrenceDateTime");
                        _Writer.WriteStartElement("udt", "DateTimeString");
                        _Writer.WriteAttributeString("format", "102");
                        _Writer.WriteValue(_formatDate(tradeLineItem.ActualDeliveryDate.Value));
                        _Writer.WriteEndElement(); // !udt:DateTimeString
                        _Writer.WriteEndElement(); // !OccurrenceDateTime()
                        _Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
                    }

                    _Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeDelivery
                }
                else
                {
                    _Writer.WriteStartElement("ram", "SpecifiedSupplyChainTradeDelivery");
                    _writeElementWithAttribute(_Writer, "ram", "BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));
                    _Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeDelivery
                }

                _Writer.WriteStartElement("ram", "SpecifiedSupplyChainTradeSettlement");

                if (_Descriptor.Profile != Profile.Basic)
                {                    
                    _Writer.WriteStartElement("ram", "ApplicableTradeTax");

                    if (tradeLineItem.TaxType.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", tradeLineItem.TaxType.EnumToString());
                    }

                    if (tradeLineItem.TaxCategoryCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString());
                    }
                    
                    _Writer.WriteElementString("ram", "ApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent));
                    _Writer.WriteEndElement(); // !ram:ApplicableTradeTax
                }

                if (tradeLineItem.BillingPeriodStart.HasValue && tradeLineItem.BillingPeriodEnd.HasValue)
                {
                    _Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                    _Writer.WriteStartElement("ram", "StartDateTime");
                    _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodStart.Value));
                    _Writer.WriteEndElement(); // !StartDateTime


                    _Writer.WriteStartElement("ram", "EndDateTime");
                    _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodEnd.Value));
                    _Writer.WriteEndElement(); // !EndDateTime

                    _Writer.WriteEndElement(); // !BillingSpecifiedPeriod
                }

                _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementLineMonetarySummationComment);
                _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementMonetarySummation");

                decimal total = 0m;

                if (tradeLineItem.LineTotalAmount.HasValue)
                {
                    total = tradeLineItem.LineTotalAmount.Value;
                }
                else
                {
                    total = tradeLineItem.NetUnitPrice * tradeLineItem.BilledQuantity;
                    if (tradeLineItem.NetQuantity.HasValue && (tradeLineItem.NetQuantity.Value != 0))
                    {
                        total /= tradeLineItem.NetQuantity.Value;
                    }
                }

                _writeElementWithAttribute(_Writer, "ram", "LineTotalAmount", "currencyID", this._Descriptor.Currency.EnumToString(), _formatDecimal(total));
                _Writer.WriteEndElement(); // ram:SpecifiedTradeSettlementMonetarySummation
                _Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeSettlement

                _Writer.WriteStartElement("ram", "SpecifiedTradeProduct");
                if ((tradeLineItem.GlobalID != null) && (tradeLineItem.GlobalID.SchemeID.HasValue) && tradeLineItem.GlobalID.SchemeID.HasValue && !String.IsNullOrWhiteSpace(tradeLineItem.GlobalID.ID))
                {
                    _writeElementWithAttribute(_Writer, "ram", "GlobalID", "schemeID", tradeLineItem.GlobalID.SchemeID.Value.EnumToString(), tradeLineItem.GlobalID.ID);
                }

                _Writer.WriteOptionalElementString("ram", "SellerAssignedID", tradeLineItem.SellerAssignedID);
                _Writer.WriteOptionalElementString("ram", "BuyerAssignedID", tradeLineItem.BuyerAssignedID);
                _Writer.WriteOptionalElementString("ram", "Name", tradeLineItem.Name);
                _Writer.WriteOptionalElementString("ram", "Description", tradeLineItem.Description);

                _Writer.WriteEndElement(); // !ram:SpecifiedTradeProduct
                _Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItemComment
            } // !foreach(tradeLineItem)

            _Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeTransaction
            #endregion

            _Writer.WriteEndElement(); // !ram:Invoice
            _Writer.WriteEndDocument();
            _Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);
        } // !Save()


        private void _WriteDocumentLevelSpecifiedTradeAllowanceCharge(ProfileAwareXmlTextWriter writer, AbstractTradeAllowanceCharge tradeAllowanceCharge)
        {
            if (tradeAllowanceCharge == null)
            {
                return;
            }

            _Writer.WriteStartElement("ram", "SpecifiedTradeAllowanceCharge");
            _Writer.WriteStartElement("ram", "ChargeIndicator", Profile.Comfort | Profile.Extended);
            _Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
            _Writer.WriteEndElement(); // !ram:ChargeIndicator

            if (tradeAllowanceCharge.BasisAmount.HasValue)
            {
                _Writer.WriteStartElement("ram", "BasisAmount", Profile.Extended);
                _Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value));
                _Writer.WriteEndElement();
            }

            _Writer.WriteStartElement("ram", "ActualAmount", Profile.Comfort | Profile.Extended);
            _Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
            _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 2));
            _Writer.WriteEndElement();

            if ((tradeAllowanceCharge is TradeAllowance allowance) && (allowance.ReasonCode != null))
            {
                _Writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<AllowanceReasonCodes>(allowance.ReasonCode), Profile.Comfort | Profile.Extended);
            }
            else if ((tradeAllowanceCharge is TradeCharge charge) && (charge.ReasonCode != null))
            {
                _Writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<ChargeReasonCodes>(charge.ReasonCode), Profile.Comfort | Profile.Extended);
            }

            _Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason, Profile.Comfort | Profile.Extended);

            if (tradeAllowanceCharge.Tax != null)
            {
                _Writer.WriteStartElement("ram", "CategoryTradeTax");

                if (tradeAllowanceCharge.Tax.TypeCode.HasValue)
                {
                    _Writer.WriteElementString("ram", "TypeCode", tradeAllowanceCharge.Tax.TypeCode.EnumToString(), Profile.Comfort | Profile.Extended);
                }

                if (tradeAllowanceCharge.Tax.CategoryCode.HasValue)
                {
                    _Writer.WriteElementString("ram", "CategoryCode", tradeAllowanceCharge.Tax.CategoryCode.EnumToString(), Profile.Comfort | Profile.Extended);
                }
                
                _Writer.WriteElementString("ram", "ApplicablePercent", _formatDecimal(tradeAllowanceCharge.Tax.Percent), Profile.Comfort | Profile.Extended);
                _Writer.WriteEndElement();
            }
            _Writer.WriteEndElement();
        } // !_WriteItemLevelSpecifiedTradeAllowanceCharge()


        internal override bool Validate(InvoiceDescriptor descriptor, bool throwExceptions = true)
        {
            if (descriptor.Profile == Profile.BasicWL)
            {
                if (throwExceptions)
                {
                    throw new UnsupportedException("Invalid profile used for ZUGFeRD 2.0 invoice.");
                }
                return false;
            }

            if (descriptor.Profile != Profile.Extended) // check tax types, only extended profile allows tax types other than vat
            {
                if (!descriptor.GetTradeLineItems().All(l => !l.TaxType.HasValue || l.TaxType.Value.Equals(TaxTypes.VAT)))
                {
                    if (throwExceptions) { throw new UnsupportedException("Tax types other than VAT only possible with extended profile."); }
                    return false;
                }
            }

            return true;
        } // !Validate()


        private void _writeOptionalAmount(ProfileAwareXmlTextWriter writer, string prefix, string tagName, decimal? value, int numDecimals = 2)
        {
            if (value.HasValue)
            {
                writer.WriteStartElement(prefix, tagName);
                writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                writer.WriteValue(_formatDecimal(value.Value, numDecimals));
                writer.WriteEndElement(); // !tagName
            }
        } // !_writeOptionalAmount()


        private void _writeOptionalAdaptiveAmount(ProfileAwareXmlTextWriter writer, string prefix, string tagName, decimal? value, int numDecimals = 2, int maxNumDecimals = 4, bool forceCurrency = false, Profile profile = Profile.Unknown)
        {
            if (!value.HasValue)
            {
                return;
            }

            writer.WriteStartElement(prefix, tagName, profile);
            if (forceCurrency)
            {
                writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
            }

            decimal rounded = Math.Round(value.Value, numDecimals, MidpointRounding.AwayFromZero);
            if (value == rounded)
            {
                writer.WriteValue(_formatDecimal(value.Value, numDecimals));
            }
            else
            {
                writer.WriteValue(_formatDecimal(value.Value, maxNumDecimals));
            }

            writer.WriteEndElement(); // !tagName
        } // !_writeOptionalAdaptiveAmount()


        private void _writeElementWithAttribute(ProfileAwareXmlTextWriter writer, string prefix, string tagName, string attributeName, string attributeValue, string nodeValue)
        {
            writer.WriteStartElement(prefix, tagName);
            writer.WriteAttributeString(attributeName, attributeValue);
            writer.WriteValue(nodeValue);
            writer.WriteEndElement(); // !tagName
        } // !_writeElementWithAttribute()


        private void _writeOptionalTaxes(ProfileAwareXmlTextWriter writer, InvoiceFormatOptions options)
        {
            foreach (Tax tax in this._Descriptor.GetApplicableTradeTaxes())
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableTradeTaxComment);
                writer.WriteStartElement("ram", "ApplicableTradeTax");

                writer.WriteStartElement("ram", "CalculatedAmount");
                writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                writer.WriteValue(_formatDecimal(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount

                writer.WriteElementString("ram", "TypeCode", tax.TypeCode.EnumToString());

                writer.WriteStartElement("ram", "BasisAmount");
                writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                writer.WriteValue(_formatDecimal(tax.BasisAmount));
                writer.WriteEndElement(); // !BasisAmount
                if (_Descriptor.Profile == Profile.Extended)
                {
                    if (tax.LineTotalBasisAmount.HasValue && (tax.LineTotalBasisAmount.Value != 0))
                    {
                        writer.WriteStartElement("ram", "LineTotalBasisAmount");
                        writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                        writer.WriteValue(_formatDecimal(tax.LineTotalBasisAmount));
                        writer.WriteEndElement();
                    }
                    if (tax.AllowanceChargeBasisAmount.HasValue && (tax.AllowanceChargeBasisAmount.Value != 0))
                    {
                        writer.WriteStartElement("ram", "AllowanceChargeBasisAmount");
                        writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                        writer.WriteValue(_formatDecimal(tax.AllowanceChargeBasisAmount));
                        writer.WriteEndElement(); // !AllowanceChargeBasisAmount
                    }
                }

                if (tax.CategoryCode.HasValue)
                {
                    writer.WriteElementString("ram", "CategoryCode", tax.CategoryCode.EnumToString());
                }

                writer.WriteElementString("ram", "ApplicablePercent", _formatDecimal(tax.Percent));
                writer.WriteEndElement(); // !ApplicableTradeTax
            }
        } // !_writeOptionalTaxes()


        private void _writeNotes(ProfileAwareXmlTextWriter writer, List<Note> notes)
        {
            if (notes.Count > 0)
            {
                foreach (Note note in notes)
                {
                    writer.WriteStartElement("ram", "IncludedNote");
                    if (note.ContentCode.HasValue)
                    {
                        writer.WriteElementString("ram", "ContentCode", note.ContentCode.EnumToString());
                    }
                    writer.WriteElementString("ram", "Content", note.Content);
                    if (note.SubjectCode.HasValue)
                    {
                        writer.WriteElementString("ram", "SubjectCode", note.SubjectCode.EnumToString());
                    }
                    writer.WriteEndElement();
                }
            }
        } // !_writeNotes()


        private void _writeOptionalParty(ProfileAwareXmlTextWriter writer, string prefix, string PartyTag, Party Party, Contact Contact = null, List<TaxRegistration> TaxRegistrations = null)
        {
            if (Party != null)
            {
                writer.WriteStartElement(prefix, PartyTag);

                if ((Party.ID != null) && !String.IsNullOrWhiteSpace(Party.ID.ID))
                {
                    if ((Party.ID.SchemeID.HasValue) && Party.ID.SchemeID.HasValue)
                    {
                        writer.WriteStartElement("ram", "ID");
                        writer.WriteAttributeString("schemeID", Party.ID.SchemeID.Value.EnumToString());
                        writer.WriteValue(Party.ID.ID);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteElementString("ram", "ID", Party.ID.ID);
                    }
                }

                if ((Party.GlobalID != null) && !String.IsNullOrWhiteSpace(Party.GlobalID.ID) && (Party.GlobalID.SchemeID.HasValue) && Party.GlobalID.SchemeID.HasValue)
                {
                    writer.WriteStartElement("ram", "GlobalID");
                    writer.WriteAttributeString("schemeID", Party.GlobalID.SchemeID.Value.EnumToString());
                    writer.WriteValue(Party.GlobalID.ID);
                    writer.WriteEndElement();
                }

                _Writer.WriteOptionalElementString("ram", "Name", Party.Name);
                writer.WriteOptionalElementString("ram", "Description", Party.Description, PROFILE_COMFORT_EXTENDED_XRECHNUNG); // BT-33
                _writeOptionalContact(writer, "ram", "DefinedTradeContact", Contact);
                writer.WriteStartElement("ram", "PostalTradeAddress");
                writer.WriteOptionalElementString("ram", "PostcodeCode", Party.Postcode);
                writer.WriteOptionalElementString("ram", "LineOne", string.IsNullOrWhiteSpace(Party.ContactName) ? Party.Street : Party.ContactName);
                if (!string.IsNullOrWhiteSpace(Party.ContactName)) { writer.WriteOptionalElementString("ram", "LineTwo", Party.Street); }
                writer.WriteOptionalElementString("ram", "CityName", Party.City);

                if (Party.Country.HasValue)
                {
                    writer.WriteElementString("ram", "CountryID", Party.Country.Value.EnumToString());
                }

                writer.WriteEndElement(); // !PostalTradeAddress

                if (TaxRegistrations != null)
                {
                    foreach (TaxRegistration taxRegistration in TaxRegistrations)
                    {
                        if (!String.IsNullOrWhiteSpace(taxRegistration.No))
                        {
                            writer.WriteStartElement("ram", "SpecifiedTaxRegistration");
                            writer.WriteStartElement("ram", "ID");
                            writer.WriteAttributeString("schemeID", taxRegistration.SchemeID.EnumToString());
                            writer.WriteValue(taxRegistration.No);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
                writer.WriteEndElement(); // !*TradeParty
            }
        } // !_writeOptionalParty()


        private void _writeOptionalContact(ProfileAwareXmlTextWriter writer, string prefix, string contactTag, Contact contact)
        {
            if (contact != null)
            {
                writer.WriteStartElement(prefix, contactTag);

                writer.WriteOptionalElementString("ram", "PersonName", contact.Name);
                writer.WriteOptionalElementString("ram", "DepartmentName", contact.OrgUnit);

                if (!String.IsNullOrWhiteSpace(contact.PhoneNo))
                {
                    writer.WriteStartElement("ram", "TelephoneUniversalCommunication");
                    writer.WriteElementString("ram", "CompleteNumber", contact.PhoneNo);
                    writer.WriteEndElement();
                }

                if (!String.IsNullOrWhiteSpace(contact.FaxNo))
                {
                    writer.WriteStartElement("ram", "FaxUniversalCommunication", Profile.Extended);
                    writer.WriteElementString("ram", "CompleteNumber", contact.FaxNo);
                    writer.WriteEndElement();
                }

                if (!String.IsNullOrWhiteSpace(contact.EmailAddress))
                {
                    writer.WriteStartElement("ram", "EmailURIUniversalCommunication");
                    writer.WriteElementString("ram", "URIID", contact.EmailAddress);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        } // !_writeOptionalContact()


        private string _translateInvoiceType(InvoiceType type)
        {
            switch (type)
            {
                case InvoiceType.Invoice: return "RECHNUNG";
                case InvoiceType.Correction: return "KORREKTURRECHNUNG";
                case InvoiceType.CreditNote: return "GUTSCHRIFT";
                case InvoiceType.DebitnoteRelatedToFinancialAdjustments: return "WERTBELASTUNG";
                case InvoiceType.DebitNote: return String.Empty;
                case InvoiceType.SelfBilledInvoice: return String.Empty;
                default: return String.Empty;
            }
        } // !_translateInvoiceType()


        private int _encodeInvoiceType(InvoiceType type)
        {
            if ((int)type > 1000)
            {
                type -= 1000;
            }

            // only these types are allowed
            // 84: 'Wertbelastung/Wertrechnung ohne Warenbezug'
            // 380: 'Handelsrechnung (Rechnung für Waren und Dienstleistungen)'
            // 389: 'Selbst ausgestellte Rechnung (Steuerrechtliche Gutschrift/Gutschriftsverfahren)'
            //
            // this is documented in ZUGFeRD-Format_1p0_c1p0_Codelisten.pdf
            // all other types are mapped accordingly
            switch (type)
            {
                case InvoiceType.SelfBilledInvoice: return (int)InvoiceType.SelfBilledInvoice;
                case InvoiceType.DebitnoteRelatedToFinancialAdjustments: return (int)InvoiceType.DebitnoteRelatedToFinancialAdjustments;                
                default: return (int)InvoiceType.Invoice;
            }
        } // !_translateInvoiceType()
    }
}
