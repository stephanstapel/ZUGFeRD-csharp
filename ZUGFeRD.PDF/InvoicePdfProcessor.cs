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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using s2industries.ZUGFeRD;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using static PdfSharp.Pdf.PdfDictionary;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Diagnostics;
using System.Linq;

namespace s2industries.ZUGFeRD.PDF
{
    public class InvoicePdfProcessor
    {
        public static async Task<InvoiceDescriptor> LoadFromPdfAsync(Stream pdfStream)
        {
            return await InvoiceDescriptorPdfLoader.LoadAsync(pdfStream);
        } // !LoadFromPdfAsync()


        public static InvoiceDescriptor LoadFromPdf(Stream pdfStream)
        {
            return InvoiceDescriptorPdfLoader.LoadAsync(pdfStream)
                .GetAwaiter()
                .GetResult();
        } // !LoadFromPdf()


        public static async Task<InvoiceDescriptor> LoadFromPdfAsync(string pdfPath)
        {
            return await InvoiceDescriptorPdfLoader.LoadAsync(pdfPath);
        } // !LoadFromPdfAsync()


        public static InvoiceDescriptor LoadFromPdf(string pdfPath)
        {
            return InvoiceDescriptorPdfLoader.LoadAsync(pdfPath)
                .GetAwaiter()
                .GetResult();
        } // !LoadFromPdf()


        public static async Task SaveToPdfAsync(Stream targetPdfStream, ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format, Stream pdfSourceStream, InvoiceDescriptor descriptor, string password = null)
        {
            await InvoiceDescriptorPdfSaver.SaveAsync(targetPdfStream, version, profile, format, pdfSourceStream, descriptor, password);
        } // !SaveToPdfAsync()


        public static void SaveToPdf(Stream targetPdfStream, ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format, Stream pdfSourceStream, InvoiceDescriptor descriptor, string password = null)
        {
            InvoiceDescriptorPdfSaver.SaveAsync(targetPdfStream, version, profile, format, pdfSourceStream, descriptor, password)
                .GetAwaiter()
                .GetResult();
        } // !SaveToPdf()


        /// <summary>
        /// Saves the invoice to a file.
        ///
        /// The invoice PDF is saved to the targetPath. The ZUGFeRD/ Factur-X or XRechnung invoice is embedded into the source PDF file.
        /// 
        /// Optionally, you can pass a password. This password is used for securing the output file.
        /// </summary>
        /// <param name="targetPdfPath"></param>
        /// <param name="version"></param>
        /// <param name="profile"></param>
        /// <param name="format"></param>
        /// <param name="pdfSourcePath"></param>
        /// <param name="descriptor"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task SaveToPdfAsync(string targetPdfPath, ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format, string pdfSourcePath, InvoiceDescriptor descriptor, string password = null)
        {
            await InvoiceDescriptorPdfSaver.SaveAsync(targetPdfPath, version, profile, format, pdfSourcePath, descriptor, password);
        } // !SaveToPdfAsync()


        /// <summary>
        /// Saves the invoice to a file.
        ///
        /// The invoice PDF is saved to the targetPath. The ZUGFeRD/ Factur-X or XRechnung invoice is embedded into the source PDF file.
        /// 
        /// Optionally, you can pass a password. This password is used for securing the output file.
        /// </summary>
        /// <param name="targetPdfPath"></param>
        /// <param name="version"></param>
        /// <param name="profile"></param>
        /// <param name="format"></param>
        /// <param name="pdfSourcePath"></param>
        /// <param name="descriptor"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static void SaveToPdf(string targetPdfPath, ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format, string pdfSourcePath, InvoiceDescriptor descriptor, string password = null)
        {
            InvoiceDescriptorPdfSaver.SaveAsync(targetPdfPath, version, profile, format, pdfSourcePath, descriptor, password)
                .GetAwaiter()
                .GetResult();
        } // !SaveToPdf()
    }
}
