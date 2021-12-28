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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using s2industries.ZUGFeRD;
using System;
using System.IO;
using System.Xml;

namespace ZUGFeRD_Test
{
    [TestClass]
    public class ZUGFeRD20Tests
    {
        [TestMethod]
        public void TestReferenceBasicInvoice()
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
        } // !TestReferenceBasicInvoice()


        [TestMethod]
        public void TestReferenceExtendedInvoice()
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
        } // !TestReferenceExtendedInvoice()
        

        [TestMethod]
        public void TestTotalRounding()
        {
            var uuid = Guid.NewGuid().ToString();
            var issueDateTime = DateTime.Today;

            var desc = new InvoiceDescriptor
            {
                ContractReferencedDocument = new ContractReferencedDocument
                {
                    ID = uuid,
                    IssueDateTime = issueDateTime
                }
            };
            desc.SetTotals(1.99m, 0m, 0m, 0m, 0m, 2m, 0m, 2m, 0.01m);

            var msExtended = new MemoryStream();
            desc.Save(msExtended, ZUGFeRDVersion.Version20, Profile.Extended);
            msExtended.Seek(0, SeekOrigin.Begin);

            var loadedInvoice = InvoiceDescriptor.Load(msExtended);
            Assert.AreEqual(loadedInvoice.RoundingAmount, 0.01m);

            var msBasic = new MemoryStream();
            desc.Save(msBasic, ZUGFeRDVersion.Version20);
            msBasic.Seek(0, SeekOrigin.Begin);

            loadedInvoice = InvoiceDescriptor.Load(msBasic);
            Assert.AreEqual(loadedInvoice.RoundingAmount, 0m);
        } // !TestTotalRounding()

        [TestMethod]
        public void TestMissingPropertiesAreNull()
        {
            string path = @"..\..\..\demodata\zugferd20\zugferd_2p0_BASIC_Einfach.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.BillingPeriodStart == null));
            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.BillingPeriodEnd == null));
        } // !TestMissingPropertiesAreNull()

        [TestMethod]
        public void TestMissingPropertListsEmpty()
        {
            string path = @"..\..\..\demodata\zugferd20\zugferd_2p0_BASIC_Einfach.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.ApplicableProductCharacteristics.Count == 0));
        } // !TestMissingPropertListsEmpty()

        [TestMethod]
        public void TestSepaPreNotification_Load()
        {
            string path = @"..\..\..\demodata\zugferd20\zugferd_2p0_EN16931_SEPA_Prenotification.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);
            Assert.AreEqual(Profile.Comfort, invoiceDescriptor.Profile);
            Assert.AreEqual(InvoiceType.Invoice, invoiceDescriptor.Type);
            Assert.AreEqual("471102", invoiceDescriptor.InvoiceNo);
            Assert.AreEqual(2, invoiceDescriptor.TradeLineItems.Count);
            Assert.AreEqual(new DateTime(2018, 3, 5), invoiceDescriptor.InvoiceDate);

            Assert.AreEqual("1", invoiceDescriptor.TradeLineItems[0].LineID);
            Assert.AreEqual("4012345001235", invoiceDescriptor.TradeLineItems[0].GlobalID.ID);
            Assert.AreEqual("0160", invoiceDescriptor.TradeLineItems[0].GlobalID.SchemeID);
            Assert.AreEqual("TB100A4", invoiceDescriptor.TradeLineItems[0].SellerAssignedID);
            Assert.AreEqual("Trennblätter A4", invoiceDescriptor.TradeLineItems[0].Name);
            Assert.AreEqual(20m, invoiceDescriptor.TradeLineItems[0].BilledQuantity);
            Assert.AreEqual(QuantityCodes.C62, invoiceDescriptor.TradeLineItems[0].UnitCode);
            Assert.AreEqual(9.9m, invoiceDescriptor.TradeLineItems[0].NetUnitPrice);
            Assert.AreEqual(9.9m, invoiceDescriptor.TradeLineItems[0].GrossUnitPrice);
            Assert.AreEqual(TaxCategoryCodes.S, invoiceDescriptor.TradeLineItems[0].TaxCategoryCode);
            Assert.AreEqual(19.0m, invoiceDescriptor.TradeLineItems[0].TaxPercent);
            Assert.AreEqual(TaxTypes.VAT, invoiceDescriptor.TradeLineItems[0].TaxType);
            Assert.AreEqual(198.00m, invoiceDescriptor.TradeLineItems[0].LineTotalAmount);

            Assert.AreEqual("2", invoiceDescriptor.TradeLineItems[1].LineID);
            Assert.AreEqual("4000050986428", invoiceDescriptor.TradeLineItems[1].GlobalID.ID);
            Assert.AreEqual("0160", invoiceDescriptor.TradeLineItems[1].GlobalID.SchemeID);
            Assert.AreEqual("ARNR2", invoiceDescriptor.TradeLineItems[1].SellerAssignedID);
            Assert.AreEqual("Joghurt Banane", invoiceDescriptor.TradeLineItems[1].Name);
            Assert.AreEqual(50m, invoiceDescriptor.TradeLineItems[1].BilledQuantity);
            Assert.AreEqual(QuantityCodes.C62, invoiceDescriptor.TradeLineItems[0].UnitCode);
            Assert.AreEqual(5.5m, invoiceDescriptor.TradeLineItems[1].NetUnitPrice);
            Assert.AreEqual(5.5m, invoiceDescriptor.TradeLineItems[1].GrossUnitPrice);
            Assert.AreEqual(TaxCategoryCodes.S, invoiceDescriptor.TradeLineItems[1].TaxCategoryCode);
            Assert.AreEqual(7.0m, invoiceDescriptor.TradeLineItems[1].TaxPercent);
            Assert.AreEqual(TaxTypes.VAT, invoiceDescriptor.TradeLineItems[1].TaxType);
            Assert.AreEqual(275.00m, invoiceDescriptor.TradeLineItems[1].LineTotalAmount);

            Assert.AreEqual("0088", invoiceDescriptor.Seller.GlobalID.SchemeID);
            Assert.AreEqual("4000001123452", invoiceDescriptor.Seller.GlobalID.ID);
            Assert.AreEqual("Lieferant GmbH", invoiceDescriptor.Seller.Name);
            Assert.AreEqual("80333", invoiceDescriptor.Seller.Postcode);
            Assert.AreEqual("Lieferantenstraße 20", invoiceDescriptor.Seller.Street);
            Assert.AreEqual("München", invoiceDescriptor.Seller.City);
            Assert.AreEqual(CountryCodes.DE, invoiceDescriptor.Seller.Country);

            Assert.AreEqual("GE2020211", invoiceDescriptor.Buyer.ID);
            Assert.AreEqual("0088", invoiceDescriptor.Buyer.GlobalID.SchemeID);
            Assert.AreEqual("4000001987658", invoiceDescriptor.Buyer.GlobalID.ID);
            Assert.AreEqual("Kunden AG Mitte", invoiceDescriptor.Buyer.Name);
            Assert.AreEqual("69876", invoiceDescriptor.Buyer.Postcode);
            Assert.AreEqual("Kundenstraße 15", invoiceDescriptor.Buyer.Street);
            Assert.AreEqual("Frankfurt", invoiceDescriptor.Buyer.City);
            Assert.AreEqual(CountryCodes.DE, invoiceDescriptor.Buyer.Country);

            Assert.AreEqual("DE98ZZZ09999999999", invoiceDescriptor.PaymentMeans.SEPACreditorIdentifier);
            Assert.AreEqual("REF A-123", invoiceDescriptor.PaymentMeans.SEPAMandateReference);
            Assert.AreEqual(1, invoiceDescriptor.DebitorBankAccounts.Count);
            Assert.AreEqual("DE21860000000086001055", invoiceDescriptor.DebitorBankAccounts[0].IBAN);

            Assert.AreEqual("Der Betrag in Höhe von EUR 529,87 wird am 20.03.2018 von Ihrem Konto per SEPA-Lastschrift eingezogen.", invoiceDescriptor.PaymentTerms.Description.Trim());

            Assert.AreEqual(CurrencyCodes.EUR, invoiceDescriptor.Currency);
            Assert.AreEqual(473.00m, invoiceDescriptor.LineTotalAmount);
            Assert.AreEqual(0.00m, invoiceDescriptor.ChargeTotalAmount);
            Assert.AreEqual(0.00m, invoiceDescriptor.AllowanceTotalAmount);
            Assert.AreEqual(473.00m, invoiceDescriptor.TaxBasisAmount);
            Assert.AreEqual(56.87m, invoiceDescriptor.TaxTotalAmount);
            Assert.AreEqual(529.87m, invoiceDescriptor.GrandTotalAmount);
            Assert.AreEqual(0.00m, invoiceDescriptor.TotalPrepaidAmount);
            Assert.AreEqual(529.87m, invoiceDescriptor.DuePayableAmount);
        } // !TestSepaPreNotification_Load()

        [TestMethod]
        public void TestSepaPreNotification_Write()
        {
            var d = new InvoiceDescriptor();
            d.Type = InvoiceType.Invoice;
            d.InvoiceNo = "471102";
            d.Currency = CurrencyCodes.EUR;
            d.InvoiceDate = new DateTime(2018, 3, 5);
            d.AddTradeLineItem(
                lineID: "1",
                id: new GlobalID("0160", "4012345001235"),
                sellerAssignedID: "TB100A4",
                name: "Trennblätter A4",
                billedQuantity: 20m,
                unitCode: QuantityCodes.C62,
                netUnitPrice: 9.9m,
                grossUnitPrice: 9.9m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);
            d.AddTradeLineItem(
                lineID: "2",
                id: new GlobalID("0160", "4000050986428"),
                sellerAssignedID: "ARNR2",
                name: "Joghurt Banane",
                billedQuantity: 50m,
                unitCode: QuantityCodes.C62,
                netUnitPrice: 5.5m,
                grossUnitPrice: 5.5m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 7.0m,
                taxType: TaxTypes.VAT);
            d.SetSeller(
                id: null,
                globalID: new GlobalID("0088", "4000001123452"),
                name: "Lieferant GmbH",
                postcode: "80333",
                city: "München",
                street: "Lieferantenstraße 20",
                country: CountryCodes.DE);
            d.SetBuyer(
                id: "GE2020211",
                globalID: new GlobalID("0088", "4000001987658"),
                name: "Kunden AG Mitte",
                postcode: "69876",
                city: "Frankfurt",
                street: "Kundenstraße 15",
                country: CountryCodes.DE);
            d.SetPaymentMeansSepaDirectDebit(
                "DE98ZZZ09999999999",
                "REF A-123");
            d.AddDebitorFinancialAccount(
                "DE21860000000086001055", 
                null);
            d.SetTradePaymentTerms(
                "Der Betrag in Höhe von EUR 529,87 wird am 20.03.2018 von Ihrem Konto per SEPA-Lastschrift eingezogen.");
            d.SetTotals(
                473.00m,
                0.00m,
                0.00m,
                473.00m,
                56.87m,
                529.87m,
                0.00m,
                529.87m);
            d.SellerTaxRegistration.Add(new TaxRegistration
            {
                SchemeID = TaxRegistrationSchemeID.FC,
                No = "201/113/40209"
            });
            d.SellerTaxRegistration.Add(new TaxRegistration
            {
                SchemeID = TaxRegistrationSchemeID.VA,
                No = "DE123456789"
            });
            d.AddApplicableTradeTax(
                275.00m,
                7.00m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);
            d.AddApplicableTradeTax(
                198.00m,
                19.00m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);

            using (var stream = new MemoryStream())
            {
                d.Save(stream, ZUGFeRDVersion.Version20, Profile.Comfort);

                stream.Seek(0, SeekOrigin.Begin);

                var d2 = InvoiceDescriptor.Load(stream);
                Assert.AreEqual("DE98ZZZ09999999999", d2.PaymentMeans.SEPACreditorIdentifier);
                Assert.AreEqual("REF A-123", d2.PaymentMeans.SEPAMandateReference);
                Assert.AreEqual(1, d2.DebitorBankAccounts.Count);
                Assert.AreEqual("DE21860000000086001055", d2.DebitorBankAccounts[0].IBAN);
            }
        }
    }
}
