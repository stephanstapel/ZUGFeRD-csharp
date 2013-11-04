using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// TODO:
    /// * TaxRegistration SchemeID (VA, FC) als enum
    /// * SubjectCode (AAK etc.) als Enum
    /// * Mehrwertsteuercode als Enum
    /// </summary>
    public class InvoiceDescriptor
    {
        private string _InvoiceNo { get; set; }
        private DateTime _InvoiceDate { get; set; }
        private string _InvoiceNoAsReference { get; set; }

        private string _OrderNo { get; set; }
        private DateTime _OrderDate { get; set; }

        private string _DeliveryNoteNo { get; set; }
        private DateTime _DeliveryNoteDate { get; set; }
        public DateTime ActualDeliveryDate { get; set; }

        private CurrencyCodes _Currency { get; set; }
        private Party _Buyer { get; set; }
        private Contact _BuyerContact { get; set; }
        private List<TaxRegistration> _BuyerTaxRegistration { get; set; }
        private Party _Seller { get; set; }
        private List<TaxRegistration> _SellerTaxRegistration { get; set; }
        private List<Tuple<string, string>> _Notes { get; set; }

        public bool IsTest { get; set; }
        public Profile Profile { get; set; }
        public InvoiceType Type { get; set; }
        public string ReferenceOrderNo { get; set; }

        
        public decimal LineTotalAmount { get; set; }
        public decimal ChargeTotalAmount { get; set; }
        public decimal AllowanceTotalAmount { get; set; }
        public decimal TaxBasisAmount { get; set; }
        public decimal TaxTotalAmount { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public decimal TotalPrepaidAmount { get; set; }
        public decimal DuePayableAmount { get; set; }
        private List<Tax> _Taxes { get; set; }
        private ServiceCharge _ServiceCharge { get; set; }
        private PaymentTerms _PaymentTerms { get; set; }
   

        public InvoiceDescriptor()
        {
            this._InvoiceNoAsReference = "";

            this.IsTest = false;
            this.Profile = Profile.Basic;
            this.Type = InvoiceType.Invoice;
            this._Notes = new List<Tuple<string, string>>();
            this._OrderDate = DateTime.MinValue;
            this._InvoiceDate = DateTime.MinValue;
            this._DeliveryNoteDate = DateTime.MinValue;
            this.ActualDeliveryDate = DateTime.MinValue;

            this.LineTotalAmount = decimal.MinValue;
            this.ChargeTotalAmount = decimal.MinValue;
            this.AllowanceTotalAmount = decimal.MinValue;
            this.TaxBasisAmount = decimal.MinValue;
            this.TaxTotalAmount = decimal.MinValue;
            this.GrandTotalAmount = decimal.MinValue;
            this.TotalPrepaidAmount = decimal.MinValue;
            this.DuePayableAmount = decimal.MinValue;
            this._Taxes = new List<Tax>();

            this._BuyerTaxRegistration = new List<TaxRegistration>();
            this._SellerTaxRegistration = new List<TaxRegistration>();
        }


        public static InvoiceDescriptor Load(string filename)
        {
            throw new NotImplementedException();
        } // !Load()


        public static InvoiceDescriptor CreateInvoice(string invoiceNo, DateTime invoiceDate, CurrencyCodes currency, string invoiceNoAsReference = "")
        {
            InvoiceDescriptor retval = new InvoiceDescriptor();
            retval._InvoiceDate = invoiceDate;
            retval._InvoiceNo = invoiceNo;
            retval._Currency = currency;
            retval._InvoiceNoAsReference = invoiceNoAsReference;
            return retval;
        } // !CreateInvoice()


        public void AddNote(string note, string type = "")
        {
            this._Notes.Add(new Tuple<string, string>(note, type));
        } // !AddNote()
        

        public void SetBuyer(string name, string postcode, string city, string street, string streetno, string country, string globalIDSchemeID = "", string globalID = "")
        {
            this._Buyer = new Party()
            {
                Name = name,
                Postcode = postcode,
                City = city,
                Street = street,
                StreetNo = streetno,
                Country = country,
                GlobalID = new GlobalID()
                {
                    ID = globalID,
                    SchemeID = globalIDSchemeID,
                }
            };
        }


        public void SetSeller(string name, string postcode, string city, string street, string streetno, string country, string globalIDSchemeID = "", string globalID = "")
        {
            this._Seller = new Party()
            {
                Name = name,
                Postcode = postcode,
                City = city,
                Street = street,
                StreetNo = streetno,
                Country = country,
                GlobalID = new GlobalID()
                {
                    ID = globalID,
                    SchemeID = globalIDSchemeID,
                }
            };
        } // !SetSeller()


        public void SetBuyerContact(string name, string orgunit = "", string emailAddress = "", string phoneno = "", string faxno = "")
        {
            this._BuyerContact = new Contact()
            {
                Name = name,
                OrgUnit = orgunit,
                EmailAddress = emailAddress,
                PhoneNo = phoneno,
                FaXNo = faxno
            };
        } // !SetBuyerContact()


        public void AddBuyerTaxRegistration(string no, string schemeID)
        {
            this._BuyerTaxRegistration.Add(new TaxRegistration()
            {
                No = no,
                SchemeID = schemeID
            });
        } // !AddBuyerTaxRegistration()


        public void AddSellerTaxRegistration(string no, string schemeID)
        {
            this._SellerTaxRegistration.Add(new TaxRegistration()
            {
                No = no,
                SchemeID = schemeID
            });
        } // !AddSellerTaxRegistration()


        public void SetBuyerOrderReferenceDocument(string orderNo, DateTime orderDate)
        {
            this._OrderNo = orderNo;
            this._OrderDate = orderDate;
        }



        public void SetDeliveryNoteReferenceDocument(string deliveryNoteNo, DateTime deliveryNoteDate)
        {
            this._DeliveryNoteNo = deliveryNoteNo;
            this._DeliveryNoteDate = deliveryNoteDate;
        } // !SetDeliveryNoteReferenceDocument()


        public void SetLogisticsServiceCharge(decimal amount, string description, string taxTypeCode, string taxCategoryCode, decimal taxPercent)
        {
            this._ServiceCharge = new ServiceCharge()
            {
                Description = description,
                Amount = amount,
                Tax = new Tax()
                {
                    CategoryCode = taxCategoryCode,
                    TypeCode = taxTypeCode,
                    Percent = taxPercent
                }
            };
        } // !SetLogisticsServiceCharge()


        public void setTradePaymentTerms(string description, DateTime dueDate)
        {
            this._PaymentTerms = new PaymentTerms()
            {
                Description = description,
                DueDate = dueDate
            };
        }

        /// <summary>
        /// Detailinformationen zu Belegsummen
        /// </summary>
        /// <param name="lineTotalAmount">Gesamtbetrag der Positionen</param>
        /// <param name="chargeTotalAmount">Gesamtbetrag der Zuschläge</param>
        /// <param name="allowanceTotalAmount">Gesamtbetrag der Abschläge</param>
        /// <param name="taxBasisAmount">Basisbetrag der Steuerberechnung</param>
        /// <param name="taxTotalAmount">Steuergesamtbetrag</param>
        /// <param name="grandTotalAmount">Bruttosumme</param>
        /// <param name="totalPrepaidAmount">Anzahlungsbetrag</param>
        /// <param name="duePayableAmount">Zahlbetrag</param>
        public void SetTotals(decimal lineTotalAmount = decimal.MinValue, decimal chargeTotalAmount = decimal.MinValue,
                              decimal allowanceTotalAmount = decimal.MinValue, decimal taxBasisAmount = decimal.MinValue,
                              decimal taxTotalAmount = decimal.MinValue, decimal grandTotalAmount = decimal.MinValue,
                              decimal totalPrepaidAmount = decimal.MinValue, decimal duePayableAmount = decimal.MinValue)
        {
            this.LineTotalAmount = lineTotalAmount;
            this.ChargeTotalAmount = chargeTotalAmount;
            this.AllowanceTotalAmount = allowanceTotalAmount;
            this.TaxBasisAmount = taxBasisAmount;
            this.TaxTotalAmount = taxTotalAmount;
            this.GrandTotalAmount = grandTotalAmount;
            this.TotalPrepaidAmount = totalPrepaidAmount;
            this.DuePayableAmount = duePayableAmount;
        }


        public void AddApplicableTradeTax(decimal taxAmount, decimal basisAmount, decimal percent, string typeCode, string categoryCode)
        {
            this._Taxes.Add(new Tax()
            {
                TaxAmount = taxAmount,
                BasisAmount = basisAmount,
                Percent = percent,
                TypeCode = typeCode,
                CategoryCode = categoryCode
            });
        } // !AddApplicableTradeTax()


        public void Save(string filename)
        {
            XmlTextWriter writer = new XmlTextWriter(filename, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();

            #region Kopfbereich
            writer.WriteStartElement("rsm:Invoice");
            writer.WriteAttributeString("xmlns", "xs", null, "http://www.w3.org/2001/XMLSchema");
            writer.WriteAttributeString("xmlns", "rsm", null, "urn:un:unece:uncefact:data:standard:CBFBUY:5");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "schemaLocation", null, "urn:un:unece:uncefact:data:standard:CBFBUY:5 ../Schema/Invoice.xsd");
            #endregion

            #region SpecifiedExchangedDocumentContext
            writer.WriteStartElement("rsm:SpecifiedExchangedDocumentContext");
            writer.WriteElementString("TestIndicator", this.IsTest ? "true" : "false");
            writer.WriteStartElement("GuidelineSpecifiedDocumentContextParameter");
            writer.WriteElementString("ID", _translateProfile(this.Profile));
            writer.WriteEndElement(); // !GuidelineSpecifiedDocumentContextParameter
            writer.WriteEndElement(); // !rsm:SpecifiedExchangedDocumentContext

            writer.WriteStartElement("rsm:HeaderExchangedDocument");
            writer.WriteElementString("ID", this._InvoiceNo);
            writer.WriteElementString("Name", _translateInvoiceType(this.Type));
            writer.WriteElementString("TypeCode", String.Format("{0}", _encodeInvoiceType(this.Type)));
            writer.WriteStartElement("IssueDateTime");
            writer.WriteAttributeString("format", "102");
            writer.WriteValue(_formatDate(this._InvoiceDate));
            writer.WriteEndElement(); // !IssueDateTime()
            _writeOptionalNotes(writer);
            writer.WriteEndElement(); // !rsm:HeaderExchangedDocument
            #endregion

            #region SpecifiedSupplyChainTradeTransaction
            writer.WriteStartElement("rsm:SpecifiedSupplyChainTradeTransaction");
            writer.WriteStartElement("ApplicableSupplyChainTradeAgreement");
            writer.WriteElementString("BuyerReference", this.ReferenceOrderNo);

            _writeOptionalParty(writer, "SellerTradeParty", this._Seller, TaxRegistrations : this._SellerTaxRegistration);
            _writeOptionalParty(writer, "BuyerTradeParty", this._Buyer, this._BuyerContact, TaxRegistrations : this._BuyerTaxRegistration);

            if ((this._OrderDate != DateTime.MinValue) && (this._OrderNo.Length > 0))
            {
                writer.WriteStartElement("BuyerOrderReferencedDocument");
                writer.WriteStartElement("IssueDateTime");
                writer.WriteAttributeString("format", "102");
                writer.WriteValue(_formatDate(this._OrderDate));
                writer.WriteEndElement(); // !IssueDateTime()
                writer.WriteElementString("ID", this._OrderNo);
                writer.WriteEndElement(); // !BuyerOrderReferencedDocument
            }

            writer.WriteEndElement(); // !ApplicableSupplyChainTradeAgreement

            writer.WriteStartElement("ApplicableSupplyChainTradeDelivery");
            if (this.ActualDeliveryDate != DateTime.MinValue)
            {
                writer.WriteStartElement("ActualDeliverySupplyChainEvent");
                writer.WriteStartElement("OccurrenceDateTime");
                writer.WriteAttributeString("format", "102");
                writer.WriteValue(_formatDate(this.ActualDeliveryDate));
                writer.WriteEndElement(); // !IssueDateTime()
                writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }

            if ((this._DeliveryNoteDate != DateTime.MinValue) && (this._DeliveryNoteNo.Length > 0))
            {
                writer.WriteStartElement("DeliveryNoteReferencedDocument");
                writer.WriteElementString("ID", this._DeliveryNoteNo);
                writer.WriteStartElement("IssueDateTime");
                writer.WriteAttributeString("format", "102");
                writer.WriteValue(_formatDate(this._DeliveryNoteDate));
                writer.WriteEndElement(); // !IssueDateTime()
                writer.WriteEndElement(); // !DeliveryNoteReferencedDocument
            }
            writer.WriteEndElement(); // !ApplicableSupplyChainTradeDelivery

            writer.WriteStartElement("ApplicableSupplyChainTradeSettlement");
            if (this._InvoiceNoAsReference.Length > 0)
            {
                writer.WriteElementString("PaymentReference", this._InvoiceNoAsReference);
            }
            writer.WriteElementString("InvoiceCurrencyCode", this._Currency.ToString());
            _writeOptionalTaxes(writer);

            if (this._ServiceCharge != null)
            {
                writer.WriteStartElement("SpecifiedLogisticsServiceCharge");
                if (this._ServiceCharge.Description.Length > 0)
                {
                    writer.WriteElementString("Description", this._ServiceCharge.Description);
                }
                writer.WriteElementString("AppliedAmount", _formatCurrency(this._ServiceCharge.Amount));
                if (this._ServiceCharge.Tax != null)
                {
                    writer.WriteStartElement("AppliedTradeTax");
                    writer.WriteElementString("TypeCode", this._ServiceCharge.Tax.TypeCode);
                    writer.WriteElementString("CategoryCode", this._ServiceCharge.Tax.CategoryCode);
                    writer.WriteElementString("ApplicablePercent", this._ServiceCharge.Tax.Percent.ToString("#"));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            if (this._PaymentTerms != null)
            {
                writer.WriteStartElement("SpecifiedTradePaymentTerms");
                if (this._PaymentTerms.Description.Length > 0)
                {
                    writer.WriteElementString("Description", this._PaymentTerms.Description);
                }
                writer.WriteStartElement("DueDateDateTime");
                writer.WriteAttributeString("format", "102");
                writer.WriteValue(_formatDate(this._PaymentTerms.DueDate));
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteStartElement("SpecifiedTradeSettlementMonetarySummation");
            _writeOptionalAmount(writer, "LineTotalAmount", this.LineTotalAmount);
            _writeOptionalAmount(writer, "ChargeTotalAmount", this.ChargeTotalAmount);
            _writeOptionalAmount(writer, "AllowanceTotalAmount", this.AllowanceTotalAmount);
            _writeOptionalAmount(writer, "TaxBasisTotalAmount", this.TaxBasisAmount);
            _writeOptionalAmount(writer, "TaxTotalAmount", this.TaxTotalAmount);
            _writeOptionalAmount(writer, "GrandTotalAmount", this.GrandTotalAmount);
            _writeOptionalAmount(writer, "TotalPrepaidAmount", this.TotalPrepaidAmount);
            _writeOptionalAmount(writer, "DuePayableAmount", this.DuePayableAmount);
            writer.WriteEndElement(); // !SpecifiedTradeSettlementMonetarySummation

            writer.WriteEndElement(); // !ApplicableSupplyChainTradeSettlement
            writer.WriteEndElement(); // !SpecifiedSupplyChainTradeTransaction
            #endregion

            writer.WriteEndElement(); // !Invoice
            writer.WriteEndDocument();

            writer.Close();
        }


        private void _writeOptionalAmount(XmlTextWriter writer, string tagName, decimal value)
        {
            if (value != decimal.MinValue)
            {
                writer.WriteStartElement(tagName);
                writer.WriteAttributeString("currencyID", this._Currency.ToString());
                writer.WriteValue(_formatCurrency(value));
                writer.WriteEndElement(); // !tagName
            }
        }


        private void _writeOptionalTaxes(XmlTextWriter writer)
        {
            foreach (Tax tax in this._Taxes)
            {
                writer.WriteStartElement("ApplicableTradeTax");

                writer.WriteStartElement("CalculatedAmount");
                writer.WriteAttributeString("currencyID", this._Currency.ToString());
                writer.WriteValue(_formatCurrency(tax.TaxAmount));
                writer.WriteEndElement(); // !CalculatedAmount
                
                writer.WriteElementString("TypeCode", tax.TypeCode);

                writer.WriteStartElement("BasisAmount");
                writer.WriteAttributeString("currencyID", this._Currency.ToString());
                writer.WriteValue(_formatCurrency(tax.BasisAmount));
                writer.WriteEndElement(); // !BasisAmount

                writer.WriteElementString("CategoryCode", tax.CategoryCode);
                writer.WriteElementString("ApplicablePercent", tax.Percent.ToString("#"));
                writer.WriteEndElement(); // !ApplicableTradeTax
            }
        } // !_writeOptionalTaxes()


        private void _writeOptionalNotes(XmlTextWriter writer)
        {
            if (this._Notes.Count > 0)
            {
                foreach (Tuple<string, string> t in this._Notes)
                {
                    writer.WriteStartElement("IncludedNote");
                    writer.WriteElementString("Content", t.Item1);
                    if (t.Item2.Length > 0)
                    {
                        writer.WriteElementString("SubjectCode", t.Item2);
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
                        writer.WriteAttributeString("schemeID", _reg.SchemeID);
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
        }


        private string _formatDate(DateTime date)
        {
            return date.ToString("yyyyMMdd");
        } // !_formatDate()
    }
}
