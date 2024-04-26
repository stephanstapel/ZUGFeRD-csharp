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
    /// Official reference:
    /// https://unece.org/trade/uncefact/cl-recommendations
    /// (Rec 20)
    /// 
    /// See ee also
    /// http://www.robert-kuhlemann.de/iso_masseinheiten.htm
    /// 
    /// Rec 21 source:
    /// https://docs.peppol.eu/poacc/billing/3.0/codelist/UNECERec20/
    /// (starting with X, length of 3)
    /// </summary>
    public enum QuantityCodes
    {
        /// <summary>
        /// Unknown/ invalid quantity code
        /// </summary>
        Unknown = 0,

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
        /// Piece: A unit of count defining the number of pieces (piece: a single item, article or exemplar).
        /// </summary>
        /// <seealso cref="QuantityCodes.C62"/>
        H87,

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
        /// Zentimeter
        /// Abkürzung: cm
        /// </summary>
        CMT,

        /// <summary>
        /// Kilometer
        /// Abkürzung: km (Rec20r13) für XRechnung
        /// </summary>
        KMT,

        /// <summary>
        /// Kilometer
        /// Abkürzung: km (Rec20r16)
        /// </summary>
        KTM,

        /// <summary>
        /// Kilowattstune
        /// Abkürzung: kWh
        /// </summary>
        KWH,

        /// <summary>
        /// Kilowatt
        /// Abkürzung: kW
        /// </summary>
        KWT,

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
        /// Quadratmeter
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
        [Obsolete("This enum will be removed in the next major version. Please use PR instead")]
        NPR,

        /// <summary>
        /// Prozent
        /// Abkürzung: %
        /// </summary>
        P1,

        /// <summary>
        /// Stück
        /// </summary>
        [Obsolete("Does not conform to ZUGFeRD standard. Use H87 ('piece') or C62 ('one') instead")]
        PCE,

        /// <summary>
        /// Paar
        /// Pair
        /// </summary>
        /// <remarks>
        /// A unit of count defining the number of pairs (pair: item described by two's).
        /// </remarks>
        PR,

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
        WEE,

        /// <summary>
        /// Monat
        /// Abkürzung: Monat(e)
        /// </summary>
        MON,

        /// <summary>
        /// Jahr
        /// Abkürzung: Jahr(e) 
        /// </summary>
        ANN,

        /// <summary>
        /// Sekunde
        /// Abkürzung: Sekunde(n) 
        /// </summary>
        SEC,

        /// <summary>
        /// Bündel
        /// Abkürzung: Bund
        /// </summary>
        XBE,

        /// <summary>
        /// Flasche
        /// Abkürzung: Fl
        /// </summary>
        /// <remarks>
        /// Bottle, non-protected, cylindrical
        /// A narrow-necked cylindrical shaped vessel without external protective packing material
        /// </remarks>
        XBO,

        /// <summary>
        /// Karton
        /// Abkürzung: Kt
        /// </summary>
        XCT,

        /// <summary>
        /// Palette
        /// Abkürzung: Pal
        /// </summary>
        /// <remarks>
        /// Platform or open-ended box, usually made of wood, on which goods are retained for ease of mechanical handling during transport and storage.
        /// </remarks>
        XPX,

        /// <summary>
        /// Stange
        /// Abkürzung: Stg
        /// </summary>
        XRD,

        /// <summary>
        /// Tafel/Board
        /// Abkürzung: Tf
        /// </summary>
        XBD,

        /// <summary>
        /// tausend Stück
        /// Abkürzung: Tsd
        /// </summary>
        /// <remarks>
        /// A unit of count defining the number of pieces in multiples of 1000 (piece: a single item, article or exemplar).
        /// </remarks>
        T3,

        /// <summary>
        /// Verpackung
        /// </summary>
        /// <remarks>
        /// Standard packaging unit
        /// </remarks>
        XPK,

        /// <summary>
        /// Ar
        /// Abkürzung: a
        /// </summary>
        /// <remarks>
        /// 100 m^2
        /// </remarks>
        FF,

        /// <summary>
        /// Rolle
        /// </summary>
        XRO,

        /// <summary>
        /// Dose
        /// </summary>
        XTN,

        /// <summary>
        /// Kanister
        /// </summary>
        XCI,

        /// <summary>
        /// Tube
        /// </summary>
        XTU,

        /// <summary>
        /// Beutel
        /// </summary>
        XBG,

        /// <summary>
        /// (Papier) Bogen
        /// </summary>
        XST,

        /// <summary>
        /// Sack
        /// </summary>
        XSA,

        /// <summary>
        /// Fass
        /// </summary>
        XBA,

        /// <summary>
        /// Eimer
        /// </summary>
        XBJ,

        /// <summary>
        /// Gramm
        /// </summary>
        GRM,

        /// <summary>
        /// Kit
        /// </summary>
        KT,

        /// <summary>
        /// Piece
        /// A loose or unpacked article.
        /// </summary>
        XPP
    }


    internal static class QuantityCodesExtensions
    {
        public static QuantityCodes FromString(this QuantityCodes _, string s)
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
