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
using System.Text;
using System.Text.Json;

namespace s2industries.ZUGFeRD.Spec
{
    /// <summary>
    /// Main entry point for extracting XRechnung specification documentation.
    /// Combines information from EN16931 XSD schemas and XRechnung Schematron rules.
    /// </summary>
    public class SpecExtractor
    {
        /// <summary>
        /// Extracts the XRechnung specification from the given documentation root directory.
        /// </summary>
        /// <param name="xrechnungDocPath">
        /// Path to the XRechnung version folder, e.g.
        /// "documentation/xRechnung/XRechnung 3.0.1"
        /// </param>
        /// <param name="en16931XsdPath">
        /// Path to the EN16931 CII XSD directory, e.g.
        /// "documentation/zugferd211en/Schema/EN16931"
        /// </param>
        /// <param name="version">Version label, e.g. "3.0.1"</param>
        /// <returns>The fully populated <see cref="XRechnungSpec"/>.</returns>
        public XRechnungSpec Extract(string xrechnungDocPath, string en16931XsdPath, string version = "3.0.1")
        {
            XRechnungSpec spec = new XRechnungSpec { Version = version };

            // ---- 1. Extract element hierarchy from EN16931 XSD ----
            XsdParser xsdParser = new XsdParser();
            List<SpecElement> rootElements = xsdParser.Parse(en16931XsdPath);
            spec.Elements = rootElements;

            // ---- 2. Extract business rules from XRechnung schematron ----
            string schematronRoot = Path.Combine(xrechnungDocPath,
                $"xrechnung-{version}-schematron-2.0.1", "schematron");

            string commonSchFile = Path.Combine(schematronRoot, "common.sch");
            string ciiSchFile    = Path.Combine(schematronRoot, "cii", "XRechnung-CII-validation.sch");
            string ublSchFile    = Path.Combine(schematronRoot, "ubl", "XRechnung-UBL-validation.sch");

            SchematronParser schParser = new SchematronParser();

            if (File.Exists(ciiSchFile))
            {
                List<SpecRule> ciiRules = schParser.Parse(ciiSchFile, "CII", commonSchFile);
                spec.Rules.AddRange(ciiRules);
            }

            if (File.Exists(ublSchFile))
            {
                List<SpecRule> ublRules = schParser.Parse(ublSchFile, "UBL", commonSchFile);
                spec.Rules.AddRange(ublRules);
            }

            // ---- 3. Link rules to element paths ----
            _linkRulesToElements(spec);

            return spec;
        } // !Extract()


        /// <summary>
        /// Serializes the extracted spec to a JSON file.
        /// </summary>
        /// <param name="spec">The extracted specification.</param>
        /// <param name="outputPath">Target path for the JSON file.</param>
        public void WriteJson(XRechnungSpec spec, string outputPath)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(spec, options);
            File.WriteAllText(outputPath, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        } // !WriteJson()


        /// <summary>
        /// Returns a flattened list of all elements (depth-first traversal).
        /// </summary>
        public List<SpecElement> FlattenElements(XRechnungSpec spec)
        {
            List<SpecElement> flat = new List<SpecElement>();
            foreach (SpecElement root in spec.Elements)
            {
                _flattenRecursive(root, flat);
            }
            return flat;
        } // !FlattenElements()


        private static void _flattenRecursive(SpecElement element, List<SpecElement> result)
        {
            result.Add(element);
            foreach (SpecElement child in element.Children)
            {
                _flattenRecursive(child, result);
            }
        } // !_flattenRecursive()


        private void _linkRulesToElements(XRechnungSpec spec)
        {
            List<SpecElement> flat = FlattenElements(spec);

            // Build a lookup: XPath → SpecElement
            Dictionary<string, SpecElement> pathLookup = new Dictionary<string, SpecElement>(StringComparer.Ordinal);
            foreach (SpecElement el in flat)
            {
                if (!pathLookup.ContainsKey(el.XPath))
                {
                    pathLookup[el.XPath] = el;
                }
            }

            // For each rule context, try to find matching elements
            foreach (SpecRule rule in spec.Rules)
            {
                // Normalize context: strip predicate expressions to get a clean path
                string normalizedContext = _normalizeXPath(rule.Context);

                foreach (SpecElement el in flat)
                {
                    if (el.XPath.EndsWith(normalizedContext, StringComparison.OrdinalIgnoreCase)
                        || normalizedContext.EndsWith(el.XPath, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!el.ApplicableRuleIds.Contains(rule.Id))
                        {
                            el.ApplicableRuleIds.Add(rule.Id);
                        }
                    }
                }
            }
        } // !_linkRulesToElements()


        private static string _normalizeXPath(string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return xpath;
            }
            // Remove predicates [ ... ]
            return System.Text.RegularExpressions.Regex.Replace(xpath, @"\[[^\]]*\]", string.Empty).Trim();
        } // !_normalizeXPath()
    }
}
