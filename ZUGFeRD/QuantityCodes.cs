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
    /// ISO Quantity Codes
    /// 
    /// for web reference, see e.g.
    /// http://www.robert-kuhlemann.de/iso_masseinheiten.htm
    /// </summary>
    public enum QuantityCodes
    {
        Unknown,

        /// <summary>
        /// Eins (Stück)
        /// Abkürzung: Stk.
        /// </summary>
        C62,

        /// <summary>
        /// Tag
        /// Abkürzung: Tag(e)
        /// </summary>
        DAY,

        /// <summary>
        /// Hektar
        /// Abkürzung: ha
        /// </summary>
        HAR,

        /// <summary>
        /// Stunde
        /// Abkürzung: Std.
        /// </summary>
        HUR,

        /// <summary>
        /// Kilogramm
        /// Abkürzung: kg
        /// </summary>
        KGM,

        /// <summary>
        /// Kilometer
        /// Abkürzung: km
        /// </summary>
        KTM,

        /// <summary>
        /// Kilowattstune
        /// Abkürzung: kWh
        /// </summary>
        KWH,

        /// <summary>
        /// Pauschale
        /// Abkürzung: pausch.
        /// </summary>
        LS,

        /// <summary>
        /// Liter
        /// Abkürzung: l
        /// </summary>
        LTR,

        /// <summary>
        /// Minute
        /// Abkürzung: min
        /// </summary>
        MIN,

        /// <summary>
        /// Quadratmillimeter
        /// Abkürzung: mm^2
        /// </summary>
        MMK,

        /// <summary>
        /// Millimeter
        /// Abkürzung: mm 
        /// </summary>
        MMT,

        /// <summary>
        /// Kubikmeter
        /// Abkürzung: m^2
        /// </summary>
        MTK,

        /// <summary>
        /// Kubikmeter
        /// Abkürzung: m^3 
        /// </summary>
        MTQ,

        /// <summary>
        /// Meter
        /// Abkürzung: m
        /// </summary>
        MTR,

        /// <summary>
        /// Anzahl Artikel
        /// Abkürzung: Anz.
        /// </summary>
        NAR,

        /// <summary>
        /// Anzahl Paare
        /// Abkürzung: Pr.
        /// </summary>
        NPR,

        /// <summary>
        /// Prozent
        /// Abkürzung: %
        /// </summary>
        P1,

        /// <summary>
        /// Set
        /// Abkürzung: Set(s)
        /// </summary>
        SET,

        /// <summary>
        /// Tonne (metrisch)
        /// Abkürzung:  t
        /// </summary>
        TNE,

        /// <summary>
        /// Woche
        /// Abkürzung: Woche(n)
        /// </summary>
        WEE
    }


    internal static class QuantityCodesExtensions
    {
        public static QuantityCodes FromString(this QuantityCodes _c, string s)
        {
            try
            {
                return (QuantityCodes)Enum.Parse(typeof(QuantityCodes), s);
            }
            catch
            {
                return QuantityCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this QuantityCodes c)
        {
            return c.ToString("g");
        } // !ToString()
    }
}
