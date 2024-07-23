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
using System.Linq.Expressions;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// For a reference see:
    /// https://docs.peppol.eu/poacc/billing/3.0/codelist/eas/
    /// </summary>
    public enum ElectronicAddressSchemeIdentifiers
    {

        /// <summary>
        /// EAN Location Code
        /// </summary>
        EanLocationCode = 0088,

        /// <summary>
        /// Leitweg-ID
        /// </summary>
        LeitwegID = 0204,

        /// <summary>
        /// Hungary VAT number
        /// </summary>      
        HungaryVatNumber = 9910,

        /// <summary>
        /// Andorra VAT number
        /// </summary>      
        AndorraVatNumber = 9922,

        /// <summary>
        /// Albania VAT number
        /// </summary>      
        AlbaniaVatNumber = 9923,

        /// <summary>
        /// Bosnia and Herzegovina VAT number
        /// </summary>      
        BosniaAndHerzegovinaVatNumber = 9924,

        /// <summary>
        /// Belgium VAT number
        /// </summary>      
        BelgiumVatNumber = 9925,

        /// <summary>
        /// Bulgaria VAT number
        /// </summary>      
        BulgariaVatNumber = 9926,

        /// <summary>
        /// Switzerland VAT number
        /// </summary>      
        SwitzerlandVatNumber = 9927,

        /// <summary>
        /// Cyprus VAT number
        /// </summary>      
        CyprusVatNumber = 9928,

        /// <summary>
        /// Czech Republic VAT number
        /// </summary>      
        CzechRepublicVatNumber = 9929,

        /// <summary>
        /// Germany VAT number
        /// </summary>
        GermanyVatNumber = 9930,

        /// <summary>
        /// Estonia VAT number
        /// </summary>
        EstoniaVatNumber = 9931,

        /// <summary>
        /// United Kingdom VAT number
        /// </summary>
        UnitedKingdomVatNumber = 9932,

        /// <summary>
        /// Greece VAT number
        /// </summary>
        GreeceVatNumber = 9933,

        /// <summary>
        /// Croatia VAT number
        /// </summary>
        CroatiaVatNumber = 9934,

        /// <summary>
        /// Ireland VAT number
        /// </summary>
        IrelandVatNumber = 9935,

        /// <summary>
        /// Liechtenstein VAT number
        /// </summary>
        LiechtensteinVatNumber = 9936,

        /// <summary>
        /// Lithuania VAT number
        /// </summary>
        LithuaniaVatNumber = 9937,

        /// <summary>
        /// Luxemburg VAT number
        /// </summary>
        LuxemburgVatNumber = 9938,

        /// <summary>
        /// Latvia VAT number
        /// </summary>
        LatviaVatNumber = 9939,

        /// <summary>
        /// Monaco VAT number
        /// </summary>
        MonacoVatNumber = 9940,

        /// <summary>
        /// Montenegro VAT number
        /// </summary>
        MontenegroVatNumber = 9941,

        /// <summary>
        /// Macedonia, of the former Yugoslav Republic VAT number
        /// </summary>
        MacedoniaVatNumber = 9942,

        /// <summary>
        /// Malta VAT number
        /// </summary>
        MaltaVatNumber = 9943,

        /// <summary>
        /// Netherlands VAT number
        /// </summary>
        NetherlandsVatNumber = 9944,

        /// <summary>
        /// Poland VAT number
        /// </summary>
        PolandVatNumber = 9945,

        /// <summary>
        /// Portugal VAT number
        /// </summary>
        PortugalVatNumber = 9946,

        /// <summary>
        /// Romania VAT number
        /// </summary>
        RomaniaVatNumber = 9947,

        /// <summary>
        /// Serbia VAT number
        /// </summary>
        SerbiaVatNumber = 9948,

        /// <summary>
        /// Slovenia VAT number
        /// </summary>
        SloveniaVatNumber = 9949,

        /// <summary>
        /// Slovakia VAT number
        /// </summary>
        SlovakiaVatNumber = 9950,

        /// <summary>
        /// San Marino VAT number
        /// </summary>
        SanMarinoVatNumber = 9951,

        /// <summary>
        /// Turkey VAT number
        /// </summary>
        TurkeyVatNumber = 9952,

        /// <summary>
        /// Holy See (Vatican City State) VAT number
        /// </summary>
        HolySeeVatNumber = 9953,

        /// <summary>
        /// Swedish VAT number
        /// </summary>
        SwedishVatNumber = 9955,

        /// <summary>
        /// French VAT number
        /// </summary>
        FrenchVatNumber = 9957,

        /// <summary>
        /// Belgian Crossroad Bank of Enterprises 
        /// </summary>
        BelgianCrossroad = 9956,

        /// <summary>
        /// German Leitweg ID
        /// </summary>
        GermanLeitwegID = 9958,

        /// <summary>
        /// Employer Identification Number (EIN, USA)
        /// </summary>
        EmployerIdentificationNumber = 9959,

        /// <summary>
        /// O.F.T.P. (ODETTE File Transfer Protocol)
        /// </summary>
        AN,

        /// <summary>
        /// X.400 address for mail text
        /// </summary>
        AQ,

        /// <summary>
        /// AS2 exchange 
        /// </summary>
        AS,

        /// <summary>
        /// File Transfer Protocol
        /// </summary>
        AU,

        /// <summary>
        /// Electronic mail
        /// </summary>
        EM
    }

    internal static class ElectronicAddressSchemeIdentifiersExtensions
    {
        public static ElectronicAddressSchemeIdentifiers? FromString(this ElectronicAddressSchemeIdentifiers _, string s)
        {
            if (Enum.TryParse(s, out ElectronicAddressSchemeIdentifiers eas))
                return eas;
            else
                return null;
        } // !FromString()

        public static string EnumToString(this ElectronicAddressSchemeIdentifiers eas)
        {
            switch (eas)
            {
                case ElectronicAddressSchemeIdentifiers.AN:
                case ElectronicAddressSchemeIdentifiers.AQ:
                case ElectronicAddressSchemeIdentifiers.AS:
                case ElectronicAddressSchemeIdentifiers.AU:
                case ElectronicAddressSchemeIdentifiers.EM:

                    return eas.ToString();
                default:
                    return ((int)eas).ToString("D4");
            }

        } // !ToString()
    }
}