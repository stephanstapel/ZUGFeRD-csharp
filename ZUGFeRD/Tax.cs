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
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Structure for holding tax information (generally applicable trade tax)
    /// </summary>
    public class Tax
    {
        /// <summary>
        /// Returns the amount of the tax (Percent * BasisAmount)
        /// 
        /// This information is not calculated anymore but must be set explicitly as of version 17.0 of the component.
        /// </summary>
        public decimal TaxAmount { get; set; }

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
        public TaxTypes? TypeCode { get; set; } = TaxTypes.VAT;

        /// <summary>
        /// The code valid for the invoiced goods sales tax category.
        /// </summary>
        public TaxCategoryCodes? CategoryCode { get; set; } = TaxCategoryCodes.S;

        /// <summary>
        /// Total amount of charges / allowances on document level
        /// </summary>
        public decimal? AllowanceChargeBasisAmount { get; set; }

        /// <summary>
        /// A monetary value used as the line total basis on which this trade related tax, levy or duty is calculated
        /// </summary>
        public decimal? LineTotalBasisAmount { get; set; }

        /// <summary>
        /// ExemptionReasonCode for no Tax
        /// </summary>
        public TaxExemptionReasonCodes? ExemptionReasonCode { get; set; }


        /// <summary>
        /// Exemption Reason Text for no Tax
        /// </summary>
        public string ExemptionReason { get; set; }

        /// <summary>
        /// Value added tax point date
        /// The date when the VAT becomes accountable for the Seller and for the Buyer in so far as that date can be determined and differs from the date of issue of the invoice, according to the VAT directive.
        /// </summary>
        public DateTime? TaxPointDate { get; set; }

        /// <summary>
        /// Value added tax point date code
        /// The code of the date when the VAT becomes accountable for the Seller and for the Buyer.
        /// </summary>
        public DateTypeCodes? DueDateTypeCode { get; set; }


        /// <summary>
        /// The tax point is usually the date goods were supplied or services completed (the 'basic tax point'). There are
        /// some variations.Please refer to Article 226 (7) of the Council Directive 2006/112/EC[2] for more information.
        /// This element is required if the Value added tax point date is different from the Invoice issue date.
        /// Both Buyer and Seller should use the Tax Point Date when provided by the Seller.The use of BT-7 and BT-8 is
        /// mutually exclusive.
        ///
        /// BT-7
        /// 
        /// Use: The date when the VAT becomes accountable for the Seller and for the Buyer in so far as that date can be
        /// determined and differs from the date of issue of the invoice, according to the VAT directive.
        /// </summary>
        /// <param name="taxPointDate">Value added tax point date</param>
        /// <param name="dueDateTypeCode">Value added tax point date code</param>
        public void SetTaxPointDate(DateTime? taxPointDate = null, DateTypeCodes? dueDateTypeCode = null)
        {
            TaxPointDate = taxPointDate;
            DueDateTypeCode = dueDateTypeCode;
        } // !SetTaxPointDate()
    }
}
