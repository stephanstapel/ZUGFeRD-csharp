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
using System.Collections.Generic;
using System.Text;


namespace s2industries.ZUGFeRD.Test
{
    [TestClass]
    public class GlobalTests : TestBase
    {
        InvoiceProvider InvoiceProvider = new InvoiceProvider();


        [TestMethod]
        public void TestAutomaticLineIds()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();

            desc.AddTradeLineItem("Item1");
            desc.AddTradeLineItem("Item2");

            Assert.AreEqual(desc.TradeLineItems[0].AssociatedDocument.LineID, "1");
            Assert.AreEqual(desc.TradeLineItems[1].AssociatedDocument.LineID, "2");
        } // !TestAutomaticLineIds()



        [TestMethod]
        public void TestManualLineIds()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();
            desc.AddTradeLineItem(lineID: "item-01", "Item1");
            desc.AddTradeLineItem(lineID: "item-02", "Item2");

            Assert.AreEqual(desc.TradeLineItems[0].AssociatedDocument.LineID, "item-01");
            Assert.AreEqual(desc.TradeLineItems[1].AssociatedDocument.LineID, "item-02");
        } // !TestManualLineIds()


        [TestMethod]
        public void TestCommentLine()
        {
            string COMMENT = System.Guid.NewGuid().ToString();
            string CUSTOM_LINE_ID = System.Guid.NewGuid().ToString();

            // test with automatic line id
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            int numberOfTradeLineItems = desc.TradeLineItems.Count;
            desc.AddTradeLineCommentItem(COMMENT);

            Assert.AreEqual(numberOfTradeLineItems + 1, desc.TradeLineItems.Count);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes);
            Assert.AreEqual(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes.Count, 1);
            Assert.AreEqual(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes[0].Content, COMMENT);


            // test with manual line id
            desc = this.InvoiceProvider.CreateInvoice();
            numberOfTradeLineItems = desc.TradeLineItems.Count;
            desc.AddTradeLineCommentItem(lineID: CUSTOM_LINE_ID, comment: COMMENT);

            Assert.AreEqual(numberOfTradeLineItems + 1, desc.TradeLineItems.Count);            
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.LineID, CUSTOM_LINE_ID);
            Assert.IsNotNull(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes);
            Assert.AreEqual(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes.Count, 1);
            Assert.AreEqual(desc.TradeLineItems[desc.TradeLineItems.Count - 1].AssociatedDocument.Notes[0].Content, COMMENT);
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
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();
            Assert.ThrowsException<UnsupportedException>(() => desc.Save(ms, version, profile, ZUGFeRDFormats.UBL));
        } // !UBLNonAvailability()


        [TestMethod]        
        public void UBLAvailability()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
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
                allowanceChargeBasisAmount: -5m
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
        } // !SavingThenReadingAppliedTradeTaxes()
    }
}
