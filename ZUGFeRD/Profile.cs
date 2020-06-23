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
    public enum Profile
    {
        Unknown = 0,
        Basic = 1,
        Comfort = 2,
        Extended = 3,
        Minimum = 4,
        BasicWL = 5,
        Standard = 6
    }


    internal static class ProfileExtensions
    {
        public static Profile FromString(this Profile _p, string s)
        {
            switch (s)
            {
                // v1
                case "urn:ferd:invoice:1.0:basic": return Profile.Basic;
                case "urn:ferd:invoice:rc:basic": return Profile.Basic;
                case "urn:ferd:CrossIndustryDocument:invoice:1p0:basic": return Profile.Basic;
                case "urn:ferd:invoice:1.0:comfort": return Profile.Comfort;
                case "urn:ferd:invoice:rc:comfort": return Profile.Comfort;
                case "urn:ferd:CrossIndustryDocument:invoice:1p0:comfort": return Profile.Comfort;
                case "urn:ferd:CrossIndustryDocument:invoice:1p0:E": return Profile.Comfort;
                case "urn:ferd:invoice:1.0:extended": return Profile.Extended;
                case "urn:ferd:invoice:rc:extended": return Profile.Extended;
                case "urn:ferd:CrossIndustryDocument:invoice:1p0:extended": return Profile.Extended;

                // v2
                case "urn:zugferd.de:2p0:minimum": return Profile.Minimum;
                case "urn:cen.eu:en16931:2017#compliant#urn:zugferd.de:2p0:basic": return Profile.Basic;
                case "urn: cen.eu:en16931: 2017": return Profile.Comfort;
                case "urn:cen.eu:en16931:2017#conformant#urn:zugferd.de:2p0:extended": return Profile.Extended;

                // v2.1
                case "urn:factur-x.eu:1p0:minimum": return Profile.Minimum;
                case "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic": return Profile.Basic;
                case "urn:factur-x.eu:1p0:basicwl": return Profile.BasicWL;
                case "urn:cen.eu:en16931:2017": return Profile.Comfort;
                case "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended": return Profile.Extended;
                case "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_1.2": return Profile.Standard;
            }

            return Profile.Unknown;
        } // !FromString()


        public static string EnumToString(this Profile profile, ZUGFeRDVersion version)
        {
            switch (version)
            {
                case ZUGFeRDVersion.Version1:
                    switch (profile)
                    {
                        case Profile.Basic: return "urn:ferd:CrossIndustryDocument:invoice:1p0:basic";
                        case Profile.Comfort: return "urn:ferd:CrossIndustryDocument:invoice:1p0:comfort";
                        case Profile.Extended: return "urn:ferd:CrossIndustryDocument:invoice:1p0:extended";
                        default: return "";
                    }
                case ZUGFeRDVersion.Version20:
                    switch (profile)
                    {
                        case Profile.Minimum: return "urn:zugferd.de:2p0:minimum";
                        case Profile.Basic: return "urn:cen.eu:en16931:2017#compliant#urn:zugferd.de:2p0:basic";
                        case Profile.Comfort: return "urn:cen.eu:en16931:2017";
                        case Profile.Extended: return "urn:cen.eu:en16931:2017#conformant#urn:zugferd.de:2p0:extended";
                        default: return "";
                    }
                case ZUGFeRDVersion.Version21:
                    switch (profile)
                    {
                        case Profile.Minimum: return "urn:factur-x.eu:1p0:minimum";
                        case Profile.Basic: return "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic";
                        case Profile.BasicWL: return "urn:factur-x.eu:1p0:basicwl";
                        case Profile.Comfort: return "urn:cen.eu:en16931:2017";
                        case Profile.Extended: return "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended";
                        default: return "";
                    }
                default:
                    return "";
            }
        } // !ToString()
    }
}
