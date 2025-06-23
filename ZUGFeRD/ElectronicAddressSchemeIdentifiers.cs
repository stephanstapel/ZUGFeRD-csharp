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
    /// Represents standardized Electronic Address Scheme (EAS) identifiers used in electronic business documents.
    /// These identifiers define the format and issuing authority for electronic addresses and business identifiers
    /// in B2B communications, particularly in e-invoicing and electronic data interchange (EDI) systems.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Electronic Address Scheme identifiers are standardized codes that specify how to interpret
    /// electronic addresses and business identifiers in electronic documents. Each identifier consists
    /// of a unique code that references a specific numbering scheme or authority.
    /// </para>
    /// <para>
    /// This enumeration includes:
    /// - International business registration numbers (DUNS, GLEIF, etc.)
    /// - National business identifiers (SIRET, ABN, etc.)
    /// - VAT identification numbers for European and international jurisdictions
    /// - Electronic communication protocol identifiers (AS2, SMTP, FTP, etc.)
    /// </para>
    /// <para>
    /// The codes are categorized as follows:
    /// - 0002-0240: International and national business identifiers
    /// - 9910-9959: VAT numbers and tax identification schemes
    /// - AN, AQ, AS, AU, EM: Electronic communication protocols
    /// </para>
    /// <para>
    /// These identifiers are commonly used in:
    /// - PEPPOL e-invoicing infrastructure
    /// - UN/CEFACT Cross Industry Invoice (CII) format
    /// - UBL (Universal Business Language) documents
    /// - Electronic procurement systems
    /// - B2B integration platforms
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Usage example for electronic address identification
    /// var scheme = ElectronicAddressSchemeIdentifiers.DunsNumber;
    /// var schemeCode = scheme.GetEnumStringValue(); // Returns "0060"
    /// 
    /// // For VAT number identification
    /// var vatScheme = ElectronicAddressSchemeIdentifiers.GermanyVatNumber;
    /// var vatCode = vatScheme.GetEnumStringValue(); // Returns "9930"
    /// </code>
    /// </example>
    /// <seealso href="https://docs.peppol.eu/poacc/billing/3.0/syntax/ubl-invoice/cac-AccountingSupplierParty/cac-Party/cbc-EndpointID/">PEPPOL BIS Billing 3.0 Endpoint ID</seealso>
    /// <seealso href="https://unece.org/trade/uncefact/cl-recommendations">UN/CEFACT Code List Recommendations</seealso>
    public enum ElectronicAddressSchemeIdentifiers
    {
        /// <summary>
        /// System Information et Repertoire des Entreprise et des Etablissements: SIRENE
        /// </summary>
        [EnumStringValue("0002")]
        Sirene,

        /// <summary>
        /// Organisationsnummer
        /// </summary>
        [EnumStringValue("0007")]
        Organisationsnummer,

        /// <summary>
        /// SIRET-CODE
        /// </summary>
        [EnumStringValue("0009")]
        SiretCode,

        /// <summary>
        /// LY-tunnus
        /// </summary>
        [EnumStringValue("0037")]
        LyTunnus,

        /// <summary>
        /// Data Universal Numbering System (D-U-N-S Number)
        /// </summary>
        [EnumStringValue("0060")]
        DunsNumber,

        /// <summary>
        /// EAN Location Code
        /// </summary>
        [EnumStringValue("0088")]
        EanLocationCode,

        /// <summary>
        /// The Danish Business Authority - P-number (DK:P)
        /// </summary>
        [EnumStringValue("0096")]
        DanishBusinessAuthorityPNumber,

        /// <summary>
        /// FTI - Ediforum Italia, (EDIRA compliant)
        /// </summary>
        [EnumStringValue("0097")]
        FtiEdiforumItalia,

        /// <summary>
        /// Vereniging van Kamers van Koophandel en Fabrieken in Nederland (Association of Chambers of Commerce and Industry in the Netherlands), Scheme (EDIRA compliant)
        /// </summary>
        [EnumStringValue("0106")]
        DutchChamberOfCommerce,

        /// <summary>
        /// Directorates of the European Commission
        /// </summary>
        [EnumStringValue("0130")]
        EuropeanCommissionDirectorates,

        /// <summary>
        /// SIA Object Identifiers
        /// </summary>
        [EnumStringValue("0135")]
        SiaObjectIdentifiers,

        /// <summary>
        /// SECETI Object Identifiers
        /// </summary>
        [EnumStringValue("0142")]
        SecetiObjectIdentifiers,

        /// <summary>
        /// Standard Company Code
        /// </summary>
        [EnumStringValue("0147")]
        StandardCompanyCode,

        /// <summary>
        /// Australian Business Number (ABN) Scheme
        /// </summary>
        [EnumStringValue("0151")]
        AustralianBusinessNumberAbn,

        /// <summary>
        /// Identification number of economic subjects: (ICO)
        /// </summary>
        [EnumStringValue("0154")]
        EconomicSubjectNumberIco,

        /// <summary>
        /// Identification number of economic subject (ICO) Act on State Statistics of 29 November 2001, § 27
        /// </summary>
        [EnumStringValue("0158")]
        EconomicSubjectNumberAct,

        /// <summary>
        /// Teikoku Company Code
        /// </summary>
        [EnumStringValue("0170")]
        TeikokuCompanyCode,

        /// <summary>
        /// Odette International Limited
        /// </summary>
        [EnumStringValue("0177")]
        OdetteInternationalLimited,

        /// <summary>
        /// Numéro d'identification suisse des enterprises (IDE), Swiss Unique Business Identification Number (UIDB)
        /// </summary>
        [EnumStringValue("0183")]
        SwissUidb,

        /// <summary>
        /// DIGSTORG
        /// </summary>
        [EnumStringValue("0184")]
        Digstorg,

        /// <summary>
        /// Corporate Number of The Social Security and Tax Number System
        /// </summary>
        [EnumStringValue("0188")]
        SocialSecurityTaxNumberSystem,

        /// <summary>
        /// Dutch Originator's Identification Number
        /// </summary>
        [EnumStringValue("0190")]
        DutchOriginatorsIdentificationNumber,

        /// <summary>
        /// Centre of Registers and Information Systems of the Ministry of Justice
        /// </summary>
        [EnumStringValue("0191")]
        JusticeRegister,

        /// <summary>
        /// Enhetsregisteret ved Bronnoysundregisterne
        /// </summary>
        [EnumStringValue("0192")]
        Bronnoysundregisterne,

        /// <summary>
        /// UBL.BE party identifier
        /// </summary>
        [EnumStringValue("0193")]
        UblBePartyIdentifier,

        /// <summary>
        /// KOIOS Open Technical Dictionary
        /// </summary>
        [EnumStringValue("0194")]
        KoiosOpenTechnicalDictionary,

        /// <summary>
        /// Singapore UEN identifier
        /// </summary>
        [EnumStringValue("0195")]
        SingaporeUenIdentifier,

        /// <summary>
        /// Kennitala - Iceland legal id for individuals and legal entities
        /// </summary>
        [EnumStringValue("0196")]
        KennitalaIceland,

        /// <summary>
        /// ERSTORG
        /// </summary>
        [EnumStringValue("0198")]
        Erstorg,

        /// <summary>
        /// Global legal entity identifier (GLEIF)
        /// </summary>
        [EnumStringValue("0199")]
        GlobalLegalEntityIdentifierGleif,

        /// <summary>
        /// Legal entity code (Lithuania)
        /// </summary>
        [EnumStringValue("0200")]
        LegalEntityCodeLithuania,

        /// <summary>
        /// Codice Univoco Unità Organizzativa iPA
        /// </summary>
        [EnumStringValue("0201")]
        CodiceUnivocoIpa,

        /// <summary>
        /// Indirizzo di Posta Elettronica Certificata
        /// </summary>
        [EnumStringValue("0202")]
        PostaElettronicaCertificata,

        /// <summary>
        /// eDelivery Network Participant identifier
        /// </summary>
        [EnumStringValue("0203")]
        EDeliveryNetworkParticipant,

        /// <summary>
        /// Leitweg-ID
        /// </summary>
        [EnumStringValue("0204")]
        LeitwegId,

        /// <summary>
        /// CODDEST
        /// </summary>
        [EnumStringValue("0205")]
        Coddest,

        /// <summary>
        /// Numero d'entreprise / ondernemingsnummer / Unternehmensnummer
        /// </summary>
        [EnumStringValue("0208")]
        Unternehmensnummer,

        /// <summary>
        /// GS1 identification keys
        /// </summary>
        [EnumStringValue("0209")]
        Gs1IdentificationKeys,

        /// <summary>
        /// CODICE FISCALE
        /// </summary>
        [EnumStringValue("0210")]
        CodiceFiscale,

        /// <summary>
        /// PARTITA IVA
        /// </summary>
        [EnumStringValue("0211")]
        PartitaIva,

        /// <summary>
        /// Finnish Organization Identifier
        /// </summary>
        [EnumStringValue("0212")]
        FinnishOrganizationIdentifier,

        /// <summary>
        /// Finnish Organization Value Add Tax Identifier
        /// </summary>
        [EnumStringValue("0213")]
        FinnishOrganizationVatIdentifier,

        /// <summary>
        /// Net service ID
        /// </summary>
        [EnumStringValue("0215")]
        NetServiceId,

        /// <summary>
        /// OVTCode
        /// </summary>
        [EnumStringValue("0216")]
        OvtCode,

        /// <summary>
        /// The Netherlands Chamber of Commerce and Industry establishment number
        /// </summary>
        [EnumStringValue("0217")]
        NetherlandsChamberOfCommerceNumber,

        /// <summary>
        /// Unified registration number (Latvia)
        /// </summary>
        [EnumStringValue("0218")]
        UnifiedRegistrationNumberLatvia,

        /// <summary>
        /// The registered number of the qualified invoice issuer
        /// </summary>
        [EnumStringValue("0221")]
        QualifiedInvoiceIssuerNumber,

        /// <summary>
        /// FRCTC ELECTRONIC ADDRESS
        /// </summary>
        [EnumStringValue("0225")]
        FrctcElectronicAddress,

        /// <summary>
        /// National e-Invoicing Framework
        /// </summary>
        [EnumStringValue("0230")]
        NationalEInvoicingFramework,

        /// <summary>
        /// UAE Tax Identification Number (TIN)
        /// </summary>
        [EnumStringValue("0235")]
        UaeTaxIdentificationNumberTin,

        /// <summary>
        /// Register of legal persons (in French : Répertoire des personnes morales)
        /// </summary>
        [EnumStringValue("0240")]
        RegisterOfLegalPersons,

        /// <summary>
        /// Hungary VAT number
        /// </summary>
        [EnumStringValue("9910")]
        HungaryVatNumber,

        /// <summary>
        /// Business Registers Network
        /// </summary>
        [EnumStringValue("9913")]
        BusinessRegistersNetwork,

        /// <summary>
        /// Österreichische Umsatzsteuer-Identifikationsnummer
        /// </summary>
        [EnumStringValue("9914")]
        AustrianVatNumber,

        /// <summary>
        /// Österreichisches Verwaltungs bzw. Organisationskennzeichen
        /// </summary>
        [EnumStringValue("9915")]
        AustrianAdministrativeCode,

        /// <summary>
        /// SOCIETY FOR WORLDWIDE INTERBANK FINANCIAL, TELECOMMUNICATION S.W.I.F.T
        /// </summary>
        [EnumStringValue("9918")]
        Swift,

        /// <summary>
        /// Kennziffer des Unternehmensregisters
        /// </summary>
        [EnumStringValue("9919")]
        CompanyRegisterNumber,

        /// <summary>
        /// Agencia Española de Administración Tributaria
        /// </summary>
        [EnumStringValue("9920")]
        SpanishTaxAgency,

        /// <summary>
        /// Andorra VAT number
        /// </summary>
        [EnumStringValue("9922")]
        AndorraVatNumber,

        /// <summary>
        /// Albania VAT number
        /// </summary>
        [EnumStringValue("9923")]
        AlbaniaVatNumber,

        /// <summary>
        /// Bosnia and Herzegovina VAT number
        /// </summary>
        [EnumStringValue("9924")]
        BosniaAndHerzegovinaVatNumber,

        /// <summary>
        /// Belgium VAT number
        /// </summary>
        [EnumStringValue("9925")]
        BelgiumVatNumber,

        /// <summary>
        /// Bulgaria VAT number
        /// </summary>
        [EnumStringValue("9926")]
        BulgariaVatNumber,

        /// <summary>
        /// Switzerland VAT number
        /// </summary>
        [EnumStringValue("9927")]
        SwitzerlandVatNumber,

        /// <summary>
        /// Cyprus VAT number
        /// </summary>
        [EnumStringValue("9928")]
        CyprusVatNumber,

        /// <summary>
        /// Czech Republic VAT number
        /// </summary>
        [EnumStringValue("9929")]
        CzechRepublicVatNumber,

        /// <summary>
        /// Germany VAT number
        /// </summary>
        [EnumStringValue("9930")]
        GermanyVatNumber,

        /// <summary>
        /// Estonia VAT number
        /// </summary>
        [EnumStringValue("9931")]
        EstoniaVatNumber,

        /// <summary>
        /// United Kingdom VAT number
        /// </summary>
        [EnumStringValue("9932")]
        UnitedKingdomVatNumber,

        /// <summary>
        /// Greece VAT number
        /// </summary>
        [EnumStringValue("9933")]
        GreeceVatNumber,

        /// <summary>
        /// Croatia VAT number
        /// </summary>
        [EnumStringValue("9934")]
        CroatiaVatNumber,

        /// <summary>
        /// Ireland VAT number
        /// </summary>
        [EnumStringValue("9935")]
        IrelandVatNumber,

        /// <summary>
        /// Liechtenstein VAT number
        /// </summary>
        [EnumStringValue("9936")]
        LiechtensteinVatNumber,

        /// <summary>
        /// Lithuania VAT number
        /// </summary>
        [EnumStringValue("9937")]
        LithuaniaVatNumber,

        /// <summary>
        /// Luxemburg VAT number
        /// </summary>
        [EnumStringValue("9938")]
        LuxemburgVatNumber,

        /// <summary>
        /// Latvia VAT number
        /// </summary>
        [EnumStringValue("9939")]
        LatviaVatNumber,

        /// <summary>
        /// Monaco VAT number
        /// </summary>
        [EnumStringValue("9940")]
        MonacoVatNumber,

        /// <summary>
        /// Montenegro VAT number
        /// </summary>
        [EnumStringValue("9941")]
        MontenegroVatNumber,

        /// <summary>
        /// Macedonia, the former Yugoslav Republic of VAT number
        /// </summary>
        [EnumStringValue("9942")]
        MacedoniaVatNumber,

        /// <summary>
        /// Malta VAT number
        /// </summary>
        [EnumStringValue("9943")]
        MaltaVatNumber,

        /// <summary>
        /// Netherlands VAT number
        /// </summary>
        [EnumStringValue("9944")]
        NetherlandsVatNumber,

        /// <summary>
        /// Poland VAT number
        /// </summary>
        [EnumStringValue("9945")]
        PolandVatNumber,

        /// <summary>
        /// Portugal VAT number
        /// </summary>
        [EnumStringValue("9946")]
        PortugalVatNumber,

        /// <summary>
        /// Romania VAT number
        /// </summary>
        [EnumStringValue("9947")]
        RomaniaVatNumber,

        /// <summary>
        /// Serbia VAT number
        /// </summary>
        [EnumStringValue("9948")]
        SerbiaVatNumber,

        /// <summary>
        /// Slovenia VAT number
        /// </summary>
        [EnumStringValue("9949")]
        SloveniaVatNumber,

        /// <summary>
        /// Slovakia VAT number
        /// </summary>
        [EnumStringValue("9950")]
        SlovakiaVatNumber,

        /// <summary>
        /// San Marino VAT number
        /// </summary>
        [EnumStringValue("9951")]
        SanMarinoVatNumber,

        /// <summary>
        /// Turkey VAT number
        /// </summary>
        [EnumStringValue("9952")]
        TurkeyVatNumber,

        /// <summary>
        /// Holy See (Vatican City State) VAT number
        /// </summary>
        [EnumStringValue("9953")]
        VaticanVatNumber,

        /// <summary>
        /// French VAT number
        /// </summary>
        [EnumStringValue("9957")]
        FrenchVatNumber,

        /// <summary>
        /// Employer Identification Number (EIN, USA)
        /// </summary>
        [EnumStringValue("9959")]
        EmployerIdentificationNumber,

        /// <summary>
        /// O.F.T.P. (ODETTE File Transfer Protocol)
        /// </summary>
        [EnumStringValue("AN")]
        OdetteFileTransferProtocol,

        /// <summary>
        /// X.400 address for mail text
        /// </summary>
        [EnumStringValue("AQ")]
        X400AddressForMailText,

        /// <summary>
        /// AS2 exchange
        /// </summary>
        [EnumStringValue("AS")]
        As2Exchange,

        /// <summary>
        /// File Transfer Protocol
        /// </summary>
        [EnumStringValue("AU")]
        FileTransferProtocol,

        /// <summary>
        /// Electronic mail (SMTP)
        /// </summary>
        [EnumStringValue("EM")]
        ElectronicMailSmtp,
    }
}
