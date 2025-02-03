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
using PdfSharp.Diagnostics;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using System.Linq;
using SkiaSharp;



namespace s2industries.ZUGFeRD.PDF
{
    internal class InvoiceDescriptorPdfSaver
    {
        internal static async Task SaveAsync(Stream targetStream, ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format, Stream pdfSourceStream, InvoiceDescriptor descriptor, string password = null)
        {
            if (pdfSourceStream == null)
            {
                throw new ArgumentNullException("Invalid pdfSourceStream");
            }

            if (descriptor == null)
            {
                throw new ArgumentNullException("Invalid invoiceDescriptor");
            }

            string invoiceFilename = _DetermineFilenameBasedOnVersionAndProfile(version, profile);

            MemoryStream xmlSourceStream = new MemoryStream();
            descriptor.Save(xmlSourceStream, version, profile, format);
            xmlSourceStream.Seek(0, SeekOrigin.Begin);

            Stream temp = _CreateFacturXStream(pdfSourceStream, xmlSourceStream, version, profile, invoiceFilename, password: password);
            await temp.CopyToAsync(targetStream);
        } // !SaveAsync()        


        internal static async Task SaveAsync(string targetPath, ZUGFeRDVersion version, Profile profile, ZUGFeRDFormats format, string pdfSourcePath, InvoiceDescriptor descriptor, string password = null)
        {
            if (!File.Exists(pdfSourcePath))
            {
                throw new FileNotFoundException("File not found", pdfSourcePath);
            }

            if (descriptor == null)
            {
                throw new ArgumentNullException("Invalid invoiceDescriptor");
            }

            using (FileStream pdfSourceStream = File.OpenRead(pdfSourcePath))
            using (MemoryStream targetStream = new MemoryStream())
            {
                await SaveAsync(targetStream, version, profile, format, pdfSourceStream, descriptor, password);
                targetStream.Seek(0, SeekOrigin.Begin);
                System.IO.File.WriteAllBytes(targetPath, targetStream.ToArray());
            }            
        } // !SaveAsync()


        private static Stream _CreateFacturXStream(Stream pdfStream, Stream xmlStream, ZUGFeRDVersion version, Profile profile, string invoiceFilename, string documentTitle = null, string documentDescription = null, string password = null)
        {
            if (pdfStream == null)
            {
                throw new ArgumentNullException(nameof(pdfStream));
            }

            if (xmlStream == null)
            {
                throw new ArgumentNullException(nameof(xmlStream));
            }

            PdfDocument inputDocument = null;
            try
            {
                if (!String.IsNullOrWhiteSpace(password))
                {
                    inputDocument = PdfReader.Open(pdfStream, password, PdfDocumentOpenMode.Import);
                }
                else
                {
                    inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);
                }
            }
            catch (Exception ex)
            {
                throw new SaveFailedException();
            }

            string safeDocumentTitle = "Invoice";
            if (!String.IsNullOrWhiteSpace(documentTitle))
            {
                safeDocumentTitle = documentTitle;
            }

            string safeDocumentDescription = "Invoice";
            if (!String.IsNullOrWhiteSpace(documentDescription))
            {
                safeDocumentDescription = documentDescription;
            }


            PdfDocument outputDocument = new PdfDocument();
            outputDocument.Options.ManualXmpGeneration = true;

            if (!String.IsNullOrWhiteSpace(password))
            {
                outputDocument.SecuritySettings.UserPassword = password;
            }

            for (int i = 0; i < inputDocument.PageCount; i++)
            {
                outputDocument.AddPage(inputDocument.Pages[i]);
            }

            string xmlChecksum = string.Empty;
            byte[] xmlFileBytes = null;
            using (var md5 = MD5.Create())
            {                
                xmlStream.Seek(0, SeekOrigin.Begin);
                xmlFileBytes = new byte[xmlStream.Length];
                xmlStream.Read(xmlFileBytes, 0, (int)xmlStream.Length);

                var hashBytes = md5.ComputeHash(xmlStream);
                xmlChecksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }

            var xmlFileEncodedBytes = PdfSharp.Pdf.Filters.Filtering.FlateDecode.Encode(xmlFileBytes);

