using System;
using System.Collections.Generic;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Class for mapping between file extensions and mime types
    /// 
    /// Only those mime types are present that are supported by the additional reference document according to 
    /// XRechnung specification, see e.g. https://projekte.kosit.org/xrechnung/xrechnung/-/issues/59
    /// </summary>
    internal class MimeTypeMapper
    {
        private static Dictionary<string, string> _MimeTypes = new Dictionary<string, string>()
        {
            { ".pdf", "application/pdf" },
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".csv", "text/csv" },
            { ".xslx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { ".xml", "application/xml" }
        };


        internal static string GetMimeType(string filename)
        {
            if (!String.IsNullOrEmpty(filename))
            {
                string extension = System.IO.Path.GetExtension(filename);                
                if (!String.IsNullOrEmpty(extension) && _MimeTypes.TryGetValue(extension, out string mimeType))
                {
                    return mimeType;
                }                    
            }

            return "application/octet-stream";
        } // !GetMimeType()
    }
}
