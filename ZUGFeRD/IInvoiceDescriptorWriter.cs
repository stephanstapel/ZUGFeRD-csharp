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
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace s2industries.ZUGFeRD
{
    internal abstract class IInvoiceDescriptorWriter
    {
        public abstract void Save(InvoiceDescriptor descriptor, Stream stream, ZUGFeRDFormats format = ZUGFeRDFormats.CII, InvoiceFormatOptions options = null);


        public void Save(InvoiceDescriptor descriptor, string filename, ZUGFeRDFormats format = ZUGFeRDFormats.CII, InvoiceFormatOptions options = null)
        {
            if (Validate(descriptor, true))
            {
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                Save(descriptor, fs, format, options);
                fs.Flush();
                fs.Close();
            }
        } // !Save()


        internal abstract bool Validate(InvoiceDescriptor descriptor, bool throwExceptions = true);


        protected void _WriteHeaderComments(ProfileAwareXmlTextWriter writer, InvoiceFormatOptions options)
        {
            if (options == null || writer == null)
            {
                return;
            }

            if (!options.IncludeXmlComments || options.XmlHeaderComments?.Count == 0)
            {
                return;
            }

            foreach (string comment in options.XmlHeaderComments)
            {
                if (!String.IsNullOrEmpty(comment))
                {
                    writer.WriteComment(comment);
                }
            }
        } // !_WriteHeaderComments()


        protected void _WriteComment(ProfileAwareXmlTextWriter writer, InvoiceFormatOptions options, string comment)
        {
            if ((writer == null) || (options == null) || String.IsNullOrEmpty(comment))
            {
                return;
            }

            if (!options.IncludeXmlComments)
            {
                return;
            }

            writer.WriteComment(comment);
        } // !_WriteComment()


        protected string _formatDecimal(decimal? value, int numDecimals = 2)
        {
            if (value == null)
            {
                return String.Empty;
            }

            return Math.Round(value.Value, numDecimals, MidpointRounding.AwayFromZero).ToString($"F{numDecimals}", CultureInfo.InvariantCulture);
        } // !_formatDecimal()


        protected string _formatDate(DateTime date, bool formatAs102 = true, bool toUBLDate = false)
        {
            if (formatAs102)
            {
                return date.ToString("yyyyMMdd");
            }
            else
            {
                if (toUBLDate)
                    return date.ToString("yyyy-MM-dd");

                return date.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        } // !_formatDate()
    }
}
