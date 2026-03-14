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

namespace s2industries.ZUGFeRD.SchemaDocumentation
{
    /// <summary>
    /// Reads ZUGFeRD / Factur-X XML schema files (XSD + Schematron) and builds a
    /// hierarchical tree of <see cref="Element"/> objects that fully describes the
    /// XML structure of a ZUGFeRD document, including cardinality, BT/BG numbers,
    /// and textual descriptions.
    ///
    /// <para>
    /// <b>BT/BG numbers</b> are NOT present in the ZUGFeRD 1.0 documentation — they
    /// were introduced with the EN16931 standard (ZUGFeRD 2.x / Factur-X).  When the
    /// path to an EN16931 Schematron file is supplied, the reader cross-references
    /// element local names against it to provide best-effort BT/BG annotations.
    /// </para>
    ///
    /// <para>
    /// Typical usage for ZUGFeRD 1.0:
    /// <code>
    /// var reader = new SchemaReader(
    ///     xsdDirectory:          @"documentation\zugferd10\Schema",
    ///     scmtFilePath:          @"documentation\zugferd10\Schema\ZUGFeRD_1p0.scmt",
    ///     en16931SchFilePath:    @"documentation\zugferd211en\Schema\EN16931\Schematron\FACTUR-X_EN16931.sch");
    ///
    /// Element root = reader.ReadSchema();
    /// </code>
    /// </para>
    /// </summary>
    public sealed class SchemaReader
    {
        // -----------------------------------------------------------------------
        // Namespace constants
        // -----------------------------------------------------------------------

        private static readonly XNamespace XsNs = "http://www.w3.org/2001/XMLSchema";
        private static readonly XNamespace SchNs = "http://purl.oclc.org/dsdl/schematron";

        /// <summary>
        /// Namespace URI → canonical prefix for ZUGFeRD 1.0 schemas.
        /// Extend or replace for other versions.
        /// </summary>
        private static readonly Dictionary<string, string> NsPrefix =
            new Dictionary<string, string>
            {
                { "urn:ferd:CrossIndustryDocument:invoice:1p0",                                                     "rsm" },
                { "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:12",              "ram" },
                { "urn:un:unece:uncefact:data:standard:QualifiedDataType:12",                                       "qdt" },
                { "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:15",                                     "udt" },
                { "http://www.w3.org/2001/XMLSchema",                                                               "xs"  },
            };

        /// <summary>Default XSD file names for ZUGFeRD 1.0 (resolved inside <see cref="_xsdDirectory"/>).</summary>
        private static readonly string[] DefaultXsdFiles =
        {
            "ZUGFeRD1p0.xsd",
            "ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_ReusableAggregateBusinessInformationEntity_12.xsd",
            "ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_QualifiedDataType_12.xsd",
            "ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_UnqualifiedDataType_15.xsd",
        };

        // -----------------------------------------------------------------------
        // Parsed internal state
        // -----------------------------------------------------------------------

        /// <summary>Qualified type name → ordered list of child element definitions.</summary>
        private readonly Dictionary<string, List<XsdElementDef>> _typeMap =
            new Dictionary<string, List<XsdElementDef>>();

        /// <summary>Root element local name → qualified type reference.</summary>
        private readonly Dictionary<string, string> _rootElements =
            new Dictionary<string, string>();

        /// <summary>Absolute XPath → list of Schematron constraints (from ZUGFeRD 1.0 SCMT).</summary>
        private readonly Dictionary<string, List<SchematronConstraint>> _scmtMap =
            new Dictionary<string, List<SchematronConstraint>>();

        /// <summary>
        /// (parentLocalName, elementLocalName) → BT/BG metadata.
        /// A fallback entry with an empty parentLocalName is also stored for
        /// elements that appear in only one context.
        /// </summary>
        private readonly Dictionary<string, BtInfo> _btMap =
            new Dictionary<string, BtInfo>();

        // -----------------------------------------------------------------------
        // Configuration
        // -----------------------------------------------------------------------

