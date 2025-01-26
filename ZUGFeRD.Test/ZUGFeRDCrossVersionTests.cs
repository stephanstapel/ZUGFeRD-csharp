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
        }
    }
}
