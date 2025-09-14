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
using System.Runtime.InteropServices;
using System.Text;

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor20Writer : IInvoiceDescriptorWriter
    {
        private ProfileAwareXmlTextWriter _Writer;
        private InvoiceDescriptor _Descriptor;


        private readonly Profile ALL_PROFILES = Profile.Minimum | Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung;
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
                throw new UnsupportedException("UBL format is not supported for ZUGFeRD 2.0");
            }

            // write data
            long streamPosition = stream.Position;

            this._Descriptor = descriptor;
            this._Writer = new ProfileAwareXmlTextWriter(stream, descriptor.Profile, options?.AutomaticallyCleanInvalidCharacters ?? false);
            this._Writer.SetNamespaces(new Dictionary<string, string>()
            {
                { "a", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100" },
                { "rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100" },
                { "qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100" },
                { "ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100" },
                { "xs", "http://www.w3.org/2001/XMLSchema" },
                { "udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100" }
            });

            _Writer.WriteStartDocument();
            _WriteHeaderComments(_Writer, options);

            #region Kopfbereich
            _Writer.WriteStartElement("rsm", "CrossIndustryInvoice");
            _Writer.WriteAttributeString("xmlns", "a", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            _Writer.WriteAttributeString("xmlns", "rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            _Writer.WriteAttributeString("xmlns", "qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            _Writer.WriteAttributeString("xmlns", "ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            _Writer.WriteAttributeString("xmlns", "xs", "http://www.w3.org/2001/XMLSchema");
            _Writer.WriteAttributeString("xmlns", "udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");
            #endregion

            #region SpecifiedExchangedDocumentContext
            _Writer.WriteStartElement("rsm", "ExchangedDocumentContext");

            if (_Descriptor.IsTest)
            {
                _Writer.WriteStartElement("ram", "TestIndicator");
                _Writer.WriteElementString("udt", "Indicator", "true");
                _Writer.WriteEndElement(); // !ram:TestIndicator
            }

            if (!String.IsNullOrWhiteSpace(this._Descriptor.BusinessProcess))
            {
                _Writer.WriteStartElement("ram", "BusinessProcessSpecifiedDocumentContextParameter");
                _Writer.WriteElementString("ram", "ID", this._Descriptor.BusinessProcess);
                _Writer.WriteEndElement(); // !ram:BusinessProcessSpecifiedDocumentContextParameter
            }

            _Writer.WriteStartElement("ram", "GuidelineSpecifiedDocumentContextParameter");
            _Writer.WriteElementString("ram", "ID", this._Descriptor.Profile.EnumToString(ZUGFeRDVersion.Version20));
            _Writer.WriteEndElement(); // !ram:GuidelineSpecifiedDocumentContextParameter
            _Writer.WriteEndElement(); // !rsm:ExchangedDocumentContext

            _Writer.WriteStartElement("rsm", "ExchangedDocument");
            _Writer.WriteElementString("ram", "ID", this._Descriptor.InvoiceNo);
            _Writer.WriteOptionalElementString("ram", "Name", this._Descriptor.Name, Profile.Extended);
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
            _Writer.WriteEndElement(); // !rsm:ExchangedDocument
            #endregion

            /*
             * @todo continue here to adopt v2 tag names
             */

            #region SupplyChainTradeTransaction
            _Writer.WriteStartElement("rsm", "SupplyChainTradeTransaction");

            foreach (TradeLineItem tradeLineItem in this._Descriptor.GetTradeLineItems())
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.IncludedSupplyChainTradeLineItemComment);
                _Writer.WriteStartElement("ram", "IncludedSupplyChainTradeLineItem");

                if (tradeLineItem.AssociatedDocument != null)
                {
                    _Writer.WriteStartElement("ram", "AssociatedDocumentLineDocument");
                    _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.AssociatedDocument.LineID);
                    if (tradeLineItem.AssociatedDocument.LineStatusCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "LineStatusCode", EnumExtensions.EnumToString<LineStatusCodes>(tradeLineItem.AssociatedDocument.LineStatusCode));
                    }
                    if (tradeLineItem.AssociatedDocument.LineStatusReasonCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "LineStatusReasonCode", tradeLineItem.AssociatedDocument.LineStatusReasonCode.Value.EnumToString());
                    }
                    _writeNotes(_Writer, tradeLineItem.AssociatedDocument.Notes);
                    _Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument
                }

                // handelt es sich um einen Kommentar?
                if ((tradeLineItem.AssociatedDocument?.Notes.Count > 0) && (tradeLineItem.BilledQuantity == 0) && (String.IsNullOrWhiteSpace(tradeLineItem.Description)))
                {
                    _Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItemComment
                    continue;
                }

                _Writer.WriteStartElement("ram", "SpecifiedTradeProduct");
                if ((tradeLineItem.GlobalID != null) && (tradeLineItem.GlobalID.SchemeID.HasValue) && tradeLineItem.GlobalID.SchemeID.HasValue && !String.IsNullOrWhiteSpace(tradeLineItem.GlobalID.ID))
                {
                    _writeElementWithAttribute(_Writer, "ram", "GlobalID", "schemeID", tradeLineItem.GlobalID.SchemeID.Value.EnumToString(), tradeLineItem.GlobalID.ID);
                }

                _Writer.WriteOptionalElementString("ram", "SellerAssignedID", tradeLineItem.SellerAssignedID);
                _Writer.WriteOptionalElementString("ram", "BuyerAssignedID", tradeLineItem.BuyerAssignedID);
                _Writer.WriteOptionalElementString("ram", "Name", tradeLineItem.Name);
                _Writer.WriteOptionalElementString("ram", "Description", tradeLineItem.Description);

                if (tradeLineItem.ApplicableProductCharacteristics != null && tradeLineItem.ApplicableProductCharacteristics.Any())
                {
                    foreach (var productCharacteristic in tradeLineItem.ApplicableProductCharacteristics)
                    {
                        _Writer.WriteStartElement("ram", "ApplicableProductCharacteristic");
                        _Writer.WriteOptionalElementString("ram", "Description", productCharacteristic.Description);
                        _Writer.WriteOptionalElementString("ram", "Value", productCharacteristic.Value);
                        _Writer.WriteEndElement(); // !ram:ApplicableProductCharacteristic
                    }
                }

                if (tradeLineItem.IncludedReferencedProducts != null && tradeLineItem.IncludedReferencedProducts.Any())
                {
                    foreach (var includedItem in tradeLineItem.IncludedReferencedProducts)
                    {
                        _Writer.WriteStartElement("ram", "IncludedReferencedProduct");
                        _Writer.WriteOptionalElementString("ram", "Name", includedItem.Name);

                        if (includedItem.UnitQuantity.HasValue)
                        {
                            _writeElementWithAttribute(_Writer, "ram", "UnitQuantity", "unitCode", includedItem.UnitCode.Value.EnumToString(), _formatDecimal(includedItem.UnitQuantity, 4));
                        }
                        _Writer.WriteEndElement(); // !ram:IncludedReferencedProduct
                    }
                }

                _Writer.WriteEndElement(); // !ram:SpecifiedTradeProduct

                _Writer.WriteStartElement("ram", "SpecifiedLineTradeAgreement", Profile.Basic | Profile.Comfort | Profile.Extended);

                if (tradeLineItem.BuyerOrderReferencedDocument != null)
                {
                    _Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument", Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    // order number
                    _Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.BuyerOrderReferencedDocument.ID);

                    // reference to the order position
                    _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.BuyerOrderReferencedDocument.LineID);

                    if (tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        _Writer.WriteStartElement("qdt", "DateTimeString");
                        _Writer.WriteAttributeString("format", "102");
                        _Writer.WriteValue(_formatDate(tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.Value));
                        _Writer.WriteEndElement(); // !qdt:DateTimeString
                        _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                    }

                    _Writer.WriteEndElement(); // !ram:BuyerOrderReferencedDocument
                }

                if (tradeLineItem.ContractReferencedDocument != null)
                {
                    _Writer.WriteStartElement("ram", "ContractReferencedDocument", Profile.Extended);

                    _Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.ContractReferencedDocument.ID);

                    // reference to the contract position
                    _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.ContractReferencedDocument.LineID);

                    if (tradeLineItem.ContractReferencedDocument.IssueDateTime.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        _Writer.WriteStartElement("qdt", "DateTimeString");
                        _Writer.WriteAttributeString("format", "102");
                        _Writer.WriteValue(_formatDate(tradeLineItem.ContractReferencedDocument.IssueDateTime.Value));
                        _Writer.WriteEndElement(); // !udt:DateTimeString
                        _Writer.WriteEndElement(); // !ram:IssueDateTime
                    }                    

                    _Writer.WriteEndElement(); // !ram:ContractReferencedDocument(Extended)
                }

                foreach (AdditionalReferencedDocument document in tradeLineItem.AdditionalReferencedDocuments)
                {
                    _Writer.WriteStartElement("ram", "AdditionalReferencedDocument");
                    if (document.IssueDateTime.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        _Writer.WriteStartElement("qdt", "DateTimeString");
                        _Writer.WriteValue(_formatDate(document.IssueDateTime.Value));
                        _Writer.WriteEndElement(); // !qdt:DateTimeString
                        _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                    }

                    _Writer.WriteElementString("ram", "LineID", String.Format("{0}", tradeLineItem.AssociatedDocument?.LineID));
                    _Writer.WriteOptionalElementString("ram", "IssuerAssignedID", document.ID);

                    if (document.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", EnumExtensions.EnumToString<AdditionalReferencedDocumentTypeCode>(document.TypeCode.Value));
                    }

                    if (document.ReferenceTypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "ReferenceTypeCode", document.ReferenceTypeCode.Value.EnumToString());
                    }

                    _Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                } // !foreach(document)

                bool needToWriteGrossUnitPrice = false;
                bool hasGrossUnitPrice = tradeLineItem.GrossUnitPrice.HasValue;
                bool hasAllowanceCharges = tradeLineItem.GetTradeAllowanceCharges().Count > 0;

                if ((descriptor.Profile == Profile.XRechnung) || (descriptor.Profile == Profile.XRechnung1) || (descriptor.Profile == Profile.Comfort))
                {
                    // PEPPOL-EN16931-R046: For XRechnung, both must be present
                    needToWriteGrossUnitPrice = hasGrossUnitPrice && hasAllowanceCharges;
                }
                else
                {
                    // For other profiles, either is sufficient
                    needToWriteGrossUnitPrice = hasGrossUnitPrice || hasAllowanceCharges;
                }

                if (needToWriteGrossUnitPrice)
                {
                    _Writer.WriteStartElement("ram", "GrossPriceProductTradePrice");
                    _writeOptionalAdaptiveAmount(_Writer, "ram", "ChargeAmount", tradeLineItem.GrossUnitPrice, 2, 4); // BT-148
                    if (tradeLineItem.GrossQuantity.HasValue)
                    {
                        _writeElementWithAttribute(_Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.GrossQuantity.Value, 4));
                    }

                    foreach (AbstractTradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.GetTradeAllowanceCharges())
                    {
                        _Writer.WriteStartElement("ram", "AppliedTradeAllowanceCharge");

                        _Writer.WriteStartElement("ram", "ChargeIndicator");
                        _Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                        _Writer.WriteEndElement(); // !ram:ChargeIndicator

                        _writeOptionalAdaptiveAmount(_Writer, "ram", "BasisAmount", tradeAllowanceCharge.BasisAmount, 2, 4, forceCurrency: false); // BT-X-35
                        _writeOptionalAdaptiveAmount(_Writer, "ram", "ActualAmount", tradeAllowanceCharge.ActualAmount, 2, 4, forceCurrency: false); // BT-147

                        _Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason, Profile.Comfort | Profile.Extended);
                        // "ReasonCode" nicht im 2.0 Standard!

                        _Writer.WriteEndElement(); // !AppliedTradeAllowanceCharge
                    }

                    _Writer.WriteEndElement(); // ram:GrossPriceProductTradePrice
                }

                _WriteComment(_Writer, options, InvoiceCommentConstants.NetPriceProductTradePriceComment);
                _Writer.WriteStartElement("ram", "NetPriceProductTradePrice");
                _writeOptionalAdaptiveAmount(_Writer, "ram", "ChargeAmount", tradeLineItem.NetUnitPrice, 2, 4);

                if (tradeLineItem.NetQuantity.HasValue)
                {
                    _writeElementWithAttribute(_Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.NetQuantity.Value, 4));
                }
                _Writer.WriteEndElement(); // ram:NetPriceProductTradePrice

                _Writer.WriteEndElement(); // !ram:SpecifiedLineTradeAgreement

                if (_Descriptor.Profile != Profile.Basic)
                {
                    _Writer.WriteStartElement("ram", "SpecifiedLineTradeDelivery");
                    _writeElementWithAttribute(_Writer, "ram", "BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));

                    if (tradeLineItem.DeliveryNoteReferencedDocument != null)
                    {
                        _Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument");
                        if (tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                        {
                            //Old path of Version 1.0
                            //_Writer.WriteStartElement("ram", "IssueDateTime");
                            //_Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                            //_Writer.WriteEndElement(); // !ram:IssueDateTime

                            _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                            _Writer.WriteStartElement("qdt", "DateTimeString");
                            _Writer.WriteAttributeString("format", "102");
                            _Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                            _Writer.WriteEndElement(); // !qdt:DateTimeString
                            _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                        }

                        _Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.DeliveryNoteReferencedDocument.ID);
                        _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.DeliveryNoteReferencedDocument.LineID);
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

                    _Writer.WriteEndElement(); // !ram:SpecifiedLineTradeDelivery
                }
                else
                {
                    _Writer.WriteStartElement("ram", "SpecifiedLineTradeDelivery");
                    _writeElementWithAttribute(_Writer, "ram", "BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));
                    _Writer.WriteEndElement(); // !ram:SpecifiedLineTradeDelivery
                }

                _Writer.WriteStartElement("ram", "SpecifiedLineTradeSettlement");

                _Writer.WriteStartElement("ram", "ApplicableTradeTax", Profile.Basic | Profile.Comfort | Profile.Extended);

                if (tradeLineItem.TaxType.HasValue)
                {
                    _Writer.WriteElementString("ram", "TypeCode", tradeLineItem.TaxType.EnumToString());
                }

                if (tradeLineItem.TaxCategoryCode.HasValue)
                {
                    _Writer.WriteElementString("ram", "CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString());
                }
                
                _Writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent));
                _Writer.WriteEndElement(); // !ram:ApplicableTradeTax

                if (tradeLineItem.BillingPeriodStart.HasValue || tradeLineItem.BillingPeriodEnd.HasValue)
                {
                    _Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    if (tradeLineItem.BillingPeriodStart.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "StartDateTime");
                        _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodStart.Value));
                        _Writer.WriteEndElement(); // !StartDateTime
                    }

                    if (tradeLineItem.BillingPeriodEnd.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "EndDateTime");
                        _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodEnd.Value));
                        _Writer.WriteEndElement(); // !EndDateTime
                    }
                    _Writer.WriteEndElement(); // !BillingSpecifiedPeriod
                }

                _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementLineMonetarySummationComment);
                _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementLineMonetarySummation");

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

                _Writer.WriteElementString("ram", "LineTotalAmount", _formatDecimal(total));

                _Writer.WriteEndElement(); // ram:SpecifiedTradeSettlementLineMonetarySummation
                _Writer.WriteEndElement(); // !ram:SpecifiedLineTradeSettlement

                _Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItemComment
            } // !foreach(tradeLineItem)

            _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableHeaderTradeAgreementComment);
            _Writer.WriteStartElement("ram", "ApplicableHeaderTradeAgreement");

            _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerReferenceComment);
            _Writer.WriteOptionalElementString("ram", "BuyerReference", this._Descriptor.ReferenceOrderNo);

            _WriteComment(_Writer, options, InvoiceCommentConstants.SellerTradePartyComment);
            _writeOptionalParty(_Writer, "ram", "SellerTradeParty", this._Descriptor.Seller, this._Descriptor.SellerContact, taxRegistrations: this._Descriptor.SellerTaxRegistration);

            _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerTradePartyComment);
            _writeOptionalParty(_Writer, "ram", "BuyerTradeParty", this._Descriptor.Buyer, this._Descriptor.BuyerContact, taxRegistrations: this._Descriptor.BuyerTaxRegistration);

            #region ApplicableTradeDeliveryTerms
            if (_Descriptor.ApplicableTradeDeliveryTermsCode.HasValue)
            {
                // BG-X-22, BT-X-145
                _Writer.WriteStartElement("ram", "ApplicableTradeDeliveryTerms", Profile.Extended);
                _Writer.WriteElementString("ram", "DeliveryTypeCode", EnumExtensions.EnumToString<TradeDeliveryTermCodes>(this._Descriptor.ApplicableTradeDeliveryTermsCode));
                _Writer.WriteEndElement(); // !ApplicableTradeDeliveryTerms
            }
            #endregion

            #region SellerTaxRepresentativeTradeParty
            // BT-63: the tax taxRegistration of the SellerTaxRepresentativeTradeParty
            _writeOptionalParty(_Writer, "ram", "SellerTaxRepresentativeTradeParty", this._Descriptor.SellerTaxRepresentative, taxRegistrations: this._Descriptor.SellerTaxRepresentativeTaxRegistration);
            #endregion

            #region SellerOrderReferencedDocument (BT-14: Comfort, Extended)
            if (null != this._Descriptor.SellerOrderReferencedDocument && !string.IsNullOrWhiteSpace(_Descriptor.SellerOrderReferencedDocument.ID))
            {
                _Writer.WriteStartElement("ram", "SellerOrderReferencedDocument", Profile.Comfort | Profile.Extended);
                _Writer.WriteElementString("ram", "IssuerAssignedID", this._Descriptor.SellerOrderReferencedDocument.ID);
                if (this._Descriptor.SellerOrderReferencedDocument.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime", Profile.Extended);
                    _Writer.WriteStartElement("qdt", "DateTimeString");
                    _Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(this._Descriptor.SellerOrderReferencedDocument.IssueDateTime.Value));
                    _Writer.WriteEndElement(); // !qdt:DateTimeString
                    _Writer.WriteEndElement(); // !IssueDateTime()
                }

                _Writer.WriteEndElement(); // !SellerOrderReferencedDocument
            }
            #endregion


            if (!String.IsNullOrWhiteSpace(this._Descriptor.OrderNo))
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerOrderReferencedDocumentComment);
                _Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument");
                _Writer.WriteElementString("ram", "IssuerAssignedID", this._Descriptor.OrderNo);
                if (this._Descriptor.OrderDate.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    _Writer.WriteStartElement("qdt", "DateTimeString");
                    _Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(this._Descriptor.OrderDate.Value));
                    _Writer.WriteEndElement(); // !qdt:DateTimeString
                    _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }
                _Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }


            if (this._Descriptor.AdditionalReferencedDocuments != null)
            {
                foreach (AdditionalReferencedDocument document in this._Descriptor.AdditionalReferencedDocuments)
                {
                    _Writer.WriteStartElement("ram", "AdditionalReferencedDocument");
                    if (document.IssueDateTime.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        _Writer.WriteStartElement("qdt", "DateTimeString");
                        _Writer.WriteAttributeString("format", "102");
                        _Writer.WriteValue(_formatDate(document.IssueDateTime.Value));
                        _Writer.WriteEndElement(); // !udt:DateTimeString
                        _Writer.WriteEndElement(); // !FormattedIssueDateTime
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
            }

            _Writer.WriteEndElement(); // !ApplicableHeaderTradeAgreement

            _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableHeaderTradeDeliveryComment);
            _Writer.WriteStartElement("ram", "ApplicableHeaderTradeDelivery"); // Pflichteintrag

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
                _Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument");

                if (this._Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    _Writer.WriteValue(_formatDate(this._Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.Value, false));
                    _Writer.WriteEndElement(); // !IssueDateTime
                }

                _Writer.WriteElementString("ram", "IssuerAssignedID", this._Descriptor.DeliveryNoteReferencedDocument.ID);
                _Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }

            _Writer.WriteEndElement(); // !ApplicableHeaderTradeDelivery

            _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableHeaderTradeSettlementComment);
            _Writer.WriteStartElement("ram", "ApplicableHeaderTradeSettlement");
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
            if (!String.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans?.SEPACreditorIdentifier))
            {
                _Writer.WriteOptionalElementString("ram", "CreditorReferenceID", _Descriptor.PaymentMeans?.SEPACreditorIdentifier, Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung | Profile.XRechnung1);
            }

            //   2. PaymentReference (optional)
            _Writer.WriteOptionalElementString("ram", "PaymentReference", this._Descriptor.PaymentReference);

            //   3. TaxCurrencyCode (optional)
            //   BT-6
            if (this._Descriptor.TaxCurrency.HasValue)
            {
                _Writer.WriteElementString("ram", "TaxCurrencyCode", this._Descriptor.TaxCurrency.Value.EnumToString(), profile: Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
            }

            //   4. InvoiceCurrencyCode (optional)
            _Writer.WriteElementString("ram", "InvoiceCurrencyCode", this._Descriptor.Currency.EnumToString());

            //   5. InvoiceIssuerReference (optional), BT-X-204
            _Writer.WriteOptionalElementString("ram", "InvoiceIssuerReference", this._Descriptor.SellerReferenceNo, Profile.Extended);

            //   6. InvoicerTradeParty (optional)
            _writeOptionalParty(_Writer, "ram", "InvoicerTradeParty", this._Descriptor.Invoicer);

            //   7. InvoiceeTradeParty (optional)
            if (_Descriptor.Profile == Profile.Extended)
            {
                _writeOptionalParty(_Writer, "ram", "InvoiceeTradeParty", this._Descriptor.Invoicee);
            }

            //   8. PayeeTradeParty (optional)
            if (_Descriptor.Profile != Profile.Minimum)
            {
                _writeOptionalParty(_Writer, "ram", "PayeeTradeParty", this._Descriptor.Payee);
            }

            //  10. SpecifiedTradeSettlementPaymentMeans (optional)
            if (!this._Descriptor.AnyCreditorFinancialAccount() && !this._Descriptor.AnyDebitorFinancialAccount())
            {
                if (this._Descriptor.PaymentMeans != null)
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementPaymentMeansComment);
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information);

                        if (this._Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            _Writer.WriteStartElement("ram", "ApplicableTradeSettlementFinancialCard", Profile.Comfort | Profile.Extended | Profile.XRechnung);
                            _Writer.WriteElementString("ram", "ID", _Descriptor.PaymentMeans.FinancialCard.Id);
                            _Writer.WriteElementString("ram", "CardholderName", _Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            _Writer.WriteEndElement(); // !ram:ApplicableTradeSettlementFinancialCard
                        }
                    }
                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }
            else
            {
                foreach (BankAccount creditorAccount in this._Descriptor.GetCreditorFinancialAccounts())
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementPaymentMeansComment);
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information);

                        if (this._Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            _Writer.WriteStartElement("ram", "ApplicableTradeSettlementFinancialCard", Profile.Comfort | Profile.Extended | Profile.XRechnung);
                            _Writer.WriteElementString("ram", "ID", _Descriptor.PaymentMeans.FinancialCard.Id);
                            _Writer.WriteElementString("ram", "CardholderName", _Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            _Writer.WriteEndElement(); // !ram:ApplicableTradeSettlementFinancialCard
                        }
                    }

                    _Writer.WriteStartElement("ram", "PayeePartyCreditorFinancialAccount");
                    _Writer.WriteElementString("ram", "IBANID", creditorAccount.IBAN);
                    _Writer.WriteOptionalElementString("ram", "AccountName", creditorAccount.Name);
                    _Writer.WriteOptionalElementString("ram", "ProprietaryID", creditorAccount.ID);
                    _Writer.WriteEndElement(); // !PayeePartyCreditorFinancialAccount

                    if (!String.IsNullOrWhiteSpace(creditorAccount.BIC))
                    {
                        _Writer.WriteStartElement("ram", "PayeeSpecifiedCreditorFinancialInstitution");
                        _Writer.WriteElementString("ram", "BICID", creditorAccount.BIC);
                        _Writer.WriteOptionalElementString("ram", "GermanBankleitzahlID", creditorAccount.Bankleitzahl);
                        _Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                    }
                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }

                foreach (BankAccount debitorAccount in this._Descriptor.GetDebitorFinancialAccounts())
                {
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information);
                    }

                    _Writer.WriteStartElement("ram", "PayerPartyDebtorFinancialAccount");
                    _Writer.WriteElementString("ram", "IBANID", debitorAccount.IBAN);
                    _Writer.WriteOptionalElementString("ram", "ProprietaryID", debitorAccount.ID);
                    _Writer.WriteEndElement(); // !PayerPartyDebtorFinancialAccount

                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
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
            _writeOptionalTaxes(_Writer, options);

            #region BillingSpecifiedPeriod
            //  12. BillingSpecifiedPeriod (optional)
            if (_Descriptor.BillingPeriodStart.HasValue || _Descriptor.BillingPeriodEnd.HasValue)
            {
                _Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                if (_Descriptor.BillingPeriodStart.HasValue)
                {
                    _Writer.WriteStartElement("ram", "StartDateTime");
                    _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(this._Descriptor.BillingPeriodStart.Value));
                    _Writer.WriteEndElement(); // !StartDateTime
                }

                if (_Descriptor.BillingPeriodEnd.HasValue)
                {
                    _Writer.WriteStartElement("ram", "EndDateTime");
                    _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(this._Descriptor.BillingPeriodEnd.Value));
                    _Writer.WriteEndElement(); // !EndDateTime
                }
                _Writer.WriteEndElement(); // !BillingSpecifiedPeriod
            }
            #endregion

            //  13. SpecifiedTradeAllowanceCharge (optional)
            foreach(TradeAllowance allowance in this._Descriptor.GetTradeAllowances())
            {
                _WriteDocumentLevelTradeAllowanceCharge(_Writer, allowance);
            } // !foreach(TradeAllowance)

            foreach(TradeCharge charge in this._Descriptor.GetTradeCharges())
            {
                _WriteDocumentLevelTradeAllowanceCharge(_Writer, charge);
            } // !foreach(TradeCharge)

            //  14. SpecifiedLogisticsServiceCharge (optional)
            foreach (ServiceCharge serviceCharge in this._Descriptor.GetLogisticsServiceCharges())
            {
                _Writer.WriteStartElement("ram", "SpecifiedLogisticsServiceCharge");
                if (!String.IsNullOrWhiteSpace(serviceCharge.Description))
                {
                    _Writer.WriteElementString("ram", "Description", serviceCharge.Description);
                }
                _Writer.WriteElementString("ram", "AppliedAmount", _formatDecimal(serviceCharge.Amount));
                if (serviceCharge.Tax != null)
                {
                    _Writer.WriteStartElement("ram", "AppliedTradeTax");

                    if (serviceCharge.Tax.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", serviceCharge.Tax.TypeCode.EnumToString());
                    }

                    if (serviceCharge.Tax.CategoryCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "CategoryCode", serviceCharge.Tax.CategoryCode.EnumToString());
                    }

                    _Writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(serviceCharge.Tax.Percent));
                    _Writer.WriteEndElement();
                }
                _Writer.WriteEndElement();
            }

            //  15. SpecifiedTradePaymentTerms (optional)
            //  The cardinality depends on the profile.
            switch (_Descriptor.Profile)
            {
                case Profile.Unknown:
                case Profile.Minimum:
                    break;
                case Profile.XRechnung:
                    if (_Descriptor.GetTradePaymentTerms().Count > 0 || !string.IsNullOrWhiteSpace(_Descriptor.PaymentMeans?.SEPAMandateReference))
                    {
                        _Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        var sbPaymentNotes = new StringBuilder();
                        DateTime? dueDate = null;
                        foreach (PaymentTerms paymentTerms in this._Descriptor.GetTradePaymentTerms())
                        {
                            if (paymentTerms.PaymentTermsType.HasValue && paymentTerms.DueDays.HasValue && paymentTerms.Percentage.HasValue)
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
                        _Writer.WriteOptionalElementString("ram", "Description", sbPaymentNotes.ToString());
                        if (dueDate.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(dueDate.Value));
                            _Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        _Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", _Descriptor.PaymentMeans?.SEPAMandateReference);
                        _Writer.WriteEndElement();
                    }
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
                                _writeOptionalAmount(_Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                _Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                _writeOptionalAmount(_Writer, "ram", "ActualDiscountAmount", paymentTerms.ActualAmount, forceCurrency: false);
                                _Writer.WriteEndElement(); // !ram:ApplicableTradePaymentDiscountTerms
                            }
                            if (paymentTerms.PaymentTermsType == PaymentTermsType.Verzug)
                            {
                                _Writer.WriteStartElement("ram", "ApplicableTradePaymentPenaltyTerms");
                                _writeOptionalAmount(_Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                _Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                _writeOptionalAmount(_Writer, "ram", "ActualPenaltyAmount", paymentTerms.ActualAmount, forceCurrency: false);
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
                    if (_Descriptor.GetTradePaymentTerms().Count > 0 || !string.IsNullOrWhiteSpace(_Descriptor.PaymentMeans?.SEPAMandateReference))
                    {
                        _Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        var sbPaymentNotes = new StringBuilder();
                        DateTime? dueDate = null;
                        foreach (PaymentTerms paymentTerms in this._Descriptor.GetTradePaymentTerms())
                        {
                            if (paymentTerms.PaymentTermsType.HasValue)
                            {
                                if (paymentTerms.PaymentTermsType == PaymentTermsType.Skonto)
                                {
                                    _Writer.WriteStartElement("ram", "ApplicableTradePaymentDiscountTerms");
                                    _writeOptionalAmount(_Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                    _Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                    _Writer.WriteEndElement(); // !ram:ApplicableTradePaymentDiscountTerms
                                }
                                if (paymentTerms.PaymentTermsType == PaymentTermsType.Verzug)
                                {
                                    _Writer.WriteStartElement("ram", "ApplicableTradePaymentPenaltyTerms");
                                    _writeOptionalAmount(_Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                    _Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                    _Writer.WriteEndElement(); // !ram:ApplicableTradePaymentPenaltyTerms
                                }
                            }
                            else
                            {
                                sbPaymentNotes.AppendLine(paymentTerms.Description);
                            }
                            dueDate = dueDate ?? paymentTerms.DueDate;
                        }
                        _Writer.WriteOptionalElementString("ram", "Description", sbPaymentNotes.ToString().TrimEnd());
                        if (dueDate.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttribute(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(dueDate.Value));
                            _Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        _Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", _Descriptor.PaymentMeans?.SEPAMandateReference);
                        _Writer.WriteEndElement();
                    }
                    break;
            }

            //  16. SpecifiedTradeSettlementHeaderMonetarySummation
            _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementHeaderMonetarySummationComment);
            _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementHeaderMonetarySummation");
            _writeOptionalAmount(_Writer, "ram", "LineTotalAmount", this._Descriptor.LineTotalAmount);
            _writeOptionalAmount(_Writer, "ram", "ChargeTotalAmount", this._Descriptor.ChargeTotalAmount);
            _writeOptionalAmount(_Writer, "ram", "AllowanceTotalAmount", this._Descriptor.AllowanceTotalAmount);
            _writeOptionalAmount(_Writer, "ram", "TaxBasisTotalAmount", this._Descriptor.TaxBasisAmount);
            _writeOptionalAmount(_Writer, "ram", "TaxTotalAmount", this._Descriptor.TaxTotalAmount, forceCurrency: true);
            _writeOptionalAmount(_Writer, "ram", "RoundingAmount", this._Descriptor.RoundingAmount, profile: Profile.Comfort | Profile.Extended);  // RoundingAmount  //Rundungsbetrag
            _writeOptionalAmount(_Writer, "ram", "GrandTotalAmount", this._Descriptor.GrandTotalAmount);

            if (this._Descriptor.TotalPrepaidAmount.HasValue)
            {
                _writeOptionalAmount(_Writer, "ram", "TotalPrepaidAmount", this._Descriptor.TotalPrepaidAmount.Value);
            }

            _writeOptionalAmount(_Writer, "ram", "DuePayableAmount", this._Descriptor.DuePayableAmount);
            _Writer.WriteEndElement(); // !ram:SpecifiedTradeSettlementHeaderMonetarySummation

            #region InvoiceReferencedDocument
            //  17. InvoiceReferencedDocument (optional)
            foreach (InvoiceReferencedDocument invoiceReferencedDocument in this._Descriptor.GetInvoiceReferencedDocuments())
            {
                _Writer.WriteStartElement("ram", "InvoiceReferencedDocument", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                _Writer.WriteOptionalElementString("ram", "IssuerAssignedID", invoiceReferencedDocument.ID);
                if (invoiceReferencedDocument.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    _writeElementWithAttribute(_Writer, "qdt", "DateTimeString", "format", "102", _formatDate(invoiceReferencedDocument.IssueDateTime.Value));
                    _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }
                _Writer.WriteEndElement(); // !ram:InvoiceReferencedDocument
                break; // only one occurrence allowed in this version!
            }
            #endregion


            _Writer.WriteEndElement(); // !ram:ApplicableHeaderTradeSettlement

            _Writer.WriteEndElement(); // !ram:SupplyChainTradeTransaction
            #endregion

            _Writer.WriteEndElement(); // !ram:Invoice
            _Writer.WriteEndDocument();
            _Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);
        } // !Save()


        private void _WriteDocumentLevelTradeAllowanceCharge(ProfileAwareXmlTextWriter writer, AbstractTradeAllowanceCharge tradeAllowanceCharge)
        {
            if (tradeAllowanceCharge == null)
            {
                return;
            }

            _Writer.WriteStartElement("ram", "SpecifiedTradeAllowanceCharge", ALL_PROFILES ^ Profile.Minimum);
            _Writer.WriteStartElement("ram", "ChargeIndicator");
            _Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
            _Writer.WriteEndElement(); // !ram:ChargeIndicator

            // TODO: SequenceNumeric

            if (tradeAllowanceCharge.BasisAmount.HasValue)
            {
                _Writer.WriteStartElement("ram", "BasisAmount");
                _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value));
                _Writer.WriteEndElement();
            }

            _Writer.WriteStartElement("ram", "ActualAmount");
            _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount));
            _Writer.WriteEndElement();

            if ((tradeAllowanceCharge is TradeAllowance allowance) && allowance.ReasonCode != null)
            {
                _Writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<AllowanceReasonCodes>(allowance.ReasonCode)); // BT-98
            }
            else if ((tradeAllowanceCharge is TradeCharge charge) && charge.ReasonCode != null)
            {
                _Writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<ChargeReasonCodes>(charge.ReasonCode)); // BT-98
            }

            _Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason); // BT-97

            if (tradeAllowanceCharge.Tax != null)
            {
                _Writer.WriteStartElement("ram", "CategoryTradeTax");

                if (tradeAllowanceCharge.Tax.TypeCode.HasValue)
                {
                    _Writer.WriteElementString("ram", "TypeCode", tradeAllowanceCharge.Tax.TypeCode.EnumToString());
                }

                if (tradeAllowanceCharge.Tax.CategoryCode.HasValue)
                {
                    _Writer.WriteElementString("ram", "CategoryCode", tradeAllowanceCharge.Tax.CategoryCode.EnumToString());
                }
                
                _Writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(tradeAllowanceCharge.Tax.Percent));
                _Writer.WriteEndElement();
            }
            _Writer.WriteEndElement();
        } // !_WriteDocumentLevelTradeAllowanceCharge()


        private void _writeOptionalAmount(ProfileAwareXmlTextWriter writer, string prefix, string tagName, decimal? value, int numDecimals = 2, bool forceCurrency = false, Profile profile = Profile.Unknown)
        {
            if (value.HasValue)
            {
                writer.WriteStartElement(prefix, tagName, profile);
                if (forceCurrency)
                {
                    writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
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
                _WriteComment(writer, options, InvoiceCommentConstants.ApplicableTradeTaxComment);
                writer.WriteStartElement("ram", "ApplicableTradeTax");

                writer.WriteStartElement("ram", "CalculatedAmount");
                writer.WriteValue(_formatDecimal(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount

                writer.WriteElementString("ram", "TypeCode", tax.TypeCode.EnumToString());
                _Writer.WriteOptionalElementString("ram", "ExemptionReason", tax.ExemptionReason);
                writer.WriteStartElement("ram", "BasisAmount");
                writer.WriteValue(_formatDecimal(tax.BasisAmount));
                writer.WriteEndElement(); // !BasisAmount

                if (tax.LineTotalBasisAmount.HasValue && (tax.LineTotalBasisAmount.Value != 0))
                {
                    writer.WriteStartElement("ram", "LineTotalBasisAmount", Profile.Extended);
                    writer.WriteValue(_formatDecimal(tax.LineTotalBasisAmount));
                    writer.WriteEndElement();
                }
                if (tax.AllowanceChargeBasisAmount.HasValue && (tax.AllowanceChargeBasisAmount.Value != 0))
                {
                    writer.WriteStartElement("ram", "AllowanceChargeBasisAmount", Profile.Extended);
                    writer.WriteValue(_formatDecimal(tax.AllowanceChargeBasisAmount));
                    writer.WriteEndElement(); // !AllowanceChargeBasisAmount
                }

                if (tax.CategoryCode.HasValue)
                {
                    writer.WriteElementString("ram", "CategoryCode", tax.CategoryCode.EnumToString());
                }

                if (tax.ExemptionReasonCode.HasValue)
                {
                    writer.WriteElementString("ram", "ExemptionReasonCode", tax.ExemptionReasonCode?.EnumToString());
                }

                if (tax.TaxPointDate.HasValue)
                {
                    _Writer.WriteStartElement("ram", "TaxPointDate");
                    _Writer.WriteStartElement("udt", "DateString");
                    _Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(tax.TaxPointDate.Value));
                    _Writer.WriteEndElement(); // !udt:DateString
                    _Writer.WriteEndElement(); // !TaxPointDate
                }
                if (tax.TaxPointDate.HasValue)
                {
                    _Writer.WriteElementString("ram", "DueDateTypeCode", tax.DueDateTypeCode?.EnumToString());
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


        private void _writeOptionalParty(ProfileAwareXmlTextWriter writer, string prefix, string partyTag, Party party, Contact contact = null, List<TaxRegistration> taxRegistrations = null)
        {
            if (party != null)
            {
                writer.WriteStartElement(prefix, partyTag);

                if ((party.ID != null) && !String.IsNullOrWhiteSpace(party.ID.ID))
                {
                    if (party.ID.SchemeID.HasValue && party.ID.SchemeID.HasValue)
                    {
                        writer.WriteStartElement("ram", "ID");
                        writer.WriteAttributeString("schemeID", party.ID.SchemeID.Value.EnumToString());
                        writer.WriteValue(party.ID.ID);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        writer.WriteElementString("ram", "ID", party.ID.ID);
                    }
                }

                if ((party.GlobalID != null) && !String.IsNullOrWhiteSpace(party.GlobalID.ID) && party.GlobalID.SchemeID.HasValue && party.GlobalID.SchemeID.HasValue)
                {
                    writer.WriteStartElement("ram", "GlobalID");
                    writer.WriteAttributeString("schemeID", party.GlobalID.SchemeID.Value.EnumToString());
                    writer.WriteValue(party.GlobalID.ID);
                    writer.WriteEndElement();
                }
                writer.WriteOptionalElementString("ram", "Name", party.Name);
                writer.WriteOptionalElementString("ram", "Description", party.Description, PROFILE_COMFORT_EXTENDED_XRECHNUNG); // BT-33
                _writeOptionalContact(writer, "ram", "DefinedTradeContact", contact);

                writer.WriteStartElement("ram", "PostalTradeAddress");
                writer.WriteOptionalElementString("ram", "PostcodeCode", party.Postcode);
                writer.WriteOptionalElementString("ram", "LineOne", string.IsNullOrWhiteSpace(party.ContactName) ? party.Street : party.ContactName);
                if (!string.IsNullOrWhiteSpace(party.ContactName)) { writer.WriteOptionalElementString("ram", "LineTwo", party.Street); }

                writer.WriteOptionalElementString("ram", "LineThree", party.AddressLine3); // BT-163

                writer.WriteOptionalElementString("ram", "CityName", party.City);

                if (party.Country.HasValue)
                {
                    writer.WriteElementString("ram", "CountryID", party.Country.Value.EnumToString());
                }

                writer.WriteOptionalElementString("ram", "CountrySubDivisionName", party.CountrySubdivisionName); // BT-79
                writer.WriteEndElement(); // !PostalTradeAddress

                if (taxRegistrations != null)
                {
                    foreach (TaxRegistration registration in taxRegistrations)
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
                if (!descriptor.GetTradeLineItems().All(l => !l.TaxType.HasValue || l.TaxType.Equals(TaxTypes.VAT)))
                {
                    if (throwExceptions) { throw new UnsupportedException("Tax types other than VAT only possible with extended profile."); }
                    return false;
                }
            }

            return true;
        } // !Validate()
    }
}
