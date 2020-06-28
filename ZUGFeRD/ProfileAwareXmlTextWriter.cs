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
    internal class ProfileAwareXmlTextWriter
    {
        private XmlTextWriter TextWriter;
        private Stack<Profile> XmlStack = new Stack<Profile>();
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

            this.XmlStack.Push(_profile);

            if (!_DoesProfileFitIntoStack(_profile))
            {
                return;
            }
            if (!_DoesProfileFitToCurrentProfile(_profile))
            {
                return;
            }

            // write value
            if (!String.IsNullOrEmpty(prefix))
            {
                this.TextWriter?.WriteStartElement(prefix, localName, ns);
            }
            else if (!String.IsNullOrEmpty(ns))
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
            Profile profileForCurrentXmlLevel = this.XmlStack.Pop();
            if (_DoesProfileFitToCurrentProfile(profileForCurrentXmlLevel) && _DoesProfileFitIntoStack(profileForCurrentXmlLevel))
            {
                this.TextWriter?.WriteEndElement();
            }
        }


        public void WriteElementString(string prefix, string localName, string ns, string value, Profile profile = Profile.Unknown)
        {
            Profile _profile = profile;
            if (profile == Profile.Unknown)
            {
                _profile = this.CurrentProfile;
            }

            if (!_DoesProfileFitIntoStack(_profile))
            {
                return;
            }

            if (!_DoesProfileFitToCurrentProfile(_profile))
            {
                return;
            }

            // write value
            if (!String.IsNullOrEmpty(prefix))
            {
                this.TextWriter?.WriteElementString(prefix, localName, ns, value);
            }
            else if (!String.IsNullOrEmpty(ns))
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
            Profile profileOfCurrentNode = this.XmlStack.First();
            if (!_DoesProfileFitToCurrentProfile(profileOfCurrentNode))
            {
                return;
            }

            if (!_DoesProfileFitToCurrentProfile(profile))
            {
                return;
            }

            // write value
            if (!String.IsNullOrEmpty(prefix))
            {
                this.TextWriter?.WriteAttributeString(prefix, localName, ns, value);
            }
            else if (!String.IsNullOrEmpty(ns))
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
            Profile profileOfCurrentNode = this.XmlStack.First();
            if (!_DoesProfileFitToCurrentProfile(profileOfCurrentNode))
            {
                return;
            }

            if (!_DoesProfileFitToCurrentProfile(profile))
            {
                return;
            }

            // write value
            /*
            if (!String.IsNullOrEmpty(prefix))
            {
                this.TextWriter?.WriteAttributeString(prefix, localName, ns, value);
            }
            else if (!String.IsNullOrEmpty(ns))
            {
                this.TextWriter?.WriteAttributeString(localName, ns, value);
            }
            else
            */
            {
                this.TextWriter?.WriteValue(value);
            }
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


        private bool _DoesProfileFitIntoStack(Profile profile)
        {
            if (profile == Profile.Unknown)
            {
                return true;
            }

            foreach (Profile stackProfile in this.XmlStack)
            {
                Profile bitmask = stackProfile & profile;
                if (bitmask != profile)
                {
                    return false;
                }
            }

            return true;
        } // !_DoesProfileFitIntoStack()
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
