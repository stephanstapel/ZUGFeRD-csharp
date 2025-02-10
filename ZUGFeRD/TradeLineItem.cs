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
using System.Net;
using System.Security.Cryptography;


namespace s2industries.ZUGFeRD
{
    /// <summary>
    ///  Structure holding item information
    ///
    /// Please note that you might use the object that is returned from InvoiceDescriptor.AddTradeLineItem(...) and use it
    /// to e.g. add an allowance charge using lineItem.AddTradeAllowanceCharge(...)
    /// </summary>
    public class TradeLineItem
    {
        /// <summary>
        /// The identification of articles based on a registered scheme
        ///
        /// The global identifier of the article is a globally unique identifier of the product being assigned to it by its
        /// producer, bases on the rules of a global standardisation body.
        ///
        /// BT-157
        /// </summary>
        public GlobalID GlobalID { get; set; } = new GlobalID();

        /// <summary>
        /// An identification of the item assigned by the seller.
        ///
        /// BT-155
        /// </summary>
        public string SellerAssignedID { get; set; }

        /// <summary>
        /// An identification of the item assigned by the buyer.
        ///
        /// BT-156
        /// </summary>
        public string BuyerAssignedID { get; set; }

        /// <summary>
        /// An item’s name
        ///
        /// BT-153
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of an item
        ///
        /// The item’s description makes it possible to describe a product and its properties more comprehensively
        /// than would be possible with just the article name.
        ///
        /// BT-154
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Included amount
        ///
        /// BT-149
        /// </summary>
        public decimal? UnitQuantity { get; set; }

        /// <summary>
        /// Invoiced quantity
        ///
        /// BT-129
        /// </summary>
        public decimal BilledQuantity { get; set; }

        /// <summary>
        /// No charge quantity
        ///
        /// BT-X-46
        /// </summary>
        public decimal? ChargeFreeQuantity { get; set; }

        /// <summary>
        /// Package quantity
        ///
        /// BT-X-47
        /// </summary>
        public decimal? PackageQuantity { get; set; }

        /// <summary>
        /// Invoice line net amount including (!) trade allowance charges for the line item
        ///
        /// BT-131
        /// </summary>
        public decimal? LineTotalAmount { get; set; }

        /// <summary>
        /// Detailed information about the invoicing period
        ///
        /// Invoicing period start date
        ///
        /// BT-134
        /// </summary>
        public DateTime? BillingPeriodStart { get; set; }

        /// <summary>
        /// Detailed information about the invoicing period
        ///
        /// Invoicing period end date
        ///
        /// BT-135
        /// </summary>
        public DateTime? BillingPeriodEnd { get; set; }

        /// <summary>
        /// The code valid for the invoiced goods sales tax category
        ///
        /// BT-151
        /// </summary>
        public TaxCategoryCodes TaxCategoryCode { get; set; }

        /// <summary>
        /// Tax rate
        ///
        /// BT-152
        /// </summary>
        public decimal TaxPercent { get; set; }

        /// <summary>
        /// Tax type
        ///
        /// BT-151-0
        /// </summary>
        public TaxTypes TaxType { get; set; } = TaxTypes.VAT;

        /// <summary>
        /// Exemption Reason Text for no Tax
        /// 
        /// BT-X-96
        /// </summary>
        public string TaxExemptionReason { get; set; }

        /// <summary>
        /// ExemptionReasonCode for no Tax
        /// 
        /// BT-X-97
        /// </summary>
        public TaxExemptionReasonCodes? TaxExemptionReasonCode { get; set; }

        /// <summary>
        /// Net unit price of the item
        ///
        /// BT-146
        /// </summary>
        [Obsolete("Please note that NetUnitPrice will not be optional but mandatory beginning with version 18.0")]
        public decimal? NetUnitPrice { get; set; }

        /// <summary>
        /// Gross unit price of the item
        ///
        /// BT-148
        /// </summary>
        public decimal? GrossUnitPrice { get; set; }

        /// <summary>
        /// Item Base Quantity Unit Code
        ///
        /// BT-130
        /// </summary>
        public QuantityCodes UnitCode { get; set; }

        /// <summary>
        /// Charge Free Quantity Unit Code
        /// 
        /// BT-X-46-0
        /// </summary>
        public QuantityCodes ChargeFreeUnitCode { get; set; }

        /// <summary>
        /// Package Quantity Unit Code
        /// 
        /// BT-X-47-0
        /// </summary>
        public QuantityCodes PackageUnitCode { get; set; }
        
