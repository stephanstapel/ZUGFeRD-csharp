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
    /// For a reference see:
    /// https://docs.peppol.eu/poacc/billing/3.0/codelist/eas/
    /// </summary>
    public enum ElectronicAddressSchemeIdentifiers
    {
        /// <summary>
        /// System Information et Repertoire des Entreprise et des Etablissements: SIRENE
        /// </summary>
        [EnumStringValue("0002")]
        SystemInformationEtRepertoireDesEntrepriseEtDesEtablissementsSirene = 0002,

        /// <summary>
        /// Organisationsnummer (Swedish legal entities)
        /// </summary>
        [EnumStringValue("0007")]
        OrganisationsnummerSwedishLegalEntities = 0007,

        /// <summary>
        /// SIRET-CODE
        /// </summary>
        [EnumStringValue("0009")]
        SiretCode = 0009,

        /// <summary>
        /// LY-tunnus
        /// </summary>
        [EnumStringValue("0037")]
        LyTunnus = 0037,

        /// <summary>
        /// Data Universal Numbering System (D-U-N-S Number)
        /// </summary>
        [EnumStringValue("0060")]
        DataUniversalNumberingSystemDUNSNumber = 0060,

        /// <summary>
        /// EAN Location Code
        /// </summary>
        [EnumStringValue("0088")]
        EanLocationCode = 0088,

        /// <summary>
        /// DANISH CHAMBER OF COMMERCE Scheme (EDIRA compliant)
        /// </summary>
        [EnumStringValue("0096")]
        DanishChamberOfCommerceSchemeEdiraCompliant = 0096,

        /// <summary>
        /// FTI - Ediforum Italia, (EDIRA compliant)
        /// </summary>
        [EnumStringValue("0097")]
        FtiEdiforumItaliaEdiraCompliant = 0097,

        /// <summary>
        /// Vereniging van Kamers van Koophandel en Fabrieken in Nederland (Association of Chambers of Commerce and Industry in the Netherlands), Scheme (EDIRA compliant)
        /// </summary>
        [EnumStringValue("0106")]
        VerenigingVanKamersVanKoophandelEnFabriekenInNederlandAssociationOfChambersOfCommerceAndIndustryInTheNetherlandsSchemeEdiraCompliant = 0106,

        /// <summary>
        /// Directorates of the European Commission
        /// </summary>
        [EnumStringValue("0130")]
        DirectoratesOfTheEuropeanCommission = 0130,

        /// <summary>
        /// SIA Object Identifiers
        /// </summary>
        [EnumStringValue("0135")]
        SiaObjectIdentifiers = 0135,

        /// <summary>
        /// SECETI Object Identifiers
        /// </summary>
        [EnumStringValue("0142")]
        SecetiObjectIdentifiers = 0142,

        /// <summary>
        /// Australian Business Number (ABN) Scheme
        /// </summary>
        [EnumStringValue("0151")]
        AustralianBusinessNumberAbnScheme = 0151,

        /// <summary>
        /// Numéro d'identification suisse des enterprises (IDE), Swiss Unique Business Identification Number (UIDB)
        /// </summary>
        [EnumStringValue("0183")]
        NumRoDIdentificationSuisseDesEnterprisesIdeSwissUniqueBusinessIdentificationNumberUidb = 0183,

        /// <summary>
        /// DIGSTORG
        /// </summary>
        [EnumStringValue("0184")]
        Digstorg = 0184,

        /// <summary>
        /// Corporate Number of The Social Security and Tax Number System
        /// </summary>
        [EnumStringValue("0188")]
        CorporateNumberOfTheSocialSecurityAndTaxNumberSystem = 0188,

        /// <summary>
        /// Dutch Originator's Identification Number
        /// </summary>
        [EnumStringValue("0190")]
        DutchOriginatorSIdentificationNumber = 0190,

        /// <summary>
        /// Centre of Registers and Information Systems of the Ministry of Justice
        /// </summary>
        [EnumStringValue("0191")]
        CentreOfRegistersAndInformationSystemsOfTheMinistryOfJustice = 0191,

        /// <summary>
        /// Enhetsregisteret ved Bronnoysundregisterne
        /// </summary>
        [EnumStringValue("0192")]
        EnhetsregisteretVedBronnoysundregisterne = 0192,

        /// <summary>
        /// UBL.BE party identifier
        /// </summary>
        [EnumStringValue("0193")]
        UblBePartyIdentifier = 0193,

        /// <summary>
        /// Singapore UEN identifier
        /// </summary>
        [EnumStringValue("0195")]
        SingaporeUenIdentifier = 0195,

        /// <summary>
        /// Kennitala - Iceland legal id for individuals and legal entities
        /// </summary>
        [EnumStringValue("0196")]
        KennitalaIcelandLegalIdForIndividualsAndLegalEntities = 0196,

        /// <summary>
        /// ERSTORG
        /// </summary>
        [EnumStringValue("0198")]
        Erstorg = 0198,

        /// <summary>
        /// Legal Entity Identifier (LEI)
        /// </summary>
        [EnumStringValue("0199")]
        LegalEntityIdentifierLei = 0199,

        /// <summary>
        /// Legal entity code (Lithuania)
        /// </summary>
        [EnumStringValue("0200")]
        LegalEntityCodeLithuania = 0200,

        /// <summary>
        /// Codice Univoco Unità Organizzativa iPA
        /// </summary>
        [EnumStringValue("0201")]
        CodiceUnivocoUnitOrganizzativaIpa = 0201,

        /// <summary>
        /// Indirizzo di Posta Elettronica Certificata
        /// </summary>
        [EnumStringValue("0202")]
        IndirizzoDiPostaElettronicaCertificata = 0202,

        /// <summary>
        /// Leitweg-ID
        /// </summary>
        [EnumStringValue("0204")]
        LeitwegID = 0204,

        /// <summary>
        /// Numero d'entreprise / ondernemingsnummer / Unternehmensnummer
        /// </summary>
        [EnumStringValue("0208")]
        NumeroDEntrepriseOndernemingsnummerUnternehmensnummer = 0208,

        /// <summary>
        /// GS1 identification keys
        /// </summary>
        [EnumStringValue("0209")]
        Gs1IdentificationKeys = 0209,

        /// <summary>
        /// CODICE FISCALE
        /// </summary>
        [EnumStringValue("0210")]
        CodiceFiscale = 0210,

        /// <summary>
        /// PARTITA IVA
        /// </summary>
        [EnumStringValue("0211")]
        PartitaIva = 0211,

        /// <summary>
        /// Finnish Organization Identifier
        /// </summary>
        [EnumStringValue("0212")]
        FinnishOrganizationIdentifier = 0212,

        /// <summary>
        /// Finnish Organization Value Add Tax Identifier
        /// </summary>
        [EnumStringValue("0213")]
        FinnishOrganizationValueAddTaxIdentifier = 0213,

        /// <summary>
        /// Net service ID
        /// </summary>
        [EnumStringValue("0215")]
        NetServiceId = 0215,

        /// <summary>
        /// OVTcode
        /// </summary>
        [EnumStringValue("0216")]
        Ovtcode = 0216,

        /// <summary>
        /// Unified registration number (Latvia)
        /// </summary>
        [EnumStringValue("0218")]
        UnifiedRegistrationNumberLatvia = 0218,

        /// <summary>
        /// The registered number of the qualified invoice issuer (Japan)
        /// </summary>
        [EnumStringValue("0221")]
        TheRegisteredNumberOfTheQualifiedInvoiceIssuerJapan = 0221,

        /// <summary>
        /// National e-Invoicing Framework (Malaysia)
        /// </summary>
        [EnumStringValue("0230")]
        NationalEInvoicingFrameworkMalaysia = 0230,

        /// <summary>
        /// Danish Ministry of the Interior and Health
        /// </summary>
        [EnumStringValue("9901")]
        DanishMinistryOfTheInteriorAndHealth = 9901,

        /// <summary>
        /// Hungary VAT number
        /// </summary>
        [EnumStringValue("9910")]
        HungaryVatNumber = 9910,

        /// <summary>
        /// Business Registers Network
        /// </summary>
        [EnumStringValue("9913")]
        BusinessRegistersNetwork = 9913,

        /// <summary>
        /// Austria VAT number
        /// </summary>
        [EnumStringValue("9914")]
        AustriaVatNumber = 9914,

        /// <summary>
        /// Österreichisches Verwaltungs bzw. Organisationskennzeichen
        /// </summary>
        [EnumStringValue("9915")]
        OesterreichischesVerwaltungsBzwOrganisationskennzeichen = 9915,

        /// <summary>
        /// SOCIETY FOR WORLDWIDE INTERBANK FINANCIAL, TELECOMMUNICATION S.W.I.F.T
        /// </summary>
        [EnumStringValue("9918")]
        SocietyForWorldwideInterbankFinancialTelecommunicationSWIFT = 9918,

        /// <summary>
        /// Kennziffer des Unternehmensregisters
        /// </summary>
        [EnumStringValue("9919")]
        KennzifferDesUnternehmensregisters = 9919,

        /// <summary>
        /// Agencia Española de Administración Tributaria
        /// </summary>
        [EnumStringValue("9920")]
        AgenciaEspaOlaDeAdministraciNTributaria = 9920,

        /// <summary>
        /// Andorra VAT number
        /// </summary>
        [EnumStringValue("9922")]
        AndorraVatNumber = 9922,

        /// <summary>
        /// Albania VAT number
        /// </summary>
        [EnumStringValue("9923")]
        AlbaniaVatNumber = 9923,

        /// <summary>
        /// Bosnia and Herzegovina VAT number
        /// </summary>
        [EnumStringValue("9924")]
        BosniaAndHerzegovinaVatNumber = 9924,

        /// <summary>
        /// Belgium VAT number
        /// </summary>
        [EnumStringValue("9925")]
        BelgiumVatNumber = 9925,

        /// <summary>
        /// Bulgaria VAT number
        /// </summary>
        [EnumStringValue("9926")]
        BulgariaVatNumber = 9926,

        /// <summary>
        /// Switzerland VAT number
        /// </summary>
        [EnumStringValue("9927")]
        SwitzerlandVatNumber = 9927,

        /// <summary>
        /// Cyprus VAT number
        /// </summary>
        [EnumStringValue("9928")]
        CyprusVatNumber = 9928,

        /// <summary>
        /// Czech Republic VAT number
        /// </summary>
        [EnumStringValue("9929")]
        CzechRepublicVatNumber = 9929,

        /// <summary>
        /// Germany VAT number
        /// </summary>
        [EnumStringValue("9930")]
        GermanyVatNumber = 9930,

        /// <summary>
        /// Estonia VAT number
        /// </summary>
        [EnumStringValue("9931")]
        EstoniaVatNumber = 9931,

        /// <summary>
        /// United Kingdom VAT number
        /// </summary>
        [EnumStringValue("9932")]
        UnitedKingdomVatNumber = 9932,

        /// <summary>
        /// Greece VAT number
        /// </summary>
        [EnumStringValue("9933")]
        GreeceVatNumber = 9933,

        /// <summary>
        /// Croatia VAT number
        /// </summary>
        [EnumStringValue("9934")]
        CroatiaVatNumber = 9934,

        /// <summary>
        /// Ireland VAT number
        /// </summary>
        [EnumStringValue("9935")]
        IrelandVatNumber = 9935,

        /// <summary>
        /// Liechtenstein VAT number
        /// </summary>
        [EnumStringValue("9936")]
        LiechtensteinVatNumber = 9936,

        /// <summary>
        /// Lithuania VAT number
        /// </summary>
        [EnumStringValue("9937")]
        LithuaniaVatNumber = 9937,

        /// <summary>
        /// Luxemburg VAT number
        /// </summary>
        [EnumStringValue("9938")]
        LuxemburgVatNumber = 9938,

        /// <summary>
        /// Latvia VAT number
        /// </summary>
        [EnumStringValue("9939")]
        LatviaVatNumber = 9939,

        /// <summary>
        /// Monaco VAT number
        /// </summary>
        [EnumStringValue("9940")]
        MonacoVatNumber = 9940,

        /// <summary>
        /// Montenegro VAT number
        /// </summary>
        [EnumStringValue("9941")]
        MontenegroVatNumber = 9941,

        /// <summary>
        /// Macedonia, of the former Yugoslav Republic VAT number
        /// </summary>
        [EnumStringValue("9942")]
        MacedoniaVatNumber = 9942,

        /// <summary>
        /// Malta VAT number
        /// </summary>
        [EnumStringValue("9943")]
        MaltaVatNumber = 9943,

        /// <summary>
        /// Netherlands VAT number
        /// </summary>
        [EnumStringValue("9944")]
        NetherlandsVatNumber = 9944,

        /// <summary>
        /// Poland VAT number
        /// </summary>
        [EnumStringValue("9945")]
        PolandVatNumber = 9945,

        /// <summary>
        /// Portugal VAT number
        /// </summary>
        [EnumStringValue("9946")]
        PortugalVatNumber = 9946,

        /// <summary>
        /// Romania VAT number
        /// </summary>
        [EnumStringValue("9947")]
        RomaniaVatNumber = 9947,

        /// <summary>
        /// Serbia VAT number
        /// </summary>
        [EnumStringValue("9948")]
        SerbiaVatNumber = 9948,

        /// <summary>
        /// Slovenia VAT number
        /// </summary>
        [EnumStringValue("9949")]
        SloveniaVatNumber = 9949,

        /// <summary>
        /// Slovakia VAT number
        /// </summary>
        [EnumStringValue("9950")]
        SlovakiaVatNumber = 9950,

        /// <summary>
        /// San Marino VAT number
        /// </summary>
        [EnumStringValue("9951")]
        SanMarinoVatNumber = 9951,

        /// <summary>
        /// Turkey VAT number
        /// </summary>
        [EnumStringValue("9952")]
        TurkeyVatNumber = 9952,

        /// <summary>
        /// Holy See (Vatican City State) VAT number
        /// </summary>
        [EnumStringValue("9953")]
        HolySeeVatNumber = 9953,

        /// <summary>
        /// Swedish VAT number
        /// </summary>
        [EnumStringValue("9955")]
        SwedishVatNumber = 9955,

        /// <summary>
        /// Belgian Crossroad Bank of Enterprises
        /// </summary>
        [EnumStringValue("9956")]
        BelgianCrossroad = 9956,

        /// <summary>
        /// French VAT number
        /// </summary>
        [EnumStringValue("9957")]
        FrenchVatNumber = 9957,

        /// <summary>
        /// German Leitweg ID
        /// </summary>
        [EnumStringValue("9958")]
        GermanLeitwegID = 9958,

        /// <summary>
        /// Employer Identification Number (EIN, USA)
        /// </summary>
        [EnumStringValue("9959")]
        EmployerIdentificationNumber = 9959,

        /// <summary>
        /// O.F.T.P. (ODETTE File Transfer Protocol)
        /// </summary>
        [EnumStringValue("AN")]
        AN,

        /// <summary>
        /// X.400 address for mail text
        /// </summary>
        [EnumStringValue("AQ")]
        AQ,  // ohne int-Zuweisung

        /// <summary>
        /// AS2 exchange 
        /// </summary>
        [EnumStringValue("AS")]
        AS,  // ohne int-Zuweisung

        /// <summary>
        /// File Transfer Protocol
        /// </summary>
        [EnumStringValue("AU")]
        AU,  // ohne int-Zuweisung

        /// <summary>
        /// Electronic mail
        /// </summary>
        [EnumStringValue("EM")]
        EM
    }
}
