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
    /// For a reference see:
    /// http://www.unece.org/trade/untdid/d00a/tred/tred1153.htm
    /// </summary>
    public enum TaxRegistrationSchemeID
    {
        /// <summary>
        /// Fiscal number
        /// 
        /// Tax payer's number. Number assigned to individual
        /// persons as well as to corporates by a public
        /// institution; this number is different from the VAT
        /// registration number.
        /// </summary>
        FC,

        /// <summary>
        /// VAT registration number
        /// 
        /// Unique number assigned by the relevant tax authority to
        /// identify a party for use in relation to Value Added Tax
        /// (VAT).
        /// </summary>
        VA,

        /// <summary>
        /// Unknown/ invalid value
        /// </summary>
        Unknown
    }


    internal static class TaxRegistrationSchemeIDExtensions
    {
        public static TaxRegistrationSchemeID FromString(this TaxRegistrationSchemeID _, string s)
        {
            try
            {
                return (TaxRegistrationSchemeID)Enum.Parse(typeof(TaxRegistrationSchemeID), s);
            }
            catch
            {
                return TaxRegistrationSchemeID.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this TaxRegistrationSchemeID codes)
        {
            return codes.ToString("g");
        } // !ToString()
    }
}
