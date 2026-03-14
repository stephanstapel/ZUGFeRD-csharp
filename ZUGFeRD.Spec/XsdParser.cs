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
using System.Linq;
using System.Xml.Linq;

namespace s2industries.ZUGFeRD.Spec
{
    /// <summary>
    /// Generic XSD parser that extracts the element hierarchy and cardinalities
    /// from any W3C XML Schema set.  Handles both the CII schema style (inline
    /// element declarations with <c>name</c> + <c>type</c> attributes) and the
    /// OASIS UBL 2.1 schema style (element references via <c>ref</c> attribute
    /// that resolve to global element declarations).
    /// </summary>
    public class XsdParser
    {
        private static readonly XNamespace _xs = "http://www.w3.org/2001/XMLSchema";

        // -----------------------------------------------------------------------
        // Preconfigured options for well-known schema sets
        // -----------------------------------------------------------------------

        /// <summary>
        /// Options for parsing the EN16931 CII (Factur-X / ZUGFeRD) schema.
        /// Root XSD file is typically named <c>FACTUR-X_EN16931.xsd</c>.
        /// </summary>
        public static readonly XsdParserOptions CiiOptions = new XsdParserOptions
        {
            RootXsdFileName = "FACTUR-X_EN16931.xsd",
            RootElementPrefix = "rsm",
            NamespacePrefixes = new Dictionary<string, string>
            {
                { "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100", "rsm" },
                { "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100", "ram" },
                { "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100", "udt" },
                { "urn:un:unece:uncefact:data:standard:QualifiedDataType:100", "qdt" }
            }
        };

        /// <summary>
        /// Options for parsing the OASIS UBL 2.1 Invoice schema.
        /// Root XSD file is <c>UBL-Invoice-2.1.xsd</c> (from the OASIS UBL 2.1
        /// distribution, typically under <c>xsd/maindoc/</c>).
        /// The full UBL 2.1 package can be downloaded from
        /// http://docs.oasis-open.org/ubl/os-UBL-2.1/UBL-2.1.zip
        /// </summary>
        public static readonly XsdParserOptions UblInvoiceOptions = new XsdParserOptions
        {
            RootXsdFileName = "UBL-Invoice-2.1.xsd",
            RootElementPrefix = "ubl",
            NamespacePrefixes = new Dictionary<string, string>
            {
                { "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2", "ubl" },
                { "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", "cac" },
                { "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", "cbc" },
                { "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDataTypes-2", "qdt" },
                { "urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2", "udt" },
                { "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2", "ext" }
            }
        };

        /// <summary>
        /// Options for parsing the OASIS UBL 2.1 CreditNote schema.
        /// Root XSD file is <c>UBL-CreditNote-2.1.xsd</c>.
        /// </summary>
        public static readonly XsdParserOptions UblCreditNoteOptions = new XsdParserOptions
        {
            RootXsdFileName = "UBL-CreditNote-2.1.xsd",
            RootElementPrefix = "cn",
            NamespacePrefixes = new Dictionary<string, string>
            {
                { "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2", "cn" },
                { "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2", "cac" },
                { "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2", "cbc" },
                { "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDataTypes-2", "qdt" },
                { "urn:oasis:names:specification:ubl:schema:xsd:UnqualifiedDataTypes-2", "udt" },
                { "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2", "ext" }
            }
        };

        // -----------------------------------------------------------------------
        // Instance state
        // -----------------------------------------------------------------------

        // typeName → XElement of the xs:complexType definition
        private readonly Dictionary<string, XElement> _complexTypes =
            new Dictionary<string, XElement>(StringComparer.Ordinal);

        // typeName → target namespace URI of the XSD file that defined it
        private readonly Dictionary<string, string> _typeNamespace =
            new Dictionary<string, string>(StringComparer.Ordinal);

        // localElementName → XElement of the global xs:element declaration
        private readonly Dictionary<string, XElement> _globalElements =
            new Dictionary<string, XElement>(StringComparer.Ordinal);

        // localElementName → target namespace URI of the XSD that declared it
        private readonly Dictionary<string, string> _globalElementNamespace =
            new Dictionary<string, string>(StringComparer.Ordinal);

        // -----------------------------------------------------------------------
        // Public API
        // -----------------------------------------------------------------------

        /// <summary>
        /// Parses the XSD files in the given directory using preconfigured CII options
        /// and returns a hierarchical list of all schema elements.
        /// </summary>
        /// <param name="xsdDirectory">Directory containing the EN16931 CII XSD files.</param>
        public List<SpecElement> ParseCii(string xsdDirectory)
        {
            return Parse(xsdDirectory, CiiOptions);
        } // !ParseCii()