        private readonly string _xsdDirectory;
        private readonly string? _scmtFilePath;
        private readonly string? _en16931SchFilePath;
        private readonly string[]? _xsdFiles;

        // -----------------------------------------------------------------------
        // Regex helpers
        // -----------------------------------------------------------------------

        private static readonly Regex BtRegex =
            new Regex(@"\(?(B[TG]-\d+)\)?", RegexOptions.Compiled);

        private static readonly Regex SimpleElemRegex =
            new Regex(@"^\s*\(?\s*([a-z]+:[A-Za-z0-9]+)\s*\)?\s*$", RegexOptions.Compiled);

        private static readonly Regex BrPrefixRegex =
            new Regex(@"^\[[A-Z0-9\-]+\]-", RegexOptions.Compiled);

        private static readonly Regex CountRegex =
            new Regex(@"count\([^)]+\)\s*([=<>!]+)\s*(\d+)", RegexOptions.Compiled);

        // -----------------------------------------------------------------------
        // Constructor
        // -----------------------------------------------------------------------

        /// <summary>
        /// Initialises a new <see cref="SchemaReader"/>.
        /// </summary>
        /// <param name="xsdDirectory">
        ///   Directory that contains the XSD schema files.
        ///   For ZUGFeRD 1.0 this is <c>documentation/zugferd10/Schema</c>.
        /// </param>
        /// <param name="scmtFilePath">
        ///   Optional path to the ZUGFeRD Schematron (.scmt) file.
        ///   Used to derive profile support, business rules, and CII cardinality.
        /// </param>
        /// <param name="en16931SchFilePath">
        ///   Optional path to a Factur-X / ZUGFeRD 2.x EN16931 Schematron (.sch) file.
        ///   Used to cross-reference BT/BG numbers and English descriptions.
        /// </param>
        /// <param name="xsdFiles">
        ///   Optional list of XSD file names to parse (relative to
        ///   <paramref name="xsdDirectory"/>).  Defaults to the standard ZUGFeRD 1.0 set.
        /// </param>
        public SchemaReader(
            string xsdDirectory,
            string? scmtFilePath = null,
            string? en16931SchFilePath = null,
            string[]? xsdFiles = null)
        {
            if (xsdDirectory == null)
                throw new ArgumentNullException(nameof(xsdDirectory));
            _xsdDirectory = xsdDirectory;
            _scmtFilePath = scmtFilePath;
            _en16931SchFilePath = en16931SchFilePath;
            _xsdFiles = xsdFiles;
        }

        // -----------------------------------------------------------------------
        // Public API
        // -----------------------------------------------------------------------

        /// <summary>
        /// Parses all configured schema files and returns the root
        /// <see cref="Element"/> of the document hierarchy.
        /// </summary>
        /// <returns>
        /// Root element (<c>CrossIndustryDocument</c> for ZUGFeRD 1.0).
        /// </returns>
        public Element ReadSchema()
        {
            ParseXsdFiles();

            if (_scmtFilePath != null && File.Exists(_scmtFilePath))
                ParseScmt(_scmtFilePath);

            if (_en16931SchFilePath != null && File.Exists(_en16931SchFilePath))
                ParseEn16931Sch(_en16931SchFilePath);

            const string rootLocalName = "CrossIndustryDocument";
            _rootElements.TryGetValue(rootLocalName, out string? rootType);
            rootType = rootType ?? "rsm:CrossIndustryDocumentType";

            var rootXPath = "/rsm:" + rootLocalName;
            return BuildTree(rootType, rootLocalName, "", rootXPath, new HashSet<string>());
        }

        // -----------------------------------------------------------------------
        // XSD Parsing
        // -----------------------------------------------------------------------

        private void ParseXsdFiles()
        {
            var files = _xsdFiles ?? DefaultXsdFiles;
            foreach (var fileName in files)
            {
                var path = Path.Combine(_xsdDirectory, fileName);
                if (File.Exists(path))
                    ParseXsdFile(path);
            }
        }

