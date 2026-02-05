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
    /// Business process codes for Croatian e-invoicing.
    /// </summary>
    public enum BusinessProcessType
    {
        /// <summary>
        /// Billing of deliveries of goods and services through orders based on contract.
        /// </summary>
        [EnumStringValue("P1")]
        OrderBasedOnContract,

        /// <summary>
        /// Periodic billing of deliveries based on contract.
        /// </summary>
        [EnumStringValue("P2")]
        PeriodicContractBilling,

        /// <summary>
        /// Billing of deliveries through unplanned orders.
        /// </summary>
        [EnumStringValue("P3")]
        UnplannedOrderBilling,

        /// <summary>
        /// Advance payment.
        /// </summary>
        [EnumStringValue("P4")]
        AdvancePayment,

        /// <summary>
        /// Payment on site.
        /// </summary>
        [EnumStringValue("P5")]
        OnSitePayment,

        /// <summary>
        /// Payment before delivery based on an order.
        /// </summary>
        [EnumStringValue("P6")]
        PreDeliveryPaymentBasedOnOrder,

        /// <summary>
        /// Invoices with a reference to dispatch advice.
        /// </summary>
        [EnumStringValue("P7")]
        InvoiceWithDispatchAdviceReference,

        /// <summary>
        /// Invoices with a reference to dispatch advice and receipt.
        /// </summary>
        [EnumStringValue("P8")]
        InvoiceWithDispatchAdviceAndReceiptReference,

        /// <summary>
        /// Credit note or negative invoicing.
        /// </summary>
        [EnumStringValue("P9")]
        CreditNoteOrNegativeInvoicing,

        /// <summary>
        /// Corrective invoicing.
        /// </summary>
        [EnumStringValue("P10")]
        CorrectiveInvoicing,

        /// <summary>
        /// Partial and final invoicing.
        /// </summary>
        [EnumStringValue("P11")]
        PartialAndFinalInvoicing,

        /// <summary>
        /// Self-billing.
        /// </summary>
        [EnumStringValue("P12")]
        SelfBilling,

        /// <summary>
        /// Customer-defined business process.
        /// </summary>
        [EnumStringValue("P99")]
        CustomerDefinedBusinessProcess
    }
}