            PdfDictionary xmlParamsDict = new PdfDictionary();
            xmlParamsDict.Elements.Add("/CheckSum", new PdfString(xmlChecksum));
            xmlParamsDict.Elements.Add("/ModDate", new PdfString(_FormatPdfDateTime(DateTime.UtcNow)));
            xmlParamsDict.Elements.Add("/Size", new PdfInteger(xmlFileBytes.Length));



            PdfDictionary fStreamDict = new PdfDictionary();
            fStreamDict.CreateStream(xmlFileEncodedBytes);
            fStreamDict.Elements.Add("/Filter", new PdfName("/FlateDecode"));
            fStreamDict.Elements.Add("/Type", new PdfName("/EmbeddedFile"));
            fStreamDict.Elements.Add("/Params", xmlParamsDict);
            fStreamDict.Elements.Add("/Subtype", new PdfName("/text/xml"));
            outputDocument.Internals.AddObject(fStreamDict);


            string relationship = "";
            switch (profile)
            {
                case Profile.Minimum:
                case Profile.BasicWL:
                    relationship = "Data";
                    break;
                default:
                    relationship = "Alternative";
                    break;
            };

            PdfDictionary af0Dict = new PdfDictionary();
            af0Dict.Elements.Add("/AFRelationship", new PdfName($"/{relationship}"));
            af0Dict.Elements.Add("/Desc", new PdfString("Factur-X XML file"));
            af0Dict.Elements.Add("/Type", new PdfName("/Filespec"));
            af0Dict.Elements.Add("/F", new PdfString(invoiceFilename));
            PdfDictionary af1Dict = new PdfDictionary();
            af1Dict.Elements.Add("/F", fStreamDict.Reference);
            af1Dict.Elements.Add("/UF", fStreamDict.Reference);

            af0Dict.Elements.Add("/EF", af1Dict);
            af0Dict.Elements.Add("/UF", new PdfString(invoiceFilename));
            outputDocument.Internals.AddObject(af0Dict);

            var afPdfArray = new PdfArray();
            afPdfArray.Elements.Add(af0Dict.Reference);
            outputDocument.Internals.AddObject(afPdfArray);
            outputDocument.Internals.Catalog.Elements.Add("/AF", afPdfArray.Reference);


            var dateTimeNow = DateTime.UtcNow;
            var conformanceLevelName = profile.GetXMPName();
            var xmpVersion = "";
            
            switch (version)
            {
                case ZUGFeRDVersion.Version1: xmpVersion = "1.0"; break;
                case ZUGFeRDVersion.Version20: xmpVersion = "2.0"; break;
                case ZUGFeRDVersion.Version23: xmpVersion = "2.3"; break;
            }

            string pdfMetadataTemplate = System.Text.Encoding.Default.GetString(_LoadEmbeddedResource("s2industries.ZUGFeRD.PDF.Resources.PdfMedatadataTemplate.xml"));
            var xmpmeta = pdfMetadataTemplate
                .Replace("{{InvoiceFilename}}", invoiceFilename)
                .Replace("{{CreationDate}}", _FormatXMPDateTime(dateTimeNow))
                .Replace("{{ModificationDate}}", _FormatXMPDateTime(dateTimeNow))
                .Replace("{{DocumentTitle}}", safeDocumentTitle)
                .Replace("{{Version}}", xmpVersion)
                .Replace("{{DocumentDescription}}", safeDocumentDescription)
                .Replace("{{ConformanceLevel}}", conformanceLevelName);

            var metadataBytes = System.Text.Encoding.UTF8.GetBytes(xmpmeta);
            if (metadataBytes[metadataBytes.Length - 1] == 0x0A) // remove trailing EOL
            {
                var trimmedMetadataBytes = new byte[metadataBytes.Length - 1];
                Array.Copy(metadataBytes, trimmedMetadataBytes, trimmedMetadataBytes.Length); // remove eol marker
                metadataBytes = trimmedMetadataBytes;
            }
            

            //            var metadataEncodedBytes = PdfSharp.Pdf.Filters.Filtering.FlateDecode.Encode(metadataBytes);

            PdfDictionary metadataDictionary = new PdfDictionary();

            metadataDictionary.CreateStream(metadataBytes);
        //    metadataDictionary.Elements.Add("/Filter", new PdfName("/FlateDecode"));
            metadataDictionary.Elements.Add("/Subtype", new PdfName("/XML"));
            metadataDictionary.Elements.Add("/Type", new PdfName("/Metadata"));

