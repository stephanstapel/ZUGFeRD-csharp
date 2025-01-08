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
using System.Formats.Asn1;
using s2industries.ZUGFeRD;
using s2industries.ZUGFeRD.PDF;

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
            targetPath = "e:\\output2.pdf";
            targetPath = _makeSurePathIsCrossPlatformCompatible(targetPath);
            
            InvoiceDescriptor descriptor = new InvoiceProvider().CreateInvoice();
            await InvoicePdfProcessor.SaveToPdfAsync(targetPath, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourcePath, descriptor);

            /* how to test? */
        } // !BasicSaveExampleFile()


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
            await InvoicePdfProcessor.SaveToPdfAsync(targetPath, ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, sourcePath, descriptor, password: "SecretPassw.rdxx");
        } // !TestFileWithPassword()
    }
}
