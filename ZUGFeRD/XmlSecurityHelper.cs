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
using System.IO;
using System.Xml;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Central helper for loading untrusted XML in a hardened way.
    ///
    /// Invoice XML (ZUGFeRD / Factur-X / XRechnung) is frequently extracted from third-party
    /// PDFs or received from external senders and must therefore be treated as untrusted input.
    ///
    /// The settings below protect against:
    ///   * XML External Entity (XXE) attacks - local file disclosure and SSRF -
    ///     by disabling the resolver (<see cref="XmlReaderSettings.XmlResolver"/> = null).
    ///   * Entity-expansion denial of service ("billion laughs")
    ///     by prohibiting document type definitions entirely
    ///     (<see cref="DtdProcessing.Prohibit"/>), so no internal or external
    ///     entities can be declared or expanded.
    ///
    /// Valid ZUGFeRD / Factur-X / XRechnung documents never contain a DTD, so legitimate
    /// invoices are unaffected. A document that does contain a DTD is rejected with an
    /// <see cref="XmlException"/> while parsing.
    /// </summary>
    internal static class XmlSecurityHelper
    {
        /// <summary>
        /// Creates <see cref="XmlReaderSettings"/> hardened against XXE and entity-expansion DoS.
        ///
        /// CloseInput is left at its default (false) so the caller-owned stream is never
        /// closed by the reader, preserving the public contract of the reader Load() methods.
        /// </summary>
        internal static XmlReaderSettings CreateSecureReaderSettings()
        {
            return new XmlReaderSettings
            {
                // Reject any DTD. This single setting blocks both external DTD/entity
                // resolution (XXE) and internal entity expansion (billion laughs).
                DtdProcessing = DtdProcessing.Prohibit,
                // Belt-and-suspenders: never resolve external resources.
                XmlResolver = null,
                // Hard cap on characters produced from entity expansion. With DtdProcessing.Prohibit
                // no custom entities can exist, but this stays as defense-in-depth.
                MaxCharactersFromEntities = 1024
            };
        } // !CreateSecureReaderSettings()


        /// <summary>
        /// Loads an <see cref="XmlDocument"/> from the given (untrusted) stream using hardened settings.
        /// The stream is not closed.
        /// </summary>
        internal static XmlDocument LoadSecureDocument(Stream stream)
        {
            XmlDocument doc = new XmlDocument
            {
                // Ensure the document itself never resolves external resources either
                // (e.g. for any subsequent operation on the loaded document).
                XmlResolver = null
            };

            using (XmlReader reader = XmlReader.Create(stream, CreateSecureReaderSettings()))
            {
                doc.Load(reader);
            }

            return doc;
        } // !LoadSecureDocument()
    }
}
