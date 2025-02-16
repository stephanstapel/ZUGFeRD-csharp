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
namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Adopted to ZUGFeRD 1.0, German description from ZUGFeRD specification
    /// </summary>
    public enum PaymentMeansTypeCodes
    {
        /// <summary>
        /// Unknown/ invalid value
        /// </summary>
        [EnumStringValue("")]
        Unknown = 0,

        /// <summary>
        /// Keine Zahlungsart definiert
        /// Available in: Extended
        /// </summary>
        [EnumStringValue("1")]
        NotDefined = 1,

        /// <summary>
        /// Belastung durch automatisierte Clearingstelle, Z.B. bei Abwicklung durch Zahlungsdienstleister wie Online-Bezahlsysteme
        /// </summary>
        [EnumStringValue("3")]
        AutomatedClearingHouseDebit = 3,

        /// <summary>
        /// Bar
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("10")]
        InCash = 10,

        /// <summary>
        /// Scheck
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("20")]
        Cheque = 20,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("30")]
        CreditTransfer = 30,

        /// <summary>
        /// Lastschriftübermittlung:
        /// Zahlung durch Belastung eines Geldbetrages eines
        /// Kontos zugunsten eines anderen.
        /// Überweisung international und nationale SEPA-Überweisung
        /// 
        /// Available in: Extended
        /// </summary>
        [EnumStringValue("31")]
        DebitTransfer = 31,

        /// <summary>
        /// Zahlung an Bankkonto
        /// Überweisung national, vor SEPA-Umstellung
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("42")]
        PaymentToBankAccount = 42,

        /// <summary>
        /// Bankkkarte, Kreditkarte
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("48")]
        BankCard = 48,

        /// <summary>
        /// Lastschriftverfahren
        /// 
        /// Available in: Basic, Extended
        /// /// </summary>
        [EnumStringValue("49")]
        DirectDebit = 49,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>        
        [EnumStringValue("57")]
        StandingAgreement = 57,


        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("58")]
        SEPACreditTransfer = 58,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>        
        [EnumStringValue("59")]
        SEPADirectDebit = 59,

        /// <summary>
        /// Payment will be made or has been made by an online payment service like Paypal, Stripe etc.
        /// </summary>
        [EnumStringValue("68")]
        OnlinePaymentService = 68,

        /// <summary>
        /// Ausgleich zwischen Partnern.
        /// Beträge, die zwei Partner sich gegenseitig schulden werden ausgeglichen um unnütze Zahlungen zu vermeiden.
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("97")]
        ClearingBetweenPartners = 97
    }
}