        private void ParseXsdFile(string filePath)
        {
            var doc = XDocument.Load(filePath);
            var schema = doc.Root;
            if (schema == null) return;

            var targetNs = (string?)schema.Attribute("targetNamespace") ?? string.Empty;
            NsPrefix.TryGetValue(targetNs, out string? prefix);
            prefix = prefix ?? string.Empty;

            foreach (var child in schema.Elements())
            {
                var localName = child.Name.LocalName;
                var name = (string?)child.Attribute("name") ?? string.Empty;
                if (string.IsNullOrEmpty(name)) continue;

                if (localName == "element")
                {
                    var typeAttr = (string?)child.Attribute("type") ?? string.Empty;
                    _rootElements[name] = NormalizeTypeRef(typeAttr);
                }
                else if (localName == "complexType")
                {
                    var qname = prefix.Length > 0 ? prefix + ":" + name : name;
                    _typeMap[qname] = ParseComplexType(child, prefix);
                }
                // simpleType: not required for tree building
            }
        }

        private List<XsdElementDef> ParseComplexType(XElement ct, string enclosingPrefix)
        {
            var elements = new List<XsdElementDef>();
            foreach (var child in ct.Elements())
            {
                var local = child.Name.LocalName;
                if (local == "sequence" || local == "choice" || local == "all")
                {
                    elements.AddRange(ParseGroup(child, enclosingPrefix));
                }
                else if (local == "simpleContent" || local == "complexContent")
                {
                    var ext = child.Elements()
                                   .FirstOrDefault(e => e.Name.LocalName == "extension");
                    if (ext != null)
                        elements.AddRange(ParseGroup(ext, enclosingPrefix));
                }
            }
            return elements;
        }

        private IEnumerable<XsdElementDef> ParseGroup(XElement group, string enclosingPrefix)
        {
            foreach (var child in group.Elements())
            {
                var local = child.Name.LocalName;
                if (local == "element")
                {
                    var elemName   = (string?)child.Attribute("name")      ?? string.Empty;
                    var typeRef    = NormalizeTypeRef((string?)child.Attribute("type") ?? string.Empty);
                    var minOccurs  = (string?)child.Attribute("minOccurs") ?? "1";
                    var maxOccurs  = (string?)child.Attribute("maxOccurs") ?? "1";

                    var def = new XsdElementDef(elemName, typeRef, minOccurs, maxOccurs, enclosingPrefix);

                    // Inline anonymous complexType
                    var inline = child.Elements()
                                      .FirstOrDefault(e => e.Name.LocalName == "complexType");
                    if (inline != null)
                        def.InlineChildren = ParseComplexType(inline, enclosingPrefix);

                    yield return def;
                }
                else if (local == "sequence" || local == "choice" || local == "all")
                {
                    foreach (var nested in ParseGroup(child, enclosingPrefix))
                        yield return nested;
                }
            }
        }

        private static string NormalizeTypeRef(string typeRef)
        {
            if (string.IsNullOrEmpty(typeRef)) return string.Empty;

            // Clark notation {uri}local → prefix:local
            if (typeRef.StartsWith("{"))
            {
                var m = Regex.Match(typeRef, @"^\{([^}]+)\}(.+)$");
                if (m.Success)
                {
                    var ns    = m.Groups[1].Value;
                    var local = m.Groups[2].Value;
                    NsPrefix.TryGetValue(ns, out string? pfx);
                    return pfx != null ? pfx + ":" + local : ns + ":" + local;
                }
            }
            return typeRef;
        }

        // -----------------------------------------------------------------------
        // SCMT (ZUGFeRD 1.0 Schematron) Parsing
        // -----------------------------------------------------------------------

