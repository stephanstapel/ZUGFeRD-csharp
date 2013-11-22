using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;


namespace s2industries.ZUGFeRD
{
    public class InvoiceDescriptorWriter
    {
        private XmlTextWriter Writer;
        private InvoiceDescriptor Descriptor;


        public void Save(InvoiceDescriptor descriptor, string filename)
        {
            this.Descriptor = descriptor;
            this.Writer = new XmlTextWriter(filename, Encoding.UTF8);
            Writer.Formatting = Formatting.Indented;
            Writer.WriteStartDocument();

            #region Kopfbereich
            Writer.WriteStartElement("rsm:Invoice");
            Writer.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");
            Writer.WriteAttributeString("xmlns", "rsm", null, "urn:un:unece:uncefact:data:standard:CBFBUY:5");
            Writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            Writer.WriteAttributeString("xsi", "schemaLocation", null, "urn:un:unece:uncefact:data:standard:CBFBUY:5 ../Schema/Invoice.xsd");
            #endregion

            #region SpecifiedExchangedDocumentContext
            Writer.WriteStartElement("rsm:SpecifiedExchangedDocumentContext");
            Writer.WriteElementString("TestIndicator", this.Descriptor.IsTest ? "true" : "false");
            Writer.WriteStartElement("GuidelineSpecifiedDocumentContextParameter");
            Writer.WriteElementString("ID", _translateProfile(this.Descriptor.Profile));
            Writer.WriteEndElement(); // !GuidelineSpecifiedDocumentContextParameter
            Writer.WriteEndElement(); // !rsm:SpecifiedExchangedDocumentContext

            Writer.WriteStartElement("rsm:HeaderExchangedDocument");
            Writer.WriteElementString("ID", this.Descriptor._InvoiceNo);
            Writer.WriteElementString("Name", _translateInvoiceType(this.Descriptor.Type));
            Writer.WriteElementString("TypeCode", String.Format("{0}", _encodeInvoiceType(this.Descriptor.Type)));
            Writer.WriteStartElement("IssueDateTime");
            Writer.WriteAttributeString("format", "102");
            Writer.WriteValue(_formatDate(this.Descriptor._InvoiceDate));
            Writer.WriteEndElement(); // !IssueDateTime()
            _writeOptionalNotes(Writer);
            Writer.WriteEndElement(); // !rsm:HeaderExchangedDocument
            #endregion

            #region SpecifiedSupplyChainTradeTransaction
            Writer.WriteStartElement("rsm:SpecifiedSupplyChainTradeTransaction");
            Writer.WriteStartElement("ApplicableSupplyChainTradeAgreement");
            Writer.WriteElementString("BuyerReference", this.Descriptor.ReferenceOrderNo);

            _writeOptionalParty(Writer, "SellerTradeParty", this.Descriptor._Seller, TaxRegistrations: this.Descriptor._SellerTaxRegistration);
            _writeOptionalParty(Writer, "BuyerTradeParty", this.Descriptor._Buyer, this.Descriptor._BuyerContact, TaxRegistrations: this.Descriptor._BuyerTaxRegistration);

            if ((this.Descriptor._OrderDate != DateTime.MinValue) && (this.Descriptor._OrderNo.Length > 0))
            {
                Writer.WriteStartElement("BuyerOrderReferencedDocument");
                Writer.WriteStartElement("IssueDateTime");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor._OrderDate));
                Writer.WriteEndElement(); // !IssueDateTime()
                Writer.WriteElementString("ID", this.Descriptor._OrderNo);
                Writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }

            Writer.WriteEndElement(); // !ApplicableSupplyChainTradeAgreement

