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
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public enum CountryCodes
    {
        /// <summary>
        /// Andorra
        /// </summary>
        [EnumStringValue("AD")]
        AD,

        /// <summary>
        /// United Arab Emirates (the)
        /// </summary>
        [EnumStringValue("AE")]
        AE,

        /// <summary>
        /// Afghanistan
        /// </summary>
        [EnumStringValue("AF")]
        AF,

        /// <summary>
        /// Antigua and Barbuda
        /// </summary>
        [EnumStringValue("AG")]
        AG,

        /// <summary>
        /// Anguilla
        /// </summary>
        [EnumStringValue("AI")]
        AI,

        /// <summary>
        /// Albania
        /// </summary>
        [EnumStringValue("AL")]
        AL,

        /// <summary>
        /// Armenia
        /// </summary>
        [EnumStringValue("AM")]
        AM,

        /// <summary>
        /// Angola
        /// </summary>
        [EnumStringValue("AO")]
        AO,

        /// <summary>
        /// Antarctica
        /// </summary>
        [EnumStringValue("AQ")]
        AQ,

        /// <summary>
        /// Argentina
        /// </summary>
        [EnumStringValue("AR")]
        AR,

        /// <summary>
        /// American Samoa
        /// </summary>
        [EnumStringValue("AS")]
        AS,

        /// <summary>
        /// Austria
        /// </summary>
        [EnumStringValue("AT")]
        AT,

        /// <summary>
        /// Australia
        /// </summary>
        [EnumStringValue("AU")]
        AU,

        /// <summary>
        /// Aruba
        /// </summary>
        [EnumStringValue("AW")]
        AW,

        /// <summary>
        /// Åland Islands
        /// </summary>
        [EnumStringValue("AX")]
        AX,

        /// <summary>
        /// Azerbaijan
        /// </summary>
        [EnumStringValue("AZ")]
        AZ,

        /// <summary>
        /// Bosnia and Herzegovina
        /// </summary>
        [EnumStringValue("BA")]
        BA,

        /// <summary>
        /// Barbados
        /// </summary>
        [EnumStringValue("BB")]
        BB,

        /// <summary>
        /// Bangladesh
        /// </summary>
        [EnumStringValue("BD")]
        BD,

        /// <summary>
        /// Belgium
        /// </summary>
        [EnumStringValue("BE")]
        BE,

        /// <summary>
        /// Burkina Faso
        /// </summary>
        [EnumStringValue("BF")]
        BF,

        /// <summary>
        /// Bulgaria
        /// </summary>
        [EnumStringValue("BG")]
        BG,

        /// <summary>
        /// Bahrain
        /// </summary>
        [EnumStringValue("BH")]
        BH,

        /// <summary>
        /// Burundi
        /// </summary>
        [EnumStringValue("BI")]
        BI,

        /// <summary>
        /// Benin
        /// </summary>
        [EnumStringValue("BJ")]
        BJ,

        /// <summary>
        /// Saint Barthélemy
        /// </summary>
        [EnumStringValue("BL")]
        BL,

        /// <summary>
        /// Bermuda
        /// </summary>
        [EnumStringValue("BM")]
        BM,

        /// <summary>
        /// Brunei Darussalam
        /// </summary>
        [EnumStringValue("BN")]
        BN,

        /// <summary>
        /// Bolivia (Plurinational State of)
        /// </summary>
        [EnumStringValue("BO")]
        BO,

        /// <summary>
        /// Caribbean Netherlands
        /// </summary>
        [EnumStringValue("BQ")]
        BQ,

        /// <summary>
        /// Brazil
        /// </summary>
        [EnumStringValue("BR")]
        BR,

        /// <summary>
        /// Bahamas (the)
        /// </summary>
        [EnumStringValue("BS")]
        BS,

        /// <summary>
        /// Bhutan
        /// </summary>
        [EnumStringValue("BT")]
        BT,

        /// <summary>
        /// Bouvet Island
        /// </summary>
        [EnumStringValue("BV")]
        BV,

        /// <summary>
        /// Botswana
        /// </summary>
        [EnumStringValue("BW")]
        BW,

        /// <summary>
        /// Belarus
        /// </summary>
        [EnumStringValue("BY")]
        BY,

        /// <summary>
        /// Belize
        /// </summary>
        [EnumStringValue("BZ")]
        BZ,

        /// <summary>
        /// Canada
        /// </summary>
        [EnumStringValue("CA")]
        CA,

        /// <summary>
        /// Cocos (Keeling) Islands (the)
        /// </summary>
        [EnumStringValue("CC")]
        CC,

        /// <summary>
        /// Congo (the Democratic Republic of the)
        /// </summary>
        [EnumStringValue("CD")]
        CD,

        /// <summary>
        /// Central African Republic (the)
        /// </summary>
        [EnumStringValue("CF")]
        CF,

        /// <summary>
        /// Congo (the)
        /// </summary>
        [EnumStringValue("CG")]
        CG,

        /// <summary>
        /// Switzerland
        /// </summary>
        [EnumStringValue("CH")]
        CH,

        /// <summary>
        /// Côte d'Ivoire
        /// </summary>
        [EnumStringValue("CI")]
        CI,

        /// <summary>
        /// Cook Islands (the)
        /// </summary>
        [EnumStringValue("CK")]
        CK,

        /// <summary>
        /// Chile
        /// </summary>
        [EnumStringValue("CL")]
        CL,

        /// <summary>
        /// Cameroon
        /// </summary>
        [EnumStringValue("CM")]
        CM,

        /// <summary>
        /// China
        /// </summary>
        [EnumStringValue("CN")]
        CN,

        /// <summary>
        /// Colombia
        /// </summary>
        [EnumStringValue("CO")]
        CO,

        /// <summary>
        /// Costa Rica
        /// </summary>
        [EnumStringValue("CR")]
        CR,

        /// <summary>
        /// Cuba
        /// </summary>
        [EnumStringValue("CU")]
        CU,

        /// <summary>
        /// Cabo Verde
        /// </summary>
        [EnumStringValue("CV")]
        CV,

        /// <summary>
        /// Curaçao
        /// </summary>
        [EnumStringValue("CW")]
        CW,

        /// <summary>
        /// Christmas Island
        /// </summary>
        [EnumStringValue("CX")]
        CX,

        /// <summary>
        /// Cyprus
        /// </summary>
        [EnumStringValue("CY")]
        CY,

        /// <summary>
        /// Czechia
        /// </summary>
        [EnumStringValue("CZ")]
        CZ,

        /// <summary>
        /// Germany
        /// </summary>
        [EnumStringValue("DE")]
        DE,

        /// <summary>
        /// Djibouti
        /// </summary>
        [EnumStringValue("DJ")]
        DJ,

        /// <summary>
        /// Denmark
        /// </summary>
        [EnumStringValue("DK")]
        DK,

        /// <summary>
        /// Dominica
        /// </summary>
        [EnumStringValue("DM")]
        DM,

        /// <summary>
        /// Dominican Republic (the)
        /// </summary>
        [EnumStringValue("DO")]
        DO,

        /// <summary>
        /// Algeria
        /// </summary>
        [EnumStringValue("DZ")]
        DZ,

        /// <summary>
        /// Ecuador
        /// </summary>
        [EnumStringValue("EC")]
        EC,

        /// <summary>
        /// Estonia
        /// </summary>
        [EnumStringValue("EE")]
        EE,

        /// <summary>
        /// Egypt
        /// </summary>
        [EnumStringValue("EG")]
        EG,

        /// <summary>
        /// Western Sahara
        /// </summary>
        [EnumStringValue("EH")]
        EH,

        /// <summary>
        /// Eritrea
        /// </summary>
        [EnumStringValue("ER")]
        ER,

        /// <summary>
        /// Spain
        /// </summary>
        [EnumStringValue("ES")]
        ES,

        /// <summary>
        /// Ethiopia
        /// </summary>
        [EnumStringValue("ET")]
        ET,

        /// <summary>
        /// Finland
        /// </summary>
        [EnumStringValue("FI")]
        FI,

        /// <summary>
        /// Fiji
        /// </summary>
        [EnumStringValue("FJ")]
        FJ,

        /// <summary>
        /// Falkland Islands (the) [Malvinas]
        /// </summary>
        [EnumStringValue("FK")]
        FK,

        /// <summary>
        /// Micronesia (Federated States of)
        /// </summary>
        [EnumStringValue("FM")]
        FM,

        /// <summary>
        /// Faroe Islands (the)
        /// </summary>
        [EnumStringValue("FO")]
        FO,

        /// <summary>
        /// France
        /// </summary>
        [EnumStringValue("FR")]
        FR,

        /// <summary>
        /// Gabon
        /// </summary>
        [EnumStringValue("GA")]
        GA,

        /// <summary>
        /// United Kingdom of Great Britain and Northern Ireland (the)
        /// </summary>
        [EnumStringValue("GB")]
        GB,

        /// <summary>
        /// Grenada
        /// </summary>
        [EnumStringValue("GD")]
        GD,

        /// <summary>
        /// Georgia
        /// </summary>
        [EnumStringValue("GE")]
        GE,

        /// <summary>
        /// French Guiana
        /// </summary>
        [EnumStringValue("GF")]
        GF,

        /// <summary>
        /// Guernsey
        /// </summary>
        [EnumStringValue("GG")]
        GG,

        /// <summary>
        /// Ghana
        /// </summary>
        [EnumStringValue("GH")]
        GH,

        /// <summary>
        /// Gibraltar
        /// </summary>
        [EnumStringValue("GI")]
        GI,

        /// <summary>
        /// Greenland
        /// </summary>
        [EnumStringValue("GL")]
        GL,

        /// <summary>
        /// Gambia (the)
        /// </summary>
        [EnumStringValue("GM")]
        GM,

        /// <summary>
        /// Guinea
        /// </summary>
        [EnumStringValue("GN")]
        GN,

        /// <summary>
        /// Guadeloupe
        /// </summary>
        [EnumStringValue("GP")]
        GP,

        /// <summary>
        /// Equatorial Guinea
        /// </summary>
        [EnumStringValue("GQ")]
        GQ,

        /// <summary>
        /// Greece
        /// </summary>
        [EnumStringValue("GR")]
        GR,

        /// <summary>
        /// South Georgia and the South Sandwich Islands
        /// </summary>
        [EnumStringValue("GS")]
        GS,

        /// <summary>
        /// Guatemala
        /// </summary>
        [EnumStringValue("GT")]
        GT,

        /// <summary>
        /// Guam
        /// </summary>
        [EnumStringValue("GU")]
        GU,

        /// <summary>
        /// Guinea-Bissau
        /// </summary>
        [EnumStringValue("GW")]
        GW,

        /// <summary>
        /// Guyana
        /// </summary>
        [EnumStringValue("GY")]
        GY,

        /// <summary>
        /// Hong Kong
        /// </summary>
        [EnumStringValue("HK")]
        HK,

        /// <summary>
        /// Heard Island and McDonald Islands
        /// </summary>
        [EnumStringValue("HM")]
        HM,

        /// <summary>
        /// Honduras
        /// </summary>
        [EnumStringValue("HN")]
        HN,

        /// <summary>
        /// Croatia
        /// </summary>
        [EnumStringValue("HR")]
        HR,

        /// <summary>
        /// Haiti
        /// </summary>
        [EnumStringValue("HT")]
        HT,

        /// <summary>
        /// Hungary
        /// </summary>
        [EnumStringValue("HU")]
        HU,

        /// <summary>
        /// Indonesia
        /// </summary>
        [EnumStringValue("ID")]
        ID,

        /// <summary>
        /// Ireland
        /// </summary>
        [EnumStringValue("IE")]
        IE,

        /// <summary>
        /// Israel
        /// </summary>
        [EnumStringValue("IL")]
        IL,

        /// <summary>
        /// Isle of Man
        /// </summary>
        [EnumStringValue("IM")]
        IM,

        /// <summary>
        /// India
        /// </summary>
        [EnumStringValue("IN")]
        IN,

        /// <summary>
        /// British Indian Ocean Territory (the)
        /// </summary>
        [EnumStringValue("IO")]
        IO,

        /// <summary>
        /// Iraq
        /// </summary>
        [EnumStringValue("IQ")]
        IQ,

        /// <summary>
        /// Iran (Islamic Republic of)
        /// </summary>
        [EnumStringValue("IR")]
        IR,

        /// <summary>
        /// Iceland
        /// </summary>
        [EnumStringValue("IS")]
        IS,

        /// <summary>
        /// Italy
        /// </summary>
        [EnumStringValue("IT")]
        IT,

        /// <summary>
        /// Jersey
        /// </summary>
        [EnumStringValue("JE")]
        JE,

        /// <summary>
        /// Jamaica
        /// </summary>
        [EnumStringValue("JM")]
        JM,

        /// <summary>
        /// Jordan
        /// </summary>
        [EnumStringValue("JO")]
        JO,

        /// <summary>
        /// Japan
        /// </summary>
        [EnumStringValue("JP")]
        JP,

        /// <summary>
        /// Kenya
        /// </summary>
        [EnumStringValue("KE")]
        KE,

        /// <summary>
        /// Kyrgyzstan
        /// </summary>
        [EnumStringValue("KG")]
        KG,

        /// <summary>
        /// Cambodia
        /// </summary>
        [EnumStringValue("KH")]
        KH,

        /// <summary>
        /// Kiribati
        /// </summary>
        [EnumStringValue("KI")]
        KI,

        /// <summary>
        /// Comoros (the)
        /// </summary>
        [EnumStringValue("KM")]
        KM,

        /// <summary>
        /// Saint Kitts and Nevis
        /// </summary>
        [EnumStringValue("KN")]
        KN,

        /// <summary>
        /// Korea (the Democratic People's Republic of)
        /// </summary>
        [EnumStringValue("KP")]
        KP,

        /// <summary>
        /// Korea (the Republic of)
        /// </summary>
        [EnumStringValue("KR")]
        KR,

        /// <summary>
        /// Kuwait
        /// </summary>
        [EnumStringValue("KW")]
        KW,

        /// <summary>
        /// Cayman Islands (the)
        /// </summary>
        [EnumStringValue("KY")]
        KY,

        /// <summary>
        /// Kazakhstan
        /// </summary>
        [EnumStringValue("KZ")]
        KZ,

        /// <summary>
        /// Lao People's Democratic Republic (the)
        /// </summary>
        [EnumStringValue("LA")]
        LA,

        /// <summary>
        /// Lebanon
        /// </summary>
        [EnumStringValue("LB")]
        LB,

        /// <summary>
        /// Saint Lucia
        /// </summary>
        [EnumStringValue("LC")]
        LC,

        /// <summary>
        /// Liechtenstein
        /// </summary>
        [EnumStringValue("LI")]
        LI,

        /// <summary>
        /// Sri Lanka
        /// </summary>
        [EnumStringValue("LK")]
        LK,

        /// <summary>
        /// Liberia
        /// </summary>
        [EnumStringValue("LR")]
        LR,

        /// <summary>
        /// Lesotho
        /// </summary>
        [EnumStringValue("LS")]
        LS,

        /// <summary>
        /// Lithuania
        /// </summary>
        [EnumStringValue("LT")]
        LT,

        /// <summary>
        /// Luxembourg
        /// </summary>
        [EnumStringValue("LU")]
        LU,

        /// <summary>
        /// Latvia
        /// </summary>
        [EnumStringValue("LV")]
        LV,

        /// <summary>
        /// Libya
        /// </summary>
        [EnumStringValue("LY")]
        LY,

        /// <summary>
        /// Morocco
        /// </summary>
        [EnumStringValue("MA")]
        MA,

        /// <summary>
        /// Monaco
        /// </summary>
        [EnumStringValue("MC")]
        MC,

        /// <summary>
        /// Moldova (the Republic of)
        /// </summary>
        [EnumStringValue("MD")]
        MD,

        /// <summary>
        /// Montenegro
        /// </summary>
        [EnumStringValue("ME")]
        ME,

        /// <summary>
        /// Saint Martin (French part)
        /// </summary>
        [EnumStringValue("MF")]
        MF,

        /// <summary>
        /// Madagascar
        /// </summary>
        [EnumStringValue("MG")]
        MG,

        /// <summary>
        /// Marshall Islands (the)
        /// </summary>
        [EnumStringValue("MH")]
        MH,

        /// <summary>
        /// North Macedonia
        /// </summary>
        [EnumStringValue("MK")]
        MK,

        /// <summary>
        /// Mali
        /// </summary>
        [EnumStringValue("ML")]
        ML,

        /// <summary>
        /// Myanmar
        /// </summary>
        [EnumStringValue("MM")]
        MM,

        /// <summary>
        /// Mongolia
        /// </summary>
        [EnumStringValue("MN")]
        MN,

        /// <summary>
        /// Macao
        /// </summary>
        [EnumStringValue("MO")]
        MO,

        /// <summary>
        /// Northern Mariana Islands (the)
        /// </summary>
        [EnumStringValue("MP")]
        MP,

        /// <summary>
        /// Martinique
        /// </summary>
        [EnumStringValue("MQ")]
        MQ,

        /// <summary>
        /// Mauritania
        /// </summary>
        [EnumStringValue("MR")]
        MR,

        /// <summary>
        /// Montserrat
        /// </summary>
        [EnumStringValue("MS")]
        MS,

        /// <summary>
        /// Malta
        /// </summary>
        [EnumStringValue("MT")]
        MT,

        /// <summary>
        /// Mauritius
        /// </summary>
        [EnumStringValue("MU")]
        MU,

        /// <summary>
        /// Maldives
        /// </summary>
        [EnumStringValue("MV")]
        MV,

        /// <summary>
        /// Malawi
        /// </summary>
        [EnumStringValue("MW")]
        MW,

        /// <summary>
        /// Mexico
        /// </summary>
        [EnumStringValue("MX")]
        MX,

        /// <summary>
        /// Malaysia
        /// </summary>
        [EnumStringValue("MY")]
        MY,

        /// <summary>
        /// Mozambique
        /// </summary>
        [EnumStringValue("MZ")]
        MZ,

        /// <summary>
        /// Namibia
        /// </summary>
        [EnumStringValue("NA")]
        NA,

        /// <summary>
        /// New Caledonia
        /// </summary>
        [EnumStringValue("NC")]
        NC,

        /// <summary>
        /// Niger (the)
        /// </summary>
        [EnumStringValue("NE")]
        NE,

        /// <summary>
        /// Norfolk Island
        /// </summary>
        [EnumStringValue("NF")]
        NF,

        /// <summary>
        /// Nigeria
        /// </summary>
        [EnumStringValue("NG")]
        NG,

        /// <summary>
        /// Nicaragua
        /// </summary>
        [EnumStringValue("NI")]
        NI,

        /// <summary>
        /// Netherlands (the)
        /// </summary>
        [EnumStringValue("NL")]
        NL,

        /// <summary>
        /// Norway
        /// </summary>
        [EnumStringValue("NO")]
        NO,

        /// <summary>
        /// Nepal
        /// </summary>
        [EnumStringValue("NP")]
        NP,

        /// <summary>
        /// Nauru
        /// </summary>
        [EnumStringValue("NR")]
        NR,

        /// <summary>
        /// Niue
        /// </summary>
        [EnumStringValue("NU")]
        NU,

        /// <summary>
        /// New Zealand
        /// </summary>
        [EnumStringValue("NZ")]
        NZ,

        /// <summary>
        /// Oman
        /// </summary>
        [EnumStringValue("OM")]
        OM,

        /// <summary>
        /// Panama
        /// </summary>
        [EnumStringValue("PA")]
        PA,

        /// <summary>
        /// Peru
        /// </summary>
        [EnumStringValue("PE")]
        PE,

        /// <summary>
        /// French Polynesia
        /// </summary>
        [EnumStringValue("PF")]
        PF,

        /// <summary>
        /// Papua New Guinea
        /// </summary>
        [EnumStringValue("PG")]
        PG,

        /// <summary>
        /// Philippines (the)
        /// </summary>
        [EnumStringValue("PH")]
        PH,

        /// <summary>
        /// Pakistan
        /// </summary>
        [EnumStringValue("PK")]
        PK,

        /// <summary>
        /// Poland
        /// </summary>
        [EnumStringValue("PL")]
        PL,

        /// <summary>
        /// Saint Pierre and Miquelon
        /// </summary>
        [EnumStringValue("PM")]
        PM,

        /// <summary>
        /// Pitcairn
        /// </summary>
        [EnumStringValue("PN")]
        PN,

        /// <summary>
        /// Puerto Rico
        /// </summary>
        [EnumStringValue("PR")]
        PR,

        /// <summary>
        /// Palestine, State of
        /// </summary>
        [EnumStringValue("PS")]
        PS,

        /// <summary>
        /// Portugal
        /// </summary>
        [EnumStringValue("PT")]
        PT,

        /// <summary>
        /// Palau
        /// </summary>
        [EnumStringValue("PW")]
        PW,

        /// <summary>
        /// Paraguay
        /// </summary>
        [EnumStringValue("PY")]
        PY,

        /// <summary>
        /// Qatar
        /// </summary>
        [EnumStringValue("QA")]
        QA,

        /// <summary>
        /// Réunion
        /// </summary>
        [EnumStringValue("RE")]
        RE,

        /// <summary>
        /// Romania
        /// </summary>
        [EnumStringValue("RO")]
        RO,

        /// <summary>
        /// Serbia
        /// </summary>
        [EnumStringValue("RS")]
        RS,

        /// <summary>
        /// Russia (the Federation)
        /// </summary>
        [EnumStringValue("RU")]
        RU,

        /// <summary>
        /// Rwanda
        /// </summary>
        [EnumStringValue("RW")]
        RW,

        /// <summary>
        /// Saudi Arabia
        /// </summary>
        [EnumStringValue("SA")]
        SA,

        /// <summary>
        /// Solomon Islands
        /// </summary>
        [EnumStringValue("SB")]
        SB,

        /// <summary>
        /// Seychelles
        /// </summary>
        [EnumStringValue("SC")]
        SC,

        /// <summary>
        /// Sudan (the)
        /// </summary>
        [EnumStringValue("SD")]
        SD,

        /// <summary>
        /// Sweden
        /// </summary>
        [EnumStringValue("SE")]
        SE,

        /// <summary>
        /// Singapore
        /// </summary>
        [EnumStringValue("SG")]
        SG,

        /// <summary>
        /// Saint Helena, Ascension and Tristan da Cunha
        /// </summary>
        [EnumStringValue("SH")]
        SH,

        /// <summary>
        /// Slovenia
        /// </summary>
        [EnumStringValue("SI")]
        SI,

        /// <summary>
        /// Svalbard and Jan Mayen
        /// </summary>
        [EnumStringValue("SJ")]
        SJ,

        /// <summary>
        /// Slovakia
        /// </summary>
        [EnumStringValue("SK")]
        SK,

        /// <summary>
        /// Sierra Leone
        /// </summary>
        [EnumStringValue("SL")]
        SL,

        /// <summary>
        /// San Marino
        /// </summary>
        [EnumStringValue("SM")]
        SM,

        /// <summary>
        /// Senegal
        /// </summary>
        [EnumStringValue("SN")]
        SN,

        /// <summary>
        /// Somalia
        /// </summary>
        [EnumStringValue("SO")]
        SO,

        /// <summary>
        /// Suriname
        /// </summary>
        [EnumStringValue("SR")]
        SR,

        /// <summary>
        /// South Sudan
        /// </summary>
        [EnumStringValue("SS")]
        SS,

        /// <summary>
        /// Sao Tome and Principe
        /// </summary>
        [EnumStringValue("ST")]
        ST,

        /// <summary>
        /// El Salvador
        /// </summary>
        [EnumStringValue("SV")]
        SV,

        /// <summary>
        /// Sint Maarten (Dutch part)
        /// </summary>
        [EnumStringValue("SX")]
        SX,

        /// <summary>
        /// Syrian Arab Republic
        /// </summary>
        [EnumStringValue("SY")]
        SY,

        /// <summary>
        /// Eswatini
        /// </summary>
        [EnumStringValue("SZ")]
        SZ,

        /// <summary>
        /// Turks and Caicos Islands (the)
        /// </summary>
        [EnumStringValue("TC")]
        TC,

        /// <summary>
        /// Chad
        /// </summary>
        [EnumStringValue("TD")]
        TD,

        /// <summary>
        /// French Southern Territories (the)
        /// </summary>
        [EnumStringValue("TF")]
        TF,

        /// <summary>
        /// Togo
        /// </summary>
        [EnumStringValue("TG")]
        TG,

        /// <summary>
        /// Thailand
        /// </summary>
        [EnumStringValue("TH")]
        TH,

        /// <summary>
        /// Tajikistan
        /// </summary>
        [EnumStringValue("TJ")]
        TJ,

        /// <summary>
        /// Tokelau
        /// </summary>
        [EnumStringValue("TK")]
        TK,

        /// <summary>
        /// Timor-Leste
        /// </summary>
        [EnumStringValue("TL")]
        TL,

        /// <summary>
        /// Turkmenistan
        /// </summary>
        [EnumStringValue("TM")]
        TM,

        /// <summary>
        /// Tunisia
        /// </summary>
        [EnumStringValue("TN")]
        TN,

        /// <summary>
        /// Tonga
        /// </summary>
        [EnumStringValue("TO")]
        TO,

        /// <summary>
        /// Turkey
        /// </summary>
        [EnumStringValue("TR")]
        TR,

        /// <summary>
        /// Trinidad and Tobago
        /// </summary>
        [EnumStringValue("TT")]
        TT,

        /// <summary>
        /// Tuvalu
        /// </summary>
        [EnumStringValue("TV")]
        TV,

        /// <summary>
        /// Taiwan (Province of China)
        /// </summary>
        [EnumStringValue("TW")]
        TW,

        /// <summary>
        /// Tanzania, United Republic of
        /// </summary>
        [EnumStringValue("TZ")]
        TZ,

        /// <summary>
        /// Ukraine
        /// </summary>
        [EnumStringValue("UA")]
        UA,

        /// <summary>
        /// Uganda
        /// </summary>
        [EnumStringValue("UG")]
        UG,

        /// <summary>
        /// United States Minor Outlying Islands (the)
        /// </summary>
        [EnumStringValue("UM")]
        UM,

        /// <summary>
        /// United States of America (the)
        /// </summary>
        [EnumStringValue("US")]
        US,

        /// <summary>
        /// Uruguay
        /// </summary>
        [EnumStringValue("UY")]
        UY,

        /// <summary>
        /// Uzbekistan
        /// </summary>
        [EnumStringValue("UZ")]
        UZ,

        /// <summary>
        /// Holy See (the)
        /// </summary>
        [EnumStringValue("VA")]
        VA,

        /// <summary>
        /// Saint Vincent and the Grenadines
        /// </summary>
        [EnumStringValue("VC")]
        VC,

        /// <summary>
        /// Venezuela (Bolivarian Republic of)
        /// </summary>
        [EnumStringValue("VE")]
        VE,

        /// <summary>
        /// Virgin Islands (British)
        /// </summary>
        [EnumStringValue("VG")]
        VG,

        /// <summary>
        /// Virgin Islands (U.S.)
        /// </summary>
        [EnumStringValue("VI")]
        VI,

        /// <summary>
        /// Vietnam
        /// </summary>
        [EnumStringValue("VN")]
        VN,

        /// <summary>
        /// Vanuatu
        /// </summary>
        [EnumStringValue("VU")]
        VU,

        /// <summary>
        /// Wallis and Futuna
        /// </summary>
        [EnumStringValue("WF")]
        WF,

        /// <summary>
        /// Samoa
        /// </summary>
        [EnumStringValue("WS")]
        WS,

        /// <summary>
        /// Yemen
        /// </summary>
        [EnumStringValue("YE")]
        YE,

        /// <summary>
        /// Mayotte
        /// </summary>
        [EnumStringValue("YT")]
        YT,

        /// <summary>
        /// South Africa
        /// </summary>
        [EnumStringValue("ZA")]
        ZA,

        /// <summary>
        /// Zambia
        /// </summary>
        [EnumStringValue("ZM")]
        ZM,

        /// <summary>
        /// Zimbabwe
        /// </summary>
        [EnumStringValue("ZW")]
        ZW,

        /// <summary>
        /// Kosovo
        /// </summary>
        [EnumStringValue("1A")]
        _1A,

        /// <summary>
        /// United Kingdom (Northern Ireland)
        /// </summary>
        [EnumStringValue("XI")]
        XI
    }
}
