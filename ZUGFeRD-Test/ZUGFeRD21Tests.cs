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
using System.Linq;
using ZUGFeRD;

namespace ZUGFeRD_Test
{
    [TestClass]
    public class ZUGFeRD21Tests
    {
        InvoiceProvider InvoiceProvider = new InvoiceProvider();

        [TestMethod]
        public void TestReferenceBasicFacturXInvoice()
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
        } // !TestReferenceBasicFacturXInvoice()


        [TestMethod]
        public void TestStoringReferenceBasicFacturXInvoice()
        {
            string path = @"..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor originalDesc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(originalDesc.Profile, Profile.Basic);
            Assert.AreEqual(originalDesc.Type, InvoiceType.Invoice);
            Assert.AreEqual(originalDesc.InvoiceNo, "471102");
            Assert.AreEqual(originalDesc.TradeLineItems.Count, 1);
            Assert.IsTrue(originalDesc.TradeLineItems.TrueForAll(x => x.BillingPeriodStart == null));
            Assert.IsTrue(originalDesc.TradeLineItems.TrueForAll(x => x.BillingPeriodEnd == null));
            Assert.IsTrue(originalDesc.TradeLineItems.TrueForAll(x => x.ApplicableProductCharacteristics.Count == 0));
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
            Assert.IsTrue(desc.TradeLineItems.TrueForAll(x => x.BillingPeriodStart == null));
            Assert.IsTrue(desc.TradeLineItems.TrueForAll(x => x.BillingPeriodEnd == null));
            Assert.IsTrue(desc.TradeLineItems.TrueForAll(x => x.ApplicableProductCharacteristics.Count == 0));
            Assert.AreEqual(desc.LineTotalAmount, 198.0m);
            Assert.AreEqual(desc.Taxes[0].TaxAmount, 37.62m);
            Assert.AreEqual(desc.Taxes[0].Percent, 19.0m);
        } // !TestStoringReferenceBasicFacturXInvoice()


