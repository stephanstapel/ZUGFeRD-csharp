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
    /// Are there discounts or surcharges for an invoice.
    /// </summary>
    public enum PaymentTermsType
    {
        /// <summary>
        /// Discount indicator
        /// </summary>
        Discount,
        /// <summary>
        /// Surcharge indicator
        /// </summary>
        Surcharge
    }


    internal static class PaymentTermsTypesExtensions
    {
        public static PaymentTermsType FromString(this PaymentTermsType _, string s)
        {
            return EnumExtensions.StringToEnum<PaymentTermsType>(s);
        } // !FromString()


        public static string EnumToString(this PaymentTermsType c)
        {
            return EnumExtensions.EnumToString<PaymentTermsType>(c);
        } // !ToString()
    }
}
