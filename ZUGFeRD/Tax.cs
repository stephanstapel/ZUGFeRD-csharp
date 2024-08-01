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

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Structure for holding tax information (generally applicable trade tax)
    /// </summary>
    public class Tax
    {
        private decimal? _taxAmount;

        /// <summary>
        /// Total tax amount, if the value is not provided, it will be automatically calculated (Percent * BasisAmount).
        /// </summary>
        public decimal TaxAmount
        {
            get => _taxAmount ?? Math.Round(0.01m * Percent * BasisAmount, 2, MidpointRounding.AwayFromZero);
            set => _taxAmount = value;
        }

        /// <summary>
        /// VAT category taxable amount
        /// </summary>
        public decimal BasisAmount { get; set; }

        /// <summary>
        /// Tax rate
        /// </summary>
        public decimal Percent { get; set; }

        /// <summary>
        /// Type of tax.
        /// 
        /// Generally, the fixed value is: "VAT"
        /// </summary>
        public TaxTypes TypeCode { get; set; } = TaxTypes.VAT;

        /// <summary>
        /// The code valid for the invoiced goods sales tax category.
        /// </summary>
        public TaxCategoryCodes? CategoryCode { get; set; } = TaxCategoryCodes.S;

        /// <summary>
        /// Total amount of charges / allowances on document level
        /// </summary>
        public decimal AllowanceChargeBasisAmount { get; set; }


        /// <summary>
        /// ExemptionReasonCode for no Tax
        /// </summary>
        public TaxExemptionReasonCodes? ExemptionReasonCode { get; set; }


        /// <summary>
        /// Exemption Reason Text for no Tax
        /// </summary>
        public string ExemptionReason { get; set; }
    }
}
