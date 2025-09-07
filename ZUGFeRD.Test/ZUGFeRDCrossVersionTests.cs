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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace s2industries.ZUGFeRD.Test
{
    [TestClass]
    public class ZUGFeRDCrossVersionTests : TestBase
    {
        private InvoiceProvider _InvoiceProvider = new InvoiceProvider();


        [TestMethod]
        public void TestAutomaticLineIds()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();

            desc.AddTradeLineItem("Item1", 0m, QuantityCodes.C62);
            desc.AddTradeLineItem("Item2", 0m, QuantityCodes.C62);

            Assert.AreEqual(desc.TradeLineItems[0].AssociatedDocument.LineID, "1");
            Assert.AreEqual(desc.TradeLineItems[1].AssociatedDocument.LineID, "2");
        } // !TestAutomaticLineIds()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.Extended)]
        public void TestNoteContentCodes(ZUGFeRDVersion version, Profile profile)
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.Notes.Clear();
            desc.AddNote("EEV", contentCode: ContentCodes.EEV);
            desc.AddNote("WEV", contentCode: ContentCodes.WEV);
            desc.AddNote("ST1", contentCode: ContentCodes.ST1);
            desc.AddNote("ST2", contentCode: ContentCodes.ST2);
            desc.AddNote("ST3", contentCode: ContentCodes.ST3);
            desc.AddNote("VEV", contentCode: ContentCodes.VEV);

            using MemoryStream ms = new();
            desc.Save(ms, version, profile);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            List<Note> notes = loadedInvoice.GetNotes();
            Assert.AreEqual(notes[0].ContentCode, ContentCodes.EEV);
            Assert.AreEqual(notes[1].ContentCode, ContentCodes.WEV);
            Assert.AreEqual(notes[2].ContentCode, ContentCodes.ST1);
            Assert.AreEqual(notes[3].ContentCode, ContentCodes.ST2);
            Assert.AreEqual(notes[4].ContentCode, ContentCodes.ST3);
            Assert.AreEqual(notes[5].ContentCode, ContentCodes.VEV);
        } // !TestNoteContentCodes()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.Extended)]
        public void TestNoteSubjectCodes(ZUGFeRDVersion version, Profile profile)
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.Notes.Clear();
            desc.AddNote("ACB", SubjectCodes.ACB);
            desc.AddNote("AAI", SubjectCodes.AAI);
            desc.AddNote("PRF", SubjectCodes.PRF);
            desc.AddNote("REG", SubjectCodes.REG);
            desc.AddNote("SUR", SubjectCodes.SUR);
            desc.AddNote("TXD", SubjectCodes.TXD);

            using MemoryStream ms = new();
            desc.Save(ms, version, profile);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            List<Note> notes = loadedInvoice.GetNotes();
            Assert.AreEqual(notes[0].SubjectCode, SubjectCodes.ACB);
            Assert.AreEqual(notes[1].SubjectCode, SubjectCodes.AAI);
            Assert.AreEqual(notes[2].SubjectCode, SubjectCodes.PRF);
            Assert.AreEqual(notes[3].SubjectCode, SubjectCodes.REG);
            Assert.AreEqual(notes[4].SubjectCode, SubjectCodes.SUR);
            Assert.AreEqual(notes[5].SubjectCode, SubjectCodes.TXD);
        } // !TestNoteSubjectCodes()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.Extended)]
        public void TestKosovoCountryCode(ZUGFeRDVersion version, Profile profile)
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.Seller = new Party()
            {
                Country = CountryCodes._1A,
            };

            using MemoryStream ms = new();
            desc.Save(ms, version, profile);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Seller.Country, CountryCodes._1A);
        } // !TestKosovoCountryCode()



        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.Extended)]
        public void TestStandardCountryCode(ZUGFeRDVersion version, Profile profile)
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.Seller = new Party()
            {
                Country = CountryCodes.US,
            };

            using MemoryStream ms = new();
            desc.Save(ms, version, profile);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Seller.Country, CountryCodes.US);
        } // !TestStandardCountryCode()


        [TestMethod]
        public void TestManualLineIds()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();
            desc.AddTradeLineItem(lineID: "item-01", "Item1", 0m, QuantityCodes.C62);
            desc.AddTradeLineItem(lineID: "item-02", "Item2", 0m, QuantityCodes.C62);

            Assert.AreEqual(desc.TradeLineItems[0].AssociatedDocument.LineID, "item-01");
            Assert.AreEqual(desc.TradeLineItems[1].AssociatedDocument.LineID, "item-02");
        } // !TestManualLineIds()


        [TestMethod]
        public void TestCommentLine()
        {
            string expectedComment = System.Guid.NewGuid().ToString();
            string expectedCustomLineId = System.Guid.NewGuid().ToString();

            // test with automatic line id
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            int numberOfTradeLineItems = desc.TradeLineItems.Count;
            desc.AddTradeLineCommentItem(expectedComment);

            Assert.AreEqual(numberOfTradeLineItems + 1, desc.TradeLineItems.Count);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes);
            Assert.AreEqual(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes.Count, 1);
            Assert.AreEqual(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes[0].Content, expectedComment);


            // test with manual line id
            desc = this._InvoiceProvider.CreateInvoice();
            numberOfTradeLineItems = desc.TradeLineItems.Count;
            desc.AddTradeLineCommentItem(lineID: expectedCustomLineId, comment: expectedComment);

            Assert.AreEqual(numberOfTradeLineItems + 1, desc.TradeLineItems.Count);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.LineID, expectedCustomLineId);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes);
            Assert.AreEqual(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes.Count, 1);
            Assert.AreEqual(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes[0].Content, expectedComment);
        } // !TestCommentLine()


        [TestMethod]
        public void TestGetVersion()
        {
            string path = @"..\..\..\..\demodata\zugferd10\ZUGFeRD_1p0_COMFORT_Einfach.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(path), ZUGFeRDVersion.Version1);

            path = @"..\..\..\..\demodata\zugferd20\zugferd_2p0_BASIC_Einfach.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(path), ZUGFeRDVersion.Version20);

            path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(path), ZUGFeRDVersion.Version23);

            path = @"..\..\..\..\demodata\xRechnung\ubl-cn-br-de-17-test-557-code-326.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(path), ZUGFeRDVersion.Version23);
        } // !TestGetVersion()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version1, Profile.XRechnung)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.XRechnung)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.XRechnung1)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.XRechnung1)]
        public void UBLNonAvailability(ZUGFeRDVersion version, Profile profile)
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();
            Assert.ThrowsException<UnsupportedException>(() => desc.Save(ms, version, profile, ZUGFeRDFormats.UBL));
        } // !UBLNonAvailability()


        [TestMethod]
        public void UBLAvailability()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
        } // !UBLAvailability()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.Extended)]
        public void SavingThenReadingAppliedTradeTaxes(ZUGFeRDVersion version, Profile profile)
        {
            InvoiceDescriptor expected = InvoiceDescriptor.CreateInvoice("123", new DateTime(2024, 12, 5), CurrencyCodes.EUR);
            var lineItem = expected.AddTradeLineItem(name: "Something",
                grossUnitPrice: 9.9m,
                netUnitPrice: 9.9m,
                unitCode: QuantityCodes.C62,
                billedQuantity: 20m,
                taxType: TaxTypes.VAT,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19m
                );
            lineItem.LineTotalAmount = 198m; // 20 * 9.9
            expected.AddApplicableTradeTax(
                basisAmount: lineItem.LineTotalAmount!.Value,
                percent: 19m,
                taxAmount: 29.82m, // 19% of 198
                typeCode: TaxTypes.VAT,
                categoryCode: TaxCategoryCodes.S,
                allowanceChargeBasisAmount: -5m,
                lineTotalBasisAmount: lineItem.LineTotalAmount!.Value
                );
            expected.LineTotalAmount = 198m;
            expected.TaxBasisAmount = 198m;
            expected.TaxTotalAmount = 29.82m;
            expected.GrandTotalAmount = 198m + 29.82m;
            expected.DuePayableAmount = expected.GrandTotalAmount;

            using MemoryStream ms = new();
            expected.Save(ms, version, profile);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor actual = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(expected.Taxes.Count, actual.Taxes.Count);
            Assert.AreEqual(1, actual.Taxes.Count);
            Tax actualTax = actual.Taxes[0];
            Assert.AreEqual(198m, actualTax.BasisAmount);
            Assert.AreEqual(19m, actualTax.Percent);
            Assert.AreEqual(29.82m, actualTax.TaxAmount);
            Assert.AreEqual(TaxTypes.VAT, actualTax.TypeCode);
            Assert.AreEqual(TaxCategoryCodes.S, actualTax.CategoryCode);
            Assert.AreEqual(-5m, actualTax.AllowanceChargeBasisAmount);
            Assert.AreEqual(198m, actualTax.LineTotalBasisAmount);
        } // !SavingThenReadingAppliedTradeTaxes()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1)]
        [DataRow(ZUGFeRDVersion.Version20)]
        [DataRow(ZUGFeRDVersion.Version23)]
        public void TestDeliveryNoteReferencedDocumentLineIdInExtended(ZUGFeRDVersion version)
        {
            string deliveryNoteNumber = "DeliveryNote-0815";
            DateTime deliveryNoteDate = DateTime.Today;
            string deliveryNoteLineID = "0815.001";

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            TradeLineItem line = desc.AddTradeLineItem("DeliveryNoteReferencedDocument-Text", 0m, QuantityCodes.C62);
            line.SetDeliveryNoteReferencedDocument(deliveryNoteNumber, deliveryNoteDate, deliveryNoteLineID);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            TradeLineItem? loadedLine = loadedInvoice.TradeLineItems.LastOrDefault();

            Assert.IsNotNull(loadedLine);
            Assert.IsNotNull(loadedLine?.DeliveryNoteReferencedDocument);
            Assert.AreEqual(deliveryNoteNumber, loadedLine?.DeliveryNoteReferencedDocument?.ID);
            Assert.AreEqual(deliveryNoteDate, loadedLine?.DeliveryNoteReferencedDocument?.IssueDateTime);
            Assert.AreEqual(deliveryNoteLineID, loadedLine?.DeliveryNoteReferencedDocument?.LineID);
        }

        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1)]
        [DataRow(ZUGFeRDVersion.Version20)]
        [DataRow(ZUGFeRDVersion.Version23)]
        public void TestContractReferencedDocumentLineIdInExtended(ZUGFeRDVersion version)
        {
            string contractNumber = "Contract-0815";
            DateTime contractDate = DateTime.Today;
            string contractLineID = "0815.001";

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            TradeLineItem line = desc.AddTradeLineItem("ContractReferencedDocument-Text", 0m, QuantityCodes.C62);
            line.SetContractReferencedDocument(contractNumber, contractDate, contractLineID);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            TradeLineItem? loadedLine = loadedInvoice.TradeLineItems.LastOrDefault();

            Assert.IsNotNull(loadedLine);
            Assert.IsNotNull(loadedLine?.ContractReferencedDocument);
            Assert.AreEqual(contractNumber, loadedLine?.ContractReferencedDocument?.ID);
            Assert.AreEqual(contractDate, loadedLine?.ContractReferencedDocument?.IssueDateTime);
            Assert.AreEqual(contractLineID, loadedLine?.ContractReferencedDocument?.LineID);
        } // !TestContractReferencedDocumentLineIdInExtended()


        [DataRow(ZUGFeRDVersion.Version1)]
        [DataRow(ZUGFeRDVersion.Version20)]
        [DataRow(ZUGFeRDVersion.Version23)]
        [TestMethod]
        public void TestLongerDecimalPlacesForNetUnitPrice(ZUGFeRDVersion version)
        {
            InvoiceDescriptor desc = _InvoiceProvider.CreateInvoice();

            desc.AddTradeLineItem("Item with 2 decimal places", netUnitPrice: 123.45m, unitCode: QuantityCodes.C62);
            desc.AddTradeLineItem("Item with 3 decimal places", netUnitPrice: 123.456m, unitCode: QuantityCodes.C62);
            desc.AddTradeLineItem("Item with 4 decimal places", netUnitPrice: 123.4567m, unitCode: QuantityCodes.C62);
            desc.AddTradeLineItem("Item with 5 decimal places", netUnitPrice: 123.45678m, unitCode: QuantityCodes.C62);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            List<TradeLineItem> items = loadedInvoice.GetTradeLineItems();

            Assert.AreEqual(items[items.Count - 4].NetUnitPrice, 123.45m);
            Assert.AreEqual(items[items.Count - 3].NetUnitPrice, 123.456m);
            Assert.AreEqual(items[items.Count - 2].NetUnitPrice, 123.4567m);
            Assert.AreEqual(items[items.Count - 1].NetUnitPrice, 123.4568m); // rounded!
        } // !TestLongerDecimalPlacesForNetUnitPrice()


        [DataRow(ZUGFeRDVersion.Version1)]
        [DataRow(ZUGFeRDVersion.Version20)]
        [DataRow(ZUGFeRDVersion.Version23)]
        [TestMethod]
        public void TestLongerDecimalPlacesForGrossUnitPrice(ZUGFeRDVersion version)
        {
            InvoiceDescriptor desc = _InvoiceProvider.CreateInvoice();

            desc.AddTradeLineItem("Item with 2 decimal places", netUnitPrice: 123.45m, unitCode: QuantityCodes.C62, grossUnitPrice: 123.45m);
            desc.AddTradeLineItem("Item with 2 decimal places", netUnitPrice: 123.456m, unitCode: QuantityCodes.C62, grossUnitPrice: 123.456m);
            desc.AddTradeLineItem("Item with 2 decimal places", netUnitPrice: 123.4567m, unitCode: QuantityCodes.C62, grossUnitPrice: 123.4567m);
            desc.AddTradeLineItem("Item with 2 decimal places", netUnitPrice: 123.45678m, unitCode: QuantityCodes.C62, grossUnitPrice: 123.45678m);            

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            List<TradeLineItem> items = loadedInvoice.GetTradeLineItems();

            Assert.AreEqual(items[items.Count - 4].GrossUnitPrice, 123.45m);
            Assert.AreEqual(items[items.Count - 3].GrossUnitPrice, 123.456m);
            Assert.AreEqual(items[items.Count - 2].GrossUnitPrice, 123.4567m);
            Assert.AreEqual(items[items.Count - 1].GrossUnitPrice, 123.4568m); // rounded!

            Assert.AreEqual(items[items.Count - 4].NetUnitPrice, 123.45m);
            Assert.AreEqual(items[items.Count - 3].NetUnitPrice, 123.456m);
            Assert.AreEqual(items[items.Count - 2].NetUnitPrice, 123.4567m);
            Assert.AreEqual(items[items.Count - 1].NetUnitPrice, 123.4568m); // rounded!
        } // !TestLongerDecimalPlacesForGrossUnitPrice()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version20, Profile.Extended, ZUGFeRDFormats.CII)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.Extended, ZUGFeRDFormats.CII)]
        [DataRow(ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL)]
        public void TestSellerTaxRepresentative(ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format)
        {
            string name = System.Guid.NewGuid().ToString();

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.SellerTaxRepresentative = new Party()
            {
                Name = name
            };

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format);
            ms.Position = 0;

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.IsNotNull(loadedInvoice.SellerTaxRepresentative);
            Assert.AreEqual(name, loadedInvoice.SellerTaxRepresentative.Name);
        } // !TestSellerTaxRepresentative()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII)]
        public void TestSellerTaxRepresentativeInNonSupportedVersions(ZUGFeRDVersion version, ZUGFeRDFormats format)
        {
            string name = System.Guid.NewGuid().ToString();

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.SellerTaxRepresentative = new Party()
            {
                Name = name
            };

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, Profile.Extended, format);
            ms.Position = 0;

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.IsNull(loadedInvoice.SellerTaxRepresentative);            
        } // !TestSellerTaxRepresentativeInNonSupportedVersions()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1)]
        [DataRow(ZUGFeRDVersion.Version20)]
        [DataRow(ZUGFeRDVersion.Version23)]
        public void TestTransportModeWithExtended(ZUGFeRDVersion version)
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TransportMode = TransportModeCodes.Road;

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.TransportMode, TransportModeCodes.Road);
            Assert.AreEqual(3, (int)TransportModeCodes.Road);
        } // !TestTransportModeWithExtended()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1)]
        [DataRow(ZUGFeRDVersion.Version20)]
        [DataRow(ZUGFeRDVersion.Version23)]
        public void TestTransportModeWithComfort(ZUGFeRDVersion version)
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TransportMode = TransportModeCodes.Road;

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.Comfort);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.IsNull(loadedInvoice.TransportMode); // not supported in comfort profile
        } // !TestTransportModeWithExtended()


        // Test for rule PEPPOL-EN16931-R046
        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version20)]
        [DataRow(ZUGFeRDVersion.Version23)]
        public void TestGrossPriceRepresentationForXRechnungAndNotXRechnungNegativeCase(ZUGFeRDVersion version)
        {
            decimal grossPrice = 10.1m;
            decimal netPrice = 10.0m;

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();

            TradeLineItem item = desc.AddTradeLineItem("Test",
                                                       netPrice, // net unit price
                                                       QuantityCodes.C62,
                                                       "Test",                                                       
                                                       1,
                                                       grossPrice, // gross unit price
                                                       1);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);

            InvoiceDescriptor zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems().Count, 1);
            Assert.AreEqual(zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GrossUnitPrice, grossPrice);
            Assert.AreEqual(zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GetTradeAllowanceCharges().Count, 0);

            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);

            InvoiceDescriptor zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems().Count, 1);
            Assert.AreEqual(zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GrossUnitPrice, null);
            Assert.AreEqual(zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GetTradeAllowanceCharges().Count, 0);
        } // !TestGrossPriceRepresentationForXRechnungAndNotXRechnungNegativeCase()


        // Test for rule PEPPOL-EN16931-R046
        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version20)]
        [DataRow(ZUGFeRDVersion.Version23)]
        public void TestGrossPriceRepresentationForXRechnungAndNotXRechnungPositiveCase(ZUGFeRDVersion version)
        {
            decimal grossPrice = 10.1m;
            decimal netPrice = 10.0m;
            // Is currently unused, therefore commented out.
            //decimal discountPercent = 10.0m;
            decimal discountAmount = 0.1m;

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();

            TradeLineItem item = desc.AddTradeLineItem("Test",
                                                       netPrice, // net unit price
                                                       QuantityCodes.C62,
                                                       "Test",                                                       
                                                       1,
                                                       grossPrice, // gross unit price
                                                       1);

            item.AddTradeAllowance(CurrencyCodes.EUR, grossPrice, discountAmount, "Discount", AllowanceReasonCodes.Discount);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);

            InvoiceDescriptor zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems().Count, 1);
            Assert.AreEqual(zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GrossUnitPrice, grossPrice);
            Assert.AreEqual(zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GetTradeAllowanceCharges().Count, 1);
            Assert.AreEqual(zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GetTradeAllowanceCharges()[0].BasisAmount, grossPrice);
            Assert.AreEqual(zugferd23ExtendedInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GetTradeAllowanceCharges()[0].ActualAmount, discountAmount);

            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);

            InvoiceDescriptor zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems().Count, 1);
            Assert.AreEqual(zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GrossUnitPrice, grossPrice);
            Assert.AreEqual(zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GetTradeAllowanceCharges().Count, 1);
            Assert.IsNull(zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GetTradeAllowanceCharges()[0].BasisAmount); // not written in XRechnung
            Assert.AreEqual(zugferd23XRechnungInvoiceWithGrossWithoutAllowanceCharge.GetTradeLineItems()[0].GetTradeAllowanceCharges()[0].ActualAmount, discountAmount);
        } // !TestGrossPriceRepresentationForXRechnungAndNotXRechnungPositiveCase()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.UBL, Profile.XRechnung)]
        public void TestHeaderComment(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            InvoiceProvider provider = new InvoiceProvider();
            InvoiceDescriptor desc = provider.CreateInvoice();

            string headerComment = System.Guid.NewGuid().ToString();
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format, InvoiceOptionsBuilder.Create().AddHeaderXmlComment(headerComment).EnableXmlComments().Build());

            string content = Encoding.UTF8.GetString(ms.ToArray());

            Assert.IsTrue(content.Contains(headerComment));
        } // !TestHeaderComment()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.UBL, Profile.XRechnung)]
        public void TestWihoutHeaderComment(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            InvoiceProvider provider = new InvoiceProvider();
            InvoiceDescriptor desc = provider.CreateInvoice();

            string headerComment = System.Guid.NewGuid().ToString();
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format, InvoiceOptionsBuilder.Create().AddHeaderXmlComment(headerComment).EnableXmlComments(false).Build());

            string content = Encoding.UTF8.GetString(ms.ToArray());
            Assert.IsFalse(content.Contains(headerComment));
        } // !TestWihoutHeaderComment()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.CII, Profile.Extended)]
        public void TestZUGFeRDElementComments(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            InvoiceProvider provider = new InvoiceProvider();
            InvoiceDescriptor desc = provider.CreateInvoice();

            string headerComment = System.Guid.NewGuid().ToString();
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format, InvoiceOptionsBuilder.Create().EnableXmlComments(true).Build());

            List<string> lines = Encoding.UTF8.GetString(ms.ToArray()).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            bool onItemLevel = false;
            for (int i = 1; i < lines.Count; i++)
            {
                if (lines[i].Contains("<ram:IncludedSupplyChainTradeLineItem>"))
                {
                    onItemLevel = true;
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }
                else if (lines[i].Contains("</ram:IncludedSupplyChainTradeLineItem>"))
                {
                    onItemLevel = false;
                }

                if (lines[i].Contains("<ram:NetPriceProductTradePrice>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // totals per item
                if (version == ZUGFeRDVersion.Version1)
                {
                    if (lines[i].Contains("<ram:SpecifiedTradeSettlementMonetarySummation>"))
                    {
                        Assert.IsTrue(lines[i - 1].Contains("<!--"));
                    }
                }
                else
                {
                    if (lines[i].Contains("<ram:SpecifiedTradeSettlementLineMonetarySummation>"))
                    {
                        Assert.IsTrue(lines[i - 1].Contains("<!--"));
                    }
                }                               

                // totals on header level
                if (version == ZUGFeRDVersion.Version1)
                {
                    if (lines[i].Contains("<ram:SpecifiedTradeSettlementMonetarySummation>"))
                    {
                        Assert.IsTrue(lines[i - 1].Contains("<!--"));
                    }
                }
                else
                {
                    if (lines[i].Contains("<ram:SpecifiedTradeSettlementHeaderMonetarySummation>"))
                    {
                        Assert.IsTrue(lines[i - 1].Contains("<!--"));
                    }
                }

                // header trade agreement (buyer, seller, ...)
                if (lines[i].Contains("<ram:ApplicableHeaderTradeAgreement>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // buyer reference
                if (lines[i].Contains("<ram:BuyerReference>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // seller
                if (lines[i].Contains("<ram:SellerTradeParty>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // buyer
                if (lines[i].Contains("<ram:BuyerTradeParty>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // buyer order information
                if (lines[i].Contains("<ram:BuyerOrderReferencedDocument>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // delivery information
                if (lines[i].Contains("<ram:ApplicableHeaderTradeDelivery>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // delivery note information
                if (lines[i].Contains("<ram:DespatchAdviceReferencedDocument>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // document information
                if (lines[i].Contains("<ram:ApplicableHeaderTradeSettlement>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // payment means
                if (lines[i].Contains("<ram:SpecifiedTradeSettlementPaymentMeans>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // tax
                if (!onItemLevel && lines[i].Contains("<ram:ApplicableTradeTax>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }
            }

        } // !TestZUGFeRDElementComments()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.UBL, Profile.XRechnung)]
        public void TestXRechnungElementComments(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            InvoiceProvider provider = new InvoiceProvider();
            InvoiceDescriptor desc = provider.CreateInvoice();

            string headerComment = System.Guid.NewGuid().ToString();
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format, InvoiceOptionsBuilder.Create().EnableXmlComments(true).Build());

            List<string> lines = Encoding.UTF8.GetString(ms.ToArray()).Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 1; i < lines.Count; i++)
            {
                // invoice line
                // test: IncludedSupplyChainTradeLineItemComment
                if (lines[i].Contains("<cac:InvoiceLine>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // item price
                // test: NetPriceProductTradePriceComment
                if (lines[i].Contains("<cbc:PriceAmount>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // totals on item level
                // test: SpecifiedTradeSettlementLineMonetarySummationComment
                if (lines[i].Contains("<cbc:LineExtensionAmount>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // totals on header level
                // test: SpecifiedTradeSettlementHeaderMonetarySummationComment
                if (lines[i].Contains("<cac:LegalMonetaryTotal>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // buyer
                // test: BuyerTradePartyComment
                if (lines[i].Contains("<cac:AccountingSupplierParty>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // seller
                // test: SellerTradePartyComment
                if (lines[i].Contains("<cac:AccountingSupplierParty>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // buyer, seller information etc.
                // test: BuyerOrderReferencedDocumentComment
                if (lines[i].Contains("<cac:OrderReference>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // delivery note information
                // test: DespatchAdviceReferencedDocumentComment
                if (lines[i].Contains("<cac:DespatchDocumentReference>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // payment means
                // test: SpecifiedTradeSettlementPaymentMeansComment
                if (lines[i].Contains("<cac:PaymentMeans>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }

                // tax information
                // test: ApplicableTradeTaxComment
                if (lines[i].Contains("<cac:TaxSubtotal>"))
                {
                    Assert.IsTrue(lines[i - 1].Contains("<!--"));
                }
            }

        } // !TestXRechnungElementComments()



        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.UBL, Profile.XRechnung)]
        public void TestInvalidXmlWithException(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            InvoiceDescriptor desc = new InvoiceProvider().CreateInvoice();
            desc.InvoiceNo = "\u001b";
            var invoiceStream = new MemoryStream();

            Assert.ThrowsException<IllegalCharacterException>(() => desc.Save(invoiceStream, version, profile, format));
        } // !TestInvalidXmlWithException()



        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.UBL, Profile.XRechnung)]
        public void TestInvalidXmlWithCleaning(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            InvoiceDescriptor desc = new InvoiceProvider().CreateInvoice();
            desc.InvoiceNo = "ABC\u001bDEF";

            InvoiceFormatOptions options = InvoiceOptionsBuilder.Create()
                .AutomaticallyCleanInvalidCharacters()
                .Build();
            var invoiceStream = new MemoryStream();
            desc.Save(invoiceStream, version, profile, format, options);
            string result = Encoding.UTF8.GetString(invoiceStream.ToArray());

            Assert.IsTrue(result.Contains("ABCDEF"), "The illegal character should be removed from the invoice number.");
        } // !TestInvalidXmlWithCleaning()



        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.CII, Profile.Extended)]
        public void TestGrossQuantity(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            decimal? desiredNetUnitQuantity = 20.0m;
            decimal? desiredGrossUnitQuantity = 23.0m;

            InvoiceDescriptor desc = new InvoiceProvider().CreateInvoice();

            foreach(TradeLineItem item in desc.TradeLineItems)
            {
                item.NetQuantity = desiredNetUnitQuantity;
                item.GrossQuantity = desiredGrossUnitQuantity;
            }

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format);            

            InvoiceDescriptor loadedDescriptor = InvoiceDescriptor.Load(ms);
            foreach (TradeLineItem item in loadedDescriptor.TradeLineItems)
            {
                Assert.IsNotNull(item.NetQuantity);
                Assert.AreEqual(desiredNetUnitQuantity, item.NetQuantity);

                Assert.IsNotNull(item.GrossQuantity);
                Assert.AreEqual(desiredGrossUnitQuantity, item.GrossQuantity);
            }
        } // !TestGrossQuantity()



        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.UBL, Profile.XRechnung)]
        public void TestGrossQuantityForXRechnung(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            decimal? desiredNetUnitQuantity = 20.0m;
            decimal? desiredGrossUnitQuantity = 23.0m;

            InvoiceDescriptor desc = new InvoiceProvider().CreateInvoice();

            foreach(TradeLineItem item in desc.TradeLineItems)
            {
                item.NetQuantity = desiredNetUnitQuantity;
                item.GrossQuantity = desiredGrossUnitQuantity;
            }

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format);            

            InvoiceDescriptor loadedDescriptor = InvoiceDescriptor.Load(ms);
            foreach (TradeLineItem item in loadedDescriptor.TradeLineItems)
            {
                Assert.IsNotNull(item.NetQuantity);
                Assert.AreEqual(desiredNetUnitQuantity, item.NetQuantity);

                Assert.IsNull(item.GrossQuantity);
            }
        } // !TestGrossQuantityForXRechnung()



        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.UBL, Profile.XRechnung)]
        public void TestWithoutGrossQuantity(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            decimal? desiredNetUnitQuantity = 20.0m;
            decimal? desiredGrossUnitQuantity = null;

            InvoiceDescriptor desc = new InvoiceProvider().CreateInvoice();

            foreach(TradeLineItem item in desc.TradeLineItems)
            {
                item.NetQuantity = desiredNetUnitQuantity;
                item.GrossQuantity = desiredGrossUnitQuantity;
            }

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format);

            InvoiceDescriptor loadedDescriptor = InvoiceDescriptor.Load(ms);
            foreach (TradeLineItem item in loadedDescriptor.TradeLineItems)
            {
                Assert.IsNotNull(item.NetQuantity);
                Assert.AreEqual(desiredNetUnitQuantity, item.NetQuantity);

                Assert.IsNull(item.GrossQuantity);
            }
        } // !TestWithoutGrossQuantity()


        [TestMethod]
        [DataRow(ZUGFeRDVersion.Version1, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version20, ZUGFeRDFormats.CII, Profile.Extended)]
        [DataRow(ZUGFeRDVersion.Version23, ZUGFeRDFormats.CII, Profile.Extended)]
        public void TestNulledGlobalIDScheme(ZUGFeRDVersion version, ZUGFeRDFormats format, Profile profile)
        {
            // Is currently unused, therefore commented out.
            //decimal? desiredNetUnitQuantity = 20.0m;
            //decimal? desiredGrossUnitQuantity = 23.0m;

            InvoiceDescriptor desc = new InvoiceProvider().CreateInvoice();

            desc.Seller.GlobalID = new GlobalID() { ID = "123" };
            desc.Buyer.GlobalID = new GlobalID() { ID = "213" };

            foreach(TradeLineItem item in desc.GetTradeLineItems())
            {
                item.GlobalID.SchemeID = null;
            }

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, version, profile, format);            

            InvoiceDescriptor loadedDescriptor = InvoiceDescriptor.Load(ms);
            Assert.IsNull(loadedDescriptor.Seller.GlobalID.SchemeID);
            Assert.IsNull(loadedDescriptor.Buyer.GlobalID.SchemeID);

            foreach (TradeLineItem item in loadedDescriptor.TradeLineItems)
            {
                Assert.IsNull(item.GlobalID.SchemeID);
            }
        } // !TestNulledGlobalIDScheme()
    }
}
