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
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZUGFeRD_Test
{
    class ZugFerd1ExtendedWarenrechnungGenerator
    {
        public void generate()
        {
            InvoiceDescriptor desc = _generateDescriptor();
            desc.Save("ZUGFeRD_1p0_EXTENDED_Warenrechnung.xml", ZUGFeRDVersion.Version1, Profile.Extended);
        } // !generate()


        private InvoiceDescriptor _generateDescriptor()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("R87654321012345", new DateTime(2013, 8, 6), CurrencyCodes.EUR);
            desc.IsTest = true;

            desc.AddNote("Es bestehen Rabatt- oder Bonusvereinbarungen.", SubjectCodes.AAK, ContentCodes.ST3);
            desc.AddNote("Der Verkäufer bleibt Eigentümer der Waren bis zu vollständigen Erfüllung der Kaufpreisforderung.", SubjectCodes.AAJ, ContentCodes.EEV);
            desc.AddNote("MUSTERLIEFERANT GMBH BAHNHOFSTRASSE 99 99199 MUSTERHAUSEN Geschäftsführung: Max Mustermann USt-IdNr: DE123456789 Telefon: +49 932 431 0 www.musterlieferant.de HRB Nr. 372876 Amtsgericht Musterstadt GLN 4304171000002 WEEE-Reg-Nr.: DE87654321", 
                         SubjectCodes.REG);
            desc.AddNote("Leergutwert: 46,50");

            desc.SetSeller("MUSTERLIEFERANT GMBH",
                           "99199",
                           "MUSTERHAUSEN",
                           "BAHNHOFSTRASSE 99",
                           CountryCodes.DE,
                           "549910",
                           new GlobalID(GlobalID.SchemeID_GLN, "4333741000005"));
            desc.SetSellerContact("", emailAddress: "max.mustermann@musterlieferant.de", phoneno: "+49 932 431 500");
            desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);

            desc.SetBuyer("MUSTER-KUNDE GMBH",
                          "40235",
                          "DUESSELDORF",
                          "KUNDENWEG 88",
                          CountryCodes.DE,
                          "009420",
                          new GlobalID(GlobalID.SchemeID_GLN, "4304171000002"));
            desc.SetBuyerOrderReferenceDocument("B123456789", new DateTime(2013, 08, 01));

            desc.AdditionalReferencedDocument = new AdditionalReferencedDocument()
            {
                ID = "A456123",
                IssueDateTime = new DateTime(2013, 08, 02),
                ReferenceTypeCode = ReferenceTypeCodes.VN
            };

            /// TODO: ApplicableSupplyChainTradeDelivery
            /// 

            desc.AddTradeLineCommentItem("Wichtige Information: Bei Bestellungen bis zum 19.12. ist die Auslieferung bis spätestens 23.12. garantiert.");
            TradeLineItem item = desc.AddTradeLineItem("Zitronensäure 100ml",
                                                        null,
                                                        QuantityCodes.C62,
                                                        null,
                                                        1,
                                                        1,
                                                        100,
                                                        TaxTypes.VAT,
                                                        TaxCategoryCodes.S,
                                                        19,
                                                        null,
                                                        new GlobalID("0088", "4123456000014"),
                                                        "ZS997",
                                                        "",
                                                        "",
                                                        null,
                                                        "",
                                                        null);

            item = desc.AddTradeLineItem("Gelierzucker Extra 250g",
                                         null,
                                         QuantityCodes.C62,
                                         null,
                                         1.5m,
                                         1.45m,
                                         50,
                                         TaxTypes.VAT,
                                         TaxCategoryCodes.S,
                                         7,
                                         null,
                                         new GlobalID("0088", "4123456000021"),
                                         "GZ250",
                                         "",
                                         "",
                                         null,
                                         "",
                                         null);
            item.AddTradeAllowanceCharge(true,
                                         CurrencyCodes.EUR,
                                         1.5m,
                                         0.03m,
                                         "Artikelrabatt 1");
            item.AddTradeAllowanceCharge(true,
                                         CurrencyCodes.EUR,
                                         1m,
                                         0.02m,
                                         "Artikelrabatt 2");

            return desc;
        } // !_generateDescriptor()


        public void read()
        {
            InvoiceDescriptor tempDesc = _generateDescriptor();
            MemoryStream ms = new MemoryStream();
            tempDesc.Save(ms, ZUGFeRDVersion.Version1, Profile.Extended);
            string s = Encoding.ASCII.GetString(ms.ToArray());
            InvoiceDescriptor desc = InvoiceDescriptor.Load(ms);

            bool equals = tempDesc.Equals(desc);
        } // !read()
    }
}
