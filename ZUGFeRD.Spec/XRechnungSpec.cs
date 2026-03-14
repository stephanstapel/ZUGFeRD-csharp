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
using System.Collections.Generic;

namespace s2industries.ZUGFeRD.Spec
{
    /// <summary>
    /// Represents the full extracted documentation of the XRechnung specification.
    /// Contains the element hierarchy from XSD schemas and the business rules from Schematron files.
    /// </summary>
    public class XRechnungSpec
    {
        /// <summary>
        /// Version of the XRechnung specification (e.g., "3.0.1")
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// All XML elements defined in the EN16931 CII schema, with their
        /// hierarchy, types, and cardinalities.
        /// </summary>
        public List<SpecElement> Elements { get; set; } = new List<SpecElement>();

        /// <summary>
        /// All business rule constraints extracted from the XRechnung schematron files.
        /// Includes both CII and UBL rules.
        /// </summary>
        public List<SpecRule> Rules { get; set; } = new List<SpecRule>();
    }

    /// <summary>
    /// Represents a single XML element from the schema definition.
    /// </summary>
    public class SpecElement
    {
        /// <summary>
        /// Local name of the XML element (without namespace prefix).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The XML namespace prefix used for this element.
        /// </summary>
        public string NamespacePrefix { get; set; } = string.Empty;

        /// <summary>
        /// Full XPath from the document root to this element.
        /// </summary>
        public string XPath { get; set; } = string.Empty;

        /// <summary>
        /// The XSD type name of this element.
        /// </summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Minimum number of occurrences. 0 = optional, 1 = mandatory.
        /// </summary>
        public int MinOccurs { get; set; } = 1;

        /// <summary>
        /// Maximum number of occurrences. -1 represents unbounded.
        /// </summary>
        public int MaxOccurs { get; set; } = 1;

        /// <summary>
        /// Human-readable cardinality string (e.g., "0..1", "1..1", "0..n", "1..n").
        /// </summary>
        public string Cardinality { get; set; } = string.Empty;

        /// <summary>
        /// Depth level in the element hierarchy (root = 0).
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Name of the parent complex type, if known.
        /// </summary>
        public string? ParentType { get; set; }

        /// <summary>
        /// Child elements defined within this element's complex type.
        /// </summary>
        public List<SpecElement> Children { get; set; } = new List<SpecElement>();

        /// <summary>
        /// Business rules that apply to this element (by XPath context or assertion).
        /// </summary>
        public List<string> ApplicableRuleIds { get; set; } = new List<string>();
    }

    /// <summary>
    /// Represents a single Schematron business rule (assert).
    /// </summary>
    public class SpecRule
    {
        /// <summary>
        /// Unique identifier of the rule, e.g., "BR-DE-1", "PEPPOL-EN16931-R001".
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// The Schematron pattern this rule belongs to (e.g., "cii-pattern", "peppol-cii-pattern-1").
        /// </summary>
        public string Pattern { get; set; } = string.Empty;

        /// <summary>
        /// The syntax binding this rule applies to: "CII", "UBL", or "common".
        /// </summary>
        public string SyntaxBinding { get; set; } = string.Empty;

        /// <summary>
        /// The XPath context of the rule (from the enclosing &lt;rule context="..."&gt;).
        /// </summary>
        public string Context { get; set; } = string.Empty;

        /// <summary>
        /// The XPath test expression (the constraint).
        /// </summary>
        public string Test { get; set; } = string.Empty;

        /// <summary>
        /// Severity: "fatal" (MUST, Pflicht) or "warning" (SHOULD, Soll).
        /// </summary>
        public string Flag { get; set; } = string.Empty;

        /// <summary>
        /// The human-readable description / error message (typically in German for DE-specific rules).
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Business Term (BT) and Business Group (BG) references found in the description.
        /// E.g., ["BT-10", "BG-16"]
        /// </summary>
        public List<string> BtBgReferences { get; set; } = new List<string>();
    }
}
