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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            desc.AddTradeLineItem("Item1");
            desc.AddTradeLineItem("Item2");

            Assert.AreEqual(desc.TradeLineItems[0].AssociatedDocument.LineID, "1");
            Assert.AreEqual(desc.TradeLineItems[1].AssociatedDocument.LineID, "2");
        } // !TestAutomaticLineIds()



        [TestMethod]
        public void TestManualLineIds()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();
            desc.AddTradeLineItem(lineID: "item-01", "Item1");
            desc.AddTradeLineItem(lineID: "item-02", "Item2");

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

            TradeLineItem line = desc.AddTradeLineItem("DeliveryNoteReferencedDocument-Text");
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

            TradeLineItem line = desc.AddTradeLineItem("ContractReferencedDocument-Text");
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

            desc.AddTradeLineItem("Item with 2 decimal places", netUnitPrice: 123.45m);
            desc.AddTradeLineItem("Item with 3 decimal places", netUnitPrice: 123.456m);
            desc.AddTradeLineItem("Item with 4 decimal places", netUnitPrice: 123.4567m);
            desc.AddTradeLineItem("Item with 5 decimal places", netUnitPrice: 123.45678m);

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

            desc.AddTradeLineItem("Item with 2 decimal places", grossUnitPrice: 123.45m);
            desc.AddTradeLineItem("Item with 3 decimal places", grossUnitPrice: 123.456m);
            desc.AddTradeLineItem("Item with 4 decimal places", grossUnitPrice: 123.4567m);
            desc.AddTradeLineItem("Item with 5 decimal places", grossUnitPrice: 123.45678m);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            List<TradeLineItem> items = loadedInvoice.GetTradeLineItems();

            Assert.AreEqual(items[items.Count - 4].GrossUnitPrice, 123.45m);
            Assert.AreEqual(items[items.Count - 3].GrossUnitPrice, 123.456m);
            Assert.AreEqual(items[items.Count - 2].GrossUnitPrice, 123.4567m);
            Assert.AreEqual(items[items.Count - 1].GrossUnitPrice, 123.4568m); // rounded!
        } // !TestLongerDecimalPlacesForGrossUnitPrice()
    }
}
