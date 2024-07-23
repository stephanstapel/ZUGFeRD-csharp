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
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// https://www.xrepository.de/details/urn:xoev-de:kosit:codeliste:VATEX_1
    /// </summary>
    public enum TaxExemptionReasonCodes
    {
        /// <summary>
        /// Exempt based on article 132 of Council Directive 2006/112/EC
        ///         
        /// Exemptions for certain activities in public interest.
        /// </summary>
        VATEX_132_2,

        /// <summary>
        /// Exempt based on article 132, section 1 (a) of Council Directive 2006/112/EC 
        ///             
        /// The supply by the public postal services of services other than passenger transport and telecommunications services, and the supply of goods incidental thereto.
        /// </summary>
        VATEX_EU_132_1A,

        /// <summary>
        /// Exempt based on article 132, section 1 (b) of Council Directive 2006/112/EC 
        /// 
        /// Hospital and medical care and closely related activities undertaken by bodies governed by public law or, under social conditions comparable with those applicable to bodies governed by public law, by hospitals, centres for medical treatment or diagnosis and other duly recognised establishments of a similar nature
        /// </summary>
        VATEX_EU_132_1B, 

        /// <summary>
        /// Exempt based on article 132, section 1 (c) of Council Directive 2006/112/EC 
        /// 
        /// The provision of medical care in the exercise of the medical and paramedical professions as defined by the Member State concerned.
        /// </summary>
        VATEX_EU_132_1C, 

        /// <summary>
        /// Exempt based on article 132, section 1 (d) of Council Directive 2006/112/EC 
        /// 
        /// The supply of human organs, blood and milk.
        /// </summary>
        VATEX_EU_132_1D,	

        /// <summary>
        /// Exempt based on article 132, section 1 (e) of Council Directive 2006/112/EC 
        /// 
        /// The supply of services by dental technicians in their professional capacity and the supply of dental prostheses by dentists and dental technicians.	
        /// </summary>
        VATEX_EU_132_1E,

        /// <summary>
        /// Exempt based on article 132, section 1 (f) of Council Directive 2006/112/EC 
        /// 
        /// The supply of services by independent groups of persons, who are carrying on an activity which is exempt from VAT or in relation to which they are not taxable persons, for the purpose of rendering their members the services directly necessary for the exercise of that activity, where those groups merely claim from their members exact reimbursement of their share of the joint expenses, provided that such exemption is not likely to cause distortion of competition.
        /// </summary>
        VATEX_EU_132_1F,	

        /// <summary>
        /// Exempt based on article 132, section 1 (g) of Council Directive 2006/112/EC 
        /// 
        /// The supply of services and of goods closely linked to welfare and social security work, including those supplied by old people's homes, by bodies governed by public law or by other bodies recognised by the Member State concerned as being devoted to social wellbeing.	
        /// </summary>
        VATEX_EU_132_1G, 

        /// <summary>
        /// Exempt based on article 132, section 1 (h) of Council Directive 2006/112/EC 
        /// 
        /// The supply of services and of goods closely linked to the protection of children and young persons by bodies governed by public law or by other organisations recognised by the Member State concerned as being devoted to social wellbeing;	
        /// </summary>
        VATEX_EU_132_1H, 

        /// <summary>
        /// Exempt based on article 132, section 1 (i) of Council Directive 2006/112/EC 
        /// 
        /// The provision of children's or young people's education, school or university education, vocational training or retraining, including the supply of services and of goods closely related thereto, by bodies governed by public law having such as their aim or by other organisations recognised by the Member State concerned as having similar objects.
        /// </summary>
        VATEX_EU_132_1I, 

        /// <summary>
        /// Exempt based on article 132, section 1 (j) of Council Directive 2006/112/EC 
        /// 
        /// Tuition given privately by teachers and covering school or university education.
        /// </summary>
        VATEX_EU_132_1J,

        /// <summary>
        /// Exempt based on article 132, section 1 (k) of Council Directive 2006/112/EC 
        /// 
        /// The supply of staff by religious or philosophical institutions for the purpose of the activities referred to in points (b), (g), (h) and (i) and with a view to spiritual welfare.	
        /// </summary>
        VATEX_EU_132_1K,

        /// <summary>
        /// Exempt based on article 132, section 1 (l) of Council Directive 2006/112/EC 
        /// 
        /// The supply of services, and the supply of goods closely linked thereto, to their members in their common interest in return for a subscription fixed in accordance with their rules by non-profitmaking organisations with aims of a political, trade-union, religious, patriotic, philosophical, philanthropic or civic nature, provided that this exemption is not likely to cause distortion of competition.
        /// </summary>
        VATEX_EU_132_1L,

        /// <summary>
        /// Exempt based on article 132, section 1 (m) of Council Directive 2006/112/EC 
        /// 
        /// The supply of certain services closely linked to sport or physical education by non-profit-making organisations to persons taking part in sport or physical education.	
        /// </summary>
        VATEX_EU_132_1M,

        /// <summary>
        /// Exempt based on article 132, section 1 (n) of Council Directive 2006/112/EC 
        /// 
        /// The supply of certain cultural services, and the supply of goods closely linked thereto, by bodies governed by public law or by other cultural bodies recognised by the Member State concerned.
        /// </summary>
        VATEX_EU_132_1N,

        /// <summary>
        /// Exempt based on article 132, section 1 (o) of Council Directive 2006/112/EC 
        /// 
        /// The supply of services and goods, by organisations whose activities are exempt pursuant to points (b), (g), (h), (i), (l), (m) and (n), in connection with fund-raising events organised exclusively for their own benefit, provided that exemption is not likely to cause distortion of competition.
        /// </summary>
        VATEX_EU_132_1O,

        /// <summary>
        /// Exempt based on article 132, section 1 (p) of Council Directive 2006/112/EC 
        /// 
        /// The supply of transport services for sick or injured persons in vehicles specially designed for the purpose, by duly authorised bodies.	
        /// </summary>
        VATEX_EU_132_1P,

        /// <summary>
        /// Exempt based on article 132, section 1 (q) of Council Directive 2006/112/EC 
        /// 
        /// The activities, other than those of a commercial nature, carried out by public radio and television bodies.
        /// </summary>
        VATEX_EU_132_1Q,

        /// <summary>
        /// Exempt based on article 143 of Council Directive 2006/112/EC 
        /// 
        /// Exemptions on importation.
        /// </summary>
        VATEX_EU_143,

        /// <summary>
        /// Exempt based on article 143, section 1 (a) of Council Directive 2006/112/EC 
        /// 
        /// The final importation of goods of which the supply by a taxable person would in all circumstances be exempt within their respective territory.	
        /// </summary>
        VATEX_EU_143_1A,

        /// <summary>
        /// Exempt based on article 143, section 1 (b) of Council Directive 2006/112/EC 
        /// 
        /// The final importation of goods governed by Council Directives 69/169/EEC (1), 83/181/EEC(2) and 2006/79/EC(3).	
        /// </summary>
        VATEX_EU_143_1B,

        /// <summary>
        /// Exempt based on article 143, section 1 (c) of Council Directive 2006/112/EC 
        /// 
        /// The final importation of goods, in free circulation from a third territory forming part of the Community customs territory, which would be entitled to exemption under point(b) if they had been imported within the meaning of the first paragraph of Article 30	
        /// </summary>
        VATEX_EU_143_1C,

        /// <summary>
        /// Exempt based on article 143, section 1 (d) of Council Directive 2006/112/EC 
        /// 
        /// The importation of goods dispatched or transported from a third territory or a third country into a Member State other than that in which the dispatch or transport of the goods ends, where the supply of such goods by the importer designated or recognised under Article 201 as liable for payment of VAT is exempt under Article 138.	
        /// </summary>
        VATEX_EU_143_1D,

        /// <summary>
        /// Exempt based on article 143, section 1 (e) of Council Directive 2006/112/EC 
        /// 
        /// The reimportation, by the person who exported them, of goods in the state in which they were exported, where those goods are exempt from customs duties.
        /// </summary>
        VATEX_EU_143_1E,

        /// <summary>
        /// Exempt based on article 143, section 1 (f) of Council Directive 2006/112/EC 
        /// 
        /// The importation, under diplomatic and consular arrangements, of goods which are exempt from customs duties.
        /// </summary>
        VATEX_EU_143_1F,

        /// <summary>
        /// Exempt based on article 143, section 1 (fa) of Council Directive 2006/112/EC 
        /// 
        /// The importation of goods by the European Community, the European Atomic Energy Community, the European Central Bank or the European Investment Bank, or by the bodies set up by the Communities to which the Protocol of 8 April 1965 on the privileges and immunities of the European Communities applies, within the limits and under the conditions of that Protocol and the agreements for its implementation or the headquarters agreements, in so far as it does not lead to distortion of competition;
        /// </summary>
        VATEX_EU_143_1FA, 
    
        /// <summary>
        /// Exempt based on article 143, section 1 (g) of Council Directive 2006/112/EC 
        /// 
        /// The importation of goods by international bodies, other than those referred to in point(fa), recognised as such by the public authorities of the host Member State, or by members of such bodies, within the limits and under the conditions laid down by the international conventions establishing the bodies or by headquarters agreements;
        /// </summary>
        VATEX_EU_143_1G,

        /// <summary>
        /// Exempt based on article 143, section 1 (h) of Council Directive 2006/112/EC 
        /// 
        /// The importation of goods, into Member States party to the North Atlantic Treaty, by the armed forces of other States party to that Treaty for the use of those forces or the civilian staff accompanying them or for supplying their messes or canteens where such forces take part in the common defence effort.
        /// </summary>
        VATEX_EU_143_1H,

        /// <summary>
        /// Exempt based on article 143, section 1 (i) of Council Directive 2006/112/EC 
        /// 
        /// The importation of goods by the armed forces of the United Kingdom stationed in the island of Cyprus pursuant to the Treaty of Establishment concerning the Republic of Cyprus, dated 16 August 1960, which are for the use of those forces or the civilian staff accompanying them or for supplying their messes or canteens.
        /// </summary>
        VATEX_EU_143_1I,

        /// <summary>
        /// Exempt based on article 143, section 1 (j) of Council Directive 2006/112/EC 
        /// 
        /// The importation into ports, by sea fishing undertakings, of their catches, unprocessed or after undergoing preservation for marketing but before being supplied.
        /// </summary>
        VATEX_EU_143_1J,

        /// <summary>
        /// Exempt based on article 143, section 1 (k) of Council Directive 2006/112/EC 
        /// 
        /// The importation of gold by central banks.	
        /// </summary>
        VATEX_EU_143_1K,

        /// <summary>
        /// Exempt based on article 143, section 1 (l) of Council Directive 2006/112/EC 
        /// 
        /// The importation of gas through a natural gas system or any network connected to such a system or fed in from a vessel transporting gas into a natural gas system or any upstream pipeline network, of electricity or of heat or cooling energy through heating or cooling networks.
        /// </summary>
        VATEX_EU_143_1L,

        /// <summary>
        /// Exempt based on article 148 of Council Directive 2006/112/EC 
        /// 
        /// Exemptions related to international transport.	
        /// </summary>
        VATEX_EU_148,

        /// <summary>
        /// Exempt based on article 148, section (a) of Council Directive 2006/112/EC 
        /// 
        /// Fuel supplies for commercial international transport vessels
        /// </summary>
        VATEX_EU_148_A,

        /// <summary>
        /// Exempt based on article 148, section (b) of Council Directive 2006/112/EC 
        /// 
        /// Fuel supplies for fighting ships in international transport.	
        /// </summary>
        VATEX_EU_148_B,

        /// <summary>
        /// Exempt based on article 148, section (c) of Council Directive 2006/112/EC 
        /// 
        /// Maintenance, modification, chartering and hiring of international transport vessels.
        /// </summary>
        VATEX_EU_148_C,

        /// <summary>
        /// Exempt based on article 148, section (d) of Council Directive 2006/112/EC 
        /// 
        /// Supply to of other services to commercial international transport vessels.
        /// </summary>
        VATEX_EU_148_D,

        /// <summary>
        /// Exempt based on article 148, section (e) of Council Directive 2006/112/EC 
        /// 
        /// Fuel supplies for aircraft on international routes.	
        /// </summary>
        VATEX_EU_148_E,

        /// <summary>
        /// Exempt based on article 148, section (f) of Council Directive 2006/112/EC 
        /// 
        /// Maintenance, modification, chartering and hiring of aircraft on international routes.	
        /// </summary>
        VATEX_EU_148_F,

        /// <summary>
        /// Exempt based on article 148, section (g) of Council Directive 2006/112/EC 
        /// 
        /// Supply to of other services to aircraft on international routes.
        /// </summary>
        VATEX_EU_148_G,

        /// <summary>
        /// Exempt based on article 151 of Council Directive 2006/112/EC 
        /// 
        /// Exemptions relating to certain Transactions treated as exports.
        /// </summary>
        VATEX_EU_151,

        /// <summary>
        /// Exempt based on article 151, section 1 (a) of Council Directive 2006/112/EC 
        /// 
        /// The supply of goods or services under diplomatic and consular arrangements.	
        /// </summary>
        VATEX_EU_151_1A,

        /// <summary>
        /// Exempt based on article 151, section 1 (aa) of Council Directive 2006/112/EC 
        /// 
        /// The supply of goods or services to the European Community, the European Atomic Energy Community, the European Central Bank or the European Investment Bank, or to the bodies set up by the Communities to which the Protocol of 8 April 1965 on the privileges and immunities of the European Communities applies, within the limits and under the conditions of that Protocol and the agreements for its implementation or the headquarters agreements, in so far as it does not lead to distortion of competition.	
        /// </summary>
        VATEX_EU_151_1AA,

        /// <summary>
        /// Exempt based on article 151, section 1 (b) of Council Directive 2006/112/EC 
        /// 
        /// The supply of goods or services to international bodies, other than those referred to in point (aa), recognised as such by the public authorities of the host Member States, and to members of such bodies, within the limits and under the conditions laid down by the international conventions establishing the bodies or by headquarters agreements.
        /// </summary>
        VATEX_EU_151_1B,

        /// <summary>
        /// Exempt based on article 151, section 1 (c) of Council Directive 2006/112/EC 
        /// 
        /// The supply of goods or services within a Member State which is a party to the North Atlantic Treaty, intended either for the armed forces of other States party to that Treaty for the use of those forces, or of the civilian staff accompanying them, or for supplying their messes or canteens when such forces take part in the common defence effort.	
        /// </summary>
        VATEX_EU_151_1C,

        /// <summary>
        /// Exempt based on article 151, section 1 (d) of Council Directive 2006/112/EC 
        /// 
        /// The supply of goods or services to another Member State, intended for the armed forces of any State which is a party to the North Atlantic Treaty, other than the Member State of destination itself, for the use of those forces, or of the civilian staff accompanying them, or for supplying their messes or canteens when such forces take part in the common defence effort.	
        /// </summary>
        VATEX_EU_151_1D,

        /// <summary>
        /// Exempt based on article 151, section 1 (e) of Council Directive 2006/112/EC 
        /// 
        /// The supply of goods or services to the armed forces of the United Kingdom stationed in the island of Cyprus pursuant to the Treaty of Establishment concerning the Republic of Cyprus, dated 16 August 1960, which are for the use of those forces, or of the civilian staff accompanying them, or for supplying their messes or canteens.
        /// </summary>
        VATEX_EU_151_1E,

        /// <summary>
        /// Exempt based on article 309 of Council Directive 2006/112/EC 
        /// 
        /// Travel agents performed outside of EU.
        /// </summary>
        VATEX_EU_309,

        /// <summary>
        /// Reverse charge 
        /// 
        /// Supports EN 16931-1 rule BR-AE-10	
        /// 
        /// Only use with VAT category code AE
        /// </summary>
        VATEX_EU_AE,

        /// <summary>
        /// Intra-Community acquisition from second hand means of transport 
        /// 
        /// Second-hand means of transport - Indication that VAT has been paid according to the relevant transitional arrangements  
        /// 
        /// Only use with VAT category code E
        /// </summary>
        VATEX_EU_D,

        /// <summary>
        /// Intra-Community acquisition of second hand goods    
        /// 
        /// Second-hand goods - Indication that the VAT margin scheme for second-hand goods has been applied.
        /// 
        /// Only use with VAT category code E
        /// </summary>
        VATEX_EU_F,

        /// <summary>
        /// Export outside the EU 
        /// 
        /// Supports EN 16931-1 rule BR-G-10	
        /// 
        /// Only use with VAT category code G
        /// </summary>
        VATEX_EU_G,

        /// <summary>
        /// Intra-Community acquisition of works of art 
        /// 
        /// Works of art - Indication that the VAT margin scheme for works of art has been applied.	
        /// 
        /// Only use with VAT category code E
        /// </summary>
        VATEX_EU_I,

        /// <summary>
        /// Intra-Community supply  
        /// 
        /// Supports EN 16931-1 rule BR-IC-10	
        /// 
        /// Only use with VAT category code K
        /// </summary>
        VATEX_EU_IC,

        /// <summary>
        /// Intra-Community acquisition of collectors items and antiques 
        /// 
        /// Collectors' items and antiques - Indication that the VAT margin scheme for collector’s items and antiques has been applied.	
        /// 
        /// Only use with VAT category code E
        /// </summary>
        VATEX_EU_J,

        /// <summary>
        /// Not subject to VAT 
        /// 
        /// Supports EN 16931-1 rule BR-O-10	
        /// 
        /// Only use with VAT category code O
        /// </summary>
        VATEX_EU_O
    }

    internal static class TaxExemptionReasonCodesExtensions
    {
        public static TaxExemptionReasonCodes? FromString(this TaxExemptionReasonCodes _, string s)
        {
            try
            {
                return (TaxExemptionReasonCodes)Enum.Parse(typeof(TaxExemptionReasonCodes), s.Replace("_","-"));
            }
            catch
            {
                return null;
            }
        } // !FromString()


        public static string EnumToString(this TaxExemptionReasonCodes codes)
        {
            return codes.ToString("g").Replace("_","-");
        } // !ToString()
    }
}
