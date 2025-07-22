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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Xml;

namespace s2industries.ZUGFeRD
{
    internal class StackInfo
    {
        public Profile Profile;
        public bool IsVisible;
    }


    internal class ProfileAwareXmlTextWriter
    {
        private XmlWriter TextWriter;
        private Stack<StackInfo> XmlStack = new Stack<StackInfo>();
        private Profile CurrentProfile = Profile.Unknown;
        private Dictionary<string, string> Namespaces = new Dictionary<string, string>();
        private bool _AutomaticallyCleanInvalidXmlCharacters = false;

        /// <summary>
        /// In case we are writing raw values, the automatic indention of end elements might get broken.
        /// In order to heal that, we need to manually add indention.
        /// </summary>
        private bool _NeedToIndentEndElement = false;


        public ProfileAwareXmlTextWriter(string filename, System.Text.Encoding encoding, Profile profile, bool automaticallyCleanInvalicXmlCharacters = false)
        {
            this.TextWriter = XmlWriter.Create(filename, new XmlWriterSettings()
            {
                Encoding = encoding,
                Indent = true
            });

            this.CurrentProfile = profile;
            this._AutomaticallyCleanInvalidXmlCharacters = automaticallyCleanInvalicXmlCharacters;
        } // !ProfileAwareXmlTextWriter()


        public ProfileAwareXmlTextWriter(System.IO.Stream w, Profile profile, bool automaticallyCleanInvalicXmlCharacters = false)
        {
            this.TextWriter = XmlWriter.Create(w, new XmlWriterSettings()
            {
                Encoding = new UTF8Encoding(false, true),
                Indent = true
            });
            this.CurrentProfile = profile;
            this._AutomaticallyCleanInvalidXmlCharacters = automaticallyCleanInvalicXmlCharacters;
        } // !ProfileAwareXmlTextWriter()


        public void Close()
        {
            this.TextWriter?.Close();
        } // !Close()


        public void Flush()
        {
            this.TextWriter?.Flush();
        } // !Flush()


        public void WriteStartElement(string prefix, string localName, Profile profile = Profile.Unknown)
        {
            Profile safeProfile = profile;
            if (profile == Profile.Unknown)
            {
                safeProfile = this.CurrentProfile;
            }

            if (!_IsNodeVisible() || !_DoesProfileFitToCurrentProfile(safeProfile))
            {
                this.XmlStack.Push(new StackInfo() { Profile = safeProfile, IsVisible = false });
                return;
            }
            else
            {
                this.XmlStack.Push(new StackInfo() { Profile = safeProfile, IsVisible = true });
            }

            if (this.TextWriter == null)
            {
                return;
            }

            string ns = Namespaces.ContainsKey(prefix) ? Namespaces[prefix] : null;

            // write value
            if (!String.IsNullOrWhiteSpace(prefix))
            {
                this.TextWriter.WriteStartElement(prefix, localName, ns);
            }
            else if (!String.IsNullOrWhiteSpace(ns))
            {
                this.TextWriter.WriteStartElement(localName, ns);
            }
            else
            {
                this.TextWriter.WriteStartElement(localName);
            }
        } // !WriteStartElement()


        public void WriteEndElement()
        {
            StackInfo infoForCurrentXmlLevel = this.XmlStack.Pop();
            if (_DoesProfileFitToCurrentProfile(infoForCurrentXmlLevel.Profile) && _IsNodeVisible())
            {
                if (_NeedToIndentEndElement)
                {
                    WriteRawIndention();
                    _NeedToIndentEndElement = false;
                }

                this.TextWriter?.WriteEndElement();
            }
        } // !WriteEndElement()


        public void WriteOptionalElementString(string prefix, string tagName, string value, Profile profile = Profile.Unknown)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (!_IsValidXmlString(value))
            {
                if (_AutomaticallyCleanInvalidXmlCharacters == true)
                {
                    value = _CleanInvalidXmlChars(value);                    
                }
                else
                {
                    throw new IllegalCharacterException($"'{value}' contains illegal characters for xml.");
                }
            }

            WriteElementString(prefix, tagName, value, profile);
        } // !WriteOptionalElementString()


        public void WriteElementString(string prefix, string localName, string ns, string value, Profile profile = Profile.Unknown)
        {
            if (!_IsValidXmlString(value))
            {
                if (_AutomaticallyCleanInvalidXmlCharacters == true)
                {
                    value = _CleanInvalidXmlChars(value);
                }
                else
                {
                    throw new IllegalCharacterException($"'{value}' contains illegal characters for xml.");
                }
            }

            Profile safeProfile = profile;
            if (profile == Profile.Unknown)
            {
                safeProfile = this.CurrentProfile;
            }

            if (this.TextWriter == null || !_IsNodeVisible() || !_DoesProfileFitToCurrentProfile(safeProfile))
            {
                return;
            }

            // write value
            if (!String.IsNullOrWhiteSpace(prefix))
            {
                this.TextWriter.WriteElementString(prefix, localName, ns, value);
            }
            else if (!String.IsNullOrWhiteSpace(ns))
            {
                this.TextWriter.WriteElementString(localName, ns, value);
            }
            else
            {
                this.TextWriter.WriteElementString(localName, value);
            }
        } // !WriteElementString()


        public void WriteStartDocument()
        {
            this.TextWriter?.WriteStartDocument();
        }


        public void WriteStartDocument(bool standalone)
        {
            this.TextWriter?.WriteStartDocument(standalone);
        }

        public void WriteEndDocument()
        {
            this.TextWriter?.WriteEndDocument();
        }


