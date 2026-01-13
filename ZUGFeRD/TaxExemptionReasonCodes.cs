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

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// VATEX code from PEPPOL BIS Billing 3.0 / EN16931 (CEF).
    /// Source : https://docs.peppol.eu/poacc/billing/3.0/codelist/vatex/ (May/Nov 2025 releases).
    /// </summary>
    public enum TaxExemptionReasonCodes
    {
        /// <summary>
        /// Exempt based on article 79, point c of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Exemptions relating to repayment of expenditures. Repayment of expenditure is not an exemption in the sense of the VAT Directive but may be handled as such in the context of the EN16931.
        /// </remarks>
        [EnumStringValue("VATEX-EU-79-C")]
        VATEX_EU_79_C,

        /// <summary>
        /// Exempt based on article 132 of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Exemptions for certain activities in public interest.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132")]
        VATEX_EU_132,

        /// <summary>
        /// Exempt based on article 132, section 1 (a) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply by the public postal services of services other than passenger transport and telecommunications services, and the supply of goods incidental thereto.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1A")]
        VATEX_EU_132_1A,

        /// <summary>
        /// Exempt based on article 132, section 1 (b) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Hospital and medical care and closely related activities undertaken by bodies governed by public law or, under social conditions comparable with those applicable to bodies governed by public law, by hospitals, centres for medical treatment or diagnosis and other duly recognised establishments of a similar nature.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1B")]
        VATEX_EU_132_1B,

        /// <summary>
        /// Exempt based on article 132, section 1 (c) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The provision of medical care in the exercise of the medical and paramedical professions as defined by the Member State concerned.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1C")]
        VATEX_EU_132_1C,

        /// <summary>
        /// Exempt based on article 132, section 1 (d) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of human organs, blood and milk.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1D")]
        VATEX_EU_132_1D,

        /// <summary>
        /// Exempt based on article 132, section 1 (e) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of services by dental technicians in their professional capacity and the supply of dental prostheses by dentists and dental technicians.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1E")]
        VATEX_EU_132_1E,

        /// <summary>
        /// Exempt based on article 132, section 1 (f) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of services by independent groups of persons, who are carrying on an activity which is exempt from VAT or in relation to which they are not taxable persons, for the purpose of rendering their members the services directly necessary for the exercise of that activity, where those groups merely claim from their members exact reimbursement of their share of the joint expenses, provided that such exemption is not likely to cause distortion of competition.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1F")]
        VATEX_EU_132_1F,

        /// <summary>
        /// Exempt based on article 132, section 1 (g) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of services and of goods closely linked to welfare and social security work, including those supplied by old people's homes, by bodies governed by public law or by other bodies recognised by the Member State concerned as being devoted to social wellbeing.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1G")]
        VATEX_EU_132_1G,

        /// <summary>
        /// Exempt based on article 132, section 1 (h) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of services and of goods closely linked to the protection of children and young persons by bodies governed by public law or by other organisations recognised by the Member State concerned as being devoted to social wellbeing.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1H")]
        VATEX_EU_132_1H,

        /// <summary>
        /// Exempt based on article 132, section 1 (i) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The provision of children's or young people's education, school or university education, vocational training or retraining, including the supply of services and of goods closely related thereto, by bodies governed by public law having such as their aim or by other organisations recognised by the Member State concerned as having similar objects.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1I")]
        VATEX_EU_132_1I,

        /// <summary>
        /// Exempt based on article 132, section 1 (j) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Tuition given privately by teachers and covering school or university education.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1J")]
        VATEX_EU_132_1J,

        /// <summary>
        /// Exempt based on article 132, section 1 (k) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of staff by religious or philosophical institutions for the purpose of the activities referred to in points (b), (g), (h) and (i) and with a view to spiritual welfare.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1K")]
        VATEX_EU_132_1K,

        /// <summary>
        /// Exempt based on article 132, section 1 (l) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of services, and the supply of goods closely linked thereto, to their members in their common interest in return for a subscription fixed in accordance with their rules by non-profitmaking organisations with aims of a political, trade-union, religious, patriotic, philosophical, philanthropic or civic nature, provided that this exemption is not likely to cause distortion of competition.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1L")]
        VATEX_EU_132_1L,

        /// <summary>
        /// Exempt based on article 132, section 1 (m) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of certain services closely linked to sport or physical education by non-profit-making organisations to persons taking part in sport or physical education.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1M")]
        VATEX_EU_132_1M,

        /// <summary>
        /// Exempt based on article 132, section 1 (n) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of certain cultural services, and the supply of goods closely linked thereto, by bodies governed by public law or by other cultural bodies recognised by the Member State concerned.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1N")]
        VATEX_EU_132_1N,

        /// <summary>
        /// Exempt based on article 132, section 1 (o) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of services and goods, by organisations whose activities are exempt pursuant to points (b), (g), (h), (i), (l), (m) and (n), in connection with fund-raising events organised exclusively for their own benefit, provided that exemption is not likely to cause distortion of competition.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1O")]
        VATEX_EU_132_1O,

        /// <summary>
        /// Exempt based on article 132, section 1 (p) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The supply of transport services for sick or injured persons in vehicles specially designed for the purpose, by duly authorised bodies.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1P")]
        VATEX_EU_132_1P,

        /// <summary>
        /// Exempt based on article 132, section 1 (q) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The activities, other than those of a commercial nature, carried out by public radio and television bodies.
        /// </remarks>
        [EnumStringValue("VATEX-EU-132-1Q")]
        VATEX_EU_132_1Q,

        /// <summary>
        /// Exempt based on article 143 of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Exemptions on importation.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143")]
        VATEX_EU_143,

        /// <summary>
        /// Exempt based on article 143, section 1 (a) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The final importation of goods of which the supply by a taxable person would in all circumstances be exempt within their respective territory.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1A")]
        VATEX_EU_143_1A,

        /// <summary>
        /// Exempt based on article 143, section 1 (b) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The final importation of goods governed by Council Directives 69/169/EEC, 83/181/EEC and 2006/79/EC.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1B")]
        VATEX_EU_143_1B,

        /// <summary>
        /// Exempt based on article 143, section 1 (c) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The final importation of goods, in free circulation from a third territory forming part of the Community customs territory, which would be entitled to exemption under point (b) if they had been imported within the meaning of the first paragraph of Article 30.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1C")]
        VATEX_EU_143_1C,

        /// <summary>
        /// Exempt based on article 143, section 1 (d) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation of goods dispatched or transported from a third territory or a third country into a Member State other than that in which the dispatch or transport of the goods ends, where the supply of such goods by the importer designated or recognised under Article 201 as liable for payment of VAT is exempt under Article 138.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1D")]
        VATEX_EU_143_1D,

        /// <summary>
        /// Exempt based on article 143, section 1 (e) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The reimportation, by the person who exported them, of goods in the state in which they were exported, where those goods are exempt from customs duties.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1E")]
        VATEX_EU_143_1E,

        /// <summary>
        /// Exempt based on article 143, section 1 (f) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation, under diplomatic and consular arrangements, of goods which are exempt from customs duties.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1F")]
        VATEX_EU_143_1F,

        /// <summary>
        /// Exempt based on article 143, section 1 (fa) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation of goods by the European Community, the European Atomic Energy Community, the European Central Bank or the European Investment Bank, or by the bodies set up by the Communities to which the Protocol of 8 April 1965 on the privileges and immunities of the European Communities applies, within the limits and under the conditions of that Protocol and the agreements for its implementation or the headquarters agreements, in so far as it does not lead to distortion of competition.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1FA")]
        VATEX_EU_143_1FA,

        /// <summary>
        /// Exempt based on article 143, section 1 (g) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation of goods by international bodies, other than those referred to in point (fa), recognised as such by the public authorities of the host Member State, or by members of such bodies, within the limits and under the conditions laid down by the international conventions establishing the bodies or by headquarters agreements.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1G")]
        VATEX_EU_143_1G,

        /// <summary>
        /// Exempt based on article 143, section 1 (h) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation of goods, into Member States party to the North Atlantic Treaty, by the armed forces of other States party to that Treaty for the use of those forces or the civilian staff accompanying them or for supplying their messes or canteens where such forces take part in the common defence effort.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1H")]
        VATEX_EU_143_1H,

        /// <summary>
        /// Exempt based on article 143, section 1 (i) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation of goods by the armed forces of the United Kingdom stationed in the island of Cyprus pursuant to the Treaty of Establishment concerning the Republic of Cyprus, dated 16 August 1960, which are for the use of those forces or the civilian staff accompanying them or for supplying their messes or canteens.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1I")]
        VATEX_EU_143_1I,

        /// <summary>
        /// Exempt based on article 143, section 1 (j) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation into ports, by sea fishing undertakings, of their catches, unprocessed or after undergoing preservation for marketing but before being supplied.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1J")]
        VATEX_EU_143_1J,

        /// <summary>
        /// Exempt based on article 143, section 1 (k) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation of gold by central banks.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1K")]
        VATEX_EU_143_1K,

        /// <summary>
        /// Exempt based on article 143, section 1 (l) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// The importation of gas through a natural gas system or any network connected to such a system or fed in from a vessel transporting gas into a natural gas system or any upstream pipeline network, of electricity or of heat or cooling energy through heating or cooling networks.
        /// </remarks>
        [EnumStringValue("VATEX-EU-143-1L")]
        VATEX_EU_143_1L,

        /// <summary>
        /// Exempt based on article 144 of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Exemptions for services linked to the import of goods.
        /// </remarks>
        [EnumStringValue("VATEX-EU-144")]
        VATEX_EU_144,

        /// <summary>
        /// Exempt based on article 146 section 1 (e) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Exemptions for services linked to the export of goods.
        /// </remarks>
        [EnumStringValue("VATEX-EU-146-1E")]
        VATEX_EU_146_1E,

        /// <summary>
        /// Exempt based on article 148 of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-148")]
        VATEX_EU_148,

        /// <summary>
        /// Exempt based on article 148, section (a) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Fuel supplies for commercial international transport vessels.
        /// </remarks>
        [EnumStringValue("VATEX-EU-148-A")]
        VATEX_EU_148_A,

        /// <summary>
        /// Exempt based on article 148, section (b) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Fuel supplies for fighting ships in international transport.
        /// </remarks>
        [EnumStringValue("VATEX-EU-148-B")]
        VATEX_EU_148_B,

        /// <summary>
        /// Exempt based on article 148, section (c) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Maintenance, modification, chartering and hiring of international transport vessels.
        /// </remarks>
        [EnumStringValue("VATEX-EU-148-C")]
        VATEX_EU_148_C,

        /// <summary>
        /// Exempt based on article 148, section (d) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Supply of other services to commercial international transport vessels.
        /// </remarks>
        [EnumStringValue("VATEX-EU-148-D")]
        VATEX_EU_148_D,

        /// <summary>
        /// Exempt based on article 148, section (e) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Fuel supplies for aircraft on international routes.
        /// </remarks>
        [EnumStringValue("VATEX-EU-148-E")]
        VATEX_EU_148_E,

        /// <summary>
        /// Exempt based on article 148, section (f) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Maintenance, modification, chartering and hiring of aircraft on international routes.
        /// </remarks>
        [EnumStringValue("VATEX-EU-148-F")]
        VATEX_EU_148_F,

        /// <summary>
        /// Exempt based on article 148, section (g) of Council Directive 2006/112/EC
        /// </summary>
        /// <remarks>
        /// Supply of other services to aircraft on international routes.
        /// </remarks>
        [EnumStringValue("VATEX-EU-148-G")]
        VATEX_EU_148_G,

        /// <summary>
        /// Exempt based on article 151 of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-151")]
        VATEX_EU_151,

        /// <summary>
        /// Export outside the EU
        /// </summary>
        /// <remarks>Only to be used with VAT category code G</remarks>
        [EnumStringValue("VATEX-EU-G")]
        VATEX_EU_G,

        /// <summary>
        /// Exempt (général / other)
        /// </summary>
        [EnumStringValue("VATEX-EU-O")]
        VATEX_EU_O,

        /// <summary>
        /// Intra-Community supply
        /// </summary>
        [EnumStringValue("VATEX-EU-IC")]
        VATEX_EU_IC,

        /// <summary>
        /// Reverse charge
        /// </summary>
        [EnumStringValue("VATEX-EU-AE")]
        VATEX_EU_AE,

        /// <summary>
        /// Not subject to VAT / Hors champ de la TVA
        /// </summary>
        [EnumStringValue("VATEX-EU-D")]
        VATEX_EU_D,

        /// <summary>
        /// Travel agents VAT scheme (marge – régime de la marge)
        /// </summary>
        [EnumStringValue("VATEX-EU-F")]
        VATEX_EU_F,

        /// <summary>
        /// Second hand goods VAT scheme (marge – biens d'occasion)
        /// </summary>
        [EnumStringValue("VATEX-EU-I")]
        VATEX_EU_I,

        /// <summary>
        /// Investment gold scheme
        /// </summary>
        [EnumStringValue("VATEX-EU-J")]
        VATEX_EU_J,

        // Codes nationaux FR (utilisés en facturation domestique France / Chorus Pro / Factur-X)
        /// <summary>
        /// France domestic VAT franchise in base
        /// </summary>
        /// <remarks>Franchise en base de TVA – Only to be used for domestic invoicing in France</remarks>
        [EnumStringValue("VATEX-FR-FRANCHISE")]
        VATEX_FR_FRANCHISE,

        /// <summary>
        /// France domestic Credit Notes without VAT, due to supplier forfeit of VAT for discount
        /// </summary>
        /// <remarks>Avoir sans TVA suite à abandon d'escompte ou forfait de TVA par le fournisseur</remarks>
        [EnumStringValue("VATEX-FR-CNWVAT")]
        VATEX_FR_CNWVAT,

        /// <summary>
        /// Exempt based on 1° of article 261 of the Code Général des Impôts (CGI ; General tax code)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261-1")]
        VATEX_FR_CGI261_1,

        /// <summary>
        /// Exempt based on 2° of article 261 of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261-2")]
        VATEX_FR_CGI261_2,

        /// <summary>
        /// Exempt based on 3° of article 261 of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261-3")]
        VATEX_FR_CGI261_3,

        /// <summary>
        /// Exempt based on 4° of article 261 of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261-4")]
        VATEX_FR_CGI261_4,

        /// <summary>
        /// Exempt based on 5° of article 261 of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261-5")]
        VATEX_FR_CGI261_5,

        /// <summary>
        /// Exempt based on 7° of article 261 of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261-7")]
        VATEX_FR_CGI261_7,

        /// <summary>
        /// Exempt based on 8° of article 261 of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261-8")]
        VATEX_FR_CGI261_8,

        /// <summary>
        /// Exempt based on article 261 A of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261A")]
        VATEX_FR_CGI261A,

        /// <summary>
        /// Exempt based on article 261 B of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261B")]
        VATEX_FR_CGI261B,

        /// <summary>
        /// Exempt based on 1° of article 261 C of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261C-1")]
        VATEX_FR_CGI261C_1,

        /// <summary>
        /// Exempt based on 2° of article 261 C of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261C-2")]
        VATEX_FR_CGI261C_2,

        /// <summary>
        /// Exempt based on 3° of article 261 C of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261C-3")]
        VATEX_FR_CGI261C_3,

        /// <summary>
        /// Exempt based on 1 of article 261 D of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261D-1")]
        VATEX_FR_CGI261D_1,

        /// <summary>
        /// Exempt based on 1 bis of article 261 D of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261D-1BIS")]
        VATEX_FR_CGI261D_1BIS,

        /// <summary>
        /// Exempt based on 2 of article 261 D of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261D-2")]
        VATEX_FR_CGI261D_2,

        /// <summary>
        /// Exempt based on 3 of article 261 D of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261D-3")]
        VATEX_FR_CGI261D_3,

        /// <summary>
        /// Exempt based on 4 of article 261 D of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261D-4")]
        VATEX_FR_CGI261D_4,

        /// <summary>
        /// Exempt based on 1 of article 261 E of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261E-1")]
        VATEX_FR_CGI261E_1,

        /// <summary>
        /// Exempt based on 2 of article 261 E of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI261E-2")]
        VATEX_FR_CGI261E_2,

        /// <summary>
        /// Exempt based on article 277 A of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI277A")]
        VATEX_FR_CGI277A,

        /// <summary>
        /// Exempt based on article 275 of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI275")]
        VATEX_FR_CGI275,

        /// <summary>
        /// Exempt based on article 298 sexdecies A of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-298SEXDECIESA")]
        VATEX_FR_298SEXDECIESA,

        /// <summary>
        /// Exempt based on article 295 of the Code Général des Impôts (CGI)
        /// </summary>
        [EnumStringValue("VATEX-FR-CGI295")]
        VATEX_FR_CGI295,

        /// <summary>
        /// Auto-liquidation (reverse charge) spécifique France
        /// </summary>
        [EnumStringValue("VATEX-FR-AE")]
        VATEX_FR_AE
    }
}
