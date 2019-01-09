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
using s2industries.ZUGFeRD;
using System.IO;

namespace ZUGFeRD_Test
{
    class Application
    {
        internal void Run()
        {

            InvoiceDescriptor desc = InvoiceDescriptor.Load("c:\\temp\\zugferd.xml");

            ZugFerdComfortEinfachGenerator generator = new ZugFerdComfortEinfachGenerator();
            generator.generate();
            generator.read();

            ZugFerdExtendedWarenrechnungGenerator generator2 = new ZugFerdExtendedWarenrechnungGenerator();
            generator2.generate();
            generator2.read();

            _loadZUGFeRDEinfachOriginalInvoice();
            _loadZUGFeRDComfortRabatteInvoice();

            _saveAndLoadZUGFeRDInvoice();
            _saveAndLoadZUGFeRDInvoiceViaStream();
        } // !run()


        private void _loadZUGFeRDEinfachOriginalInvoice()
        {
            string path = @"..\..\..\demodata\ZUGFeRD_1p0_COMFORT_Einfach_Original.xml";
            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            Assert.AreEqual(desc.Profile, Profile.Comfort);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
        } // !_loadZUGFeRDEinfachOriginalInvoice()


        private void _loadZUGFeRDComfortRabatteInvoice()
        {
            string path = @"..\..\..\demodata\ZUGFeRD_1p0_COMFORT_Rabatte.xml";
            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            desc.Save("test.xml");

            Assert.AreEqual(desc.Profile, Profile.Comfort);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.CreditorBankAccounts[0].BankName, "Hausbank München");
        } // !_loadZUGFeRDComfortRabatteInvoice()


        private void _saveAndLoadZUGFeRDInvoice()
        {
            string path = "output.xml";
            InvoiceDescriptor desc = _createInvoice();
            desc.Save(path);

            InvoiceDescriptor desc2 = InvoiceDescriptor.Load(path);
        } // !_saveAndLoadZUGFeRDInvoice()


        private void _saveAndLoadZUGFeRDInvoiceViaStream()
        {
            InvoiceDescriptor desc = _createInvoice();

            string path = "output_stream.xml";
            FileStream saveStream = new FileStream(path, FileMode.Create);
            desc.Save(saveStream);
            saveStream.Close();

            FileStream loadStream = new FileStream(path, FileMode.Open);
            InvoiceDescriptor desc2 = InvoiceDescriptor.Load(loadStream);
            loadStream.Close();

            Assert.AreEqual(desc2.Profile, Profile.Comfort);
            Assert.AreEqual(desc2.Type, InvoiceType.Invoice);


            // try again with a memory stream
            MemoryStream ms = new MemoryStream();
            desc.Save(ms);

            byte[] data = ms.ToArray();
            string s = System.Text.Encoding.Default.GetString(data);
        } // !_saveAndLoadZUGFeRDInvoiceViaStream()


