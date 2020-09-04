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
using System.Runtime.InteropServices;

namespace ZUGFeRD_Test
{
    class Application
    {
        internal void Run()
        {
            InvoiceDescriptor desc = _createNewInvoice();
            desc.Save("xrechnung.xml", ZUGFeRDVersion.Version21, Profile.XRechnung);

            // --- ZUGFeRD 2.0x tests ---

            // load demo data
            //_loadZUGFeRD2EinfachInvoice();
            //_loadZUGFeRD2ExtendedInvoice();

            _loadZUGFeRD21BasicInvoice();
            //_loadZUGFeRD21BasicWLInvoice();
            //_loadZUGFeRD21ExtendedInvoice();
            //_loadZUGFeRD21MinimumInvoice();

            //_loadXRechnungCII();

            _loadSaveLoadZUGFeRD21BasicInvoice();

            // --- ZUGFeRD 1.x tests ---
            ZugFerd1ComfortEinfachGenerator generator = new ZugFerd1ComfortEinfachGenerator();
            generator.generate();
            generator.read();

            ZugFerd1ExtendedWarenrechnungGenerator generator2 = new ZugFerd1ExtendedWarenrechnungGenerator();
            generator2.generate();
            generator2.read();

            _loadZUGFeRD1EinfachOriginalInvoice();
            _loadZUGFeRD1ComfortRabatteInvoice();

            _saveAndLoadZUGFeRD1Invoice();
            _saveAndLoadZUGFeRD1InvoiceViaStream();
        } // !run()
        

        private void _loadZUGFeRD21BasicInvoice()
        {
            string path = @"..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.Basic);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "471102");
            Assert.AreEqual(desc.TradeLineItems.Count, 1);
            Assert.AreEqual(desc.LineTotalAmount, 198.0m);
        } // !_loadZUGFeRD21BasicInvoice()


        private void _loadSaveLoadZUGFeRD21BasicInvoice()
        {
            string path = @"..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor originalDesc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(originalDesc.Profile, Profile.Basic);
            Assert.AreEqual(originalDesc.Type, InvoiceType.Invoice);
            Assert.AreEqual(originalDesc.InvoiceNo, "471102");
            Assert.AreEqual(originalDesc.TradeLineItems.Count, 1);
            Assert.AreEqual(originalDesc.LineTotalAmount, 198.0m);
            Assert.AreEqual(originalDesc.Taxes[0].TaxAmount, 37.62m);
            Assert.AreEqual(originalDesc.Taxes[0].Percent, 19.0m);
            originalDesc.IsTest = false;

            Stream ms = new MemoryStream();
            originalDesc.Save(ms, ZUGFeRDVersion.Version21, Profile.Basic);
            originalDesc.Save(@"zugferd_2p1_BASIC_Einfach-factur-x_Result.xml", ZUGFeRDVersion.Version21);

            InvoiceDescriptor desc = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(desc.Profile, Profile.Basic);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "471102");
            Assert.AreEqual(desc.TradeLineItems.Count, 1);
            Assert.AreEqual(desc.LineTotalAmount, 198.0m);
            Assert.AreEqual(desc.Taxes[0].TaxAmount, 37.62m);
            Assert.AreEqual(desc.Taxes[0].Percent, 19.0m);
        } // !_loadZUGFeRD21BasicInvoice()


        private void _loadZUGFeRD21BasicWLInvoice()
        {

            string path = @"..\..\..\demodata\zugferd21\zugferd_2p1_BASIC-WL_Einfach-factur-x.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.BasicWL);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "471102");
            Assert.AreEqual(desc.TradeLineItems.Count, 0);
            Assert.AreEqual(desc.LineTotalAmount, 624.90m);
        } // !_loadZUGFeRD21BasicWLInvoice()


        private void _loadZUGFeRD21ExtendedInvoice()
        {
            string path = @"..\..\..\demodata\zugferd21\zugferd_2p1_EXTENDED_Warenrechnung-factur-x.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.Extended);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "R87654321012345");
            Assert.AreEqual(desc.TradeLineItems.Count, 6);
            Assert.AreEqual(desc.LineTotalAmount, 457.20m);
        } // !_loadZUGFeRD21ExtendedInvoice()


