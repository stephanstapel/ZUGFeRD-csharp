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
        /// </summary>
        public GlobalID GlobalID { get; set; } = new GlobalID();

        /// <summary>
        /// An identification of the item assigned by the seller.
        /// </summary>
        public string SellerAssignedID { get; set; }

        /// <summary>
        /// An identification of the item assigned by the buyer.
        /// </summary>
        public string BuyerAssignedID { get; set; }

        /// <summary>
        /// An article’s name
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
        /// Invoice line net amount including (!) trade allowance charges for the line item
        /// BT-131
        /// </summary>
        public decimal? LineTotalAmount { get; set; }

        /// <summary>
        /// Detailed information about the invoicing period
        ///
        /// Invoicing period start date
        /// </summary>
        public DateTime? BillingPeriodStart { get; set; }

        /// <summary>
        /// Detailed information about the invoicing period
        ///
        /// Invoicing period end date
        /// </summary>
        public DateTime? BillingPeriodEnd { get; set; }

        /// <summary>
        /// he code valid for the invoiced goods sales tax category
        ///
        /// BT-151
        /// </summary>
        public TaxCategoryCodes TaxCategoryCode { get; set; }

        /// <summary>
        /// Tax rate
        /// </summary>
        public decimal TaxPercent { get; set; }

        /// <summary>
        /// Tax type
        ///
        /// BT-151-0
        /// </summary>
        public TaxTypes TaxType { get; set; } = TaxTypes.VAT;

        /// <summary>
        /// net unit price of the item
        ///
        /// BT-146
        /// </summary>
        public decimal? NetUnitPrice { get; set; }

        /// <summary>
        /// gross unit price of the item
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
        /// Identifier of the invoice line item
        ///
        /// BT-126
        /// </summary>
        public AssociatedDocument AssociatedDocument { get; internal set; }

        /// <summary>
        /// Detailed information about the actual Delivery
        /// </summary>
        public DateTime? ActualDeliveryDate { get; set; }

        /// <summary>
        /// Details of the associated order
        /// </summary>
        public BuyerOrderReferencedDocument BuyerOrderReferencedDocument { get; set; }

        /// <summary>
        /// Detailed information about the corresponding delivery note
        /// </summary>
        public DeliveryNoteReferencedDocument DeliveryNoteReferencedDocument { get; set; }

        /// <summary>
        /// Details of the associated contract
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
        /// </summary>
        private List<TradeAllowanceCharge> SpecifiedTradeAllowanceCharges { get; set; } = new List<TradeAllowanceCharge>();

        /// <summary>
        /// Detailed information on the accounting reference
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

        private List<DesignatedProductClassification> DesignedProductClassifications { get; set; } = new List<DesignatedProductClassification>();


        /// <summary>
        /// Recipient of the delivered goods. This party is optional and is written in Extended profile only
        /// </summary>
        public Party ShipTo { get; set; }

        /// <summary>
        /// Detailed information on the deviating final recipient. This party is optional and only relevant for Extended profile
        /// </summary>
        public Party UltimateShipTo { get; set; }

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
        public void AddTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal? basisAmount, decimal actualAmount, string reason)
        {
            this._TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                Reason = reason
            });
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
        public void AddTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal? basisAmount, decimal actualAmount, decimal? chargePercentage, string reason)
        {
            this._TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                ChargePercentage = chargePercentage,
                Reason = reason
            });
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
        public void AddSpecifiedTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal? basisAmount, decimal actualAmount, string reason)
        {
            this.SpecifiedTradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                Reason = reason
            });
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
        public void AddSpecifiedTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal? basisAmount, decimal actualAmount, decimal? chargePercentage, string reason)
        {
            this.SpecifiedTradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                ChargePercentage = chargePercentage,
                Reason = reason
            });
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
        public void SetParentLineId(string parentLineId)
        {
            this.AssociatedDocument.ParentLineID = parentLineId;
        }

        public void SetLineStatus(LineStatusCodes lineStatusCode, LineStatusReasonCodes lineStatusReasonCode)
        {
            this.AssociatedDocument.LineStatusCode = lineStatusCode;
            this.AssociatedDocument.LineStatusReasonCode = lineStatusReasonCode;
        }

        public void SetDeliveryNoteReferencedDocument(string deliveryNoteId, DateTime? deliveryNoteDate)
        {
            this.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            {
                ID = deliveryNoteId,
                IssueDateTime = deliveryNoteDate
            };
        } // !SetDeliveryNoteReferencedDocument()


        public void AddAdditionalReferencedDocument(string id, AdditionalReferencedDocumentTypeCode typeCode, ReferenceTypeCodes code = ReferenceTypeCodes.Unknown, DateTime? issueDateTime = null)
        {
            this._AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ID = id,
                IssueDateTime = issueDateTime,
                TypeCode = typeCode,
                ReferenceTypeCode = code
            });
        } // !AddAdditionalReferencedDocument()

        public void AddIncludedReferencedProduct(string name, decimal? unitQuantity = null, QuantityCodes? quantityCodes = null)
        {
            this.IncludedReferencedProducts.Add(new IncludedReferencedProduct()
            {
                Name = name,
                UnitQuantity = unitQuantity,
                UnitCode = quantityCodes
            });
        }

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
        public void AddAdditionalReferencedDocument(string id, AdditionalReferencedDocumentTypeCode typeCode, DateTime? issueDateTime = null, string name = null, ReferenceTypeCodes referenceTypeCode = ReferenceTypeCodes.Unknown, byte[] attachmentBinaryObject = null, string filename = null)
        {
            this._AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ReferenceTypeCode = referenceTypeCode,
                ID = id,
                IssueDateTime = issueDateTime,
                Name = name,
                AttachmentBinaryObject = attachmentBinaryObject,
                Filename = filename,
                TypeCode = typeCode
            });
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
        public void SetOrderReferencedDocument(string orderReferencedId, DateTime? orderReferencedDate)
        {
            this.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
            {
                ID = orderReferencedId,
                IssueDateTime = orderReferencedDate
            };
        } // !SetOrderReferencedDocument()


        public void SetContractReferencedDocument(string contractReferencedId, DateTime? contractReferencedDate)
        {
            this.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = contractReferencedId,
                IssueDateTime = contractReferencedDate
            };
        } // !SetContractReferencedDocument()

        public void AddReceivableSpecifiedTradeAccountingAccount(string AccountID)
        {
            AddReceivableSpecifiedTradeAccountingAccount(AccountID, AccountingAccountTypeCodes.Unknown);
        }


        /// <summary>
        /// Adds an invoice line Buyer accounting reference. BT-133
        /// Please note that XRechnung/ FacturX allows a maximum of one such reference
        /// </summary>
        public void AddReceivableSpecifiedTradeAccountingAccount(string AccountID, AccountingAccountTypeCodes AccountTypeCode)
        {
            this.ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
            {
                TradeAccountID = AccountID,
                TradeAccountTypeCode = AccountTypeCode
            });
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
            this.DesignedProductClassifications.Add(new DesignatedProductClassification()
            {
                ClassCode = classCode,
                ClassName = className,
                ListID = listID,
                ListVersionID = listVersionID
            });
        } // !AddDesignatedProductClassification()


        /// <summary>
        /// Returns all existing designated product classifications
        /// </summary>
        /// <returns></returns>
        public List<DesignatedProductClassification> GetDesignatedProductClassifications()
        {
            return this.DesignedProductClassifications;
        } // !GetDesignatedProductClassifications()


        /// <summary>
        /// Returns all existing designated product classifications
        /// </summary>
        /// <returns></returns>
        public List<DesignatedProductClassification> GetDesignatedProductClassificationsByClassCode(string classCode)
        {
            return this.DesignedProductClassifications.Where(c => c.ClassCode.Equals(classCode)).ToList();
        } // !GetDesignatedProductClassificationsByClassCode()
    }
}
