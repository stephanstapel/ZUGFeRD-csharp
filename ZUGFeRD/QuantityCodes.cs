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
        ///
        /// Previously, PCE was also used. This has been removed.
        /// </summary>
        C62,

		/// <summary>
		/// centigram; Zentigramm
		/// </summary>
		CGM,

		/// <summary>
        /// hundred leave; 100 Blätter
        /// A unit of count defining the number of leaves, expressed in units of one hundred leaves.
		/// </summary>
		CLF,

        /// <summary>
        /// centilitre
        /// </summary>
		CLT,

        /// <summary>
        /// square centimetre
        /// </summary>
        CMK,

        /// <summary>
        /// cubic centimetre
        /// </summary>
        CMQ,

        /// <summary>
        /// hundred pack
        /// A unit of count defining the number of hundred-packs (hundred-pack: set of one hundred items packaged together).
        /// </summary>
        CNP,

        /// <summary>
        /// Tag
        /// Abkürzung: Tag(e)
        /// </summary>
        DAY,

        /// <summary>
        /// decilitre
        /// </summary>
        DLT,

        /// <summary>
        /// square decimetre
        /// </summary>
        DMK,

        /// <summary>
        /// cubic decimetre
        /// </summary>
        DMQ,

        /// <summary>
        /// decimetre
        /// </summary>
        DMT,

        /// <summary>
        /// each: A unit of count defining the number of items regarded as separate units.
        /// </summary>
        EA,

        /// <summary>
        /// Piece: A unit of count defining the number of pieces (piece: a single item, article or exemplar).
        /// </summary>
        /// <seealso cref="QuantityCodes.C62"/>
        H87,

        /// <summary>
        /// square hectometre
        /// Abbreviation: ha
        /// </summary>
        H18,

        /// <summary>
        /// hectolitre
        /// </summary>
        HLT,

        /// <summary>
        /// Stunde
        /// Abkürzung: Std.
        /// </summary>
        HUR,

        /// <summary>
        /// person
        /// A unit of count defining the number of persons.
        /// </summary>
        IE,

        /// <summary>
        /// Kilogramm
        /// Abkürzung: kg
        /// </summary>
        KGM,

        /// <summary>
        /// Hundred
        /// </summary>
        CEN,

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
        /// Kilowattstunde
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
        /// Megawatt Stunde (1000 kW.h)
        /// Abkürzung: MWh
        /// </summary>
        MWH,

        /// <summary>
        /// Square Metre Day
        /// </summary>
        MKD,

        /// <summary>
        /// Square Metre Month
        /// </summary>
        MKM,

        /// <summary>
        /// Square Metre Week
        /// </summary>
        MKW,

        /// <summary>
        /// cubic millimetre
        /// </summary>
        MMQ,

        /// <summary>
        /// Cubic Metre Day
        /// </summary>
        MQD,

        /// <summary>
        /// cubic metre per hour
        /// </summary>
        MQH,

        /// <summary>
        /// Cubic Metre Month
        /// </summary>
        MQM,

        /// <summary>
        /// cubic metre per second
        /// </summary>
        MQS,

        /// <summary>
        /// Cubic Metre Week
        /// </summary>
        MQW,

        /// <summary>
        /// Anzahl Artikel
        /// Abkürzung: Anz.
        /// </summary>
        NAR,

        /// <summary>
        /// number of packs
        /// A unit of count defining the number of packs (pack: a collection of objects packaged together).
        /// </summary>
        NMP,

        /// <summary>
        /// Prozent
        /// Abkürzung: %
        /// </summary>
        P1,

        /// <summary>
        /// Paar
        /// Pair
        /// </summary>
        /// <remarks>
        /// A unit of count defining the number of pairs (pair: item described by two's).
        ///
        /// Previously, NPR was used to indicate pairs. This has been removed.
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
        /// Ten pack
        /// </summary>
        TP,

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
        XPP,

        /// <summary>
        /// Box
        /// </summary>
        XBX,

        /// <summary>
        /// Trommel
        /// Drum
        /// </summary>
        XDR,

        /// <summary>
        /// Kiste
        /// Crate
        /// </summary>
        XCR,

        /// <summary>
        /// Becher
        /// Cup
        /// </summary>
        XCU,

        /// <summary>
        /// Block
        /// </summary>
        XOK,

        /// <summary>
        ///  Tray
        /// </summary>
        XPU,

        /// <summary>
        /// Container
        /// Chest
        /// </summary>
        XCH,

        /// <summary>
        /// Korb
        /// Basket
        /// </summary>
        XBK,

        /// <summary>
        /// Zentner
        /// decitonne
        /// </summary>
        DTN,

        /// <summary>
        /// Portion
        /// </summary>
        /// <remarks>
        /// A quantity of allowance of food allotted to, or enough for, one person.
        /// </remarks>
        PTN,

        /// <summary>
        /// microcurie
        /// Abkürzung: µCi
        /// </summary>
        M5,

        /// <summary>
        /// microlitre
        /// Abkürzung: µl
        /// </summary>
        _4G,

        /// <summary>
        /// megabecquerel
        /// Abkürzung: MBq
        /// </summary>
        _4N,

        /// <summary>
        /// microgram
        /// Abkürzung: µg
        /// </summary>
        MC,

        /// <summary>
        /// micromole
        /// Abkürzung: µmol
        /// </summary>
        FH,

        /// <summary>
        /// becquerel
        /// Abkürzung: Bq
        /// </summary>
        BQL,

        /// <summary>
        /// curie
        /// Abkürzung: Ci
        /// </summary>
        CUR,

        /// <summary>
        /// millicurie
        /// Abkürzung: mCi
        /// </summary>
        MCU,

        /// <summary>
        /// milligram
        /// Abkürzung: mg
        /// </summary>
        MGM,

        /// <summary>
        /// millilitre
        /// Abkürzung: ml
        /// </summary>
        MLT,

        /// <summary>
        /// nanomole
        /// Abkürzung: nmol
        /// </summary>
        Z9,

        /// <summary>
        /// Packet
        /// </summary>
        XPA,

        /// <summary>
        /// Service
        /// </summary>
        /// <remarks>
        /// Services offered with no time frame specified
        /// </remarks>
        E48,

        /// <summary>
        /// Metric Carat
        /// </summary>
        /// <remarks>
        /// Einheit für die Masse von Edelsteinen. Abkürzung Kt oder ct (kein gesetzliches Einheitszeichen)
        /// </remarks>
        CTM
    }


    internal static class QuantityCodesExtensions
    {
        public static QuantityCodes FromString(this QuantityCodes _, string s)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(s) && char.IsDigit(s[0]))
                {
                    return (QuantityCodes)Enum.Parse(typeof(QuantityCodes), "_" + s);
                }
                else
                {
                    return (QuantityCodes)Enum.Parse(typeof(QuantityCodes), s);
                }
            }
            catch
            {
                // mapping of legacy unit codes
                if (s == "NPR")
                {
                    return QuantityCodes.PR;
                }
                else if (s == "PCE")
                {
                    return QuantityCodes.C62;
                }
                else if (s == "KTM")
                {
                    return QuantityCodes.KMT;
                }
                else if (s == "HAR")
                {
                    return QuantityCodes.H18;
                }
                else if (s == "D64")
                {
                    return QuantityCodes.XOK;
                }

                return QuantityCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this QuantityCodes c)
        {
            return c.ToString("g").Replace("_","");
        } // !ToString()
    }
}
