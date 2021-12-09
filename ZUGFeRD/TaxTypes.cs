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
    /// Please note that all profiles support VAT.
    /// 
    /// Only EXTENDED profile supports tax types other than VAT.
    /// http://www.unece.org/trade/untdid/d00a/tred/tred5153.htm
    /// </summary>
    public enum TaxTypes
    {
        /// <summary>
        /// Petroleum tax
        ///
        ///Self explanatory.
        /// </summary>
        AAA,
        /// <summary>
        /// Provisional countervailing duty cash
        ///
        ///Countervailing duty paid in cash prior to a formal
        ///finding of subsidization by Customs.
        /// </summary>
        AAB,
        /// <summary>
        /// Provisional countervailing duty bond
        ///
        ///Countervailing duty paid by posting a bond during an
        ///investigation period prior to a formal decision on
        ///subsidization by Customs.
        /// </summary>
        AAC,
        /// <summary>
        /// Anti-dumping duty
        ///
        ///Duty applied to goods ruled to have been dumped in an
        ///import market at a price lower than that in the
        ///exporter's domestic market.
        /// </summary>
        ADD,
        /// <summary>
        ///Stamp duty (Imposta di Bollo)
        ///
        ///Tax required in Italy, which may be fixed or graduated in
        ///various circumstances (e.g. VAT exempt documents or bank
        ///receipts).
        /// </summary>
        BOL,
        /// <summary>
        ///Agricultural levy
        ///
        ///Levy imposed on agricultural products where there is a
        ///difference between the selling price between trading
        ///countries.
        /// </summary>
        CAP,
        /// <summary>
        ///Car tax
        ///
        ///Self explanatory.
        /// </summary>
        CAR,
        /// <summary>
        ///Paper consortium tax (Italy)
        ///
        ///Description to be provided.
        /// </summary>
        COC,
        /// <summary>
        ///Commodity specific tax
        ///
        ///Tax related to a specified commodity, e.g. illuminants,
        ///salts.
        /// </summary>
        CST,
        /// <summary>
        ///Customs duty
        ///
        ///Duties laid down in the Customs tariff, to which goods
        ///are liable on entering or leaving the Customs territory
        ///(CCC).
        /// </summary>
        CUD,
        /// <summary>
		///Countervailing duty
        ///
        ///A duty on imported goods applied for compensate for
        ///subsidies granted to those goods in the exporting
        ///country.
        /// </summary>
        CVD,
        /// <summary>
		///Environmental tax
        ///
        ///Tax assessed for funding or assuring environmental
        ///protection or clean-up.
        /// </summary>
        ENV,
        /// <summary>
		///Excise duty
        ///
        ///Customs or fiscal authorities code to identify a specific
        ///or ad valorem levy on a specific commodity, applied
        ///either domestically or at time of importation.
        /// </summary>
        EXC,
        /// <summary>
		///Agricultural export rebate
        ///
        ///Monetary rebate given to the seller in certain
        ///circumstances when agricultural products are exported.
        /// </summary>
        EXP,
        /// <summary>
		///Federal excise tax
        ///
        ///Tax levied by the federal government on the manufacture
        ///of specific items.
        /// </summary>
        FET,
        /// <summary>
		///Free
        ///
        ///Self explanatory.
        /// </summary>
        FRE,
        /// <summary>
		///General construction tax
        ///
        ///General tax for construction.
        /// </summary>
        GCN,
        /// <summary>
		///Goods and services tax
        ///
        ///Tax levied on the final consumption of goods and services
        ///throughout the production and distribution chain.
        /// </summary>
        GST,
        /// <summary>
		///Illuminants tax
        ///
        ///Tax of illuminants.
        /// </summary>
        ILL,
        /// <summary>
		///Import tax
        ///
        ///Tax assessed on imports.
        /// </summary>
        IMP,
                /// <summary>
		///Individual tax
        ///
        ///Self explanatory.
        /// </summary>
        IND,
        /// <summary>
		///Business license fee
        ///
        ///Government assessed charge for permit to do business.
        /// </summary>
        LAC,
        /// <summary>
		///Local construction tax
        ///
        ///Local tax for construction.
        /// </summary>
        LCN,
        /// <summary>
		///Light dues payable
        ///
        ///Fee levied on a vessel to pay for port navigation lights.
        /// </summary>
        LDP,
        /// <summary>
		///Local sales taxes
        ///
        ///Assessment charges on sale of goods or services by city,
        ///borough country or other taxing authorities below
        ///state/provincial level.
        /// </summary>
        LOC,
        /// <summary>
		///Lust tax
        ///
        ///Tax imposed for clean-up of leaky underground storage
        ///tanks.
        /// </summary>
        LST,
        /// <summary>
		///Monetary compensatory amount
        ///
        ///Levy on Common Agricultural Policy (EC) goods used to
        ///compensate for fluctuating currencies between member
        ///states.
        /// </summary>
        MCA,
        /// <summary>
		///Miscellaneous cash deposit
        ///
        ///Duty paid and held on deposit, by Customs, during an
        ///investigation period prior to a final decision being made
        ///on any aspect related to imported goods (except
        ///valuation) by Customs.
        /// </summary>
        MCD,
        /// <summary>
		///Other taxes
        ///
        ///Unspecified, miscellaneous tax charges.
        /// </summary>
        OTH,
        ///Provisional duty bond
        ///
        ///Anti-dumping duty paid by posting a bond during an
        ///investigation period prior to a formal decision on
        ///dumping by Customs.
        /// </summary>
        PDB,
        /// <summary>
		///Provisional duty cash
        ///
        ///Anti-dumping duty paid in cash prior to a formal finding
        ///of dumping by Customs.
        /// </summary>
        PDC,
        /// <summary>
		///Preference duty
        ///
        ///Self explanatory.
        /// </summary>
        PRF,
        /// <summary>
		///Special construction tax
        ///
        ///Special tax for construction.
        /// </summary>
        SCN,
        /// <summary>
		///Shifted social securities
        ///
        ///Social securities share of the invoice amount to be paid
        ///directly to the social securities collector.
        /// </summary>
        SSS,
        /// <summary>
		///State/provincial sales tax
        ///
        ///All applicable sale taxes by authorities at the state or
        ///provincial level, below national level.
        /// </summary>
        SIT,
        /// <summary>
		///Suspended duty
        ///
        ///Duty suspended or deferred from payment.
        /// </summary>
        SUP,
        /// <summary>
		///Surtax
        ///
        ///A tax or duty applied on and in addition to existing
        ///duties and taxes.
        /// </summary>
        SUR,
        /// <summary>
		///Shifted wage tax
        ///
        ///Wage tax share of the invoice amount to be paid directly
        ///to the tax collector(s office).
        /// </summary>
        SWT,
        /// <summary>
		///Alcohol mark tax
        ///
        ///Self explanatory.
        /// </summary>
        TAC,
        /// <summary>
		///Total
        ///
        ///Self explanatory.
        /// </summary>
        TOT,
        /// <summary>
		///Turnover tax
        ///
        ///Tax levied on the total sales/turnover of a corporation.
        /// </summary>
        TOX,
        /// <summary>
		///Tonnage taxes
        ///
        ///Tax levied based on the vessel's net tonnage.
        /// </summary>
        TTA,
        /// <summary>
		///Valuation deposit
        ///
        ///Duty paid and held on deposit, by Customs, during an
        ///investigation period prior to a formal decision on
        ///valuation of the goods being made.
        /// </summary>
        VAD,
        /// <summary>
        /// Value added tax
        /// 
        /// A tax on domestic or imported goods applied to the value
        /// added at each stage in the production/distribution
        /// cycle.
        /// </summary>
        VAT,
        
        Unknown
    }


    internal static class TaxTypesExtensions
    {
        public static TaxTypes FromString(this TaxTypes _, string s)
        {
            try
            {
                return (TaxTypes)Enum.Parse(typeof(TaxTypes), s);
            }
            catch
            {
                return TaxTypes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this TaxTypes t)
        {
            return t.ToString("g");
        } // !ToString()
    }
}