        private void ParseScmt(string filePath)
        {
            var doc = XDocument.Load(filePath);
            foreach (var pattern in doc.Descendants(SchNs + "pattern"))
            {
                foreach (var rule in pattern.Descendants(SchNs + "rule"))
                {
                    var context     = (string?)rule.Attribute("context") ?? string.Empty;
                    var constraints = new List<SchematronConstraint>();

                    foreach (var child in rule.Elements())
                    {
                        var local = child.Name.LocalName;
                        if (local == "assert" || local == "report")
                        {
                            constraints.Add(new SchematronConstraint(
                                local,
                                (string?)child.Attribute("test") ?? string.Empty,
                                child.Value.Trim()));
                        }
                    }

                    if (constraints.Count > 0)
                    {
                        List<SchematronConstraint>? list;
                        if (!_scmtMap.TryGetValue(context, out list))
                            _scmtMap[context] = list = new List<SchematronConstraint>();
                        list.AddRange(constraints);
                    }
                }
            }
        }

        // -----------------------------------------------------------------------
        // EN16931 Schematron Parsing — BT/BG number cross-reference
        // -----------------------------------------------------------------------

        private void ParseEn16931Sch(string filePath)
        {
            var doc = XDocument.Load(filePath);

            // (parentLocal, elemLocal) → BtInfo
            var ctxBts = new Dictionary<string, BtInfo>();

            foreach (var pattern in doc.Descendants(SchNs + "pattern"))
            {
                foreach (var rule in pattern.Descendants(SchNs + "rule"))
                {
                    var context  = (string?)rule.Attribute("context") ?? string.Empty;
                    var ctxLocal = ContextTail(context);

                    foreach (var assert in rule.Elements(SchNs + "assert"))
                    {
                        var test = (string?)assert.Attribute("test") ?? string.Empty;
                        var msg  = BrPrefixRegex.Replace(assert.Value.Trim(), string.Empty);

                        var bts = BtRegex.Matches(msg)
                                         .Cast<Match>()
                                         .Select(m => m.Groups[1].Value)
                                         .Distinct()
                                         .ToList();

                        if (bts.Count == 0) continue;

                        var em = SimpleElemRegex.Match(test);
                        if (!em.Success) continue;

                        var elemLocal = em.Groups[1].Value.Split(':').Last();
                        var key = MakeBtKey(ctxLocal, elemLocal);

                        BtInfo? info;
                        if (!ctxBts.TryGetValue(key, out info))
                        {
                            ctxBts[key] = new BtInfo(
                                new List<string>(bts),
                                msg.Length > 200 ? msg.Substring(0, 200) : msg);
                        }
                        else
                        {
                            foreach (var bt in bts)
                                if (!info.Bts.Contains(bt))
                                    info.Bts.Add(bt);
                        }
                    }
                }
            }

            // Fallback map: keyed on element local name only (for single-context elements)
            var elemBts = new Dictionary<string, BtInfo>();
            foreach (var kvp in ctxBts)
            {
                var parts     = kvp.Key.Split('\0');
                var elemLocal = parts.Length > 1 ? parts[1] : parts[0];
                BtInfo? ei;
                if (!elemBts.TryGetValue(elemLocal, out ei))
                {
                    elemBts[elemLocal] = new BtInfo(
                        new List<string>(kvp.Value.Bts),
                        kvp.Value.Description);
                }
                else
                {
                    foreach (var bt in kvp.Value.Bts)
                        if (!ei.Bts.Contains(bt))
                            ei.Bts.Add(bt);
                }
            }

            foreach (var kvp in ctxBts)
                _btMap[kvp.Key] = kvp.Value;

            foreach (var kvp in elemBts)
                _btMap[MakeBtKey(string.Empty, kvp.Key)] = kvp.Value;
        }

        private static string MakeBtKey(string parentLocal, string elemLocal)
            => parentLocal + "\0" + elemLocal;

