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
using System.IO;


namespace s2industries.ZUGFeRD
{
    public class InvoiceDescriptor
    {
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceNoAsReference { get; set; }

        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }

        public string DeliveryNoteNo { get; set; }
        public DateTime? DeliveryNoteDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        public CurrencyCodes Currency { get; set; }
        public Party Buyer { get; set; }
        public Contact BuyerContact { get; set; }
        public List<TaxRegistration> BuyerTaxRegistration { get; set; }
        public Party Seller { get; set; }
        public Contact SellerContact { get; set; }
        public List<TaxRegistration> SellerTaxRegistration { get; set; }
        public List<Tuple<string, SubjectCodes>> Notes { get; set; }

        public bool IsTest { get; set; }
        public Profile Profile { get; set; }
        public InvoiceType Type { get; set; }
        public string ReferenceOrderNo { get; set; }
        public List<TradeLineItem> TradeLineItems { get; set; }

        
        public decimal LineTotalAmount { get; set; }
        public decimal ChargeTotalAmount { get; set; }
        public decimal AllowanceTotalAmount { get; set; }
        public decimal TaxBasisAmount { get; set; }
        public decimal TaxTotalAmount { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public decimal TotalPrepaidAmount { get; set; }
        public decimal DuePayableAmount { get; set; }
        public List<Tax> Taxes { get; set; }
        public List<ServiceCharge> ServiceCharges { get; set; }
        public List<TradeAllowanceCharge> TradeAllowanceCharges { get; set; }
        public PaymentTerms PaymentTerms { get; set; }
        public List<BankAccount> CreditorBankAccounts { get; set; }
        public PaymentMeans PaymentMeans { get; set; }
   

        internal InvoiceDescriptor()
        {
            this.InvoiceNoAsReference = "";

            this.IsTest = false;
            this.Profile = Profile.Basic;
            this.Type = InvoiceType.Invoice;
            this.Notes = new List<Tuple<string, SubjectCodes>>();
            this.OrderDate = DateTime.MinValue;
            this.InvoiceDate = DateTime.MinValue;
            this.DeliveryNoteDate = DateTime.MinValue;
            this.ActualDeliveryDate = DateTime.MinValue;

            this.LineTotalAmount = decimal.MinValue;
            this.ChargeTotalAmount = decimal.MinValue;
            this.AllowanceTotalAmount = decimal.MinValue;
            this.TaxBasisAmount = decimal.MinValue;
            this.TaxTotalAmount = decimal.MinValue;
            this.GrandTotalAmount = decimal.MinValue;
            this.TotalPrepaidAmount = decimal.MinValue;
            this.DuePayableAmount = decimal.MinValue;
            this.TradeLineItems = new List<TradeLineItem>();
            this.Taxes = new List<Tax>();
            this.ServiceCharges = new List<ServiceCharge>();
            this.TradeAllowanceCharges = new List<TradeAllowanceCharge>();

            this.BuyerTaxRegistration = new List<TaxRegistration>();
            this.SellerTaxRegistration = new List<TaxRegistration>();
            this.CreditorBankAccounts = new List<BankAccount>();
        }


        public static InvoiceDescriptor Load(Stream stream)
        {
            return InvoiceDescriptorReader.Load(stream);
        } // !Load()


        public static InvoiceDescriptor Load(string filename)
        {
            return InvoiceDescriptorReader.Load(filename);
        } // !Load()


        public static InvoiceDescriptor CreateInvoice(string invoiceNo, DateTime invoiceDate, CurrencyCodes currency, string invoiceNoAsReference = "")
        {
            InvoiceDescriptor retval = new InvoiceDescriptor();
            retval.InvoiceDate = invoiceDate;
            retval.InvoiceNo = invoiceNo;
            retval.Currency = currency;
            retval.InvoiceNoAsReference = invoiceNoAsReference;
            return retval;
        } // !CreateInvoice()


        public void AddNote(string note, SubjectCodes code = SubjectCodes.Unknown)
        {
            this.Notes.Add(new Tuple<string, SubjectCodes>(note, code));
        } // !AddNote()
        

        public void SetBuyer(string name, string postcode, string city, string street, string streetno, CountryCodes country, string id, string globalIDSchemeID = "", string globalID = "")
        {
            this.Buyer = new Party()
            {
                ID = id,
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


        public void SetSeller(string name, string postcode, string city, string street, string streetno, CountryCodes country, string id, string globalIDSchemeID = "", string globalID = "")
        {
            this.Seller = new Party()
            {
                ID = id,
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


        public void SetSellerContact(string name, string orgunit = "", string emailAddress = "", string phoneno = "", string faxno = "")
        {
            this.SellerContact = new Contact()
            {
                Name = name,
                OrgUnit = orgunit,
                EmailAddress = emailAddress,
                PhoneNo = phoneno,
                FaxNo = faxno
            };
        } // !SetSellerContact()


        public void SetBuyerContact(string name, string orgunit = "", string emailAddress = "", string phoneno = "", string faxno = "")
        {
            this.BuyerContact = new Contact()
            {
                Name = name,
                OrgUnit = orgunit,
                EmailAddress = emailAddress,
                PhoneNo = phoneno,
                FaxNo = faxno
            };
        } // !SetBuyerContact()


        public void AddBuyerTaxRegistration(string no, TaxRegistrationSchemeID schemeID)
        {
            this.BuyerTaxRegistration.Add(new TaxRegistration()
            {
                No = no,
                SchemeID = schemeID
            });
        } // !AddBuyerTaxRegistration()


        public void AddSellerTaxRegistration(string no, TaxRegistrationSchemeID schemeID)
        {
            this.SellerTaxRegistration.Add(new TaxRegistration()
            {
                No = no,
                SchemeID = schemeID
            });
        } // !AddSellerTaxRegistration()


        public void SetBuyerOrderReferenceDocument(string orderNo, DateTime orderDate)
        {
            this.OrderNo = orderNo;
            this.OrderDate = orderDate;
        }



        public void SetDeliveryNoteReferenceDocument(string deliveryNoteNo, DateTime deliveryNoteDate)
        {
            this.DeliveryNoteNo = deliveryNoteNo;
            this.DeliveryNoteDate = deliveryNoteDate;
        } // !SetDeliveryNoteReferenceDocument()


        public void AddLogisticsServiceCharge(decimal amount, string description, TaxTypes taxTypeCode, TaxCategoryCodes taxCategoryCode, decimal taxPercent)
        {
            this.ServiceCharges.Add(new ServiceCharge()
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


        public void AddTradeAllowanceCharge(bool isDiscount, decimal basisAmount, CurrencyCodes currency, decimal actualAmount, string reason, TaxTypes taxTypeCode, TaxCategoryCodes taxCategoryCode, decimal taxPercent)
        {
            this.TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Reason = reason,
                BasisAmount = basisAmount,
                ActualAmount = actualAmount,
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
            this.PaymentTerms = new PaymentTerms()
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


        public void AddApplicableTradeTax(decimal basisAmount, decimal percent, TaxTypes typeCode, TaxCategoryCodes categoryCode)
        {
            this.Taxes.Add(new Tax()
            {
                BasisAmount = basisAmount,
                Percent = percent,
                TypeCode = typeCode,
                CategoryCode = categoryCode
            });
        } // !AddApplicableTradeTax()


        /// <summary>
        /// Saves the descriptor object into a stream.
        /// 
        /// The stream position will be reset to the original position after writing is finished.
        /// This allows easy further processing of the stream.
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream)
        {
            InvoiceDescriptorWriter writer = new InvoiceDescriptorWriter();
            writer.Save(this, stream);
        } // !Save()


        public void Save(string filename)
        {
            InvoiceDescriptorWriter writer = new InvoiceDescriptorWriter();
            writer.Save(this, filename);
        } // !Save()


        public void addTradeLineCommentItem(string comment)
        {
            this.TradeLineItems.Add(new TradeLineItem()
            {
                Comment = comment
            });
        } // !addTradeLineCommentItem()


        /// <summary>
        /// TODO: Rabatt fehlt:
        /// <AppliedTradeAllowanceCharge>
		///				<ChargeIndicator>false</ChargeIndicator>
		///				<ActualAmount currencyID="EUR">0.6667</ActualAmount>
		///				<Reason>Rabatt</Reason>
        ///			</AppliedTradeAllowanceCharge>
        /// </summary>
        public void addTradeLineItem(string name, string description,
                                     QuantityCodes unitCode, decimal unitQuantity,
                                     decimal grossUnitPrice,
                                     decimal netUnitPrice,
                                     decimal billedQuantity,
                                     TaxTypes taxType, TaxCategoryCodes categoryCode, decimal taxPercent,
                                     string comment = "",
                                     string globalIDSchemeID = "", string globalID = "",
                                     string sellerAssignedID = "", string buyerAssignedID = "")
        {
            TradeLineItem newItem = new TradeLineItem()
            {
                GlobalID = new GlobalID(globalIDSchemeID, globalID),
                SellerAssignedID = sellerAssignedID,
                BuyerAssignedID = buyerAssignedID,
                Name = name,
                Description = description,
                UnitCode = unitCode,
                UnitQuantity = unitQuantity,
                GrossUnitPrice = grossUnitPrice,
                NetUnitPrice = netUnitPrice,
                BilledQuantity = billedQuantity,
                TaxType = taxType,
                TaxCategoryCode = categoryCode,
                TaxPercent = taxPercent,
                Comment = comment
            };

            this.TradeLineItems.Add(newItem);
        } // !addTradeLineItem()


        public void setPaymentMeans(string paymentCode, string information = "")
        {
            this.PaymentMeans = new PaymentMeans
            {
                TypeCode = paymentCode,
                Information = information
            };
        } // !setPaymentMeans()


        public void addCreditorFinancialAccount(string iban, string bic, string id, string bankleitzahl, string bankName)
        {
            this.CreditorBankAccounts.Add(new BankAccount()
            {
                ID = id,
                IBAN = iban,
                BIC = bic,
                Bankleitzahl = bankleitzahl,
                BankName = bankName
            });
        } // !addCreditorFinancialAccount()
    }
}