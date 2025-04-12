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


        protected bool _IsReadableByThisReaderVersion(Stream stream, IList<string> validURIs)
        {
            long oldStreamPosition = stream.Position;
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                string data = reader.ReadToEnd().Replace(" ", String.Empty);
                foreach (string validURI in validURIs)
                {
                    if (data.IndexOf(String.Format(">{0}<", validURI), StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        stream.Position = oldStreamPosition;
                        return true;
                    }
                }
            }

            stream.Position = oldStreamPosition;
            return false;
        } // !_IsReadableByThisReaderVersion()
    }
}
