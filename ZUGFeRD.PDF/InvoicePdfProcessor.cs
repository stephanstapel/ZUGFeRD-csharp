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

        public static async Task<InvoiceDescriptor> SaveToPdfAsync(Stream pdfStream, InvoiceDescriptor descriptor)
        {
            return await InvoiceDescriptorPdfSaver.SaveAsync(pdfStream);
        } // !LoadFromPdfAsync()


        public static async Task<InvoiceDescriptor> SaveToPdfAsync(string pdfPath, InvoiceDescriptor descriptor)
        {
            return await InvoiceDescriptorPdfSaver.SaveAsync(pdfPath);
        } // !LoadFromPdfAsync()
    }
}
