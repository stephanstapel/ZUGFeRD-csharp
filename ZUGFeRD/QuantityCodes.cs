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

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// ISO Quantity Codes
    ///
    /// Official reference:
    /// https://unece.org/trade/uncefact/cl-recommendations
    /// (Rec 20)
    ///
    /// See also
    /// http://www.robert-kuhlemann.de/iso_masseinheiten.htm
    ///
    /// Rec 21 source:
    /// https://docs.peppol.eu/poacc/billing/3.0/codelist/UNECERec20/
    /// (starting with X, length of 3)
    ///
    /// Codes updated from above PEPPOL list: 2025-01-06
    /// </summary>
    public enum QuantityCodes
    {
        #region Codes
        /// <summary>
        /// Unknown/ invalid quantity code
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// group
        /// A unit of count defining the number of groups (group: set of items classified together).
        /// </summary>
        _10,

        /// <summary>
        /// outfit
        /// A unit of count defining the number of outfits (outfit: a complete set of equipment / materials / objects used for a specific purpose).
        /// </summary>
        _11,

        /// <summary>
        /// ration
        /// A unit of count defining the number of rations (ration: a single portion of provisions).
        /// </summary>
        _13,

        /// <summary>
        /// shot
        /// A unit of liquid measure, especially related to spirits.
        /// </summary>
        _14,

        /// <summary>
        /// stick, military
        /// A unit of count defining the number of military sticks (military stick: bombs or paratroops released in rapid succession from an aircraft).
        /// </summary>
        _15,

        /// <summary>
        /// twenty foot container
        /// A unit of count defining the number of shipping containers that measure 20 foot in length.
        /// </summary>
        _20,

        /// <summary>
        /// forty foot container
        /// A unit of count defining the number of shipping containers that measure 40 foot in length.
        /// </summary>
        _21,

        /// <summary>
        /// decilitre per gram
        /// </summary>
        _22,

        /// <summary>
        /// gram per cubic centimetre
        /// </summary>
        _23,

        /// <summary>
        /// theoretical pound
        /// A unit of mass defining the expected mass of material expressed as the number of pounds.
        /// </summary>
        _24,

        /// <summary>
        /// gram per square centimetre
        /// </summary>
        _25,

        /// <summary>
        /// theoretical ton
        /// A unit of mass defining the expected mass of material, expressed as the number of tons.
        /// </summary>
        _27,

        /// <summary>
        /// kilogram per square metre
        /// </summary>
        _28,

        /// <summary>
        /// kilopascal square metre per gram
        /// </summary>
        _33,

        /// <summary>
        /// kilopascal per millimetre
        /// </summary>
        _34,

        /// <summary>
        /// millilitre per square centimetre second
        /// </summary>
        _35,

        /// <summary>
        /// ounce per square foot
        /// </summary>
        _37,

        /// <summary>
        /// ounce per square foot per 0,01inch
        /// </summary>
        _38,

        /// <summary>
        /// millilitre per second
        /// </summary>
        _40,

        /// <summary>
        /// millilitre per minute
        /// </summary>
        _41,

        /// <summary>
        /// sitas
        /// A unit of area for tin plate equal to a surface area of 100 square metres.
        /// </summary>
        _56,

        /// <summary>
        /// mesh
        /// A unit of count defining the number of strands per inch as a measure of the fineness of a woven product.
        /// </summary>
        _57,

        /// <summary>
        /// net kilogram
        /// A unit of mass defining the total number of kilograms after deductions.
        /// </summary>
        _58,

        /// <summary>
        /// part per million
        /// A unit of proportion equal to 10⁻⁶.
        /// </summary>
        _59,

        /// <summary>
        /// percent weight
        /// A unit of proportion equal to 10⁻².
        /// </summary>
        _60,

        /// <summary>
        /// part per billion (US)
        /// A unit of proportion equal to 10⁻⁹.
        /// </summary>
        _61,

        /// <summary>
        /// millipascal
        /// </summary>
        _74,

        /// <summary>
        /// milli-inch
        /// </summary>
        _77,

        /// <summary>
        /// pound per square inch absolute
        /// </summary>
        _80,

        /// <summary>
        /// henry
        /// </summary>
        _81,

        /// <summary>
        /// foot pound-force
        /// </summary>
        _85,

        /// <summary>
        /// pound per cubic foot
        /// </summary>
        _87,

        /// <summary>
        /// poise
        /// </summary>
        _89,

        /// <summary>
        /// stokes
        /// </summary>
        _91,

        /// <summary>
        /// fixed rate
        /// A unit of quantity expressed as a predetermined or set rate for usage of a facility or service.
        /// </summary>
        _1I,

        /// <summary>
        /// radian per second
        /// Refer ISO/TC12 SI Guide
        /// </summary>
        _2A,

        /// <summary>
        /// radian per second squared
        /// Refer ISO/TC12 SI Guide
        /// </summary>
        _2B,

        /// <summary>
        /// roentgen
        /// </summary>
        _2C,

        /// <summary>
        /// volt AC
        /// A unit of electric potential in relation to alternating current (AC).
        /// </summary>
        _2G,

        /// <summary>
        /// volt DC
        /// A unit of electric potential in relation to direct current (DC).
        /// </summary>
        _2H,

        /// <summary>
        /// British thermal unit (international table) per hour
        /// </summary>
        _2I,

        /// <summary>
        /// cubic centimetre per second
        /// </summary>
        _2J,

        /// <summary>
        /// cubic foot per hour
        /// </summary>
        _2K,

        /// <summary>
        /// cubic foot per minute
        /// </summary>
        _2L,

        /// <summary>
        /// centimetre per second
        /// </summary>
        _2M,

        /// <summary>
        /// decibel
        /// </summary>
        _2N,

        /// <summary>
        /// kilobyte
        /// A unit of information equal to 10³ (1000) bytes.
        /// </summary>
        _2P,

        /// <summary>
        /// kilobecquerel
        /// </summary>
        _2Q,

        /// <summary>
        /// kilocurie
        /// </summary>
        _2R,

        /// <summary>
        /// megagram
        /// </summary>
        _2U,

        /// <summary>
        /// metre per minute
        /// </summary>
        _2X,

        /// <summary>
        /// milliroentgen
        /// </summary>
        _2Y,

        /// <summary>
        /// millivolt
        /// </summary>
        _2Z,

        /// <summary>
        /// megajoule
        /// </summary>
        _3B,

        /// <summary>
        /// manmonth
        /// A unit of count defining the number of months for a person or persons to perform an undertaking.
        /// </summary>
        _3C,

        /// <summary>
        /// centistokes
        /// </summary>
        _4C,

        /// <summary>
        /// microlitre
        /// </summary>
        _4G,

        /// <summary>
        /// micrometre (micron)
        /// </summary>
        _4H,

        /// <summary>
        /// milliampere
        /// </summary>
        _4K,

        /// <summary>
        /// megabyte
        /// A unit of information equal to 10⁶ (1000000) bytes.
        /// </summary>
        _4L,

        /// <summary>
        /// milligram per hour
        /// </summary>
        _4M,

        /// <summary>
        /// megabecquerel
        /// </summary>
        _4N,

        /// <summary>
        /// microfarad
        /// </summary>
        _4O,

        /// <summary>
        /// newton per metre
        /// </summary>
        _4P,

        /// <summary>
        /// ounce inch
        /// </summary>
        _4Q,

        /// <summary>
        /// ounce foot
        /// </summary>
        _4R,

        /// <summary>
        /// picofarad
        /// </summary>
        _4T,

        /// <summary>
        /// pound per hour
        /// </summary>
        _4U,

        /// <summary>
        /// ton (US) per hour
        /// </summary>
        _4W,

        /// <summary>
        /// kilolitre per hour
        /// </summary>
        _4X,

        /// <summary>
        /// barrel (US) per minute
        /// </summary>
        _5A,

        /// <summary>
        /// batch
        /// A unit of count defining the number of batches (batch: quantity of material produced in one operation or number of animals or persons coming at once).
        /// </summary>
        _5B,

        /// <summary>
        /// MMSCF/day
        /// A unit of volume equal to one million (1000000) cubic feet of gas per day.
        /// </summary>
        _5E,

        /// <summary>
        /// hydraulic horse power
        /// A unit of power defining the hydraulic horse power delivered by a fluid pump depending on the viscosity of the fluid.
        /// </summary>
        _5J,

        /// <summary>
        /// ampere square metre per joule second
        /// </summary>
        A10,

        /// <summary>
        /// angstrom
        /// </summary>
        A11,

        /// <summary>
        /// astronomical unit
        /// </summary>
        A12,

        /// <summary>
        /// attojoule
        /// </summary>
        A13,

        /// <summary>
        /// barn
        /// </summary>
        A14,

        /// <summary>
        /// barn per electronvolt
        /// </summary>
        A15,

        /// <summary>
        /// barn per steradian electronvolt
        /// </summary>
        A16,

        /// <summary>
        /// barn per steradian
        /// </summary>
        A17,

        /// <summary>
        /// becquerel per kilogram
        /// </summary>
        A18,

        /// <summary>
        /// becquerel per cubic metre
        /// </summary>
        A19,

        /// <summary>
        /// ampere per centimetre
        /// </summary>
        A2,

        /// <summary>
        /// British thermal unit (international table) per second square foot degree Rankine
        /// </summary>
        A20,

        /// <summary>
        /// British thermal unit (international table) per pound degree Rankine
        /// </summary>
        A21,

        /// <summary>
        /// British thermal unit (international table) per second foot degree Rankine
        /// </summary>
        A22,

        /// <summary>
        /// British thermal unit (international table) per hour square foot degree Rankine
        /// </summary>
        A23,

        /// <summary>
        /// candela per square metre
        /// </summary>
        A24,

        /// <summary>
        /// coulomb metre
        /// </summary>
        A26,

        /// <summary>
        /// coulomb metre squared per volt
        /// </summary>
        A27,

        /// <summary>
        /// coulomb per cubic centimetre
        /// </summary>
        A28,

        /// <summary>
        /// coulomb per cubic metre
        /// </summary>
        A29,

        /// <summary>
        /// ampere per millimetre
        /// </summary>
        A3,

        /// <summary>
        /// coulomb per cubic millimetre
        /// </summary>
        A30,

        /// <summary>
        /// coulomb per kilogram second
        /// </summary>
        A31,

        /// <summary>
        /// coulomb per mole
        /// </summary>
        A32,

        /// <summary>
        /// coulomb per square centimetre
        /// </summary>
        A33,

        /// <summary>
        /// coulomb per square metre
        /// </summary>
        A34,

        /// <summary>
        /// coulomb per square millimetre
        /// </summary>
        A35,

        /// <summary>
        /// cubic centimetre per mole
        /// </summary>
        A36,

        /// <summary>
        /// cubic decimetre per mole
        /// </summary>
        A37,

        /// <summary>
        /// cubic metre per coulomb
        /// </summary>
        A38,

        /// <summary>
        /// cubic metre per kilogram
        /// </summary>
        A39,

        /// <summary>
        /// ampere per square centimetre
        /// </summary>
        A4,

        /// <summary>
        /// cubic metre per mole
        /// </summary>
        A40,

        /// <summary>
        /// ampere per square metre
        /// </summary>
        A41,

        /// <summary>
        /// curie per kilogram
        /// </summary>
        A42,

        /// <summary>
        /// deadweight tonnage
        /// A unit of mass defining the difference between the weight of a ship when completely empty and its weight when completely loaded, expressed as the number of tons.
        /// </summary>
        A43,

        /// <summary>
        /// decalitre
        /// </summary>
        A44,

        /// <summary>
        /// decametre
        /// </summary>
        A45,

        /// <summary>
        /// decitex
        /// A unit of yarn density. One decitex equals a mass of 1 gram per 10 kilometres of length.
        /// </summary>
        A47,

        /// <summary>
        /// degree Rankine
        /// Refer ISO 80000-5 (Quantities and units — Part 5: Thermodynamics)
        /// </summary>
        A48,

        /// <summary>
        /// denier
        /// A unit of yarn density. One denier equals a mass of 1 gram per 9 kilometres of length.
        /// </summary>
        A49,

        /// <summary>
        /// ampere square metre
        /// </summary>
        A5,

        /// <summary>
        /// electronvolt
        /// </summary>
        A53,

        /// <summary>
        /// electronvolt per metre
        /// </summary>
        A54,

        /// <summary>
        /// electronvolt square metre
        /// </summary>
        A55,

        /// <summary>
        /// electronvolt square metre per kilogram
        /// </summary>
        A56,

        /// <summary>
        /// 8-part cloud cover
        /// A unit of count defining the number of eighth-parts as a measure of the celestial dome cloud coverage. Synonym: OKTA , OCTA
        /// </summary>
        A59,

        /// <summary>
        /// ampere per square metre kelvin squared
        /// </summary>
        A6,

        /// <summary>
        /// exajoule
        /// </summary>
        A68,

        /// <summary>
        /// farad per metre
        /// </summary>
        A69,

        /// <summary>
        /// ampere per square millimetre
        /// </summary>
        A7,

        /// <summary>
        /// femtojoule
        /// </summary>
        A70,

        /// <summary>
        /// femtometre
        /// </summary>
        A71,

        /// <summary>
        /// foot per second squared
        /// </summary>
        A73,

        /// <summary>
        /// foot pound-force per second
        /// </summary>
        A74,

        /// <summary>
        /// freight ton
        /// A unit of information typically used for billing purposes, defined as either the number of metric tons or the number of cubic metres, whichever is the larger.
        /// </summary>
        A75,

        /// <summary>
        /// gal
        /// </summary>
        A76,

        /// <summary>
        /// ampere second
        /// </summary>
        A8,

        /// <summary>
        /// gigacoulomb per cubic metre
        /// </summary>
        A84,

        /// <summary>
        /// gigaelectronvolt
        /// </summary>
        A85,

        /// <summary>
        /// gigahertz
        /// </summary>
        A86,

        /// <summary>
        /// gigaohm
        /// </summary>
        A87,

        /// <summary>
        /// gigaohm metre
        /// </summary>
        A88,

        /// <summary>
        /// gigapascal
        /// </summary>
        A89,

        /// <summary>
        /// rate
        /// A unit of quantity expressed as a rate for usage of a facility or service.
        /// </summary>
        A9,

        /// <summary>
        /// gigawatt
        /// </summary>
        A90,

        /// <summary>
        /// gon
        /// Synonym: grade
        /// </summary>
        A91,

        /// <summary>
        /// gram per cubic metre
        /// </summary>
        A93,

        /// <summary>
        /// gram per mole
        /// </summary>
        A94,

        /// <summary>
        /// gray
        /// </summary>
        A95,

        /// <summary>
        /// gray per second
        /// </summary>
        A96,

        /// <summary>
        /// hectopascal
        /// </summary>
        A97,

        /// <summary>
        /// henry per metre
        /// </summary>
        A98,

        /// <summary>
        /// bit
        /// A unit of information equal to one binary digit.
        /// </summary>
        A99,

        /// <summary>
        /// ball
        /// A unit of count defining the number of balls (ball: object formed in the shape of sphere).
        /// </summary>
        AA,

        /// <summary>
        /// bulk pack
        /// A unit of count defining the number of items per bulk pack.
        /// </summary>
        AB,

        /// <summary>
        /// acre
        /// </summary>
        ACR,

        /// <summary>
        /// activity
        /// A unit of count defining the number of activities (activity: a unit of work or action).
        /// </summary>
        ACT,

        /// <summary>
        /// byte
        /// A unit of information equal to 8 bits.
        /// </summary>
        AD,

        /// <summary>
        /// ampere per metre
        /// </summary>
        AE,

        /// <summary>
        /// additional minute
        /// A unit of time defining the number of minutes in addition to the referenced minutes.
        /// </summary>
        AH,

        /// <summary>
        /// average minute per call
        /// A unit of count defining the number of minutes for the average interval of a call.
        /// </summary>
        AI,

        /// <summary>
        /// fathom
        /// </summary>
        AK,

        /// <summary>
        /// access line
        /// A unit of count defining the number of telephone access lines.
        /// </summary>
        AL,

        /// <summary>
        /// ampere hour
        /// A unit of electric charge defining the amount of charge accumulated by a steady flow of one ampere for one hour.
        /// </summary>
        AMH,

        /// <summary>
        /// ampere
        /// </summary>
        AMP,

        /// <summary>
        /// year
        /// Unit of time equal to 365,25 days. Synonym: Julian year
        /// </summary>
        ANN,

        /// <summary>
        /// troy ounce or apothecary ounce
        /// </summary>
        APZ,

        /// <summary>
        /// anti-hemophilic factor (AHF) unit
        /// A unit of measure for blood potency (US).
        /// </summary>
        AQ,

        /// <summary>
        /// assortment
        /// A unit of count defining the number of assortments (assortment: set of items grouped in a mixed collection).
        /// </summary>
        AS,

        /// <summary>
        /// alcoholic strength by mass
        /// A unit of mass defining the alcoholic strength of a liquid.
        /// </summary>
        ASM,

        /// <summary>
        /// alcoholic strength by volume
        /// A unit of volume defining the alcoholic strength of a liquid (e.g. spirit, wine, beer, etc), often at a specific temperature.
        /// </summary>
        ASU,

        /// <summary>
        /// standard atmosphere
        /// </summary>
        ATM,

        /// <summary>
        /// american wire gauge
        /// A unit of distance used for measuring the diameter of small tubes or wires such as the outer diameter of hypotermic or suture needles.
        /// </summary>
        AWG,

        /// <summary>
        /// assembly
        /// A unit of count defining the number of assemblies (assembly: items that consist of component parts).
        /// </summary>
        AY,

        /// <summary>
        /// British thermal unit (international table) per pound
        /// </summary>
        AZ,

        /// <summary>
        /// barrel (US) per day
        /// </summary>
        B1,

        /// <summary>
        /// bit per second
        /// A unit of information equal to one binary digit per second.
        /// </summary>
        B10,

        /// <summary>
        /// joule per kilogram kelvin
        /// </summary>
        B11,

        /// <summary>
        /// joule per metre
        /// </summary>
        B12,

        /// <summary>
        /// joule per square metre
        /// Synonym: joule per metre squared
        /// </summary>
        B13,

        /// <summary>
        /// joule per metre to the fourth power
        /// </summary>
        B14,

        /// <summary>
        /// joule per mole
        /// </summary>
        B15,

        /// <summary>
        /// joule per mole kelvin
        /// </summary>
        B16,

        /// <summary>
        /// credit
        /// A unit of count defining the number of entries made to the credit side of an account.
        /// </summary>
        B17,

        /// <summary>
        /// joule second
        /// </summary>
        B18,

        /// <summary>
        /// digit
        /// A unit of information defining the quantity of numerals used to form a number.
        /// </summary>
        B19,

        /// <summary>
        /// joule square metre per kilogram
        /// </summary>
        B20,

        /// <summary>
        /// kelvin per watt
        /// </summary>
        B21,

        /// <summary>
        /// kiloampere
        /// </summary>
        B22,

        /// <summary>
        /// kiloampere per square metre
        /// </summary>
        B23,

        /// <summary>
        /// kiloampere per metre
        /// </summary>
        B24,

        /// <summary>
        /// kilobecquerel per kilogram
        /// </summary>
        B25,

        /// <summary>
        /// kilocoulomb
        /// </summary>
        B26,

        /// <summary>
        /// kilocoulomb per cubic metre
        /// </summary>
        B27,

        /// <summary>
        /// kilocoulomb per square metre
        /// </summary>
        B28,

        /// <summary>
        /// kiloelectronvolt
        /// </summary>
        B29,

        /// <summary>
        /// batting pound
        /// A unit of mass defining the number of pounds of wadded fibre.
        /// </summary>
        B3,

        /// <summary>
        /// gibibit
        /// A unit of information equal to 2³⁰ bits (binary digits).
        /// </summary>
        B30,

        /// <summary>
        /// kilogram metre per second
        /// </summary>
        B31,

        /// <summary>
        /// kilogram metre squared
        /// </summary>
        B32,

        /// <summary>
        /// kilogram metre squared per second
        /// </summary>
        B33,

        /// <summary>
        /// kilogram per cubic decimetre
        /// </summary>
        B34,

        /// <summary>
        /// kilogram per litre
        /// </summary>
        B35,

        /// <summary>
        /// barrel, imperial
        /// A unit of volume used to measure beer. One beer barrel equals 36 imperial gallons.
        /// </summary>
        B4,

        /// <summary>
        /// kilojoule per kelvin
        /// </summary>
        B41,

        /// <summary>
        /// kilojoule per kilogram
        /// </summary>
        B42,

        /// <summary>
        /// kilojoule per kilogram kelvin
        /// </summary>
        B43,

        /// <summary>
        /// kilojoule per mole
        /// </summary>
        B44,

        /// <summary>
        /// kilomole
        /// </summary>
        B45,

        /// <summary>
        /// kilomole per cubic metre
        /// </summary>
        B46,

        /// <summary>
        /// kilonewton
        /// </summary>
        B47,

        /// <summary>
        /// kilonewton metre
        /// </summary>
        B48,

        /// <summary>
        /// kiloohm
        /// </summary>
        B49,

        /// <summary>
        /// kiloohm metre
        /// </summary>
        B50,

        /// <summary>
        /// kilosecond
        /// </summary>
        B52,

        /// <summary>
        /// kilosiemens
        /// </summary>
        B53,

        /// <summary>
        /// kilosiemens per metre
        /// </summary>
        B54,

        /// <summary>
        /// kilovolt per metre
        /// </summary>
        B55,

        /// <summary>
        /// kiloweber per metre
        /// </summary>
        B56,

        /// <summary>
        /// light year
        /// A unit of length defining the distance that light travels in a vacuum in one year.
        /// </summary>
        B57,

        /// <summary>
        /// litre per mole
        /// </summary>
        B58,

        /// <summary>
        /// lumen hour
        /// </summary>
        B59,

        /// <summary>
        /// lumen per square metre
        /// </summary>
        B60,

        /// <summary>
        /// lumen per watt
        /// </summary>
        B61,

        /// <summary>
        /// lumen second
        /// </summary>
        B62,

        /// <summary>
        /// lux hour
        /// </summary>
        B63,

        /// <summary>
        /// lux second
        /// </summary>
        B64,

        /// <summary>
        /// megaampere per square metre
        /// </summary>
        B66,

        /// <summary>
        /// megabecquerel per kilogram
        /// </summary>
        B67,

        /// <summary>
        /// gigabit
        /// A unit of information equal to 10⁹ bits (binary digits).
        /// </summary>
        B68,

        /// <summary>
        /// megacoulomb per cubic metre
        /// </summary>
        B69,

        /// <summary>
        /// cycle
        /// A unit of count defining the number of cycles (cycle: a recurrent period of definite duration).
        /// </summary>
        B7,

        /// <summary>
        /// megacoulomb per square metre
        /// </summary>
        B70,

        /// <summary>
        /// megaelectronvolt
        /// </summary>
        B71,

        /// <summary>
        /// megagram per cubic metre
        /// </summary>
        B72,

        /// <summary>
        /// meganewton
        /// </summary>
        B73,

        /// <summary>
        /// meganewton metre
        /// </summary>
        B74,

        /// <summary>
        /// megaohm
        /// </summary>
        B75,

        /// <summary>
        /// megaohm metre
        /// </summary>
        B76,

        /// <summary>
        /// megasiemens per metre
        /// </summary>
        B77,

        /// <summary>
        /// megavolt
        /// </summary>
        B78,

        /// <summary>
        /// megavolt per metre
        /// </summary>
        B79,

        /// <summary>
        /// joule per cubic metre
        /// </summary>
        B8,

        /// <summary>
        /// gigabit per second
        /// A unit of information equal to 10⁹ bits (binary digits) per second.
        /// </summary>
        B80,

        /// <summary>
        /// reciprocal metre squared reciprocal second
        /// </summary>
        B81,

        /// <summary>
        /// inch per linear foot
        /// A unit of length defining the number of inches per linear foot.
        /// </summary>
        B82,

        /// <summary>
        /// metre to the fourth power
        /// </summary>
        B83,

        /// <summary>
        /// microampere
        /// </summary>
        B84,

        /// <summary>
        /// microbar
        /// </summary>
        B85,

        /// <summary>
        /// microcoulomb
        /// </summary>
        B86,

        /// <summary>
        /// microcoulomb per cubic metre
        /// </summary>
        B87,

        /// <summary>
        /// microcoulomb per square metre
        /// </summary>
        B88,

        /// <summary>
        /// microfarad per metre
        /// </summary>
        B89,

        /// <summary>
        /// microhenry
        /// </summary>
        B90,

        /// <summary>
        /// microhenry per metre
        /// </summary>
        B91,

        /// <summary>
        /// micronewton
        /// </summary>
        B92,

        /// <summary>
        /// micronewton metre
        /// </summary>
        B93,

        /// <summary>
        /// microohm
        /// </summary>
        B94,

        /// <summary>
        /// microohm metre
        /// </summary>
        B95,

        /// <summary>
        /// micropascal
        /// </summary>
        B96,

        /// <summary>
        /// microradian
        /// </summary>
        B97,

        /// <summary>
        /// microsecond
        /// </summary>
        B98,

        /// <summary>
        /// microsiemens
        /// </summary>
        B99,

        /// <summary>
        /// bar [unit of pressure]
        /// </summary>
        BAR,

        /// <summary>
        /// base box
        /// A unit of area of 112 sheets of tin mil products (tin plate, tin free steel or black plate) 14 by 20 inches, or 31,360 square inches.
        /// </summary>
        BB,

        /// <summary>
        /// board foot
        /// A unit of volume defining the number of cords (cord: a stack of firewood of 128 cubic feet).
        /// </summary>
        BFT,

        /// <summary>
        /// brake horse power
        /// </summary>
        BHP,

        /// <summary>
        /// billion (EUR)
        /// Synonym: trillion (US)
        /// </summary>
        BIL,

        /// <summary>
        /// dry barrel (US)
        /// </summary>
        BLD,

        /// <summary>
        /// barrel (US)
        /// </summary>
        BLL,

        /// <summary>
        /// hundred board foot
        /// A unit of volume equal to one hundred board foot.
        /// </summary>
        BP,

        /// <summary>
        /// beats per minute
        /// The number of beats per minute.
        /// </summary>
        BPM,

        /// <summary>
        /// becquerel
        /// </summary>
        BQL,

        /// <summary>
        /// British thermal unit (international table)
        /// </summary>
        BTU,

        /// <summary>
        /// bushel (US)
        /// </summary>
        BUA,

        /// <summary>
        /// bushel (UK)
        /// </summary>
        BUI,

        /// <summary>
        /// call
        /// A unit of count defining the number of calls (call: communication session or visitation).
        /// </summary>
        C0,

        /// <summary>
        /// millifarad
        /// </summary>
        C10,

        /// <summary>
        /// milligal
        /// </summary>
        C11,

        /// <summary>
        /// milligram per metre
        /// </summary>
        C12,

        /// <summary>
        /// milligray
        /// </summary>
        C13,

        /// <summary>
        /// millihenry
        /// </summary>
        C14,

        /// <summary>
        /// millijoule
        /// </summary>
        C15,

        /// <summary>
        /// millimetre per second
        /// </summary>
        C16,

        /// <summary>
        /// millimetre squared per second
        /// </summary>
        C17,

        /// <summary>
        /// millimole
        /// </summary>
        C18,

        /// <summary>
        /// mole per kilogram
        /// </summary>
        C19,

        /// <summary>
        /// millinewton
        /// </summary>
        C20,

        /// <summary>
        /// kibibit
        /// A unit of information equal to 2¹⁰ (1024) bits (binary digits).
        /// </summary>
        C21,

        /// <summary>
        /// millinewton per metre
        /// </summary>
        C22,

        /// <summary>
        /// milliohm metre
        /// </summary>
        C23,

        /// <summary>
        /// millipascal second
        /// </summary>
        C24,

        /// <summary>
        /// milliradian
        /// </summary>
        C25,

        /// <summary>
        /// millisecond
        /// </summary>
        C26,

        /// <summary>
        /// millisiemens
        /// </summary>
        C27,

        /// <summary>
        /// millisievert
        /// </summary>
        C28,

        /// <summary>
        /// millitesla
        /// </summary>
        C29,

        /// <summary>
        /// microvolt per metre
        /// </summary>
        C3,

        /// <summary>
        /// millivolt per metre
        /// </summary>
        C30,

        /// <summary>
        /// milliwatt
        /// </summary>
        C31,

        /// <summary>
        /// milliwatt per square metre
        /// </summary>
        C32,

        /// <summary>
        /// milliweber
        /// </summary>
        C33,

        /// <summary>
        /// mole
        /// </summary>
        C34,

        /// <summary>
        /// mole per cubic decimetre
        /// </summary>
        C35,

        /// <summary>
        /// mole per cubic metre
        /// </summary>
        C36,

        /// <summary>
        /// kilobit
        /// A unit of information equal to 10³ (1000) bits (binary digits).
        /// </summary>
        C37,

        /// <summary>
        /// mole per litre
        /// </summary>
        C38,

        /// <summary>
        /// nanoampere
        /// </summary>
        C39,

        /// <summary>
        /// nanocoulomb
        /// </summary>
        C40,

        /// <summary>
        /// nanofarad
        /// </summary>
        C41,

        /// <summary>
        /// nanofarad per metre
        /// </summary>
        C42,

        /// <summary>
        /// nanohenry
        /// </summary>
        C43,

        /// <summary>
        /// nanohenry per metre
        /// </summary>
        C44,

        /// <summary>
        /// nanometre
        /// </summary>
        C45,

        /// <summary>
        /// nanoohm metre
        /// </summary>
        C46,

        /// <summary>
        /// nanosecond
        /// </summary>
        C47,

        /// <summary>
        /// nanotesla
        /// </summary>
        C48,

        /// <summary>
        /// nanowatt
        /// </summary>
        C49,

        /// <summary>
        /// neper
        /// </summary>
        C50,

        /// <summary>
        /// neper per second
        /// </summary>
        C51,

        /// <summary>
        /// picometre
        /// </summary>
        C52,

        /// <summary>
        /// newton metre second
        /// </summary>
        C53,

        /// <summary>
        /// newton metre squared per kilogram squared
        /// </summary>
        C54,

        /// <summary>
        /// newton per square metre
        /// </summary>
        C55,

        /// <summary>
        /// newton per square millimetre
        /// </summary>
        C56,

        /// <summary>
        /// newton second
        /// </summary>
        C57,

        /// <summary>
        /// newton second per metre
        /// </summary>
        C58,

        /// <summary>
        /// octave
        /// A unit used in music to describe the ratio in frequency between notes.
        /// </summary>
        C59,

        /// <summary>
        /// ohm centimetre
        /// </summary>
        C60,

        /// <summary>
        /// ohm metre
        /// </summary>
        C61,

        /// <summary>
        /// one
        /// Synonym: unit
        /// </summary>
        C62,

        /// <summary>
        /// parsec
        /// </summary>
        C63,

        /// <summary>
        /// pascal per kelvin
        /// </summary>
        C64,

        /// <summary>
        /// pascal second
        /// </summary>
        C65,

        /// <summary>
        /// pascal second per cubic metre
        /// </summary>
        C66,

        /// <summary>
        /// pascal second per metre
        /// </summary>
        C67,

        /// <summary>
        /// petajoule
        /// </summary>
        C68,

        /// <summary>
        /// phon
        /// A unit of subjective sound loudness. A sound has loudness p phons if it seems to the listener to be equal in loudness to the sound of a pure tone of frequency 1 kilohertz and strength p decibels.
        /// </summary>
        C69,

        /// <summary>
        /// centipoise
        /// </summary>
        C7,

        /// <summary>
        /// picoampere
        /// </summary>
        C70,

        /// <summary>
        /// picocoulomb
        /// </summary>
        C71,

        /// <summary>
        /// picofarad per metre
        /// </summary>
        C72,

        /// <summary>
        /// picohenry
        /// </summary>
        C73,

        /// <summary>
        /// kilobit per second
        /// A unit of information equal to 10³ (1000) bits (binary digits) per second.
        /// </summary>
        C74,

        /// <summary>
        /// picowatt
        /// </summary>
        C75,

        /// <summary>
        /// picowatt per square metre
        /// </summary>
        C76,

        /// <summary>
        /// pound-force
        /// </summary>
        C78,

        /// <summary>
        /// kilovolt ampere hour
        /// A unit of accumulated energy of 1000 volt amperes over a period of one hour.
        /// </summary>
        C79,

        /// <summary>
        /// millicoulomb per kilogram
        /// </summary>
        C8,

        /// <summary>
        /// rad
        /// </summary>
        C80,

        /// <summary>
        /// radian
        /// </summary>
        C81,

        /// <summary>
        /// radian square metre per mole
        /// </summary>
        C82,

        /// <summary>
        /// radian square metre per kilogram
        /// </summary>
        C83,

        /// <summary>
        /// radian per metre
        /// </summary>
        C84,

        /// <summary>
        /// reciprocal angstrom
        /// </summary>
        C85,

        /// <summary>
        /// reciprocal cubic metre
        /// </summary>
        C86,

        /// <summary>
        /// reciprocal cubic metre per second
        /// Synonym: reciprocal second per cubic metre
        /// </summary>
        C87,

        /// <summary>
        /// reciprocal electron volt per cubic metre
        /// </summary>
        C88,

        /// <summary>
        /// reciprocal henry
        /// </summary>
        C89,

        /// <summary>
        /// coil group
        /// A unit of count defining the number of coil groups (coil group: groups of items arranged by lengths of those items placed in a joined sequence of concentric circles).
        /// </summary>
        C9,

        /// <summary>
        /// reciprocal joule per cubic metre
        /// </summary>
        C90,

        /// <summary>
        /// reciprocal kelvin or kelvin to the power minus one
        /// </summary>
        C91,

        /// <summary>
        /// reciprocal metre
        /// </summary>
        C92,

        /// <summary>
        /// reciprocal square metre
        /// Synonym: reciprocal metre squared
        /// </summary>
        C93,

        /// <summary>
        /// reciprocal minute
        /// </summary>
        C94,

        /// <summary>
        /// reciprocal mole
        /// </summary>
        C95,

        /// <summary>
        /// reciprocal pascal or pascal to the power minus one
        /// </summary>
        C96,

        /// <summary>
        /// reciprocal second
        /// </summary>
        C97,

        /// <summary>
        /// reciprocal second per metre squared
        /// </summary>
        C99,

        /// <summary>
        /// carrying capacity in metric ton
        /// A unit of mass defining the carrying capacity, expressed as the number of metric tons.
        /// </summary>
        CCT,

        /// <summary>
        /// candela
        /// </summary>
        CDL,

        /// <summary>
        /// degree Celsius
        /// Refer ISO 80000-5 (Quantities and units — Part 5: Thermodynamics)
        /// </summary>
        CEL,

        /// <summary>
        /// hundred
        /// A unit of count defining the number of units in multiples of 100.
        /// </summary>
        CEN,

        /// <summary>
        /// card
        /// A unit of count defining the number of units of card (card: thick stiff paper or cardboard).
        /// </summary>
        CG,

        /// <summary>
        /// centigram
        /// </summary>
        CGM,

        /// <summary>
        /// coulomb per kilogram
        /// </summary>
        CKG,

        /// <summary>
        /// hundred leave
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
        /// centimetre
        /// </summary>
        CMT,

        /// <summary>
        /// hundred pack
        /// A unit of count defining the number of hundred-packs (hundred-pack: set of one hundred items packaged together).
        /// </summary>
        CNP,

        /// <summary>
        /// cental (UK)
        /// A unit of mass equal to one hundred weight (US).
        /// </summary>
        CNT,

        /// <summary>
        /// coulomb
        /// </summary>
        COU,

        /// <summary>
        /// content gram
        /// A unit of mass defining the number of grams of a named item in a product.
        /// </summary>
        CTG,

        /// <summary>
        /// metric carat
        /// </summary>
        CTM,

        /// <summary>
        /// content ton (metric)
        /// A unit of mass defining the number of metric tons of a named item in a product.
        /// </summary>
        CTN,

        /// <summary>
        /// curie
        /// </summary>
        CUR,

        /// <summary>
        /// hundred pound (cwt) / hundred weight (US)
        /// </summary>
        CWA,

        /// <summary>
        /// hundred weight (UK)
        /// </summary>
        CWI,

        /// <summary>
        /// kilowatt hour per hour
        /// A unit of accumulated energy of a thousand watts over a period of one hour.
        /// </summary>
        D03,

        /// <summary>
        /// lot [unit of weight]
        /// A unit of weight equal to about 1/2 ounce or 15 grams.
        /// </summary>
        D04,

        /// <summary>
        /// reciprocal second per steradian
        /// </summary>
        D1,

        /// <summary>
        /// siemens per metre
        /// </summary>
        D10,

        /// <summary>
        /// mebibit
        /// A unit of information equal to 2²⁰ (1048576) bits (binary digits).
        /// </summary>
        D11,

        /// <summary>
        /// siemens square metre per mole
        /// </summary>
        D12,

        /// <summary>
        /// sievert
        /// </summary>
        D13,

        /// <summary>
        /// sone
        /// A unit of subjective sound loudness. One sone is the loudness of a pure tone of frequency one kilohertz and strength 40 decibels.
        /// </summary>
        D15,

        /// <summary>
        /// square centimetre per erg
        /// </summary>
        D16,

        /// <summary>
        /// square centimetre per steradian erg
        /// </summary>
        D17,

        /// <summary>
        /// metre kelvin
        /// </summary>
        D18,

        /// <summary>
        /// square metre kelvin per watt
        /// </summary>
        D19,

        /// <summary>
        /// reciprocal second per steradian metre squared
        /// </summary>
        D2,

        /// <summary>
        /// square metre per joule
        /// </summary>
        D20,

        /// <summary>
        /// square metre per kilogram
        /// </summary>
        D21,

        /// <summary>
        /// square metre per mole
        /// </summary>
        D22,

        /// <summary>
        /// pen gram (protein)
        /// A unit of count defining the number of grams of amino acid prescribed for parenteral/enteral therapy.
        /// </summary>
        D23,

        /// <summary>
        /// square metre per steradian
        /// </summary>
        D24,

        /// <summary>
        /// square metre per steradian joule
        /// </summary>
        D25,

        /// <summary>
        /// square metre per volt second
        /// </summary>
        D26,

        /// <summary>
        /// steradian
        /// </summary>
        D27,

        /// <summary>
        /// terahertz
        /// </summary>
        D29,

        /// <summary>
        /// terajoule
        /// </summary>
        D30,

        /// <summary>
        /// terawatt
        /// </summary>
        D31,

        /// <summary>
        /// terawatt hour
        /// </summary>
        D32,

        /// <summary>
        /// tesla
        /// </summary>
        D33,

        /// <summary>
        /// tex
        /// A unit of yarn density. One decitex equals a mass of 1 gram per 1 kilometre of length.
        /// </summary>
        D34,

        /// <summary>
        /// megabit
        /// A unit of information equal to 10⁶ (1000000) bits (binary digits).
        /// </summary>
        D36,

        /// <summary>
        /// tonne per cubic metre
        /// </summary>
        D41,

        /// <summary>
        /// tropical year
        /// </summary>
        D42,

        /// <summary>
        /// unified atomic mass unit
        /// </summary>
        D43,

        /// <summary>
        /// var
        /// The name of the unit is an acronym for volt-ampere-reactive.
        /// </summary>
        D44,

        /// <summary>
        /// volt squared per kelvin squared
        /// </summary>
        D45,

        /// <summary>
        /// volt - ampere
        /// </summary>
        D46,

        /// <summary>
        /// volt per centimetre
        /// </summary>
        D47,

        /// <summary>
        /// volt per kelvin
        /// </summary>
        D48,

        /// <summary>
        /// millivolt per kelvin
        /// </summary>
        D49,

        /// <summary>
        /// kilogram per square centimetre
        /// </summary>
        D5,

        /// <summary>
        /// volt per metre
        /// </summary>
        D50,

        /// <summary>
        /// volt per millimetre
        /// </summary>
        D51,

        /// <summary>
        /// watt per kelvin
        /// </summary>
        D52,

        /// <summary>
        /// watt per metre kelvin
        /// </summary>
        D53,

        /// <summary>
        /// watt per square metre
        /// </summary>
        D54,

        /// <summary>
        /// watt per square metre kelvin
        /// </summary>
        D55,

        /// <summary>
        /// watt per square metre kelvin to the fourth power
        /// </summary>
        D56,

        /// <summary>
        /// watt per steradian
        /// </summary>
        D57,

        /// <summary>
        /// watt per steradian square metre
        /// </summary>
        D58,

        /// <summary>
        /// weber per metre
        /// </summary>
        D59,

        /// <summary>
        /// roentgen per second
        /// </summary>
        D6,

        /// <summary>
        /// weber per millimetre
        /// </summary>
        D60,

        /// <summary>
        /// minute [unit of angle]
        /// </summary>
        D61,

        /// <summary>
        /// second [unit of angle]
        /// </summary>
        D62,

        /// <summary>
        /// book
        /// A unit of count defining the number of books (book: set of items bound together or written document of a material whole).
        /// </summary>
        D63,

        /// <summary>
        /// round
        /// A unit of count defining the number of rounds (round: A circular or cylindrical object).
        /// </summary>
        D65,

        /// <summary>
        /// number of words
        /// A unit of count defining the number of words.
        /// </summary>
        D68,

        /// <summary>
        /// inch to the fourth power
        /// </summary>
        D69,

        /// <summary>
        /// joule square metre
        /// </summary>
        D73,

        /// <summary>
        /// kilogram per mole
        /// </summary>
        D74,

        /// <summary>
        /// megacoulomb
        /// </summary>
        D77,

        /// <summary>
        /// megajoule per second
        /// A unit of accumulated energy equal to one million joules per second.
        /// </summary>
        D78,

        /// <summary>
        /// microwatt
        /// </summary>
        D80,

        /// <summary>
        /// microtesla
        /// </summary>
        D81,

        /// <summary>
        /// microvolt
        /// </summary>
        D82,

        /// <summary>
        /// millinewton metre
        /// </summary>
        D83,

        /// <summary>
        /// microwatt per square metre
        /// </summary>
        D85,

        /// <summary>
        /// millicoulomb
        /// </summary>
        D86,

        /// <summary>
        /// millimole per kilogram
        /// </summary>
        D87,

        /// <summary>
        /// millicoulomb per cubic metre
        /// </summary>
        D88,

        /// <summary>
        /// millicoulomb per square metre
        /// </summary>
        D89,

        /// <summary>
        /// rem
        /// </summary>
        D91,

        /// <summary>
        /// second per cubic metre
        /// </summary>
        D93,

        /// <summary>
        /// second per cubic metre radian
        /// </summary>
        D94,

        /// <summary>
        /// joule per gram
        /// </summary>
        D95,

        /// <summary>
        /// decare
        /// </summary>
        DAA,

        /// <summary>
        /// ten day
        /// A unit of time defining the number of days in multiples of 10.
        /// </summary>
        DAD,

        /// <summary>
        /// day
        /// </summary>
        DAY,

        /// <summary>
        /// dry pound
        /// A unit of mass defining the number of pounds of a product, disregarding the water content of the product.
        /// </summary>
        DB,

        /// <summary>
        /// Decibel-milliwatts
        /// </summary>
        DBM,

        /// <summary>
        /// Decibel watt
        /// </summary>
        DBW,

        /// <summary>
        /// degree [unit of angle]
        /// </summary>
        DD,

        /// <summary>
        /// decade
        /// A unit of count defining the number of decades (decade: quantity equal to 10 or time equal to 10 years).
        /// </summary>
        DEC,

        /// <summary>
        /// decigram
        /// </summary>
        DG,

        /// <summary>
        /// decagram
        /// </summary>
        DJ,

        /// <summary>
        /// decilitre
        /// </summary>
        DLT,

        /// <summary>
        /// cubic decametre
        /// </summary>
        DMA,

        /// <summary>
        /// square decimetre
        /// </summary>
        DMK,

        /// <summary>
        /// standard kilolitre
        /// A unit of volume defining the number of kilolitres of a product at a temperature of 15 degrees Celsius, especially in relation to hydrocarbon oils.
        /// </summary>
        DMO,

        /// <summary>
        /// cubic decimetre
        /// </summary>
        DMQ,

        /// <summary>
        /// decimetre
        /// </summary>
        DMT,

        /// <summary>
        /// decinewton metre
        /// </summary>
        DN,

        /// <summary>
        /// dozen piece
        /// A unit of count defining the number of pieces in multiples of 12 (piece: a single item, article or exemplar).
        /// </summary>
        DPC,

        /// <summary>
        /// dozen pair
        /// A unit of count defining the number of pairs in multiples of 12 (pair: item described by two's).
        /// </summary>
        DPR,

        /// <summary>
        /// displacement tonnage
        /// A unit of mass defining the volume of sea water a ship displaces, expressed as the number of tons.
        /// </summary>
        DPT,

        /// <summary>
        /// dram (US)
        /// Synonym: drachm (UK), troy dram
        /// </summary>
        DRA,

        /// <summary>
        /// dram (UK)
        /// Synonym: avoirdupois dram
        /// </summary>
        DRI,

        /// <summary>
        /// dozen roll
        /// A unit of count defining the number of rolls, expressed in twelve roll units.
        /// </summary>
        DRL,

        /// <summary>
        /// dry ton
        /// A unit of mass defining the number of tons of a product, disregarding the water content of the product.
        /// </summary>
        DT,

        /// <summary>
        /// decitonne
        /// Synonym: centner, metric 100 kg; quintal, metric 100 kg
        /// </summary>
        DTN,

        /// <summary>
        /// pennyweight
        /// </summary>
        DWT,

        /// <summary>
        /// dozen
        /// A unit of count defining the number of units in multiples of 12.
        /// </summary>
        DZN,

        /// <summary>
        /// dozen pack
        /// A unit of count defining the number of packs in multiples of 12 (pack: standard packaging unit).
        /// </summary>
        DZP,

        /// <summary>
        /// newton per square centimetre
        /// A measure of pressure expressed in newtons per square centimetre.
        /// </summary>
        E01,

        /// <summary>
        /// megawatt hour per hour
        /// A unit of accumulated energy of a million watts over a period of one hour.
        /// </summary>
        E07,

        /// <summary>
        /// megawatt per hertz
        /// A unit of energy expressed as the load change in million watts that will cause a frequency shift of one hertz.
        /// </summary>
        E08,

        /// <summary>
        /// milliampere hour
        /// A unit of power load delivered at the rate of one thousandth of an ampere over a period of one hour.
        /// </summary>
        E09,

        /// <summary>
        /// degree day
        /// A unit of measure used in meteorology and engineering to measure the demand for heating or cooling over a given period of days.
        /// </summary>
        E10,

        /// <summary>
        /// mille
        /// A unit of count defining the number of cigarettes in units of 1000.
        /// </summary>
        E12,

        /// <summary>
        /// kilocalorie (international table)
        /// A unit of heat energy equal to one thousand calories.
        /// </summary>
        E14,

        /// <summary>
        /// kilocalorie (thermochemical) per hour
        /// A unit of energy equal to one thousand calories per hour.
        /// </summary>
        E15,

        /// <summary>
        /// million Btu(IT) per hour
        /// A unit of power equal to one million British thermal units per hour.
        /// </summary>
        E16,

        /// <summary>
        /// cubic foot per second
        /// A unit of volume equal to one cubic foot passing a given point in a period of one second.
        /// </summary>
        E17,

        /// <summary>
        /// tonne per hour
        /// A unit of weight or mass equal to one tonne per hour.
        /// </summary>
        E18,

        /// <summary>
        /// ping
        /// A unit of area equal to 3.3 square metres.
        /// </summary>
        E19,

        /// <summary>
        /// megabit per second
        /// A unit of information equal to 10⁶ (1000000) bits (binary digits) per second.
        /// </summary>
        E20,

        /// <summary>
        /// shares
        /// A unit of count defining the number of shares (share: a total or portion of the parts into which a business entity’s capital is divided).
        /// </summary>
        E21,

        /// <summary>
        /// TEU
        /// A unit of count defining the number of twenty-foot equivalent units (TEUs) as a measure of containerized cargo capacity.
        /// </summary>
        E22,

        /// <summary>
        /// tyre
        /// A unit of count defining the number of tyres (a solid or air-filled covering placed around a wheel rim to form a soft contact with the road, absorb shock and provide traction).
        /// </summary>
        E23,

        /// <summary>
        /// active unit
        /// A unit of count defining the number of active units within a substance.
        /// </summary>
        E25,

        /// <summary>
        /// dose
        /// A unit of count defining the number of doses (dose: a definite quantity of a medicine or drug).
        /// </summary>
        E27,

        /// <summary>
        /// air dry ton
        /// A unit of mass defining the number of tons of a product, disregarding the water content of the product.
        /// </summary>
        E28,

        /// <summary>
        /// strand
        /// A unit of count defining the number of strands (strand: long, thin, flexible, single thread, strip of fibre, constituent filament or multiples of the same, twisted together).
        /// </summary>
        E30,

        /// <summary>
        /// square metre per litre
        /// A unit of count defining the number of square metres per litre.
        /// </summary>
        E31,

        /// <summary>
        /// litre per hour
        /// A unit of count defining the number of litres per hour.
        /// </summary>
        E32,

        /// <summary>
        /// foot per thousand
        /// A unit of count defining the number of feet per thousand units.
        /// </summary>
        E33,

        /// <summary>
        /// gigabyte
        /// A unit of information equal to 10⁹ bytes.
        /// </summary>
        E34,

        /// <summary>
        /// terabyte
        /// A unit of information equal to 10¹² bytes.
        /// </summary>
        E35,

        /// <summary>
        /// petabyte
        /// A unit of information equal to 10¹⁵ bytes.
        /// </summary>
        E36,

        /// <summary>
        /// pixel
        /// A unit of count defining the number of pixels (pixel: picture element).
        /// </summary>
        E37,

        /// <summary>
        /// megapixel
        /// A unit of count equal to 10⁶ (1000000) pixels (picture elements).
        /// </summary>
        E38,

        /// <summary>
        /// dots per inch
        /// A unit of information defining the number of dots per linear inch as a measure of the resolution or sharpness of a graphic image.
        /// </summary>
        E39,

        /// <summary>
        /// gross kilogram
        /// A unit of mass defining the total number of kilograms before deductions.
        /// </summary>
        E4,

        /// <summary>
        /// part per hundred thousand
        /// A unit of proportion equal to 10⁻⁵.
        /// </summary>
        E40,

        /// <summary>
        /// kilogram-force per square millimetre
        /// A unit of pressure defining the number of kilograms force per square millimetre.
        /// </summary>
        E41,

        /// <summary>
        /// kilogram-force per square centimetre
        /// A unit of pressure defining the number of kilograms force per square centimetre.
        /// </summary>
        E42,

        /// <summary>
        /// joule per square centimetre
        /// A unit of energy defining the number of joules per square centimetre.
        /// </summary>
        E43,

        /// <summary>
        /// kilogram-force metre per square centimetre
        /// A unit of torsion defining the torque kilogram-force metre per square centimetre.
        /// </summary>
        E44,

        /// <summary>
        /// milliohm
        /// </summary>
        E45,

        /// <summary>
        /// kilowatt hour per cubic metre
        /// A unit of energy consumption expressed as kilowatt hour per cubic metre.
        /// </summary>
        E46,

        /// <summary>
        /// kilowatt hour per kelvin
        /// A unit of energy consumption expressed as kilowatt hour per kelvin.
        /// </summary>
        E47,

        /// <summary>
        /// service unit
        /// A unit of count defining the number of service units (service unit: defined period / property / facility / utility of supply).
        /// </summary>
        E48,

        /// <summary>
        /// working day
        /// A unit of count defining the number of working days (working day: a day on which work is ordinarily performed).
        /// </summary>
        E49,

        /// <summary>
        /// accounting unit
        /// A unit of count defining the number of accounting units.
        /// </summary>
        E50,

        /// <summary>
        /// job
        /// A unit of count defining the number of jobs.
        /// </summary>
        E51,

        /// <summary>
        /// run foot
        /// A unit of count defining the number feet per run.
        /// </summary>
        E52,

        /// <summary>
        /// test
        /// A unit of count defining the number of tests.
        /// </summary>
        E53,

        /// <summary>
        /// trip
        /// A unit of count defining the number of trips.
        /// </summary>
        E54,

        /// <summary>
        /// use
        /// A unit of count defining the number of times an object is used.
        /// </summary>
        E55,

        /// <summary>
        /// well
        /// A unit of count defining the number of wells.
        /// </summary>
        E56,

        /// <summary>
        /// zone
        /// A unit of count defining the number of zones.
        /// </summary>
        E57,

        /// <summary>
        /// exabit per second
        /// A unit of information equal to 10¹⁸ bits (binary digits) per second.
        /// </summary>
        E58,

        /// <summary>
        /// exbibyte
        /// A unit of information equal to 2⁶⁰ bytes.
        /// </summary>
        E59,

        /// <summary>
        /// pebibyte
        /// A unit of information equal to 2⁵⁰ bytes.
        /// </summary>
        E60,

        /// <summary>
        /// tebibyte
        /// A unit of information equal to 2⁴⁰ bytes.
        /// </summary>
        E61,

        /// <summary>
        /// gibibyte
        /// A unit of information equal to 2³⁰ bytes.
        /// </summary>
        E62,

        /// <summary>
        /// mebibyte
        /// A unit of information equal to 2²⁰ bytes.
        /// </summary>
        E63,

        /// <summary>
        /// kibibyte
        /// A unit of information equal to 2¹⁰ bytes.
        /// </summary>
        E64,

        /// <summary>
        /// exbibit per metre
        /// A unit of information equal to 2⁶⁰ bits (binary digits) per metre.
        /// </summary>
        E65,

        /// <summary>
        /// exbibit per square metre
        /// A unit of information equal to 2⁶⁰ bits (binary digits) per square metre.
        /// </summary>
        E66,

        /// <summary>
        /// exbibit per cubic metre
        /// A unit of information equal to 2⁶⁰ bits (binary digits) per cubic metre.
        /// </summary>
        E67,

        /// <summary>
        /// gigabyte per second
        /// A unit of information equal to 10⁹ bytes per second.
        /// </summary>
        E68,

        /// <summary>
        /// gibibit per metre
        /// A unit of information equal to 2³⁰ bits (binary digits) per metre.
        /// </summary>
        E69,

        /// <summary>
        /// gibibit per square metre
        /// A unit of information equal to 2³⁰ bits (binary digits) per square metre.
        /// </summary>
        E70,

        /// <summary>
        /// gibibit per cubic metre
        /// A unit of information equal to 2³⁰ bits (binary digits) per cubic metre.
        /// </summary>
        E71,

        /// <summary>
        /// kibibit per metre
        /// A unit of information equal to 2¹⁰ bits (binary digits) per metre.
        /// </summary>
        E72,

        /// <summary>
        /// kibibit per square metre
        /// A unit of information equal to 2¹⁰ bits (binary digits) per square metre.
        /// </summary>
        E73,

        /// <summary>
        /// kibibit per cubic metre
        /// A unit of information equal to 2¹⁰ bits (binary digits) per cubic metre.
        /// </summary>
        E74,

        /// <summary>
        /// mebibit per metre
        /// A unit of information equal to 2²⁰ bits (binary digits) per metre.
        /// </summary>
        E75,

        /// <summary>
        /// mebibit per square metre
        /// A unit of information equal to 2²⁰ bits (binary digits) per square metre.
        /// </summary>
        E76,

        /// <summary>
        /// mebibit per cubic metre
        /// A unit of information equal to 2²⁰ bits (binary digits) per cubic metre.
        /// </summary>
        E77,

        /// <summary>
        /// petabit
        /// A unit of information equal to 10¹⁵ bits (binary digits).
        /// </summary>
        E78,

        /// <summary>
        /// petabit per second
        /// A unit of information equal to 10¹⁵ bits (binary digits) per second.
        /// </summary>
        E79,

        /// <summary>
        /// pebibit per metre
        /// A unit of information equal to 2⁵⁰ bits (binary digits) per metre.
        /// </summary>
        E80,

        /// <summary>
        /// pebibit per square metre
        /// A unit of information equal to 2⁵⁰ bits (binary digits) per square metre.
        /// </summary>
        E81,

        /// <summary>
        /// pebibit per cubic metre
        /// A unit of information equal to 2⁵⁰ bits (binary digits) per cubic metre.
        /// </summary>
        E82,

        /// <summary>
        /// terabit
        /// A unit of information equal to 10¹² bits (binary digits).
        /// </summary>
        E83,

        /// <summary>
        /// terabit per second
        /// A unit of information equal to 10¹² bits (binary digits) per second.
        /// </summary>
        E84,

        /// <summary>
        /// tebibit per metre
        /// A unit of information equal to 2⁴⁰ bits (binary digits) per metre.
        /// </summary>
        E85,

        /// <summary>
        /// tebibit per cubic metre
        /// A unit of information equal to 2⁴⁰ bits (binary digits) per cubic metre.
        /// </summary>
        E86,

        /// <summary>
        /// tebibit per square metre
        /// A unit of information equal to 2⁴⁰ bits (binary digits) per square metre.
        /// </summary>
        E87,

        /// <summary>
        /// bit per metre
        /// A unit of information equal to 1 bit (binary digit) per metre.
        /// </summary>
        E88,

        /// <summary>
        /// bit per square metre
        /// A unit of information equal to 1 bit (binary digit) per square metre.
        /// </summary>
        E89,

        /// <summary>
        /// reciprocal centimetre
        /// </summary>
        E90,

        /// <summary>
        /// reciprocal day
        /// </summary>
        E91,

        /// <summary>
        /// cubic decimetre per hour
        /// </summary>
        E92,

        /// <summary>
        /// kilogram per hour
        /// </summary>
        E93,

        /// <summary>
        /// kilomole per second
        /// </summary>
        E94,

        /// <summary>
        /// mole per second
        /// </summary>
        E95,

        /// <summary>
        /// degree per second
        /// </summary>
        E96,

        /// <summary>
        /// millimetre per degree Celcius metre
        /// </summary>
        E97,

        /// <summary>
        /// degree Celsius per kelvin
        /// </summary>
        E98,

        /// <summary>
        /// hectopascal per bar
        /// </summary>
        E99,

        /// <summary>
        /// each
        /// A unit of count defining the number of items regarded as separate units.
        /// </summary>
        EA,

        /// <summary>
        /// electronic mail box
        /// A unit of count defining the number of electronic mail boxes.
        /// </summary>
        EB,

        /// <summary>
        /// equivalent gallon
        /// A unit of volume defining the number of gallons of product produced from concentrate.
        /// </summary>
        EQ,

        /// <summary>
        /// bit per cubic metre
        /// A unit of information equal to 1 bit (binary digit) per cubic metre.
        /// </summary>
        F01,

        /// <summary>
        /// kelvin per kelvin
        /// </summary>
        F02,

        /// <summary>
        /// kilopascal per bar
        /// </summary>
        F03,

        /// <summary>
        /// millibar per bar
        /// </summary>
        F04,

        /// <summary>
        /// megapascal per bar
        /// </summary>
        F05,

        /// <summary>
        /// poise per bar
        /// </summary>
        F06,

        /// <summary>
        /// pascal per bar
        /// </summary>
        F07,

        /// <summary>
        /// milliampere per inch
        /// </summary>
        F08,

        /// <summary>
        /// kelvin per hour
        /// </summary>
        F10,

        /// <summary>
        /// kelvin per minute
        /// </summary>
        F11,

        /// <summary>
        /// kelvin per second
        /// </summary>
        F12,

        /// <summary>
        /// slug
        /// A unit of mass. One slug is the mass accelerated at 1 foot per second per second by a force of 1 pound.
        /// </summary>
        F13,

        /// <summary>
        /// gram per kelvin
        /// </summary>
        F14,

        /// <summary>
        /// kilogram per kelvin
        /// </summary>
        F15,

        /// <summary>
        /// milligram per kelvin
        /// </summary>
        F16,

        /// <summary>
        /// pound-force per foot
        /// </summary>
        F17,

        /// <summary>
        /// kilogram square centimetre
        /// </summary>
        F18,

        /// <summary>
        /// kilogram square millimetre
        /// </summary>
        F19,

        /// <summary>
        /// pound inch squared
        /// </summary>
        F20,

        /// <summary>
        /// pound-force inch
        /// </summary>
        F21,

        /// <summary>
        /// pound-force foot per ampere
        /// </summary>
        F22,

        /// <summary>
        /// gram per cubic decimetre
        /// </summary>
        F23,

        /// <summary>
        /// kilogram per kilomol
        /// </summary>
        F24,

        /// <summary>
        /// gram per hertz
        /// </summary>
        F25,

        /// <summary>
        /// gram per day
        /// </summary>
        F26,

        /// <summary>
        /// gram per hour
        /// </summary>
        F27,

        /// <summary>
        /// gram per minute
        /// </summary>
        F28,

        /// <summary>
        /// gram per second
        /// </summary>
        F29,

        /// <summary>
        /// kilogram per day
        /// </summary>
        F30,

        /// <summary>
        /// kilogram per minute
        /// </summary>
        F31,

        /// <summary>
        /// milligram per day
        /// </summary>
        F32,

        /// <summary>
        /// milligram per minute
        /// </summary>
        F33,

        /// <summary>
        /// milligram per second
        /// </summary>
        F34,

        /// <summary>
        /// gram per day kelvin
        /// </summary>
        F35,

        /// <summary>
        /// gram per hour kelvin
        /// </summary>
        F36,

        /// <summary>
        /// gram per minute kelvin
        /// </summary>
        F37,

        /// <summary>
        /// gram per second kelvin
        /// </summary>
        F38,

        /// <summary>
        /// kilogram per day kelvin
        /// </summary>
        F39,

        /// <summary>
        /// kilogram per hour kelvin
        /// </summary>
        F40,

        /// <summary>
        /// kilogram per minute kelvin
        /// </summary>
        F41,

        /// <summary>
        /// kilogram per second kelvin
        /// </summary>
        F42,

        /// <summary>
        /// milligram per day kelvin
        /// </summary>
        F43,

        /// <summary>
        /// milligram per hour kelvin
        /// </summary>
        F44,

        /// <summary>
        /// milligram per minute kelvin
        /// </summary>
        F45,

        /// <summary>
        /// milligram per second kelvin
        /// </summary>
        F46,

        /// <summary>
        /// newton per millimetre
        /// </summary>
        F47,

        /// <summary>
        /// pound-force per inch
        /// </summary>
        F48,

        /// <summary>
        /// rod [unit of distance]
        /// A unit of distance equal to 5.5 yards (16 feet 6 inches).
        /// </summary>
        F49,

        /// <summary>
        /// micrometre per kelvin
        /// </summary>
        F50,

        /// <summary>
        /// centimetre per kelvin
        /// </summary>
        F51,

        /// <summary>
        /// metre per kelvin
        /// </summary>
        F52,

        /// <summary>
        /// millimetre per kelvin
        /// </summary>
        F53,

        /// <summary>
        /// milliohm per metre
        /// </summary>
        F54,

        /// <summary>
        /// ohm per mile (statute mile)
        /// </summary>
        F55,

        /// <summary>
        /// ohm per kilometre
        /// </summary>
        F56,

        /// <summary>
        /// milliampere per pound-force per square inch
        /// </summary>
        F57,

        /// <summary>
        /// reciprocal bar
        /// </summary>
        F58,

        /// <summary>
        /// milliampere per bar
        /// </summary>
        F59,

        /// <summary>
        /// degree Celsius per bar
        /// </summary>
        F60,

        /// <summary>
        /// kelvin per bar
        /// </summary>
        F61,

        /// <summary>
        /// gram per day bar
        /// </summary>
        F62,

        /// <summary>
        /// gram per hour bar
        /// </summary>
        F63,

        /// <summary>
        /// gram per minute bar
        /// </summary>
        F64,

        /// <summary>
        /// gram per second bar
        /// </summary>
        F65,

        /// <summary>
        /// kilogram per day bar
        /// </summary>
        F66,

        /// <summary>
        /// kilogram per hour bar
        /// </summary>
        F67,

        /// <summary>
        /// kilogram per minute bar
        /// </summary>
        F68,

        /// <summary>
        /// kilogram per second bar
        /// </summary>
        F69,

        /// <summary>
        /// milligram per day bar
        /// </summary>
        F70,

        /// <summary>
        /// milligram per hour bar
        /// </summary>
        F71,

        /// <summary>
        /// milligram per minute bar
        /// </summary>
        F72,

        /// <summary>
        /// milligram per second bar
        /// </summary>
        F73,

        /// <summary>
        /// gram per bar
        /// </summary>
        F74,

        /// <summary>
        /// milligram per bar
        /// </summary>
        F75,

        /// <summary>
        /// milliampere per millimetre
        /// </summary>
        F76,

        /// <summary>
        /// pascal second per kelvin
        /// </summary>
        F77,

        /// <summary>
        /// inch of water
        /// </summary>
        F78,

        /// <summary>
        /// inch of mercury
        /// </summary>
        F79,

        /// <summary>
        /// water horse power
        /// A unit of power defining the amount of power required to move a given volume of water against acceleration of gravity to a specified elevation (pressure head).
        /// </summary>
        F80,

        /// <summary>
        /// bar per kelvin
        /// </summary>
        F81,

        /// <summary>
        /// hectopascal per kelvin
        /// </summary>
        F82,

        /// <summary>
        /// kilopascal per kelvin
        /// </summary>
        F83,

        /// <summary>
        /// millibar per kelvin
        /// </summary>
        F84,

        /// <summary>
        /// megapascal per kelvin
        /// </summary>
        F85,

        /// <summary>
        /// poise per kelvin
        /// </summary>
        F86,

        /// <summary>
        /// volt per litre minute
        /// </summary>
        F87,

        /// <summary>
        /// newton centimetre
        /// </summary>
        F88,

        /// <summary>
        /// newton metre per degree
        /// </summary>
        F89,

        /// <summary>
        /// newton metre per ampere
        /// </summary>
        F90,

        /// <summary>
        /// bar litre per second
        /// </summary>
        F91,

        /// <summary>
        /// bar cubic metre per second
        /// </summary>
        F92,

        /// <summary>
        /// hectopascal litre per second
        /// </summary>
        F93,

        /// <summary>
        /// hectopascal cubic metre per second
        /// </summary>
        F94,

        /// <summary>
        /// millibar litre per second
        /// </summary>
        F95,

        /// <summary>
        /// millibar cubic metre per second
        /// </summary>
        F96,

        /// <summary>
        /// megapascal litre per second
        /// </summary>
        F97,

        /// <summary>
        /// megapascal cubic metre per second
        /// </summary>
        F98,

        /// <summary>
        /// pascal litre per second
        /// </summary>
        F99,

        /// <summary>
        /// degree Fahrenheit
        /// Refer ISO 80000-5 (Quantities and units — Part 5: Thermodynamics)
        /// </summary>
        FAH,

        /// <summary>
        /// farad
        /// </summary>
        FAR,

        /// <summary>
        /// fibre metre
        /// A unit of length defining the number of metres of individual fibre.
        /// </summary>
        FBM,

        /// <summary>
        /// thousand cubic foot
        /// A unit of volume equal to one thousand cubic foot.
        /// </summary>
        FC,

        /// <summary>
        /// hundred cubic metre
        /// A unit of volume equal to one hundred cubic metres.
        /// </summary>
        FF,

        /// <summary>
        /// micromole
        /// </summary>
        FH,

        /// <summary>
        /// failures in time
        /// A unit of count defining the number of failures that can be expected over a specified time interval. Failure rates of semiconductor components are often specified as FIT (failures in time unit) where 1 FIT = 10⁻⁹ /h.
        /// </summary>
        FIT,

        /// <summary>
        /// flake ton
        /// A unit of mass defining the number of tons of a flaked substance (flake: a small flattish fragment).
        /// </summary>
        FL,

        /// <summary>
        /// Formazin nephelometric unit
        /// </summary>
        FNU,

        /// <summary>
        /// foot
        /// </summary>
        FOT,

        /// <summary>
        /// pound per square foot
        /// </summary>
        FP,

        /// <summary>
        /// foot per minute
        /// </summary>
        FR,

        /// <summary>
        /// foot per second
        /// </summary>
        FS,

        /// <summary>
        /// square foot
        /// </summary>
        FTK,

        /// <summary>
        /// cubic foot
        /// </summary>
        FTQ,

        /// <summary>
        /// pascal cubic metre per second
        /// </summary>
        G01,

        /// <summary>
        /// centimetre per bar
        /// </summary>
        G04,

        /// <summary>
        /// metre per bar
        /// </summary>
        G05,

        /// <summary>
        /// millimetre per bar
        /// </summary>
        G06,

        /// <summary>
        /// square inch per second
        /// </summary>
        G08,

        /// <summary>
        /// square metre per second kelvin
        /// </summary>
        G09,

        /// <summary>
        /// stokes per kelvin
        /// </summary>
        G10,

        /// <summary>
        /// gram per cubic centimetre bar
        /// </summary>
        G11,

        /// <summary>
        /// gram per cubic decimetre bar
        /// </summary>
        G12,

        /// <summary>
        /// gram per litre bar
        /// </summary>
        G13,

        /// <summary>
        /// gram per cubic metre bar
        /// </summary>
        G14,

        /// <summary>
        /// gram per millilitre bar
        /// </summary>
        G15,

        /// <summary>
        /// kilogram per cubic centimetre bar
        /// </summary>
        G16,

        /// <summary>
        /// kilogram per litre bar
        /// </summary>
        G17,

        /// <summary>
        /// kilogram per cubic metre bar
        /// </summary>
        G18,

        /// <summary>
        /// newton metre per kilogram
        /// </summary>
        G19,

        /// <summary>
        /// US gallon per minute
        /// </summary>
        G2,

        /// <summary>
        /// pound-force foot per pound
        /// </summary>
        G20,

        /// <summary>
        /// cup [unit of volume]
        /// </summary>
        G21,

        /// <summary>
        /// peck
        /// </summary>
        G23,

        /// <summary>
        /// tablespoon (US)
        /// </summary>
        G24,

        /// <summary>
        /// teaspoon (US)
        /// </summary>
        G25,

        /// <summary>
        /// stere
        /// </summary>
        G26,

        /// <summary>
        /// cubic centimetre per kelvin
        /// </summary>
        G27,

        /// <summary>
        /// litre per kelvin
        /// </summary>
        G28,

        /// <summary>
        /// cubic metre per kelvin
        /// </summary>
        G29,

        /// <summary>
        /// Imperial gallon per minute
        /// </summary>
        G3,

        /// <summary>
        /// millilitre per kelvin
        /// </summary>
        G30,

        /// <summary>
        /// kilogram per cubic centimetre
        /// </summary>
        G31,

        /// <summary>
        /// ounce (avoirdupois) per cubic yard
        /// </summary>
        G32,

        /// <summary>
        /// gram per cubic centimetre kelvin
        /// </summary>
        G33,

        /// <summary>
        /// gram per cubic decimetre kelvin
        /// </summary>
        G34,

        /// <summary>
        /// gram per litre kelvin
        /// </summary>
        G35,

        /// <summary>
        /// gram per cubic metre kelvin
        /// </summary>
        G36,

        /// <summary>
        /// gram per millilitre kelvin
        /// </summary>
        G37,

        /// <summary>
        /// kilogram per cubic centimetre kelvin
        /// </summary>
        G38,

        /// <summary>
        /// kilogram per litre kelvin
        /// </summary>
        G39,

        /// <summary>
        /// kilogram per cubic metre kelvin
        /// </summary>
        G40,

        /// <summary>
        /// square metre per second bar
        /// </summary>
        G41,

        /// <summary>
        /// microsiemens per centimetre
        /// </summary>
        G42,

        /// <summary>
        /// microsiemens per metre
        /// </summary>
        G43,

        /// <summary>
        /// nanosiemens per centimetre
        /// </summary>
        G44,

        /// <summary>
        /// nanosiemens per metre
        /// </summary>
        G45,

        /// <summary>
        /// stokes per bar
        /// </summary>
        G46,

        /// <summary>
        /// cubic centimetre per day
        /// </summary>
        G47,

        /// <summary>
        /// cubic centimetre per hour
        /// </summary>
        G48,

        /// <summary>
        /// cubic centimetre per minute
        /// </summary>
        G49,

        /// <summary>
        /// gallon (US) per hour
        /// </summary>
        G50,

        /// <summary>
        /// litre per second
        /// </summary>
        G51,

        /// <summary>
        /// cubic metre per day
        /// </summary>
        G52,

        /// <summary>
        /// cubic metre per minute
        /// </summary>
        G53,

        /// <summary>
        /// millilitre per day
        /// </summary>
        G54,

        /// <summary>
        /// millilitre per hour
        /// </summary>
        G55,

        /// <summary>
        /// cubic inch per hour
        /// </summary>
        G56,

        /// <summary>
        /// cubic inch per minute
        /// </summary>
        G57,

        /// <summary>
        /// cubic inch per second
        /// </summary>
        G58,

        /// <summary>
        /// milliampere per litre minute
        /// </summary>
        G59,

        /// <summary>
        /// volt per bar
        /// </summary>
        G60,

        /// <summary>
        /// cubic centimetre per day kelvin
        /// </summary>
        G61,

        /// <summary>
        /// cubic centimetre per hour kelvin
        /// </summary>
        G62,

        /// <summary>
        /// cubic centimetre per minute kelvin
        /// </summary>
        G63,

        /// <summary>
        /// cubic centimetre per second kelvin
        /// </summary>
        G64,

        /// <summary>
        /// litre per day kelvin
        /// </summary>
        G65,

        /// <summary>
        /// litre per hour kelvin
        /// </summary>
        G66,

        /// <summary>
        /// litre per minute kelvin
        /// </summary>
        G67,

        /// <summary>
        /// litre per second kelvin
        /// </summary>
        G68,

        /// <summary>
        /// cubic metre per day kelvin
        /// </summary>
        G69,

        /// <summary>
        /// cubic metre per hour kelvin
        /// </summary>
        G70,

        /// <summary>
        /// cubic metre per minute kelvin
        /// </summary>
        G71,

        /// <summary>
        /// cubic metre per second kelvin
        /// </summary>
        G72,

        /// <summary>
        /// millilitre per day kelvin
        /// </summary>
        G73,

        /// <summary>
        /// millilitre per hour kelvin
        /// </summary>
        G74,

        /// <summary>
        /// millilitre per minute kelvin
        /// </summary>
        G75,

        /// <summary>
        /// millilitre per second kelvin
        /// </summary>
        G76,

        /// <summary>
        /// millimetre to the fourth power
        /// </summary>
        G77,

        /// <summary>
        /// cubic centimetre per day bar
        /// </summary>
        G78,

        /// <summary>
        /// cubic centimetre per hour bar
        /// </summary>
        G79,

        /// <summary>
        /// cubic centimetre per minute bar
        /// </summary>
        G80,

        /// <summary>
        /// cubic centimetre per second bar
        /// </summary>
        G81,

        /// <summary>
        /// litre per day bar
        /// </summary>
        G82,

        /// <summary>
        /// litre per hour bar
        /// </summary>
        G83,

        /// <summary>
        /// litre per minute bar
        /// </summary>
        G84,

        /// <summary>
        /// litre per second bar
        /// </summary>
        G85,

        /// <summary>
        /// cubic metre per day bar
        /// </summary>
        G86,

        /// <summary>
        /// cubic metre per hour bar
        /// </summary>
        G87,

        /// <summary>
        /// cubic metre per minute bar
        /// </summary>
        G88,

        /// <summary>
        /// cubic metre per second bar
        /// </summary>
        G89,

        /// <summary>
        /// millilitre per day bar
        /// </summary>
        G90,

        /// <summary>
        /// millilitre per hour bar
        /// </summary>
        G91,

        /// <summary>
        /// millilitre per minute bar
        /// </summary>
        G92,

        /// <summary>
        /// millilitre per second bar
        /// </summary>
        G93,

        /// <summary>
        /// cubic centimetre per bar
        /// </summary>
        G94,

        /// <summary>
        /// litre per bar
        /// </summary>
        G95,

        /// <summary>
        /// cubic metre per bar
        /// </summary>
        G96,

        /// <summary>
        /// millilitre per bar
        /// </summary>
        G97,

        /// <summary>
        /// microhenry per kiloohm
        /// </summary>
        G98,

        /// <summary>
        /// microhenry per ohm
        /// </summary>
        G99,

        /// <summary>
        /// gallon (US) per day
        /// </summary>
        GB,

        /// <summary>
        /// gigabecquerel
        /// </summary>
        GBQ,

        /// <summary>
        /// gram, dry weight
        /// A unit of mass defining the number of grams of a product, disregarding the water content of the product.
        /// </summary>
        GDW,

        /// <summary>
        /// pound per gallon (US)
        /// </summary>
        GE,

        /// <summary>
        /// gram per metre (gram per 100 centimetres)
        /// </summary>
        GF,

        /// <summary>
        /// gram of fissile isotope
        /// A unit of mass defining the number of grams of a fissile isotope (fissile isotope: an isotope whose nucleus is able to be split when irradiated with low energy neutrons).
        /// </summary>
        GFI,

        /// <summary>
        /// great gross
        /// A unit of count defining the number of units in multiples of 1728 (12 x 12 x 12).
        /// </summary>
        GGR,

        /// <summary>
        /// gill (US)
        /// </summary>
        GIA,

        /// <summary>
        /// gram, including container
        /// A unit of mass defining the number of grams of a product, including its container.
        /// </summary>
        GIC,

        /// <summary>
        /// gill (UK)
        /// </summary>
        GII,

        /// <summary>
        /// gram, including inner packaging
        /// A unit of mass defining the number of grams of a product, including its inner packaging materials.
        /// </summary>
        GIP,

        /// <summary>
        /// gram per millilitre
        /// </summary>
        GJ,

        /// <summary>
        /// gram per litre
        /// </summary>
        GL,

        /// <summary>
        /// dry gallon (US)
        /// </summary>
        GLD,

        /// <summary>
        /// gallon (UK)
        /// </summary>
        GLI,

        /// <summary>
        /// gallon (US)
        /// </summary>
        GLL,

        /// <summary>
        /// gram per square metre
        /// </summary>
        GM,

        /// <summary>
        /// milligram per square metre
        /// </summary>
        GO,

        /// <summary>
        /// milligram per cubic metre
        /// </summary>
        GP,

        /// <summary>
        /// microgram per cubic metre
        /// </summary>
        GQ,

        /// <summary>
        /// gram
        /// </summary>
        GRM,

        /// <summary>
        /// grain
        /// </summary>
        GRN,

        /// <summary>
        /// gross
        /// A unit of count defining the number of units in multiples of 144 (12 x 12).
        /// </summary>
        GRO,

        /// <summary>
        /// gigajoule
        /// </summary>
        GV,

        /// <summary>
        /// gigawatt hour
        /// </summary>
        GWH,

        /// <summary>
        /// henry per kiloohm
        /// </summary>
        H03,

        /// <summary>
        /// henry per ohm
        /// </summary>
        H04,

        /// <summary>
        /// millihenry per kiloohm
        /// </summary>
        H05,

        /// <summary>
        /// millihenry per ohm
        /// </summary>
        H06,

        /// <summary>
        /// pascal second per bar
        /// </summary>
        H07,

        /// <summary>
        /// microbecquerel
        /// </summary>
        H08,

        /// <summary>
        /// reciprocal year
        /// </summary>
        H09,

        /// <summary>
        /// reciprocal hour
        /// </summary>
        H10,

        /// <summary>
        /// reciprocal month
        /// </summary>
        H11,

        /// <summary>
        /// degree Celsius per hour
        /// </summary>
        H12,

        /// <summary>
        /// degree Celsius per minute
        /// </summary>
        H13,

        /// <summary>
        /// degree Celsius per second
        /// </summary>
        H14,

        /// <summary>
        /// square centimetre per gram
        /// </summary>
        H15,

        /// <summary>
        /// square decametre
        /// Synonym: are
        /// </summary>
        H16,

        /// <summary>
        /// square hectometre
        /// Synonym: hectare
        /// </summary>
        H18,

        /// <summary>
        /// cubic hectometre
        /// </summary>
        H19,

        /// <summary>
        /// cubic kilometre
        /// </summary>
        H20,

        /// <summary>
        /// blank
        /// A unit of count defining the number of blanks.
        /// </summary>
        H21,

        /// <summary>
        /// volt square inch per pound-force
        /// </summary>
        H22,

        /// <summary>
        /// volt per inch
        /// </summary>
        H23,

        /// <summary>
        /// volt per microsecond
        /// </summary>
        H24,

        /// <summary>
        /// percent per kelvin
        /// A unit of proportion, equal to 0.01, in relation to the SI base unit Kelvin.
        /// </summary>
        H25,

        /// <summary>
        /// ohm per metre
        /// </summary>
        H26,

        /// <summary>
        /// degree per metre
        /// </summary>
        H27,

        /// <summary>
        /// microfarad per kilometre
        /// </summary>
        H28,

        /// <summary>
        /// microgram per litre
        /// </summary>
        H29,

        /// <summary>
        /// square micrometre (square micron)
        /// </summary>
        H30,

        /// <summary>
        /// ampere per kilogram
        /// </summary>
        H31,

        /// <summary>
        /// ampere squared second
        /// </summary>
        H32,

        /// <summary>
        /// farad per kilometre
        /// </summary>
        H33,

        /// <summary>
        /// hertz metre
        /// </summary>
        H34,

        /// <summary>
        /// kelvin metre per watt
        /// </summary>
        H35,

        /// <summary>
        /// megaohm per kilometre
        /// </summary>
        H36,

        /// <summary>
        /// megaohm per metre
        /// </summary>
        H37,

        /// <summary>
        /// megaampere
        /// </summary>
        H38,

        /// <summary>
        /// megahertz kilometre
        /// </summary>
        H39,

        /// <summary>
        /// newton per ampere
        /// </summary>
        H40,

        /// <summary>
        /// newton metre watt to the power minus 0,5
        /// </summary>
        H41,

        /// <summary>
        /// pascal per metre
        /// </summary>
        H42,

        /// <summary>
        /// siemens per centimetre
        /// </summary>
        H43,

        /// <summary>
        /// teraohm
        /// </summary>
        H44,

        /// <summary>
        /// volt second per metre
        /// </summary>
        H45,

        /// <summary>
        /// volt per second
        /// </summary>
        H46,

        /// <summary>
        /// watt per cubic metre
        /// </summary>
        H47,

        /// <summary>
        /// attofarad
        /// </summary>
        H48,

        /// <summary>
        /// centimetre per hour
        /// </summary>
        H49,

        /// <summary>
        /// reciprocal cubic centimetre
        /// </summary>
        H50,

        /// <summary>
        /// decibel per kilometre
        /// </summary>
        H51,

        /// <summary>
        /// decibel per metre
        /// </summary>
        H52,

        /// <summary>
        /// kilogram per bar
        /// </summary>
        H53,

        /// <summary>
        /// kilogram per cubic decimetre kelvin
        /// </summary>
        H54,

        /// <summary>
        /// kilogram per cubic decimetre bar
        /// </summary>
        H55,

        /// <summary>
        /// kilogram per square metre second
        /// </summary>
        H56,

        /// <summary>
        /// inch per two pi radiant
        /// </summary>
        H57,

        /// <summary>
        /// metre per volt second
        /// </summary>
        H58,

        /// <summary>
        /// square metre per newton
        /// </summary>
        H59,

        /// <summary>
        /// cubic metre per cubic metre
        /// </summary>
        H60,

        /// <summary>
        /// millisiemens per centimetre
        /// </summary>
        H61,

        /// <summary>
        /// millivolt per minute
        /// </summary>
        H62,

        /// <summary>
        /// milligram per square centimetre
        /// </summary>
        H63,

        /// <summary>
        /// milligram per gram
        /// </summary>
        H64,

        /// <summary>
        /// millilitre per cubic metre
        /// </summary>
        H65,

        /// <summary>
        /// millimetre per year
        /// </summary>
        H66,

        /// <summary>
        /// millimetre per hour
        /// </summary>
        H67,

        /// <summary>
        /// millimole per gram
        /// </summary>
        H68,

        /// <summary>
        /// picopascal per kilometre
        /// </summary>
        H69,

        /// <summary>
        /// picosecond
        /// </summary>
        H70,

        /// <summary>
        /// percent per month
        /// A unit of proportion, equal to 0.01, in relation to a month.
        /// </summary>
        H71,

        /// <summary>
        /// percent per hectobar
        /// A unit of proportion, equal to 0.01, in relation to 100-fold of the unit bar.
        /// </summary>
        H72,

        /// <summary>
        /// percent per decakelvin
        /// A unit of proportion, equal to 0.01, in relation to 10-fold of the SI base unit Kelvin.
        /// </summary>
        H73,

        /// <summary>
        /// watt per metre
        /// </summary>
        H74,

        /// <summary>
        /// decapascal
        /// </summary>
        H75,

        /// <summary>
        /// gram per millimetre
        /// </summary>
        H76,

        /// <summary>
        /// module width
        /// A unit of measure used to describe the breadth of electronic assemblies as an installation standard or mounting dimension.
        /// </summary>
        H77,

        /// <summary>
        /// French gauge
        /// A unit of distance used for measuring the diameter of small tubes such as urological instruments and catheters. Synonym: French, Charrière, Charrière gauge
        /// </summary>
        H79,

        /// <summary>
        /// rack unit
        /// A unit of measure used to describe the height in rack units of equipment intended for mounting in a 19-inch rack or a 23-inch rack. One rack unit is 1.75 inches (44.45 mm) high.
        /// </summary>
        H80,

        /// <summary>
        /// millimetre per minute
        /// </summary>
        H81,

        /// <summary>
        /// big point
        /// A unit of length defining the number of big points (big point: Adobe software(US) defines the big point to be exactly 1/72 inch (0.013 888 9 inch or 0.352 777 8 millimeters))
        /// </summary>
        H82,

        /// <summary>
        /// litre per kilogram
        /// </summary>
        H83,

        /// <summary>
        /// gram millimetre
        /// </summary>
        H84,

        /// <summary>
        /// reciprocal week
        /// </summary>
        H85,

        /// <summary>
        /// piece
        /// A unit of count defining the number of pieces (piece: a single item, article or exemplar).
        /// </summary>
        H87,

        /// <summary>
        /// megaohm kilometre
        /// </summary>
        H88,

        /// <summary>
        /// percent per ohm
        /// A unit of proportion, equal to 0.01, in relation to the SI derived unit ohm.
        /// </summary>
        H89,

        /// <summary>
        /// percent per degree
        /// A unit of proportion, equal to 0.01, in relation to an angle of one degree.
        /// </summary>
        H90,

        /// <summary>
        /// percent per ten thousand
        /// A unit of proportion, equal to 0.01, in relation to multiples of ten thousand.
        /// </summary>
        H91,

        /// <summary>
        /// percent per one hundred thousand
        /// A unit of proportion, equal to 0.01, in relation to multiples of one hundred thousand.
        /// </summary>
        H92,

        /// <summary>
        /// percent per hundred
        /// A unit of proportion, equal to 0.01, in relation to multiples of one hundred.
        /// </summary>
        H93,

        /// <summary>
        /// percent per thousand
        /// A unit of proportion, equal to 0.01, in relation to multiples of one thousand.
        /// </summary>
        H94,

        /// <summary>
        /// percent per volt
        /// A unit of proportion, equal to 0.01, in relation to the SI derived unit volt.
        /// </summary>
        H95,

        /// <summary>
        /// percent per bar
        /// A unit of proportion, equal to 0.01, in relation to an atmospheric pressure of one bar.
        /// </summary>
        H96,

        /// <summary>
        /// percent per inch
        /// A unit of proportion, equal to 0.01, in relation to an inch.
        /// </summary>
        H98,

        /// <summary>
        /// percent per metre
        /// A unit of proportion, equal to 0.01, in relation to a metre.
        /// </summary>
        H99,

        /// <summary>
        /// hank
        /// A unit of length, typically for yarn.
        /// </summary>
        HA,

        /// <summary>
        /// Piece Day
        /// </summary>
        HAD,

        /// <summary>
        /// hectobar
        /// </summary>
        HBA,

        /// <summary>
        /// hundred boxes
        /// A unit of count defining the number of boxes in multiples of one hundred box units.
        /// </summary>
        HBX,

        /// <summary>
        /// hundred count
        /// A unit of count defining the number of units counted in multiples of 100.
        /// </summary>
        HC,

        /// <summary>
        /// hundred kilogram, dry weight
        /// A unit of mass defining the number of hundred kilograms of a product, disregarding the water content of the product.
        /// </summary>
        HDW,

        /// <summary>
        /// head
        /// A unit of count defining the number of heads (head: a person or animal considered as one of a number).
        /// </summary>
        HEA,

        /// <summary>
        /// hectogram
        /// </summary>
        HGM,

        /// <summary>
        /// hundred cubic foot
        /// A unit of volume equal to one hundred cubic foot.
        /// </summary>
        HH,

        /// <summary>
        /// hundred international unit
        /// A unit of count defining the number of international units in multiples of 100.
        /// </summary>
        HIU,

        /// <summary>
        /// hundred kilogram, net mass
        /// A unit of mass defining the number of hundred kilograms of a product, after deductions.
        /// </summary>
        HKM,

        /// <summary>
        /// hectolitre
        /// </summary>
        HLT,

        /// <summary>
        /// mile per hour (statute mile)
        /// </summary>
        HM,

        /// <summary>
        /// Piece Month
        /// </summary>
        HMO,

        /// <summary>
        /// million cubic metre
        /// A unit of volume equal to one million cubic metres.
        /// </summary>
        HMQ,

        /// <summary>
        /// hectometre
        /// </summary>
        HMT,

        /// <summary>
        /// hectolitre of pure alcohol
        /// A unit of volume equal to one hundred litres of pure alcohol.
        /// </summary>
        HPA,

        /// <summary>
        /// hertz
        /// </summary>
        HTZ,

        /// <summary>
        /// hour
        /// </summary>
        HUR,

        /// <summary>
        /// inch pound (pound inch)
        /// </summary>
        IA,

        /// <summary>
        /// person
        /// A unit of count defining the number of persons.
        /// </summary>
        IE,

        /// <summary>
        /// inch
        /// </summary>
        INH,

        /// <summary>
        /// square inch
        /// </summary>
        INK,

        /// <summary>
        /// cubic inch
        /// Synonym: inch cubed
        /// </summary>
        INQ,

        /// <summary>
        /// international sugar degree
        /// A unit of measure defining the sugar content of a solution, expressed in degrees.
        /// </summary>
        ISD,

        /// <summary>
        /// inch per second
        /// </summary>
        IU,

        /// <summary>
        /// international unit per gram
        /// </summary>
        IUG,

        /// <summary>
        /// inch per second squared
        /// </summary>
        IV,

        /// <summary>
        /// percent per millimetre
        /// A unit of proportion, equal to 0.01, in relation to a millimetre.
        /// </summary>
        J10,

        /// <summary>
        /// per mille per psi
        /// A unit of pressure equal to one thousandth of a psi (pound-force per square inch).
        /// </summary>
        J12,

        /// <summary>
        /// degree API
        /// A unit of relative density as a measure of how heavy or light a petroleum liquid is compared to water (API: American Petroleum Institute).
        /// </summary>
        J13,

        /// <summary>
        /// degree Baume (origin scale)
        /// A traditional unit of relative density for liquids. Named after Antoine Baumé.
        /// </summary>
        J14,

        /// <summary>
        /// degree Baume (US heavy)
        /// A unit of relative density for liquids heavier than water.
        /// </summary>
        J15,

        /// <summary>
        /// degree Baume (US light)
        /// A unit of relative density for liquids lighter than water.
        /// </summary>
        J16,

        /// <summary>
        /// degree Balling
        /// A unit of density as a measure of sugar content, especially of beer wort. Named after Karl Balling.
        /// </summary>
        J17,

        /// <summary>
        /// degree Brix
        /// A unit of proportion used in measuring the dissolved sugar-to-water mass ratio of a liquid. Named after Adolf Brix.
        /// </summary>
        J18,

        /// <summary>
        /// degree Fahrenheit hour square foot per British thermal unit (thermochemical)
        /// </summary>
        J19,

        /// <summary>
        /// joule per kilogram
        /// </summary>
        J2,

        /// <summary>
        /// degree Fahrenheit per kelvin
        /// </summary>
        J20,

        /// <summary>
        /// degree Fahrenheit per bar
        /// </summary>
        J21,

        /// <summary>
        /// degree Fahrenheit hour square foot per British thermal unit (international table)
        /// </summary>
        J22,

        /// <summary>
        /// degree Fahrenheit per hour
        /// </summary>
        J23,

        /// <summary>
        /// degree Fahrenheit per minute
        /// </summary>
        J24,

        /// <summary>
        /// degree Fahrenheit per second
        /// </summary>
        J25,

        /// <summary>
        /// reciprocal degree Fahrenheit
        /// </summary>
        J26,

        /// <summary>
        /// degree Oechsle
        /// A unit of density as a measure of sugar content of must, the unfermented liqueur from which wine is made. Named after Ferdinand Oechsle.
        /// </summary>
        J27,

        /// <summary>
        /// degree Rankine per hour
        /// </summary>
        J28,

        /// <summary>
        /// degree Rankine per minute
        /// </summary>
        J29,

        /// <summary>
        /// degree Rankine per second
        /// </summary>
        J30,

        /// <summary>
        /// degree Twaddell
        /// A unit of density for liquids that are heavier than water. 1 degree Twaddle represents a difference in specific gravity of 0.005.
        /// </summary>
        J31,

        /// <summary>
        /// micropoise
        /// </summary>
        J32,

        /// <summary>
        /// microgram per kilogram
        /// </summary>
        J33,

        /// <summary>
        /// microgram per cubic metre kelvin
        /// </summary>
        J34,

        /// <summary>
        /// microgram per cubic metre bar
        /// </summary>
        J35,

        /// <summary>
        /// microlitre per litre
        /// </summary>
        J36,

        /// <summary>
        /// baud
        /// A unit of signal transmission speed equal to one signalling event per second.
        /// </summary>
        J38,

        /// <summary>
        /// British thermal unit (mean)
        /// </summary>
        J39,

        /// <summary>
        /// British thermal unit (international table) foot per hour square foot degree Fahrenheit
        /// </summary>
        J40,

        /// <summary>
        /// British thermal unit (international table) inch per hour square foot degree Fahrenheit
        /// </summary>
        J41,

        /// <summary>
        /// British thermal unit (international table) inch per second square foot degree Fahrenheit
        /// </summary>
        J42,

        /// <summary>
        /// British thermal unit (international table) per pound degree Fahrenheit
        /// </summary>
        J43,

        /// <summary>
        /// British thermal unit (international table) per minute
        /// </summary>
        J44,

        /// <summary>
        /// British thermal unit (international table) per second
        /// </summary>
        J45,

        /// <summary>
        /// British thermal unit (thermochemical) foot per hour square foot degree Fahrenheit
        /// </summary>
        J46,

        /// <summary>
        /// British thermal unit (thermochemical) per hour
        /// </summary>
        J47,

        /// <summary>
        /// British thermal unit (thermochemical) inch per hour square foot degree Fahrenheit
        /// </summary>
        J48,

        /// <summary>
        /// British thermal unit (thermochemical) inch per second square foot degree Fahrenheit
        /// </summary>
        J49,

        /// <summary>
        /// British thermal unit (thermochemical) per pound degree Fahrenheit
        /// </summary>
        J50,

        /// <summary>
        /// British thermal unit (thermochemical) per minute
        /// </summary>
        J51,

        /// <summary>
        /// British thermal unit (thermochemical) per second
        /// </summary>
        J52,

        /// <summary>
        /// coulomb square metre per kilogram
        /// </summary>
        J53,

        /// <summary>
        /// megabaud
        /// A unit of signal transmission speed equal to 10⁶ (1000000) signaling events per second.
        /// </summary>
        J54,

        /// <summary>
        /// watt second
        /// </summary>
        J55,

        /// <summary>
        /// bar per bar
        /// </summary>
        J56,

        /// <summary>
        /// barrel (UK petroleum)
        /// </summary>
        J57,

        /// <summary>
        /// barrel (UK petroleum) per minute
        /// </summary>
        J58,

        /// <summary>
        /// barrel (UK petroleum) per day
        /// </summary>
        J59,

        /// <summary>
        /// barrel (UK petroleum) per hour
        /// </summary>
        J60,

        /// <summary>
        /// barrel (UK petroleum) per second
        /// </summary>
        J61,

        /// <summary>
        /// barrel (US petroleum) per hour
        /// </summary>
        J62,

        /// <summary>
        /// barrel (US petroleum) per second
        /// </summary>
        J63,

        /// <summary>
        /// bushel (UK) per day
        /// </summary>
        J64,

        /// <summary>
        /// bushel (UK) per hour
        /// </summary>
        J65,

        /// <summary>
        /// bushel (UK) per minute
        /// </summary>
        J66,

        /// <summary>
        /// bushel (UK) per second
        /// </summary>
        J67,

        /// <summary>
        /// bushel (US dry) per day
        /// </summary>
        J68,

        /// <summary>
        /// bushel (US dry) per hour
        /// </summary>
        J69,

        /// <summary>
        /// bushel (US dry) per minute
        /// </summary>
        J70,

        /// <summary>
        /// bushel (US dry) per second
        /// </summary>
        J71,

        /// <summary>
        /// centinewton metre
        /// </summary>
        J72,

        /// <summary>
        /// centipoise per kelvin
        /// </summary>
        J73,

        /// <summary>
        /// centipoise per bar
        /// </summary>
        J74,

        /// <summary>
        /// calorie (mean)
        /// </summary>
        J75,

        /// <summary>
        /// calorie (international table) per gram degree Celsius
        /// </summary>
        J76,

        /// <summary>
        /// calorie (thermochemical) per centimetre second degree Celsius
        /// </summary>
        J78,

        /// <summary>
        /// calorie (thermochemical) per gram degree Celsius
        /// </summary>
        J79,

        /// <summary>
        /// calorie (thermochemical) per minute
        /// </summary>
        J81,

        /// <summary>
        /// calorie (thermochemical) per second
        /// </summary>
        J82,

        /// <summary>
        /// clo
        /// </summary>
        J83,

        /// <summary>
        /// centimetre per second kelvin
        /// </summary>
        J84,

        /// <summary>
        /// centimetre per second bar
        /// </summary>
        J85,

        /// <summary>
        /// cubic centimetre per cubic metre
        /// </summary>
        J87,

        /// <summary>
        /// cubic decimetre per day
        /// </summary>
        J90,

        /// <summary>
        /// cubic decimetre per cubic metre
        /// </summary>
        J91,

        /// <summary>
        /// cubic decimetre per minute
        /// </summary>
        J92,

        /// <summary>
        /// cubic decimetre per second
        /// </summary>
        J93,

        /// <summary>
        /// ounce (UK fluid) per day
        /// </summary>
        J95,

        /// <summary>
        /// ounce (UK fluid) per hour
        /// </summary>
        J96,

        /// <summary>
        /// ounce (UK fluid) per minute
        /// </summary>
        J97,

        /// <summary>
        /// ounce (UK fluid) per second
        /// </summary>
        J98,

        /// <summary>
        /// ounce (US fluid) per day
        /// </summary>
        J99,

        /// <summary>
        /// joule per kelvin
        /// </summary>
        JE,

        /// <summary>
        /// megajoule per kilogram
        /// </summary>
        JK,

        /// <summary>
        /// megajoule per cubic metre
        /// </summary>
        JM,

        /// <summary>
        /// pipeline joint
        /// A count of the number of pipeline joints.
        /// </summary>
        JNT,

        /// <summary>
        /// joule
        /// </summary>
        JOU,

        /// <summary>
        /// hundred metre
        /// A unit of count defining the number of 100 metre lengths.
        /// </summary>
        JPS,

        /// <summary>
        /// number of jewels
        /// A unit of count defining the number of jewels (jewel: precious stone).
        /// </summary>
        JWL,

        /// <summary>
        /// kilowatt demand
        /// A unit of measure defining the power load measured at predetermined intervals.
        /// </summary>
        K1,

        /// <summary>
        /// ounce (US fluid) per hour
        /// </summary>
        K10,

        /// <summary>
        /// ounce (US fluid) per minute
        /// </summary>
        K11,

        /// <summary>
        /// ounce (US fluid) per second
        /// </summary>
        K12,

        /// <summary>
        /// foot per degree Fahrenheit
        /// </summary>
        K13,

        /// <summary>
        /// foot per hour
        /// </summary>
        K14,

        /// <summary>
        /// foot pound-force per hour
        /// </summary>
        K15,

        /// <summary>
        /// foot pound-force per minute
        /// </summary>
        K16,

        /// <summary>
        /// foot per psi
        /// </summary>
        K17,

        /// <summary>
        /// foot per second degree Fahrenheit
        /// </summary>
        K18,

        /// <summary>
        /// foot per second psi
        /// </summary>
        K19,

        /// <summary>
        /// kilovolt ampere reactive demand
        /// A unit of measure defining the reactive power demand equal to one kilovolt ampere of reactive power.
        /// </summary>
        K2,

        /// <summary>
        /// reciprocal cubic foot
        /// </summary>
        K20,

        /// <summary>
        /// cubic foot per degree Fahrenheit
        /// </summary>
        K21,

        /// <summary>
        /// cubic foot per day
        /// </summary>
        K22,

        /// <summary>
        /// cubic foot per psi
        /// </summary>
        K23,

        /// <summary>
        /// gallon (UK) per day
        /// </summary>
        K26,

        /// <summary>
        /// gallon (UK) per hour
        /// </summary>
        K27,

        /// <summary>
        /// gallon (UK) per second
        /// </summary>
        K28,

        /// <summary>
        /// kilovolt ampere reactive hour
        /// A unit of measure defining the accumulated reactive energy equal to one kilovolt ampere of reactive power per hour.
        /// </summary>
        K3,

        /// <summary>
        /// gallon (US liquid) per second
        /// </summary>
        K30,

        /// <summary>
        /// gram-force per square centimetre
        /// </summary>
        K31,

        /// <summary>
        /// gill (UK) per day
        /// </summary>
        K32,

        /// <summary>
        /// gill (UK) per hour
        /// </summary>
        K33,

        /// <summary>
        /// gill (UK) per minute
        /// </summary>
        K34,

        /// <summary>
        /// gill (UK) per second
        /// </summary>
        K35,

        /// <summary>
        /// gill (US) per day
        /// </summary>
        K36,

        /// <summary>
        /// gill (US) per hour
        /// </summary>
        K37,

        /// <summary>
        /// gill (US) per minute
        /// </summary>
        K38,

        /// <summary>
        /// gill (US) per second
        /// </summary>
        K39,

        /// <summary>
        /// standard acceleration of free fall
        /// </summary>
        K40,

        /// <summary>
        /// grain per gallon (US)
        /// </summary>
        K41,

        /// <summary>
        /// horsepower (boiler)
        /// </summary>
        K42,

        /// <summary>
        /// horsepower (electric)
        /// </summary>
        K43,

        /// <summary>
        /// inch per degree Fahrenheit
        /// </summary>
        K45,

        /// <summary>
        /// inch per psi
        /// </summary>
        K46,

        /// <summary>
        /// inch per second degree Fahrenheit
        /// </summary>
        K47,

        /// <summary>
        /// inch per second psi
        /// </summary>
        K48,

        /// <summary>
        /// reciprocal cubic inch
        /// </summary>
        K49,

        /// <summary>
        /// kilobaud
        /// A unit of signal transmission speed equal to 10³ (1000) signaling events per second.
        /// </summary>
        K50,

        /// <summary>
        /// kilocalorie (mean)
        /// </summary>
        K51,

        /// <summary>
        /// kilocalorie (international table) per hour metre degree Celsius
        /// </summary>
        K52,

        /// <summary>
        /// kilocalorie (thermochemical)
        /// </summary>
        K53,

        /// <summary>
        /// kilocalorie (thermochemical) per minute
        /// </summary>
        K54,

        /// <summary>
        /// kilocalorie (thermochemical) per second
        /// </summary>
        K55,

        /// <summary>
        /// kilomole per hour
        /// </summary>
        K58,

        /// <summary>
        /// kilomole per cubic metre kelvin
        /// </summary>
        K59,

        /// <summary>
        /// kilolitre
        /// </summary>
        K6,

        /// <summary>
        /// kilomole per cubic metre bar
        /// </summary>
        K60,

        /// <summary>
        /// kilomole per minute
        /// </summary>
        K61,

        /// <summary>
        /// litre per litre
        /// </summary>
        K62,

        /// <summary>
        /// reciprocal litre
        /// </summary>
        K63,

        /// <summary>
        /// pound (avoirdupois) per degree Fahrenheit
        /// </summary>
        K64,

        /// <summary>
        /// pound (avoirdupois) square foot
        /// </summary>
        K65,

        /// <summary>
        /// pound (avoirdupois) per day
        /// </summary>
        K66,

        /// <summary>
        /// pound per foot hour
        /// </summary>
        K67,

        /// <summary>
        /// pound per foot second
        /// </summary>
        K68,

        /// <summary>
        /// pound (avoirdupois) per cubic foot degree Fahrenheit
        /// </summary>
        K69,

        /// <summary>
        /// pound (avoirdupois) per cubic foot psi
        /// </summary>
        K70,

        /// <summary>
        /// pound (avoirdupois) per gallon (UK)
        /// </summary>
        K71,

        /// <summary>
        /// pound (avoirdupois) per hour degree Fahrenheit
        /// </summary>
        K73,

        /// <summary>
        /// pound (avoirdupois) per hour psi
        /// </summary>
        K74,

        /// <summary>
        /// pound (avoirdupois) per cubic inch degree Fahrenheit
        /// </summary>
        K75,

        /// <summary>
        /// pound (avoirdupois) per cubic inch psi
        /// </summary>
        K76,

        /// <summary>
        /// pound (avoirdupois) per psi
        /// </summary>
        K77,

        /// <summary>
        /// pound (avoirdupois) per minute
        /// </summary>
        K78,

        /// <summary>
        /// pound (avoirdupois) per minute degree Fahrenheit
        /// </summary>
        K79,

        /// <summary>
        /// pound (avoirdupois) per minute psi
        /// </summary>
        K80,

        /// <summary>
        /// pound (avoirdupois) per second
        /// </summary>
        K81,

        /// <summary>
        /// pound (avoirdupois) per second degree Fahrenheit
        /// </summary>
        K82,

        /// <summary>
        /// pound (avoirdupois) per second psi
        /// </summary>
        K83,

        /// <summary>
        /// pound per cubic yard
        /// </summary>
        K84,

        /// <summary>
        /// pound-force per square foot
        /// </summary>
        K85,

        /// <summary>
        /// pound-force per square inch degree Fahrenheit
        /// </summary>
        K86,

        /// <summary>
        /// psi cubic inch per second
        /// </summary>
        K87,

        /// <summary>
        /// psi litre per second
        /// </summary>
        K88,

        /// <summary>
        /// psi cubic metre per second
        /// </summary>
        K89,

        /// <summary>
        /// psi cubic yard per second
        /// </summary>
        K90,

        /// <summary>
        /// pound-force second per square foot
        /// </summary>
        K91,

        /// <summary>
        /// pound-force second per square inch
        /// </summary>
        K92,

        /// <summary>
        /// reciprocal psi
        /// </summary>
        K93,

        /// <summary>
        /// quart (UK liquid) per day
        /// </summary>
        K94,

        /// <summary>
        /// quart (UK liquid) per hour
        /// </summary>
        K95,

        /// <summary>
        /// quart (UK liquid) per minute
        /// </summary>
        K96,

        /// <summary>
        /// quart (UK liquid) per second
        /// </summary>
        K97,

        /// <summary>
        /// quart (US liquid) per day
        /// </summary>
        K98,

        /// <summary>
        /// quart (US liquid) per hour
        /// </summary>
        K99,

        /// <summary>
        /// cake
        /// A unit of count defining the number of cakes (cake: object shaped into a flat, compact mass).
        /// </summary>
        KA,

        /// <summary>
        /// katal
        /// A unit of catalytic activity defining the catalytic activity of enzymes and other catalysts.
        /// </summary>
        KAT,

        /// <summary>
        /// kilocharacter
        /// A unit of information equal to 10³ (1000) characters.
        /// </summary>
        KB,

        /// <summary>
        /// kilobar
        /// </summary>
        KBA,

        /// <summary>
        /// kilogram of choline chloride
        /// A unit of mass equal to one thousand grams of choline chloride.
        /// </summary>
        KCC,

        /// <summary>
        /// kilogram drained net weight
        /// A unit of mass defining the net number of kilograms of a product, disregarding the liquid content of the product.
        /// </summary>
        KDW,

        /// <summary>
        /// kelvin
        /// Refer ISO 80000-5 (Quantities and units — Part 5: Thermodynamics)
        /// </summary>
        KEL,

        /// <summary>
        /// kilogram
        /// A unit of mass equal to one thousand grams.
        /// </summary>
        KGM,

        /// <summary>
        /// kilogram per second
        /// </summary>
        KGS,

        /// <summary>
        /// kilogram of hydrogen peroxide
        /// A unit of mass equal to one thousand grams of hydrogen peroxide.
        /// </summary>
        KHY,

        /// <summary>
        /// kilohertz
        /// </summary>
        KHZ,

        /// <summary>
        /// kilogram per millimetre width
        /// </summary>
        KI,

        /// <summary>
        /// kilogram, including container
        /// A unit of mass defining the number of kilograms of a product, including its container.
        /// </summary>
        KIC,

        /// <summary>
        /// kilogram, including inner packaging
        /// A unit of mass defining the number of kilograms of a product, including its inner packaging materials.
        /// </summary>
        KIP,

        /// <summary>
        /// kilosegment
        /// A unit of information equal to 10³ (1000) segments.
        /// </summary>
        KJ,

        /// <summary>
        /// kilojoule
        /// </summary>
        KJO,

        /// <summary>
        /// kilogram per metre
        /// </summary>
        KL,

        /// <summary>
        /// lactic dry material percentage
        /// A unit of proportion defining the percentage of dry lactic material in a product.
        /// </summary>
        KLK,

        /// <summary>
        /// kilolux
        /// A unit of illuminance equal to one thousand lux.
        /// </summary>
        KLX,

        /// <summary>
        /// kilogram of methylamine
        /// A unit of mass equal to one thousand grams of methylamine.
        /// </summary>
        KMA,

        /// <summary>
        /// kilometre per hour
        /// </summary>
        KMH,

        /// <summary>
        /// square kilometre
        /// </summary>
        KMK,

        /// <summary>
        /// kilogram per cubic metre
        /// A unit of weight expressed in kilograms of a substance that fills a volume of one cubic metre.
        /// </summary>
        KMQ,

        /// <summary>
        /// kilometre
        /// </summary>
        KMT,

        /// <summary>
        /// kilogram of nitrogen
        /// A unit of mass equal to one thousand grams of nitrogen.
        /// </summary>
        KNI,

        /// <summary>
        /// kilonewton per square metre
        /// Pressure expressed in kN/m2.
        /// </summary>
        KNM,

        /// <summary>
        /// kilogram named substance
        /// A unit of mass equal to one kilogram of a named substance.
        /// </summary>
        KNS,

        /// <summary>
        /// knot
        /// </summary>
        KNT,

        /// <summary>
        /// milliequivalence caustic potash per gram of product
        /// A unit of count defining the number of milligrams of potassium hydroxide per gram of product as a measure of the concentration of potassium hydroxide in the product.
        /// </summary>
        KO,

        /// <summary>
        /// kilopascal
        /// </summary>
        KPA,

        /// <summary>
        /// kilogram of potassium hydroxide (caustic potash)
        /// A unit of mass equal to one thousand grams of potassium hydroxide (caustic potash).
        /// </summary>
        KPH,

        /// <summary>
        /// kilogram of potassium oxide
        /// A unit of mass equal to one thousand grams of potassium oxide.
        /// </summary>
        KPO,

        /// <summary>
        /// kilogram of phosphorus pentoxide (phosphoric anhydride)
        /// A unit of mass equal to one thousand grams of phosphorus pentoxide phosphoric anhydride.
        /// </summary>
        KPP,

        /// <summary>
        /// kiloroentgen
        /// </summary>
        KR,

        /// <summary>
        /// kilogram of substance 90 % dry
        /// A unit of mass equal to one thousand grams of a named substance that is 90% dry.
        /// </summary>
        KSD,

        /// <summary>
        /// kilogram of sodium hydroxide (caustic soda)
        /// A unit of mass equal to one thousand grams of sodium hydroxide (caustic soda).
        /// </summary>
        KSH,

        /// <summary>
        /// kit
        /// A unit of count defining the number of kits (kit: tub, barrel or pail).
        /// </summary>
        KT,

        /// <summary>
        /// kilotonne
        /// </summary>
        KTN,

        /// <summary>
        /// kilogram of uranium
        /// A unit of mass equal to one thousand grams of uranium.
        /// </summary>
        KUR,

        /// <summary>
        /// kilovolt - ampere
        /// </summary>
        KVA,

        /// <summary>
        /// kilovar
        /// </summary>
        KVR,

        /// <summary>
        /// kilovolt
        /// </summary>
        KVT,

        /// <summary>
        /// kilogram per millimetre
        /// </summary>
        KW,

        /// <summary>
        /// kilowatt hour
        /// </summary>
        KWH,

        /// <summary>
        /// Kilowatt hour per normalized cubic metre
        /// </summary>
        KWN,

        /// <summary>
        /// kilogram of tungsten trioxide
        /// A unit of mass equal to one thousand grams of tungsten trioxide.
        /// </summary>
        KWO,

        /// <summary>
        /// Kilowatt hour per standard cubic metre
        /// </summary>
        KWS,

        /// <summary>
        /// kilowatt
        /// </summary>
        KWT,

        /// <summary>
        /// kilowatt year
        /// </summary>
        KWY,

        /// <summary>
        /// millilitre per kilogram
        /// </summary>
        KX,

        /// <summary>
        /// quart (US liquid) per minute
        /// </summary>
        L10,

        /// <summary>
        /// quart (US liquid) per second
        /// </summary>
        L11,

        /// <summary>
        /// metre per second kelvin
        /// </summary>
        L12,

        /// <summary>
        /// metre per second bar
        /// </summary>
        L13,

        /// <summary>
        /// square metre hour degree Celsius per kilocalorie (international table)
        /// </summary>
        L14,

        /// <summary>
        /// millipascal second per kelvin
        /// </summary>
        L15,

        /// <summary>
        /// millipascal second per bar
        /// </summary>
        L16,

        /// <summary>
        /// milligram per cubic metre kelvin
        /// </summary>
        L17,

        /// <summary>
        /// milligram per cubic metre bar
        /// </summary>
        L18,

        /// <summary>
        /// millilitre per litre
        /// </summary>
        L19,

        /// <summary>
        /// litre per minute
        /// </summary>
        L2,

        /// <summary>
        /// reciprocal cubic millimetre
        /// </summary>
        L20,

        /// <summary>
        /// cubic millimetre per cubic metre
        /// </summary>
        L21,

        /// <summary>
        /// mole per hour
        /// </summary>
        L23,

        /// <summary>
        /// mole per kilogram kelvin
        /// </summary>
        L24,

        /// <summary>
        /// mole per kilogram bar
        /// </summary>
        L25,

        /// <summary>
        /// mole per litre kelvin
        /// </summary>
        L26,

        /// <summary>
        /// mole per litre bar
        /// </summary>
        L27,

        /// <summary>
        /// mole per cubic metre kelvin
        /// </summary>
        L28,

        /// <summary>
        /// mole per cubic metre bar
        /// </summary>
        L29,

        /// <summary>
        /// mole per minute
        /// </summary>
        L30,

        /// <summary>
        /// milliroentgen aequivalent men
        /// </summary>
        L31,

        /// <summary>
        /// nanogram per kilogram
        /// </summary>
        L32,

        /// <summary>
        /// ounce (avoirdupois) per day
        /// </summary>
        L33,

        /// <summary>
        /// ounce (avoirdupois) per hour
        /// </summary>
        L34,

        /// <summary>
        /// ounce (avoirdupois) per minute
        /// </summary>
        L35,

        /// <summary>
        /// ounce (avoirdupois) per second
        /// </summary>
        L36,

        /// <summary>
        /// ounce (avoirdupois) per gallon (UK)
        /// </summary>
        L37,

        /// <summary>
        /// ounce (avoirdupois) per gallon (US)
        /// </summary>
        L38,

        /// <summary>
        /// ounce (avoirdupois) per cubic inch
        /// </summary>
        L39,

        /// <summary>
        /// ounce (avoirdupois)-force
        /// </summary>
        L40,

        /// <summary>
        /// ounce (avoirdupois)-force inch
        /// </summary>
        L41,

        /// <summary>
        /// picosiemens per metre
        /// </summary>
        L42,

        /// <summary>
        /// peck (UK)
        /// </summary>
        L43,

        /// <summary>
        /// peck (UK) per day
        /// </summary>
        L44,

        /// <summary>
        /// peck (UK) per hour
        /// </summary>
        L45,

        /// <summary>
        /// peck (UK) per minute
        /// </summary>
        L46,

        /// <summary>
        /// peck (UK) per second
        /// </summary>
        L47,

        /// <summary>
        /// peck (US dry) per day
        /// </summary>
        L48,

        /// <summary>
        /// peck (US dry) per hour
        /// </summary>
        L49,

        /// <summary>
        /// peck (US dry) per minute
        /// </summary>
        L50,

        /// <summary>
        /// peck (US dry) per second
        /// </summary>
        L51,

        /// <summary>
        /// psi per psi
        /// </summary>
        L52,

        /// <summary>
        /// pint (UK) per day
        /// </summary>
        L53,

        /// <summary>
        /// pint (UK) per hour
        /// </summary>
        L54,

        /// <summary>
        /// pint (UK) per minute
        /// </summary>
        L55,

        /// <summary>
        /// pint (UK) per second
        /// </summary>
        L56,

        /// <summary>
        /// pint (US liquid) per day
        /// </summary>
        L57,

        /// <summary>
        /// pint (US liquid) per hour
        /// </summary>
        L58,

        /// <summary>
        /// pint (US liquid) per minute
        /// </summary>
        L59,

        /// <summary>
        /// pint (US liquid) per second
        /// </summary>
        L60,

        /// <summary>
        /// slug per day
        /// </summary>
        L63,

        /// <summary>
        /// slug per foot second
        /// </summary>
        L64,

        /// <summary>
        /// slug per cubic foot
        /// </summary>
        L65,

        /// <summary>
        /// slug per hour
        /// </summary>
        L66,

        /// <summary>
        /// slug per minute
        /// </summary>
        L67,

        /// <summary>
        /// slug per second
        /// </summary>
        L68,

        /// <summary>
        /// tonne per kelvin
        /// </summary>
        L69,

        /// <summary>
        /// tonne per bar
        /// </summary>
        L70,

        /// <summary>
        /// tonne per day
        /// </summary>
        L71,

        /// <summary>
        /// tonne per day kelvin
        /// </summary>
        L72,

        /// <summary>
        /// tonne per day bar
        /// </summary>
        L73,

        /// <summary>
        /// tonne per hour kelvin
        /// </summary>
        L74,

        /// <summary>
        /// tonne per hour bar
        /// </summary>
        L75,

        /// <summary>
        /// tonne per cubic metre kelvin
        /// </summary>
        L76,

        /// <summary>
        /// tonne per cubic metre bar
        /// </summary>
        L77,

        /// <summary>
        /// tonne per minute
        /// </summary>
        L78,

        /// <summary>
        /// tonne per minute kelvin
        /// </summary>
        L79,

        /// <summary>
        /// tonne per minute bar
        /// </summary>
        L80,

        /// <summary>
        /// tonne per second
        /// </summary>
        L81,

        /// <summary>
        /// tonne per second kelvin
        /// </summary>
        L82,

        /// <summary>
        /// tonne per second bar
        /// </summary>
        L83,

        /// <summary>
        /// ton (UK shipping)
        /// </summary>
        L84,

        /// <summary>
        /// ton long per day
        /// </summary>
        L85,

        /// <summary>
        /// ton (US shipping)
        /// </summary>
        L86,

        /// <summary>
        /// ton short per degree Fahrenheit
        /// </summary>
        L87,

        /// <summary>
        /// ton short per day
        /// </summary>
        L88,

        /// <summary>
        /// ton short per hour degree Fahrenheit
        /// </summary>
        L89,

        /// <summary>
        /// ton short per hour psi
        /// </summary>
        L90,

        /// <summary>
        /// ton short per psi
        /// </summary>
        L91,

        /// <summary>
        /// ton (UK long) per cubic yard
        /// </summary>
        L92,

        /// <summary>
        /// ton (US short) per cubic yard
        /// </summary>
        L93,

        /// <summary>
        /// ton-force (US short)
        /// </summary>
        L94,

        /// <summary>
        /// common year
        /// </summary>
        L95,

        /// <summary>
        /// sidereal year
        /// </summary>
        L96,

        /// <summary>
        /// yard per degree Fahrenheit
        /// </summary>
        L98,

        /// <summary>
        /// yard per psi
        /// </summary>
        L99,

        /// <summary>
        /// pound per cubic inch
        /// </summary>
        LA,

        /// <summary>
        /// lactose excess percentage
        /// A unit of proportion defining the percentage of lactose in a product that exceeds a defined percentage level.
        /// </summary>
        LAC,

        /// <summary>
        /// pound
        /// </summary>
        LBR,

        /// <summary>
        /// troy pound (US)
        /// </summary>
        LBT,

        /// <summary>
        /// litre per day
        /// </summary>
        LD,

        /// <summary>
        /// leaf
        /// A unit of count defining the number of leaves.
        /// </summary>
        LEF,

        /// <summary>
        /// linear foot
        /// A unit of count defining the number of feet (12-inch) in length of a uniform width object.
        /// </summary>
        LF,

        /// <summary>
        /// labour hour
        /// A unit of time defining the number of labour hours.
        /// </summary>
        LH,

        /// <summary>
        /// link
        /// A unit of distance equal to 0.01 chain.
        /// </summary>
        LK,

        /// <summary>
        /// linear metre
        /// A unit of count defining the number of metres in length of a uniform width object.
        /// </summary>
        LM,

        /// <summary>
        /// length
        /// A unit of distance defining the linear extent of an item measured from end to end.
        /// </summary>
        LN,

        /// <summary>
        /// lot [unit of procurement]
        /// A unit of count defining the number of lots (lot: a collection of associated items).
        /// </summary>
        LO,

        /// <summary>
        /// liquid pound
        /// A unit of mass defining the number of pounds of a liquid substance.
        /// </summary>
        LP,

        /// <summary>
        /// litre of pure alcohol
        /// A unit of volume equal to one litre of pure alcohol.
        /// </summary>
        LPA,

        /// <summary>
        /// layer
        /// A unit of count defining the number of layers.
        /// </summary>
        LR,

        /// <summary>
        /// lump sum
        /// A unit of count defining the number of whole or a complete monetary amounts.
        /// </summary>
        LS,

        /// <summary>
        /// ton (UK) or long ton (US)
        /// Synonym: gross ton (2240 lb)
        /// </summary>
        LTN,

        /// <summary>
        /// litre
        /// </summary>
        LTR,

        /// <summary>
        /// metric ton, lubricating oil
        /// A unit of mass defining the number of metric tons of lubricating oil.
        /// </summary>
        LUB,

        /// <summary>
        /// lumen
        /// </summary>
        LUM,

        /// <summary>
        /// lux
        /// </summary>
        LUX,

        /// <summary>
        /// linear yard
        /// A unit of count defining the number of 36-inch units in length of a uniform width object.
        /// </summary>
        LY,

        /// <summary>
        /// milligram per litre
        /// </summary>
        M1,

        /// <summary>
        /// reciprocal cubic yard
        /// </summary>
        M10,

        /// <summary>
        /// cubic yard per degree Fahrenheit
        /// </summary>
        M11,

        /// <summary>
        /// cubic yard per day
        /// </summary>
        M12,

        /// <summary>
        /// cubic yard per hour
        /// </summary>
        M13,

        /// <summary>
        /// cubic yard per psi
        /// </summary>
        M14,

        /// <summary>
        /// cubic yard per minute
        /// </summary>
        M15,

        /// <summary>
        /// cubic yard per second
        /// </summary>
        M16,

        /// <summary>
        /// kilohertz metre
        /// </summary>
        M17,

        /// <summary>
        /// gigahertz metre
        /// </summary>
        M18,

        /// <summary>
        /// Beaufort
        /// An empirical measure for describing wind speed based mainly on observed sea conditions. The Beaufort scale indicates the wind speed by numbers that typically range from 0 for calm, to 12 for hurricane.
        /// </summary>
        M19,

        /// <summary>
        /// reciprocal megakelvin or megakelvin to the power minus one
        /// </summary>
        M20,

        /// <summary>
        /// reciprocal kilovolt - ampere reciprocal hour
        /// </summary>
        M21,

        /// <summary>
        /// millilitre per square centimetre minute
        /// </summary>
        M22,

        /// <summary>
        /// newton per centimetre
        /// </summary>
        M23,

        /// <summary>
        /// ohm kilometre
        /// </summary>
        M24,

        /// <summary>
        /// percent per degree Celsius
        /// A unit of proportion, equal to 0.01, in relation to a temperature of one degree.
        /// </summary>
        M25,

        /// <summary>
        /// gigaohm per metre
        /// </summary>
        M26,

        /// <summary>
        /// megahertz metre
        /// </summary>
        M27,

        /// <summary>
        /// kilogram per kilogram
        /// </summary>
        M29,

        /// <summary>
        /// reciprocal volt - ampere reciprocal second
        /// </summary>
        M30,

        /// <summary>
        /// kilogram per kilometre
        /// </summary>
        M31,

        /// <summary>
        /// pascal second per litre
        /// </summary>
        M32,

        /// <summary>
        /// millimole per litre
        /// </summary>
        M33,

        /// <summary>
        /// newton metre per square metre
        /// </summary>
        M34,

        /// <summary>
        /// millivolt - ampere
        /// </summary>
        M35,

        /// <summary>
        /// 30-day month
        /// A unit of count defining the number of months expressed in multiples of 30 days, one day equals 24 hours.
        /// </summary>
        M36,

        /// <summary>
        /// actual/360
        /// A unit of count defining the number of years expressed in multiples of 360 days, one day equals 24 hours.
        /// </summary>
        M37,

        /// <summary>
        /// kilometre per second squared
        /// 1000-fold of the SI base unit metre divided by the power of the SI base unit second by exponent 2.
        /// </summary>
        M38,

        /// <summary>
        /// centimetre per second squared
        /// 0,01-fold of the SI base unit metre divided by the power of the SI base unit second by exponent 2.
        /// </summary>
        M39,

        /// <summary>
        /// monetary value
        /// A unit of measure expressed as a monetary amount.
        /// </summary>
        M4,

        /// <summary>
        /// yard per second squared
        /// Unit of the length according to the Anglo-American and Imperial system of units divided by the power of the SI base unit second by exponent 2.
        /// </summary>
        M40,

        /// <summary>
        /// millimetre per second squared
        /// 0,001-fold of the SI base unit metre divided by the power of the SI base unit second by exponent 2.
        /// </summary>
        M41,

        /// <summary>
        /// mile (statute mile) per second squared
        /// Unit of the length according to the Imperial system of units divided by the power of the SI base unit second by exponent 2.
        /// </summary>
        M42,

        /// <summary>
        /// mil
        /// Unit to indicate an angle at military zone, equal to the 6400th part of the full circle of the 360° or 2·p·rad.
        /// </summary>
        M43,

        /// <summary>
        /// revolution
        /// Unit to identify an angle of the full circle of 360° or 2·p·rad (Refer ISO/TC12 SI Guide).
        /// </summary>
        M44,

        /// <summary>
        /// degree [unit of angle] per second squared
        /// 360 part of a full circle divided by the power of the SI base unit second and the exponent 2.
        /// </summary>
        M45,

        /// <summary>
        /// revolution per minute
        /// Unit of the angular velocity.
        /// </summary>
        M46,

        /// <summary>
        /// circular mil
        /// Unit of an area, of which the size is given by a diameter of length of 1 mm (0,001 in) based on the formula: area = p·(diameter/2)².
        /// </summary>
        M47,

        /// <summary>
        /// square mile (based on U.S. survey foot)
        /// Unit of the area, which is mainly common in the agriculture and forestry.
        /// </summary>
        M48,

        /// <summary>
        /// chain (based on U.S. survey foot)
        /// Unit of the length according the Anglo-American system of units.
        /// </summary>
        M49,

        /// <summary>
        /// microcurie
        /// </summary>
        M5,

        /// <summary>
        /// furlong
        /// Unit commonly used in Great Britain at rural distances: 1 furlong = 40 rods = 10 chains (UK) = 1/8 mile = 1/10 furlong = 220 yards = 660 foot.
        /// </summary>
        M50,

        /// <summary>
        /// foot (U.S. survey)
        /// Unit commonly used in the United States for ordnance survey.
        /// </summary>
        M51,

        /// <summary>
        /// mile (based on U.S. survey foot)
        /// Unit commonly used in the United States for ordnance survey.
        /// </summary>
        M52,

        /// <summary>
        /// metre per pascal
        /// SI base unit metre divided by the derived SI unit pascal.
        /// </summary>
        M53,

        /// <summary>
        /// metre per radiant
        /// Unit of the translation factor for implementation from rotation to linear movement.
        /// </summary>
        M55,

        /// <summary>
        /// shake
        /// Unit for a very short period.
        /// </summary>
        M56,

        /// <summary>
        /// mile per minute
        /// Unit of velocity from the Imperial system of units.
        /// </summary>
        M57,

        /// <summary>
        /// mile per second
        /// Unit of the velocity from the Imperial system of units.
        /// </summary>
        M58,

        /// <summary>
        /// metre per second pascal
        /// SI base unit meter divided by the product of SI base unit second and the derived SI unit pascal.
        /// </summary>
        M59,

        /// <summary>
        /// metre per hour
        /// SI base unit metre divided by the unit hour.
        /// </summary>
        M60,

        /// <summary>
        /// inch per year
        /// Unit of the length according to the Anglo-American and Imperial system of units divided by the unit common year with 365 days.
        /// </summary>
        M61,

        /// <summary>
        /// kilometre per second
        /// 1000-fold of the SI base unit metre divided by the SI base unit second.
        /// </summary>
        M62,

        /// <summary>
        /// inch per minute
        /// Unit inch according to the Anglo-American and Imperial system of units divided by the unit minute.
        /// </summary>
        M63,

        /// <summary>
        /// yard per second
        /// Unit yard according to the Anglo-American and Imperial system of units divided by the SI base unit second.
        /// </summary>
        M64,

        /// <summary>
        /// yard per minute
        /// Unit yard according to the Anglo-American and Imperial system of units divided by the unit minute.
        /// </summary>
        M65,

        /// <summary>
        /// yard per hour
        /// Unit yard according to the Anglo-American and Imperial system of units divided by the unit hour.
        /// </summary>
        M66,

        /// <summary>
        /// acre-foot (based on U.S. survey foot)
        /// Unit of the volume, which is used in the United States to measure/gauge the capacity of reservoirs.
        /// </summary>
        M67,

        /// <summary>
        /// cord (128 ft3)
        /// Traditional unit of the volume of stacked firewood which has been measured with a cord.
        /// </summary>
        M68,

        /// <summary>
        /// cubic mile (UK statute)
        /// Unit of volume according to the Imperial system of units.
        /// </summary>
        M69,

        /// <summary>
        /// micro-inch
        /// </summary>
        M7,

        /// <summary>
        /// ton, register
        /// Traditional unit of the cargo capacity.
        /// </summary>
        M70,

        /// <summary>
        /// cubic metre per pascal
        /// Power of the SI base unit meter by exponent 3 divided by the derived SI base unit pascal.
        /// </summary>
        M71,

        /// <summary>
        /// bel
        /// Logarithmic relationship to base 10.
        /// </summary>
        M72,

        /// <summary>
        /// kilogram per cubic metre pascal
        /// SI base unit kilogram divided by the product of the power of the SI base unit metre with exponent 3 and the derived SI unit pascal.
        /// </summary>
        M73,

        /// <summary>
        /// kilogram per pascal
        /// SI base unit kilogram divided by the derived SI unit pascal.
        /// </summary>
        M74,

        /// <summary>
        /// kilopound-force
        /// 1000-fold of the unit of the force pound-force (lbf) according to the Anglo-American system of units with the relationship.
        /// </summary>
        M75,

        /// <summary>
        /// poundal
        /// Non SI-conforming unit of the power, which corresponds to a mass of a pound multiplied with the acceleration of a foot per square second.
        /// </summary>
        M76,

        /// <summary>
        /// kilogram metre per second squared
        /// Product of the SI base unit kilogram and the SI base unit metre divided by the power of the SI base unit second by exponent 2.
        /// </summary>
        M77,

        /// <summary>
        /// pond
        /// 0,001-fold of the unit of the weight, defined as a mass of 1 kg which finds out about a weight strength from 1 kp by the gravitational force at sea level which corresponds to a strength of 9,806 65 newton.
        /// </summary>
        M78,

        /// <summary>
        /// square foot per hour
        /// Power of the unit foot according to the Anglo-American and Imperial system of units by exponent 2 divided by the unit of time hour.
        /// </summary>
        M79,

        /// <summary>
        /// stokes per pascal
        /// CGS (Centimetre-Gram-Second system) unit stokes divided by the derived SI unit pascal.
        /// </summary>
        M80,

        /// <summary>
        /// square centimetre per second
        /// 0,000 1-fold of the power of the SI base unit metre by exponent 2 divided by the SI base unit second.
        /// </summary>
        M81,

        /// <summary>
        /// square metre per second pascal
        /// Power of the SI base unit metre with the exponent 2 divided by the SI base unit second and the derived SI unit pascal.
        /// </summary>
        M82,

        /// <summary>
        /// denier
        /// Traditional unit for the indication of the linear mass of textile fibers and yarns.
        /// </summary>
        M83,

        /// <summary>
        /// pound per yard
        /// Unit for linear mass according to avoirdupois system of units.
        /// </summary>
        M84,

        /// <summary>
        /// ton, assay
        /// Non SI-conforming unit of the mass used in the mineralogy to determine the concentration of precious metals in ore according to the mass of the precious metal in milligrams in a sample of the mass of an assay sound (number of troy ounces in a short ton (1 000 lb)).
        /// </summary>
        M85,

        /// <summary>
        /// pfund
        /// Outdated unit of the mass used in Germany.
        /// </summary>
        M86,

        /// <summary>
        /// kilogram per second pascal
        /// SI base unit kilogram divided by the product of the SI base unit second and the derived SI unit pascal.
        /// </summary>
        M87,

        /// <summary>
        /// tonne per month
        /// Unit tonne divided by the unit month.
        /// </summary>
        M88,

        /// <summary>
        /// tonne per year
        /// Unit tonne divided by the unit year with 365 days.
        /// </summary>
        M89,

        /// <summary>
        /// million Btu per 1000 cubic foot
        /// </summary>
        M9,

        /// <summary>
        /// kilopound per hour
        /// 1000-fold of the unit of the mass avoirdupois pound according to the avoirdupois unit system divided by the unit hour.
        /// </summary>
        M90,

        /// <summary>
        /// pound per pound
        /// Proportion of the mass consisting of the avoirdupois pound according to the avoirdupois unit system divided by the avoirdupois pound according to the avoirdupois unit system.
        /// </summary>
        M91,

        /// <summary>
        /// pound-force foot
        /// Product of the unit pound-force according to the Anglo-American system of units and the unit foot according to the Anglo-American and the Imperial system of units.
        /// </summary>
        M92,

        /// <summary>
        /// newton metre per radian
        /// Product of the derived SI unit newton and the SI base unit metre divided by the unit radian.
        /// </summary>
        M93,

        /// <summary>
        /// kilogram metre
        /// Unit of imbalance as a product of the SI base unit kilogram and the SI base unit metre.
        /// </summary>
        M94,

        /// <summary>
        /// poundal foot
        /// Product of the non SI-conforming unit of the force poundal and the unit foot according to the Anglo-American and Imperial system of units .
        /// </summary>
        M95,

        /// <summary>
        /// poundal inch
        /// Product of the non SI-conforming unit of the force poundal and the unit inch according to the Anglo-American and Imperial system of units .
        /// </summary>
        M96,

        /// <summary>
        /// dyne metre
        /// CGS (Centimetre-Gram-Second system) unit of the rotational moment.
        /// </summary>
        M97,

        /// <summary>
        /// kilogram centimetre per second
        /// Product of the SI base unit kilogram and the 0,01-fold of the SI base unit metre divided by the SI base unit second.
        /// </summary>
        M98,

        /// <summary>
        /// gram centimetre per second
        /// Product of the 0,001-fold of the SI base unit kilogram and the 0,01-fold of the SI base unit metre divided by the SI base unit second.
        /// </summary>
        M99,

        /// <summary>
        /// megavolt ampere reactive hour
        /// A unit of electrical reactive power defining the total amount of reactive power across a power system.
        /// </summary>
        MAH,

        /// <summary>
        /// megalitre
        /// </summary>
        MAL,

        /// <summary>
        /// megametre
        /// </summary>
        MAM,

        /// <summary>
        /// megavar
        /// A unit of electrical reactive power represented by a current of one thousand amperes flowing due a potential difference of one thousand volts where the sine of the phase angle between them is 1.
        /// </summary>
        MAR,

        /// <summary>
        /// megawatt
        /// A unit of power defining the rate of energy transferred or consumed when a current of 1000 amperes flows due to a potential of 1000 volts at unity power factor.
        /// </summary>
        MAW,

        /// <summary>
        /// thousand standard brick equivalent
        /// A unit of count defining the number of one thousand brick equivalent units.
        /// </summary>
        MBE,

        /// <summary>
        /// thousand board foot
        /// A unit of volume equal to one thousand board foot.
        /// </summary>
        MBF,

        /// <summary>
        /// millibar
        /// </summary>
        MBR,

        /// <summary>
        /// microgram
        /// </summary>
        MC,

        /// <summary>
        /// millicurie
        /// </summary>
        MCU,

        /// <summary>
        /// air dry metric ton
        /// A unit of count defining the number of metric tons of a product, disregarding the water content of the product.
        /// </summary>
        MD,

        /// <summary>
        /// milligram
        /// </summary>
        MGM,

        /// <summary>
        /// megahertz
        /// </summary>
        MHZ,

        /// <summary>
        /// square mile (statute mile)
        /// </summary>
        MIK,

        /// <summary>
        /// thousand
        /// </summary>
        MIL,

        /// <summary>
        /// minute [unit of time]
        /// </summary>
        MIN,

        /// <summary>
        /// million
        /// </summary>
        MIO,

        /// <summary>
        /// million international unit
        /// A unit of count defining the number of international units in multiples of 10.
        /// </summary>
        MIU,

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
        /// milliard
        /// Synonym: billion (US)
        /// </summary>
        MLD,

        /// <summary>
        /// millilitre
        /// </summary>
        MLT,

        /// <summary>
        /// square millimetre
        /// </summary>
        MMK,

        /// <summary>
        /// cubic millimetre
        /// </summary>
        MMQ,

        /// <summary>
        /// millimetre
        /// </summary>
        MMT,

        /// <summary>
        /// kilogram, dry weight
        /// A unit of mass defining the number of kilograms of a product, disregarding the water content of the product.
        /// </summary>
        MND,

        /// <summary>
        /// Mega Joule per Normalised cubic Metre
        /// </summary>
        MNJ,

        /// <summary>
        /// month
        /// Unit of time equal to 1/12 of a year of 365,25 days.
        /// </summary>
        MON,

        /// <summary>
        /// megapascal
        /// </summary>
        MPA,

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
        /// Metre Day
        /// </summary>
        MRD,

        /// <summary>
        /// Metre Month
        /// </summary>
        MRM,

        /// <summary>
        /// Metre Week
        /// </summary>
        MRW,

        /// <summary>
        /// metre per second squared
        /// </summary>
        MSK,

        /// <summary>
        /// square metre
        /// </summary>
        MTK,

        /// <summary>
        /// cubic metre
        /// Synonym: metre cubed
        /// </summary>
        MTQ,

        /// <summary>
        /// metre
        /// </summary>
        MTR,

        /// <summary>
        /// metre per second
        /// </summary>
        MTS,

        /// <summary>
        /// milihertz
        /// </summary>
        MTZ,

        /// <summary>
        /// megavolt - ampere
        /// </summary>
        MVA,

        /// <summary>
        /// megawatt hour (1000 kW.h)
        /// A unit of power defining the total amount of bulk energy transferred or consumed.
        /// </summary>
        MWH,

        /// <summary>
        /// pen calorie
        /// A unit of count defining the number of calories prescribed daily for parenteral/enteral therapy.
        /// </summary>
        N1,

        /// <summary>
        /// pound foot per second
        /// Product of the avoirdupois pound according to the avoirdupois unit system and the unit foot according to the Anglo-American and Imperial system of units divided by the SI base unit second.
        /// </summary>
        N10,

        /// <summary>
        /// pound inch per second
        /// Product of the avoirdupois pound according to the avoirdupois unit system and the unit inch according to the Anglo-American and Imperial system of units divided by the SI base unit second.
        /// </summary>
        N11,

        /// <summary>
        /// Pferdestaerke
        /// Obsolete unit of the power relating to DIN 1301-3:1979: 1 PS = 735,498 75 W.
        /// </summary>
        N12,

        /// <summary>
        /// centimetre of mercury (0 ºC)
        /// Non SI-conforming unit of pressure, at which a value of 1 cmHg meets the static pressure, which is generated by a mercury at a temperature of 0 °C with a height of 1 centimetre .
        /// </summary>
        N13,

        /// <summary>
        /// centimetre of water (4 ºC)
        /// Non SI-conforming unit of pressure, at which a value of 1 cmH2O meets the static pressure, which is generated by a head of water at a temperature of 4 °C with a height of 1 centimetre .
        /// </summary>
        N14,

        /// <summary>
        /// foot of water (39.2 ºF)
        /// Non SI-conforming unit of pressure according to the Anglo-American and Imperial system for units, whereas the value of 1 ftH2O is equivalent to the static pressure, which is generated by a head of water at a temperature 39,2°F with a height of 1 foot .
        /// </summary>
        N15,

        /// <summary>
        /// inch of mercury (32 ºF)
        /// Non SI-conforming unit of pressure according to the Anglo-American and Imperial system for units, whereas the value of 1 inHg meets the static pressure, which is generated by a mercury at a temperature of 32°F with a height of 1 inch.
        /// </summary>
        N16,

        /// <summary>
        /// inch of mercury (60 ºF)
        /// Non SI-conforming unit of pressure according to the Anglo-American and Imperial system for units, whereas the value of 1 inHg meets the static pressure, which is generated by a mercury at a temperature of 60°F with a height of 1 inch.
        /// </summary>
        N17,

        /// <summary>
        /// inch of water (39.2 ºF)
        /// Non SI-conforming unit of pressure according to the Anglo-American and Imperial system for units, whereas the value of 1 inH2O meets the static pressure, which is generated by a head of water at a temperature of 39,2°F with a height of 1 inch .
        /// </summary>
        N18,

        /// <summary>
        /// inch of water (60 ºF)
        /// Non SI-conforming unit of pressure according to the Anglo-American and Imperial system for units, whereas the value of 1 inH2O meets the static pressure, which is generated by a head of water at a temperature of 60°F with a height of 1 inch .
        /// </summary>
        N19,

        /// <summary>
        /// kip per square inch
        /// Non SI-conforming unit of the pressure according to the Anglo-American system of units as the 1000-fold of the unit of the force pound-force divided by the power of the unit inch by exponent 2.
        /// </summary>
        N20,

        /// <summary>
        /// poundal per square foot
        /// Non SI-conforming unit of pressure by the Imperial system of units according to NIST: 1 pdl/ft² = 1,488 164 Pa.
        /// </summary>
        N21,

        /// <summary>
        /// ounce (avoirdupois) per square inch
        /// Unit of the surface specific mass (avoirdupois ounce according to the avoirdupois system of units according to the surface square inch according to the Anglo-American and Imperial system of units).
        /// </summary>
        N22,

        /// <summary>
        /// conventional metre of water
        /// Not SI-conforming unit of pressure, whereas a value of 1 mH2O is equivalent to the static pressure, which is produced by one metre high water column .
        /// </summary>
        N23,

        /// <summary>
        /// gram per square millimetre
        /// 0,001-fold of the SI base unit kilogram divided by the 0.000 001-fold of the power of the SI base unit meter by exponent 2.
        /// </summary>
        N24,

        /// <summary>
        /// pound per square yard
        /// Unit for areal-related mass as a unit pound according to the avoirdupois unit system divided by the power of the unit yard according to the Anglo-American and Imperial system of units with exponent 2.
        /// </summary>
        N25,

        /// <summary>
        /// poundal per square inch
        /// Non SI-conforming unit of the pressure according to the Imperial system of units (poundal by square inch).
        /// </summary>
        N26,

        /// <summary>
        /// foot to the fourth power
        /// Power of the unit foot according to the Anglo-American and Imperial system of units by exponent 4 according to NIST: 1 ft4 = 8,630 975 m4.
        /// </summary>
        N27,

        /// <summary>
        /// cubic decimetre per kilogram
        /// 0,001 fold of the power of the SI base unit meter by exponent 3 divided by the SI based unit kilogram.
        /// </summary>
        N28,

        /// <summary>
        /// cubic foot per pound
        /// Power of the unit foot according to the Anglo-American and Imperial system of units by exponent 3 divided by the unit avoirdupois pound according to the avoirdupois unit system.
        /// </summary>
        N29,

        /// <summary>
        /// print point
        /// </summary>
        N3,

        /// <summary>
        /// cubic inch per pound
        /// Power of the unit inch according to the Anglo-American and Imperial system of units by exponent 3 divided by the avoirdupois pound according to the avoirdupois unit system .
        /// </summary>
        N30,

        /// <summary>
        /// kilonewton per metre
        /// 1000-fold of the derived SI unit newton divided by the SI base unit metre.
        /// </summary>
        N31,

        /// <summary>
        /// poundal per inch
        /// Non SI-conforming unit of the surface tension according to the Imperial unit system as quotient poundal by inch.
        /// </summary>
        N32,

        /// <summary>
        /// pound-force per yard
        /// Unit of force per unit length based on the Anglo-American system of units.
        /// </summary>
        N33,

        /// <summary>
        /// poundal second per square foot
        /// Non SI-conforming unit of viscosity.
        /// </summary>
        N34,

        /// <summary>
        /// poise per pascal
        /// CGS (Centimetre-Gram-Second system) unit poise divided by the derived SI unit pascal.
        /// </summary>
        N35,

        /// <summary>
        /// newton second per square metre
        /// Unit of the dynamic viscosity as a product of unit of the pressure (newton by square metre) multiplied with the SI base unit second.
        /// </summary>
        N36,

        /// <summary>
        /// kilogram per metre second
        /// Unit of the dynamic viscosity as a quotient SI base unit kilogram divided by the SI base unit metre and by the SI base unit second.
        /// </summary>
        N37,

        /// <summary>
        /// kilogram per metre minute
        /// Unit of the dynamic viscosity as a quotient SI base unit kilogram divided by the SI base unit metre and by the unit minute.
        /// </summary>
        N38,

        /// <summary>
        /// kilogram per metre day
        /// Unit of the dynamic viscosity as a quotient SI base unit kilogram divided by the SI base unit metre and by the unit day.
        /// </summary>
        N39,

        /// <summary>
        /// kilogram per metre hour
        /// Unit of the dynamic viscosity as a quotient SI base unit kilogram divided by the SI base unit metre and by the unit hour.
        /// </summary>
        N40,

        /// <summary>
        /// gram per centimetre second
        /// Unit of the dynamic viscosity as a quotient of the 0,001-fold of the SI base unit kilogram divided by the 0,01-fold of the SI base unit metre and SI base unit second.
        /// </summary>
        N41,

        /// <summary>
        /// poundal second per square inch
        /// Non SI-conforming unit of dynamic viscosity according to the Imperial system of units as product unit of the pressure (poundal by square inch) multiplied by the SI base unit second.
        /// </summary>
        N42,

        /// <summary>
        /// pound per foot minute
        /// Unit of the dynamic viscosity according to the Anglo-American unit system.
        /// </summary>
        N43,

        /// <summary>
        /// pound per foot day
        /// Unit of the dynamic viscosity according to the Anglo-American unit system.
        /// </summary>
        N44,

        /// <summary>
        /// cubic metre per second pascal
        /// Power of the SI base unit meter by exponent 3 divided by the product of the SI base unit second and the derived SI base unit pascal.
        /// </summary>
        N45,

        /// <summary>
        /// foot poundal
        /// Unit of the work (force-path).
        /// </summary>
        N46,

        /// <summary>
        /// inch poundal
        /// Unit of work (force multiplied by path) according to the Imperial system of units as a product unit inch multiplied by poundal.
        /// </summary>
        N47,

        /// <summary>
        /// watt per square centimetre
        /// Derived SI unit watt divided by the power of the 0,01-fold the SI base unit metre by exponent 2.
        /// </summary>
        N48,

        /// <summary>
        /// watt per square inch
        /// Derived SI unit watt divided by the power of the unit inch according to the Anglo-American and Imperial system of units by exponent 2.
        /// </summary>
        N49,

        /// <summary>
        /// British thermal unit (international table) per square foot hour
        /// Unit of the surface heat flux according to the Imperial system of units.
        /// </summary>
        N50,

        /// <summary>
        /// British thermal unit (thermochemical) per square foot hour
        /// Unit of the surface heat flux according to the Imperial system of units.
        /// </summary>
        N51,

        /// <summary>
        /// British thermal unit (thermochemical) per square foot minute
        /// Unit of the surface heat flux according to the Imperial system of units.
        /// </summary>
        N52,

        /// <summary>
        /// British thermal unit (international table) per square foot second
        /// Unit of the surface heat flux according to the Imperial system of units.
        /// </summary>
        N53,

        /// <summary>
        /// British thermal unit (thermochemical) per square foot second
        /// Unit of the surface heat flux according to the Imperial system of units.
        /// </summary>
        N54,

        /// <summary>
        /// British thermal unit (international table) per square inch second
        /// Unit of the surface heat flux according to the Imperial system of units.
        /// </summary>
        N55,

        /// <summary>
        /// calorie (thermochemical) per square centimetre minute
        /// Unit of the surface heat flux according to the Imperial system of units.
        /// </summary>
        N56,

        /// <summary>
        /// calorie (thermochemical) per square centimetre second
        /// Unit of the surface heat flux according to the Imperial system of units.
        /// </summary>
        N57,

        /// <summary>
        /// British thermal unit (international table) per cubic foot
        /// Unit of the energy density according to the Imperial system of units.
        /// </summary>
        N58,

        /// <summary>
        /// British thermal unit (thermochemical) per cubic foot
        /// Unit of the energy density according to the Imperial system of units.
        /// </summary>
        N59,

        /// <summary>
        /// British thermal unit (international table) per degree Fahrenheit
        /// Unit of the heat capacity according to the Imperial system of units.
        /// </summary>
        N60,

        /// <summary>
        /// British thermal unit (thermochemical) per degree Fahrenheit
        /// Unit of the heat capacity according to the Imperial system of units.
        /// </summary>
        N61,

        /// <summary>
        /// British thermal unit (international table) per degree Rankine
        /// Unit of the heat capacity according to the Imperial system of units.
        /// </summary>
        N62,

        /// <summary>
        /// British thermal unit (thermochemical) per degree Rankine
        /// Unit of the heat capacity according to the Imperial system of units.
        /// </summary>
        N63,

        /// <summary>
        /// British thermal unit (thermochemical) per pound degree Rankine
        /// Unit of the heat capacity (British thermal unit according to the international table according to the Rankine degree) according to the Imperial system of units divided by the unit avoirdupois pound according to the avoirdupois system of units.
        /// </summary>
        N64,

        /// <summary>
        /// kilocalorie (international table) per gram kelvin
        /// Unit of the mass-related heat capacity as quotient 1000-fold of the calorie (international table) divided by the product of the 0,001-fold of the SI base units kilogram and kelvin.
        /// </summary>
        N65,

        /// <summary>
        /// British thermal unit (39 ºF)
        /// Unit of heat energy according to the Imperial system of units in a reference temperature of 39 °F.
        /// </summary>
        N66,

        /// <summary>
        /// British thermal unit (59 ºF)
        /// Unit of heat energy according to the Imperial system of units in a reference temperature of 59 °F.
        /// </summary>
        N67,

        /// <summary>
        /// British thermal unit (60 ºF)
        /// Unit of head energy according to the Imperial system of units at a reference temperature of 60 °F.
        /// </summary>
        N68,

        /// <summary>
        /// calorie (20 ºC)
        /// Unit for quantity of heat, which is to be required for 1 g air free water at a constant pressure from 101,325 kPa, to warm up the pressure of standard atmosphere at sea level, from 19,5 °C on 20,5 °C.
        /// </summary>
        N69,

        /// <summary>
        /// quad (1015 BtuIT)
        /// Unit of heat energy according to the imperial system of units.
        /// </summary>
        N70,

        /// <summary>
        /// therm (EC)
        /// Unit of heat energy in commercial use, within the EU defined: 1 thm (EC) = 100 000 BtuIT.
        /// </summary>
        N71,

        /// <summary>
        /// therm (U.S.)
        /// Unit of heat energy in commercial use.
        /// </summary>
        N72,

        /// <summary>
        /// British thermal unit (thermochemical) per pound
        /// Unit of the heat energy according to the Imperial system of units divided the unit avoirdupois pound according to the avoirdupois system of units.
        /// </summary>
        N73,

        /// <summary>
        /// British thermal unit (international table) per hour square foot degree Fahrenheit
        /// Unit of the heat transition coefficient according to the Imperial system of units.
        /// </summary>
        N74,

        /// <summary>
        /// British thermal unit (thermochemical) per hour square foot degree Fahrenheit
        /// Unit of the heat transition coefficient according to the imperial system of units.
        /// </summary>
        N75,

        /// <summary>
        /// British thermal unit (international table) per second square foot degree Fahrenheit
        /// Unit of the heat transition coefficient according to the imperial system of units.
        /// </summary>
        N76,

        /// <summary>
        /// British thermal unit (thermochemical) per second square foot degree Fahrenheit
        /// Unit of the heat transition coefficient according to the imperial system of units.
        /// </summary>
        N77,

        /// <summary>
        /// kilowatt per square metre kelvin
        /// 1000-fold of the derived SI unit watt divided by the product of the power of the SI base unit metre by exponent 2 and the SI base unit kelvin.
        /// </summary>
        N78,

        /// <summary>
        /// kelvin per pascal
        /// SI base unit kelvin divided by the derived SI unit pascal.
        /// </summary>
        N79,

        /// <summary>
        /// watt per metre degree Celsius
        /// Derived SI unit watt divided by the product of the SI base unit metre and the unit for temperature degree Celsius.
        /// </summary>
        N80,

        /// <summary>
        /// kilowatt per metre kelvin
        /// 1000-fold of the derived SI unit watt divided by the product of the SI base unit metre and the SI base unit kelvin.
        /// </summary>
        N81,

        /// <summary>
        /// kilowatt per metre degree Celsius
        /// 1000-fold of the derived SI unit watt divided by the product of the SI base unit metre and the unit for temperature degree Celsius.
        /// </summary>
        N82,

        /// <summary>
        /// metre per degree Celcius metre
        /// SI base unit metre divided by the product of the unit degree Celsius and the SI base unit metre.
        /// </summary>
        N83,

        /// <summary>
        /// degree Fahrenheit hour per British thermal unit (international table)
        /// Non SI-conforming unit of the thermal resistance according to the Imperial system of units.
        /// </summary>
        N84,

        /// <summary>
        /// degree Fahrenheit hour per British thermal unit (thermochemical)
        /// Non SI-conforming unit of the thermal resistance according to the Imperial system of units.
        /// </summary>
        N85,

        /// <summary>
        /// degree Fahrenheit second per British thermal unit (international table)
        /// Non SI-conforming unit of the thermal resistance according to the Imperial system of units.
        /// </summary>
        N86,

        /// <summary>
        /// degree Fahrenheit second per British thermal unit (thermochemical)
        /// Non SI-conforming unit of the thermal resistance according to the Imperial system of units.
        /// </summary>
        N87,

        /// <summary>
        /// degree Fahrenheit hour square foot per British thermal unit (international table) inch
        /// Unit of specific thermal resistance according to the Imperial system of units.
        /// </summary>
        N88,

        /// <summary>
        /// degree Fahrenheit hour square foot per British thermal unit (thermochemical) inch
        /// Unit of specific thermal resistance according to the Imperial system of units.
        /// </summary>
        N89,

        /// <summary>
        /// kilofarad
        /// 1000-fold of the derived SI unit farad.
        /// </summary>
        N90,

        /// <summary>
        /// reciprocal joule
        /// Reciprocal of the derived SI unit joule.
        /// </summary>
        N91,

        /// <summary>
        /// picosiemens
        /// 0,000 000 000 001-fold of the derived SI unit siemens.
        /// </summary>
        N92,

        /// <summary>
        /// ampere per pascal
        /// SI base unit ampere divided by the derived SI unit pascal.
        /// </summary>
        N93,

        /// <summary>
        /// franklin
        /// CGS (Centimetre-Gram-Second system) unit of the electrical charge, where the charge amounts to exactly 1 Fr where the force of 1 dyn on an equal load is performed at a distance of 1 cm.
        /// </summary>
        N94,

        /// <summary>
        /// ampere minute
        /// A unit of electric charge defining the amount of charge accumulated by a steady flow of one ampere for one minute..
        /// </summary>
        N95,

        /// <summary>
        /// biot
        /// CGS (Centimetre-Gram-Second system) unit of the electric power which is defined by a force of 2 dyn per cm between two parallel conductors of infinite length with negligible cross-section in the distance of 1 cm.
        /// </summary>
        N96,

        /// <summary>
        /// gilbert
        /// CGS (Centimetre-Gram-Second system) unit of the magnetomotive force, which is defined by the work to increase the magnetic potential of a positive common pol with 1 erg.
        /// </summary>
        N97,

        /// <summary>
        /// volt per pascal
        /// Derived SI unit volt divided by the derived SI unit pascal.
        /// </summary>
        N98,

        /// <summary>
        /// picovolt
        /// 0,000 000 000 001-fold of the derived SI unit volt.
        /// </summary>
        N99,

        /// <summary>
        /// milligram per kilogram
        /// </summary>
        NA,

        /// <summary>
        /// number of articles
        /// A unit of count defining the number of articles (article: item).
        /// </summary>
        NAR,

        /// <summary>
        /// number of cells
        /// A unit of count defining the number of cells (cell: an enclosed or circumscribed space, cavity, or volume).
        /// </summary>
        NCL,

        /// <summary>
        /// newton
        /// </summary>
        NEW,

        /// <summary>
        /// message
        /// A unit of count defining the number of messages.
        /// </summary>
        NF,

        /// <summary>
        /// nil
        /// A unit of count defining the number of instances of nothing.
        /// </summary>
        NIL,

        /// <summary>
        /// number of international units
        /// A unit of count defining the number of international units.
        /// </summary>
        NIU,

        /// <summary>
        /// load
        /// A unit of volume defining the number of loads (load: a quantity of items carried or processed at one time).
        /// </summary>
        NL,

        /// <summary>
        /// Normalised cubic metre
        /// Normalised cubic metre (temperature 0°C and pressure 101325 millibars )
        /// </summary>
        NM3,

        /// <summary>
        /// nautical mile
        /// </summary>
        NMI,

        /// <summary>
        /// number of packs
        /// A unit of count defining the number of packs (pack: a collection of objects packaged together).
        /// </summary>
        NMP,

        /// <summary>
        /// number of parts
        /// A unit of count defining the number of parts (part: component of a larger entity).
        /// </summary>
        NPT,

        /// <summary>
        /// net ton
        /// A unit of mass equal to 2000 pounds, see ton (US). Refer International Convention on tonnage measurement of Ships.
        /// </summary>
        NT,

        /// <summary>
        /// Nephelometric turbidity unit
        /// </summary>
        NTU,

        /// <summary>
        /// newton metre
        /// </summary>
        NU,

        /// <summary>
        /// part per thousand
        /// A unit of proportion equal to 10⁻³. Synonym: per mille
        /// </summary>
        NX,

        /// <summary>
        /// panel
        /// A unit of count defining the number of panels (panel: a distinct, usually rectangular, section of a surface).
        /// </summary>
        OA,

        /// <summary>
        /// ozone depletion equivalent
        /// A unit of mass defining the ozone depletion potential in kilograms of a product relative to the calculated depletion for the reference substance, Trichlorofluoromethane (CFC-11).
        /// </summary>
        ODE,

        /// <summary>
        /// ODS Grams
        /// </summary>
        ODG,

        /// <summary>
        /// ODS Kilograms
        /// </summary>
        ODK,

        /// <summary>
        /// ODS Milligrams
        /// </summary>
        ODM,

        /// <summary>
        /// ohm
        /// </summary>
        OHM,

        /// <summary>
        /// ounce per square yard
        /// </summary>
        ON,

        /// <summary>
        /// ounce (avoirdupois)
        /// </summary>
        ONZ,

        /// <summary>
        /// oscillations per minute
        /// The number of oscillations per minute.
        /// </summary>
        OPM,

        /// <summary>
        /// overtime hour
        /// A unit of time defining the number of overtime hours.
        /// </summary>
        OT,

        /// <summary>
        /// fluid ounce (US)
        /// </summary>
        OZA,

        /// <summary>
        /// fluid ounce (UK)
        /// </summary>
        OZI,

        /// <summary>
        /// percent
        /// A unit of proportion equal to 0.01.
        /// </summary>
        P1,

        /// <summary>
        /// coulomb per metre
        /// Derived SI unit coulomb divided by the SI base unit metre.
        /// </summary>
        P10,

        /// <summary>
        /// kiloweber
        /// 1000 fold of the derived SI unit weber.
        /// </summary>
        P11,

        /// <summary>
        /// gamma
        /// Unit of magnetic flow density.
        /// </summary>
        P12,

        /// <summary>
        /// kilotesla
        /// 1000-fold of the derived SI unit tesla.
        /// </summary>
        P13,

        /// <summary>
        /// joule per second
        /// Quotient of the derived SI unit joule divided by the SI base unit second.
        /// </summary>
        P14,

        /// <summary>
        /// joule per minute
        /// Quotient from the derived SI unit joule divided by the unit minute.
        /// </summary>
        P15,

        /// <summary>
        /// joule per hour
        /// Quotient from the derived SI unit joule divided by the unit hour.
        /// </summary>
        P16,

        /// <summary>
        /// joule per day
        /// Quotient from the derived SI unit joule divided by the unit day.
        /// </summary>
        P17,

        /// <summary>
        /// kilojoule per second
        /// Quotient from the 1000-fold of the derived SI unit joule divided by the SI base unit second.
        /// </summary>
        P18,

        /// <summary>
        /// kilojoule per minute
        /// Quotient from the 1000-fold of the derived SI unit joule divided by the unit minute.
        /// </summary>
        P19,

        /// <summary>
        /// pound per foot
        /// </summary>
        P2,

        /// <summary>
        /// kilojoule per hour
        /// Quotient from the 1000-fold of the derived SI unit joule divided by the unit hour.
        /// </summary>
        P20,

        /// <summary>
        /// kilojoule per day
        /// Quotient from the 1000-fold of the derived SI unit joule divided by the unit day.
        /// </summary>
        P21,

        /// <summary>
        /// nanoohm
        /// 0,000 000 001-fold of the derived SI unit ohm.
        /// </summary>
        P22,

        /// <summary>
        /// ohm circular-mil per foot
        /// Unit of resistivity.
        /// </summary>
        P23,

        /// <summary>
        /// kilohenry
        /// 1000-fold of the derived SI unit henry.
        /// </summary>
        P24,

        /// <summary>
        /// lumen per square foot
        /// Derived SI unit lumen divided by the power of the unit foot according to the Anglo-American and Imperial system of units by exponent 2.
        /// </summary>
        P25,

        /// <summary>
        /// phot
        /// CGS (Centimetre-Gram-Second system) unit of luminance, defined as lumen by square centimetre.
        /// </summary>
        P26,

        /// <summary>
        /// footcandle
        /// Non SI conform traditional unit, defined as density of light which impinges on a surface which has a distance of one foot from a light source, which shines with an intensity of an international candle.
        /// </summary>
        P27,

        /// <summary>
        /// candela per square inch
        /// SI base unit candela divided by the power of unit inch according to the Anglo-American and Imperial system of units by exponent 2.
        /// </summary>
        P28,

        /// <summary>
        /// footlambert
        /// Unit of the luminance according to the Anglo-American system of units, defined as emitted or reflected luminance of a lm/ft².
        /// </summary>
        P29,

        /// <summary>
        /// lambert
        /// CGS (Centimetre-Gram-Second system) unit of luminance, defined as the emitted or reflected luminance by one lumen per square centimetre.
        /// </summary>
        P30,

        /// <summary>
        /// stilb
        /// CGS (Centimetre-Gram-Second system) unit of luminance, defined as emitted or reflected luminance by one lumen per square centimetre.
        /// </summary>
        P31,

        /// <summary>
        /// candela per square foot
        /// Base unit SI candela divided by the power of the unit foot according to the Anglo-American and Imperial system of units by exponent 2.
        /// </summary>
        P32,

        /// <summary>
        /// kilocandela
        /// 1000-fold of the SI base unit candela.
        /// </summary>
        P33,

        /// <summary>
        /// millicandela
        /// 0,001-fold of the SI base unit candela.
        /// </summary>
        P34,

        /// <summary>
        /// Hefner-Kerze
        /// Obsolete, non-legal unit of the power in Germany relating to DIN 1301-3:1979: 1 HK = 0,903 cd.
        /// </summary>
        P35,

        /// <summary>
        /// international candle
        /// Obsolete, non-legal unit of the power in Germany relating to DIN 1301-3:1979: 1 HK = 1,019 cd.
        /// </summary>
        P36,

        /// <summary>
        /// British thermal unit (international table) per square foot
        /// Unit of the areal-related energy transmission according to the Imperial system of units.
        /// </summary>
        P37,

        /// <summary>
        /// British thermal unit (thermochemical) per square foot
        /// Unit of the areal-related energy transmission according to the Imperial system of units.
        /// </summary>
        P38,

        /// <summary>
        /// calorie (thermochemical) per square centimetre
        /// Unit of the areal-related energy transmission according to the Imperial system of units.
        /// </summary>
        P39,

        /// <summary>
        /// langley
        /// CGS (Centimetre-Gram-Second system) unit of the areal-related energy transmission (as a measure of the incident quantity of heat of solar radiation on the earth's surface).
        /// </summary>
        P40,

        /// <summary>
        /// decade (logarithmic)
        /// 1 Dec := log2 10 ˜ 3,32 according to the logarithm for frequency range between f1 and f2, when f2/f1 = 10.
        /// </summary>
        P41,

        /// <summary>
        /// pascal squared second
        /// Unit of the set as a product of the power of derived SI unit pascal with exponent 2 and the SI base unit second.
        /// </summary>
        P42,

        /// <summary>
        /// bel per metre
        /// Unit bel divided by the SI base unit metre.
        /// </summary>
        P43,

        /// <summary>
        /// pound mole
        /// Non SI-conforming unit of quantity of a substance relating that one pound mole of a chemical composition corresponds to the same number of pounds as the molecular weight of one molecule of this composition in atomic mass units.
        /// </summary>
        P44,

        /// <summary>
        /// pound mole per second
        /// Non SI-conforming unit of the power of the amount of substance non-SI compliant unit of the molar flux relating that a pound mole of a chemical composition the same number of pound corresponds like the molecular weight of a molecule of this composition in atomic mass units.
        /// </summary>
        P45,

        /// <summary>
        /// pound mole per minute
        /// Non SI-conforming unit of the power of the amount of substance non-SI compliant unit of the molar flux relating that a pound mole of a chemical composition the same number of pound corresponds like the molecular weight of a molecule of this composition in atomic mass units.
        /// </summary>
        P46,

        /// <summary>
        /// kilomole per kilogram
        /// 1000-fold of the SI base unit mol divided by the SI base unit kilogram.
        /// </summary>
        P47,

        /// <summary>
        /// pound mole per pound
        /// Non SI-conforming unit of the material molar flux divided by the avoirdupois pound for mass according to the avoirdupois unit system.
        /// </summary>
        P48,

        /// <summary>
        /// newton square metre per ampere
        /// Product of the derived SI unit newton and the power of SI base unit metre with exponent 2 divided by the SI base unit ampere.
        /// </summary>
        P49,

        /// <summary>
        /// five pack
        /// A unit of count defining the number of five-packs (five-pack: set of five items packaged together).
        /// </summary>
        P5,

        /// <summary>
        /// weber metre
        /// Product of the derived SI unit weber and SI base unit metre.
        /// </summary>
        P50,

        /// <summary>
        /// mol per kilogram pascal
        /// SI base unit mol divided by the product of the SI base unit kilogram and the derived SI unit pascal.
        /// </summary>
        P51,

        /// <summary>
        /// mol per cubic metre pascal
        /// SI base unit mol divided by the product of the power from the SI base unit metre with exponent 3 and the derived SI unit pascal.
        /// </summary>
        P52,

        /// <summary>
        /// unit pole
        /// CGS (Centimetre-Gram-Second system) unit for magnetic flux of a magnetic pole (according to the interaction of identical poles of 1 dyn at a distance of a cm).
        /// </summary>
        P53,

        /// <summary>
        /// milligray per second
        /// 0,001-fold of the derived SI unit gray divided by the SI base unit second.
        /// </summary>
        P54,

        /// <summary>
        /// microgray per second
        /// 0,000 001-fold of the derived SI unit gray divided by the SI base unit second.
        /// </summary>
        P55,

        /// <summary>
        /// nanogray per second
        /// 0,000 000 001-fold of the derived SI unit gray divided by the SI base unit second.
        /// </summary>
        P56,

        /// <summary>
        /// gray per minute
        /// SI derived unit gray divided by the unit minute.
        /// </summary>
        P57,

        /// <summary>
        /// milligray per minute
        /// 0,001-fold of the derived SI unit gray divided by the unit minute.
        /// </summary>
        P58,

        /// <summary>
        /// microgray per minute
        /// 0,000 001-fold of the derived SI unit gray divided by the unit minute.
        /// </summary>
        P59,

        /// <summary>
        /// nanogray per minute
        /// 0,000 000 001-fold of the derived SI unit gray divided by the unit minute.
        /// </summary>
        P60,

        /// <summary>
        /// gray per hour
        /// SI derived unit gray divided by the unit hour.
        /// </summary>
        P61,

        /// <summary>
        /// milligray per hour
        /// 0,001-fold of the derived SI unit gray divided by the unit hour.
        /// </summary>
        P62,

        /// <summary>
        /// microgray per hour
        /// 0,000 001-fold of the derived SI unit gray divided by the unit hour.
        /// </summary>
        P63,

        /// <summary>
        /// nanogray per hour
        /// 0,000 000 001-fold of the derived SI unit gray divided by the unit hour.
        /// </summary>
        P64,

        /// <summary>
        /// sievert per second
        /// Derived SI unit sievert divided by the SI base unit second.
        /// </summary>
        P65,

        /// <summary>
        /// millisievert per second
        /// 0,001-fold of the derived SI unit sievert divided by the SI base unit second.
        /// </summary>
        P66,

        /// <summary>
        /// microsievert per second
        /// 0,000 001-fold of the derived SI unit sievert divided by the SI base unit second.
        /// </summary>
        P67,

        /// <summary>
        /// nanosievert per second
        /// 0,000 000 001-fold of the derived SI unit sievert divided by the SI base unit second.
        /// </summary>
        P68,

        /// <summary>
        /// rem per second
        /// Unit for the equivalent tin rate relating to DIN 1301-3:1979: 1 rem/s = 0,01 J/(kg·s) = 1 Sv/s.
        /// </summary>
        P69,

        /// <summary>
        /// sievert per hour
        /// Derived SI unit sievert divided by the unit hour.
        /// </summary>
        P70,

        /// <summary>
        /// millisievert per hour
        /// 0,001-fold of the derived SI unit sievert divided by the unit hour.
        /// </summary>
        P71,

        /// <summary>
        /// microsievert per hour
        /// 0,000 001-fold of the derived SI unit sievert divided by the unit hour.
        /// </summary>
        P72,

        /// <summary>
        /// nanosievert per hour
        /// 0,000 000 001-fold of the derived SI unit sievert divided by the unit hour.
        /// </summary>
        P73,

        /// <summary>
        /// sievert per minute
        /// Derived SI unit sievert divided by the unit minute.
        /// </summary>
        P74,

        /// <summary>
        /// millisievert per minute
        /// 0,001-fold of the derived SI unit sievert divided by the unit minute.
        /// </summary>
        P75,

        /// <summary>
        /// microsievert per minute
        /// 0,000 001-fold of the derived SI unit sievert divided by the unit minute.
        /// </summary>
        P76,

        /// <summary>
        /// nanosievert per minute
        /// 0,000 000 001-fold of the derived SI unit sievert divided by the unit minute.
        /// </summary>
        P77,

        /// <summary>
        /// reciprocal square inch
        /// Complement of the power of the unit inch according to the Anglo-American and Imperial system of units by exponent 2.
        /// </summary>
        P78,

        /// <summary>
        /// pascal square metre per kilogram
        /// Unit of the burst index as derived unit for pressure pascal related to the substance, represented as a quotient from the SI base unit kilogram divided by the power of the SI base unit metre by exponent 2.
        /// </summary>
        P79,

        /// <summary>
        /// millipascal per metre
        /// 0,001-fold of the derived SI unit pascal divided by the SI base unit metre.
        /// </summary>
        P80,

        /// <summary>
        /// kilopascal per metre
        /// 1000-fold of the derived SI unit pascal divided by the SI base unit metre.
        /// </summary>
        P81,

        /// <summary>
        /// hectopascal per metre
        /// 100-fold of the derived SI unit pascal divided by the SI base unit metre.
        /// </summary>
        P82,

        /// <summary>
        /// standard atmosphere per metre
        /// Outdated unit of the pressure divided by the SI base unit metre.
        /// </summary>
        P83,

        /// <summary>
        /// technical atmosphere per metre
        /// Obsolete and non-legal unit of the pressure which is generated by a 10 metre water column divided by the SI base unit metre.
        /// </summary>
        P84,

        /// <summary>
        /// torr per metre
        /// CGS (Centimetre-Gram-Second system) unit of the pressure divided by the SI base unit metre.
        /// </summary>
        P85,

        /// <summary>
        /// psi per inch
        /// Compound unit for pressure (pound-force according to the Anglo-American unit system divided by the power of the unit inch according to the Anglo-American and Imperial system of units with the exponent 2) divided by the unit inch according to the Anglo-American and Imperial system of units .
        /// </summary>
        P86,

        /// <summary>
        /// cubic metre per second square metre
        /// Unit of volume flow cubic meters by second related to the transmission surface in square metres.
        /// </summary>
        P87,

        /// <summary>
        /// rhe
        /// Non SI-conforming unit of fluidity of dynamic viscosity.
        /// </summary>
        P88,

        /// <summary>
        /// pound-force foot per inch
        /// Unit for length-related rotational moment according to the Anglo-American and Imperial system of units.
        /// </summary>
        P89,

        /// <summary>
        /// pound-force inch per inch
        /// Unit for length-related rotational moment according to the Anglo-American and Imperial system of units.
        /// </summary>
        P90,

        /// <summary>
        /// perm (0 ºC)
        /// Traditional unit for the ability of a material to allow the transition of the steam, defined at a temperature of 0 °C as steam transmittance, where the mass of one grain steam penetrates an area of one foot squared at a pressure from one inch mercury per hour.
        /// </summary>
        P91,

        /// <summary>
        /// perm (23 ºC)
        /// Traditional unit for the ability of a material to allow the transition of the steam, defined at a temperature of 23 °C as steam transmittance at which the mass of one grain of steam penetrates an area of one square foot at a pressure of one inch mercury per hour.
        /// </summary>
        P92,

        /// <summary>
        /// byte per second
        /// Unit byte divided by the SI base unit second.
        /// </summary>
        P93,

        /// <summary>
        /// kilobyte per second
        /// 1000-fold of the unit byte divided by the SI base unit second.
        /// </summary>
        P94,

        /// <summary>
        /// megabyte per second
        /// 1 000 000-fold of the unit byte divided by the SI base unit second.
        /// </summary>
        P95,

        /// <summary>
        /// reciprocal volt
        /// Reciprocal of the derived SI unit volt.
        /// </summary>
        P96,

        /// <summary>
        /// reciprocal radian
        /// Reciprocal of the unit radian.
        /// </summary>
        P97,

        /// <summary>
        /// pascal to the power sum of stoichiometric numbers
        /// Unit of the equilibrium constant on the basis of the pressure(ISO 80000-9:2009, 9-35.a).
        /// </summary>
        P98,

        /// <summary>
        /// mole per cubiv metre to the power sum of stoichiometric numbers
        /// Unit of the equilibrium constant on the basis of the concentration (ISO 80000-9:2009, 9-36.a).
        /// </summary>
        P99,

        /// <summary>
        /// pascal
        /// </summary>
        PAL,

        /// <summary>
        /// pad
        /// A unit of count defining the number of pads (pad: block of paper sheets fastened together at one end).
        /// </summary>
        PD,

        /// <summary>
        /// proof litre
        /// A unit of volume equal to one litre of proof spirits, or the alcohol equivalent thereof. Used for measuring the strength of distilled alcoholic liquors, expressed as a percentage of the alcohol content of a standard mixture at a specific temperature.
        /// </summary>
        PFL,

        /// <summary>
        /// proof gallon
        /// A unit of volume equal to one gallon of proof spirits, or the alcohol equivalent thereof. Used for measuring the strength of distilled alcoholic liquors, expressed as a percentage of the alcohol content of a standard mixture at a specific temperature.
        /// </summary>
        PGL,

        /// <summary>
        /// pitch
        /// A unit of count defining the number of characters that fit in a horizontal inch.
        /// </summary>
        PI,

        /// <summary>
        /// degree Plato
        /// A unit of proportion defining the sugar content of a product, especially in relation to beer.
        /// </summary>
        PLA,

        /// <summary>
        /// pound per inch of length
        /// </summary>
        PO,

        /// <summary>
        /// page per inch
        /// A unit of quantity defining the degree of thickness of a bound publication, expressed as the number of pages per inch of thickness.
        /// </summary>
        PQ,

        /// <summary>
        /// pair
        /// A unit of count defining the number of pairs (pair: item described by two's).
        /// </summary>
        PR,

        /// <summary>
        /// pound-force per square inch
        /// </summary>
        PS,

        /// <summary>
        /// dry pint (US)
        /// </summary>
        PTD,

        /// <summary>
        /// pint (UK)
        /// </summary>
        PTI,

        /// <summary>
        /// liquid pint (US)
        /// </summary>
        PTL,

        /// <summary>
        /// portion
        /// A quantity of allowance of food allotted to, or enough for, one person.
        /// </summary>
        PTN,

        /// <summary>
        /// joule per tesla
        /// Unit of the magnetic dipole moment of the molecule as derived SI unit joule divided by the derived SI unit tesla.
        /// </summary>
        Q10,

        /// <summary>
        /// erlang
        /// Unit of the market value according to the feature of a single feature as a statistical measurement of the existing utilization.
        /// </summary>
        Q11,

        /// <summary>
        /// octet
        /// Synonym for byte: 1 octet = 8 bit = 1 byte.
        /// </summary>
        Q12,

        /// <summary>
        /// octet per second
        /// Unit octet divided by the SI base unit second.
        /// </summary>
        Q13,

        /// <summary>
        /// shannon
        /// Logarithmic unit for information equal to the content of decision of a sentence of two mutually exclusive events, expressed as a logarithm to base 2.
        /// </summary>
        Q14,

        /// <summary>
        /// hartley
        /// Logarithmic unit for information equal to the content of decision of a sentence of ten mutually exclusive events, expressed as a logarithm to base 10.
        /// </summary>
        Q15,

        /// <summary>
        /// natural unit of information
        /// Logarithmic unit for information equal to the content of decision of a sentence of ,718 281 828 459 mutually exclusive events, expressed as a logarithm to base Euler value e.
        /// </summary>
        Q16,

        /// <summary>
        /// shannon per second
        /// Time related logarithmic unit for information equal to the content of decision of a sentence of two mutually exclusive events, expressed as a logarithm to base 2.
        /// </summary>
        Q17,

        /// <summary>
        /// hartley per second
        /// Time related logarithmic unit for information equal to the content of decision of a sentence of ten mutually exclusive events, expressed as a logarithm to base 10.
        /// </summary>
        Q18,

        /// <summary>
        /// natural unit of information per second
        /// Time related logarithmic unit for information equal to the content of decision of a sentence of 2,718 281 828 459 mutually exclusive events, expressed as a logarithm to base of the Euler value e.
        /// </summary>
        Q19,

        /// <summary>
        /// second per kilogramm
        /// Unit of the Einstein transition probability for spontaneous or inducing emissions and absorption according to ISO 80000-7:2008, expressed as SI base unit second divided by the SI base unit kilogram.
        /// </summary>
        Q20,

        /// <summary>
        /// watt square metre
        /// Unit of the first radiation constants c1 = 2·p·h·c0², the value of which is 3,741 771 18·10?¹6-fold that of the comparative value of the product of the derived SI unit watt multiplied with the power of the SI base unit metre with the exponent 2.
        /// </summary>
        Q21,

        /// <summary>
        /// second per radian cubic metre
        /// Unit of the density of states as an expression of angular frequency as complement of the product of hertz and radiant and the power of SI base unit metre by exponent 3 .
        /// </summary>
        Q22,

        /// <summary>
        /// weber to the power minus one
        /// Complement of the derived SI unit weber as unit of the Josephson constant, which value is equal to the 384 597,891-fold of the reference value gigahertz divided by volt.
        /// </summary>
        Q23,

        /// <summary>
        /// reciprocal inch
        /// Complement of the unit inch according to the Anglo-American and Imperial system of units.
        /// </summary>
        Q24,

        /// <summary>
        /// dioptre
        /// Unit used at the statement of relative refractive indexes of optical systems as complement of the focal length with correspondence to: 1 dpt = 1/m.
        /// </summary>
        Q25,

        /// <summary>
        /// one per one
        /// Value of the quotient from two physical units of the same kind as a numerator and denominator whereas the units are shortened mutually.
        /// </summary>
        Q26,

        /// <summary>
        /// newton metre per metre
        /// Unit for length-related rotational moment as product of the derived SI unit newton and the SI base unit metre divided by the SI base unit metre.
        /// </summary>
        Q27,

        /// <summary>
        /// kilogram per square metre pascal second
        /// Unit for the ability of a material to allow the transition of steam.
        /// </summary>
        Q28,

        /// <summary>
        /// microgram per hectogram
        /// Microgram per hectogram.
        /// </summary>
        Q29,

        /// <summary>
        /// pH (potential of Hydrogen)
        /// The activity of the (solvated) hydrogen ion (a logarithmic measure used to state the acidity or alkalinity of a chemical solution).
        /// </summary>
        Q30,

        /// <summary>
        /// kilojoule per gram
        /// </summary>
        Q31,

        /// <summary>
        /// femtolitre
        /// </summary>
        Q32,

        /// <summary>
        /// picolitre
        /// </summary>
        Q33,

        /// <summary>
        /// nanolitre
        /// </summary>
        Q34,

        /// <summary>
        /// megawatts per minute
        /// A unit of power defining the total amount of bulk energy transferred or consumer per minute.
        /// </summary>
        Q35,

        /// <summary>
        /// square metre per cubic metre
        /// A unit of the amount of surface area per unit volume of an object or collection of objects.
        /// </summary>
        Q36,

        /// <summary>
        /// Standard cubic metre per day
        /// Standard cubic metre (temperature 15°C and pressure 101325 millibars ) per day
        /// </summary>
        Q37,

        /// <summary>
        /// Standard cubic metre per hour
        /// Standard cubic metre (temperature 15°C and pressure 101325 millibars ) per hour
        /// </summary>
        Q38,

        /// <summary>
        /// Normalized cubic metre per day
        /// Normalized cubic metre (temperature 0°C and pressure 101325 millibars ) per day
        /// </summary>
        Q39,

        /// <summary>
        /// Normalized cubic metre per hour
        /// Normalized cubic metre (temperature 0°C and pressure 101325 millibars ) per hour
        /// </summary>
        Q40,

        /// <summary>
        /// Joule per normalised cubic metre
        /// </summary>
        Q41,

        /// <summary>
        /// Joule per standard cubic metre
        /// </summary>
        Q42,

        /// <summary>
        /// meal
        /// A unit of count defining the number of meals (meal: an amount of food to be eaten on a single occasion).
        /// </summary>
        Q3,

        /// <summary>
        /// page - facsimile
        /// A unit of count defining the number of facsimile pages.
        /// </summary>
        QA,

        /// <summary>
        /// quarter (of a year)
        /// A unit of time defining the number of quarters (3 months).
        /// </summary>
        QAN,

        /// <summary>
        /// page - hardcopy
        /// A unit of count defining the number of hardcopy pages (hardcopy page: a page rendered as printed or written output on paper, film, or other permanent medium).
        /// </summary>
        QB,

        /// <summary>
        /// quire
        /// A unit of count for paper, expressed as the number of quires (quire: a number of paper sheets, typically 25).
        /// </summary>
        QR,

        /// <summary>
        /// dry quart (US)
        /// </summary>
        QTD,

        /// <summary>
        /// quart (UK)
        /// </summary>
        QTI,

        /// <summary>
        /// liquid quart (US)
        /// </summary>
        QTL,

        /// <summary>
        /// quarter (UK)
        /// A traditional unit of weight equal to 1/4 hundredweight. In the United Kingdom, one quarter equals 28 pounds.
        /// </summary>
        QTR,

        /// <summary>
        /// pica
        /// A unit of count defining the number of picas. (pica: typographical length equal to 12 points or 4.22 mm (approx.)).
        /// </summary>
        R1,

        /// <summary>
        /// thousand cubic metre
        /// A unit of volume equal to one thousand cubic metres.
        /// </summary>
        R9,

        /// <summary>
        /// running or operating hour
        /// A unit of time defining the number of hours of operation.
        /// </summary>
        RH,

        /// <summary>
        /// ream
        /// A unit of count for paper, expressed as the number of reams (ream: a large quantity of paper sheets, typically 500).
        /// </summary>
        RM,

        /// <summary>
        /// room
        /// A unit of count defining the number of rooms.
        /// </summary>
        ROM,

        /// <summary>
        /// pound per ream
        /// A unit of mass for paper, expressed as pounds per ream. (ream: a large quantity of paper, typically 500 sheets).
        /// </summary>
        RP,

        /// <summary>
        /// revolutions per minute
        /// Refer ISO/TC12 SI Guide
        /// </summary>
        RPM,

        /// <summary>
        /// revolutions per second
        /// Refer ISO/TC12 SI Guide
        /// </summary>
        RPS,

        /// <summary>
        /// revenue ton mile
        /// A unit of information typically used for billing purposes, expressed as the number of revenue tons (revenue ton: either a metric ton or a cubic metres, whichever is the larger), moved over a distance of one mile.
        /// </summary>
        RT,

        /// <summary>
        /// square foot per second
        /// Synonym: foot squared per second
        /// </summary>
        S3,

        /// <summary>
        /// square metre per second
        /// Synonym: metre squared per second (square metres/second US)
        /// </summary>
        S4,

        /// <summary>
        /// half year (6 months)
        /// A unit of time defining the number of half years (6 months).
        /// </summary>
        SAN,

        /// <summary>
        /// score
        /// A unit of count defining the number of units in multiples of 20.
        /// </summary>
        SCO,

        /// <summary>
        /// scruple
        /// </summary>
        SCR,

        /// <summary>
        /// second [unit of time]
        /// </summary>
        SEC,

        /// <summary>
        /// set
        /// A unit of count defining the number of sets (set: a number of objects grouped together).
        /// </summary>
        SET,

        /// <summary>
        /// segment
        /// A unit of information equal to 64000 bytes.
        /// </summary>
        SG,

        /// <summary>
        /// siemens
        /// </summary>
        SIE,

        /// <summary>
        /// Standard cubic metre
        /// Standard cubic metre (temperature 15°C and pressure 101325 millibars )
        /// </summary>
        SM3,

        /// <summary>
        /// mile (statute mile)
        /// </summary>
        SMI,

        /// <summary>
        /// square
        /// A unit of count defining the number of squares (square: rectangular shape).
        /// </summary>
        SQ,

        /// <summary>
        /// square, roofing
        /// A unit of count defining the number of squares of roofing materials, measured in multiples of 100 square feet.
        /// </summary>
        SQR,

        /// <summary>
        /// strip
        /// A unit of count defining the number of strips (strip: long narrow piece of an object).
        /// </summary>
        SR,

        /// <summary>
        /// stick
        /// A unit of count defining the number of sticks (stick: slender and often cylindrical piece of a substance).
        /// </summary>
        STC,

        /// <summary>
        /// stone (UK)
        /// </summary>
        STI,

        /// <summary>
        /// stick, cigarette
        /// A unit of count defining the number of cigarettes in the smallest unit for stock-taking and/or duty computation.
        /// </summary>
        STK,

        /// <summary>
        /// standard litre
        /// A unit of volume defining the number of litres of a product at a temperature of 15 degrees Celsius, especially in relation to hydrocarbon oils.
        /// </summary>
        STL,

        /// <summary>
        /// ton (US) or short ton (UK/US)
        /// Synonym: net ton (2000 lb)
        /// </summary>
        STN,

        /// <summary>
        /// straw
        /// A unit of count defining the number of straws (straw: a slender tube used for sucking up liquids).
        /// </summary>
        STW,

        /// <summary>
        /// skein
        /// A unit of count defining the number of skeins (skein: a loosely-coiled bundle of yarn or thread).
        /// </summary>
        SW,

        /// <summary>
        /// shipment
        /// A unit of count defining the number of shipments (shipment: an amount of goods shipped or transported).
        /// </summary>
        SX,

        /// <summary>
        /// syringe
        /// A unit of count defining the number of syringes (syringe: a small device for pumping, spraying and/or injecting liquids through a small aperture).
        /// </summary>
        SYR,

        /// <summary>
        /// telecommunication line in service
        /// A unit of count defining the number of lines in service.
        /// </summary>
        T0,

        /// <summary>
        /// thousand piece
        /// A unit of count defining the number of pieces in multiples of 1000 (piece: a single item, article or exemplar).
        /// </summary>
        T3,

        /// <summary>
        /// kiloampere hour (thousand ampere hour)
        /// </summary>
        TAH,

        /// <summary>
        /// total acid number
        /// A unit of chemistry defining the amount of potassium hydroxide (KOH) in milligrams that is needed to neutralize the acids in one gram of oil. It is an important quality measurement of crude oil.
        /// </summary>
        TAN,

        /// <summary>
        /// thousand square inch
        /// </summary>
        TI,

        /// <summary>
        /// metric ton, including container
        /// A unit of mass defining the number of metric tons of a product, including its container.
        /// </summary>
        TIC,

        /// <summary>
        /// metric ton, including inner packaging
        /// A unit of mass defining the number of metric tons of a product, including its inner packaging materials.
        /// </summary>
        TIP,

        /// <summary>
        /// tonne kilometre
        /// A unit of information typically used for billing purposes, expressed as the number of tonnes (metric tons) moved over a distance of one kilometre.
        /// </summary>
        TKM,

        /// <summary>
        /// kilogram of imported meat, less offal
        /// A unit of mass equal to one thousand grams of imported meat, disregarding less valuable by-products such as the entrails.
        /// </summary>
        TMS,

        /// <summary>
        /// tonne (metric ton)
        /// Synonym: metric ton
        /// </summary>
        TNE,

        /// <summary>
        /// ten pack
        /// A unit of count defining the number of items in multiples of 10.
        /// </summary>
        TP,

        /// <summary>
        /// teeth per inch
        /// The number of teeth per inch.
        /// </summary>
        TPI,

        /// <summary>
        /// ten pair
        /// A unit of count defining the number of pairs in multiples of 10 (pair: item described by two's).
        /// </summary>
        TPR,

        /// <summary>
        /// thousand cubic metre per day
        /// A unit of volume equal to one thousand cubic metres per day.
        /// </summary>
        TQD,

        /// <summary>
        /// trillion (EUR)
        /// </summary>
        TRL,

        /// <summary>
        /// ten set
        /// A unit of count defining the number of sets in multiples of 10 (set: a number of objects grouped together).
        /// </summary>
        TST,

        /// <summary>
        /// ten thousand sticks
        /// A unit of count defining the number of sticks in multiples of 10000 (stick: slender and often cylindrical piece of a substance).
        /// </summary>
        TTS,

        /// <summary>
        /// treatment
        /// A unit of count defining the number of treatments (treatment: subjection to the action of a chemical, physical or biological agent).
        /// </summary>
        U1,

        /// <summary>
        /// tablet
        /// A unit of count defining the number of tablets (tablet: a small flat or compressed solid object).
        /// </summary>
        U2,

        /// <summary>
        /// telecommunication line in service average
        /// A unit of count defining the average number of lines in service.
        /// </summary>
        UB,

        /// <summary>
        /// telecommunication port
        /// A unit of count defining the number of network access ports.
        /// </summary>
        UC,

        /// <summary>
        /// volt - ampere per kilogram
        /// </summary>
        VA,

        /// <summary>
        /// volt
        /// </summary>
        VLT,

        /// <summary>
        /// percent volume
        /// A measure of concentration, typically expressed as the percentage volume of a solute in a solution.
        /// </summary>
        VP,

        /// <summary>
        /// wet kilo
        /// A unit of mass defining the number of kilograms of a product, including the water content of the product.
        /// </summary>
        W2,

        /// <summary>
        /// watt per kilogram
        /// </summary>
        WA,

        /// <summary>
        /// wet pound
        /// A unit of mass defining the number of pounds of a material, including the water content of the material.
        /// </summary>
        WB,

        /// <summary>
        /// cord
        /// A unit of volume used for measuring lumber. One board foot equals 1/12 of a cubic foot.
        /// </summary>
        WCD,

        /// <summary>
        /// wet ton
        /// A unit of mass defining the number of tons of a material, including the water content of the material.
        /// </summary>
        WE,

        /// <summary>
        /// weber
        /// </summary>
        WEB,

        /// <summary>
        /// week
        /// </summary>
        WEE,

        /// <summary>
        /// wine gallon
        /// A unit of volume equal to 231 cubic inches.
        /// </summary>
        WG,

        /// <summary>
        /// watt hour
        /// </summary>
        WHR,

        /// <summary>
        /// working month
        /// A unit of time defining the number of working months.
        /// </summary>
        WM,

        /// <summary>
        /// standard
        /// A unit of volume of finished lumber equal to 165 cubic feet. Synonym: standard cubic foot
        /// </summary>
        WSD,

        /// <summary>
        /// watt
        /// </summary>
        WTT,

        /// <summary>
        /// Gunter's chain
        /// A unit of distance used or formerly used by British surveyors.
        /// </summary>
        X1,

        /// <summary>
        /// square yard
        /// </summary>
        YDK,

        /// <summary>
        /// cubic yard
        /// </summary>
        YDQ,

        /// <summary>
        /// yard
        /// </summary>
        YRD,

        /// <summary>
        /// hanging container
        /// A unit of count defining the number of hanging containers.
        /// </summary>
        Z11,

        /// <summary>
        /// nanomole
        /// </summary>
        Z9,

        /// <summary>
        /// page
        /// A unit of count defining the number of pages.
        /// </summary>
        ZP,

        /// <summary>
        /// mutually defined
        /// A unit of measure as agreed in common between two or more parties.
        /// </summary>
        ZZ,

        /// <summary>
        /// Drum, steel
        /// </summary>
        X1A,

        /// <summary>
        /// Drum, aluminium
        /// </summary>
        X1B,

        /// <summary>
        /// Drum, plywood
        /// </summary>
        X1D,

        /// <summary>
        /// Container, flexible
        /// A packaging container of flexible construction.
        /// </summary>
        X1F,

        /// <summary>
        /// Drum, fibre
        /// </summary>
        X1G,

        /// <summary>
        /// Drum, wooden
        /// </summary>
        X1W,

        /// <summary>
        /// Barrel, wooden
        /// </summary>
        X2C,

        /// <summary>
        /// Jerrican, steel
        /// </summary>
        X3A,

        /// <summary>
        /// Jerrican, plastic
        /// </summary>
        X3H,

        /// <summary>
        /// Bag, super bulk
        /// A cloth plastic or paper based bag having the dimensions of the pallet on which it is constructed.
        /// </summary>
        X43,

        /// <summary>
        /// Bag, polybag
        /// A type of plastic bag, typically used to wrap promotional pieces, publications, product samples, and/or catalogues.
        /// </summary>
        X44,

        /// <summary>
        /// Box, steel
        /// </summary>
        X4A,

        /// <summary>
        /// Box, aluminium
        /// </summary>
        X4B,

        /// <summary>
        /// Box, natural wood
        /// </summary>
        X4C,

        /// <summary>
        /// Box, plywood
        /// </summary>
        X4D,

        /// <summary>
        /// Box, reconstituted wood
        /// </summary>
        X4F,

        /// <summary>
        /// Box, fibreboard
        /// </summary>
        X4G,

        /// <summary>
        /// Box, plastic
        /// </summary>
        X4H,

        /// <summary>
        /// Bag, woven plastic
        /// </summary>
        X5H,

        /// <summary>
        /// Bag, textile
        /// </summary>
        X5L,

        /// <summary>
        /// Bag, paper
        /// </summary>
        X5M,

        /// <summary>
        /// Composite packaging, plastic receptacle
        /// </summary>
        X6H,

        /// <summary>
        /// Composite packaging, glass receptacle
        /// </summary>
        X6P,

        /// <summary>
        /// Case, car
        /// A type of portable container designed to store equipment for carriage in an automobile.
        /// </summary>
        X7A,

        /// <summary>
        /// Case, wooden
        /// A case made of wood for retaining substances or articles.
        /// </summary>
        X7B,

        /// <summary>
        /// Pallet, wooden
        /// A platform or open-ended box, made of wood, on which goods are retained for ease of mechanical handling during transport and storage.
        /// </summary>
        X8A,

        /// <summary>
        /// Crate, wooden
        /// A receptacle, made of wood, on which goods are retained for ease of mechanical handling during transport and storage.
        /// </summary>
        X8B,

        /// <summary>
        /// Bundle, wooden
        /// Loose or unpacked pieces of wood tied or wrapped together.
        /// </summary>
        X8C,

        /// <summary>
        /// Intermediate bulk container, rigid plastic
        /// </summary>
        XAA,

        /// <summary>
        /// Receptacle, fibre
        /// Containment vessel made of fibre used for retaining substances or articles.
        /// </summary>
        XAB,

        /// <summary>
        /// Receptacle, paper
        /// Containment vessel made of paper for retaining substances or articles.
        /// </summary>
        XAC,

        /// <summary>
        /// Receptacle, wooden
        /// Containment vessel made of wood for retaining substances or articles.
        /// </summary>
        XAD,

        /// <summary>
        /// Aerosol
        /// </summary>
        XAE,

        /// <summary>
        /// Pallet, modular, collars 80cms * 60cms
        /// Standard sized pallet of dimensions 80 centimeters by 60 centimeters (cms).
        /// </summary>
        XAF,

        /// <summary>
        /// Pallet, shrinkwrapped
        /// Pallet load secured with transparent plastic film that has been wrapped around and then shrunk tightly.
        /// </summary>
        XAG,

        /// <summary>
        /// Pallet, 100cms * 110cms
        /// Standard sized pallet of dimensions 100centimeters by 110 centimeters (cms).
        /// </summary>
        XAH,

        /// <summary>
        /// Clamshell
        /// </summary>
        XAI,

        /// <summary>
        /// Cone
        /// Container used in the transport of linear material such as yarn.
        /// </summary>
        XAJ,

        /// <summary>
        /// Ball
        /// A spherical containment vessel for retaining substances or articles.
        /// </summary>
        XAL,

        /// <summary>
        /// Ampoule, non-protected
        /// </summary>
        XAM,

        /// <summary>
        /// Ampoule, protected
        /// </summary>
        XAP,

        /// <summary>
        /// Atomizer
        /// </summary>
        XAT,

        /// <summary>
        /// Capsule
        /// </summary>
        XAV,

        /// <summary>
        /// Belt
        /// A band use to retain multiple articles together.
        /// </summary>
        XB4,

        /// <summary>
        /// Barrel
        /// </summary>
        XBA,

        /// <summary>
        /// Bobbin
        /// </summary>
        XBB,

        /// <summary>
        /// Bottlecrate / bottlerack
        /// </summary>
        XBC,

        /// <summary>
        /// Board
        /// </summary>
        XBD,

        /// <summary>
        /// Bundle
        /// </summary>
        XBE,

        /// <summary>
        /// Balloon, non-protected
        /// </summary>
        XBF,

        /// <summary>
        /// Bag
        /// A receptacle made of flexible material with an open or closed top.
        /// </summary>
        XBG,

        /// <summary>
        /// Bunch
        /// </summary>
        XBH,

        /// <summary>
        /// Bin
        /// </summary>
        XBI,

        /// <summary>
        /// Bucket
        /// </summary>
        XBJ,

        /// <summary>
        /// Basket
        /// </summary>
        XBK,

        /// <summary>
        /// Bale, compressed
        /// </summary>
        XBL,

        /// <summary>
        /// Basin
        /// </summary>
        XBM,

        /// <summary>
        /// Bale, non-compressed
        /// </summary>
        XBN,

        /// <summary>
        /// Bottle, non-protected, cylindrical
        /// A narrow-necked cylindrical shaped vessel without external protective packing material.
        /// </summary>
        XBO,

        /// <summary>
        /// Balloon, protected
        /// </summary>
        XBP,

        /// <summary>
        /// Bottle, protected cylindrical
        /// A narrow-necked cylindrical shaped vessel with external protective packing material.
        /// </summary>
        XBQ,

        /// <summary>
        /// Bar
        /// </summary>
        XBR,

        /// <summary>
        /// Bottle, non-protected, bulbous
        /// A narrow-necked bulb shaped vessel without external protective packing material.
        /// </summary>
        XBS,

        /// <summary>
        /// Bolt
        /// </summary>
        XBT,

        /// <summary>
        /// Butt
        /// </summary>
        XBU,

        /// <summary>
        /// Bottle, protected bulbous
        /// A narrow-necked bulb shaped vessel with external protective packing material.
        /// </summary>
        XBV,

        /// <summary>
        /// Box, for liquids
        /// </summary>
        XBW,

        /// <summary>
        /// Box
        /// </summary>
        XBX,

        /// <summary>
        /// Board, in bundle/bunch/truss
        /// </summary>
        XBY,

        /// <summary>
        /// Bars, in bundle/bunch/truss
        /// </summary>
        XBZ,

        /// <summary>
        /// Can, rectangular
        /// </summary>
        XCA,

        /// <summary>
        /// Crate, beer
        /// </summary>
        XCB,

        /// <summary>
        /// Churn
        /// </summary>
        XCC,

        /// <summary>
        /// Can, with handle and spout
        /// </summary>
        XCD,

        /// <summary>
        /// Creel
        /// </summary>
        XCE,

        /// <summary>
        /// Coffer
        /// </summary>
        XCF,

        /// <summary>
        /// Cage
        /// </summary>
        XCG,

        /// <summary>
        /// Chest
        /// </summary>
        XCH,

        /// <summary>
        /// Canister
        /// </summary>
        XCI,

        /// <summary>
        /// Coffin
        /// </summary>
        XCJ,

        /// <summary>
        /// Cask
        /// </summary>
        XCK,

        /// <summary>
        /// Coil
        /// </summary>
        XCL,

        /// <summary>
        /// Card
        /// A flat package usually made of fibreboard from/to which product is often hung or attached.
        /// </summary>
        XCM,

        /// <summary>
        /// Container, not otherwise specified as transport equipment
        /// </summary>
        XCN,

        /// <summary>
        /// Carboy, non-protected
        /// </summary>
        XCO,

        /// <summary>
        /// Carboy, protected
        /// </summary>
        XCP,

        /// <summary>
        /// Cartridge
        /// Package containing a charge such as propelling explosive for firearms or ink toner for a printer.
        /// </summary>
        XCQ,

        /// <summary>
        /// Crate
        /// </summary>
        XCR,

        /// <summary>
        /// Case
        /// </summary>
        XCS,

        /// <summary>
        /// Carton
        /// </summary>
        XCT,

        /// <summary>
        /// Cup
        /// </summary>
        XCU,

        /// <summary>
        /// Cover
        /// </summary>
        XCV,

        /// <summary>
        /// Cage, roll
        /// </summary>
        XCW,

        /// <summary>
        /// Can, cylindrical
        /// </summary>
        XCX,

        /// <summary>
        /// Cylinder
        /// </summary>
        XCY,

        /// <summary>
        /// Canvas
        /// </summary>
        XCZ,

        /// <summary>
        /// Crate, multiple layer, plastic
        /// </summary>
        XDA,

        /// <summary>
        /// Crate, multiple layer, wooden
        /// </summary>
        XDB,

        /// <summary>
        /// Crate, multiple layer, cardboard
        /// </summary>
        XDC,

        /// <summary>
        /// Cage, Commonwealth Handling Equipment Pool (CHEP)
        /// </summary>
        XDG,

        /// <summary>
        /// Box, Commonwealth Handling Equipment Pool (CHEP), Eurobox
        /// A box mounted on a pallet base under the control of CHEP.
        /// </summary>
        XDH,

        /// <summary>
        /// Drum, iron
        /// </summary>
        XDI,

        /// <summary>
        /// Demijohn, non-protected
        /// </summary>
        XDJ,

        /// <summary>
        /// Crate, bulk, cardboard
        /// </summary>
        XDK,

        /// <summary>
        /// Crate, bulk, plastic
        /// </summary>
        XDL,

        /// <summary>
        /// Crate, bulk, wooden
        /// </summary>
        XDM,

        /// <summary>
        /// Dispenser
        /// </summary>
        XDN,

        /// <summary>
        /// Demijohn, protected
        /// </summary>
        XDP,

        /// <summary>
        /// Drum
        /// </summary>
        XDR,

        /// <summary>
        /// Tray, one layer no cover, plastic
        /// </summary>
        XDS,

        /// <summary>
        /// Tray, one layer no cover, wooden
        /// </summary>
        XDT,

        /// <summary>
        /// Tray, one layer no cover, polystyrene
        /// </summary>
        XDU,

        /// <summary>
        /// Tray, one layer no cover, cardboard
        /// </summary>
        XDV,

        /// <summary>
        /// Tray, two layers no cover, plastic tray
        /// </summary>
        XDW,

        /// <summary>
        /// Tray, two layers no cover, wooden
        /// </summary>
        XDX,

        /// <summary>
        /// Tray, two layers no cover, cardboard
        /// </summary>
        XDY,

        /// <summary>
        /// Bag, plastic
        /// </summary>
        XEC,

        /// <summary>
        /// Case, with pallet base
        /// </summary>
        XED,

        /// <summary>
        /// Case, with pallet base, wooden
        /// </summary>
        XEE,

        /// <summary>
        /// Case, with pallet base, cardboard
        /// </summary>
        XEF,

        /// <summary>
        /// Case, with pallet base, plastic
        /// </summary>
        XEG,

        /// <summary>
        /// Case, with pallet base, metal
        /// </summary>
        XEH,

        /// <summary>
        /// Case, isothermic
        /// </summary>
        XEI,

        /// <summary>
        /// Envelope
        /// </summary>
        XEN,

        /// <summary>
        /// Flexibag
        /// A flexible containment bag made of plastic, typically for the transportation bulk non-hazardous cargoes using standard size shipping containers.
        /// </summary>
        XFB,

        /// <summary>
        /// Crate, fruit
        /// </summary>
        XFC,

        /// <summary>
        /// Crate, framed
        /// </summary>
        XFD,

        /// <summary>
        /// Flexitank
        /// A flexible containment tank made of plastic, typically for the transportation bulk non-hazardous cargoes using standard size shipping containers.
        /// </summary>
        XFE,

        /// <summary>
        /// Firkin
        /// </summary>
        XFI,

        /// <summary>
        /// Flask
        /// </summary>
        XFL,

        /// <summary>
        /// Footlocker
        /// </summary>
        XFO,

        /// <summary>
        /// Filmpack
        /// </summary>
        XFP,

        /// <summary>
        /// Frame
        /// </summary>
        XFR,

        /// <summary>
        /// Foodtainer
        /// </summary>
        XFT,

        /// <summary>
        /// Cart, flatbed
        /// Wheeled flat bedded device on which trays or other regular shaped items are packed for transportation purposes.
        /// </summary>
        XFW,

        /// <summary>
        /// Bag, flexible container
        /// </summary>
        XFX,

        /// <summary>
        /// Bottle, gas
        /// A narrow-necked metal cylinder for retention of liquefied or compressed gas.
        /// </summary>
        XGB,

        /// <summary>
        /// Girder
        /// </summary>
        XGI,

        /// <summary>
        /// Container, gallon
        /// A container with a capacity of one gallon.
        /// </summary>
        XGL,

        /// <summary>
        /// Receptacle, glass
        /// Containment vessel made of glass for retaining substances or articles.
        /// </summary>
        XGR,

        /// <summary>
        /// Tray, containing horizontally stacked flat items
        /// Tray containing flat items stacked on top of one another.
        /// </summary>
        XGU,

        /// <summary>
        /// Bag, gunny
        /// A sack made of gunny or burlap, used for transporting coarse commodities, such as grains, potatoes, and other agricultural products.
        /// </summary>
        XGY,

        /// <summary>
        /// Girders, in bundle/bunch/truss
        /// </summary>
        XGZ,

        /// <summary>
        /// Basket, with handle, plastic
        /// </summary>
        XHA,

        /// <summary>
        /// Basket, with handle, wooden
        /// </summary>
        XHB,

        /// <summary>
        /// Basket, with handle, cardboard
        /// </summary>
        XHC,

        /// <summary>
        /// Hogshead
        /// </summary>
        XHG,

        /// <summary>
        /// Hanger
        /// A purpose shaped device with a hook at the top for hanging items from a rail.
        /// </summary>
        XHN,

        /// <summary>
        /// Hamper
        /// </summary>
        XHR,

        /// <summary>
        /// Package, display, wooden
        /// </summary>
        XIA,

        /// <summary>
        /// Package, display, cardboard
        /// </summary>
        XIB,

        /// <summary>
        /// Package, display, plastic
        /// </summary>
        XIC,

        /// <summary>
        /// Package, display, metal
        /// </summary>
        XID,

        /// <summary>
        /// Package, show
        /// </summary>
        XIE,

        /// <summary>
        /// Package, flow
        /// A flexible tubular package or skin, possibly transparent, often used for containment of foodstuffs (e.g. salami sausage).
        /// </summary>
        XIF,

        /// <summary>
        /// Package, paper wrapped
        /// </summary>
        XIG,

        /// <summary>
        /// Drum, plastic
        /// </summary>
        XIH,

        /// <summary>
        /// Package, cardboard, with bottle grip-holes
        /// Packaging material made out of cardboard that facilitates the separation of individual glass or plastic bottles.
        /// </summary>
        XIK,

        /// <summary>
        /// Tray, rigid, lidded stackable (CEN TS 14482:2002)
        /// Lidded stackable rigid tray compliant with CEN TS 14482:2002.
        /// </summary>
        XIL,

        /// <summary>
        /// Ingot
        /// </summary>
        XIN,

        /// <summary>
        /// Ingots, in bundle/bunch/truss
        /// </summary>
        XIZ,

        /// <summary>
        /// Bag, jumbo
        /// A flexible containment bag, widely used for storage, transportation and handling of powder, flake or granular materials. Typically constructed from woven polypropylene (PP) fabric in the form of cubic bags.
        /// </summary>
        XJB,

        /// <summary>
        /// Jerrican, rectangular
        /// </summary>
        XJC,

        /// <summary>
        /// Jug
        /// </summary>
        XJG,

        /// <summary>
        /// Jar
        /// </summary>
        XJR,

        /// <summary>
        /// Jutebag
        /// </summary>
        XJT,

        /// <summary>
        /// Jerrican, cylindrical
        /// </summary>
        XJY,

        /// <summary>
        /// Keg
        /// </summary>
        XKG,

        /// <summary>
        /// Kit
        /// A set of articles or implements used for a specific purpose.
        /// </summary>
        XKI,

        /// <summary>
        /// Luggage
        /// A collection of bags, cases and/or containers which hold personal belongings for a journey.
        /// </summary>
        XLE,

        /// <summary>
        /// Log
        /// </summary>
        XLG,

        /// <summary>
        /// Lot
        /// </summary>
        XLT,

        /// <summary>
        /// Lug
        /// A wooden box for the transportation and storage of fruit or vegetables.
        /// </summary>
        XLU,

        /// <summary>
        /// Liftvan
        /// A wooden or metal container used for packing household goods and personal effects.
        /// </summary>
        XLV,

        /// <summary>
        /// Logs, in bundle/bunch/truss
        /// </summary>
        XLZ,

        /// <summary>
        /// Crate, metal
        /// Containment box made of metal for retaining substances or articles.
        /// </summary>
        XMA,

        /// <summary>
        /// Bag, multiply
        /// </summary>
        XMB,

        /// <summary>
        /// Crate, milk
        /// </summary>
        XMC,

        /// <summary>
        /// Container, metal
        /// A type of containment box made of metal for retaining substances or articles, not otherwise specified as transport equipment.
        /// </summary>
        XME,

        /// <summary>
        /// Receptacle, metal
        /// Containment vessel made of metal for retaining substances or articles.
        /// </summary>
        XMR,

        /// <summary>
        /// Sack, multi-wall
        /// </summary>
        XMS,

        /// <summary>
        /// Mat
        /// </summary>
        XMT,

        /// <summary>
        /// Receptacle, plastic wrapped
        /// Containment vessel wrapped with plastic for retaining substances or articles.
        /// </summary>
        XMW,

        /// <summary>
        /// Matchbox
        /// </summary>
        XMX,

        /// <summary>
        /// Not available
        /// </summary>
        XNA,

        /// <summary>
        /// Unpacked or unpackaged
        /// </summary>
        XNE,

        /// <summary>
        /// Unpacked or unpackaged, single unit
        /// </summary>
        XNF,

        /// <summary>
        /// Unpacked or unpackaged, multiple units
        /// </summary>
        XNG,

        /// <summary>
        /// Nest
        /// </summary>
        XNS,

        /// <summary>
        /// Net
        /// </summary>
        XNT,

        /// <summary>
        /// Net, tube, plastic
        /// </summary>
        XNU,

        /// <summary>
        /// Net, tube, textile
        /// </summary>
        XNV,

        /// <summary>
        /// Two sided cage on wheels with fixing strap
        /// </summary>
        XO1,

        /// <summary>
        /// Trolley
        /// </summary>
        XO2,

        /// <summary>
        /// Oneway pallet ISO 0 - 1/2 EURO Pallet
        /// </summary>
        XO3,

        /// <summary>
        /// Oneway pallet ISO 1 - 1/1 EURO Pallet
        /// </summary>
        XO4,

        /// <summary>
        /// Oneway pallet ISO 2 - 2/1 EURO Pallet
        /// </summary>
        XO5,

        /// <summary>
        /// Pallet with exceptional dimensions
        /// </summary>
        XO6,

        /// <summary>
        /// Wooden pallet 40 cm x 80 cm
        /// </summary>
        XO7,

        /// <summary>
        /// Plastic pallet SRS 60 cm x 80 cm
        /// </summary>
        XO8,

        /// <summary>
        /// Plastic pallet SRS 80 cm x 120 cm
        /// </summary>
        XO9,

        /// <summary>
        /// Pallet, CHEP 40 cm x 60 cm
        /// Commonwealth Handling Equipment Pool (CHEP) standard pallet of dimensions 40 centimeters x 60 centimeters.
        /// </summary>
        XOA,

        /// <summary>
        /// Pallet, CHEP 80 cm x 120 cm
        /// Commonwealth Handling Equipment Pool (CHEP) standard pallet of dimensions 80 centimeters x 120 centimeters.
        /// </summary>
        XOB,

        /// <summary>
        /// Pallet, CHEP 100 cm x 120 cm
        /// Commonwealth Handling Equipment Pool (CHEP) standard pallet of dimensions 100 centimeters x 120 centimeters.
        /// </summary>
        XOC,

        /// <summary>
        /// Pallet, AS 4068-1993
        /// Australian standard pallet of dimensions 115.5 centimeters x 116.5 centimeters.
        /// </summary>
        XOD,

        /// <summary>
        /// Pallet, ISO T11
        /// ISO standard pallet of dimensions 110 centimeters x 110 centimeters, prevalent in Asia - Pacific region.
        /// </summary>
        XOE,

        /// <summary>
        /// Platform, unspecified weight or dimension
        /// A pallet equivalent shipping platform of unknown dimensions or unknown weight.
        /// </summary>
        XOF,

        /// <summary>
        /// Pallet ISO 0 - 1/2 EURO Pallet
        /// </summary>
        XOG,

        /// <summary>
        /// Pallet ISO 1 - 1/1 EURO Pallet
        /// </summary>
        XOH,

        /// <summary>
        /// Pallet ISO 2 – 2/1 EURO Pallet
        /// </summary>
        XOI,

        /// <summary>
        /// Block
        /// A solid piece of a hard substance, such as granite, having one or more flat sides.
        /// </summary>
        XOK,

        /// <summary>
        /// 1/4 EURO Pallet
        /// </summary>
        XOJ,

        /// <summary>
        /// 1/8 EURO Pallet
        /// </summary>
        XOL,

        /// <summary>
        /// Synthetic pallet ISO 1
        /// </summary>
        XOM,

        /// <summary>
        /// Synthetic pallet ISO 2
        /// </summary>
        XON,

        /// <summary>
        /// Wholesaler pallet
        /// </summary>
        XOP,

        /// <summary>
        /// Pallet 80 X 100 cm
        /// </summary>
        XOQ,

        /// <summary>
        /// Pallet 60 X 100 cm
        /// </summary>
        XOR,

        /// <summary>
        /// Oneway pallet
        /// </summary>
        XOS,

        /// <summary>
        /// Returnable pallet
        /// </summary>
        XOV,

        /// <summary>
        /// Large bag, pallet sized
        /// </summary>
        XOW,

        /// <summary>
        /// Octabin
        /// A standard cardboard container of large dimensions for storing for example vegetables, granules of plastics or other dry products.
        /// </summary>
        XOT,

        /// <summary>
        /// Container, outer
        /// A type of containment box that serves as the outer shipping container, not otherwise specified as transport equipment.
        /// </summary>
        XOU,

        /// <summary>
        /// A wheeled pallet with raised rim (81 x 67 x 135)
        /// </summary>
        XOX,

        /// <summary>
        /// A Wheeled pallet with raised rim (81 x 72 x 135)
        /// </summary>
        XOY,

        /// <summary>
        /// Wheeled pallet with raised rim ( 81 x 60 x 16)
        /// </summary>
        XOZ,

        /// <summary>
        /// CHEP pallet 60 cm x 80 cm
        /// </summary>
        XP1,

        /// <summary>
        /// Pan
        /// A shallow, wide, open container, usually of metal.
        /// </summary>
        XP2,

        /// <summary>
        /// LPR pallet 60 cm x 80 cm
        /// </summary>
        XP3,

        /// <summary>
        /// LPR pallet 80 cm x 120 cm
        /// </summary>
        XP4,

        /// <summary>
        /// Packet
        /// Small package.
        /// </summary>
        XPA,

        /// <summary>
        /// Pallet, box Combined open-ended box and pallet
        /// </summary>
        XPB,

        /// <summary>
        /// Parcel
        /// </summary>
        XPC,

        /// <summary>
        /// Pallet, modular, collars 80cms * 100cms
        /// Standard sized pallet of dimensions 80 centimeters by 100 centimeters (cms).
        /// </summary>
        XPD,

        /// <summary>
        /// Pallet, modular, collars 80cms * 120cms
        /// Standard sized pallet of dimensions 80 centimeters by 120 centimeters (cms).
        /// </summary>
        XPE,

        /// <summary>
        /// Pen
        /// A small open top enclosure for retaining animals.
        /// </summary>
        XPF,

        /// <summary>
        /// Plate
        /// </summary>
        XPG,

        /// <summary>
        /// Pitcher
        /// </summary>
        XPH,

        /// <summary>
        /// Pipe
        /// </summary>
        XPI,

        /// <summary>
        /// Punnet
        /// </summary>
        XPJ,

        /// <summary>
        /// Package
        /// Standard packaging unit.
        /// </summary>
        XPK,

        /// <summary>
        /// Pail
        /// </summary>
        XPL,

        /// <summary>
        /// Plank
        /// </summary>
        XPN,

        /// <summary>
        /// Pouch
        /// </summary>
        XPO,

        /// <summary>
        /// Piece
        /// A loose or unpacked article.
        /// </summary>
        XPP,

        /// <summary>
        /// Receptacle, plastic
        /// Containment vessel made of plastic for retaining substances or articles.
        /// </summary>
        XPR,

        /// <summary>
        /// Pot
        /// </summary>
        XPT,

        /// <summary>
        /// Tray
        /// </summary>
        XPU,

        /// <summary>
        /// Pipes, in bundle/bunch/truss
        /// </summary>
        XPV,

        /// <summary>
        /// Pallet
        /// Platform or open-ended box, usually made of wood, on which goods are retained for ease of mechanical handling during transport and storage.
        /// </summary>
        XPX,

        /// <summary>
        /// Plates, in bundle/bunch/truss
        /// </summary>
        XPY,

        /// <summary>
        /// Planks, in bundle/bunch/truss
        /// </summary>
        XPZ,

        /// <summary>
        /// Drum, steel, non-removable head
        /// </summary>
        XQA,

        /// <summary>
        /// Drum, steel, removable head
        /// </summary>
        XQB,

        /// <summary>
        /// Drum, aluminium, non-removable head
        /// </summary>
        XQC,

        /// <summary>
        /// Drum, aluminium, removable head
        /// </summary>
        XQD,

        /// <summary>
        /// Drum, plastic, non-removable head
        /// </summary>
        XQF,

        /// <summary>
        /// Drum, plastic, removable head
        /// </summary>
        XQG,

        /// <summary>
        /// Barrel, wooden, bung type
        /// </summary>
        XQH,

        /// <summary>
        /// Barrel, wooden, removable head
        /// </summary>
        XQJ,

        /// <summary>
        /// Jerrican, steel, non-removable head
        /// </summary>
        XQK,

        /// <summary>
        /// Jerrican, steel, removable head
        /// </summary>
        XQL,

        /// <summary>
        /// Jerrican, plastic, non-removable head
        /// </summary>
        XQM,

        /// <summary>
        /// Jerrican, plastic, removable head
        /// </summary>
        XQN,

        /// <summary>
        /// Box, wooden, natural wood, ordinary
        /// </summary>
        XQP,

        /// <summary>
        /// Box, wooden, natural wood, with sift proof walls
        /// </summary>
        XQQ,

        /// <summary>
        /// Box, plastic, expanded
        /// </summary>
        XQR,

        /// <summary>
        /// Box, plastic, solid
        /// </summary>
        XQS,

        /// <summary>
        /// Rod
        /// </summary>
        XRD,

        /// <summary>
        /// Ring
        /// </summary>
        XRG,

        /// <summary>
        /// Rack, clothing hanger
        /// </summary>
        XRJ,

        /// <summary>
        /// Rack
        /// </summary>
        XRK,

        /// <summary>
        /// Reel
        /// Cylindrical rotatory device with a rim at each end on which materials are wound.
        /// </summary>
        XRL,

        /// <summary>
        /// Roll
        /// </summary>
        XRO,

        /// <summary>
        /// Rednet
        /// Containment material made of red mesh netting for retaining articles (e.g. trees).
        /// </summary>
        XRT,

        /// <summary>
        /// Rods, in bundle/bunch/truss
        /// </summary>
        XRZ,

        /// <summary>
        /// Sack
        /// </summary>
        XSA,

        /// <summary>
        /// Slab
        /// </summary>
        XSB,

        /// <summary>
        /// Crate, shallow
        /// </summary>
        XSC,

        /// <summary>
        /// Spindle
        /// </summary>
        XSD,

        /// <summary>
        /// Sea-chest
        /// </summary>
        XSE,

        /// <summary>
        /// Sachet
        /// </summary>
        XSH,

        /// <summary>
        /// Skid
        /// A low movable platform or pallet to facilitate the handling and transport of goods.
        /// </summary>
        XSI,

        /// <summary>
        /// Case, skeleton
        /// </summary>
        XSK,

        /// <summary>
        /// Slipsheet
        /// Hard plastic sheeting primarily used as the base on which to stack goods to optimise the space within a container. May be used as an alternative to a palletized packaging.
        /// </summary>
        XSL,

        /// <summary>
        /// Sheetmetal
        /// </summary>
        XSM,

        /// <summary>
        /// Spool
        /// A packaging container used in the transport of such items as wire, cable, tape and yarn.
        /// </summary>
        XSO,

        /// <summary>
        /// Sheet, plastic wrapping
        /// </summary>
        XSP,

        /// <summary>
        /// Case, steel
        /// </summary>
        XSS,

        /// <summary>
        /// Sheet
        /// </summary>
        XST,

        /// <summary>
        /// Suitcase
        /// </summary>
        XSU,

        /// <summary>
        /// Envelope, steel
        /// </summary>
        XSV,

        /// <summary>
        /// Shrinkwrapped
        /// Goods retained in a transparent plastic film that has been wrapped around and then shrunk tightly on to the goods.
        /// </summary>
        XSW,

        /// <summary>
        /// Set
        /// </summary>
        XSX,

        /// <summary>
        /// Sleeve
        /// </summary>
        XSY,

        /// <summary>
        /// Sheets, in bundle/bunch/truss
        /// </summary>
        XSZ,

        /// <summary>
        /// Tablet
        /// A loose or unpacked article in the form of a bar, block or piece.
        /// </summary>
        XT1,

        /// <summary>
        /// Tub
        /// </summary>
        XTB,

        /// <summary>
        /// Tea-chest
        /// </summary>
        XTC,

        /// <summary>
        /// Tube, collapsible
        /// </summary>
        XTD,

        /// <summary>
        /// Tyre
        /// A ring made of rubber and/or metal surrounding a wheel.
        /// </summary>
        XTE,

        /// <summary>
        /// Tank container, generic
        /// A specially constructed container for transporting liquids and gases in bulk.
        /// </summary>
        XTG,

        /// <summary>
        /// Tierce
        /// </summary>
        XTI,

        /// <summary>
        /// Tank, rectangular
        /// </summary>
        XTK,

        /// <summary>
        /// Tub, with lid
        /// </summary>
        XTL,

        /// <summary>
        /// Tin
        /// </summary>
        XTN,

        /// <summary>
        /// Tun
        /// </summary>
        XTO,

        /// <summary>
        /// Trunk
        /// </summary>
        XTR,

        /// <summary>
        /// Truss
        /// </summary>
        XTS,

        /// <summary>
        /// Bag, tote
        /// A capacious bag or basket.
        /// </summary>
        XTT,

        /// <summary>
        /// Tube
        /// </summary>
        XTU,

        /// <summary>
        /// Tube, with nozzle
        /// A tube made of plastic, metal or cardboard fitted with a nozzle, containing a liquid or semi-liquid product, e.g. silicon.
        /// </summary>
        XTV,

        /// <summary>
        /// Pallet, triwall
        /// A lightweight pallet made from heavy duty corrugated board.
        /// </summary>
        XTW,

        /// <summary>
        /// Tank, cylindrical
        /// </summary>
        XTY,

        /// <summary>
        /// Tubes, in bundle/bunch/truss
        /// </summary>
        XTZ,

        /// <summary>
        /// Uncaged
        /// </summary>
        XUC,

        /// <summary>
        /// Unit
        /// A type of package composed of a single item or object, not otherwise specified as a unit of transport equipment.
        /// </summary>
        XUN,

        /// <summary>
        /// Vat
        /// </summary>
        XVA,

        /// <summary>
        /// Bulk, gas (at 1031 mbar and 15°C)
        /// </summary>
        XVG,

        /// <summary>
        /// Vial
        /// </summary>
        XVI,

        /// <summary>
        /// Vanpack
        /// A type of wooden crate.
        /// </summary>
        XVK,

        /// <summary>
        /// Bulk, liquid
        /// </summary>
        XVL,

        /// <summary>
        /// Bulk, solid, large particles (“nodules”)
        /// </summary>
        XVO,

        /// <summary>
        /// Vacuum-packed
        /// </summary>
        XVP,

        /// <summary>
        /// Bulk, liquefied gas (at abnormal temperature/pressure)
        /// </summary>
        XVQ,

        /// <summary>
        /// Vehicle
        /// A self-propelled means of conveyance.
        /// </summary>
        XVN,

        /// <summary>
        /// Bulk, solid, granular particles (“grains”)
        /// </summary>
        XVR,

        /// <summary>
        /// Bulk, scrap metal
        /// Loose or unpacked scrap metal transported in bulk form.
        /// </summary>
        XVS,

        /// <summary>
        /// Bulk, solid, fine particles (“powders”)
        /// </summary>
        XVY,

        /// <summary>
        /// Intermediate bulk container
        /// A reusable container made of metal, plastic, textile, wood or composite materials used to facilitate transportation of bulk solids and liquids in manageable volumes.
        /// </summary>
        XWA,

        /// <summary>
        /// Wickerbottle
        /// </summary>
        XWB,

        /// <summary>
        /// Intermediate bulk container, steel
        /// </summary>
        XWC,

        /// <summary>
        /// Intermediate bulk container, aluminium
        /// </summary>
        XWD,

        /// <summary>
        /// Intermediate bulk container, metal
        /// </summary>
        XWF,

        /// <summary>
        /// Intermediate bulk container, steel, pressurised > 10 kpa
        /// </summary>
        XWG,

        /// <summary>
        /// Intermediate bulk container, aluminium, pressurised > 10 kpa
        /// </summary>
        XWH,

        /// <summary>
        /// Intermediate bulk container, metal, pressure 10 kpa
        /// </summary>
        XWJ,

        /// <summary>
        /// Intermediate bulk container, steel, liquid
        /// </summary>
        XWK,

        /// <summary>
        /// Intermediate bulk container, aluminium, liquid
        /// </summary>
        XWL,

        /// <summary>
        /// Intermediate bulk container, metal, liquid
        /// </summary>
        XWM,

        /// <summary>
        /// Intermediate bulk container, woven plastic, without coat/liner
        /// </summary>
        XWN,

        /// <summary>
        /// Intermediate bulk container, woven plastic, coated
        /// </summary>
        XWP,

        /// <summary>
        /// Intermediate bulk container, woven plastic, with liner
        /// </summary>
        XWQ,

        /// <summary>
        /// Intermediate bulk container, woven plastic, coated and liner
        /// </summary>
        XWR,

        /// <summary>
        /// Intermediate bulk container, plastic film
        /// </summary>
        XWS,

        /// <summary>
        /// Intermediate bulk container, textile with out coat/liner
        /// </summary>
        XWT,

        /// <summary>
        /// Intermediate bulk container, natural wood, with inner liner
        /// </summary>
        XWU,

        /// <summary>
        /// Intermediate bulk container, textile, coated
        /// </summary>
        XWV,

        /// <summary>
        /// Intermediate bulk container, textile, with liner
        /// </summary>
        XWW,

        /// <summary>
        /// Intermediate bulk container, textile, coated and liner
        /// </summary>
        XWX,

        /// <summary>
        /// Intermediate bulk container, plywood, with inner liner
        /// </summary>
        XWY,

        /// <summary>
        /// Intermediate bulk container, reconstituted wood, with inner liner
        /// </summary>
        XWZ,

        /// <summary>
        /// Bag, woven plastic, without inner coat/liner
        /// </summary>
        XXA,

        /// <summary>
        /// Bag, woven plastic, sift proof
        /// </summary>
        XXB,

        /// <summary>
        /// Bag, woven plastic, water resistant
        /// </summary>
        XXC,

        /// <summary>
        /// Bag, plastics film
        /// </summary>
        XXD,

        /// <summary>
        /// Bag, textile, without inner coat/liner
        /// </summary>
        XXF,

        /// <summary>
        /// Bag, textile, sift proof
        /// </summary>
        XXG,

        /// <summary>
        /// Bag, textile, water resistant
        /// </summary>
        XXH,

        /// <summary>
        /// Bag, paper, multi-wall
        /// </summary>
        XXJ,

        /// <summary>
        /// Bag, paper, multi-wall, water resistant
        /// </summary>
        XXK,

        /// <summary>
        /// Composite packaging, plastic receptacle in steel drum
        /// </summary>
        XYA,

        /// <summary>
        /// Composite packaging, plastic receptacle in steel crate box
        /// </summary>
        XYB,

        /// <summary>
        /// Composite packaging, plastic receptacle in aluminium drum
        /// </summary>
        XYC,

        /// <summary>
        /// Composite packaging, plastic receptacle in aluminium crate
        /// </summary>
        XYD,

        /// <summary>
        /// Composite packaging, plastic receptacle in wooden box
        /// </summary>
        XYF,

        /// <summary>
        /// Composite packaging, plastic receptacle in plywood drum
        /// </summary>
        XYG,

        /// <summary>
        /// Composite packaging, plastic receptacle in plywood box
        /// </summary>
        XYH,

        /// <summary>
        /// Composite packaging, plastic receptacle in fibre drum
        /// </summary>
        XYJ,

        /// <summary>
        /// Composite packaging, plastic receptacle in fibreboard box
        /// </summary>
        XYK,

        /// <summary>
        /// Composite packaging, plastic receptacle in plastic drum
        /// </summary>
        XYL,

        /// <summary>
        /// Composite packaging, plastic receptacle in solid plastic box
        /// </summary>
        XYM,

        /// <summary>
        /// Composite packaging, glass receptacle in steel drum
        /// </summary>
        XYN,

        /// <summary>
        /// Composite packaging, glass receptacle in steel crate box
        /// </summary>
        XYP,

        /// <summary>
        /// Composite packaging, glass receptacle in aluminium drum
        /// </summary>
        XYQ,

        /// <summary>
        /// Composite packaging, glass receptacle in aluminium crate
        /// </summary>
        XYR,

        /// <summary>
        /// Composite packaging, glass receptacle in wooden box
        /// </summary>
        XYS,

        /// <summary>
        /// Composite packaging, glass receptacle in plywood drum
        /// </summary>
        XYT,

        /// <summary>
        /// Composite packaging, glass receptacle in wickerwork hamper
        /// </summary>
        XYV,

        /// <summary>
        /// Composite packaging, glass receptacle in fibre drum
        /// </summary>
        XYW,

        /// <summary>
        /// Composite packaging, glass receptacle in fibreboard box
        /// </summary>
        XYX,

        /// <summary>
        /// Composite packaging, glass receptacle in expandable plastic pack
        /// </summary>
        XYY,

        /// <summary>
        /// Composite packaging, glass receptacle in solid plastic pack
        /// </summary>
        XYZ,

        /// <summary>
        /// Intermediate bulk container, paper, multi-wall
        /// </summary>
        XZA,

        /// <summary>
        /// Bag, large
        /// </summary>
        XZB,

        /// <summary>
        /// Intermediate bulk container, paper, multi-wall, water resistant
        /// </summary>
        XZC,

        /// <summary>
        /// Intermediate bulk container, rigid plastic, with structural equipment, solids
        /// </summary>
        XZD,

        /// <summary>
        /// Intermediate bulk container, rigid plastic, freestanding, solids
        /// </summary>
        XZF,

        /// <summary>
        /// Intermediate bulk container, rigid plastic, with structural equipment, pressurised
        /// </summary>
        XZG,

        /// <summary>
        /// Intermediate bulk container, rigid plastic, freestanding, pressurised
        /// </summary>
        XZH,

        /// <summary>
        /// Intermediate bulk container, rigid plastic, with structural equipment, liquids
        /// </summary>
        XZJ,

        /// <summary>
        /// Intermediate bulk container, rigid plastic, freestanding, liquids
        /// </summary>
        XZK,

        /// <summary>
        /// Intermediate bulk container, composite, rigid plastic, solids
        /// </summary>
        XZL,

        /// <summary>
        /// Intermediate bulk container, composite, flexible plastic, solids
        /// </summary>
        XZM,

        /// <summary>
        /// Intermediate bulk container, composite, rigid plastic, pressurised
        /// </summary>
        XZN,

        /// <summary>
        /// Intermediate bulk container, composite, flexible plastic, pressurised
        /// </summary>
        XZP,

        /// <summary>
        /// Intermediate bulk container, composite, rigid plastic, liquids
        /// </summary>
        XZQ,

        /// <summary>
        /// Intermediate bulk container, composite, flexible plastic, liquids
        /// </summary>
        XZR,

        /// <summary>
        /// Intermediate bulk container, composite
        /// </summary>
        XZS,

        /// <summary>
        /// Intermediate bulk container, fibreboard
        /// </summary>
        XZT,

        /// <summary>
        /// Intermediate bulk container, flexible
        /// </summary>
        XZU,

        /// <summary>
        /// Intermediate bulk container, metal, other than steel
        /// </summary>
        XZV,

        /// <summary>
        /// Intermediate bulk container, natural wood
        /// </summary>
        XZW,

        /// <summary>
        /// Intermediate bulk container, plywood
        /// </summary>
        XZX,

        /// <summary>
        /// Intermediate bulk container, reconstituted wood
        /// </summary>
        XZY,

        /// <summary>
        /// Mutually defined
        /// </summary>
        XZZ
        #endregion
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
                return (QuantityCodes)Enum.Parse(typeof(QuantityCodes), s);
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
