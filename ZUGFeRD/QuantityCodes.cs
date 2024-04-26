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
    /// Bobbin
    /// </summary>
    XBB,

    /// <summary>
    /// Bottlecrate / bottlerack
    /// </summary>
    XBC,

    /// <summary>
    /// Balloon, non-protected
    /// </summary>
    XBF,

    /// <summary>
    /// Bunch
    /// </summary>
    XBH,

    /// <summary>
    /// Bin
    /// </summary>
    XBI,

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
    /// Wooden pallet  40 cm x 80 cm
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
    /// Pallet ISO 2 â 2/1 EURO Pallet
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
    /// Rednet
    /// Containment material made of red mesh netting for retaining articles (e.g. trees).
    /// </summary>
    XRT,

    /// <summary>
    /// Rods, in bundle/bunch/truss
    /// </summary>
    XRZ,

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
    /// Bulk, gas (at 1031 mbar and 15Â°C)
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
    /// Bulk, solid, large particles (ânodulesâ)
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
    /// Bulk, solid, granular particles (âgrainsâ)
    /// </summary>
    XVR,

    /// <summary>
    /// Bulk, scrap metal
    /// Loose or unpacked scrap metal transported in bulk form.
    /// </summary>
    XVS,

    /// <summary>
    /// Bulk, solid, fine particles (âpowdersâ)
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
    /// Intermediate bulk container, steel, pressurised &gt; 10 kpa
    /// </summary>
    XWG,

    /// <summary>
    /// Intermediate bulk container, aluminium, pressurised &gt; 10 kpa
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
