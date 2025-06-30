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
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static PdfSharp.Pdf.PdfDictionary;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD.PDF
{
    internal class InvoiceDescriptorPdfLoader
    {
        internal static async Task<InvoiceDescriptor> LoadAsync(Stream pdfStream)
        {
            PdfDocument pdfDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
            return await _LoadXmlAsync(pdfDocument);
        } // !LoadAsync()


        internal static async Task<InvoiceDescriptor> LoadAsync(string pdfPath)
        {
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException("File not found", pdfPath);
            }

            using (var pdfFile = File.OpenRead(pdfPath))
            {
                PdfDocument pdfDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);
                return await _LoadXmlAsync(pdfDocument);
            }
        } // !LoadAsync()


        private static async Task<InvoiceDescriptor> _LoadXmlAsync(PdfDocument document)
        {
            PdfStream stream = _GetEmbeddedXmlStream(document);
            if (stream == null)
            {
                return null;
            }

            byte[] bytes;
            if (stream.TryUnfilter())
            {
                bytes = stream.Value;
            }
            else
            {
                PdfSharp.Pdf.Filters.FlateDecode flate = new PdfSharp.Pdf.Filters.FlateDecode();
                bytes = flate.Decode(stream.Value, new PdfSharp.Pdf.Filters.FilterParms(null));
            }

            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            string text = uTF8Encoding.GetString(bytes);
            if ((bytes.Length > 3) & (bytes[0] == 239) & (bytes[1] == 187) & (bytes[2] == 191))
            {
                text = text.Substring(1);
            }

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            await sw.WriteAsync(text);
            await sw.FlushAsync();
            ms.Position = 0;

            return InvoiceDescriptor.Load(ms);
        } // !_LoadXmlAsync()


        private static PdfStream _GetEmbeddedXmlStream(PdfDocument document, string xmlFileName = null)
        {
            List<string> potentialFilenames = new List<string>() { "zugferd.xml", "zugferd-invoice.xml", "factur-x.xml", "xrechnung.xml" };
            if (!String.IsNullOrWhiteSpace(xmlFileName))
            {
                potentialFilenames.Add(xmlFileName);
                potentialFilenames = potentialFilenames.Distinct().ToList();
            }

            if (!document.Internals.Catalog.Elements.ContainsKey("/AF"))
            {
                throw new Exception($"Could not find PDF Element AF containing {xmlFileName}");
            }

            var element = document.Internals.Catalog.Elements["/AF"];

            if (element is PdfReference)
            {
                PdfReference objectReferece = element as PdfReference;
                element = objectReferece.Value;
            }


            PdfArray xObject = element as PdfArray;
            if (xObject != null)
            {
                foreach (var pdfElement in xObject.Elements)
                {
                    if (!(pdfElement is PdfReference))
                    {
                        continue;
                    }

                    PdfReference reference = (PdfReference)pdfElement;
                    if (!(reference.Value is PdfDictionary))
                    {
                        continue;
                    }

                    var dict = (PdfDictionary)reference.Value;
                    if (dict.Elements.ContainsKey("/EF") && dict.Elements.ContainsKey("/F"))
                    {
                        var filename = dict.Elements["/F"] as PdfString;

                        if (string.IsNullOrWhiteSpace(filename?.Value) || !potentialFilenames.Any(pfn => pfn.Equals(filename.Value, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }


                        PdfItem efElement = dict.Elements["/EF"];

                        // the element might not be present directly, but be a reference only.
                        // Thus we have to iterate the references
                        while (efElement is PdfReference)
                        {                                                        
                            var efReference = efElement as PdfReference;
                            efElement = document.Internals.GetObject(efReference.ObjectID);
                        }

                        var efDict = efElement as PdfDictionary;
                        var fDict = efDict.Elements["/F"] as PdfReference;

                        if (fDict != null)
                        {
                            var embeded = fDict.Value as PdfDictionary;
                            return embeded.Stream;
                        }
                    }
                } // !foreach(pdfElement)
            }
            return null;
        } // !_GetEmbeddedXmlStream()
    }
}