        private InvoiceDescriptor _createInvoice()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2013, 6, 5), CurrencyCodes.EUR, "GE2020211-471102");
            desc.Profile = Profile.Comfort;
            desc.ReferenceOrderNo = "AB-312";
            desc.AddNote("Rechnung gemäß Bestellung Nr. 2013-471331 vom 01.03.2013.");
            desc.AddNote("Es bestehen Rabatt- und Bonusvereinbarungen.", SubjectCodes.AAK);
            desc.SetBuyer("Kunden Mitte AG", "69876", "Frankfurt", "Kundenstraße 15", CountryCodes.DE, "GE2020211", new GlobalID(GlobalID.SchemeID_GLN, "4000001987658"));
            desc.AddBuyerTaxRegistration("DE234567890", TaxRegistrationSchemeID.VA);
            desc.SetBuyerContact("Hans Muster");
            desc.SetSeller("Lieferant GmbH", "80333", "München", "Lieferantenstraße 20", CountryCodes.DE, "", new GlobalID(GlobalID.SchemeID_GLN, "4000001123452"));
            desc.AddSellerTaxRegistration("201/113/40209", TaxRegistrationSchemeID.FC);
            desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);
            desc.SetBuyerOrderReferenceDocument("2013-471331", new DateTime(2013, 03, 01));
            desc.SetDeliveryNoteReferenceDocument("2013-51111", new DateTime(2013, 6, 3));
            desc.ActualDeliveryDate = new DateTime(2013, 6, 3);
            desc.SetTotals(202.76m, 5.80m, 14.73m, 193.83m, 21.31m, 215.14m, 50.0m, 165.14m);
            desc.AddApplicableTradeTax(129.37m, 7m, TaxTypes.VAT, TaxCategoryCodes.S);
            desc.AddApplicableTradeTax(64.46m, 19m, TaxTypes.VAT, TaxCategoryCodes.S);
            desc.AddLogisticsServiceCharge(5.80m, "Versandkosten", TaxTypes.VAT, TaxCategoryCodes.S, 7m);
            desc.AddTradeAllowanceCharge(true, 10m, CurrencyCodes.EUR, 1m, "Sondernachlass", TaxTypes.VAT, TaxCategoryCodes.S, 19);
            desc.AddTradeAllowanceCharge(true, 137.7m, CurrencyCodes.EUR, 13.73m, "Sondernachlass", TaxTypes.VAT, TaxCategoryCodes.S, 7);
            desc.SetTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.07.2013, 3% Skonto innerhalb 10 Tagen bis 15.06.2013", new DateTime(2013, 07, 04));

            desc.setPaymentMeans(PaymentMeansTypeCodes.PaymentMeans_42, "Überweisung");
            desc.addCreditorFinancialAccount("DE08700901001234567890", "GENODEF1M04", "1234567890", "70090100", "Hausbank München");

            desc.addTradeLineCommentItem("Wir erlauben uns Ihnen folgende Positionen aus der Lieferung Nr. 2013-51112 in Rechnung zu stellen:");
            desc.addTradeLineItem(name: "Kunstrasen grün 3m breit",
                                  description: "300cm x 100 cm",
                                  unitCode: QuantityCodes.MTK,
                                  unitQuantity: 1,
                                  grossUnitPrice: 4.00m,
                                  netUnitPrice: 3.3333m,
                                  billedQuantity: 3,
                                  taxType: TaxTypes.VAT, 
                                  categoryCode: TaxCategoryCodes.S, 
                                  taxPercent: 19,
                                  id:new GlobalID(GlobalID.SchemeID_EAN, "4012345001235"),
                                  sellerAssignedID: "KR3M", 
                                  buyerAssignedID: "55T01");

            desc.addTradeLineItem(name: "Schweinesteak",
                                  description: "aus Deutschland",
                                  unitCode: QuantityCodes.KGM, 
                                  unitQuantity: 1,
                                  grossUnitPrice: 5.50m,
                                  netUnitPrice: 5.50m,
                                  billedQuantity: 5,
                                  taxType: TaxTypes.VAT, 
                                  categoryCode: TaxCategoryCodes.S, 
                                  taxPercent: 7,
                                  id: new GlobalID(GlobalID.SchemeID_EAN,"4000050986428"),
                                  sellerAssignedID: "SFK5", 
                                  buyerAssignedID: "55T02");


            desc.addTradeLineItem(name: "Mineralwasser Medium 12 x 1,0l PET",
                                  description: "",
                                  unitCode: QuantityCodes.C62, 
                                  unitQuantity: 1,
                                  grossUnitPrice: 5.49m,
                                  netUnitPrice: 5.49m,
                                  billedQuantity: 20,
                                  taxType: TaxTypes.VAT, 
                                  categoryCode: TaxCategoryCodes.S, 
                                  taxPercent: 7,
                                  id: new GlobalID(GlobalID.SchemeID_EAN, "4000001234561"),
                                  sellerAssignedID: "GTRWA5", 
                                  buyerAssignedID: "55T03"); 
            return desc;
        } // _createInvoice()
    }
}
