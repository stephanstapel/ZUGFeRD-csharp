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
using System.Linq.Expressions;
using System.Text;

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
        SystemInformationEtRepertoireDesEntrepriseEtDesEtablissementsSirene = 0002,

        /// <summary>
        /// Organisationsnummer (Swedish legal entities)
        /// </summary>
        OrganisationsnummerSwedishLegalEntities = 0007,

        /// <summary>
        /// SIRET-CODE
        /// </summary>
        SiretCode = 0009,

        /// <summary>
        /// LY-tunnus
        /// </summary>
        LyTunnus = 0037,

        /// <summary>
        /// Data Universal Numbering System (D-U-N-S Number)
        /// </summary>
        DataUniversalNumberingSystemDUNSNumber = 0060,

        /// <summary>
        /// EAN Location Code
        /// </summary>
        EanLocationCode = 0088,

        /// <summary>
        /// DANISH CHAMBER OF COMMERCE Scheme (EDIRA compliant)
        /// </summary>
        DanishChamberOfCommerceSchemeEdiraCompliant = 0096,

        /// <summary>
        /// FTI - Ediforum Italia, (EDIRA compliant)
        /// </summary>
        FtiEdiforumItaliaEdiraCompliant = 0097,

        /// <summary>
        /// "Vereniging van Kamers van Koophandel en Fabrieken in Nederland (Association of Chambers of Commerce and Industry in the Netherlands), Scheme (EDIRA compliant)"
        /// </summary>
        VerenigingVanKamersVanKoophandelEnFabriekenInNederlandAssociationOfChambersOfCommerceAndIndustryInTheNetherlandsSchemeEdiraCompliant = 0106,

        /// <summary>
        /// Directorates of the European Commission
        /// </summary>
        DirectoratesOfTheEuropeanCommission = 0130,

        /// <summary>
        /// SIA Object Identifiers
        /// </summary>
        SiaObjectIdentifiers = 0135,

        /// <summary>
        /// SECETI Object Identifiers
        /// </summary>
        SecetiObjectIdentifiers = 0142,

        /// <summary>
        /// Australian Business Number (ABN) Scheme
        /// </summary>
        AustralianBusinessNumberAbnScheme = 0151,

        /// <summary>
        /// Numéro d'identification suisse des enterprises (IDE), Swiss Unique Business Identification Number (UIDB)
        /// </summary>
        NumRoDIdentificationSuisseDesEnterprisesIdeSwissUniqueBusinessIdentificationNumberUidb = 0183,

        /// <summary>
        /// DIGSTORG
        /// </summary>
        Digstorg = 0184,

        /// <summary>
        /// Corporate Number of The Social Security and Tax Number System
        /// </summary>
        CorporateNumberOfTheSocialSecurityAndTaxNumberSystem = 0188,

        /// <summary>
        /// Dutch Originator's Identification Number
        /// </summary>
        DutchOriginatorSIdentificationNumber = 0190,

        /// <summary>
        /// Centre of Registers and Information Systems of the Ministry of Justice
        /// </summary>
        CentreOfRegistersAndInformationSystemsOfTheMinistryOfJustice = 0191,

        /// <summary>
        /// Enhetsregisteret ved Bronnoysundregisterne
        /// </summary>
        EnhetsregisteretVedBronnoysundregisterne = 0192,

        /// <summary>
        /// UBL.BE party identifier
        /// </summary>
        UblBePartyIdentifier = 0193,

        /// <summary>
        /// Singapore UEN identifier
        /// </summary>
        SingaporeUenIdentifier = 0195,

        /// <summary>
        /// Kennitala - Iceland legal id for individuals and legal entities
        /// </summary>
        KennitalaIcelandLegalIdForIndividualsAndLegalEntities = 0196,

        /// <summary>
        /// ERSTORG
        /// </summary>
        Erstorg = 0198,

        /// <summary>
        /// Legal Entity Identifier (LEI)
        /// </summary>
        LegalEntityIdentifierLei = 0199,

        /// <summary>
        /// Legal entity code (Lithuania)
        /// </summary>
        LegalEntityCodeLithuania = 0200,

        /// <summary>
        /// Codice Univoco Unità Organizzativa iPA
        /// </summary>
        CodiceUnivocoUnitOrganizzativaIpa = 0201,

        /// <summary>
        /// Indirizzo di Posta Elettronica Certificata
        /// </summary>
        IndirizzoDiPostaElettronicaCertificata = 0202,

        /// <summary>
        /// Leitweg-ID
        /// </summary>
        LeitwegID = 0204,

        /// <summary>
        /// Numero d'entreprise / ondernemingsnummer / Unternehmensnummer
        /// </summary>
        NumeroDEntrepriseOndernemingsnummerUnternehmensnummer = 0208,

        /// <summary>
        /// GS1 identification keys
        /// </summary>
        Gs1IdentificationKeys = 0209,

        /// <summary>
        /// CODICE FISCALE
        /// </summary>
        CodiceFiscale = 0210,

        /// <summary>
        /// PARTITA IVA
        /// </summary>
        PartitaIva = 0211,

        /// <summary>
        /// Finnish Organization Identifier
        /// </summary>
        FinnishOrganizationIdentifier = 0212,

        /// <summary>
        /// Finnish Organization Value Add Tax Identifier
        /// </summary>
        FinnishOrganizationValueAddTaxIdentifier = 0213,

        /// <summary>
        /// Net service ID
        /// </summary>
        NetServiceId = 0215,

        /// <summary>
        /// OVTcode
        /// </summary>
        Ovtcode = 0216,

        /// <summary>
        /// Unified registration number (Latvia)
        /// </summary>
        UnifiedRegistrationNumberLatvia = 0218,

        /// <summary>
        /// The registered number of the qualified invoice issuer (Japan)
        /// </summary>
        TheRegisteredNumberOfTheQualifiedInvoiceIssuerJapan = 0221,

        /// <summary>
        /// National e-Invoicing Framework (Malaysia)
        /// </summary>
        NationalEInvoicingFrameworkMalaysia = 0230,

        /// <summary>
        /// Danish Ministry of the Interior and Health
        /// </summary>
        DanishMinistryOfTheInteriorAndHealth = 9901,

        /// <summary>
        /// Hungary VAT number
        /// </summary>
        HungaryVatNumber = 9910,

        /// <summary>
        /// Business Registers Network
        /// </summary>
        BusinessRegistersNetwork = 9913,

        /// <summary>
        /// Austria VAT number
        /// </summary>
        AustriaVatNumber = 9914,

        /// <summary>
        /// "Österreichisches Verwaltungs bzw. Organisationskennzeichen"
        /// </summary>
        OesterreichischesVerwaltungsBzwOrganisationskennzeichen = 9915,

        /// <summary>
        /// "SOCIETY FOR WORLDWIDE INTERBANK FINANCIAL, TELECOMMUNICATION S.W.I.F.T"
        /// </summary>
        SocietyForWorldwideInterbankFinancialTelecommunicationSWIFT = 9918,

        /// <summary>
        /// Kennziffer des Unternehmensregisters
        /// </summary>
        KennzifferDesUnternehmensregisters = 9919,

        /// <summary>
        /// Agencia Española de Administración Tributaria
        /// </summary>
        AgenciaEspaOlaDeAdministraciNTributaria = 9920,

        /// <summary>
        /// Andorra VAT number
        /// </summary>
        AndorraVatNumber = 9922,

        /// <summary>
        /// Albania VAT number
        /// </summary>
        AlbaniaVatNumber = 9923,

        /// <summary>
        /// Bosnia and Herzegovina VAT number
        /// </summary>
        BosniaAndHerzegovinaVatNumber = 9924,

        /// <summary>
        /// Belgium VAT number
        /// </summary>
        BelgiumVatNumber = 9925,

        /// <summary>
        /// Bulgaria VAT number
        /// </summary>
        BulgariaVatNumber = 9926,

        /// <summary>
        /// Switzerland VAT number
        /// </summary>
        SwitzerlandVatNumber = 9927,

        /// <summary>
        /// Cyprus VAT number
        /// </summary>
        CyprusVatNumber = 9928,

        /// <summary>
        /// Czech Republic VAT number
        /// </summary>
        CzechRepublicVatNumber = 9929,

        /// <summary>
        /// Germany VAT number
        /// </summary>
        GermanyVatNumber = 9930,

        /// <summary>
        /// Estonia VAT number
        /// </summary>
        EstoniaVatNumber = 9931,

        /// <summary>
        /// United Kingdom VAT number
        /// </summary>
        UnitedKingdomVatNumber = 9932,

        /// <summary>
        /// Greece VAT number
        /// </summary>
        GreeceVatNumber = 9933,

        /// <summary>
        /// Croatia VAT number
        /// </summary>
        CroatiaVatNumber = 9934,

        /// <summary>
        /// Ireland VAT number
        /// </summary>
        IrelandVatNumber = 9935,

        /// <summary>
        /// Liechtenstein VAT number
        /// </summary>
        LiechtensteinVatNumber = 9936,

        /// <summary>
        /// Lithuania VAT number
        /// </summary>
        LithuaniaVatNumber = 9937,

        /// <summary>
        /// Luxemburg VAT number
        /// </summary>
        LuxemburgVatNumber = 9938,

        /// <summary>
        /// Latvia VAT number
        /// </summary>
        LatviaVatNumber = 9939,

        /// <summary>
        /// Monaco VAT number
        /// </summary>
        MonacoVatNumber = 9940,

        /// <summary>
        /// Montenegro VAT number
        /// </summary>
        MontenegroVatNumber = 9941,

        /// <summary>
        /// Macedonia, of the former Yugoslav Republic VAT number
        /// </summary>
        MacedoniaVatNumber = 9942,

        /// <summary>
        /// Malta VAT number
        /// </summary>
        MaltaVatNumber = 9943,

        /// <summary>
        /// Netherlands VAT number
        /// </summary>
        NetherlandsVatNumber = 9944,

        /// <summary>
        /// Poland VAT number
        /// </summary>
        PolandVatNumber = 9945,

        /// <summary>
        /// Portugal VAT number
        /// </summary>
        PortugalVatNumber = 9946,

        /// <summary>
        /// Romania VAT number
        /// </summary>
        RomaniaVatNumber = 9947,

        /// <summary>
        /// Serbia VAT number
        /// </summary>
        SerbiaVatNumber = 9948,

        /// <summary>
        /// Slovenia VAT number
        /// </summary>
        SloveniaVatNumber = 9949,

        /// <summary>
        /// Slovakia VAT number
        /// </summary>
        SlovakiaVatNumber = 9950,

        /// <summary>
        /// San Marino VAT number
        /// </summary>
        SanMarinoVatNumber = 9951,

        /// <summary>
        /// Turkey VAT number
        /// </summary>
        TurkeyVatNumber = 9952,

        /// <summary>
        /// Holy See (Vatican City State) VAT number
        /// </summary>
        HolySeeVatNumber = 9953,

        /// <summary>
        /// Swedish VAT number
        /// </summary>
        SwedishVatNumber = 9955,

        /// <summary>
        /// Belgian Crossroad Bank of Enterprises
        /// </summary>
        BelgianCrossroad = 9956,

        /// <summary>
        /// French VAT number
        /// </summary>
        FrenchVatNumber = 9957,

        /// <summary>
        /// German Leitweg ID
        /// </summary>
        GermanLeitwegID = 9958,

        /// <summary>
        /// Employer Identification Number (EIN, USA)
        /// </summary>
        EmployerIdentificationNumber = 9959,

        /// <summary>
        /// O.F.T.P. (ODETTE File Transfer Protocol)
        /// </summary>
        AN,

        /// <summary>
        /// X.400 address for mail text
        /// </summary>
        AQ,

        /// <summary>
        /// AS2 exchange 
        /// </summary>
        AS,

        /// <summary>
        /// File Transfer Protocol
        /// </summary>
        AU,

        /// <summary>
        /// Electronic mail
        /// </summary>
        EM
    }

    internal static class ElectronicAddressSchemeIdentifiersExtensions
    {
        public static ElectronicAddressSchemeIdentifiers? FromString(this ElectronicAddressSchemeIdentifiers _, string s)
        {
            if (Enum.TryParse(s, out ElectronicAddressSchemeIdentifiers eas))
                return eas;
            else
                return null;
        } // !FromString()

        public static string EnumToString(this ElectronicAddressSchemeIdentifiers eas)
        {
            switch (eas)
            {
                case ElectronicAddressSchemeIdentifiers.AN:
                case ElectronicAddressSchemeIdentifiers.AQ:
                case ElectronicAddressSchemeIdentifiers.AS:
                case ElectronicAddressSchemeIdentifiers.AU:
                case ElectronicAddressSchemeIdentifiers.EM:

                    return eas.ToString();
                default:
                    return ((int)eas).ToString("D4");
            }

        } // !ToString()
    }
}
