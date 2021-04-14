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
        }

        [TestMethod]
        public void TestMissingPropertiesAreEmpty()
        {
            var path = @"..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.ApplicableProductCharacteristics.Count == 0));
        }

        [TestMethod]
        public void TestReadTradeLineBillingPeriod()
        {
            var path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement data.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            var tradeLineItem = invoiceDescriptor.TradeLineItems.Single();
            Assert.AreEqual(new DateTime(2021, 01, 01), tradeLineItem.BillingPeriodStart);
            Assert.AreEqual(new DateTime(2021, 01, 31), tradeLineItem.BillingPeriodEnd);
        }

        [TestMethod]
        public void TestReadTradeLineLineID()
        {
            var path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement data.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);
            var tradeLineItem = invoiceDescriptor.TradeLineItems.Single();
            Assert.AreEqual("2", tradeLineItem.LineID);
        }

        [TestMethod]
        public void TestReadTradeLineProductCharacteristics()
        {
            var path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement data.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);
            var tradeLineItem = invoiceDescriptor.TradeLineItems.Single();

            var firstProductCharacteristic = tradeLineItem.ApplicableProductCharacteristics[0];
            Assert.AreEqual("METER_LOCATION", firstProductCharacteristic.Description);
            Assert.AreEqual("DE213410213", firstProductCharacteristic.Value);

            var secondProductCharacteristic = tradeLineItem.ApplicableProductCharacteristics[1];
            Assert.AreEqual("METER_NUMBER", secondProductCharacteristic.Description);
            Assert.AreEqual("123", secondProductCharacteristic.Value);
        }


        [TestMethod]
        public void TestWriteTradeLineProductCharacteristics()
        {
            var path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.TradeLineItems.Add(new TradeLineItem()
            {
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
            });

            originalInvoiceDescriptor.TradeLineItems.Add(new TradeLineItem()
            {
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
                }.ToList()
            });

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version21, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version21);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(memoryStream);

                var firstTradeLineItem = invoiceDescriptor.TradeLineItems[0];
                Assert.AreEqual("Description_1_1", firstTradeLineItem.ApplicableProductCharacteristics[0].Description);
                Assert.AreEqual("Value_1_1", firstTradeLineItem.ApplicableProductCharacteristics[0].Value);
                Assert.AreEqual("Description_1_2", firstTradeLineItem.ApplicableProductCharacteristics[1].Description);
                Assert.AreEqual("Value_1_2", firstTradeLineItem.ApplicableProductCharacteristics[1].Value);

                var secondTradeLineItem = invoiceDescriptor.TradeLineItems[1];
                Assert.AreEqual("Description_2_1", secondTradeLineItem.ApplicableProductCharacteristics[0].Description);
                Assert.AreEqual("Value_2_1", secondTradeLineItem.ApplicableProductCharacteristics[0].Value);
                Assert.AreEqual("Description_2_2", secondTradeLineItem.ApplicableProductCharacteristics[1].Description);
                Assert.AreEqual("Value_2_2", secondTradeLineItem.ApplicableProductCharacteristics[1].Value);
            }
        }

        [TestMethod]
        public void TestWriteTradeLineBillingPeriod()
        {
            // Read XRechnung
            string path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor originalInvoiceDescriptor = InvoiceDescriptor.Load(s);
            s.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.TradeLineItems.Add(new TradeLineItem()
            {
                BillingPeriodStart = new DateTime(2020, 1, 1),
                BillingPeriodEnd = new DateTime(2021, 1, 1),
            });

            originalInvoiceDescriptor.TradeLineItems.Add(new TradeLineItem()
            {
                BillingPeriodStart = new DateTime(2021, 1, 1),
                BillingPeriodEnd = new DateTime(2022, 1, 1)
            });

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version21, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version21);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(memoryStream);

                var firstTradeLineItem = invoiceDescriptor.TradeLineItems[0];
                Assert.AreEqual(new DateTime(2020, 1, 1), firstTradeLineItem.BillingPeriodStart);
                Assert.AreEqual(new DateTime(2021, 1, 1), firstTradeLineItem.BillingPeriodEnd);

                var secondTradeLineItem = invoiceDescriptor.TradeLineItems[1];
                Assert.AreEqual(new DateTime(2021, 1, 1), secondTradeLineItem.BillingPeriodStart);
                Assert.AreEqual(new DateTime(2022, 1, 1), secondTradeLineItem.BillingPeriodEnd);
            }
        }

        [TestMethod]
        public void TestWriteTradeLineBilledQuantity()
        {
            // Read XRechnung
            var path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.TradeLineItems.Add(new TradeLineItem()
            {
                BilledQuantity = 10,
                NetUnitPrice = 1
            });

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version21, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version21);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(memoryStream);
                Assert.AreEqual(10, invoiceDescriptor.TradeLineItems[0].BilledQuantity);
            }
        }

        [TestMethod]
        public void TestWriteTradeLineNetUnitPrice()
        {
            // Read XRechnung
            var path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.TradeLineItems.Add(new TradeLineItem()
            {
                NetUnitPrice = 25
            });

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version21, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version21);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(memoryStream);
                Assert.AreEqual(25, invoiceDescriptor.TradeLineItems[0].NetUnitPrice);
            }
        }

        [TestMethod]
        public void TestWriteTradeLineLineID()
        {
            // Read XRechnung
            var path = @"..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.TradeLineItems.RemoveAll(_ => true);

            originalInvoiceDescriptor.AddTradeLineCommentItem("2", "Comment_2");
            originalInvoiceDescriptor.AddTradeLineCommentItem("3", "Comment_3");
            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version21, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version21);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(@"xrechnung with trade line settlement filled.xml");

                var firstTradeLineItem = invoiceDescriptor.TradeLineItems[0];
                Assert.AreEqual("2", firstTradeLineItem.LineID);

                var secondTradeLineItem = invoiceDescriptor.TradeLineItems[1];
                Assert.AreEqual("3", secondTradeLineItem.LineID);
            }
        }
    }
}