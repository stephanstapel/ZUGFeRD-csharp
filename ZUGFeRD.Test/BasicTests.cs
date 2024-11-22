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
    public class BasicTests : TestBase
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
            desc.AddTradeLineCommentItem(CUSTOM_LINE_ID, COMMENT);

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
    }
}