        /// <summary>
        /// Parses the XSD files in the given directory using preconfigured UBL 2.1 Invoice
        /// options and returns a hierarchical list of all schema elements.
        /// </summary>
        /// <param name="xsdDirectory">
        /// Directory containing the OASIS UBL 2.1 XSD files.
        /// This should be the folder that contains <c>UBL-Invoice-2.1.xsd</c>
        /// together with the common component XSDs.
        /// The full UBL 2.1 XSD package is available at
        /// http://docs.oasis-open.org/ubl/os-UBL-2.1/UBL-2.1.zip
        /// (extract and point to the <c>xsd/maindoc</c> directory or the
        /// flat <c>xsdrt/maindoc</c> directory).
        /// </param>
        public List<SpecElement> ParseUblInvoice(string xsdDirectory)
        {
            return Parse(xsdDirectory, UblInvoiceOptions);
        } // !ParseUblInvoice()


        /// <summary>
        /// Parses the XSD files in the given directory using preconfigured UBL 2.1 CreditNote
        /// options and returns a hierarchical list of all schema elements.
        /// </summary>
        /// <param name="xsdDirectory">Directory containing the OASIS UBL 2.1 XSD files.</param>
        public List<SpecElement> ParseUblCreditNote(string xsdDirectory)
        {
            return Parse(xsdDirectory, UblCreditNoteOptions);
        } // !ParseUblCreditNote()


        /// <summary>
        /// Parses the XSD files in the given directory using the supplied options
        /// and returns a hierarchical list of all schema elements starting from the
        /// configured root element.
        /// </summary>
        /// <param name="xsdDirectory">Directory that contains the XSD files to parse.</param>
        /// <param name="options">
        /// Configuration describing the root XSD file name, the root element
        /// namespace prefix, and the namespace-to-prefix mapping.
        /// Use <see cref="CiiOptions"/>, <see cref="UblInvoiceOptions"/>, or
        /// <see cref="UblCreditNoteOptions"/> for the well-known schema sets, or
        /// supply custom options for other schemas.
        /// </param>
        public List<SpecElement> Parse(string xsdDirectory, XsdParserOptions options)
        {
            _complexTypes.Clear();
            _typeNamespace.Clear();
            _globalElements.Clear();
            _globalElementNamespace.Clear();

            // Index all types and global element declarations from every XSD in the directory
            _loadSchemas(xsdDirectory, options);

            // Find the root XSD file
            string rootXsdPath = Path.Combine(xsdDirectory, options.RootXsdFileName);
            if (!File.Exists(rootXsdPath))
            {
                throw new FileNotFoundException(
                    $"Root XSD not found: {rootXsdPath}. " +
                    $"Ensure the XSD directory contains '{options.RootXsdFileName}'.",
                    rootXsdPath);
            }

            XDocument rootXsd = XDocument.Load(rootXsdPath);

            // Find the root element declaration in the root XSD
            XElement? rootElementDecl = rootXsd.Root?.Elements(_xs + "element").FirstOrDefault();
            if (rootElementDecl == null)
            {
                return new List<SpecElement>();
            }

            string rootName = rootElementDecl.Attribute("name")?.Value ?? string.Empty;
            string rootTypeName = _stripPrefix(rootElementDecl.Attribute("type")?.Value ?? string.Empty);
            string rootNsPrefix = options.RootElementPrefix;

            SpecElement root = new SpecElement
            {
                Name = rootName,
                NamespacePrefix = rootNsPrefix,
                XPath = $"/{rootNsPrefix}:{rootName}",
                TypeName = rootTypeName,
                MinOccurs = 1,
                MaxOccurs = 1,
                Cardinality = "1..1",
                Depth = 0
            };

            _expandChildren(root, rootTypeName, options, new HashSet<string>());

            return new List<SpecElement> { root };
        } // !Parse()


        // -----------------------------------------------------------------------
        // Private helpers
        // -----------------------------------------------------------------------