            outputDocument.Internals.AddObject(metadataDictionary);
            outputDocument.Internals.Catalog.Elements.Add("/Metadata", metadataDictionary.Reference);

            var namesPdfArray = new PdfArray();
            namesPdfArray.Elements.Add(new PdfString(invoiceFilename));
            namesPdfArray.Elements.Add(af0Dict.Reference);
            PdfDictionary embeddedFilesDict = new PdfDictionary();
            embeddedFilesDict.Elements.Add("/Names", namesPdfArray);
            PdfDictionary namesDict = new PdfDictionary();
            namesDict.Elements.Add("/EmbeddedFiles", embeddedFilesDict);
            outputDocument.Internals.Catalog.Elements.Add("/Names", namesDict);

            // MarkInfo
            PdfDictionary markInfoDict = new PdfDictionary();
            markInfoDict.Elements.Add("/Marked", new PdfBoolean(true));
            outputDocument.Internals.Catalog.Elements.Add("/MarkInfo", markInfoDict);

            // StructureRoot
            PdfDictionary structTreeRoot = new PdfDictionary();
            structTreeRoot.Elements["/Type"] = new PdfName("/StructTreeRoot");            
            PdfDictionary structElement = new PdfDictionary();
            structElement.Elements["/Type"] = new PdfName("/StructElem");
            structElement.Elements["/S"] = new PdfName("/Document");
            structElement.Elements["/P"] = structTreeRoot;
            outputDocument.Internals.Catalog.Elements.Add("/StructTreeRoot", structTreeRoot);

            // Profile
            PdfDictionary rgbProfileDict = new PdfDictionary();
            rgbProfileDict.CreateStream(_LoadEmbeddedResource("s2industries.ZUGFeRD.PDF.Resources.sRGB-IEC61966-2.1.icc"));
            rgbProfileDict.Elements.Add("/N", new PdfInteger(3));
            outputDocument.Internals.AddObject(rgbProfileDict);

            PdfDictionary outputIntent0Dict = new PdfDictionary();
            outputIntent0Dict.Elements.Add("/DestOutputProfile", rgbProfileDict.Reference);
            outputIntent0Dict.Elements.Add("/OutputConditionIdentifier", new PdfString("sRGB IEC61966-2.1"));
            outputIntent0Dict.Elements.Add("/S", new PdfName("/GTS_PDFA1"));
            outputIntent0Dict.Elements.Add("/Type", new PdfName("/OutputIntent"));
            outputDocument.Internals.AddObject(outputIntent0Dict);

            var outputIntentsArray = new PdfArray();
            outputIntentsArray.Elements.Add(outputIntent0Dict.Reference);
            outputDocument.Internals.Catalog.Elements.Add("/OutputIntents", outputIntentsArray);
            outputDocument.Info.Creator = "S2 Industries";

            _EmbedFonts(outputDocument);

            MemoryStream memoryStream = new MemoryStream();
            try
            {
                outputDocument.Save(memoryStream);
            }
            catch (Exception)
            {
                throw new SaveFailedException();
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        } // !_CreateFacturXStream()

        private static void _EmbedFonts(PdfDocument pdfDocument)
        {
            XGraphics gfx = XGraphics.FromPdfPage(pdfDocument.Pages[0]);
            foreach (XFont usedFont in _LoadFonts(pdfDocument))
            {
                // Use the embedded font
                XFont xFont = new XFont(usedFont.Name, 1, usedFont.Style, new XPdfFontOptions(PdfFontEmbedding.EmbedCompleteFontFile));
                gfx.DrawString(" ", xFont, XBrushes.Transparent, new XPoint(1, 1));
            }
            gfx.Dispose();
        }

        private static List<XFont> _LoadFonts(PdfDocument pdfDocument)
        {
            List<XFont> retval = new List<XFont>();

            var fontManager = SKFontManager.Default;
            var availableFamilies = fontManager.FontFamilies;

            // Postscript to TrueType mapping as the PDF file contains the Postscript names of the font
            var postScriptToTrueTypeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Arial-BoldMT", "Arial Bold" },
                { "ArialMT", "Arial" },
                { "Times-Roman", "Times New Roman" },
                { "Courier-Bold", "Courier New" }
                // add more mappings here if needed
            };

