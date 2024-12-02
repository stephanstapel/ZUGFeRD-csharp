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
            PdfDocument _pdfDocument = PdfReader.Open(pdfStream);
            return await _LoadXmlAsync(_pdfDocument);
        } // !LoadAsync()


        internal static async Task<InvoiceDescriptor> LoadAsync(string pdfPath)
        {
            if (!File.Exists(pdfPath))
            {
                throw new FileNotFoundException("File not found", pdfPath);
            }

            using (var pdfFile = File.OpenRead(pdfPath))
            {
                PdfDocument _pdfDocument = PdfReader.Open(pdfFile);
                return await _LoadXmlAsync(_pdfDocument);
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
            List<string> potentialFilenames = new List<string>() { "zugferd.xml", "factur-x.xml" };
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

                        if (!potentialFilenames.Contains(filename.Value))
                        {
                            continue;
                        }

                        var efDict = dict.Elements["/EF"] as PdfDictionary;
                        var fDict = efDict.Elements["/F"] as PdfReference;
                        var embeded = fDict.Value as PdfDictionary;
                        return embeded.Stream;
                    }
                } // !foreach(pdfElement)
            }
            return null;
        } // !_GetEmbeddedXmlStream()
    }
}