        private void _loadSchemas(string xsdDirectory, XsdParserOptions options)
        {
            foreach (string xsdFile in Directory.GetFiles(xsdDirectory, "*.xsd"))
            {
                try
                {
                    XDocument doc = XDocument.Load(xsdFile);
                    if (doc.Root == null) continue;

                    string targetNs = doc.Root.Attribute("targetNamespace")?.Value ?? string.Empty;

                    // Index complex types
                    foreach (XElement ct in doc.Root.Elements(_xs + "complexType"))
                    {
                        string name = ct.Attribute("name")?.Value ?? string.Empty;
                        if (name.Length > 0 && !_complexTypes.ContainsKey(name))
                        {
                            _complexTypes[name] = ct;
                            _typeNamespace[name] = targetNs;
                        }
                    }

                    // Index global element declarations (needed for UBL ref= pattern)
                    foreach (XElement el in doc.Root.Elements(_xs + "element"))
                    {
                        string name = el.Attribute("name")?.Value ?? string.Empty;
                        if (name.Length > 0 && !_globalElements.ContainsKey(name))
                        {
                            _globalElements[name] = el;
                            _globalElementNamespace[name] = targetNs;
                        }
                    }
                }
                catch
                {
                    // Skip files that cannot be parsed
                }
            }
        } // !_loadSchemas()


        private void _expandChildren(
            SpecElement parent,
            string typeName,
            XsdParserOptions options,
            HashSet<string> visitedTypes)
        {
            if (string.IsNullOrEmpty(typeName) ||
                !_complexTypes.TryGetValue(typeName, out XElement? complexType))
            {
                return;
            }

            // Prevent infinite recursion for recursive types
            if (!visitedTypes.Add(typeName))
            {
                return;
            }

            // Walk through sequence / all groups (handle nesting, e.g. choice inside sequence)
            IEnumerable<XElement> elementDecls = _collectElementDecls(complexType);

            foreach (XElement elementDecl in elementDecls)
            {
                string? childName;
                string? childTypeName;
                string nsPrefix;
                int minOccurs = _parseOccurs(elementDecl.Attribute("minOccurs")?.Value, 1);
                int maxOccurs = _parseOccurs(elementDecl.Attribute("maxOccurs")?.Value, 1);

                string refAttr = elementDecl.Attribute("ref")?.Value ?? string.Empty;
                if (refAttr.Length > 0)
                {
                    // UBL style: <xs:element ref="cbc:ID" minOccurs="0"/>
                    // The ref value may include a namespace prefix (e.g., "cbc:ID") or just a local name.
                    string refLocalName = _stripPrefix(refAttr);
                    string refPrefix = _getPrefixFromQName(refAttr);

                    if (!string.IsNullOrEmpty(refPrefix))
                    {
                        // Prefix was explicit in ref — use it directly
                        nsPrefix = refPrefix;
                    }
                    else if (_globalElementNamespace.TryGetValue(refLocalName, out string? elemNs)
                             && !string.IsNullOrEmpty(elemNs))
                    {
                        // Derive prefix from the target namespace of the global element's XSD
                        nsPrefix = _resolvePrefix(elemNs, options);
                    }
                    else
                    {
                        nsPrefix = options.RootElementPrefix;
                    }

                    childName = refLocalName;

                    // Look up the global element declaration to find its type
                    childTypeName = string.Empty;
                    if (_globalElements.TryGetValue(refLocalName, out XElement? globalEl))
                    {
                        string? globalType = globalEl.Attribute("type")?.Value;
                        childTypeName = globalType != null ? _stripPrefix(globalType) : string.Empty;
                    }
                }
                else
                {
                    // CII style (or any inline declaration): <xs:element name="..." type="..."/>
                    childName = elementDecl.Attribute("name")?.Value;
                    if (string.IsNullOrEmpty(childName))
                    {
                        continue;
                    }

                    string? rawType = elementDecl.Attribute("type")?.Value;
                    childTypeName = rawType != null ? _stripPrefix(rawType) : string.Empty;

                    // Determine prefix from the type's namespace
                    if (!string.IsNullOrEmpty(childTypeName)
                        && _typeNamespace.TryGetValue(childTypeName, out string? typeNs)
                        && !string.IsNullOrEmpty(typeNs))
                    {
                        nsPrefix = _resolvePrefix(typeNs, options);
                    }
                    else
                    {
                        // Fallback: use the parent type's namespace prefix
                        string parentTypeNs = _typeNamespace.TryGetValue(typeName, out string? pns) ? pns : string.Empty;
                        nsPrefix = _resolvePrefix(parentTypeNs, options);
                    }
                }

                if (string.IsNullOrEmpty(childName))
                {
                    continue;
                }

                string childXPath = $"{parent.XPath}/{nsPrefix}:{childName}";

                SpecElement child = new SpecElement
                {
                    Name = childName!,
                    NamespacePrefix = nsPrefix,
                    XPath = childXPath,
                    TypeName = childTypeName ?? string.Empty,
                    MinOccurs = minOccurs,
                    MaxOccurs = maxOccurs,
                    Cardinality = _buildCardinality(minOccurs, maxOccurs),
                    Depth = parent.Depth + 1,
                    ParentType = typeName
                };

                parent.Children.Add(child);

                // Recurse into child type (create a fresh copy of visitedTypes per branch)
                if (!string.IsNullOrEmpty(childTypeName))
                {
                    _expandChildren(child, childTypeName!, options, new HashSet<string>(visitedTypes));
                }
            }

            visitedTypes.Remove(typeName);
        } // !_expandChildren()


