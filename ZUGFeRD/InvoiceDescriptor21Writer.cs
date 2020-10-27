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
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using System.IO;


namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor21Writer : IInvoiceDescriptorWriter
    {
        private ProfileAwareXmlTextWriter Writer;
        private InvoiceDescriptor Descriptor;


        private readonly Profile ALL_PROFILES = Profile.Minimum | Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung;


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

            long streamPosition = stream.Position;

            this.Descriptor = descriptor;
            this.Writer = new ProfileAwareXmlTextWriter(stream, Encoding.UTF8, descriptor.Profile);
            Writer.Formatting = Formatting.Indented;
            Writer.WriteStartDocument();

            #region Kopfbereich
            Writer.WriteStartElement("rsm:CrossIndustryInvoice");
            Writer.WriteAttributeString("xmlns", "a", null, "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            Writer.WriteAttributeString("xmlns", "rsm", null, "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            Writer.WriteAttributeString("xmlns", "qdt", null, "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            Writer.WriteAttributeString("xmlns", "ram", null, "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            Writer.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");
            Writer.WriteAttributeString("xmlns", "udt", null, "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");
            #endregion

            #region ExchangedDocumentContext
            //Prozesssteuerung
            Writer.WriteStartElement("rsm:ExchangedDocumentContext");
            if (this.Descriptor.IsTest)
            {
                Writer.WriteStartElement("ram:TestIndicator");
                Writer.WriteElementString("udt:Indicator", "true");
                Writer.WriteEndElement(); // !ram:TestIndicator
            }
            Writer.WriteStartElement("ram:GuidelineSpecifiedDocumentContextParameter");
            //Gruppierung der Anwendungsempfehlungsinformationen
            Writer.WriteElementString("ram:ID", this.Descriptor.Profile.EnumToString(ZUGFeRDVersion.Version21));
            Writer.WriteEndElement(); // !ram:GuidelineSpecifiedDocumentContextParameter
            Writer.WriteEndElement(); // !rsm:ExchangedDocumentContext
            #endregion

            #region ExchangedDocument
            //Gruppierung der Eigenschaften, die das gesamte Dokument betreffen.
            Writer.WriteStartElement("rsm:ExchangedDocument");
            Writer.WriteElementString("ram:ID", this.Descriptor.InvoiceNo); //Rechnungsnummer
            Writer.WriteElementString("ram:Name", _translateInvoiceType(this.Descriptor.Type), Profile.Extended); //Dokumentenart (Freitext)
            Writer.WriteElementString("ram:TypeCode", String.Format("{0}", _encodeInvoiceType(this.Descriptor.Type))); //Code für den Rechnungstyp
            //ToDo: LanguageID      //Sprachkennzeichen
            //ToDo: IncludedNote    //Freitext zur Rechnung
            if (this.Descriptor.InvoiceDate.HasValue)
            {
                Writer.WriteStartElement("ram:IssueDateTime");
                Writer.WriteStartElement("udt:DateTimeString");  //Rechnungsdatum
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
            //Gruppierung der Informationen zum Geschäftsvorfall
            Writer.WriteStartElement("rsm:SupplyChainTradeTransaction");

            #region  IncludedSupplyChainTradeLineItem
            foreach (TradeLineItem tradeLineItem in this.Descriptor.TradeLineItems)
            {
                Writer.WriteStartElement("ram:IncludedSupplyChainTradeLineItem");

                #region AssociatedDocumentLineDocument
                //Gruppierung von allgemeinen Positionsangaben
                if (tradeLineItem.AssociatedDocument != null)
                {
                    Writer.WriteStartElement("ram:AssociatedDocumentLineDocument", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);
                    if (tradeLineItem.AssociatedDocument.LineID.HasValue)
                    {
                        Writer.WriteElementString("ram:LineID", String.Format("{0}", tradeLineItem.AssociatedDocument.LineID));
                    }
                    _writeNotes(Writer, tradeLineItem.AssociatedDocument.Notes);
                    Writer.WriteEndElement(); // ram:AssociatedDocumentLineDocument(Basic|Comfort|Extended|XRechnung)
                }
                #endregion

                // handelt es sich um einen Kommentar?
                bool isCommentItem = false;
                if ((tradeLineItem.AssociatedDocument?.Notes.Count > 0) && (tradeLineItem.BilledQuantity == 0) && (String.IsNullOrEmpty(tradeLineItem.Description)))
                {
                    isCommentItem = true;
                }

                #region SpecifiedTradeProduct
                //Eine Gruppe von betriebswirtschaftlichen Begriffen, die Informationen über die in Rechnung gestellten Waren und Dienstleistungen enthält
                Writer.WriteStartElement("ram:SpecifiedTradeProduct", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);
                if ((tradeLineItem.GlobalID != null) && !String.IsNullOrEmpty(tradeLineItem.GlobalID.SchemeID) && !String.IsNullOrEmpty(tradeLineItem.GlobalID.ID))
                {
                    _writeElementWithAttribute(Writer, "ram:GlobalID", "schemeID", tradeLineItem.GlobalID.SchemeID, tradeLineItem.GlobalID.ID);
                }

                _writeOptionalElementString(Writer, "ram:SellerAssignedID", tradeLineItem.SellerAssignedID);
                _writeOptionalElementString(Writer, "ram:BuyerAssignedID", tradeLineItem.BuyerAssignedID);
                _writeOptionalElementString(Writer, "ram:Name", tradeLineItem.Name, ALL_PROFILES ^ Profile.XRechnung);
                _writeOptionalElementString(Writer, "ram:Name", !isCommentItem ? tradeLineItem.Name : "TEXT", Profile.XRechnung); // XRechnung erfordert einen Item-Namen
                _writeOptionalElementString(Writer, "ram:Description", tradeLineItem.Description);

                Writer.WriteEndElement(); // !ram:SpecifiedTradeProduct(Basic|Comfort|Extended|XRechnung)
                #endregion

                #region SpecifiedLineTradeAgreement (Basic, Comfort, Extended, XRechnung)
                //Eine Gruppe von betriebswirtschaftlichen Begriffen, die Informationen über den Preis für die in der betreffenden Rechnungsposition in Rechnung gestellten Waren und Dienstleistungen enthält

                if (new Profile[] { Profile.Basic, Profile.Comfort, Profile.Extended, Profile.XRechnung }.Contains(descriptor.Profile))
                {                 
                    Writer.WriteStartElement("ram:SpecifiedLineTradeAgreement", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);

                    #region BuyerOrderReferencedDocument (Comfort, Extended, XRechnung)
                    //Detailangaben zur zugehörigen Bestellung                   
                    if (tradeLineItem.BuyerOrderReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram:BuyerOrderReferencedDocument", Profile.Comfort | Profile.Extended | Profile.XRechnung);

                        #region IssuerAssignedID
                        //Bestellnummer
                        if (!String.IsNullOrEmpty(tradeLineItem.BuyerOrderReferencedDocument.ID))
                        {
                            Writer.WriteElementString("ram:IssuerAssignedID", tradeLineItem.BuyerOrderReferencedDocument.ID);
                        }
                        #endregion

                        #region LineID
                        //Referenz zur Bestellposition
                        //ToDo: fehlt ganz
                        #endregion

                        #region IssueDateTime
                        if (tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.HasValue)
                        {
                            Writer.WriteStartElement("ram:FormattedIssueDateTime");
                            Writer.WriteStartElement("qdt:DateTimeString");
                            Writer.WriteAttributeString("format", "102");
                            Writer.WriteValue(_formatDate(tradeLineItem.BuyerOrderReferencedDocument.IssueDateTime.Value));
                            Writer.WriteEndElement(); // !qdt:DateTimeString
                            Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                        }
                        #endregion

                        Writer.WriteEndElement(); // !ram:BuyerOrderReferencedDocument
                    }
                    #endregion

                    #region ContractReferencedDocument
                    //Detailangaben zum zugehörigen Vertrag
                    if (tradeLineItem.ContractReferencedDocument != null)
                    {
                        Writer.WriteStartElement("ram:ContractReferencedDocument", Profile.Extended);
                        if (tradeLineItem.ContractReferencedDocument.IssueDateTime.HasValue)
                        {
                            Writer.WriteStartElement("ram:IssueDateTime");
                            Writer.WriteStartElement("udt:DateTimeString");
                            Writer.WriteAttributeString("format", "102");
                            Writer.WriteValue(_formatDate(tradeLineItem.ContractReferencedDocument.IssueDateTime.Value));
                            Writer.WriteEndElement(); // !udt:DateTimeString
                            Writer.WriteEndElement(); // !ram:IssueDateTime
                        }
                        if (!String.IsNullOrEmpty(tradeLineItem.ContractReferencedDocument.ID))
                        {
                            Writer.WriteElementString("ram:IssuerAssignedID", tradeLineItem.ContractReferencedDocument.ID);
                        }

                        Writer.WriteEndElement(); // !ram:ContractReferencedDocument(Extended)
                    }
                    #endregion

                    #region AdditionalReferencedDocument (Extended)

                    //Detailangaben zu einer zusätzlichen Dokumentenreferenz                        
                    if (tradeLineItem.AdditionalReferencedDocuments != null)
                    {
                        foreach (AdditionalReferencedDocument document in tradeLineItem.AdditionalReferencedDocuments)
                        {
                            Writer.WriteStartElement("ram:AdditionalReferencedDocument", Profile.Extended);
                            if (document.IssueDateTime.HasValue)
                            {
                                Writer.WriteStartElement("ram:IssueDateTime");
                                Writer.WriteStartElement("udt:DateTimeString");
                                Writer.WriteAttributeString("format", "102");
                                Writer.WriteValue(_formatDate(document.IssueDateTime.Value));
                                Writer.WriteEndElement(); // !udt:DateTimeString
                                Writer.WriteEndElement(); // !ram:IssueDateTime
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
                    #endregion

                    #region GrossPriceProductTradePrice (Comfort, Extended, XRechnung)                 
                    Writer.WriteStartElement("ram:GrossPriceProductTradePrice", Profile.Comfort | Profile.Extended | Profile.XRechnung);
                    _writeOptionalAmount(Writer, "ram:ChargeAmount", tradeLineItem.GrossUnitPrice, 2);
                    if (tradeLineItem.UnitQuantity.HasValue)
                    {
                        _writeElementWithAttribute(Writer, "ram:BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 2));
                    }                    

                    #region AppliedTradeAllowanceCharge
                    foreach (TradeAllowanceCharge tradeAllowanceCharge in tradeLineItem.TradeAllowanceCharges)
                    {
                        Writer.WriteStartElement("ram:AppliedTradeAllowanceCharge");

                        #region ChargeIndicator
                        Writer.WriteStartElement("ram:ChargeIndicator");
                        Writer.WriteElementString("udt:Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                        Writer.WriteEndElement(); // !ram:ChargeIndicator
                        #endregion

                        #region BasisAmount
                        Writer.WriteStartElement("ram:BasisAmount", profile: Profile.Extended | Profile.XRechnung);
                        Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount, 2));
                        Writer.WriteEndElement();
                        #endregion

                        #region ActualAmount
                        Writer.WriteStartElement("ram:ActualAmount");
                        Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 2));
                        Writer.WriteEndElement();
                        #endregion

                        _writeOptionalElementString(Writer, "ram:Reason", tradeAllowanceCharge.Reason, Profile.Extended | Profile.XRechnung);

                        Writer.WriteEndElement(); // !AppliedTradeAllowanceCharge
                    }

                    Writer.WriteEndElement(); // ram:GrossPriceProductTradePrice(Comfort|Extended|XRechnung)
                    #endregion
                    #endregion // !GrossPriceProductTradePrice(Comfort|Extended|XRechnung)

                    #region NetPriceProductTradePrice                    
                    //Im Nettopreis sind alle Zu- und Abschläge enthalten, jedoch nicht die Umsatzsteuer.
                    Writer.WriteStartElement("ram:NetPriceProductTradePrice", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);
                    _writeOptionalAmount(Writer, "ram:ChargeAmount", tradeLineItem.NetUnitPrice, 2);

                    if (tradeLineItem.UnitQuantity.HasValue)
                    {
                        _writeElementWithAttribute(Writer, "ram:BasisQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.UnitQuantity.Value, 2));
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
                Writer.WriteStartElement("ram:SpecifiedLineTradeDelivery", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);
                _writeElementWithAttribute(Writer, "ram:BilledQuantity", "unitCode", tradeLineItem.UnitCode.EnumToString(), _formatDecimal(tradeLineItem.BilledQuantity, 2));                

                if (tradeLineItem.DeliveryNoteReferencedDocument != null)
                {
                    Writer.WriteStartElement("ram:DeliveryNoteReferencedDocument", ALL_PROFILES ^ Profile.XRechnung);
                    if (!String.IsNullOrEmpty(tradeLineItem.DeliveryNoteReferencedDocument.ID))
                    {
                        Writer.WriteElementString("ram:IssuerAssignedID", tradeLineItem.DeliveryNoteReferencedDocument.ID);
                    }

                    if (tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                    {
                        Writer.WriteStartElement("ram:FormattedIssueDateTime");
                        Writer.WriteStartElement("qdt:DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(tradeLineItem.DeliveryNoteReferencedDocument.IssueDateTime.Value, false));
                        Writer.WriteEndElement(); // "qdt:DateTimeString
                        Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
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
                #endregion

                #region SpecifiedLineTradeSettlement                
                Writer.WriteStartElement("ram:SpecifiedLineTradeSettlement"); //ToDo: Prüfen               
                #region ApplicableTradeTax
                Writer.WriteStartElement("ram:ApplicableTradeTax", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);
                Writer.WriteElementString("ram:TypeCode", tradeLineItem.TaxType.EnumToString());
                if (!String.IsNullOrEmpty(_translateTaxCategoryCode(tradeLineItem.TaxCategoryCode)))
                {
                    Writer.WriteElementString("ram:ExemptionReason", _translateTaxCategoryCode(tradeLineItem.TaxCategoryCode));
                }
                Writer.WriteElementString("ram:CategoryCode", tradeLineItem.TaxCategoryCode.EnumToString());

                if (tradeLineItem.TaxCategoryCode != TaxCategoryCodes.O) // notwendig, damit die Validierung klappt
                {
                    Writer.WriteElementString("ram:RateApplicablePercent", _formatDecimal(tradeLineItem.TaxPercent));
                }

                Writer.WriteEndElement(); // !ram:ApplicableTradeTax(Basic|Comfort|Extended|XRechnung)
                #endregion // !ApplicableTradeTax(Basic|Comfort|Extended|XRechnung)

                #region BillingSpecifiedPeriod (Comfort, Extended)
                //Eine Gruppe von betriebswirtschaftlichen Begriffen, die Informationen über den für die Rechnungsposition maßgeblichen Zeitraum enthält
                //ToDo: BillingSpecifiedPeriod für Comfort und Extended
                #endregion

                #region SpecifiedTradeAllowanceCharge
                //Abschläge auf Ebene der Rechnungsposition (Basic, Comfort, Extended)               
                //ToDo: SpecifiedTradeAllowanceCharge für Basic, Comfort und Extended
                #endregion

                #region SpecifiedTradeSettlementLineMonetarySummation (Basic, Comfort, Extended)
                //Detailinformationen zu Positionssummen
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

                Writer.WriteStartElement("ram:LineTotalAmount", Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);
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
                //ToDo: ReceivableSpecifiedTradeAccountingAccount
                #endregion

                Writer.WriteEndElement(); // !ram:SpecifiedLineTradeSettlement
                #endregion

                Writer.WriteEndElement(); // !ram:IncludedSupplyChainTradeLineItem
            } // !foreach(tradeLineItem)
            #endregion

            #region ApplicableHeaderTradeAgreement
                Writer.WriteStartElement("ram:ApplicableHeaderTradeAgreement");//CG
                if (!String.IsNullOrEmpty(this.Descriptor.ReferenceOrderNo))
                { 
                    Writer.WriteElementString("ram:BuyerReference", this.Descriptor.ReferenceOrderNo);
                }

                #region SellerTradeParty
                _writeOptionalParty(Writer, "ram:SellerTradeParty", this.Descriptor.Seller, this.Descriptor.SellerContact, this.Descriptor.SellerTaxRegistration, descriptor.Profile);
                #endregion

                #region BuyerTradeParty
                _writeOptionalParty(Writer, "ram:BuyerTradeParty", this.Descriptor.Buyer, this.Descriptor.BuyerContact, this.Descriptor.BuyerTaxRegistration, descriptor.Profile);
                #endregion

                #region BuyerOrderReferencedDocument
                if (this.Descriptor.OrderDate.HasValue || ((this.Descriptor.OrderNo != null) && (this.Descriptor.OrderNo.Length > 0)))
                {
                    Writer.WriteStartElement("ram:BuyerOrderReferencedDocument");
                    Writer.WriteElementString("ram:IssuerAssignedID", this.Descriptor.OrderNo);
                    if (this.Descriptor.OrderDate.HasValue)
                    {
                        Writer.WriteStartElement("ram:FormattedIssueDateTime", ALL_PROFILES ^ Profile.XRechnung);
                        Writer.WriteStartElement("qdt:DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(this.Descriptor.OrderDate.Value));
                        Writer.WriteEndElement(); // !qdt:DateTimeString
                        Writer.WriteEndElement(); // !IssueDateTime()
                    }
                    
                Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }
            #endregion

            #region ContractReferencedDocument
            if (this.Descriptor.ContractReferencedDocument != null)
            {
                Writer.WriteStartElement("ram:ContractReferencedDocument");
                Writer.WriteElementString("ram:IssuerAssignedID", this.Descriptor.ContractReferencedDocument.ID);
                if (this.Descriptor.ContractReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram:FormattedIssueDateTime", ALL_PROFILES ^ Profile.XRechnung);
                    Writer.WriteStartElement("qdt:DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.ContractReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // !qdt:DateTimeString
                    Writer.WriteEndElement(); // !IssueDateTime()                    
                }
             }
             #endregion

            #region AdditionalReferencedDocument
            if (this.Descriptor.AdditionalReferencedDocuments != null)
            { 
                foreach (AdditionalReferencedDocument document in this.Descriptor.AdditionalReferencedDocuments)
                {
                    Writer.WriteStartElement("ram:AdditionalReferencedDocument");
                    Writer.WriteElementString("ram:IssuerAssignedID", document.IssuerAssignedID);
                    Writer.WriteElementString("ram:TypeCode", document.TypeCode.EnumValueToString());                    

                    if (document.ReferenceTypeCode != ReferenceTypeCodes.Unknown)
                    {
                        Writer.WriteElementString("ram:TypeCode", document.ReferenceTypeCode.EnumToString());
                    }

                    if (!String.IsNullOrEmpty(document.Name))
                    {
                        Writer.WriteElementString("ram:Name", document.Name);
                    }

                    if (document.AttachmentBinaryObject != null)
                    {                        
                        Writer.WriteStartElement("ram:AttachmentBinaryObject");
                        Writer.WriteAttributeString("filename", document.Filename);
                        Writer.WriteAttributeString("mimeCode", MimeTypeMapper.GetMimeType(document.Filename));
                        Writer.WriteValue(Convert.ToBase64String(document.AttachmentBinaryObject));
                        Writer.WriteEndElement(); // !AttachmentBinaryObject()
                    }

                    if (document.IssueDateTime.HasValue)
                    {
                        Writer.WriteStartElement("ram:FormattedIssueDateTime");
                        Writer.WriteStartElement("qdt:DateTimeString");
                        Writer.WriteAttributeString("format", "102");
                        Writer.WriteValue(_formatDate(document.IssueDateTime.Value));
                        Writer.WriteEndElement(); // !qdt:DateTimeString
                        Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                    }

                    Writer.WriteEndElement(); // !ram:AdditionalReferencedDocument
                }
            }
            #endregion

            #region SpecifiedProcuringProject
                if (Descriptor.SpecifiedProcuringProject != null)
                {

                    Writer.WriteStartElement("ram:SpecifiedProcuringProject", Profile.Comfort | Profile.Extended | Profile.XRechnung);
                    Writer.WriteElementString("ram:ID", Descriptor.SpecifiedProcuringProject.ID, Profile.Comfort | Profile.Extended | Profile.XRechnung);
                    Writer.WriteElementString("ram:Name", Descriptor.SpecifiedProcuringProject.Name, Profile.Comfort | Profile.Extended | Profile.XRechnung);
                    Writer.WriteEndElement(); // !ram:SpecifiedProcuringProject
                }
                #endregion

            Writer.WriteEndElement(); // !ApplicableHeaderTradeAgreement
            #endregion

            #region ApplicableHeaderTradeDelivery
            Writer.WriteStartElement("ram:ApplicableHeaderTradeDelivery"); // Pflichteintrag
            _writeOptionalParty(Writer, "ram:ShipToTradeParty", this.Descriptor.ShipTo, profile : Profile.Extended);
            //ToDo: UltimateShipToTradeParty
            _writeOptionalParty(Writer, "ram:ShipFromTradeParty", this.Descriptor.ShipFrom, profile: Profile.Extended);

            #region ActualDeliverySupplyChainEvent
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
            #endregion

            #region DeliveryNoteReferencedDocument
            if (this.Descriptor.DeliveryNoteReferencedDocument != null)
            {
                Writer.WriteStartElement("ram:DeliveryNoteReferencedDocument");
                Writer.WriteElementString("ram:IssuerAssignedID", this.Descriptor.DeliveryNoteReferencedDocument.ID);

                if (this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.HasValue)
                {
                    Writer.WriteStartElement("ram:FormattedIssueDateTime", ALL_PROFILES ^ Profile.XRechnung);
                    Writer.WriteStartElement("qdt:DateTimeString");
                    Writer.WriteAttributeString("format", "102");
                    Writer.WriteValue(_formatDate(this.Descriptor.DeliveryNoteReferencedDocument.IssueDateTime.Value));
                    Writer.WriteEndElement(); // "qdt:DateTimeString
                    Writer.WriteEndElement(); // !ram:FormattedIssueDateTime
                }
                
                Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }
            #endregion

            Writer.WriteEndElement(); // !ApplicableHeaderTradeDelivery
            #endregion

            #region ApplicableHeaderTradeSettlement
                Writer.WriteStartElement("ram:ApplicableHeaderTradeSettlement");

                if (Descriptor.Profile == Profile.Extended)
                {
                    _writeOptionalParty(Writer, "ram:InvoiceeTradeParty", this.Descriptor.Invoicee);
                }
                if (Descriptor.Profile != Profile.Minimum)
                {
                    _writeOptionalParty(Writer, "ram:PayeeTradeParty", this.Descriptor.Payee);
                }

                if (!String.IsNullOrEmpty(this.Descriptor.InvoiceNoAsReference))
                {
                    _writeOptionalElementString(Writer, "ram:PaymentReference", this.Descriptor.InvoiceNoAsReference);
                }
                Writer.WriteElementString("ram:InvoiceCurrencyCode", this.Descriptor.Currency.EnumToString());

                #region SpecifiedTradeSettlementPaymentMeans (alle außer Minimum)
                if (this.Descriptor.CreditorBankAccounts.Count == 0 && this.Descriptor.DebitorBankAccounts.Count == 0)
                {
                    if (this.Descriptor.PaymentMeans != null)
                    {

                        if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                        {
                            Writer.WriteStartElement("ram:SpecifiedTradeSettlementPaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);
                            Writer.WriteElementString("ram:TypeCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                            Writer.WriteElementString("ram:Information", this.Descriptor.PaymentMeans.Information);

                            if (!String.IsNullOrEmpty(this.Descriptor.PaymentMeans.SEPACreditorIdentifier) && !String.IsNullOrEmpty(this.Descriptor.PaymentMeans.SEPAMandateReference))
                            {
                                Writer.WriteStartElement("ram:ID");
                                Writer.WriteAttributeString("schemeAgencyID", this.Descriptor.PaymentMeans.SEPACreditorIdentifier);
                                Writer.WriteValue(this.Descriptor.PaymentMeans.SEPAMandateReference);
                                Writer.WriteEndElement(); // !ram:ID
                            }
                            Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                        }
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
                        if (!String.IsNullOrEmpty(account.Name))
                        {
                            Writer.WriteElementString("ram:AccountName", account.Name);
                        }
                        if (!String.IsNullOrEmpty(account.ID))
                        {
                            Writer.WriteElementString("ram:ProprietaryID", account.ID);
                        }
                        Writer.WriteEndElement(); // !PayerPartyDebtorFinancialAccount

                        Writer.WriteStartElement("ram:PayerSpecifiedDebtorFinancialInstitution");
                        Writer.WriteElementString("ram:BICID", account.BIC);                                  
                        Writer.WriteEndElement(); // !PayerSpecifiedDebtorFinancialInstitution

                        Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                    }
                }
                #endregion

                #region ApplicableTradeTax
                _writeOptionalTaxes(Writer);
                #endregion

                #region BillingSpecifiedPeriod
                if(Descriptor.BillingPeriodStart.HasValue || Descriptor.BillingPeriodEnd.HasValue)
                {
                    Writer.WriteStartElement("ram:BillingSpecifiedPeriod", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung);
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
                #endregion

                if ((this.Descriptor.TradeAllowanceCharges != null) && (this.Descriptor.TradeAllowanceCharges.Count > 0))
                {
                    foreach (TradeAllowanceCharge tradeAllowanceCharge in this.Descriptor.TradeAllowanceCharges)
                    {
                        Writer.WriteStartElement("ram:SpecifiedTradeAllowanceCharge");
                        Writer.WriteStartElement("ram:ChargeIndicator");
                        Writer.WriteElementString("udt:Indicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");
                        Writer.WriteEndElement(); // !ram:ChargeIndicator

                        Writer.WriteStartElement("ram:BasisAmount", profile: Profile.Comfort | Profile.Extended | Profile.XRechnung);
                        Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount));
                        Writer.WriteEndElement();

                        Writer.WriteStartElement("ram:ActualAmount");
                        Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount, 2));
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
                        Writer.WriteStartElement("ram:SpecifiedLogisticsServiceCharge", ALL_PROFILES ^ Profile.XRechnung);
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

                #region SpecifiedTradeSettlementHeaderMonetarySummation
                //Gesamtsummen auf Dokumentenebene
                Writer.WriteStartElement("ram:SpecifiedTradeSettlementHeaderMonetarySummation");
                _writeOptionalAmount(Writer, "ram:LineTotalAmount", this.Descriptor.LineTotalAmount);                          // Summe der Nettobeträge aller Rechnungspositionen
                _writeOptionalAmount(Writer, "ram:ChargeTotalAmount", this.Descriptor.ChargeTotalAmount);                      // Summe der Zuschläge auf Dokumentenebene
                _writeOptionalAmount(Writer, "ram:AllowanceTotalAmount", this.Descriptor.AllowanceTotalAmount);                // Summe der Abschläge auf Dokumentenebene
                _writeOptionalAmount(Writer, "ram:TaxBasisTotalAmount", this.Descriptor.TaxBasisAmount);                       // Rechnungsgesamtbetrag ohne Umsatzsteuer
                _writeOptionalAmount(Writer, "ram:TaxTotalAmount", this.Descriptor.TaxTotalAmount, forceCurrency: true);       // Gesamtbetrag der Rechnungsumsatzsteuer, Steuergesamtbetrag in Buchungswährung
                //ToDo: RoundingAmount  //Rundungsbetrag
                _writeOptionalAmount(Writer, "ram:GrandTotalAmount", this.Descriptor.GrandTotalAmount);                       // Rechnungsgesamtbetrag einschließlich Umsatzsteuer
                _writeOptionalAmount(Writer, "ram:TotalPrepaidAmount", this.Descriptor.TotalPrepaidAmount);                   // Vorauszahlungsbetrag
                _writeOptionalAmount(Writer, "ram:DuePayableAmount", this.Descriptor.DuePayableAmount);                       // Fälliger Zahlungsbetrag
                Writer.WriteEndElement(); // !ram:SpecifiedTradeSettlementMonetarySummation
                #endregion

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

            #endregion

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
                if (throwExceptions) { throw new UnsupportedException("Invalid profile used for ZUGFeRD 2.0 invoice."); }
                return false;
            }

            if (descriptor.Profile == Profile.XRechnung)
            {
                if (descriptor.Seller != null)
                {
                    if (descriptor.SellerContact == null)
                    {
                        if (throwExceptions) { throw new MissingDataException("Seller contact required when seller is set."); }
                        return false;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(descriptor.SellerContact.EmailAddress))
                        {
                            if (throwExceptions) { throw new MissingDataException("Seller contact email address is required (BT-43)."); }
                            return false;
                        }
                        if (String.IsNullOrEmpty(descriptor.SellerContact.PhoneNo))
                        {
                            if (throwExceptions) { throw new MissingDataException("Seller contact phone no is required (BT-42)."); }
                            return false;
                        }
                        if (String.IsNullOrEmpty(descriptor.SellerContact.Name) && String.IsNullOrEmpty(descriptor.SellerContact.OrgUnit))
                        {
                            if (throwExceptions) { throw new MissingDataException("Seller contact point (name or org unit) no is required (BT-41)."); }
                            return false;
                        }
                    }
                }
            }

            return true;
        } // !Validate()


        private void _writeOptionalAmount(ProfileAwareXmlTextWriter writer, string tagName, decimal? value, int numDecimals = 2, bool forceCurrency = false, Profile profile = Profile.Unknown)
        {
            if (value.HasValue && (value.Value != decimal.MinValue))
            {
                writer.WriteStartElement(tagName);
                if (forceCurrency)
                {
                    writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                }
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
                writer.WriteValue(_formatDecimal(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount

                writer.WriteElementString("ram:TypeCode", tax.TypeCode.EnumToString());

                writer.WriteStartElement("ram:BasisAmount");
                writer.WriteValue(_formatDecimal(tax.BasisAmount));
                writer.WriteEndElement(); // !BasisAmount

                if (tax.AllowanceChargeBasisAmount != 0)
                {
                    writer.WriteStartElement("ram:AllowanceChargeBasisAmount");
                    writer.WriteValue(_formatDecimal(tax.AllowanceChargeBasisAmount));
                    writer.WriteEndElement(); // !AllowanceChargeBasisAmount
                }

                if (tax.CategoryCode.HasValue)
                {
                    writer.WriteElementString("ram:CategoryCode", tax.CategoryCode?.EnumToString());
                }
                writer.WriteElementString("ram:RateApplicablePercent", _formatDecimal(tax.Percent));
                writer.WriteEndElement(); // !RateApplicablePercent
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


        private void _writeOptionalParty(ProfileAwareXmlTextWriter writer, string partyTag, Party party, Contact contact = null, List<TaxRegistration> taxRegistrations = null, Profile profile = Profile.Unknown)
        {
            if (party != null)
            {
                writer.WriteStartElement(partyTag, profile);

                if (!String.IsNullOrEmpty(party.ID))
                {
                    writer.WriteElementString("ram:ID", party.ID);
                }

                if ((party.GlobalID != null) && !String.IsNullOrEmpty(party.GlobalID.ID) && !String.IsNullOrEmpty(party.GlobalID.SchemeID))
                {
                    writer.WriteStartElement("ram:GlobalID");
                    writer.WriteAttributeString("schemeID", party.GlobalID.SchemeID);
                    writer.WriteValue(party.GlobalID.ID);
                    writer.WriteEndElement();
                }

                if (!String.IsNullOrEmpty(party.Name))
                {
                    writer.WriteElementString("ram:Name", party.Name);
                }

                if (contact != null)
                {
                    _writeOptionalContact(writer, "ram:DefinedTradeContact", contact, Profile.Extended | Profile.XRechnung);
                }
                else if ((profile & Profile.XRechnung) == Profile.XRechnung)
                {
                    _writeOptionalContact(writer, "ram:DefinedTradeContact", new Contact(), Profile.XRechnung);
                }

                writer.WriteStartElement("ram:PostalTradeAddress");
                writer.WriteElementString("ram:PostcodeCode", party.Postcode);
                writer.WriteElementString("ram:LineOne", string.IsNullOrEmpty(party.ContactName) ? party.Street : party.ContactName);
                if (!string.IsNullOrEmpty(party.ContactName))
                    writer.WriteElementString("ram:LineTwo", party.Street);
                writer.WriteElementString("ram:CityName", party.City);
                writer.WriteElementString("ram:CountryID", party.Country.EnumToString());
                writer.WriteEndElement(); // !PostalTradeAddress

                if (taxRegistrations != null)
                {
                    foreach (TaxRegistration _reg in taxRegistrations)
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


        private void _writeOptionalContact(ProfileAwareXmlTextWriter writer, string contactTag, Contact contact, Profile profile = Profile.Unknown)
        {
            if (contact != null)
            {
                writer.WriteStartElement(contactTag, profile);

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
                    writer.WriteStartElement("ram:FaxUniversalCommunication", ALL_PROFILES ^ Profile.XRechnung);
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
    }
}