            for (int pageIndex = 0; pageIndex < pdfDocument.PageCount; pageIndex++)
            {
                PdfPage page = pdfDocument.Pages[pageIndex];

                // Get the page's resources
                PdfDictionary resources = page.Resources;
                if (resources == null || !resources.Elements.ContainsKey("/Font"))
                {
                    continue;
                }

                PdfDictionary fonts = resources.Elements.GetDictionary("/Font");

                foreach (var fontNameInPdfFile in fonts.Elements.Keys)
                {
                    PdfDictionary font = fonts.Elements.GetDictionary(fontNameInPdfFile);
                    if (font == null)
                    {
                        continue;
                    }

                    string baseFont = font.Elements.GetString("/BaseFont").Split('+').Last();

                    _ExtractFontValues(baseFont, fonts, out string fontFamily, out XFontStyleEx fontStyle);

                    bool existsInSystem = availableFamilies.Any(f => f.Equals(fontFamily, StringComparison.OrdinalIgnoreCase));


                    if (!existsInSystem && (postScriptToTrueTypeMap.TryGetValue(fontFamily, out string tempFontName)))
                    {
                        fontFamily = tempFontName;
                    }

                    if (String.IsNullOrWhiteSpace(fontFamily))
                    {
                        continue;
                    }

                    try
                    {
                        var embeddedFont = new XFont(fontFamily, 10, fontStyle);
                        retval.Add(embeddedFont);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return retval;
        } // !_LoadFonts()


        private static void _ExtractFontValues(string baseFont, PdfDictionary fontDict, out string fontFamily, out XFontStyleEx fontStyle)
        {
            fontStyle = XFontStyleEx.Regular;

            // Split by comma or hyphen to detect styles
            string[] fontParts = baseFont.Split(new[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            fontFamily = fontParts[0].TrimStart('/'); // Base font name (e.g., "Arial")

            if (fontParts.Length > 1)
            {
                string stylePart = fontParts[1].ToLower();
                if (stylePart.IndexOf("bold", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    fontStyle |= XFontStyleEx.Bold;
                }

                if ((stylePart.IndexOf("italic", StringComparison.OrdinalIgnoreCase) > -1) ||
                    (stylePart.IndexOf("oblique", StringComparison.OrdinalIgnoreCase) > -1))
                {
                    fontStyle |= XFontStyleEx.Italic;
                }
            }

            // Check FontDescriptor flags (if available)
            if (fontDict.Elements.ContainsKey("/FontDescriptor"))
            {
                var fontDescriptor = fontDict.Elements.GetDictionary("/FontDescriptor");
                if (fontDescriptor.Elements.ContainsKey("/Flags"))
                {
                    int flags = fontDescriptor.Elements.GetInteger("/Flags");
                    if ((flags & 0x40000) != 0) fontStyle |= XFontStyleEx.Bold;   // Bold flag
                    if ((flags & 0x10) != 0) fontStyle |= XFontStyleEx.Italic; // Italic flag
                }
            }
        } // !_ExtractFontValues()


        private static string _DetermineFilenameBasedOnVersionAndProfile(ZUGFeRDVersion version, Profile profile)
        {
            if (version == ZUGFeRDVersion.Version1)
            {
                return "ZUGFeRD-invoice.xml";
            }
            else if (version == ZUGFeRDVersion.Version20)
            {
                return "zugferd-invoice.xml";
            }
            else
            {
                if ((profile == Profile.XRechnung1) || (profile == Profile.XRechnung))
                {
                    return "xrechnung.xml";
                }
            }

            return "factur-x.xml";
        } // !_DetermineFilenameBasedOnVersionAndProfile()


        private static string _FormatPdfDateTime(DateTime dateTime)
        {
            // Get the offset for the current time zone
            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(dateTime);

            // Format the offset as "+HH'mm'" or "-HH'mm'"
            string offsetString = $"{(offset >= TimeSpan.Zero ? "+" : "-")}{offset.Hours:00}'{offset.Minutes:00}'";

            // Format the datetime according to the PDF specification
            return $"D:{dateTime:yyyyMMddHHmmss}{offsetString}";
        } // !_FormatPdfDateTime()


        private static string _FormatXMPDateTime(DateTime dateTime)
        {
            DateTime utcTime = dateTime.ToUniversalTime();
            return $"{utcTime:yyyy-MM-ddTHH:mm}Z";
        } // !_FormatXMPDateTime()


        private static byte[] _LoadEmbeddedResource(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    return null;
                }

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        } // !_LoadEmbeddedResource()
    }
}
