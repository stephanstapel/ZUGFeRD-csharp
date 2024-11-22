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

        /// <summary>
        /// In case we are writing raw values, the automatic indention of end elements might get broken.
        /// In order to heal that, we need to manually add indention.
        /// </summary>
        private bool _NeedToIndentEndElement = false;


        public ProfileAwareXmlTextWriter(string filename, System.Text.Encoding encoding, Profile profile)
        {
            this.TextWriter = XmlWriter.Create(filename, new XmlWriterSettings()
            {
                Encoding = encoding,
                Indent = true
            });
            
            this.CurrentProfile = profile;
        }


        public ProfileAwareXmlTextWriter(System.IO.Stream w, Profile profile)
        {
            this.TextWriter = XmlWriter.Create(w, new XmlWriterSettings()
            {
                Encoding = new UTF8Encoding(false, true),
                Indent = true
            });
            this.CurrentProfile = profile;
        }


        public void Close()
        {
            this.TextWriter?.Close();
        }


        public void Flush()
        {
            this.TextWriter?.Flush();
        }


        public void WriteStartElement(string prefix, string localName, Profile profile = Profile.Unknown)
        {
            Profile _profile = profile;
            if (profile == Profile.Unknown)
            {
                _profile = this.CurrentProfile;
            }

            if (!_IsNodeVisible() || !_DoesProfileFitToCurrentProfile(_profile))
            {
                this.XmlStack.Push(new StackInfo() { Profile = _profile, IsVisible = false });
                return;
            }
            else
            {
                this.XmlStack.Push(new StackInfo() { Profile = _profile, IsVisible = true });
            }


            string ns = Namespaces.ContainsKey(prefix) ? Namespaces[prefix] : null; 

            // write value
            if (!String.IsNullOrWhiteSpace(prefix))
            {
                this.TextWriter?.WriteStartElement(prefix, localName, ns);
            }
            else if (!String.IsNullOrWhiteSpace(ns))
            {
                this.TextWriter?.WriteStartElement(localName, ns);
            }
            else
            {
                this.TextWriter?.WriteStartElement(localName);
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
        }


        public void WriteOptionalElementString(string prefix, string tagName, string value, Profile profile = Profile.Unknown)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                WriteElementString(prefix, tagName, value, profile);
            }
        } // !WriteOptionalElementString()


        public void WriteElementString(string prefix, string localName, string ns, string value, Profile profile = Profile.Unknown)
        {
            Profile _profile = profile;
            if (profile == Profile.Unknown)
            {
                _profile = this.CurrentProfile;
            }

            if (!_IsNodeVisible() || !_DoesProfileFitToCurrentProfile(_profile))
            {
                return;
            }

            // write value
            if (!String.IsNullOrWhiteSpace(prefix))
            {
                this.TextWriter?.WriteElementString(prefix, localName, ns, value);
            }
            else if (!String.IsNullOrWhiteSpace(ns))
            {
                this.TextWriter?.WriteElementString(localName, ns, value);
            }
            else
            {
                this.TextWriter?.WriteElementString(localName, value);
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
            StackInfo infoForCurrentNode = this.XmlStack.First();
            if (!infoForCurrentNode.IsVisible)
            {
                return;
            }

            // write value
            this.TextWriter?.WriteValue(value);
        } // !WriteAttributeString()


        public void WriteRawString(string value, Profile profile = Profile.Unknown)
        {
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


        #region Convience functions        
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
    }
}
