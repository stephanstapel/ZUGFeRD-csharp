using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Country codes based on ISO 3166
    /// source:
    /// http://www.iso.org/iso/home/standards/country_codes/country_names_and_code_elements_txt.htm
    /// 
    /// enum can be re-generated using:
    /// 
    /// f = file("countries.txt")
    /// g = file("output.cs", "w+")
    /// 
    /// for line in f.readlines():
    ///     parts = line.replace("\n", "").split(";")
    /// 
    ///     g.write("/// <summary>\n")
    ///     g.write("/// " + parts[0] + "\n")
    ///     g.write("/// </summary>\n")
    ///     g.write(parts[1] + ",\n")
    ///     g.write("\n")    
    /// 
    /// f.close()
    /// g.close()

    /// </summary>
    public enum CountryCodes
    {
        /// <summary>
        /// AFGHANISTAN
        /// </summary>
        AF,

        /// <summary>
        /// ÅLAND ISLANDS
        /// </summary>
        AX,

        /// <summary>
        /// ALBANIA
        /// </summary>
        AL,

        /// <summary>
        /// ALGERIA
        /// </summary>
        DZ,

        /// <summary>
        /// AMERICAN SAMOA
        /// </summary>
        AS,

        /// <summary>
        /// ANDORRA
        /// </summary>
        AD,

        /// <summary>
        /// ANGOLA
        /// </summary>
        AO,

        /// <summary>
        /// ANGUILLA
        /// </summary>
        AI,

        /// <summary>
        /// ANTARCTICA
        /// </summary>
        AQ,

        /// <summary>
        /// ANTIGUA AND BARBUDA
        /// </summary>
        AG,

        /// <summary>
        /// ARGENTINA
        /// </summary>
        AR,

        /// <summary>
        /// ARMENIA
        /// </summary>
        AM,

        /// <summary>
        /// ARUBA
        /// </summary>
        AW,

        /// <summary>
        /// AUSTRALIA
        /// </summary>
        AU,

        /// <summary>
        /// AUSTRIA
        /// </summary>
        AT,

        /// <summary>
        /// AZERBAIJAN
        /// </summary>
        AZ,

        /// <summary>
        /// BAHAMAS
        /// </summary>
        BS,

        /// <summary>
        /// BAHRAIN
        /// </summary>
        BH,

        /// <summary>
        /// BANGLADESH
        /// </summary>
        BD,

        /// <summary>
        /// BARBADOS
        /// </summary>
        BB,

        /// <summary>
        /// BELARUS
        /// </summary>
        BY,

        /// <summary>
        /// BELGIUM
        /// </summary>
        BE,

        /// <summary>
        /// BELIZE
        /// </summary>
        BZ,

        /// <summary>
        /// BENIN
        /// </summary>
        BJ,

        /// <summary>
        /// BERMUDA
        /// </summary>
        BM,

        /// <summary>
        /// BHUTAN
        /// </summary>
        BT,

        /// <summary>
        /// BOLIVIA, PLURINATIONAL STATE OF
        /// </summary>
        BO,

        /// <summary>
        /// BONAIRE, SINT EUSTATIUS AND SABA
        /// </summary>
        BQ,

        /// <summary>
        /// BOSNIA AND HERZEGOVINA
        /// </summary>
        BA,

        /// <summary>
        /// BOTSWANA
        /// </summary>
        BW,

        /// <summary>
        /// BOUVET ISLAND
        /// </summary>
        BV,

        /// <summary>
        /// BRAZIL
        /// </summary>
        BR,

        /// <summary>
        /// BRITISH INDIAN OCEAN TERRITORY
        /// </summary>
        IO,

        /// <summary>
        /// BRUNEI DARUSSALAM
        /// </summary>
        BN,

        /// <summary>
        /// BULGARIA
        /// </summary>
        BG,

        /// <summary>
        /// BURKINA FASO
        /// </summary>
        BF,

        /// <summary>
        /// BURUNDI
        /// </summary>
        BI,

        /// <summary>
        /// CAMBODIA
        /// </summary>
        KH,

        /// <summary>
        /// CAMEROON
        /// </summary>
        CM,

        /// <summary>
        /// CANADA
        /// </summary>
        CA,

        /// <summary>
        /// CAPE VERDE
        /// </summary>
        CV,

        /// <summary>
        /// CAYMAN ISLANDS
        /// </summary>
        KY,

        /// <summary>
        /// CENTRAL AFRICAN REPUBLIC
        /// </summary>
        CF,

        /// <summary>
        /// CHAD
        /// </summary>
        TD,

        /// <summary>
        /// CHILE
        /// </summary>
        CL,

        /// <summary>
        /// CHINA
        /// </summary>
        CN,

        /// <summary>
        /// CHRISTMAS ISLAND
        /// </summary>
        CX,

        /// <summary>
        /// COCOS (KEELING) ISLANDS
        /// </summary>
        CC,

        /// <summary>
        /// COLOMBIA
        /// </summary>
        CO,

        /// <summary>
        /// COMOROS
        /// </summary>
        KM,

        /// <summary>
        /// CONGO
        /// </summary>
        CG,

        /// <summary>
        /// CONGO, THE DEMOCRATIC REPUBLIC OF THE
        /// </summary>
        CD,

        /// <summary>
        /// COOK ISLANDS
        /// </summary>
        CK,

        /// <summary>
        /// COSTA RICA
        /// </summary>
        CR,

        /// <summary>
        /// CÔTE D'IVOIRE
        /// </summary>
        CI,

        /// <summary>
        /// CROATIA
        /// </summary>
        HR,

        /// <summary>
        /// CUBA
        /// </summary>
        CU,

        /// <summary>
        /// CURAÇAO
        /// </summary>
        CW,

        /// <summary>
        /// CYPRUS
        /// </summary>
        CY,

        /// <summary>
        /// CZECH REPUBLIC
        /// </summary>
        CZ,

        /// <summary>
        /// DENMARK
        /// </summary>
        DK,

        /// <summary>
        /// DJIBOUTI
        /// </summary>
        DJ,

        /// <summary>
        /// DOMINICA
        /// </summary>
        DM,

        /// <summary>
        /// DOMINICAN REPUBLIC
        /// </summary>
        DO,

        /// <summary>
        /// ECUADOR
        /// </summary>
        EC,

        /// <summary>
        /// EGYPT
        /// </summary>
        EG,

        /// <summary>
        /// EL SALVADOR
        /// </summary>
        SV,

        /// <summary>
        /// EQUATORIAL GUINEA
        /// </summary>
        GQ,

        /// <summary>
        /// ERITREA
        /// </summary>
        ER,

        /// <summary>
        /// ESTONIA
        /// </summary>
        EE,

        /// <summary>
        /// ETHIOPIA
        /// </summary>
        ET,

        /// <summary>
        /// FALKLAND ISLANDS (MALVINAS)
        /// </summary>
        FK,

        /// <summary>
        /// FAROE ISLANDS
        /// </summary>
        FO,

        /// <summary>
        /// FIJI
        /// </summary>
        FJ,

        /// <summary>
        /// FINLAND
        /// </summary>
        FI,

        /// <summary>
        /// FRANCE
        /// </summary>
        FR,

        /// <summary>
        /// FRENCH GUIANA
        /// </summary>
        GF,

        /// <summary>
        /// FRENCH POLYNESIA
        /// </summary>
        PF,

        /// <summary>
        /// FRENCH SOUTHERN TERRITORIES
        /// </summary>
        TF,

        /// <summary>
        /// GABON
        /// </summary>
        GA,

        /// <summary>
        /// GAMBIA
        /// </summary>
        GM,

        /// <summary>
        /// GEORGIA
        /// </summary>
        GE,

        /// <summary>
        /// GERMANY
        /// </summary>
        DE,

        /// <summary>
        /// GHANA
        /// </summary>
        GH,

        /// <summary>
        /// GIBRALTAR
        /// </summary>
        GI,

        /// <summary>
        /// GREECE
        /// </summary>
        GR,

        /// <summary>
        /// GREENLAND
        /// </summary>
        GL,

        /// <summary>
        /// GRENADA
        /// </summary>
        GD,

        /// <summary>
        /// GUADELOUPE
        /// </summary>
        GP,

        /// <summary>
        /// GUAM
        /// </summary>
        GU,

        /// <summary>
        /// GUATEMALA
        /// </summary>
        GT,

        /// <summary>
        /// GUERNSEY
        /// </summary>
        GG,

        /// <summary>
        /// GUINEA
        /// </summary>
        GN,

        /// <summary>
        /// GUINEA-BISSAU
        /// </summary>
        GW,

        /// <summary>
        /// GUYANA
        /// </summary>
        GY,

        /// <summary>
        /// HAITI
        /// </summary>
        HT,

        /// <summary>
        /// HEARD ISLAND AND MCDONALD ISLANDS
        /// </summary>
        HM,

        /// <summary>
        /// HOLY SEE (VATICAN CITY STATE)
        /// </summary>
        VA,

        /// <summary>
        /// HONDURAS
        /// </summary>
        HN,

        /// <summary>
        /// HONG KONG
        /// </summary>
        HK,

        /// <summary>
        /// HUNGARY
        /// </summary>
        HU,

        /// <summary>
        /// ICELAND
        /// </summary>
        IS,

        /// <summary>
        /// INDIA
        /// </summary>
        IN,

        /// <summary>
        /// INDONESIA
        /// </summary>
        ID,

        /// <summary>
        /// IRAN, ISLAMIC REPUBLIC OF
        /// </summary>
        IR,

        /// <summary>
        /// IRAQ
        /// </summary>
        IQ,

        /// <summary>
        /// IRELAND
        /// </summary>
        IE,

        /// <summary>
        /// ISLE OF MAN
        /// </summary>
        IM,

        /// <summary>
        /// ISRAEL
        /// </summary>
        IL,

        /// <summary>
        /// ITALY
        /// </summary>
        IT,

        /// <summary>
        /// JAMAICA
        /// </summary>
        JM,

        /// <summary>
        /// JAPAN
        /// </summary>
        JP,

        /// <summary>
        /// JERSEY
        /// </summary>
        JE,

        /// <summary>
        /// JORDAN
        /// </summary>
        JO,

        /// <summary>
        /// KAZAKHSTAN
        /// </summary>
        KZ,

        /// <summary>
        /// KENYA
        /// </summary>
        KE,

        /// <summary>
        /// KIRIBATI
        /// </summary>
        KI,

        /// <summary>
        /// KOREA, DEMOCRATIC PEOPLE'S REPUBLIC OF
        /// </summary>
        KP,

        /// <summary>
        /// KOREA, REPUBLIC OF
        /// </summary>
        KR,

        /// <summary>
        /// KUWAIT
        /// </summary>
        KW,

        /// <summary>
        /// KYRGYZSTAN
        /// </summary>
        KG,

        /// <summary>
        /// LAO PEOPLE'S DEMOCRATIC REPUBLIC
        /// </summary>
        LA,

        /// <summary>
        /// LATVIA
        /// </summary>
        LV,

        /// <summary>
        /// LEBANON
        /// </summary>
        LB,

        /// <summary>
        /// LESOTHO
        /// </summary>
        LS,

        /// <summary>
        /// LIBERIA
        /// </summary>
        LR,

        /// <summary>
        /// LIBYA
        /// </summary>
        LY,

        /// <summary>
        /// LIECHTENSTEIN
        /// </summary>
        LI,

        /// <summary>
        /// LITHUANIA
        /// </summary>
        LT,

        /// <summary>
        /// LUXEMBOURG
        /// </summary>
        LU,

        /// <summary>
        /// MACAO
        /// </summary>
        MO,

        /// <summary>
        /// MACEDONIA, THE FORMER YUGOSLAV REPUBLIC OF
        /// </summary>
        MK,

        /// <summary>
        /// MADAGASCAR
        /// </summary>
        MG,

        /// <summary>
        /// MALAWI
        /// </summary>
        MW,

        /// <summary>
        /// MALAYSIA
        /// </summary>
        MY,

        /// <summary>
        /// MALDIVES
        /// </summary>
        MV,

        /// <summary>
        /// MALI
        /// </summary>
        ML,

        /// <summary>
        /// MALTA
        /// </summary>
        MT,

        /// <summary>
        /// MARSHALL ISLANDS
        /// </summary>
        MH,

        /// <summary>
        /// MARTINIQUE
        /// </summary>
        MQ,

        /// <summary>
        /// MAURITANIA
        /// </summary>
        MR,

        /// <summary>
        /// MAURITIUS
        /// </summary>
        MU,

        /// <summary>
        /// MAYOTTE
        /// </summary>
        YT,

        /// <summary>
        /// MEXICO
        /// </summary>
        MX,

        /// <summary>
        /// MICRONESIA, FEDERATED STATES OF
        /// </summary>
        FM,

        /// <summary>
        /// MOLDOVA, REPUBLIC OF
        /// </summary>
        MD,

        /// <summary>
        /// MONACO
        /// </summary>
        MC,

        /// <summary>
        /// MONGOLIA
        /// </summary>
        MN,

        /// <summary>
        /// MONTENEGRO
        /// </summary>
        ME,

        /// <summary>
        /// MONTSERRAT
        /// </summary>
        MS,

        /// <summary>
        /// MOROCCO
        /// </summary>
        MA,

        /// <summary>
        /// MOZAMBIQUE
        /// </summary>
        MZ,

        /// <summary>
        /// MYANMAR
        /// </summary>
        MM,

        /// <summary>
        /// NAMIBIA
        /// </summary>
        NA,

        /// <summary>
        /// NAURU
        /// </summary>
        NR,

        /// <summary>
        /// NEPAL
        /// </summary>
        NP,

        /// <summary>
        /// NETHERLANDS
        /// </summary>
        NL,

        /// <summary>
        /// NEW CALEDONIA
        /// </summary>
        NC,

        /// <summary>
        /// NEW ZEALAND
        /// </summary>
        NZ,

        /// <summary>
        /// NICARAGUA
        /// </summary>
        NI,

        /// <summary>
        /// NIGER
        /// </summary>
        NE,

        /// <summary>
        /// NIGERIA
        /// </summary>
        NG,

        /// <summary>
        /// NIUE
        /// </summary>
        NU,

        /// <summary>
        /// NORFOLK ISLAND
        /// </summary>
        NF,

        /// <summary>
        /// NORTHERN MARIANA ISLANDS
        /// </summary>
        MP,

        /// <summary>
        /// NORWAY
        /// </summary>
        NO,

        /// <summary>
        /// OMAN
        /// </summary>
        OM,

        /// <summary>
        /// PAKISTAN
        /// </summary>
        PK,

        /// <summary>
        /// PALAU
        /// </summary>
        PW,

        /// <summary>
        /// PALESTINE, STATE OF
        /// </summary>
        PS,

        /// <summary>
        /// PANAMA
        /// </summary>
        PA,

        /// <summary>
        /// PAPUA NEW GUINEA
        /// </summary>
        PG,

        /// <summary>
        /// PARAGUAY
        /// </summary>
        PY,

        /// <summary>
        /// PERU
        /// </summary>
        PE,

        /// <summary>
        /// PHILIPPINES
        /// </summary>
        PH,

        /// <summary>
        /// PITCAIRN
        /// </summary>
        PN,

        /// <summary>
        /// POLAND
        /// </summary>
        PL,

        /// <summary>
        /// PORTUGAL
        /// </summary>
        PT,

        /// <summary>
        /// PUERTO RICO
        /// </summary>
        PR,

        /// <summary>
        /// QATAR
        /// </summary>
        QA,

        /// <summary>
        /// RÉUNION
        /// </summary>
        RE,

        /// <summary>
        /// ROMANIA
        /// </summary>
        RO,

        /// <summary>
        /// RUSSIAN FEDERATION
        /// </summary>
        RU,

        /// <summary>
        /// RWANDA
        /// </summary>
        RW,

        /// <summary>
        /// SAINT BARTHÉLEMY
        /// </summary>
        BL,

        /// <summary>
        /// SAINT HELENA, ASCENSION AND TRISTAN DA CUNHA
        /// </summary>
        SH,

        /// <summary>
        /// SAINT KITTS AND NEVIS
        /// </summary>
        KN,

        /// <summary>
        /// SAINT LUCIA
        /// </summary>
        LC,

        /// <summary>
        /// SAINT MARTIN (FRENCH PART)
        /// </summary>
        MF,

        /// <summary>
        /// SAINT PIERRE AND MIQUELON
        /// </summary>
        PM,

        /// <summary>
        /// SAINT VINCENT AND THE GRENADINES
        /// </summary>
        VC,

        /// <summary>
        /// SAMOA
        /// </summary>
        WS,

        /// <summary>
        /// SAN MARINO
        /// </summary>
        SM,

        /// <summary>
        /// SAO TOME AND PRINCIPE
        /// </summary>
        ST,

        /// <summary>
        /// SAUDI ARABIA
        /// </summary>
        SA,

        /// <summary>
        /// SENEGAL
        /// </summary>
        SN,

        /// <summary>
        /// SERBIA
        /// </summary>
        RS,

        /// <summary>
        /// SEYCHELLES
        /// </summary>
        SC,

        /// <summary>
        /// SIERRA LEONE
        /// </summary>
        SL,

        /// <summary>
        /// SINGAPORE
        /// </summary>
        SG,

        /// <summary>
        /// SINT MAARTEN (DUTCH PART)
        /// </summary>
        SX,

        /// <summary>
        /// SLOVAKIA
        /// </summary>
        SK,

        /// <summary>
        /// SLOVENIA
        /// </summary>
        SI,

        /// <summary>
        /// SOLOMON ISLANDS
        /// </summary>
        SB,

        /// <summary>
        /// SOMALIA
        /// </summary>
        SO,

        /// <summary>
        /// SOUTH AFRICA
        /// </summary>
        ZA,

        /// <summary>
        /// SOUTH GEORGIA AND THE SOUTH SANDWICH ISLANDS
        /// </summary>
        GS,

        /// <summary>
        /// SOUTH SUDAN
        /// </summary>
        SS,

        /// <summary>
        /// SPAIN
        /// </summary>
        ES,

        /// <summary>
        /// SRI LANKA
        /// </summary>
        LK,

        /// <summary>
        /// SUDAN
        /// </summary>
        SD,

        /// <summary>
        /// SURINAME
        /// </summary>
        SR,

        /// <summary>
        /// SVALBARD AND JAN MAYEN
        /// </summary>
        SJ,

        /// <summary>
        /// SWAZILAND
        /// </summary>
        SZ,

        /// <summary>
        /// SWEDEN
        /// </summary>
        SE,

        /// <summary>
        /// SWITZERLAND
        /// </summary>
        CH,

        /// <summary>
        /// SYRIAN ARAB REPUBLIC
        /// </summary>
        SY,

        /// <summary>
        /// TAIWAN, PROVINCE OF CHINA
        /// </summary>
        TW,

        /// <summary>
        /// TAJIKISTAN
        /// </summary>
        TJ,

        /// <summary>
        /// TANZANIA, UNITED REPUBLIC OF
        /// </summary>
        TZ,

        /// <summary>
        /// THAILAND
        /// </summary>
        TH,

        /// <summary>
        /// TIMOR-LESTE
        /// </summary>
        TL,

        /// <summary>
        /// TOGO
        /// </summary>
        TG,

        /// <summary>
        /// TOKELAU
        /// </summary>
        TK,

        /// <summary>
        /// TONGA
        /// </summary>
        TO,

        /// <summary>
        /// TRINIDAD AND TOBAGO
        /// </summary>
        TT,

        /// <summary>
        /// TUNISIA
        /// </summary>
        TN,

        /// <summary>
        /// TURKEY
        /// </summary>
        TR,

        /// <summary>
        /// TURKMENISTAN
        /// </summary>
        TM,

        /// <summary>
        /// TURKS AND CAICOS ISLANDS
        /// </summary>
        TC,

        /// <summary>
        /// TUVALU
        /// </summary>
        TV,

        /// <summary>
        /// UGANDA
        /// </summary>
        UG,

        /// <summary>
        /// UKRAINE
        /// </summary>
        UA,

        /// <summary>
        /// UNITED ARAB EMIRATES
        /// </summary>
        AE,

        /// <summary>
        /// UNITED KINGDOM
        /// </summary>
        GB,

        /// <summary>
        /// UNITED STATES
        /// </summary>
        US,

        /// <summary>
        /// UNITED STATES MINOR OUTLYING ISLANDS
        /// </summary>
        UM,

        /// <summary>
        /// URUGUAY
        /// </summary>
        UY,

        /// <summary>
        /// UZBEKISTAN
        /// </summary>
        UZ,

        /// <summary>
        /// VANUATU
        /// </summary>
        VU,

        /// <summary>
        /// VENEZUELA, BOLIVARIAN REPUBLIC OF
        /// </summary>
        VE,

        /// <summary>
        /// VIET NAM
        /// </summary>
        VN,

        /// <summary>
        /// VIRGIN ISLANDS, BRITISH
        /// </summary>
        VG,

        /// <summary>
        /// VIRGIN ISLANDS, U.S.
        /// </summary>
        VI,

        /// <summary>
        /// WALLIS AND FUTUNA
        /// </summary>
        WF,

        /// <summary>
        /// WESTERN SAHARA
        /// </summary>
        EH,

        /// <summary>
        /// YEMEN
        /// </summary>
        YE,

        /// <summary>
        /// ZAMBIA
        /// </summary>
        ZM,

        /// <summary>
        /// ZIMBABWE
        /// </summary>
        ZW,

        Unknown
    }


    internal static class CountryCodesExtensions
    {
        public static CountryCodes FromString(this CountryCodes _c, string s)
        {
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
            return c.ToString("g");
        } // !ToString()
    }
}