        private void _loadZUGFeRD21MinimumInvoice()
        {
            string path = @"..\..\..\demodata\zugferd21\zugferd_2p1_MINIMUM_Rechnung-factur-x.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.Minimum);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "471102");
            Assert.AreEqual(desc.TradeLineItems.Count, 0);
            Assert.AreEqual(desc.LineTotalAmount, 0.0m); // not present in file
            Assert.AreEqual(desc.TaxBasisAmount, 198.0m);
        } // !_loadZUGFeRD21MinimumInvoice()


        private void _loadXRechnungCII()
        {
            string path = @"..\..\..\demodata\xRechnung\xRechnung CII.xml";
            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            Assert.AreEqual(desc.Profile, Profile.XRechnung);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "0815-99-1-a");
            Assert.AreEqual(desc.TradeLineItems.Count, 2);
            Assert.AreEqual(desc.LineTotalAmount, 1445.98m);
        } // !_loadZUGFeRD1EinfachOriginalInvoice()


        private void _loadZUGFeRD2EinfachInvoice()
        {
            string path = @"..\..\..\demodata\zugferd20\zugferd_2p0_BASIC_Einfach.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.Basic);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "471102");
            Assert.AreEqual(desc.TradeLineItems.Count, 1);
            Assert.AreEqual(desc.LineTotalAmount, 198.0m);
        } // !_loadZUGFeRD2EinfachInvoice()


        private void _loadZUGFeRD2ExtendedInvoice()
        {
            string path = @"..\..\..\demodata\zugferd20\zugferd_2p0_EXTENDED_Warenrechnung.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.Extended);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "R87654321012345");
            Assert.AreEqual(desc.TradeLineItems.Count, 6);
            Assert.AreEqual(desc.LineTotalAmount, 457.20m);
        } // !_loadZUGFeRD2ExtendedInvoice()


        private void _loadZUGFeRD1EinfachOriginalInvoice()
        {
            string path = @"..\..\..\demodata\zugferd10\ZUGFeRD_1p0_COMFORT_Einfach.xml";
            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            Assert.AreEqual(desc.Profile, Profile.Comfort);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
        } // !_loadZUGFeRD1EinfachOriginalInvoice()


        private void _loadZUGFeRD1ComfortRabatteInvoice()
        {
            string path = @"..\..\..\demodata\zugferd10\ZUGFeRD_1p0_COMFORT_Rabatte.xml";
            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            desc.Save("test.xml", ZUGFeRDVersion.Version1, Profile.Comfort);

            Assert.AreEqual(desc.Profile, Profile.Comfort);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.CreditorBankAccounts[0].BankName, "Hausbank München");
        } // !_loadZUGFeRDComfortRabatteInvoice()


        private void _saveAndLoadZUGFeRD1Invoice()
        {
            string path = "output.xml";
            InvoiceDescriptor desc = _createInvoice();
            desc.Save(path, ZUGFeRDVersion.Version1, Profile.Comfort);

            InvoiceDescriptor desc2 = InvoiceDescriptor.Load(path);
        } // !_saveAndLoadZUGFeRD1Invoice()


        private void _saveAndLoadZUGFeRD1InvoiceViaStream()
        {
            InvoiceDescriptor desc = _createInvoice();

            string path = "output_stream.xml";
            FileStream saveStream = new FileStream(path, FileMode.Create);
            desc.Save(saveStream, ZUGFeRDVersion.Version1, Profile.Comfort);
            saveStream.Close();

            FileStream loadStream = new FileStream(path, FileMode.Open);
            InvoiceDescriptor desc2 = InvoiceDescriptor.Load(loadStream);
            loadStream.Close();

            Assert.AreEqual(desc2.Profile, Profile.Comfort);
            Assert.AreEqual(desc2.Type, InvoiceType.Invoice);


            // try again with a memory stream
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version1, Profile.Comfort);

            byte[] data = ms.ToArray();
            string s = System.Text.Encoding.Default.GetString(data);
        } // !_saveAndLoadZUGFeRD1InvoiceViaStream()

    
        private InvoiceDescriptor _createInvoice()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2018, 03, 05), CurrencyCodes.EUR);
            desc.AddNote("Rechnung gemäß Bestellung vom 01.03.2018.");
            desc.AddNote(note: "Lieferant GmbH\r\nLieferantenstraße 20\r\n80333 München\r\nDeutschland\r\nGeschäftsführer: Hans Muster\r\nHandelsregisternummer: H A 123\r\n",
                         subjectCode: SubjectCodes.REG
                        );

            desc.AddTradeLineItem(name: "Trennblätter A4",
                                  unitCode: QuantityCodes.H87,
                                  sellerAssignedID: "TB100A4",
                                  id: new GlobalID("0160", "4012345001235"),
                                  grossUnitPrice: 9.9m,
                                  netUnitPrice: 9.9m,
                                  billedQuantity: 20m,
                                  taxType: TaxTypes.VAT,
                                  categoryCode: TaxCategoryCodes.S,
                                  taxPercent:19m
                                 );

            desc.AddTradeLineItem(name: "Joghurt Banane",
                unitCode: QuantityCodes.H87,
                sellerAssignedID: "ARNR2",
                id: new GlobalID("0160", "4000050986428"),
                grossUnitPrice: 5.5m,
                netUnitPrice: 5.5m,
                billedQuantity: 50,
                taxType: TaxTypes.VAT,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 7
                );

            desc.ReferenceOrderNo = "04011000-12345-34";
            desc.SetSeller(name: "Lieferant GmbH",
                           postcode: "80333",
                           city: "München",
                           street: "Lieferantenstraße 20",
                           country:  CountryCodes.DE,
                           id: "",
                           globalID: new GlobalID("0088", "4000001123452")
                           );
            desc.SetSellerContact(name: "Max Mustermann",
                                  orgunit: "Muster-Einkauf",
                                  emailAddress: "Max@Mustermann.de",
                                  phoneno: "+49891234567"
                                 );
            desc.AddSellerTaxRegistration("201/113/40209", TaxRegistrationSchemeID.FC);
            desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);

            desc.SetBuyer(name: "Kunden AG Mitte",
                          postcode: "69876",
                          city: "Frankfurt",
                          street: "Kundenstraße 15",
                          country: CountryCodes.DE,
                          id: "GE2020211"
                          );

            desc.ActualDeliveryDate = new DateTime(2018, 03, 05);
            desc.SetPaymentMeans(PaymentMeansTypeCodes.SEPACreditTransfer, "Zahlung per SEPA Überweisung.");
            desc.AddCreditorFinancialAccount(iban: "DE02120300000000202051", bic: "BYLADEM1001", name: "Kunden AG");

            desc.AddApplicableTradeTax(basisAmount: 275.0m,
                                       percent: 7m,
                                       typeCode: TaxTypes.VAT,
                                       categoryCode: TaxCategoryCodes.S
                                       );

            desc.AddApplicableTradeTax(basisAmount: 198.0m,
                                       percent: 19m,
                                       typeCode: TaxTypes.VAT,
                                       categoryCode: TaxCategoryCodes.S
                                       );

            desc.SetTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018, 3% Skonto innerhalb 10 Tagen bis 15.03.2018");
            desc.SetTotals(lineTotalAmount: 473.0m,
                           chargeTotalAmount: 0.0m,
                           allowanceTotalAmount: 0.0m,
                           taxBasisAmount: 473.0m,
                           taxTotalAmount: 56.87m,
                           grandTotalAmount: 529.87m,
                           totalPrepaidAmount: 0.0m,
                           duePayableAmount: 529.87m
                          );

            return desc;
        } // !_createInvoice()


        private InvoiceDescriptor _createInvoiceOld()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2013, 6, 5), CurrencyCodes.EUR, "GE2020211-471102");            
            desc.ReferenceOrderNo = "AB-312";
            desc.AddNote("Rechnung gemäß Bestellung Nr. 2013-471331 vom 01.03.2013.");
            desc.AddNote("Es bestehen Rabatt- und Bonusvereinbarungen.", SubjectCodes.AAK);
            desc.SetBuyer("Kunden Mitte AG", "69876", "Frankfurt", "Kundenstraße 15", CountryCodes.DE, "GE2020211", new GlobalID(GlobalID.SchemeID_GLN, "4000001987658"));
            desc.AddBuyerTaxRegistration("DE234567890", TaxRegistrationSchemeID.VA);
            desc.SetBuyerContact("Hans Muster");
            desc.SetSeller("Lieferant GmbH", "80333", "München", "Lieferantenstraße 20", CountryCodes.DE, "", new GlobalID(GlobalID.SchemeID_GLN, "4000001123452"));
            desc.SetSellerContact("Frau Mustermann", "Vertrieb", "mustermann@lieferant.internal", "0049-89-12345678");
            desc.AddSellerTaxRegistration("201/113/40209", TaxRegistrationSchemeID.FC);
            desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);
            desc.SetBuyerOrderReferenceDocument("2013-471331", new DateTime(2013, 03, 01));
            desc.SetDeliveryNoteReferenceDocument("2013-51111", new DateTime(2013, 6, 3));
            desc.ActualDeliveryDate = new DateTime(2013, 6, 3);
            desc.SetTotals(lineTotalAmount: 147.30m,
                chargeTotalAmount: 0m,
                allowanceTotalAmount: 14.73m,
                taxBasisAmount: 132.57m,
                taxTotalAmount: 10.36m,
                grandTotalAmount: 142.93m,
                totalPrepaidAmount: 50m,
                duePayableAmount: 92.93m);            
            desc.AddApplicableTradeTax(123.57m, 7m, TaxTypes.VAT, TaxCategoryCodes.S);
            desc.AddApplicableTradeTax(9m, 19m, TaxTypes.VAT, TaxCategoryCodes.S);
            desc.AddLogisticsServiceCharge(5.80m, "Versandkosten", TaxTypes.VAT, TaxCategoryCodes.S, 7m);
            desc.AddTradeAllowanceCharge(true, 10m, CurrencyCodes.EUR, 1m, "Sondernachlass", TaxTypes.VAT, TaxCategoryCodes.S, 19);
            desc.AddTradeAllowanceCharge(true, 137.7m, CurrencyCodes.EUR, 13.73m, "Sondernachlass", TaxTypes.VAT, TaxCategoryCodes.S, 7);
            desc.SetTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.07.2013, 3% Skonto innerhalb 10 Tagen bis 15.06.2013", new DateTime(2013, 07, 04));

            desc.SetPaymentMeans(PaymentMeansTypeCodes.PaymentToBankAccount, "Überweisung");
            desc.AddCreditorFinancialAccount("DE08700901001234567890", "GENODEF1M04", "1234567890", "70090100", "Hausbank München");

            desc.AddTradeLineCommentItem("Wir erlauben uns Ihnen folgende Positionen aus der Lieferung Nr. 2013-51112 in Rechnung zu stellen:");
            desc.AddTradeLineItem(name: "Kunstrasen grün 3m breit",
                                  description: "300cm x 100 cm",
                                  unitCode: QuantityCodes.MTK,
                                  unitQuantity: 1,
                                  grossUnitPrice: 4.00m,
                                  netUnitPrice: 3.3333m,
                                  billedQuantity: 3,
                                  taxType: TaxTypes.VAT,
                                  categoryCode: TaxCategoryCodes.S,
                                  taxPercent: 19,
                                  id: new GlobalID(GlobalID.SchemeID_EAN, "4012345001235"),
                                  sellerAssignedID: "KR3M",
                                  buyerAssignedID: "55T01");

            desc.AddTradeLineItem(name: "Schweinesteak",
                                  description: "aus Deutschland",
                                  unitCode: QuantityCodes.KGM,
                                  unitQuantity: 1,
                                  grossUnitPrice: 5.50m,
                                  netUnitPrice: 5.50m,
                                  billedQuantity: 5,
                                  taxType: TaxTypes.VAT,
                                  categoryCode: TaxCategoryCodes.S,
                                  taxPercent: 7,
                                  id: new GlobalID(GlobalID.SchemeID_EAN, "4000050986428"),
                                  sellerAssignedID: "SFK5",
                                  buyerAssignedID: "55T02");


            desc.AddTradeLineItem(name: "Mineralwasser Medium 12 x 1,0l PET",
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
                                  buyerAssignedID: "55T03",
                                  buyerOrderID: "123");
            return desc;
        } // _createInvoiceOld()
    }
}