        private static string ContextTail(string context)
        {
            var parts = context.Split('/')
                               .Where(p => p.Length > 0 && p.Contains(':'))
                               .ToArray();
            if (parts.Length == 0) return string.Empty;
            var tail = parts[parts.Length - 1];
            var bracketIdx = tail.IndexOf('[');
            if (bracketIdx >= 0) tail = tail.Substring(0, bracketIdx);
            var colonIdx = tail.IndexOf(':');
            return colonIdx >= 0 ? tail.Substring(colonIdx + 1) : tail;
        }

        // -----------------------------------------------------------------------
        // Tree Building
        // -----------------------------------------------------------------------

        private Element BuildTree(
            string typeName,
            string elementLocalName,
            string parentBtLocal,
            string xpath,
            HashSet<string> visited)
        {
            // BT/BG lookup
            var btInfo = LookupBt(parentBtLocal, elementLocalName);

            // SCMT constraints for this XPath
            _scmtMap.TryGetValue(xpath, out List<SchematronConstraint>? constraints);
            constraints = constraints ?? new List<SchematronConstraint>();

            var assertMsgs = constraints
                .Where(c => c.Type == "assert")
                .Select(c => c.Message)
                .ToList();

            var reportMsgs = constraints
                .Where(c => c.Type == "report")
                .Select(c => c.Message)
                .ToList();

            // Derive CII cardinality from assert messages if available
            var ciiCardinality = DeriveCiiCardinality(assertMsgs, reportMsgs);

            // Namespace prefix for the element name
            var nsPrefix = ResolveNsPrefix(typeName);
            var qualifiedName = nsPrefix.Length > 0
                ? nsPrefix + ":" + elementLocalName
                : elementLocalName;

            var element = new Element
            {
                Name         = qualifiedName,
                TypeName     = typeName,
                XPath        = xpath,
                Description  = btInfo.Description,
                BusinessTerm = string.Join(", ",
                    btInfo.Bts.Where(b => b.StartsWith("BT-"))),
                Id           = string.Join(", ",
                    btInfo.Bts.Where(b => b.StartsWith("BG-"))),
                BusinessRule = string.Join("; ", assertMsgs),
                CiiCardinality = ciiCardinality,
                ProfileSupport = DeriveProfileSupport(assertMsgs, reportMsgs),
            };

            if (visited.Contains(typeName))
            {
                element.AdditionalData["note"] = "circular reference – not expanded";
                return element;
            }

            List<XsdElementDef>? childDefs;
            if (!_typeMap.TryGetValue(typeName, out childDefs))
                return element;

            // Derive the parent-local name for BT lookups of children.
            // e.g. "ram:ApplicableTradeTaxType" → "ApplicableTradeTax"
            var typeLocal = typeName.Contains(':')
                ? typeName.Substring(typeName.IndexOf(':') + 1)
                : typeName;
            var btParentLocal = typeLocal.EndsWith("Type")
                ? typeLocal.Substring(0, typeLocal.Length - "Type".Length)
                : typeLocal;

            var newVisited = new HashSet<string>(visited) { typeName };

            foreach (var def in childDefs)
            {
                var childPrefix = def.NsPrefix;
                var childXPath = childPrefix.Length > 0
                    ? xpath + "/" + childPrefix + ":" + def.Name
                    : xpath + "/" + def.Name;

                var child = BuildTree(def.TypeRef, def.Name, btParentLocal, childXPath, newVisited);

                var cardinality = FormatCardinality(def.MinOccurs, def.MaxOccurs);
                child.XsdCardinality = cardinality;
                if (string.IsNullOrEmpty(child.CiiCardinality))
                    child.CiiCardinality = cardinality;

                element.Children.Add(child);
            }

            return element;
        }

        // -----------------------------------------------------------------------
        // Lookup helpers
        // -----------------------------------------------------------------------

        private BtInfo LookupBt(string parentLocal, string elemLocal)
        {
            BtInfo? info;
            if (_btMap.TryGetValue(MakeBtKey(parentLocal, elemLocal), out info))
                return info;
            if (_btMap.TryGetValue(MakeBtKey(string.Empty, elemLocal), out info))
                return info;
            return new BtInfo(new List<string>(), string.Empty);
        }

