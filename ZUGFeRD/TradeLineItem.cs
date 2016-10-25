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
    public class TradeLineItem
    {
        public GlobalID GlobalID { get; set; }
        public string SellerAssignedID { get; set; }
        public string BuyerAssignedID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? UnitQuantity { get; set; }
        public decimal BilledQuantity { get; set; }
        public decimal? LineTotalAmount { get; set; }
        public TaxCategoryCodes TaxCategoryCode { get; set; }
        public decimal TaxPercent { get; set; }
        public TaxTypes TaxType { get; set; }
        public decimal NetUnitPrice { get; set; }
        public decimal GrossUnitPrice { get; set; }
        public QuantityCodes UnitCode { get; set; }
        public AssociatedDocument AssociatedDocument { get; set; }
        public BuyerOrderReferencedDocument BuyerOrderReferencedDocument { get; set; }
        public DeliveryNoteReferencedDocument DeliveryNoteReferencedDocument { get; set; }
        public ContractReferencedDocument ContractReferencedDocument { get; set; }
        public List<AdditionalReferencedDocument> AdditionalReferencedDocuments { get; set; }
        public List<TradeAllowanceCharge> TradeAllowanceCharges { get; set; }
        

        public TradeLineItem()
        {
            this.NetUnitPrice = decimal.MinValue;
            this.GrossUnitPrice = decimal.MinValue;
            this.GlobalID = new GlobalID();
            this.TradeAllowanceCharges = new List<TradeAllowanceCharge>();
            this.AdditionalReferencedDocuments = new List<AdditionalReferencedDocument>();
        }


        public void addTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal basisAmount, decimal actualAmount, string reason)
        {
            this.TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                Reason = reason
            });
        } // !addTradeAllowanceCharge()


        public void setDeliveryNoteReferencedDocument(string deliveryNoteId, DateTime? deliveryNoteDate)
        {
            this.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            {
                 ID = deliveryNoteId,
                 IssueDateTime = deliveryNoteDate
            };
        } // !setDeliveryNoteReferencedDocument()


        public void addAdditionalReferencedDocument(string id, DateTime? date = null, ReferenceTypeCodes code = ReferenceTypeCodes.Unknown)
        {
            this.AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ID = id,
                IssueDateTime = date,
                ReferenceTypeCode = code
            });
        } // !addAdditionalReferencedDocument()


        public void setOrderReferencedDocument(string orderReferencedId, DateTime? orderReferencedDate)
        {
            this.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
            {
                ID = orderReferencedId,
                IssueDateTime = orderReferencedDate
            };
        } // !setOrderReferencedDocument()


        public void setContractReferencedDocument(string contractReferencedId, DateTime? contractReferencedDate)
        {
            this.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = contractReferencedId,
                IssueDateTime = contractReferencedDate
            };
        } // !setContractReferencedDocument()
    }
}
