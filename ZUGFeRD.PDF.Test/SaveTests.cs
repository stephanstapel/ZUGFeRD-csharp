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
using System.Reflection.Metadata;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Structure;
using PdfSharp.Quality;
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

            string targetPath = @"output-embedded.pdf";
            targetPath = _makeSurePathIsCrossPlatformCompatible(targetPath);

            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            await InvoicePdfProcessor.SaveToPdfAsync(targetPath, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourcePath, descriptor);

            // very basic test
            string result = System.IO.File.ReadAllText(targetPath);
            Assert.IsTrue(result.Contains("/FontFile") || result.Contains("/FontFile2") || result.Contains("/FontFile3"));

            // load fonts using PDFsharp
            // every font element should appear twice
            Dictionary<string, int> fontAppeareanceCount = new Dictionary<string, int>();
            Dictionary<string, bool> fontFileOccurs = new Dictionary<string, bool>();
            PdfDocument document = PdfReader.Open(targetPath);
            Assert.IsTrue(document.Pages.Count > 0);

            var resources = document.Pages[0].Elements.GetDictionary("/Resources");
            Assert.IsNotNull(resources);

            var fonts = resources.Elements.GetDictionary("/Font");
            Assert.IsNotNull(fonts);

            foreach (var value in fonts.Elements.Values)
            {
                var reference = (PdfReference)value;
                var font = document.Internals.GetObject(new PdfObjectID(reference.ObjectNumber)) as PdfDictionary;


                var fontDescriptor = font.Elements.GetDictionary("/FontDescriptor");
                Assert.IsNotNull(fontDescriptor);

                string fontName = fontDescriptor.Elements.GetString("/FontName");
                fontName = fontName.Substring(1);
                if (fontName.Contains("+"))
                {
                    fontName = fontName.Substring(fontName.IndexOf("+") + 1);
                }

                if (!fontAppeareanceCount.ContainsKey(fontName))
                {
                    fontAppeareanceCount.Add(fontName, 1);
                    fontFileOccurs.Add(fontName, false);
                }
                else
                {
                    fontAppeareanceCount[fontName]++;
                }

                if (fontDescriptor.Elements.ContainsKey("/FontFile2"))
                {
                    PdfReference fontFileRef = fontDescriptor.Elements["/FontFile2"] as PdfReference;
                    Assert.IsNotNull(fontFileRef);
                        
                    int fontObjectNumber = fontFileRef.ObjectNumber;
                    PdfObject fontFileObj = document.Internals.GetObject(new PdfObjectID(fontObjectNumber));

                    Assert.IsInstanceOfType(fontFileObj, typeof(PdfDictionary));
                    fontFileOccurs[fontName] = true;                        
                }
            }

            Assert.AreEqual(fontFileOccurs.Count, fontAppeareanceCount.Count);
            Assert.IsTrue(fontFileOccurs.Values.All(x => x));
            Assert.IsTrue(fontAppeareanceCount.Values.All(x => x == 2));
        } // !BasicSaveFileAndEmbedFonts()


        [TestMethod]
        public async Task VeraPdfLikeFontEmbeddingTest()
        {
            // --- Arrange ---
            string sourcePath = @"..\..\..\not-embedded.pdf";
            sourcePath = _makeSurePathIsCrossPlatformCompatible(sourcePath);

            string targetPath = @"output-embedded.pdf";
            targetPath = _makeSurePathIsCrossPlatformCompatible(targetPath);

            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();

            // --- Act ---
            await InvoicePdfProcessor.SaveToPdfAsync(
                targetPath,
                ZUGFeRDVersion.Version23,
                Profile.Comfort,
                ZUGFeRDFormats.CII,
                sourcePath,
                descriptor
            );

            Assert.IsTrue(File.Exists(targetPath), "Output PDF was not created.");

            // --- Assert: open pdf and analyse ---
            PdfDocument document = PdfReader.Open(targetPath);
            Assert.IsTrue(document.Pages.Count > 0);

            HashSet<string> usedFonts = new HashSet<string>();
            Dictionary<string, bool> embeddedStatus = new Dictionary<string, bool>();

            foreach (var page in document.Pages.Cast<PdfPage>())
            {
                var resources = page.Elements.GetDictionary("/Resources");
                if (resources == null) continue;

                var fonts = resources.Elements.GetDictionary("/Font");
                if (fonts == null) continue;

                foreach (var entry in fonts.Elements)
                {
                    var reference = entry.Value as PdfReference;
                    if (reference == null) continue;

                    var fontDict = document.Internals.GetObject(reference.ObjectID) as PdfDictionary;
                    if (fontDict == null) continue;

                    var fontDescriptor = fontDict.Elements.GetDictionary("/FontDescriptor");
                    Assert.IsNotNull(fontDescriptor, $"FontDescriptor missing for {entry.Key}.");

                    string fontName = fontDescriptor.Elements.GetString("/FontName")?.TrimStart('/');
                    usedFonts.Add(fontName);

                    bool embedded =
                        fontDescriptor.Elements.ContainsKey("/FontFile") ||
                        fontDescriptor.Elements.ContainsKey("/FontFile2") ||
                        fontDescriptor.Elements.ContainsKey("/FontFile3");

                    embeddedStatus[fontName] = embedded;
                }
            }

            // not embedded fonts
            var notEmbedded = embeddedStatus.Where(x => !x.Value).Select(x => x.Key).ToList();

            Assert.IsFalse(
                notEmbedded.Any(),
                "These fonts that are not embedded in PDF: " + string.Join(", ", notEmbedded));
        } // !VeraPdfLikeFontEmbeddingTest()


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
            Assert.IsTrue(pdfRawString.Contains($"<zf:DocumentFileName>{designatedFilename}</zf:DocumentFileName>"));
            Assert.IsTrue(pdfRawString.Contains($"<zf:Version>1.0</zf:Version>"));
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
            Assert.IsTrue(pdfRawString.Contains($"<zf:DocumentFileName>{designatedFilename}</zf:DocumentFileName>"));
            Assert.IsTrue(pdfRawString.Contains($"<zf:Version>2p0</zf:Version>"));
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
            Assert.IsTrue(pdfRawString.Contains($"<fx:Version>1.0</fx:Version>"));
        } // !TestSaveFilenameVersion23()


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
            Assert.IsTrue(pdfRawString.Contains($"<fx:Version>3.0</fx:Version>"));
        } // !TestFileWithPassword()
    }
}
