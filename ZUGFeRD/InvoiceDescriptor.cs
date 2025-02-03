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
    /// <summary>
    /// Represents a ZUGFeRD/ Factur-X invoice
    /// </summary>
    public class InvoiceDescriptor
    {
        /// <summary>
        /// Invoice Number
        ///
        /// BT-1
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// Invoice date
        ///
        /// BT-2
        /// </summary>
        public DateTime? InvoiceDate { get; set; } = null;

        /// <summary>
        /// A textual value used to establish a link between the payment and the invoice, issued by the seller.
        /// </summary>
        public string PaymentReference { get; set; } = String.Empty;

        /// <summary>
        /// Order Id
        /// </summary>
        public string OrderNo { get; set; } = string.Empty;

        /// <summary>
        /// Order date
        /// </summary>
        public DateTime? OrderDate { get; set; } = null;

        /// <summary>
        /// Details of an additional document reference
        ///
        /// A new reference document is added by AddAdditionalReferenceDocument()
        /// </summary>
        public List<AdditionalReferencedDocument> AdditionalReferencedDocuments { get; internal set; } = new List<AdditionalReferencedDocument>();

        /// <summary>
        /// Detailed information about the corresponding despatch advice
        /// </summary>
        public DespatchAdviceReferencedDocument DespatchAdviceReferencedDocument { get; internal set; } = null;

        /// <summary>
        /// Detailed information about the corresponding delivery note
        /// </summary>
        public DeliveryNoteReferencedDocument DeliveryNoteReferencedDocument { get; set; } = null;

        /// <summary>
        /// Actual delivery date
        /// </summary>
        public DateTime? ActualDeliveryDate { get; set; } = null;

        /// <summary>
        /// Detailed information on the associated contract
        ///
        /// BT-12
        /// </summary>
        public ContractReferencedDocument ContractReferencedDocument { get; set; }

        /// <summary>
        /// Details about a project reference
        /// </summary>
        public SpecifiedProcuringProject SpecifiedProcuringProject { get; set; }

        /// <summary>
        /// Currency of the invoice
        /// </summary>
        public CurrencyCodes Currency { get; set; }

        /// <summary>
        /// The VAT total amount expressed in the accounting currency accepted or
        /// required in the country of the seller.
        ///
        /// Note: Shall be used in combination with the invoice total VAT amount
        /// in accounting currency (BT-111), if the VAT accounting currency code
        /// differs from the invoice currency code.
        ///
        /// In normal invoicing scenarios, leave this property empty!
        ///
        /// The lists of valid currencies are
        /// registered with the ISO 4217 Maintenance Agency „Codes for the
        /// representation of currencies and funds”. Please refer to Article 230
        /// of the Council Directive 2006/112/EC [2] for further information.
        ///
        /// BT-6
        /// </summary>
        public CurrencyCodes? TaxCurrency { get; set; }


        /// <summary>
        /// Information about the buyer
        /// BG-7
        /// </summary>
        public Party Buyer { get; set; }

        /// <summary>
        /// Buyer contact information
        ///
        /// A group of business terms providing contact information relevant for the buyer.
        /// </summary>
        public Contact BuyerContact { get; set; }
        public List<TaxRegistration> BuyerTaxRegistration { get; set; } = new List<TaxRegistration>();

        /// <summary>
        /// Buyer electronic address
        /// BT-49
        /// </summary>
        public ElectronicAddress BuyerElectronicAddress { get; set; }

        /// <summary>
        /// BG-4
        /// </summary>
        public Party Seller { get; set; }
        public Contact SellerContact { get; set; }
        public List<TaxRegistration> SellerTaxRegistration { get; set; } = new List<TaxRegistration>();

        /// <summary>
        /// Seller electronic address
        /// BT-34
        /// </summary>
        public ElectronicAddress SellerElectronicAddress { get; set; }

        /// <summary>
        /// Given seller reference number for routing purposes after biliteral agreement
        ///
        /// This field seems not to be used in common scenarios.
        /// </summary>
        public string SellerReferenceNo { get; set; } = String.Empty;

        /// <summary>
        /// This party is optional and only relevant for Extended profile
        /// </summary>
        public Party Invoicee { get; set; }

        /// <summary>
        /// This party is optional and only relevant for Extended profile.
        ///
        /// It seems to be used under rate condition only.
        /// </summary>
        public Party Invoicer { get; set; }

        /// <summary>
        /// This party is optional and is written in most profiles except Minimum profile
        ///
        /// BG-13
        /// </summary>
        public Party ShipTo { get; set; }

        /// <summary>
        /// Detailed contact information of the recipient
        /// BG-X-26
        /// </summary>
        public Contact ShipToContact { get; set; }

        /// <summary>
        /// This party is optional and only relevant for Extended profile
        /// </summary>
        public Party UltimateShipTo { get; set; }

        /// <summary>
        /// Detailed contact information of the final goods recipient
        /// BG-X-11
        /// </summary>
        public Contact UltimateShipToContact { get; set; }

        /// <summary>
        /// This party is optional and only relevant for Extended profile
        /// </summary>
        public Party Payee { get; set; }

        /// <summary>
        /// This party is optional and only relevant for Extended profile
        /// </summary>
        public Party ShipFrom { get; set; }

        /// <summary>
        /// Free text on header level
        /// </summary>
        public List<Note> Notes { get; set; } = new List<Note>();

        /// <summary>
        /// Description: Identifies the context of a business process where the transaction is taking place,
        /// thus allowing the buyer to process the invoice in an appropriate manner.
        ///
        /// Note: These data make it possible to define the purpose of the settlement(invoice of the authorised person,
        /// contractual partner, subcontractor, settlement document for a building contract etc.).
        ///
        /// BT-23
        /// </summary>
        public string BusinessProcess { get; set; }

        /// <summary>
        /// The Indicator type may be used when implementing a new system in order to mark the invoice as „trial invoice“.
        /// </summary>
        public bool IsTest { get; set; } = false;

        /// <summary>
        /// Representation of information that should be used for the document.
        ///
        /// As the library can be used to both write ZUGFeRD files and read ZUGFeRD files, the profile serves two purposes:
        /// It indicates the profile that was used to write the ZUGFeRD file that was loaded or the profile that is to be used when
        /// the document is saved.
        /// </summary>
        public Profile Profile { get; internal set; } = Profile.Basic;

        /// <summary>
        /// Document name (free text)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates the type of the document, if it represents an invoice, a credit note or one of the available 'sub types'
        ///
        /// BT-3
        /// </summary>
        public InvoiceType Type { get; set; } = InvoiceType.Invoice;

        /// <summary>
        /// The identifier is defined by the buyer (e.g. contact ID, department, office ID, project code), but provided by the seller in the invoice.
        /// In France it needs to be filled with 999, if not available.
        ///
        /// BT-10
        /// </summary>
        public string ReferenceOrderNo { get; set; }

        /// <summary>
        /// An aggregation of business terms containing information about individual invoice positions
        /// </summary>
        [Obsolete("This property will not be available any more with version 18.0. Please use GetTradeLineItems() instead")]
        public List<TradeLineItem> TradeLineItems { get; internal set; } = new List<TradeLineItem>();

        /// <summary>
        /// Sum of all invoice line net amounts in the invoice
        /// </summary>
        public decimal? LineTotalAmount { get; set; } = null;

        /// <summary>
        /// Sum of all surcharges on document level in the invoice
        ///
        /// Surcharges on line level are included in the invoice line net amount which is summed up into the sum of invoice line net amount.
        /// </summary>
        public decimal? ChargeTotalAmount { get; set; } = null;

        /// <summary>
        /// Sum of discounts on document level in the invoice
        ///
        /// Discounts on line level are included in the invoice line net amount which is summed up into the sum of invoice line net amount.
        /// </summary>
        public decimal? AllowanceTotalAmount { get; set; } = null;

        /// <summary>
        /// The total amount of the invoice without VAT.
        ///
        /// The invoice total amount without VAT is the sum of invoice line net amount minus sum of discounts on document level plus sum of surcharges on document level.
        /// </summary>
        public decimal? TaxBasisAmount { get; set; } = null;

        /// <summary>
        /// The total VAT amount for the invoice.
        /// The VAT total amount expressed in the accounting currency accepted or required in the country of the seller
        ///
        /// To be used when the VAT accounting currency (BT-6) differs from the Invoice currency code (BT-5) in accordance
        /// with article 230 of Directive 2006/112 / EC on VAT. The VAT amount in accounting currency is not used
        /// in the calculation of the Invoice totals..
        /// </summary>
        public decimal? TaxTotalAmount { get; set; } = null;

        /// <summary>
        /// Invoice total amount with VAT
        ///
        /// The invoice total amount with VAT is the invoice without VAT plus the invoice total VAT amount.
        /// </summary>
        public decimal? GrandTotalAmount { get; set; } = null;

        /// <summary>
        /// Sum of amount paid in advance
        ///
        /// This amount is subtracted from the invoice total amount with VAT to calculate the amount due for payment.
        /// </summary>
        public decimal? TotalPrepaidAmount { get; set; } = null;

        /// <summary>
        /// The amount to be added to the invoice total to round the amount to be paid.
        /// </summary>
        public decimal? RoundingAmount { get; set; } = null;

        /// <summary>
        /// The outstanding amount that is requested to be paid.
        ///
        /// This amount is the invoice total amount with VAT minus the paid amount that has
        /// been paid in advance. The amount is zero in case of a fully paid invoice.
        /// The amount may be negative; in that case the seller owes the amount to the buyer.
        /// </summary>
        public decimal? DuePayableAmount { get; set; } = null;

        /// <summary>
        /// A group of business terms providing information about VAT breakdown by different categories, rates and exemption reasons
        /// </summary>
        [Obsolete("This property will be removed in version 18.0. Please use GetApplicableTradeTaxes() instead")]
        public  List<Tax> Taxes { get; internal set; } = new List<Tax>();

        /// <summary>
        /// Transport and packaging costs
        /// </summary>
        [Obsolete("This property will be removed in version 18.0. Please use GetLogisticsServiceCharges() instead")]
        public List<ServiceCharge> ServiceCharges { get; internal set; } = new List<ServiceCharge>();

        /// <summary>
        /// Detailed information on discounts and charges.
        /// This field is marked as private now, please use GetTradeAllowanceCharges() to retrieve all trade allowance charges
        /// </summary>
        private List<TradeAllowanceCharge> _TradeAllowanceCharges { get; set; } = new List<TradeAllowanceCharge>();

        /// <summary>
        /// Detailed information about payment terms
        /// </summary>
        /// <remarks>BT-20</remarks>
        private List<PaymentTerms> _PaymentTerms { get; set; } = new List<PaymentTerms>();

        /// <summary>
        /// A group of business terms providing information about a preceding invoices.
        ///
        /// To be used in case:
        /// — a preceding invoice is corrected;
        /// — preceding partial invoices are referred to from a final invoice;
        /// — preceding pre-payment invoices are referred to from a final invoice.
        /// </summary>
        private List<InvoiceReferencedDocument> _InvoiceReferencedDocuments { get; set; } = new List<InvoiceReferencedDocument>();

        /// <summary>
        /// Detailed information about the accounting reference
        /// </summary>
        [Obsolete("This property will be removed in version 18.0. Please use GetReceivableSpecifiedTradeAccountingAccounts() instead")]
        public List<ReceivableSpecifiedTradeAccountingAccount> _ReceivableSpecifiedTradeAccountingAccounts { get; internal set; } = new List<ReceivableSpecifiedTradeAccountingAccount>();

        /// <summary>
        /// Credit Transfer
        ///
        /// A group of business terms to specify credit transfer payments
        /// </summary>
        [Obsolete("This property will be removed in version 18.0. Please use GetCreditorFinancialAccounts() instead")]
        public List<BankAccount> CreditorBankAccounts { get; internal set; } = new List<BankAccount>();

        /// <summary>
        /// Buyer bank information
        /// </summary>
        [Obsolete("This property will be removed in version 18.0. Please use GetDebitorFinancialAccounts() instead")]
        public List<BankAccount> DebitorBankAccounts { get; internal set; } = new List<BankAccount>();

        /// <summary>
        /// Payment instructions
        ///
        /// /// If various accounts for credit transfers shall be transferred, the element
        /// SpecifiedTradeSettlementPaymentMeans can be repeated for each account. The code
        /// for the type of payment within the element typecode (BT-81) should therefore not
        /// differ within the repetitions.
        /// </summary>
        public PaymentMeans PaymentMeans { get; set; }

        /// <summary>
        /// Detailed information about the invoicing period, start date
        /// </summary>
        public DateTime? BillingPeriodStart { get; set; }

        /// <summary>
        /// Detailed information about the invoicing period, end date
        /// </summary>
        public DateTime? BillingPeriodEnd { get; set; }

        /// <summary>
        /// Details about the associated order confirmation (BT-14).
        /// This is optional and can be used in Profiles Comfort and Extended.
        /// If you add a SellerOrderReferencedDocument you must set the property "ID".
        /// The property "IssueDateTime" is optional an only used in profile "Extended"
        /// </summary>
        public SellerOrderReferencedDocument SellerOrderReferencedDocument { get; set; } = new SellerOrderReferencedDocument();

        /// <summary>
        /// Gets the ZUGFeRD version of a ZUGFeRD invoice that is passed via filename
        ///
        /// </summary>
        /// <param name="filename">Stream where to read the ZUGFeRD invoice</param>
        /// <returns>ZUGFeRD version of the invoice that was passed to the function</returns>
        public static ZUGFeRDVersion GetVersion(string filename)
        {
            IInvoiceDescriptorReader reader = new InvoiceDescriptor1Reader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return ZUGFeRDVersion.Version1;
            }

            reader = new InvoiceDescriptor22UBLReader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return ZUGFeRDVersion.Version23;
            }

            reader = new InvoiceDescriptor23CIIReader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return ZUGFeRDVersion.Version23;
            }

            reader = new InvoiceDescriptor20Reader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return ZUGFeRDVersion.Version20;
            }

            throw new UnsupportedException("No ZUGFeRD invoice reader was able to parse this file '" + filename + "'!");

            // return null;
        } // !GetVersion()



        /// <summary>
        /// Gets the ZUGFeRD version of a ZUGFeRD invoice that is passed via stream
        ///
        /// </summary>
        /// <param name="stream">Stream where to read the ZUGFeRD invoice</param>
        /// <returns>ZUGFeRD version of the invoice that was passed to the function</returns>
        public static ZUGFeRDVersion GetVersion(Stream stream)
        {
            IInvoiceDescriptorReader reader = new InvoiceDescriptor1Reader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return ZUGFeRDVersion.Version1;
            }

            reader = new InvoiceDescriptor22UBLReader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return ZUGFeRDVersion.Version23;
            }

            reader = new InvoiceDescriptor23CIIReader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return ZUGFeRDVersion.Version23;
            }

            reader = new InvoiceDescriptor20Reader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return ZUGFeRDVersion.Version20;
            }

            throw new UnsupportedException("No ZUGFeRD invoice reader was able to parse this stream!");
        } // !GetVersion()


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

            reader = new InvoiceDescriptor22UBLReader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return reader.Load(stream);
            }

            reader = new InvoiceDescriptor23CIIReader();
            if (reader.IsReadableByThisReaderVersion(stream))
            {
                return reader.Load(stream);
            }

            reader = new InvoiceDescriptor20Reader();
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

            reader = new InvoiceDescriptor22UBLReader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return reader.Load(filename);
            }

            reader = new InvoiceDescriptor23CIIReader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return reader.Load(filename);
            }

            reader = new InvoiceDescriptor20Reader();
            if (reader.IsReadableByThisReaderVersion(filename))
            {
                return reader.Load(filename);
            }

            throw new UnsupportedException("No ZUGFeRD invoice reader was able to parse this file '" + filename + "'!");

            // return null;
        } // !Load()


        /// <summary>
        /// Initializes a new invoice object and returns it.
        /// </summary>
        /// <param name="invoiceNo">Invoice number</param>
        /// <param name="invoiceDate">Invoice date</param>
        /// <param name="currency">Currency</param>
        /// <param name="invoiceNoAsReference">Remittance information</param>
        /// <returns></returns>
        public static InvoiceDescriptor CreateInvoice(string invoiceNo, DateTime invoiceDate, CurrencyCodes currency, string invoiceNoAsReference = "")
        {
            InvoiceDescriptor retval = new InvoiceDescriptor
            {
                InvoiceDate = invoiceDate,
                InvoiceNo = invoiceNo,
                Currency = currency,
                PaymentReference = invoiceNoAsReference
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


        public void SetBuyer(string name, string postcode, string city, string street, CountryCodes country, string id = null,
                             GlobalID globalID = null, string receiver = "", LegalOrganization legalOrganization = null, string countrySubdivisonName = null, string addressLine3 = null)
        {
            this.Buyer = new Party()
            {
                ID = new GlobalID(GlobalIDSchemeIdentifiers.Unknown, id),
                Name = name,
                Postcode = postcode,
                ContactName = receiver,
                City = city,
                Street = street,
                CountrySubdivisionName = countrySubdivisonName,
                AddressLine3 = addressLine3,
                Country = country,
                GlobalID = globalID,
                SpecifiedLegalOrganization = legalOrganization,
            };
        }


        public void SetSeller(string name, string postcode, string city, string street, CountryCodes country, string id = null,
                              GlobalID globalID = null, LegalOrganization legalOrganization = null, string description = null, string countrySubdivisonName = null, string addressLine3 = null)
        {
            this.Seller = new Party()
            {
                ID = new GlobalID(GlobalIDSchemeIdentifiers.Unknown, id),
                Name = name,
                Postcode = postcode,
                City = city,
                Street = street,
                CountrySubdivisionName = countrySubdivisonName,
                AddressLine3 = addressLine3,
                Country = country,
                GlobalID = globalID,
                SpecifiedLegalOrganization = legalOrganization,
                Description = description
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


        /// <summary>
        /// Sets the SpecifiedProcuringProject
        /// </summary>
        /// <param name="id">ProjectId</param>
        /// <param name="name">ProjectName</param>
        public void SetSpecifiedProcuringProject(string id, string name)
        {
            this.SpecifiedProcuringProject = new SpecifiedProcuringProject()
            {
                ID = id,
                Name = name
            };
        } // SetSpecifiedProcuringProject


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

        /// <summary>
        /// Sets the Buyer Electronic Address for Peppol
        /// </summary>
        /// <param name="address">Peppol Address</param>
        /// <param name="electronicAddressSchemeID">ElectronicAddressSchemeIdentifier</param>
        public void SetBuyerElectronicAddress(string address, ElectronicAddressSchemeIdentifiers electronicAddressSchemeID)
        {
            this.BuyerElectronicAddress = new ElectronicAddress()
            {
                Address = address,
                ElectronicAddressSchemeID = electronicAddressSchemeID
            };
        } // !SetBuyerEndpointID()

        /// <summary>
        /// Sets the Seller Electronic Address for Peppol
        /// </summary>
        /// <param name="address">Peppol Address</param>
        /// <param name="electronicAddressSchemeID">ElectronicAddressSchemeIdentifier</param>
        public void SetSellerElectronicAddress(string address, ElectronicAddressSchemeIdentifiers electronicAddressSchemeID)
        {
            this.SellerElectronicAddress = new ElectronicAddress()
            {
                Address = address,
                ElectronicAddressSchemeID = electronicAddressSchemeID
            };
        } // !SetSellerEndpointID()


        /// <summary>
        /// Add an additional reference document
        /// Note: LineID is only on line item-level
        /// </summary>
        /// <param name="id">Document number such as delivery note no or credit memo no</param>
        /// <param name="typeCode"></param>
        /// <param name="issueDateTime">Document Date</param>
        /// <param name="name"></param>
        /// <param name="referenceTypeCode">Type of the referenced document</param>
        /// <param name="attachmentBinaryObject"></param>
        /// <param name="filename"></param>
        /// <param name="uriID"></param>
        public void AddAdditionalReferencedDocument(string id, AdditionalReferencedDocumentTypeCode typeCode,
            DateTime? issueDateTime = null, string name = null, ReferenceTypeCodes referenceTypeCode = ReferenceTypeCodes.Unknown,
            byte[] attachmentBinaryObject = null, string filename = null, string uriID = null)
        {
            this.AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ReferenceTypeCode = referenceTypeCode,
                ID = id,
                IssueDateTime = issueDateTime,
                Name = name,
                AttachmentBinaryObject = attachmentBinaryObject,
                Filename = filename,
                TypeCode = typeCode,
                URIID = uriID
            });
        } // !AddAdditionalReferencedDocument()

        /// <summary>
        /// Sets details of the associated order
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="orderDate"></param>
        public void SetBuyerOrderReferenceDocument(string orderNo, DateTime? orderDate = null)
        {
            this.OrderNo = orderNo;
            this.OrderDate = orderDate;
        } // !SetBuyerOrderReferenceDocument()

        /// <summary>
        /// Sets detailed information about the corresponding despatch advice
        /// </summary>
        /// <param name="despatchAdviceNo"></param>
        /// <param name="despatchAdviceDate"></param>
        public void SetDespatchAdviceReferencedDocument(string despatchAdviceNo, DateTime? despatchAdviceDate = null)
        {
            this.DespatchAdviceReferencedDocument = new DespatchAdviceReferencedDocument()
            {
                ID = despatchAdviceNo,
                IssueDateTime = despatchAdviceDate
            };
        } // !SetDespatchAdviceReferencedDocument()

        /// <summary>
        /// Sets detailed information about the corresponding delivery note
        /// </summary>
        /// <param name="deliveryNoteNo"></param>
        /// <param name="deliveryNoteDate"></param>
        public void SetDeliveryNoteReferenceDocument(string deliveryNoteNo, DateTime? deliveryNoteDate = null)
        {
            this.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            {
                ID = deliveryNoteNo,
                IssueDateTime = deliveryNoteDate
            };
        } // !SetDeliveryNoteReferenceDocument()

        /// <summary>
        /// Sets detailed information about the corresponding contract
        /// </summary>
        /// <param name="contractNo">Contract number</param>
        /// <param name="contractDate">Date of the contract</param>
        public void SetContractReferencedDocument(string contractNo, DateTime? contractDate)
        {
            this.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = contractNo,
                IssueDateTime = contractDate
            };
        } // !SetContractReferencedDocument()


        /// <summary>
        /// The logistics service charge (ram:SpecifiedLogisticsServiceCharge) is part of the ZUGFeRD specification.
        /// Please note that it is not part of the XRechnung specification, thus, everything passed to this function will not
        /// be written when writing XRechnung format.
        ///
        /// You might use AddTradeAllowanceCharge() instead.
        /// </summary>
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


        public List<ServiceCharge> GetLogisticsServiceCharges()
        {
            return this.ServiceCharges;
        } // !GetLogisticsServiceCharges()


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
        /// <param name="reasonCode">Reason code for the allowance</param>
        /// <param name="taxTypeCode">VAT type code for document level allowance/ charge</param>
        /// <param name="taxCategoryCode">VAT type code for document level allowance/ charge</param>
        /// <param name="taxPercent">VAT rate for the allowance</param>
        public void AddTradeAllowanceCharge(bool isDiscount, decimal? basisAmount, CurrencyCodes currency, decimal actualAmount,
                                            string reason, TaxTypes taxTypeCode, TaxCategoryCodes taxCategoryCode, decimal taxPercent,
                                            AllowanceReasonCodes reasonCode = AllowanceReasonCodes.Unknown)
        {
            this._TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Reason = reason,
                ReasonCode = reasonCode,
                BasisAmount = basisAmount,
                ActualAmount = actualAmount,
                Currency = currency,
                Amount = actualAmount,
                ChargePercentage = null,
                Tax = new Tax()
                {
                    CategoryCode = taxCategoryCode,
                    TypeCode = taxTypeCode,
                    Percent = taxPercent
                }
            });
        } // !AddTradeAllowanceCharge()


        /// <summary>
        /// Adds an allowance or charge on document level.
        ///
        /// Allowance represents a discount whereas charge represents a surcharge.
        /// </summary>
        /// <param name="isDiscount">Marks if the allowance charge is a discount. Please note that in contrary to this function, the xml file indicated a surcharge, not a discount (value will be inverted)</param>
        /// <param name="basisAmount">Base amount (basis of allowance)</param>
        /// <param name="currency">Curency of the allowance</param>
        /// <param name="actualAmount">Actual allowance charge amount</param>
        /// <param name="chargePercentage">Actual allowance charge percentage</param>
        /// <param name="reason">Reason for the allowance</param>
        /// <param name="taxTypeCode">VAT type code for document level allowance/ charge</param>
        /// <param name="taxCategoryCode">VAT type code for document level allowance/ charge</param>
        /// <param name="taxPercent">VAT rate for the allowance</param>
        /// <param name="reasonCode">Reason code for the allowance</param>
        public void AddTradeAllowanceCharge(bool isDiscount, decimal? basisAmount, CurrencyCodes currency, decimal actualAmount, decimal? chargePercentage, string reason, TaxTypes taxTypeCode, TaxCategoryCodes taxCategoryCode, decimal taxPercent, AllowanceReasonCodes reasonCode = AllowanceReasonCodes.Unknown)
        {
            this._TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Reason = reason,
                ReasonCode = reasonCode,
                BasisAmount = basisAmount,
                ActualAmount = actualAmount,
                Currency = currency,
                Amount = actualAmount,
                ChargePercentage = chargePercentage,
                Tax = new Tax()
                {
                    CategoryCode = taxCategoryCode,
                    TypeCode = taxTypeCode,
                    Percent = taxPercent
                }
            });
        } // !AddTradeAllowanceCharge()


        /// <summary>
        /// Returns all existing trade allowance charges
        /// </summary>
        /// <returns></returns>
        public IList<TradeAllowanceCharge> GetTradeAllowanceCharges()
        {
            return this._TradeAllowanceCharges;
        } // !GetTradeAllowanceCharges()


        /// <summary>
        /// Adds a trade payment term.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="paymentTermsType"></param>
        /// <param name="dueDays"></param>
        /// <param name="percentage"></param>
        /// <param name="baseAmount"></param>
        /// <param name="actualAmount"></param>
        public void AddTradePaymentTerms(string description, DateTime? dueDate = null,
            PaymentTermsType? paymentTermsType = null, int? dueDays = null,
            decimal? percentage = null, decimal? baseAmount = null, decimal? actualAmount = null)
        {
            _PaymentTerms.Add(new PaymentTerms()
            {
                Description = description,
                DueDate = dueDate,
                PaymentTermsType = paymentTermsType,
                DueDays = dueDays,
                Percentage = percentage,
                BaseAmount = baseAmount,
                ActualAmount = actualAmount
            });
        }


        /// <summary>
        /// Removes all existing trade payment terms
        /// </summary>
        public void ClearTradePaymentTerms()
        {
            _PaymentTerms.Clear();
        } // !ClearTradePaymentTerms()


        /// <summary>
        /// Returns all existing trade payment terms.
        /// </summary>
        /// <returns></returns>
        public IList<PaymentTerms> GetTradePaymentTerms()
        {
            return _PaymentTerms;
        }

        /// <summary>
        /// Set Information about Preceding Invoice. Please note that all versions prior ZUGFeRD 2.3 and UBL only
        /// allow one of such reference.
        /// </summary>
        /// <param name="id">Preceding InvoiceNo</param>
        /// <param name="IssueDateTime">Preceding Invoice Date</param>
        public void AddInvoiceReferencedDocument(string id, DateTime? IssueDateTime = null)
        {
            this._InvoiceReferencedDocuments.Add(new InvoiceReferencedDocument()
            {
                ID = id,
                IssueDateTime = IssueDateTime
            });
        }


        /// <summary>
        /// Retrieves all preceding invoice references
        /// </summary>
        /// <returns></returns>
        public List<InvoiceReferencedDocument> GetInvoiceReferencedDocuments()
        {
            return this._InvoiceReferencedDocuments;
        } // !GetInvoiceReferencedDocuments()


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
        /// <param name="roundingAmount">RoundingAmount / Rundungsbetrag, profile COMFORT and EXTENDED</param>
        public void SetTotals(decimal? lineTotalAmount = null, decimal? chargeTotalAmount = null,
                              decimal? allowanceTotalAmount = null, decimal? taxBasisAmount = null,
                              decimal? taxTotalAmount = null, decimal? grandTotalAmount = null,
                              decimal? totalPrepaidAmount = null, decimal? duePayableAmount = null,
                              decimal? roundingAmount = null)
        {
            this.LineTotalAmount = lineTotalAmount;
            this.ChargeTotalAmount = chargeTotalAmount;
            this.AllowanceTotalAmount = allowanceTotalAmount;
            this.TaxBasisAmount = taxBasisAmount;
            this.TaxTotalAmount = taxTotalAmount;
            this.GrandTotalAmount = grandTotalAmount;
            this.TotalPrepaidAmount = totalPrepaidAmount;
            this.DuePayableAmount = duePayableAmount;
            this.RoundingAmount = roundingAmount;
        }


        /// <summary>
        /// Add information about VAT and apply to the invoice line items for goods and services on the invoice.
        ///
        /// This tax is added per VAT/ tax rate.
        /// </summary>
        /// <param name="basisAmount"></param>
        /// <param name="percent">Tax rate where the tax belongs to</param>
        /// <param name="taxAmount">Tax amount, i.e. basisAmount * percent</param>
        /// <param name="typeCode"></param>
        /// <param name="categoryCode"></param>
        /// <param name="allowanceChargeBasisAmount"></param>
        /// <param name="exemptionReasonCode"></param>
        /// <param name="exemptionReason"></param>
        /// <param name="lineTotalBasisAmount">A monetary value used as the line total basis on which this trade related tax, levy or duty is calculated</param>
        public void AddApplicableTradeTax(decimal basisAmount,
            decimal percent,
            decimal taxAmount,
            TaxTypes typeCode,
            TaxCategoryCodes? categoryCode = null,
            decimal? allowanceChargeBasisAmount = null,
            TaxExemptionReasonCodes? exemptionReasonCode = null,
            string exemptionReason = null,
            decimal? lineTotalBasisAmount = null)
        {
            Tax tax = new Tax()
            {
                BasisAmount = basisAmount,
                TaxAmount = taxAmount,
                Percent = percent,
                TypeCode = typeCode,
                AllowanceChargeBasisAmount = allowanceChargeBasisAmount,
                LineTotalBasisAmount = lineTotalBasisAmount,
                ExemptionReasonCode = exemptionReasonCode,
                ExemptionReason = exemptionReason
            };

            if ((categoryCode != null) && (categoryCode.Value != TaxCategoryCodes.Unknown))
            {
                tax.CategoryCode = categoryCode;
            }

            this.Taxes.Add(tax);
        } // !AddApplicableTradeTax()


        public List<Tax> GetApplicableTradeTaxes()
        {
            return this.Taxes;
        } // !GetApplicableTradeTaxes()


        public bool AnyApplicableTradeTaxes()
        {
            return this.Taxes.Any();
        } // !AnyApplicableTradeTaxes()


        private static IInvoiceDescriptorWriter _SelectInvoiceDescriptorWriter(ZUGFeRDVersion version)
        {
            switch (version)
            {
                case ZUGFeRDVersion.Version1:
                    return new InvoiceDescriptor1Writer();
                case ZUGFeRDVersion.Version20:
                    return new InvoiceDescriptor20Writer();
                case ZUGFeRDVersion.Version23:
                    return new InvoiceDescriptor23Writer();
                default:
                    throw new UnsupportedException("New ZUGFeRDVersion '" + version + "' defined but not implemented!");
            }
        } // !_SelectInvoiceDescriptorWriter()


        /// <summary>
        /// Saves the descriptor object into a stream.
        ///
        /// The stream position will be reset to the original position after writing is finished.
        /// This allows easy further processing of the stream.
        /// </summary>
        /// <param name="stream">The stream where the data should be saved to.</param>
        /// <param name="version">The ZUGFeRD version you want to use. Defaults to version 1.</param>
        /// <param name="profile">The ZUGFeRD profile you want to use. Defaults to Basic.</param>
        /// <param name="format">The format of the target file that may be CII or UBL</param>
        public void Save(Stream stream, ZUGFeRDVersion version = ZUGFeRDVersion.Version1, Profile profile = Profile.Basic, ZUGFeRDFormats format = ZUGFeRDFormats.CII)
        {
            this.Profile = profile;
            IInvoiceDescriptorWriter writer = _SelectInvoiceDescriptorWriter(version);
            writer.Save(this, stream, format);
        } // !Save()


        /// <summary>
        /// Saves the descriptor object into a file with given name.
        /// </summary>
        /// <param name="filename">The filename where the data should be saved to.</param>
        /// <param name="version">The ZUGFeRD version you want to use. Defaults to version 1.</param>
        /// <param name="profile">The ZUGFeRD profile you want to use. Defaults to Basic.</param>
        public void Save(string filename, ZUGFeRDVersion version = ZUGFeRDVersion.Version1, Profile profile = Profile.Basic, ZUGFeRDFormats format = ZUGFeRDFormats.CII)
        {
            this.Profile = profile;
            IInvoiceDescriptorWriter writer = _SelectInvoiceDescriptorWriter(version);
            writer.Save(this, filename, format);
        } // !Save()


        /// <summary>
        /// Adds a new comment as a dedicated line of the invoice.
        ///
        /// The line id is generated automatically
        /// </summary>
        /// <param name="comment"></param>
        /// /// <param name="name">The item name (could e.g. be TEXT or COMMENT for comment items)</param>
        /// <param name="sellerAssignedID">ID of the comment, same as item no for regular invoice lines. Could e.g. bei TEXT or COMMENT</param>
        public TradeLineItem AddTradeLineCommentItem(string comment, string name = "", string sellerAssignedID = "")
        {
            return AddTradeLineCommentItem(_getNextLineId(), comment, name, sellerAssignedID);
        } // !AddTradeLineCommentItem()


        /// <summary>
        /// Adds a new comment as a dedicated line of the invoice.
        ///
        /// The line id is passed as a parameter
        /// </summary>
        /// <param name="lineID"></param>
        /// <param name="comment"></param>
        /// <param name="name">The item name (could e.g. be TEXT or COMMENT for comment items)</param>
        /// <param name="sellerAssignedID">ID of the comment, same as item no for regular invoice lines. Could e.g. bei TEXT or COMMENT</param>
        public TradeLineItem AddTradeLineCommentItem(string lineID, string comment, string name = "", string sellerAssignedID = "")
        {
            if (String.IsNullOrWhiteSpace(lineID))
            {
                throw new ArgumentException("LineID cannot be Null or Empty");
            }
            else
            {
                if (this.TradeLineItems.Any(p => p.AssociatedDocument.LineID.Equals(lineID, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("LineID must be unique");
                }
            }

            TradeLineItem item = new TradeLineItem(lineID)
            {
                GrossUnitPrice = 0m,
                NetUnitPrice = 0m,
                BilledQuantity = 0m,
                UnitCode = QuantityCodes.C62,
                TaxCategoryCode = TaxCategoryCodes.O
            };

            if (!string.IsNullOrWhiteSpace(name))
            {
                item.Name = name;
            }

            if (!string.IsNullOrWhiteSpace(sellerAssignedID))
            {
                item.SellerAssignedID = sellerAssignedID;
            }

            item.AssociatedDocument.Notes.Add(new Note(
                content: comment,
                subjectCode: SubjectCodes.Unknown,
                contentCode: ContentCodes.Unknown
            ));

            this.TradeLineItems.Add(item);
            return item;
        } // !AddTradeLineCommentItem()




        /// <summary>
        /// Adds a new line to the invoice. The line id is generated automatically.
        ///
        /// Please note that this function returns the new trade line item object that you might use
        /// in your code to add more detailed information to the trade line item.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="unitCode"></param>
        /// <param name="unitQuantity"></param>
        /// <param name="grossUnitPrice"></param>
        /// <param name="netUnitPrice"></param>
        /// <param name="billedQuantity"></param>
        /// <param name="lineTotalAmount">net total including discounts and surcharges. This parameter is optional. If it is not filled, the line total amount is automatically calculated based on netUnitPrice and billedQuantity</param>
        /// <param name="taxType"></param>
        /// <param name="categoryCode"></param>
        /// <param name="taxPercent"></param>
        /// <param name="comment"></param>
        /// <param name="id"></param>
        /// <param name="sellerAssignedID"></param>
        /// <param name="buyerAssignedID"></param>
        /// <param name="deliveryNoteID"></param>
        /// <param name="deliveryNoteDate"></param>
        /// <param name="buyerOrderLineID"></param>
        /// <param name="buyerOrderID">only Extended</param>
        /// <param name="buyerOrderDate"></param>
        /// <param name="billingPeriodStart"></param>
        /// <param name="billingPeriodEnd"></param>
        /// <returns>Returns the instance of the trade line item. You might use this object to add details such as trade allowance charges</returns>
        public TradeLineItem AddTradeLineItem(string name,
                                     string description = null,
                                     QuantityCodes unitCode = QuantityCodes.Unknown,
                                     decimal? unitQuantity = null,
                                     decimal? grossUnitPrice = null,
                                     decimal? netUnitPrice = null,
                                     decimal billedQuantity = 0,
                                     decimal? lineTotalAmount = null,
                                     TaxTypes taxType = TaxTypes.Unknown,
                                     TaxCategoryCodes categoryCode = TaxCategoryCodes.Unknown,
                                     decimal taxPercent = 0,
                                     string comment = null,
                                     GlobalID id = null,
                                     string sellerAssignedID = "", string buyerAssignedID = "",
                                     string deliveryNoteID = "", DateTime? deliveryNoteDate = null,
                                     string buyerOrderLineID = "", string buyerOrderID = "", DateTime? buyerOrderDate = null,
                                     DateTime? billingPeriodStart = null, DateTime? billingPeriodEnd = null
                                     )
        {
            return AddTradeLineItem(lineID: _getNextLineId(),
                             name: name,
                             description: description,
                             unitCode: unitCode,
                             unitQuantity: unitQuantity,
                             grossUnitPrice: grossUnitPrice,
                             netUnitPrice: netUnitPrice,
                             billedQuantity: billedQuantity,
                             lineTotalAmount: lineTotalAmount,
                             taxType: taxType,
                             categoryCode: categoryCode,
                             taxPercent: taxPercent,
                             comment: comment,
                             id: id,
                             sellerAssignedID: sellerAssignedID,
                             buyerAssignedID: buyerAssignedID,
                             deliveryNoteID: deliveryNoteID,
                             deliveryNoteDate: deliveryNoteDate,
                             buyerOrderLineID: buyerOrderLineID,
                             buyerOrderID: buyerOrderID, // Extended!
                             buyerOrderDate: buyerOrderDate,
                             billingPeriodStart: billingPeriodStart,
                             billingPeriodEnd: billingPeriodEnd
                             );
        } // !AddTradeLineItem()



        /// <summary>
        /// Adds a new line to the invoice. The line id is passed as a parameter.
        /// </summary>
        public TradeLineItem AddTradeLineItem(string lineID,
                                     string name,
                                     string description = null,
                                     QuantityCodes unitCode = QuantityCodes.Unknown,
                                     decimal? unitQuantity = null,
                                     decimal? grossUnitPrice = null,
                                     decimal? netUnitPrice = null,
                                     decimal billedQuantity = 0,
                                     decimal? lineTotalAmount = null,
                                     TaxTypes taxType = TaxTypes.Unknown,
                                     TaxCategoryCodes categoryCode = TaxCategoryCodes.Unknown,
                                     decimal taxPercent = 0,
                                     string comment = null,
                                     GlobalID id = null,
                                     string sellerAssignedID = "", string buyerAssignedID = "",
                                     string deliveryNoteID = "", DateTime? deliveryNoteDate = null,
                                     string buyerOrderLineID = "", string buyerOrderID = "", DateTime? buyerOrderDate = null,
                                     DateTime? billingPeriodStart = null, DateTime? billingPeriodEnd = null
                                     )
        {
            if (String.IsNullOrWhiteSpace(lineID))
            {
                throw new ArgumentException("LineID cannot be Null or Empty");
            }
            else
            {
                if (this.TradeLineItems.Any(p => p.AssociatedDocument.LineID.Equals(lineID, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("LineID must be unique");
                }
            }

            TradeLineItem newItem = new TradeLineItem(lineID)
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
                LineTotalAmount = lineTotalAmount,
                TaxType = taxType,
                TaxCategoryCode = categoryCode,
                TaxPercent = taxPercent,
                BillingPeriodStart = billingPeriodStart,
                BillingPeriodEnd = billingPeriodEnd
            };

            if (!String.IsNullOrWhiteSpace(comment))
            {
                newItem.AssociatedDocument.Notes.Add(new Note(comment, SubjectCodes.Unknown, ContentCodes.Unknown));
            }

            if (!String.IsNullOrWhiteSpace(deliveryNoteID) || deliveryNoteDate.HasValue)
            {
                newItem.SetDeliveryNoteReferencedDocument(deliveryNoteID, deliveryNoteDate);
            }

            if (!String.IsNullOrWhiteSpace(buyerOrderLineID) || buyerOrderDate.HasValue || !String.IsNullOrWhiteSpace(buyerOrderID))
            {
                newItem.SetOrderReferencedDocument(buyerOrderID, buyerOrderDate, buyerOrderLineID);
            }

            this.TradeLineItems.Add(newItem);
            return newItem;
        } // !AddTradeLineItem()


        internal void _AddTradeLineItem(TradeLineItem item)
        {
            this.TradeLineItems.Add(item);
        } // !_AddTradeLineItem()


        internal void _AddTradeLineItems(IEnumerable<TradeLineItem> items)
        {
            this.TradeLineItems.AddRange(items);
        } // !_AddTradeLineItems()


        public List<TradeLineItem> GetTradeLineItems()
        {
            return this.TradeLineItems;
        } // !GetTradeLineItems()


        public bool AnyTradeLineItems()
        {
            return this.TradeLineItems.Any();
        } // !AnyTradeLineItems()


        /// <summary>
        /// Sets up the payment means.
        /// </summary>
        /// <param name="paymentCode">Payment means type code.</param>
        /// <param name="information">Additional information.</param>
        /// <param name="identifikationsnummer">SEPA creditor identifier.</param>
        /// <param name="mandatsnummer">SEPA mandate reference.</param>
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


        /// <summary>
        /// Sets up the payment means for SEPA direct debit.
        /// </summary>
        public void SetPaymentMeansSepaDirectDebit(string sepaCreditorIdentifier, string sepaMandateReference, string information = "")
        {
            this.PaymentMeans = new PaymentMeans
            {
                TypeCode = PaymentMeansTypeCodes.SEPADirectDebit,
                Information = information,
                SEPACreditorIdentifier = sepaCreditorIdentifier,
                SEPAMandateReference = sepaMandateReference
            };
        } // !SetPaymentMeans()


        /// <summary>
        ///     Sets up the payment means for payment via bank card.
        /// </summary>
        public void SetPaymentMeansBankCard(string bankCardId, string bankCardCardholder, string information = "")
        {
            this.PaymentMeans = new PaymentMeans
            {
                TypeCode = PaymentMeansTypeCodes.BankCard,
                Information = information,
                FinancialCard = new FinancialCard
                {
                    Id = bankCardId,
                    CardholderName = bankCardCardholder
                }
            };
        } // !SetPaymentMeans()


        /// <summary>
        /// Adds a group of business terms to specify credit transfer payments
        /// </summary>
        /// <param name="iban">IBAN</param>
        /// <param name="bic">BIC</param>
        /// <param name="id">Optional: old German bank account no</param>
        /// <param name="bankleitzahl">Optional: old German Bankleitzahl</param>
        /// <param name="bankName">Optional: old German bank name</param>
        /// <param name="name">Optional: bank account name</param>
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


        internal void _AddCreditorFinancialAccount(BankAccount bankAccount)
        {
            this.CreditorBankAccounts.Add(bankAccount);
        } // !_AddCreditorFinancialAccount()


        public List<BankAccount> GetCreditorFinancialAccounts()
        {
            return this.CreditorBankAccounts;
        } // !GetCreditorFinancialAccounts()


        public bool AnyCreditorFinancialAccount()
        {
            return this.CreditorBankAccounts.Any();
        } // !AnyCreditorFinancialAccount()


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


        internal void _AddDebitorFinancialAccount(BankAccount bankAccount)
        {
            this.DebitorBankAccounts.Add(bankAccount);
        } // !_AddDebitorFinancialAccount()


        public List<BankAccount> GetDebitorFinancialAccounts()
        {
            return this.DebitorBankAccounts;
        } // !GetDebitorFinancialAccounts()


        public bool AnyDebitorFinancialAccount()
        {
            return this.DebitorBankAccounts.Any();
        } // !AnyDebitorFinancialAccount()


        public void AddReceivableSpecifiedTradeAccountingAccount(string AccountID)
        {
            AddReceivableSpecifiedTradeAccountingAccount(AccountID, AccountingAccountTypeCodes.Unknown);
        }


        public void AddReceivableSpecifiedTradeAccountingAccount(string AccountID, AccountingAccountTypeCodes AccountTypeCode)
        {
            this._ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
            {
                TradeAccountID = AccountID,
                TradeAccountTypeCode = AccountTypeCode
            });
        } // !AddReceivableSpecifiedTradeAccountingAccount()


        public List<ReceivableSpecifiedTradeAccountingAccount> GetReceivableSpecifiedTradeAccountingAccounts()
        {
            return this._ReceivableSpecifiedTradeAccountingAccounts;
        } // !GetReceivableSpecifiedTradeAccountingAccounts()


        public bool AnyReceivableSpecifiedTradeAccountingAccounts()
        {
            return this._ReceivableSpecifiedTradeAccountingAccounts.Any();
        } // !AnyReceivableSpecifiedTradeAccountingAccounts()


        private string _getNextLineId()
        {
            int highestLineId = this.TradeLineItems.Select(i => { if (Int32.TryParse(i.AssociatedDocument?.LineID, out int id) == true) return id; else return 0; }).DefaultIfEmpty(0).Max();
            return (highestLineId + 1).ToString();
        } // !_getNextLineId()
    }
}