        /// <summary>
        /// Derives the namespace prefix for an element from the *type* it belongs to.
        /// </summary>
        private static string ResolveNsPrefix(string typeName)
        {
            if (!typeName.Contains(':')) return string.Empty;
            return typeName.Substring(0, typeName.IndexOf(':'));
        }

        private static List<string> DeriveProfileSupport(
            IReadOnlyList<string> assertMsgs,
            IReadOnlyList<string> reportMsgs)
        {
            // A "not used" report means the element is absent from all profiles.
            if (reportMsgs.Any(m => m.IndexOf("not used", StringComparison.OrdinalIgnoreCase) >= 0))
                return new List<string>();

            // For ZUGFeRD 1.0 there is a single SCMT covering all three profiles;
            // elements with assert rules are present in all supported profiles.
            if (assertMsgs.Count > 0)
                return new List<string> { "BASIC", "COMFORT", "EXTENDED" };

            // No Schematron data — element is optional per XSD; include all profiles.
            return new List<string> { "BASIC", "COMFORT", "EXTENDED" };
        }

        private static string DeriveCiiCardinality(
            IReadOnlyList<string> assertMsgs,
            IReadOnlyList<string> reportMsgs)
        {
            // "not used" → effectively 0..0
            if (reportMsgs.Any(m => m.IndexOf("not used", StringComparison.OrdinalIgnoreCase) >= 0))
                return "0..0";

            foreach (var msg in assertMsgs)
            {
                // "must occur exactly N times" → N..N
                var mExact = Regex.Match(msg, @"must occur exactly (\d+) times?", RegexOptions.IgnoreCase);
                if (mExact.Success)
                {
                    var n = mExact.Groups[1].Value;
                    return n + ".." + n;
                }

                // "must occur at least N times" → N..*
                var mMin = Regex.Match(msg, @"must occur at least (\d+) times?", RegexOptions.IgnoreCase);
                if (mMin.Success)
                    return mMin.Groups[1].Value + "..*";

                // "may occur at maximum N times" → 0..N
                var mMax = Regex.Match(msg, @"may occur at maximum (\d+) times?", RegexOptions.IgnoreCase);
                if (mMax.Success)
                    return "0.." + mMax.Groups[1].Value;
            }

            return string.Empty;
        }

        private static string FormatCardinality(string minOccurs, string maxOccurs)
        {
            var maxStr = maxOccurs == "unbounded" ? "*" : maxOccurs;
            return minOccurs + ".." + maxStr;
        }

        // -----------------------------------------------------------------------
        // Private data-holding types
        // -----------------------------------------------------------------------

        private sealed class XsdElementDef
        {
            public string Name { get; }
            public string TypeRef { get; }
            public string MinOccurs { get; }
            public string MaxOccurs { get; }
            /// <summary>Namespace prefix of the schema file that owns the enclosing type.</summary>
            public string NsPrefix { get; }
            public List<XsdElementDef>? InlineChildren { get; set; }

            public XsdElementDef(
                string name, string typeRef,
                string minOccurs, string maxOccurs,
                string nsPrefix)
            {
                Name      = name;
                TypeRef   = typeRef;
                MinOccurs = minOccurs;
                MaxOccurs = maxOccurs;
                NsPrefix  = nsPrefix;
            }
        }

        private sealed class SchematronConstraint
        {
            public string Type    { get; }   // "assert" or "report"
            public string Test    { get; }
            public string Message { get; }

            public SchematronConstraint(string type, string test, string message)
            {
                Type    = type;
                Test    = test;
                Message = message;
            }
        }

        private sealed class BtInfo
        {
            public List<string> Bts         { get; }
            public string       Description { get; }

            public BtInfo(List<string> bts, string description)
            {
                Bts         = bts;
                Description = description;
            }
        }
    }
}
