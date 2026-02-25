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
        public string Prefix;
        public string LocalName;
        public bool IsWritten;
    }

    /// <summary>
    /// Profilgesteuerter XML-Writer, der einen <see cref="XmlWriter"/> kapselt und zwei
    /// zentrale Verhaltensweisen hinzufügt:
    ///
    /// <para><strong>1. Profilfilterung:</strong><br/>
    /// Jedes XML-Element und -Attribut kann mit einem <see cref="Profile"/> verknüpft werden.
    /// Elemente, deren Profil nicht zum <c>CurrentProfile</c> des Writers passt, werden
    /// stillschweigend unterdrückt – einschließlich aller Kindelemente. Dadurch kann ein
    /// einziger Codepfad XML für verschiedene ZUGFeRD-/XRechnungs-Profile erzeugen.</para>
    ///
    /// <para><strong>2. Unterdrückung leerer Elemente (verzögertes Schreiben):</strong><br/>
    /// Start-Elemente werden <em>nicht</em> sofort in den zugrundeliegenden
    /// <see cref="XmlWriter"/> geschrieben. Stattdessen speichert
    /// <see cref="WriteStartElement"/> die Element-Informationen (Prefix, lokaler Name,
    /// Profil, Sichtbarkeit) lediglich auf einem internen Stack.
    /// Das eigentliche Start-Tag wird erst dann in die Ausgabe geschrieben, wenn
    /// konkreter Inhalt innerhalb des Elements geschrieben wird – d.h. wenn eine der
    /// folgenden Methoden aufgerufen wird:
    /// <list type="bullet">
    ///   <item><see cref="WriteElementString(string, string, string, string, Profile)"/></item>
    ///   <item><see cref="WriteAttributeString(string, string, string, Profile)"/></item>
    ///   <item><see cref="WriteValue"/></item>
    ///   <item><see cref="WriteComment"/></item>
    ///   <item><see cref="WriteRawString"/></item>
    ///   <item><see cref="WriteRawIndention"/></item>
    /// </list>
    /// Wird <see cref="WriteEndElement"/> erreicht, ohne dass Inhalt geschrieben wurde,
    /// werden weder Start- noch End-Tag ausgegeben – das leere Element wird damit
    /// vollständig aus der Ausgabe entfernt.</para>
    ///
    /// <para><strong>Interne Funktionsweise:</strong><br/>
    /// <see cref="StackInfo.IsWritten"/> verfolgt, ob ein Start-Tag bereits geschrieben
    /// ("geflusht") wurde. Vor dem Schreiben jeglichen Inhalts durchläuft die private
    /// Methode <c>_FlushPendingStartElements()</c> den Stack von der Wurzel bis zum
    /// aktuellen Element und schreibt alle ausstehenden (sichtbaren, noch nicht
    /// geschriebenen) Start-Tags in Dokumentreihenfolge. Dadurch wird sichergestellt,
    /// dass Elternelemente vor ihren Kindelementen geöffnet werden.</para>
    ///
    /// <para><strong>Beispiele:</strong></para>
    ///
    /// <para><em>Beispiel 1 – Leeres Element wird unterdrückt:</em></para>
    /// <code>
    /// writer.WriteStartElement("ram", "PartyName");
    /// // kein Inhalt geschrieben
    /// writer.WriteEndElement();
    /// // Ergebnis: es wird nichts in die Ausgabe geschrieben
    /// </code>
    ///
    /// <para><em>Beispiel 2 – Element mit Inhalt wird normal geschrieben:</em></para>
    /// <code>
    /// writer.WriteStartElement("ram", "PartyName");
    ///   writer.WriteElementString("ram", "Name", "Lieferant GmbH");
    /// writer.WriteEndElement();
    /// // Ergebnis: &lt;ram:PartyName&gt;&lt;ram:Name&gt;Lieferant GmbH&lt;/ram:Name&gt;&lt;/ram:PartyName&gt;
    /// </code>
    ///
    /// <para><em>Beispiel 3 – Verschachtelte leere Elemente werden alle unterdrückt:</em></para>
    /// <code>
    /// writer.WriteStartElement("ram", "SellerTradeParty");
    ///   writer.WriteStartElement("ram", "PostalTradeAddress");
    ///     // kein Inhalt
    ///   writer.WriteEndElement();
    /// writer.WriteEndElement();
    /// // Ergebnis: nichts wird geschrieben – beide Elemente werden unterdrückt,
    /// //           da keines von ihnen Inhalt hat
    /// </code>
    ///
    /// <para><em>Beispiel 4 – Verschachtelte Elemente, bei denen nur das innere Inhalt hat:</em></para>
    /// <code>
    /// writer.WriteStartElement("ram", "SellerTradeParty");
    ///   writer.WriteStartElement("ram", "PostalTradeAddress");
    ///     writer.WriteElementString("ram", "CityName", "Berlin");
    ///   writer.WriteEndElement();
    /// writer.WriteEndElement();
    /// // Ergebnis: beide Elemente werden geschrieben, weil das innere Element Inhalt hat.
    /// //           Das löst das Flushen aller Vorfahren-Start-Tags aus:
    /// // &lt;ram:SellerTradeParty&gt;
    /// //   &lt;ram:PostalTradeAddress&gt;
    /// //     &lt;ram:CityName&gt;Berlin&lt;/ram:CityName&gt;
    /// //   &lt;/ram:PostalTradeAddress&gt;
    /// // &lt;/ram:SellerTradeParty&gt;
    /// </code>
    ///
    /// <para><em>Beispiel 5 – Element nur mit Attribut wird geschrieben (Attribute zählen als Inhalt):</em></para>
    /// <code>
    /// writer.WriteStartElement("ram", "ID");
    ///   writer.WriteAttributeString("schemeID", "0088");
    /// writer.WriteEndElement();
    /// // Ergebnis: &lt;ram:ID schemeID="0088" /&gt;
    /// </code>
    /// </summary>
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
                this.XmlStack.Push(new StackInfo() { Profile = safeProfile, IsVisible = false, Prefix = prefix, LocalName = localName });
                return;
            }
            else
            {
                this.XmlStack.Push(new StackInfo() { Profile = safeProfile, IsVisible = true, Prefix = prefix, LocalName = localName });
            }
        } // !WriteStartElement()

        public void WriteEndElement()
        {
            StackInfo infoForCurrentXmlLevel = this.XmlStack.Pop();
            if (infoForCurrentXmlLevel.IsWritten)
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

            _FlushPendingStartElements();

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

            _FlushPendingStartElements();

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

            _FlushPendingStartElements();

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

            _FlushPendingStartElements();

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

            _FlushPendingStartElements();
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

            _FlushPendingStartElements();
            _NeedToIndentEndElement = true;

            // write value
            for (int i = 0; i < this.XmlStack.Count; i++)
            {
                this.TextWriter?.WriteString(this.TextWriter.Settings.IndentChars);
            }
        } // !WriteRawIndention()


        #region Stack Management
        private void _FlushPendingStartElements()
        {
            if (this.TextWriter == null)
            {
                return;
            }

            var items = this.XmlStack.ToArray();
            // items[0] = top (current), items[Length-1] = bottom (root)
            // iterate from root to current so start elements are written in document order
            for (int i = items.Length - 1; i >= 0; i--)
            {
                var info = items[i];
                if (!info.IsVisible)
                {
                    return;
                }

                if (info.IsWritten)
                {
                    continue;
                }

                string ns = Namespaces.ContainsKey(info.Prefix) ? Namespaces[info.Prefix] : null;

                if (!String.IsNullOrWhiteSpace(info.Prefix))
                {
                    this.TextWriter.WriteStartElement(info.Prefix, info.LocalName, ns);
                }
                else if (!String.IsNullOrWhiteSpace(ns))
                {
                    this.TextWriter.WriteStartElement(info.LocalName, ns);
                }
                else
                {
                    this.TextWriter.WriteStartElement(info.LocalName);
                }

                info.IsWritten = true;
            }
        } // !_FlushPendingStartElements()


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

        #region Cleanup functions
        private static bool ScanXmlString(string input, bool cleanInvalidCharacters, out string cleaned)
        {
            cleaned = input;

            if (string.IsNullOrEmpty(input))
                return true;

            StringBuilder sb = cleanInvalidCharacters ? new StringBuilder(input.Length) : null;

            int len = input.Length;
            for (int i = 0; i < len; i++)
            {
                char current = input[i];

                // 1) Normal BMP XML char
                if (XmlConvert.IsXmlChar(current))
                {
                    sb?.Append(current);
                    continue;
                }

                // 2) Surrogate pair (supplementary plane)
                if (char.IsHighSurrogate(current) && i + 1 < len)
                {
                    char next = input[i + 1];
                    if (char.IsLowSurrogate(next) && XmlConvert.IsXmlSurrogatePair(next, current))
                    {
                        sb?.Append(current);
                        sb?.Append(next);
                        i++; // consume low surrogate
                        continue;
                    }
                }

                // 3) Invalid XML char
                if (!cleanInvalidCharacters)
                    return false;
            }

            if (sb != null)
                cleaned = sb.ToString();

            return true;
        }

        private string _CleanInvalidXmlChars(string input)
        {
            ScanXmlString(input, true, out var cleaned);
            return cleaned;
        }

        private bool _IsValidXmlString(string input)
        {
            return ScanXmlString(input, false, out _);
        }
        #endregion // !Cleanup function
    }
}
