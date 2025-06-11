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
    /// https://www.xrepository.de/details/urn:xoev-de:kosit:codeliste:VATEX_1
    /// </summary>
    public enum TaxExemptionReasonCodes
    {
        /// <summary>
        /// Exempt based on article 79, point c of Council Directive 2006/112/EC
        /// Exemptions relating to repayment of expenditures.
        /// <remarks>
        /// Repayment of expenditure is not an exemption in the sense of the VAT Directive but may be handled as such in the context of the EN16931.
        /// </remarks>
        /// </summary>
        [EnumStringValue("VATEX-EU-79-C")]
        VATEX_EU_79_C,

        /// <summary>
        /// Exempt based on article 132 of Council Directive 2006/112/EC
        /// Exemptions for certain activities in public interest.
        /// </summary>
        [EnumStringValue("VATEX-EU-132")]
        VATEX_EU_132,

        /// <summary>
        /// Exempt based on article 132 of Council Directive 2006/112/EC
        /// Exemptions for certain activities in public interest.
        /// </summary>
        [EnumStringValue("VATEX-132-2")]
        VATEX_132_2,

        /// <summary>
        /// Exempt based on article 132, section 1 (a) of Council Directive 2006/112/EC
        /// The supply by the public postal services of services other than passenger transport and telecommunications services, and the supply of goods incidental thereto.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1A")]
        VATEX_EU_132_1A,

        /// <summary>
        /// Exempt based on article 132, section 1 (b) of Council Directive 2006/112/EC
        /// Hospital and medical care and closely related activities undertaken by bodies governed by public law or, under social conditions comparable with those applicable to bodies governed by public law, by hospitals, centres for medical treatment or diagnosis and other duly recognised establishments of a similar nature.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1B")]
        VATEX_EU_132_1B,

        /// <summary>
        /// Exempt based on article 132, section 1 (c) of Council Directive 2006/112/EC
        /// The provision of medical care in the exercise of the medical and paramedical professions as defined by the Member State concerned.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1C")]
        VATEX_EU_132_1C,

        /// <summary>
        /// Exempt based on article 132, section 1 (d) of Council Directive 2006/112/EC
        /// The supply of human organs, blood and milk.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1D")]
        VATEX_EU_132_1D,

        /// <summary>
        /// Exempt based on article 132, section 1 (e) of Council Directive 2006/112/EC
        /// The supply of services by dental technicians in their professional capacity and the supply of dental prostheses by dentists and dental technicians.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1E")]
        VATEX_EU_132_1E,

        /// <summary>
        /// Exempt based on article 132, section 1 (f) of Council Directive 2006/112/EC
        /// The supply of services by independent groups of persons, who are carrying on an activity which is exempt from VAT or in relation to which they are not taxable persons, for the purpose of rendering their members the services directly necessary for the exercise of that activity, where those groups merely claim from their members exact reimbursement of their share of the joint expenses, provided that such exemption is not likely to cause distortion of competition.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1F")]
        VATEX_EU_132_1F,

        /// <summary>
        /// Exempt based on article 132, section 1 (g) of Council Directive 2006/112/EC
        /// The supply of services and of goods closely linked to welfare and social security work, including those supplied by old people's homes, by bodies governed by public law or by other bodies recognised by the Member State concerned as being devoted to social wellbeing.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1G")]
        VATEX_EU_132_1G,

        /// <summary>
        /// Exempt based on article 132, section 1 (h) of Council Directive 2006/112/EC
        /// The supply of services and of goods closely linked to the protection of children and young persons by bodies governed by public law or by other organisations recognised by the Member State concerned as being devoted to social wellbeing.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1H")]
        VATEX_EU_132_1H,

        /// <summary>
        /// Exempt based on article 132, section 1 (i) of Council Directive 2006/112/EC
        /// The provision of children's or young people's education, school or university education, vocational training or retraining, including the supply of services and of goods closely related thereto, by bodies governed by public law having such as their aim or by other organisations recognised by the Member State concerned as having similar objects.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1I")]
        VATEX_EU_132_1I,

        /// <summary>
        /// Exempt based on article 132, section 1 (j) of Council Directive 2006/112/EC
        /// Tuition given privately by teachers and covering school or university education.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1J")]
        VATEX_EU_132_1J,

        /// <summary>
        /// Exempt based on article 132, section 1 (k) of Council Directive 2006/112/EC
        /// The supply of staff by religious or philosophical institutions for the purpose
        /// of the activities referred to in points (b), (g), (h) and (i) and with a view
        /// to spiritual welfare.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1K")]
        VATEX_EU_132_1K,

        /// <summary>
        /// Exempt based on article 132, section 1 (l) of Council Directive 2006/112/EC
        /// The supply of services, and the supply of goods closely linked thereto, to their
        /// members in their common interest in return for a subscription fixed in accordance
        /// with their rules by non-profitmaking organisations with aims of a political, trade-union,
        /// religious, patriotic, philosophical, philanthropic or civic nature, provided that this
        /// exemption is not likely to cause distortion of competition.
        /// </summary>
        [EnumStringValue("VATEX-EU-132-1L")]
        VATEX_EU_132_1L,

        /// <summary>
        /// Exempt based on article 143 of Council Directive 2006/112/EC
        /// Member States shall exempt the supply of services relating to the importation of goods.
        /// </summary>
        [EnumStringValue("VATEX-EU-143")]
        VATEX_EU_143,

        /// <summary>
        /// Exempt based on article 143, section 1 (a) of Council Directive 2006/112/EC
        /// The final importation of goods of which the supply by a taxable person would in all
        /// circumstances be exempt within their respective territory.
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1A")]
        VATEX_EU_143_1A,

        /// <summary>
        /// Exempt based on article 143, section 1 (b) of Council Directive 2006/112/EC
        /// The final importation of goods governed by Council Directives 69/169/EEC(1), 83/181/EEC(2) and 2006/79/EC(3).
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1B")]
        VATEX_EU_143_1B,

        /// <summary>
        /// Exempt based on article 143, section 1 (c) of Council Directive 2006/112/EC
        /// The final importation of goods, in free circulation from a third territory
        /// forming part of the Community customs territory, which would be entitled to exemption
        /// under point(b) if they had been imported within the meaning of the first paragraph of Article 30.
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1C")]
        VATEX_EU_143_1C,

        /// <summary>
        /// Exempt based on article 143, section 1 (d) of Council Directive 2006/112/EC
        /// The importation of goods dispatched or transported from a third territory or a
        /// third country into a Member State other than that in which the dispatch or transport
        /// of the goods ends, where the supply of such goods by the importer designated or
        /// recognised under Article 201 as liable for payment of VAT is exempt under Article 138.
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1D")]
        VATEX_EU_143_1D,

        /// <summary>
        /// Exempt based on article 143, section 1 (e) of Council Directive 2006/112/EC
        /// The reimportation, by the person who exported them, of goods in the state in
        /// which they were exported, where those goods are exempt from customs duties.
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1E")]
        VATEX_EU_143_1E,

        /// <summary>
        /// Exempt based on article 143, section 1 (f) of Council Directive 2006/112/EC
        /// The importation, under diplomatic and consular arrangements, of goods which are
        /// exempt from customs duties.
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1F")]
        VATEX_EU_143_1F,

        /// <summary>
        /// Exempt based on article 143, section 1 (fa) of Council Directive 2006/112/EC
        /// The importation of goods by the European Community, the European Atomic Energy
        /// Community, the European Central Bank or the European Investment Bank, or by
        /// the bodies set up by the Communities ... (etc.)
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1FA")]
        VATEX_EU_143_1FA,

        /// <summary>
        /// Exempt based on article 143, section 1 (g) of Council Directive 2006/112/EC
        /// The importation of goods by international bodies, other than those referred
        /// to in point (fa), recognised as such...
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1G")]
        VATEX_EU_143_1G,

        /// <summary>
        /// Exempt based on article 143, section 1 (h) of Council Directive 2006/112/EC
        /// The importation of goods, into Member States party to the North Atlantic Treaty,
        /// by the armed forces...
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1H")]
        VATEX_EU_143_1H,

        /// <summary>
        /// Exempt based on article 143, section 1 (i) of Council Directive 2006/112/EC
        /// The importation of goods by the armed forces of the United Kingdom
        /// stationed in the island of Cyprus...
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1I")]
        VATEX_EU_143_1I,

        /// <summary>
        /// Exempt based on article 143, section 1 (j) of Council Directive 2006/112/EC
        /// The importation into ports, by sea fishing undertakings, of their catches...
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1J")]
        VATEX_EU_143_1J,

        /// <summary>
        /// Exempt based on article 143, section 1 (k) of Council Directive 2006/112/EC
        /// The importation of gold by central banks.
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1K")]
        VATEX_EU_143_1K,

        /// <summary>
        /// Exempt based on article 143, section 1 (l) of Council Directive 2006/112/EC
        /// The importation of gas, electricity or heat via networks.
        /// </summary>
        [EnumStringValue("VATEX-EU-143-1L")]
        VATEX_EU_143_1L,

        /// <summary>
        /// Exempt based on article 144 of Council Directive 2006/112/EC
        /// Member States shall exempt the supply of services relating to the supply of goods.
        /// </summary>
        [EnumStringValue("VATEX-EU-144")]
        VATEX_EU_144,

        /// <summary>
        /// Exempt based on article 146 section 1 (e) of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-146-1E")]
        VATEX_EU_146_1E,

        /// <summary>
        /// Exempt based on article 148 of Council Directive 2006/112/EC
        /// Exemptions related to international transport.
        /// </summary>
        [EnumStringValue("VATEX-EU-148")]
        VATEX_EU_148,

        /// <summary>
        /// Exempt based on article 148, section (a) of Council Directive 2006/112/EC
        /// Fuel supplies for commercial international transport vessels.
        /// </summary>
        [EnumStringValue("VATEX-EU-148-A")]
        VATEX_EU_148_A,

        /// <summary>
        /// Exempt based on article 148, section (b) of Council Directive 2006/112/EC
        /// Fuel supplies for fighting ships in international transport.
        /// </summary>
        [EnumStringValue("VATEX-EU-148-B")]
        VATEX_EU_148_B,

        /// <summary>
        /// Exempt based on article 148, section (c) of Council Directive 2006/112/EC
        /// Maintenance, modification, chartering and hiring of international transport vessels.
        /// </summary>
        [EnumStringValue("VATEX-EU-148-C")]
        VATEX_EU_148_C,

        /// <summary>
        /// Exempt based on article 148, section (d) of Council Directive 2006/112/EC
        /// Supply of other services to commercial international transport vessels.
        /// </summary>
        [EnumStringValue("VATEX-EU-148-D")]
        VATEX_EU_148_D,

        /// <summary>
        /// Exempt based on article 148, section (e) of Council Directive 2006/112/EC
        /// Fuel supplies for aircraft on international routes.
        /// </summary>
        [EnumStringValue("VATEX-EU-148-E")]
        VATEX_EU_148_E,

        /// <summary>
        /// Exempt based on article 148, section (f) of Council Directive 2006/112/EC
        /// Maintenance, modification, chartering and hiring of aircraft on international routes.
        /// </summary>
        [EnumStringValue("VATEX-EU-148-F")]
        VATEX_EU_148_F,

        /// <summary>
        /// Exempt based on article 148, section (g) of Council Directive 2006/112/EC
        /// Supply of other services to aircraft on international routes.
        /// </summary>
        [EnumStringValue("VATEX-EU-148-G")]
        VATEX_EU_148_G,

        /// <summary>
        /// Exempt based on article 151 of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-151")]
        VATEX_EU_151,

        /// <summary>
        /// Exempt based on article 151, section 1 (a) of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-151-1A")]
        VATEX_EU_151_1A,

        /// <summary>
        /// Exempt based on article 151, section 1 (aa) of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-151-1AA")]
        VATEX_EU_151_1AA,

        /// <summary>
        /// Exempt based on article 151, section 1 (b) of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-151-1B")]
        VATEX_EU_151_1B,

        /// <summary>
        /// Exempt based on article 151, section 1 (c) of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-151-1C")]
        VATEX_EU_151_1C,

        /// <summary>
        /// Exempt based on article 151, section 1 (d) of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-151-1D")]
        VATEX_EU_151_1D,

        /// <summary>
        /// Exempt based on article 151, section 1 (e) of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-151-1E")]
        VATEX_EU_151_1E,

        /// <summary>
        /// Exempt based on article 159 of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-159")]
        VATEX_EU_159,

        /// <summary>
        /// Exempt based on article 309 of Council Directive 2006/112/EC
        /// </summary>
        [EnumStringValue("VATEX-EU-309")]
        VATEX_EU_309,

        /// <summary>
        /// Reverse charge – Only use with VAT category code AE
        /// </summary>
        [EnumStringValue("VATEX-EU-AE")]
        VATEX_EU_AE,

        /// <summary>
        /// Intra-Community acquisition from second hand means of transport – Only use with VAT category code E
        /// </summary>
        [EnumStringValue("VATEX-EU-D")]
        VATEX_EU_D,

        /// <summary>
        /// Intra-Community acquisition of second hand goods – Only use with VAT category code E
        /// </summary>
        [EnumStringValue("VATEX-EU-F")]
        VATEX_EU_F,

        /// <summary>
        /// Export outside the EU – Only use with VAT category code G
        /// </summary>
        [EnumStringValue("VATEX-EU-G")]
        VATEX_EU_G,

        /// <summary>
        /// Intra-Community acquisition of works of art – Only use with VAT category code E
        /// </summary>
        [EnumStringValue("VATEX-EU-I")]
        VATEX_EU_I,

        /// <summary>
        /// Intra-Community supply – Only use with VAT category code K
        /// </summary>
        [EnumStringValue("VATEX-EU-IC")]
        VATEX_EU_IC,

        /// <summary>
        /// Intra-Community acquisition of collectors items and antiques – Only use with VAT category code E
        /// </summary>
        [EnumStringValue("VATEX-EU-J")]
        VATEX_EU_J,

        /// <summary>
        /// Not subject to VAT – Only use with VAT category code O
        /// </summary>
        [EnumStringValue("VATEX-EU-O")]
        VATEX_EU_O,

        /// <summary>
        /// France domestic VAT franchise
        /// </summary>
        [EnumStringValue("VATEX-FR-FRANCHISE")]
        VATEX_FR_FRANCHISE,

        /// <summary>
        /// France domestic Credit Notes without VAT, due to supplier forfeit of VAT for discount
        /// </summary>
        [EnumStringValue("VATEX-FR-CNWVAT")]
        VATEX_FR_CNWVAT
    }
}
