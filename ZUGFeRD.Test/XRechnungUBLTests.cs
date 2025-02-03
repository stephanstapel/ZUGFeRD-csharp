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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace s2industries.ZUGFeRD.Test
{
    [TestClass]
    public class XRechnungUBLTests : TestBase
    {
        private InvoiceProvider _InvoiceProvider = new InvoiceProvider();
        private ZUGFeRDVersion _Version = ZUGFeRDVersion.Version23;

        [TestMethod]
        public void TestParentLineId()
        {
            string path = @"..\..\..\..\demodata\xRechnung\xRechnung UBL.xml";
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

            TradeLineItem subTradeLineItem1 = desc.AddTradeLineItem(
                lineID: "2.1",
                name: "Abschlagsrechnung vom 01.01.2024",
                billedQuantity: -1m,
                unitCode: QuantityCodes.C62,
                netUnitPrice: 500,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);
            subTradeLineItem1.SetParentLineId("2");

            TradeLineItem subTradeLineItem2 = desc.AddTradeLineItem(
                lineID: "2.2",
                name: "Abschlagsrechnung vom 20.01.2024",
                billedQuantity: -1m,
                unitCode: QuantityCodes.C62,
                netUnitPrice: 500,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);
            subTradeLineItem2.SetParentLineId("2");

            TradeLineItem subTradeLineItem3 = desc.AddTradeLineItem(
                lineID: "2.2.1",
                name: "Abschlagsrechnung vom 10.01.2024",
                billedQuantity: -1m,
                unitCode: QuantityCodes.C62,
                netUnitPrice: 100,
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19.0m,
                taxType: TaxTypes.VAT);
            subTradeLineItem3.SetParentLineId("2.2");

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
            Assert.AreEqual(loadedInvoice.TradeLineItems.Count, 6);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].AssociatedDocument.ParentLineID, null);
            Assert.AreEqual(loadedInvoice.TradeLineItems[1].AssociatedDocument.ParentLineID, null);
            Assert.AreEqual(loadedInvoice.TradeLineItems[2].AssociatedDocument.ParentLineID, "2");
            Assert.AreEqual(loadedInvoice.TradeLineItems[3].AssociatedDocument.ParentLineID, "2");
            Assert.AreEqual(loadedInvoice.TradeLineItems[4].AssociatedDocument.ParentLineID, "2.2");
            Assert.AreEqual(loadedInvoice.TradeLineItems[5].AssociatedDocument.ParentLineID, null);
        }


        [TestMethod]
        public void TestInvoiceCreation()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
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
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

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

            desc.Save(ms, _Version, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics.Count, 2);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics[0].Description, "Test Description");
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics[1].Value, "3 kg");
        } // !TestTradelineitemProductCharacterstics()

        [TestMethod]
        public void TestSpecialUnitCodes()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            desc.TradeLineItems[0].UnitCode = QuantityCodes._4G;
            desc.TradeLineItems[1].UnitCode = QuantityCodes.H87;

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, _Version, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // test the raw xml file
            string content = Encoding.UTF8.GetString(ms.ToArray());
            Assert.IsTrue(content.Contains("unitCode=\"H87\"", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(content.Contains("unitCode=\"4G\"", StringComparison.OrdinalIgnoreCase));

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].UnitCode, QuantityCodes._4G);
            Assert.AreEqual(loadedInvoice.TradeLineItems[1].UnitCode, QuantityCodes.H87);
        } // !TestSpecialUnitCodes()


        [TestMethod]
        public void TestTradelineitemAdditionalDocuments()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            desc.TradeLineItems[0].AddAdditionalReferencedDocument("testid", AdditionalReferencedDocumentTypeCode.InvoiceDataSheet, referenceTypeCode: ReferenceTypeCodes.ON);
            desc.TradeLineItems[0].AddAdditionalReferencedDocument("testid2", AdditionalReferencedDocumentTypeCode.InvoiceDataSheet, referenceTypeCode: ReferenceTypeCodes.ON);

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, _Version, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].GetAdditionalReferencedDocuments().Count, 2);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].GetAdditionalReferencedDocuments()[0].ID, "testid");
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].GetAdditionalReferencedDocuments()[1].ID, "testid2");
        } // !TestTradelineitemAdditionalDocuments()


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

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.AddApplicableTradeTax(basisAmount, percent, basisAmount / 100m * percent, TaxTypes.LOC, TaxCategoryCodes.K, allowanceChargeBasisAmount);
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, _Version, Profile.XRechnung, ZUGFeRDFormats.UBL);
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
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();

            // Test Values
            bool isDiscount = true;
            decimal? basisAmount = 123.45m;
            CurrencyCodes currency = CurrencyCodes.EUR;
            decimal actualAmount = 12.34m;
            string reason = "Gutschrift";
            AllowanceReasonCodes reasonCode = AllowanceReasonCodes.Packaging;
            TaxTypes taxTypeCode = TaxTypes.VAT;
            TaxCategoryCodes taxCategoryCode = TaxCategoryCodes.AA;
            decimal taxPercent = 19.0m;

            desc.AddTradeAllowanceCharge(isDiscount, basisAmount, currency, actualAmount, reason, taxTypeCode, taxCategoryCode, taxPercent, reasonCode);

            desc.TradeLineItems[0].AddTradeAllowanceCharge(true, CurrencyCodes.EUR, 100, 10, "test", reasonCode);

            TradeAllowanceCharge? testAllowanceCharge = desc.GetTradeAllowanceCharges().FirstOrDefault();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, _Version, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            TradeAllowanceCharge loadedAllowanceCharge = loadedInvoice.GetTradeAllowanceCharges()[0];

            Assert.AreEqual(loadedInvoice.GetTradeAllowanceCharges().Count(), 1);
            Assert.AreEqual(loadedAllowanceCharge.ChargeIndicator, !isDiscount, message: "isDiscount");
            Assert.AreEqual(loadedAllowanceCharge.BasisAmount, basisAmount, message: "basisAmount");
            Assert.AreEqual(loadedAllowanceCharge.Currency, currency, message: "currency");
            Assert.AreEqual(loadedAllowanceCharge.Amount, actualAmount, message: "actualAmount");
            Assert.AreEqual(loadedAllowanceCharge.Reason, reason, message: "reason");
            Assert.AreEqual(loadedAllowanceCharge.ReasonCode, reasonCode, message: "reasonCode");
            Assert.AreEqual(loadedAllowanceCharge.Tax.TypeCode, taxTypeCode, message: "taxTypeCode");
            Assert.AreEqual(loadedAllowanceCharge.Tax.CategoryCode, taxCategoryCode, message: "taxCategoryCode");
            Assert.AreEqual(loadedAllowanceCharge.Tax.Percent, taxPercent, message: "taxPercent");

        } // !TestAllowanceChargeOnDocumentLevel


        [TestMethod]
        public void TestInvoiceWithAttachment()
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

            desc.Save(ms, _Version, Profile.XRechnung, ZUGFeRDFormats.UBL);
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
            DateTime timestamp = new DateTime(2024, 08, 11);
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
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
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
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
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            // ActualDeliveryDate is set by the _InvoiceProvider, we are resetting it to the default value
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
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
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
            var desc = _InvoiceProvider.CreateInvoice();

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
            var desc = _InvoiceProvider.CreateInvoice();

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

        [TestMethod]
        public void TestBuyerOrderReferenceLineId()
        {
            string path = @"..\..\..\..\demodata\xRechnung\xRechnung with LineId.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.TradeLineItems[0].BuyerOrderReferencedDocument.LineID, "6171175.1");
        }

        [TestMethod]
        public void TestMultipleCreditorBankAccounts()
        {
            string iban1 = "DE901213213312311231";
            string iban2 = "DE911213213312311231";
            string bic1 = "BIC-Test";
            string bic2 = "BIC-Test2";

            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            desc.CreditorBankAccounts.Clear();

            desc.CreditorBankAccounts.Add(new BankAccount()
            {
                IBAN = iban1,
                BIC = bic1
            });
            desc.CreditorBankAccounts.Add(new BankAccount()
            {
                IBAN = iban2,
                BIC = bic2
            });

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            // test the BankAccounts
            Assert.AreEqual(iban1, loadedInvoice.CreditorBankAccounts[0].IBAN);
            Assert.AreEqual(bic1, loadedInvoice.CreditorBankAccounts[0].BIC);
            Assert.AreEqual(iban2, loadedInvoice.CreditorBankAccounts[1].IBAN);
            Assert.AreEqual(bic2, loadedInvoice.CreditorBankAccounts[1].BIC);
        } // !TestMultipleCreditorBankAccounts()

        [TestMethod]
        public void TestBuyerPartyIdwithoutGloablID()
        {
            var d = new InvoiceDescriptor();
            d.Type = InvoiceType.Invoice;
            d.InvoiceNo = "471102";
            d.Currency = CurrencyCodes.EUR;
            d.InvoiceDate = new DateTime(2018, 3, 5);
            d.SetBuyer(
                id: "GE2020211",
                globalID: null,
                name: "Kunden AG Mitte",
                postcode: "69876",
                city: "Frankfurt",
                street: "Kundenstraße 15",
                country: CountryCodes.DE);

            using (var stream = new MemoryStream())
            {
                d.Save(stream, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
                stream.Seek(0, SeekOrigin.Begin);
                InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(stream);

                Assert.AreEqual(loadedInvoice.Buyer.ID.ID, "GE2020211");
            }
        } // !TestInDebitInvoiceTheFinancialAccountNameShouldNotExist()


        [TestMethod]
        public void TestPartyIdentificationForSeller()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            loadedInvoice.SetPaymentMeans(PaymentMeansTypeCodes.SEPACreditTransfer, "Hier sind Informationen", "DE75512108001245126199", "[Mandate reference identifier]");
            MemoryStream resultStream = new MemoryStream();
            loadedInvoice.Save(resultStream, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);

            // test the raw xml file
            XmlDocument doc = new XmlDocument();
            doc.Load(resultStream);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ubl", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            // PartyIdentification may only exist once
            Assert.AreEqual(doc.SelectNodes("//cac:AccountingSupplierParty//cac:PartyIdentification", nsmgr).Count, 1);

            // PartyIdentification may only exist once
            Assert.AreEqual(doc.SelectNodes("//cac:AccountingCustomerParty//cac:PartyIdentification", nsmgr).Count, 1);
        } // !TestPartyIdentificationForSeller()


        [TestMethod]
        public void TestPartyIdentificationShouldExistOneTime()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            MemoryStream resultStream = new MemoryStream();
            loadedInvoice.Save(resultStream, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);

            // test the raw xml file
            XmlDocument doc = new XmlDocument();
            doc.Load(resultStream);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ubl", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            Assert.AreEqual(doc.SelectNodes("//cac:PartyIdentification", nsmgr).Count, 1);
        } // !TestPartyIdentificationShouldNotExist()


        [TestMethod]
        public void TestInDebitInvoiceTheFinancialAccountNameAndFinancialInstitutionBranchShouldNotExist()
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
                198.00m / 100m * 19.00m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);

            using (var stream = new MemoryStream())
            {
                d.Save(stream, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
                stream.Seek(0, SeekOrigin.Begin);

                // test the raw xml file
                string content = Encoding.UTF8.GetString(stream.ToArray());

                Assert.IsFalse(Regex.IsMatch(content, @"<cac:PaymentMandate.*>.*<cbc:Name.*>.*</cac:PaymentMandate>", RegexOptions.Singleline));
                Assert.IsFalse(Regex.IsMatch(content, "<cac:PaymentMandate.*>.*<cac:FinancialInstitutionBranch.*></cac:PaymentMandate>", RegexOptions.Singleline));
            }
        } // !TestInDebitInvoiceTheFinancialAccountNameShouldNotExist()

        [TestMethod]
        public void TestInDebitInvoiceThePaymentMandateIdShouldExist()
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
                198.00m / 100m * 19.00m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);

            using (var stream = new MemoryStream())
            {
                d.Save(stream, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
                stream.Seek(0, SeekOrigin.Begin);

                // test the raw xml file
                string content = Encoding.UTF8.GetString(stream.ToArray());

                Assert.IsTrue(Regex.IsMatch(content, @"<cac:PaymentMeans.*>.*<cac:PaymentMandate.*>.*<cbc:ID.*>REF A-123</cbc:ID.*>.*</cac:PaymentMandate>", RegexOptions.Singleline));
            }
        } // !TestInDebitInvoiceThePaymentMandateIdShouldExist()

        [TestMethod]
        public void TestInvoiceWithoutOrderReferenceShouldNotWriteEmptyOrderReferenceElement()
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
                198.00m / 100m * 19.00m,
                TaxTypes.VAT,
                TaxCategoryCodes.S);

            using (var stream = new MemoryStream())
            {
                d.Save(stream, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
                stream.Seek(0, SeekOrigin.Begin);

                // test the raw xml file
                string content = Encoding.UTF8.GetString(stream.ToArray());

                Assert.IsFalse(content.Contains("OrderReference"));
            }
        } // !TestInvoiceWithoutOrderReferenceShouldNotWriteEmptyOrderReferenceElement()


        [TestMethod]
        public void TestApplicableTradeTaxWithExemption()
        {
            InvoiceDescriptor descriptor = _InvoiceProvider.CreateInvoice();
            int taxCount = descriptor.Taxes.Count;
            descriptor.AddApplicableTradeTax(123.00m, 23m, 23m, TaxTypes.VAT, TaxCategoryCodes.S, exemptionReasonCode: TaxExemptionReasonCodes.VATEX_132_2, exemptionReason: "Tax exemption reason");

            MemoryStream ms = new MemoryStream();
            descriptor.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.IsNotNull(loadedInvoice);

            Assert.AreEqual(loadedInvoice.Taxes.Count, taxCount + 1);
            Assert.AreEqual(loadedInvoice.Taxes.Last().ExemptionReason, "Tax exemption reason");
            Assert.AreEqual(loadedInvoice.Taxes.Last().ExemptionReasonCode, TaxExemptionReasonCodes.VATEX_132_2);
        } // !TestApplicableTradeTaxWithExemption()


        [TestMethod]
        public void TestNote()
        {
            InvoiceDescriptor descriptor = _InvoiceProvider.CreateInvoice();
            String note = System.Guid.NewGuid().ToString();
            descriptor.AddNote(note);

            MemoryStream ms = new MemoryStream();
            descriptor.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.IsNotNull(loadedInvoice);

            Assert.AreEqual(loadedInvoice.Notes.Count, 3); // 2 notes are already added by the _InvoiceProvider
            Assert.AreEqual(loadedInvoice.Notes.Last().Content, note);
        } // !TestNote()


        [TestMethod]
        public void TestDespatchDocumentReference()
        {
            String reference = System.Guid.NewGuid().ToString();
            DateTime adviseDate = DateTime.Today;

            InvoiceDescriptor descriptor = _InvoiceProvider.CreateInvoice();
            descriptor.SetDespatchAdviceReferencedDocument(reference, adviseDate);

            MemoryStream ms = new MemoryStream();
            descriptor.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);

            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);
            Assert.IsNotNull(loadedInvoice);

            Assert.IsNotNull(loadedInvoice.DespatchAdviceReferencedDocument);
            Assert.AreEqual(loadedInvoice.DespatchAdviceReferencedDocument.ID, reference);
            Assert.IsNull(loadedInvoice.DespatchAdviceReferencedDocument.IssueDateTime); // not defined in Peppol standard!
        } // !TestNote()


        [TestMethod]
        public void TestSampleCreditNote326()
        {
            string path = @"..\..\..\..\demodata\xRechnung\ubl-cn-br-de-17-test-557-code-326.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Type, InvoiceType.PartialInvoice);
            Assert.AreEqual(desc.Notes.Count, 1);
            Assert.AreEqual(desc.Notes.First().Content, "Invoice Note Description");
            Assert.AreEqual(desc.TradeLineItems.Count, 2);
            Assert.AreEqual(desc.TradeLineItems.First().Name, "Beratung");
            Assert.AreEqual(desc.TradeLineItems.First().Description, "Anforderungmanagament");
            Assert.AreEqual(desc.TradeLineItems.Last().Name, "Beratung");
            Assert.AreEqual(desc.TradeLineItems.Last().Description, String.Empty);
        } // !TestSampleCreditNote326()
        

        [TestMethod]
        public void TestSampleCreditNote384()
        {
            string path = @"..\..\..\..\demodata\xRechnung\ubl-cn-br-de-17-test-559-code-384.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            Stream s = File.Open(path, FileMode.Open);
            InvoiceDescriptor desc = InvoiceDescriptor.Load(s);
            s.Close();

            Assert.AreEqual(desc.Type, InvoiceType.Correction);
            Assert.AreEqual(desc.Notes.Count, 1);
            Assert.AreEqual(desc.Notes.First().Content, "Invoice Note Description");
            Assert.AreEqual(desc.TradeLineItems.Count, 2);
            Assert.AreEqual(desc.TradeLineItems.First().Name, "Beratung");
            Assert.AreEqual(desc.TradeLineItems.First().Description, "Anforderungmanagament");
            Assert.AreEqual(desc.TradeLineItems.Last().Name, "Beratung");
            Assert.AreEqual(desc.TradeLineItems.Last().Description, String.Empty);
        } // !TestSampleCreditNote326()


        [TestMethod]
        public void TestReferenceXRechnung21UBL()
        {
            string path = @"..\..\..\..\demodata\xRechnung\xRechnung UBL.xml";
            path = _makeSurePathIsCrossPlatformCompatible(path);

            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            Assert.AreEqual(desc.Profile, Profile.XRechnung);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);

            Assert.AreEqual(desc.InvoiceNo, "0815-99-1-a");
            Assert.AreEqual(desc.InvoiceDate, new DateTime(2020, 6, 21));
            Assert.AreEqual(desc.PaymentReference, "0815-99-1-a");
            Assert.AreEqual(desc.OrderNo, "0815-99-1");
            Assert.AreEqual(desc.Currency, CurrencyCodes.EUR);

            Assert.AreEqual(desc.Buyer.Name, "Rechnungs Roulette GmbH & Co KG");
            Assert.AreEqual(desc.Buyer.City, "Klein Schlappstadt a.d. Lusche");
            Assert.AreEqual(desc.Buyer.Postcode, "12345");
            Assert.AreEqual(desc.Buyer.Country, (CountryCodes)276);
            Assert.AreEqual(desc.Buyer.Street, "Beispielgasse 17b");
            Assert.AreEqual(desc.Buyer.SpecifiedLegalOrganization.TradingBusinessName, "Rechnungs Roulette GmbH & Co KG");

            Assert.AreEqual(desc.BuyerContact.Name, "Manfred Mustermann");
            Assert.AreEqual(desc.BuyerContact.EmailAddress, "manfred.mustermann@rr.de");
            Assert.AreEqual(desc.BuyerContact.PhoneNo, "012345 98 765 - 44");

            Assert.AreEqual(desc.Seller.Name, "Harry Hirsch Holz- und Trockenbau");
            Assert.AreEqual(desc.Seller.City, "Klein Schlappstadt a.d. Lusche");
            Assert.AreEqual(desc.Seller.Postcode, "12345");
            Assert.AreEqual(desc.Seller.Country, (CountryCodes)276);
            Assert.AreEqual(desc.Seller.Street, "Beispielgasse 17a");
            Assert.AreEqual(desc.Seller.SpecifiedLegalOrganization.TradingBusinessName, "Harry Hirsch Holz- und Trockenbau");

            Assert.AreEqual(desc.SellerContact.Name, "Harry Hirsch");
            Assert.AreEqual(desc.SellerContact.EmailAddress, "harry.hirsch@hhhtb.de");
            Assert.AreEqual(desc.SellerContact.PhoneNo, "012345 78 657 - 8");

            Assert.AreEqual(desc.TradeLineItems.Count, 2);

            Assert.AreEqual(desc.TradeLineItems[0].SellerAssignedID, "0815");
            Assert.AreEqual(desc.TradeLineItems[0].Name, "Leimbinder");
            Assert.AreEqual(desc.TradeLineItems[0].Description, "Leimbinder 2x18m; Birke");
            Assert.AreEqual(desc.TradeLineItems[0].BilledQuantity, 1);
            Assert.AreEqual(desc.TradeLineItems[0].LineTotalAmount, 1245.98m);
            Assert.AreEqual(desc.TradeLineItems[0].TaxPercent, 19);

            Assert.AreEqual(desc.TradeLineItems[1].SellerAssignedID, "MON");
            Assert.AreEqual(desc.TradeLineItems[1].Name, "Montage");
            Assert.AreEqual(desc.TradeLineItems[1].Description, "Montage durch Fachpersonal");
            Assert.AreEqual(desc.TradeLineItems[1].BilledQuantity, 1);
            Assert.AreEqual(desc.TradeLineItems[1].LineTotalAmount, 200.00m);
            Assert.AreEqual(desc.TradeLineItems[1].TaxPercent, 7);

            Assert.AreEqual(desc.LineTotalAmount, 1445.98m);
            Assert.AreEqual(desc.TaxTotalAmount, 250.74m);
            Assert.AreEqual(desc.GrandTotalAmount, 1696.72m);
            Assert.AreEqual(desc.DuePayableAmount, 1696.72m);

            Assert.AreEqual(desc.Taxes[0].TaxAmount, 236.74m);
            Assert.AreEqual(desc.Taxes[0].BasisAmount, 1245.98m);
            Assert.AreEqual(desc.Taxes[0].Percent, 19);
            Assert.AreEqual(desc.Taxes[0].TypeCode, TaxTypes.VAT);
            Assert.AreEqual(desc.Taxes[0].CategoryCode, TaxCategoryCodes.S);

            Assert.AreEqual(desc.Taxes[1].TaxAmount, 14.0000m);
            Assert.AreEqual(desc.Taxes[1].BasisAmount, 200.00m);
            Assert.AreEqual(desc.Taxes[1].Percent, 7);
            Assert.AreEqual(desc.Taxes[1].TypeCode, TaxTypes.VAT);
            Assert.AreEqual(desc.Taxes[1].CategoryCode, TaxCategoryCodes.S);

            Assert.AreEqual(desc.GetTradePaymentTerms().FirstOrDefault().DueDate, new DateTime(2020, 6, 21));

            Assert.AreEqual(desc.CreditorBankAccounts[0].IBAN, "DE12500105170648489890");
            Assert.AreEqual(desc.CreditorBankAccounts[0].BIC, "INGDDEFFXXX");
            Assert.AreEqual(desc.CreditorBankAccounts[0].Name, "Harry Hirsch");

            Assert.AreEqual(desc.PaymentMeans.TypeCode, (PaymentMeansTypeCodes)30);
        } // !TestReferenceXRechnung21UBL()


        [TestMethod]
        public void TestDecimals()
        {
            InvoiceDescriptor desc = this._InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();

            decimal netUnitPrice = 9.123456789m;
            decimal lineTotalTotalAmount = 123.456789m;
            decimal taxBasisAmount = 123.456789m;
            decimal grandTotalAmount = 123.456789m;
            decimal duePayableAmount = 123.456789m;

            desc.LineTotalAmount = lineTotalTotalAmount;
            desc.TaxBasisAmount = taxBasisAmount;
            desc.GrandTotalAmount = grandTotalAmount;
            desc.DuePayableAmount = duePayableAmount;

            desc.AddTradeLineItem(
                lineID: "1",
                id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4012345001235"),
                sellerAssignedID: "TB100A4",
                name: "Trennblätter A4",
                billedQuantity: 20,
                unitCode: QuantityCodes.H87,
                netUnitPrice: netUnitPrice,
                grossUnitPrice: null, // isn't written in UBL format
                categoryCode: TaxCategoryCodes.S,
                taxPercent: 19m,
                taxType: TaxTypes.VAT);

            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);
            desc = InvoiceDescriptor.Load(ms);

            string invoiceAsString = Encoding.UTF8.GetString(ms.ToArray());

            // PriceAmount might have 4 decimals
            Assert.IsFalse(invoiceAsString.Contains($">{Math.Round(netUnitPrice, 2, MidpointRounding.AwayFromZero).ToString("F2", CultureInfo.InvariantCulture)}<"));
            Assert.IsTrue(invoiceAsString.Contains($">{Math.Round(netUnitPrice, 4, MidpointRounding.AwayFromZero).ToString("F4", CultureInfo.InvariantCulture)}<"));
            Assert.AreEqual(desc.TradeLineItems.First().NetUnitPrice, Math.Round(netUnitPrice, 4, MidpointRounding.AwayFromZero));

            // Grand total, due payable etc. must have two decimals max
            Assert.IsTrue(invoiceAsString.Contains($">{Math.Round(lineTotalTotalAmount, 2, MidpointRounding.AwayFromZero).ToString("F2", CultureInfo.InvariantCulture)}<"));
            Assert.IsFalse(invoiceAsString.Contains($">{Math.Round(lineTotalTotalAmount, 4, MidpointRounding.AwayFromZero).ToString("F4", CultureInfo.InvariantCulture)}<"));
            Assert.AreEqual(desc.LineTotalAmount, Math.Round(lineTotalTotalAmount, 2, MidpointRounding.AwayFromZero));

            Assert.IsTrue(invoiceAsString.Contains($">{Math.Round(taxBasisAmount, 2, MidpointRounding.AwayFromZero).ToString("F2", CultureInfo.InvariantCulture)}<"));
            Assert.IsFalse(invoiceAsString.Contains($">{Math.Round(taxBasisAmount, 4, MidpointRounding.AwayFromZero).ToString("F4", CultureInfo.InvariantCulture)}<"));
            Assert.AreEqual(desc.TaxBasisAmount, Math.Round(taxBasisAmount, 2, MidpointRounding.AwayFromZero));

            Assert.IsTrue(invoiceAsString.Contains($">{Math.Round(grandTotalAmount, 2, MidpointRounding.AwayFromZero).ToString("F2", CultureInfo.InvariantCulture)}<"));
            Assert.IsFalse(invoiceAsString.Contains($">{Math.Round(grandTotalAmount, 4, MidpointRounding.AwayFromZero).ToString("F4", CultureInfo.InvariantCulture)}<"));
            Assert.AreEqual(desc.GrandTotalAmount, Math.Round(grandTotalAmount, 2, MidpointRounding.AwayFromZero));

            Assert.IsTrue(invoiceAsString.Contains($">{Math.Round(duePayableAmount, 2, MidpointRounding.AwayFromZero).ToString("F2", CultureInfo.InvariantCulture)}<"));
            Assert.IsFalse(invoiceAsString.Contains($">{Math.Round(duePayableAmount, 4, MidpointRounding.AwayFromZero).ToString("F4", CultureInfo.InvariantCulture)}<"));
            Assert.AreEqual(desc.DuePayableAmount, Math.Round(duePayableAmount, 2, MidpointRounding.AwayFromZero));
        } // !TestDecimals()


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

            desc.Save(ms, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);
            desc.Save("e:\\output.xml", ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.UBL);


            // string comparison
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            string content = reader.ReadToEnd();
            Assert.IsTrue(content.Contains("<cac:CommodityClassification>"));
            Assert.IsTrue(content.Contains("<cbc:ItemClassificationCode listID=\"HS\" listVersionID=\"List Version ID Value\">Class Code</cbc:ItemClassificationCode>"));
            Assert.IsTrue(content.Contains("</cac:CommodityClassification>"));

            // structure comparison
            ms.Seek(0, SeekOrigin.Begin);
            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(DesignatedProductClassificationClassCodes.HS, loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ListID);
            Assert.AreEqual("List Version ID Value", loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ListVersionID);
            Assert.AreEqual("Class Code", loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassCode);
            Assert.AreEqual(String.Empty, loadedInvoice.TradeLineItems.First().GetDesignatedProductClassifications().First().ClassName);
        } // !TestDesignatedProductClassificationWithFullClassification()
    }
}
