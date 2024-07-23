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
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Sources:
    /// http://www.unece.org/trade/untdid/d13b/tred/tred1153.htm
    /// and
    /// https://www.xrepository.de/details/urn:xoev-de:kosit:codeliste:untdid.1153_3#version
    /// </summary>
    public enum ReferenceTypeCodes
    {
        /// <summary>
        /// Unknown/ invalid value
        /// </summary>
        Unknown = 0, 

        /// <summary>
        /// Auftragsbestätigungsnummer
        /// </summary>
        AAA,

        /// <summary>
        /// Proforma-Rechnung
        /// </summary>
        AAB,

        /// <summary>
        /// Angebotsnummer
        /// </summary>
        AAG,

        /// <summary>
        /// Lieferauftragsnummer
        /// </summary>
        AAJ,

        /// <summary>
        /// Zeichnungsnummer
        /// </summary>
        AAL,

        /// <summary>
        /// Frachtbriefnummer
        /// </summary>
        AAM,

        /// <summary>
        /// Transportdokumenten-Nummer
        /// 
        /// Referenz zu einem Transportdokument, vergeben vom Frachtführer oder seinem Agenten. (z.B.
        /// Paketdienst-Zustell-Nr.)
        /// </summary>
        AAS,

        /// <summary>
        /// Zollerklärungsnummer
        /// </summary>
        ABT,

        /// <summary>
        /// Vehicle licence number
        /// 
        /// Number of the licence issued for a vehicle by an agency of government.
        /// </summary>
        ABZ,

        /// <summary>
        /// Projektspezifikationsnummer
        /// </summary>
        AER,

        /// <summary>
        /// Reklamationsummer
        /// </summary>
        AGG,

        /// <summary>
        /// Vereinbarungs-Nummer
        /// </summary>
        AJS,

        /// <summary>
        /// Vehicle Identification Number (VIN)
        ///
        /// The identification number which uniquely distinguishes one vehicle from another through the lifespan of the vehicle.
        /// </summary>
        AKG,

        /// <summary>
        /// Nummer einer Rücksendungsanzeige
        /// 
        /// Referenznummer für eine Rücksendungsanzeige. (z.B. Retourennummer)
        /// </summary>
        ALQ,

        /// <summary>
        /// Wareneingangsmeldung-Nummer
        /// </summary>
        ALO,

        /// <summary>
        /// Bestandsberichtnr. Bei Inventurdifferenzen in Berechnung
        /// </summary>
        API,

        /// <summary>
        /// Referenznummer zum Abliefernachweis
        /// </summary>
        ASI,

        /// <summary>
        /// Inkasso-Referenz
        /// </summary>
        AUD,

        /// <summary>
        /// Net area supplier reference
        /// A reference identifying a supplier within a net area.
        /// </summary>
        AUT,

        /// <summary>
        /// Ursprungsbelegnummer
        /// </summary>
        AWR,

        /// <summary>
        /// Rahmenauftragsnummer
        /// </summary>
        BO,

        /// <summary>
        /// Vertragsnummer (Käufer)
        /// 
        /// Referenznummer vergeben vom Käufer für einen Vertrag (z.B. Abkommennummer)
        /// </summary>
        BC,

        /// <summary>
        /// Gutschrift
        /// </summary>
        CD,

        /// <summary>
        /// Belastunganzeige
        /// </summary>
        DL,

        /// <summary>
        /// Invoice document identifier
        /// [1334]
        /// Reference number to identify an invoice.
        /// </summary>
        IV,

        /// <summary>
        /// Zählernummer
        /// 
        /// z.B. Zählpunktbezeichnung
        /// </summary>
        MG,

        /// <summary>
        /// Vorherige Rechnungsnummer
        /// </summary>
        OI,

        /// <summary>
        /// Preisliste
        /// </summary>
        PL,

        /// <summary>
        /// Packlistennummer
        /// </summary>
        PK,

        /// <summary>
        /// Bestellantwort
        /// </summary>
        POR,

        /// <summary>
        /// Bestelländerung
        /// </summary>
        PP,

        /// <summary>
        /// Transportauftragsnummer
        /// </summary>
        TIN,

        /// <summary>
        /// Auftragsnummer (Lieferant)
        /// </summary>
        VN
    }


    internal static class ReferenceTypeCodesExtensions
    {
        public static ReferenceTypeCodes FromString(this ReferenceTypeCodes _, string s)
        {
            try
            {
                return (ReferenceTypeCodes)Enum.Parse(typeof(ReferenceTypeCodes), s);
            }
            catch
            {
                return ReferenceTypeCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this ReferenceTypeCodes c)
        {
            return c.ToString("g");
        } // !ToString()
    }
}