        /// <summary>
        /// Collects all direct xs:element children from sequence / all / choice groups
        /// within a complex type, handling one level of nesting for choice/sequence.
        /// </summary>
        private static IEnumerable<XElement> _collectElementDecls(XElement complexType)
        {
            // Find the top-level sequence / all / choice container
            XElement? container = complexType.Elements(_xs + "sequence").FirstOrDefault()
                ?? complexType.Elements(_xs + "all").FirstOrDefault()
                ?? complexType.Elements(_xs + "choice").FirstOrDefault();

            if (container == null)
            {
                // Check for a complexContent/extension/sequence pattern
                XElement? extension = complexType
                    .Descendants(_xs + "extension")
                    .FirstOrDefault();
                if (extension != null)
                {
                    container = extension.Elements(_xs + "sequence").FirstOrDefault()
                        ?? extension.Elements(_xs + "all").FirstOrDefault();
                }
            }

            if (container == null)
            {
                return Enumerable.Empty<XElement>();
            }

            // Collect direct element declarations and also those inside nested groups
            List<XElement> result = new List<XElement>();
            foreach (XNode node in container.Nodes())
            {
                if (node is XElement child)
                {
                    string localName = child.Name.LocalName;
                    if (localName == "element")
                    {
                        result.Add(child);
                    }
                    else if (localName == "sequence" || localName == "all" || localName == "choice")
                    {
                        // Collect elements from nested group
                        result.AddRange(child.Elements(_xs + "element"));
                    }
                }
            }
            return result;
        } // !_collectElementDecls()


        private static string _resolvePrefix(string namespaceUri, XsdParserOptions options)
        {
            if (!string.IsNullOrEmpty(namespaceUri)
                && options.NamespacePrefixes.TryGetValue(namespaceUri, out string? prefix))
            {
                return prefix;
            }
            return options.RootElementPrefix;
        } // !_resolvePrefix()


        private static string _stripPrefix(string qualifiedName)
        {
            int colon = qualifiedName.IndexOf(':');
            return colon >= 0 ? qualifiedName.Substring(colon + 1) : qualifiedName;
        } // !_stripPrefix()


        private static string _getPrefixFromQName(string qualifiedName)
        {
            int colon = qualifiedName.IndexOf(':');
            return colon >= 0 ? qualifiedName.Substring(0, colon) : string.Empty;
        } // !_getPrefixFromQName()


        private static int _parseOccurs(string? value, int defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            if (value == "unbounded")
            {
                return -1;
            }
            return int.TryParse(value, out int result) ? result : defaultValue;
        } // !_parseOccurs()


        private static string _buildCardinality(int min, int max)
        {
            string maxStr = max == -1 ? "n" : max.ToString();
            return $"{min}..{maxStr}";
        } // !_buildCardinality()
    }


    /// <summary>
    /// Configuration options for <see cref="XsdParser"/>.
    /// </summary>
    public class XsdParserOptions
    {
        /// <summary>
        /// File name of the root (main document) XSD within the schema directory.
        /// E.g., <c>FACTUR-X_EN16931.xsd</c> or <c>UBL-Invoice-2.1.xsd</c>.
        /// </summary>
        public string RootXsdFileName { get; set; } = string.Empty;

        /// <summary>
        /// Namespace prefix to use for the root element in XPath expressions.
        /// E.g., <c>rsm</c> for CII or <c>ubl</c> for UBL Invoice.
        /// </summary>
        public string RootElementPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Maps namespace URIs to their conventional short prefixes.
        /// Used when deriving element XPaths from the schema.
        /// </summary>
        public Dictionary<string, string> NamespacePrefixes { get; set; } =
            new Dictionary<string, string>(StringComparer.Ordinal);
    }
}
