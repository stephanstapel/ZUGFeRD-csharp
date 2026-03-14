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
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace s2industries.ZUGFeRD.Spec
{
    /// <summary>
    /// Parses XRechnung Schematron (.sch) files and extracts all business rules.
    /// Handles both CII and UBL syntax bindings, including the common.sch patterns.
    /// </summary>
    public class SchematronParser
    {
        private static readonly XNamespace _sch = "http://purl.oclc.org/dsdl/schematron";

        // BT-xx or BG-xx reference pattern
        private static readonly Regex _btBgRegex = new Regex(
            @"\b(BT-\d+[a-z]?|BG-\d+)\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Pattern to extract the clean ID prefix from assert message, e.g. "[BR-DE-1]"
        private static readonly Regex _idInMsgRegex = new Regex(
            @"^\s*\[([^\]]+)\]\s*",
            RegexOptions.Compiled);

        /// <summary>
        /// Parses one or more Schematron files and returns all extracted business rules.
        /// </summary>
        /// <param name="schFile">Path to the Schematron .sch file.</param>
        /// <param name="syntaxBinding">Human-readable syntax label, e.g. "CII" or "UBL".</param>
        /// <param name="commonSchFile">Optional path to the common.sch file to include.</param>
        public List<SpecRule> Parse(string schFile, string syntaxBinding, string? commonSchFile = null)
        {
            List<SpecRule> rules = new List<SpecRule>();

            XDocument doc = XDocument.Load(schFile);
            XElement? schemaRoot = doc.Root;

            if (schemaRoot == null)
            {
                return rules;
            }

            // Collect all patterns defined directly in the file
            List<XElement> patterns = schemaRoot.Elements(_sch + "pattern").ToList();

            // If a common.sch is specified, also load patterns from it
            if (!string.IsNullOrEmpty(commonSchFile) && File.Exists(commonSchFile))
            {
                XDocument commonDoc = XDocument.Load(commonSchFile);
                List<XElement> commonPatterns = commonDoc.Root?.Elements(_sch + "pattern").ToList()
                    ?? new List<XElement>();
                patterns.AddRange(commonPatterns);
            }

            foreach (XElement pattern in patterns)
            {
                string patternId = pattern.Attribute("id")?.Value ?? string.Empty;

                foreach (XElement rule in pattern.Elements(_sch + "rule"))
                {
                    string context = rule.Attribute("context")?.Value ?? string.Empty;
                    context = _normalizeWhitespace(context);

                    foreach (XElement assert in rule.Elements(_sch + "assert"))
                    {
                        string ruleId = assert.Attribute("id")?.Value ?? string.Empty;
                        string flag = assert.Attribute("flag")?.Value ?? string.Empty;
                        string test = assert.Attribute("test")?.Value ?? string.Empty;
                        test = _normalizeWhitespace(test);

                        // Get the text content of the assert element
                        // Schematron allows mixed content (text + value-of + name elements)
                        string description = _extractDescription(assert);
                        description = _normalizeWhitespace(description);

                        // Extract BT/BG references from the description
                        List<string> btBgRefs = _extractBtBgReferences(description);

                        SpecRule specRule = new SpecRule
                        {
                            Id = ruleId,
                            Pattern = patternId,
                            SyntaxBinding = syntaxBinding,
                            Context = context,
                            Test = test,
                            Flag = flag,
                            Description = description,
                            BtBgReferences = btBgRefs
                        };

                        rules.Add(specRule);
                    }
                }
            }

            return rules;
        } // !Parse()


        private static string _extractDescription(XElement assert)
        {
            // Mixed content: <assert>text<name/>text<value-of select="..."/>text</assert>
            // Collect all text nodes and substitute placeholders
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (XNode node in assert.Nodes())
            {
                if (node is XText text)
                {
                    sb.Append(text.Value);
                }
                else if (node is XElement child)
                {
                    string localName = child.Name.LocalName;
                    if (localName == "name")
                    {
                        // <name/> outputs the context element name; use placeholder
                        sb.Append("{element}");
                    }
                    else if (localName == "value-of")
                    {
                        string select = child.Attribute("select")?.Value ?? string.Empty;
                        sb.Append($"{{value-of:{select}}}");
                    }
                }
            }

            return sb.ToString().Trim();
        } // !_extractDescription()


        private static List<string> _extractBtBgReferences(string description)
        {
            HashSet<string> refs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match match in _btBgRegex.Matches(description))
            {
                refs.Add(match.Value.ToUpperInvariant());
            }
            return refs.OrderBy(r => r).ToList();
        } // !_extractBtBgReferences()


        private static string _normalizeWhitespace(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            return Regex.Replace(s.Trim(), @"\s+", " ");
        } // !_normalizeWhitespace()
    }
}
