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

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor22UBLWriter : IInvoiceDescriptorWriter
    {
        private ProfileAwareXmlTextWriter _Writer;
        private InvoiceDescriptor _Descriptor;

        private readonly Profile ALL_PROFILES = Profile.Minimum | Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung;

        public override void Save(InvoiceDescriptor descriptor, Stream stream, ZUGFeRDFormats format = ZUGFeRDFormats.UBL, InvoiceFormatOptions options = null)
        {
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new IllegalStreamException("Cannot write to stream");
            }


            long streamPosition = stream.Position;

            this._Descriptor = descriptor;
            this._Writer = new ProfileAwareXmlTextWriter(stream, descriptor.Profile, options?.AutomaticallyCleanInvalidCharacters ?? false);
            bool isInvoice = true;
            if (this._Descriptor.Type == InvoiceType.Invoice || this._Descriptor.Type == InvoiceType.Correction)
            {
                // this is a duplicate, just to make sure: also a Correction is regarded as an Invoice
                isInvoice = true;
            }
            else if (this._Descriptor.Type == InvoiceType.CreditNote)
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
            this._Writer.SetNamespaces(namespaces);


            _Writer.WriteStartDocument();
            _WriteHeaderComments(_Writer, options);

            #region Kopfbereich
            // UBL has different namespace for different types
            if (isInvoice)
            {
                _Writer.WriteStartElement("ubl", "Invoice");
                _Writer.WriteAttributeString("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            }
            else
            {
                _Writer.WriteStartElement("ubl", "CreditNote");
                _Writer.WriteAttributeString("xmlns", "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2");
            }
            _Writer.WriteAttributeString("xmlns", "cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            _Writer.WriteAttributeString("xmlns", "cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            _Writer.WriteAttributeString("xmlns", "ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            _Writer.WriteAttributeString("xmlns", "xs", "http://www.w3.org/2001/XMLSchema");
            #endregion


            _Writer.WriteElementString("cbc", "CustomizationID", "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0");
            _Writer.WriteElementString("cbc", "ProfileID", "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0");

            _Writer.WriteElementString("cbc", "ID", this._Descriptor.InvoiceNo); //Rechnungsnummer
            _Writer.WriteElementString("cbc", "IssueDate", _formatDate(this._Descriptor.InvoiceDate.Value, false, true));


            if (isInvoice)
            {
                // DueDate (BT-9)
                // has cardinality 0..1
                DateTime? dueDate = this._Descriptor.GetTradePaymentTerms().FirstOrDefault(x => x.DueDate != null)?.DueDate;
                if (dueDate != null)
                {
                    _Writer.WriteElementString("cbc", "DueDate", _formatDate(dueDate.Value, false, true));
                }
            }

            if (isInvoice)
            {
                _Writer.WriteElementString("cbc", "InvoiceTypeCode", String.Format("{0}", EnumExtensions.EnumToString<InvoiceType>(this._Descriptor.Type))); //Code für den Rechnungstyp
            }
            else
            {
                _Writer.WriteElementString("cbc", "CreditNoteTypeCode", String.Format("{0}", EnumExtensions.EnumToString<InvoiceType>(this._Descriptor.Type))); //Code für den Rechnungstyp
            }


            _writeNotes(_Writer, this._Descriptor.Notes);

            _Writer.WriteElementString("cbc", "DocumentCurrencyCode", this._Descriptor.Currency.EnumToString());

            //   BT-6
            if (this._Descriptor.TaxCurrency.HasValue)
            {
                _Writer.WriteElementString("cbc", "TaxCurrencyCode", this._Descriptor.TaxCurrency.Value.EnumToString());
            }

            _Writer.WriteOptionalElementString("cbc", "BuyerReference", this._Descriptor.ReferenceOrderNo);

            if (this._Descriptor.BillingPeriodEnd.HasValue || this._Descriptor.BillingPeriodEnd.HasValue)
            {
                _Writer.WriteStartElement("cac", "InvoicePeriod");

                if (this._Descriptor.BillingPeriodStart.HasValue)
                {
                    _Writer.WriteElementString("cbc", "StartDate", _formatDate(this._Descriptor.BillingPeriodStart.Value, false, true));
                }
                if (this._Descriptor.BillingPeriodEnd.HasValue)
                {
                    _Writer.WriteElementString("cbc", "EndDate", _formatDate(this._Descriptor.BillingPeriodEnd.Value, false, true));
                }

                _Writer.WriteEndElement(); // !InvoicePeriod
            }

            // OrderReference is optional
            if (!string.IsNullOrWhiteSpace(this._Descriptor.OrderNo))
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerOrderReferencedDocumentComment);
                _Writer.WriteStartElement("cac", "OrderReference");
                _Writer.WriteElementString("cbc", "ID", this._Descriptor.OrderNo);
                _Writer.WriteOptionalElementString("cbc", "SalesOrderID",
                    this._Descriptor.SellerOrderReferencedDocument?.ID);
                _Writer.WriteEndElement(); // !OrderReference
            }

            // BillingReference
            if (this._Descriptor.GetInvoiceReferencedDocuments().Count > 0)
            {
                _Writer.WriteStartElement("cac", "BillingReference");
                foreach (InvoiceReferencedDocument invoiceReferencedDocument in this._Descriptor.GetInvoiceReferencedDocuments())
                {
                    _Writer.WriteStartElement("cac", "InvoiceDocumentReference", Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                    _Writer.WriteOptionalElementString("cbc", "ID", invoiceReferencedDocument.ID);
                    if (invoiceReferencedDocument.IssueDateTime.HasValue)
                    {
                        _Writer.WriteElementString("cbc", "IssueDate", _formatDate(invoiceReferencedDocument.IssueDateTime.Value, false, true));
                    }
                    _Writer.WriteEndElement(); // !cac:InvoiceDocumentReference
                    break; // only one reference allowed in UBL
                }
                _Writer.WriteEndElement(); // !cac:BillingReference
            }

            // DespatchDocumentReference
            if (this._Descriptor.DespatchAdviceReferencedDocument != null)
            {
                _WriteComment(_Writer, options, InvoiceCommentConstants.DespatchAdviceReferencedDocumentComment);
                _Writer.WriteStartElement("cac", "DespatchDocumentReference");
                _Writer.WriteOptionalElementString("cbc", "ID", this._Descriptor.DespatchAdviceReferencedDocument.ID);
                _Writer.WriteEndElement(); // !DespatchDocumentReference
            }

            // ContractDocumentReference
            if (this._Descriptor.ContractReferencedDocument != null)
            {
                _Writer.WriteStartElement("cac", "ContractDocumentReference");
                _Writer.WriteOptionalElementString("cbc", "ID", this._Descriptor.ContractReferencedDocument.ID);
                _Writer.WriteEndElement(); // !ContractDocumentReference
            }

            if (this._Descriptor.AdditionalReferencedDocuments.Count > 0)
            {
                foreach (AdditionalReferencedDocument document in this._Descriptor.AdditionalReferencedDocuments)
                {
                    _Writer.WriteStartElement("cac", "AdditionalDocumentReference");
                    _Writer.WriteStartElement("cbc", "ID"); // BT-18, BT-22

                    if (document.ReferenceTypeCode.HasValue)
                    {
                        _Writer.WriteAttributeString("schemeID", document.ReferenceTypeCode.Value.EnumToString()); // BT-18-1
                    }

                    _Writer.WriteValue(document.ID);
                    _Writer.WriteEndElement(); // !cbc:ID
                    if (document.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("cbc", "DocumentTypeCode", EnumExtensions.EnumToString<AdditionalReferencedDocumentTypeCode>(document.TypeCode.Value));
                    }
                    _Writer.WriteOptionalElementString("cbc", "DocumentDescription", document.Name); // BT-123

                    if (document.AttachmentBinaryObject != null)
                    {
                        _Writer.WriteStartElement("cac", "Attachment");

                        _Writer.WriteStartElement("cbc", "EmbeddedDocumentBinaryObject"); // BT-125
                        _Writer.WriteAttributeString("filename", document.Filename);
                        _Writer.WriteAttributeString("mimeCode", MimeTypeMapper.GetMimeType(document.Filename));
                        _Writer.WriteValue(Convert.ToBase64String(document.AttachmentBinaryObject));
                        _Writer.WriteEndElement(); // !cbc:EmbeddedDocumentBinaryObject

                        /*
                         // not supported yet
                        _Writer.WriteStartElement("cac", "ExternalReference");
                        _Writer.WriteStartElement("cbc", "URI"); // BT-124
                        _Writer.WriteValue("");
                        _Writer.WriteEndElement(); // !cbc:URI
                        _Writer.WriteEndElement(); // !cac:ExternalReference
                        */

                        _Writer.WriteEndElement(); // !cac:Attachment
                    }

                    _Writer.WriteEndElement(); // !AdditionalDocumentReference
                }
            }

            // ProjectReference
            if (this._Descriptor.SpecifiedProcuringProject != null)
            {
                _Writer.WriteStartElement("cac", "ProjectReference");
                _Writer.WriteOptionalElementString("cbc", "ID", this._Descriptor.SpecifiedProcuringProject.ID);
                _Writer.WriteEndElement(); // !ProjectReference
            }


            #region SellerTradeParty

            // AccountingSupplierParty = PartyTypes.SellerTradeParty
            _WriteComment(_Writer, options, InvoiceCommentConstants.SellerTradePartyComment);
            _writeOptionalParty(_Writer, PartyTypes.SellerTradeParty, this._Descriptor.Seller, this._Descriptor.SellerContact, this._Descriptor.SellerElectronicAddress, this._Descriptor.SellerTaxRegistration);
            #endregion

            #region BuyerTradeParty
            //AccountingCustomerParty = PartyTypes.BuyerTradeParty
            _WriteComment(_Writer, options, InvoiceCommentConstants.BuyerTradePartyComment);
            _writeOptionalParty(_Writer, PartyTypes.BuyerTradeParty, this._Descriptor.Buyer, this._Descriptor.BuyerContact, this._Descriptor.BuyerElectronicAddress, this._Descriptor.BuyerTaxRegistration);
            #endregion

            if (this._Descriptor.SellerTaxRepresentative != null)
            {
                _writeOptionalParty(_Writer, PartyTypes.SellerTaxRepresentativeTradeParty, this._Descriptor.SellerTaxRepresentative);
            }

            // Delivery = ShipToTradeParty
            if ((this._Descriptor.ShipTo != null) || (this._Descriptor.ActualDeliveryDate.HasValue))
            {
                _Writer.WriteStartElement("cac", "Delivery");

                if (this._Descriptor.ActualDeliveryDate.HasValue)
                {
                    _Writer.WriteStartElement("cbc", "ActualDeliveryDate");
                    _Writer.WriteValue(_formatDate(this._Descriptor.ActualDeliveryDate.Value, false, true));
                    _Writer.WriteEndElement(); // !ActualDeliveryDate
                }

                if (this._Descriptor.ShipTo != null)
                {
                    _Writer.WriteStartElement("cac", "DeliveryLocation");

                    if (this._Descriptor.ShipTo.ID != null) // despite this is a mandatory field, the component should not throw an exception if this is not the case
                    {
                        _Writer.WriteOptionalElementString("cbc", "ID", this._Descriptor.ShipTo.ID.ID);
                    }
                    _Writer.WriteStartElement("cac", "Address");
                    _Writer.WriteOptionalElementString("cbc", "StreetName", this._Descriptor.ShipTo.Street);
                    _Writer.WriteOptionalElementString("cbc", "AdditionalStreetName", this._Descriptor.ShipTo.AddressLine3);
                    _Writer.WriteOptionalElementString("cbc", "CityName", this._Descriptor.ShipTo.City);
                    _Writer.WriteOptionalElementString("cbc", "PostalZone", this._Descriptor.ShipTo.Postcode);
                    _Writer.WriteOptionalElementString("cbc", "CountrySubentity", this._Descriptor.ShipTo.CountrySubdivisionName);
                    _Writer.WriteStartElement("cac", "Country");
                    if (this._Descriptor.ShipTo.Country.HasValue)
                    {
                        _Writer.WriteElementString("cbc", "IdentificationCode", this._Descriptor.ShipTo.Country.Value.EnumToString());
                    }
                    _Writer.WriteEndElement(); // !Country
                    _Writer.WriteEndElement(); // !Address
                    _Writer.WriteEndElement(); // !DeliveryLocation

                    if (!string.IsNullOrWhiteSpace(this._Descriptor.ShipTo.Name))
                    {
                        _Writer.WriteStartElement("cac", "DeliveryParty");
                        _Writer.WriteStartElement("cac", "PartyName");
                        _Writer.WriteStartElement("cbc", "Name");
                        _Writer.WriteValue(this._Descriptor.ShipTo.Name);
                        _Writer.WriteEndElement(); // !Name
                        _Writer.WriteEndElement(); // !PartyName
                        _Writer.WriteEndElement(); // !DeliveryParty
                    }
                }

                _Writer.WriteEndElement(); // !Delivery
            }

            // PaymentMeans
            _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableHeaderTradeSettlementComment);
            if (!this._Descriptor.AnyCreditorFinancialAccount() && !this._Descriptor.AnyDebitorFinancialAccount())
            {
                if (this._Descriptor.PaymentMeans != null)
                {

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementPaymentMeansComment);
                        _Writer.WriteStartElement("cac", "PaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                        _Writer.WriteElementString("cbc", "PaymentMeansCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("cbc", "PaymentID", this._Descriptor.PaymentReference);

                        if (this._Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            _Writer.WriteStartElement("cac", "CardAccount", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                            _Writer.WriteElementString("cbc", "PrimaryAccountNumberID", this._Descriptor.PaymentMeans.FinancialCard.Id);
                            _Writer.WriteOptionalElementString("cbc", "HolderName", this._Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            _Writer.WriteEndElement(); //!CardAccount
                        }
                        _Writer.WriteEndElement(); // !PaymentMeans
                    }
                }
            }
            else
            {
                foreach (BankAccount account in this._Descriptor.GetCreditorFinancialAccounts())
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementPaymentMeansComment);
                    _Writer.WriteStartElement("cac", "PaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("cbc", "PaymentMeansCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("cbc", "PaymentID", this._Descriptor.PaymentReference);

                        if (this._Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            _Writer.WriteStartElement("cac", "CardAccount", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                            _Writer.WriteElementString("cbc", "PrimaryAccountNumberID", this._Descriptor.PaymentMeans.FinancialCard.Id);
                            _Writer.WriteOptionalElementString("cbc", "HolderName", this._Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            _Writer.WriteEndElement(); //!CardAccount
                        }
                    }

                    // PayeeFinancialAccount
                    _Writer.WriteStartElement("cac", "PayeeFinancialAccount");
                    _Writer.WriteElementString("cbc", "ID", account.IBAN);
                    _Writer.WriteOptionalElementString("cbc", "Name", account.Name);

                    if (!string.IsNullOrWhiteSpace(account.BIC))
                    {
                        _Writer.WriteStartElement("cac", "FinancialInstitutionBranch");
                        _Writer.WriteElementString("cbc", "ID", account.BIC);

                        //[UBL - CR - 664] - A UBL invoice should not include the FinancialInstitutionBranch FinancialInstitution
                        //_Writer.WriteStartElement("cac", "FinancialInstitution");
                        //_Writer.WriteElementString("cbc", "Name", account.BankName);

                        //_Writer.WriteEndElement(); // !FinancialInstitution
                        _Writer.WriteEndElement(); // !FinancialInstitutionBranch
                    }

                    _Writer.WriteEndElement(); // !PayeeFinancialAccount

                    _Writer.WriteEndElement(); // !PaymentMeans
                }

                //[BR - 67] - An Invoice shall contain maximum one Payment Mandate(BG - 19).
                foreach (BankAccount account in this._Descriptor.GetDebitorFinancialAccounts())
                {
                    _Writer.WriteStartElement("cac", "PaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                    if ((this._Descriptor.PaymentMeans != null) && this._Descriptor.PaymentMeans.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("cbc", "PaymentMeansCode", this._Descriptor.PaymentMeans.TypeCode.EnumToString());
                        _Writer.WriteOptionalElementString("cbc", "PaymentID", this._Descriptor.PaymentReference);
                    }

                    _Writer.WriteStartElement("cac", "PaymentMandate");

                    //PEPPOL-EN16931-R061: Mandate reference MUST be provided for direct debit.
                    _Writer.WriteElementString("cbc", "ID", this._Descriptor.PaymentMeans.SEPAMandateReference);

                    _Writer.WriteStartElement("cac", "PayerFinancialAccount");

                    _Writer.WriteElementString("cbc", "ID", account.IBAN);

                    //[UBL-CR-440]-A UBL invoice should not include the PaymentMeans PaymentMandate PayerFinancialAccount Name
                    //_Writer.WriteElementString("cbc", "Name", account.Name);

                    //[UBL-CR-446]-A UBL invoice should not include the PaymentMeans PaymentMandate PayerFinancialAccount FinancialInstitutionBranch
                    //_Writer.WriteStartElement("cac", "FinancialInstitutionBranch");
                    //_Writer.WriteElementString("cbc", "ID", account.BIC);

                    //[UBL - CR - 664] - A UBL invoice should not include the FinancialInstitutionBranch FinancialInstitution
                    //_Writer.WriteStartElement("cac", "FinancialInstitution");
                    //_Writer.WriteElementString("cbc", "Name", account.BankName);

                    //_Writer.WriteEndElement(); // !FinancialInstitution
                    //_Writer.WriteEndElement(); // !FinancialInstitutionBranch

                    _Writer.WriteEndElement(); // !PayerFinancialAccount
                    _Writer.WriteEndElement(); // !PaymentMandate

                    _Writer.WriteEndElement(); // !PaymentMeans
                }
            }

            // PaymentTerms (optional)
            if (this._Descriptor.GetTradePaymentTerms().Where(x => !string.IsNullOrEmpty(x.Description)).ToList().Count > 0)
            {
                _Writer.WriteStartElement("cac", "PaymentTerms");

                if (this._Descriptor.GetTradePaymentTerms().Any(x => !string.IsNullOrWhiteSpace(x.Description)))
                {
                    _Writer.WriteStartElement("cbc", "Note");

                    foreach (PaymentTerms paymentTerms in this._Descriptor.GetTradePaymentTerms().Where(x => !string.IsNullOrEmpty(x.Description)))
                    {
                        _Writer.WriteRawString(Environment.NewLine);
                        _Writer.WriteRawIndention();
                        _Writer.WriteValue(paymentTerms.Description);
                    }

                    _Writer.WriteRawString(Environment.NewLine);
                    _Writer.WriteEndElement(); // !Note()
                }

                _Writer.WriteEndElement(); // !PaymentTerms
            }

            #region AllowanceCharge
            foreach(TradeAllowance allowance in descriptor.GetTradeAllowances())
            {
                _WriteDocumentLevelAllowanceCharges(_Writer, allowance);
            } // !foreach(allowance)

            foreach (TradeCharge charge in descriptor.GetTradeCharges())
            {
                _WriteDocumentLevelAllowanceCharges(_Writer, charge);
            } // !foreach(charge)            
            #endregion

            // Tax Total
            if (this._Descriptor.AnyApplicableTradeTaxes() && (this._Descriptor.TaxTotalAmount != null))
            {
                _Writer.WriteStartElement("cac", "TaxTotal");
                _writeOptionalAmount(_Writer, "cbc", "TaxAmount", this._Descriptor.TaxTotalAmount, forceCurrency: true);

                foreach (Tax tax in this._Descriptor.GetApplicableTradeTaxes())
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.ApplicableTradeTaxComment);
                    _Writer.WriteStartElement("cac", "TaxSubtotal");
                    _writeOptionalAmount(_Writer, "cbc", "TaxableAmount", tax.BasisAmount, forceCurrency: true);
                    _writeOptionalAmount(_Writer, "cbc", "TaxAmount", tax.TaxAmount, forceCurrency: true);

                    _Writer.WriteStartElement("cac", "TaxCategory");
                    _Writer.WriteElementString("cbc", "ID", tax.CategoryCode.ToString());
                    _Writer.WriteElementString("cbc", "Percent", _formatDecimal(tax.Percent));

                    if (tax.ExemptionReasonCode.HasValue)
                    {
                        _Writer.WriteElementString("cbc", "TaxExemptionReasonCode", tax.ExemptionReasonCode.Value.EnumToString());
                    }
                    _Writer.WriteOptionalElementString("cbc", "TaxExemptionReason", tax.ExemptionReason);
                    _Writer.WriteStartElement("cac", "TaxScheme");
                    _Writer.WriteElementString("cbc", "ID", tax.TypeCode.EnumToString());
                    _Writer.WriteEndElement(); // !TaxScheme

                    _Writer.WriteEndElement(); // !TaxCategory
                    _Writer.WriteEndElement(); // !TaxSubtotal
                }

                _Writer.WriteEndElement(); // !TaxTotal
            }

            _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementHeaderMonetarySummationComment);
            _Writer.WriteStartElement("cac", "LegalMonetaryTotal");
            _writeOptionalAmount(_Writer, "cbc", "LineExtensionAmount", this._Descriptor.LineTotalAmount, forceCurrency: true);
            _writeOptionalAmount(_Writer, "cbc", "TaxExclusiveAmount", this._Descriptor.TaxBasisAmount, forceCurrency: true);
            _writeOptionalAmount(_Writer, "cbc", "TaxInclusiveAmount", this._Descriptor.GrandTotalAmount, forceCurrency: true);
            _writeOptionalAmount(_Writer, "cbc", "AllowanceTotalAmount", this._Descriptor.AllowanceTotalAmount, forceCurrency: true);
            _writeOptionalAmount(_Writer, "cbc", "ChargeTotalAmount", this._Descriptor.ChargeTotalAmount, forceCurrency: true);
            //_writeOptionalAmount(_Writer, "cbc", "TaxAmount", this._Descriptor.TaxTotalAmount, forceCurrency: true);
            _writeOptionalAmount(_Writer, "cbc", "PrepaidAmount", this._Descriptor.TotalPrepaidAmount, forceCurrency: true);
            _writeOptionalAmount(_Writer, "cbc", "PayableAmount", this._Descriptor.DuePayableAmount, forceCurrency: true);
            //_writeOptionalAmount(_Writer, "cbc", "PayableAlternativeAmount", this._Descriptor.RoundingAmount, forceCurrency: true);
            _Writer.WriteEndElement(); //!LegalMonetaryTotal            

            foreach (TradeLineItem tradeLineItem in this._Descriptor.GetTradeLineItems())
            {
                //Skip items with parent line id because these are written recursively in the _WriteTradeLineItem method
                if (String.IsNullOrEmpty(tradeLineItem.AssociatedDocument.ParentLineID))
                {
                    _WriteComment(_Writer, options, InvoiceCommentConstants.IncludedSupplyChainTradeLineItemComment);
                    _WriteTradeLineItem(tradeLineItem, isInvoice, options);
                }
            }


            _Writer.WriteEndDocument(); //Invoice
            _Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);

        }


        private void _WriteDocumentLevelAllowanceCharges(ProfileAwareXmlTextWriter writer, AbstractTradeAllowanceCharge tradeAllowanceCharge)
        {
            if (tradeAllowanceCharge == null)
            {
                return;
            }

            _Writer.WriteStartElement("cac", "AllowanceCharge");

            _Writer.WriteElementString("cbc", "ChargeIndicator", tradeAllowanceCharge.ChargeIndicator ? "true" : "false");

            if ((tradeAllowanceCharge is TradeAllowance allowance) && (allowance.ReasonCode != null))
            {
                _Writer.WriteStartElement("cbc", "AllowanceChargeReasonCode"); // BT-97                
                string s = EnumExtensions.EnumToString<AllowanceReasonCodes>(allowance.ReasonCode);
                _Writer.WriteValue(EnumExtensions.EnumToString<AllowanceReasonCodes>(allowance.ReasonCode));
                _Writer.WriteEndElement();
            }
            else if ((tradeAllowanceCharge is TradeCharge charge) && (charge.ReasonCode != null))
            {
                _Writer.WriteStartElement("cbc", "AllowanceChargeReasonCode"); // BT-104
                _Writer.WriteValue(EnumExtensions.EnumToString<ChargeReasonCodes>(charge.ReasonCode));
                _Writer.WriteEndElement();
            }

            if (!string.IsNullOrWhiteSpace(tradeAllowanceCharge.Reason))
            {
                _Writer.WriteStartElement("cbc", "AllowanceChargeReason"); // BT-97 / BT-104
                _Writer.WriteValue(tradeAllowanceCharge.Reason);
                _Writer.WriteEndElement();
            }

            if (tradeAllowanceCharge.ChargePercentage.HasValue && tradeAllowanceCharge.BasisAmount != null)
            {
                _Writer.WriteStartElement("cbc", "MultiplierFactorNumeric");
                _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ChargePercentage.Value, 2));
                _Writer.WriteEndElement();
            }

            _Writer.WriteStartElement("cbc", "Amount"); // BT-92 / BT-99
            _Writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
            _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount));
            _Writer.WriteEndElement();

            if (tradeAllowanceCharge.BasisAmount != null)
            {
                _Writer.WriteStartElement("cbc", "BaseAmount"); // BT-93 / BT-100
                _Writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount));
                _Writer.WriteEndElement();
            }

            _Writer.WriteStartElement("cac", "TaxCategory");
            _Writer.WriteElementString("cbc", "ID", tradeAllowanceCharge.Tax.CategoryCode.ToString());
            if (tradeAllowanceCharge.Tax?.Percent != null)
            {
                _Writer.WriteElementString("cbc", "Percent", _formatDecimal(tradeAllowanceCharge.Tax.Percent));
            }
            _Writer.WriteStartElement("cac", "TaxScheme");
            _Writer.WriteElementString("cbc", "ID", tradeAllowanceCharge.Tax.TypeCode.EnumToString());
            _Writer.WriteEndElement(); // cac:TaxScheme
            _Writer.WriteEndElement(); // cac:TaxCategory

            _Writer.WriteEndElement(); // !AllowanceCharge()            
        } // !_WriteDocumentLevelAllowanceCharges()


        private void _WriteTradeLineItem(TradeLineItem tradeLineItem, bool isInvoice = true, InvoiceFormatOptions options = null)
        {
            if (String.IsNullOrWhiteSpace(tradeLineItem.AssociatedDocument.ParentLineID))
            {
                if (isInvoice)
                {
                    _Writer.WriteStartElement("cac", "InvoiceLine");
                }
                else
                {
                    _Writer.WriteStartElement("cac", "CreditNoteLine");
                }
            }
            else
            {
                if (isInvoice)
                {
                    _Writer.WriteStartElement("cac", "SubInvoiceLine");
                }
                else
                {
                    _Writer.WriteStartElement("cac", "SubCreditNoteLine");
                }
            }
            _Writer.WriteElementString("cbc", "ID", tradeLineItem.AssociatedDocument.LineID);

            if (tradeLineItem.AssociatedDocument?.Notes?.Count > 0)
            {
                // BT-127
                _Writer.WriteStartElement("cbc", "Note");
                _Writer.WriteValue(String.Join(Environment.NewLine, tradeLineItem.AssociatedDocument.Notes.Select(n => n.Content)));
                _Writer.WriteEndElement(); // cbc:Note
            }

            if (isInvoice)
            {
                _Writer.WriteStartElement("cbc", "InvoicedQuantity");
            }
            else
            {
                _Writer.WriteStartElement("cbc", "CreditedQuantity");
            }
            _Writer.WriteAttributeString("unitCode", tradeLineItem.UnitCode.EnumToString());
            _Writer.WriteValue(_formatDecimal(tradeLineItem.BilledQuantity, 4));
            _Writer.WriteEndElement(); // !InvoicedQuantity || CreditedQuantity

            _WriteComment(_Writer, options, InvoiceCommentConstants.SpecifiedTradeSettlementLineMonetarySummationComment);
            _writeOptionalAmount(_Writer, "cbc", "LineExtensionAmount", tradeLineItem.LineTotalAmount, forceCurrency: true);

            if (tradeLineItem.AdditionalReferencedDocuments.Count > 0)
            {
                foreach (AdditionalReferencedDocument document in tradeLineItem.AdditionalReferencedDocuments)
                {
                    _Writer.WriteStartElement("cac", "DocumentReference");
                    _Writer.WriteStartElement("cbc", "ID"); // BT-18, BT-22

                    if (document.ReferenceTypeCode.HasValue)
                    {
                        _Writer.WriteAttributeString("schemeID", document.ReferenceTypeCode.Value.EnumToString()); // BT-18-1
                    }

                    _Writer.WriteValue(document.ID);
                    _Writer.WriteEndElement(); // !cbc:ID
                    if (document.TypeCode.HasValue)
                    {
                        _Writer.WriteElementString("cbc", "DocumentTypeCode", EnumExtensions.EnumToString<AdditionalReferencedDocumentTypeCode>(document.TypeCode.Value));
                    }
                    _Writer.WriteOptionalElementString("cbc", "DocumentDescription", document.Name); // BT-123

                    _Writer.WriteEndElement(); // !DocumentReference
                }
            }

            foreach (var specifiedTradeAllowanceCharge in tradeLineItem.GetSpecifiedTradeAllowances())
            {
                _WriteItemLevelSpecifiedTradeAllowanceCharge(specifiedTradeAllowanceCharge);
            }
            
            foreach (var specifiedTradeAllowanceCharge in tradeLineItem.GetSpecifiedTradeCharges())
            {
                _WriteItemLevelSpecifiedTradeAllowanceCharge(specifiedTradeAllowanceCharge);
            }

            _Writer.WriteStartElement("cac", "Item");

            _Writer.WriteOptionalElementString("cbc", "Description", tradeLineItem.Description);
            _Writer.WriteElementString("cbc", "Name", tradeLineItem.Name);

            if (!string.IsNullOrWhiteSpace(tradeLineItem.BuyerAssignedID))
            {
                _Writer.WriteStartElement("cac", "BuyersItemIdentification");
                _Writer.WriteElementString("cbc", "ID", tradeLineItem.BuyerAssignedID);
                _Writer.WriteEndElement(); //!BuyersItemIdentification
            }

            if (!string.IsNullOrWhiteSpace(tradeLineItem.SellerAssignedID))
            {
                _Writer.WriteStartElement("cac", "SellersItemIdentification");
                _Writer.WriteElementString("cbc", "ID", tradeLineItem.SellerAssignedID);
                _Writer.WriteEndElement(); //!SellersItemIdentification
            }

            _writeIncludedReferencedProducts(_Writer, tradeLineItem.IncludedReferencedProducts);
            _WriteCommodityClassification(_Writer, tradeLineItem.GetDesignatedProductClassifications());

            //[UBL-SR-48] - Invoice lines shall have one and only one classified tax category.
            _Writer.WriteStartElement("cac", "ClassifiedTaxCategory");
            _Writer.WriteElementString("cbc", "ID", tradeLineItem.TaxCategoryCode.EnumToString());
            _Writer.WriteElementString("cbc", "Percent", _formatDecimal(tradeLineItem.TaxPercent));

            _Writer.WriteStartElement("cac", "TaxScheme");
            _Writer.WriteElementString("cbc", "ID", tradeLineItem.TaxType.EnumToString());
            _Writer.WriteEndElement();// !TaxScheme

            _Writer.WriteEndElement();// !ClassifiedTaxCategory

            _writeApplicableProductCharacteristics(_Writer, tradeLineItem.ApplicableProductCharacteristics);

            _Writer.WriteEndElement(); //!Item

            _Writer.WriteStartElement("cac", "Price");  // BG-29

            _WriteComment(_Writer, options, InvoiceCommentConstants.NetPriceProductTradePriceComment);
            _Writer.WriteStartElement("cbc", "PriceAmount");
            _Writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
			// UBL-DT-01 explicitly excempts the price amount from the 2 decimal rule for amount elements,
			// thus allowing for 4 decimal places (needed for e.g. fuel prices)
            _Writer.WriteValue(_formatDecimal(tradeLineItem.NetUnitPrice, 4));
            _Writer.WriteEndElement();

            if (tradeLineItem.NetQuantity.HasValue)
            {
                _Writer.WriteStartElement("cbc", "BaseQuantity"); // BT-149
                _Writer.WriteAttributeString("unitCode", tradeLineItem.UnitCode.EnumToString()); // BT-150
                _Writer.WriteValue(_formatDecimal(tradeLineItem.NetQuantity));
                _Writer.WriteEndElement();
            }

            IList<AbstractTradeAllowanceCharge> charges = tradeLineItem.GetTradeAllowanceCharges();
            if (charges.Count > 0) // only one charge possible in UBL
            {
                if (charges[0] is AbstractTradeAllowanceCharge tradeAllowanceCharge)
                {
                    _Writer.WriteStartElement("cac", "AllowanceCharge");

                    if (tradeAllowanceCharge is TradeAllowance)
                    {
                        _Writer.WriteElementString("cbc", "ChargeIndicator", "false");
                    }
                    else
                    {
                        _Writer.WriteElementString("cbc", "ChargeIndicator", "true");
                    }                    

                    _Writer.WriteStartElement("cbc", "Amount"); // BT-147
                    _Writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                    _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.ActualAmount));
                    _Writer.WriteEndElement();

                    if (tradeAllowanceCharge.BasisAmount != null) // BT-148 is optional
                    {
                        _Writer.WriteStartElement("cbc", "BaseAmount"); // BT-148
                        _Writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                        _Writer.WriteValue(_formatDecimal(tradeAllowanceCharge.BasisAmount));
                        _Writer.WriteEndElement();
                    }

                    _Writer.WriteEndElement(); // !AllowanceCharge()
                }
            }

            _Writer.WriteEndElement(); //!Price

            // TODO Add Tax Information for the tradeline item

            //Write sub invoice lines recursively
            foreach (TradeLineItem subTradeLineItem in this._Descriptor.GetTradeLineItems().Where(t => t.AssociatedDocument.ParentLineID == tradeLineItem.AssociatedDocument.LineID))
            {
                _WriteTradeLineItem(subTradeLineItem, isInvoice, options);
            }

            _Writer.WriteEndElement(); //!InvoiceLine
        }

        private void _WriteItemLevelSpecifiedTradeAllowanceCharge(AbstractTradeAllowanceCharge specifiedTradeAllowanceCharge)
        {
            _Writer.WriteStartElement("cac", "AllowanceCharge");
            _Writer.WriteElementString("cbc", "ChargeIndicator",
                specifiedTradeAllowanceCharge.ChargeIndicator ? "true" : "false"); // BG-28-0
            switch (specifiedTradeAllowanceCharge)
            {
                case TradeAllowance allowance when allowance.ReasonCode != null:
                    _Writer.WriteOptionalElementString("cbc", "AllowanceChargeReasonCode", allowance.ReasonCode.EnumToString()); // BT-140
                    break;
                case TradeCharge charge when charge.ReasonCode != null:
                    _Writer.WriteOptionalElementString("cbc", "AllowanceChargeReasonCode", charge.ReasonCode.EnumToString()); // BT-145
                    break;
            }
            
            _Writer.WriteOptionalElementString("cbc", "AllowanceChargeReason",
                specifiedTradeAllowanceCharge.Reason); // BT-139, BT-144

            if (specifiedTradeAllowanceCharge.ChargePercentage.HasValue)
            {
                _Writer.WriteOptionalElementString("cbc", "MultiplierFactorNumeric",
                    _formatDecimal(specifiedTradeAllowanceCharge.ChargePercentage));
            }

            _Writer.WriteStartElement("cbc", "Amount");
            _Writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
            _Writer.WriteValue(_formatDecimal(specifiedTradeAllowanceCharge.ActualAmount));
            _Writer.WriteEndElement(); // !Amount
            if (specifiedTradeAllowanceCharge.BasisAmount.HasValue)
            {
                _Writer.WriteStartElement("cbc", "BaseAmount"); // BT-137, BT-142
                _Writer.WriteAttributeString("currencyID", this._Descriptor.Currency.EnumToString());
                _Writer.WriteValue(_formatDecimal(specifiedTradeAllowanceCharge.BasisAmount));
                _Writer.WriteEndElement(); // !BaseAmount
            }

            _Writer.WriteEndElement(); // !AllowanceCharge
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
                if (classification.ListID == null)
                {
                    continue;
                }

                writer.WriteStartElement("cbc", "ItemClassificationCode"); // BT-158
                _Writer.WriteAttributeString("listID", classification.ListID.EnumToString()); // BT-158-1

                if (!String.IsNullOrWhiteSpace(classification.ListVersionID))
                {
                    _Writer.WriteAttributeString("listVersionID", classification.ListVersionID); // BT-158-2
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
                case PartyTypes.SellerTaxRepresentativeTradeParty:
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
                        writer.WriteStartElement("cac", "AccountingSupplierParty", this._Descriptor.Profile);
                        break;
                    case PartyTypes.BuyerTradeParty:
                        writer.WriteStartElement("cac", "AccountingCustomerParty", this._Descriptor.Profile);
                        break;
                    case PartyTypes.SellerTaxRepresentativeTradeParty:
                        writer.WriteStartElement("cac", "TaxRepresentativeParty", this._Descriptor.Profile);
                        break;
                        //case PartyTypes.ShipToTradeParty:
                        //    writer.WriteStartElement("ram", "ShipToTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.UltimateShipToTradeParty:
                        //    writer.WriteStartElement("ram", "UltimateShipToTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.ShipFromTradeParty:
                        //    writer.WriteStartElement("ram", "ShipFromTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.InvoiceeTradeParty:
                        //    writer.WriteStartElement("ram", "InvoiceeTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.PayeeTradeParty:
                        //    writer.WriteStartElement("ram", "PayeeTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.PayerTradeParty:
                        //    writer.WriteStartElement("ram", "PayerTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.SalesAgentTradeParty:
                        //    writer.WriteStartElement("ram", "SalesAgentTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.BuyerTaxRepresentativeTradeParty:
                        //    writer.WriteStartElement("ram", "BuyerTaxRepresentativeTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.ProductEndUserTradeParty:
                        //    writer.WriteStartElement("ram", "ProductEndUserTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.BuyerAgentTradeParty:
                        //    writer.WriteStartElement("ram", "BuyerAgentTradeParty", this._Descriptor.Profile);
                        //    break;
                        //case PartyTypes.InvoicerTradeParty:
                        //    writer.WriteStartElement("ram", "InvoicerTradeParty", this._Descriptor.Profile);
                        //    break;
                }

                writer.WriteStartElement("cac", "Party", this._Descriptor.Profile);

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
                    if (!string.IsNullOrWhiteSpace(this._Descriptor.PaymentMeans?.SEPACreditorIdentifier))
                    {
                        writer.WriteStartElement("cac", "PartyIdentification");
                        writer.WriteStartElement("cbc", "ID"); // BT-90
                        writer.WriteAttributeString("schemeID", "SEPA");
                        writer.WriteValue(this._Descriptor.PaymentMeans.SEPACreditorIdentifier);
                        writer.WriteEndElement();//!ID
                        writer.WriteEndElement();//!PartyIdentification
                    }
                    // no 'else' because the cardinality is 0..n
                    if ((party.ID != null) && (!String.IsNullOrWhiteSpace(party.ID.ID)))
                    {
                        writer.WriteStartElement("cac", "PartyIdentification");
                        writer.WriteStartElement("cbc", "ID"); // BT-29
                        // 'SchemeID' is optional
                        if (party.ID.SchemeID.HasValue)
                        {
                            writer.WriteAttributeString("schemeID", party.ID.SchemeID.Value.EnumToString());
                        }
                        writer.WriteValue(party.ID.ID);
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
                _Writer.WriteOptionalElementString("cbc", "StreetName", party.Street);
                _Writer.WriteOptionalElementString("cbc", "AdditionalStreetName", party.AddressLine3);
                _Writer.WriteElementString("cbc", "CityName", party.City);
                _Writer.WriteElementString("cbc", "PostalZone", party.Postcode);
                _Writer.WriteOptionalElementString("cbc", "CountrySubentity", party.CountrySubdivisionName);

                writer.WriteStartElement("cac", "Country");
                if (party.Country.HasValue)
                {
                    _Writer.WriteElementString("cbc", "IdentificationCode", party.Country.Value.EnumToString());
                }
                writer.WriteEndElement(); //!Country

                writer.WriteEndElement(); //!PostalTradeAddress


                if (taxRegistrations != null)
                {
                    foreach (var tax in taxRegistrations)
                    {
                        _Writer.WriteStartElement("cac", "PartyTaxScheme");
                        _Writer.WriteElementString("cbc", "CompanyID", tax.No);
                        _Writer.WriteStartElement("cac", "TaxScheme");
                        _Writer.WriteElementString("cbc", "ID", UBLTaxRegistrationSchemeIDMapper.Map(tax.SchemeID));
                        _Writer.WriteEndElement(); //!TaxScheme
                        _Writer.WriteEndElement(); //!PartyTaxScheme
                    }
                }

                writer.WriteStartElement("cac", "PartyLegalEntity");
                writer.WriteElementString("cbc", "RegistrationName", party.Name);

                if (party.GlobalID != null)
                {
                    //Party legal registration identifier (BT-30)
                    _Writer.WriteElementString("cbc", "CompanyID", party.GlobalID.ID);
                }

                if (party.Description != null)
                {
                    //Party additional legal information (BT-33)
                    _Writer.WriteElementString("cbc", "CompanyLegalForm", party.Description);
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
                _Writer.WriteEndElement(); //Invoice
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


        internal override bool Validate(InvoiceDescriptor descriptor, bool throwExceptions = true)
        {
            throw new NotImplementedException();
        }
    }
}
