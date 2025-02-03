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
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;
using s2industries.ZUGFeRD;


namespace s2industries.ZUGFeRD.Test
{
    [TestClass]
    public class ZUGFeRD22Tests : TestBase
    {
        private InvoiceProvider _InvoiceProvider = new InvoiceProvider();


        [TestMethod]
        public void TestLineStatusCode()
        {
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_EXTENDED_Warenrechnung-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            desc.TradeLineItems.Clear();

            desc.AddTradeLineItem(
                name: "Trennblätter A4",
                billedQuantity: 20m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 9.9m,
                grossUnitPrice: 9.9m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT)
            .SetLineStatus(LineStatusCodes.New, LineStatusReasonCodes.DETAIL);

            desc.AddTradeLineItem(
                name: "Joghurt Banane",
                billedQuantity: 50m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 5.5m,
                grossUnitPrice: 5.5m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 7.0m,
                taxType: TaxTypes.VAT);

            desc.AddTradeLineItem(
                name: "Abschlagsrechnung vom 01.01.2024",
                billedQuantity: -1m,
                unitCode: QuantityCodes.C62,
                netUnitPrice: 500,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT)
            .SetLineStatus(LineStatusCodes.DocumentationClaim, LineStatusReasonCodes.INFORMATION);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.TradeLineItems.Count, 3);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].AssociatedDocument.LineStatusCode, LineStatusCodes.New);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].AssociatedDocument.LineStatusReasonCode, LineStatusReasonCodes.DETAIL);
            Assert.AreEqual(loadedInvoice.TradeLineItems[1].AssociatedDocument.LineStatusCode, null);
            Assert.AreEqual(loadedInvoice.TradeLineItems[1].AssociatedDocument.LineStatusReasonCode, null);
            Assert.AreEqual(loadedInvoice.TradeLineItems[2].AssociatedDocument.LineStatusCode, LineStatusCodes.DocumentationClaim);
            Assert.AreEqual(loadedInvoice.TradeLineItems[2].AssociatedDocument.LineStatusReasonCode, LineStatusReasonCodes.INFORMATION);
        }

        [TestMethod]
        public void TestExtendedInvoiceWithIncludedItems()
        {
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_EXTENDED_Warenrechnung-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            desc.TradeLineItems.Clear();

            desc.AddTradeLineItem(
                lineID: "1",
                name: "Trennblätter A4",
                billedQuantity: 20m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 9.9m,
                grossUnitPrice: 9.9m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT)
            .AddIncludedReferencedProduct("Test", 1, QuantityCodes.C62)
            .AddIncludedReferencedProduct("Test2");

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.TradeLineItems.Count, 1);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].IncludedReferencedProducts.Count, 2);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].IncludedReferencedProducts[0].Name, "Test");
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].IncludedReferencedProducts[0].UnitQuantity, 1);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].IncludedReferencedProducts[0].UnitCode, QuantityCodes.C62);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].IncludedReferencedProducts[1].Name, "Test2");
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].IncludedReferencedProducts[1].UnitQuantity.HasValue, false);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].IncludedReferencedProducts[1].UnitCode, null);
        }

        [TestMethod]
        public void TestReferenceEReportingFacturXInvoice()
        {
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_EREPORTING-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.EReporting);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "471102");
            Assert.AreEqual(desc.TradeLineItems.Count, 0);
            Assert.IsNull(desc.LineTotalAmount); // not present in file
            Assert.AreEqual(desc.TaxBasisAmount, 198.0m);
            Assert.AreEqual(desc.IsTest, false); // not present in file
        }

        [TestMethod]
        public void TestReferenceBasicFacturXInvoice()
        {
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

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
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

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
            originalDesc.Save(ms, ZUGFeRDVersion.Version23, Profile.Basic);
            originalDesc.Save(@"zugferd_2p1_BASIC_Einfach-factur-x_Result.xml", ZUGFeRDVersion.Version23);

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
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_BASIC-WL_Einfach-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

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
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_EXTENDED_Warenrechnung-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.Extended);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "R87654321012345");
            Assert.AreEqual(desc.TradeLineItems.Count, 6);
            Assert.AreEqual(desc.LineTotalAmount, 457.20m);

            IList<TradeAllowanceCharge> tradeAllowanceCharges = desc.GetTradeAllowanceCharges();
            foreach (TradeAllowanceCharge charge in tradeAllowanceCharges)
            {
                Assert.AreEqual(charge.Tax.TypeCode, TaxTypes.VAT);
                Assert.AreEqual(charge.Tax.CategoryCode, TaxCategoryCodes.S);
            }

            Assert.AreEqual(tradeAllowanceCharges.Count, 4);
            Assert.AreEqual(tradeAllowanceCharges[0].Tax.Percent, 19m);
            Assert.AreEqual(tradeAllowanceCharges[1].Tax.Percent, 7m);
            Assert.AreEqual(tradeAllowanceCharges[2].Tax.Percent, 19m);
            Assert.AreEqual(tradeAllowanceCharges[3].Tax.Percent, 7m);

            Assert.AreEqual(desc.ServiceCharges.Count, 1);
            Assert.AreEqual(desc.ServiceCharges[0].Tax.TypeCode, TaxTypes.VAT);
            Assert.AreEqual(desc.ServiceCharges[0].Tax.CategoryCode, TaxCategoryCodes.S);
            Assert.AreEqual(desc.ServiceCharges[0].Tax.Percent, 19m);
        } // !TestReferenceExtendedInvoice()


        [TestMethod]
        public void TestReferenceMinimumInvoice()
        {
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_MINIMUM_Rechnung-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Profile, Profile.Minimum);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "471102");
            Assert.AreEqual(desc.TradeLineItems.Count, 0);
            Assert.IsNull(desc.LineTotalAmount); // not present in file
            Assert.AreEqual(desc.TaxBasisAmount, 198.0m);
        } // !TestReferenceMinimumInvoice()


        [TestMethod]
        public void TestReferenceXRechnung1CII()
        {
            string path = @"..\..\..\..\demodata\xRechnung\xRechnung CII.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            Assert.AreEqual(desc.Profile, Profile.XRechnung1);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "0815-99-1-a");
            Assert.AreEqual(desc.TradeLineItems.Count, 2);
            Assert.AreEqual(desc.LineTotalAmount, 1445.98m);
        } // !TestReferenceXRechnung1CII()

        [TestMethod]
        public void TestElectronicAddress()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.SetSellerElectronicAddress("DE123456789", ElectronicAddressSchemeIdentifiers.GermanyVatNumber);
            desc.SetBuyerElectronicAddress("LU987654321", ElectronicAddressSchemeIdentifiers.LuxemburgVatNumber);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.SellerElectronicAddress.Address, "DE123456789");
            Assert.AreEqual(desc.SellerElectronicAddress.ElectronicAddressSchemeID, ElectronicAddressSchemeIdentifiers.GermanyVatNumber);
            Assert.AreEqual(desc.BuyerElectronicAddress.Address, "LU987654321");
            Assert.AreEqual(desc.BuyerElectronicAddress.ElectronicAddressSchemeID, ElectronicAddressSchemeIdentifiers.LuxemburgVatNumber);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.SellerElectronicAddress.Address, "DE123456789");
            Assert.AreEqual(loadedInvoice.SellerElectronicAddress.ElectronicAddressSchemeID, ElectronicAddressSchemeIdentifiers.GermanyVatNumber);
            Assert.AreEqual(loadedInvoice.BuyerElectronicAddress.Address, "LU987654321");
            Assert.AreEqual(loadedInvoice.BuyerElectronicAddress.ElectronicAddressSchemeID, ElectronicAddressSchemeIdentifiers.LuxemburgVatNumber);
        } // !TestElectronicAddress()


        [TestMethod]
        public void TestMinimumInvoice()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.Invoicee = new Party() // this information will not be stored in the output file since it is available in Extended profile only
            {
                Name = "Invoicee"
            };
            desc.Seller = new Party()
            {
                Name = "Seller",
                SpecifiedLegalOrganization = new LegalOrganization()
                {
                    TradingBusinessName = "Trading business name for seller party"
                }
            };
            desc.TaxBasisAmount = 73; // this information will not be stored in the output file since it is available in Extended profile only
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Minimum);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Invoicee, null);
            Assert.AreNotEqual(loadedInvoice.Seller, null);
            Assert.AreNotEqual(loadedInvoice.Seller.SpecifiedLegalOrganization, null);
            Assert.AreEqual(loadedInvoice.Seller.SpecifiedLegalOrganization.TradingBusinessName, String.Empty);
        } // !TestMinimumInvoice()


        [TestMethod]
        public void TestInvoiceWithAttachmentXRechnung()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            string filename = "myrandomdata.bin";
            byte[] data = new byte[32768];
            new Random().NextBytes(data);

            desc.AddAdditionalReferencedDocument(
                id: "My-File",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                name: "Ausführbare Datei",
                attachmentBinaryObject: data,
                filename: filename);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.AdditionalReferencedDocuments.Count, 1);

            foreach (AdditionalReferencedDocument document in loadedInvoice.AdditionalReferencedDocuments)
            {
                if (document.ID == "My-File")
                {
                    CollectionAssert.AreEqual(document.AttachmentBinaryObject, data);
                    Assert.AreEqual(document.Filename, filename);
                    break;
                }
            }
        } // !TestInvoiceWithAttachmentXRechnung()


        [TestMethod]
        public void TestInvoiceWithAttachmentExtended()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            string filename = "myrandomdata.bin";
            byte[] data = new byte[32768];
            new Random().NextBytes(data);

            desc.AddAdditionalReferencedDocument(
                id: "My-File",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                name: "Ausführbare Datei",
                attachmentBinaryObject: data,
                filename: filename);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.AdditionalReferencedDocuments.Count, 1);

            foreach (AdditionalReferencedDocument document in loadedInvoice.AdditionalReferencedDocuments)
            {
                if (document.ID == "My-File")
                {
                    CollectionAssert.AreEqual(document.AttachmentBinaryObject, data);
                    Assert.AreEqual(document.Filename, filename);
                    break;
                }
            }
        } // !TestInvoiceWithAttachmentExtended()


        [TestMethod]
        public void TestInvoiceWithAttachmentComfort()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            string filename = "myrandomdata.bin";
            byte[] data = new byte[32768];
            new Random().NextBytes(data);

            desc.AddAdditionalReferencedDocument(
                id: "My-File",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                name: "Ausführbare Datei",
                attachmentBinaryObject: data,
                filename: filename);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Comfort);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.AdditionalReferencedDocuments.Count, 1);

            foreach (AdditionalReferencedDocument document in loadedInvoice.AdditionalReferencedDocuments)
            {
                if (document.ID == "My-File")
                {
                    CollectionAssert.AreEqual(document.AttachmentBinaryObject, data);
                    Assert.AreEqual(document.Filename, filename);
                    break;
                }
            }
        } // !TestInvoiceWithAttachmentComfort()


        [TestMethod]
        public void TestInvoiceWithAttachmentBasic()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            string filename = "myrandomdata.bin";
            byte[] data = new byte[32768];
            new Random().NextBytes(data);

            desc.AddAdditionalReferencedDocument(
                id: "My-File",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                name: "Ausführbare Datei",
                attachmentBinaryObject: data,
                filename: filename);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Basic);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.AdditionalReferencedDocuments.Count, 0);
        } // !TestInvoiceWithAttachmentBasic()


        [TestMethod]
        public void TestXRechnung1()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung1);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.Profile, Profile.XRechnung1);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Profile, Profile.XRechnung1);
        } // !TestXRechnung1()


        [TestMethod]
        public void TestXRechnung2()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
            Assert.AreEqual(desc.Profile, Profile.XRechnung);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Profile, Profile.XRechnung);
        } // !TestXRechnung2()


        [TestMethod]
        public void TestCreateInvoice_WithProfileEReporting()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.EReporting);
            Assert.AreEqual(desc.Profile, Profile.EReporting);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Profile, Profile.EReporting);
        } // !TestCreateInvoice_WithProfileEReporting()


        [TestMethod]
        public void TestBuyerOrderReferencedDocumentWithExtended()
        {
            string uuid = System.Guid.NewGuid().ToString();
            DateTime orderDate = DateTime.Today;

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.SetBuyerOrderReferenceDocument(uuid, orderDate);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.Profile, Profile.Extended);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.OrderNo, uuid);
            Assert.AreEqual(loadedInvoice.OrderDate, orderDate); // explicitly not to be set in XRechnung, see separate test case
        } // !TestBuyerOrderReferencedDocumentWithExtended()


        [TestMethod]
        public void TestBuyerOrderReferencedDocumentWithXRechnung()
        {
            string uuid = System.Guid.NewGuid().ToString();
            DateTime orderDate = DateTime.Today;

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.SetBuyerOrderReferenceDocument(uuid, orderDate);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.Profile, Profile.XRechnung);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.OrderNo, uuid);
            Assert.AreEqual(loadedInvoice.OrderDate, null); // explicitly not to be set in XRechnung, see separate test case
        } // !TestBuyerOrderReferencedDocumentWithXRechnung()


        [TestMethod]
        public void TestContractReferencedDocumentWithXRechnung()
        {
            string uuid = System.Guid.NewGuid().ToString();
            DateTime issueDateTime = DateTime.Today;

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = uuid,
                IssueDateTime = issueDateTime
            };


            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
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

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.ContractReferencedDocument = new ContractReferencedDocument()
            {
                ID = uuid,
                IssueDateTime = issueDateTime
            };


            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(desc.Profile, Profile.Extended);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.ContractReferencedDocument.ID, uuid);
            Assert.AreEqual(loadedInvoice.ContractReferencedDocument.IssueDateTime, issueDateTime); // explicitly not to be set in XRechnung, see separate test case
        } // !TestContractReferencedDocumentWithExtended()


        [TestMethod]
        public void TestTotalRoundingExtended()
        {
            var uuid = Guid.NewGuid().ToString();
            var issueDateTime = DateTime.Today;

            var desc = _InvoiceProvider.CreateInvoice();
            desc.ContractReferencedDocument = new ContractReferencedDocument
            {
                ID = uuid,
                IssueDateTime = issueDateTime
            };
            desc.SetTotals(1.99m, 0m, 0m, 0m, 0m, 2m, 0m, 2m, 0.01m);

            var msExtended = new MemoryStream();
            desc.Save(msExtended, ZUGFeRDVersion.Version23, Profile.Extended);
            msExtended.Seek(0, SeekOrigin.Begin);

            var loadedInvoice = InvoiceDescriptor.Load(msExtended);
            Assert.AreEqual(loadedInvoice.RoundingAmount, 0.01m);

            var msBasic = new MemoryStream();
            desc.Save(msBasic, ZUGFeRDVersion.Version23);
            msBasic.Seek(0, SeekOrigin.Begin);

            loadedInvoice = InvoiceDescriptor.Load(msBasic);
            Assert.IsNull(loadedInvoice.RoundingAmount);
        } // !TestTotalRoundingExtended()


        [TestMethod]
        public void TestTotalRoundingXRechnung()
        {
            var uuid = Guid.NewGuid().ToString();
            var issueDateTime = DateTime.Today;

            var desc = _InvoiceProvider.CreateInvoice();
            desc.ContractReferencedDocument = new ContractReferencedDocument
            {
                ID = uuid,
                IssueDateTime = issueDateTime
            };
            desc.SetTotals(1.99m, 0m, 0m, 0m, 0m, 2m, 0m, 2m, 0.01m);

            var msExtended = new MemoryStream();
            desc.Save(msExtended, ZUGFeRDVersion.Version23, Profile.XRechnung);
            msExtended.Seek(0, SeekOrigin.Begin);

            var loadedInvoice = InvoiceDescriptor.Load(msExtended);
            Assert.AreEqual(loadedInvoice.RoundingAmount, 0.01m);

            var msBasic = new MemoryStream();
            desc.Save(msBasic, ZUGFeRDVersion.Version23);
            msBasic.Seek(0, SeekOrigin.Begin);

            loadedInvoice = InvoiceDescriptor.Load(msBasic);
            Assert.IsNull(loadedInvoice.RoundingAmount);
        } // !TestTotalRoundingExtended()



        [TestMethod]
        public void TestMissingPropertiesAreNull()
        {
            var path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.BillingPeriodStart == null));
            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.BillingPeriodEnd == null));
        }

        [TestMethod]
        public void TestMissingPropertiesAreEmpty()
        {
            var path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_BASIC_Einfach-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.ApplicableProductCharacteristics.Count == 0));
        }

        [TestMethod]
        public void TestReadTradeLineBillingPeriod()
        {
            var path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement data.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            var tradeLineItem = invoiceDescriptor.TradeLineItems.Single();
            Assert.AreEqual(new DateTime(2021, 01, 01), tradeLineItem.BillingPeriodStart);
            Assert.AreEqual(new DateTime(2021, 01, 31), tradeLineItem.BillingPeriodEnd);
        }

        [TestMethod]
        public void TestReadTradeLineLineID()
        {
            var path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement data.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var invoiceDescriptor = InvoiceDescriptor.Load(path);
            var tradeLineItem = invoiceDescriptor.TradeLineItems.Single();
            Assert.AreEqual("2", tradeLineItem.AssociatedDocument.LineID);
        }

        [TestMethod]
        public void TestReadTradeLineProductCharacteristics()
        {
            var path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement data.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

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
            var path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            TradeLineItem item0 = originalInvoiceDescriptor.AddTradeLineItem(name: String.Empty);
            item0.ApplicableProductCharacteristics = new List<ApplicableProductCharacteristic>()
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
                }
            };

            TradeLineItem item1 = originalInvoiceDescriptor.AddTradeLineItem(name: String.Empty);
            item1.ApplicableProductCharacteristics = new List<ApplicableProductCharacteristic>()
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
                }
            };

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version23, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version23);

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
            string path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor originalInvoiceDescriptor = InvoiceDescriptor.Load(s);
            s.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.AddTradeLineItem(
                name: String.Empty,
                billingPeriodStart: new DateTime(2020, 1, 1),
                billingPeriodEnd: new DateTime(2021, 1, 1));

            originalInvoiceDescriptor.AddTradeLineItem(
                name: String.Empty,
                billingPeriodStart: new DateTime(2021, 1, 1),
                billingPeriodEnd: new DateTime(2022, 1, 1));

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version23, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version23);

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
            var path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.AddTradeLineItem(
                name: String.Empty,
                billedQuantity: 10,
                netUnitPrice: 1);

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version23, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version23);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(memoryStream);
                Assert.AreEqual(10, invoiceDescriptor.TradeLineItems[0].BilledQuantity);
            }
        }

        [TestMethod]
        public void TestWriteTradeLineChargeFreePackage()
        {
            // Read XRechnung
            var path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.AddTradeLineItem(name: String.Empty)
                .SetChargeFreeQuantity(10, QuantityCodes.C62)
                .SetPackageQuantity(20, QuantityCodes.C62);

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version23, Profile.Extended);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version23, Profile.Extended);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(memoryStream);
                Assert.AreEqual(10, invoiceDescriptor.TradeLineItems[0].ChargeFreeQuantity);
                Assert.AreEqual(20, invoiceDescriptor.TradeLineItems[0].PackageQuantity);
            }
        }
        [TestMethod]
        public void TestWriteTradeLineNetUnitPrice()
        {
            // Read XRechnung
            var path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.AddTradeLineItem(name: String.Empty, netUnitPrice: 25);

            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version23, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version23);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(memoryStream);
                Assert.AreEqual(25, invoiceDescriptor.TradeLineItems[0].NetUnitPrice);
            }
        }

        [TestMethod]
        public void TestWriteTradeLineLineID()
        {
            // Read XRechnung
            var path = @"..\..\..\..\demodata\xRechnung\xrechnung with trade line settlement empty.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var fileStream = File.Open(path, FileMode.Open);
            var originalInvoiceDescriptor = InvoiceDescriptor.Load(fileStream);
            fileStream.Close();

            // Modifiy trade line settlement data
            originalInvoiceDescriptor.TradeLineItems.RemoveAll(_ => true);

            originalInvoiceDescriptor.AddTradeLineCommentItem(lineID: "2", comment: "Comment_2");
            originalInvoiceDescriptor.AddTradeLineCommentItem(lineID: "3", comment: "Comment_3");
            originalInvoiceDescriptor.IsTest = false;

            using (var memoryStream = new MemoryStream())
            {
                originalInvoiceDescriptor.Save(memoryStream, ZUGFeRDVersion.Version23, Profile.Basic);
                originalInvoiceDescriptor.Save(@"xrechnung with trade line settlement filled.xml", ZUGFeRDVersion.Version23);

                // Load Invoice and compare to expected
                var invoiceDescriptor = InvoiceDescriptor.Load(@"xrechnung with trade line settlement filled.xml");

                var firstTradeLineItem = invoiceDescriptor.TradeLineItems[0];
                Assert.AreEqual("2", firstTradeLineItem.AssociatedDocument.LineID);

                var secondTradeLineItem = invoiceDescriptor.TradeLineItems[1];
                Assert.AreEqual("3", secondTradeLineItem.AssociatedDocument.LineID);
            }
        }

        // BR-DE-13
        [TestMethod]
        public void TestLoadingSepaPreNotification()
        {
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_EN16931_SEPA_Prenotification.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            var invoiceDescriptor = InvoiceDescriptor.Load(path);
            Assert.AreEqual(Profile.Comfort, invoiceDescriptor.Profile);
            Assert.AreEqual(InvoiceType.Invoice, invoiceDescriptor.Type);

            Assert.AreEqual("DE98ZZZ09999999999", invoiceDescriptor.PaymentMeans.SEPACreditorIdentifier);
            Assert.AreEqual("REF A-123", invoiceDescriptor.PaymentMeans.SEPAMandateReference);
            Assert.AreEqual(1, invoiceDescriptor.DebitorBankAccounts.Count);
            Assert.AreEqual("DE21860000000086001055", invoiceDescriptor.DebitorBankAccounts[0].IBAN);

            Assert.AreEqual("Der Betrag in Höhe von EUR 529,87 wird am 20.03.2018 von Ihrem Konto per SEPA-Lastschrift eingezogen.",
                invoiceDescriptor.GetTradePaymentTerms().FirstOrDefault().Description.Trim());
        } // !TestLoadingSepaPreNotification()


        [TestMethod]
        public void TestStoringSepaPreNotification()
        {
            var d = new InvoiceDescriptor();
            d.Type = InvoiceType.Invoice;
            d.InvoiceNo = "471102";
            d.Currency = CurrencyCodes.EUR;
            d.InvoiceDate = new DateTime(2018, 3, 5);
            d.AddTradeLineItem(
                lineID: "1",
                id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4012345001235"),
                sellerAssignedID: "TB100A4",
                name: "Trennblätter A4",
                billedQuantity: 20m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 9.9m,
                grossUnitPrice: 9.9m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);
            d.AddTradeLineItem(
                lineID: "2",
                id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4000050986428"),
                sellerAssignedID: "ARNR2",
                name: "Joghurt Banane",
                billedQuantity: 50m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 5.5m,
                grossUnitPrice: 5.5m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 7.0m,
                taxType: TaxTypes.VAT);
            d.SetSeller(
                id: null,
                globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"),
                name: "Lieferant GmbH",
                postcode: "80333",
                city: "München",
                street: "Lieferantenstraße 20",
                country: CountryCodes.DE,
                legalOrganization: new LegalOrganization(GlobalIDSchemeIdentifiers.GLN, "4000001123452", "Lieferant GmbH"));
            d.SetBuyer(
                id: "GE2020211",
                globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001987658"),
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
            d.AddTradePaymentTerms(
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
            d.SellerTaxRegistration.Add(
                new TaxRegistration
                {
                    SchemeID = TaxRegistrationSchemeID.FC,
                    No = "201/113/40209"
                });
            d.SellerTaxRegistration.Add(
                new TaxRegistration
                {
                    SchemeID = TaxRegistrationSchemeID.VA,
                    No = "DE123456789"
                });
            d.AddApplicableTradeTax(
                275.00m,
                7.00m,
                275.00m / 100m * 7.00m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);
            d.AddApplicableTradeTax(
                198.00m,
                19.00m,
                198.00m / 100m * 19.00m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);

            using (var stream = new MemoryStream())
            {
                d.Save(stream, ZUGFeRDVersion.Version23, Profile.Comfort);

                stream.Seek(0, SeekOrigin.Begin);

                var d2 = InvoiceDescriptor.Load(stream);
                Assert.AreEqual("DE98ZZZ09999999999", d2.PaymentMeans.SEPACreditorIdentifier);
                Assert.AreEqual("REF A-123", d2.PaymentMeans.SEPAMandateReference);
                Assert.AreEqual(1, d2.DebitorBankAccounts.Count);
                Assert.AreEqual("DE21860000000086001055", d2.DebitorBankAccounts[0].IBAN);
                Assert.IsTrue(d.Seller.SpecifiedLegalOrganization.ID.SchemeID.HasValue);
                Assert.AreEqual("0088", d.Seller.SpecifiedLegalOrganization.ID.SchemeID.Value.EnumToString());
                Assert.AreEqual("4000001123452", d.Seller.SpecifiedLegalOrganization.ID.ID);
                Assert.AreEqual("Lieferant GmbH", d.Seller.SpecifiedLegalOrganization.TradingBusinessName);
            }
        } // !TestStoringSepaPreNotification()


        [TestMethod]
        public void TestValidTaxTypes()
        {
            InvoiceDescriptor invoice = _InvoiceProvider.CreateInvoice();
            invoice.TradeLineItems.ForEach(i => i.TaxType = TaxTypes.VAT);

            MemoryStream ms = new MemoryStream();
            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.Basic);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.BasicWL);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.Comfort);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.Extended);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.XRechnung1);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            invoice.TradeLineItems.ForEach(i => i.TaxType = TaxTypes.AAA);
            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.XRechnung);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            // extended profile supports other tax types as well:
            invoice.TradeLineItems.ForEach(i => i.TaxType = TaxTypes.AAA);
            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.Extended);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }
        } // !TestValidTaxTypes()


        [TestMethod]
        public void TestInvalidTaxTypes()
        {
            InvoiceDescriptor invoice = _InvoiceProvider.CreateInvoice();
            invoice.TradeLineItems.ForEach(i => i.TaxType = TaxTypes.AAA);

            MemoryStream ms = new MemoryStream();
            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.Basic);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.BasicWL);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.Comfort);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.Comfort);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            // allowed in extended profile

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.XRechnung1);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }

            try
            {
                invoice.Save(ms, version: ZUGFeRDVersion.Version23, profile: Profile.XRechnung);
            }
            catch (UnsupportedException)
            {
                Assert.Fail();
            }
        } // !TestInvalidTaxTypes()


        [TestMethod]
        public void TestAdditionalReferencedDocument()
        {
            string id = Guid.NewGuid().ToString();
            string uriID = Guid.NewGuid().ToString();
            DateTime issueDateTime = DateTime.Today;

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            // BG-24
            desc.AddAdditionalReferencedDocument(
                id: id,
                typeCode: AdditionalReferencedDocumentTypeCode.InvoiceDataSheet,
                referenceTypeCode: ReferenceTypeCodes.AAB,
                issueDateTime: issueDateTime,
                name: "Invoice Data Sheet",
                uriID: uriID);
            desc.AddAdditionalReferencedDocument(
                id: id+"2",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                referenceTypeCode: ReferenceTypeCodes.PP,
                issueDateTime: issueDateTime,
                name: "Reference Document");

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(2, loadedInvoice.AdditionalReferencedDocuments.Count);
            // checks for 1st document
            Assert.AreEqual("Invoice Data Sheet", loadedInvoice.AdditionalReferencedDocuments[0].Name);
            Assert.AreEqual(issueDateTime, loadedInvoice.AdditionalReferencedDocuments[0].IssueDateTime);
            Assert.AreEqual(id, loadedInvoice.AdditionalReferencedDocuments[0].ID);
            Assert.AreEqual(uriID, loadedInvoice.AdditionalReferencedDocuments[0].URIID);
            Assert.IsNull(loadedInvoice.AdditionalReferencedDocuments[0].LineID);
            Assert.AreEqual(ReferenceTypeCodes.Unknown, loadedInvoice.AdditionalReferencedDocuments[0].ReferenceTypeCode);
            Assert.AreEqual(AdditionalReferencedDocumentTypeCode.InvoiceDataSheet, loadedInvoice.AdditionalReferencedDocuments[0].TypeCode);
            // checks for 2nd document
            Assert.AreEqual("Reference Document", loadedInvoice.AdditionalReferencedDocuments[1].Name);
            Assert.AreEqual(issueDateTime, loadedInvoice.AdditionalReferencedDocuments[1].IssueDateTime);
            Assert.AreEqual(id+"2", loadedInvoice.AdditionalReferencedDocuments[1].ID);
            Assert.IsNull(loadedInvoice.AdditionalReferencedDocuments[1].URIID);
            Assert.IsNull(loadedInvoice.AdditionalReferencedDocuments[1].LineID);
            Assert.AreEqual(ReferenceTypeCodes.Unknown, loadedInvoice.AdditionalReferencedDocuments[1].ReferenceTypeCode);
        } // !TestAdditionalReferencedDocument()


        [TestMethod]
        public void TestPartyExtensions()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.Invoicee = new Party() // most of this information will NOT be stored in the output file
            {
                Name = "Invoicee",
                ContactName = "Max Mustermann",
                Postcode = "83022",
                City = "Rosenheim",
                Street = "Münchnerstraße 123",
                AddressLine3 = "EG links",
                CountrySubdivisionName = "Bayern",
                Country = CountryCodes.DE
            };

            desc.Payee = new Party() // most of this information will NOT be stored in the output file
            {
                Name = "Payee",
                ContactName = "Max Mustermann",
                Postcode = "83022",
                City = "Rosenheim",
                Street = "Münchnerstraße 123",
                AddressLine3 = "EG links",
                CountrySubdivisionName = "Bayern",
                Country = CountryCodes.DE
            };

            MemoryStream ms = new MemoryStream();

            // test with Comfort
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Comfort);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.IsNull(loadedInvoice.Invoicee);
            Assert.IsNotNull(loadedInvoice.Seller);
            Assert.IsNotNull(loadedInvoice.Payee);

            Assert.AreEqual(loadedInvoice.Seller.Name, "Lieferant GmbH");
            Assert.AreEqual(loadedInvoice.Seller.Street, "Lieferantenstraße 20");
            Assert.AreEqual(loadedInvoice.Seller.City, "München");
            Assert.AreEqual(loadedInvoice.Seller.Postcode, "80333");

            Assert.AreEqual(loadedInvoice.Payee.Name, "Payee");
            Assert.IsNull(loadedInvoice.Payee.ContactName);
            Assert.AreEqual(loadedInvoice.Payee.Postcode, String.Empty);
            Assert.AreEqual(loadedInvoice.Payee.City, String.Empty);
            Assert.AreEqual(loadedInvoice.Payee.Street, String.Empty);
            Assert.AreEqual(loadedInvoice.Payee.AddressLine3, String.Empty);
            Assert.AreEqual(loadedInvoice.Payee.CountrySubdivisionName, String.Empty);
            Assert.AreEqual(loadedInvoice.Payee.Country, CountryCodes.Unknown);


            // test with Extended
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Invoicee.Name, "Invoicee");
            Assert.AreEqual(loadedInvoice.Invoicee.ContactName, "Max Mustermann");
            Assert.AreEqual(loadedInvoice.Invoicee.Postcode, "83022");
            Assert.AreEqual(loadedInvoice.Invoicee.City, "Rosenheim");
            Assert.AreEqual(loadedInvoice.Invoicee.Street, "Münchnerstraße 123");
            Assert.AreEqual(loadedInvoice.Invoicee.AddressLine3, "EG links");
            Assert.AreEqual(loadedInvoice.Invoicee.CountrySubdivisionName, "Bayern");
            Assert.AreEqual(loadedInvoice.Invoicee.Country, CountryCodes.DE);

            Assert.AreEqual(loadedInvoice.Seller.Name, "Lieferant GmbH");
            Assert.AreEqual(loadedInvoice.Seller.Street, "Lieferantenstraße 20");
            Assert.AreEqual(loadedInvoice.Seller.City, "München");
            Assert.AreEqual(loadedInvoice.Seller.Postcode, "80333");

            Assert.AreEqual(loadedInvoice.Payee.Name, "Payee");
            Assert.AreEqual(loadedInvoice.Payee.ContactName, "Max Mustermann");
            Assert.AreEqual(loadedInvoice.Payee.Postcode, "83022");
            Assert.AreEqual(loadedInvoice.Payee.City, "Rosenheim");
            Assert.AreEqual(loadedInvoice.Payee.Street, "Münchnerstraße 123");
            Assert.AreEqual(loadedInvoice.Payee.AddressLine3, "EG links");
            Assert.AreEqual(loadedInvoice.Payee.CountrySubdivisionName, "Bayern");
            Assert.AreEqual(loadedInvoice.Payee.Country, CountryCodes.DE);

            //
            // Check the output in the XML for Comfort.
            // REM: In Comfort only ID, GlobalID, Name, and SpecifiedLegalOrganization are allowed.

            desc.Payee = new Party() {
                ID = new GlobalID(GlobalIDSchemeIdentifiers.Unknown, "SL1001"),
                Name = "Max Mustermann"
                // Country is not set and should not be written into the XML
            };

            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Comfort);
            ms.Seek(0, SeekOrigin.Begin);

            XDocument doc = XDocument.Load(ms);
            XNamespace rsm = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100";
            XNamespace ram = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100";

            Assert.AreEqual(doc.Root
                ?.Element(rsm + "SupplyChainTradeTransaction")
                ?.Element(ram + "ApplicableHeaderTradeSettlement")
                ?.Element(ram + "PayeeTradeParty")
                ?.Element(ram + "ID")?.Value, "SL1001");
            Assert.AreEqual(doc.Root
                ?.Element(rsm + "SupplyChainTradeTransaction")
                ?.Element(ram + "ApplicableHeaderTradeSettlement")
                ?.Element(ram + "PayeeTradeParty")
                ?.Element(ram + "Name")?.Value, "Max Mustermann");
            Assert.IsNull(doc.Root
                ?.Element(rsm + "SupplyChainTradeTransaction")
                ?.Element(ram + "ApplicableHeaderTradeSettlement")
                ?.Element(ram + "PayeeTradeParty")
                ?.Element(ram + "PostalTradeAddress")); // !!!

        } // !TestMinimumInvoice()


        [TestMethod]
        public void TestShipTo() {

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            desc.ShipTo = new Party() {
                ID = new GlobalID(GlobalIDSchemeIdentifiers.Unknown, "SL1001"),
                GlobalID = new GlobalID(GlobalIDSchemeIdentifiers.GLN, "MusterGLN"),
                Name = "AbKunden AG Mitte",
                Postcode = "12345",
                ContactName = "Einheit: 5.OG rechts",
                Street = "Verwaltung Straße 40",
                City = "Musterstadt",
                Country = CountryCodes.DE,
                CountrySubdivisionName = "Hessen"
            };

            // test minimum
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Minimum);
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNull(loadedInvoice.ShipTo);

            // test basic
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Basic);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.ShipTo);
            Assert.AreEqual(loadedInvoice.ShipTo.ID.ID, "SL1001");
            Assert.AreEqual(loadedInvoice.ShipTo.GlobalID.ID, "MusterGLN");
            Assert.AreEqual(loadedInvoice.ShipTo.GlobalID.SchemeID, GlobalIDSchemeIdentifiers.GLN);
            Assert.AreEqual(loadedInvoice.ShipTo.Name, "AbKunden AG Mitte");
            Assert.AreEqual(loadedInvoice.ShipTo.Postcode, "12345");
            Assert.AreEqual(loadedInvoice.ShipTo.ContactName, "Einheit: 5.OG rechts");
            Assert.AreEqual(loadedInvoice.ShipTo.Street, "Verwaltung Straße 40");
            Assert.AreEqual(loadedInvoice.ShipTo.City, "Musterstadt");
            Assert.AreEqual(loadedInvoice.ShipTo.Country, CountryCodes.DE);
            Assert.AreEqual(loadedInvoice.ShipTo.CountrySubdivisionName, "Hessen");

            // test basic wl
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.BasicWL);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.ShipTo);
            Assert.AreEqual(loadedInvoice.ShipTo.ID.ID, "SL1001");
            Assert.AreEqual(loadedInvoice.ShipTo.GlobalID.ID, "MusterGLN");
            Assert.AreEqual(loadedInvoice.ShipTo.GlobalID.SchemeID, GlobalIDSchemeIdentifiers.GLN);
            Assert.AreEqual(loadedInvoice.ShipTo.Name, "AbKunden AG Mitte");
            Assert.AreEqual(loadedInvoice.ShipTo.Postcode, "12345");
            Assert.AreEqual(loadedInvoice.ShipTo.ContactName, "Einheit: 5.OG rechts");
            Assert.AreEqual(loadedInvoice.ShipTo.Street, "Verwaltung Straße 40");
            Assert.AreEqual(loadedInvoice.ShipTo.City, "Musterstadt");
            Assert.AreEqual(loadedInvoice.ShipTo.Country, CountryCodes.DE);
            Assert.AreEqual(loadedInvoice.ShipTo.CountrySubdivisionName, "Hessen");

            // test comfort
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Comfort);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.ShipTo);
            Assert.AreEqual(loadedInvoice.ShipTo.ID.ID, "SL1001");
            Assert.AreEqual(loadedInvoice.ShipTo.GlobalID.ID, "MusterGLN");
            Assert.AreEqual(loadedInvoice.ShipTo.GlobalID.SchemeID, GlobalIDSchemeIdentifiers.GLN);
            Assert.AreEqual(loadedInvoice.ShipTo.Name, "AbKunden AG Mitte");
            Assert.AreEqual(loadedInvoice.ShipTo.Postcode, "12345");
            Assert.AreEqual(loadedInvoice.ShipTo.ContactName, "Einheit: 5.OG rechts");
            Assert.AreEqual(loadedInvoice.ShipTo.Street, "Verwaltung Straße 40");
            Assert.AreEqual(loadedInvoice.ShipTo.City, "Musterstadt");
            Assert.AreEqual(loadedInvoice.ShipTo.Country, CountryCodes.DE);
            Assert.AreEqual(loadedInvoice.ShipTo.CountrySubdivisionName, "Hessen");

            // test extended
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.ShipTo);
            Assert.AreEqual(loadedInvoice.ShipTo.ID.ID, "SL1001");
            Assert.AreEqual(loadedInvoice.ShipTo.GlobalID.ID, "MusterGLN");
            Assert.AreEqual(loadedInvoice.ShipTo.GlobalID.SchemeID, GlobalIDSchemeIdentifiers.GLN);
            Assert.AreEqual(loadedInvoice.ShipTo.Name, "AbKunden AG Mitte");
            Assert.AreEqual(loadedInvoice.ShipTo.Postcode, "12345");
            Assert.AreEqual(loadedInvoice.ShipTo.ContactName, "Einheit: 5.OG rechts");
            Assert.AreEqual(loadedInvoice.ShipTo.Street, "Verwaltung Straße 40");
            Assert.AreEqual(loadedInvoice.ShipTo.City, "Musterstadt");
            Assert.AreEqual(loadedInvoice.ShipTo.Country, CountryCodes.DE);
            Assert.AreEqual(loadedInvoice.ShipTo.CountrySubdivisionName, "Hessen");
        }


        [TestMethod]
        public void TestShipToTradePartyOnItemLevel()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.First().ShipTo = new Party()
            {
                Name = "ShipTo",
                City = "ShipToCity"
            };

            // test minimum
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Minimum);
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().ShipTo);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().UltimateShipTo);

            // test basic
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Basic);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().ShipTo);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().UltimateShipTo);

            // test comfort
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Comfort);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().ShipTo);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().UltimateShipTo);

            // test extended
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.IsNotNull(loadedInvoice.TradeLineItems.First().ShipTo);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().UltimateShipTo);

            Assert.AreEqual(loadedInvoice.TradeLineItems.First().ShipTo.Name, "ShipTo");
            Assert.AreEqual(loadedInvoice.TradeLineItems.First().ShipTo.City, "ShipToCity");
        } // !TestShipToTradePartyOnItemLevel()


        [TestMethod]
        public void TestUltimateShipToTradePartyOnItemLevel()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.First().UltimateShipTo = new Party()
            {
                Name = "ShipTo",
                City = "ShipToCity"
            };

            // test minimum
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Minimum);
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().ShipTo);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().UltimateShipTo);

            // test basic
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Basic);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().ShipTo);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().UltimateShipTo);

            // test comfort
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Comfort);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().ShipTo);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().UltimateShipTo);

            // test extended
            ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);
            loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.IsNull(loadedInvoice.TradeLineItems.First().ShipTo);
            Assert.IsNotNull(loadedInvoice.TradeLineItems.First().UltimateShipTo);

            Assert.AreEqual(loadedInvoice.TradeLineItems.First().UltimateShipTo.Name, "ShipTo");
            Assert.AreEqual(loadedInvoice.TradeLineItems.First().UltimateShipTo.City, "ShipToCity");
        } // !TestUltimateShipToTradePartyOnItemLevel()


        [TestMethod]
        public void TestMimetypeOfEmbeddedAttachment()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            string filename1 = "myrandomdata.pdf";
            string filename2 = "myrandomdata.bin";
            DateTime timestamp = DateTime.Now.Date;
            byte[] data = new byte[32768];
            new Random().NextBytes(data);

            desc.AddAdditionalReferencedDocument(
                id: "My-File-PDF",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                issueDateTime: timestamp,
                name: "EmbeddedPdf",
                attachmentBinaryObject: data,
                filename: filename1);

            desc.AddAdditionalReferencedDocument(
                id: "My-File-BIN",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                issueDateTime: timestamp.AddDays(-2),
                name: "EmbeddedPdf",
                attachmentBinaryObject: data,
                filename: filename2);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.AdditionalReferencedDocuments.Count, 2);

            foreach (AdditionalReferencedDocument document in loadedInvoice.AdditionalReferencedDocuments)
            {
                if (document.ID == "My-File-PDF")
                {
                    Assert.AreEqual(document.Filename, filename1);
                    Assert.AreEqual("application/pdf", document.MimeType);
                    Assert.AreEqual(timestamp, document.IssueDateTime);
                }

                if (document.ID == "My-File-BIN")
                {
                    Assert.AreEqual(document.Filename, filename2);
                    Assert.AreEqual("application/octet-stream", document.MimeType);
                    Assert.AreEqual(timestamp.AddDays(-2), document.IssueDateTime);
                }
            }
        } // !TestMimetypeOfEmbeddedAttachment()

        [TestMethod]
        public void TestOrderInformation()
        {
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_EXTENDED_Warenrechnung-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            DateTime timestamp = DateTime.Now.Date;

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            desc.OrderDate = timestamp;
            desc.OrderNo = "12345";
            s.Close();

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);

            desc.Save("..\\output.xml", ZUGFeRDVersion.Version23, Profile.Extended);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(timestamp, loadedInvoice.OrderDate);
            Assert.AreEqual("12345", loadedInvoice.OrderNo);

        } // !TestOrderInformation()

        [TestMethod]
        public void TestSellerOrderReferencedDocument()
        {
            string uuid = System.Guid.NewGuid().ToString();
            DateTime issueDateTime = DateTime.Today;

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.SellerOrderReferencedDocument = new SellerOrderReferencedDocument()
            {
                ID = uuid,
                IssueDateTime = issueDateTime
            };

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(Profile.Extended, loadedInvoice.Profile);
            Assert.AreEqual(uuid, loadedInvoice.SellerOrderReferencedDocument.ID);
            Assert.AreEqual(issueDateTime, loadedInvoice.SellerOrderReferencedDocument.IssueDateTime);
        } // !TestSellerOrderReferencedDocument()

        [TestMethod]
        public void TestWriteAndReadBusinessProcess()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.BusinessProcess = "A1";

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual("A1", loadedInvoice.BusinessProcess);
        } // !TestWriteAndReadBusinessProcess

        /// <summary>
        /// This test ensure that Writer and Reader uses the same path and namespace for elements
        /// </summary>
        [TestMethod]
        public void TestWriteAndReadExtended()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            string filename2 = "myrandomdata.bin";
            DateTime timestamp = DateTime.Now.Date;
            byte[] data = new byte[32768];
            new Random().NextBytes(data);

            desc.AddAdditionalReferencedDocument(
                id: "My-File-BIN",
                typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
                issueDateTime: timestamp.AddDays(-2),
                name: "EmbeddedPdf",
                attachmentBinaryObject: data,
                filename: filename2);

            desc.OrderNo = "12345";
            desc.OrderDate = timestamp;

            desc.SetContractReferencedDocument("12345", timestamp);

            desc.SpecifiedProcuringProject = new SpecifiedProcuringProject { ID = "123", Name = "Project 123" };

            desc.ShipTo = new Party
            {
                ID = new GlobalID(GlobalIDSchemeIdentifiers.Unknown, "123"),
                GlobalID = new GlobalID(GlobalIDSchemeIdentifiers.DUNS, "789"),
                Name = "Ship To",
                ContactName = "Max Mustermann",
                Street = "Münchnerstr. 55",
                Postcode = "83022",
                City = "Rosenheim",
                Country = CountryCodes.DE
            };

            desc.UltimateShipTo = new Party
            {
                ID = new GlobalID(GlobalIDSchemeIdentifiers.Unknown, "123"),
                GlobalID = new GlobalID(GlobalIDSchemeIdentifiers.DUNS, "789"),
                Name = "Ultimate Ship To",
                ContactName = "Max Mustermann",
                Street = "Münchnerstr. 55",
                Postcode = "83022",
                City = "Rosenheim",
                Country = CountryCodes.DE
            };

            desc.ShipFrom = new Party
            {
                ID = new GlobalID(GlobalIDSchemeIdentifiers.Unknown, "123"),
                GlobalID = new GlobalID(GlobalIDSchemeIdentifiers.DUNS, "789"),
                Name = "Ship From",
                ContactName = "Eva Musterfrau",
                Street = "Alpenweg 5",
                Postcode = "83022",
                City = "Rosenheim",
                Country = CountryCodes.DE
            };

            desc.PaymentMeans.SEPACreditorIdentifier = "SepaID";
            desc.PaymentMeans.SEPAMandateReference = "SepaMandat";
            desc.PaymentMeans.FinancialCard = new FinancialCard { Id = "123", CardholderName = "Mustermann" };

            desc.PaymentReference = "PaymentReference";

            desc.Invoicee = new Party()
            {
                Name = "Test",
                ContactName = "Max Mustermann",
                Postcode = "83022",
                City = "Rosenheim",
                Street = "Münchnerstraße 123",
                AddressLine3 = "EG links",
                CountrySubdivisionName = "Bayern",
                Country = CountryCodes.DE
            };

            desc.Payee = new Party() // this information will not be stored in the output file since it is available in Extended profile only
            {
                Name = "Test",
                ContactName = "Max Mustermann",
                Postcode = "83022",
                City = "Rosenheim",
                Street = "Münchnerstraße 123",
                AddressLine3 = "EG links",
                CountrySubdivisionName = "Bayern",
                Country = CountryCodes.DE
            };

            desc.AddDebitorFinancialAccount(iban: "DE02120300000000202052", bic: "BYLADEM1001", bankName: "Musterbank");
            desc.BillingPeriodStart = timestamp;
            desc.BillingPeriodEnd = timestamp.AddDays(14);

            desc.AddTradeAllowanceCharge(false, 5m, CurrencyCodes.EUR, 15m, "Reason for charge", TaxTypes.AAB, TaxCategoryCodes.AB, 19m, AllowanceReasonCodes.Packaging);
            desc.AddLogisticsServiceCharge(10m, "Logistics service charge", TaxTypes.AAC, TaxCategoryCodes.AC, 7m);

            desc.GetTradePaymentTerms().FirstOrDefault().DueDate = timestamp.AddDays(14);
            desc.AddInvoiceReferencedDocument("RE-12345", timestamp);


            //set additional LineItem data
            var lineItem = desc.TradeLineItems.FirstOrDefault(i => i.SellerAssignedID == "TB100A4");
            Assert.IsNotNull(lineItem);
            lineItem.Description = "This is line item TB100A4";
            lineItem.BuyerAssignedID = "0815";
            lineItem.SetOrderReferencedDocument("12345", timestamp, "1");
            lineItem.SetDeliveryNoteReferencedDocument("12345", timestamp, "1");
            lineItem.SetContractReferencedDocument("12345", timestamp, "1");

            lineItem.AddAdditionalReferencedDocument("xyz", AdditionalReferencedDocumentTypeCode.ReferenceDocument, ReferenceTypeCodes.AAB, timestamp);
            lineItem.AddAdditionalReferencedDocument("abc", AdditionalReferencedDocumentTypeCode.InvoiceDataSheet, ReferenceTypeCodes.PP, timestamp);

            lineItem.UnitQuantity = 3m;
            lineItem.ActualDeliveryDate = timestamp;

            lineItem.ApplicableProductCharacteristics.Add(new ApplicableProductCharacteristic
            {
                Description = "Product characteristics",
                Value = "Product value"
            });

            lineItem.BillingPeriodStart = timestamp;
            lineItem.BillingPeriodEnd = timestamp.AddDays(10);

            lineItem.AddReceivableSpecifiedTradeAccountingAccount("987654");
            lineItem.AddTradeAllowanceCharge(false, CurrencyCodes.EUR, 10m, 50m, "Reason: UnitTest", AllowanceReasonCodes.Packaging);


            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);

            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(ms), ZUGFeRDVersion.Version23);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual("471102", loadedInvoice.InvoiceNo);
            Assert.AreEqual(new DateTime(2018, 03, 05), loadedInvoice.InvoiceDate);
            Assert.AreEqual(CurrencyCodes.EUR, loadedInvoice.Currency);
            Assert.IsTrue(loadedInvoice.Notes.Any(n => n.Content == "Rechnung gemäß Bestellung vom 01.03.2018."));
            Assert.AreEqual("04011000-12345-34", loadedInvoice.ReferenceOrderNo);
            Assert.AreEqual("Lieferant GmbH", loadedInvoice.Seller.Name);
            Assert.AreEqual("80333", loadedInvoice.Seller.Postcode);
            Assert.AreEqual("München", loadedInvoice.Seller.City);

            Assert.AreEqual("Lieferantenstraße 20", loadedInvoice.Seller.Street);
            Assert.AreEqual(CountryCodes.DE, loadedInvoice.Seller.Country);
            Assert.AreEqual(GlobalIDSchemeIdentifiers.GLN, loadedInvoice.Seller.GlobalID.SchemeID);
            Assert.AreEqual("4000001123452", loadedInvoice.Seller.GlobalID.ID);
            Assert.AreEqual("Max Mustermann", loadedInvoice.SellerContact.Name);
            Assert.AreEqual("Muster-Einkauf", loadedInvoice.SellerContact.OrgUnit);
            Assert.AreEqual("Max@Mustermann.de", loadedInvoice.SellerContact.EmailAddress);
            Assert.AreEqual("+49891234567", loadedInvoice.SellerContact.PhoneNo);

            Assert.AreEqual("Kunden AG Mitte", loadedInvoice.Buyer.Name);
            Assert.AreEqual("69876", loadedInvoice.Buyer.Postcode);
            Assert.AreEqual("Frankfurt", loadedInvoice.Buyer.City);
            Assert.AreEqual("Kundenstraße 15", loadedInvoice.Buyer.Street);
            Assert.AreEqual(CountryCodes.DE, loadedInvoice.Buyer.Country);
            Assert.AreEqual<string>("GE2020211", loadedInvoice.Buyer.ID.ID);

            Assert.AreEqual("12345", loadedInvoice.OrderNo);
            Assert.AreEqual(timestamp, loadedInvoice.OrderDate);

            Assert.AreEqual("12345", loadedInvoice.ContractReferencedDocument.ID);
            Assert.AreEqual(timestamp, loadedInvoice.ContractReferencedDocument.IssueDateTime);

            Assert.AreEqual("123", loadedInvoice.SpecifiedProcuringProject.ID);
            Assert.AreEqual("Project 123", loadedInvoice.SpecifiedProcuringProject.Name);

            Assert.AreEqual("Ultimate Ship To", loadedInvoice.UltimateShipTo.Name);
            /**
             * @todo we can add further asserts for the remainder of properties
             */

            Assert.AreEqual<string>("123", loadedInvoice.ShipTo.ID.ID);
            Assert.AreEqual(GlobalIDSchemeIdentifiers.DUNS, loadedInvoice.ShipTo.GlobalID.SchemeID);
            Assert.AreEqual("789", loadedInvoice.ShipTo.GlobalID.ID);
            Assert.AreEqual("Ship To", loadedInvoice.ShipTo.Name);
            Assert.AreEqual("Max Mustermann", loadedInvoice.ShipTo.ContactName);
            Assert.AreEqual("Münchnerstr. 55", loadedInvoice.ShipTo.Street);
            Assert.AreEqual("83022", loadedInvoice.ShipTo.Postcode);
            Assert.AreEqual("Rosenheim", loadedInvoice.ShipTo.City);
            Assert.AreEqual(CountryCodes.DE, loadedInvoice.ShipTo.Country);

            Assert.AreEqual<string>("123", loadedInvoice.ShipFrom.ID.ID);
            Assert.AreEqual(GlobalIDSchemeIdentifiers.DUNS, loadedInvoice.ShipFrom.GlobalID.SchemeID);
            Assert.AreEqual("789", loadedInvoice.ShipFrom.GlobalID.ID);
            Assert.AreEqual("Ship From", loadedInvoice.ShipFrom.Name);
            Assert.AreEqual("Eva Musterfrau", loadedInvoice.ShipFrom.ContactName);
            Assert.AreEqual("Alpenweg 5", loadedInvoice.ShipFrom.Street);
            Assert.AreEqual("83022", loadedInvoice.ShipFrom.Postcode);
            Assert.AreEqual("Rosenheim", loadedInvoice.ShipFrom.City);
            Assert.AreEqual(CountryCodes.DE, loadedInvoice.ShipFrom.Country);


            Assert.AreEqual(new DateTime(2018, 03, 05), loadedInvoice.ActualDeliveryDate);
            Assert.AreEqual(PaymentMeansTypeCodes.SEPACreditTransfer, loadedInvoice.PaymentMeans.TypeCode);
            Assert.AreEqual("Zahlung per SEPA Überweisung.", loadedInvoice.PaymentMeans.Information);

            Assert.AreEqual("PaymentReference", loadedInvoice.PaymentReference);

            Assert.AreEqual("", loadedInvoice.PaymentMeans.SEPACreditorIdentifier);
            Assert.AreEqual("SepaMandat", loadedInvoice.PaymentMeans.SEPAMandateReference);
            Assert.AreEqual("123", loadedInvoice.PaymentMeans.FinancialCard.Id);
            Assert.AreEqual("Mustermann", loadedInvoice.PaymentMeans.FinancialCard.CardholderName);

            var bankAccount = loadedInvoice.CreditorBankAccounts.FirstOrDefault(a => a.IBAN == "DE02120300000000202051");
            Assert.IsNotNull(bankAccount);
            Assert.AreEqual("Kunden AG", bankAccount.Name);
            Assert.AreEqual("DE02120300000000202051", bankAccount.IBAN);
            Assert.AreEqual("BYLADEM1001", bankAccount.BIC);

            var debitorBankAccount = loadedInvoice.DebitorBankAccounts.FirstOrDefault(a => a.IBAN == "DE02120300000000202052");
            Assert.IsNotNull(debitorBankAccount);
            Assert.AreEqual("DE02120300000000202052", debitorBankAccount.IBAN);


            Assert.AreEqual("Test", loadedInvoice.Invoicee.Name);
            Assert.AreEqual("Max Mustermann", loadedInvoice.Invoicee.ContactName);
            Assert.AreEqual("83022", loadedInvoice.Invoicee.Postcode);
            Assert.AreEqual("Rosenheim", loadedInvoice.Invoicee.City);
            Assert.AreEqual("Münchnerstraße 123", loadedInvoice.Invoicee.Street);
            Assert.AreEqual("EG links", loadedInvoice.Invoicee.AddressLine3);
            Assert.AreEqual("Bayern", loadedInvoice.Invoicee.CountrySubdivisionName);
            Assert.AreEqual(CountryCodes.DE, loadedInvoice.Invoicee.Country);

            Assert.AreEqual("Test", loadedInvoice.Payee.Name);
            Assert.AreEqual("Max Mustermann", loadedInvoice.Payee.ContactName);
            Assert.AreEqual("83022", loadedInvoice.Payee.Postcode);
            Assert.AreEqual("Rosenheim", loadedInvoice.Payee.City);
            Assert.AreEqual("Münchnerstraße 123", loadedInvoice.Payee.Street);
            Assert.AreEqual("EG links", loadedInvoice.Payee.AddressLine3);
            Assert.AreEqual("Bayern", loadedInvoice.Payee.CountrySubdivisionName);
            Assert.AreEqual(CountryCodes.DE, loadedInvoice.Payee.Country);


            var tax = loadedInvoice.Taxes.FirstOrDefault(t => t.BasisAmount == 275m);
            Assert.IsNotNull(tax);
            Assert.AreEqual(275m, tax.BasisAmount);
            Assert.AreEqual(7m, tax.Percent);
            Assert.AreEqual(TaxTypes.VAT, tax.TypeCode);
            Assert.AreEqual(TaxCategoryCodes.S, tax.CategoryCode);

            Assert.AreEqual(timestamp, loadedInvoice.BillingPeriodStart);
            Assert.AreEqual(timestamp.AddDays(14), loadedInvoice.BillingPeriodEnd);

            //TradeAllowanceCharges
            var tradeAllowanceCharge = loadedInvoice.GetTradeAllowanceCharges().FirstOrDefault(i => i.Reason == "Reason for charge");
            Assert.IsNotNull(tradeAllowanceCharge);
            Assert.IsTrue(tradeAllowanceCharge.ChargeIndicator);
            Assert.AreEqual("Reason for charge", tradeAllowanceCharge.Reason);
            Assert.AreEqual(5m, tradeAllowanceCharge.BasisAmount);
            Assert.AreEqual(15m, tradeAllowanceCharge.ActualAmount);
            Assert.AreEqual(CurrencyCodes.EUR, tradeAllowanceCharge.Currency);
            Assert.AreEqual(19m, tradeAllowanceCharge.Tax.Percent);
            Assert.AreEqual(TaxTypes.AAB, tradeAllowanceCharge.Tax.TypeCode);
            Assert.AreEqual(TaxCategoryCodes.AB, tradeAllowanceCharge.Tax.CategoryCode);

            //ServiceCharges
            var serviceCharge = desc.ServiceCharges.FirstOrDefault(i => i.Description == "Logistics service charge");
            Assert.IsNotNull(serviceCharge);
            Assert.AreEqual("Logistics service charge", serviceCharge.Description);
            Assert.AreEqual(10m, serviceCharge.Amount);
            Assert.AreEqual(7m, serviceCharge.Tax.Percent);
            Assert.AreEqual(TaxTypes.AAC, serviceCharge.Tax.TypeCode);
            Assert.AreEqual(TaxCategoryCodes.AC, serviceCharge.Tax.CategoryCode);

            //PaymentTerms
            var paymentTerms = loadedInvoice.GetTradePaymentTerms().FirstOrDefault();
            Assert.IsNotNull(paymentTerms);
            Assert.AreEqual("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018, 3% Skonto innerhalb 10 Tagen bis 15.03.2018", paymentTerms.Description);
            Assert.AreEqual(timestamp.AddDays(14), paymentTerms.DueDate);

            Assert.AreEqual(473.0m, loadedInvoice.LineTotalAmount);
            Assert.AreEqual(0m, loadedInvoice.ChargeTotalAmount); // mandatory, even if 0!
            Assert.AreEqual(0m, loadedInvoice.AllowanceTotalAmount); // mandatory, even if 0!
            Assert.AreEqual(473.0m, loadedInvoice.TaxBasisAmount);
            Assert.AreEqual(56.87m, loadedInvoice.TaxTotalAmount);
            Assert.AreEqual(529.87m, loadedInvoice.GrandTotalAmount);
            Assert.AreEqual(null, loadedInvoice.TotalPrepaidAmount); // optional
            Assert.AreEqual(529.87m, loadedInvoice.DuePayableAmount);

            //InvoiceReferencedDocument
            Assert.AreEqual("RE-12345", loadedInvoice.GetInvoiceReferencedDocuments().First().ID);
            Assert.AreEqual(timestamp, loadedInvoice.GetInvoiceReferencedDocuments().First().IssueDateTime);


            //Line items
            var loadedLineItem = loadedInvoice.TradeLineItems.FirstOrDefault(i => i.SellerAssignedID == "TB100A4");
            Assert.IsNotNull(loadedLineItem);
            Assert.IsTrue(!string.IsNullOrEmpty(loadedLineItem.AssociatedDocument.LineID));
            Assert.AreEqual("This is line item TB100A4", loadedLineItem.Description);

            Assert.AreEqual("Trennblätter A4", loadedLineItem.Name);

            Assert.AreEqual("TB100A4", loadedLineItem.SellerAssignedID);
            Assert.AreEqual("0815", loadedLineItem.BuyerAssignedID);
            Assert.AreEqual(GlobalIDSchemeIdentifiers.EAN, loadedLineItem.GlobalID.SchemeID);
            Assert.AreEqual("4012345001235", loadedLineItem.GlobalID.ID);

            //GrossPriceProductTradePrice
            Assert.AreEqual(9.9m, loadedLineItem.GrossUnitPrice);
            Assert.AreEqual(QuantityCodes.H87, loadedLineItem.UnitCode);
            Assert.AreEqual(3m, loadedLineItem.UnitQuantity);

            //NetPriceProductTradePrice
            Assert.AreEqual(9.9m, loadedLineItem.NetUnitPrice);
            Assert.AreEqual(20m, loadedLineItem.BilledQuantity);

            Assert.AreEqual(TaxTypes.VAT, loadedLineItem.TaxType);
            Assert.AreEqual(TaxCategoryCodes.S, loadedLineItem.TaxCategoryCode);
            Assert.AreEqual(19m, loadedLineItem.TaxPercent);

            Assert.AreEqual("1", loadedLineItem.BuyerOrderReferencedDocument.LineID);
            Assert.AreEqual("12345", loadedLineItem.BuyerOrderReferencedDocument.ID);
            Assert.AreEqual(timestamp, loadedLineItem.BuyerOrderReferencedDocument.IssueDateTime);
            Assert.AreEqual("1", loadedLineItem.DeliveryNoteReferencedDocument.LineID);
            Assert.AreEqual("12345", loadedLineItem.DeliveryNoteReferencedDocument.ID);
            Assert.AreEqual(timestamp, loadedLineItem.DeliveryNoteReferencedDocument.IssueDateTime);
            Assert.AreEqual("1", loadedLineItem.ContractReferencedDocument.LineID);
            Assert.AreEqual("12345", loadedLineItem.ContractReferencedDocument.ID);
            Assert.AreEqual(timestamp, loadedLineItem.ContractReferencedDocument.IssueDateTime);

            Assert.IsTrue(loadedLineItem.GetAdditionalReferencedDocuments().Count == 2);
            var lineItemReferencedDoc = loadedLineItem.GetAdditionalReferencedDocuments().FirstOrDefault();
            Assert.IsNotNull(lineItemReferencedDoc);
            Assert.AreEqual("xyz", lineItemReferencedDoc.ID);
            Assert.AreEqual(AdditionalReferencedDocumentTypeCode.ReferenceDocument, lineItemReferencedDoc.TypeCode);
            Assert.AreEqual(timestamp, lineItemReferencedDoc.IssueDateTime);
            Assert.AreEqual(ReferenceTypeCodes.AAB, lineItemReferencedDoc.ReferenceTypeCode);


            var productCharacteristics = loadedLineItem.ApplicableProductCharacteristics.FirstOrDefault();
            Assert.IsNotNull(productCharacteristics);
            Assert.AreEqual("Product characteristics", productCharacteristics.Description);
            Assert.AreEqual("Product value", productCharacteristics.Value);

            Assert.AreEqual(timestamp, loadedLineItem.ActualDeliveryDate);
            Assert.AreEqual(timestamp, loadedLineItem.BillingPeriodStart);
            Assert.AreEqual(timestamp.AddDays(10), loadedLineItem.BillingPeriodEnd);

            var accountingAccount = loadedLineItem.ReceivableSpecifiedTradeAccountingAccounts.FirstOrDefault();
            Assert.IsNotNull(accountingAccount);
            Assert.AreEqual("987654", accountingAccount.TradeAccountID);


            var lineItemTradeAllowanceCharge = loadedLineItem.GetTradeAllowanceCharges().FirstOrDefault(i => i.Reason == "Reason: UnitTest");
            Assert.IsNotNull(lineItemTradeAllowanceCharge);
            Assert.IsTrue(lineItemTradeAllowanceCharge.ChargeIndicator);
            Assert.AreEqual(10m, lineItemTradeAllowanceCharge.BasisAmount);
            Assert.AreEqual(50m, lineItemTradeAllowanceCharge.ActualAmount);
            Assert.AreEqual("Reason: UnitTest", lineItemTradeAllowanceCharge.Reason);
        }

        /// <summary>
        /// This test ensure that BIC won't be written if empty
        /// </summary>
        [TestMethod]
        public void TestFinancialInstitutionBICEmpty()
        {
            DateTime issueDateTime = DateTime.Today;

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            //PayeeSpecifiedCreditorFinancialInstitution
            desc.CreditorBankAccounts[0].BIC = String.Empty;
            //PayerSpecifiedDebtorFinancialInstitution
            desc.AddDebitorFinancialAccount("DE02120300000000202051", String.Empty);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Comfort);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            XmlDocument doc = new XmlDocument();
            doc.Load(ms);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.DocumentElement.OwnerDocument.NameTable);
            nsmgr.AddNamespace("qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            nsmgr.AddNamespace("a", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            nsmgr.AddNamespace("rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            nsmgr.AddNamespace("ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            nsmgr.AddNamespace("udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");

            XmlNodeList creditorFinancialInstitutions = doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeeSpecifiedCreditorFinancialInstitution", nsmgr);
            XmlNodeList debitorFinancialInstitutions = doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayerSpecifiedDebtorFinancialInstitution", nsmgr);

            Assert.AreEqual(creditorFinancialInstitutions.Count, 0);
            Assert.AreEqual(debitorFinancialInstitutions.Count, 0);
        } // !TestFinancialInstitutionBICEmpty()


        /// <summary>
        /// This test ensure that BIC won't be written if empty
        /// </summary>
        [TestMethod]
        public void TestAltteilSteuer()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("112233", new DateTime(2021, 04, 23), CurrencyCodes.EUR);
            desc.Notes.Clear();
            desc.Notes.Add(new Note("Rechnung enthält 100 EUR (Umsatz)Steuer auf Altteile gem. Abschn. 10.5 Abs. 3 UStAE", SubjectCodes.ADU, ContentCodes.Unknown));

            desc.TradeLineItems.Clear();
            desc.AddTradeLineItem(name: "Neumotor",
                                  unitCode: QuantityCodes.C62,
                                  unitQuantity: 1,
                                  billedQuantity: 1,
                                  netUnitPrice: 1000,
                                  taxType: TaxTypes.VAT,
                                  categoryCode: TaxCategoryCodes.S,
                                  taxPercent: 19);

            desc.AddTradeLineItem(name: "Bemessungsgrundlage und Umsatzsteuer auf Altteil",
                                  unitCode: QuantityCodes.C62,
                                  unitQuantity: 1,
                                  billedQuantity: 1,
                                  netUnitPrice: 100,
                                  taxType: TaxTypes.VAT,
                                  categoryCode: TaxCategoryCodes.S,
                                  taxPercent: 19);

            desc.AddTradeLineItem(name: "Korrektur/Stornierung Bemessungsgrundlage der Umsatzsteuer auf Altteil",
                                  unitCode: QuantityCodes.C62,
                                  unitQuantity: 1,
                                  billedQuantity: -1,
                                  netUnitPrice: 100,
                                  taxType: TaxTypes.VAT,
                                  categoryCode: TaxCategoryCodes.Z,
                                  taxPercent: 0);

            desc.AddApplicableTradeTax(basisAmount: 1000m,
                                       percent: 19m,
                                       1000m / 100m * 19m,
                                       TaxTypes.VAT,
                                       TaxCategoryCodes.S);

            desc.SetTotals(lineTotalAmount: 1500m,
                     taxBasisAmount: 1500m,
                     taxTotalAmount: 304m,
                     grandTotalAmount: 1804m,
                     duePayableAmount: 1804m
                    );

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.Invoicee, null);
        } // !TestAltteilSteuer()

        [TestMethod]
        public void TestBasisQuantityStandard()
        {
            InvoiceDescriptor desc = _InvoiceProvider.CreateInvoice();

            desc.TradeLineItems.Clear();
            desc.AddTradeLineItem(name: "Joghurt Banane",
                                  unitCode: QuantityCodes.H87,
                                  sellerAssignedID: "ARNR2",
                                  id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4000050986428"),
                                  grossUnitPrice: 5.5m,
                                  netUnitPrice: 5.5m,
                                  billedQuantity: 50,
                                  taxType: TaxTypes.VAT,
                                  categoryCode: TaxCategoryCodes.S,
                                  taxPercent: 7
                                  );
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);

            XmlDocument doc = new XmlDocument();
            doc.Load(ms);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.DocumentElement.OwnerDocument.NameTable);
            nsmgr.AddNamespace("qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            nsmgr.AddNamespace("a", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            nsmgr.AddNamespace("rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            nsmgr.AddNamespace("ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            nsmgr.AddNamespace("udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");

            XmlNode node = doc.SelectSingleNode("//ram:SpecifiedTradeSettlementLineMonetarySummation//ram:LineTotalAmount", nsmgr);
            Assert.AreEqual("275.00", node.InnerText);
        } // !TestBasisQuantityStandard()


        [TestMethod]
        public void TestBasisQuantityMultiple()
        {
            InvoiceDescriptor desc = _InvoiceProvider.CreateInvoice();

            desc.TradeLineItems.Clear();
            TradeLineItem tli = desc.AddTradeLineItem(name: "Joghurt Banane",
                                                      unitCode: QuantityCodes.H87,
                                                      sellerAssignedID: "ARNR2",
                                                      id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4000050986428"),
                                                      grossUnitPrice: 5.5m,
                                                      netUnitPrice: 5.5m,
                                                      billedQuantity: 50,
                                                      taxType: TaxTypes.VAT,
                                                      categoryCode: TaxCategoryCodes.S,
                                                      taxPercent: 7,
                                                      unitQuantity: 10
                                                      );
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);

            XmlDocument doc = new XmlDocument();
            doc.Load(ms);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.DocumentElement.OwnerDocument.NameTable);
            nsmgr.AddNamespace("qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            nsmgr.AddNamespace("a", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            nsmgr.AddNamespace("rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            nsmgr.AddNamespace("ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            nsmgr.AddNamespace("udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");

            XmlNode node = doc.SelectSingleNode("//ram:SpecifiedTradeSettlementLineMonetarySummation//ram:LineTotalAmount", nsmgr);
            Assert.AreEqual("27.50", node.InnerText);
        } // !TestBasisQuantityMultiple()


        [TestMethod]
        public void TestTradeAllowanceChargeWithoutExplicitPercentage()
        {
            InvoiceDescriptor invoice = _InvoiceProvider.CreateInvoice();

            // fake values, does not matter for our test case
            invoice.AddTradeAllowanceCharge(true, 100, CurrencyCodes.EUR, 10, String.Empty, TaxTypes.VAT, TaxCategoryCodes.S, 19, AllowanceReasonCodes.Packaging);

            MemoryStream ms = new MemoryStream();
            invoice.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Position = 0;

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            IList<TradeAllowanceCharge> allowanceCharges = loadedInvoice.GetTradeAllowanceCharges();

            Assert.IsTrue(allowanceCharges.Count == 1);
            Assert.AreEqual(allowanceCharges[0].BasisAmount, 100m);
            Assert.AreEqual(allowanceCharges[0].Amount, 10m);
            Assert.AreEqual(allowanceCharges[0].ChargePercentage, null);
        } // !TestTradeAllowanceChargeWithoutExplicitPercentage()


        [TestMethod]
        public void TestTradeAllowanceChargeWithExplicitPercentage()
        {
            InvoiceDescriptor invoice = _InvoiceProvider.CreateInvoice();

            // fake values, does not matter for our test case
            invoice.AddTradeAllowanceCharge(true, 100, CurrencyCodes.EUR, 10, 12, String.Empty, TaxTypes.VAT, TaxCategoryCodes.S, 19, AllowanceReasonCodes.Packaging);

            MemoryStream ms = new MemoryStream();
            invoice.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Position = 0;
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            IList<TradeAllowanceCharge> allowanceCharges = loadedInvoice.GetTradeAllowanceCharges();

            Assert.IsTrue(allowanceCharges.Count == 1);
            Assert.AreEqual(allowanceCharges[0].BasisAmount, 100m);
            Assert.AreEqual(allowanceCharges[0].Amount, 10m);
            Assert.AreEqual(allowanceCharges[0].ChargePercentage, 12);
        } // !TestTradeAllowanceChargeWithExplicitPercentage()


        [TestMethod]
        public void TestWriteAndReadDespatchAdviceDocumentReferenceXRechnung()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            string despatchAdviceNo = "421567982";
            DateTime despatchAdviceDate = new DateTime(2024, 5, 14);
            desc.SetDespatchAdviceReferencedDocument(despatchAdviceNo, despatchAdviceDate);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(despatchAdviceNo, loadedInvoice.DespatchAdviceReferencedDocument.ID);
            Assert.AreEqual(despatchAdviceDate, loadedInvoice.DespatchAdviceReferencedDocument.IssueDateTime);
        } //!TestWriteAndReadDespatchAdviceDocumentReference


        [TestMethod]
        [DataRow(Profile.Basic)]
        [DataRow(Profile.Comfort)]
        [DataRow(Profile.Extended)]
        [DataRow(Profile.XRechnung)]
        public void TestSpecifiedTradeAllowanceCharge(Profile profile)
        {
            InvoiceDescriptor invoice = _InvoiceProvider.CreateInvoice();

            invoice.TradeLineItems[0].AddSpecifiedTradeAllowanceCharge(true, CurrencyCodes.EUR, 198m, 19.8m, 10m, "Discount 10%");

            MemoryStream ms = new MemoryStream();
            invoice.Save(ms, ZUGFeRDVersion.Version23, profile);
            ms.Position = 0;

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            TradeAllowanceCharge allowanceCharge = loadedInvoice.TradeLineItems[0].GetSpecifiedTradeAllowanceCharges().First();

            Assert.AreEqual(allowanceCharge.ChargeIndicator, false);//false = discount
            //CurrencyCodes are not written bei InvoiceDescriptor22Writer
            //Assert.AreEqual(allowanceCharge.Currency, CurrencyCodes.EUR);
            if (profile != Profile.Basic)
            {
                Assert.AreEqual(allowanceCharge.BasisAmount, 198m);
                Assert.AreEqual(allowanceCharge.ChargePercentage, 10m);
            }
            Assert.AreEqual(allowanceCharge.ActualAmount, 19.8m);
            Assert.AreEqual(allowanceCharge.Reason, "Discount 10%");
        } // !SpecifiedTradeAllowanceCharge()


        [TestMethod]
        public void TestSpecifiedTradeAllowanceChargeNotWrittenInMinimum()
        {
            InvoiceDescriptor invoice = _InvoiceProvider.CreateInvoice();

            invoice.TradeLineItems[0].AddSpecifiedTradeAllowanceCharge(true, CurrencyCodes.EUR, 198m, 19.8m, 10m, "Discount 10%");

            MemoryStream ms = new MemoryStream();
            invoice.Save(ms, ZUGFeRDVersion.Version23, Profile.Minimum);
            ms.Position = 0;

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(0, loadedInvoice.TradeLineItems[0].GetSpecifiedTradeAllowanceCharges().Count);
        } // !TestSpecifiedTradeAllowanceChargeNotWrittenInMinimum()


        [TestMethod]
        public void TestSellerDescription()
        {
            InvoiceDescriptor invoice = _InvoiceProvider.CreateInvoice();

            string description = "Test description";

            invoice.SetSeller(name: "Lieferant GmbH",
                              postcode: "80333",
                              city: "München",
                              street: "Lieferantenstraße 20",
                              country: CountryCodes.DE,
                              id: String.Empty,
                              globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"),
                              legalOrganization: new LegalOrganization(GlobalIDSchemeIdentifiers.GLN, "4000001123452", "Lieferant GmbH"),
                              description: description
                              );

            MemoryStream ms = new MemoryStream();
            invoice.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Position = 0;

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.Seller.Description, description);
        } // !TestSellerDescription()


        [TestMethod]
        public void TestSellerContact()
        {
            InvoiceDescriptor invoice = _InvoiceProvider.CreateInvoice();

            string description = "Test description";

            invoice.SetSeller(name: "Lieferant GmbH",
                              postcode: "80333",
                              city: "München",
                              street: "Lieferantenstraße 20",
                              country: CountryCodes.DE,
                              id: String.Empty,
                              globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"),
                              legalOrganization: new LegalOrganization(GlobalIDSchemeIdentifiers.GLN, "4000001123452", "Lieferant GmbH"),
                              description: description
                              );

            string sellerContact = "1-123";
            string orgUnit = "2-123";
            string emailAddress = "3-123";
            string phoneNo = "4-123";
            string faxNo = "5-123";
            invoice.SetSellerContact(sellerContact, orgUnit, emailAddress, phoneNo, faxNo);

            MemoryStream ms = new MemoryStream();
            invoice.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
            ms.Position = 0;

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(sellerContact, loadedInvoice.SellerContact.Name);
            Assert.AreEqual(orgUnit, loadedInvoice.SellerContact.OrgUnit);
            Assert.AreEqual(emailAddress, loadedInvoice.SellerContact.EmailAddress);
            Assert.AreEqual(phoneNo, loadedInvoice.SellerContact.PhoneNo);
            Assert.AreEqual(faxNo, loadedInvoice.SellerContact.FaxNo);

            Assert.AreEqual(loadedInvoice.Seller.Description, description);
        } // !TestSellerContact()

        [TestMethod]
        public void ShouldLoadCiiWithoutQdtNamespace()
        {
            string path = @"..\..\..\..\demodata\xRechnung\xRechnung CII - without qdt.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            Assert.AreEqual(desc.Profile, Profile.XRechnung);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.InvoiceNo, "123456XX");
            Assert.AreEqual(desc.TradeLineItems.Count, 2);
            Assert.AreEqual(desc.LineTotalAmount, 314.86m);
        } // !ShouldLoadCIIWithoutQdtNamespace()


        [TestMethod]
        public void TestDesignatedProductClassificationWithFullClassification()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.First().AddDesignatedProductClassification(
                DesignatedProductClassificationClassCodes.HS,
                "List Version ID Value",
                "Class Code",
                "Class Name");

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);

            // string comparison
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string content = reader.ReadToEnd();
            Assert.IsTrue(content.Contains("<ram:DesignatedProductClassification>"));
            Assert.IsTrue(content.Contains("<ram:ClassCode listID=\"HS\" listVersionID=\"List Version ID Value\">Class Code</ram:ClassCode>"));
            Assert.IsTrue(content.Contains("<ram:ClassName>Class Name</ram:ClassName>"));

            // structure comparison
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(DesignatedProductClassificationClassCodes.HS, loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ListID);
            Assert.AreEqual("List Version ID Value", loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ListVersionID);
            Assert.AreEqual("Class Code", loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassCode);
            Assert.AreEqual("Class Name", loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassName);
        } // !TestDesignatedProductClassificationWithFullClassification()


        [TestMethod]
        public void TestDesignatedProductClassificationWithEmptyVersionId()
        {
            // test with empty _Version id value
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.First().AddDesignatedProductClassification(
                DesignatedProductClassificationClassCodes.HS,
                null,
                "Class Code",
                "Class Name"
                );

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(DesignatedProductClassificationClassCodes.HS, desc.TradeLineItems.First().GetDesignatedProductClassifications().First().ListID);
            Assert.IsNull(desc.TradeLineItems.First().GetDesignatedProductClassifications().First().ListVersionID);
            Assert.AreEqual("Class Code", desc.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassCode);
            Assert.AreEqual("Class Name", desc.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassName);
        } // !TestDesignatedProductClassificationWithEmptyVersionId()



        [TestMethod]
        public void TestDesignatedProductClassificationWithEmptyListIdAndVersionId()
        {
            // test with empty _Version id value
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.First().AddDesignatedProductClassification(
                DesignatedProductClassificationClassCodes.HS,
                null,
                "Class Code"
                );

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            var x = loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First();


            Assert.AreEqual(DesignatedProductClassificationClassCodes.HS, loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ListID);
            Assert.AreEqual(String.Empty, loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ListVersionID);
            Assert.AreEqual("Class Code", loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassCode);
            Assert.AreEqual(String.Empty, loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassName);
        } // !TestDesignatedProductClassificationWithEmptyListIdAndVersionId()


        [TestMethod]
        public void TestDesignatedProductClassificationWithoutAnyOptionalInformation()
        {
            // test with empty _Version id value
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.First().AddDesignatedProductClassification(DesignatedProductClassificationClassCodes.HS);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(DesignatedProductClassificationClassCodes.HS, desc.TradeLineItems.First().GetDesignatedProductClassifications().First().ListID);
            Assert.IsNull(desc.TradeLineItems.First().GetDesignatedProductClassifications().First().ListVersionID);
            Assert.IsNull(desc.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassCode);
            Assert.IsNull(desc.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassName);
        } // !TestDesignatedProductClassificationWithoutAnyOptionalInformation()


        [TestMethod]
        public void TestPaymentTermsMultiCardinalityWithExtended()
        {
            // Arrange
            DateTime timestamp = DateTime.Now.Date;
            decimal baseAmount = 123m;
            decimal percentage = 3m;
            decimal actualAmount = 123m * 3m / 100m;
            var desc = _InvoiceProvider.CreateInvoice();
            desc.GetTradePaymentTerms().Clear();
            desc.AddTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018", timestamp.AddDays(14));
            desc.AddTradePaymentTerms("3% Skonto innerhalb 10 Tagen bis 15.03.2018", new DateTime(2018, 3, 15), PaymentTermsType.Skonto, 10, percentage: percentage, baseAmount: baseAmount, actualAmount: actualAmount);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(ms), ZUGFeRDVersion.Version23);

            // Act
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // Assert
            // PaymentTerms
            var paymentTerms = loadedInvoice.GetTradePaymentTerms();
            Assert.IsNotNull(paymentTerms);
            Assert.AreEqual(2, paymentTerms.Count);
            var paymentTerm = loadedInvoice.GetTradePaymentTerms().FirstOrDefault(i => i.Description.StartsWith("Zahlbar"));
            Assert.IsNotNull(paymentTerm);
            Assert.AreEqual("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018", paymentTerm.Description);
            Assert.AreEqual(timestamp.AddDays(14), paymentTerm.DueDate);

            paymentTerm = loadedInvoice.GetTradePaymentTerms().FirstOrDefault(i => i.PaymentTermsType == PaymentTermsType.Skonto);
            Assert.IsNotNull(paymentTerm);
            Assert.AreEqual("3% Skonto innerhalb 10 Tagen bis 15.03.2018", paymentTerm.Description);
            // Assert.AreEqual(10, firstPaymentTerm.DueDays);
            Assert.AreEqual(percentage, paymentTerm.Percentage);
            Assert.AreEqual(baseAmount, paymentTerm.BaseAmount);
            Assert.AreEqual(actualAmount, paymentTerm.ActualAmount);
        } // !TestPaymentTermsMultiCardinalityWithExtended()


        [TestMethod]
        public void TestPaymentTermsMultiCardinalityWithBasic()
        {
            // Arrange
            DateTime timestamp = DateTime.Now.Date;
            var desc = _InvoiceProvider.CreateInvoice();
            desc.GetTradePaymentTerms().Clear();
            desc.AddTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018", new DateTime(2018, 4, 4));
            desc.AddTradePaymentTerms("3% Skonto innerhalb 10 Tagen bis 15.03.2018", new DateTime(2018, 3, 15), PaymentTermsType.Skonto, 10, 3m);
            desc.GetTradePaymentTerms().FirstOrDefault().DueDate = timestamp.AddDays(14);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Basic);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(ms), ZUGFeRDVersion.Version23);

            // Act
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // Assert
            // PaymentTerms
            var paymentTerms = loadedInvoice.GetTradePaymentTerms();
            Assert.IsNotNull(paymentTerms);
            Assert.AreEqual(2, paymentTerms.Count);
            var paymentTerm = loadedInvoice.GetTradePaymentTerms().FirstOrDefault(i => i.Description.StartsWith("Zahlbar"));
            Assert.IsNotNull(paymentTerm);
            Assert.IsNull(paymentTerm.PaymentTermsType);
            Assert.AreEqual("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018", paymentTerm.Description);
            Assert.AreEqual(timestamp.AddDays(14), paymentTerm.DueDate);

            paymentTerm = loadedInvoice.GetTradePaymentTerms().LastOrDefault();
            Assert.IsNotNull(paymentTerm);
            Assert.IsNull(paymentTerm.PaymentTermsType);
            Assert.AreEqual("3% Skonto innerhalb 10 Tagen bis 15.03.2018", paymentTerm.Description);
            Assert.IsNull(paymentTerm.Percentage);
        } // !TestPaymentTermsMultiCardinalityWithBasic()


        [TestMethod]
        public void TestPaymentTermsMultiCardinalityWithMinimum()
        {
            // Arrange
            DateTime timestamp = DateTime.Now.Date;
            var desc = _InvoiceProvider.CreateInvoice();
            desc.GetTradePaymentTerms().Clear();
            desc.AddTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018", new DateTime(2018, 4, 4));
            desc.AddTradePaymentTerms("3% Skonto innerhalb 10 Tagen bis 15.03.2018", new DateTime(2018, 3, 15), PaymentTermsType.Skonto, 10, 3m);
            desc.GetTradePaymentTerms().FirstOrDefault().DueDate = timestamp.AddDays(14);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Minimum);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(ms), ZUGFeRDVersion.Version23);

            // Act
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // Assert
            // PaymentTerms
            var paymentTerms = loadedInvoice.GetTradePaymentTerms();
            Assert.IsNotNull(paymentTerms);
            Assert.AreEqual(0, paymentTerms.Count);
        } // !TestPaymentTermsMultiCardinalityWithMinimum()


        [TestMethod]
        public void TestPaymentTermsSingleCardinality()
        {
            // Arrange
            DateTime timestamp = DateTime.Now.Date;
            var desc = _InvoiceProvider.CreateInvoice();
            desc.GetTradePaymentTerms().Clear();
            desc.AddTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018", new DateTime(2018, 4, 4));
            desc.GetTradePaymentTerms().First().DueDate = timestamp.AddDays(14);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Comfort);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(ms), ZUGFeRDVersion.Version23);

            // Act
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // Assert
            // PaymentTerms
            var paymentTerms = loadedInvoice.GetTradePaymentTerms();
            Assert.IsNotNull(paymentTerms);
            Assert.AreEqual(1, paymentTerms.Count);
            var paymentTerm = loadedInvoice.GetTradePaymentTerms().FirstOrDefault();
            Assert.IsNotNull(paymentTerm);
            Assert.AreEqual($"Zahlbar innerhalb 30 Tagen netto bis 04.04.2018", paymentTerm.Description);
            Assert.AreEqual(timestamp.AddDays(14), paymentTerm.DueDate);
        } // !TestPaymentTermsSingleCardinality()


        [TestMethod]
        public void TestPaymentTermsMultiCardinalityXRechnungStructured()
        {
            DateTime timestamp = DateTime.Now.Date;
            var desc = _InvoiceProvider.CreateInvoice();
            desc.GetTradePaymentTerms().Clear();
            desc.AddTradePaymentTerms(String.Empty, null, PaymentTermsType.Skonto, 14, 2.25m);
            desc.GetTradePaymentTerms().First().DueDate = timestamp.AddDays(14);
            desc.AddTradePaymentTerms("Description2", null, PaymentTermsType.Skonto, 28, 1m);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung);

            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string text = reader.ReadToEnd();

            ms.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(InvoiceDescriptor.GetVersion(ms), ZUGFeRDVersion.Version23);

            // Act
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // Assert
            // PaymentTerms
            var paymentTerms = loadedInvoice.GetTradePaymentTerms();
            Assert.IsNotNull(paymentTerms);
            Assert.AreEqual(2, paymentTerms.Count);
            var firstPaymentTerm = loadedInvoice.GetTradePaymentTerms().FirstOrDefault();
            Assert.IsNotNull(firstPaymentTerm);
            Assert.AreEqual($"#SKONTO#TAGE=14#PROZENT=2.25#", firstPaymentTerm.Description);
            Assert.AreEqual(timestamp.AddDays(14), firstPaymentTerm.DueDate);


            var secondPaymentTerm = loadedInvoice.GetTradePaymentTerms().LastOrDefault();
            Assert.IsNotNull(secondPaymentTerm);
            Assert.AreEqual($"Description2{XmlConstants.XmlNewLine}#SKONTO#TAGE=28#PROZENT=1.00#", secondPaymentTerm.Description);

            //Assert.AreEqual(PaymentTermsType.Skonto, firstPaymentTerm.PaymentTermsType);
            //Assert.AreEqual(10, firstPaymentTerm.DueDays);
            //Assert.AreEqual(3m, firstPaymentTerm.Percentage);
        } // !TestPaymentTermsSingleCardinalityStructured()

        [TestMethod]
        public void TestBuyerOrderReferenceLineId()
        {
            string path = @"..\..\..\..\demodata\zugferd22\zugferd_2p2_EXTENDED_Fremdwaehrung-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.GetTradeLineItems().First().BuyerOrderReferencedDocument.LineID, "1");
            Assert.AreEqual(desc.GetTradeLineItems().First().BuyerOrderReferencedDocument.ID, "ORDER84359");
        }

        [TestMethod]
        public void TestRequiredDirectDebitFieldsShouldExist()
        {
            var d = new InvoiceDescriptor();
            d.Type = InvoiceType.Invoice;
            d.InvoiceNo = "471102";
            d.Currency = CurrencyCodes.EUR;
            d.InvoiceDate = new DateTime(2018, 3, 5);
            d.AddTradeLineItem(
                lineID: "1",
                id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4012345001235"),
                sellerAssignedID: "TB100A4",
                name: "Trennblätter A4",
                billedQuantity: 20m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 9.9m,
                grossUnitPrice: 11.781m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);
            d.SetSeller(
                id: null,
                globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"),
                name: "Lieferant GmbH",
                postcode: "80333",
                city: "München",
                street: "Lieferantenstraße 20",
                country: CountryCodes.DE,
                legalOrganization: new LegalOrganization(GlobalIDSchemeIdentifiers.GLN, "4000001123452", "Lieferant GmbH"));
            d.SetBuyer(
                id: "GE2020211",
                globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001987658"),
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
            d.AddTradePaymentTerms(
                "Der Betrag in Höhe von EUR 235,62 wird am 20.03.2018 von Ihrem Konto per SEPA-Lastschrift eingezogen.");
            d.SetTotals(
                198.00m,
                0.00m,
                0.00m,
                198.00m,
                37.62m,
                235.62m,
                0.00m,
                235.62m);
            d.SellerTaxRegistration.Add(
                new TaxRegistration
                {
                    SchemeID = TaxRegistrationSchemeID.FC,
                    No = "201/113/40209"
                });
            d.SellerTaxRegistration.Add(
                new TaxRegistration
                {
                    SchemeID = TaxRegistrationSchemeID.VA,
                    No = "DE123456789"
                });
            d.AddApplicableTradeTax(
                198.00m,
                19.00m,
                198.00m / 100m * 19m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);

            using (var stream = new MemoryStream())
            {
                d.Save(stream, ZUGFeRDVersion.Version23, Profile.XRechnung);
                stream.Seek(0, SeekOrigin.Begin);

                // test the raw xml file
                string content = Encoding.UTF8.GetString(stream.ToArray());

                Assert.IsTrue(content.Contains($"<ram:CreditorReferenceID>DE98ZZZ09999999999</ram:CreditorReferenceID>"));
                Assert.IsTrue(content.Contains($"<ram:DirectDebitMandateID>REF A-123</ram:DirectDebitMandateID>"));
            }
        } // !TestRequiredDirectDebitFieldsShouldExist()

        [TestMethod]
        public void TestInNonDebitInvoiceTheDirectDebitFieldsShouldNotExist()
        {
            var d = new InvoiceDescriptor();
            d.Type = InvoiceType.Invoice;
            d.InvoiceNo = "471102";
            d.Currency = CurrencyCodes.EUR;
            d.InvoiceDate = new DateTime(2018, 3, 5);
            d.AddTradeLineItem(
                lineID: "1",
                id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4012345001235"),
                sellerAssignedID: "TB100A4",
                name: "Trennblätter A4",
                billedQuantity: 20m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 9.9m,
                grossUnitPrice: 11.781m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);
            d.SetSeller(
                id: null,
                globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"),
                name: "Lieferant GmbH",
                postcode: "80333",
                city: "München",
                street: "Lieferantenstraße 20",
                country: CountryCodes.DE,
                legalOrganization: new LegalOrganization(GlobalIDSchemeIdentifiers.GLN, "4000001123452", "Lieferant GmbH"));
            d.SetBuyer(
                id: "GE2020211",
                globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001987658"),
                name: "Kunden AG Mitte",
                postcode: "69876",
                city: "Frankfurt",
                street: "Kundenstraße 15",
                country: CountryCodes.DE);
            d.SetPaymentMeans(PaymentMeansTypeCodes.SEPACreditTransfer,
                "Information of Payment Means",
                "DE98ZZZ09999999999",
                "REF A-123");
            d.AddDebitorFinancialAccount(
                "DE21860000000086001055",
                null);
            d.AddTradePaymentTerms(
                "Der Betrag in Höhe von EUR 235,62 wird am 20.03.2018 von Ihrem Konto per SEPA-Lastschrift eingezogen.");
            d.SetTotals(
                198.00m,
                0.00m,
                0.00m,
                198.00m,
                37.62m,
                235.62m,
                0.00m,
                235.62m);
            d.SellerTaxRegistration.Add(
                new TaxRegistration
                {
                    SchemeID = TaxRegistrationSchemeID.FC,
                    No = "201/113/40209"
                });
            d.SellerTaxRegistration.Add(
                new TaxRegistration
                {
                    SchemeID = TaxRegistrationSchemeID.VA,
                    No = "DE123456789"
                });
            d.AddApplicableTradeTax(
                198.00m,
                19.00m,
                198.00m / 100m * 19.00m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);

            using (var stream = new MemoryStream())
            {
                d.Save(stream, ZUGFeRDVersion.Version23, Profile.XRechnung);
                stream.Seek(0, SeekOrigin.Begin);

                // test the raw xml file
                string content = Encoding.UTF8.GetString(stream.ToArray());

                Assert.IsFalse(content.Contains($"<ram:CreditorReferenceID>DE98ZZZ09999999999</ram:CreditorReferenceID>"));
                Assert.IsFalse(content.Contains($"<ram:DirectDebitMandateID>REF A-123</ram:DirectDebitMandateID>"));
            }
        } // !TestInNonDebitInvoiceTheDirectDebitFieldsShouldNotExist()


        [TestMethod]
        public void TestSpecifiedTradePaymentTermsDueDate()
        {
            string path = @"..\..\..\..\documentation\zugferd23en\Examples\2. BASIC\BASIC_Einfach\factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);
            Assert.IsTrue(desc.GetTradePaymentTerms().First().DueDate.HasValue);
            Assert.AreEqual(new DateTime(2024, 12, 15), desc.GetTradePaymentTerms().First().DueDate.Value);
        } // !TestSpecifiedTradePaymentTermsDueDate()


        [TestMethod]
        public void TestSpecifiedTradePaymentTermsDescription()
        {
            string path = @"..\..\..\..\documentation\zugferd23en\Examples\4. EXTENDED\EXTENDED_Warenrechnung\factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);
            Assert.IsNotNull(desc.GetTradePaymentTerms().First().Description);
            Assert.AreEqual("Bei Zahlung innerhalb 14 Tagen gewähren wir 2,0% Skonto.", desc.GetTradePaymentTerms().First().Description);
        } // !TestSpecifiedTradePaymentTermsDescription()


        [TestMethod]
        public void TestSpecifiedTradePaymentTermsCalculationPercent()
        {
            string path = @"..\..\..\..\documentation\zugferd23en\Examples\4. EXTENDED\EXTENDED_Warenrechnung\factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);
            Assert.IsNotNull(desc.GetTradePaymentTerms().First().Percentage);
            Assert.AreEqual(2m, desc.GetTradePaymentTerms().First().Percentage);
        } // !TestSpecifiedTradePaymentTermsCalculationPercent()
    }
}
