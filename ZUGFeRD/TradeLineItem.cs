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
        /// <summary>
        /// Artikelnummer des Verkäufers
        /// </summary>
        public string SellerAssignedID { get; set; }
        /// <summary>
        /// Artikelnummer des Käufers
        /// </summary>
        public string BuyerAssignedID { get; set; }
        /// <summary>
        /// Artikelname
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Artikel Beschreibung
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Menge, enthalten
        /// </summary>
        public decimal? UnitQuantity { get; set; }
        /// <summary>
        /// Basismenge zum Artikelpreis
        /// </summary>
        public decimal BilledQuantity { get; set; }
        /// <summary>
        /// Nettobetrag der Rechnungsposition
        /// </summary>
        public decimal? LineTotalAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TaxCategoryCodes TaxCategoryCode { get; set; }
        /// <summary>
        /// Steuersatz
        /// </summary>
        public decimal TaxPercent { get; set; }
        /// <summary>
        /// Steuertyp
        /// </summary>
        public TaxTypes TaxType { get; set; }
        /// <summary>
        /// Netto Einzelpreis
        /// </summary>
        public decimal NetUnitPrice { get; set; }
        /// <summary>
        /// Brutto Einzelpreis
        /// </summary>
        public decimal GrossUnitPrice { get; set; }
        /// <summary>
        /// Einheit der Preisbasismenge
        /// </summary>
        public QuantityCodes UnitCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AssociatedDocument AssociatedDocument { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ActualDeliveryDate { get; set; }
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


        [Obsolete("This function is deprecated. Please use AddTradeAllowanceCharge() instead.")]
        public void addTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal basisAmount, decimal actualAmount, string reason)
        {
            AddTradeAllowanceCharge(isDiscount, currency, basisAmount, actualAmount, reason);
        } // !addTradeAllowanceCharge()


        public void AddTradeAllowanceCharge(bool isDiscount, CurrencyCodes currency, decimal basisAmount, decimal actualAmount, string reason)
        {
            this.TradeAllowanceCharges.Add(new TradeAllowanceCharge()
            {
                ChargeIndicator = !isDiscount,
                Currency = currency,
                ActualAmount = actualAmount,
                BasisAmount = basisAmount,
                Reason = reason
            });
        } // !AddTradeAllowanceCharge()


        [Obsolete("This function is obsolete. Please use SetDeliveryNoteReferencedDocument() instead.")]
        public void setDeliveryNoteReferencedDocument(string deliveryNoteId, DateTime? deliveryNoteDate)
        {
            SetDeliveryNoteReferencedDocument(deliveryNoteId, deliveryNoteDate);
        } // !setDeliveryNoteReferencedDocument()


        public void SetDeliveryNoteReferencedDocument(string deliveryNoteId, DateTime? deliveryNoteDate)
        {
            this.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            {
                 ID = deliveryNoteId,
                 IssueDateTime = deliveryNoteDate
            };
        } // !SetDeliveryNoteReferencedDocument()


        [Obsolete("This function is deprecated. Please use AddAdditionalReferencedDocument() instead.")]
        public void addAdditionalReferencedDocument(string id, DateTime? date = null, ReferenceTypeCodes code = ReferenceTypeCodes.Unknown)
        {
            AddAdditionalReferencedDocument(id, date, code);
        } // !addAdditionalReferencedDocument()


        public void AddAdditionalReferencedDocument(string id, DateTime? date = null, ReferenceTypeCodes code = ReferenceTypeCodes.Unknown)
        {
            this.AdditionalReferencedDocuments.Add(new AdditionalReferencedDocument()
            {
                ID = id,
                IssueDateTime = date,
                ReferenceTypeCode = code
            });
        } // !AddAdditionalReferencedDocument()


        [Obsolete("This function is deprecated. Please use SetOrderReferencedDocument() instead.")]
        public void setOrderReferencedDocument(string orderReferencedId, DateTime? orderReferencedDate)
        {
            SetOrderReferencedDocument(orderReferencedId, orderReferencedDate);
        } // !setOrderReferencedDocument()


        public void SetOrderReferencedDocument(string orderReferencedId, DateTime? orderReferencedDate)
        {
            this.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
            {
                ID = orderReferencedId,
                IssueDateTime = orderReferencedDate
            };
        } // !SetOrderReferencedDocument()


        [Obsolete("This function is obsolete. Please use SetContractReferencedDocument() instead.")]
        public void setContractReferencedDocument(string contractReferencedId, DateTime? contractReferencedDate)
        {
            SetContractReferencedDocument(contractReferencedId, contractReferencedDate);
        } // !setContractReferencedDocument()

        public void SetContractReferencedDocument(string contractReferencedId, DateTime? contractReferencedDate)
        {
            this.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = contractReferencedId,
                IssueDateTime = contractReferencedDate
            };
        } // !SetContractReferencedDocument()
    }
}
