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

    // You can regenerate the codes using:
    //
    // https://cemil.dev/countrieslist
    //
    // g = open('output.cs', 'w+')
    // 
    // with open('countries.json') as json_file:
    // data = json.load(json_file)
    // for p in data:
    //     alphaTwo = p['alpha-2']
    //     countryCode = p['country-code']
    //     name = p['name']
    //     g.write('/// <summary>\n')
    //     g.write('/// ' + name + '\n')
    //     g.write('/// ' + alphaTwo + ' = ' + countryCode + '\n')
    //     g.write('/// </summary>\n')
    //     g.write(alphaTwo + ' = ' + countryCode + ',\n')
    //     g.write('\n')
    // 
    // g.close()
    //
    // Kosovo needs manual addition and special treatment

    /// <summary>
    /// Country codes based on ISO 3166 source
    /// with addition of Kosovo according to ZUGFeRD standard
    /// 
    /// English short name
    /// Alpha-2 code = numeric code
    /// </summary>
    public enum CountryCodes
    {

        /// <summary>
        /// Andorra
        /// AD = 20
        /// </summary>
        AD = 20,

        /// <summary>
        /// United Arab Emirates (the)
        /// AE = 784
        /// </summary>
        AE = 784,

        /// <summary>
        /// Afghanistan
        /// AF = 4
        /// </summary>
        AF = 4,

        /// <summary>
        /// Antigua and Barbuda
        /// AG = 28
        /// </summary>
        AG = 28,

        /// <summary>
        /// Anguilla
        /// AI = 660
        /// </summary>
        AI = 660,

        /// <summary>
        /// Albania
        /// AL = 8
        /// </summary>
        AL = 8,

        /// <summary>
        /// Armenia
        /// AM = 51
        /// </summary>
        AM = 51,

        /// <summary>
        /// Angola
        /// AO = 24
        /// </summary>
        AO = 24,

        /// <summary>
        /// Antarctica
        /// AQ = 10
        /// </summary>
        AQ = 10,

        /// <summary>
        /// Argentina
        /// AR = 32
        /// </summary>
        AR = 32,

        /// <summary>
        /// American Samoa
        /// AS = 16
        /// </summary>
        AS = 16,

        /// <summary>
        /// Austria
        /// AT = 40
        /// </summary>
        AT = 40,

        /// <summary>
        /// Australia
        /// AU = 36
        /// </summary>
        AU = 36,

        /// <summary>
        /// Aruba
        /// AW = 533
        /// </summary>
        AW = 533,

        /// <summary>
        /// �land Islands
        /// AX = 248
        /// </summary>
        AX = 248,

        /// <summary>
        /// Azerbaijan
        /// AZ = 31
        /// </summary>
        AZ = 31,

        /// <summary>
        /// Bosnia and Herzegovina
        /// BA = 70
        /// </summary>
        BA = 70,

        /// <summary>
        /// Barbados
        /// BB = 52
        /// </summary>
        BB = 52,

        /// <summary>
        /// Bangladesh
        /// BD = 50
        /// </summary>
        BD = 50,

        /// <summary>
        /// Belgium
        /// BE = 56
        /// </summary>
        BE = 56,

        /// <summary>
        /// Burkina Faso
        /// BF = 854
        /// </summary>
        BF = 854,

        /// <summary>
        /// Bulgaria
        /// BG = 100
        /// </summary>
        BG = 100,

        /// <summary>
        /// Bahrain
        /// BH = 48
        /// </summary>
        BH = 48,

        /// <summary>
        /// Burundi
        /// BI = 108
        /// </summary>
        BI = 108,

        /// <summary>
        /// Benin
        /// BJ = 204
        /// </summary>
        BJ = 204,

        /// <summary>
        /// Saint Barthélem
        /// BL = 652
        /// </summary>
        BL = 652,

        /// <summary>
        /// Bermuda
        /// BM = 60
        /// </summary>
        BM = 60,

        /// <summary>
        /// Brunei Darussalam
        /// BN = 96
        /// </summary>
        BN = 96,

        /// <summary>
        /// Bolivia (Plurinational State of)
        /// BO = 68
        /// </summary>
        BO = 68,

        /// <summary>
        /// Bonaire, Sint Eustatius and Saba
        /// BQ = 535
        /// </summary>
        BQ = 535,

        /// <summary>
        /// Brazil
        /// BR = 76
        /// </summary>
        BR = 76,

        /// <summary>
        /// Bahamas (the)
        /// BS = 44
        /// </summary>
        BS = 44,

        /// <summary>
        /// Bhutan
        /// BT = 64
        /// </summary>
        BT = 64,

        /// <summary>
        /// Bouvet Island
        /// BV = 74
        /// </summary>
        BV = 74,

        /// <summary>
        /// Botswana
        /// BW = 72
        /// </summary>
        BW = 72,

        /// <summary>
        /// Belarus
        /// BY = 112
        /// </summary>
        BY = 112,

        /// <summary>
        /// Belize
        /// BZ = 84
        /// </summary>
        BZ = 84,

        /// <summary>
        /// Canada
        /// CA = 124
        /// </summary>
        CA = 124,

        /// <summary>
        /// Cocos (Keeling) Islands (the)
        /// CC = 166
        /// </summary>
        CC = 166,

        /// <summary>
        /// Congo (the Democratic Republic of the)
        /// CD = 180
        /// </summary>
        CD = 180,

        /// <summary>
        /// Central African Republic (the)
        /// CF = 140
        /// </summary>
        CF = 140,

        /// <summary>
        /// Congo (the)
        /// CG = 178
        /// </summary>
        CG = 178,

        /// <summary>
        /// Switzerland
        /// CH = 756
        /// </summary>
        CH = 756,

        /// <summary>
        /// Côte d'Ivoir
        /// CI = 384
        /// </summary>
        CI = 384,

        /// <summary>
        /// Cook Islands (the)
        /// CK = 184
        /// </summary>
        CK = 184,

        /// <summary>
        /// Chile
        /// CL = 152
        /// </summary>
        CL = 152,

        /// <summary>
        /// Cameroon
        /// CM = 120
        /// </summary>
        CM = 120,

        /// <summary>
        /// China
        /// CN = 156
        /// </summary>
        CN = 156,

        /// <summary>
        /// Colombia
        /// CO = 170
        /// </summary>
        CO = 170,

        /// <summary>
        /// Costa Rica
        /// CR = 188
        /// </summary>
        CR = 188,

        /// <summary>
        /// Cuba
        /// CU = 192
        /// </summary>
        CU = 192,

        /// <summary>
        /// Cabo Verde
        /// CV = 132
        /// </summary>
        CV = 132,

        /// <summary>
        /// Curaça
        /// CW = 531
        /// </summary>
        CW = 531,

        /// <summary>
        /// Christmas Island
        /// CX = 162
        /// </summary>
        CX = 162,

        /// <summary>
        /// Cyprus
        /// CY = 196
        /// </summary>
        CY = 196,

        /// <summary>
        /// Czechia
        /// CZ = 203
        /// </summary>
        CZ = 203,

        /// <summary>
        /// Germany
        /// DE = 276
        /// </summary>
        DE = 276,

        /// <summary>
        /// Djibouti
        /// DJ = 262
        /// </summary>
        DJ = 262,

        /// <summary>
        /// Denmark
        /// DK = 208
        /// </summary>
        DK = 208,

        /// <summary>
        /// Dominica
        /// DM = 212
        /// </summary>
        DM = 212,

        /// <summary>
        /// Dominican Republic (the)
        /// DO = 214
        /// </summary>
        DO = 214,

        /// <summary>
        /// Algeria
        /// DZ = 12
        /// </summary>
        DZ = 12,

        /// <summary>
        /// Ecuador
        /// EC = 218
        /// </summary>
        EC = 218,

        /// <summary>
        /// Estonia
        /// EE = 233
        /// </summary>
        EE = 233,

        /// <summary>
        /// Egypt
        /// EG = 818
        /// </summary>
        EG = 818,

        /// <summary>
        /// Western Sahara*
        /// EH = 732
        /// </summary>
        EH = 732,

        /// <summary>
        /// Eritrea
        /// ER = 232
        /// </summary>
        ER = 232,

        /// <summary>
        /// Spain
        /// ES = 724
        /// </summary>
        ES = 724,

        /// <summary>
        /// Ethiopia
        /// ET = 231
        /// </summary>
        ET = 231,

        /// <summary>
        /// Finland
        /// FI = 246
        /// </summary>
        FI = 246,

        /// <summary>
        /// Fiji
        /// FJ = 242
        /// </summary>
        FJ = 242,

        /// <summary>
        /// Falkland Islands (the) [Malvinas]
        /// FK = 238
        /// </summary>
        FK = 238,

        /// <summary>
        /// Micronesia (Federated States of)
        /// FM = 583
        /// </summary>
        FM = 583,

        /// <summary>
        /// Faroe Islands (the)
        /// FO = 234
        /// </summary>
        FO = 234,

        /// <summary>
        /// France
        /// FR = 250
        /// </summary>
        FR = 250,

        /// <summary>
        /// Gabon
        /// GA = 266
        /// </summary>
        GA = 266,

        /// <summary>
        /// United Kingdom of Great Britain and Northern Ireland (the)
        /// GB = 826
        /// </summary>
        GB = 826,

        /// <summary>
        /// Grenada
        /// GD = 308
        /// </summary>
        GD = 308,

        /// <summary>
        /// Georgia
        /// GE = 268
        /// </summary>
        GE = 268,

        /// <summary>
        /// Guernsey
        /// GG = 831
        /// </summary>
        GF = 254,

        /// <summary>
        /// Guernsey
        /// GG = 831
        /// </summary>
        GG = 831,

        /// <summary>
        /// Ghana
        /// GH = 288
        /// </summary>
        GH = 288,

        /// <summary>
        /// Gibraltar
        /// GI = 292
        /// </summary>
        GI = 292,

        /// <summary>
        /// Greenland
        /// GL = 304
        /// </summary>
        GL = 304,

        /// <summary>
        /// Gambia (the)
        /// GM = 270
        /// </summary>
        GM = 270,

        /// <summary>
        /// Guinea
        /// GN = 324
        /// </summary>
        GN = 324,

        /// <summary>
        /// Guadeloupe
        /// GP = 312
        /// </summary>
        GP = 312,

        /// <summary>
        /// Equatorial Guinea
        /// GQ = 226
        /// </summary>
        GQ = 226,

        /// <summary>
        /// Greece
        /// GR = 300
        /// </summary>
        GR = 300,

        /// <summary>
        /// South Georgia and the South Sandwich Islands
        /// GS = 239
        /// </summary>
        GS = 239,

        /// <summary>
        /// Guatemala
        /// GT = 320
        /// </summary>
        GT = 320,

        /// <summary>
        /// Guam
        /// GU = 316
        /// </summary>
        GU = 316,

        /// <summary>
        /// Guinea-Bissau
        /// GW = 624
        /// </summary>
        GW = 624,

        /// <summary>
        /// Guyana
        /// GY = 328
        /// </summary>
        GY = 328,

        /// <summary>
        /// Hong Kong
        /// HK = 344
        /// </summary>
        HK = 344,

        /// <summary>
        /// Heard Island and McDonald Islands
        /// HM = 334
        /// </summary>
        HM = 334,

        /// <summary>
        /// Honduras
        /// HN = 340
        /// </summary>
        HN = 340,

        /// <summary>
        /// Croatia
        /// HR = 191
        /// </summary>
        HR = 191,

        /// <summary>
        /// Haiti
        /// HT = 332
        /// </summary>
        HT = 332,

        /// <summary>
        /// Hungary
        /// HU = 348
        /// </summary>
        HU = 348,

        /// <summary>
        /// Indonesia
        /// ID = 360
        /// </summary>
        ID = 360,

        /// <summary>
        /// Ireland
        /// IE = 372
        /// </summary>
        IE = 372,

        /// <summary>
        /// Israel
        /// IL = 376
        /// </summary>
        IL = 376,

        /// <summary>
        /// Isle of Man
        /// IM = 833
        /// </summary>
        IM = 833,

        /// <summary>
        /// India
        /// IN = 356
        /// </summary>
        IN = 356,

        /// <summary>
        /// British Indian Ocean Territory (the)
        /// IO = 86
        /// </summary>
        IO = 86,

        /// <summary>
        /// Iraq
        /// IQ = 368
        /// </summary>
        IQ = 368,

        /// <summary>
        /// Iran (Islamic Republic of)
        /// IR = 364
        /// </summary>
        IR = 364,

        /// <summary>
        /// Iceland
        /// IS = 352
        /// </summary>
        IS = 352,

        /// <summary>
        /// Italy
        /// IT = 380
        /// </summary>
        IT = 380,

        /// <summary>
        /// Jersey
        /// JE = 832
        /// </summary>
        JE = 832,

        /// <summary>
        /// Jamaica
        /// JM = 388
        /// </summary>
        JM = 388,

        /// <summary>
        /// Jordan
        /// JO = 400
        /// </summary>
        JO = 400,

        /// <summary>
        /// Japan
        /// JP = 392
        /// </summary>
        JP = 392,

        /// <summary>
        /// Kenya
        /// KE = 404
        /// </summary>
        KE = 404,

        /// <summary>
        /// Kyrgyzstan
        /// KG = 417
        /// </summary>
        KG = 417,

        /// <summary>
        /// Cambodia
        /// KH = 116
        /// </summary>
        KH = 116,

        /// <summary>
        /// Kiribati
        /// KI = 296
        /// </summary>
        KI = 296,

        /// <summary>
        /// Comoros (the)
        /// KM = 174
        /// </summary>
        KM = 174,

        /// <summary>
        /// Saint Kitts and Nevis
        /// KN = 659
        /// </summary>
        KN = 659,

        /// <summary>
        /// Korea (the Democratic People's Republic of)
        /// KP = 408
        /// </summary>
        KP = 408,

        /// <summary>
        /// Korea (the Republic of)
        /// KR = 410
        /// </summary>
        KR = 410,

        /// <summary>
        /// Kuwait
        /// KW = 414
        /// </summary>
        KW = 414,

        /// <summary>
        /// Cayman Islands (the)
        /// KY = 136
        /// </summary>
        KY = 136,

        /// <summary>
        /// Kazakhstan
        /// KZ = 398
        /// </summary>
        KZ = 398,

        /// <summary>
        /// Lao People's Democratic Republic (the)
        /// LA = 418
        /// </summary>
        LA = 418,

        /// <summary>
        /// Lebanon
        /// LB = 422
        /// </summary>
        LB = 422,

        /// <summary>
        /// Saint Lucia
        /// LC = 662
        /// </summary>
        LC = 662,

        /// <summary>
        /// Liechtenstein
        /// LI = 438
        /// </summary>
        LI = 438,

        /// <summary>
        /// Sri Lanka
        /// LK = 144
        /// </summary>
        LK = 144,

        /// <summary>
        /// Liberia
        /// LR = 430
        /// </summary>
        LR = 430,

        /// <summary>
        /// Lesotho
        /// LS = 426
        /// </summary>
        LS = 426,

        /// <summary>
        /// Lithuania
        /// LT = 440
        /// </summary>
        LT = 440,

        /// <summary>
        /// Luxembourg
        /// LU = 442
        /// </summary>
        LU = 442,

        /// <summary>
        /// Latvia
        /// LV = 428
        /// </summary>
        LV = 428,

        /// <summary>
        /// Libya
        /// LY = 434
        /// </summary>
        LY = 434,

        /// <summary>
        /// Morocco
        /// MA = 504
        /// </summary>
        MA = 504,

        /// <summary>
        /// Monaco
        /// MC = 492
        /// </summary>
        MC = 492,

        /// <summary>
        /// Moldova (the Republic of)
        /// MD = 498
        /// </summary>
        MD = 498,

        /// <summary>
        /// Montenegro
        /// ME = 499
        /// </summary>
        ME = 499,

        /// <summary>
        /// Saint Martin (French part)
        /// MF = 663
        /// </summary>
        MF = 663,

        /// <summary>
        /// Madagascar
        /// MG = 450
        /// </summary>
        MG = 450,

        /// <summary>
        /// Marshall Islands (the)
        /// MH = 584
        /// </summary>
        MH = 584,

        /// <summary>
        /// North Macedonia
        /// MK = 807
        /// </summary>
        MK = 807,

        /// <summary>
        /// Mali
        /// ML = 466
        /// </summary>
        ML = 466,

        /// <summary>
        /// Myanmar
        /// MM = 104
        /// </summary>
        MM = 104,

        /// <summary>
        /// Mongolia
        /// MN = 496
        /// </summary>
        MN = 496,

        /// <summary>
        /// Macao
        /// MO = 446
        /// </summary>
        MO = 446,

        /// <summary>
        /// Northern Mariana Islands (the)
        /// MP = 580
        /// </summary>
        MP = 580,

        /// <summary>
        /// Martinique
        /// MQ = 474
        /// </summary>
        MQ = 474,

        /// <summary>
        /// Mauritania
        /// MR = 478
        /// </summary>
        MR = 478,

        /// <summary>
        /// Montserrat
        /// MS = 500
        /// </summary>
        MS = 500,

        /// <summary>
        /// Malta
        /// MT = 470
        /// </summary>
        MT = 470,

        /// <summary>
        /// Mauritius
        /// MU = 480
        /// </summary>
        MU = 480,

        /// <summary>
        /// Maldives
        /// MV = 462
        /// </summary>
        MV = 462,

        /// <summary>
        /// Malawi
        /// MW = 454
        /// </summary>
        MW = 454,

        /// <summary>
        /// Malaysia
        /// MY = 458
        /// </summary>
        MX = 484,

        /// <summary>
        /// Malaysia
        /// MY = 458
        /// </summary>
        MY = 458,

        /// <summary>
        /// Mozambique
        /// MZ = 508
        /// </summary>
        MZ = 508,

        /// <summary>
        /// Namibia
        /// NA = 516
        /// </summary>
        NA = 516,

        /// <summary>
        /// New Caledonia
        /// NC = 540
        /// </summary>
        NC = 540,

        /// <summary>
        /// Niger (the)
        /// NE = 562
        /// </summary>
        NE = 562,

        /// <summary>
        /// Norfolk Island
        /// NF = 574
        /// </summary>
        NF = 574,

        /// <summary>
        /// Nigeria
        /// NG = 566
        /// </summary>
        NG = 566,

        /// <summary>
        /// Nicaragua
        /// NI = 558
        /// </summary>
        NI = 558,

        /// <summary>
        /// Netherlands (the)
        /// NL = 528
        /// </summary>
        NL = 528,

        /// <summary>
        /// Norway
        /// NO = 578
        /// </summary>
        NO = 578,

        /// <summary>
        /// Nepal
        /// NP = 524
        /// </summary>
        NP = 524,

        /// <summary>
        /// Nauru
        /// NR = 520
        /// </summary>
        NR = 520,

        /// <summary>
        /// Niue
        /// NU = 570
        /// </summary>
        NU = 570,

        /// <summary>
        /// New Zealand
        /// NZ = 554
        /// </summary>
        NZ = 554,

        /// <summary>
        /// Oman
        /// OM = 512
        /// </summary>
        OM = 512,

        /// <summary>
        /// Panama
        /// PA = 591
        /// </summary>
        PA = 591,

        /// <summary>
        /// Peru
        /// PE = 604
        /// </summary>
        PE = 604,

        /// <summary>
        /// French Polynesia
        /// PF = 258
        /// </summary>
        PF = 258,

        /// <summary>
        /// Papua New Guinea
        /// PG = 598
        /// </summary>
        PG = 598,

        /// <summary>
        /// Philippines (the)
        /// PH = 608
        /// </summary>
        PH = 608,

        /// <summary>
        /// Pakistan
        /// PK = 586
        /// </summary>
        PK = 586,

        /// <summary>
        /// Poland
        /// PL = 616
        /// </summary>
        PL = 616,

        /// <summary>
        /// Saint Pierre and Miquelon
        /// PM = 666
        /// </summary>
        PM = 666,

        /// <summary>
        /// Pitcairn
        /// PN = 612
        /// </summary>
        PN = 612,

        /// <summary>
        /// Puerto Rico
        /// PR = 630
        /// </summary>
        PR = 630,

        /// <summary>
        /// Palestine, State of
        /// PS = 275
        /// </summary>
        PS = 275,

        /// <summary>
        /// Portugal
        /// PT = 620
        /// </summary>
        PT = 620,

        /// <summary>
        /// Palau
        /// PW = 585
        /// </summary>
        PW = 585,

        /// <summary>
        /// Paraguay
        /// PY = 600
        /// </summary>
        PY = 600,

        /// <summary>
        /// Qatar
        /// QA = 634
        /// </summary>
        QA = 634,

        /// <summary>
        /// Réunio
        /// RE = 638
        /// </summary>
        RE = 638,

        /// <summary>
        /// Romania
        /// RO = 642
        /// </summary>
        RO = 642,

        /// <summary>
        /// Serbia
        /// RS = 688
        /// </summary>
        RS = 688,

        /// <summary>
        /// Russian Federation (the)
        /// RU = 643
        /// </summary>
        RU = 643,

        /// <summary>
        /// Rwanda
        /// RW = 646
        /// </summary>
        RW = 646,

        /// <summary>
        /// Saudi Arabia
        /// SA = 682
        /// </summary>
        SA = 682,

        /// <summary>
        /// Solomon Islands
        /// SB = 90
        /// </summary>
        SB = 90,

        /// <summary>
        /// Seychelles
        /// SC = 690
        /// </summary>
        SC = 690,

        /// <summary>
        /// Sudan (the)
        /// SD = 729
        /// </summary>
        SD = 729,

        /// <summary>
        /// Sweden
        /// SE = 752
        /// </summary>
        SE = 752,

        /// <summary>
        /// Singapore
        /// SG = 702
        /// </summary>
        SG = 702,

        /// <summary>
        /// Saint Helena, Ascension and Tristan da Cunha
        /// SH = 654
        /// </summary>
        SH = 654,

        /// <summary>
        /// Slovenia
        /// SI = 705
        /// </summary>
        SI = 705,

        /// <summary>
        /// Svalbard and Jan Mayen
        /// SJ = 744
        /// </summary>
        SJ = 744,

        /// <summary>
        /// Slovakia
        /// SK = 703
        /// </summary>
        SK = 703,

        /// <summary>
        /// Sierra Leone
        /// SL = 694
        /// </summary>
        SL = 694,

        /// <summary>
        /// San Marino
        /// SM = 674
        /// </summary>
        SM = 674,

        /// <summary>
        /// Senegal
        /// SN = 686
        /// </summary>
        SN = 686,

        /// <summary>
        /// Somalia
        /// SO = 706
        /// </summary>
        SO = 706,

        /// <summary>
        /// Suriname
        /// SR = 740
        /// </summary>
        SR = 740,

        /// <summary>
        /// South Sudan
        /// SS = 728
        /// </summary>
        SS = 728,

        /// <summary>
        /// Sao Tome and Principe
        /// ST = 678
        /// </summary>
        ST = 678,

        /// <summary>
        /// El Salvador
        /// SV = 222
        /// </summary>
        SV = 222,

        /// <summary>
        /// Sint Maarten (Dutch part)
        /// SX = 534
        /// </summary>
        SX = 534,

        /// <summary>
        /// Syrian Arab Republic (the)
        /// SY = 760
        /// </summary>
        SY = 760,

        /// <summary>
        /// Eswatini
        /// SZ = 748
        /// </summary>
        SZ = 748,

        /// <summary>
        /// Turks and Caicos Islands (the)
        /// TC = 796
        /// </summary>
        TC = 796,

        /// <summary>
        /// Chad
        /// TD = 148
        /// </summary>
        TD = 148,

        /// <summary>
        /// French Southern Territories (the)
        /// TF = 260
        /// </summary>
        TF = 260,

        /// <summary>
        /// Togo
        /// TG = 768
        /// </summary>
        TG = 768,

        /// <summary>
        /// Thailand
        /// TH = 764
        /// </summary>
        TH = 764,

        /// <summary>
        /// Tajikistan
        /// TJ = 762
        /// </summary>
        TJ = 762,

        /// <summary>
        /// Tokelau
        /// TK = 772
        /// </summary>
        TK = 772,

        /// <summary>
        /// Timor-Leste
        /// TL = 626
        /// </summary>
        TL = 626,

        /// <summary>
        /// Turkmenistan
        /// TM = 795
        /// </summary>
        TM = 795,

        /// <summary>
        /// Tunisia
        /// TN = 788
        /// </summary>
        TN = 788,

        /// <summary>
        /// Tonga
        /// TO = 776
        /// </summary>
        TO = 776,

        /// <summary>
        /// Turkey
        /// TR = 792
        /// </summary>
        TR = 792,

        /// <summary>
        /// Trinidad and Tobago
        /// TT = 780
        /// </summary>
        TT = 780,

        /// <summary>
        /// Tuvalu
        /// TV = 798
        /// </summary>
        TV = 798,

        /// <summary>
        /// Taiwan (Province of China)
        /// TW = 158
        /// </summary>
        TW = 158,

        /// <summary>
        /// Tanzania, the United Republic of
        /// TZ = 834
        /// </summary>
        TZ = 834,

        /// <summary>
        /// Ukraine
        /// UA = 804
        /// </summary>
        UA = 804,

        /// <summary>
        /// Uganda
        /// UG = 800
        /// </summary>
        UG = 800,

        /// <summary>
        /// United States Minor Outlying Islands (the)
        /// UM = 581
        /// </summary>
        UM = 581,

        /// <summary>
        /// United States of America (the)
        /// US = 840
        /// </summary>
        US = 840,

        /// <summary>
        /// Uruguay
        /// UY = 858
        /// </summary>
        UY = 858,

        /// <summary>
        /// Uzbekistan
        /// UZ = 860
        /// </summary>
        UZ = 860,

        /// <summary>
        /// Holy See (the)
        /// VA = 336
        /// </summary>
        VA = 336,

        /// <summary>
        /// Saint Vincent and the Grenadines
        /// VC = 670
        /// </summary>
        VC = 670,

        /// <summary>
        /// Venezuela (Bolivarian Republic of)
        /// VE = 862
        /// </summary>
        VE = 862,

        /// <summary>
        /// Virgin Islands (British)
        /// VG = 92
        /// </summary>
        VG = 92,

        /// <summary>
        /// Virgin Islands (U.S.)
        /// VI = 850
        /// </summary>
        VI = 850,

        /// <summary>
        /// Viet Nam
        /// VN = 704
        /// </summary>
        VN = 704,

        /// <summary>
        /// Vanuatu
        /// VU = 548
        /// </summary>
        VU = 548,

        /// <summary>
        /// Wallis and Futuna
        /// WF = 876
        /// </summary>
        WF = 876,

        /// <summary>
        /// Samoa
        /// WS = 882
        /// </summary>
        WS = 882,

        /// <summary>
        /// Yemen
        /// YE = 887
        /// </summary>
        YE = 887,

        /// <summary>
        /// Mayotte
        /// YT = 175
        /// </summary>
        YT = 175,

        /// <summary>
        /// South Africa
        /// ZA = 710
        /// </summary>
        ZA = 710,

        /// <summary>
        /// Zambia
        /// ZM = 894
        /// </summary>
        ZM = 894,

        /// <summary>
        /// Zimbabwe
        /// ZW = 716
        /// </summary>
        ZW = 716,

        /// <summary>
        /// Kosovo
        /// 1A = 999
        /// This is a temporary code,
        /// special treatment required as enums
        /// don't like members starting with a number!
        /// </summary>
        _1A = 999,

        /// <summary>
        /// Fall back for unsupported
        /// Country Codes
        /// Unknown = 0
        /// </summary>
        Unknown = 0


    }


    public static class CountryCodesExtensions
    {
        public static CountryCodes FromString(this CountryCodes _, string s)
        {
            // Special treatment for temporary code of Kosovo
            if ("1A" == s)
            {
                return CountryCodes._1A;
            }

            try
            {
                return (CountryCodes)Enum.Parse(typeof(CountryCodes), s);
            }
            catch
            {
                return CountryCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this CountryCodes c)
        {
            // Special treatment for temporary code of Kosovo
            return (CountryCodes._1A == c ? "1A" : c.ToString("g"));
        } // !ToString()
    }
}
