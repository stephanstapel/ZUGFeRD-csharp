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
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using System.IO;


namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor1Writer : IInvoiceDescriptorWriter
    {
        private ProfileAwareXmlTextWriter Writer;
        private InvoiceDescriptor Descriptor;


        /// <summary>
        /// Saves the given invoice to the given stream.
        /// Make sure that the stream is open and writeable. Otherwise, an IllegalStreamException will be thron.        
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="stream"></param>
        public override void Save(InvoiceDescriptor descriptor, Stream stream)
        {
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new IllegalStreamException("Cannot write to stream");
            }

            // validate data
            if ((descriptor.Profile == Profile.BasicWL) || (descriptor.Profile == Profile.Minimum))
            {
                throw new UnsupportedException("Invalid profile used for ZUGFeRD 1.x invoice.");
            }

            // write data
            long streamPosition = stream.Position;

            this.Descriptor = descriptor;
            this.Writer = new ProfileAwareXmlTextWriter(stream, Encoding.UTF8, descriptor.Profile);
            Writer.Formatting = Formatting.Indented;
            Writer.WriteStartDocument();

            #region Kopfbereich
            Writer.WriteStartElement("rsm:CrossIndustryDocument");
            Writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            Writer.WriteAttributeString("xmlns", "rsm", null, "urn:ferd:CrossIndustryDocument:invoice:1p0");
            Writer.WriteAttributeString("xmlns", "ram", null, "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:12");
            Writer.WriteAttributeString("xmlns", "udt", null, "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:15");
            #endregion

            #region SpecifiedExchangedDocumentContext
            Writer.WriteStartElement("rsm:SpecifiedExchangedDocumentContext");
            Writer.WriteStartElement("ram:TestIndicator");
            Writer.WriteElementString("udt:Indicator", this.Descriptor.IsTest ? "true" : "false");
            Writer.WriteEndElement(); // !ram:TestIndicator

            if (!String.IsNullOrWhiteSpace(this.Descriptor.BusinessProcess))
            {
                Writer.WriteStartElement("ram:BusinessProcessSpecifiedDocumentContextParameter", Profile.Extended);
                Writer.WriteElementString("ram:ID", this.Descriptor.BusinessProcess, Profile.Extended);
                Writer.WriteEndElement(); // !ram:BusinessProcessSpecifiedDocumentContextParameter
            }

            Writer.WriteStartElement("ram:GuidelineSpecifiedDocumentContextParameter");
            Writer.WriteElementString("ram:ID", this.Descriptor.Profile.EnumToString(ZUGFeRDVersion.Version1));
            Writer.WriteEndElement(); // !ram:GuidelineSpecifiedDocumentContextParameter
            Writer.WriteEndElement(); // !rsm:SpecifiedExchangedDocumentContext

            Writer.WriteStartElement("rsm:HeaderExchangedDocument");
            Writer.WriteElementString("ram:ID", this.Descriptor.InvoiceNo);
            Writer.WriteElementString("ram:Name", _translateInvoiceType(this.Descriptor.Type));
            Writer.WriteElementString("ram:TypeCode", String.Format("{0}", _encodeInvoiceType(this.Descriptor.Type)));

            if (this.Descriptor.InvoiceDate.HasValue)
            {
                Writer.WriteStartElement("ram:IssueDateTime");
                Writer.WriteStartElement("udt:DateTimeString");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor.InvoiceDate.Value));
                Writer.WriteEndElement(); // !udt:DateTimeString
                Writer.WriteEndElement(); // !IssueDateTime
            }
            _writeNotes(Writer, this.Descriptor.Notes);
            Writer.WriteEndElement(); // !rsm:HeaderExchangedDocument
            #endregion

            #region SpecifiedSupplyChainTradeTransaction
            Writer.WriteStartElement("rsm:SpecifiedSupplyChainTradeTransaction");
            Writer.WriteStartElement("ram:ApplicableSupplyChainTradeAgreement");
            if (!String.IsNullOrWhiteSpace(this.Descriptor.ReferenceOrderNo))
            {
                Writer.WriteElementString("ram:BuyerReference", this.Descriptor.ReferenceOrderNo);
            }

            _writeOptionalParty(Writer, "ram:SellerTradeParty", this.Descriptor.Seller, this.Descriptor.SellerContact, TaxRegistrations: this.Descriptor.SellerTaxRegistration);
            _writeOptionalParty(Writer, "ram:BuyerTradeParty", this.Descriptor.Buyer, this.Descriptor.BuyerContact, TaxRegistrations: this.Descriptor.BuyerTaxRegistration);

            if (!String.IsNullOrWhiteSpace(this.Descriptor.OrderNo))
            {
                Writer.WriteStartElement("ram:BuyerOrderReferencedDocument");
                if (this.Descriptor.OrderDate.HasValue)
                {
                    Writer.WriteStartElement("ram:IssueDateTime");
                    //Writer.WriteStartElement("udt:DateTimeString");
                    //Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.OrderDate.Value, false));
                    //Writer.WriteEndElement(); // !udt:DateTimeString
                    Writer.WriteEndElement(); // !IssueDateTime()
                }

                Writer.WriteElementString("ram:ID", this.Descriptor.OrderNo);
                Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }


            foreach (AdditionalReferencedDocument document in this.Descriptor.AdditionalReferencedDocuments)
            {
                Writer.WriteStartElement("ram:AdditionalReferencedDocument");
                if (document.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram:IssueDateTime");
                    //Writer.WriteStartElement("udt:DateTimeString");
                    //Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(document.IssueDateTime.Value, false));
                    //Writer.WriteEndElement(); // !udt:DateTimeString
                    Writer.WriteEndElement(); // !IssueDateTime()
                }

                if (document.ReferenceTypeCode != ReferenceTypeCodes.Unknown)
                {
                    Writer.WriteElementString("ram:TypeCode", document.ReferenceTypeCode.EnumToString());
                }

                Writer.WriteElementString("ram:ID", document.ID);
                Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
            } // !foreach(document)

            Writer.WriteEndElement(); // !ApplicableSupplyChainTradeAgreement

            Writer.WriteStartElement("ram:ApplicableSupplyChainTradeDelivery"); // Pflichteintrag

            if (Descriptor.Profile == Profile.Extended)
            {
                _writeOptionalParty(Writer, "ram:ShipToTradeParty", this.Descriptor.ShipTo);
                _writeOptionalParty(Writer, "ram:ShipFromTradeParty", this.Descriptor.ShipFrom);
            }

            if (this.Descriptor.ActualDeliveryDate.HasValue)
            {
                Writer.WriteStartElement("ram:ActualDeliverySupplyChainEvent");
                Writer.WriteStartElement("ram:OccurrenceDateTime");
                Writer.WriteStartElement("udt:DateTimeString");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor.ActualDeliveryDate.Value));
                Writer.WriteEndElement(); // "udt:DateTimeString
                Writer.WriteEndElement(); // !OccurrenceDateTime()
                Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
            }

            if (this.Descriptor.DeliveryNoteReferencedDocument != null)
            {
                Writer.WriteStartElement("ram:DeliveryNoteReferencedDocument");

                if (this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram:IssueDateTime");
                    Writer.WriteValue(_formatDate(this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.Value, false));
                    Writer.WriteEndElement(); // !IssueDateTime
                }

                Writer.WriteElementString("ram:ID", this.Descriptor.DeliveryNoteReferencedDocument.ID);
                Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }

            Writer.WriteEndElement(); // !ApplicableSupplyChainTradeDelivery

            Writer.WriteStartElement("ram:ApplicableSupplyChainTradeSettlement");
            Writer.WriteElementString("ram:InvoiceCurrencyCode", this.Descriptor.Currency.EnumToString());

            if (Descriptor.Profile != Profile.Basic)
            {
                _writeOptionalParty(Writer, "ram:InvoiceeTradeParty", this.Descriptor.Invoicee);
            }
            if (Descriptor.Profile == Profile.Extended)
            {
                _writeOptionalParty(Writer, "ram:PayeeTradeParty", this.Descriptor.Payee);
            }

            Writer.WriteOptionalElementString("ram:PaymentReference", this.Descriptor.PaymentReference);

            if (this.Descriptor.CreditorBankAccounts.Count == 0 && this.Descriptor.DebitorBankAccounts.Count == 0)
            {
                if (this.Descriptor.PaymentMeans != null)
                {
                    Writer.WriteStartElement("ram:SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram:TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram:Information", this.Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans.SEPAMandateReference))
                        {
                            Writer.WriteStartElement("ram:ID");
                            Writer.WriteAttributeString("schemeAgencyID", this.Descriptor.PaymentMeans.SEPACreditorIdentifier);
                            Writer.WriteValue(this.Descriptor.PaymentMeans.SEPAMandateReference);
                            Writer.WriteEndElement(); // !ram:ID
                        }
                    }
                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }
            else
            {
                foreach (BankAccount account in this.Descriptor.CreditorBankAccounts)
                {
                    Writer.WriteStartElement("ram:SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram:TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram:Information", this.Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans.SEPAMandateReference))
                        {
                            Writer.WriteStartElement("ram:ID");
                            Writer.WriteAttributeString("schemeAgencyID", this.Descriptor.PaymentMeans.SEPACreditorIdentifier);
                            Writer.WriteValue(this.Descriptor.PaymentMeans.SEPAMandateReference);
                            Writer.WriteEndElement(); // !ram:ID
                        }
                    }

                    Writer.WriteStartElement("ram:PayeePartyCreditorFinancialAccount");
                    Writer.WriteElementString("ram:IBANID", account.IBAN);
                    if (!String.IsNullOrWhiteSpace(account.Name))
                    {
                        Writer.WriteOptionalElementString("ram:AccountName", account.Name);
                    }
                    Writer.WriteOptionalElementString("ram:ProprietaryID", account.ID);
                    Writer.WriteEndElement(); // !PayeePartyCreditorFinancialAccount

                    Writer.WriteStartElement("ram:PayeeSpecifiedCreditorFinancialInstitution");
                    Writer.WriteElementString("ram:BICID", account.BIC);
                    Writer.WriteOptionalElementString("ram:GermanBankleitzahlID", account.Bankleitzahl);
                    Writer.WriteOptionalElementString("ram:Name", account.BankName);
                    Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }

                foreach (BankAccount account in this.Descriptor.DebitorBankAccounts)
                {
                    Writer.WriteStartElement("ram:SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram:TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram:Information", this.Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans.SEPAMandateReference))
                        {
                            Writer.WriteStartElement("ram:ID");
                            Writer.WriteAttributeString("schemeAgencyID", this.Descriptor.PaymentMeans.SEPACreditorIdentifier);
                            Writer.WriteValue(this.Descriptor.PaymentMeans.SEPAMandateReference);
                            Writer.WriteEndElement(); // !ram:ID
                        }
                    }

                    Writer.WriteStartElement("ram:PayerPartyDebtorFinancialAccount");
                    Writer.WriteElementString("ram:IBANID", account.IBAN);
                    Writer.WriteOptionalElementString("ram:ProprietaryID", account.ID);
                    Writer.WriteEndElement(); // !PayerPartyDebtorFinancialAccount

                    Writer.WriteStartElement("ram:PayerSpecifiedDebtorFinancialInstitution");
                    Writer.WriteElementString("ram:BICID", account.BIC);
                    Writer.WriteOptionalElementString("ram:GermanBankleitzahlID", account.Bankleitzahl);
                    Writer.WriteOptionalElementString("ram:Name", account.BankName);
                    Writer.WriteEndElement(); // !PayerSpecifiedDebtorFinancialInstitution
                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }

            _writeOptionalTaxes(Writer);

            foreach (TradeAllowanceCharge tradeAllowanceCharge in this.Descriptor.GetTradeAllowanceCharges())
            {
                Writer.WriteStartElement("ram:SpecifiedTradeAllowanceCharge");
                Writer.WriteStartElement("ram:ChargeIndicator", Profile.Comfort | Profile.Extended);
                Writer.WriteElementString("udt:Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                Writer.WriteEndElement(); // !ram:ChargeIndicator

                if (tradeAllowanceCharge.BasisAmount.HasValue)
                {
                    Writer.WriteStartElement("ram:BasisAmount", Profile.Extended);
                    Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value));
                    Writer.WriteEndElement();
                }

                Writer.WriteStartElement("ram:ActualAmount", Profile.Comfort | Profile.Extended);
                Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 2));
                Writer.WriteEndElement();


                Writer.WriteOptionalElementString("ram:Reason", tradeAllowanceCharge.Reason, Profile.Comfort | Profile.Extended);

                if (tradeAllowanceCharge.Tax != null)
                {
                    Writer.WriteStartElement("ram:CategoryTradeTax");
                    Writer.WriteElementString("ram:TypeCode", tradeAllowanceCharge.Tax.TypeCode.EnumToString(), Profile.Comfort | Profile.Extended);
                    if (tradeAllowanceCharge.Tax.CategoryCode.HasValue)
                        Writer.WriteElementString("ram:CategoryCode", tradeAllowanceCharge.Tax.CategoryCode?.EnumToString(), Profile.Comfort | Profile.Extended);
                    Writer.WriteElementString("ram:ApplicablePercent", _formatDecimal(tradeAllowanceCharge.Tax.Percent), Profile.Comfort | Profile.Extended);
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();
            }

            foreach (ServiceCharge serviceCharge in this.Descriptor.ServiceCharges)
            {
                Writer.WriteStartElement("ram:SpecifiedLogisticsServiceCharge");
                Writer.WriteOptionalElementString("ram:Description", serviceCharge.Description, Profile.Comfort | Profile.Extended);
                Writer.WriteElementString("ram:AppliedAmount", _formatDecimal(serviceCharge.Amount), Profile.Comfort | Profile.Extended);
                if (serviceCharge.Tax != null)
                {
                    Writer.WriteStartElement("ram:AppliedTradeTax");
                    Writer.WriteElementString("ram:TypeCode", serviceCharge.Tax.TypeCode.EnumToString(), Profile.Comfort | Profile.Extended);
                    if (serviceCharge.Tax.CategoryCode.HasValue)
                        Writer.WriteElementString("ram:CategoryCode", serviceCharge.Tax.CategoryCode?.EnumToString(), Profile.Comfort | Profile.Extended);
                    Writer.WriteElementString("ram:ApplicablePercent", _formatDecimal(serviceCharge.Tax.Percent), Profile.Comfort | Profile.Extended);
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();
            }

            if (this.Descriptor.PaymentTerms != null)
            {
                Writer.WriteStartElement("ram:SpecifiedTradePaymentTerms");
                Writer.WriteOptionalElementString("ram:Description", this.Descriptor.PaymentTerms.Description);
                if (this.Descriptor.PaymentTerms.DueDate.HasValue)
                {
                    Writer.WriteStartElement("ram:DueDateDateTime");
                    _writeElementWithAttribute(Writer, "udt:DateTimeString", "format", "102", _formatDate(this.Descriptor.PaymentTerms.DueDate.Value));
                    Writer.WriteEndElement(); // !ram:DueDateDateTime
                }
                Writer.WriteEndElement();
            }

            Writer.WriteStartElement("ram:SpecifiedTradeSettlementMonetarySummation");
            _writeOptionalAmount(Writer, "ram:LineTotalAmount", this.Descriptor.LineTotalAmount);

            _writeOptionalAmount(Writer, "ram:ChargeTotalAmount", this.Descriptor.ChargeTotalAmount);
            _writeOptionalAmount(Writer, "ram:AllowanceTotalAmount", this.Descriptor.AllowanceTotalAmount);
            _writeOptionalAmount(Writer, "ram:TaxBasisTotalAmount", this.Descriptor.TaxBasisAmount);
            _writeOptionalAmount(Writer, "ram:TaxTotalAmount", this.Descriptor.TaxTotalAmount);
            _writeOptionalAmount(Writer, "ram:GrandTotalAmount", this.Descriptor.GrandTotalAmount);
            _writeOptionalAmount(Writer, "ram:TotalPrepaidAmount", this.Descriptor.TotalPrepaidAmount);
            _writeOptionalAmount(Writer, "ram:DuePayableAmount", this.Descriptor.DuePayableAmount);
            Writer.WriteEndElement(); // !ram:SpecifiedTradeSettlementMonetarySummation

            Writer.WriteEndElement(); // !ram:ApplicableSupplyChainTradeSettlement

            foreach (TradeLineItem tradeLineItem in this.Descriptor.TradeLineItems)
            {
                Writer.WriteStartElement("ram:IncludedSupplyChainTradeLineItem");

                if (tradeLineItem.AssociatedDocument != null)
                {
                    Writer.WriteStartElement("ram:AssociatedDocumentLineDocument");
                    Writer.WriteOptionalElementString("ram:LineID", tradeLineItem.AssociatedDocument.LineID);
                    _writeNotes(Writer, tradeLineItem.AssociatedDocument.Notes);
                    Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument
                }

                // handelt es sich um einen Kommentar?
                if ((tradeLineItem.AssociatedDocument?.Notes.Count > 0) && (tradeLineItem.BilledQuantity == 0) && (String.IsNullOrWhiteSpace(tradeLineItem.Description)))
                {
                    Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
                    continue;
                }

                if (Descriptor.Profile != Profile.Basic)
                {
                    Writer.WriteStartElement("ram:SpecifiedSupplyChainTradeAgreement");

                    if (tradeLineItem.BuyerOrderReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram:BuyerOrderReferencedDocument");
                        if (tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.HasValue)
                        {
                            Writer.WriteStartElement("ram:IssueDateTime");
                            Writer.WriteValue(_formatDate(tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.Value, false));
                            Writer.WriteEndElement(); // !ram:IssueDateTime
                        }
                        Writer.WriteOptionalElementString("ram:ID", tradeLineItem.BuyerOrderReferencedDocument.ID);
                        Writer.WriteEndElement(); // !ram:BuyerOrderReferencedDocument
                    }

                    if (tradeLineItem.ContractReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram:ContractReferencedDocument");
                        if (tradeLineItem.ContractReferencedDocument.IssueDateTime.HasValue)
                        {
                            Writer.WriteStartElement("ram:IssueDateTime");
                            Writer.WriteValue(_formatDate(tradeLineItem.ContractReferencedDocument.IssueDateTime.Value, false));
                            Writer.WriteEndElement(); // !ram:IssueDateTime
                        }
                        Writer.WriteOptionalElementString("ram:ID", tradeLineItem.ContractReferencedDocument.ID);
                        Writer.WriteEndElement(); // !ram:ContractReferencedDocument
                    }

                    if (tradeLineItem.AdditionalReferencedDocuments != null)
                    {
                        foreach (AdditionalReferencedDocument document in tradeLineItem.AdditionalReferencedDocuments)
                        {
                            Writer.WriteStartElement("ram:AdditionalReferencedDocument");
                            if (document.IssueDateTime.HasValue)
                            {
                                Writer.WriteStartElement("ram:IssueDateTime");
                                Writer.WriteValue(_formatDate(document.IssueDateTime.Value, false));
                                Writer.WriteEndElement(); // !ram:IssueDateTime
                            }

                            Writer.WriteElementString("ram:LineID", String.Format("{0}", tradeLineItem.AssociatedDocument?.LineID));
                            Writer.WriteOptionalElementString("ram:ID", document.ID);
                            Writer.WriteElementString("ram:ReferenceTypeCode", document.ReferenceTypeCode.EnumToString());

                            Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                        }
                    }

                    Writer.WriteStartElement("ram:GrossPriceProductTradePrice");
                    _writeOptionalAmount(Writer, "ram:ChargeAmount", tradeLineItem.GrossUnitPrice, 4);
                    if (tradeLineItem.UnitQuantity.HasValue)
                    {
                        _writeElementWithAttribute(Writer, "ram:BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 4));
                    }

                    foreach (TradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.GetTradeAllowanceCharges())
                    {
                        Writer.WriteStartElement("ram:AppliedTradeAllowanceCharge");

                        Writer.WriteStartElement("ram:ChargeIndicator", Profile.Comfort | Profile.Extended);
                        Writer.WriteElementString("udt:Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                        Writer.WriteEndElement(); // !ram:ChargeIndicator

                        if (tradeAllowanceCharge.BasisAmount.HasValue)
                        {
                            Writer.WriteStartElement("ram:BasisAmount", Profile.Extended);
                            Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                            Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value, 4));
                            Writer.WriteEndElement();
                        }
                        Writer.WriteStartElement("ram:ActualAmount", Profile.Comfort | Profile.Extended);
                        Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                        Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 4));
                        Writer.WriteEndElement();

                        Writer.WriteOptionalElementString("ram:Reason", tradeAllowanceCharge.Reason, Profile.Comfort | Profile.Extended);

                        Writer.WriteEndElement(); // !AppliedTradeAllowanceCharge
                    }

                    Writer.WriteEndElement(); // ram:GrossPriceProductTradePrice

                    Writer.WriteStartElement("ram:NetPriceProductTradePrice");
                    _writeOptionalAmount(Writer, "ram:ChargeAmount", tradeLineItem.NetUnitPrice, 4);

                    if (tradeLineItem.UnitQuantity.HasValue)
                    {
                        _writeElementWithAttribute(Writer, "ram:BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 4));
                    }
                    Writer.WriteEndElement(); // ram:NetPriceProductTradePrice

                    Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeAgreement
                }

                if (Descriptor.Profile != Profile.Basic)
                {
                    Writer.WriteStartElement("ram:SpecifiedSupplyChainTradeDelivery");
                    _writeElementWithAttribute(Writer, "ram:BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));

                    if (tradeLineItem.DeliveryNoteReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram:DeliveryNoteReferencedDocument");
                        if (tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                        {
                            Writer.WriteStartElement("ram:IssueDateTime");
                            Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value, false));
                            Writer.WriteEndElement(); // !ram:IssueDateTime
                        }
                        Writer.WriteOptionalElementString("ram:ID", tradeLineItem.DeliveryNoteReferencedDocument.ID);
                        Writer.WriteEndElement(); // !ram:DeliveryNoteReferencedDocument
                    }

                    if (tradeLineItem.ActualDeliveryDate.HasValue)
                    {
                        Writer.WriteStartElement("ram:ActualDeliverySupplyChainEvent");
                        Writer.WriteStartElement("ram:OccurrenceDateTime");
                        Writer.WriteStartElement("udt:DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(tradeLineItem.ActualDeliveryDate.Value));
                        Writer.WriteEndElement(); // "udt:DateTimeString
                        Writer.WriteEndElement(); // !OccurrenceDateTime()
                        Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
                    }

                    Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeDelivery
                }
                else
                {
                    Writer.WriteStartElement("ram:SpecifiedSupplyChainTradeDelivery");
                    _writeElementWithAttribute(Writer, "ram:BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));
                    Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeDelivery
                }

                Writer.WriteStartElement("ram:SpecifiedSupplyChainTradeSettlement");

                if (Descriptor.Profile != Profile.Basic)
                {
                    Writer.WriteStartElement("ram:ApplicableTradeTax");
                    Writer.WriteElementString("ram:TypeCode", tradeLineItem.TaxType.EnumToString());
                    Writer.WriteElementString("ram:CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString());
                    Writer.WriteElementString("ram:ApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent));
                    Writer.WriteEndElement(); // !ram:ApplicableTradeTax
                }

                if (tradeLineItem.BillingPeriodStart.HasValue && tradeLineItem.BillingPeriodEnd.HasValue)
                {
                    Writer.WriteStartElement("ram:BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                    Writer.WriteStartElement("ram:StartDateTime");
                    _writeElementWithAttribute(Writer, "udt:DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodStart.Value));
                    Writer.WriteEndElement(); // !StartDateTime


                    Writer.WriteStartElement("ram:EndDateTime");
                    _writeElementWithAttribute(Writer, "udt:DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodEnd.Value));
                    Writer.WriteEndElement(); // !EndDateTime

                    Writer.WriteEndElement(); // !BillingSpecifiedPeriod
                }

                Writer.WriteStartElement("ram:SpecifiedTradeSettlementMonetarySummation");

                decimal _total = 0m;

                if (tradeLineItem.LineTotalAmount.HasValue)
                {
                    _total = tradeLineItem.LineTotalAmount.Value;
                }
                else if (tradeLineItem.NetUnitPrice.HasValue)
                {
                    _total = tradeLineItem.NetUnitPrice.Value * tradeLineItem.BilledQuantity;
                    if (tradeLineItem.UnitQuantity.HasValue && (tradeLineItem.UnitQuantity.Value != 0))
                    {
                        _total /= tradeLineItem.UnitQuantity.Value;
                    }
                }

                _writeElementWithAttribute(Writer, "ram:LineTotalAmount", "currencyID", this.Descriptor.Currency.EnumToString(), _formatDecimal(_total));
                Writer.WriteEndElement(); // ram:SpecifiedTradeSettlementMonetarySummation
                Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeSettlement

                Writer.WriteStartElement("ram:SpecifiedTradeProduct");
                if ((tradeLineItem.GlobalID != null) && (tradeLineItem.GlobalID.SchemeID != GlobalIDSchemeIdentifiers.Unknown) && !String.IsNullOrWhiteSpace(tradeLineItem.GlobalID.ID))
                {
                    _writeElementWithAttribute(Writer, "ram:GlobalID", "schemeID", tradeLineItem.GlobalID.SchemeID.EnumToString(), tradeLineItem.GlobalID.ID);
                }

                Writer.WriteOptionalElementString("ram:SellerAssignedID", tradeLineItem.SellerAssignedID);
                Writer.WriteOptionalElementString("ram:BuyerAssignedID", tradeLineItem.BuyerAssignedID);
                Writer.WriteOptionalElementString("ram:Name", tradeLineItem.Name);
                Writer.WriteOptionalElementString("ram:Description", tradeLineItem.Description);

                Writer.WriteEndElement(); // !ram:SpecifiedTradeProduct
                Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
            } // !foreach(tradeLineItem)

            Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeTransaction
            #endregion

            Writer.WriteEndElement(); // !ram:Invoice
            Writer.WriteEndDocument();
            Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);
        } // !Save()  


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
                if (!descriptor.TradeLineItems.All(l => l.TaxType.Equals(TaxTypes.VAT) || l.TaxType.Equals(TaxTypes.Unknown)))
                {
                    if (throwExceptions) { throw new UnsupportedException("Tax types other than VAT only possible with extended profile."); }
                    return false;
                }
            }

            return true;
        } // !Validate()


        private void _writeOptionalAmount(ProfileAwareXmlTextWriter writer, string tagName, decimal? value, int numDecimals = 2)
        {
            if (value.HasValue)
            {
                writer.WriteStartElement(tagName);
                writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                writer.WriteValue(_formatDecimal(value.Value, numDecimals));
                writer.WriteEndElement(); // !tagName
            }
        } // !_writeOptionalAmount()


        private void _writeElementWithAttribute(ProfileAwareXmlTextWriter writer, string tagName, string attributeName, string attributeValue, string nodeValue)
        {
            writer.WriteStartElement(tagName);
            writer.WriteAttributeString(attributeName, attributeValue);
            writer.WriteValue(nodeValue);
            writer.WriteEndElement(); // !tagName
        } // !_writeElementWithAttribute()


        private void _writeOptionalTaxes(ProfileAwareXmlTextWriter writer)
        {
            foreach (Tax tax in this.Descriptor.Taxes)
            {
                writer.WriteStartElement("ram:ApplicableTradeTax");

                writer.WriteStartElement("ram:CalculatedAmount");
                writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                writer.WriteValue(_formatDecimal(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount

                writer.WriteElementString("ram:TypeCode", tax.TypeCode.EnumToString());

                writer.WriteStartElement("ram:BasisAmount");
                writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                writer.WriteValue(_formatDecimal(tax.BasisAmount));
                writer.WriteEndElement(); // !BasisAmount

                if (tax.AllowanceChargeBasisAmount != 0)
                {
                    writer.WriteStartElement("ram:AllowanceChargeBasisAmount");
                    writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                    writer.WriteValue(_formatDecimal(tax.AllowanceChargeBasisAmount));
                    writer.WriteEndElement(); // !AllowanceChargeBasisAmount
                }

                if (tax.CategoryCode.HasValue)
                {
                    writer.WriteElementString("ram:CategoryCode", tax.CategoryCode?.EnumToString());
                }
                writer.WriteElementString("ram:ApplicablePercent", _formatDecimal(tax.Percent));
                writer.WriteEndElement(); // !ApplicableTradeTax
            }
        } // !_writeOptionalTaxes()


        private void _writeNotes(ProfileAwareXmlTextWriter writer, List<Note> notes)
        {
            if (notes.Count > 0)
            {
                foreach (Note note in notes)
                {
                    writer.WriteStartElement("ram:IncludedNote");
                    if (note.ContentCode != ContentCodes.Unknown)
                    {
                        writer.WriteElementString("ram:ContentCode", note.ContentCode.EnumToString());
                    }
                    writer.WriteElementString("ram:Content", note.Content);
                    if (note.SubjectCode != SubjectCodes.Unknown)
                    {
                        writer.WriteElementString("ram:SubjectCode", note.SubjectCode.EnumToString());
                    }
                    writer.WriteEndElement();
                }
            }
        } // !_writeNotes()


        private void _writeOptionalParty(ProfileAwareXmlTextWriter writer, string PartyTag, Party Party, Contact Contact = null, List<TaxRegistration> TaxRegistrations = null)
        {
            if (Party != null)
            {
                writer.WriteStartElement(PartyTag);

                if (Party.ID != null)
                {
                    if (!String.IsNullOrWhiteSpace(Party.ID.ID) && (Party.ID.SchemeID != GlobalIDSchemeIdentifiers.Unknown))
                    {
                        writer.WriteStartElement("ram:ID");
                        writer.WriteAttributeString("schemeID", Party.ID.SchemeID.EnumToString());
                        writer.WriteValue(Party.ID.ID);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteElementString("ram:ID", Party.ID.ID);
                    }
                }

                if ((Party.GlobalID != null) && !String.IsNullOrWhiteSpace(Party.GlobalID.ID) && (Party.GlobalID.SchemeID != GlobalIDSchemeIdentifiers.Unknown))
                {
                    writer.WriteStartElement("ram:GlobalID");
                    writer.WriteAttributeString("schemeID", Party.GlobalID.SchemeID.EnumToString());
                    writer.WriteValue(Party.GlobalID.ID);
                    writer.WriteEndElement();
                }

                Writer.WriteOptionalElementString("ram:Name", Party.Name);
                _writeOptionalContact(writer, "ram:DefinedTradeContact", Contact);
                writer.WriteStartElement("ram:PostalTradeAddress");
                writer.WriteOptionalElementString("ram:PostcodeCode", Party.Postcode);
                writer.WriteOptionalElementString("ram:LineOne", string.IsNullOrWhiteSpace(Party.ContactName) ? Party.Street : Party.ContactName);
                if (!string.IsNullOrWhiteSpace(Party.ContactName)) { writer.WriteOptionalElementString("ram:LineTwo", Party.Street); }
                writer.WriteOptionalElementString("ram:CityName", Party.City);
                writer.WriteElementString("ram:CountryID", Party.Country.EnumToString());
                writer.WriteEndElement(); // !PostalTradeAddress

                if (TaxRegistrations != null)
                {
                    foreach (TaxRegistration _reg in TaxRegistrations)
                    {
                        if (!String.IsNullOrWhiteSpace(_reg.No))
                        {
                            writer.WriteStartElement("ram:SpecifiedTaxRegistration");
                            writer.WriteStartElement("ram:ID");
                            writer.WriteAttributeString("schemeID", _reg.SchemeID.EnumToString());
                            writer.WriteValue(_reg.No);
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                }
                writer.WriteEndElement(); // !*TradeParty
            }
        } // !_writeOptionalParty()


        private void _writeOptionalContact(ProfileAwareXmlTextWriter writer, string contactTag, Contact contact)
        {
            if (contact != null)
            {
                writer.WriteStartElement(contactTag);

                writer.WriteOptionalElementString("ram:PersonName", contact.Name);
                writer.WriteOptionalElementString("ram:DepartmentName", contact.OrgUnit);

                if (!String.IsNullOrWhiteSpace(contact.PhoneNo))
                {
                    writer.WriteStartElement("ram:TelephoneUniversalCommunication");
                    writer.WriteElementString("ram:CompleteNumber", contact.PhoneNo);
                    writer.WriteEndElement();
                }

                if (!String.IsNullOrWhiteSpace(contact.FaxNo))
                {
                    writer.WriteStartElement("ram:FaxUniversalCommunication");
                    writer.WriteElementString("ram:CompleteNumber", contact.FaxNo);
                    writer.WriteEndElement();
                }

                if (!String.IsNullOrWhiteSpace(contact.EmailAddress))
                {
                    writer.WriteStartElement("ram:EmailURIUniversalCommunication");
                    writer.WriteElementString("ram:URIID", contact.EmailAddress);
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
                case InvoiceType.DebitNote: return "";
                case InvoiceType.SelfBilledInvoice: return "";
                default: return "";
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
                case InvoiceType.Unknown: return (int)InvoiceType.Unknown;
                default: return (int)InvoiceType.Invoice;
            }
        } // !_translateInvoiceType()
    }
}
