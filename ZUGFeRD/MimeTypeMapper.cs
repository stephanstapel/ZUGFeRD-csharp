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
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { ".xml", "application/xml" }
        };


        internal static string GetMimeType(string filename)
        {
            if (!String.IsNullOrWhiteSpace(filename))
            {
                string extension = System.IO.Path.GetExtension(filename);                
                if (!String.IsNullOrWhiteSpace(extension) && _MimeTypes.TryGetValue(extension.ToLower(), out string mimeType))
                {
                    return mimeType;
                }                    
            }

            return "application/octet-stream";
        } // !GetMimeType()
    }
}
