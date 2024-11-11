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
using System.IO;
using System.Text;

namespace ZUGFeRD_Test
{
    [TestClass]
    public class XRechnungUBLTests : TestBase
    {
        InvoiceProvider InvoiceProvider = new InvoiceProvider();
        ZUGFeRDVersion version = ZUGFeRDVersion.Version23;

        [TestMethod]
        public void TestParentLineId()
        {
            string path = @"..\..\..\..\demodata\zugferd21\zugferd_2p1_EXTENDED_Warenrechnung-factur-x.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            desc.TradeLineItems.Clear();
            desc.AdditionalReferencedDocuments.Clear();

            desc.AddTradeLineItem(
                lineID: "1",
                name: "Trennblätter A4",
                billedQuantity: 20m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 9.9m,
                grossUnitPrice: 9.9m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);

            desc.AddTradeLineItem(
                lineID: "2",
                name: "Abschlagsrechnungen",
                billedQuantity: 0m,
                unitCode: QuantityCodes.C62,
                netUnitPrice: 0m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 0m,
                taxType: TaxTypes.VAT);

            TradeLineItem tradeLineItem = desc.AddTradeLineItem(
                lineID: "2.1",
                name: "Abschlagsrechnung vom 01.01.2024",
                billedQuantity: -1m,
                unitCode: QuantityCodes.C62,
                netUnitPrice: 500,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);
            tradeLineItem.SetParentLineId("2");

            desc.AddTradeLineItem(
                lineID: "3",
                name: "Joghurt Banane",
                billedQuantity: 50m,
                unitCode: QuantityCodes.H87,
                netUnitPrice: 5.5m,
                grossUnitPrice: 5.5m,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 7.0m,
                taxType: TaxTypes.VAT);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.AreEqual(loadedInvoice.TradeLineItems.Count, 4);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].AssociatedDocument.ParentLineID, null);
            Assert.AreEqual(loadedInvoice.TradeLineItems[1].AssociatedDocument.ParentLineID, null);
            Assert.AreEqual(loadedInvoice.TradeLineItems[2].AssociatedDocument.ParentLineID, "2");
            Assert.AreEqual(loadedInvoice.TradeLineItems[3].AssociatedDocument.ParentLineID, null);
        }


        [TestMethod]
        public void TestInvoiceCreation()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.Invoicee, null);
            Assert.AreNotEqual(loadedInvoice.Seller, null);
            Assert.AreEqual(loadedInvoice.Taxes.Count, 2);
            Assert.AreEqual(loadedInvoice.SellerContact.Name, "Max Mustermann");
            Assert.IsNull(loadedInvoice.BuyerContact);
        } // !TestInvoiceCreation()


        [TestMethod]
        public void TestTradelineitemProductCharacterstics()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();

            desc.TradeLineItems[0].ApplicableProductCharacteristics = new ApplicableProductCharacteristic[]
                    {
                        new ApplicableProductCharacteristic()
                        {
                            Description = "Test Description",
                            Value = "1.5 kg"
                        },
                        new ApplicableProductCharacteristic()
                        {
                            Description = "UBL Characterstics 2",
                            Value = "3 kg"
                        },
                    }.ToList();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics.Count, 2);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics[0].Description, "Test Description");
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics[1].Value, "3 kg");
        } // !TestTradelineitemProductCharacterstics()


        /// <summary>
        /// https://github.com/stephanstapel/ZUGFeRD-csharp/issues/319
        /// </summary>
        [TestMethod]
        public void TestSkippingOfAllowanceChargeBasisAmount()
        {
            // actual values do not matter
            decimal basisAmount = 123.0m;
            decimal percent = 11.0m;
            decimal allowanceChargeBasisAmount = 121.0m;

            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.AddApplicableTradeTax(basisAmount, percent, TaxTypes.LOC, TaxCategoryCodes.K, allowanceChargeBasisAmount);
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Tax tax = loadedInvoice.Taxes.FirstOrDefault(t => t.TypeCode == TaxTypes.LOC);
            Assert.IsNotNull(tax);
            Assert.AreEqual(basisAmount, tax.BasisAmount);
            Assert.AreEqual(percent, tax.Percent);
            Assert.AreEqual(null, tax.AllowanceChargeBasisAmount);
        } // !TestInvoiceCreation()

        [TestMethod]
        public void TestAllowanceChargeOnDocumentLevel()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();

            // Test Values
            bool isDiscount = true;
            decimal? basisAmount = 123.45m;
            CurrencyCodes currency = CurrencyCodes.EUR;
            decimal actualAmount = 12.34m;
            string reason = "Gutschrift";
            TaxTypes taxTypeCode = TaxTypes.VAT;
            TaxCategoryCodes taxCategoryCode = TaxCategoryCodes.AA;
            decimal taxPercent = 19.0m;

            desc.AddTradeAllowanceCharge(isDiscount, basisAmount, currency, actualAmount, reason, taxTypeCode, taxCategoryCode, taxPercent);

            TradeAllowanceCharge? testAllowanceCharge = desc.GetTradeAllowanceCharges().FirstOrDefault();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, version, Profile.Extended, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            TradeAllowanceCharge loadedAllowanceCharge = loadedInvoice.GetTradeAllowanceCharges()[0];

            Assert.AreEqual(loadedInvoice.GetTradeAllowanceCharges().Count(), 1);
            Assert.AreEqual(loadedAllowanceCharge.ChargeIndicator, !isDiscount, message: "isDiscount");
            Assert.AreEqual(loadedAllowanceCharge.BasisAmount, basisAmount, message: "basisAmount");
            Assert.AreEqual(loadedAllowanceCharge.Currency, currency, message: "currency");
            Assert.AreEqual(loadedAllowanceCharge.Amount, actualAmount, message: "actualAmount");
            Assert.AreEqual(loadedAllowanceCharge.Reason, reason, message: "reason");
            Assert.AreEqual(loadedAllowanceCharge.Tax.TypeCode, taxTypeCode, message: "taxTypeCode");
            Assert.AreEqual(loadedAllowanceCharge.Tax.CategoryCode, taxCategoryCode, message: "taxCategoryCode");
            Assert.AreEqual(loadedAllowanceCharge.Tax.Percent, taxPercent, message: "taxPercent");

        } // !TestAllowanceChargeOnDocumentLevel

        [TestMethod]
        public void TestInvoiceWithAttachment()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
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

            desc.Save(ms, version, Profile.Extended, ZUGFeRDFormats.UBL);
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
        } // !TestInvoiceWithAttachment()

        
        
        [TestMethod]
        public void TestActualDeliveryDateWithoutDeliveryAddress()
        {
            DateTime timestamp = new DateTime(2024,08,11);
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            desc.ActualDeliveryDate = timestamp;

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // test the ActualDeliveryDate
            Assert.AreEqual(timestamp, loadedInvoice.ActualDeliveryDate);
            Assert.IsNull(loadedInvoice.ShipTo);
        } // !TestActualDeliveryDateWithoutDeliveryAddress()



        [TestMethod]
        public void TestActualDeliveryDateWithDeliveryAddress()
        {
            DateTime timestamp = new DateTime(2024, 08, 11);
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            desc.ActualDeliveryDate = timestamp;

            string shipToID = "1234";
            string shipToName = "Test ShipTo Name";
            CountryCodes shipToCountry = CountryCodes.DE;

            desc.ShipTo = new Party()
            {
                ID = new GlobalID()
                {
                    ID = shipToID
                },
                Name = shipToName,
                Country = shipToCountry
            };

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // test the ActualDeliveryDate
            Assert.AreEqual(timestamp, loadedInvoice.ActualDeliveryDate);
            Assert.IsNotNull(loadedInvoice.ShipTo);
            Assert.IsNotNull(loadedInvoice.ShipTo.ID);
            Assert.AreEqual(loadedInvoice.ShipTo.ID.ID, shipToID);
            Assert.AreEqual(loadedInvoice.ShipTo.Name, shipToName);
            Assert.AreEqual(loadedInvoice.ShipTo.Country, shipToCountry);
        } // !TestActualDeliveryDateWithDeliveryAddress()



        [TestMethod]
        public void TestActualDeliveryAddressWithoutDeliveryDate()
        {            
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            // ActualDeliveryDate is set by the InvoiceProvider, we are resetting it to the default value
            desc.ActualDeliveryDate = null;

            string shipToID = "1234";
            string shipToName = "Test ShipTo Name";
            CountryCodes shipToCountry = CountryCodes.DE;

            desc.ShipTo = new Party()
            {
                ID = new GlobalID()
                {
                    ID = shipToID
                },
                Name = shipToName,
                Country = shipToCountry
            };

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // test the ActualDeliveryDate
            Assert.IsNull(loadedInvoice.ActualDeliveryDate);
            Assert.IsNotNull(loadedInvoice.ShipTo);
            Assert.IsNotNull(loadedInvoice.ShipTo.ID);
            Assert.AreEqual(loadedInvoice.ShipTo.ID.ID, shipToID);
            Assert.AreEqual(loadedInvoice.ShipTo.Name, shipToName);
            Assert.AreEqual(loadedInvoice.ShipTo.Country, shipToCountry);
        } // !TestActualDeliveryAddressWithoutDeliveryDate()


        [TestMethod]
        public void TestTaxTypes()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            
            // test writing and parsing
            Assert.AreEqual(loadedInvoice.Taxes.Count, 2);
            Assert.IsTrue(loadedInvoice.Taxes.All(t => t.TypeCode == TaxTypes.VAT));

            // test the raw xml file
            string content = Encoding.UTF8.GetString(ms.ToArray());
            Assert.IsFalse(content.Contains("<cbc:ID>VA</cbc:ID>", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(content.Contains("<cbc:ID>VAT</cbc:ID>", StringComparison.OrdinalIgnoreCase));

            Assert.IsFalse(content.Contains("<cbc:ID>FC</cbc:ID>", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(content.Contains("<cbc:ID>ID</cbc:ID>", StringComparison.OrdinalIgnoreCase));
        } // !TestInvoiceCreation()


        /// <summary>
        /// We expect this format:
        ///   <cac:PaymentTerms>
        ///     <cbc:Note>
        ///       #SKONTO#TAGE#14#PROZENT=0.00#BASISBETRAG=123.45#
        ///     </cbc:Note>
        ///   </cac:PaymentTerms>
        /// </summary>
        [TestMethod]
        public void TestSingleSkontoForCorrectIndention()
        {
            var desc = InvoiceProvider.CreateInvoice();

            desc.ClearTradePaymentTerms();
            desc.AddTradePaymentTerms("#SKONTO#TAGE#14#PROZENT=0.00#BASISBETRAG=123.45#");

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);

            var lines = new StreamReader(ms).ReadToEnd().Split(new[] { System.Environment.NewLine }, StringSplitOptions.None).ToList();

            bool insidePaymentTerms = false;
            bool insideCbcNote = false;
            int noteIndentation = -1;

            foreach (var line in lines)
            {
                // Trim the line to remove leading/trailing whitespace
                var trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("<cac:PaymentTerms>", StringComparison.OrdinalIgnoreCase))
                {
                    insidePaymentTerms = true;
                    continue;
                }
                else if (!insidePaymentTerms)
                {
                    continue;
                }

                // Check if we found the opening <cbc:Note>
                if (!insideCbcNote && trimmedLine.StartsWith("<cbc:Note>", StringComparison.OrdinalIgnoreCase))
                {
                    insideCbcNote = true;
                    noteIndentation = line.TakeWhile(char.IsWhiteSpace).Count();
                    Assert.IsTrue(noteIndentation >= 0, "Indentation for <cbc:Note> should be non-negative.");
                    continue;
                }

                // Check if we found the closing </cbc:Note>
                if (insideCbcNote && trimmedLine.StartsWith("</cbc:Note>", StringComparison.OrdinalIgnoreCase))
                {
                    int endNoteIndentation = line.TakeWhile(char.IsWhiteSpace).Count();
                    Assert.AreEqual(noteIndentation, endNoteIndentation); // Ensure closing tag matches indentation
                    insideCbcNote = false;
                    break;
                }

                // After finding <cbc:Note>, check for indentation of the next line
                if (insideCbcNote)
                {
                    int indention = line.TakeWhile(char.IsWhiteSpace).Count();
                    Assert.AreEqual(noteIndentation + 2, indention); // Ensure next line is indented one more
                    continue;
                }                
            }

            // Assert that we entered and exited the <cbc:Note> block
            Assert.IsFalse(insideCbcNote, "We should have exited the <cbc:Note> block.");
        } // !TestSingleSkontoForCorrectIndention()


        /// <summary>
        /// We expect this format:
        ///   <cac:PaymentTerms>
        ///     <cbc:Note>
        ///       #SKONTO#TAGE#14#PROZENT=5.00#BASISBETRAG=123.45#
        ///       #SKONTO#TAGE#21#PROZENT=1.00#BASISBETRAG=123.45#
        ///     </cbc:Note>
        ///   </cac:PaymentTerms>
        /// </summary>
        [TestMethod]
        public void TestMultiSkontoForCorrectIndention()
        {
            var desc = InvoiceProvider.CreateInvoice();

            desc.ClearTradePaymentTerms();
            desc.AddTradePaymentTerms("#SKONTO#TAGE#14#PROZENT=5.00#BASISBETRAG=123.45#");
            desc.AddTradePaymentTerms("#SKONTO#TAGE#21#PROZENT=1.00#BASISBETRAG=123.45#");

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);

            var lines = new StreamReader(ms).ReadToEnd().Split(new[] { System.Environment.NewLine }, StringSplitOptions.None).ToList();

            bool insidePaymentTerms = false;
            bool insideCbcNote = false;
            int noteIndentation = -1;

            foreach (var line in lines)
            {
                // Trim the line to remove leading/trailing whitespace
                var trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("<cac:PaymentTerms>", StringComparison.OrdinalIgnoreCase))
                {
                    insidePaymentTerms = true;
                    continue;
                }
                else if (!insidePaymentTerms)
                {
                    continue;
                }

                // Check if we found the opening <cbc:Note>
                if (!insideCbcNote && trimmedLine.StartsWith("<cbc:Note>", StringComparison.OrdinalIgnoreCase))
                {
                    insideCbcNote = true;
                    noteIndentation = line.TakeWhile(char.IsWhiteSpace).Count();
                    Assert.IsTrue(noteIndentation >= 0, "Indentation for <cbc:Note> should be non-negative.");
                    continue;
                }

                // Check if we found the closing </cbc:Note>
                if (insideCbcNote && trimmedLine.StartsWith("</cbc:Note>", StringComparison.OrdinalIgnoreCase))
                {
                    int endNoteIndentation = line.TakeWhile(char.IsWhiteSpace).Count();
                    Assert.AreEqual(noteIndentation, endNoteIndentation); // Ensure closing tag matches indentation
                    insideCbcNote = false;
                    break;
                }

                // After finding <cbc:Note>, check for indentation of the next line
                if (insideCbcNote)
                {
                    int indention = line.TakeWhile(char.IsWhiteSpace).Count();
                    Assert.AreEqual(noteIndentation + 2, indention); // Ensure next line is indented one more
                    continue;
                }                
            }

            // Assert that we entered and exited the <cbc:Note> block
            Assert.IsFalse(insideCbcNote, "We should have exited the <cbc:Note> block.");
        } // !TestMultiSkontoForCorrectIndention()
    }
}
