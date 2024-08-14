using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml;

namespace s2industries.ZUGFeRD
{
    internal class XmlUtils
    {
        internal static bool NodeAsBool(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, bool defaultValue = true)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string value = NodeAsString(node, xpath, nsmgr);
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
        } // !NodeAsBool()


        internal static string NodeAsString(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, string defaultValue = "")
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
    }
}
