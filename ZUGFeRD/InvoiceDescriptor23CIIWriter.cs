﻿/*
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
    internal class InvoiceDescriptor23CIIWriter : IInvoiceDescriptorWriter
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

            #region ExchangedDocumentContext
            //Prozesssteuerung
            Writer.WriteStartElement("rsm", "ExchangedDocumentContext");
            if (this.Descriptor.IsTest)
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
            //Gruppierung der Anwendungsempfehlungsinformationen
            Writer.WriteElementString("ram", "ID", this.Descriptor.Profile.EnumToString(ZUGFeRDVersion.Version23));
            Writer.WriteEndElement(); // !ram:GuidelineSpecifiedDocumentContextParameter
            Writer.WriteEndElement(); // !rsm:ExchangedDocumentContext
            #endregion

            #region ExchangedDocument
            //Gruppierung der Eigenschaften, die das gesamte Dokument betreffen.
            Writer.WriteStartElement("rsm", "ExchangedDocument");
            Writer.WriteElementString("ram", "ID", this.Descriptor.InvoiceNo); //Rechnungsnummer
            Writer.WriteOptionalElementString("ram", "Name", this.Descriptor.Name, Profile.Extended); //Dokumentenart (Freitext)
            Writer.WriteElementString("ram", "TypeCode", String.Format("{0}", _encodeInvoiceType(this.Descriptor.Type))); //Code für den Rechnungstyp
                                                                                                                          //ToDo: LanguageID      //Sprachkennzeichen
                                                                                                                          //ToDo: IncludedNote    //Freitext zur Rechnung
            if (this.Descriptor.InvoiceDate.HasValue)
            {
                Writer.WriteStartElement("ram", "IssueDateTime");
                Writer.WriteStartElement("udt", "DateTimeString");  //Rechnungsdatum
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor.InvoiceDate.Value));
                Writer.WriteEndElement(); // !udt:DateTimeString
                Writer.WriteEndElement(); // !IssueDateTime
            }
            _writeNotes(Writer, this.Descriptor.Notes, ALL_PROFILES ^ Profile.Minimum);
            Writer.WriteEndElement(); // !rsm:ExchangedDocument
            #endregion


            #region SpecifiedSupplyChainTradeTransaction
            //Gruppierung der Informationen zum Geschäftsvorfall
            Writer.WriteStartElement("rsm", "SupplyChainTradeTransaction");

            #region  IncludedSupplyChainTradeLineItem
            foreach (TradeLineItem tradeLineItem in this.Descriptor.TradeLineItems)
            {
                Writer.WriteStartElement("ram", "IncludedSupplyChainTradeLineItem");

                #region AssociatedDocumentLineDocument
                //Gruppierung von allgemeinen Positionsangaben
                if (tradeLineItem.AssociatedDocument != null)
                {
                    Writer.WriteStartElement("ram", "AssociatedDocumentLineDocument", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    if (!String.IsNullOrWhiteSpace(tradeLineItem.AssociatedDocument.LineID))
                    {
                        Writer.WriteElementString("ram", "LineID", tradeLineItem.AssociatedDocument.LineID);
                    }
                    // It is necessary that Parent Line Id be written directly under LineId
                    if (!String.IsNullOrWhiteSpace(tradeLineItem.AssociatedDocument.ParentLineID))
                    {
                        Writer.WriteElementString("ram", "ParentLineID", tradeLineItem.AssociatedDocument.ParentLineID);
                    }
                    if (tradeLineItem.AssociatedDocument.LineStatusCode.HasValue)
                    {
                        Writer.WriteElementString("ram", "LineStatusCode", tradeLineItem.AssociatedDocument.LineStatusCode.Value.EnumValueToString());
                    }
                    if (tradeLineItem.AssociatedDocument.LineStatusReasonCode.HasValue)
                    {
                        Writer.WriteElementString("ram", "LineStatusReasonCode", tradeLineItem.AssociatedDocument.LineStatusReasonCode.Value.EnumToString());
                    }
                    _writeNotes(Writer, tradeLineItem.AssociatedDocument.Notes, ALL_PROFILES ^ Profile.Minimum ^ Profile.BasicWL);
                    Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument(Basic|Comfort|Extended|XRechnung)
                }
                #endregion

                // handelt es sich um einen Kommentar?
                bool isCommentItem = false;
                if ((tradeLineItem.AssociatedDocument?.Notes.Count > 0) && (tradeLineItem.BilledQuantity == 0) && (String.IsNullOrWhiteSpace(tradeLineItem.Description)))
                {
                    isCommentItem = true;
                }

                #region SpecifiedTradeProduct
                //Eine Gruppe von betriebswirtschaftlichen Begriffen, die Informationen über die in Rechnung gestellten Waren und Dienstleistungen enthält
                Writer.WriteStartElement("ram", "SpecifiedTradeProduct", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                if ((tradeLineItem.GlobalID != null) && (tradeLineItem.GlobalID.SchemeID.HasValue) && (tradeLineItem.GlobalID.SchemeID.Value != GlobalIDSchemeIdentifiers.Unknown) && !String.IsNullOrWhiteSpace(tradeLineItem.GlobalID.ID))
                {
                    _writeElementWithAttributeWithPrefix(Writer, "ram", "GlobalID", "schemeID", tradeLineItem.GlobalID.SchemeID.Value.EnumToString(), tradeLineItem.GlobalID.ID, Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                }

                Writer.WriteOptionalElementString("ram", "SellerAssignedID", tradeLineItem.SellerAssignedID, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                Writer.WriteOptionalElementString("ram", "BuyerAssignedID", tradeLineItem.BuyerAssignedID, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                // BT-153
                Writer.WriteOptionalElementString("ram", "Name", tradeLineItem.Name, Profile.Basic | Profile.Comfort | Profile.Extended);
                Writer.WriteOptionalElementString("ram", "Name", isCommentItem ? "TEXT" : tradeLineItem.Name, Profile.XRechnung1 | Profile.XRechnung); // XRechnung erfordert einen Item-Namen (BR-25)

                Writer.WriteOptionalElementString("ram", "Description", tradeLineItem.Description, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

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
                            _writeElementWithAttributeWithPrefix(Writer, "ram", "UnitQuantity", "unitCode", includedItem.UnitCode.Value.EnumToString(), _formatDecimal(includedItem.UnitQuantity, 4));
                        }
                        Writer.WriteEndElement(); // !ram:IncludedReferencedProduct
                    }
                }

                if (tradeLineItem.GetDesignatedProductClassifications().Any())
                {
                    foreach (var designatedProductClassification in tradeLineItem.GetDesignatedProductClassifications())
                    {
                        if (designatedProductClassification.ListID == default(DesignatedProductClassificationClassCodes))
                        {
                            continue;
                        }

                        Writer.WriteStartElement("ram", "DesignatedProductClassification", Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                        Writer.WriteStartElement("ram", "ClassCode");
                        Writer.WriteAttributeString("listID", designatedProductClassification.ListID.EnumToString());
                        Writer.WriteAttributeString("listVersionID", designatedProductClassification.ListVersionID);
                        Writer.WriteValue(designatedProductClassification.ClassCode);
                        Writer.WriteEndElement(); // !ram::ClassCode
                        Writer.WriteOptionalElementString("ram", "ClassName", designatedProductClassification.ClassName);
                        Writer.WriteEndElement(); // !ram:DesignatedProductClassification
                    }
                }

                Writer.WriteEndElement(); // !ram:SpecifiedTradeProduct(Basic|Comfort|Extended|XRechnung)
                #endregion

                #region SpecifiedLineTradeAgreement (Basic, Comfort, Extended, XRechnung)
                //Eine Gruppe von betriebswirtschaftlichen Begriffen, die Informationen über den Preis für die in der betreffenden Rechnungsposition in Rechnung gestellten Waren und Dienstleistungen enthält

                if (new Profile[] { Profile.Basic, Profile.Comfort, Profile.Extended, Profile.XRechnung, Profile.XRechnung1 }.Contains(descriptor.Profile))
                {
                    Writer.WriteStartElement("ram", "SpecifiedLineTradeAgreement", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                    #region BuyerOrderReferencedDocument (Comfort, Extended, XRechnung)
                    //Detailangaben zur zugehörigen Bestellung
                    if (tradeLineItem.BuyerOrderReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument", Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                        //Bestellnummer
                        Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.BuyerOrderReferencedDocument.ID, Profile.Extended);

                        // reference to the order position
                        Writer.WriteOptionalElementString("ram", "LineID", tradeLineItem.BuyerOrderReferencedDocument.LineID);

                        if (tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.HasValue)
                        {
                            Writer.WriteStartElement("ram", "FormattedIssueDateTime", Profile.Extended);
                            Writer.WriteStartElement("qdt", "DateTimeString");
                            Writer.WriteAttributeString("format", "102");
                            Writer.WriteValue(_formatDate(tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.Value));
                            Writer.WriteEndElement(); // !qdt:DateTimeString
                            Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                        }

                        Writer.WriteEndElement(); // !ram:BuyerOrderReferencedDocument
                    }
                    #endregion

                    #region ContractReferencedDocument
                    //Detailangaben zum zugehörigen Vertrag
                    if (tradeLineItem.ContractReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram", "ContractReferencedDocument", Profile.Extended);
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
                    #endregion

                    #region AdditionalReferencedDocument (Extended)

                    //Detailangaben zu einer zusätzlichen Dokumentenreferenz
                    foreach (AdditionalReferencedDocument document in tradeLineItem._AdditionalReferencedDocuments)
                    {
                        _writeAdditionalReferencedDocument(document, Profile.Extended);
                    } // !foreach(document)
                    #endregion

                    #region GrossPriceProductTradePrice (Comfort, Extended, XRechnung)
                    bool needToWriteGrossUnitPrice = false;

                    // the PEPPOL business rule for XRechnung is very specific
                    // PEPPOL-EN16931-R046
                    if ((descriptor.Profile == Profile.XRechnung) && tradeLineItem.GrossUnitPrice.HasValue && (tradeLineItem.GetTradeAllowanceCharges().Count > 0))
                    {
                        needToWriteGrossUnitPrice = true;
                    }
                    else if ((descriptor.Profile != Profile.XRechnung) && ((tradeLineItem.GrossUnitPrice.HasValue || (tradeLineItem.GetTradeAllowanceCharges().Count > 0))))
                    {
                        needToWriteGrossUnitPrice = true;
                    }


                    if (needToWriteGrossUnitPrice)
                    {
                        Writer.WriteStartElement("ram", "GrossPriceProductTradePrice", Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                        _writeOptionalAmount(Writer, "ram", "ChargeAmount", tradeLineItem.GrossUnitPrice, 4);   // BT-148
                        if (tradeLineItem.UnitQuantity.HasValue)
                        {
                            _writeElementWithAttributeWithPrefix(Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 4));
                        }

                        foreach (TradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.GetTradeAllowanceCharges()) // BT-147
                        {
                            Writer.WriteStartElement("ram", "AppliedTradeAllowanceCharge");

                            #region ChargeIndicator
                            Writer.WriteStartElement("ram", "ChargeIndicator");
                            Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                            Writer.WriteEndElement(); // !ram:ChargeIndicator
                            #endregion

                            #region ChargePercentage
                            if (tradeAllowanceCharge.ChargePercentage.HasValue)
                            {
                                Writer.WriteStartElement("ram", "CalculationPercent", profile: Profile.Extended | Profile.XRechnung);
                                Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ChargePercentage.Value, 2));
                                Writer.WriteEndElement();
                            }
                            #endregion

                            #region BasisAmount
                            if (tradeAllowanceCharge.BasisAmount.HasValue)
                            {
                                Writer.WriteStartElement("ram", "BasisAmount", profile: Profile.Extended); // not in XRechnung, according to CII-SR-123
                                Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value, 2));
                                Writer.WriteEndElement();
                            }
                            #endregion

                            #region ActualAmount
                            Writer.WriteStartElement("ram", "ActualAmount");
                            Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 2));
                            Writer.WriteEndElement();
                            #endregion

                            Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason, Profile.Extended); // not in XRechnung according to CII-SR-128

                            Writer.WriteEndElement(); // !AppliedTradeAllowanceCharge
                        }

                        Writer.WriteEndElement(); // ram:GrossPriceProductTradePrice(Comfort|Extended|XRechnung)
                    }
                    #endregion // !GrossPriceProductTradePrice(Comfort|Extended|XRechnung)

                    #region NetPriceProductTradePrice
                    //Im Nettopreis sind alle Zu- und Abschläge enthalten, jedoch nicht die Umsatzsteuer.
                    Writer.WriteStartElement("ram", "NetPriceProductTradePrice", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    _writeOptionalAmount(Writer, "ram", "ChargeAmount", tradeLineItem.NetUnitPrice, 4); // BT-146

                    if (tradeLineItem.UnitQuantity.HasValue)
                    {
                        _writeElementWithAttributeWithPrefix(Writer, "ram", "BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 4));
                    }
                    Writer.WriteEndElement(); // ram:NetPriceProductTradePrice(Basic|Comfort|Extended|XRechnung)
                    #endregion // !NetPriceProductTradePrice(Basic|Comfort|Extended|XRechnung)

                    #region UltimateCustomerOrderReferencedDocument
                    //ToDo: UltimateCustomerOrderReferencedDocument
                    #endregion
                    Writer.WriteEndElement(); // ram:SpecifiedLineTradeAgreement
                }
                #endregion

                #region SpecifiedLineTradeDelivery (Basic, Comfort, Extended)
                Writer.WriteStartElement("ram", "SpecifiedLineTradeDelivery", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                _writeElementWithAttributeWithPrefix(Writer, "ram", "BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 4));

                if (tradeLineItem.ShipTo != null)
                {
                    _writeOptionalParty(Writer, PartyTypes.ShipToTradeParty, tradeLineItem.ShipTo, Profile.Extended);
                }

                if (tradeLineItem.UltimateShipTo != null)
                {
                    _writeOptionalParty(Writer, PartyTypes.UltimateShipToTradeParty, tradeLineItem.UltimateShipTo, Profile.Extended);
                }

                if (tradeLineItem.ActualDeliveryDate.HasValue)
                {
                    Writer.WriteStartElement("ram", "ActualDeliverySupplyChainEvent", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung)); // this violates CII-SR-170 for XRechnung 3
                    Writer.WriteStartElement("ram", "OccurrenceDateTime");
                    Writer.WriteStartElement("udt", "DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(tradeLineItem.ActualDeliveryDate.Value));
                    Writer.WriteEndElement(); // !udt:DateTimeString
                    Writer.WriteEndElement(); // !OccurrenceDateTime()
                    Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
                }

                if (tradeLineItem.DeliveryNoteReferencedDocument != null)
                {
                    Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung)); // this violates CII-SR-175 for XRechnung 3
                    Writer.WriteOptionalElementString("ram", "IssuerAssignedID", tradeLineItem.DeliveryNoteReferencedDocument.ID);

                    if (tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                    {
                        Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                        Writer.WriteStartElement("qdt", "DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                        Writer.WriteEndElement(); // !qdt:DateTimeString
                        Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                    }

                    Writer.WriteEndElement(); // !ram:DeliveryNoteReferencedDocument
                }

                /// TODO: Add ShipToTradeParty
                /// TODO: Add UltimateShipToTradeParty

                Writer.WriteEndElement(); // !ram:SpecifiedLineTradeDelivery
                #endregion

                #region SpecifiedLineTradeSettlement
                Writer.WriteStartElement("ram", "SpecifiedLineTradeSettlement", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                #region ApplicableTradeTax
                Writer.WriteStartElement("ram", "ApplicableTradeTax", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                Writer.WriteElementString("ram", "TypeCode", tradeLineItem.TaxType.EnumToString());
                Writer.WriteOptionalElementString("ram", "ExemptionReason", _translateTaxCategoryCode(tradeLineItem.TaxCategoryCode), Profile.Extended);
                Writer.WriteElementString("ram", "CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString()); // BT-151



                if (tradeLineItem.TaxCategoryCode != TaxCategoryCodes.O) // notwendig, damit die Validierung klappt
                {
                    Writer.WriteElementString("ram", "RateApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent));
                }

                Writer.WriteEndElement(); // !ram:ApplicableTradeTax(Basic|Comfort|Extended|XRechnung)
                #endregion // !ApplicableTradeTax(Basic|Comfort|Extended|XRechnung)

                #region BillingSpecifiedPeriod
                if (tradeLineItem.BillingPeriodStart.HasValue || tradeLineItem.BillingPeriodEnd.HasValue)
                {
                    Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    if (tradeLineItem.BillingPeriodStart.HasValue)
                    {
                        Writer.WriteStartElement("ram", "StartDateTime");
                        _writeElementWithAttributeWithPrefix(Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodStart.Value));
                        Writer.WriteEndElement(); // !StartDateTime
                    }

                    if (tradeLineItem.BillingPeriodEnd.HasValue)
                    {
                        Writer.WriteStartElement("ram", "EndDateTime");
                        _writeElementWithAttributeWithPrefix(Writer, "udt", "DateTimeString", "format", "102", _formatDate(tradeLineItem.BillingPeriodEnd.Value));
                        Writer.WriteEndElement(); // !EndDateTime
                    }
                    Writer.WriteEndElement(); // !BillingSpecifiedPeriod
                }
                #endregion

                #region SpecifiedTradeAllowanceCharge (Basic, Comfort, Extended, XRechnung)
                //Abschläge auf Ebene der Rechnungsposition (Basic, Comfort, Extended, XRechnung)
                if (new Profile[] { Profile.Basic, Profile.Comfort, Profile.Extended, Profile.XRechnung1, Profile.XRechnung }.Contains(descriptor.Profile))
                {
                    if (tradeLineItem.GetSpecifiedTradeAllowanceCharges().Count > 0)
                    {
                        foreach (TradeAllowanceCharge specifiedTradeAllowanceCharge in tradeLineItem.GetSpecifiedTradeAllowanceCharges()) // BG-28
                        {
                            Writer.WriteStartElement("ram", "SpecifiedTradeAllowanceCharge");
                            #region ChargeIndicator
                            Writer.WriteStartElement("ram", "ChargeIndicator"); // BG-28-0
                            Writer.WriteElementString("udt", "Indicator", specifiedTradeAllowanceCharge.ChargeIndicator ? "true" : "false"); // BG-28-1
                            Writer.WriteEndElement(); // !ram:ChargeIndicator
                            #endregion

                            #region ChargePercentage
                            if (specifiedTradeAllowanceCharge.ChargePercentage.HasValue)
                            {
                                Writer.WriteStartElement("ram", "CalculationPercent"); // BT-143
                                Writer.WriteValue(_formatDecimal(specifiedTradeAllowanceCharge.ChargePercentage.Value, 2));
                                Writer.WriteEndElement();
                            }
                            #endregion

                            #region BasisAmount
                            if (specifiedTradeAllowanceCharge.BasisAmount.HasValue)
                            {
                                Writer.WriteStartElement("ram", "BasisAmount", profile: Profile.Extended | Profile.XRechnung | Profile.Comfort);
                                Writer.WriteValue(_formatDecimal(specifiedTradeAllowanceCharge.BasisAmount.Value, 2));
                                Writer.WriteEndElement();
                            }
                            #endregion

                            #region ActualAmount
                            Writer.WriteStartElement("ram", "ActualAmount");
                            Writer.WriteValue(_formatDecimal(specifiedTradeAllowanceCharge.ActualAmount, 2));
                            Writer.WriteEndElement();
                            #endregion

                            Writer.WriteOptionalElementString("ram", "Reason", specifiedTradeAllowanceCharge.Reason, Profile.Extended | Profile.XRechnung | Profile.Comfort);
                            Writer.WriteEndElement(); // !ram:SpecifiedTradeAllowanceCharge
                        }
                    }
                }
                #endregion

                #region SpecifiedTradeSettlementLineMonetarySummation (Basic, Comfort, Extended)
                //Detailinformationen zu Positionssummen
                Writer.WriteStartElement("ram", "SpecifiedTradeSettlementLineMonetarySummation");
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

                Writer.WriteStartElement("ram", "LineTotalAmount", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                Writer.WriteValue(_formatDecimal(_total));
                Writer.WriteEndElement(); // !ram:LineTotalAmount

                //ToDo: TotalAllowanceChargeAmount
                //Gesamtbetrag der Positionszu- und Abschläge
                Writer.WriteEndElement(); // ram:SpecifiedTradeSettlementMonetarySummation
                #endregion

                #region AdditionalReferencedDocument
                //Objektkennung auf Ebene der Rechnungsposition
                //ToDo: AdditionalReferencedDocument
                #endregion

                #region ReceivableSpecifiedTradeAccountingAccount
                //Detailinformationen zur Buchungsreferenz
                if ((descriptor.Profile == Profile.XRechnung1 || descriptor.Profile == Profile.XRechnung) && tradeLineItem.ReceivableSpecifiedTradeAccountingAccounts.Count > 0)
                {
                    //only one ReceivableSpecifiedTradeAccountingAccount (BT-133) is allowed in Profile XRechnung
                    Writer.WriteStartElement("ram", "ReceivableSpecifiedTradeAccountingAccount", Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    {
                        Writer.WriteStartElement("ram", "ID");
                        Writer.WriteValue(tradeLineItem.ReceivableSpecifiedTradeAccountingAccounts[0].TradeAccountID);  //BT-133
                        Writer.WriteEndElement(); // !ram:ID
                    }
                    Writer.WriteEndElement(); // !ram:ReceivableSpecifiedTradeAccountingAccount
                }
                else
                {
                    //multiple ReceivableSpecifiedTradeAccountingAccounts are allowed in other profiles
                    foreach (ReceivableSpecifiedTradeAccountingAccount RSTA in tradeLineItem.ReceivableSpecifiedTradeAccountingAccounts)
                    {
                        Writer.WriteStartElement("ram", "ReceivableSpecifiedTradeAccountingAccount", Profile.Comfort | Profile.Extended);

                        {
                            Writer.WriteStartElement("ram", "ID");
                            Writer.WriteValue(RSTA.TradeAccountID);
                            Writer.WriteEndElement(); // !ram:ID
                        }

                        if (RSTA.TradeAccountTypeCode != AccountingAccountTypeCodes.Unknown)
                        {
                            Writer.WriteStartElement("ram", "TypeCode", Profile.Extended);
                            Writer.WriteValue(((int)RSTA.TradeAccountTypeCode).ToString());
                            Writer.WriteEndElement(); // !ram:TypeCode
                        }

                        Writer.WriteEndElement(); // !ram:ReceivableSpecifiedTradeAccountingAccount
                    }
                }
                #endregion

                Writer.WriteEndElement(); // !ram:SpecifiedLineTradeSettlement
                #endregion

                Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
            } // !foreach(tradeLineItem)
            #endregion

            #region ApplicableHeaderTradeAgreement
            Writer.WriteStartElement("ram", "ApplicableHeaderTradeAgreement");

            // BT-10
            Writer.WriteOptionalElementString("ram", "BuyerReference", this.Descriptor.ReferenceOrderNo);

            #region SellerTradeParty
            // BT-31: this.Descriptor.SellerTaxRegistration
            _writeOptionalParty(Writer, PartyTypes.SellerTradeParty, this.Descriptor.Seller, ALL_PROFILES, this.Descriptor.SellerContact, this.Descriptor.SellerElectronicAddress, this.Descriptor.SellerTaxRegistration);
            #endregion

            #region BuyerTradeParty
            // BT-48: this.Descriptor.BuyerTaxRegistration
            _writeOptionalParty(Writer, PartyTypes.BuyerTradeParty, this.Descriptor.Buyer, ALL_PROFILES, this.Descriptor.BuyerContact, this.Descriptor.BuyerElectronicAddress, this.Descriptor.BuyerTaxRegistration);
            #endregion

            // TODO: implement SellerTaxRepresentativeTradeParty
            // BT-63: the tax registration of the SellerTaxRepresentativeTradeParty

            #region 1. SellerOrderReferencedDocument (BT-14: Comfort, Extended)
            if (null != this.Descriptor.SellerOrderReferencedDocument && !string.IsNullOrWhiteSpace(Descriptor.SellerOrderReferencedDocument.ID))
            {
                Writer.WriteStartElement("ram", "SellerOrderReferencedDocument", Profile.Comfort | Profile.Extended | Profile.XRechnung);
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

            #region 2. BuyerOrderReferencedDocument
            if (!String.IsNullOrWhiteSpace(this.Descriptor.OrderNo))
            {
                Writer.WriteStartElement("ram", "BuyerOrderReferencedDocument");
                Writer.WriteElementString("ram", "IssuerAssignedID", this.Descriptor.OrderNo);
                if (this.Descriptor.OrderDate.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung));
                    Writer.WriteStartElement("qdt", "DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.OrderDate.Value));
                    Writer.WriteEndElement(); // !qdt:DateTimeString
                    Writer.WriteEndElement(); // !IssueDateTime()
                }

                Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }
            #endregion

            #region 3. ContractReferencedDocument
            // BT-12
            if (this.Descriptor.ContractReferencedDocument != null)
            {
                Writer.WriteStartElement("ram", "ContractReferencedDocument");
                Writer.WriteElementString("ram", "IssuerAssignedID", this.Descriptor.ContractReferencedDocument.ID);
                if (this.Descriptor.ContractReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung));
                    Writer.WriteStartElement("qdt", "DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.ContractReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // !qdt:DateTimeString
                    Writer.WriteEndElement(); // !IssueDateTime()
                }

                Writer.WriteEndElement(); // !ram:ContractReferencedDocument
            }
            #endregion

            #region 4. AdditionalReferencedDocument
            if (this.Descriptor.AdditionalReferencedDocuments != null)
            {
                foreach (AdditionalReferencedDocument document in this.Descriptor.AdditionalReferencedDocuments)
                {
                    _writeAdditionalReferencedDocument(document, Profile.Comfort | Profile.Extended | Profile.XRechnung); // BG-24         
                }
            }
            #endregion

            #region SpecifiedProcuringProject
            if (Descriptor.SpecifiedProcuringProject != null)
            {

                Writer.WriteStartElement("ram", "SpecifiedProcuringProject", Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                Writer.WriteElementString("ram", "ID", Descriptor.SpecifiedProcuringProject.ID, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                Writer.WriteElementString("ram", "Name", Descriptor.SpecifiedProcuringProject.Name, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                Writer.WriteEndElement(); // !ram:SpecifiedProcuringProject
            }
            #endregion

            Writer.WriteEndElement(); // !ApplicableHeaderTradeAgreement
            #endregion

            #region ApplicableHeaderTradeDelivery
            Writer.WriteStartElement("ram", "ApplicableHeaderTradeDelivery"); // Pflichteintrag
            _writeOptionalParty(Writer, PartyTypes.ShipToTradeParty, this.Descriptor.ShipTo, ALL_PROFILES ^ Profile.Minimum);
            _writeOptionalParty(Writer, PartyTypes.UltimateShipToTradeParty, this.Descriptor.UltimateShipTo, Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
            _writeOptionalParty(Writer, PartyTypes.ShipFromTradeParty, this.Descriptor.ShipFrom, Profile.Extended);

            #region ActualDeliverySupplyChainEvent
            if (this.Descriptor.ActualDeliveryDate.HasValue)
            {
                Writer.WriteStartElement("ram", "ActualDeliverySupplyChainEvent");
                Writer.WriteStartElement("ram", "OccurrenceDateTime");
                Writer.WriteStartElement("udt", "DateTimeString");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor.ActualDeliveryDate.Value));
                Writer.WriteEndElement(); // "udt:DateTimeString
                Writer.WriteEndElement(); // !OccurrenceDateTime()
                Writer.WriteEndElement(); // !ActualDeliverySupplyChainEvent
            }
            #endregion

            #region DespatchAdviceReferencedDocument
            if (this.Descriptor.DespatchAdviceReferencedDocument != null)
            {
                Writer.WriteStartElement("ram", "DespatchAdviceReferencedDocument");
                Writer.WriteElementString("ram", "IssuerAssignedID", this.Descriptor.DespatchAdviceReferencedDocument.ID);

                if (this.Descriptor.DespatchAdviceReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    Writer.WriteStartElement("qdt", "DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.DespatchAdviceReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // "qdt:DateTimeString
                    Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }

                Writer.WriteEndElement(); // !DespatchAdviceReferencedDocument
            }
            #endregion

            #region DeliveryNoteReferencedDocument
            if (this.Descriptor.DeliveryNoteReferencedDocument != null)
            {
                Writer.WriteStartElement("ram", "DeliveryNoteReferencedDocument");
                Writer.WriteElementString("ram", "IssuerAssignedID", this.Descriptor.DeliveryNoteReferencedDocument.ID);

                if (this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung));
                    Writer.WriteStartElement("qdt", "DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // !qdt:DateTimeString
                    Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }

                Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }
            #endregion

            Writer.WriteEndElement(); // !ApplicableHeaderTradeDelivery
            #endregion

            #region ApplicableHeaderTradeSettlement
            Writer.WriteStartElement("ram", "ApplicableHeaderTradeSettlement");
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

            //   1. CreditorReferenceID(BT-90) is only required/allowed on DirectDebit (BR-DE-30)
            if ((this.Descriptor.PaymentMeans?.TypeCode == PaymentMeansTypeCodes.DirectDebit || this.Descriptor.PaymentMeans?.TypeCode == PaymentMeansTypeCodes.SEPADirectDebit) && !String.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans?.SEPACreditorIdentifier))
            {
                Writer.WriteElementString("ram", "CreditorReferenceID", Descriptor.PaymentMeans?.SEPACreditorIdentifier, Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung | Profile.XRechnung1);
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
            Writer.WriteOptionalElementString("ram", "InvoiceIssuerReference", this.Descriptor.SellerReferenceNo, profile: Profile.Extended);

            //   6. InvoicerTradeParty (optional)
            _writeOptionalParty(Writer, PartyTypes.InvoicerTradeParty, this.Descriptor.Invoicer, Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

            //   7. InvoiceeTradeParty (optional)
            _writeOptionalParty(Writer, PartyTypes.InvoiceeTradeParty, this.Descriptor.Invoicee, Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

            //   8. PayeeTradeParty (optional)
            _writeOptionalParty(Writer, PartyTypes.PayeeTradeParty, this.Descriptor.Payee, ALL_PROFILES ^ Profile.Minimum);

            #region SpecifiedTradeSettlementPaymentMeans
            //  10. SpecifiedTradeSettlementPaymentMeans (optional)

            if (this.Descriptor.CreditorBankAccounts.Count == 0 && this.Descriptor.DebitorBankAccounts.Count == 0)
            {
                if (this.Descriptor.PaymentMeans != null)
                {

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                        Writer.WriteElementString("ram", "TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram", "Information", this.Descriptor.PaymentMeans.Information, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                        if (this.Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            Writer.WriteStartElement("ram", "ApplicableTradeSettlementFinancialCard", Profile.Comfort | Profile.Extended | Profile.XRechnung);
                            Writer.WriteOptionalElementString("ram", "ID", Descriptor.PaymentMeans.FinancialCard.Id);
                            Writer.WriteOptionalElementString("ram", "CardholderName", Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            Writer.WriteEndElement(); // !ram:ApplicableTradeSettlementFinancialCard
                        }
                        Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                    }
                }
            }
            else
            {
                foreach (BankAccount account in this.Descriptor.CreditorBankAccounts)
                {
                    Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans");

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram", "TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram", "Information", this.Descriptor.PaymentMeans.Information, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                        if (this.Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            Writer.WriteStartElement("ram", "ApplicableTradeSettlementFinancialCard", Profile.Comfort | Profile.Extended | Profile.XRechnung);
                            Writer.WriteOptionalElementString("ram", "ID", Descriptor.PaymentMeans.FinancialCard.Id);
                            Writer.WriteOptionalElementString("ram", "CardholderName", Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            Writer.WriteEndElement(); // !ram:ApplicableTradeSettlementFinancialCard
                        }
                    }

                    Writer.WriteStartElement("ram", "PayeePartyCreditorFinancialAccount");
                    Writer.WriteElementString("ram", "IBANID", account.IBAN);
                    Writer.WriteOptionalElementString("ram", "AccountName", account.Name, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    Writer.WriteOptionalElementString("ram", "ProprietaryID", account.ID);
                    Writer.WriteEndElement(); // !PayeePartyCreditorFinancialAccount

                    if (!String.IsNullOrWhiteSpace(account.BIC))
                    {
                        Writer.WriteStartElement("ram", "PayeeSpecifiedCreditorFinancialInstitution", Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                        Writer.WriteElementString("ram", "BICID", account.BIC);
                        Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                    }

                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }

                foreach (BankAccount account in this.Descriptor.DebitorBankAccounts)
                {
                    Writer.WriteStartElement("ram", "SpecifiedTradeSettlementPaymentMeans"); // BG-16

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("ram", "TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("ram", "Information", this.Descriptor.PaymentMeans.Information, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    }

                    Writer.WriteStartElement("ram", "PayerPartyDebtorFinancialAccount");
                    Writer.WriteElementString("ram", "IBANID", account.IBAN);
                    Writer.WriteOptionalElementString("ram", "AccountName", account.Name, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    Writer.WriteOptionalElementString("ram", "ProprietaryID", account.ID);
                    Writer.WriteEndElement(); // !PayerPartyDebtorFinancialAccount

                    if (!String.IsNullOrWhiteSpace(account.BIC))
                    {
                        Writer.WriteStartElement("ram", "PayerSpecifiedDebtorFinancialInstitution");
                        Writer.WriteElementString("ram", "BICID", account.BIC);
                        Writer.WriteEndElement(); // !PayerSpecifiedDebtorFinancialInstitution
                    }

                    Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                }
            }
            #endregion

            #region ApplicableTradeTax
            //  11. ApplicableTradeTax (optional)
            _writeOptionalTaxes(Writer);
            #endregion

            #region BillingSpecifiedPeriod
            //  12. BillingSpecifiedPeriod (optional)
            if (Descriptor.BillingPeriodStart.HasValue || Descriptor.BillingPeriodEnd.HasValue)
            {
                Writer.WriteStartElement("ram", "BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                if (Descriptor.BillingPeriodStart.HasValue)
                {
                    Writer.WriteStartElement("ram", "StartDateTime");
                    _writeElementWithAttributeWithPrefix(Writer, "udt", "DateTimeString", "format", "102", _formatDate(this.Descriptor.BillingPeriodStart.Value));
                    Writer.WriteEndElement(); // !StartDateTime
                }

                if (Descriptor.BillingPeriodEnd.HasValue)
                {
                    Writer.WriteStartElement("ram", "EndDateTime");
                    _writeElementWithAttributeWithPrefix(Writer, "udt", "DateTimeString", "format", "102", _formatDate(this.Descriptor.BillingPeriodEnd.Value));
                    Writer.WriteEndElement(); // !EndDateTime
                }
                Writer.WriteEndElement(); // !BillingSpecifiedPeriod
            }
            #endregion

            //  13. SpecifiedTradeAllowanceCharge (optional)
            foreach (TradeAllowanceCharge tradeAllowanceCharge in this.Descriptor.GetTradeAllowanceCharges())
            {
                Writer.WriteStartElement("ram", "SpecifiedTradeAllowanceCharge");
                Writer.WriteStartElement("ram", "ChargeIndicator");
                Writer.WriteElementString("udt", "Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                Writer.WriteEndElement(); // !ram:ChargeIndicator

                if (tradeAllowanceCharge.ChargePercentage.HasValue)
                {
                    Writer.WriteStartElement("ram", "CalculationPercent", profile: Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ChargePercentage.Value));
                    Writer.WriteEndElement();
                }

                if (tradeAllowanceCharge.BasisAmount.HasValue)
                {
                    Writer.WriteStartElement("ram", "BasisAmount", profile: Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount.Value));
                    Writer.WriteEndElement();
                }

                Writer.WriteStartElement("ram", "ActualAmount");
                Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 2));
                Writer.WriteEndElement();


                Writer.WriteOptionalElementString("ram", "Reason", tradeAllowanceCharge.Reason);

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
            foreach (ServiceCharge serviceCharge in this.Descriptor.ServiceCharges)
            {
                Writer.WriteStartElement("ram", "SpecifiedLogisticsServiceCharge", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung));
                Writer.WriteOptionalElementString("ram", "Description", serviceCharge.Description);
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
                            // every line break must be a valid xml line break.
                            // if a note already exists, append a valid line break.
                            if (sbPaymentNotes.Length > 0)
                            {
                                sbPaymentNotes.Append(XmlConstants.XmlNewLine);
                            }

                            if (paymentTerms.PaymentTermsType.HasValue)
                            {
                                // also write the description if it exists.
                                if (!string.IsNullOrWhiteSpace(paymentTerms.Description))
                                {
                                    sbPaymentNotes.Append(paymentTerms.Description);
                                    sbPaymentNotes.Append(XmlConstants.XmlNewLine);
                                }

                                sbPaymentNotes.Append($"#{((PaymentTermsType)paymentTerms.PaymentTermsType).EnumToString<PaymentTermsType>().ToUpper()}");
                                sbPaymentNotes.Append($"#TAGE={paymentTerms.DueDays}");
                                sbPaymentNotes.Append($"#PROZENT={_formatDecimal(paymentTerms.Percentage)}");
                                sbPaymentNotes.Append(paymentTerms.BaseAmount.HasValue ? $"#BASISBETRAG={_formatDecimal(paymentTerms.BaseAmount)}" : "");
                                sbPaymentNotes.Append("#");
                            }
                            else
                            {
                                sbPaymentNotes.Append(paymentTerms.Description);
                            }
                            dueDate = dueDate ?? paymentTerms.DueDate;
                        }
                        Writer.WriteOptionalElementString("ram", "Description", sbPaymentNotes.ToString());
                        if (dueDate.HasValue)
                        {
                            Writer.WriteStartElement("ram", "DueDateDateTime");
                            _writeElementWithAttributeWithPrefix(Writer, "udt", "DateTimeString", "format", "102", _formatDate(dueDate.Value));
                            Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }

                        // BT-89 is only required/allowed on DirectDebit (BR-DE-29)
                        if (this.Descriptor.PaymentMeans?.TypeCode == PaymentMeansTypeCodes.DirectDebit || this.Descriptor.PaymentMeans?.TypeCode == PaymentMeansTypeCodes.SEPADirectDebit)
                        {
                            Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", Descriptor.PaymentMeans?.SEPAMandateReference);
                        }
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
                            _writeElementWithAttributeWithPrefix(Writer, "udt", "DateTimeString", "format", "102", _formatDate(paymentTerms.DueDate.Value));
                            Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
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
                            _writeElementWithAttributeWithPrefix(Writer, "udt", "DateTimeString", "format", "102", _formatDate(dueDate.Value));
                            Writer.WriteEndElement(); // !ram:DueDateDateTime
                        }
                        Writer.WriteOptionalElementString("ram", "DirectDebitMandateID", Descriptor.PaymentMeans?.SEPAMandateReference);
                        Writer.WriteEndElement();
                    }
                    break;
            }

            #region SpecifiedTradeSettlementHeaderMonetarySummation
            //Gesamtsummen auf Dokumentenebene
            Writer.WriteStartElement("ram", "SpecifiedTradeSettlementHeaderMonetarySummation");
            _writeOptionalAmount(Writer, "ram", "LineTotalAmount", this.Descriptor.LineTotalAmount, profile: ALL_PROFILES ^ Profile.Minimum);   // Summe der Nettobeträge aller Rechnungspositionen
            _writeAmount(Writer, "ram", "ChargeTotalAmount", this.Descriptor.ChargeTotalAmount, profile: ALL_PROFILES ^ Profile.Minimum);       // Summe der Zuschläge auf Dokumentenebene, BT-108
            _writeAmount(Writer, "ram", "AllowanceTotalAmount", this.Descriptor.AllowanceTotalAmount, profile: ALL_PROFILES ^ Profile.Minimum); // Summe der Abschläge auf Dokumentenebene, BT-107
                                                                                                                                                // both fields are mandatory according to BR-FXEXT-CO-11
                                                                                                                                                // and BR-FXEXT-CO-12

            if (this.Descriptor.Profile == Profile.Extended)
            {
                // there shall be no currency for tax basis total amount, see
                // https://github.com/stephanstapel/ZUGFeRD-csharp/issues/56#issuecomment-655525467
                _writeOptionalAmount(Writer, "ram", "TaxBasisTotalAmount", this.Descriptor.TaxBasisAmount, forceCurrency: false);   // Rechnungsgesamtbetrag ohne Umsatzsteuer
            }
            else
            {
                _writeOptionalAmount(Writer, "ram", "TaxBasisTotalAmount", this.Descriptor.TaxBasisAmount);   // Rechnungsgesamtbetrag ohne Umsatzsteuer
            }
            _writeOptionalAmount(Writer, "ram", "TaxTotalAmount", this.Descriptor.TaxTotalAmount, forceCurrency: true);               // Gesamtbetrag der Rechnungsumsatzsteuer, Steuergesamtbetrag in Buchungswährung
            _writeOptionalAmount(Writer, "ram", "RoundingAmount", this.Descriptor.RoundingAmount, profile: Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);  // RoundingAmount  //Rundungsbetrag
            _writeOptionalAmount(Writer, "ram", "GrandTotalAmount", this.Descriptor.GrandTotalAmount);                                // Rechnungsgesamtbetrag einschließlich Umsatzsteuer
            _writeOptionalAmount(Writer, "ram", "TotalPrepaidAmount", this.Descriptor.TotalPrepaidAmount);                            // Vorauszahlungsbetrag
            _writeOptionalAmount(Writer, "ram", "DuePayableAmount", this.Descriptor.DuePayableAmount);                                // Fälliger Zahlungsbetrag
            Writer.WriteEndElement(); // !ram:SpecifiedTradeSettlementMonetarySummation
            #endregion

            #region InvoiceReferencedDocument
            foreach (InvoiceReferencedDocument invoiceReferencedDocument in this.Descriptor.GetInvoiceReferencedDocuments())
            {
                Writer.WriteStartElement("ram", "InvoiceReferencedDocument", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                Writer.WriteOptionalElementString("ram", "IssuerAssignedID", invoiceReferencedDocument.ID);
                if (invoiceReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                    _writeElementWithAttributeWithPrefix(Writer, "qdt", "DateTimeString", "format", "102", _formatDate(invoiceReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }
                Writer.WriteEndElement(); // !ram:InvoiceReferencedDocument
            }
            #endregion

            #region ReceivableSpecifiedTradeAccountingAccount
            if (this.Descriptor.ReceivableSpecifiedTradeAccountingAccounts != null && this.Descriptor.ReceivableSpecifiedTradeAccountingAccounts.Count > 0)
            {
                if (descriptor.Profile == Profile.XRechnung1 || descriptor.Profile == Profile.XRechnung)
                {
                    if (!string.IsNullOrWhiteSpace(this.Descriptor.ReceivableSpecifiedTradeAccountingAccounts[0].TradeAccountID))
                    {
                        Writer.WriteStartElement("ram", "ReceivableSpecifiedTradeAccountingAccount");
                        {
                            //BT-19
                            Writer.WriteStartElement("ram", "ID");
                            Writer.WriteValue(this.Descriptor.ReceivableSpecifiedTradeAccountingAccounts[0].TradeAccountID);
                            Writer.WriteEndElement(); // !ram:ID
                        }
                        Writer.WriteEndElement(); // !ram:ReceivableSpecifiedTradeAccountingAccount
                    }
                }
                else
                {
                    foreach (ReceivableSpecifiedTradeAccountingAccount RSTAA in this.Descriptor.ReceivableSpecifiedTradeAccountingAccounts)
                    {
                        Writer.WriteStartElement("ram", "ReceivableSpecifiedTradeAccountingAccount", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended);

                        {
                            //BT-19
                            Writer.WriteStartElement("ram", "ID", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended);
                            Writer.WriteValue(RSTAA.TradeAccountID);
                            Writer.WriteEndElement(); // !ram:ID
                        }

                        if (RSTAA.TradeAccountTypeCode != AccountingAccountTypeCodes.Unknown)
                        {
                            Writer.WriteStartElement("ram", "TypeCode", Profile.Extended);
                            Writer.WriteValue(((int)RSTAA.TradeAccountTypeCode).ToString());
                            Writer.WriteEndElement(); // !ram:TypeCode
                        }

                        Writer.WriteEndElement(); // !ram:ReceivableSpecifiedTradeAccountingAccount
                    }
                }
            }
            #endregion
            Writer.WriteEndElement(); // !ram:ApplicableHeaderTradeSettlement

            #endregion

            Writer.WriteEndElement(); // !ram:SpecifiedSupplyChainTradeTransaction
            #endregion

            Writer.WriteEndElement(); // !ram:Invoice
            Writer.WriteEndDocument();
            Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);
        } // !Save()

        private void _writeAdditionalReferencedDocument(AdditionalReferencedDocument document, Profile profile)
        {
            Writer.WriteStartElement("ram", "AdditionalReferencedDocument", profile);
            Writer.WriteElementString("ram", "IssuerAssignedID", document.ID);

            if (document.TypeCode != AdditionalReferencedDocumentTypeCode.Unknown)
            {
                Writer.WriteElementString("ram", "TypeCode", document.TypeCode.EnumValueToString());
            }

            if (document.ReferenceTypeCode != ReferenceTypeCodes.Unknown)
            {
                Writer.WriteElementString("ram", "ReferenceTypeCode", document.ReferenceTypeCode.EnumToString());
            }

            Writer.WriteOptionalElementString("ram", "Name", document.Name);

            if (document.AttachmentBinaryObject != null)
            {
                Writer.WriteStartElement("ram", "AttachmentBinaryObject");
                Writer.WriteAttributeString("filename", document.Filename);
                Writer.WriteAttributeString("mimeCode", MimeTypeMapper.GetMimeType(document.Filename));
                Writer.WriteValue(Convert.ToBase64String(document.AttachmentBinaryObject));
                Writer.WriteEndElement(); // !AttachmentBinaryObject()
            }

            if (document.IssueDateTime.HasValue)
            {
                Writer.WriteStartElement("ram", "FormattedIssueDateTime");
                Writer.WriteStartElement("qdt", "DateTimeString");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(document.IssueDateTime.Value));
                Writer.WriteEndElement(); // !qdt:DateTimeString
                Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
            }

            Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
        } // !_writeAdditionalReferencedDocument()


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


        private void _writeAmount(ProfileAwareXmlTextWriter writer, string prefix, string tagName, decimal? value, decimal defaultValue = 0m, int numDecimals = 2, bool forceCurrency = false, Profile profile = Profile.Unknown)
        {
            decimal _value = defaultValue;
            if (value.HasValue)
            {
                _value = value.Value;
            }

            writer.WriteStartElement(prefix, tagName, profile);
            if (forceCurrency)
            {
                writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
            }
            writer.WriteValue(_formatDecimal(_value, numDecimals));
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


        private void _writeOptionalTaxes(ProfileAwareXmlTextWriter writer)
        {
            foreach (Tax tax in this.Descriptor.Taxes)
            {
                writer.WriteStartElement("ram", "ApplicableTradeTax");

                writer.WriteStartElement("ram", "CalculatedAmount");
                writer.WriteValue(_formatDecimal(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount

                writer.WriteElementString("ram", "TypeCode", tax.TypeCode.EnumToString());
                writer.WriteOptionalElementString("ram", "ExemptionReason", tax.ExemptionReason);
                writer.WriteStartElement("ram", "BasisAmount");
                writer.WriteValue(_formatDecimal(tax.BasisAmount));
                writer.WriteEndElement(); // !BasisAmount

                if (tax.AllowanceChargeBasisAmount.HasValue && tax.AllowanceChargeBasisAmount.Value != 0)
                {
                    writer.WriteStartElement("ram", "AllowanceChargeBasisAmount", Profile.Extended);
                    writer.WriteValue(_formatDecimal(tax.AllowanceChargeBasisAmount));
                    writer.WriteEndElement(); // !AllowanceChargeBasisAmount
                }
                if (tax.LineTotalBasisAmount.HasValue && tax.LineTotalBasisAmount.Value != 0)
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
                writer.WriteEndElement(); // !RateApplicablePercent
            }
        } // !_writeOptionalTaxes()


        private void _writeNotes(ProfileAwareXmlTextWriter writer, List<Note> notes, Profile profile = Profile.Unknown)
        {
            if (notes.Count > 0)
            {
                foreach (Note note in notes)
                {
                    writer.WriteStartElement("ram", "IncludedNote", profile);
                    if (note.ContentCode != ContentCodes.Unknown)
                    {
                        writer.WriteElementString("ram", "ContentCode", note.ContentCode.EnumToString());
                    }
                    writer.WriteOptionalElementString("ram", "Content", note.Content);
                    if (note.SubjectCode != SubjectCodes.Unknown)
                    {
                        writer.WriteElementString("ram", "SubjectCode", note.SubjectCode.EnumToString());
                    }
                    writer.WriteEndElement();
                }
            }
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
                    // all profiles
                    break;
                case PartyTypes.BuyerTradeParty:
                    // all profiles
                    break;
                case PartyTypes.ShipToTradeParty:
                    if (this.Descriptor.Profile == Profile.Minimum) { return; } // it is also possible to add ShipToTradeParty() to a LineItem. In this case, the correct profile filter is different!
                    break;
                case PartyTypes.UltimateShipToTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                case PartyTypes.ShipFromTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                case PartyTypes.InvoiceeTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                case PartyTypes.PayeeTradeParty:
                    // all profiles
                    break;
                case PartyTypes.SalesAgentTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                case PartyTypes.BuyerTaxRepresentativeTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                case PartyTypes.ProductEndUserTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                case PartyTypes.BuyerAgentTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                case PartyTypes.InvoicerTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                case PartyTypes.PayerTradeParty:
                    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                    break;
                default:
                    return;
            }

            writer.WriteStartElement(prefix, legalOrganizationTag, this.Descriptor.Profile);
            if (legalOrganization.ID != null)
            {
                if (!String.IsNullOrWhiteSpace(legalOrganization.ID.ID) && legalOrganization.ID.SchemeID.HasValue && !String.IsNullOrWhiteSpace(legalOrganization.ID.SchemeID.Value.EnumToString()))
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


                // filter according to https://github.com/stephanstapel/ZUGFeRD-csharp/pull/221
                if ((this.Descriptor.Profile == Profile.Minimum && partyType.In(PartyTypes.SellerTradeParty, PartyTypes.PayeeTradeParty, PartyTypes.BuyerTradeParty)) ||
                    (this.Descriptor.Profile == Profile.Extended)) /* remaining party types */
                {
                    writer.WriteOptionalElementString("ram", "TradingBusinessName", legalOrganization.TradingBusinessName, this.Descriptor.Profile);
                }
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
            }

            if (party.ID != null && !string.IsNullOrWhiteSpace(party.ID.ID))
            {
                if (party.ID.SchemeID.HasValue && (party.ID.SchemeID.Value != GlobalIDSchemeIdentifiers.Unknown))
                {
                    writer.WriteStartElement("ram", "ID");
                    writer.WriteAttributeString("schemeID", party.ID.SchemeID.Value.EnumToString());
                    writer.WriteValue(party.ID.ID);
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteOptionalElementString("ram", "ID", party.ID.ID);
                }
            }

            if ((party.GlobalID != null) && !String.IsNullOrWhiteSpace(party.GlobalID.ID) && party.GlobalID.SchemeID.HasValue && (party.GlobalID.SchemeID.Value != GlobalIDSchemeIdentifiers.Unknown))
            {
                writer.WriteStartElement("ram", "GlobalID");
                writer.WriteAttributeString("schemeID", party.GlobalID.SchemeID.Value.EnumToString());
                writer.WriteValue(party.GlobalID.ID);
                writer.WriteEndElement();
            }

            writer.WriteOptionalElementString("ram", "Name", party.Name);
            writer.WriteOptionalElementString("ram", "Description", party.Description, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

            _writeOptionalLegalOrganization(writer, "ram", "SpecifiedLegalOrganization", party.SpecifiedLegalOrganization, partyType);
            _writeOptionalContact(writer, "ram", "DefinedTradeContact", contact, Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

            // spec 2.3 says: Minimum/BuyerTradeParty does not include PostalTradeAddress
            if ((this.Descriptor.Profile == Profile.Extended) || partyType.In(PartyTypes.BuyerTradeParty, PartyTypes.SellerTradeParty, PartyTypes.BuyerTaxRepresentativeTradeParty, PartyTypes.ShipToTradeParty, PartyTypes.ShipToTradeParty, PartyTypes.UltimateShipToTradeParty, PartyTypes.SalesAgentTradeParty))
            {
                writer.WriteStartElement("ram", "PostalTradeAddress");
                writer.WriteOptionalElementString("ram", "PostcodeCode", party.Postcode); // buyer: BT-53
                writer.WriteOptionalElementString("ram", "LineOne", string.IsNullOrWhiteSpace(party.ContactName) ? party.Street : party.ContactName); // buyer: BT-50
                if (!string.IsNullOrWhiteSpace(party.ContactName)) { writer.WriteOptionalElementString("ram", "LineTwo", party.Street); } // buyer: BT-51
                writer.WriteOptionalElementString("ram", "LineThree", party.AddressLine3); // buyer: BT-163
                writer.WriteOptionalElementString("ram", "CityName", party.City); // buyer: BT-52

                if (party.Country != CountryCodes.Unknown)
                {
                    writer.WriteElementString("ram", "CountryID", party.Country.EnumToString()); // buyer: BT-55
                }

                writer.WriteOptionalElementString("ram", "CountrySubDivisionName", party.CountrySubdivisionName); // BT-79
                writer.WriteEndElement(); // !PostalTradeAddress
            }

            if (electronicAddress != null)
            {
                if (!String.IsNullOrWhiteSpace(electronicAddress.Address))
                {
                    writer.WriteStartElement("ram", "URIUniversalCommunication");
                    writer.WriteStartElement("ram", "URIID");
                    writer.WriteAttributeString("schemeID", electronicAddress.ElectronicAddressSchemeID.EnumToString());
                    writer.WriteValue(electronicAddress.Address);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }

            if (taxRegistrations != null)
            {
                // for seller: BT-31
                // for buyer : BT-48
                foreach (TaxRegistration _reg in taxRegistrations)
                {
                    if (!String.IsNullOrWhiteSpace(_reg.No))
                    {
                        writer.WriteStartElement("ram", "SpecifiedTaxRegistration");
                        writer.WriteStartElement("ram", "ID");
                        writer.WriteAttributeString("schemeID", _reg.SchemeID.EnumToString());
                        writer.WriteValue(_reg.No);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
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
                writer.WriteStartElement("ram", "FaxUniversalCommunication", ALL_PROFILES ^ (Profile.XRechnung1 | Profile.XRechnung));
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


        private string _translateTaxCategoryCode(TaxCategoryCodes taxCategoryCode)
        {
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
                    return "Normalsatz";
                case TaxCategoryCodes.Z:
                    return "nach dem Nullsatz zu versteuernde Waren";
                case TaxCategoryCodes.Unknown:
                    break;
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
                case InvoiceType.Correction:
                case InvoiceType.CorrectionOld: return "KORREKTURRECHNUNG";
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

            if (type == InvoiceType.CorrectionOld)
            {
                return (int)InvoiceType.Correction;
            }

            return EnumExtensions.EnumToInt<InvoiceType>(type);
        } // !_translateInvoiceType()


        /// <summary>
        /// This function is implemented in class InvoiceDescriptor22Writer.
        /// </summary>
        internal override bool Validate(InvoiceDescriptor descriptor, bool throwExceptions = true)
        {
            return false;
        } // !Validate()
    }
}
