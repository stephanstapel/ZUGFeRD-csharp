
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Xml;

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
            this.Writer = new ProfileAwareXmlTextWriter(stream, Encoding.UTF8, descriptor.Profile);
            Writer.Formatting = Formatting.Indented;
            Writer.WriteStartDocument();

            if (this.Descriptor.Type != InvoiceType.Invoice && this.Descriptor.Type != InvoiceType.CreditNote)
                throw new NotImplementedException("Not implemented yet.");

            #region Kopfbereich
            // UBL has different namespace for different types
            if (this.Descriptor.Type == InvoiceType.Invoice)
            {
                Writer.WriteStartElement("Invoice");
                Writer.WriteAttributeString("xmlns", null, null, "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            }
            else if(this.Descriptor.Type == InvoiceType.CreditNote)
            {
                Writer.WriteStartElement("CreditNote");
                Writer.WriteAttributeString("xmlns", null, null, "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2");
            }
            Writer.WriteAttributeString("xmlns", "cac", null, "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            Writer.WriteAttributeString("xmlns", "cbc", null, "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            Writer.WriteAttributeString("xmlns", "ext", null, "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            Writer.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");
            #endregion


            Writer.WriteElementString("cbc:CustomizationID", "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_3.0");
            Writer.WriteElementString("cbc:ProfileID", "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0");

            Writer.WriteElementString("cbc:ID", this.Descriptor.InvoiceNo); //Rechnungsnummer
            Writer.WriteElementString("cbc:IssueDate", _formatDate(this.Descriptor.InvoiceDate.Value, false));

            Writer.WriteElementString("cbc:InvoiceTypeCode", String.Format("{0}", _encodeInvoiceType(this.Descriptor.Type))); //Code für den Rechnungstyp


            _writeNotes(Writer, this.Descriptor.Notes);

            Writer.WriteElementString("cbc:DocumentCurrencyCode", this.Descriptor.Currency.EnumToString());

            Writer.WriteOptionalElementString("cbc:BuyerReference", this.Descriptor.ReferenceOrderNo);

            // OrderReference
            Writer.WriteStartElement("cac:OrderReference");
            Writer.WriteElementString("cbc:ID", this.Descriptor.OrderNo);
            Writer.WriteEndElement(); // !OrderReference



            // ContractDocumentReference
            if (this.Descriptor.ContractReferencedDocument != null)
            {
                Writer.WriteStartElement("cac:ContractDocumentReference");
                Writer.WriteOptionalElementString("cbc:ID", this.Descriptor.ContractReferencedDocument.ID);
                Writer.WriteEndElement(); // !ContractDocumentReference
            }

            // ProjectReference
            if (this.Descriptor.SpecifiedProcuringProject != null)
            {
                Writer.WriteStartElement("cac:ProjectReference");
                Writer.WriteOptionalElementString("cbc:ID", this.Descriptor.SpecifiedProcuringProject.ID);
                Writer.WriteEndElement(); // !ProjectReference
            }


            #region SellerTradeParty
            //AccountingSupplierParty
            _writeOptionalParty(Writer, PartyTypes.SellerTradeParty, this.Descriptor.Seller, this.Descriptor.SellerContact, this.Descriptor.SellerElectronicAddress, this.Descriptor.SellerTaxRegistration);
            #endregion

            #region BuyerTradeParty
            //AccountingCustomerParty
            _writeOptionalParty(Writer, PartyTypes.BuyerTradeParty, this.Descriptor.Buyer, this.Descriptor.BuyerContact, this.Descriptor.BuyerElectronicAddress, this.Descriptor.BuyerTaxRegistration);
            #endregion

            // TODO PaymentMeans Not fully implemented 
            if (Descriptor.CreditorBankAccounts.Count == 0)
            {
                if (this.Descriptor.PaymentMeans != null)
                {

                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteStartElement("cac:PaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                        Writer.WriteElementString("cbc:PaymentMeansCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("cbc:PaymentID", this.Descriptor.PaymentMeans.Information);

                        if (this.Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            Writer.WriteStartElement("cac:PayeeFinancialAccount", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                            Writer.WriteElementString("cbc:ID", this.Descriptor.PaymentMeans.FinancialCard.Id);
                            Writer.WriteElementString("cbc:Name", this.Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            Writer.WriteEndElement(); //!PayeeFinancialAccount
                        }
                        Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMeans
                    }
                }

            }
            else
            {
                foreach (BankAccount account in this.Descriptor.CreditorBankAccounts)
                {
                    if ((this.Descriptor.PaymentMeans != null) && (this.Descriptor.PaymentMeans.TypeCode != PaymentMeansTypeCodes.Unknown))
                    {
                        Writer.WriteStartElement("cac:PaymentMeans", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                        Writer.WriteElementString("cbc:PaymentMeansCode", this.Descriptor.PaymentMeans.TypeCode.EnumToString());
                        Writer.WriteOptionalElementString("cbc:PaymentID", this.Descriptor.PaymentMeans.Information);

                        if (this.Descriptor.PaymentMeans.FinancialCard != null)
                        {
                            Writer.WriteStartElement("cac:PayeeFinancialAccount", Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);
                            Writer.WriteElementString("cbc:ID", this.Descriptor.PaymentMeans.FinancialCard.Id);
                            Writer.WriteElementString("cbc:Name", this.Descriptor.PaymentMeans.FinancialCard.CardholderName);
                            Writer.WriteEndElement(); //!PayeeFinancialAccount
                        }
                        Writer.WriteEndElement(); // !SpecifiedTradeSettlementPaymentMea
                    }

                    //TODO Add Bank details data

                    //Writer.WriteStartElement("ram:PayeePartyCreditorFinancialAccount");
                    //Writer.WriteElementString("ram:IBANID", account.IBAN);
                    //Writer.WriteOptionalElementString("ram:AccountName", account.Name);
                    //Writer.WriteOptionalElementString("ram:ProprietaryID", account.ID);
                    //Writer.WriteEndElement(); // !PayeePartyCreditorFinancialAccount

                    //if (!String.IsNullOrWhiteSpace(account.BIC))
                    //{
                    //    Writer.WriteStartElement("ram:PayeeSpecifiedCreditorFinancialInstitution");
                    //    Writer.WriteElementString("ram:BICID", account.BIC);
                    //    Writer.WriteEndElement(); // !PayeeSpecifiedCreditorFinancialInstitution
                    //}

                }
            }


            // PaymentTerms (optional)
            if (this.Descriptor.PaymentTerms != null)
            {
                Writer.WriteStartElement("cac:PaymentTerms");
                Writer.WriteOptionalElementString("cbc:Note", this.Descriptor.PaymentTerms?.Description);
                Writer.WriteEndElement();
            }


            // Tax Total
            Writer.WriteStartElement("cac:TaxTotal");
            _writeOptionalAmount(Writer, "cbc:TaxAmount", this.Descriptor.TaxTotalAmount, forceCurrency: true);

            foreach (Tax tax in this.Descriptor.Taxes)
            {
                Writer.WriteStartElement("cac:TaxSubtotal");
                _writeOptionalAmount(Writer, "cbc:TaxableAmount", tax.BasisAmount, forceCurrency: true);
                _writeOptionalAmount(Writer, "cbc:TaxAmount", tax.TaxAmount, forceCurrency: true);

                Writer.WriteStartElement("cac:TaxCategory");
                Writer.WriteElementString("cbc:ID", tax.CategoryCode.ToString());
                Writer.WriteElementString("cbc:Percent", _formatDecimal(tax.Percent));

                Writer.WriteStartElement("cac:TaxScheme");
                Writer.WriteElementString("cbc:ID", tax.TypeCode.EnumToString());
                Writer.WriteEndElement();// !TaxScheme

                Writer.WriteEndElement();// !TaxCategory
                Writer.WriteEndElement();// !TaxSubtotal
            }

            Writer.WriteEndElement();// !TaxTotal

            Writer.WriteStartElement("cac:LegalMonetaryTotal");
            _writeOptionalAmount(Writer, "cbc:LineExtensionAmount", this.Descriptor.LineTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc:TaxExclusiveAmount", this.Descriptor.TaxBasisAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc:TaxInclusiveAmount", this.Descriptor.GrandTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc:ChargeTotalAmount", this.Descriptor.ChargeTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc:AllowanceTotalAmount", this.Descriptor.AllowanceTotalAmount, forceCurrency: true);
            //_writeOptionalAmount(Writer, "cbc:TaxAmount", this.Descriptor.TaxTotalAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc:PrepaidAmount", this.Descriptor.TotalPrepaidAmount, forceCurrency: true);
            _writeOptionalAmount(Writer, "cbc:PayableAmount", this.Descriptor.DuePayableAmount, forceCurrency: true);
            //_writeOptionalAmount(Writer, "cbc:PayableAlternativeAmount", this.Descriptor.RoundingAmount, forceCurrency: true);
            Writer.WriteEndElement(); //!LegalMonetaryTotal



            foreach (TradeLineItem tradeLineItem in this.Descriptor.TradeLineItems)
            {
                Writer.WriteStartElement("cac:InvoiceLine");
                Writer.WriteElementString("cbc:ID", tradeLineItem.AssociatedDocument.LineID);
                Writer.WriteEndElement(); //!InvoiceLine
            }


            Writer.WriteEndDocument(); //Invoice
            Writer.Flush();

            stream.Seek(streamPosition, SeekOrigin.Begin);

        }

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
                //case PartyTypes.ShipToTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.UltimateShipToTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.ShipFromTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.InvoiceeTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.PayeeTradeParty:
                //    if (this.Descriptor.Profile == Profile.Minimum) { return; } // party is written for all profiles but minimum
                //    break;
                //case PartyTypes.SalesAgentTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.BuyerTaxRepresentativeTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.ProductEndUserTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.BuyerAgentTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.InvoicerTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                //case PartyTypes.PayerTradeParty:
                //    if ((this.Descriptor.Profile != Profile.Extended) && (this.Descriptor.Profile != Profile.XRechnung1) && (this.Descriptor.Profile != Profile.XRechnung)) { return; } // extended, XRechnung1, XRechnung profile only
                //    break;
                default:
                    return;
            }

            if (party != null)
            {
                switch (partyType)
                {
                    case PartyTypes.SellerTradeParty:
                        writer.WriteStartElement("cac:AccountingSupplierParty", this.Descriptor.Profile);
                        break;
                    case PartyTypes.BuyerTradeParty:
                        writer.WriteStartElement("cac:AccountingCustomerParty", this.Descriptor.Profile);
                        break;
                        //case PartyTypes.ShipToTradeParty:
                        //    writer.WriteStartElement("ram:ShipToTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.UltimateShipToTradeParty:
                        //    writer.WriteStartElement("ram:UltimateShipToTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.ShipFromTradeParty:
                        //    writer.WriteStartElement("ram:ShipFromTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.InvoiceeTradeParty:
                        //    writer.WriteStartElement("ram:InvoiceeTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.PayeeTradeParty:
                        //    writer.WriteStartElement("ram:PayeeTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.PayerTradeParty:
                        //    writer.WriteStartElement("ram:PayerTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.SalesAgentTradeParty:
                        //    writer.WriteStartElement("ram:SalesAgentTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.BuyerTaxRepresentativeTradeParty:
                        //    writer.WriteStartElement("ram:BuyerTaxRepresentativeTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.ProductEndUserTradeParty:
                        //    writer.WriteStartElement("ram:ProductEndUserTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.BuyerAgentTradeParty:
                        //    writer.WriteStartElement("ram:BuyerAgentTradeParty", this.Descriptor.Profile);
                        //    break;
                        //case PartyTypes.InvoicerTradeParty:
                        //    writer.WriteStartElement("ram:InvoicerTradeParty", this.Descriptor.Profile);
                        //    break;
                }

                writer.WriteStartElement("cac:Party", this.Descriptor.Profile);

                if (ElectronicAddress != null)
                {
                    writer.WriteStartElement("cbc:EndpointID");
                    writer.WriteAttributeString("schemeID", ElectronicAddress.ElectronicAddressSchemeID.ToString());
                    writer.WriteValue(ElectronicAddress.Address);
                    writer.WriteEndElement();
                }

                writer.WriteStartElement("cac:PostalAddress");

                Writer.WriteElementString("cbc:StreetName", party.Street);
                Writer.WriteOptionalElementString("cbc:AdditionalStreetName", party.AddressLine3);
                Writer.WriteElementString("cbc:CityName", party.City);
                Writer.WriteElementString("cbc:PostalZone", party.Postcode);

                writer.WriteStartElement("cac:Country");
                Writer.WriteElementString("cbc:IdentificationCode", party.Country.ToString());
                writer.WriteEndElement(); //!Country

                writer.WriteEndElement(); //!PostalTradeAddress


                foreach (var tax in taxRegistrations)
                {
                    writer.WriteStartElement("cac:PartyTaxScheme");

                    Writer.WriteElementString("cbc:CompanyID", tax.No);

                    writer.WriteStartElement("cac:TaxScheme");

                    Writer.WriteElementString("cbc:ID", tax.SchemeID.ToString());

                    writer.WriteEndElement(); //!TaxScheme

                    writer.WriteEndElement(); //!PartyTaxScheme
                }


                writer.WriteStartElement("cac:PartyLegalEntity");

                writer.WriteElementString("cbc:RegistrationName", party.Name);

                writer.WriteEndElement(); //!PartyLegalEntity

                if (contact != null)
                {
                    writer.WriteStartElement("cac:Contact");

                    writer.WriteElementString("cbc:Name", contact.Name);
                    writer.WriteElementString("cbc:Telephone", contact.PhoneNo);
                    writer.WriteElementString("cbc:ElectronicMail", contact.EmailAddress);

                    writer.WriteEndElement();
                }



                writer.WriteEndElement(); //!Party



                //if (party.ID != null)
                //{
                //    if (!String.IsNullOrWhiteSpace(party.ID.ID) && (party.ID.SchemeID != GlobalIDSchemeIdentifiers.Unknown))
                //    {
                //        writer.WriteStartElement("ram:ID");
                //        writer.WriteAttributeString("schemeID", party.ID.SchemeID.EnumToString());
                //        writer.WriteValue(party.ID.ID);
                //        writer.WriteEndElement();
                //    }

                //    writer.WriteOptionalElementString("ram:ID", party.ID.ID);
                //}

                //if ((party.GlobalID != null) && !String.IsNullOrWhiteSpace(party.GlobalID.ID) && (party.GlobalID.SchemeID != GlobalIDSchemeIdentifiers.Unknown))
                //{
                //    writer.WriteStartElement("ram:GlobalID");
                //    writer.WriteAttributeString("schemeID", party.GlobalID.SchemeID.EnumToString());
                //    writer.WriteValue(party.GlobalID.ID);
                //    writer.WriteEndElement();
                //}
                //_writeOptionalLegalOrganization(writer, "ram:SpecifiedLegalOrganization", party.SpecifiedLegalOrganization, partyType);
                //_writeOptionalContact(writer, "ram:DefinedTradeContact", contact, Profile.Extended | Profile.XRechnung1 | Profile.XRechnung);

                //writer.WriteOptionalElementString("ram:PostcodeCode", party.Postcode); // buyer: BT-53
                //writer.WriteOptionalElementString("ram:LineOne", string.IsNullOrWhiteSpace(party.ContactName) ? party.Street : party.ContactName); // buyer: BT-50
                //if (!string.IsNullOrWhiteSpace(party.ContactName)) { writer.WriteOptionalElementString("ram:LineTwo", party.Street); } // buyer: BT-51

                //writer.WriteOptionalElementString("ram:LineThree", party.AddressLine3); // buyer: BT-163
                //writer.WriteOptionalElementString("ram:CityName", party.City); // buyer: BT-52
                //writer.WriteElementString("ram:CountryID", party.Country.EnumToString()); // buyer: BT-55
                //writer.WriteOptionalElementString("ram:CountrySubDivisionName", party.CountrySubdivisionName); // BT-79
                //writer.WriteEndElement(); // !PostalTradeAddress

                //if (ElectronicAddress != null)
                //{
                //    if (!String.IsNullOrWhiteSpace(ElectronicAddress.Address))
                //    {
                //        writer.WriteStartElement("ram:URIUniversalCommunication");
                //        writer.WriteStartElement("ram:URIID");
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
                //            writer.WriteStartElement("ram:SpecifiedTaxRegistration");
                //            writer.WriteStartElement("ram:ID");
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


        private void _writeNotes(ProfileAwareXmlTextWriter writer, List<Note> notes)
        {
            if (notes.Count > 0)
            {
                foreach (Note note in notes)
                {
                    writer.WriteElementString("cbc:Note", note.Content);
                }
            }
        } // !_writeNotes()


        private void _writeOptionalAmount(ProfileAwareXmlTextWriter writer, string tagName, decimal? value, int numDecimals = 2, bool forceCurrency = false, Profile profile = Profile.Unknown)
        {
            if (value.HasValue)
            {
                writer.WriteStartElement(tagName, profile);
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

            if (type == InvoiceType.CorrectionOld)
            {
                return (int)InvoiceType.Correction;
            }

            return (int)type;
        } // !_translateInvoiceType()
        internal override bool Validate(InvoiceDescriptor descriptor, bool throwExceptions = true)
        {
            throw new NotImplementedException();
        }
    }
}
