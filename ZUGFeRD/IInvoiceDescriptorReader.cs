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
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace s2industries.ZUGFeRD
{
    internal abstract class IInvoiceDescriptorReader
    {
        public abstract InvoiceDescriptor Load(Stream stream);        
        public abstract bool IsReadableByThisReaderVersion(Stream stream);


        public InvoiceDescriptor Load(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            InvoiceDescriptor retval = Load(fs);
            fs.Close();
            return retval;
        } // !Load()


        public bool IsReadableByThisReaderVersion(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            Stream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            bool retval = IsReadableByThisReaderVersion(fs);
            fs.Close();
            return retval;            
        } // !IsReadableByThisReaderVersion()


        protected XmlNamespaceManager _GenerateNamespaceManagerFromNode(XmlNode node)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(node.OwnerDocument.NameTable);   
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Prefix == "xmlns")
                {
                    nsmgr.AddNamespace(attr.LocalName, attr.Value);
                }
                else if (attr.Name == "xmlns")
                {
                    nsmgr.AddNamespace(string.Empty, attr.Value);
                }
            }

            return nsmgr;
        } // !_GenerateNamespaceManagerFromNode()


        protected static bool _nodeAsBool(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, bool defaultValue = true)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string value = _nodeAsString(node, xpath, nsmgr);
            if (value == "")
            {
                return defaultValue;
            }
            else
            {
                if (value.Trim().Equals("true", StringComparison.OrdinalIgnoreCase) || (value.Trim() == "1"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        } // !_nodeAsBool()


        protected static string _nodeAsString(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, string defaultValue = "")
        {
            if (node == null)
            {
                return defaultValue;
            }

            try
            {
                XmlNode _node = node.SelectSingleNode(xpath, nsmgr);
                if (_node == null)
                {
                    return defaultValue;
                }
                else
                {
                    return _node.InnerText;
                }
            }
            catch (XPathException)
            {
                return defaultValue;
            }
            catch (Exception ex)
            {
                throw ex;
            };
        } // _nodeAsString()


        protected static int _nodeAsInt(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, int defaultValue = 0)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string temp = _nodeAsString(node, xpath, nsmgr, "");
            if (Int32.TryParse(temp, out int retval))
            {
                return retval;
            }
            else
            {
                return defaultValue;
            }
        } // !_nodeAsInt()


        /// <summary>
        ///  reads the value from given xpath and interprets the value as decimal
        /// </summary>
        protected static decimal? _nodeAsDecimal(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, decimal? defaultValue = default(decimal?))
        {
            if (node == null)
            {
                return defaultValue;
            }

            string temp = _nodeAsString(node, xpath, nsmgr, "");
            if (decimal.TryParse(temp, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal retval))
            {
                return retval;
            }
            else
            {
                return defaultValue;
            }
        } // !_nodeAsDecimal()


        /// <summary>
        ///  reads the value from given xpath and interprets the value as date time
        /// </summary>
        protected static DateTime? _nodeAsDateTime(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, DateTime? defaultValue = null)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string format = "";
            XmlNode dateNode = node.SelectSingleNode(xpath, nsmgr);
            if (dateNode == null)
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                else
                {
                    return null;
                }
            }

            if (((XmlElement)dateNode).HasAttribute("format"))
            {
                format = dateNode.Attributes["format"].InnerText;
            }

            string rawValue = dateNode.InnerText;

            if (String.IsNullOrWhiteSpace(rawValue)) // we have to deal with real-life ZUGFeRD files :(
            {
                return null;
            }

            switch (format)
            {
                case "102":
                    {
                        if (rawValue.Length != 8)
                        {
                            throw new Exception("Wrong length of datetime element (format 102)");
                        }

                        string year = rawValue.Substring(0, 4);
                        string month = rawValue.Substring(4, 2);
                        string day = rawValue.Substring(6, 2);

                        return _safeParseDateTime(year, month, day);
                    }
                case "610":
                    {
                        if (rawValue.Length != 6)
                        {
                            throw new Exception("Wrong length of datetime element (format 610)");
                        }

                        string year = rawValue.Substring(0, 4);
                        string month = rawValue.Substring(4, 2);
                        string day = "1";

                        return _safeParseDateTime(year, month, day);
                    }
                case "616":
                    {
                        if (rawValue.Length != 6)
                        {
                            throw new Exception("Wrong length of datetime element (format 616)");
                        }

                        string year = rawValue.Substring(0, 4);
                        string week = rawValue.Substring(4, 2);

                        // code from https://capens.net/content/get-first-day-given-week-iso-8601
                        DateTime jan4 = new DateTime(Int32.Parse(year), 1, 4);
                        DateTime day = jan4.AddDays((Int32.Parse(week) - 1) * 7); // get a day in the requested week                        
                        int dayOfWeek = ((int)day.DayOfWeek + 6) % 7; // get day of week, with [mon = 0 ... sun = 6] instead of [sun = 0 ... sat = 6]

                        return day.AddDays(-dayOfWeek);
                    }
            }

            // if none of the codes above is present, use fallback approach
            if (rawValue.Length == 8)
            {
                string year = rawValue.Substring(0, 4);
                string month = rawValue.Substring(4, 2);
                string day = rawValue.Substring(6, 2);

                return _safeParseDateTime(year, month, day);
            }
            else if ((rawValue.Length == 10) && (rawValue[4] == '-') && (rawValue[7] == '-')) // yyyy-mm-dd
            {
                string year = rawValue.Substring(0, 4);
                string month = rawValue.Substring(5, 2);
                string day = rawValue.Substring(8, 2);

                return _safeParseDateTime(year, month, day);
            }
            else if (rawValue.Length == 19)
            {
                string year = rawValue.Substring(0, 4);
                string month = rawValue.Substring(5, 2);
                string day = rawValue.Substring(8, 2);

                string hour = rawValue.Substring(11, 2);
                string minute = rawValue.Substring(14, 2);
                string second = rawValue.Substring(17, 2);


                return _safeParseDateTime(year, month, day, hour, minute, second);
            }
            else
            {
                throw new UnsupportedException("Invalid length of datetime value");
            }
        } // !_nodeAsDateTime()


        private static DateTime? _safeParseDateTime(string year, string month, string day, string hour = "0", string minute = "0", string second = "0")
        {
            if (!Int32.TryParse(year, out int _year))
            {
                return null;
            }

            if (!Int32.TryParse(month, out int _month))
            {
                return null;
            }

            if (!Int32.TryParse(day, out int _day))
            {
                return null;
            }

            if (!Int32.TryParse(hour, out int _hour))
            {
                return null;
            }

            if (!Int32.TryParse(minute, out int _minute))
            {
                return null;
            }

            if (!Int32.TryParse(second, out int _second))
            {
                return null;
            }

            return new DateTime(_year, _month, _day, _hour, _minute, _second);
        } // !_safeParseDateTime()


        protected bool _IsReadableByThisReaderVersion(Stream stream, IList<string> validURIs)
        {
            long _oldStreamPosition = stream.Position;
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                string data = reader.ReadToEnd().Replace(" ", "").ToLower();
                foreach (string validURI in validURIs)
                {
                    if (data.Contains(String.Format(">{0}<", validURI.ToLower())))
                    {
                        stream.Position = _oldStreamPosition;
                        return true;
                    }
                }
            }

            stream.Position = _oldStreamPosition;
            return false;
        } // !_IsReadableByThisReaderVersion()
    }
}