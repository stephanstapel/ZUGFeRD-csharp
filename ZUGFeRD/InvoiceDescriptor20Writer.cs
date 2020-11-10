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
using System.Text;
using System.Xml;


namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor20Writer : IInvoiceDescriptorWriter
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

            // write data
            long streamPosition = stream.Position;

            this.Descriptor = descriptor;
            this.Writer = new ProfileAwareXmlTextWriter(stream, Encoding.UTF8, descriptor.Profile);
            Writer.Formatting = Formatting.Indented;
            Writer.WriteStartDocument();

            #region Kopfbereich
            Writer.WriteStartElement("rsm:CrossIndustryInvoice");
            Writer.WriteAttributeString("xmlns", "a", null, "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            Writer.WriteAttributeString("xmlns", "rsm", null, "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            Writer.WriteAttributeString("xmlns", "qdt", null, "urn:un:unece:uncefact:data:standard:QualifiedDataType:10");
            Writer.WriteAttributeString("xmlns", "ram", null, "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            Writer.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");
            Writer.WriteAttributeString("xmlns", "udt", null, "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");
            #endregion

            #region SpecifiedExchangedDocumentContext
            Writer.WriteStartElement("rsm:ExchangedDocumentContext");
            Writer.WriteStartElement("ram:TestIndicator");
            Writer.WriteElementString("udt:Indicator", this.Descriptor.IsTest ? "true" : "false");
            Writer.WriteEndElement(); // !ram:TestIndicator
            Writer.WriteStartElement("ram:GuidelineSpecifiedDocumentContextParameter");
            Writer.WriteElementString("ram:ID", this.Descriptor.Profile.EnumToString(ZUGFeRDVersion.Version20));
            Writer.WriteEndElement(); // !ram:GuidelineSpecifiedDocumentContextParameter
            Writer.WriteEndElement(); // !rsm:ExchangedDocumentContext

            Writer.WriteStartElement("rsm:ExchangedDocument");
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
            Writer.WriteEndElement(); // !rsm:ExchangedDocument
            #endregion

            /*
             * @todo continue here to adopt v2 tag names
             */

            #region SpecifiedSupplyChainTradeTransaction
            Writer.WriteStartElement("rsm:SupplyChainTradeTransaction");

            foreach (TradeLineItem tradeLineItem in this.Descriptor.TradeLineItems)
            {
                Writer.WriteStartElement("ram:IncludedSupplyChainTradeLineItem");

                if (tradeLineItem.AssociatedDocument != null)
                {
                    Writer.WriteStartElement("ram:AssociatedDocumentLineDocument");
                    if (!String.IsNullOrEmpty(tradeLineItem.AssociatedDocument.LineID))
                    {
                        Writer.WriteElementString("ram:LineID", tradeLineItem.AssociatedDocument.LineID);
                    }
                    _writeNotes(Writer, tradeLineItem.AssociatedDocument.Notes);
                    Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument
                }

                // handelt es sich um einen Kommentar?
                if ((tradeLineItem.AssociatedDocument?.Notes.Count > 0) && (tradeLineItem.BilledQuantity == 0) && (String.IsNullOrEmpty(tradeLineItem.Description)))
                {
                    Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
                    continue;
                }

                Writer.WriteStartElement("ram:SpecifiedTradeProduct");
                if ((tradeLineItem.GlobalID != null) && !String.IsNullOrEmpty(tradeLineItem.GlobalID.SchemeID) && !String.IsNullOrEmpty(tradeLineItem.GlobalID.ID))
                {
                    _writeElementWithAttribute(Writer, "ram:GlobalID", "schemeID", tradeLineItem.GlobalID.SchemeID, tradeLineItem.GlobalID.ID);
                }

                _writeOptionalElementString(Writer, "ram:SellerAssignedID", tradeLineItem.SellerAssignedID);
                _writeOptionalElementString(Writer, "ram:BuyerAssignedID", tradeLineItem.BuyerAssignedID);
                _writeOptionalElementString(Writer, "ram:Name", tradeLineItem.Name);
                _writeOptionalElementString(Writer, "ram:Description", tradeLineItem.Description);

                Writer.WriteEndElement(); // !ram:SpecifiedTradeProduct

                if (Descriptor.Profile != Profile.Basic)
                {
                    Writer.WriteStartElement("ram:SpecifiedLineTradeAgreement");

                    if (tradeLineItem.BuyerOrderReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram:BuyerOrderReferencedDocument");
                        if (tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.HasValue)
                        {
                            Writer.WriteStartElement("ram:IssueDateTime");
                            Writer.WriteValue(_formatDate(tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.Value, false));
                            Writer.WriteEndElement(); // !ram:IssueDateTime
                        }
                        if (!String.IsNullOrEmpty(tradeLineItem.BuyerOrderReferencedDocument.ID))
                        {
                            Writer.WriteElementString("ram:ID", tradeLineItem.BuyerOrderReferencedDocument.ID);
                        }

                        Writer.WriteEndElement(); // !ram:BuyerOrderReferencedDocument
                    }

                    if (tradeLineItem.ContractReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram:InvoiceReferencedDocument");
                        if (tradeLineItem.ContractReferencedDocument.IssueDateTime.HasValue)
                        {
                            Writer.WriteStartElement("ram:FormattedIssueDateTime");
                            Writer.WriteValue(_formatDate(tradeLineItem.ContractReferencedDocument.IssueDateTime.Value, true));
                            Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                        }
                        if (!String.IsNullOrEmpty(tradeLineItem.ContractReferencedDocument.ID))
                        {
                            Writer.WriteElementString("ram:IssuerAssignedID", tradeLineItem.ContractReferencedDocument.ID);
                        }

                        Writer.WriteEndElement(); // !ram:InvoiceReferencedDocument
                    }

                    if (tradeLineItem.AdditionalReferencedDocuments != null)
                    {
                        foreach (AdditionalReferencedDocument document in tradeLineItem.AdditionalReferencedDocuments)
                        {
                            Writer.WriteStartElement("ram:AdditionalReferencedDocument");
                            if (document.IssueDateTime.HasValue)
                            {
                                Writer.WriteStartElement("ram:FormattedIssueDateTime");
                                Writer.WriteValue(_formatDate(document.IssueDateTime.Value, false));
                                Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                            }

                            Writer.WriteElementString("ram:LineID", String.Format("{0}", tradeLineItem.AssociatedDocument?.LineID));

                            if (!String.IsNullOrEmpty(document.ID))
                            {
                                Writer.WriteElementString("ram:IssuerAssignedID", document.ID);
                            }

                            Writer.WriteElementString("ram:ReferenceTypeCode", document.ReferenceTypeCode.EnumToString());

                            Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                        } // !foreach(document)
                    }

                    Writer.WriteStartElement("ram:GrossPriceProductTradePrice");
                    _writeOptionalAmount(Writer, "ram:ChargeAmount", tradeLineItem.GrossUnitPrice, 4);
                    if (tradeLineItem.UnitQuantity.HasValue)
                    {
                        _writeElementWithAttribute(Writer, "ram:BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 4));
                    }

                    foreach (TradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.TradeAllowanceCharges)
                    {
                        Writer.WriteStartElement("ram:AppliedTradeAllowanceCharge");

                        Writer.WriteStartElement("ram:ChargeIndicator");
                        Writer.WriteElementString("udt:Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                        Writer.WriteEndElement(); // !ram:ChargeIndicator

                        Writer.WriteStartElement("ram:BasisAmount");
                        Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                        Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount, 4));
                        Writer.WriteEndElement();
                        Writer.WriteStartElement("ram:ActualAmount");
                        Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                        Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 4));
                        Writer.WriteEndElement();

                        _writeOptionalElementString(Writer, "ram:Reason", tradeAllowanceCharge.Reason);

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

                    Writer.WriteEndElement(); // !ram:SpecifiedLineTradeAgreement
                }

                if (Descriptor.Profile != Profile.Basic)
                {
                    Writer.WriteStartElement("ram:SpecifiedLineTradeDelivery");
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
                        if (!String.IsNullOrEmpty(tradeLineItem.DeliveryNoteReferencedDocument.ID))
                        {
                            Writer.WriteElementString("ram:ID", tradeLineItem.DeliveryNoteReferencedDocument.ID);
                        }

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

                    Writer.WriteEndElement(); // !ram:SpecifiedLineTradeDelivery
                }
                else
                {
                    Writer.WriteStartElement("ram:SpecifiedLineTradeDelivery");
                    _writeElementWithAttribute(Writer, "ram:BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));
                    Writer.WriteEndElement(); // !ram:SpecifiedLineTradeDelivery
                }

                Writer.WriteStartElement("ram:SpecifiedLineTradeSettlement");

                if (Descriptor.Profile != Profile.Basic)
                {
                    Writer.WriteStartElement("ram:ApplicableTradeTax");
                    Writer.WriteElementString("ram:TypeCode", tradeLineItem.TaxType.EnumToString());
                    Writer.WriteElementString("ram:CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString());
                    Writer.WriteElementString("ram:RateApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent));
                    Writer.WriteEndElement(); // !ram:ApplicableTradeTax
                }

                if (Descriptor.BillingPeriodStart.HasValue || Descriptor.BillingPeriodEnd.HasValue)
                {
                    Writer.WriteStartElement("ram:BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    if (Descriptor.BillingPeriodStart.HasValue)
                    {
                        Writer.WriteStartElement("ram:StartDateTime");
                        _writeElementWithAttribute(Writer, "udt:DateTimeString", "format", "102", _formatDate(this.Descriptor.BillingPeriodStart.Value));
                        Writer.WriteEndElement(); // !StartDateTime
                    }

                    if (Descriptor.BillingPeriodEnd.HasValue)
                    {
                        Writer.WriteStartElement("ram:EndDateTime");
                        _writeElementWithAttribute(Writer, "udt:DateTimeString", "format", "102", _formatDate(this.Descriptor.BillingPeriodEnd.Value));
                        Writer.WriteEndElement(); // !EndDateTime
                    }
                    Writer.WriteEndElement(); // !BillingSpecifiedPeriod
                }

                Writer.WriteStartElement("ram:SpecifiedTradeSettlementLineMonetarySummation");

                decimal _total = 0m;

                if (tradeLineItem.LineTotalAmount.HasValue)
                {
                    _total = tradeLineItem.LineTotalAmount.Value;
                }
                else
                {
                    _total = tradeLineItem.NetUnitPrice * tradeLineItem.BilledQuantity;
                }

                _writeElementWithAttribute(Writer, "ram:LineTotalAmount", "currencyID", this.Descriptor.Currency.EnumToString(), _formatDecimal(_total));
                Writer.WriteEndElement(); // ram:SpecifiedTradeSettlementLineMonetarySummation
                Writer.WriteEndElement(); // !ram:SpecifiedLineTradeSettlement

                Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
            } // !foreach(tradeLineItem)

            Writer.WriteStartElement("ram:ApplicableHeaderTradeAgreement");
            if (!String.IsNullOrEmpty(this.Descriptor.ReferenceOrderNo))
            {
                Writer.WriteElementString("ram:BuyerReference", this.Descriptor.ReferenceOrderNo);
            }

            _writeOptionalParty(Writer, "ram:SellerTradeParty", this.Descriptor.Seller, this.Descriptor.SellerContact, TaxRegistrations: this.Descriptor.SellerTaxRegistration);
            _writeOptionalParty(Writer, "ram:BuyerTradeParty", this.Descriptor.Buyer, this.Descriptor.BuyerContact, TaxRegistrations: this.Descriptor.BuyerTaxRegistration);

            if (this.Descriptor.OrderDate.HasValue || ((this.Descriptor.OrderNo != null) && (this.Descriptor.OrderNo.Length > 0)))
            {
                Writer.WriteStartElement("ram:BuyerOrderReferencedDocument");
                if (this.Descriptor.OrderDate.HasValue)
                {
                    Writer.WriteStartElement("ram:FormattedIssueDateTime");
                    Writer.WriteStartElement("udt:DateTimeString");
                    Writer.WriteValue(_formatDate(this.Descriptor.OrderDate.Value, false));
                    Writer.WriteEndElement(); // !udt:DateTimeString	
                    Writer.WriteEndElement(); // !FormattedIssueDateTime	
                }
                Writer.WriteElementString("ram:IssuerAssignedID", this.Descriptor.OrderNo);
                Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }


            if (this.Descriptor.AdditionalReferencedDocuments != null)
            {
                foreach (AdditionalReferencedDocument document in this.Descriptor.AdditionalReferencedDocuments)
                {
                    Writer.WriteStartElement("ram:AdditionalReferencedDocument");
                    if (document.IssueDateTime.HasValue)
                    {
                        Writer.WriteStartElement("ram:FormattedIssueDateTime");
                        Writer.WriteStartElement("udt:DateTimeString");
                        Writer.WriteValue(_formatDate(document.IssueDateTime.Value, false));
                        Writer.WriteEndElement(); // !udt:DateTimeString
                        Writer.WriteEndElement(); // !FormattedIssueDateTime
                    }

                    if (document.ReferenceTypeCode != ReferenceTypeCodes.Unknown)
                    {
                        Writer.WriteElementString("ram:TypeCode", document.ReferenceTypeCode.EnumToString());
                    }

                    Writer.WriteElementString("ram:ID", document.ID);
                    Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                } // !foreach(document)
            }

            Writer.WriteEndElement(); // !ApplicableHeaderTradeAgreement

            Writer.WriteStartElement("ram:ApplicableHeaderTradeDelivery"); // Pflichteintrag

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
                    Writer.WriteStartElement("ram:FormattedIssueDateTime");
                    Writer.WriteValue(_formatDate(this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.Value, false));
                    Writer.WriteEndElement(); // !IssueDateTime
                }

                Writer.WriteElementString("ram:IssuerAssignedID", this.Descriptor.DeliveryNoteReferencedDocument.ID);
                Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }

            Writer.WriteEndElement(); // !ApplicableHeaderTradeDelivery

            Writer.WriteStartElement("ram:ApplicableHeaderTradeSettlement");

            if (Descriptor.Profile == Profile.Extended)
            {
                _writeOptionalParty(Writer, "ram:InvoiceeTradeParty", this.Descriptor.Invoicee);
            }
            if (Descriptor.Profile != Profile.Minimum)
            {
                _writeOptionalParty(Writer, "ram:PayeeTradeParty", this.Descriptor.Payee);
            }

            if (!String.IsNullOrEmpty(this.Descriptor.PaymentReference))
            {
                _writeOptionalElementString(Writer, "ram:PaymentReference", this.Descriptor.PaymentReference);
            }
            Writer.WriteElementString("ram:InvoiceCurrencyCode", this.Descriptor.Currency.EnumToString());

            if (this.Descriptor.CreditorBankAccounts.Count == 0 && this.Descriptor.DebitorBankAccounts.Count == 0)
            {
                if (this.Descriptor.PaymentMeans != null)
                {
                    Writer.WriteStartElement("ram:SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram:TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteElementString("ram:Information", this.Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrEmpty(this.Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrEmpty(this.Descriptor.PaymentMeans.SEPAMandateReference))
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
                        Writer.WriteElementString("ram:Information", this.Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrEmpty(this.Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrEmpty(this.Descriptor.PaymentMeans.SEPAMandateReference))
                        {
                            Writer.WriteStartElement("ram:ID");
                            Writer.WriteAttributeString("schemeAgencyID", this.Descriptor.PaymentMeans.SEPACreditorIdentifier);
                            Writer.WriteValue(this.Descriptor.PaymentMeans.SEPAMandateReference);
                            Writer.WriteEndElement(); // !ram:ID
                        }
                    }

                    Writer.WriteStartElement("ram:PayeePartyCreditorFinancialAccount");
                    Writer.WriteElementString("ram:IBANID", account.IBAN);
                    if (!String.IsNullOrEmpty(account.Name))
                    {
                        Writer.WriteElementString("ram:AccountName", account.Name);
                    }
                    if (!String.IsNullOrEmpty(account.ID))
                    {
                        Writer.WriteElementString("ram:ProprietaryID", account.ID);
                    }
                    Writer.WriteEndElement(); // !PayeePartyCreditorFinancialAccount

                    Writer.WriteStartElement("ram:PayeeSpecifiedCreditorFinancialInstitution");
                    Writer.WriteElementString("ram:BICID", account.BIC);

                    if (!String.IsNullOrEmpty(account.Bankleitzahl))
                    {
                        Writer.WriteElementString("ram:GermanBankleitzahlID", account.Bankleitzahl);
                    }

                    Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }

                foreach (BankAccount account in this.Descriptor.DebitorBankAccounts)
                {
                    Writer.WriteStartElement("ram:SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram:TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteElementString("ram:Information", this.Descriptor.PaymentMeans.Information);

                        if (!String.IsNullOrEmpty(this.Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrEmpty(this.Descriptor.PaymentMeans.SEPAMandateReference))
                        {
                            Writer.WriteStartElement("ram:ID");
                            Writer.WriteAttributeString("schemeAgencyID", this.Descriptor.PaymentMeans.SEPACreditorIdentifier);
                            Writer.WriteValue(this.Descriptor.PaymentMeans.SEPAMandateReference);
                            Writer.WriteEndElement(); // !ram:ID
                        }
                    }

                    Writer.WriteStartElement("ram:PayerPartyDebtorFinancialAccount");
                    Writer.WriteElementString("ram:IBANID", account.IBAN);
                    if (!String.IsNullOrEmpty(account.ID))
                    {
                        Writer.WriteElementString("ram:ProprietaryID", account.ID);
                    }
                    Writer.WriteEndElement(); // !PayerPartyDebtorFinancialAccount

                    Writer.WriteStartElement("ram:PayerSpecifiedDebtorFinancialInstitution");
                    Writer.WriteElementString("ram:BICID", account.BIC);

                    if (!String.IsNullOrEmpty(account.Bankleitzahl))
                    {
                        Writer.WriteElementString("ram:GermanBankleitzahlID", account.Bankleitzahl);
                    }

                    if (!String.IsNullOrEmpty(account.BankName))
                    {
                        Writer.WriteElementString("ram:Name", account.BankName);
                    }
                    Writer.WriteEndElement(); // !PayerSpecifiedDebtorFinancialInstitution
                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }


            /*
             * @todo add writer for this:
             * <SpecifiedTradeSettlementPaymentMeans>
            * <TypeCode>42</TypeCode>
          * 	<Information>Überweisung</Information>
           * <PayeePartyCreditorFinancialAccount>
          * 		<IBANID>DE08700901001234567890</IBANID>
          * 		<ProprietaryID>1234567890</ProprietaryID>
          * 	</PayeePartyCreditorFinancialAccount>
          * 	<PayeeSpecifiedCreditorFinancialInstitution>
          * 		<BICID>GENODEF1M04</BICID>
          * 		<GermanBankleitzahlID>70090100</GermanBankleitzahlID>
          * 		<Name>Hausbank München</Name>
          * 	</PayeeSpecifiedCreditorFinancialInstitution>
          * </SpecifiedTradeSettlementPaymentMeans>
             */

            _writeOptionalTaxes(Writer);

            if ((this.Descriptor.TradeAllowanceCharges != null) && (this.Descriptor.TradeAllowanceCharges.Count > 0))
            {
                foreach (TradeAllowanceCharge tradeAllowanceCharge in this.Descriptor.TradeAllowanceCharges)
                {
                    Writer.WriteStartElement("ram:SpecifiedTradeAllowanceCharge");
                    Writer.WriteStartElement("ram:ChargeIndicator");
                    Writer.WriteElementString("udt:Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                    Writer.WriteEndElement(); // !ram:ChargeIndicator

                    Writer.WriteStartElement("ram:BasisAmount");
                    Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount));
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("ram:ActualAmount");
                    Writer.WriteAttributeString("currencyID", tradeAllowanceCharge.Currency.EnumToString());
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount));
                    Writer.WriteEndElement();


                    _writeOptionalElementString(Writer, "ram:Reason", tradeAllowanceCharge.Reason);

                    if (tradeAllowanceCharge.Tax != null)
                    {
                        Writer.WriteStartElement("ram:CategoryTradeTax");
                        Writer.WriteElementString("ram:TypeCode", tradeAllowanceCharge.Tax.TypeCode.EnumToString());
                        if (tradeAllowanceCharge.Tax.CategoryCode.HasValue)
                            Writer.WriteElementString("ram:CategoryCode", tradeAllowanceCharge.Tax.CategoryCode?.EnumToString());
                        Writer.WriteElementString("ram:RateApplicablePercent", _formatDecimal(tradeAllowanceCharge.Tax.Percent));
                        Writer.WriteEndElement();
                    }
                    Writer.WriteEndElement();
                }
            }

            if ((this.Descriptor.ServiceCharges != null) && (this.Descriptor.ServiceCharges.Count > 0))
            {
                foreach (ServiceCharge serviceCharge in this.Descriptor.ServiceCharges)
                {
                    Writer.WriteStartElement("ram:SpecifiedLogisticsServiceCharge");
                    if (!String.IsNullOrEmpty(serviceCharge.Description))
                    {
                        Writer.WriteElementString("ram:Description", serviceCharge.Description);
                    }
                    Writer.WriteElementString("ram:AppliedAmount", _formatDecimal(serviceCharge.Amount));
                    if (serviceCharge.Tax != null)
                    {
                        Writer.WriteStartElement("ram:AppliedTradeTax");
                        Writer.WriteElementString("ram:TypeCode", serviceCharge.Tax.TypeCode.EnumToString());
                        if (serviceCharge.Tax.CategoryCode.HasValue)
                            Writer.WriteElementString("ram:CategoryCode", serviceCharge.Tax.CategoryCode?.EnumToString());
                        Writer.WriteElementString("ram:RateApplicablePercent", _formatDecimal(serviceCharge.Tax.Percent));
                        Writer.WriteEndElement();
                    }
                    Writer.WriteEndElement();
                }
            }

            if (this.Descriptor.PaymentTerms != null)
            {
                Writer.WriteStartElement("ram:SpecifiedTradePaymentTerms");
                _writeOptionalElementString(Writer, "ram:Description", this.Descriptor.PaymentTerms.Description);
                if (this.Descriptor.PaymentTerms.DueDate.HasValue)
                {
                    Writer.WriteStartElement("ram:DueDateDateTime");
                    _writeElementWithAttribute(Writer, "udt:DateTimeString", "format", "102", _formatDate(this.Descriptor.PaymentTerms.DueDate.Value));
                    Writer.WriteEndElement(); // !ram:DueDateDateTime
                }
                Writer.WriteEndElement();
            }

            Writer.WriteStartElement("ram:SpecifiedTradeSettlementHeaderMonetarySummation");
            _writeOptionalAmount(Writer, "ram:LineTotalAmount", this.Descriptor.LineTotalAmount);
            _writeOptionalAmount(Writer, "ram:ChargeTotalAmount", this.Descriptor.ChargeTotalAmount);
            _writeOptionalAmount(Writer, "ram:AllowanceTotalAmount", this.Descriptor.AllowanceTotalAmount);
            _writeOptionalAmount(Writer, "ram:TaxBasisTotalAmount", this.Descriptor.TaxBasisAmount);
            _writeOptionalAmount(Writer, "ram:TaxTotalAmount", this.Descriptor.TaxTotalAmount);
            _writeOptionalAmount(Writer, "ram:GrandTotalAmount", this.Descriptor.GrandTotalAmount);

            if (this.Descriptor.TotalPrepaidAmount.HasValue)
            {
                _writeOptionalAmount(Writer, "ram:TotalPrepaidAmount", this.Descriptor.TotalPrepaidAmount.Value);
            }

            _writeOptionalAmount(Writer, "ram:DuePayableAmount", this.Descriptor.DuePayableAmount);
            Writer.WriteEndElement(); // !ram:SpecifiedTradeSettlementHeaderMonetarySummation

            #region InvoiceReferencedDocument
            if (this.Descriptor.InvoiceReferencedDocument != null)
            {
                Writer.WriteStartElement("ram:InvoiceReferencedDocument");
                _writeOptionalElementString(Writer, "ram:IssuerAssignedID", this.Descriptor.InvoiceReferencedDocument.ID);
                if (this.Descriptor.InvoiceReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram:FormattedIssueDateTime");
                    _writeElementWithAttribute(Writer, "qdt:DateTimeString", "format", "102", _formatDate(this.Descriptor.InvoiceReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }
                Writer.WriteEndElement(); // !ram:InvoiceReferencedDocument
            }
            #endregion


            Writer.WriteEndElement(); // !ram:ApplicableHeaderTradeSettlement

            Writer.WriteEndElement(); // !ram:SupplyChainTradeTransaction
            #endregion

            Writer.WriteEndElement(); // !ram:Invoice
            Writer.WriteEndDocument();
            Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);
        } // !Save()


        private void _writeOptionalAmount(ProfileAwareXmlTextWriter writer, string tagName, decimal? value, int numDecimals = 2)
        {
            if (value.HasValue && (value.Value != decimal.MinValue))
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

                if (!String.IsNullOrEmpty(tax.ExemptionReason))
                {
                    writer.WriteElementString("ram:ExemptionReason", tax.ExemptionReason);
                }

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

                if (tax.ExemptionReasonCode.HasValue)
                {
                    writer.WriteElementString("ram:ExemptionReasonCode", tax.ExemptionReasonCode?.EnumToString());
                }

                writer.WriteElementString("ram:RateApplicablePercent", _formatDecimal(tax.Percent));
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

                if (!String.IsNullOrEmpty(Party.ID))
                {
                    writer.WriteElementString("ram:ID", Party.ID);
                }

                if ((Party.GlobalID != null) && !String.IsNullOrEmpty(Party.GlobalID.ID) && !String.IsNullOrEmpty(Party.GlobalID.SchemeID))
                {
                    writer.WriteStartElement("ram:GlobalID");
                    writer.WriteAttributeString("schemeID", Party.GlobalID.SchemeID);
                    writer.WriteValue(Party.GlobalID.ID);
                    writer.WriteEndElement();
                }

                if (!String.IsNullOrEmpty(Party.Name))
                {
                    writer.WriteElementString("ram:Name", Party.Name);
                }

                if (Contact != null)
                {
                    _writeOptionalContact(writer, "ram:DefinedTradeContact", Contact);
                }

                writer.WriteStartElement("ram:PostalTradeAddress");
                writer.WriteElementString("ram:PostcodeCode", Party.Postcode);
                writer.WriteElementString("ram:LineOne", string.IsNullOrEmpty(Party.ContactName) ? Party.Street : Party.ContactName);
                if (!string.IsNullOrEmpty(Party.ContactName))
                    writer.WriteElementString("ram:LineTwo", Party.Street);
                writer.WriteElementString("ram:CityName", Party.City);
                writer.WriteElementString("ram:CountryID", Party.Country.EnumToString());
                writer.WriteEndElement(); // !PostalTradeAddress

                if (TaxRegistrations != null)
                {
                    foreach (TaxRegistration _reg in TaxRegistrations)
                    {
                        if (!String.IsNullOrEmpty(_reg.No))
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

                if (!String.IsNullOrEmpty(contact.Name))
                {
                    writer.WriteElementString("ram:PersonName", contact.Name);
                }

                if (!String.IsNullOrEmpty(contact.OrgUnit))
                {
                    writer.WriteElementString("ram:DepartmentName", contact.OrgUnit);
                }

                if (!String.IsNullOrEmpty(contact.PhoneNo))
                {
                    writer.WriteStartElement("ram:TelephoneUniversalCommunication");
                    writer.WriteElementString("ram:CompleteNumber", contact.PhoneNo);
                    writer.WriteEndElement();
                }

                if (!String.IsNullOrEmpty(contact.FaxNo))
                {
                    writer.WriteStartElement("ram:FaxUniversalCommunication");
                    writer.WriteElementString("ram:CompleteNumber", contact.FaxNo);
                    writer.WriteEndElement();
                }

                if (!String.IsNullOrEmpty(contact.EmailAddress))
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
                case InvoiceType.SelfBilledInvoice:
                case InvoiceType.Invoice: return "RECHNUNG";
                case InvoiceType.SelfBilledCreditNote:
                case InvoiceType.CreditNote: return "GUTSCHRIFT";
                case InvoiceType.DebitNote: return "BELASTUNGSANZEIGE";
                case InvoiceType.DebitnoteRelatedToFinancialAdjustments: return "WERTBELASTUNG";
                case InvoiceType.PartialInvoice: return "TEILRECHNUNG";
                case InvoiceType.PrepaymentInvoice: return "VORAUSZAHLUNGSRECHNUNG";
                case InvoiceType.InvoiceInformation: return "KEINERECHNUNG";
                case InvoiceType.Correction:
                case InvoiceType.CorrectionOld: return "KORREKTURRECHNUNG";
                case InvoiceType.Unknown: return "";
                default: return "";
            }
        } // !_translateInvoiceType()


        private int _encodeInvoiceType(InvoiceType type)
        {
            if ((int)type > 1000)
            {
                type -= 1000;
            }

            if (type == InvoiceType.CorrectionOld)
            {
                return (int)InvoiceType.Correction;
            }

            return (int)type;
        } // !_translateInvoiceType()


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

            return true;
        } // !Validate()
    }
}