        /// <summary>
        /// Identifier of the invoice line item
        ///
        /// BT-126
        /// </summary>
        public AssociatedDocument AssociatedDocument { get; internal set; }

        /// <summary>
        /// Detailed information about the actual Delivery
        ///
        /// BT-72
        /// </summary>
        public DateTime? ActualDeliveryDate { get; set; }

        /// <summary>
        /// Details of the associated order
        ///
        /// BT-132
        /// </summary>
        public BuyerOrderReferencedDocument BuyerOrderReferencedDocument { get; set; }

        /// <summary>
        /// Detailed information about the corresponding delivery note
        ///
        /// BG-X-83
        /// </summary>
        public DeliveryNoteReferencedDocument DeliveryNoteReferencedDocument { get; set; }

        /// <summary>
        /// Details of the associated contract
        ///
        /// BG-X-2
        /// </summary>
        public ContractReferencedDocument ContractReferencedDocument { get; set; }

        /// <summary>
        /// Details of an additional document reference
        ///
        /// Marked as internal so it can be accessed by the readers and writers
        /// </summary>
        internal List<AdditionalReferencedDocument> _AdditionalReferencedDocuments { get; set; } = new List<AdditionalReferencedDocument>();

        /// <summary>
        /// A group of business terms providing information about the applicable surcharges or discounts on the total amount of the invoice
        ///
        /// Now private. Please use GetTradeAllowanceCharges() instead
        /// </summary>
        private List<TradeAllowanceCharge> _TradeAllowanceCharges { get; set; } = new List<TradeAllowanceCharge>();

        /// <summary>
        /// A group of business terms providing information about the applicable surcharges or discounts on the total amount of the invoice item
        ///
        /// Now private. Please use GetSpecifiedTradeAllowanceCharges() instead
        ///
        /// BG-27 / BG-28
        /// </summary>
        private List<TradeAllowanceCharge> SpecifiedTradeAllowanceCharges { get; set; } = new List<TradeAllowanceCharge>();

        /// <summary>
        /// Detailed information on the accounting reference
        ///
        /// BT-19-00
        /// </summary>
        public List<ReceivableSpecifiedTradeAccountingAccount> ReceivableSpecifiedTradeAccountingAccounts { get; set; } = new List<ReceivableSpecifiedTradeAccountingAccount>();

        /// <summary>
        /// Included Items referenced from this trade product.
        ///
        /// BG-X-1
        /// </summary>
        public List<IncludedReferencedProduct> IncludedReferencedProducts { get; internal set; } = new List<IncludedReferencedProduct>();

        /// <summary>
        /// Additional product information
        ///
        /// BG-32
        /// </summary>
        public List<ApplicableProductCharacteristic> ApplicableProductCharacteristics { get; set; } = new List<ApplicableProductCharacteristic>();


        /// <summary>
        /// Detailed information on the item classification
        ///
        /// BG-158
        /// </summary>
        private List<DesignatedProductClassification> DesignatedProductClassifications { get; set; } = new List<DesignatedProductClassification>();


        /// <summary>
        /// Recipient of the delivered goods. This party is optional and is written in Extended profile only
        ///
        /// BG-X-7
        /// </summary>
        public Party ShipTo { get; set; }


        /// <summary>
        /// Detailed information on the deviating final recipient. This party is optional and only relevant for Extended profile
        ///
        /// BG-X-10
        /// </summary>
        public Party UltimateShipTo { get; set; }


        /// <summary>
        /// Creates a new trade line item with the specified line identifier
        /// </summary>
        /// <param name="lineId">The unique identifier for this trade line item</param>
        public TradeLineItem(string lineId)
        {
            this.AssociatedDocument = new AssociatedDocument(lineId);
        } // !TradeLineItem()


