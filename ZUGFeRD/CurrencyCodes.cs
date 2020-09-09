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
    // Source: https://raw.githubusercontent.com/datasets/currency-codes/master/data/codes-all.csv
    // 
    // You can regenerate the codes using:
    // 
    // import csv
    // 
    // currencies = {}
    // 
    // with open('currencies.csv', newline= '') as f:
    // lines = csv.reader(f, delimiter = ',', quotechar = '"')
    // for line in lines:        
    //     if len(line) < 4:
    //         continue
    // 
    //     country = line[0]
    //     currency = line[1]
    //     alphaThree = line[2]
    //     code = line[3]
    // 
    //     if len(code) == 0:
    //         continue
    // 
    //     # first line
    //     if country.find('Entity') > -1:
    //         continue
    // 
    //     if currency.find('Zimbabwe Dollar (old)') > -1:
    //         continue
    // 
    //     if code in currencies:
    //         continue
    // 
    //     currencies[code] = { 'country' : country, 'currency' : currency, 'alphaThree' : alphaThree, 'code' : code }
    // 
    // g = open('currencies.cs', 'w+')
    // 
    // for currency in currencies.values():    
    //     g.write('/// <summary>\n')
    //     g.write('/// Country: ' + currency['country'] + '\n')
    //     g.write('/// Currency: ' + currency['currency'] + '\n')
    //     g.write('/// </summary>\n')
    //     g.write(currency['alphaThree'] + ' = ' + currency['code'] + ',\n')
    //     g.write('\n')
    // 
    // g.close()


    /// <summary>
    /// Full usage of http://csharpmoney.codeplex.com/ not required here,
    /// mapping of ISO codes sufficient.
    /// 
    /// ISO 4217 currency codes
    /// 
    /// Source: https://raw.githubusercontent.com/datasets/currency-codes/master/data/codes-all.csv
    /// </summary>
    public enum CurrencyCodes
    {
        /// <summary>
        /// Country: AFGHANISTAN
        /// Currency: Afghani
        /// </summary>
        AFN = 971,

        /// <summary>
        /// Country: ÅLAND ISLANDS
        /// Currency: Euro
        /// </summary>
        EUR = 978,

        /// <summary>
        /// Country: ALBANIA
        /// Currency: Lek
        /// </summary>
        ALL = 008,

        /// <summary>
        /// Country: ALGERIA
        /// Currency: Algerian Dinar
        /// </summary>
        DZD = 012,

        /// <summary>
        /// Country: AMERICAN SAMOA
        /// Currency: US Dollar
        /// </summary>
        USD = 840,

        /// <summary>
        /// Country: ANGOLA
        /// Currency: Kwanza
        /// </summary>
        AOA = 973,

        /// <summary>
        /// Country: ANGUILLA
        /// Currency: East Caribbean Dollar
        /// </summary>
        XCD = 951,

        /// <summary>
        /// Country: ARGENTINA
        /// Currency: Argentine Peso
        /// </summary>
        ARS = 032,

        /// <summary>
        /// Country: ARMENIA
        /// Currency: Armenian Dram
        /// </summary>
        AMD = 051,

        /// <summary>
        /// Country: ARUBA
        /// Currency: Aruban Florin
        /// </summary>
        AWG = 533,

        /// <summary>
        /// Country: AUSTRALIA
        /// Currency: Australian Dollar
        /// </summary>
        AUD = 036,

        /// <summary>
        /// Country: AZERBAIJAN
        /// Currency: Azerbaijan Manat
        /// </summary>
        AZN = 944,

        /// <summary>
        /// Country: BAHAMAS (THE)
        /// Currency: Bahamian Dollar
        /// </summary>
        BSD = 044,

        /// <summary>
        /// Country: BAHRAIN
        /// Currency: Bahraini Dinar
        /// </summary>
        BHD = 048,

        /// <summary>
        /// Country: BANGLADESH
        /// Currency: Taka
        /// </summary>
        BDT = 050,

        /// <summary>
        /// Country: BARBADOS
        /// Currency: Barbados Dollar
        /// </summary>
        BBD = 052,

        /// <summary>
        /// Country: BELARUS
        /// Currency: Belarusian Ruble
        /// </summary>
        BYN = 933,

        /// <summary>
        /// Country: BELIZE
        /// Currency: Belize Dollar
        /// </summary>
        BZD = 084,

        /// <summary>
        /// Country: BENIN
        /// Currency: CFA Franc BCEAO
        /// </summary>
        XOF = 952,

        /// <summary>
        /// Country: BERMUDA
        /// Currency: Bermudian Dollar
        /// </summary>
        BMD = 060,

        /// <summary>
        /// Country: BHUTAN
        /// Currency: Indian Rupee
        /// </summary>
        INR = 356,

        /// <summary>
        /// Country: BHUTAN
        /// Currency: Ngultrum
        /// </summary>
        BTN = 064,

        /// <summary>
        /// Country: BOLIVIA (PLURINATIONAL STATE OF)
        /// Currency: Boliviano
        /// </summary>
        BOB = 068,

        /// <summary>
        /// Country: BOLIVIA (PLURINATIONAL STATE OF)
        /// Currency: Mvdol
        /// </summary>
        BOV = 984,

        /// <summary>
        /// Country: BOSNIA AND HERZEGOVINA
        /// Currency: Convertible Mark
        /// </summary>
        BAM = 977,

        /// <summary>
        /// Country: BOTSWANA
        /// Currency: Pula
        /// </summary>
        BWP = 072,

        /// <summary>
        /// Country: BOUVET ISLAND
        /// Currency: Norwegian Krone
        /// </summary>
        NOK = 578,

        /// <summary>
        /// Country: BRAZIL
        /// Currency: Brazilian Real
        /// </summary>
        BRL = 986,

        /// <summary>
        /// Country: BRUNEI DARUSSALAM
        /// Currency: Brunei Dollar
        /// </summary>
        BND = 096,

        /// <summary>
        /// Country: BULGARIA
        /// Currency: Bulgarian Lev
        /// </summary>
        BGN = 975,

        /// <summary>
        /// Country: BURUNDI
        /// Currency: Burundi Franc
        /// </summary>
        BIF = 108,

        /// <summary>
        /// Country: CABO VERDE
        /// Currency: Cabo Verde Escudo
        /// </summary>
        CVE = 132,

        /// <summary>
        /// Country: CAMBODIA
        /// Currency: Riel
        /// </summary>
        KHR = 116,

        /// <summary>
        /// Country: CAMEROON
        /// Currency: CFA Franc BEAC
        /// </summary>
        XAF = 950,

        /// <summary>
        /// Country: CANADA
        /// Currency: Canadian Dollar
        /// </summary>
        CAD = 124,

        /// <summary>
        /// Country: CAYMAN ISLANDS (THE)
        /// Currency: Cayman Islands Dollar
        /// </summary>
        KYD = 136,

        /// <summary>
        /// Country: CHILE
        /// Currency: Chilean Peso
        /// </summary>
        CLP = 152,

        /// <summary>
        /// Country: CHILE
        /// Currency: Unidad de Fomento
        /// </summary>
        CLF = 990,

        /// <summary>
        /// Country: CHINA
        /// Currency: Yuan Renminbi
        /// </summary>
        CNY = 156,

        /// <summary>
        /// Country: COLOMBIA
        /// Currency: Colombian Peso
        /// </summary>
        COP = 170,

        /// <summary>
        /// Country: COLOMBIA
        /// Currency: Unidad de Valor Real
        /// </summary>
        COU = 970,

        /// <summary>
        /// Country: COMOROS (THE)
        /// Currency: Comorian Franc 
        /// </summary>
        KMF = 174,

        /// <summary>
        /// Country: CONGO (THE DEMOCRATIC REPUBLIC OF THE)
        /// Currency: Congolese Franc
        /// </summary>
        CDF = 976,

        /// <summary>
        /// Country: COOK ISLANDS (THE)
        /// Currency: New Zealand Dollar
        /// </summary>
        NZD = 554,

        /// <summary>
        /// Country: COSTA RICA
        /// Currency: Costa Rican Colon
        /// </summary>
        CRC = 188,

        /// <summary>
        /// Country: CROATIA
        /// Currency: Kuna
        /// </summary>
        HRK = 191,

        /// <summary>
        /// Country: CUBA
        /// Currency: Cuban Peso
        /// </summary>
        CUP = 192,

        /// <summary>
        /// Country: CUBA
        /// Currency: Peso Convertible
        /// </summary>
        CUC = 931,

        /// <summary>
        /// Country: CURAÇAO
        /// Currency: Netherlands Antillean Guilder
        /// </summary>
        ANG = 532,

        /// <summary>
        /// Country: CZECHIA
        /// Currency: Czech Koruna
        /// </summary>
        CZK = 203,

        /// <summary>
        /// Country: DENMARK
        /// Currency: Danish Krone
        /// </summary>
        DKK = 208,

        /// <summary>
        /// Country: DJIBOUTI
        /// Currency: Djibouti Franc
        /// </summary>
        DJF = 262,

        /// <summary>
        /// Country: DOMINICAN REPUBLIC (THE)
        /// Currency: Dominican Peso
        /// </summary>
        DOP = 214,

        /// <summary>
        /// Country: EGYPT
        /// Currency: Egyptian Pound
        /// </summary>
        EGP = 818,

        /// <summary>
        /// Country: EL SALVADOR
        /// Currency: El Salvador Colon
        /// </summary>
        SVC = 222,

        /// <summary>
        /// Country: ERITREA
        /// Currency: Nakfa
        /// </summary>
        ERN = 232,

        /// <summary>
        /// Country: ETHIOPIA
        /// Currency: Ethiopian Birr
        /// </summary>
        ETB = 230,

        /// <summary>
        /// Country: FALKLAND ISLANDS (THE) [MALVINAS]
        /// Currency: Falkland Islands Pound
        /// </summary>
        FKP = 238,

        /// <summary>
        /// Country: FIJI
        /// Currency: Fiji Dollar
        /// </summary>
        FJD = 242,

        /// <summary>
        /// Country: FRENCH POLYNESIA
        /// Currency: CFP Franc
        /// </summary>
        XPF = 953,

        /// <summary>
        /// Country: GAMBIA (THE)
        /// Currency: Dalasi
        /// </summary>
        GMD = 270,

        /// <summary>
        /// Country: GEORGIA
        /// Currency: Lari
        /// </summary>
        GEL = 981,

        /// <summary>
        /// Country: GHANA
        /// Currency: Ghana Cedi
        /// </summary>
        GHS = 936,

        /// <summary>
        /// Country: GIBRALTAR
        /// Currency: Gibraltar Pound
        /// </summary>
        GIP = 292,

        /// <summary>
        /// Country: GUATEMALA
        /// Currency: Quetzal
        /// </summary>
        GTQ = 320,

        /// <summary>
        /// Country: GUERNSEY
        /// Currency: Pound Sterling
        /// </summary>
        GBP = 826,

        /// <summary>
        /// Country: GUINEA
        /// Currency: Guinean Franc
        /// </summary>
        GNF = 324,

        /// <summary>
        /// Country: GUYANA
        /// Currency: Guyana Dollar
        /// </summary>
        GYD = 328,

        /// <summary>
        /// Country: HAITI
        /// Currency: Gourde
        /// </summary>
        HTG = 332,

        /// <summary>
        /// Country: HONDURAS
        /// Currency: Lempira
        /// </summary>
        HNL = 340,

        /// <summary>
        /// Country: HONG KONG
        /// Currency: Hong Kong Dollar
        /// </summary>
        HKD = 344,

        /// <summary>
        /// Country: HUNGARY
        /// Currency: Forint
        /// </summary>
        HUF = 348,

        /// <summary>
        /// Country: ICELAND
        /// Currency: Iceland Krona
        /// </summary>
        ISK = 352,

        /// <summary>
        /// Country: INDONESIA
        /// Currency: Rupiah
        /// </summary>
        IDR = 360,

        /// <summary>
        /// Country: INTERNATIONAL MONETARY FUND (IMF) 
        /// Currency: SDR (Special Drawing Right)
        /// </summary>
        XDR = 960,

        /// <summary>
        /// Country: IRAN (ISLAMIC REPUBLIC OF)
        /// Currency: Iranian Rial
        /// </summary>
        IRR = 364,

        /// <summary>
        /// Country: IRAQ
        /// Currency: Iraqi Dinar
        /// </summary>
        IQD = 368,

        /// <summary>
        /// Country: ISRAEL
        /// Currency: New Israeli Sheqel
        /// </summary>
        ILS = 376,

        /// <summary>
        /// Country: JAMAICA
        /// Currency: Jamaican Dollar
        /// </summary>
        JMD = 388,

        /// <summary>
        /// Country: JAPAN
        /// Currency: Yen
        /// </summary>
        JPY = 392,

        /// <summary>
        /// Country: JORDAN
        /// Currency: Jordanian Dinar
        /// </summary>
        JOD = 400,

        /// <summary>
        /// Country: KAZAKHSTAN
        /// Currency: Tenge
        /// </summary>
        KZT = 398,

        /// <summary>
        /// Country: KENYA
        /// Currency: Kenyan Shilling
        /// </summary>
        KES = 404,

        /// <summary>
        /// Country: KOREA (THE DEMOCRATIC PEOPLE’S REPUBLIC OF)
        /// Currency: North Korean Won
        /// </summary>
        KPW = 408,

        /// <summary>
        /// Country: KOREA (THE REPUBLIC OF)
        /// Currency: Won
        /// </summary>
        KRW = 410,

        /// <summary>
        /// Country: KUWAIT
        /// Currency: Kuwaiti Dinar
        /// </summary>
        KWD = 414,

        /// <summary>
        /// Country: KYRGYZSTAN
        /// Currency: Som
        /// </summary>
        KGS = 417,

        /// <summary>
        /// Country: LAO PEOPLE’S DEMOCRATIC REPUBLIC (THE)
        /// Currency: Lao Kip
        /// </summary>
        LAK = 418,

        /// <summary>
        /// Country: LEBANON
        /// Currency: Lebanese Pound
        /// </summary>
        LBP = 422,

        /// <summary>
        /// Country: LESOTHO
        /// Currency: Loti
        /// </summary>
        LSL = 426,

        /// <summary>
        /// Country: LESOTHO
        /// Currency: Rand
        /// </summary>
        ZAR = 710,

        /// <summary>
        /// Country: LIBERIA
        /// Currency: Liberian Dollar
        /// </summary>
        LRD = 430,

        /// <summary>
        /// Country: LIBYA
        /// Currency: Libyan Dinar
        /// </summary>
        LYD = 434,

        /// <summary>
        /// Country: LIECHTENSTEIN
        /// Currency: Swiss Franc
        /// </summary>
        CHF = 756,

        /// <summary>
        /// Country: MACAO
        /// Currency: Pataca
        /// </summary>
        MOP = 446,

        /// <summary>
        /// Country: MACEDONIA (THE FORMER YUGOSLAV REPUBLIC OF)
        /// Currency: Denar
        /// </summary>
        MKD = 807,

        /// <summary>
        /// Country: MADAGASCAR
        /// Currency: Malagasy Ariary
        /// </summary>
        MGA = 969,

        /// <summary>
        /// Country: MALAWI
        /// Currency: Malawi Kwacha
        /// </summary>
        MWK = 454,

        /// <summary>
        /// Country: MALAYSIA
        /// Currency: Malaysian Ringgit
        /// </summary>
        MYR = 458,

        /// <summary>
        /// Country: MALDIVES
        /// Currency: Rufiyaa
        /// </summary>
        MVR = 462,

        /// <summary>
        /// Country: MAURITANIA
        /// Currency: Ouguiya
        /// </summary>
        MRU = 929,

        /// <summary>
        /// Country: MAURITIUS
        /// Currency: Mauritius Rupee
        /// </summary>
        MUR = 480,

        /// <summary>
        /// Country: MEMBER COUNTRIES OF THE AFRICAN DEVELOPMENT BANK GROUP
        /// Currency: ADB Unit of Account
        /// </summary>
        XUA = 965,

        /// <summary>
        /// Country: MEXICO
        /// Currency: Mexican Peso
        /// </summary>
        MXN = 484,

        /// <summary>
        /// Country: MEXICO
        /// Currency: Mexican Unidad de Inversion (UDI)
        /// </summary>
        MXV = 979,

        /// <summary>
        /// Country: MOLDOVA (THE REPUBLIC OF)
        /// Currency: Moldovan Leu
        /// </summary>
        MDL = 498,

        /// <summary>
        /// Country: MONGOLIA
        /// Currency: Tugrik
        /// </summary>
        MNT = 496,

        /// <summary>
        /// Country: MOROCCO
        /// Currency: Moroccan Dirham
        /// </summary>
        MAD = 504,

        /// <summary>
        /// Country: MOZAMBIQUE
        /// Currency: Mozambique Metical
        /// </summary>
        MZN = 943,

        /// <summary>
        /// Country: MYANMAR
        /// Currency: Kyat
        /// </summary>
        MMK = 104,

        /// <summary>
        /// Country: NAMIBIA
        /// Currency: Namibia Dollar
        /// </summary>
        NAD = 516,

        /// <summary>
        /// Country: NEPAL
        /// Currency: Nepalese Rupee
        /// </summary>
        NPR = 524,

        /// <summary>
        /// Country: NICARAGUA
        /// Currency: Cordoba Oro
        /// </summary>
        NIO = 558,

        /// <summary>
        /// Country: NIGERIA
        /// Currency: Naira
        /// </summary>
        NGN = 566,

        /// <summary>
        /// Country: OMAN
        /// Currency: Rial Omani
        /// </summary>
        OMR = 512,

        /// <summary>
        /// Country: PAKISTAN
        /// Currency: Pakistan Rupee
        /// </summary>
        PKR = 586,

        /// <summary>
        /// Country: PANAMA
        /// Currency: Balboa
        /// </summary>
        PAB = 590,

        /// <summary>
        /// Country: PAPUA NEW GUINEA
        /// Currency: Kina
        /// </summary>
        PGK = 598,

        /// <summary>
        /// Country: PARAGUAY
        /// Currency: Guarani
        /// </summary>
        PYG = 600,

        /// <summary>
        /// Country: PERU
        /// Currency: Sol
        /// </summary>
        PEN = 604,

        /// <summary>
        /// Country: PHILIPPINES (THE)
        /// Currency: Philippine Peso
        /// </summary>
        PHP = 608,

        /// <summary>
        /// Country: POLAND
        /// Currency: Zloty
        /// </summary>
        PLN = 985,

        /// <summary>
        /// Country: QATAR
        /// Currency: Qatari Rial
        /// </summary>
        QAR = 634,

        /// <summary>
        /// Country: ROMANIA
        /// Currency: Romanian Leu
        /// </summary>
        RON = 946,

        /// <summary>
        /// Country: RUSSIAN FEDERATION (THE)
        /// Currency: Russian Ruble
        /// </summary>
        RUB = 643,

        /// <summary>
        /// Country: RWANDA
        /// Currency: Rwanda Franc
        /// </summary>
        RWF = 646,

        /// <summary>
        /// Country: SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA
        /// Currency: Saint Helena Pound
        /// </summary>
        SHP = 654,

        /// <summary>
        /// Country: SAMOA
        /// Currency: Tala
        /// </summary>
        WST = 882,

        /// <summary>
        /// Country: SAO TOME AND PRINCIPE
        /// Currency: Dobra
        /// </summary>
        STN = 930,

        /// <summary>
        /// Country: SAUDI ARABIA
        /// Currency: Saudi Riyal
        /// </summary>
        SAR = 682,

        /// <summary>
        /// Country: SERBIA
        /// Currency: Serbian Dinar
        /// </summary>
        RSD = 941,

        /// <summary>
        /// Country: SEYCHELLES
        /// Currency: Seychelles Rupee
        /// </summary>
        SCR = 690,

        /// <summary>
        /// Country: SIERRA LEONE
        /// Currency: Leone
        /// </summary>
        SLL = 694,

        /// <summary>
        /// Country: SINGAPORE
        /// Currency: Singapore Dollar
        /// </summary>
        SGD = 702,

        /// <summary>
        /// Country: SISTEMA UNITARIO DE COMPENSACION REGIONAL DE PAGOS "SUCRE"
        /// Currency: Sucre
        /// </summary>
        XSU = 994,

        /// <summary>
        /// Country: SOLOMON ISLANDS
        /// Currency: Solomon Islands Dollar
        /// </summary>
        SBD = 090,

        /// <summary>
        /// Country: SOMALIA
        /// Currency: Somali Shilling
        /// </summary>
        SOS = 706,

        /// <summary>
        /// Country: SOUTH SUDAN
        /// Currency: South Sudanese Pound
        /// </summary>
        SSP = 728,

        /// <summary>
        /// Country: SRI LANKA
        /// Currency: Sri Lanka Rupee
        /// </summary>
        LKR = 144,

        /// <summary>
        /// Country: SUDAN (THE)
        /// Currency: Sudanese Pound
        /// </summary>
        SDG = 938,

        /// <summary>
        /// Country: SURINAME
        /// Currency: Surinam Dollar
        /// </summary>
        SRD = 968,

        /// <summary>
        /// Country: ESWATINI
        /// Currency: Lilangeni
        /// </summary>
        SZL = 748,

        /// <summary>
        /// Country: SWEDEN
        /// Currency: Swedish Krona
        /// </summary>
        SEK = 752,

        /// <summary>
        /// Country: SWITZERLAND
        /// Currency: WIR Euro
        /// </summary>
        CHE = 947,

        /// <summary>
        /// Country: SWITZERLAND
        /// Currency: WIR Franc
        /// </summary>
        CHW = 948,

        /// <summary>
        /// Country: SYRIAN ARAB REPUBLIC
        /// Currency: Syrian Pound
        /// </summary>
        SYP = 760,

        /// <summary>
        /// Country: TAIWAN (PROVINCE OF CHINA)
        /// Currency: New Taiwan Dollar
        /// </summary>
        TWD = 901,

        /// <summary>
        /// Country: TAJIKISTAN
        /// Currency: Somoni
        /// </summary>
        TJS = 972,

        /// <summary>
        /// Country: TANZANIA, UNITED REPUBLIC OF
        /// Currency: Tanzanian Shilling
        /// </summary>
        TZS = 834,

        /// <summary>
        /// Country: THAILAND
        /// Currency: Baht
        /// </summary>
        THB = 764,

        /// <summary>
        /// Country: TONGA
        /// Currency: Pa’anga
        /// </summary>
        TOP = 776,

        /// <summary>
        /// Country: TRINIDAD AND TOBAGO
        /// Currency: Trinidad and Tobago Dollar
        /// </summary>
        TTD = 780,

        /// <summary>
        /// Country: TUNISIA
        /// Currency: Tunisian Dinar
        /// </summary>
        TND = 788,

        /// <summary>
        /// Country: TURKEY
        /// Currency: Turkish Lira
        /// </summary>
        TRY = 949,

        /// <summary>
        /// Country: TURKMENISTAN
        /// Currency: Turkmenistan New Manat
        /// </summary>
        TMT = 934,

        /// <summary>
        /// Country: UGANDA
        /// Currency: Uganda Shilling
        /// </summary>
        UGX = 800,

        /// <summary>
        /// Country: UKRAINE
        /// Currency: Hryvnia
        /// </summary>
        UAH = 980,

        /// <summary>
        /// Country: UNITED ARAB EMIRATES (THE)
        /// Currency: UAE Dirham
        /// </summary>
        AED = 784,

        /// <summary>
        /// Country: UNITED STATES OF AMERICA (THE)
        /// Currency: US Dollar (Next day)
        /// </summary>
        USN = 997,

        /// <summary>
        /// Country: URUGUAY
        /// Currency: Peso Uruguayo
        /// </summary>
        UYU = 858,

        /// <summary>
        /// Country: URUGUAY
        /// Currency: Uruguay Peso en Unidades Indexadas (UI)
        /// </summary>
        UYI = 940,

        /// <summary>
        /// Country: URUGUAY
        /// Currency: Unidad Previsional
        /// </summary>
        UYW = 927,

        /// <summary>
        /// Country: UZBEKISTAN
        /// Currency: Uzbekistan Sum
        /// </summary>
        UZS = 860,

        /// <summary>
        /// Country: VANUATU
        /// Currency: Vatu
        /// </summary>
        VUV = 548,

        /// <summary>
        /// Country: VENEZUELA (BOLIVARIAN REPUBLIC OF)
        /// Currency: Bolívar Soberano
        /// </summary>
        VES = 928,

        /// <summary>
        /// Country: VIET NAM
        /// Currency: Dong
        /// </summary>
        VND = 704,

        /// <summary>
        /// Country: YEMEN
        /// Currency: Yemeni Rial
        /// </summary>
        YER = 886,

        /// <summary>
        /// Country: ZAMBIA
        /// Currency: Zambian Kwacha
        /// </summary>
        ZMW = 967,

        /// <summary>
        /// Country: ZIMBABWE
        /// Currency: Zimbabwe Dollar
        /// </summary>
        ZWL = 932,

        /// <summary>
        /// Country: ZZ01_Bond Markets Unit European_EURCO
        /// Currency: Bond Markets Unit European Composite Unit (EURCO)
        /// </summary>
        XBA = 955,

        /// <summary>
        /// Country: ZZ02_Bond Markets Unit European_EMU-6
        /// Currency: Bond Markets Unit European Monetary Unit (E.M.U.-6)
        /// </summary>
        XBB = 956,

        /// <summary>
        /// Country: ZZ03_Bond Markets Unit European_EUA-9
        /// Currency: Bond Markets Unit European Unit of Account 9 (E.U.A.-9)
        /// </summary>
        XBC = 957,

        /// <summary>
        /// Country: ZZ04_Bond Markets Unit European_EUA-17
        /// Currency: Bond Markets Unit European Unit of Account 17 (E.U.A.-17)
        /// </summary>
        XBD = 958,

        /// <summary>
        /// Country: ZZ06_Testing_Code
        /// Currency: Codes specifically reserved for testing purposes
        /// </summary>
        XTS = 963,

        /// <summary>
        /// Country: ZZ07_No_Currency
        /// Currency: The codes assigned for transactions where no currency is involved
        /// </summary>
        XXX = 999,

        /// <summary>
        /// Country: ZZ08_Gold
        /// Currency: Gold
        /// </summary>
        XAU = 959,

        /// <summary>
        /// Country: ZZ09_Palladium
        /// Currency: Palladium
        /// </summary>
        XPD = 964,

        /// <summary>
        /// Country: ZZ10_Platinum
        /// Currency: Platinum
        /// </summary>
        XPT = 962,

        /// <summary>
        /// Country: ZZ11_Silver
        /// Currency: Silver
        /// </summary>
        XAG = 961,

        /// <summary>
        /// Country: AFGHANISTAN
        /// Currency: Afghani
        /// </summary>
        AFA = 004,

        /// <summary>
        /// Country: ÅLAND ISLANDS
        /// Currency: Markka
        /// </summary>
        FIM = 246,

        /// <summary>
        /// Country: ANDORRA
        /// Currency: Andorran Peseta
        /// </summary>
        ADP = 020,

        /// <summary>
        /// Country: ANDORRA
        /// Currency: Spanish Peseta
        /// </summary>
        ESP = 724,

        /// <summary>
        /// Country: ANDORRA
        /// Currency: French Franc
        /// </summary>
        FRF = 250,

        /// <summary>
        /// Country: ANGOLA
        /// Currency: Kwanza
        /// </summary>
        AOK = 024,

        /// <summary>
        /// Country: ANGOLA
        /// Currency: Kwanza Reajustado
        /// </summary>
        AOR = 982,

        /// <summary>
        /// Country: ARMENIA
        /// Currency: Russian Ruble
        /// </summary>
        RUR = 810,

        /// <summary>
        /// Country: AUSTRIA
        /// Currency: Schilling
        /// </summary>
        ATS = 040,

        /// <summary>
        /// Country: AZERBAIJAN
        /// Currency: Azerbaijan Manat
        /// </summary>
        AYM = 945,

        /// <summary>
        /// Country: AZERBAIJAN
        /// Currency: Azerbaijanian Manat
        /// </summary>
        AZM = 031,

        /// <summary>
        /// Country: BELARUS
        /// Currency: Belarusian Ruble
        /// </summary>
        BYB = 112,

        /// <summary>
        /// Country: BELARUS
        /// Currency: Belarusian Ruble
        /// </summary>
        BYR = 974,

        /// <summary>
        /// Country: BELGIUM
        /// Currency: Convertible Franc
        /// </summary>
        BEC = 993,

        /// <summary>
        /// Country: BELGIUM
        /// Currency: Belgian Franc
        /// </summary>
        BEF = 056,

        /// <summary>
        /// Country: BELGIUM
        /// Currency: Financial Franc
        /// </summary>
        BEL = 992,

        /// <summary>
        /// Country: BOSNIA AND HERZEGOVINA
        /// Currency: Dinar
        /// </summary>
        BAD = 070,

        /// <summary>
        /// Country: BRAZIL
        /// Currency: Cruzeiro
        /// </summary>
        BRB = 076,

        /// <summary>
        /// Country: BRAZIL
        /// Currency: Cruzeiro Real
        /// </summary>
        BRR = 987,

        /// <summary>
        /// Country: BULGARIA
        /// Currency: Lev A/52
        /// </summary>
        BGJ = 100,

        /// <summary>
        /// Country: CYPRUS
        /// Currency: Cyprus Pound
        /// </summary>
        CYP = 196,

        /// <summary>
        /// Country: CZECHOSLOVAKIA
        /// Currency: Koruna
        /// </summary>
        CSK = 200,

        /// <summary>
        /// Country: ECUADOR
        /// Currency: Sucre
        /// </summary>
        ECS = 218,

        /// <summary>
        /// Country: ECUADOR
        /// Currency: Unidad de Valor Constante (UVC)
        /// </summary>
        ECV = 983,

        /// <summary>
        /// Country: EQUATORIAL GUINEA
        /// Currency: Ekwele
        /// </summary>
        GQE = 226,

        /// <summary>
        /// Country: ESTONIA
        /// Currency: Kroon
        /// </summary>
        EEK = 233,

        /// <summary>
        /// Country: EUROPEAN MONETARY CO-OPERATION FUND (EMCF)
        /// Currency: European Currency Unit (E.C.U)
        /// </summary>
        XEU = 954,

        /// <summary>
        /// Country: GEORGIA
        /// Currency: Georgian Coupon
        /// </summary>
        GEK = 268,

        /// <summary>
        /// Country: GERMAN DEMOCRATIC REPUBLIC
        /// Currency: Mark der DDR
        /// </summary>
        DDM = 278,

        /// <summary>
        /// Country: GERMANY
        /// Currency: Deutsche Mark
        /// </summary>
        DEM = 276,

        /// <summary>
        /// Country: GHANA
        /// Currency: Cedi
        /// </summary>
        GHC = 288,

        /// <summary>
        /// Country: GHANA
        /// Currency: Ghana Cedi
        /// </summary>
        GHP = 939,

        /// <summary>
        /// Country: GREECE
        /// Currency: Drachma
        /// </summary>
        GRD = 300,

        /// <summary>
        /// Country: GUINEA-BISSAU
        /// Currency: Guinea Escudo
        /// </summary>
        GWE = 624,

        /// <summary>
        /// Country: HOLY SEE (VATICAN CITY STATE)
        /// Currency: Italian Lira
        /// </summary>
        ITL = 380,

        /// <summary>
        /// Country: IRELAND
        /// Currency: Irish Pound
        /// </summary>
        IEP = 372,

        /// <summary>
        /// Country: LATVIA
        /// Currency: Latvian Lats
        /// </summary>
        LVL = 428,

        /// <summary>
        /// Country: LESOTHO
        /// Currency: Financial Rand
        /// </summary>
        ZAL = 991,

        /// <summary>
        /// Country: LITHUANIA
        /// Currency: Lithuanian Litas
        /// </summary>
        LTL = 440,

        /// <summary>
        /// Country: LUXEMBOURG
        /// Currency: Luxembourg Convertible Franc
        /// </summary>
        LUC = 989,

        /// <summary>
        /// Country: LUXEMBOURG
        /// Currency: Luxembourg Franc
        /// </summary>
        LUF = 442,

        /// <summary>
        /// Country: LUXEMBOURG
        /// Currency: Luxembourg Financial Franc
        /// </summary>
        LUL = 988,

        /// <summary>
        /// Country: MADAGASCAR
        /// Currency: Malagasy Franc
        /// </summary>
        MGF = 450,

        /// <summary>
        /// Country: MALI
        /// Currency: Mali Franc
        /// </summary>
        MLF = 466,

        /// <summary>
        /// Country: MALTA
        /// Currency: Maltese Lira
        /// </summary>
        MTL = 470,

        /// <summary>
        /// Country: MAURITANIA
        /// Currency: Ouguiya
        /// </summary>
        MRO = 478,

        /// <summary>
        /// Country: MOZAMBIQUE
        /// Currency: Mozambique Escudo
        /// </summary>
        MZE = 508,

        /// <summary>
        /// Country: NETHERLANDS
        /// Currency: Netherlands Guilder
        /// </summary>
        NLG = 528,

        /// <summary>
        /// Country: POLAND
        /// Currency: Zloty
        /// </summary>
        PLZ = 616,

        /// <summary>
        /// Country: PORTUGAL
        /// Currency: Portuguese Escudo
        /// </summary>
        PTE = 620,

        /// <summary>
        /// Country: ROMANIA
        /// Currency: Leu A/52
        /// </summary>
        ROK = 642,

        /// <summary>
        /// Country: SAO TOME AND PRINCIPE
        /// Currency: Dobra
        /// </summary>
        STD = 678,

        /// <summary>
        /// Country: SERBIA AND MONTENEGRO
        /// Currency: Serbian Dinar
        /// </summary>
        CSD = 891,

        /// <summary>
        /// Country: SLOVAKIA
        /// Currency: Slovak Koruna
        /// </summary>
        SKK = 703,

        /// <summary>
        /// Country: SLOVENIA
        /// Currency: Tolar
        /// </summary>
        SIT = 705,

        /// <summary>
        /// Country: SOUTHERN RHODESIA 
        /// Currency: Rhodesian Dollar
        /// </summary>
        RHD = 716,

        /// <summary>
        /// Country: SPAIN
        /// Currency: Spanish Peseta
        /// </summary>
        ESA = 996,

        /// <summary>
        /// Country: SPAIN
        /// Currency: "A" Account (convertible Peseta Account)
        /// </summary>
        ESB = 995,

        /// <summary>
        /// Country: SUDAN
        /// Currency: Sudanese Dinar
        /// </summary>
        SDD = 736,

        /// <summary>
        /// Country: SURINAME
        /// Currency: Surinam Guilder
        /// </summary>
        SRG = 740,

        /// <summary>
        /// Country: TAJIKISTAN
        /// Currency: Tajik Ruble
        /// </summary>
        TJR = 762,

        /// <summary>
        /// Country: TIMOR-LESTE
        /// Currency: Timor Escudo
        /// </summary>
        TPE = 626,

        /// <summary>
        /// Country: TURKEY
        /// Currency: Old Turkish Lira
        /// </summary>
        TRL = 792,

        /// <summary>
        /// Country: TURKMENISTAN
        /// Currency: Turkmenistan Manat
        /// </summary>
        TMM = 795,

        /// <summary>
        /// Country: UKRAINE
        /// Currency: Karbovanet
        /// </summary>
        UAK = 804,

        /// <summary>
        /// Country: UNITED STATES
        /// Currency: US Dollar (Same day)
        /// </summary>
        USS = 998,

        /// <summary>
        /// Country: VENEZUELA
        /// Currency: Bolivar
        /// </summary>
        VEB = 862,

        /// <summary>
        /// Country: VENEZUELA
        /// Currency: Bolivar Fuerte
        /// </summary>
        VEF = 937,

        /// <summary>
        /// Country: YEMEN, DEMOCRATIC
        /// Currency: Yemeni Dinar
        /// </summary>
        YDD = 720,

        /// <summary>
        /// Country: YUGOSLAVIA
        /// Currency: New Yugoslavian Dinar
        /// </summary>
        YUD = 890,

        /// <summary>
        /// Country: ZAIRE
        /// Currency: New Zaire
        /// </summary>
        ZRN = 180,

        /// <summary>
        /// Country: ZAMBIA
        /// Currency: Zambian Kwacha
        /// </summary>
        ZMK = 894,

        /// <summary>
        /// Country: ZIMBABWE
        /// Currency: Zimbabwe Dollar (new)
        /// </summary>
        ZWN = 942,

        /// <summary>
        /// Country: ZIMBABWE
        /// Currency: Zimbabwe Dollar
        /// </summary>
        ZWR = 935,

        /// <summary>
        /// Fallback value
        /// </summary>
        Unknown = 0
    }


    internal static class CurrencyCodesExtensions
    {
        public static CurrencyCodes FromString(this CurrencyCodes _, string s)
        {
            try
            {
                return (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), s);
            }
            catch
            {
                return CurrencyCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this CurrencyCodes c)
        {
            return c.ToString("g");
        } // !ToString()
    }
}
