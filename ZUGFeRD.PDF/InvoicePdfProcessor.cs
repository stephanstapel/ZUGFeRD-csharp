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


        public static async Task<InvoiceDescriptor> LoadFromPdfAsync(string pdfPath)
        {
            return await InvoiceDescriptorPdfLoader.LoadAsync(pdfPath);
        } // !LoadFromPdfAsync()


        public static async Task<InvoiceDescriptor> SaveToPdfAsync(Stream targetPdfStream, Stream pdfSourceStream, InvoiceDescriptor descriptor)
        {
            return await InvoiceDescriptorPdfSaver.SaveAsync(targetPdfStream, pdfSourceStream, descriptor);
        } // !SaveToPdfAsync()


        public static async Task<InvoiceDescriptor> SaveToPdfAsync(string targetPdfPath, string pdfSourcePath, InvoiceDescriptor descriptor)
        {
            return await InvoiceDescriptorPdfSaver.SaveAsync(targetPdfPath, pdfSourcePath, descriptor);
        } // !SaveToPdfAsync()
    }
}
