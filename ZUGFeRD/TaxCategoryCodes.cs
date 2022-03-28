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
    /// http://www.unece.org/trade/untdid/d07a/tred/tred5305.htm
    /// 
    /// Used in BT-151
    /// </summary>
    public enum TaxCategoryCodes
    {
        /// <summary>
        /// Mixed tax rate
        /// 
        /// Code specifying that the rate is based on mixed tax.
        /// </summary>
        A,

        /// <summary>
        /// Lower rate
        /// 
        /// Tax rate is lower than standard rate.
        /// </summary>
        AA,

        /// <summary>
        /// Exempt for resale
        /// 
        /// A tax category code indicating the item is tax exempt
        /// when the item is bought for future resale.
        /// </summary>
        AB,

        /// <summary>
        /// Value Added Tax (VAT) not now due for payment
        /// 
        /// A code to indicate that the Value Added Tax (VAT) amount
        /// which is due on the current invoice is to be paid on
        /// receipt of a separate VAT payment request.
        /// </summary>
        AC,

        /// <summary>
        /// Value Added Tax (VAT) due from a previous invoice
        /// 
        /// A code to indicate that the Value Added Tax (VAT) amount
        /// of a previous invoice is to be paid.
        /// </summary>
        AD,

        /// <summary>
        /// VAT Reverse charge
        /// </summary>
        AE,

        /// <summary>
        /// Transferred (VAT)
        /// 
        /// VAT not to be paid to the issuer of the invoice but
        /// directly to relevant tax authority.
        /// </summary>
        B,

        /// <summary>
        /// Duty paid by supplier
        /// Duty associated with shipment of goods is paid by the
        /// supplier; customer receives goods with duty paid.
        /// </summary>
        C,

        /// <summary>
        /// Value Added Tax (VAT) margin scheme - travel agents
        /// 
        /// Indication that the VAT margin scheme for travel agents
        /// is applied.
        /// </summary>
        D,

        /// <summary>
        /// Exempt from tax
        /// Code specifying that taxes are not applicable.
        /// </summary>
        E,

        /// <summary>
        /// Value Added Tax (VAT) margin scheme - second-hand goods
        /// 
        /// Indication that the VAT margin scheme for second-hand
        /// goods is applied.
        /// </summary>
        F,

        /// <summary>
        /// Free export item, tax not charged
        /// Code specifying that the item is free export and taxes
        /// are not charged.
        /// </summary>
        G,

        /// <summary>
        /// Higher rate
        /// Code specifying a higher rate of duty or tax or fee.
        /// </summary>
        H,

        /// <summary>
        /// Value Added Tax (VAT) margin scheme - works of art Margin scheme Works of art
        /// 
        /// Indication that the VAT margin scheme for works of art
        /// is applied.
        /// </summary>
        I,

        /// <summary>
        /// Value Added Tax (VAT) margin scheme - collector'
        /// 
        /// Indication that the VAT margin scheme for collector's
        /// items and antiques is applied items and antiques
        /// </summary>
        J,

        /// <summary>
        /// VAT exempt for EEA intra-community supply of goods and services
        /// 
        /// A tax category code indicating the item is VAT exempt
        /// due to an intra-community supply in the European
        /// Economic Area.
        /// </summary>
        K,

        /// <summary>
        /// Canary Islands general indirect tax
        /// 
        /// Impuesto General Indirecto Canario (IGIC) is an indirect
        /// tax levied on goods and services supplied in the Canary
        /// Islands (Spain) by traders and professionals, as well as
        /// on import of goods.
        /// </summary>
        L,

        /// <summary>
        /// Tax for production, services and importation in Ceuta and Melilla
        /// 
        /// Impuesto sobre la Produccion, los Servicios y la
        /// Importacion (IPSI) is an indirect municipal tax, levied
        /// on the production, processing and import of all kinds of
        /// movable tangible property, the supply of services and
        /// the transfer of immovable property located in the cities
        /// of Ceuta and Melilla.
        /// </summary>
        M,

        /// <summary>
        /// Services outside scope of tax
        /// Code specifying that taxes are not applicable to the
        /// services.
        /// </summary>
        O,

        /// <summary>
        /// Standard rate
        /// 
        /// Code specifying the standard rate.
        /// </summary>
        S,

        /// <summary>
        /// Zero rated goods
        /// 
        /// Code specifying that the goods are at a zero rate.
        /// </summary>
        Z,

        /// <summary>
        /// Default value, not specified
        /// </summary>
        Unknown
    }


    internal static class TaxCategoryCodesExtensions
    {
        public static TaxCategoryCodes FromString(this TaxCategoryCodes _, string s)
        {
            try
            {
                return (TaxCategoryCodes)Enum.Parse(typeof(TaxCategoryCodes), s);
            }
            catch
            {
                return TaxCategoryCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this TaxCategoryCodes codes)
        {
            return codes.ToString("g");
        } // !ToString()
    }
}
