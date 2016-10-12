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
    internal class InvoiceDescriptorWriter
    {
        private XmlTextWriter Writer;
        private InvoiceDescriptor Descriptor;


        public void Save(InvoiceDescriptor descriptor, Stream stream)
        {
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new IllegalStreamException("Cannot write to stream");
            }

            long streamPosition = stream.Position;

            this.Descriptor = descriptor;
            this.Writer = new XmlTextWriter(stream, Encoding.UTF8);
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
            Writer.WriteStartElement("ram:GuidelineSpecifiedDocumentContextParameter");
            Writer.WriteElementString("ram:ID", this.Descriptor.Profile.EnumToString());
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
            _writeOptionalNotes(Writer);
            Writer.WriteEndElement(); // !rsm:HeaderExchangedDocument
            #endregion

            #region SpecifiedSupplyChainTradeTransaction
            Writer.WriteStartElement("rsm:SpecifiedSupplyChainTradeTransaction");
            Writer.WriteStartElement("ram:ApplicableSupplyChainTradeAgreement");
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
                    Writer.WriteStartElement("ram:IssueDateTime");
                    Writer.WriteStartElement("udt:DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.OrderDate.Value));
                    Writer.WriteEndElement(); // !udt:DateTimeString
                    Writer.WriteEndElement(); // !IssueDateTime()
                }
                Writer.WriteElementString("ID", this.Descriptor.OrderNo);
                Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }

            Writer.WriteEndElement(); // !ApplicableSupplyChainTradeAgreement

            Writer.WriteStartElement("ram:ApplicableSupplyChainTradeDelivery"); // Pflichteintrag
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
            if (!String.IsNullOrEmpty(this.Descriptor.InvoiceNoAsReference))
            {
                _writeOptionalElementString(Writer, "ram:PaymentReference", this.Descriptor.InvoiceNoAsReference);
            }
            Writer.WriteElementString("ram:InvoiceCurrencyCode", this.Descriptor.Currency.EnumToString());

            if ((this.Descriptor.CreditorBankAccounts.Count > 0) || (this.Descriptor.PaymentMeans != null))
            {
                Writer.WriteStartElement("ram:SpecifiedTradeSettlementPaymentMeans");

                if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                {
                    Writer.WriteElementString("ram:TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                    Writer.WriteElementString("ram:Information", this.Descriptor.PaymentMeans.Information);
                }

                foreach (BankAccount account in this.Descriptor.CreditorBankAccounts)
                {
                    Writer.WriteStartElement("ram:PayeePartyCreditorFinancialAccount");
                    Writer.WriteElementString("ram:IBANID", account.IBAN);
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

                    if (!String.IsNullOrEmpty(account.BankName))
                    {
                        Writer.WriteElementString("ram:Name", account.BankName);
                    }
                    Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                }

                Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
            }


            /**
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
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 2));
                    Writer.WriteEndElement();


                    _writeOptionalElementString(Writer, "ram:Reason", tradeAllowanceCharge.Reason);

                    if (tradeAllowanceCharge.Tax != null)
                    {
                        Writer.WriteStartElement("ram:CategoryTradeTax");
                        Writer.WriteElementString("ram:TypeCode", tradeAllowanceCharge.Tax.TypeCode.EnumToString());
                        Writer.WriteElementString("ram:CategoryCode", tradeAllowanceCharge.Tax.CategoryCode.EnumToString());
                        Writer.WriteElementString("ram:ApplicablePercent", _formatDecimal(tradeAllowanceCharge.Tax.Percent));
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
                        Writer.WriteElementString("ram:CategoryCode", serviceCharge.Tax.CategoryCode.EnumToString());
                        Writer.WriteElementString("ram:ApplicablePercent", _formatDecimal(serviceCharge.Tax.Percent));
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

            int counter = 0;
            foreach(TradeLineItem tradeLineItem in this.Descriptor.TradeLineItems)
            {
                Writer.WriteStartElement("ram:IncludedSupplyChainTradeLineItem");
                Writer.WriteStartElement("ram:AssociatedDocumentLineDocument");

                if ((tradeLineItem.BilledQuantity != 0) && (tradeLineItem.Name == null) && (!String.IsNullOrEmpty(tradeLineItem.Comment)))
                {
                    Writer.WriteStartElement("ram:IncludedNote");
                    Writer.WriteElementString("ram:Content", tradeLineItem.Comment);
                    Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument
                }
                else
                {
                    counter += 1;
                    string _lineID = String.Format("{0}", counter);
                    Writer.WriteElementString("ram:LineID", _lineID);
                    if (!String.IsNullOrEmpty(tradeLineItem.Comment))
                    {
                        Writer.WriteElementString("Content", tradeLineItem.Comment);
                    }

                    Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument

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
                        if (!String.IsNullOrEmpty(tradeLineItem.BuyerOrderReferencedDocument.ID))
                        {
                            Writer.WriteElementString("ram:ID", tradeLineItem.BuyerOrderReferencedDocument.ID);
                        }

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
                        if (!String.IsNullOrEmpty(tradeLineItem.ContractReferencedDocument.ID))
                        {
                            Writer.WriteElementString("ram:ID", tradeLineItem.ContractReferencedDocument.ID);
                        }

                        Writer.WriteEndElement(); // !ram:ContractReferencedDocument
                    }

                    if ((tradeLineItem.AdditionalReferencedDocuments != null) && (tradeLineItem.AdditionalReferencedDocuments.Count > 0))
                    {
                        foreach (AdditionalReferencedDocument doc in tradeLineItem.AdditionalReferencedDocuments)
                        {
                            Writer.WriteStartElement("ram:AdditionalReferencedDocument");
                            if (doc.IssueDateTime.HasValue)
                            {
                                Writer.WriteStartElement("ram:IssueDateTime");
                                Writer.WriteValue(_formatDate(doc.IssueDateTime.Value, false));
                                Writer.WriteEndElement(); // !ram:IssueDateTime
                            }

                            Writer.WriteElementString("ram:LineID", _lineID);

                            if (!String.IsNullOrEmpty(doc.ID))
                            {
                                Writer.WriteElementString("ram:ID", doc.ID);
                            }

                            Writer.WriteElementString("ram:ReferenceTypeCode", doc.ReferenceTypeCode.EnumToString());

                            Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                        }
                    }

                    Writer.WriteStartElement("ram:GrossPriceProductTradePrice");
                    _writeOptionalAmount(Writer, "ram:ChargeAmount", tradeLineItem.GrossUnitPrice, 4);
                    if (tradeLineItem.UnitQuantity.HasValue)
                    {
                        _writeElementWithAttribute(Writer, "ram:BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 4));
                    }

                    foreach(TradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.TradeAllowanceCharges)
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

                    Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeAgreement

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
                        if (!String.IsNullOrEmpty(tradeLineItem.DeliveryNoteReferencedDocument.ID))
                        {
                            Writer.WriteElementString("ram:ID", tradeLineItem.DeliveryNoteReferencedDocument.ID);
                        }

                        Writer.WriteEndElement(); // !ram:DeliveryNoteReferencedDocument
                    }

                    Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeDelivery

                    Writer.WriteStartElement("ram:SpecifiedSupplyChainTradeSettlement");
                    Writer.WriteStartElement("ram:ApplicableTradeTax");
                    Writer.WriteElementString("ram:TypeCode", tradeLineItem.TaxType.EnumToString());
                    Writer.WriteElementString("ram:CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString());
                    Writer.WriteElementString("ram:ApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent));
                    Writer.WriteEndElement(); // !ram:ApplicableTradeTax
                    Writer.WriteStartElement("ram:SpecifiedTradeSettlementMonetarySummation");

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
                    Writer.WriteEndElement(); // ram:SpecifiedTradeSettlementMonetarySummation
                    Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeSettlement

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
                }
                Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
            } // !foreach(tradeLineItem)

            Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeTransaction
            #endregion

            Writer.WriteEndElement(); // !ram:Invoice
            Writer.WriteEndDocument();
            Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);
        } // !Save()


        public void Save(InvoiceDescriptor descriptor, string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            Save(descriptor, fs);
            fs.Flush();
            fs.Close();
        } // !Save()


        private void _writeOptionalAmount(XmlTextWriter writer, string tagName, decimal value, int numDecimals = 2)
        {
            if (value != decimal.MinValue)
            {
                writer.WriteStartElement(tagName);
                writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                writer.WriteValue(_formatDecimal(value, numDecimals));
                writer.WriteEndElement(); // !tagName
            }
        } // !_writeOptionalAmount()


        private void _writeElementWithAttribute(XmlTextWriter writer, string tagName, string attributeName, string attributeValue, string nodeValue)
        {
            writer.WriteStartElement(tagName);
            writer.WriteAttributeString(attributeName, attributeValue);
            writer.WriteValue(nodeValue);
            writer.WriteEndElement(); // !tagName
        } // !_writeElementWithAttribute()


        private void _writeOptionalTaxes(XmlTextWriter writer)
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

                writer.WriteElementString("ram:CategoryCode", tax.CategoryCode.EnumToString());
                writer.WriteElementString("ram:ApplicablePercent", _formatDecimal(tax.Percent));
                writer.WriteEndElement(); // !ApplicableTradeTax
            }
        } // !_writeOptionalTaxes()


        private void _writeOptionalNotes(XmlTextWriter writer)
        {
            if (this.Descriptor.Notes.Count > 0)
            {
                foreach (Tuple<string, SubjectCodes, ContentCodes> t in this.Descriptor.Notes)
                {
                    writer.WriteStartElement("ram:IncludedNote");
                    if (t.Item3 != ContentCodes.Unknown)
                    {
                        writer.WriteElementString("ram:ContentCode", t.Item3.EnumToString());
                    }
                    writer.WriteElementString("ram:Content", t.Item1);
                    if (t.Item2 != SubjectCodes.Unknown)
                    {
                        writer.WriteElementString("ram:SubjectCode", t.Item2.EnumToString());
                    }
                    writer.WriteEndElement();
                }
            }
        } // !_writeOptionalNotes()


        private void _writeOptionalParty(XmlTextWriter writer, string PartyTag, Party Party, Contact Contact = null, List<TaxRegistration> TaxRegistrations = null)
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
                writer.WriteElementString("ram:LineOne", Party.Street);
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


        private void _writeOptionalContact(XmlTextWriter writer, string contactTag, Contact contact)
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


        private void _writeOptionalElementString(XmlTextWriter writer, string tagName, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WriteElementString(tagName, value);
            }
        } // !_writeOptionalElementString()


        private string _formatDecimal(decimal value, int numDecimals = 2)
        {
            string formatString = "0.";
            for(int i = 0; i < numDecimals; i++)
            {
                formatString += "0";
            }

            return value.ToString(formatString).Replace(",", ".");
        } // !_formatDecimal()


        private string _translateInvoiceType(InvoiceType type)
        {
            switch (type)
            {
                case InvoiceType.Invoice: return "RECHNUNG";
                case InvoiceType.Correction: return "KORREKTURRECHNUNG";
                case InvoiceType.CreditNote: return "GUTSCHRIFT";
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

            return (int)type;
        } // !_translateInvoiceType()
        

        private string _formatDate(DateTime date, bool formatAs102 = true)
        {
            if (formatAs102)
            {
                return date.ToString("yyyyMMdd");
            }
            else
            {
                return date.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        } // !_formatDate()
    }
}
