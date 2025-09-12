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
using System.Net;
using System.Text;


namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor23CIIWriter : IInvoiceDescriptorWriter
    {
        private ProfileAwareXmlTextWriter _Writer;
        private InvoiceDescriptor _Descriptor;


        private readonly Profile PROFILE_COMFORT_EXTENDED_XRECHNUNG = Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung;
        private readonly Profile ALL_PROFILES = Profile.Minimum | Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung;


        /// <summary>
        /// Saves the given invoice to the given stream.
        /// Make sure that the stream is open and writeable. Otherwise, an IllegalStreamException will be thrown.
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

            #region ExchangedDocumentContext
            //Prozesssteuerung
            _Writer.WriteStartElement("rsm", "ExchangedDocumentContext");
            if (this._Descriptor.IsTest)
            {
                _Writer.WriteStartElement("ram", "TestIndicator", Profile.Extended);
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
            //Gruppierung der Anwendungsempfehlungsinformationen
            _Writer.WriteElementString("ram", "ID", this._Descriptor.Profile.EnumToString(ZUGFeRDVersion.Version23));
            _Writer.WriteEndElement(); // !ram:GuidelineSpecifiedDocumentContextParameter
            _Writer.WriteEndElement(); // !rsm:ExchangedDocumentContext
            #endregion

            #region ExchangedDocument
            //Gruppierung der Eigenschaften, die das gesamte Dokument betreffen.
            _Writer.WriteStartElement("rsm", "ExchangedDocument");
            _Writer.WriteElementString("ram", "ID", this._Descriptor.InvoiceNo); //Rechnungsnummer
            _Writer.WriteOptionalElementString("ram", "Name", this._Descriptor.Name, Profile.Extended); //Dokumentenart (Freitext), ISO 15000-5:2014, Anhang B
            _Writer.WriteElementString("ram", "TypeCode", String.Format("{0}", EnumExtensions.EnumToString<InvoiceType>(this._Descriptor.Type))); //Code für den Rechnungstyp

            if (this._Descriptor.InvoiceDate.HasValue)
            {
                _Writer.WriteStartElement("ram", "IssueDateTime");
                _Writer.WriteStartElement("udt", "DateTimeString");  //Rechnungsdatum
                _Writer.WriteAttributeString("format", "102");
                _Writer.WriteValue(_formatDate(this._Descriptor.InvoiceDate.Value));
                _Writer.WriteEndElement(); // !udt:DateTimeString
                _Writer.WriteEndElement(); // !IssueDateTime
            }

            // TODO: CopyIndicator                // BT-X-3, Kopiekennzeichen, Extended
            // TODO: LanguageID                  // BT-X-4, Sprachkennzeichen, Extended

            _writeNotes(_Writer, this._Descriptor.Notes, ALL_PROFILES ^ Profile.Minimum); // BG-1, BT-X-5, BT-22, BT-21

            // TODO: EffectiveSpecifiedPeriod    // BT-X-6, Vertragliches Fälligkeitsdatum der Rechnung, Extended

            _Writer.WriteEndElement(); // !rsm:ExchangedDocument
            #endregion

            #region SupplyChainTradeTransaction
            //Gruppierung der Informationen zum Geschäftsvorfall
            _Writer.WriteStartElement("rsm", "SupplyChainTradeTransaction");

            #region IncludedSupplyChainTradeLineItem            
            foreach (TradeLineItem tradeLineItem in this._Descriptor.GetTradeLineItems())
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.IncludedSupplyChainTradeLineItemComment);
                _Writer.WriteStartElement("ram", "IncludedSupplyChainTradeLineItem");

                #region AssociatedDocumentLineDocument
                //Gruppierung von allgemeinen Positionsangaben
                if (tradeLineItem.AssociatedDocument != null)
                {
                    _Writer.WriteStartElement("ram", "AssociatedDocumentLineDocument", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    // Kennung der Rechnungsposition, BT-126
                    if (!String.IsNullOrWhiteSpace(tradeLineItem.AssociatedDocument.LineID))
                    {
                        _Writer.WriteElementString("ram", "LineID", tradeLineItem.AssociatedDocument.LineID);
                    }
                    // ID der übergeordneten Zeile, BT-X-304, Extended
                    // It is necessary that Parent Line Id be written directly under LineId
                    if (!String.IsNullOrWhiteSpace(tradeLineItem.AssociatedDocument.ParentLineID))
                    {
                        _Writer.WriteElementString("ram", "ParentLineID", tradeLineItem.AssociatedDocument.ParentLineID, Profile.Extended);
                    }
                    // Typ der Rechnungsposition (Code), BT-X-7, Extended
                    if (tradeLineItem.AssociatedDocument.LineStatusCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "LineStatusCode", EnumExtensions.EnumToString<LineStatusCodes>(tradeLineItem.AssociatedDocument.LineStatusCode), Profile.Extended);
                    }
                    // Untertyp der Rechnungsposition, BT-X-8, Extended
                    if (tradeLineItem.AssociatedDocument.LineStatusReasonCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "LineStatusReasonCode", EnumExtensions.EnumToString<LineStatusReasonCodes>(tradeLineItem.AssociatedDocument.LineStatusReasonCode), Profile.Extended);
                    }
                    _writeNotes(_Writer, tradeLineItem.AssociatedDocument.Notes, ALL_PROFILES ^ Profile.Minimum ^ Profile.BasicWL);
                    _Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument(Basic|Comfort|Extended|XRechnung)
                }
                #endregion

                // TODO: IncludedNote            // BT-127, Detailinformationen zum Freitext zur Position, Basic+Comfort+Extended+XRechnung

                // handelt es sich um einen Kommentar?
                bool isCommentItem = false;
                if ((tradeLineItem.AssociatedDocument?.Notes.Count > 0) && (tradeLineItem.BilledQuantity == 0) && (String.IsNullOrWhiteSpace(tradeLineItem.Description)))
                {
                    isCommentItem = true;
                }

                #region SpecifiedTradeProduct
                //Eine Gruppe von betriebswirtschaftlichen Begriffen, die Informationen über die in Rechnung gestellten Waren und Dienstleistungen enthält
                _Writer.WriteStartElement("ram", "SpecifiedTradeProduct", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                if ((tradeLineItem.GlobalID != null) && (tradeLineItem.GlobalID.SchemeID.HasValue) && tradeLineItem.GlobalID.SchemeID.HasValue && !String.IsNullOrWhiteSpace(tradeLineItem.GlobalID.ID))
                {
                    _writeElementWithAttributeWithPrefix(_Writer, "ram", "GlobalID", "schemeID", tradeLineItem.GlobalID.SchemeID.Value.EnumToString(), tradeLineItem.GlobalID.ID, Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                }

                _Writer.WriteOptionalElementString("ram", "SellerAssignedID", tradeLineItem.SellerAssignedID, PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                _Writer.WriteOptionalElementString("ram", "BuyerAssignedID", tradeLineItem.BuyerAssignedID, PROFILE_COMFORT_EXTENDED_XRECHNUNG);

                // TODO: IndustryAssignedID     // BT-X-532, Von der Industrie zugewiesene Produktkennung
                // TODO: ModelID                // BT-X-533, Modelkennung des Artikels

                // BT-153
                _Writer.WriteOptionalElementString("ram", "Name", tradeLineItem.Name, Profile.Basic | Profile.Comfort | Profile.Extended);
                _Writer.WriteOptionalElementString("ram", "Name", isCommentItem ? "TEXT" : tradeLineItem.Name, Profile.XRechnung1 | Profile.XRechnung); // XRechnung erfordert einen Item-Namen (BR-25)

                _Writer.WriteOptionalElementString("ram", "Description", tradeLineItem.Description, PROFILE_COMFORT_EXTENDED_XRECHNUNG);

                // TODO: BatchID                // BT-X-534, Kennung der Charge (des Loses) des Artikels
                // TODO: BrandName              // BT-X-535, Markenname des Artikels
                // TODO: ModelName              // BT-X-536, Modellbezeichnung des Artikels

                // BG-32, Artikelattribute
                if (tradeLineItem.ApplicableProductCharacteristics?.Any() == true)
                {
                    foreach (var productCharacteristic in tradeLineItem.ApplicableProductCharacteristics)
                    {
                        _Writer.WriteStartElement("ram", "ApplicableProductCharacteristic");
                        // TODO: TypeCode        // BT-X-11, Art der Produkteigenschaft (Code), Extended
                        _Writer.WriteOptionalElementString("ram", "Description", productCharacteristic.Description);
                        // TODO: ValueMeasure    // BT-X-12, Wert der Produkteigenschaft (numerische Messgröße), mit unitCode, Extended
                        _Writer.WriteOptionalElementString("ram", "Value", productCharacteristic.Value); // BT-161
                        _Writer.WriteEndElement(); // !ram:ApplicableProductCharacteristic
                    }
                }

                foreach (var designatedProductClassification in tradeLineItem.GetDesignatedProductClassifications())
                {
                    if (designatedProductClassification.ListID == default)
                    {
                        continue;
                    }

                    _Writer.WriteStartElement("ram", "DesignatedProductClassification", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                    _Writer.WriteStartElement("ram", "ClassCode");
                    _Writer.WriteAttributeString("listID", designatedProductClassification.ListID.EnumToString());

                    if (!String.IsNullOrWhiteSpace(designatedProductClassification.ListVersionID))
                    {
                        _Writer.WriteAttributeString("listVersionID", designatedProductClassification.ListVersionID);
                    }

                    _Writer.WriteValue(designatedProductClassification.ClassCode);
                    _Writer.WriteEndElement(); // !ram::ClassCode
                    _Writer.WriteOptionalElementString("ram", "ClassName", designatedProductClassification.ClassName);
                    _Writer.WriteEndElement(); // !ram:DesignatedProductClassification
                }

                // TODO: IndividualTradeProductInstance, BG-X-84, Artikel (Handelsprodukt) Instanzen

                // BT-159, Detailinformationen zur Produktherkunft
                if (tradeLineItem.OriginTradeCountry != null)
                {
                    _Writer.WriteStartElement("ram", "OriginTradeCountry", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                    _Writer.WriteElementString("ram", "ID", tradeLineItem.OriginTradeCountry.ToString());
                    _Writer.WriteEndElement(); // !ram:OriginTradeCountry
                }

                if ((descriptor.Profile == Profile.Extended) && (tradeLineItem.IncludedReferencedProducts?.Any() == true)) // BG-X-1
                {
                    foreach (var includedItem in tradeLineItem.IncludedReferencedProducts)
                    {
                        _Writer.WriteStartElement("ram", "IncludedReferencedProduct");
                        // TODO: GlobalID, SellerAssignedID, BuyerAssignedID, IndustryAssignedID, Description
                        _Writer.WriteOptionalElementString("ram", "Name", includedItem.Name); // BT-X-18

                        if (includedItem.UnitQuantity.HasValue)
                        {
                            _writeElementWithAttributeWithPrefix(_Writer, "ram", "UnitQuantity", "unitCode", includedItem.UnitCode.Value.EnumToString(), _formatDecimal(includedItem.UnitQuantity, 4));
                        }
                        _Writer.WriteEndElement(); // !ram:IncludedReferencedProduct
                    }
                }

                _Writer.WriteEndElement(); // !ram:SpecifiedTradeProduct(Basic|Comfort|Extended|XRechnung)
                #endregion

                #region SpecifiedLineTradeAgreement (Basic, Comfort, Extended, XRechnung)
                //Eine Gruppe von betriebswirtschaftlichen Begriffen, die Informationen über den Preis für die in der betreffenden Rechnungsposition in Rechnung gestellten Waren und Dienstleistungen enthält

                if (descriptor.Profile.In(Profile.Basic, Profile.Comfort, Profile.Extended, Profile.XRechnung, Profile.XRechnung1))
                {
                    _Writer.WriteStartElement("ram", "SpecifiedLineTradeAgreement", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                    #region BuyerOrderReferencedDocument (Comfort, Extended, XRechnung)
                    // Detailangaben zur zugehörigen Bestellung
                    bool hasLineID = !string.IsNullOrWhiteSpace(tradeLineItem.BuyerOrderReferencedDocument?.LineID);
                    bool hasIssuerAssignedID = !string.IsNullOrWhiteSpace(tradeLineItem.BuyerOrderReferencedDocument?.ID);
                    bool hasIssueDateTime = tradeLineItem.BuyerOrderReferencedDocument?.IssueDateTime != null;

                    if (tradeLineItem.BuyerOrderReferencedDocument != null &&
                        (((descriptor.Profile != Profile.Extended) && hasLineID) ||
                         ((descriptor.Profile == Profile.Extended) && (hasLineID || hasIssuerAssignedID || hasIssueDateTime))))
                    {
                        _Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument", PROFILE_COMFORT_EXTENDED_XRECHNUNG);

                        //Bestellnummer
                        _Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.BuyerOrderReferencedDocument.ID, Profile.Extended);

                        // reference to the order position
                        _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.BuyerOrderReferencedDocument.LineID);

                        if (tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "FormattedIssueDateTime", Profile.Extended);
                            _Writer.WriteStartElement("qdt", "DateTimeString");
                            _Writer.WriteAttributeString("format", "102");
                            _Writer.WriteValue(_formatDate(tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.Value));
                            _Writer.WriteEndElement(); // !qdt:DateTimeString
                            _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                        }

                        _Writer.WriteEndElement(); // !ram:BuyerOrderReferencedDocument
                    }
                    #endregion

                    #region ContractReferencedDocument
                    //Detailangaben zum zugehörigen Vertrag
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
                    #endregion

                    #region AdditionalReferencedDocument (Extended)

                    //Detailangaben zu einer zusätzlichen Dokumentenreferenz
                    foreach (AdditionalReferencedDocument document in tradeLineItem.AdditionalReferencedDocuments)
                    {
                        _writeAdditionalReferencedDocument(document, Profile.Extended, "BG-X-3");
                    } // !foreach(document)
                    #endregion

                    #region GrossPriceProductTradePrice (Comfort, Extended, XRechnung)
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
                        _Writer.WriteStartElement("ram", "GrossPriceProductTradePrice", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                        _writeOptionalAdaptiveAmount(_Writer, "ram", "ChargeAmount", tradeLineItem.GrossUnitPrice, 2, 4);   // BT-148
                        if (tradeLineItem.GrossQuantity.HasValue)
                        {
                            _writeElementWithAttributeWithPrefix(_Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.GrossQuantity.Value, 4));
                        }

                        foreach (AbstractTradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.GetTradeAllowanceCharges()) // BT-147
                        {
                            _WriteItemLevelAppliedTradeAllowanceCharge(_Writer, tradeAllowanceCharge);
                        }

                        _Writer.WriteEndElement(); // ram:GrossPriceProductTradePrice(Comfort|Extended|XRechnung)
                    }
                    #endregion // !GrossPriceProductTradePrice(Comfort|Extended|XRechnung)

                    #region NetPriceProductTradePrice
                    //Im Nettopreis sind alle Zu- und Abschläge enthalten, jedoch nicht die Umsatzsteuer.
                    _WriteComment(_Writer, options, InvoiceCommentConstants.NetPriceProductTradePriceComment);
                    _Writer.WriteStartElement("ram", "NetPriceProductTradePrice", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    _writeOptionalAdaptiveAmount(_Writer, "ram", "ChargeAmount", tradeLineItem.NetUnitPrice, 2, 4); // BT-146

                    if (tradeLineItem.NetQuantity.HasValue)
                    {
                        _writeElementWithAttributeWithPrefix(_Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.NetQuantity.Value, 4));
                    }
                    _Writer.WriteEndElement(); // ram:NetPriceProductTradePrice(Basic|Comfort|Extended|XRechnung)
                    #endregion // !NetPriceProductTradePrice(Basic|Comfort|Extended|XRechnung)

                    #region UltimateCustomerOrderReferencedDocument
                    // TODO: UltimateCustomerOrderReferencedDocument
                    #endregion
                    _Writer.WriteEndElement(); // ram:SpecifiedLineTradeAgreement
                }
                #endregion

                #region SpecifiedLineTradeDelivery (Basic, Comfort, Extended)
                _Writer.WriteStartElement("ram", "SpecifiedLineTradeDelivery", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                _writeElementWithAttributeWithPrefix(_Writer, "ram", "BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));
                if (tradeLineItem.ChargeFreeQuantity.HasValue)
                {
                    _writeElementWithAttributeWithPrefix(_Writer, "ram", "ChargeFreeQuantity", "unitCode", tradeLineItem.ChargeFreeUnitCode.EnumToString(), _formatDecimal(tradeLineItem.ChargeFreeQuantity, 4), Profile.Extended);
                }
                if (tradeLineItem.PackageQuantity.HasValue)
                {
                    _writeElementWithAttributeWithPrefix(_Writer, "ram", "PackageQuantity", "unitCode", tradeLineItem.PackageUnitCode.EnumToString(), _formatDecimal(tradeLineItem.PackageQuantity, 4), Profile.Extended);
                }
                if (tradeLineItem.ShipTo != null)
                {
                    _writeOptionalParty(_Writer, PartyTypes.ShipToTradeParty, tradeLineItem.ShipTo, Profile.Extended);
                }

                if (tradeLineItem.UltimateShipTo != null)
                {
                    _writeOptionalParty(_Writer, PartyTypes.UltimateShipToTradeParty, tradeLineItem.UltimateShipTo, Profile.Extended);
                }

                if (tradeLineItem.ActualDeliveryDate.HasValue)
                {
                    _Writer.WriteStartElement("ram", "ActualDeliverySupplyChainEvent", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung)); // this violates CII-SR-170 for XRechnung 3
                    _Writer.WriteStartElement("ram", "OccurrenceDateTime");
                    _Writer.WriteStartElement("udt", "DateTimeString");
                    _Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(tradeLineItem.ActualDeliveryDate.Value));
                    _Writer.WriteEndElement(); // !udt:DateTimeString
                    _Writer.WriteEndElement(); // !OccurrenceDateTime()
                    _Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
                }

                if (tradeLineItem.DeliveryNoteReferencedDocument != null)
                {
                    _Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument", Profile.Extended); // this violates CII-SR-175 for XRechnung 3
                    _Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.DeliveryNoteReferencedDocument.ID);

                    // reference to the delivery note item
                    _Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.DeliveryNoteReferencedDocument.LineID);

                    if (tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        _Writer.WriteStartElement("qdt", "DateTimeString");
                        _Writer.WriteAttributeString("format", "102");
                        _Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                        _Writer.WriteEndElement(); // !qdt:DateTimeString
                        _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                    }

                    _Writer.WriteEndElement(); // !ram:DeliveryNoteReferencedDocument
                }

                _Writer.WriteEndElement(); // !ram:SpecifiedLineTradeDelivery
                #endregion

                #region SpecifiedLineTradeSettlement
                _Writer.WriteStartElement("ram", "SpecifiedLineTradeSettlement", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                #region ApplicableTradeTax
                _Writer.WriteStartElement("ram", "ApplicableTradeTax", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung); // BG-30

                if (tradeLineItem.TaxType.HasValue)
                {
                    _Writer.WriteElementString("ram", "TypeCode", tradeLineItem.TaxType.EnumToString()); // BT-151-0
                }

                _Writer.WriteOptionalElementString("ram", "ExemptionReason", string.IsNullOrEmpty(tradeLineItem.TaxExemptionReason) ? _TranslateTaxCategoryCode(tradeLineItem.TaxCategoryCode) : tradeLineItem.TaxExemptionReason, Profile.Extended); // BT-X-96

                if (tradeLineItem.TaxCategoryCode.HasValue)
                {
                    _Writer.WriteElementString("ram", "CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString()); // BT-151
                }

                if (tradeLineItem.TaxExemptionReasonCode.HasValue)
                {
                    _Writer.WriteOptionalElementString("ram", "ExemptionReasonCode", tradeLineItem.TaxExemptionReasonCode?.EnumToString(), Profile.Extended); // BT-X-97
                }

                if (tradeLineItem.TaxCategoryCode.HasValue && (tradeLineItem.TaxCategoryCode.Value != TaxCategoryCodes.O)) // notwendig, damit die Validierung klappt
                {
                    _Writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent)); // BT-152
                }

                _Writer.WriteEndElement(); // !ram:ApplicableTradeTax(Basic|Comfort|Extended|XRechnung)
                #endregion // !ApplicableTradeTax(Basic|Comfort|Extended|XRechnung)

                #region BillingSpecifiedPeriod
                if (tradeLineItem.BillingPeriodStart.HasValue || tradeLineItem.BillingPeriodEnd.HasValue)
                {
                    _Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", ALL_PROFILES ^ Profile.Minimum);
                    if (tradeLineItem.BillingPeriodStart.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "StartDateTime");
                        _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodStart.Value));
                        _Writer.WriteEndElement(); // !StartDateTime
                    }

                    if (tradeLineItem.BillingPeriodEnd.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "EndDateTime");
                        _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodEnd.Value));
                        _Writer.WriteEndElement(); // !EndDateTime
                    }
                    _Writer.WriteEndElement(); // !BillingSpecifiedPeriod
                }
                #endregion

                #region SpecifiedTradeAllowanceCharge (Basic, Comfort, Extended, XRechnung)
                //Abschläge auf Ebene der Rechnungsposition (Basic, Comfort, Extended, XRechnung)
                if (descriptor.Profile.In(Profile.Basic, Profile.Comfort, Profile.Extended, Profile.XRechnung1, Profile.XRechnung))
                {
                    foreach (TradeAllowance specifiedTradeAllowance in tradeLineItem.GetSpecifiedTradeAllowances()) // BG-28
                    {
                        _WriteItemLevelSpecifiedTradeAllowanceCharge(_Writer, specifiedTradeAllowance);
                    }

                    foreach (TradeCharge specifiedTradeCharge in tradeLineItem.GetSpecifiedTradeCharges()) // BG-28
                    {
                        _WriteItemLevelSpecifiedTradeAllowanceCharge(_Writer, specifiedTradeCharge);
                    }
                }
                #endregion

                #region SpecifiedTradeSettlementLineMonetarySummation (Basic, Comfort, Extended)
                //Detailinformationen zu Positionssummen
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

                _Writer.WriteStartElement("ram", "LineTotalAmount", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                _Writer.WriteValue(_formatDecimal(total));
                _Writer.WriteEndElement(); // !ram:LineTotalAmount

                // TODO: TotalAllowanceChargeAmount
                //Gesamtbetrag der Positionszu- und Abschläge
                _Writer.WriteEndElement(); // ram:SpecifiedTradeSettlementMonetarySummation
                #endregion                

                #region AdditionalReferencedDocument
                //Objektkennung auf Ebene der Rechnungsposition, BT-128-00
                if (tradeLineItem.GetAdditionalReferencedDocuments().Count > 0)
                {
                    foreach (var document in tradeLineItem.GetAdditionalReferencedDocuments()
                                                          .Where(x => x.TypeCode == AdditionalReferencedDocumentTypeCode.InvoiceDataSheet))  // PEPPOL-EN16931-R101: Element Document reference can only be used for Invoice line object
                    {
                        if (string.IsNullOrWhiteSpace(document.ID))
                        {
                            continue;
                        }

                        _writeAdditionalReferencedDocument(document, PROFILE_COMFORT_EXTENDED_XRECHNUNG, "BT-128-00");
                        // only Extended allows multiple entries
                        if (this._Descriptor.Profile != Profile.Extended)
                        {
                            break;
                        }
                    }
                }
                #endregion

                #region ReceivableSpecifiedTradeAccountingAccount
                // Detailinformationen zur Buchungsreferenz, BT-133-00
                if (tradeLineItem.ReceivableSpecifiedTradeAccountingAccounts?.Any() == true)
                {
                    foreach (var traceAccountingAccount in tradeLineItem.ReceivableSpecifiedTradeAccountingAccounts)
                    {
                        if (string.IsNullOrWhiteSpace(traceAccountingAccount.TradeAccountID))
                        {
                            continue;
                        }

                        _Writer.WriteStartElement("ram", "ReceivableSpecifiedTradeAccountingAccount", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                        _Writer.WriteStartElement("ram", "ID");
                        _Writer.WriteValue(traceAccountingAccount.TradeAccountID); // BT-133
                        _Writer.WriteEndElement(); // !ram:ID

                        if (traceAccountingAccount.TradeAccountTypeCode.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "TypeCode", Profile.Extended);
                            _Writer.WriteValue(((int)traceAccountingAccount.TradeAccountTypeCode.Value).ToString()); // BT-X-99
                            _Writer.WriteEndElement(); // !ram:TypeCode
                        }

                        _Writer.WriteEndElement(); // !ram:ReceivableSpecifiedTradeAccountingAccount

                        // Only Extended allows multiple accounts per line item, otherwise break
                        if (descriptor.Profile != Profile.Extended)
                        {
                            break;
                        }
                    }
                }
                #endregion

                _Writer.WriteEndElement(); // !ram:SpecifiedLineTradeSettlement
                #endregion

                _Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItemComment
            } // !foreach(tradeLineItem)
            #endregion

            #region ApplicableHeaderTradeAgreement
            _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableHeaderTradeAgreementComment);
            _Writer.WriteStartElement("ram", "ApplicableHeaderTradeAgreement");

            #region BuyerReference
            // BT-10
            _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerReferenceComment);
            _Writer.WriteOptionalElementString("ram", "BuyerReference", this._Descriptor.ReferenceOrderNo);
            #endregion

            #region SellerTradeParty
            // BT-31: this._Descriptor.SellerTaxRegistration
            _WriteComment(_Writer, options, InvoiceCommentConstants.SellerTradePartyComment);
            _writeOptionalParty(_Writer, PartyTypes.SellerTradeParty, this._Descriptor.Seller, ALL_PROFILES, this._Descriptor.SellerContact, this._Descriptor.SellerElectronicAddress, this._Descriptor.SellerTaxRegistration);
            #endregion

            #region BuyerTradeParty
            // BT-48: this._Descriptor.BuyerTaxRegistration
            _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerTradePartyComment);
            _writeOptionalParty(_Writer, PartyTypes.BuyerTradeParty, this._Descriptor.Buyer, ALL_PROFILES, this._Descriptor.BuyerContact, this._Descriptor.BuyerElectronicAddress, this._Descriptor.BuyerTaxRegistration);
            #endregion

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
            // BG-11 (SellerTaxRepresentativeTradeParty)
            _writeOptionalParty(_Writer, PartyTypes.SellerTaxRepresentativeTradeParty, this._Descriptor.SellerTaxRepresentative, ALL_PROFILES, null, null, this._Descriptor.SellerTaxRepresentativeTaxRegistration);
            #endregion

            #region 1. SellerOrderReferencedDocument (BT-14-00: Comfort+)
            if (!string.IsNullOrWhiteSpace(_Descriptor.SellerOrderReferencedDocument?.ID))
            {
                _Writer.WriteStartElement("ram", "SellerOrderReferencedDocument", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                _Writer.WriteElementString("ram", "IssuerAssignedID", this._Descriptor.SellerOrderReferencedDocument.ID); // BT-14
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

            #region 2. BuyerOrderReferencedDocument
            if (!String.IsNullOrWhiteSpace(this._Descriptor.OrderNo))
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerOrderReferencedDocumentComment);
                _Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument");
                _Writer.WriteElementString("ram", "IssuerAssignedID", this._Descriptor.OrderNo);
                if (this._Descriptor.OrderDate.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime", Profile.Extended);
                    _Writer.WriteStartElement("qdt", "DateTimeString");
                    _Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(this._Descriptor.OrderDate.Value));
                    _Writer.WriteEndElement(); // !qdt:DateTimeString
                    _Writer.WriteEndElement(); // !IssueDateTime()
                }

                _Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }
            #endregion

            #region 3. ContractReferencedDocument
            // BT-12
            if (this._Descriptor.ContractReferencedDocument != null)
            {
                _Writer.WriteStartElement("ram", "ContractReferencedDocument");
                _Writer.WriteElementString("ram", "IssuerAssignedID", this._Descriptor.ContractReferencedDocument.ID);
                if (this._Descriptor.ContractReferencedDocument.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung));
                    _Writer.WriteStartElement("qdt", "DateTimeString");
                    _Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(this._Descriptor.ContractReferencedDocument.IssueDateTime.Value));
                    _Writer.WriteEndElement(); // !qdt:DateTimeString
                    _Writer.WriteEndElement(); // !IssueDateTime()
                }

                _Writer.WriteEndElement(); // !ram:ContractReferencedDocument
            }
            #endregion

            #region 4. AdditionalReferencedDocument
            if (this._Descriptor.AdditionalReferencedDocuments != null) // BG-24 | BT-18-00
            {
                foreach (var document in this._Descriptor.AdditionalReferencedDocuments)
                {
                    _writeAdditionalReferencedDocument(document, PROFILE_COMFORT_EXTENDED_XRECHNUNG,
                        document.ReferenceTypeCode.HasValue ? "BT-18-00" : "BG-24");
                }
            }
            #endregion

            #region SpecifiedProcuringProject
            if (_Descriptor.SpecifiedProcuringProject != null)
            {

                _Writer.WriteStartElement("ram", "SpecifiedProcuringProject", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                _Writer.WriteElementString("ram", "ID", _Descriptor.SpecifiedProcuringProject.ID, PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                _Writer.WriteElementString("ram", "Name", _Descriptor.SpecifiedProcuringProject.Name, PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                _Writer.WriteEndElement(); // !ram:SpecifiedProcuringProject
            }
            #endregion

            _Writer.WriteEndElement(); // !ApplicableHeaderTradeAgreement
            #endregion

            #region ApplicableHeaderTradeDelivery
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

            _writeOptionalParty(_Writer, PartyTypes.ShipToTradeParty, this._Descriptor.ShipTo, ALL_PROFILES ^ Profile.Minimum, this._Descriptor.ShipToContact, default, this._Descriptor.GetShipToTaxRegistration());
            _writeOptionalParty(_Writer, PartyTypes.UltimateShipToTradeParty, this._Descriptor.UltimateShipTo, Profile.Extended | Profile.XRechnung1 | Profile.XRechnung, this._Descriptor.UltimateShipToContact);
            _writeOptionalParty(_Writer, PartyTypes.ShipFromTradeParty, this._Descriptor.ShipFrom, Profile.Extended);

            #region ActualDeliverySupplyChainEvent
            if (this._Descriptor.ActualDeliveryDate.HasValue)
            {
                _Writer.WriteStartElement("ram", "ActualDeliverySupplyChainEvent");
                _Writer.WriteStartElement("ram", "OccurrenceDateTime");
                _Writer.WriteStartElement("udt", "DateTimeString");
                _Writer.WriteAttributeString("format", "102");
                _Writer.WriteValue(_formatDate(this._Descriptor.ActualDeliveryDate.Value));
                _Writer.WriteEndElement(); // "udt:DateTimeString
                _Writer.WriteEndElement(); // !OccurrenceDateTime()
                _Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
            }
            #endregion

            #region DespatchAdviceReferencedDocument
            if (this._Descriptor.DespatchAdviceReferencedDocument != null)
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.DespatchAdviceReferencedDocumentComment);
                _Writer.WriteStartElement("ram", "DespatchAdviceReferencedDocument", Profile.Extended);
                _Writer.WriteElementString("ram", "IssuerAssignedID", this._Descriptor.DespatchAdviceReferencedDocument.ID);

                if (this._Descriptor.DespatchAdviceReferencedDocument.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime", Profile.Extended);
                    _Writer.WriteStartElement("qdt", "DateTimeString");
                    _Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(this._Descriptor.DespatchAdviceReferencedDocument.IssueDateTime.Value));
                    _Writer.WriteEndElement(); // "qdt:DateTimeString
                    _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }

                _Writer.WriteEndElement(); // !DespatchAdviceReferencedDocument
            }
            #endregion

            #region DeliveryNoteReferencedDocument
            if (this._Descriptor.DeliveryNoteReferencedDocument != null)
            {
                _Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument", Profile.Extended);
                _Writer.WriteElementString("ram", "IssuerAssignedID", this._Descriptor.DeliveryNoteReferencedDocument.ID);
                // TODO: LineID, Lieferscheinposition, BT-X-93

                if (this._Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung));
                    _Writer.WriteStartElement("qdt", "DateTimeString");
                    _Writer.WriteAttributeString("format", "102");
                    _Writer.WriteValue(_formatDate(this._Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                    _Writer.WriteEndElement(); // !qdt:DateTimeString
                    _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }

                _Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }
            #endregion

            _Writer.WriteEndElement(); // !ApplicableHeaderTradeDelivery
            #endregion

            #region ApplicableHeaderTradeSettlement            
            _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableHeaderTradeSettlementComment);
            _Writer.WriteStartElement("ram", "ApplicableHeaderTradeSettlement");
            // order of sub-elements of ApplicableHeaderTradeSettlement:
            //   1. CreditorReferenceID (BT-90) is only required/allowed on DirectDebit (BR-DE-30)
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

            //   1. CreditorReferenceID (BT-90) is only required/allowed on DirectDebit (BR-DE-30)
            if ((this._Descriptor.PaymentMeans?.TypeCode == PaymentMeansTypeCodes.DirectDebit || this._Descriptor.PaymentMeans?.TypeCode == PaymentMeansTypeCodes.SEPADirectDebit) &&
                !String.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans?.SEPACreditorIdentifier))
            {
                _Writer.WriteElementString("ram", "CreditorReferenceID", _Descriptor.PaymentMeans?.SEPACreditorIdentifier, ALL_PROFILES ^ Profile.Minimum);
            }

            //   2. PaymentReference (optional), Verwendungszweck, BT-83
            _Writer.WriteOptionalElementString("ram", "PaymentReference", this._Descriptor.PaymentReference, ALL_PROFILES ^ Profile.Minimum);

            //   3. TaxCurrencyCode (optional)
            //   BT-6
            if (this._Descriptor.TaxCurrency.HasValue)
            {
                _Writer.WriteElementString("ram", "TaxCurrencyCode", this._Descriptor.TaxCurrency.Value.EnumToString(), profile: Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
            }

            //   4. InvoiceCurrencyCode (optional), BT-5
            _Writer.WriteElementString("ram", "InvoiceCurrencyCode", this._Descriptor.Currency.EnumToString());

            //   5. InvoiceIssuerReference (optional), BT-X-204
            _Writer.WriteOptionalElementString("ram", "InvoiceIssuerReference", this._Descriptor.SellerReferenceNo, Profile.Extended);

            //   6. InvoicerTradeParty (optional), BG-X-33
            _writeOptionalParty(_Writer, PartyTypes.InvoicerTradeParty, this._Descriptor.Invoicer, Profile.Extended);

            //   7. InvoiceeTradeParty (optional), BG-X-36
            _writeOptionalParty(_Writer, PartyTypes.InvoiceeTradeParty, this._Descriptor.Invoicee, Profile.Extended, default, default, this._Descriptor.GetInvoiceeTaxRegistration());

            //   8. PayeeTradeParty (optional), BG-10
            _writeOptionalParty(_Writer, PartyTypes.PayeeTradeParty, this._Descriptor.Payee, ALL_PROFILES ^ Profile.Minimum);

            #region SpecifiedTradeSettlementPaymentMeans
            //  10. SpecifiedTradeSettlementPaymentMeans (optional), BG-16

            if (!this._Descriptor.AnyCreditorFinancialAccount() && !this._Descriptor.AnyDebitorFinancialAccount())
            {
                if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementPaymentMeansComment);
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans", ALL_PROFILES ^ Profile.Minimum); // BG-16
                    _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString(), ALL_PROFILES ^ Profile.Minimum);
                    _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information, PROFILE_COMFORT_EXTENDED_XRECHNUNG);

                    if (!string.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans.FinancialCard?.Id)) // BG-18
                    {
                        _Writer.WriteStartElement("ram", "ApplicableTradeSettlementFinancialCard", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                        _Writer.WriteElementString("ram", "ID", _Descriptor.PaymentMeans.FinancialCard.Id); // BT-87
                        _Writer.WriteOptionalElementString("ram", "CardholderName", _Descriptor.PaymentMeans.FinancialCard.CardholderName); // BT-88
                        _Writer.WriteEndElement(); // !ram:ApplicableTradeSettlementFinancialCard
                    }
                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }
            else
            {
                foreach (BankAccount account in this._Descriptor.GetCreditorFinancialAccounts())
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementPaymentMeansComment);
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans", ALL_PROFILES ^ Profile.Minimum);

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString(), ALL_PROFILES ^ Profile.Minimum);
                        _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information, PROFILE_COMFORT_EXTENDED_XRECHNUNG);

                        if (this._Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            _Writer.WriteStartElement("ram", "ApplicableTradeSettlementFinancialCard", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                            _Writer.WriteOptionalElementString("ram", "ID", _Descriptor.PaymentMeans.FinancialCard.Id);
                            _Writer.WriteOptionalElementString("ram", "CardholderName", _Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            _Writer.WriteEndElement(); // !ram:ApplicableTradeSettlementFinancialCard
                        }
                    }

                    _Writer.WriteStartElement("ram", "PayeePartyCreditorFinancialAccount", ALL_PROFILES ^ Profile.Minimum);
                    _Writer.WriteElementString("ram", "IBANID", account.IBAN);
                    _Writer.WriteOptionalElementString("ram", "AccountName", account.Name, PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                    _Writer.WriteOptionalElementString("ram", "ProprietaryID", account.ID);
                    _Writer.WriteEndElement(); // !PayeePartyCreditorFinancialAccount

                    if (!String.IsNullOrWhiteSpace(account.BIC))
                    {
                        _Writer.WriteStartElement("ram", "PayeeSpecifiedCreditorFinancialInstitution", PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                        _Writer.WriteElementString("ram", "BICID", account.BIC);
                        _Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                    }

                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }

                foreach (BankAccount account in this._Descriptor.GetDebitorFinancialAccounts())
                {
                    _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans", ALL_PROFILES ^ Profile.Minimum); // BG-16

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("ram", "TypeCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString(), ALL_PROFILES ^ Profile.Minimum);
                        _Writer.WriteOptionalElementString("ram", "Information", this._Descriptor.PaymentMeans.Information, PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                    }

                    _Writer.WriteStartElement("ram", "PayerPartyDebtorFinancialAccount", ALL_PROFILES ^ Profile.Minimum);
                    _Writer.WriteElementString("ram", "IBANID", account.IBAN);
                    _Writer.WriteOptionalElementString("ram", "AccountName", account.Name, PROFILE_COMFORT_EXTENDED_XRECHNUNG);
                    _Writer.WriteOptionalElementString("ram", "ProprietaryID", account.ID);
                    _Writer.WriteEndElement(); // !PayerPartyDebtorFinancialAccount

                    _Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }
            #endregion

            #region ApplicableTradeTax
            //  11. ApplicableTradeTax (optional)            
            _writeOptionalTaxes(_Writer, options);
            #endregion

            #region BillingSpecifiedPeriod
            //  12. BillingSpecifiedPeriod (optional)
            if (_Descriptor.BillingPeriodStart.HasValue || _Descriptor.BillingPeriodEnd.HasValue)
            {
                _Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", ALL_PROFILES ^ Profile.Minimum);
                if (_Descriptor.BillingPeriodStart.HasValue)
                {
                    _Writer.WriteStartElement("ram", "StartDateTime");
                    _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(this._Descriptor.BillingPeriodStart.Value));
                    _Writer.WriteEndElement(); // !StartDateTime
                }

                if (_Descriptor.BillingPeriodEnd.HasValue)
                {
                    _Writer.WriteStartElement("ram", "EndDateTime");
                    _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(this._Descriptor.BillingPeriodEnd.Value));
                    _Writer.WriteEndElement(); // !EndDateTime
                }
                _Writer.WriteEndElement(); // !BillingSpecifiedPeriod
            }
            #endregion

            //  13. SpecifiedTradeAllowanceCharge (optional)
            foreach (TradeAllowance tradeAllowance in this._Descriptor.GetTradeAllowances())
            {
                _WriteDocumentLevelSpecifiedTradeAllowanceCharge(_Writer, tradeAllowance);
            }

            foreach (TradeCharge tradeCharge in this._Descriptor.GetTradeCharges())
            {
                _WriteDocumentLevelSpecifiedTradeAllowanceCharge(_Writer, tradeCharge);
            }

            //  14. SpecifiedLogisticsServiceCharge (optional)
            foreach (ServiceCharge serviceCharge in this._Descriptor.GetLogisticsServiceCharges())
            {
                _Writer.WriteStartElement("ram", "SpecifiedLogisticsServiceCharge", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung));
                _Writer.WriteOptionalElementString("ram", "Description", serviceCharge.Description);
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

                            // every line break must be a valid xml line break.
                            // if a note already exists, append a valid line break.
                            if (sbPaymentNotes.Length > 0)
                            {
                                sbPaymentNotes.Append("\n");
                            }

                            if (paymentTerms.PaymentTermsType.HasValue)
                            {
                                // also write the descriptions if it exists.
                                if (!string.IsNullOrWhiteSpace(paymentTerms.Description))
                                {
                                    sbPaymentNotes.Append(paymentTerms.Description);
                                    sbPaymentNotes.Append("\n");
                                }

                                if (paymentTerms.PaymentTermsType.HasValue && paymentTerms.DueDays.HasValue && paymentTerms.Percentage.HasValue)
                                {
                                    sbPaymentNotes.Append($"#{((PaymentTermsType)paymentTerms.PaymentTermsType).EnumToString<PaymentTermsType>().ToUpper()}");
                                    sbPaymentNotes.Append($"#TAGE={paymentTerms.DueDays}");
                                    sbPaymentNotes.Append($"#PROZENT={_formatDecimal(paymentTerms.Percentage)}");
                                    sbPaymentNotes.Append(paymentTerms.BaseAmount.HasValue ? $"#BASISBETRAG={_formatDecimal(paymentTerms.BaseAmount)}" : "");
                                    sbPaymentNotes.Append("#");
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(paymentTerms.Description))
                                {
                                    sbPaymentNotes.Append(paymentTerms.Description.Trim());
                                }
                            }
                            dueDate = dueDate ?? paymentTerms.DueDate;
                        }

                        _Writer.WriteStartElement("ram", "Description");
                        _Writer.WriteRawString(sbPaymentNotes.ToString().TrimEnd()); // BT-20
                        _Writer.WriteRawString("\n");
                        _Writer.WriteEndElement(); // !ram:Description
                        if (dueDate.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(dueDate.Value));
                            _Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }

                        // BT-89 is only required/allowed on DirectDebit (BR-DE-29)
                        if (this._Descriptor.PaymentMeans?.TypeCode == PaymentMeansTypeCodes.DirectDebit || this._Descriptor.PaymentMeans?.TypeCode == PaymentMeansTypeCodes.SEPADirectDebit)
                        {
                            _Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", _Descriptor.PaymentMeans?.SEPAMandateReference);
                        }

                        _Writer.WriteEndElement(); // !ram:SpecifiedTradePaymentTerms
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
                            _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(paymentTerms.DueDate.Value));
                            _Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        _Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", _Descriptor.PaymentMeans?.SEPAMandateReference);
                        if (paymentTerms.PaymentTermsType.HasValue)
                        {
                            if (paymentTerms.PaymentTermsType == PaymentTermsType.Skonto)
                            {
                                _Writer.WriteStartElement("ram", "ApplicableTradePaymentDiscountTerms");
                                if (paymentTerms.MaturityDate.HasValue)
                                {
                                    _Writer.WriteStartElement("ram", "BasisDateTime");
                                    _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(paymentTerms.MaturityDate.Value));
                                    _Writer.WriteEndElement(); // !ram:BasisDateTime
                                }
                                if (paymentTerms.DueDays.HasValue)
                                {
                                    _writeElementWithAttribute(_Writer, "ram", "BasisPeriodMeasure", "unitCode", "DAY", paymentTerms.DueDays.Value.ToString());
                                }
                                _writeOptionalAmount(_Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                _Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                _writeOptionalAmount(_Writer, "ram", "ActualDiscountAmount", paymentTerms.ActualAmount, forceCurrency: false);
                                _Writer.WriteEndElement(); // !ram:ApplicableTradePaymentDiscountTerms
                            }
                            if (paymentTerms.PaymentTermsType == PaymentTermsType.Verzug)
                            {
                                _Writer.WriteStartElement("ram", "ApplicableTradePaymentPenaltyTerms");
                                if (paymentTerms.MaturityDate.HasValue)
                                {
                                    _Writer.WriteStartElement("ram", "BasisDateTime");
                                    _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(paymentTerms.MaturityDate.Value));
                                    _Writer.WriteEndElement(); // !ram:BasisDateTime
                                }
                                if (paymentTerms.DueDays.HasValue)
                                {
                                    _writeElementWithAttribute(_Writer, "ram", "BasisPeriodMeasure", "unitCode", "DAY", paymentTerms.DueDays.Value.ToString());
                                }
                                _writeOptionalAmount(_Writer, "ram", "BasisAmount", paymentTerms.BaseAmount, forceCurrency: false);
                                _Writer.WriteOptionalElementString("ram", "CalculationPercent", _formatDecimal(paymentTerms.Percentage));
                                _writeOptionalAmount(_Writer, "ram", "ActualPenaltyAmount", paymentTerms.ActualAmount, forceCurrency: false);
                                _Writer.WriteEndElement(); // !ram:ApplicableTradePaymentPenaltyTerms
                            }
                        }
                        _Writer.WriteEndElement(); // !ram:SpecifiedTradePaymentTerms
                    }
                    if (this._Descriptor.GetTradePaymentTerms().Count == 0 && !string.IsNullOrWhiteSpace(_Descriptor.PaymentMeans?.SEPAMandateReference))
                    {
                        _Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        _Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", _Descriptor.PaymentMeans?.SEPAMandateReference);
                        _Writer.WriteEndElement();
                    }
                    break;
                default:
                    foreach (PaymentTerms paymentTerms in this._Descriptor.GetTradePaymentTerms())
                    {
                        _Writer.WriteStartElement("ram", "SpecifiedTradePaymentTerms");
                        _Writer.WriteOptionalElementString("ram", "Description", paymentTerms.Description, ALL_PROFILES ^ Profile.Minimum);
                        if (paymentTerms.DueDate.HasValue)
                        {
                            _Writer.WriteStartElement("ram", "DueDateDateTime", ALL_PROFILES ^ Profile.Minimum);
                            _writeElementWithAttributeWithPrefix(_Writer, "udt", "DateTimeString", "format", "102", _formatDate(paymentTerms.DueDate.Value));
                            _Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        _Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", _Descriptor.PaymentMeans?.SEPAMandateReference, ALL_PROFILES ^ Profile.Minimum);
                        _Writer.WriteEndElement(); // !ram:SpecifiedTradePaymentTerms
                    }
                    break;
            }

            #region SpecifiedTradeSettlementHeaderMonetarySummation
            //Gesamtsummen auf Dokumentenebene
            _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementHeaderMonetarySummationComment);
            _Writer.WriteStartElement("ram", "SpecifiedTradeSettlementHeaderMonetarySummation");
            _writeAmount(_Writer, "ram", "LineTotalAmount", this._Descriptor.LineTotalAmount, profile: ALL_PROFILES ^ Profile.Minimum);   // Summe der Nettobeträge aller Rechnungspositionen
            _writeOptionalAmount(_Writer, "ram", "ChargeTotalAmount", this._Descriptor.ChargeTotalAmount, profile: ALL_PROFILES ^ Profile.Minimum);       // Summe der Zuschläge auf Dokumentenebene, BT-108
            _writeOptionalAmount(_Writer, "ram", "AllowanceTotalAmount", this._Descriptor.AllowanceTotalAmount, profile: ALL_PROFILES ^ Profile.Minimum); // Summe der Abschläge auf Dokumentenebene, BT-107
                                                                                                                                                // both fields are mandatory according to BR-FXEXT-CO-11
                                                                                                                                                // and BR-FXEXT-CO-12

            if (this._Descriptor.Profile == Profile.Extended)
            {
                // there shall be no currency for tax basis total amount, see
                // https://github.com/stephanstapel/ZUGFeRD-csharp/issues/56#issuecomment-655525467
                _writeOptionalAmount(_Writer, "ram", "TaxBasisTotalAmount", this._Descriptor.TaxBasisAmount, forceCurrency: false);   // Rechnungsgesamtbetrag ohne Umsatzsteuer
            }
            else
            {
                _writeOptionalAmount(_Writer, "ram", "TaxBasisTotalAmount", this._Descriptor.TaxBasisAmount);   // Rechnungsgesamtbetrag ohne Umsatzsteuer
            }
            _writeOptionalAmount(_Writer, "ram", "TaxTotalAmount", this._Descriptor.TaxTotalAmount, forceCurrency: true);               // Gesamtbetrag der Rechnungsumsatzsteuer, Steuergesamtbetrag in Buchungswährung
            _writeOptionalAmount(_Writer, "ram", "RoundingAmount", this._Descriptor.RoundingAmount, profile: Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);  // RoundingAmount  //Rundungsbetrag
            _writeOptionalAmount(_Writer, "ram", "GrandTotalAmount", this._Descriptor.GrandTotalAmount);                                // Rechnungsgesamtbetrag einschließlich Umsatzsteuer
            _writeOptionalAmount(_Writer, "ram", "TotalPrepaidAmount", this._Descriptor.TotalPrepaidAmount);                            // Vorauszahlungsbetrag
            _writeOptionalAmount(_Writer, "ram", "DuePayableAmount", this._Descriptor.DuePayableAmount);                                // Fälliger Zahlungsbetrag
            _Writer.WriteEndElement(); // !ram:SpecifiedTradeSettlementMonetarySummation
            #endregion

            #region InvoiceReferencedDocument
            foreach (InvoiceReferencedDocument invoiceReferencedDocument in this._Descriptor.GetInvoiceReferencedDocuments())
            {
                _Writer.WriteStartElement("ram", "InvoiceReferencedDocument", ALL_PROFILES ^ Profile.Minimum);
                _Writer.WriteOptionalElementString("ram", "IssuerAssignedID", invoiceReferencedDocument.ID);
                _Writer.WriteOptionalElementString("ram", "TypeCode", EnumExtensions.EnumToString(invoiceReferencedDocument.TypeCode), profile: Profile.Extended); // BT-X-332 
                if (invoiceReferencedDocument.IssueDateTime.HasValue)
                {
                    _Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    _writeElementWithAttributeWithPrefix(_Writer, "qdt", "DateTimeString", "format", "102", _formatDate(invoiceReferencedDocument.IssueDateTime.Value));
                    _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }
                _Writer.WriteEndElement(); // !ram:InvoiceReferencedDocument
            }
            #endregion

            #region ReceivableSpecifiedTradeAccountingAccount
            // Detailinformationen zur Buchungsreferenz, BT-19-00
            if (this._Descriptor.AnyReceivableSpecifiedTradeAccountingAccounts())
            {
                foreach (var traceAccountingAccount in this._Descriptor.GetReceivableSpecifiedTradeAccountingAccounts())
                {
                    if (string.IsNullOrWhiteSpace(traceAccountingAccount.TradeAccountID))
                    {
                        continue;
                    }

                    _Writer.WriteStartElement("ram", "ReceivableSpecifiedTradeAccountingAccount", ALL_PROFILES ^ Profile.Minimum);
                    _Writer.WriteStartElement("ram", "ID");
                    _Writer.WriteValue(traceAccountingAccount.TradeAccountID); // BT-19
                    _Writer.WriteEndElement(); // !ram:ID

                    if (traceAccountingAccount.TradeAccountTypeCode.HasValue)
                    {
                        _Writer.WriteStartElement("ram", "TypeCode", Profile.Extended);
                        _Writer.WriteValue(((int)traceAccountingAccount.TradeAccountTypeCode.Value).ToString()); // BT-X-290
                        _Writer.WriteEndElement(); // !ram:TypeCode
                    }

                    _Writer.WriteEndElement(); // !ram:ReceivableSpecifiedTradeAccountingAccount

                    // Only BasicWL and Extended allow multiple accounts
                    if (!this._Descriptor.Profile.In(Profile.BasicWL, Profile.Extended))
                    {
                        break;
                    }
                }
            }

            // TODO: SpecifiedAdvancePayment (0..unbounded), BG-X-45

            #endregion
            _Writer.WriteEndElement(); // !ram:ApplicableHeaderTradeSettlement

            #endregion

            _Writer.WriteEndElement(); // !ram:SupplyChainTradeTransaction
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

            writer.WriteStartElement("ram", "SpecifiedTradeAllowanceCharge", ALL_PROFILES ^ Profile.Minimum);
            writer.WriteStartElement("ram", "ChargeIndicator"); // BG-21-0
            writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false"); // BG-21-1
            writer.WriteEndElement(); // !ram:ChargeIndicator

            // TODO: SequenceNumeric, BT-X-268, Berechnungsreihenfolge

            if (tradeAllowanceCharge.ChargePercentage.HasValue)
            {
                writer.WriteStartElement("ram", "CalculationPercent"); // BT-101
                writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ChargePercentage.Value));
                writer.WriteEndElement();
            }

            if (tradeAllowanceCharge.BasisAmount.HasValue)
            {
                writer.WriteStartElement("ram", "BasisAmount"); // BT-100
                writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value));
                writer.WriteEndElement();
            }

            // TODO: BasisQuantity (+unitCode), BT-X-269, Basismenge des Rabatts

            writer.WriteStartElement("ram", "ActualAmount"); // BT-99
            writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 2));
            writer.WriteEndElement();

            if ((tradeAllowanceCharge is TradeAllowance allowance) && (allowance.ReasonCode != null))
            {
                writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<AllowanceReasonCodes>(allowance.ReasonCode)); // BT-98
            }
            else if ((tradeAllowanceCharge is TradeCharge charge) && (charge.ReasonCode != null))
            {
                writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<ChargeReasonCodes>(charge.ReasonCode));
            }

            writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason); // BT-97

            if (tradeAllowanceCharge.Tax != null)
            {
                writer.WriteStartElement("ram", "CategoryTradeTax");

                if (tradeAllowanceCharge.Tax.TypeCode.HasValue)
                {
                    writer.WriteElementString("ram", "TypeCode", tradeAllowanceCharge.Tax.TypeCode.EnumToString());
                }

                if (tradeAllowanceCharge.Tax.CategoryCode.HasValue)
                {
                    writer.WriteElementString("ram", "CategoryCode", tradeAllowanceCharge.Tax.CategoryCode.EnumToString());
                }

                writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(tradeAllowanceCharge.Tax.Percent));
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        } // !_WriteDocumentLevelSpecifiedTradeAllowanceCharge()


        private void _WriteItemLevelSpecifiedTradeAllowanceCharge(ProfileAwareXmlTextWriter writer, AbstractTradeAllowanceCharge specifiedTradeAllowanceCharge)
        {
            if (specifiedTradeAllowanceCharge == null)
            {
                return;
            }

            _Writer.WriteStartElement("ram", "SpecifiedTradeAllowanceCharge");
            #region ChargeIndicator
            _Writer.WriteStartElement("ram", "ChargeIndicator"); // BG-28-0
            _Writer.WriteElementString("udt", "Indicator", specifiedTradeAllowanceCharge.ChargeIndicator ? "true" : "false"); // BG-28-1
            _Writer.WriteEndElement(); // !ram:ChargeIndicator
            #endregion

            #region ChargePercentage
            if (specifiedTradeAllowanceCharge.ChargePercentage.HasValue)
            {
                _Writer.WriteStartElement("ram", "CalculationPercent"); // BT-138, BT-143
                _Writer.WriteValue(_formatDecimal(specifiedTradeAllowanceCharge.ChargePercentage.Value, 2));
                _Writer.WriteEndElement();
            }
            #endregion

            #region BasisAmount
            if (specifiedTradeAllowanceCharge.BasisAmount.HasValue)
            {
                _Writer.WriteStartElement("ram", "BasisAmount", ALL_PROFILES ^ Profile.Basic); // BT-137, BT-142
                _Writer.WriteValue(_formatDecimal(specifiedTradeAllowanceCharge.BasisAmount.Value, 2));
                _Writer.WriteEndElement();
            }
            #endregion

            #region ActualAmount
            _Writer.WriteStartElement("ram", "ActualAmount");
            _Writer.WriteValue(_formatDecimal(specifiedTradeAllowanceCharge.ActualAmount, 2));
            _Writer.WriteEndElement();
            #endregion

            if ((specifiedTradeAllowanceCharge is TradeAllowance allowance) && (allowance.ReasonCode != null))
            {
                _Writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<AllowanceReasonCodes>(allowance.ReasonCode)); // BT-140
            }
            else if ((specifiedTradeAllowanceCharge is TradeCharge charge) && (charge.ReasonCode != null))
            {
                _Writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<ChargeReasonCodes>(charge.ReasonCode)); // BT-145
            }

            _Writer.WriteOptionalElementString("ram", "Reason", specifiedTradeAllowanceCharge.Reason); // BT-139, BT-144

            _Writer.WriteEndElement(); // !ram:SpecifiedTradeAllowanceCharge
        } // !_WriteItemLevelSpecifiedTradeAllowanceCharge()


        private void _WriteItemLevelAppliedTradeAllowanceCharge(ProfileAwareXmlTextWriter writer, AbstractTradeAllowanceCharge tradeAllowanceCharge)
        {
            if (tradeAllowanceCharge == null)
            {
                return;
            }

            _Writer.WriteStartElement("ram", "AppliedTradeAllowanceCharge");

            #region ChargeIndicator
            _Writer.WriteStartElement("ram", "ChargeIndicator");
            _Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
            _Writer.WriteEndElement(); // !ram:ChargeIndicator
            #endregion

            #region ChargePercentage
            if (tradeAllowanceCharge.ChargePercentage.HasValue)
            {
                _Writer.WriteStartElement("ram", "CalculationPercent", profile: Profile.Extended); // not in XRechnung, according to CII-SR-122
                _writeOptionalAdaptiveValue(writer, tradeAllowanceCharge.ChargePercentage.Value, 2, 4); // BT-X-34
                _Writer.WriteEndElement();
            }
            #endregion

            #region BasisAmount
            if (tradeAllowanceCharge.BasisAmount.HasValue)
            {
                _Writer.WriteStartElement("ram", "BasisAmount", profile: Profile.Extended); // not in XRechnung, according to CII-SR-123
                _writeOptionalAdaptiveValue(writer, tradeAllowanceCharge.BasisAmount.Value, 2, 4); // BT-X-35
                _Writer.WriteEndElement();
            }
            #endregion

            #region ActualAmount
            _Writer.WriteStartElement("ram", "ActualAmount");
            _writeOptionalAdaptiveValue(writer, tradeAllowanceCharge.ActualAmount, 2, 4); // BT-147
            _Writer.WriteEndElement();
            #endregion

            if ((tradeAllowanceCharge is TradeAllowance allowance) && (allowance.ReasonCode != null))
            {
                _Writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<AllowanceReasonCodes>(allowance.ReasonCode), Profile.Extended);
            }
            else if ((tradeAllowanceCharge is TradeCharge charge) && (charge.ReasonCode != null))
            {
                _Writer.WriteOptionalElementString("ram", "ReasonCode", EnumExtensions.EnumToString<ChargeReasonCodes>(charge.ReasonCode), Profile.Extended);
            }

            _Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason, Profile.Extended); // not in XRechnung according to CII-SR-128

            _Writer.WriteEndElement(); // !AppliedTradeAllowanceCharge
        } // !_WriteItemLevelAppliedTradeAllowanceCharge()


        private void _writeAdditionalReferencedDocument(AdditionalReferencedDocument document, Profile profile, string parentElement = "")
        {
            if (string.IsNullOrWhiteSpace(document?.ID))
            {
                return;
            }

            _Writer.WriteStartElement("ram", "AdditionalReferencedDocument", profile);
            _Writer.WriteElementString("ram", "IssuerAssignedID", document.ID);

            var subProfile = profile;
            switch (parentElement)
            {
                case "BT-18-00":
                case "BG-24":
                    subProfile = Profile.Comfort | Profile.Extended | Profile.XRechnung;
                    break;
                case "BG-X-3":
                    subProfile = Profile.Extended;
                    break;
            }
            if (parentElement == "BG-24" || parentElement == "BG-X-3")
            {
                _Writer.WriteOptionalElementString("ram", "URIID", document.URIID, subProfile); // BT-124, BT-X-28
            }
            if (parentElement == "BG-X-3")
            {
                _Writer.WriteOptionalElementString("ram", "LineID", document.LineID, subProfile); // BT-X-29
            }

            if (document.TypeCode.HasValue)
            {
                _Writer.WriteElementString("ram", "TypeCode", EnumExtensions.EnumToString<AdditionalReferencedDocumentTypeCode>(document.TypeCode.Value));
            }

            if (document.ReferenceTypeCode.HasValue)
            {
                // CII-DT-024: ReferenceTypeCode is only allowed in BT-18-00 and BT-128-00 for InvoiceDataSheet
                if (((parentElement == "BT-18-00" || parentElement == "BT-128-00") && document.TypeCode == AdditionalReferencedDocumentTypeCode.InvoiceDataSheet)
                    || parentElement == "BG-X-3")
                {
                    _Writer.WriteOptionalElementString("ram", "ReferenceTypeCode", document.ReferenceTypeCode.Value.EnumToString()); // BT-128-1, BT-18-1, BT-X-32
                }
            }

            if (parentElement == "BG-24" || parentElement == "BG-X-3")
            {
                _Writer.WriteOptionalElementString("ram", "Name", document.Name, subProfile); // BT-123, BT-X-299
            }

            if (document.AttachmentBinaryObject != null)
            {
                _Writer.WriteStartElement("ram", "AttachmentBinaryObject", subProfile); // BT-125, BT-X-31
                _Writer.WriteAttributeString("filename", document.Filename);
                _Writer.WriteAttributeString("mimeCode", MimeTypeMapper.GetMimeType(document.Filename));
                _Writer.WriteValue(Convert.ToBase64String(document.AttachmentBinaryObject));
                _Writer.WriteEndElement(); // !AttachmentBinaryObject()
            }

            if (document.IssueDateTime.HasValue)
            {
                _Writer.WriteStartElement("ram", "FormattedIssueDateTime", Profile.Extended);
                _Writer.WriteStartElement("qdt", "DateTimeString");
                _Writer.WriteAttributeString("format", "102");
                _Writer.WriteValue(_formatDate(document.IssueDateTime.Value));
                _Writer.WriteEndElement(); // !qdt:DateTimeString
                _Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
            }

            _Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
        } // !_writeAdditionalReferencedDocument()


        private void _writeOptionalAdaptiveValue(ProfileAwareXmlTextWriter writer, decimal? value, int numDecimals = 2, int maxNumDecimals = 4, Profile profile = Profile.Unknown)
        {
            if (!value.HasValue)
            {
                return;
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
        } // !_writeOptionalAdaptiveValue()


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

            _writeOptionalAdaptiveValue(writer, value, numDecimals, maxNumDecimals);

            writer.WriteEndElement(); // !tagName
        } // !_writeOptionalAdaptiveAmount()


        private void _writeOptionalAmount(ProfileAwareXmlTextWriter writer, string prefix, string tagName, decimal? value, int numDecimals = 2, bool forceCurrency = false, Profile profile = Profile.Unknown)
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
            writer.WriteValue(_formatDecimal(value.Value, numDecimals));
            writer.WriteEndElement(); // !tagName
        } // !_writeOptionalAmount()


        private void _writeAmount(ProfileAwareXmlTextWriter writer, string prefix, string tagName, decimal? value, decimal defaultValue = 0m, int numDecimals = 2, bool forceCurrency = false, Profile profile = Profile.Unknown)
        {
            writer.WriteStartElement(prefix, tagName, profile);
            if (forceCurrency)
            {
                writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
            }
            writer.WriteValue(_formatDecimal(value ?? defaultValue, numDecimals));
            writer.WriteEndElement(); // !tagName
        } // !_writeAmount()


        private void _writeElementWithAttribute(ProfileAwareXmlTextWriter writer, string prefix, string tagName, string attributeName, string attributeValue, string nodeValue, Profile profile = Profile.Unknown)
        {
            writer.WriteStartElement(prefix, tagName, profile);
            writer.WriteAttributeString(attributeName, attributeValue);
            writer.WriteValue(nodeValue);
            writer.WriteEndElement(); // !tagName
        } // !_writeElementWithAttribute()


        private void _writeElementWithAttributeWithPrefix(ProfileAwareXmlTextWriter writer, string prefix, string tagName, string attributeName, string attributeValue, string nodeValue, Profile profile = Profile.Unknown)
        {
            writer.WriteStartElement(prefix, tagName, profile);
            writer.WriteAttributeString(attributeName, attributeValue);
            writer.WriteValue(nodeValue);
            writer.WriteEndElement(); // !tagName
        } // !_writeElementWithAttribute()


        private void _writeOptionalTaxes(ProfileAwareXmlTextWriter writer, InvoiceFormatOptions options)
        {
            this._Descriptor.GetApplicableTradeTaxes()?.ForEach(tax =>
            {
                _WriteComment(writer, options, InvoiceCommentConstants.ApplicableTradeTaxComment);
                writer.WriteStartElement("ram", "ApplicableTradeTax");

                writer.WriteStartElement("ram", "CalculatedAmount");
                writer.WriteValue(_formatDecimal(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount

                writer.WriteElementString("ram", "TypeCode", tax.TypeCode.EnumToString());
                writer.WriteOptionalElementString("ram", "ExemptionReason", tax.ExemptionReason);
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
            });
        } // !_writeOptionalTaxes()


        private void _writeNotes(ProfileAwareXmlTextWriter writer, List<Note> notes, Profile profile = Profile.Unknown)
        {
            notes?.ForEach(note =>
            {
                writer.WriteStartElement("ram", "IncludedNote", profile);
                if (note.ContentCode.HasValue)
                {
                    writer.WriteElementString("ram", "ContentCode", note.ContentCode.EnumToString());
                }
                writer.WriteOptionalElementString("ram", "Content", note.Content);
                if (note.SubjectCode.HasValue)
                {
                    writer.WriteElementString("ram", "SubjectCode", note.SubjectCode.EnumToString());
                }
                writer.WriteEndElement();
            });
        } // !_writeNotes()


        private void _writeOptionalLegalOrganization(ProfileAwareXmlTextWriter writer, string prefix, string legalOrganizationTag, LegalOrganization legalOrganization, PartyTypes partyType = PartyTypes.Unknown)
        {
            if (legalOrganization == null)
            {
                return;
            }

            switch (partyType)
            {
                case PartyTypes.SellerTradeParty:
                case PartyTypes.BuyerTradeParty:
                    // all profiles
                    break;
                case PartyTypes.ShipToTradeParty:
                    if (this._Descriptor.Profile == Profile.Minimum) { return; } // it is also possible to add ShipToTradeParty() to a LineItem. In this case, the correct profile filter is different!
                    break;
                case PartyTypes.PayeeTradeParty:
                    if (this._Descriptor.Profile == Profile.Minimum) { return; } // BT-61 / BT-X-508-00
                    break;
                case PartyTypes.BuyerAgentTradeParty:
                case PartyTypes.BuyerTaxRepresentativeTradeParty:
                case PartyTypes.InvoiceeTradeParty:
                case PartyTypes.InvoicerTradeParty:
                case PartyTypes.PayerTradeParty:
                case PartyTypes.ProductEndUserTradeParty:
                case PartyTypes.SalesAgentTradeParty:
                case PartyTypes.ShipFromTradeParty:
                case PartyTypes.UltimateShipToTradeParty:
                    if (this._Descriptor.Profile != Profile.Extended) { return; }
                    break;
                default:
                    return;
            }

            writer.WriteStartElement(prefix, legalOrganizationTag, this._Descriptor.Profile);
            if (!String.IsNullOrWhiteSpace(legalOrganization.ID?.ID))
            {
                if (legalOrganization.ID.SchemeID.HasValue && !String.IsNullOrWhiteSpace(legalOrganization.ID.SchemeID.Value.EnumToString()))
                {
                    writer.WriteStartElement("ram", "ID");
                    writer.WriteAttributeString("schemeID", legalOrganization.ID.SchemeID.Value.EnumToString());
                    writer.WriteValue(legalOrganization.ID.ID);
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteElementString("ram", "ID", legalOrganization.ID.ID);
                }
            }

            // filter according to https://github.com/stephanstapel/ZUGFeRD-csharp/pull/221
            if ((this._Descriptor.Profile == Profile.Extended) ||
                ((partyType == PartyTypes.SellerTradeParty) && (this._Descriptor.Profile != Profile.Minimum)) ||
                ((partyType == PartyTypes.BuyerTradeParty) && this._Descriptor.Profile.In(Profile.Comfort, Profile.XRechnung1, Profile.XRechnung, Profile.Extended)))
            {
                writer.WriteOptionalElementString("ram", "TradingBusinessName", legalOrganization.TradingBusinessName, this._Descriptor.Profile);
            }
            writer.WriteEndElement();
        } // !_writeOptionalLegalOrganization()


        private void _writeOptionalParty(ProfileAwareXmlTextWriter writer, PartyTypes partyType, Party party, Profile profile, Contact contact = null, ElectronicAddress electronicAddress = null, List<TaxRegistration> taxRegistrations = null)
        {
            if (party == null)
            {
                return;
            }

            switch (partyType)
            {
                case PartyTypes.SellerTradeParty:
                    writer.WriteStartElement("ram", "SellerTradeParty", profile);
                    break;
                case PartyTypes.SellerTaxRepresentativeTradeParty:
                    writer.WriteStartElement("ram", "SellerTaxRepresentativeTradeParty", profile);
                    break;
                case PartyTypes.BuyerTradeParty:
                    writer.WriteStartElement("ram", "BuyerTradeParty", profile);
                    break;
                case PartyTypes.ShipToTradeParty:
                    writer.WriteStartElement("ram", "ShipToTradeParty", profile);
                    break;
                case PartyTypes.UltimateShipToTradeParty:
                    writer.WriteStartElement("ram", "UltimateShipToTradeParty", profile);
                    break;
                case PartyTypes.ShipFromTradeParty:
                    writer.WriteStartElement("ram", "ShipFromTradeParty", profile);
                    break;
                case PartyTypes.InvoiceeTradeParty:
                    writer.WriteStartElement("ram", "InvoiceeTradeParty", profile);
                    break;
                case PartyTypes.PayeeTradeParty:
                    writer.WriteStartElement("ram", "PayeeTradeParty", profile);
                    break;
                case PartyTypes.PayerTradeParty:
                    writer.WriteStartElement("ram", "PayerTradeParty", profile);
                    break;
                case PartyTypes.SalesAgentTradeParty:
                    writer.WriteStartElement("ram", "SalesAgentTradeParty", profile);
                    break;
                case PartyTypes.BuyerTaxRepresentativeTradeParty:
                    writer.WriteStartElement("ram", "BuyerTaxRepresentativeTradeParty", profile);
                    break;
                case PartyTypes.ProductEndUserTradeParty:
                    writer.WriteStartElement("ram", "ProductEndUserTradeParty", profile);
                    break;
                case PartyTypes.BuyerAgentTradeParty:
                    writer.WriteStartElement("ram", "BuyerAgentTradeParty", profile);
                    break;
                case PartyTypes.InvoicerTradeParty:
                    writer.WriteStartElement("ram", "InvoicerTradeParty", profile);
                    break;
                default:
                    return;
            }

            if (!string.IsNullOrWhiteSpace(party.ID?.ID))
            {
                writer.WriteOptionalElementString("ram", "ID", party.ID.ID);
            }

            if (!String.IsNullOrWhiteSpace(party.GlobalID?.ID) && party.GlobalID.SchemeID.HasValue && party.GlobalID.SchemeID.HasValue)
            {
                writer.WriteStartElement("ram", "GlobalID");
                writer.WriteAttributeString("schemeID", party.GlobalID.SchemeID.Value.EnumToString());
                writer.WriteValue(party.GlobalID.ID);
                writer.WriteEndElement();
            }

            writer.WriteOptionalElementString("ram", "Name", party.Name);
            writer.WriteOptionalElementString("ram", "Description", party.Description, PROFILE_COMFORT_EXTENDED_XRECHNUNG); // BT-33

            _writeOptionalLegalOrganization(writer, "ram", "SpecifiedLegalOrganization", party.SpecifiedLegalOrganization, partyType);
            _writeOptionalContact(writer, "ram", "DefinedTradeContact", contact, PROFILE_COMFORT_EXTENDED_XRECHNUNG);

            // spec 2.3 says: Minimum/BuyerTradeParty does not include PostalTradeAddress
            if ((this._Descriptor.Profile == Profile.Extended) || partyType.In(PartyTypes.BuyerTradeParty, PartyTypes.SellerTradeParty, PartyTypes.SellerTaxRepresentativeTradeParty, PartyTypes.BuyerTaxRepresentativeTradeParty, PartyTypes.ShipToTradeParty, PartyTypes.ShipToTradeParty, PartyTypes.UltimateShipToTradeParty, PartyTypes.SalesAgentTradeParty))
            {
                writer.WriteStartElement("ram", "PostalTradeAddress");
                writer.WriteOptionalElementString("ram", "PostcodeCode", party.Postcode); // buyer: BT-53
                writer.WriteOptionalElementString("ram", "LineOne", string.IsNullOrWhiteSpace(party.ContactName) ? party.Street : party.ContactName); // buyer: BT-50
                if (!string.IsNullOrWhiteSpace(party.ContactName))
                {
                    writer.WriteOptionalElementString("ram", "LineTwo", party.Street); // buyer: BT-51
                }
                writer.WriteOptionalElementString("ram", "LineThree", party.AddressLine3); // buyer: BT-163
                writer.WriteOptionalElementString("ram", "CityName", party.City); // buyer: BT-52

                if (party.Country != null)
                {
                    writer.WriteElementString("ram", "CountryID", party.Country.Value.EnumToString()); // buyer: BT-55
                }

                writer.WriteOptionalElementString("ram", "CountrySubDivisionName", party.CountrySubdivisionName); // BT-79
                writer.WriteEndElement(); // !PostalTradeAddress
            }

            if (!String.IsNullOrWhiteSpace(electronicAddress?.Address))
            {
                writer.WriteStartElement("ram", "URIUniversalCommunication");
                writer.WriteStartElement("ram", "URIID");
                writer.WriteAttributeString("schemeID", electronicAddress.ElectronicAddressSchemeID.EnumToString());
                writer.WriteValue(electronicAddress.Address);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            if (taxRegistrations?.Count > 0)
            {
                // for seller: BT-31
                // for buyer : BT-48
                foreach (TaxRegistration taxRegistration in taxRegistrations.Where(tr => !String.IsNullOrWhiteSpace(tr.No)))
                {
                    writer.WriteStartElement("ram", "SpecifiedTaxRegistration");
                    writer.WriteStartElement("ram", "ID");
                    writer.WriteAttributeString("schemeID", taxRegistration.SchemeID.EnumToString());
                    writer.WriteValue(taxRegistration.No);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement(); // !*Party
        } // !_writeOptionalParty()


        private void _writeOptionalContact(ProfileAwareXmlTextWriter writer, string prefix, string contactTag, Contact contact, Profile profile = Profile.Unknown)
        {
            if (contact == null)
            {
                return;
            }

            writer.WriteStartElement(prefix, contactTag, profile);

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
        } // !_writeOptionalContact()


        private string _TranslateTaxCategoryCode(TaxCategoryCodes? taxCategoryCode)
        {
            if (!taxCategoryCode.HasValue)
            {
                return null;
            }

            switch (taxCategoryCode)
            {
                case TaxCategoryCodes.A:
                    break;
                case TaxCategoryCodes.AA:
                    break;
                case TaxCategoryCodes.AB:
                    break;
                case TaxCategoryCodes.AC:
                    break;
                case TaxCategoryCodes.AD:
                    break;
                case TaxCategoryCodes.AE:
                    return "Umkehrung der Steuerschuldnerschaft";
                case TaxCategoryCodes.B:
                    break;
                case TaxCategoryCodes.C:
                    break;
                case TaxCategoryCodes.E:
                    return "steuerbefreit";
                case TaxCategoryCodes.G:
                    return "freier Ausfuhrartikel, Steuer nicht erhoben";
                case TaxCategoryCodes.H:
                    break;
                case TaxCategoryCodes.O:
                    return "Dienstleistungen außerhalb des Steueranwendungsbereichs";
                case TaxCategoryCodes.S:
                    return null; // don't write exemption reason for standard tax category code
                case TaxCategoryCodes.Z:
                    return "nach dem Nullsatz zu versteuernde Waren";
                case TaxCategoryCodes.D:
                    break;
                case TaxCategoryCodes.F:
                    break;
                case TaxCategoryCodes.I:
                    break;
                case TaxCategoryCodes.J:
                    break;
                case TaxCategoryCodes.K:
                    return "Kein Ausweis der Umsatzsteuer bei innergemeinschaftlichen Lieferungen";
                case TaxCategoryCodes.L:
                    return "IGIC (Kanarische Inseln)";
                case TaxCategoryCodes.M:
                    return "IPSI (Ceuta/Melilla)";
            }

            return null;
        }


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

            return EnumExtensions.EnumToInt<InvoiceType>(type);
        } // !_translateInvoiceType()


        /// <summary>
        /// This function is implemented in class InvoiceDescriptor23Writer.
        /// </summary>
        internal override bool Validate(InvoiceDescriptor descriptor, bool throwExceptions = true)
        {
            return false;
        } // !Validate()
    }
}