        /// <summary>
        /// As an allowance or charge on item level, attaching it to the corresponding item.
        /// </summary>
        /// <param name="isDiscount">Marks if its an allowance (true) or charge (false). Please note that the xml will present inversed values</param>
        /// <param name="currency">Currency of the allowance or surcharge</param>
        /// <param name="basisAmount">Basis aount for the allowance or surcharge, typicalls the net amount of the item</param>
        /// <param name="actualAmount">The actual allowance or surcharge amount</param>
        /// <param name="reason">Reason for the allowance or surcharge</param>
        /// <param name="reasonCode">Reason code for the allowance or surcharge</param>
        public TradeLineItem AddTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal? basisAmount, decimal actualAmount,
                                            string reason, AllowanceReasonCodes reasonCode = AllowanceReasonCodes.Unknown)
        {
            this._TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                Reason = reason,
                ReasonCode = reasonCode
            });
            return this;
        } // !AddTradeAllowanceCharge()


        /// <summary>
        /// As an allowance or charge on item level, attaching it to the corresponding item.
        /// </summary>
        /// <param name="isDiscount">Marks if its an allowance (true) or charge (false). Please note that the xml will present inversed values</param>
        /// <param name="currency">Currency of the allowance or surcharge</param>
        /// <param name="basisAmount">Basis aount for the allowance or surcharge, typicalls the net amount of the item</param>
        /// <param name="actualAmount">The actual allowance or surcharge amount</param>
        /// <param name="chargePercentage">Actual allowance or surcharge charge percentage</param>
        /// <param name="reason">Reason for the allowance or surcharge</param>
        /// <param name="reasonCode">Reason code for the allowance or surcharge</param>
        public TradeLineItem AddTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal? basisAmount, decimal actualAmount,
                                            decimal? chargePercentage, string reason, AllowanceReasonCodes reasonCode = AllowanceReasonCodes.Unknown)
        {
            this._TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                ChargePercentage = chargePercentage,
                Reason = reason,
                ReasonCode = reasonCode
            });
            return this;
        } // !AddTradeAllowanceCharge()


        /// <summary>
        /// Returns all trade allowance charges for the trade line item
        /// </summary>
        /// <returns></returns>
        public IList<TradeAllowanceCharge> GetTradeAllowanceCharges()
        {
            return this._TradeAllowanceCharges;
        } // !GetTradeAllowanceCharges()


        /// <summary>
        /// As an allowance or charge on total item price, attaching it to the corresponding item.
        /// </summary>
        /// <param name="isDiscount">Marks if its an allowance (true) or charge (false). Please note that the xml will present inversed values</param>
        /// <param name="currency">Currency of the allowance or surcharge</param>
        /// <param name="basisAmount">Basis aount for the allowance or surcharge, typicalls the net amount of the item</param>
        /// <param name="actualAmount">The actual allowance or surcharge amount</param>
        /// <param name="reason">Reason for the allowance or surcharge</param>
        public TradeLineItem AddSpecifiedTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal? basisAmount, decimal actualAmount,
                                                     string reason,
                                                     AllowanceReasonCodes reasonCode = AllowanceReasonCodes.Unknown)
        {
            this.SpecifiedTradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                Reason = reason,
                ReasonCode = reasonCode
            });
            return this;
        } // !AddSpecifiedTradeAllowanceCharge()


        /// <summary>
        /// As an allowance or charge on total item price, attaching it to the corresponding item.
        /// </summary>
        /// <param name="isDiscount">Marks if its an allowance (true) or charge (false). Please note that the xml will present inversed values</param>
        /// <param name="currency">Currency of the allowance or surcharge</param>
        /// <param name="basisAmount">Basis aount for the allowance or surcharge, typicalls the net amount of the item</param>
        /// <param name="actualAmount">The actual allowance or surcharge amount</param>
        /// <param name="chargePercentage">Actual allowance or surcharge charge percentage</param>
        /// <param name="reason">Reason for the allowance or surcharge</param>
        public TradeLineItem AddSpecifiedTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal? basisAmount, decimal actualAmount,
                                                     decimal? chargePercentage, string reason,
                                                     AllowanceReasonCodes reasonCode = AllowanceReasonCodes.Unknown)
        {
            this.SpecifiedTradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                ChargePercentage = chargePercentage,
                Reason = reason,
                ReasonCode = reasonCode
            });
            return this;
        } // !AddSpecifiedTradeAllowanceCharge()


        /// <summary>
        /// Returns all specified trade allowance charges for the trade line item
        /// </summary>
        /// <returns></returns>
        public IList<TradeAllowanceCharge> GetSpecifiedTradeAllowanceCharges()
        {
            return this.SpecifiedTradeAllowanceCharges;
        } // !GetSpecifiedTradeAllowanceCharges()


        /// <summary>
        /// The value given here refers to the superior line. In this way, a hierarchy tree of invoice items can be mapped.
        /// </summary>
        public TradeLineItem SetParentLineId(string parentLineId)
        {
            this.AssociatedDocument.ParentLineID = parentLineId;
            return this;
        }

        /// <summary>
        /// Sets the status code and reason code for this trade line item
        /// </summary>
        /// <param name="lineStatusCode">The status code for this line</param>
        /// <param name="lineStatusReasonCode">The reason code explaining the status</param>
        public TradeLineItem SetLineStatus(LineStatusCodes lineStatusCode, LineStatusReasonCodes lineStatusReasonCode)
        {
            this.AssociatedDocument.LineStatusCode = lineStatusCode;
            this.AssociatedDocument.LineStatusReasonCode = lineStatusReasonCode;
            return this;
        }


        /// <summary>
        /// Sets the delivery note reference information for this trade line item. BG-X-83
        /// Only available in Extended profile.
        /// </summary>
        /// <param name="deliveryNoteId">The identifier of the delivery note. BT-X-92</param>
        /// <param name="deliveryNoteDate">The date of the delivery note. BT-X-94</param>
        /// <param name="deliveryNoteReferencedLineId">The identifier of the delivery note item. BT-X-93</param>
        public TradeLineItem SetDeliveryNoteReferencedDocument(string deliveryNoteId, DateTime? deliveryNoteDate, string deliveryNoteReferencedLineId = null)
        {
            this.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            {
                ID = deliveryNoteId,
                IssueDateTime = deliveryNoteDate,
                LineID = deliveryNoteReferencedLineId
            };
            return this;
        } // !SetDeliveryNoteReferencedDocument()


        /// <summary>
        /// Adds an additional reference document with basic information
        /// </summary>
        /// <param name="id">Document identifier</param>
        /// <param name="typeCode">Type of the document</param>
        /// <param name="code">Reference type code</param>
        /// <param name="issueDateTime">Issue date and time of the document</param>
        public TradeLineItem AddAdditionalReferencedDocument(string id, AdditionalReferencedDocumentTypeCode typeCode, ReferenceTypeCodes code = ReferenceTypeCodes.Unknown, DateTime? issueDateTime = null)
        {
            this._AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ID = id,
                IssueDateTime = issueDateTime,
                TypeCode = typeCode,
                ReferenceTypeCode = code
            });
            return this;
        } // !AddAdditionalReferencedDocument()


        /// <summary>
        /// Adds a referenced product that is included in this trade line item
        /// </summary>
        /// <param name="name">Name of the included product</param>
        /// <param name="unitQuantity">Quantity of the included product</param>
        /// <param name="quantityCodes">Unit code for the quantity</param>
        public TradeLineItem AddIncludedReferencedProduct(string name, decimal? unitQuantity = null, QuantityCodes? quantityCodes = null)
        {
            this.IncludedReferencedProducts.Add(new IncludedReferencedProduct()
            {
                Name = name,
                UnitQuantity = unitQuantity,
                UnitCode = quantityCodes
            });
            return this;
        } // !AddIncludedReferencedProduct()


        /// <summary>
        /// Add an additional reference document
        /// </summary>
        /// <param name="id">Document number such as delivery note no or credit memo no</param>
        /// <param name="typeCode"></param>
        /// <param name="issueDateTime">Document Date</param>
        /// <param name="name"></param>
        /// <param name="referenceTypeCode">Type of the referenced document</param>
        /// <param name="attachmentBinaryObject"></param>
        /// <param name="filename"></param>
        /// <param name="uriID"></param>
        /// <param name="lineID"></param>
        public TradeLineItem AddAdditionalReferencedDocument(string id, AdditionalReferencedDocumentTypeCode typeCode, DateTime? issueDateTime = null,
            string name = null, ReferenceTypeCodes referenceTypeCode = ReferenceTypeCodes.Unknown, byte[] attachmentBinaryObject = null,
            string filename = null, string uriID = null, string lineID = null)
        {
            this._AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ReferenceTypeCode = referenceTypeCode,
                ID = id,
                IssueDateTime = issueDateTime,
                Name = name,
                AttachmentBinaryObject = attachmentBinaryObject,
                Filename = filename,
                TypeCode = typeCode,
                URIID = uriID,
                LineID = lineID
            });
            return this;
        } // !AddAdditionalReferencedDocument()


        /// <summary>
        /// Returns all additional referenced documents for the trade line item
        /// </summary>
        /// <returns></returns>
        public IList<AdditionalReferencedDocument> GetAdditionalReferencedDocuments()
        {
            return this._AdditionalReferencedDocuments;
        } // !GetAdditionalReferencedDocuments()


        /// <summary>
        /// Sets a purchase order line reference. BT-132
        /// Please note that XRechnung/ FacturX allows a maximum of one such reference and will only output the referenced order line id
        /// but not issuer assigned id and date
        /// </summary>
        public TradeLineItem SetOrderReferencedDocument(string orderReferencedId, DateTime? orderReferencedDate, string orderReferencedLineId)
        {
            this.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
            {
                ID = orderReferencedId,
                IssueDateTime = orderReferencedDate,
                LineID = orderReferencedLineId
            };
            return this;
        } // !SetOrderReferencedDocument()


        /// <summary>
        /// Sets the contract reference information for this trade line item. BG-X-2
        /// Only available in Extended profile.
        /// </summary>
        /// <param name="contractReferencedId">The identifier of the contract. BT-X-24</param>
        /// <param name="contractReferencedDate">The date of the contract. BT-X-26</param>
        /// <param name="contractReferencedLineId">The identifier of the contract position. BT-X-25</param>
        public TradeLineItem SetContractReferencedDocument(string contractReferencedId, DateTime? contractReferencedDate, string contractReferencedLineId = null)
        {
            this.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = contractReferencedId,
                IssueDateTime = contractReferencedDate,
                LineID = contractReferencedLineId
            };
            return this;
        } // !SetContractReferencedDocument()


        /// <summary>
        /// Adds an invoice line Buyer accounting reference with default Unknown account type
        /// </summary>
        /// <param name="AccountID">The accounting reference identifier</param>
        public TradeLineItem AddReceivableSpecifiedTradeAccountingAccount(string AccountID)
        {
            AddReceivableSpecifiedTradeAccountingAccount(AccountID, AccountingAccountTypeCodes.Unknown);
            return this;
        }


        /// <summary>
        /// Adds an invoice line Buyer accounting reference. BT-133
        /// Please note that XRechnung/ FacturX allows a maximum of one such reference
        /// </summary>
        public TradeLineItem AddReceivableSpecifiedTradeAccountingAccount(string AccountID, AccountingAccountTypeCodes AccountTypeCode)
        {
            this.ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
            {
                TradeAccountID = AccountID,
                TradeAccountTypeCode = AccountTypeCode
            });
            return this;
        }


        /// <summary>
        /// Adds a product classification
        /// </summary>
        /// <param name="className">Classification name. If you leave className empty, it will be omitted in the output</param>
        /// <param name="classCode">Identifier of the item classification (optional)</param>
        /// <param name="listID">Product classification name (optional)</param>
        /// <param name="listVersionID">Version of product classification (optional)</param>
        public void AddDesignatedProductClassification(DesignatedProductClassificationClassCodes listID, string listVersionID = null, string classCode = null, string className = null)
        {
            this.DesignatedProductClassifications.Add(new DesignatedProductClassification()
            {
                ClassCode = classCode,
                ClassName = className,
                ListID = listID,
                ListVersionID = listVersionID
            });
        } // !AddDesignatedProductClassification()


        public bool AnyDesignatedProductClassifications()
        {
            return this.DesignatedProductClassifications.Any();
        } // !AnyDesignatedProductClassifications()


        /// <summary>
        /// Returns all existing designated product classifications
        /// </summary>
        /// <returns></returns>
        public List<DesignatedProductClassification> GetDesignatedProductClassifications()
        {
            return this.DesignatedProductClassifications;
        } // !GetDesignatedProductClassifications()


        /// <summary>
        /// Returns all existing designated product classifications
        /// </summary>
        /// <returns></returns>
        public List<DesignatedProductClassification> GetDesignatedProductClassificationsByClassCode(string classCode)
        {
            return this.DesignatedProductClassifications.Where(c => c.ClassCode.Equals(classCode)).ToList();
        } // !GetDesignatedProductClassificationsByClassCode()

        /// sets the quantity, at line level, free of charge, in this trade delivery.
        /// </summary>
        /// <param name="chargeFreeQuantity">Quantity of the included charge free product</param>
        /// <param name="chargeFreeUnitCode">Unit code for the quantity</param>
        /// </summary>
        /// <returns></returns>
        public TradeLineItem SetChargeFreeQuantity(decimal chargeFreeQuantity, QuantityCodes chargeFreeUnitCode)
        {
            ChargeFreeQuantity = chargeFreeQuantity;
            ChargeFreeUnitCode = chargeFreeUnitCode;
            return this;
        } // !SetChargeFreeQuantity()

        /// sets the number of packages, at line level, in this trade delivery.
        /// </summary>
        /// <param name="packageQuantity">Quantity of the included charge free product</param>
        /// <param name="packageUnitCode">Unit code for the quantity</param>
        /// </summary>
        /// <returns></returns>
        public TradeLineItem SetPackageQuantity(decimal packageQuantity, QuantityCodes packageUnitCode)
        {
            PackageQuantity = packageQuantity;
            PackageUnitCode = packageUnitCode;
            return this;
        } // !SetPackageQuantity()

    }
}
