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
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Specification of the invoice currency, local currency and exchange rate
    /// </summary>
    public class TradeCurrencyExchange
    {
        /// <summary>
        /// Invoice currency
        /// </summary>
        public CurrencyCodes SourceCurrency { get; private set; }

        /// <summary>
        /// Local currency
        /// </summary>
        public CurrencyCodes TargetCurrency { get; private set; }

        /// <summary>
        /// Exchange rate
        /// </summary>
        public decimal ConversionRate { get; private set; }

        /// <summary>
        /// Exchange rate date
        /// </summary>
        public DateTime? ConversionRateTimestamp { get; private set; }


        /// <summary>
        /// Constructor without exchange rate date
        /// </summary>
        /// <param name="sourceCurrency">Invoice currency</param>
        /// <param name="targetCurrency">Local currency</param>
        /// <param name="conversionRate">Exchange rate</param>
        public TradeCurrencyExchange(CurrencyCodes sourceCurrency, CurrencyCodes targetCurrency, decimal conversionRate)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            ConversionRate = conversionRate;
        } // !TradeCurrencyExchange()


        /// <summary>
        /// Constructor with exchange rate date
        /// </summary>
        /// <param name="sourceCurrency">Invoice currency</param>
        /// <param name="targetCurrency">Local currency</param>
        /// <param name="conversionRate">Exchange rate</param>
        /// <param name="conversionRateTimestamp">Exchange rate date</param>
        public TradeCurrencyExchange(CurrencyCodes sourceCurrency, CurrencyCodes targetCurrency, decimal conversionRate, DateTime conversionRateTimestamp)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            ConversionRate = conversionRate;
            ConversionRateTimestamp = conversionRateTimestamp;
        } // !TradeCurrencyExchange()
    }
}
