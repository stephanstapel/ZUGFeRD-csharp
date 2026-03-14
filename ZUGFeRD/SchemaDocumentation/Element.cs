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

namespace s2industries.ZUGFeRD.SchemaDocumentation
{
    /// <summary>
    /// Represents a single XML element extracted from a ZUGFeRD / Factur-X schema
    /// together with its documentation metadata (cardinality, BT/BG numbers, etc.).
    ///
    /// <para>
    /// Instances are produced by <see cref="SchemaReader"/> and form a tree via
    /// <see cref="Children"/>.
    /// </para>
    /// </summary>
    public sealed class Element
    {
        /// <summary>
        /// Qualified element name, including the namespace prefix where applicable.
        /// Example: <c>ram:ExchangedDocumentContext</c>
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Qualified XSD type name, including the namespace prefix where applicable.
        /// Example: <c>ram:ExchangedDocumentContextType</c>
        /// </summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Cardinality as defined in the XSD schema, formatted as
        /// <c>minOccurs..maxOccurs</c>.
        /// Examples: <c>0..1</c>, <c>1..1</c>, <c>0..*</c>
        /// </summary>
        public string XsdCardinality { get; set; } = string.Empty;

        /// <summary>Child elements of this element.</summary>
        public List<Element> Children { get; } = new List<Element>();

        /// <summary>
        /// Absolute, namespace-qualified XPath expression for this element
        /// from the document root.
        /// Example: <c>/rsm:CrossIndustryDocument/rsm:HeaderExchangedDocument/ram:ID</c>
        /// </summary>
        public string XPath { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable description sourced from the EN16931 Schematron
        /// cross-reference.  Empty when no mapping is available.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// List of ZUGFeRD profiles in which this element is used.
        /// Derived from Schematron assertion / report rules.
        /// Example values: <c>BASIC</c>, <c>COMFORT</c>, <c>EXTENDED</c>
        /// </summary>
        public List<string> ProfileSupport { get; internal set; } = new List<string>();

        /// <summary>
        /// EN16931 Business Term identifier(s) associated with this element,
        /// comma-separated when multiple apply.
        /// Example: <c>BT-116</c>
        /// </summary>
        public string BusinessTerm { get; internal set; } = string.Empty;

        /// <summary>
        /// EN16931 Business Group identifier(s) associated with this element,
        /// comma-separated when multiple apply.
        /// Example: <c>BG-23</c>
        /// </summary>
        public string Id { get; internal set; } = string.Empty;

        /// <summary>
        /// Validation rule messages sourced from the ZUGFeRD 1.0 Schematron
        /// (assert statements that enforce mandatory presence or count).
        /// </summary>
        public string BusinessRule { get; internal set; } = string.Empty;

        /// <summary>
        /// Cardinality as mandated by the CII / Schematron validation rules.
        /// May be stricter than <see cref="XsdCardinality"/> (e.g., XSD allows
        /// optional, but Schematron requires exactly 1).
        /// </summary>
        public string CiiCardinality { get; internal set; } = string.Empty;

        /// <summary>
        /// Arbitrary additional metadata not covered by the typed properties above.
        /// The <see cref="SchemaReader"/> may populate this with extra context.
        /// </summary>
        public Dictionary<string, string> AdditionalData { get; set; } =
            new Dictionary<string, string>();
    }
}
