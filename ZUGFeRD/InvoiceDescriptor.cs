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
using System.Linq;
using System.Text;


namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// TODO: Ländercode als enum
    /// </summary>
    public class InvoiceDescriptor
    {
        internal string _InvoiceNo { get; set; }
        internal DateTime _InvoiceDate { get; set; }
        internal string _InvoiceNoAsReference { get; set; }

        internal string _OrderNo { get; set; }
        internal DateTime _OrderDate { get; set; }

        internal string _DeliveryNoteNo { get; set; }
        internal DateTime _DeliveryNoteDate { get; set; }
        public DateTime ActualDeliveryDate { get; set; }

        internal CurrencyCodes _Currency { get; set; }
        internal Party _Buyer { get; set; }
        internal Contact _BuyerContact { get; set; }
        internal List<TaxRegistration> _BuyerTaxRegistration { get; set; }
        internal Party _Seller { get; set; }
        internal List<TaxRegistration> _SellerTaxRegistration { get; set; }
        internal List<Tuple<string, SubjectCode>> _Notes { get; set; }

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
        internal List<Tax> _Taxes { get; set; }
        internal List<ServiceCharge> _ServiceCharges { get; set; }
        internal List<TradeAllowanceCharge> _TradeAllowanceCharges { get; set; }
        internal PaymentTerms _PaymentTerms { get; set; }
   

        public InvoiceDescriptor()
        {
            this._InvoiceNoAsReference = "";

            this.IsTest = false;
            this.Profile = Profile.Basic;
            this.Type = InvoiceType.Invoice;
            this._Notes = new List<Tuple<string, SubjectCode>>();
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
            this._ServiceCharges = new List<ServiceCharge>();
            this._TradeAllowanceCharges = new List<TradeAllowanceCharge>();

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


        public void AddNote(string note, SubjectCode code = SubjectCode.Unknown)
        {
            this._Notes.Add(new Tuple<string, SubjectCode>(note, code));
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


        public void AddBuyerTaxRegistration(string no, TaxRegistrationSchemeID schemeID)
        {
            this._BuyerTaxRegistration.Add(new TaxRegistration()
            {
                No = no,
                SchemeID = schemeID
            });
        } // !AddBuyerTaxRegistration()


        public void AddSellerTaxRegistration(string no, TaxRegistrationSchemeID schemeID)
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


        public void AddLogisticsServiceCharge(decimal amount, string description, TaxType taxTypeCode, TaxCategoryCode taxCategoryCode, decimal taxPercent)
        {
            this._ServiceCharges.Add(new ServiceCharge()
            {
                Description = description,
                Amount = amount,
                Tax = new Tax()
                {
                    CategoryCode = taxCategoryCode,
                    TypeCode = taxTypeCode,
                    Percent = taxPercent
                }
            });
        } // !AddLogisticsServiceCharge()


        public void AddTradeAllowanceCharge(bool isDiscount, decimal basisAmount, CurrencyCodes currency, decimal actualAmount, string reason, TaxType taxTypeCode, TaxCategoryCode taxCategoryCode, decimal taxPercent)
        {
            this._TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Reason = reason,
                BasisAmount = basisAmount,
                Currency = currency,
                Amount = actualAmount,
                Tax = new Tax()
                {
                    CategoryCode = taxCategoryCode,
                    TypeCode = taxTypeCode,
                    Percent = taxPercent
                }
            });
        } // !AddTradeAllowanceCharge()


        public void SetTradePaymentTerms(string description, DateTime dueDate)
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


        public void AddApplicableTradeTax(decimal taxAmount, decimal basisAmount, decimal percent, TaxType typeCode, TaxCategoryCode categoryCode)
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
            InvoiceDescriptorWriter writer = new InvoiceDescriptorWriter();
            writer.Save(this, filename);
        } // !Save()
    }
}
