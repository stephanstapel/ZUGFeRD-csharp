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

        public AdditionalReferencedDocument AdditionalReferencedDocument { get; set; }
        public DeliveryNoteReferencedDocument DeliveryNoteReferencedDocument { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        public CurrencyCodes Currency { get; set; }
        public Party Buyer { get; set; }
        public Contact BuyerContact { get; set; }
        public List<TaxRegistration> BuyerTaxRegistration { get; set; }
        public Party Seller { get; set; }
        public Contact SellerContact { get; set; }
        public List<TaxRegistration> SellerTaxRegistration { get; set; }

        /**
         * This party is optional and only relevant for Extended profile
         */
        public Party Invoicee { get; set; }

        /**
         * This party is optional and only relevant for Extended profile
         */
        public Party ShipTo { get; set; }

        /**
         * This party is optional and only relevant for Extended profile
         */
        public Party Payee { get; set; }

        /**
         * This party is optional and only relevant for Extended profile
         */
        public Party ShipFrom { get; set; }

        public List<Note> Notes { get; set; }
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
        public List<BankAccount> DebitorBankAccounts { get; set; }
        public PaymentMeans PaymentMeans { get; set; }


        internal InvoiceDescriptor()
        {
            this.InvoiceNoAsReference = "";

            this.IsTest = false;
            this.Profile = Profile.Basic;
            this.Type = InvoiceType.Invoice;
            this.Notes = new List<Note>();
            this.OrderNo = "";
            this.OrderDate = null;
            this.InvoiceDate = null;
            this.AdditionalReferencedDocument = null;
            this.DeliveryNoteReferencedDocument = null;
            this.ActualDeliveryDate = null;

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
            this.DebitorBankAccounts = new List<BankAccount>();
        }


        public static InvoiceDescriptor Load(Stream stream)
        {
            IInvoiceDescriptorReader reader = new InvoiceDescriptor1Reader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return reader.Load(stream);
            }

            reader = new InvoiceDescriptor20Reader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return reader.Load(stream);
            }

            reader = new InvoiceDescriptor21Reader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return reader.Load(stream);
            }

            return null;
        } // !Load()


        public static InvoiceDescriptor Load(string filename)
        {
            IInvoiceDescriptorReader reader = new InvoiceDescriptor1Reader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return reader.Load(filename);
            }

            reader = new InvoiceDescriptor20Reader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return reader.Load(filename);
            }

            reader = new InvoiceDescriptor21Reader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return reader.Load(filename);
            }

            return null;
        } // !Load()


        public static InvoiceDescriptor CreateInvoice(string invoiceNo, DateTime invoiceDate, CurrencyCodes currency, string invoiceNoAsReference = "")
        {
            InvoiceDescriptor retval = new InvoiceDescriptor
            {
                InvoiceDate = invoiceDate,
                InvoiceNo = invoiceNo,
                Currency = currency,
                InvoiceNoAsReference = invoiceNoAsReference
            };
            return retval;
        } // !CreateInvoice()


        public void AddNote(string note, SubjectCodes subjectCode = SubjectCodes.Unknown, ContentCodes contentCode = ContentCodes.Unknown)
        {
            /**
             * @todo prüfen:
             * ST1, ST2, ST3 nur mit AAK
             * EEV, WEB, VEV nur mit AAJ
             */

            this.Notes.Add(new Note(note, subjectCode, contentCode));
        } // !AddNote()        


        public void SetBuyer(string name, string postcode, string city, string street, CountryCodes country, string id, GlobalID globalID = null, string receiver = "")
        {
            this.Buyer = new Party()
            {
                ID = id,
                Name = name,
                Postcode = postcode,
                ContactName = receiver,
                City = city,
                Street = street,
                Country = country,
                GlobalID = globalID
            };
        }


        public void SetSeller(string name, string postcode, string city, string street, CountryCodes country, string id, GlobalID globalID = null)
        {
            this.Seller = new Party()
            {
                ID = id,
                Name = name,
                Postcode = postcode,
                City = city,
                Street = street,
                Country = country,
                GlobalID = globalID
            };
        } // !SetSeller()


        public void SetSellerContact(string name = "", string orgunit = "", string emailAddress = "", string phoneno = "", string faxno = "")
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
            this.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            {
                ID = deliveryNoteNo,
                IssueDateTime = deliveryNoteDate
            };
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


        public void AddApplicableTradeTax(decimal basisAmount, decimal percent, TaxTypes typeCode, TaxCategoryCodes? categoryCode = null, decimal allowanceChargeBasisAmount = 0)
        {
            Tax tax = new Tax()
            {
                BasisAmount = basisAmount,
                Percent = percent,
                TypeCode = typeCode,
                AllowanceChargeBasisAmount = allowanceChargeBasisAmount
            };

            if ((categoryCode != null) && (categoryCode.Value != TaxCategoryCodes.Unknown))
            {
                tax.CategoryCode = categoryCode;
            }

            this.Taxes.Add(tax);
        } // !AddApplicableTradeTax()


        /// <summary>
        /// Saves the descriptor object into a stream.
        /// 
        /// The stream position will be reset to the original position after writing is finished.
        /// This allows easy further processing of the stream.
        /// </summary>
        /// <param name="stream"></param>
        public void Save(Stream stream, ZUGFeRDVersion version = ZUGFeRDVersion.Version1)
        {
            IInvoiceDescriptorWriter writer = null;
            switch (version)
            {
                case ZUGFeRDVersion.Version1:
                    writer = new InvoiceDescriptor1Writer();
                    break;
                case ZUGFeRDVersion.Version20:
                    writer = new InvoiceDescriptor20Writer();
                    break;
                case ZUGFeRDVersion.Version21:
                    writer = new InvoiceDescriptor21Writer(); 
                    break;
                default:
                    break;
            }
            writer.Save(this, stream);
        } // !Save()


        public void Save(string filename, ZUGFeRDVersion version = ZUGFeRDVersion.Version1)
        {
            IInvoiceDescriptorWriter writer = null;
            switch (version)
            {
                case ZUGFeRDVersion.Version1:
                    writer = new InvoiceDescriptor1Writer();
                    break;
                case ZUGFeRDVersion.Version20:
                    writer = new InvoiceDescriptor20Writer();
                    break;
                case ZUGFeRDVersion.Version21:
                    writer = new InvoiceDescriptor21Writer();
                    break;
                default:
                    break;
            }
            writer.Save(this, filename);
        } // !Save()


        public void addTradeLineCommentItem(string comment)
        {
            TradeLineItem item = new TradeLineItem()
            {
                AssociatedDocument = new ZUGFeRD.AssociatedDocument(),
                GrossUnitPrice = 0m,
                NetUnitPrice= 0m,
                BilledQuantity = 0m
            };

            item.AssociatedDocument.Notes.Add(new Note(
                content: comment,
                subjectCode: SubjectCodes.Unknown,
                contentCode: ContentCodes.Unknown
            ));

            this.TradeLineItems.Add(item);
        } // !addTradeLineCommentItem()


        /// <summary>
        /// @todo Rabatt ergänzen:
        /// <ram:AppliedTradeAllowanceCharge>
        /// 				<ram:ChargeIndicator><udt:Indicator>false</udt:Indicator></ram:ChargeIndicator>
        /// 				<ram:CalculationPercent>2.00</ram:CalculationPercent>
        /// 				<ram:BasisAmount currencyID = "EUR" > 1.5000 </ ram:BasisAmount>
        /// 				<ram:ActualAmount currencyID = "EUR" > 0.0300 </ ram:ActualAmount>
        /// 				<ram:Reason>Artikelrabatt 1</ram:Reason>
        /// 			</ram:AppliedTradeAllowanceCharge>
        /// </summary>
        public TradeLineItem addTradeLineItem(string name,
                                     string description = null,
                                     QuantityCodes unitCode = QuantityCodes.Unknown,
                                     decimal? unitQuantity = null,
                                     decimal grossUnitPrice = Decimal.MinValue,
                                     decimal netUnitPrice = Decimal.MinValue,
                                     decimal billedQuantity = Decimal.MinValue,
                                     TaxTypes taxType = TaxTypes.Unknown,
                                     TaxCategoryCodes categoryCode = TaxCategoryCodes.Unknown,
                                     decimal taxPercent = Decimal.MinValue,
                                     string comment = null,
                                     GlobalID id = null,
                                     string sellerAssignedID = "", string buyerAssignedID = "",
                                     string deliveryNoteID = "", DateTime? deliveryNoteDate = null,
                                     string buyerOrderID = "", DateTime? buyerOrderDate = null)
        {
            TradeLineItem newItem = new TradeLineItem()
            {
                GlobalID = id,
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
                LineTotalAmount = netUnitPrice * billedQuantity
            };

            int? _lineID = null;
            if (this.TradeLineItems.Count > 0)
            {
                _lineID = this.TradeLineItems.Last().AssociatedDocument.LineID;
            }

            if (_lineID.HasValue)
            {
                _lineID = _lineID.Value + 1;
            }
            else
            {
                _lineID = 1;
            }

            newItem.AssociatedDocument = new ZUGFeRD.AssociatedDocument(_lineID);
            if (!String.IsNullOrEmpty(comment))
            {
                newItem.AssociatedDocument.Notes.Add(new Note(comment, SubjectCodes.Unknown, ContentCodes.Unknown));
            }

            if (!String.IsNullOrEmpty(deliveryNoteID) || deliveryNoteDate.HasValue)
            {
                newItem.setDeliveryNoteReferencedDocument(deliveryNoteID, deliveryNoteDate);
            }

            if (!String.IsNullOrEmpty(buyerOrderID) || buyerOrderDate.HasValue)
            {
                newItem.setOrderReferencedDocument(buyerOrderID, buyerOrderDate);
            }

            this.TradeLineItems.Add(newItem);
            return newItem;
        } // !addTradeLineItem()


        public void setPaymentMeans(PaymentMeansTypeCodes paymentCode, string information = "", string identifikationsnummer = null, string mandatsnummer = null)
        {
            this.PaymentMeans = new PaymentMeans
            {
                TypeCode = paymentCode,
                Information = information,
                SEPACreditorIdentifier = identifikationsnummer,
                SEPAMandateReference = mandatsnummer
            };
        } // !setPaymentMeans()


        public void addCreditorFinancialAccount(string iban, string bic, string id = null, string bankleitzahl = null, string bankName = null, string name = null)
        {
            this.CreditorBankAccounts.Add(new BankAccount()
            {
                ID = id,
                IBAN = iban,
                BIC = bic,
                Bankleitzahl = bankleitzahl,
                BankName = bankName,
                Name = name
            });
        } // !addCreditorFinancialAccount()

        public void addDebitorFinancialAccount(string iban, string bic, string id = null, string bankleitzahl = null, string bankName = null)
        {
            this.DebitorBankAccounts.Add(new BankAccount()
            {
                ID = id,
                IBAN = iban,
                BIC = bic,
                Bankleitzahl = bankleitzahl,
                BankName = bankName
            });
        } // !addDebitorFinancialAccount()
   }
}