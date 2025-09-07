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
using System.Diagnostics;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Condition that surrounds the payment part of an invoice, describing the specific details and the due date of the invoice.
    /// </summary>
    [DebuggerDisplay("{Description} - {DueDate,nq}")]
    public class PaymentTerms
    {
        /// <summary>
        /// A textual description of the payment terms that apply to the amount due for payment (including description of possible penalties).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The date when the payment is due
        /// </summary>
        public DateTime? DueDate { get; set; } = null;

        /// <summary>
        /// Type whether it's a discount or a surcharge / interest
        /// </summary>
        public PaymentTermsType? PaymentTermsType { get; set; }

        /// <summary>
        /// Number of days within terms are valid
        /// </summary>
        public int? DueDays { get; set; }

        /// <summary>
        /// Fälligkeitsdatum im Kontext der spezifischen Zahlungsbedingung
        /// </summary>
        /// BT-X-276-0/BT-X-282-0
        public DateTime? MaturityDate { get; set; } = null; 

        /// <summary>
        /// Percentage of discount or surcharge
        /// </summary>
        public decimal? Percentage { get; set; }

        /// <summary>
        /// Base amount applied to percentage of discount or surcharge
        /// </summary>
        public decimal? BaseAmount { get; set; }

        /// <summary>
        /// The actual amount of discount or surcharge
        /// </summary>
        public decimal? ActualAmount { get; set; }
    }
}
