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
    /// Full usage of http://csharpmoney.codeplex.com/ not required here,
    /// mapping of ISO codes sufficient.
    /// 
    /// ISO 4217 currency codes
    /// </summary>
    public enum CurrencyCodes
    {
        AED = 784,
        AFN = 971,
        ALL = 8,
        AMD = 51,
        ARS = 32,
        AUD = 36,
        AZN = 944,
        BAM = 977,
        BDT = 50,
        BGN = 975,
        BHD = 48,
        BND = 96,
        BOB = 68,
        BRL = 986,
        BYR = 974,
        BZD = 84,
        CAD = 124,
        CHF = 756,
        CLP = 152,
        CNY = 156,
        COP = 170,
        CRC = 188,
        CZK = 203,
        DKK = 208,
        DOP = 214,
        DZD = 12,
        EEK = 233,
        EGP = 818,
        ETB = 230,
        EUR = 978,
        GBP = 826,
        GEL = 981,
        GTQ = 320,
        HKD = 344,
        HNL = 340,
        HRK = 191,
        HUF = 348,
        IDR = 360,
        ILS = 376,
        INR = 356,
        IQD = 368,
        IRR = 364,
        ISK = 352,
        JMD = 388,
        JOD = 400,
        JPY = 392,
        KES = 404,
        KGS = 417,
        KHR = 116,
        KRW = 410,
        KWD = 414,
        KZT = 398,
        LAK = 418,
        LBP = 422,
        LKR = 144,
        LTL = 440,
        LVL = 428,
        LYD = 434,
        MAD = 504,
        MKD = 807,
        MNT = 496,
        MOP = 446,
        MVR = 462,
        MXN = 484,
        MYR = 458,
        NIO = 558,
        NOK = 578,
        NPR = 524,
        NZD = 554,
        OMR = 512,
        PAB = 590,
        PEN = 604,
        PHP = 608,
        PKR = 586,
        PLN = 985,
        PYG = 600,
        QAR = 634,
        RON = 946,
        RSD = 941,
        RUB = 643,
        RWF = 646,
        SAR = 682,
        SEK = 752,
        SGD = 702,
        SYP = 760,
        THB = 764,
        TJS = 972,
        TND = 788,
        TRY = 949,
        TTD = 780,
        TWD = 901,
        UAH = 980,
        USD = 840,
        UYU = 858,
        UZS = 860,
        VEF = 937,
        VND = 704,
        XOF = 952,
        YER = 886,
        ZAR = 710,
        ZWL = 932,
        Unknown = 0
    }


    internal static class CurrencyCodesExtensions
    {
        public static CurrencyCodes FromString(this CurrencyCodes _c, string s)
        {
            try
            {
                return (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), s);
            }
            catch
            {
                return CurrencyCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this CurrencyCodes c)
        {
            return c.ToString("g");
        } // !ToString()
    }
}
