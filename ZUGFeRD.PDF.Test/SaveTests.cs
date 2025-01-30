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
using System.Formats.Asn1;
using s2industries.ZUGFeRD;
using s2industries.ZUGFeRD.PDF;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace s2industries.ZUGFeRD.PDF.Test
{
    [TestClass]
    public sealed class SaveTests : TestBase
    {
        [TestMethod]
        public async Task BasicSaveExampleFile()
        {
            string sourcePath = @"..\..\..\dummy.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);

            string targetPath = @"output.pdf";
            targetPath = _makeSurePathIsCrossPlatformCompatible(targetPath);
            
            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            await InvoicePdfProcessor.SaveToPdfAsync(targetPath, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourcePath, descriptor);

            /* how to test? */
        } // !BasicSaveExampleFile()


        [TestMethod]
        public async Task BasicSaveFileAndEmbedFonts()
        {
            string sourcePath = @"..\..\..\not-embedded.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);

            string targetPath = @"output-not-embedded.pdf";
            targetPath = _makeSurePathIsCrossPlatformCompatible(targetPath);

            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            await InvoicePdfProcessor.SaveToPdfAsync(targetPath, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourcePath, descriptor);

            /* how to test? */
        } // !BasicSaveExampleFile()

        [TestMethod]
        public async Task BasicSaveExampleAsStream()
        {
            string sourcePath = @"..\..\..\dummy.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);                        

            string targetPath = @"output.pdf";
            targetPath = _makeSurePathIsCrossPlatformCompatible(targetPath);
            
            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();

            MemoryStream targetStream = new MemoryStream();
            using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {                
                await InvoicePdfProcessor.SaveToPdfAsync(targetStream, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourceStream, descriptor);
            }

            /* how to test? */
        } // !BasicSaveExampleAsStream()


        [TestMethod]
        public async Task BasicSaveFromNonExistingPdfFile()
        {
            string sourcePath = @"doesnotexist.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);

            string targetPath = @"output.pdf";            
            targetPath = _makeSurePathIsCrossPlatformCompatible(targetPath);

            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            await Assert.ThrowsExceptionAsync<FileNotFoundException>(() => InvoicePdfProcessor.SaveToPdfAsync(targetPath, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourcePath, descriptor));
        } // !BasicSaveFromNonExistingPdfFile()


        [TestMethod]
        public async Task TestFileWithPassword()
        {
            string sourcePath = @"..\..\..\PDFWithPassword.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);

            string targetPath = @"output.pdf";
            targetPath = _makeSurePathIsCrossPlatformCompatible(targetPath);

            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            await InvoicePdfProcessor.SaveToPdfAsync(targetPath, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourcePath, descriptor, password: "SecretPassw.rd");
        } // !TestFileWithPassword()


        [TestMethod]
        public async Task TestSaveFilenameVersion1()
        {
            string sourcePath = @"..\..\..\dummy.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);
            
            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            MemoryStream targetStream = new MemoryStream();
            using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                await InvoicePdfProcessor.SaveToPdfAsync(targetStream, ZUGFeRDVersion.Version1, Profile.Comfort, ZUGFeRDFormats.CII, sourceStream, descriptor);
            }            

            string pdfRawString = System.Text.Encoding.UTF8.GetString(targetStream.ToArray());

            string designatedFilename = "ZUGFeRD-invoice.xml";
            Assert.IsTrue(pdfRawString.Contains($"/F({designatedFilename})"));
            Assert.IsTrue(pdfRawString.Contains($"/UF({designatedFilename})"));
            Assert.IsTrue(pdfRawString.Contains($"<fx:DocumentFileName>{designatedFilename}</fx:DocumentFileName>"));
        } // !TestFileWithPassword()


        [TestMethod]
        public async Task TestSaveFilenameVersion20()
        {
            string sourcePath = @"..\..\..\dummy.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);
            
            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            MemoryStream targetStream = new MemoryStream();
            using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                await InvoicePdfProcessor.SaveToPdfAsync(targetStream, ZUGFeRDVersion.Version20, Profile.Comfort, ZUGFeRDFormats.CII, sourceStream, descriptor);
            }            

            string pdfRawString = System.Text.Encoding.UTF8.GetString(targetStream.ToArray());

            string designatedFilename = "zugferd-invoice.xml";
            Assert.IsTrue(pdfRawString.Contains($"/F({designatedFilename})"));
            Assert.IsTrue(pdfRawString.Contains($"/UF({designatedFilename})"));
            Assert.IsTrue(pdfRawString.Contains($"<fx:DocumentFileName>{designatedFilename}</fx:DocumentFileName>"));
        } // !TestFileWithPassword()


        [TestMethod]
        public async Task TestSaveFilenameVersion23()
        {
            string sourcePath = @"..\..\..\dummy.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);
            
            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            MemoryStream targetStream = new MemoryStream();
            using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                await InvoicePdfProcessor.SaveToPdfAsync(targetStream, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourceStream, descriptor);
            }            

            string pdfRawString = System.Text.Encoding.UTF8.GetString(targetStream.ToArray());

            string designatedFilename = "factur-x.xml";
            Assert.IsTrue(pdfRawString.Contains($"/F({designatedFilename})"));
            Assert.IsTrue(pdfRawString.Contains($"/UF({designatedFilename})"));
            Assert.IsTrue(pdfRawString.Contains($"<fx:DocumentFileName>{designatedFilename}</fx:DocumentFileName>"));
        } // !TestFileWithPassword()


        [TestMethod]
        public async Task TestSaveFilenameVersion23XRechnung()
        {
            string sourcePath = @"..\..\..\dummy.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);
            
            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            MemoryStream targetStream = new MemoryStream();
            using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            {
                await InvoicePdfProcessor.SaveToPdfAsync(targetStream, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.CII, sourceStream, descriptor);
            }            

            string pdfRawString = System.Text.Encoding.UTF8.GetString(targetStream.ToArray());

            string designatedFilename = "xrechnung.xml";
            Assert.IsTrue(pdfRawString.Contains($"/F({designatedFilename})"));
            Assert.IsTrue(pdfRawString.Contains($"/UF({designatedFilename})"));
            Assert.IsTrue(pdfRawString.Contains($"<fx:DocumentFileName>{designatedFilename}</fx:DocumentFileName>"));
        } // !TestFileWithPassword()
    }
}