        public void WriteAttributeString(string prefix, string localName, string value, Profile profile = Profile.Unknown)
        {
            StackInfo infoForCurrentNode = this.XmlStack.First();
            if (!infoForCurrentNode.IsVisible)
            {
                return;
            }

            if (profile != Profile.Unknown)
            {
                Profile bitmask = profile & this.CurrentProfile;
                if (bitmask != this.CurrentProfile)
                {
                    return;
                }
            }

            // write value
            if (!String.IsNullOrWhiteSpace(prefix))
            {
                this.TextWriter?.WriteAttributeString(prefix, localName, null, value);
            }
            else
            {
                this.TextWriter?.WriteAttributeString(localName, value);
            }
        } // !WriteAttributeString()


        public void WriteValue(string value, Profile profile = Profile.Unknown)
        {
            if (!_IsValidXmlString(value))
            {
                if (_AutomaticallyCleanInvalidXmlCharacters == true)
                {
                    value = _CleanInvalidXmlChars(value);
                }
                else
                {
                    throw new IllegalCharacterException($"'{value}' contains illegal characters for xml.");
                }
            }

            StackInfo infoForCurrentNode = this.XmlStack.First();
            if (!infoForCurrentNode.IsVisible)
            {
                return;
            }

            // write value
            this.TextWriter?.WriteValue(value);
        } // !WriteAttributeString()


        public void WriteComment(string comment, Profile profile = Profile.Unknown)
        {
            if (!_IsValidXmlString(comment))
            {
                if (_AutomaticallyCleanInvalidXmlCharacters == true)
                {
                    comment = _CleanInvalidXmlChars(comment);
                }
                else
                {
                    throw new IllegalCharacterException($"'{comment}' contains illegal characters for xml.");
                }
            }

            StackInfo infoForCurrentNode = this.XmlStack.FirstOrDefault();
            if ((infoForCurrentNode != null) && !infoForCurrentNode.IsVisible)
            {
                return;
            }

            // write value
            this.TextWriter?.WriteComment(comment);
        } // !WriteComment()


        public void WriteRawString(string value, Profile profile = Profile.Unknown)
        {
            if (!_IsValidXmlString(value))
            {
                if (_AutomaticallyCleanInvalidXmlCharacters == true)
                {
                    value = _CleanInvalidXmlChars(value);
                }
                else
                {
                    throw new IllegalCharacterException($"'{value}' contains illegal characters for xml.");
                }
            }

            StackInfo infoForCurrentNode = this.XmlStack.First();
            if (!infoForCurrentNode.IsVisible)
            {
                return;
            }

            _NeedToIndentEndElement = true;

            // write value
            this.TextWriter?.WriteString(value);
        } // !WriteRawString()


        /// <summary>
        /// Writes the raw indention using IndentChars according to the current xml tree position.
        /// </summary>
        public void WriteRawIndention(Profile profile = Profile.Unknown)
        {
            if (this.TextWriter == null)
            {
                return;
            }

            StackInfo infoForCurrentNode = this.XmlStack.First();
            if (!infoForCurrentNode.IsVisible)
            {
                return;
            }

            _NeedToIndentEndElement = true;

            // write value
            for (int i = 0; i < this.XmlStack.Count; i++)
            {
                this.TextWriter?.WriteString(this.TextWriter.Settings.IndentChars);
            }
        } // !WriteRawIndention()


        #region Stack Management
        private bool _DoesProfileFitToCurrentProfile(Profile profile)
        {
            if (profile != Profile.Unknown)
            {
                Profile maskedProfile = (profile & this.CurrentProfile);
                if (maskedProfile != this.CurrentProfile)
                {
                    return false;
                }
            }

            return true;
        } // !_DoesProfileFitToCurrentProfile()


        private bool _IsNodeVisible()
        {
            foreach (StackInfo stackInfo in this.XmlStack)
            {
                if (!stackInfo.IsVisible)
                {
                    return false;
                }
            }

            return true;
        } // !_IsNodeVisible()
        #endregion // !Stack Management


        #region Convenience functions
        public void WriteElementString(string prefix, string localName, string value, Profile profile = Profile.Unknown)
        {
            this.WriteElementString(prefix, localName, null, value, profile);
        } // !WriteElementString()


        public void WriteAttributeString(string localName, string value, Profile profile = Profile.Unknown)
        {
            this.WriteAttributeString(null, localName, value, profile);
        } // !WriteAttributeString(


        internal void SetNamespaces(Dictionary<string, string> namespaces)
        {
            this.Namespaces = namespaces;
        }
        #endregion // !Convenience functions


        #region Cleanúp functions
        /// <summary>
        /// Make sure that the given string does not contain invalid xml characters.
        /// The invalid characters are removed from the string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string _CleanInvalidXmlChars(string input)
        {
            var output = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (_IsValidXmlChar(c))
                {
                    output.Append(c);
                }
            }
            return output.ToString();
        } // !_CleanInvalidXmlChars()


        private bool _IsValidXmlString(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return true; // empty strings are valid
            }

            foreach (char c in input)
            {
                if (!_IsValidXmlChar(c))
                {
                    return false;
                }
            }
            return true;
        } // !_IsValidXmlString()
        

        private bool _IsValidXmlChar(char c)
        {
            return
                c == 0x9 || c == 0xA || c == 0xD ||
                (c >= 0x20 && c <= 0xD7FF) ||
                (c >= 0xE000 && c <= 0xFFFD) ||
                (c >= 0x10000 && c <= 0x10FFFF);
        } // !_IsValidXmlChar()

        #endregion // !Cleanup function
    }
}
