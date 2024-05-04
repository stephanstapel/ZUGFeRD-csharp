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
using System.Xml.Linq;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Zu- und Abschlag
    /// 
    /// Beispiel:
    /// <SpecifiedTradeAllowanceCharge>
	///   <ChargeIndicator>false</ChargeIndicator>
    ///      <BasisAmount currencyID="EUR">137.30</BasisAmount>
    ///      <ActualAmount>13.73</ActualAmount>
    ///      <Reason>Sondernachlass</Reason>
    ///      <CategoryTradeTax>
    ///        <TypeCode>VAT</TypeCode>
    ///        <CategoryCode>S</CategoryCode>
    ///        <ApplicablePercent>7</ApplicablePercent>
    ///      </CategoryTradeTax>
    ///    </SpecifiedTradeAllowanceCharge>
    /// </summary>
    public class TradeAllowanceCharge : Charge
    {
        /// <summary>
        /// Switch for discount and surcharge
        /// 
        /// false: Discount
        /// true: Surcharge
        /// 
        /// In case of a discount (BG-27) the value of the ChargeIndicators has to be "false". In case of a surcharge (BG-28) the value of the ChargeIndicators has to be "true".
        /// </summary>
        public bool ChargeIndicator { get; internal set; }

        /// <summary>
        /// The reason for the surcharge or discount in written form
        /// </summary>
        public string Reason { get; internal set; }

        /// <summary>
        /// The base amount that may be used in conjunction with the percentage of the invoice line discount to calculate the amount of the invoice line discount
        /// </summary>
        public decimal? BasisAmount { get; internal set; }

        /// <summary>
        /// Currency that is used for representing BasisAmount and ActualAmount
        /// </summary>
        public CurrencyCodes Currency { get; internal set; }

        /// <summary>
        /// The amount of the discount / surcharge or discount without VAT
        /// </summary>
        public decimal ActualAmount { get; internal set; }

        /// <summary>
        /// The percentage that may be used in conjunction with the document level discount base amount, to calculate the
        /// document level discount amount.
        /// BT-101
        /// </summary>
        public decimal? ChargePercentage { get; internal set; }
    }
}
