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

namespace ZUGFeRD_Test
{
    [TestClass]
    public class ZUGFeRD10Tests
    {
        InvoiceProvider InvoiceProvider = new InvoiceProvider();

        [TestMethod]
        public void TestReferenceComfortInvoice()
        {
            string path = @"..\..\..\demodata\zugferd10\ZUGFeRD_1p0_COMFORT_Einfach.xml";
            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            Assert.AreEqual(desc.Profile, Profile.Comfort);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
        } // !TestReferenceComfortInvoice()


        [TestMethod]
        public void TestReferenceComfortInvoiceRabattiert()
        {
            string path = @"..\..\..\demodata\zugferd10\ZUGFeRD_1p0_COMFORT_Rabatte.xml";
            InvoiceDescriptor desc = InvoiceDescriptor.Load(path);

            desc.Save("test.xml", ZUGFeRDVersion.Version1, Profile.Comfort);

            Assert.AreEqual(desc.Profile, Profile.Comfort);
            Assert.AreEqual(desc.Type, InvoiceType.Invoice);
            Assert.AreEqual(desc.CreditorBankAccounts[0].BankName, "Hausbank München");
        } // !TestReferenceComfortInvoiceRabattiert()


        [TestMethod]
        public void TestStoringInvoiceViaFile()
        {
            string path = "output.xml";
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.Save(path, ZUGFeRDVersion.Version1, Profile.Comfort);

            InvoiceDescriptor desc2 = InvoiceDescriptor.Load(path);
            // TODO: Add more asserts
        } // !TestStoringInvoiceViaFile()


        [TestMethod]
        public void TestStoringInvoiceViaStreams()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();

            string path = "output_stream.xml";
            FileStream saveStream = new FileStream(path, FileMode.Create);
            desc.Save(saveStream, ZUGFeRDVersion.Version1, Profile.Comfort);
            saveStream.Close();

            FileStream loadStream = new FileStream(path, FileMode.Open);
            InvoiceDescriptor desc2 = InvoiceDescriptor.Load(loadStream);
            loadStream.Close();

            Assert.AreEqual(desc2.Profile, Profile.Comfort);
            Assert.AreEqual(desc2.Type, InvoiceType.Invoice);


            // try again with a memory stream
            MemoryStream ms = new MemoryStream();
            desc.Save(ms, ZUGFeRDVersion.Version1, Profile.Comfort);

            byte[] data = ms.ToArray();
            string s = System.Text.Encoding.Default.GetString(data);
            // TODO: Add more asserts
        } // !TestStoringInvoiceViaStream()


        [TestMethod]
        public void TestMissingPropertiesAreNull()
        {
            string path = @"..\..\..\demodata\zugferd10\ZUGFeRD_1p0_COMFORT_Einfach.xml";
            var invoiceDescriptor = InvoiceDescriptor.Load(path);

            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.BillingPeriodStart == null));
            Assert.IsTrue(invoiceDescriptor.TradeLineItems.TrueForAll(x => x.BillingPeriodEnd == null));
        } // !TestMissingPropertiesAreNull()
    }
}
