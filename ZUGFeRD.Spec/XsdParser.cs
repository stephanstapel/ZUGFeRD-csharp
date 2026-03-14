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
    /// Parses EN16931 CII XSD schema files to extract the element hierarchy,
    /// types, and cardinalities.
    /// </summary>
    public class XsdParser
    {
        private static readonly XNamespace _xs = "http://www.w3.org/2001/XMLSchema";
        private static readonly XNamespace _rsm = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100";
        private static readonly XNamespace _ram = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100";

        // Map: typeName → complex type element from RABIE XSD
        private readonly Dictionary<string, XElement> _complexTypes = new Dictionary<string, XElement>(StringComparer.Ordinal);

        // Namespace prefix map for XPath construction
        private static readonly Dictionary<string, string> _nsPrefixes = new Dictionary<string, string>
        {
            { "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100", "rsm" },
            { "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100", "ram" },
            { "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100", "udt" },
            { "urn:un:unece:uncefact:data:standard:QualifiedDataType:100", "qdt" }
        };

        /// <summary>
        /// Parses the XSD files in the given directory and returns a hierarchical list of
        /// all elements defined in the CrossIndustryInvoice schema.
        /// </summary>
        /// <param name="xsdDirectory">Directory containing the EN16931 XSD files.</param>
        /// <returns>Root-level elements with nested children.</returns>
        public List<SpecElement> Parse(string xsdDirectory)
        {
            _complexTypes.Clear();

            // Load and index complex types from the RABIE XSD (main type definitions)
            _loadComplexTypes(xsdDirectory);

            // Start from root XSD
            string rootXsdPath = Path.Combine(xsdDirectory, "FACTUR-X_EN16931.xsd");
            if (!File.Exists(rootXsdPath))
            {
                throw new FileNotFoundException($"Root XSD not found: {rootXsdPath}");
            }

            XDocument rootXsd = XDocument.Load(rootXsdPath);

            // Find root element declaration
            XElement? rootElement = rootXsd.Root?.Elements(_xs + "element").FirstOrDefault();
            if (rootElement == null)
            {
                return new List<SpecElement>();
            }

            string rootName = rootElement.Attribute("name")?.Value ?? string.Empty;
            string rootTypeName = rootElement.Attribute("type")?.Value ?? string.Empty;

            // Strip namespace prefix from type name (e.g. "rsm:CrossIndustryInvoiceType" → "CrossIndustryInvoiceType")
            rootTypeName = _stripPrefix(rootTypeName);

            SpecElement root = new SpecElement
            {
                Name = rootName,
                NamespacePrefix = "rsm",
                XPath = $"/rsm:{rootName}",
                TypeName = rootTypeName,
                MinOccurs = 1,
                MaxOccurs = 1,
                Cardinality = "1..1",
                Depth = 0
            };

            // Find the root complex type in the root XSD itself
            XElement? rootComplexType = rootXsd.Root?
                .Elements(_xs + "complexType")
                .FirstOrDefault(e => e.Attribute("name")?.Value == rootTypeName);

            if (rootComplexType != null)
            {
                _complexTypes[rootTypeName] = rootComplexType;
            }

            _expandChildren(root, rootTypeName, new HashSet<string>());

            return new List<SpecElement> { root };
        } // !Parse()


        private void _loadComplexTypes(string xsdDirectory)
        {
            // Load all XSD files in the directory
            foreach (string xsdFile in Directory.GetFiles(xsdDirectory, "*.xsd"))
            {
                try
                {
                    XDocument doc = XDocument.Load(xsdFile);
                    foreach (XElement ct in doc.Root?.Elements(_xs + "complexType") ?? Enumerable.Empty<XElement>())
                    {
                        string? name = ct.Attribute("name")?.Value;
                        if (!string.IsNullOrEmpty(name) && !_complexTypes.ContainsKey(name!))
                        {
                            _complexTypes[name!] = ct;
                        }
                    }
                }
                catch
                {
                    // Skip unparseable files
                }
            }
        } // !_loadComplexTypes()


        private void _expandChildren(SpecElement parent, string typeName, HashSet<string> visitedTypes)
        {
            if (string.IsNullOrEmpty(typeName) || !_complexTypes.TryGetValue(typeName, out XElement? complexType))
            {
                return;
            }

            // Prevent infinite recursion for recursive types
            if (!visitedTypes.Add(typeName))
            {
                return;
            }

            string parentXPath = parent.XPath;

            // Find the sequence/all/choice element inside the complex type
            XElement? sequence = complexType
                .Descendants(_xs + "sequence")
                .FirstOrDefault()
                ?? complexType.Descendants(_xs + "all").FirstOrDefault();

            if (sequence == null)
            {
                visitedTypes.Remove(typeName);
                return;
            }

            foreach (XElement elementDecl in sequence.Elements(_xs + "element"))
            {
                string? childName = elementDecl.Attribute("name")?.Value;
                if (string.IsNullOrEmpty(childName))
                {
                    continue;
                }

                string? childTypeName = elementDecl.Attribute("type")?.Value;
                childTypeName = childTypeName != null ? _stripPrefix(childTypeName) : string.Empty;

                int minOccurs = _parseOccurs(elementDecl.Attribute("minOccurs")?.Value, 1);
                int maxOccurs = _parseOccurs(elementDecl.Attribute("maxOccurs")?.Value, 1);

                // Determine namespace prefix by checking if type is in ram namespace
                string nsPrefix = _guessNsPrefix(childTypeName, complexType);

                string childXPath = $"{parentXPath}/{nsPrefix}:{childName}";

                SpecElement child = new SpecElement
                {
                    Name = childName!,
                    NamespacePrefix = nsPrefix,
                    XPath = childXPath,
                    TypeName = childTypeName,
                    MinOccurs = minOccurs,
                    MaxOccurs = maxOccurs,
                    Cardinality = _buildCardinality(minOccurs, maxOccurs),
                    Depth = parent.Depth + 1,
                    ParentType = typeName
                };

                parent.Children.Add(child);

                // Recurse into child type (clone visited set per branch)
                if (!string.IsNullOrEmpty(childTypeName))
                {
                    _expandChildren(child, childTypeName, new HashSet<string>(visitedTypes));
                }
            }

            visitedTypes.Remove(typeName);
        } // !_expandChildren()


        private static string _guessNsPrefix(string typeName, XElement contextElement)
        {
            // Most types in the RABIE schema belong to ram namespace
            // Types in the root schema belong to rsm namespace
            if (typeName.EndsWith("Type", StringComparison.Ordinal))
            {
                // Check if the type is defined in the RSM context
                if (typeName == "CrossIndustryInvoiceType" || typeName == "SupplyChainTradeTransactionType")
                {
                    return "rsm";
                }
                return "ram";
            }
            return "ram";
        } // !_guessNsPrefix()


        private static string _stripPrefix(string typeName)
        {
            int colon = typeName.IndexOf(':');
            return colon >= 0 ? typeName.Substring(colon + 1) : typeName;
        } // !_stripPrefix()


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
}
