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
using System.Security.Cryptography;


namespace s2industries.ZUGFeRD
{
    /// <summary>
    ///  Structure holding item information
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
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of an item
        /// 
        /// The item’s description makes it possible to describe a product and its properties more comprehensively
        /// than would be possible with just the article name.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Included amount
        /// </summary>
        public decimal? UnitQuantity { get; set; }

        /// <summary>
        /// Invoiced quantity
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
        /// </summary>
        public TaxCategoryCodes TaxCategoryCode { get; set; }

        /// <summary>
        /// Tax rate
        /// </summary>
        public decimal TaxPercent { get; set; }

        /// <summary>
        /// Tax type
        /// </summary>
        public TaxTypes TaxType { get; set; } = TaxTypes.VAT;

        /// <summary>
        /// net unit price of the item
        /// </summary>
        public decimal? NetUnitPrice { get; set; }

        /// <summary>
        /// gross unit price of the item
        /// </summary>
        public decimal? GrossUnitPrice { get; set; }

        /// <summary>
        /// Item Base Quantity Unit Code
        /// </summary>
        public QuantityCodes UnitCode { get; set; }

        /// <summary>
        /// Identifier of the invoice line item
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
        /// </summary>
        public List<AdditionalReferencedDocument> AdditionalReferencedDocuments { get; set; } = new List<AdditionalReferencedDocument>();

        /// <summary>       
        /// A group of business terms providing information about the applicable surcharges or discounts on the total amount of the invoice
        /// 
        /// Now private. Please use GetTradeAllowanceCharges() instead
        /// </summary>
        private List<TradeAllowanceCharge> _TradeAllowanceCharges { get; set; } = new List<TradeAllowanceCharge>();

        /// <summary>
        /// Detailed information on the accounting reference
        /// </summary>
        public List<ReceivableSpecifiedTradeAccountingAccount> ReceivableSpecifiedTradeAccountingAccounts { get; set; } = new List<ReceivableSpecifiedTradeAccountingAccount>();

        /// <summary>
        /// Additional product information
        /// </summary>
        public List<ApplicableProductCharacteristic> ApplicableProductCharacteristics { get; set; } = new List<ApplicableProductCharacteristic>();


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


        public void SetDeliveryNoteReferencedDocument(string deliveryNoteId, DateTime? deliveryNoteDate)
        {
            this.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            {
                ID = deliveryNoteId,
                IssueDateTime = deliveryNoteDate
            };
        } // !SetDeliveryNoteReferencedDocument()


        public void AddAdditionalReferencedDocument(string id, DateTime? date = null, ReferenceTypeCodes code = ReferenceTypeCodes.Unknown)
        {
            this.AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ID = id,
                IssueDateTime = date,
                ReferenceTypeCode = code
            });
        } // !AddAdditionalReferencedDocument()



		/// <summary>
		/// Sets a purchase order line reference. BT-132
		/// Please note that XRechnung/ FacturX allows a maximum of one such reference
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
    }
}
