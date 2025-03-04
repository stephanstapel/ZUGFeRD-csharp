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
using System.Xml;
using System.Xml.XPath;

namespace s2industries.ZUGFeRD
{
    internal class XmlUtils
    {
        internal static bool NodeExists(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null)
        {
            if (node == null)
            {
                return false;
            }
            try
            {
                return node.SelectSingleNode(xpath, nsmgr) != null;
            }
            catch (XPathException)
            {
                return false;
            }
            catch
            {
                throw;
            };
        } // !NodeExists()


        internal static bool NodeAsBool(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, bool defaultValue = true)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string value = NodeAsString(node, xpath, nsmgr);
            if (value == String.Empty)
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
        } // !NodeAsBool()


        internal static string NodeAsString(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, string defaultValue = "")
        {
            if (node == null)
            {
                return defaultValue;
            }

            try
            {
                XmlNode selectedNode = node.SelectSingleNode(xpath, nsmgr);
                if (selectedNode == null)
                {
                    return defaultValue;
                }
                else
                {
                    return selectedNode.InnerText;
                }
            }
            catch (XPathException)
            {
                return defaultValue;
            }
            catch
            {
                throw;
            };
        } // NodeAsString()


        internal static int NodeAsInt(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, int defaultValue = 0)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string temp = NodeAsString(node, xpath, nsmgr, "");
            if (Int32.TryParse(temp, out int retval))
            {
                return retval;
            }
            else
            {
                return defaultValue;
            }
        } // !NodeAsInt()


        /// <summary>
        ///  reads the value from given xpath and interprets the value as decimal
        /// </summary>
        internal static decimal? NodeAsDecimal(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, decimal? defaultValue = default(decimal?))
        {
            if (node == null)
            {
                return defaultValue;
            }

            string temp = NodeAsString(node, xpath, nsmgr, "");
            if (decimal.TryParse(temp, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal retval))
            {
                return retval;
            }
            else
            {
                return defaultValue;
            }
        } // !NodeAsDecimal()


        /// <summary>
        ///  reads the value from given xpath and interprets the value as date time
        /// </summary>
        internal static DateTime? NodeAsDateTime(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, DateTime? defaultValue = null)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string format = String.Empty;
            XmlNode dateNode = node.SelectSingleNode(xpath, nsmgr);
            if (dateNode == null)
            {
                return defaultValue;
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

            // to protect from space and /r /n characters
            rawValue = rawValue.Trim();

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
            if (rawValue.Length == 8) // yyyyMMdd
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
            else if (rawValue.Length == 16 && rawValue[4] == '-' && rawValue[7] == '-' && rawValue[10] == '+') // yyyy-mm-dd+hh:mm
            {
                string year = rawValue.Substring(0, 4);
                string month = rawValue.Substring(5, 2);
                string day = rawValue.Substring(8, 2);

                return _safeParseDateTime(year, month, day);
            }
            else if (rawValue.Length == 19) // yyyy-mm-ddThh:mm:ss
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
        } // !NodeAsDateTime()


        internal static T NodeAsEnum<T>(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, T defaultValue = default(T)) where T : Enum
        {
            int value = NodeAsInt(node, xpath, nsmgr);
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)(object)value;
            }
            else
            {
                return defaultValue;
            }
        } // !NodeAsEnum<T>()


        private static DateTime? _safeParseDateTime(string year, string month, string day, string hour = "0", string minute = "0", string second = "0")
        {
            if (!Int32.TryParse(year, out int yearParsed))
            {
                return null;
            }

            if (!Int32.TryParse(month, out int monthParsed))
            {
                return null;
            }

            if (!Int32.TryParse(day, out int dayParsed))
            {
                return null;
            }

            if (!Int32.TryParse(hour, out int hourParsed))
            {
                return null;
            }

            if (!Int32.TryParse(minute, out int minuteParsed))
            {
                return null;
            }

            if (!Int32.TryParse(second, out int secondParsed))
            {
                return null;
            }

            return new DateTime(yearParsed, monthParsed, dayParsed, hourParsed, minuteParsed, secondParsed);
        } // !_safeParseDateTime()
    }
}
