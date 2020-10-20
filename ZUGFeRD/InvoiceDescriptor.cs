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
    /// <summary>
    /// Represents a ZUGFeRD/ Factur-X invoice
    /// </summary>
    public class InvoiceDescriptor
    {
        /// <summary>
        /// Invoice Number
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// Invoice date
        /// </summary>
        public DateTime? InvoiceDate { get; set; } = null;
        public string InvoiceNoAsReference { get; set; } = "";

        public string OrderNo { get; set; } = "";
        public DateTime? OrderDate { get; set; } = null;

        public List<AdditionalReferencedDocument> AdditionalReferencedDocuments { get; internal set; } = new List<AdditionalReferencedDocument>();
        public DeliveryNoteReferencedDocument DeliveryNoteReferencedDocument { get; set; } = null;
        public DateTime? ActualDeliveryDate { get; set; } = null;

        public ContractReferencedDocument ContractReferencedDocument { get; set; }

        public SpecifiedProcuringProject SpecifiedProcuringProject { get; set; }

        /// <summary>
        /// Currency of the invoice
        /// </summary>
        public CurrencyCodes Currency { get; set; }
        public Party Buyer { get; set; }
        public Contact BuyerContact { get; set; }
        public List<TaxRegistration> BuyerTaxRegistration { get; set; } = new List<TaxRegistration>();
        public Party Seller { get; set; }
        public Contact SellerContact { get; set; }
        public List<TaxRegistration> SellerTaxRegistration { get; set; } = new List<TaxRegistration>();

        /// <summary>
        /// This party is optional and only relevant for Extended profile
        /// </summary>
        public Party Invoicee { get; set; }

        /// <summary>
        /// This party is optional and only relevant for Extended profile
        /// </summary>
        public Party ShipTo { get; set; }

        /// <summary>
        /// This party is optional and only relevant for Extended profile
        /// </summary>
        public Party Payee { get; set; }

        /// <summary>
        /// This party is optional and only relevant for Extended profile
        /// </summary>
        public Party ShipFrom { get; set; }

        public List<Note> Notes { get; set; } = new List<Note>();
        public bool IsTest { get; set; } = false;
        public Profile Profile { get; internal set; } = Profile.Basic;
        public InvoiceType Type { get; set; } = InvoiceType.Invoice;
        public string ReferenceOrderNo { get; set; }
        public List<TradeLineItem> TradeLineItems { get; set; } = new List<TradeLineItem>();


        public decimal LineTotalAmount { get; set; } = Decimal.MinValue;
        public decimal? ChargeTotalAmount { get; set; } = Decimal.MinValue;
        public decimal? AllowanceTotalAmount { get; set; } = Decimal.MinValue;
        public decimal? TaxBasisAmount { get; set; } = Decimal.MinValue;
        public decimal TaxTotalAmount { get; set; } = Decimal.MinValue;
        public decimal GrandTotalAmount { get; set; } = Decimal.MinValue;
        public decimal? TotalPrepaidAmount { get; set; } = Decimal.MinValue;
        public decimal DuePayableAmount { get; set; } = Decimal.MinValue;
        public List<Tax> Taxes { get; set; } = new List<Tax>();
        public List<ServiceCharge> ServiceCharges { get; set; } = new List<ServiceCharge>();
        public List<TradeAllowanceCharge> TradeAllowanceCharges { get; set; } = new List<TradeAllowanceCharge>();
        public PaymentTerms PaymentTerms { get; set; }
        public List<BankAccount> CreditorBankAccounts { get; set; } = new List<BankAccount>();
        public List<BankAccount> DebitorBankAccounts { get; set; } = new List<BankAccount>();
        public PaymentMeans PaymentMeans { get; set; }

        public DateTime? BillingPeriodStart { get; set; }
        public DateTime? BillingPeriodEnd { get; set; }



        /// <summary>
        /// Loads a ZUGFeRD invoice from a stream.
        /// 
        /// Please make sure that the stream is open, otherwise this call will raise an IllegalStreamException.
        /// 
        /// Important: the stream will not be closed by this function, make sure to close it by yourself!
        /// 
        /// </summary>
        /// <param name="stream">Stream where to read the ZUGFeRD invoice</param>
        /// <returns></returns>
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

            throw new UnsupportedException("No ZUGFeRD invoice reader was able to parse this stream!");

            // return null;
        } // !Load()


        /// <summary>
        /// Loads a ZUGFeRD invoice from a file.
        /// 
        /// Please make sure that the file is exists, otherwise this call will raise a FileNotFoundException.
        /// </summary>
        /// <param name="filename">Name of the ZUGFeRD invoice file</param>
        /// <returns></returns>
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

            throw new UnsupportedException("No ZUGFeRD invoice reader was able to parse this file '" + filename + "'!");

            // return null;
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
            /*
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


        public void AddAdditionalReferencedDocument(string issuerAssignedID, DateTime? issueDateTime = null, AdditionalReferencedDocumentTypeCode typeCode = AdditionalReferencedDocumentTypeCode.Unknown, string name = null, ReferenceTypeCodes referenceTypeCode = ReferenceTypeCodes.Unknown, byte[] attachmentBinaryObject = null, string filename = null)
        {
            this.AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ReferenceTypeCode = referenceTypeCode,
                IssuerAssignedID = issuerAssignedID,
                IssueDateTime = issueDateTime,
                Name = name,
                AttachmentBinaryObject = attachmentBinaryObject,
                Filename = filename,
                TypeCode = typeCode
            });
        } // !AddAdditionalReferencedDocument()


        public void SetBuyerOrderReferenceDocument(string orderNo, DateTime orderDate)
        {
            this.OrderNo = orderNo;
            this.OrderDate = orderDate;
        } // !SetBuyerOrderReferenceDocument()


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


        /// <summary>
        /// Adds an allowance or charge on document level.
        /// 
        /// Allowance represents a discount whereas charge represents a surcharge.
        /// </summary>
        /// <param name="isDiscount">Marks if the allowance charge is a discount. Please note that in contrary to this function, the xml file indicated a surcharge, not a discount (value will be inverted)</param>
        /// <param name="basisAmount">Base amount (basis of allowance)</param>
        /// <param name="currency">Curency of the allowance</param>
        /// <param name="actualAmount">Actual allowance charge amount</param>
        /// <param name="reason">Reason for the allowance</param>
        /// <param name="taxTypeCode">VAT type code for document level allowance/ charge</param>
        /// <param name="taxCategoryCode">VAT type code for document level allowance/ charge</param>
        /// <param name="taxPercent">VAT rate for the allowance</param>
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


        public void SetTradePaymentTerms(string description, DateTime? dueDate = null)
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

        private IInvoiceDescriptorWriter _selectInvoiceDescriptorWriter(ZUGFeRDVersion version)
        {
            switch (version)
            {
                case ZUGFeRDVersion.Version1:
                    return new InvoiceDescriptor1Writer();
                case ZUGFeRDVersion.Version20:
                    return new InvoiceDescriptor20Writer();
                case ZUGFeRDVersion.Version21:
                    return new InvoiceDescriptor21Writer();
                default:
                    throw new UnsupportedException("New ZUGFeRDVersion '" + version + "' defined but not implemented!");
            }
        } // !_selectInvoiceDescriptorWriter()


        /// <summary>
        /// Saves the descriptor object into a stream.
        /// 
        /// The stream position will be reset to the original position after writing is finished.
        /// This allows easy further processing of the stream.
        /// </summary>
        /// <param name="stream">The stream where the data should be saved to.</param>
        /// <param name="version">The ZUGFeRD version you want to use. Defaults to version 1.</param>
        /// <param name="profile">The ZUGFeRD profile you want to use. Defaults to Basic.</param>
        public void Save(Stream stream, ZUGFeRDVersion version = ZUGFeRDVersion.Version1, Profile profile = Profile.Basic)
        {
            this.Profile = profile;            
            IInvoiceDescriptorWriter writer = _selectInvoiceDescriptorWriter(version);
            writer.Save(this, stream);
        } // !Save()


        /// <summary>
        /// Saves the descriptor object into a file with given name.        
        /// </summary>
        /// <param name="filename">The filename where the data should be saved to.</param>
        /// <param name="version">The ZUGFeRD version you want to use. Defaults to version 1.</param>
        /// <param name="profile">The ZUGFeRD profile you want to use. Defaults to Basic.</param>
        public void Save(string filename, ZUGFeRDVersion version = ZUGFeRDVersion.Version1, Profile profile = Profile.Basic)
        {
            this.Profile = profile;
            IInvoiceDescriptorWriter writer = _selectInvoiceDescriptorWriter(version);
            writer.Save(this, filename);
        } // !Save()

#pragma warning disable IDE1006
        [Obsolete("This function is deprecated. Please use AddTradeLineCommentItem() instead")]
        public void addTradeLineCommentItem(string comment)
        {
            AddTradeLineCommentItem(comment);
        } // !addTradeLineCommentItem()
#pragma warning restore IDE1006

        public void AddTradeLineCommentItem(string comment)
        {
            TradeLineItem item = new TradeLineItem()
            {
                AssociatedDocument = new ZUGFeRD.AssociatedDocument(),
                GrossUnitPrice = 0m,
                NetUnitPrice= 0m,
                BilledQuantity = 0m,
                TaxCategoryCode = TaxCategoryCodes.O
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

            item.AssociatedDocument = new ZUGFeRD.AssociatedDocument(_lineID);

            item.AssociatedDocument.Notes.Add(new Note(
                content: comment,
                subjectCode: SubjectCodes.Unknown,
                contentCode: ContentCodes.Unknown
            ));

            this.TradeLineItems.Add(item);
        } // !AddTradeLineCommentItem()


#pragma warning disable IDE1006
        [Obsolete("This function is deprecated. Please use AddTradeLineItem() instead")]
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
            return AddTradeLineItem(name, description, unitCode, unitQuantity, grossUnitPrice, netUnitPrice, billedQuantity, taxType, categoryCode, taxPercent, comment, id, sellerAssignedID, buyerAssignedID, deliveryNoteID, deliveryNoteDate, buyerOrderID, buyerOrderDate);
        } // !addTradeLineItem()
#pragma warning restore IDE1006

        // TODO Rabatt ergänzen:
        // <ram:AppliedTradeAllowanceCharge>
        //     <ram:ChargeIndicator><udt:Indicator>false</udt:Indicator></ram:ChargeIndicator>
        //     <ram:CalculationPercent>2.00</ram:CalculationPercent>
        //     <ram:BasisAmount currencyID = "EUR" > 1.5000 </ram:BasisAmount>
        //     <ram:ActualAmount currencyID = "EUR" > 0.0300 </ram:ActualAmount>
        //     <ram:Reason>Artikelrabatt 1</ram:Reason>
        // </ram:AppliedTradeAllowanceCharge>
        public TradeLineItem AddTradeLineItem(string name,
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
                newItem.SetDeliveryNoteReferencedDocument(deliveryNoteID, deliveryNoteDate);
            }

            if (!String.IsNullOrEmpty(buyerOrderID) || buyerOrderDate.HasValue)
            {
                newItem.SetOrderReferencedDocument(buyerOrderID, buyerOrderDate);
            }

            this.TradeLineItems.Add(newItem);
            return newItem;
        } // !AddTradeLineItem()


#pragma warning disable IDE1006
        [Obsolete("This function is deprecated. Please use SetPaymentMeans() instead.")]
        public void setPaymentMeans(PaymentMeansTypeCodes paymentCode, string information = "", string identifikationsnummer = null, string mandatsnummer = null)
        {
            SetPaymentMeans(paymentCode, information, identifikationsnummer, mandatsnummer);
        } // !setPaymentMeans()
#pragma warning restore IDE1006


        public void SetPaymentMeans(PaymentMeansTypeCodes paymentCode, string information = "", string identifikationsnummer = null, string mandatsnummer = null)
        { 
            this.PaymentMeans = new PaymentMeans
            {
                TypeCode = paymentCode,
                Information = information,
                SEPACreditorIdentifier = identifikationsnummer,
                SEPAMandateReference = mandatsnummer
            };
        } // !SetPaymentMeans()


#pragma warning disable IDE1006
        [Obsolete("This function is deprecated. Please use AddCreditorFinancialAccount() instead.")]
        public void addCreditorFinancialAccount(string iban, string bic, string id = null, string bankleitzahl = null, string bankName = null, string name = null)
        {
            AddCreditorFinancialAccount(iban, bic, id, bankleitzahl, bankName, name);
        } // !addCreditorFinancialAccount()
#pragma warning restore IDE1006


        public void AddCreditorFinancialAccount(string iban, string bic, string id = null, string bankleitzahl = null, string bankName = null, string name = null)
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
        } // !AddCreditorFinancialAccount()


#pragma warning disable IDE1006
        [Obsolete("This function is deprecated. Please use AddDebitorFinancialAccount() instead.")]
        public void addDebitorFinancialAccount(string iban, string bic, string id = null, string bankleitzahl = null, string bankName = null)
        {
            AddDebitorFinancialAccount(iban, bic, id, bankleitzahl, bankName);
        } // !addDebitorFinancialAccount()
#pragma warning restore IDE1006


        public void AddDebitorFinancialAccount(string iban, string bic, string id = null, string bankleitzahl = null, string bankName = null)
        {
            this.DebitorBankAccounts.Add(new BankAccount()
            {
                ID = id,
                IBAN = iban,
                BIC = bic,
                Bankleitzahl = bankleitzahl,
                BankName = bankName
            });
        } // !AddDebitorFinancialAccount()
   }
}
