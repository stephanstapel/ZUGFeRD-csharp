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
using System.Xml.Linq;

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor22UBLWriter : IInvoiceDescriptorWriter
    {
        private ProfileAwareXmlTextWriter Writer;
        private InvoiceDescriptor Descriptor;

        private readonly Profile ALL_PROFILES = Profile.Minimum | Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung;

        public override void Save(InvoiceDescriptor descriptor, Stream stream, ZUGFeRDFormats format = ZUGFeRDFormats.UBL)
        {
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new IllegalStreamException("Cannot write to stream");
            }
            

            long streamPosition = stream.Position;

            this.Descriptor = descriptor;
            this.Writer = new ProfileAwareXmlTextWriter(stream, descriptor.Profile);
            bool isInvoice = true;
            if (this.Descriptor.Type == InvoiceType.Invoice || this.Descriptor.Type == InvoiceType.Correction)
            {
                // this is a duplicate, just to make sure: also a Correction is regarded as an Invoice
                isInvoice = true;
            }
            else if (this.Descriptor.Type == InvoiceType.CreditNote)
            {
                isInvoice = false;
            }
            else
            {
                throw new NotImplementedException("Not implemented yet.");
            }

            Dictionary<string, string> namespaces = new Dictionary<string, string>()
            {
                { "cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2" },
                { "cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" },
                { "ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2" },
                { "xs", "http://www.w3.org/2001/XMLSchema" }
            };
            
            if (isInvoice)
            {                
                namespaces.Add("ubl", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            }
            else
            {                
                namespaces.Add("ubl", "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2");
            }
            this.Writer.SetNamespaces(namespaces);


            Writer.WriteStartDocument();

            #region Kopfbereich
            // UBL has different namespace for different types
            if (isInvoice)
            {
                Writer.WriteStartElement("ubl", "Invoice");
                Writer.WriteAttributeString("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            }
            else
            {
                Writer.WriteStartElement("ubl", "CreditNote");
                Writer.WriteAttributeString("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2");
            }
            Writer.WriteAttributeString("xmlns", "cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            Writer.WriteAttributeString("xmlns", "cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            Writer.WriteAttributeString("xmlns", "ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            Writer.WriteAttributeString("xmlns", "xs", "http://www.w3.org/2001/XMLSchema");
            #endregion


            Writer.WriteElementString("cbc", "CustomizationID", "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0");
            Writer.WriteElementString("cbc", "ProfileID", "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0");

            Writer.WriteElementString("cbc", "ID", this.Descriptor.InvoiceNo); //Rechnungsnummer
            Writer.WriteElementString("cbc", "IssueDate", _formatDate(this.Descriptor.InvoiceDate.Value, false, true));

            // DueDate (BT-9)
            // has cardinality 0..1
            DateTime? dueDate = this.Descriptor.GetTradePaymentTerms().FirstOrDefault(x => x.DueDate != null)?.DueDate;
            if (dueDate != null)
            {
                Writer.WriteElementString("cbc", "DueDate", _formatDate(dueDate.Value, false, true));
            }

            if (isInvoice)
            {
                Writer.WriteElementString("cbc", "InvoiceTypeCode", String.Format("{0}", _encodeInvoiceType(this.Descriptor.Type))); //Code für den Rechnungstyp
            }
            else
            {
                Writer.WriteElementString("cbc", "CreditNoteTypeCode", String.Format("{0}", _encodeInvoiceType(this.Descriptor.Type))); //Code für den Rechnungstyp
            }


            _writeNotes(Writer, this.Descriptor.Notes);

            Writer.WriteElementString("cbc", "DocumentCurrencyCode", this.Descriptor.Currency.EnumToString());

            //   BT-6
            if (this.Descriptor.TaxCurrency.HasValue)
            {
                Writer.WriteElementString("cbc", "TaxCurrencyCode", this.Descriptor.TaxCurrency.Value.EnumToString());
            }

            Writer.WriteOptionalElementString("cbc", "BuyerReference", this.Descriptor.ReferenceOrderNo);

            if (this.Descriptor.BillingPeriodEnd.HasValue || this.Descriptor.BillingPeriodEnd.HasValue)
            {
                Writer.WriteStartElement("cac", "InvoicePeriod");

                if (this.Descriptor.BillingPeriodStart.HasValue)
                {
                    Writer.WriteElementString("cbc", "StartDate", _formatDate(this.Descriptor.BillingPeriodStart.Value, false, true));
                }
                if (this.Descriptor.BillingPeriodEnd.HasValue)
                {
                    Writer.WriteElementString("cbc", "EndDate", _formatDate(this.Descriptor.BillingPeriodEnd.Value, false, true));
                }
            }

            // OrderReference is optional
            if (!string.IsNullOrWhiteSpace(this.Descriptor.OrderNo))
            {
                Writer.WriteStartElement("cac", "OrderReference");
                Writer.WriteElementString("cbc", "ID", this.Descriptor.OrderNo);
                Writer.WriteOptionalElementString("cbc", "SalesOrderID",
                    this.Descriptor.SellerOrderReferencedDocument?.ID);
                Writer.WriteEndElement(); // !OrderReference
            }

            // BillingReference
            if (this.Descriptor.GetInvoiceReferencedDocuments().Count > 0)
            {
                Writer.WriteStartElement("cac", "BillingReference");
                foreach (InvoiceReferencedDocument invoiceReferencedDocument in this.Descriptor.GetInvoiceReferencedDocuments())
                {
                    Writer.WriteStartElement("cac", "InvoiceDocumentReference", Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    Writer.WriteOptionalElementString("cbc", "ID", invoiceReferencedDocument.ID);
                    if (invoiceReferencedDocument.IssueDateTime.HasValue)
                    {
                        Writer.WriteElementString("cbc", "IssueDate", _formatDate(invoiceReferencedDocument.IssueDateTime.Value, false, true));
                    }
                    Writer.WriteEndElement(); // !ram:InvoiceDocumentReference
                    break; // only one reference allowed in UBL
                }
                Writer.WriteEndElement(); // !cac:BillingReference
            }

            // DespatchDocumentReference
            if (this.Descriptor.DespatchAdviceReferencedDocument != null)
            {
                Writer.WriteStartElement("cac", "DespatchDocumentReference");
                Writer.WriteOptionalElementString("cbc", "ID", this.Descriptor.DespatchAdviceReferencedDocument.ID);
                Writer.WriteEndElement(); // !DespatchDocumentReference
            }

            // ContractDocumentReference
            if (this.Descriptor.ContractReferencedDocument != null)
            {
                Writer.WriteStartElement("cac", "ContractDocumentReference");
                Writer.WriteOptionalElementString("cbc", "ID", this.Descriptor.ContractReferencedDocument.ID);
                Writer.WriteEndElement(); // !ContractDocumentReference
            }

            if (this.Descriptor.AdditionalReferencedDocuments.Count > 0)
            {
                foreach (AdditionalReferencedDocument document in this.Descriptor.AdditionalReferencedDocuments)
                {
                    Writer.WriteStartElement("cac", "AdditionalDocumentReference");
                    Writer.WriteStartElement("cbc", "ID"); // BT-18, BT-22

                    if (document.ReferenceTypeCode != ReferenceTypeCodes.Unknown)
                    {
                        Writer.WriteAttributeString("schemeID", document.ReferenceTypeCode.EnumToString()); // BT-18-1
                    }

                    Writer.WriteValue(document.ID);
                    Writer.WriteEndElement(); // !cbc:ID
                    if (document.TypeCode != AdditionalReferencedDocumentTypeCode.Unknown)
                    {
                        Writer.WriteElementString("cbc", "DocumentTypeCode", document.TypeCode.EnumValueToString());
                    }
                    Writer.WriteOptionalElementString("cbc", "DocumentDescription", document.Name); // BT-123

                    if (document.AttachmentBinaryObject != null)
                    {
                        Writer.WriteStartElement("cac", "Attachment");

                        Writer.WriteStartElement("cbc", "EmbeddedDocumentBinaryObject"); // BT-125
                        Writer.WriteAttributeString("filename", document.Filename);
                        Writer.WriteAttributeString("mimeCode", MimeTypeMapper.GetMimeType(document.Filename));
                        Writer.WriteValue(Convert.ToBase64String(document.AttachmentBinaryObject));
                        Writer.WriteEndElement(); // !cbc:EmbeddedDocumentBinaryObject

                        /*
                         // not supported yet
                        Writer.WriteStartElement("cac", "ExternalReference");
                        Writer.WriteStartElement("cbc", "URI"); // BT-124
                        Writer.WriteValue("");
                        Writer.WriteEndElement(); // !cbc:URI
                        Writer.WriteEndElement(); // !cac:ExternalReference
                        */

                        Writer.WriteEndElement(); // !cac:Attachment
                    }

                    Writer.WriteEndElement(); // !AdditionalDocumentReference
                }
            }

            // ProjectReference
            if (this.Descriptor.SpecifiedProcuringProject != null)
            {
                Writer.WriteStartElement("cac", "ProjectReference");
                Writer.WriteOptionalElementString("cbc", "ID", this.Descriptor.SpecifiedProcuringProject.ID);
                Writer.WriteEndElement(); // !ProjectReference
            }


            #region SellerTradeParty

            // AccountingSupplierParty = PartyTypes.SellerTradeParty
            _writeOptionalParty(Writer, PartyTypes.SellerTradeParty, this.Descriptor.Seller, this.Descriptor.SellerContact, this.Descriptor.SellerElectronicAddress, this.Descriptor.SellerTaxRegistration);
            #endregion

            #region BuyerTradeParty
            //AccountingCustomerParty = PartyTypes.BuyerTradeParty
            _writeOptionalParty(Writer, PartyTypes.BuyerTradeParty, this.Descriptor.Buyer, this.Descriptor.BuyerContact, this.Descriptor.BuyerElectronicAddress, this.Descriptor.BuyerTaxRegistration);
            #endregion


            // Deliery = ShipToTradeParty
            if ((this.Descriptor.ShipTo != null) || (this.Descriptor.ActualDeliveryDate.HasValue))
            {
                Writer.WriteStartElement("cac", "Delivery");

                if (this.Descriptor.ActualDeliveryDate.HasValue)
                {
                    Writer.WriteStartElement("cbc", "ActualDeliveryDate");
                    Writer.WriteValue(_formatDate(this.Descriptor.ActualDeliveryDate.Value, false, true));
                    Writer.WriteEndElement(); // !ActualDeliveryDate
                }

                if (this.Descriptor.ShipTo != null)
                {
                    Writer.WriteStartElement("cac", "DeliveryLocation");

                    if (this.Descriptor.ShipTo.ID != null) // despite this is a mandatory field, the component should not throw an exception if this is not the case
                    {
                        Writer.WriteOptionalElementString("cbc", "ID", this.Descriptor.ShipTo.ID.ID);
                    }
                    Writer.WriteStartElement("cac", "Address");
                    Writer.WriteOptionalElementString("cbc", "StreetName", this.Descriptor.ShipTo.Street);
                    Writer.WriteOptionalElementString("cbc", "AdditionalStreetName", this.Descriptor.ShipTo.AddressLine3);
                    Writer.WriteOptionalElementString("cbc", "CityName", this.Descriptor.ShipTo.City);
                    Writer.WriteOptionalElementString("cbc", "PostalZone", this.Descriptor.ShipTo.Postcode);
                    Writer.WriteOptionalElementString("cbc", "CountrySubentity", this.Descriptor.ShipTo.CountrySubdivisionName);
                    Writer.WriteStartElement("cac", "Country");
                    if (this.Descriptor.ShipTo.Country != CountryCodes.Unknown)
                    {
                        Writer.WriteElementString("cbc", "IdentificationCode", this.Descriptor.ShipTo.Country.ToString());
                    }
                    Writer.WriteEndElement(); // !Country
                    Writer.WriteEndElement(); // !Address
                    Writer.WriteEndElement(); // !DeliveryLocation

                    if (!string.IsNullOrWhiteSpace(this.Descriptor.ShipTo.Name))
                    {
                        Writer.WriteStartElement("cac", "DeliveryParty");
                        Writer.WriteStartElement("cac", "PartyName");
                        Writer.WriteStartElement("cbc", "Name");
                        Writer.WriteValue(this.Descriptor.ShipTo.Name);
                        Writer.WriteEndElement(); // !Name
                        Writer.WriteEndElement(); // !PartyName
                        Writer.WriteEndElement(); // !DeliveryParty
                    }
                }

                Writer.WriteEndElement(); // !Delivery

            }

            // PaymentMeans
            if (!this.Descriptor.AnyCreditorFinancialAccount() && !this.Descriptor.AnyDebitorFinancialAccount())
            {
                if (this.Descriptor.PaymentMeans != null)
                {

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteStartElement("cac", "PaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                        Writer.WriteElementString("cbc", "PaymentMeansCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("cbc", "PaymentID", this.Descriptor.PaymentReference);

                        if (this.Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            Writer.WriteStartElement("cac", "CardAccount", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                            Writer.WriteElementString("cbc", "PrimaryAccountNumberID", this.Descriptor.PaymentMeans.FinancialCard.Id);
                            Writer.WriteOptionalElementString("cbc", "HolderName", this.Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            Writer.WriteEndElement(); //!CardAccount
                        }
                        Writer.WriteEndElement(); // !PaymentMeans
                    }
                }
            }
            else
            {
                foreach (BankAccount account in this.Descriptor.GetCreditorFinancialAccounts())
                {
                    Writer.WriteStartElement("cac", "PaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("cbc", "PaymentMeansCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("cbc", "PaymentID", this.Descriptor.PaymentReference);

                        if (this.Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            Writer.WriteStartElement("cac", "CardAccount", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                            Writer.WriteElementString("cbc", "PrimaryAccountNumberID", this.Descriptor.PaymentMeans.FinancialCard.Id);
                            Writer.WriteOptionalElementString("cbc", "HolderName", this.Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            Writer.WriteEndElement(); //!CardAccount
                        }
                    }

                    // PayeeFinancialAccount
                    Writer.WriteStartElement("cac", "PayeeFinancialAccount");
                    Writer.WriteElementString("cbc", "ID", account.IBAN);
                    Writer.WriteOptionalElementString("cbc", "Name", account.Name);

                    if (!string.IsNullOrWhiteSpace(account.BIC))
                    {
                        Writer.WriteStartElement("cac", "FinancialInstitutionBranch");
                        Writer.WriteElementString("cbc", "ID", account.BIC);

                        //[UBL - CR - 664] - A UBL invoice should not include the FinancialInstitutionBranch FinancialInstitution
                        //Writer.WriteStartElement("cac", "FinancialInstitution");
                        //Writer.WriteElementString("cbc", "Name", account.BankName);

                        //Writer.WriteEndElement(); // !FinancialInstitution
                        Writer.WriteEndElement(); // !FinancialInstitutionBranch
                    }

                    Writer.WriteEndElement(); // !PayeeFinancialAccount

                    Writer.WriteEndElement(); // !PaymentMeans
                }

                //[BR - 67] - An Invoice shall contain maximum one Payment Mandate(BG - 19).
                foreach (BankAccount account in this.Descriptor.GetDebitorFinancialAccounts())
                {
                    Writer.WriteStartElement("cac", "PaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteElementString("cbc", "PaymentMeansCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("cbc", "PaymentID", this.Descriptor.PaymentReference);
                    }

                    Writer.WriteStartElement("cac", "PaymentMandate");

                    //PEPPOL-EN16931-R061: Mandate reference MUST be provided for direct debit.
                    Writer.WriteElementString("cbc", "ID", this.Descriptor.PaymentMeans.SEPAMandateReference);

                    Writer.WriteStartElement("cac", "PayerFinancialAccount");

                    Writer.WriteElementString("cbc", "ID", account.IBAN);

                    //[UBL-CR-440]-A UBL invoice should not include the PaymentMeans PaymentMandate PayerFinancialAccount Name
                    //Writer.WriteElementString("cbc", "Name", account.Name);

                    //[UBL-CR-446]-A UBL invoice should not include the PaymentMeans PaymentMandate PayerFinancialAccount FinancialInstitutionBranch
                    //Writer.WriteStartElement("cac", "FinancialInstitutionBranch");
                    //Writer.WriteElementString("cbc", "ID", account.BIC);

                    //[UBL - CR - 664] - A UBL invoice should not include the FinancialInstitutionBranch FinancialInstitution
                    //Writer.WriteStartElement("cac", "FinancialInstitution");
                    //Writer.WriteElementString("cbc", "Name", account.BankName);

                    //Writer.WriteEndElement(); // !FinancialInstitution
                    //Writer.WriteEndElement(); // !FinancialInstitutionBranch

                    Writer.WriteEndElement(); // !PayerFinancialAccount
                    Writer.WriteEndElement(); // !PaymentMandate

                    Writer.WriteEndElement(); // !PaymentMeans
                }
            }

            // PaymentTerms (optional)
            if (this.Descriptor.GetTradePaymentTerms().Where(x => !string.IsNullOrEmpty(x.Description)).ToList().Count > 0)
            {
                Writer.WriteStartElement("cac", "PaymentTerms");

                if (this.Descriptor.GetTradePaymentTerms().Any(x => !string.IsNullOrWhiteSpace(x.Description)))
                {
                    Writer.WriteStartElement("cbc", "Note");

                    foreach (PaymentTerms paymentTerms in this.Descriptor.GetTradePaymentTerms().Where(x => !string.IsNullOrEmpty(x.Description)))
                    {
                        Writer.WriteRawString(Environment.NewLine);
                        Writer.WriteRawIndention();
                        Writer.WriteValue(paymentTerms.Description);
                    }

                    Writer.WriteRawString(Environment.NewLine);
                    Writer.WriteEndElement(); // !Note()
                }

                Writer.WriteEndElement(); // !PaymentTerms
            }

            #region AllowanceCharge
            foreach (TradeAllowanceCharge tradeAllowanceCharge in descriptor.GetTradeAllowanceCharges())
            {
                Writer.WriteStartElement("cac", "AllowanceCharge");

                Writer.WriteElementString("cbc", "ChargeIndicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");

                if (tradeAllowanceCharge.ReasonCode != AllowanceReasonCodes.Unknown)
                {
                    Writer.WriteStartElement("cbc", "AllowanceChargeReasonCode"); // BT-97 / BT-104
                    Writer.WriteValue(tradeAllowanceCharge.ReasonCode.GetDescriptionAttribute());
                    Writer.WriteEndElement();
                }

                if (!string.IsNullOrWhiteSpace(tradeAllowanceCharge.Reason))
                {
                    Writer.WriteStartElement("cbc", "AllowanceChargeReason"); // BT-97 / BT-104
                    Writer.WriteValue(tradeAllowanceCharge.Reason);
                    Writer.WriteEndElement();
                }

                if (tradeAllowanceCharge.ChargePercentage.HasValue && tradeAllowanceCharge.BasisAmount != null)
                {
                    Writer.WriteStartElement("cbc", "MultiplierFactorNumeric"); 
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ChargePercentage.Value, 2));
                    Writer.WriteEndElement();
                }

                Writer.WriteStartElement("cbc", "Amount"); // BT-92 / BT-99
                Writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount));
                Writer.WriteEndElement();

                if (tradeAllowanceCharge.BasisAmount != null)
                {
                    Writer.WriteStartElement("cbc", "BaseAmount"); // BT-93 / BT-100
                    Writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                    Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount));
                    Writer.WriteEndElement();
                }

                Writer.WriteStartElement("cac", "TaxCategory");
                Writer.WriteElementString("cbc", "ID", tradeAllowanceCharge.Tax.CategoryCode.ToString());
                if (tradeAllowanceCharge.Tax?.Percent != null)
                {
                    Writer.WriteElementString("cbc", "Percent", _formatDecimal(tradeAllowanceCharge.Tax.Percent));
                }
                Writer.WriteStartElement("cac", "TaxScheme");
                Writer.WriteElementString("cbc", "ID", tradeAllowanceCharge.Tax.TypeCode.EnumToString());
                Writer.WriteEndElement(); // cac:TaxScheme
                Writer.WriteEndElement(); // cac:TaxCategory

                Writer.WriteEndElement(); // !AllowanceCharge()
            }
            #endregion

            // Tax Total
            if (this.Descriptor.AnyApplicableTradeTaxes() && (this.Descriptor.TaxTotalAmount != null))
            {
                Writer.WriteStartElement("cac", "TaxTotal");
                _writeOptionalAmount(Writer, "cbc", "TaxAmount", this.Descriptor.TaxTotalAmount, forceCurrency: true);

                foreach (Tax tax in this.Descriptor.GetApplicableTradeTaxes())
                {
                    Writer.WriteStartElement("cac", "TaxSubtotal");
                    _writeOptionalAmount(Writer, "cbc", "TaxableAmount", tax.BasisAmount, forceCurrency: true);
                    _writeOptionalAmount(Writer, "cbc", "TaxAmount", tax.TaxAmount, forceCurrency: true);

                    Writer.WriteStartElement("cac", "TaxCategory");
                    Writer.WriteElementString("cbc", "ID", tax.CategoryCode.ToString());
                    Writer.WriteElementString("cbc", "Percent", _formatDecimal(tax.Percent));

                    if (tax.ExemptionReasonCode.HasValue)
                    {
                        Writer.WriteElementString("cbc", "TaxExemptionReasonCode", tax.ExemptionReasonCode.Value.EnumToString());
                    }
                    Writer.WriteOptionalElementString("cbc", "TaxExemptionReason", tax.ExemptionReason);
                    Writer.WriteStartElement("cac", "TaxScheme");
                    Writer.WriteElementString("cbc", "ID", tax.TypeCode.EnumToString());
                    Writer.WriteEndElement(); // !TaxScheme

                    Writer.WriteEndElement(); // !TaxCategory
                    Writer.WriteEndElement(); // !TaxSubtotal
                }

                Writer.WriteEndElement(); // !TaxTotal
            }

            Writer.WriteStartElement("cac", "LegalMonetaryTotal");
            _writeOptionalAmount(Writer, "cbc", "LineExtensionAmount", this.Descriptor.LineTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc", "TaxExclusiveAmount", this.Descriptor.TaxBasisAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc", "TaxInclusiveAmount", this.Descriptor.GrandTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc", "AllowanceTotalAmount", this.Descriptor.AllowanceTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc", "ChargeTotalAmount", this.Descriptor.ChargeTotalAmount, forceCurrency: true);
            //_writeOptionalAmount(Writer, "cbc", "TaxAmount", this.Descriptor.TaxTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc", "PrepaidAmount", this.Descriptor.TotalPrepaidAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc", "PayableAmount", this.Descriptor.DuePayableAmount, forceCurrency: true);
            //_writeOptionalAmount(Writer, "cbc", "PayableAlternativeAmount", this.Descriptor.RoundingAmount, forceCurrency: true);
            Writer.WriteEndElement(); //!LegalMonetaryTotal


            foreach (TradeLineItem tradeLineItem in this.Descriptor.GetTradeLineItems())
            {
                //Skip items with parent line id because these are written recursively in the _WriteTradeLineItem method
                if (String.IsNullOrEmpty(tradeLineItem.AssociatedDocument.ParentLineID))
                {
                    _WriteTradeLineItem(tradeLineItem, isInvoice);
                }
            }


            Writer.WriteEndDocument(); //Invoice
            Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);

        }

        private void _WriteTradeLineItem(TradeLineItem tradeLineItem, bool isInvoice = true)
        {
            if (String.IsNullOrWhiteSpace(tradeLineItem.AssociatedDocument.ParentLineID))
            {
                if (isInvoice)
                {
                    Writer.WriteStartElement("cac", "InvoiceLine");
                }
                else
                {
                    Writer.WriteStartElement("cac", "CreditNoteLine");
                }
            }
            else
            {
                if (isInvoice)
                {
                    Writer.WriteStartElement("cac", "SubInvoiceLine");
                }
                else
                {
                    Writer.WriteStartElement("cac", "SubCreditNoteLine");
                }
            }
            Writer.WriteElementString("cbc", "ID", tradeLineItem.AssociatedDocument.LineID);

            //Writer.WriteElementString("cbc", "InvoicedQuantity", tradeLineItem.BilledQuantity.ToString());
            Writer.WriteStartElement("cbc", "InvoicedQuantity");
            Writer.WriteAttributeString("unitCode", tradeLineItem.UnitCode.EnumToString());
            Writer.WriteValue(_formatDecimal(tradeLineItem.BilledQuantity));
            Writer.WriteEndElement();


            //Writer.WriteElementString("cbc", "LineExtensionAmount", tradeLineItem.LineTotalAmount.ToString());
            Writer.WriteStartElement("cbc", "LineExtensionAmount");
            Writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
            Writer.WriteValue(_formatDecimal(tradeLineItem.LineTotalAmount));
            Writer.WriteEndElement();

            if (tradeLineItem._AdditionalReferencedDocuments.Count > 0)
            {
                foreach (AdditionalReferencedDocument document in tradeLineItem._AdditionalReferencedDocuments)
                {
                    Writer.WriteStartElement("cac", "DocumentReference");
                    Writer.WriteStartElement("cbc", "ID"); // BT-18, BT-22

                    if (document.ReferenceTypeCode != ReferenceTypeCodes.Unknown)
                    {
                        Writer.WriteAttributeString("schemeID", document.ReferenceTypeCode.EnumToString()); // BT-18-1
                    }

                    Writer.WriteValue(document.ID);
                    Writer.WriteEndElement(); // !cbc:ID
                    if (document.TypeCode != AdditionalReferencedDocumentTypeCode.Unknown)
                    {
                        Writer.WriteElementString("cbc", "DocumentTypeCode", document.TypeCode.EnumValueToString());
                    }
                    Writer.WriteOptionalElementString("cbc", "DocumentDescription", document.Name); // BT-123

                    Writer.WriteEndElement(); // !DocumentReference
                }
            }

            Writer.WriteStartElement("cac", "Item");

            Writer.WriteOptionalElementString("cbc", "Description", tradeLineItem.Description);
            Writer.WriteElementString("cbc", "Name", tradeLineItem.Name);

            if (!string.IsNullOrWhiteSpace(tradeLineItem.BuyerAssignedID))
            {
                Writer.WriteStartElement("cac", "BuyersItemIdentification");
                Writer.WriteElementString("cbc", "ID", tradeLineItem.BuyerAssignedID);
                Writer.WriteEndElement(); //!BuyersItemIdentification
            }

            if (!string.IsNullOrWhiteSpace(tradeLineItem.SellerAssignedID))
            {
                Writer.WriteStartElement("cac", "SellersItemIdentification");
                Writer.WriteElementString("cbc", "ID", tradeLineItem.SellerAssignedID);
                Writer.WriteEndElement(); //!SellersItemIdentification
            }

            _writeIncludedReferencedProducts(Writer, tradeLineItem.IncludedReferencedProducts);
            _WriteCommodityClassification(Writer, tradeLineItem.GetDesignatedProductClassifications());

            //[UBL-SR-48] - Invoice lines shall have one and only one classified tax category.
            Writer.WriteStartElement("cac", "ClassifiedTaxCategory");
            Writer.WriteElementString("cbc", "ID", tradeLineItem.TaxCategoryCode.EnumToString());
            Writer.WriteElementString("cbc", "Percent", _formatDecimal(tradeLineItem.TaxPercent));

            Writer.WriteStartElement("cac", "TaxScheme");
            Writer.WriteElementString("cbc", "ID", tradeLineItem.TaxType.EnumToString());
            Writer.WriteEndElement();// !TaxScheme

            Writer.WriteEndElement();// !ClassifiedTaxCategory

            _writeApplicableProductCharacteristics(Writer, tradeLineItem.ApplicableProductCharacteristics);

            Writer.WriteEndElement(); //!Item

            Writer.WriteStartElement("cac", "Price");  // BG-29

            Writer.WriteStartElement("cbc", "PriceAmount");
            Writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
			// UBL-DT-01 explicitly excempts the price amount from the 2 decimal rule for amount elements,
			// thus allowing for 4 decimal places (needed for e.g. fuel prices)
            Writer.WriteValue(_formatDecimal(tradeLineItem.NetUnitPrice.Value, 4));
            Writer.WriteEndElement();

            if (tradeLineItem.UnitQuantity.HasValue)
            {
                Writer.WriteStartElement("cbc", "BaseQuantity"); // BT-149
                Writer.WriteAttributeString("unitCode", tradeLineItem.UnitCode.EnumToString()); // BT-150
                Writer.WriteValue(_formatDecimal(tradeLineItem.UnitQuantity));
                Writer.WriteEndElement();
            }

            IList<TradeAllowanceCharge> charges = tradeLineItem.GetTradeAllowanceCharges();
            if (charges.Count > 0) // only one charge possible in UBL
            {
                Writer.WriteStartElement("cac", "AllowanceCharge");

                Writer.WriteElementString("cbc", "ChargeIndicator", charges[0].ChargeIndicator ? "true" : "false");

                Writer.WriteStartElement("cbc", "Amount"); // BT-147
                Writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                Writer.WriteValue(_formatDecimal(charges[0].ActualAmount));
                Writer.WriteEndElement();

                if (charges[0].BasisAmount != null) // BT-148 is optional
                {
                    Writer.WriteStartElement("cbc", "BaseAmount"); // BT-148
                    Writer.WriteAttributeString("currencyID", this.Descriptor.Currency.EnumToString());
                    Writer.WriteValue(_formatDecimal(charges[0].BasisAmount));
                    Writer.WriteEndElement();
                }

                Writer.WriteEndElement(); // !AllowanceCharge()
            }

            Writer.WriteEndElement(); //!Price

            // TODO Add Tax Information for the tradeline item

            //Write sub invoice lines recursively
            foreach (TradeLineItem subTradeLineItem in this.Descriptor.GetTradeLineItems().Where(t => t.AssociatedDocument.ParentLineID == tradeLineItem.AssociatedDocument.LineID))
            {
                _WriteTradeLineItem(subTradeLineItem, isInvoice);
            }

            Writer.WriteEndElement(); //!InvoiceLine
        }


        private void _WriteCommodityClassification(ProfileAwareXmlTextWriter writer, List<DesignatedProductClassification> designatedProductClassifications)
        {
            if ((designatedProductClassifications == null) || (designatedProductClassifications.Count == 0))
            {
                return;
            }

            writer.WriteStartElement("cac", "CommodityClassification");

            foreach (DesignatedProductClassification classification in designatedProductClassifications)
            {
                if (classification.ListID == default(DesignatedProductClassificationClassCodes))
                {
                    continue;
                }

                writer.WriteStartElement("cbc", "ItemClassificationCode"); // BT-158                
                Writer.WriteAttributeString("listID", classification.ListID.EnumToString()); // BT-158-1

                if (!String.IsNullOrWhiteSpace(classification.ListVersionID))
                {
                    Writer.WriteAttributeString("listVersionID", classification.ListVersionID); // BT-158-2
                }

                // no name attribute in Peppol Billing!
                writer.WriteValue(classification.ClassCode, profile: ALL_PROFILES);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        } // !_WriteCommodityClassification()


        private void _writeOptionalParty(ProfileAwareXmlTextWriter writer, PartyTypes partyType, Party party, Contact contact = null, ElectronicAddress ElectronicAddress = null, List<TaxRegistration> taxRegistrations = null)
        {
            // filter according to https://github.com/stephanstapel/ZUGFeRD-csharp/pull/221
            switch (partyType)
            {
                case PartyTypes.Unknown:
                    return;
                case PartyTypes.SellerTradeParty:
                    break;
                case PartyTypes.BuyerTradeParty:
                    break;
                case PartyTypes.ShipFromTradeParty:
                    return;
                case PartyTypes.ShipToTradeParty: // ship to trade party/ cac:Delivery has very minimized information, thus generic _writeOptionalParty() cannot be used
                    return;
                default:
                    return;
            }

            if (party != null)
            {
                switch (partyType)
                {
                    case PartyTypes.SellerTradeParty:
                        writer.WriteStartElement("cac", "AccountingSupplierParty", this.Descriptor.Profile);
                        break;
                    case PartyTypes.BuyerTradeParty:
                        writer.WriteStartElement("cac", "AccountingCustomerParty", this.Descriptor.Profile);
                        break;
                        //case PartyTypes.ShipToTradeParty:
                        //    writer.WriteStartElement("ram", "ShipToTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.UltimateShipToTradeParty:
                        //    writer.WriteStartElement("ram", "UltimateShipToTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.ShipFromTradeParty:
                        //    writer.WriteStartElement("ram", "ShipFromTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.InvoiceeTradeParty:
                        //    writer.WriteStartElement("ram", "InvoiceeTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.PayeeTradeParty:
                        //    writer.WriteStartElement("ram", "PayeeTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.PayerTradeParty:
                        //    writer.WriteStartElement("ram", "PayerTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.SalesAgentTradeParty:
                        //    writer.WriteStartElement("ram", "SalesAgentTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.BuyerTaxRepresentativeTradeParty:
                        //    writer.WriteStartElement("ram", "BuyerTaxRepresentativeTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.ProductEndUserTradeParty:
                        //    writer.WriteStartElement("ram", "ProductEndUserTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.BuyerAgentTradeParty:
                        //    writer.WriteStartElement("ram", "BuyerAgentTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.InvoicerTradeParty:
                        //    writer.WriteStartElement("ram", "InvoicerTradeParty", this.Descriptor.Profile);
                        //    break;
                }

                writer.WriteStartElement("cac", "Party", this.Descriptor.Profile);

                if (ElectronicAddress != null)
                {
                    writer.WriteStartElement("cbc", "EndpointID");
                    writer.WriteAttributeString("schemeID", ElectronicAddress.ElectronicAddressSchemeID.EnumToString());
                    writer.WriteValue(ElectronicAddress.Address);
                    writer.WriteEndElement();
                }

                if (partyType == PartyTypes.SellerTradeParty)
                {
                    // This is the identification of the seller, not the buyer
                    if (!string.IsNullOrWhiteSpace(this.Descriptor.PaymentMeans?.SEPACreditorIdentifier))
                    {
                        writer.WriteStartElement("cac", "PartyIdentification");
                        writer.WriteStartElement("cbc", "ID");
                        writer.WriteAttributeString("schemeID", "SEPA");
                        writer.WriteValue(this.Descriptor.PaymentMeans.SEPACreditorIdentifier);
                        writer.WriteEndElement();//!ID
                        writer.WriteEndElement();//!PartyIdentification
                    }
                }
                else if (partyType == PartyTypes.BuyerTradeParty)
                {
                    if ((party.GlobalID != null) && (!String.IsNullOrWhiteSpace(party.GlobalID.ID)))
                    {
                        writer.WriteStartElement("cac", "PartyIdentification");
                        writer.WriteStartElement("cbc", "ID");

                        if (party.GlobalID.SchemeID.HasValue)
                        {
                            writer.WriteAttributeString("schemeID", party.GlobalID.SchemeID.Value.EnumToString());
                        }
                        writer.WriteValue(party.GlobalID.ID);
                        writer.WriteEndElement();//!ID
                        writer.WriteEndElement();//!PartyIdentification
                    }
                    else if ((party.ID != null) && (!String.IsNullOrWhiteSpace(party.ID.ID)))
                    {
                        writer.WriteStartElement("cac", "PartyIdentification");
                        writer.WriteStartElement("cbc", "ID");
                        writer.WriteValue(party.ID.ID);
                        writer.WriteEndElement();//!ID
                        writer.WriteEndElement();//!PartyIdentification
                    }
                }

                if (!string.IsNullOrWhiteSpace(party.Name))
                {
                    writer.WriteStartElement("cac", "PartyName");
                    writer.WriteStartElement("cbc", "Name");
                    writer.WriteValue(party.Name);
                    writer.WriteEndElement();//!Name
                    writer.WriteEndElement();//!PartyName
                }

                writer.WriteStartElement("cac", "PostalAddress");
                Writer.WriteOptionalElementString("cbc", "StreetName", party.Street);
                Writer.WriteOptionalElementString("cbc", "AdditionalStreetName", party.AddressLine3);
                Writer.WriteElementString("cbc", "CityName", party.City);
                Writer.WriteElementString("cbc", "PostalZone", party.Postcode);
                Writer.WriteOptionalElementString("cbc", "CountrySubentity", party.CountrySubdivisionName);

                writer.WriteStartElement("cac", "Country");
                if (party.Country != CountryCodes.Unknown)
                {
                    Writer.WriteElementString("cbc", "IdentificationCode", party.Country.ToString());
                }
                writer.WriteEndElement(); //!Country

                writer.WriteEndElement(); //!PostalTradeAddress


                foreach (var tax in taxRegistrations)
                {
                    Writer.WriteStartElement("cac", "PartyTaxScheme");
                    Writer.WriteElementString("cbc", "CompanyID", tax.No);
                    Writer.WriteStartElement("cac", "TaxScheme");
                    Writer.WriteElementString("cbc", "ID", UBLTaxRegistrationSchemeIDMapper.Map(tax.SchemeID));
                    Writer.WriteEndElement(); //!TaxScheme
                    Writer.WriteEndElement(); //!PartyTaxScheme
                }


                writer.WriteStartElement("cac", "PartyLegalEntity");
                writer.WriteElementString("cbc", "RegistrationName", party.Name);

                if (party.GlobalID != null)
                {
                    //Party legal registration identifier (BT-30)
                    Writer.WriteElementString("cbc", "CompanyID", party.GlobalID.ID);
                }

                writer.WriteEndElement(); //!PartyLegalEntity

                if (contact != null)
                {
                    writer.WriteStartElement("cac", "Contact");
                    writer.WriteOptionalElementString("cbc", "Name", contact.Name);
                    writer.WriteOptionalElementString("cbc", "Telephone", contact.PhoneNo);
                    writer.WriteOptionalElementString("cbc", "ElectronicMail", contact.EmailAddress);
                    writer.WriteEndElement(); // !Contact
                }

                writer.WriteEndElement(); //!Party



                //if (party.ID != null)
                //{
                //    if (!String.IsNullOrWhiteSpace(party.ID.ID) && (party.ID.SchemeID != GlobalIDSchemeIdentifiers.Unknown))
                //    {
                //        writer.WriteStartElement("ram", "ID");
                //        writer.WriteAttributeString("schemeID", party.ID.SchemeID.EnumToString());
                //        writer.WriteValue(party.ID.ID);
                //        writer.WriteEndElement();
                //    }

                //    writer.WriteOptionalElementString("ram", "ID", party.ID.ID);
                //}

                //if ((party.GlobalID != null) && !String.IsNullOrWhiteSpace(party.GlobalID.ID) && (party.GlobalID.SchemeID != GlobalIDSchemeIdentifiers.Unknown))
                //{
                //    writer.WriteStartElement("ram", "GlobalID");
                //    writer.WriteAttributeString("schemeID", party.GlobalID.SchemeID.EnumToString());
                //    writer.WriteValue(party.GlobalID.ID);
                //    writer.WriteEndElement();
                //}
                //_writeOptionalLegalOrganization(writer, "ram:SpecifiedLegalOrganization", party.SpecifiedLegalOrganization, partyType);
                //_writeOptionalContact(writer, "ram:DefinedTradeContact", contact, Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                //writer.WriteOptionalElementString("ram", "PostcodeCode", party.Postcode); // buyer: BT-53
                //writer.WriteOptionalElementString("ram", "LineOne", string.IsNullOrWhiteSpace(party.ContactName) ? party.Street : party.ContactName); // buyer: BT-50
                //if (!string.IsNullOrWhiteSpace(party.ContactName)) { writer.WriteOptionalElementString("ram", "LineTwo", party.Street); } // buyer: BT-51

                //writer.WriteOptionalElementString("ram", "LineThree", party.AddressLine3); // buyer: BT-163
                //writer.WriteOptionalElementString("ram", "CityName", party.City); // buyer: BT-52
                //writer.WriteElementString("ram", "CountryID", party.Country.EnumToString()); // buyer: BT-55
                //writer.WriteOptionalElementString("ram", "CountrySubDivisionName", party.CountrySubdivisionName); // BT-79
                //writer.WriteEndElement(); // !PostalTradeAddress

                //if (ElectronicAddress != null)
                //{
                //    if (!String.IsNullOrWhiteSpace(ElectronicAddress.Address))
                //    {
                //        writer.WriteStartElement("ram", "URIUniversalCommunication");
                //        writer.WriteStartElement("ram", "URIID");
                //        writer.WriteAttributeString("schemeID", ElectronicAddress.ElectronicAddressSchemeID.EnumToString());
                //        writer.WriteValue(ElectronicAddress.Address);
                //        writer.WriteEndElement();
                //        writer.WriteEndElement();
                //    }
                //}

                //if (taxRegistrations != null)
                //{
                //    // for seller: BT-31
                //    // for buyer : BT-48
                //    foreach (TaxRegistration _reg in taxRegistrations)
                //    {
                //        if (!String.IsNullOrWhiteSpace(_reg.No))
                //        {
                //            writer.WriteStartElement("ram", "SpecifiedTaxRegistration");
                //            writer.WriteStartElement("ram", "ID");
                //            writer.WriteAttributeString("schemeID", _reg.SchemeID.EnumToString());
                //            writer.WriteValue(_reg.No);
                //            writer.WriteEndElement();
                //            writer.WriteEndElement();
                //        }
                //    }
                //}
                //writer.WriteEndElement(); // !*TradeParty
                Writer.WriteEndElement(); //Invoice
            }
        } // !_writeOptionalParty()

        private void _writeIncludedReferencedProducts(ProfileAwareXmlTextWriter writer, List<IncludedReferencedProduct> includedReferencedProducts)
        {
            if (includedReferencedProducts.Count > 0)
            {
                foreach (var item in includedReferencedProducts)
                {
                    //TODO:
                }
            }
        }

        private void _writeApplicableProductCharacteristics(ProfileAwareXmlTextWriter writer, List<ApplicableProductCharacteristic> productCharacteristics)
        {

            if (productCharacteristics.Count > 0)
            {
                foreach (var characteristic in productCharacteristics)
                {
                    writer.WriteStartElement("cac", "AdditionalItemProperty");
                    writer.WriteElementString("cbc", "Name", characteristic.Description);
                    writer.WriteElementString("cbc", "Value", characteristic.Value);
                    writer.WriteEndElement();
                }
            }
        } // !_writeApplicableProductCharacteristics()

        private void _writeNotes(ProfileAwareXmlTextWriter writer, List<Note> notes)
        {
            if (notes.Count > 0)
            {
                foreach (Note note in notes)
                {
                    writer.WriteElementString("cbc", "Note", note.Content);
                }
            }
        } // !_writeNotes()

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
            throw new NotImplementedException();
        }
    }
}
