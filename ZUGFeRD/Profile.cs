﻿/*
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
    /// ZUGFeRD allows reading and writing invoices in various profiles, each containing different density of information.
    /// </summary>
    public enum Profile
    {
        /// <summary>
        /// Fallback value
        /// </summary>
        Unknown = 65536,

        /// <summary>
        /// Contains core line
        /// information required or useful for buyers for their process automation.
        /// </summary>
        Basic = 1,

        /// <summary>
        /// The Comfort profile corresponds to the European standard EN 16931.
        ///
        /// Invoices in this profile contain all necessary information and thus are valid electronic invoices.
        /// </summary>
        Comfort = 2,

        /// <summary>
        /// Based on the Comfort/ EN 16931 profile. Contains additional information.
        /// </summary>
        Extended = 4,

        /// <summary>
        /// corresponding to the minimum invoice information
        /// Invoices in this profile are no valid electronic invoices.
        /// They contain document level invoice information that are mostly required or useful for buyers for their process automation.
        /// </summary>
        Minimum = 8,

        /// <summary>
        /// Invoices in this profile are no valid electronic invoices.
        /// They contain document level invoice information that are mostly required or useful for buyers for their process automation.
        /// </summary>
        BasicWL = 16,

        /// <summary>
        /// Invoice format based on EU Directive 2014/55/EU, adopted to Germany in E-Invoice Law of April 4, 2017 (BGBl. I p. 770)
        /// </summary>
        XRechnung = 32,

        /// <summary>
        /// Invoice format based on EU Directive 2014/55/EU, adopted to Germany in E-Invoice Law of April 4, 2017 (BGBl. I p. 770).
        /// Important note: using this profile will generate a version 1 XRechnung (as valid until 31/12/2020)
        /// </summary>
        XRechnung1 = 64,

        /// <summary>
        /// The e-reporting (https://www.impots.gouv.fr/e-reporting-la-transmission-de-donnees-de-transaction-ladministration)
        /// concern companies subject to VAT in France and trading with private individuals and, more generally, non-taxable persons
        /// (business to consumer or BtoC), with companies not established on French territory (i.e. taxable persons who do not have
        /// an establishment, domicile or habitual residence in France).
        /// </summary>
        EReporting = 128,
    }


    public static class ProfileExtensions
    {
        internal static Profile FromString(this Profile _, string s)
        {
            var profileMap = new Dictionary<string, Profile>(StringComparer.OrdinalIgnoreCase)
            {
                // v1
                { "urn:ferd:invoice:1.0:basic", Profile.Basic },
                { "urn:ferd:invoice:rc:basic", Profile.Basic },
                { "urn:ferd:CrossIndustryDocument:invoice:1p0:basic", Profile.Basic },
                { "urn:ferd:invoice:1.0:comfort", Profile.Comfort },
                { "urn:ferd:invoice:rc:comfort", Profile.Comfort },
                { "urn:ferd:CrossIndustryDocument:invoice:1p0:comfort", Profile.Comfort },
                { "urn:ferd:CrossIndustryDocument:invoice:1p0:E", Profile.Comfort },
                { "urn:ferd:invoice:1.0:extended", Profile.Extended },
                { "urn:ferd:invoice:rc:extended", Profile.Extended },
                { "urn:ferd:CrossIndustryDocument:invoice:1p0:extended", Profile.Extended },

                // v2
                { "urn:zugferd.de:2p0:minimum", Profile.Minimum },
                { "urn:cen.eu:en16931:2017#compliant#urn:zugferd.de:2p0:basic", Profile.Basic },
                { "urn:cen.eu:en16931:2017", Profile.Comfort },
                { "urn:cen.eu:en16931:2017#conformant#urn:zugferd.de:2p0:extended", Profile.Extended },

                // v2.3
                { "urn:factur-x.eu:1p0:minimum", Profile.Minimum },
                { "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic", Profile.Basic },
                { "urn:factur-x.eu:1p0:basicwl", Profile.BasicWL },
                // { "urn:cen.eu:en16931:2017", Profile.Comfort }, already inserted for v2. In case we remove v2, this line should be uncommented
                { "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", Profile.Extended },
                { "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_1.2", Profile.XRechnung1 },
                { "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.0", Profile.XRechnung },
                { "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.1", Profile.XRechnung },
                { "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.2", Profile.XRechnung },
                { "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.3", Profile.XRechnung },
                { "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0", Profile.XRechnung },
                { "urn.cpro.gouv.fr:1p0:ereporting", Profile.EReporting }
            };

            if (profileMap.TryGetValue(s, out Profile profile))
            {
                return profile;
            }
            else
            {
                return Profile.Unknown;
            }
        } // !FromString()


        internal static string EnumToString(this Profile profile, ZUGFeRDVersion version)
        {
            switch (version)
            {
                case ZUGFeRDVersion.Version1:
                    switch (profile)
                    {
                        case Profile.Basic: return "urn:ferd:CrossIndustryDocument:invoice:1p0:basic";
                        case Profile.Comfort: return "urn:ferd:CrossIndustryDocument:invoice:1p0:comfort";
                        case Profile.Extended: return "urn:ferd:CrossIndustryDocument:invoice:1p0:extended";
                        default: throw new Exception("Unsupported profile for ZUGFeRD version 1");
                    }
                case ZUGFeRDVersion.Version20:
                    switch (profile)
                    {
                        case Profile.Minimum: return "urn:zugferd.de:2p0:minimum";
                        case Profile.Basic: return "urn:cen.eu:en16931:2017#compliant#urn:zugferd.de:2p0:basic";
                        case Profile.BasicWL: return "urn:zugferd.de:2p0:basicwl";
                        case Profile.Comfort: return "urn:cen.eu:en16931:2017";
                        case Profile.Extended: return "urn:cen.eu:en16931:2017#conformant#urn:zugferd.de:2p0:extended";
                        default: throw new Exception("Unsupported profile for ZUGFeRD version 20");
                    }
                case ZUGFeRDVersion.Version23:
                    switch (profile)
                    {
                        case Profile.Minimum: return "urn:factur-x.eu:1p0:minimum";
                        case Profile.Basic: return "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic";
                        case Profile.BasicWL: return "urn:factur-x.eu:1p0:basicwl";
                        case Profile.Comfort: return "urn:cen.eu:en16931:2017";
                        case Profile.Extended: return "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended";
                        case Profile.XRechnung1: return "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_1.2";
                        case Profile.XRechnung:
                            {
                                if (DateTime.Now >= new DateTime(2024, 02, 01))
                                {
                                    return "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0";
                                }
                                else
                                {
                                    return "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.3";
                                }
                            }
                        case Profile.EReporting: return "urn.cpro.gouv.fr:1p0:ereporting";
                        default: throw new Exception("Unsupported profile for ZUGFeRD version 21");
                    }
                default:
                    // return "";
                    throw new UnsupportedException("New ZUGFeRDVersion '" + version + "' defined but not implemented!");
            }
        } // !EnumToString()


        public static string GetXMPName(this Profile profile)
        {
            switch (profile)
            {
                case Profile.BasicWL: return "BASIC WL";
                case Profile.Comfort:
                case Profile.XRechnung1:
                case Profile.XRechnung: return "EN 16931";
                default: return profile.ToString().ToUpper();
            }
        } // !GetXMPName()


        internal static bool In(this Profile profile, params Profile[] allowedProfiles)
        {
            return allowedProfiles.Contains(profile);
        }
    }
}