            Writer.WriteStartElement("ApplicableSupplyChainTradeDelivery");
            if (this.Descriptor.ActualDeliveryDate != DateTime.MinValue)
            {
                Writer.WriteStartElement("ActualDeliverySupplyChainEvent");
                Writer.WriteStartElement("OccurrenceDateTime");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor.ActualDeliveryDate));
                Writer.WriteEndElement(); // !IssueDateTime()
                Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }

            if ((this.Descriptor._DeliveryNoteDate != DateTime.MinValue) && (this.Descriptor._DeliveryNoteNo.Length > 0))
            {
                Writer.WriteStartElement("DeliveryNoteReferencedDocument");
                Writer.WriteElementString("ID", this.Descriptor._DeliveryNoteNo);
                Writer.WriteStartElement("IssueDateTime");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor._DeliveryNoteDate));
                Writer.WriteEndElement(); // !IssueDateTime()
                Writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }
            Writer.WriteEndElement(); // !ApplicableSupplyChainTradeDelivery

            Writer.WriteStartElement("ApplicableSupplyChainTradeSettlement");
            if (this.Descriptor._InvoiceNoAsReference.Length > 0)
            {
                Writer.WriteElementString("PaymentReference", this.Descriptor._InvoiceNoAsReference);
            }
            Writer.WriteElementString("InvoiceCurrencyCode", this.Descriptor._Currency.ToString());
            _writeOptionalTaxes(Writer);

            if (this.Descriptor._ServiceCharge != null)
            {
                Writer.WriteStartElement("SpecifiedLogisticsServiceCharge");
                if (this.Descriptor._ServiceCharge.Description.Length > 0)
                {
                    Writer.WriteElementString("Description", this.Descriptor._ServiceCharge.Description);
                }
                Writer.WriteElementString("AppliedAmount", _formatCurrency(this.Descriptor._ServiceCharge.Amount));
                if (this.Descriptor._ServiceCharge.Tax != null)
                {
                    Writer.WriteStartElement("AppliedTradeTax");
                    Writer.WriteElementString("TypeCode", _translateTaxType(this.Descriptor._ServiceCharge.Tax.TypeCode));
                    Writer.WriteElementString("CategoryCode", _translateTaxCategoryCode(this.Descriptor._ServiceCharge.Tax.CategoryCode));
                    Writer.WriteElementString("ApplicablePercent", this.Descriptor._ServiceCharge.Tax.Percent.ToString("#"));
                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();
            }

            if (this.Descriptor._PaymentTerms != null)
            {
                Writer.WriteStartElement("SpecifiedTradePaymentTerms");
                if (this.Descriptor._PaymentTerms.Description.Length > 0)
                {
                    Writer.WriteElementString("Description", this.Descriptor._PaymentTerms.Description);
                }
                Writer.WriteStartElement("DueDateDateTime");
                Writer.WriteAttributeString("format", "102");
                Writer.WriteValue(_formatDate(this.Descriptor._PaymentTerms.DueDate));
                Writer.WriteEndElement();
                Writer.WriteEndElement();
            }

            Writer.WriteStartElement("SpecifiedTradeSettlementMonetarySummation");
            _writeOptionalAmount(Writer, "LineTotalAmount", this.Descriptor.LineTotalAmount);
            _writeOptionalAmount(Writer, "ChargeTotalAmount", this.Descriptor.ChargeTotalAmount);
            _writeOptionalAmount(Writer, "AllowanceTotalAmount", this.Descriptor.AllowanceTotalAmount);
            _writeOptionalAmount(Writer, "TaxBasisTotalAmount", this.Descriptor.TaxBasisAmount);
            _writeOptionalAmount(Writer, "TaxTotalAmount", this.Descriptor.TaxTotalAmount);
            _writeOptionalAmount(Writer, "GrandTotalAmount", this.Descriptor.GrandTotalAmount);
            _writeOptionalAmount(Writer, "TotalPrepaidAmount", this.Descriptor.TotalPrepaidAmount);
            _writeOptionalAmount(Writer, "DuePayableAmount", this.Descriptor.DuePayableAmount);
            Writer.WriteEndElement(); // !SpecifiedTradeSettlementMonetarySummation

            Writer.WriteEndElement(); // !ApplicableSupplyChainTradeSettlement
            Writer.WriteEndElement(); // !SpecifiedSupplyChainTradeTransaction
            #endregion

            Writer.WriteEndElement(); // !Invoice
            Writer.WriteEndDocument();

            Writer.Close();
        } // !Save()


        private void _writeOptionalAmount(XmlTextWriter writer, string tagName, decimal value)
        {
            if (value != decimal.MinValue)
            {
                writer.WriteStartElement(tagName);
                writer.WriteAttributeString("currencyID", this.Descriptor._Currency.ToString());
                writer.WriteValue(_formatCurrency(value));
                writer.WriteEndElement(); // !tagName
            }
        }


        private void _writeOptionalTaxes(XmlTextWriter writer)
        {
            foreach (Tax tax in this.Descriptor._Taxes)
            {
                writer.WriteStartElement("ApplicableTradeTax");

                writer.WriteStartElement("CalculatedAmount");
                writer.WriteAttributeString("currencyID", this.Descriptor._Currency.ToString());
                writer.WriteValue(_formatCurrency(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount

                writer.WriteElementString("TypeCode", _translateTaxType(tax.TypeCode));

                writer.WriteStartElement("BasisAmount");
                writer.WriteAttributeString("currencyID", this.Descriptor._Currency.ToString());
                writer.WriteValue(_formatCurrency(tax.BasisAmount));
                writer.WriteEndElement(); // !BasisAmount

                writer.WriteElementString("CategoryCode", _translateTaxCategoryCode(tax.CategoryCode));
                writer.WriteElementString("ApplicablePercent", tax.Percent.ToString("#"));
                writer.WriteEndElement(); // !ApplicableTradeTax
            }
        } // !_writeOptionalTaxes()


        private void _writeOptionalNotes(XmlTextWriter writer)
        {
            if (this.Descriptor._Notes.Count > 0)
            {
                foreach (Tuple<string, SubjectCode> t in this.Descriptor._Notes)
                {
                    writer.WriteStartElement("IncludedNote");
                    writer.WriteElementString("Content", t.Item1);
                    if (t.Item2 != SubjectCode.Unknown)
                    {
                        writer.WriteElementString("SubjectCode", _translateSubjectCode(t.Item2));
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

                if ((Party.GlobalID != null) && (Party.GlobalID.ID.Length > 0) && (Party.GlobalID.SchemeID.Length > 0))
                {
                    writer.WriteStartElement("GlobalID");
                    writer.WriteAttributeString("schemeID", Party.GlobalID.SchemeID);
                    writer.WriteValue(Party.GlobalID.ID);
                    writer.WriteEndElement();
                }

                writer.WriteElementString("Name", Party.Name);

                if (Contact != null)
                {
                    _writeOptionalContact(writer, "DefinedTradeContact", Contact);
                }

                writer.WriteStartElement("PostalTradeAddress");
                writer.WriteElementString("PostcodeCode", Party.Postcode);
                writer.WriteElementString("LineOne", _formatStreet(Party.Street, Party.StreetNo));
                writer.WriteElementString("CityName", Party.City);
                writer.WriteElementString("CountryID", Party.Country);
                writer.WriteEndElement(); // !PostalTradeAddress

                if (TaxRegistrations != null)
                {
                    foreach (TaxRegistration _reg in TaxRegistrations)
                    {
                        writer.WriteStartElement("SpecifiedTaxRegistration");
                        writer.WriteStartElement("ID");
                        writer.WriteAttributeString("schemeID", _translateTaxRegistrationSchemeID(_reg.SchemeID));
                        writer.WriteValue(_reg.No);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
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

                if (contact.Name.Length > 0)
                {
                    writer.WriteElementString("Name", contact.Name);
                }

                ///
                /// TODO: restliche Kontaktattribute einpflegen
                ///

                writer.WriteEndElement();
            }
        } // !_writeOptionalContact()


        private string _formatCurrency(decimal value)
        {
            return value.ToString("0.00").Replace(",", ".");
        } // !_formatCurrency()


        private string _formatStreet(string street, string streetNo)
        {
            string retval = street;
            if (streetNo.Length > 0)
            {
                retval += " " + streetNo;
            }
            return retval;
        } // !_formatStreet()


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


        private string _translateProfile(Profile profile)
        {
            switch (profile)
            {
                case Profile.Basic: return "urn:ferd:invoice:1.0:basic";
                case Profile.Comfort: return "urn:ferd:invoice:1.0:comfort";
                case Profile.Extended: return "urn:ferd:invoice:1.0:extended";
                default: return "";
            }
        } // !_translateProfile()


        private string _translateTaxRegistrationSchemeID(TaxRegistrationSchemeID schemeID)
        {
            return schemeID.ToString("g");
        } // !_translateTaxRegistrationSchemeID()


        private string _translateSubjectCode(SubjectCode code)
        {
            return code.ToString("g");
        } // !_translateSubjectCode()


        private string _translateTaxType(TaxType type)
        {
            return type.ToString("g");
        } // !_translateTaxType()


        private string _translateTaxCategoryCode(TaxCategoryCode code)
        {
            return code.ToString("g");
        } // !_translateTaxCategoryCode()


        private string _formatDate(DateTime date)
        {
            return date.ToString("yyyyMMdd");
        } // !_formatDate()
    }
}
