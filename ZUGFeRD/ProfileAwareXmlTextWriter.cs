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
        private XmlTextWriter TextWriter;
        private Stack<StackInfo> XmlStack = new Stack<StackInfo>();
        private Profile CurrentProfile = Profile.Unknown;


        public System.Xml.Formatting Formatting
        {
            get
            {
                return this.TextWriter.Formatting;
            }
            set
            {
                this.TextWriter.Formatting = value;
            }
        }


        public ProfileAwareXmlTextWriter(string filename, System.Text.Encoding encoding, Profile profile)
        {
            this.TextWriter = new XmlTextWriter(filename, encoding);
            this.CurrentProfile = profile;
        }


        public ProfileAwareXmlTextWriter(System.IO.Stream w, System.Text.Encoding encoding, Profile profile)
        {
            this.TextWriter = new XmlTextWriter(w, encoding);
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


        public void WriteStartElement(string prefix, string localName, string ns, Profile profile = Profile.Unknown)
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
                this.TextWriter?.WriteEndElement();
            }
        }


        public void WriteOptionalElementString(string tagName, string value, Profile profile = Profile.Unknown)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                WriteElementString(tagName, value, profile);
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


        public void WriteAttributeString(string prefix, string localName, string ns, string value, Profile profile = Profile.Unknown)
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
                this.TextWriter?.WriteAttributeString(prefix, localName, ns, value);
            }
            else if (!String.IsNullOrWhiteSpace(ns))
            {
                this.TextWriter?.WriteAttributeString(localName, ns, value);
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
        public void WriteElementString(string localName, string ns, string value, Profile profile = Profile.Unknown)
        {
            this.WriteElementString(null, localName, ns, value, profile);
        } // !WriteElementString()


        public void WriteElementString(string localName, string value, Profile profile = Profile.Unknown)
        {
            this.WriteElementString(null, localName, null, value, profile);
        } // !WriteElementString()


        public void WriteAttributeString(string localName, string value, Profile profile = Profile.Unknown)
        {
            this.WriteAttributeString(null, localName, null, value, profile);
        } // !WriteAttributeString()


        public void WriteAttributeString(string localName, string ns, string value, Profile profile = Profile.Unknown)
        {
            this.WriteAttributeString(null, localName, ns, value, profile);
        } // !WriteAttributeString()


        public void WriteStartElement(string localName, Profile profile = Profile.Unknown)
        {
            this.WriteStartElement(null, localName, null, profile);
        } // !WriteStartElement()


        public void WriteStartElement(string localName, string ns, Profile profile = Profile.Unknown)
        {
            this.WriteStartElement(null, localName, ns, profile);
        } // !WriteStartElement()
        #endregion // !Convenience functions
    }
}