        [TestMethod]
        public void TestReferenceBasicWLInvoice()
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
        } // !TestReferenceBasicWLInvoice()


        [TestMethod]
        public void TestReferenceExtendedInvoice()
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
        } // !TestReferenceExtendedInvoice()


        [TestMethod]
        public void TestReferenceMinimumInvoice()
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
        } // !TestReferenceMinimumInvoice()


        [TestMethod]
        public void TestReferenceXRechnung1CII()
        {
            string path = @"..\..\..\demodata\xRechnung\xRechnung CII.xml";
            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            Assert.AreEqual(desc.Profile, Profile.XRechnung1);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "0815-99-1-a");
            Assert.AreEqual(desc.TradeLineItems.Count, 2);
            Assert.AreEqual(desc.LineTotalAmount, 1445.98m);
        } // !TestReferenceXRechnung1CII()


        [TestMethod]
        public void TestMinimumInvoice()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.Invoicee = new Party() // this information will not be stored in the output file since it is available in Extended profile only
            {
                Name = "Test"
            };
            desc.TaxBasisAmount = 73; // this information will not be stored in the output file since it is available in Extended profile only
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version21, Profile.Minimum);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Invoicee, null);
        } // !TestMinimumInvoice()


        [TestMethod]
        public void TestInvoiceWithAttachment()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            string filename = "myrandomdata.bin";
            byte[] data = new byte[32768];
            new Random().NextBytes(data);

            desc.AddAdditionalReferencedDocument(
                issuerAssignedID: "My-File",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                name: "Ausführbare Datei",
                attachmentBinaryObject: data,
                filename: filename);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version21, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            foreach (AdditionalReferencedDocument document in loadedInvoice.AdditionalReferencedDocuments)
            {
                if (document.IssuerAssignedID == "My-File")
                {
                    CollectionAssert.AreEqual(document.AttachmentBinaryObject, data);
                    Assert.AreEqual(document.Filename, filename);
                    break;
                }
            }
        } // !TestInvoiceWithAttachment()


        [TestMethod]
        public void TestXRechnung1()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version21, Profile.XRechnung1);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.Profile, Profile.XRechnung1);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Profile, Profile.XRechnung1);
        } // !TestXRechnung1()


        [TestMethod]
        public void TestXRechnung2()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version21, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.Profile, Profile.XRechnung);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Profile, Profile.XRechnung);
        } // !TestXRechnung2()

        [TestMethod]
        public void TestContractReferencedDocumentWithXRechnung()
        {
            string uuid = System.Guid.NewGuid().ToString();
            DateTime issueDateTime = DateTime.Today;

            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = uuid,
                IssueDateTime = issueDateTime
            };


            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version21, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.Profile, Profile.XRechnung);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.ContractReferencedDocument.ID, uuid);
            Assert.AreEqual(loadedInvoice.ContractReferencedDocument.IssueDateTime, null); // explicitly not to be set in XRechnung
        } // !TestContractReferencedDocumentWithXRechnung()


        [TestMethod]
        public void TestContractReferencedDocumentWithExtended()
        {
            string uuid = System.Guid.NewGuid().ToString();
            DateTime issueDateTime = DateTime.Today;

            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = uuid,
                IssueDateTime = issueDateTime
            };


            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version21, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.Profile, Profile.Extended);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.ContractReferencedDocument.ID, uuid);
            Assert.AreEqual(loadedInvoice.ContractReferencedDocument.IssueDateTime, issueDateTime); // explicitly not to be set in XRechnung
        } // !TestContractReferencedDocumentWithExtended()        


        [TestMethod]
        public void TestTotalRounding()
        {
            var uuid = Guid.NewGuid().ToString();
            var issueDateTime = DateTime.Today;

            var desc = InvoiceProvider.CreateInvoice();
            desc.ContractReferencedDocument = new ContractReferencedDocument
            {
                ID = uuid,
                IssueDateTime = issueDateTime
            };
            desc.SetTotals(1.99m, 0m, 0m, 0m, 0m, 2m, 0m, 2m, 0.01m);

            var msExtended = new MemoryStream();
            desc.Save(msExtended, ZUGFeRDVersion.Version21, Profile.Extended);
            msExtended.Seek(0, SeekOrigin.Begin);

            var loadedInvoice = InvoiceDescriptor.Load(msExtended);
            Assert.AreEqual(loadedInvoice.RoundingAmount, 0.01m);

            var msBasic = new MemoryStream();
            desc.Save(msBasic, ZUGFeRDVersion.Version21);
            msBasic.Seek(0, SeekOrigin.Begin);

            loadedInvoice = InvoiceDescriptor.Load(msBasic);
            Assert.AreEqual(loadedInvoice.RoundingAmount, 0m);
        } // !TestTotalRounding()

        [TestMethod]
        public void TestMissingPropertiesAreNull()
        {
            var path = @"..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.BillingPeriodStart == null));
            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.BillingPeriodEnd == null));
        } // !TestMissingPropertiesAreNull()


        [TestMethod]
        public void TestReadTradeLineSettlement()
        {
            var path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement data.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            var tradeLineItem = invoiceDescriptor.TradeLineItems.Single();
            // Test LineID
            tradeLineItem.LineID = "2"; 

            // Test BillingPeriod
            Assert.AreEqual(new DateTime(2021, 01, 01), tradeLineItem.BillingPeriodStart);
            Assert.AreEqual(new DateTime(2021, 01, 31), tradeLineItem.BillingPeriodEnd);

            // Test Product Characteristics
            var firstProductCharacteristic = tradeLineItem.ApplicableProductCharacteristics[0];
            Assert.AreEqual("METER_LOCATION", firstProductCharacteristic.Description);
            Assert.AreEqual("DE213410213", firstProductCharacteristic.Value);

            var secondProductCharacteristic = tradeLineItem.ApplicableProductCharacteristics[1];
            Assert.AreEqual("METER_NUMBER", secondProductCharacteristic.Description);
            Assert.AreEqual("123", secondProductCharacteristic.Value);

        } // !TestReadTradeLineSettlement()


        [TestMethod]
        public void TestTradeLineSettlement()
        {
            // Read XRechnung
            string path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor originalDesc = InvoiceDescriptor.Load(s);
            s.Close();

            // Modifiy trade line settlement data
            originalDesc.TradeLineItems.Add(new TradeLineItem()
            {
                BillingPeriodStart = new DateTime(2020, 1, 1),
                BillingPeriodEnd = new DateTime(2021, 1, 1),
                BilledQuantity = 5,
                ApplicableProductCharacteristics = new ApplicableProductCharacteristic[]
                {
                    new ApplicableProductCharacteristic()
                    {
                        Description = "Description_1_1",
                        Value = "Value_1_1"
                    },
                    new ApplicableProductCharacteristic()
                    {
                        Description = "Description_1_2",
                        Value = "Value_1_2"
                    },
                }.ToList(),
                NetUnitPrice = 1,
            });

            originalDesc.TradeLineItems.Add(new TradeLineItem()
            {
                BillingPeriodStart = new DateTime(2021, 1, 1),
                BillingPeriodEnd = new DateTime(2022, 1, 1),
                BilledQuantity = 10,
                ApplicableProductCharacteristics = new ApplicableProductCharacteristic[]
                {
                    new ApplicableProductCharacteristic()
                    {
                        Description = "Description_2_1",
                        Value = "Value_2_1"
                    },
                    new ApplicableProductCharacteristic()
                    {
                        Description = "Description_2_2",
                        Value = "Value_2_2"
                    },
                }.ToList(),
                NetUnitPrice = 1,
            });

            originalDesc.IsTest = false;

            Stream ms = new MemoryStream();
            originalDesc.Save(ms, ZUGFeRDVersion.Version21, Profile.Basic);
            originalDesc.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version21);

            // Load Invoice and compare to expected
            InvoiceDescriptor desc = InvoiceDescriptor.Load(ms);

            var firstTradeLineItem = desc.TradeLineItems[0];

            Assert.AreEqual(5, firstTradeLineItem.BilledQuantity);
            Assert.AreEqual(new DateTime(2020, 1, 1), firstTradeLineItem.BillingPeriodStart);
            Assert.AreEqual(new DateTime(2021, 1, 1), firstTradeLineItem.BillingPeriodEnd);
            Assert.AreEqual("Description_1_1", firstTradeLineItem.ApplicableProductCharacteristics[0].Description);
            Assert.AreEqual("Value_1_1", firstTradeLineItem.ApplicableProductCharacteristics[0].Value);
            Assert.AreEqual("Description_1_2", firstTradeLineItem.ApplicableProductCharacteristics[1].Description);
            Assert.AreEqual("Value_1_2", firstTradeLineItem.ApplicableProductCharacteristics[1].Value);

            var secondTradeLineItem = desc.TradeLineItems[1];

            Assert.AreEqual(10, secondTradeLineItem.BilledQuantity);
            Assert.AreEqual(new DateTime(2021, 1, 1), secondTradeLineItem.BillingPeriodStart);
            Assert.AreEqual(new DateTime(2022, 1, 1), secondTradeLineItem.BillingPeriodEnd);
            Assert.AreEqual("Description_2_1", secondTradeLineItem.ApplicableProductCharacteristics[0].Description);
            Assert.AreEqual("Value_2_1", secondTradeLineItem.ApplicableProductCharacteristics[0].Value);
            Assert.AreEqual("Description_2_2", secondTradeLineItem.ApplicableProductCharacteristics[1].Description);
            Assert.AreEqual("Value_2_2", secondTradeLineItem.ApplicableProductCharacteristics[1].Value);
        }
    }
}
