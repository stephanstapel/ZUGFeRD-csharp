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

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor20Writer : IInvoiceDescriptorWriter
    {
        private ProfileAwareXmlTextWriter Writer;
        private InvoiceDescriptor Descriptor;


        private readonly Profile ALL_PROFILES = Profile.Minimum | Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung;


        /// <summary>
        /// Saves the given invoice to the given stream.
        /// Make sure that the stream is open and writeable. Otherwise, an IllegalStreamException will be thron.
        /// </summary>
        /// <param name="descriptor">The invoice object that should be saved</param>
        /// <param name="stream">The target stream for saving the invoice</param>
        /// <param name="format">Format of the target file</param>
        public override void Save(InvoiceDescriptor descriptor, Stream stream, ZUGFeRDFormats format = ZUGFeRDFormats.CII)
        {
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new IllegalStreamException("Cannot write to stream");
            }

            if (format == ZUGFeRDFormats.UBL)
            {
                throw new UnsupportedException("UBL format is not supported for ZUGFeRD 2.0");
            }

            // write data
            long streamPosition = stream.Position;

            this.Descriptor = descriptor;
            this.Writer = new ProfileAwareXmlTextWriter(stream, descriptor.Profile);
            this.Writer.SetNamespaces(new Dictionary<string, string>()
            {
                { "a", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100" },
                { "rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100" },
                { "qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100" },
                { "ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100" },
                { "xs", "http://www.w3.org/2001/XMLSchema" },
                { "udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100" }
            });

            Writer.WriteStartDocument();

            #region Kopfbereich
            Writer.WriteStartElement("rsm", "CrossIndustryInvoice");
            Writer.WriteAttributeString("xmlns", "a", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            Writer.WriteAttributeString("xmlns", "rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            Writer.WriteAttributeString("xmlns", "qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            Writer.WriteAttributeString("xmlns", "ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            Writer.WriteAttributeString("xmlns", "xs", "http://www.w3.org/2001/XMLSchema");
            Writer.WriteAttributeString("xmlns", "udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");
            #endregion

            #region SpecifiedExchangedDocumentContext
            Writer.WriteStartElement("rsm", "ExchangedDocumentContext");

            if (Descriptor.IsTest)
            {
                Writer.WriteStartElement("ram", "TestIndicator");
                Writer.WriteElementString("udt", "Indicator", "true");
                Writer.WriteEndElement(); // !ram:TestIndicator
            }

            if (!String.IsNullOrWhiteSpace(this.Descriptor.BusinessProcess))
            {
                Writer.WriteStartElement("ram", "BusinessProcessSpecifiedDocumentContextParameter");
                Writer.WriteElementString("ram", "ID", this.Descriptor.BusinessProcess);
                Writer.WriteEndElement(); // !ram:BusinessProcessSpecifiedDocumentContextParameter
            }

            Writer.WriteStartElement("ram", "GuidelineSpecifiedDocumentContextParameter");
            Writer.WriteElementString("ram", "ID", this.Descriptor.Profile.EnumToString(ZUGFeRDVersion.Version20));
            Writer.WriteEndElement(); // !ram:GuidelineSpecifiedDocumentContextParameter
            Writer.WriteEndElement(); // !rsm:ExchangedDocumentContext

            Writer.WriteStartElement("rsm", "ExchangedDocument");
            Writer.WriteElementString("ram", "ID", this.Descriptor.InvoiceNo);
            Writer.WriteOptionalElementString("ram", "Name", this.Descriptor.Name, Profile.Extended);
            Writer.WriteElementString("ram", "TypeCode", String.Format("{0}", _encodeInvoiceType(this.Descriptor.Type)));

            if (this.Descriptor.InvoiceDate.HasValue)
            {
                Writer.WriteStartElement("ram", "IssueDateTime");
                Writer.WriteStartElement("udt", "DateTimeString");
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

            #region SupplyChainTradeTransaction
            Writer.WriteStartElement("rsm", "SupplyChainTradeTransaction");

            foreach (TradeLineItem tradeLineItem in this.Descriptor.GetTradeLineItems())
            {
                Writer.WriteStartElement("ram", "IncludedSupplyChainTradeLineItem");

                if (tradeLineItem.AssociatedDocument != null)
                {
                    Writer.WriteStartElement("ram", "AssociatedDocumentLineDocument");
                    Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.AssociatedDocument.LineID);
                    if (tradeLineItem.AssociatedDocument.LineStatusCode.HasValue)
                    {
                        Writer.WriteElementString("ram", "LineStatusCode", tradeLineItem.AssociatedDocument.LineStatusCode.Value.EnumValueToString());
                    }
                    if (tradeLineItem.AssociatedDocument.LineStatusReasonCode.HasValue)
                    {
                        Writer.WriteElementString("ram", "LineStatusReasonCode", tradeLineItem.AssociatedDocument.LineStatusReasonCode.Value.EnumToString());
                    }
                    _writeNotes(Writer, tradeLineItem.AssociatedDocument.Notes);
                    Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument
                }

                // handelt es sich um einen Kommentar?
                if ((tradeLineItem.AssociatedDocument?.Notes.Count > 0) && (tradeLineItem.BilledQuantity == 0) && (String.IsNullOrWhiteSpace(tradeLineItem.Description)))
                {
                    Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
                    continue;
                }

                Writer.WriteStartElement("ram", "SpecifiedTradeProduct");
                if ((tradeLineItem.GlobalID != null) && (tradeLineItem.GlobalID.SchemeID.HasValue) && (tradeLineItem.GlobalID.SchemeID.Value != GlobalIDSchemeIdentifiers.Unknown) && !String.IsNullOrWhiteSpace(tradeLineItem.GlobalID.ID))
                {
                    _writeElementWithAttribute(Writer, "ram", "GlobalID", "schemeID", tradeLineItem.GlobalID.SchemeID.Value.EnumToString(), tradeLineItem.GlobalID.ID);
                }

                Writer.WriteOptionalElementString("ram", "SellerAssignedID", tradeLineItem.SellerAssignedID);
                Writer.WriteOptionalElementString("ram", "BuyerAssignedID", tradeLineItem.BuyerAssignedID);
                Writer.WriteOptionalElementString("ram", "Name", tradeLineItem.Name);
                Writer.WriteOptionalElementString("ram", "Description", tradeLineItem.Description);

                if (tradeLineItem.ApplicableProductCharacteristics != null && tradeLineItem.ApplicableProductCharacteristics.Any())
                {
                    foreach (var productCharacteristic in tradeLineItem.ApplicableProductCharacteristics)
                    {
                        Writer.WriteStartElement("ram", "ApplicableProductCharacteristic");
                        Writer.WriteOptionalElementString("ram", "Description", productCharacteristic.Description);
                        Writer.WriteOptionalElementString("ram", "Value", productCharacteristic.Value);
                        Writer.WriteEndElement(); // !ram:ApplicableProductCharacteristic
                    }
                }

                if (tradeLineItem.IncludedReferencedProducts != null && tradeLineItem.IncludedReferencedProducts.Any())
                {
                    foreach (var includedItem in tradeLineItem.IncludedReferencedProducts)
                    {
                        Writer.WriteStartElement("ram", "IncludedReferencedProduct");
                        Writer.WriteOptionalElementString("ram", "Name", includedItem.Name);

                        if (includedItem.UnitQuantity.HasValue)
                        {
                            _writeElementWithAttribute(Writer, "ram", "UnitQuantity", "unitCode", includedItem.UnitCode.Value.EnumToString(), _formatDecimal(includedItem.UnitQuantity, 4));
                        }
                        Writer.WriteEndElement(); // !ram:IncludedReferencedProduct
                    }
                }

                Writer.WriteEndElement(); // !ram:SpecifiedTradeProduct

                Writer.WriteStartElement("ram", "SpecifiedLineTradeAgreement", Profile.Basic | Profile.Comfort | Profile.Extended);

                if (tradeLineItem.BuyerOrderReferencedDocument != null)
                {
                    Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument", Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    // order number
                    Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.BuyerOrderReferencedDocument.ID);

                    // reference to the order position
                    Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.BuyerOrderReferencedDocument.LineID);

                    if (tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.HasValue)
                    {
                        Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        Writer.WriteStartElement("qdt", "DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.Value));
                        Writer.WriteEndElement(); // !qdt:DateTimeString
                        Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                    }

                    Writer.WriteEndElement(); // !ram:BuyerOrderReferencedDocument
                }

                if (tradeLineItem.ContractReferencedDocument != null)
                {
                    Writer.WriteStartElement("ram", "ContractReferencedDocument", Profile.Extended);

                    // reference to the contract position
                    Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.ContractReferencedDocument.LineID);

                    if (tradeLineItem.ContractReferencedDocument.IssueDateTime.HasValue)
                    {
                        Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        Writer.WriteStartElement("qdt", "DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(tradeLineItem.ContractReferencedDocument.IssueDateTime.Value));
                        Writer.WriteEndElement(); // !udt:DateTimeString
                        Writer.WriteEndElement(); // !ram:IssueDateTime
                    }
                    Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.ContractReferencedDocument.ID);

                    Writer.WriteEndElement(); // !ram:ContractReferencedDocument(Extended)
                }

                foreach (AdditionalReferencedDocument document in tradeLineItem._AdditionalReferencedDocuments)
                {
                    Writer.WriteStartElement("ram", "AdditionalReferencedDocument");
                    if (document.IssueDateTime.HasValue)
                    {
                        Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        Writer.WriteStartElement("qdt", "DateTimeString");
                        Writer.WriteValue(_formatDate(document.IssueDateTime.Value));
                        Writer.WriteEndElement(); // !qdt:DateTimeString
                        Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                    }

                    Writer.WriteElementString("ram", "LineID", String.Format("{0}", tradeLineItem.AssociatedDocument?.LineID));
                    Writer.WriteOptionalElementString("ram", "IssuerAssignedID", document.ID);

                    if (document.TypeCode != AdditionalReferencedDocumentTypeCode.Unknown)
                    {
                        Writer.WriteElementString("ram", "TypeCode", document.TypeCode.EnumValueToString());
                    }

                    if (document.ReferenceTypeCode != ReferenceTypeCodes.Unknown)
                    {
                        Writer.WriteElementString("ram", "ReferenceTypeCode", document.ReferenceTypeCode.EnumToString());
                    }

                    Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                } // !foreach(document)

                Writer.WriteStartElement("ram", "GrossPriceProductTradePrice");
                _writeOptionalAdaptiveAmount(Writer, "ram", "ChargeAmount", tradeLineItem.GrossUnitPrice, 2, 4);
                if (tradeLineItem.UnitQuantity.HasValue)
                {
                    _writeElementWithAttribute(Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 4));
                }

                foreach (TradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.GetTradeAllowanceCharges())
                {
                    Writer.WriteStartElement("ram", "AppliedTradeAllowanceCharge");

                    Writer.WriteStartElement("ram", "ChargeIndicator");
                    Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                    Writer.WriteEndElement(); // !ram:ChargeIndicator

                    if (tradeAllowanceCharge.BasisAmount.HasValue)
                    {
                        Writer.WriteStartElement("ram", "BasisAmount");
                        Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value, 4));
                        Writer.WriteEndElement();
                    }
                    Writer.WriteStartElement("ram", "ActualAmount");
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 4));
                    Writer.WriteEndElement();

                    Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason, Profile.Comfort | Profile.Extended);
                    // "ReasonCode" nicht im 2.0 Standard!

                    Writer.WriteEndElement(); // !AppliedTradeAllowanceCharge
                }

                Writer.WriteEndElement(); // ram:GrossPriceProductTradePrice

                Writer.WriteStartElement("ram", "NetPriceProductTradePrice");
                _writeOptionalAdaptiveAmount(Writer, "ram", "ChargeAmount", tradeLineItem.NetUnitPrice, 2, 4);

                if (tradeLineItem.UnitQuantity.HasValue)
                {
                    _writeElementWithAttribute(Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 4));
                }
                Writer.WriteEndElement(); // ram:NetPriceProductTradePrice

                Writer.WriteEndElement(); // !ram:SpecifiedLineTradeAgreement

                if (Descriptor.Profile != Profile.Basic)
                {
                    Writer.WriteStartElement("ram", "SpecifiedLineTradeDelivery");
                    _writeElementWithAttribute(Writer, "ram", "BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));

                    if (tradeLineItem.DeliveryNoteReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument");
                        if (tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                        {
                            //Old path of Version 1.0
                            //Writer.WriteStartElement("ram", "IssueDateTime");
                            //Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                            //Writer.WriteEndElement(); // !ram:IssueDateTime

                            Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                            Writer.WriteStartElement("qdt", "DateTimeString");
                            Writer.WriteAttributeString("format", "102");
                            Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                            Writer.WriteEndElement(); // !qdt:DateTimeString
                            Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                        }

                        Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.DeliveryNoteReferencedDocument.ID);
                        Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.DeliveryNoteReferencedDocument.LineID);
                        Writer.WriteEndElement(); // !ram:DeliveryNoteReferencedDocument
                    }

                    if (tradeLineItem.ActualDeliveryDate.HasValue)
                    {
                        Writer.WriteStartElement("ram", "ActualDeliverySupplyChainEvent");
                        Writer.WriteStartElement("ram", "OccurrenceDateTime");
                        Writer.WriteStartElement("udt", "DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(tradeLineItem.ActualDeliveryDate.Value));
                        Writer.WriteEndElement(); // !udt:DateTimeString
                        Writer.WriteEndElement(); // !OccurrenceDateTime()
                        Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
                    }

                    Writer.WriteEndElement(); // !ram:SpecifiedLineTradeDelivery
                }
                else
                {
                    Writer.WriteStartElement("ram", "SpecifiedLineTradeDelivery");
                    _writeElementWithAttribute(Writer, "ram", "BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));
                    Writer.WriteEndElement(); // !ram:SpecifiedLineTradeDelivery
                }

                Writer.WriteStartElement("ram", "SpecifiedLineTradeSettlement");

                Writer.WriteStartElement("ram", "ApplicableTradeTax", Profile.Basic | Profile.Comfort | Profile.Extended);
                Writer.WriteElementString("ram", "TypeCode", tradeLineItem.TaxType.EnumToString());
                Writer.WriteElementString("ram", "CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString());
                Writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent));
                Writer.WriteEndElement(); // !ram:ApplicableTradeTax

                if (tradeLineItem.BillingPeriodStart.HasValue || tradeLineItem.BillingPeriodEnd.HasValue)
                {
                    Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    if (tradeLineItem.BillingPeriodStart.HasValue)
                    {
                        Writer.WriteStartElement("ram", "StartDateTime");
                        _writeElementWithAttribute(Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodStart.Value));
                        Writer.WriteEndElement(); // !StartDateTime
                    }

                    if (tradeLineItem.BillingPeriodEnd.HasValue)
                    {
                        Writer.WriteStartElement("ram", "EndDateTime");
                        _writeElementWithAttribute(Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodEnd.Value));
                        Writer.WriteEndElement(); // !EndDateTime
                    }
                    Writer.WriteEndElement(); // !BillingSpecifiedPeriod
                }

                Writer.WriteStartElement("ram", "SpecifiedTradeSettlementLineMonetarySummation");

                decimal total = 0m;

                if (tradeLineItem.LineTotalAmount.HasValue)
                {
                    total = tradeLineItem.LineTotalAmount.Value;
                }
                else if (tradeLineItem.NetUnitPrice.HasValue)
                {
                    total = tradeLineItem.NetUnitPrice.Value * tradeLineItem.BilledQuantity;
                    if (tradeLineItem.UnitQuantity.HasValue && (tradeLineItem.UnitQuantity.Value != 0))
                    {
                        total /= tradeLineItem.UnitQuantity.Value;
                    }
                }

                Writer.WriteElementString("ram", "LineTotalAmount", _formatDecimal(total));

                Writer.WriteEndElement(); // ram:SpecifiedTradeSettlementLineMonetarySummation
                Writer.WriteEndElement(); // !ram:SpecifiedLineTradeSettlement

                Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
            } // !foreach(tradeLineItem)

            Writer.WriteStartElement("ram", "ApplicableHeaderTradeAgreement");
            Writer.WriteOptionalElementString("ram", "BuyerReference", this.Descriptor.ReferenceOrderNo);
            _writeOptionalParty(Writer, "ram", "SellerTradeParty", this.Descriptor.Seller, this.Descriptor.SellerContact, TaxRegistrations: this.Descriptor.SellerTaxRegistration);
            _writeOptionalParty(Writer, "ram", "BuyerTradeParty", this.Descriptor.Buyer, this.Descriptor.BuyerContact, TaxRegistrations: this.Descriptor.BuyerTaxRegistration);

            #region ApplicableTradeDeliveryTerms
            if (Descriptor.ApplicableTradeDeliveryTermsCode.HasValue)
            {
                // BG-X-22, BT-X-145
                Writer.WriteStartElement("ram", "ApplicableTradeDeliveryTerms", Profile.Extended);
                Writer.WriteElementString("ram", "DeliveryTypeCode", this.Descriptor.ApplicableTradeDeliveryTermsCode.Value.GetDescriptionAttribute());
                Writer.WriteEndElement(); // !ApplicableTradeDeliveryTerms
            }
            #endregion

            #region SellerOrderReferencedDocument (BT-14: Comfort, Extended)
            if (null != this.Descriptor.SellerOrderReferencedDocument && !string.IsNullOrWhiteSpace(Descriptor.SellerOrderReferencedDocument.ID))
            {
                Writer.WriteStartElement("ram", "SellerOrderReferencedDocument", Profile.Comfort | Profile.Extended);
                Writer.WriteElementString("ram", "IssuerAssignedID", this.Descriptor.SellerOrderReferencedDocument.ID);
                if (this.Descriptor.SellerOrderReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime", Profile.Extended);
                    Writer.WriteStartElement("qdt", "DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.SellerOrderReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // !qdt:DateTimeString
                    Writer.WriteEndElement(); // !IssueDateTime()
                }

                Writer.WriteEndElement(); // !SellerOrderReferencedDocument
            }
            #endregion


            if (!String.IsNullOrWhiteSpace(this.Descriptor.OrderNo))
            {
                Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument");
                Writer.WriteElementString("ram", "IssuerAssignedID", this.Descriptor.OrderNo);
                if (this.Descriptor.OrderDate.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    Writer.WriteStartElement("qdt", "DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.OrderDate.Value));
                    Writer.WriteEndElement(); // !qdt:DateTimeString
                    Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }
                Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }


            if (this.Descriptor.AdditionalReferencedDocuments != null)
            {
                foreach (AdditionalReferencedDocument document in this.Descriptor.AdditionalReferencedDocuments)
                {
                    Writer.WriteStartElement("ram", "AdditionalReferencedDocument");
                    if (document.IssueDateTime.HasValue)
                    {
                        Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        Writer.WriteStartElement("qdt", "DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(document.IssueDateTime.Value));
                        Writer.WriteEndElement(); // !udt:DateTimeString
                        Writer.WriteEndElement(); // !FormattedIssueDateTime
                    }

                    if (document.TypeCode != AdditionalReferencedDocumentTypeCode.Unknown)
                    {
                        Writer.WriteElementString("ram", "TypeCode", document.TypeCode.EnumToString());
                    }

                    if (document.ReferenceTypeCode != ReferenceTypeCodes.Unknown)
                    {
                        Writer.WriteElementString("ram", "ReferenceTypeCode", document.ReferenceTypeCode.EnumToString());
                    }

                    Writer.WriteElementString("ram", "ID", document.ID);
                    Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                } // !foreach(document)
            }

            Writer.WriteEndElement(); // !ApplicableHeaderTradeAgreement

            Writer.WriteStartElement("ram", "ApplicableHeaderTradeDelivery"); // Pflichteintrag

            if (Descriptor.Profile == Profile.Extended)
            {
                _writeOptionalParty(Writer, "ram", "ShipToTradeParty", this.Descriptor.ShipTo);
                _writeOptionalParty(Writer, "ram", "ShipFromTradeParty", this.Descriptor.ShipFrom);
            }

            if (this.Descriptor.ActualDeliveryDate.HasValue)
            {
                Writer.WriteStartElement("ram", "ActualDeliverySupplyChainEvent");
                Writer.WriteStartElement("ram", "OccurrenceDateTime");
                Writer.WriteStartElement("udt", "DateTimeString");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor.ActualDeliveryDate.Value));
                Writer.WriteEndElement(); // !udt:DateTimeString
                Writer.WriteEndElement(); // !OccurrenceDateTime()
                Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
            }

            if (this.Descriptor.DeliveryNoteReferencedDocument != null)
            {
                Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument");

                if (this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    Writer.WriteValue(_formatDate(this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.Value, false));
                    Writer.WriteEndElement(); // !IssueDateTime
                }

                Writer.WriteElementString("ram", "IssuerAssignedID", this.Descriptor.DeliveryNoteReferencedDocument.ID);
                Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }

            Writer.WriteEndElement(); // !ApplicableHeaderTradeDelivery

            Writer.WriteStartElement("ram", "ApplicableHeaderTradeSettlement");
            // order of sub-elements of ApplicableHeaderTradeSettlement:
            //   1. CreditorReferenceID (optional)
            //   2. PaymentReference (optional)
            //   3. TaxCurrencyCode (optional)
            //   4. InvoiceCurrencyCode (optional)
            //   5. InvoiceIssuerReference (optional)
            //   6. InvoicerTradeParty (optional)
            //   7. InvoiceeTradeParty (optional)
            //   8. PayeeTradeParty (optional)
            //   9. TaxApplicableTradeCurrencyExchange (optional)
            //  10. SpecifiedTradeSettlementPaymentMeans (optional)
            //  11. ApplicableTradeTax (optional)
            //  12. BillingSpecifiedPeriod (optional)
            //  13. SpecifiedTradeAllowanceCharge (optional)
            //  14. SpecifiedLogisticsServiceCharge (optional)
            //  15. SpecifiedTradePaymentTerms (optional)
            //  16. SpecifiedTradeSettlementHeaderMonetarySummation
            //  17. InvoiceReferencedDocument (optional)
            //  18. ReceivableSpecifiedTradeAccountingAccount (optional)
            //  19. SpecifiedAdvancePayment (optional)

            //   1. CreditorReferenceID (optional)
            if (!String.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans?.SEPACreditorIdentifier))
            {
                Writer.WriteOptionalElementString("ram", "CreditorReferenceID", Descriptor.PaymentMeans?.SEPACreditorIdentifier, Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung | Profile.XRechnung1);
            }

            //   2. PaymentReference (optional)
            Writer.WriteOptionalElementString("ram", "PaymentReference", this.Descriptor.PaymentReference);

            //   3. TaxCurrencyCode (optional)
            //   BT-6
            if (this.Descriptor.TaxCurrency.HasValue)
            {
                Writer.WriteElementString("ram", "TaxCurrencyCode", this.Descriptor.TaxCurrency.Value.EnumToString(), profile: Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
            }

            //   4. InvoiceCurrencyCode (optional)
            Writer.WriteElementString("ram", "InvoiceCurrencyCode", this.Descriptor.Currency.EnumToString());

            //   5. InvoiceIssuerReference (optional)
            Writer.WriteElementString("ram", "InvoiceIssuerReference", this.Descriptor.SellerReferenceNo, profile: Profile.Extended);

            //   6. InvoicerTradeParty (optional)
            _writeOptionalParty(Writer, "ram", "InvoicerTradeParty", this.Descriptor.Invoicer);

            //   7. InvoiceeTradeParty (optional)
            if (Descriptor.Profile == Profile.Extended)
            {
                _writeOptionalParty(Writer, "ram", "InvoiceeTradeParty", this.Descriptor.Invoicee);
            }

            //   8. PayeeTradeParty (optional)
            if (Descriptor.Profile != Profile.Minimum)
            {
                _writeOptionalParty(Writer, "ram", "PayeeTradeParty", this.Descriptor.Payee);
            }

            //  10. SpecifiedTradeSettlementPaymentMeans (optional)
            if (!this.Descriptor.AnyCreditorFinancialAccount() && !this.Descriptor.AnyDebitorFinancialAccount())
            {
                if (this.Descriptor.PaymentMeans != null)
                {
                    Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram", "TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram", "Information", this.Descriptor.PaymentMeans.Information);

                        if (this.Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            Writer.WriteStartElement("ram", "ApplicableTradeSettlementFinancialCard", Profile.Comfort | Profile.Extended | Profile.XRechnung);
                            Writer.WriteElementString("ram", "ID", Descriptor.PaymentMeans.FinancialCard.Id);
                            Writer.WriteElementString("ram", "CardholderName", Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            Writer.WriteEndElement(); // !ram:ApplicableTradeSettlementFinancialCard
                        }
                    }
                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }
            else
            {
                foreach (BankAccount creditorAccount in this.Descriptor.GetCreditorFinancialAccounts())
                {
                    Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram", "TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram", "Information", this.Descriptor.PaymentMeans.Information);

                        if (this.Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            Writer.WriteStartElement("ram", "ApplicableTradeSettlementFinancialCard", Profile.Comfort | Profile.Extended | Profile.XRechnung);
                            Writer.WriteElementString("ram", "ID", Descriptor.PaymentMeans.FinancialCard.Id);
                            Writer.WriteElementString("ram", "CardholderName", Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            Writer.WriteEndElement(); // !ram:ApplicableTradeSettlementFinancialCard
                        }
                    }

                    Writer.WriteStartElement("ram", "PayeePartyCreditorFinancialAccount");
                    Writer.WriteElementString("ram", "IBANID", creditorAccount.IBAN);
                    Writer.WriteOptionalElementString("ram", "AccountName", creditorAccount.Name);
                    Writer.WriteOptionalElementString("ram", "ProprietaryID", creditorAccount.ID);
                    Writer.WriteEndElement(); // !PayeePartyCreditorFinancialAccount

                    Writer.WriteStartElement("ram", "PayeeSpecifiedCreditorFinancialInstitution");
                    Writer.WriteElementString("ram", "BICID", creditorAccount.BIC);
                    Writer.WriteOptionalElementString("ram", "GermanBankleitzahlID", creditorAccount.Bankleitzahl);
                    Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }

                foreach (BankAccount debitorAccount in this.Descriptor.GetDebitorFinancialAccounts())
                {
                    Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram", "TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram", "Information", this.Descriptor.PaymentMeans.Information);
                    }

                    Writer.WriteStartElement("ram", "PayerPartyDebtorFinancialAccount");
                    Writer.WriteElementString("ram", "IBANID", debitorAccount.IBAN);
                    Writer.WriteOptionalElementString("ram", "ProprietaryID", debitorAccount.ID);
                    Writer.WriteEndElement(); // !PayerPartyDebtorFinancialAccount

                    if (!string.IsNullOrWhiteSpace(debitorAccount.BIC) ||
                        !string.IsNullOrWhiteSpace(debitorAccount.Bankleitzahl) ||
                        !string.IsNullOrWhiteSpace(debitorAccount.BankName))
                    {
                        Writer.WriteStartElement("ram", "PayerSpecifiedDebtorFinancialInstitution");

                        Writer.WriteOptionalElementString("ram", "BICID", debitorAccount.BIC);
                        Writer.WriteOptionalElementString("ram", "GermanBankleitzahlID", debitorAccount.Bankleitzahl);
                        Writer.WriteOptionalElementString("ram", "Name", debitorAccount.BankName);
                        Writer.WriteEndElement(); // !PayerSpecifiedDebtorFinancialInstitution
                    }

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

            //  11. ApplicableTradeTax (optional)
            _writeOptionalTaxes(Writer);

            #region BillingSpecifiedPeriod
            //  12. BillingSpecifiedPeriod (optional)
            if (Descriptor.BillingPeriodStart.HasValue || Descriptor.BillingPeriodEnd.HasValue)
            {
                Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                if (Descriptor.BillingPeriodStart.HasValue)
                {
                    Writer.WriteStartElement("ram", "StartDateTime");
                    _writeElementWithAttribute(Writer, "udt", "DateTimeString", "format", "102", _formatDate(this.Descriptor.BillingPeriodStart.Value));
                    Writer.WriteEndElement(); // !StartDateTime
                }

                if (Descriptor.BillingPeriodEnd.HasValue)
                {
                    Writer.WriteStartElement("ram", "EndDateTime");
                    _writeElementWithAttribute(Writer, "udt", "DateTimeString", "format", "102", _formatDate(this.Descriptor.BillingPeriodEnd.Value));
                    Writer.WriteEndElement(); // !EndDateTime
                }
                Writer.WriteEndElement(); // !BillingSpecifiedPeriod
            }
            #endregion

            //  13. SpecifiedTradeAllowanceCharge (optional)
            foreach (TradeAllowanceCharge tradeAllowanceCharge in this.Descriptor.GetTradeAllowanceCharges())
            {
                Writer.WriteStartElement("ram", "SpecifiedTradeAllowanceCharge", ALL_PROFILES ^ Profile.Minimum);
                Writer.WriteStartElement("ram", "ChargeIndicator");
                Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                Writer.WriteEndElement(); // !ram:ChargeIndicator

                // TODO: SequenceNumeric

                if (tradeAllowanceCharge.BasisAmount.HasValue)
                {
                    Writer.WriteStartElement("ram", "BasisAmount");
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value));
                    Writer.WriteEndElement();
                }

                Writer.WriteStartElement("ram", "ActualAmount");
                Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount));
                Writer.WriteEndElement();

                Writer.WriteOptionalElementString("ram", "ReasonCode", tradeAllowanceCharge.ReasonCode.GetDescriptionAttribute()); // BT-98
                Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason); // BT-97

                if (tradeAllowanceCharge.Tax != null)
                {
                    Writer.WriteStartElement("ram", "CategoryTradeTax");
                    Writer.WriteElementString("ram", "TypeCode", tradeAllowanceCharge.Tax.TypeCode.EnumToString());
                    if (tradeAllowanceCharge.Tax.CategoryCode.HasValue)
                        Writer.WriteElementString("ram", "CategoryCode", tradeAllowanceCharge.Tax.CategoryCode?.EnumToString());
                    Writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(tradeAllowanceCharge.Tax.Percent));
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();
            }

            //  14. SpecifiedLogisticsServiceCharge (optional)
            foreach (ServiceCharge serviceCharge in this.Descriptor.GetLogisticsServiceCharges())
            {
                Writer.WriteStartElement("ram", "SpecifiedLogisticsServiceCharge");
                if (!String.IsNullOrWhiteSpace(serviceCharge.Description))
                {
                    Writer.WriteElementString("ram", "Description", serviceCharge.Description);
                }
                Writer.WriteElementString("ram", "AppliedAmount", _formatDecimal(serviceCharge.Amount));
                if (serviceCharge.Tax != null)
                {
                    Writer.WriteStartElement("ram", "AppliedTradeTax");
                    Writer.WriteElementString("ram", "TypeCode", serviceCharge.Tax.TypeCode.EnumToString());
                    if (serviceCharge.Tax.CategoryCode.HasValue)
                        Writer.WriteElementString("ram", "CategoryCode", serviceCharge.Tax.CategoryCode?.EnumToString());
                    Writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(serviceCharge.Tax.Percent));
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();
            }

            //  15. SpecifiedTradePaymentTerms (optional)
            //  The cardinality depends on the profile.
            switch (Descriptor.Profile)
            {
                case Profile.Unknown:
                case Profile.Minimum:
                    break;
                case Profile.XRechnung:
                    if (Descriptor.GetTradePaymentTerms().Count > 0 || !string.IsNullOrWhiteSpace(Descriptor.PaymentMeans?.SEPAMandateReference))
                    {
                        Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        var sbPaymentNotes = new StringBuilder();
                        DateTime? dueDate = null;
                        foreach (PaymentTerms paymentTerms in this.Descriptor.GetTradePaymentTerms())
                        {
                            if (paymentTerms.PaymentTermsType.HasValue)
                            {
                                sbPaymentNotes.Append($"#{((PaymentTermsType)paymentTerms.PaymentTermsType).EnumToString<PaymentTermsType>().ToUpper()}");
                                sbPaymentNotes.Append($"#TAGE={paymentTerms.DueDays}");
                                sbPaymentNotes.Append($"#PROZENT={_formatDecimal(paymentTerms.Percentage)}");
                                sbPaymentNotes.Append(paymentTerms.BaseAmount.HasValue ? $"#BASISBETRAG={_formatDecimal(paymentTerms.BaseAmount)}" : String.Empty);
                                sbPaymentNotes.AppendLine("#");
                            }
                            else
                            {
                                sbPaymentNotes.AppendLine(paymentTerms.Description);
                            }
                            dueDate = dueDate ?? paymentTerms.DueDate;
                        }
                        Writer.WriteOptionalElementString("ram", "Description", sbPaymentNotes.ToString());
                        if (dueDate.HasValue)
                        {
                            Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttribute(Writer, "udt", "DateTimeString", "format", "102", _formatDate(dueDate.Value));
                            Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", Descriptor.PaymentMeans?.SEPAMandateReference);
                        Writer.WriteEndElement();
                    }
                    break;
                case Profile.Extended:
                    foreach (PaymentTerms paymentTerms in this.Descriptor.GetTradePaymentTerms())
                    {
                        Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        Writer.WriteOptionalElementString("ram", "Description", paymentTerms.Description);
                        if (paymentTerms.DueDate.HasValue)
                        {
                            Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttribute(Writer, "udt", "DateTimeString", "format", "102", _formatDate(paymentTerms.DueDate.Value));
                            Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        if (paymentTerms.PaymentTermsType.HasValue)
                        {
                            if (paymentTerms.PaymentTermsType == PaymentTermsType.Skonto)
                            {
                                Writer.WriteStartElement("ram", "ApplicableTradePaymentDiscountTerms");
                                _writeOptionalAmount(Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                _writeOptionalAmount(Writer, "ram", "ActualDiscountAmount", paymentTerms.ActualAmount, forceCurrency: false);
                                Writer.WriteEndElement(); // !ram:ApplicableTradePaymentDiscountTerms
                            }
                            if (paymentTerms.PaymentTermsType == PaymentTermsType.Verzug)
                            {
                                Writer.WriteStartElement("ram", "ApplicableTradePaymentPenaltyTerms");
                                _writeOptionalAmount(Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                _writeOptionalAmount(Writer, "ram", "ActualPenaltyAmount", paymentTerms.ActualAmount, forceCurrency: false);
                                Writer.WriteEndElement(); // !ram:ApplicableTradePaymentPenaltyTerms
                            }
                        }
                        Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", Descriptor.PaymentMeans?.SEPAMandateReference);
                        Writer.WriteEndElement();
                    }
                    if (this.Descriptor.GetTradePaymentTerms().Count == 0 && !string.IsNullOrWhiteSpace(Descriptor.PaymentMeans?.SEPAMandateReference))
                    {
                        Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", Descriptor.PaymentMeans?.SEPAMandateReference);
                        Writer.WriteEndElement();
                    }
                    break;
                default:
                    if (Descriptor.GetTradePaymentTerms().Count > 0 || !string.IsNullOrWhiteSpace(Descriptor.PaymentMeans?.SEPAMandateReference))
                    {
                        Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        var sbPaymentNotes = new StringBuilder();
                        DateTime? dueDate = null;
                        foreach (PaymentTerms paymentTerms in this.Descriptor.GetTradePaymentTerms())
                        {
                            if (paymentTerms.PaymentTermsType.HasValue)
                            {
                                if (paymentTerms.PaymentTermsType == PaymentTermsType.Skonto)
                                {
                                    Writer.WriteStartElement("ram", "ApplicableTradePaymentDiscountTerms");
                                    _writeOptionalAmount(Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                    Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                    Writer.WriteEndElement(); // !ram:ApplicableTradePaymentDiscountTerms
                                }
                                if (paymentTerms.PaymentTermsType == PaymentTermsType.Verzug)
                                {
                                    Writer.WriteStartElement("ram", "ApplicableTradePaymentPenaltyTerms");
                                    _writeOptionalAmount(Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                    Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                    Writer.WriteEndElement(); // !ram:ApplicableTradePaymentPenaltyTerms
                                }
                            }
                            else
                            {
                                sbPaymentNotes.AppendLine(paymentTerms.Description);
                            }
                            dueDate = dueDate ?? paymentTerms.DueDate;
                        }
                        Writer.WriteOptionalElementString("ram", "Description", sbPaymentNotes.ToString().TrimEnd());
                        if (dueDate.HasValue)
                        {
                            Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttribute(Writer, "udt", "DateTimeString", "format", "102", _formatDate(dueDate.Value));
                            Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", Descriptor.PaymentMeans?.SEPAMandateReference);
                        Writer.WriteEndElement();
                    }
                    break;
            }

            //  16. SpecifiedTradeSettlementHeaderMonetarySummation
            Writer.WriteStartElement("ram", "SpecifiedTradeSettlementHeaderMonetarySummation");
            _writeOptionalAmount(Writer, "ram", "LineTotalAmount", this.Descriptor.LineTotalAmount);
            _writeOptionalAmount(Writer, "ram", "ChargeTotalAmount", this.Descriptor.ChargeTotalAmount);
            _writeOptionalAmount(Writer, "ram", "AllowanceTotalAmount", this.Descriptor.AllowanceTotalAmount);
            _writeOptionalAmount(Writer, "ram", "TaxBasisTotalAmount", this.Descriptor.TaxBasisAmount);
            _writeOptionalAmount(Writer, "ram", "TaxTotalAmount", this.Descriptor.TaxTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "ram", "RoundingAmount", this.Descriptor.RoundingAmount, profile: Profile.Comfort | Profile.Extended);  // RoundingAmount  //Rundungsbetrag
            _writeOptionalAmount(Writer, "ram", "GrandTotalAmount", this.Descriptor.GrandTotalAmount);

            if (this.Descriptor.TotalPrepaidAmount.HasValue)
            {
                _writeOptionalAmount(Writer, "ram", "TotalPrepaidAmount", this.Descriptor.TotalPrepaidAmount.Value);
            }

            _writeOptionalAmount(Writer, "ram", "DuePayableAmount", this.Descriptor.DuePayableAmount);
            Writer.WriteEndElement(); // !ram:SpecifiedTradeSettlementHeaderMonetarySummation

            #region InvoiceReferencedDocument
            //  17. InvoiceReferencedDocument (optional)
            foreach (InvoiceReferencedDocument invoiceReferencedDocument in this.Descriptor.GetInvoiceReferencedDocuments())
            {
                Writer.WriteStartElement("ram", "InvoiceReferencedDocument", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                Writer.WriteOptionalElementString("ram", "IssuerAssignedID", invoiceReferencedDocument.ID);
                if (invoiceReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    _writeElementWithAttribute(Writer, "qdt", "DateTimeString", "format", "102", _formatDate(invoiceReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }
                Writer.WriteEndElement(); // !ram:InvoiceReferencedDocument
                break; // only one occurrence allowed in this version!
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


        private void _writeOptionalAmount(ProfileAwareXmlTextWriter writer, string prefix, string tagName, decimal? value, int numDecimals = 2, bool forceCurrency = false, Profile profile = Profile.Unknown)
        {
            if (value.HasValue)
            {
                writer.WriteStartElement(prefix, tagName, profile);
                if (forceCurrency)
                {
                    writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                }
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
                writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
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


        private void _writeOptionalTaxes(ProfileAwareXmlTextWriter writer)
        {
            foreach (Tax tax in this.Descriptor.GetApplicableTradeTaxes())
            {
                writer.WriteStartElement("ram", "ApplicableTradeTax");

                writer.WriteStartElement("ram", "CalculatedAmount");
                writer.WriteValue(_formatDecimal(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount

                writer.WriteElementString("ram", "TypeCode", tax.TypeCode.EnumToString());
                Writer.WriteOptionalElementString("ram", "ExemptionReason", tax.ExemptionReason);
                writer.WriteStartElement("ram", "BasisAmount");
                writer.WriteValue(_formatDecimal(tax.BasisAmount));
                writer.WriteEndElement(); // !BasisAmount

                if (tax.AllowanceChargeBasisAmount.HasValue && (tax.AllowanceChargeBasisAmount.Value != 0))
                {
                    writer.WriteStartElement("ram", "AllowanceChargeBasisAmount", Profile.Extended);
                    writer.WriteValue(_formatDecimal(tax.AllowanceChargeBasisAmount));
                    writer.WriteEndElement(); // !AllowanceChargeBasisAmount
                }
                if (tax.LineTotalBasisAmount.HasValue && (tax.LineTotalBasisAmount.Value != 0))
                {
                    writer.WriteStartElement("ram", "LineTotalBasisAmount", Profile.Extended);
                    writer.WriteValue(_formatDecimal(tax.LineTotalBasisAmount));
                    writer.WriteEndElement();
                }

                if (tax.CategoryCode.HasValue)
                {
                    writer.WriteElementString("ram", "CategoryCode", tax.CategoryCode?.EnumToString());
                }

                if (tax.ExemptionReasonCode.HasValue)
                {
                    writer.WriteElementString("ram", "ExemptionReasonCode", tax.ExemptionReasonCode?.EnumToString());
                }

                writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(tax.Percent));
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
                    if (note.ContentCode != ContentCodes.Unknown)
                    {
                        writer.WriteElementString("ram", "ContentCode", note.ContentCode.EnumToString());
                    }
                    writer.WriteElementString("ram", "Content", note.Content);
                    if (note.SubjectCode != SubjectCodes.Unknown)
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
                    if (Party.ID.SchemeID.HasValue && (Party.ID.SchemeID.Value != GlobalIDSchemeIdentifiers.Unknown))
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

                if ((Party.GlobalID != null) && !String.IsNullOrWhiteSpace(Party.GlobalID.ID) && Party.GlobalID.SchemeID.HasValue && (Party.GlobalID.SchemeID.Value != GlobalIDSchemeIdentifiers.Unknown))
                {
                    writer.WriteStartElement("ram", "GlobalID");
                    writer.WriteAttributeString("schemeID", Party.GlobalID.SchemeID.Value.EnumToString());
                    writer.WriteValue(Party.GlobalID.ID);
                    writer.WriteEndElement();
                }
                writer.WriteOptionalElementString("ram", "Name", Party.Name);
                writer.WriteOptionalElementString("ram", "Description", Party.Description, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                _writeOptionalContact(writer, "ram", "DefinedTradeContact", Contact);

                writer.WriteStartElement("ram", "PostalTradeAddress");
                writer.WriteOptionalElementString("ram", "PostcodeCode", Party.Postcode);
                writer.WriteOptionalElementString("ram", "LineOne", string.IsNullOrWhiteSpace(Party.ContactName) ? Party.Street : Party.ContactName);
                if (!string.IsNullOrWhiteSpace(Party.ContactName)) { writer.WriteOptionalElementString("ram", "LineTwo", Party.Street); }

                writer.WriteOptionalElementString("ram", "LineThree", Party.AddressLine3); // BT-163

                writer.WriteOptionalElementString("ram", "CityName", Party.City);

                if (Party.Country != CountryCodes.Unknown)
                {
                    writer.WriteElementString("ram", "CountryID", Party.Country.EnumToString());
                }

                writer.WriteOptionalElementString("ram", "CountrySubDivisionName", Party.CountrySubdivisionName); // BT-79
                writer.WriteEndElement(); // !PostalTradeAddress

                if (TaxRegistrations != null)
                {
                    foreach (TaxRegistration registration in TaxRegistrations)
                    {
                        if (!String.IsNullOrWhiteSpace(registration.No))
                        {
                            writer.WriteStartElement("ram", "SpecifiedTaxRegistration");
                            writer.WriteStartElement("ram", "ID");
                            writer.WriteAttributeString("schemeID", registration.SchemeID.EnumToString());
                            writer.WriteValue(registration.No);
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
                case InvoiceType.SelfBilledInvoice:
                case InvoiceType.Invoice: return "RECHNUNG";
                case InvoiceType.SelfBilledCreditNote:
                case InvoiceType.CreditNote: return "GUTSCHRIFT";
                case InvoiceType.DebitNote: return "BELASTUNGSANZEIGE";
                case InvoiceType.DebitnoteRelatedToFinancialAdjustments: return "WERTBELASTUNG";
                case InvoiceType.PartialInvoice: return "TEILRECHNUNG";
                case InvoiceType.PrepaymentInvoice: return "VORAUSZAHLUNGSRECHNUNG";
                case InvoiceType.InvoiceInformation: return "KEINERECHNUNG";
                case InvoiceType.Correction: return "KORREKTURRECHNUNG";
                case InvoiceType.Unknown: return String.Empty;
                default: return String.Empty;
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
                if (!descriptor.GetTradeLineItems().All(l => l.TaxType.Equals(TaxTypes.VAT) || l.TaxType.Equals(TaxTypes.Unknown)))
                {
                    if (throwExceptions) { throw new UnsupportedException("Tax types other than VAT only possible with extended profile."); }
                    return false;
                }
            }

            return true;
        } // !Validate()
    }
}
