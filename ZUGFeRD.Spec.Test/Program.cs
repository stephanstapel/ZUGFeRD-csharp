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
using Mono.Options;
using s2industries.ZUGFeRD.Spec;

namespace s2industries.ZUGFeRD.Spec.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string? xrechnungDocPath = null;
            string? xsdPath = null;
            string? outputJson = null;
            string version = "3.0.1";
            string syntaxStr = "cii";
            bool showHelp = false;
            bool verbose = false;

            OptionSet? options = null;
            try
            {
                options = new OptionSet
                {
                    { "?|help|h", "Print help and exit", _ => showHelp = true },
                    { "x|xrechnung=", "Path to the XRechnung version folder\n(e.g. documentation/xRechnung/XRechnung 3.0.1)", v => xrechnungDocPath = v },
                    { "s|schema=", "Path to the XSD directory.\n" +
                                   "  CII  : EN16931 FACTUR-X XSD directory\n" +
                                   "         (e.g. documentation/zugferd211en/Schema/EN16931)\n" +
                                   "  UBL  : OASIS UBL 2.1 XSD directory containing\n" +
                                   "         UBL-Invoice-2.1.xsd / UBL-CreditNote-2.1.xsd.\n" +
                                   "         Download: http://docs.oasis-open.org/ubl/os-UBL-2.1/UBL-2.1.zip\n" +
                                   "         (use the xsd/maindoc or xsdrt/maindoc subfolder)", v => xsdPath = v },
                    { "o|output=", "Output JSON file path", v => outputJson = v },
                    { "ver|version=", "XRechnung version label (default: 3.0.1)", v => version = v },
                    { "syntax=", "Syntax binding to extract:\n" +
                                 "  cii               : CII (ZUGFeRD/Factur-X, default)\n" +
                                 "  ubl | ubl-invoice : UBL 2.1 Invoice (XRechnung UBL)\n" +
                                 "  ubl-creditnote    : UBL 2.1 CreditNote", v => syntaxStr = v?.Trim().ToLowerInvariant() ?? "cii" },
                    { "v|verbose", "Print extracted statistics", _ => verbose = true },
                };
            }
            catch (FormatException)
            {
                Console.Error.WriteLine("Error parsing arguments.");
                Environment.Exit(1);
            }

            try
            {
                options!.Parse(args);
            }
            catch (OptionException ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                options?.WriteOptionDescriptions(Console.Error);
                Environment.Exit(1);
            }

            if (showHelp)
            {
                Console.WriteLine("XRechnung Specification Extractor");
                Console.WriteLine("Extracts element hierarchy, cardinalities, and business rules");
                Console.WriteLine("from XRechnung schematron and schema (CII XSD or OASIS UBL 2.1 XSD) files.");
                Console.WriteLine();
                options!.WriteOptionDescriptions(Console.Out);
                return;
            }

            if (string.IsNullOrEmpty(xrechnungDocPath))
            {
                Console.Error.WriteLine("Error: -x/--xrechnung is required.");
                options!.WriteOptionDescriptions(Console.Error);
                Environment.Exit(1);
            }

            if (string.IsNullOrEmpty(xsdPath))
            {
                Console.Error.WriteLine("Error: -s/--schema is required.");
                options!.WriteOptionDescriptions(Console.Error);
                Environment.Exit(1);
            }

            // Parse syntax binding
            SyntaxBinding syntax;
            switch (syntaxStr)
            {
                case "ubl":
                case "ubl-invoice":
                    syntax = SyntaxBinding.UblInvoice;
                    break;
                case "ubl-creditnote":
                case "ubl-credit-note":
                    syntax = SyntaxBinding.UblCreditNote;
                    break;
                default:
                    syntax = SyntaxBinding.Cii;
                    break;
            }

            if (string.IsNullOrEmpty(outputJson))
            {
                string syntaxSuffix = syntax == SyntaxBinding.Cii ? "cii" :
                                      syntax == SyntaxBinding.UblCreditNote ? "ubl-cn" : "ubl";
                outputJson = Path.Combine(
                    Path.GetDirectoryName(xrechnungDocPath) ?? ".",
                    $"xrechnung-{version}-{syntaxSuffix}-spec.json");
            }

            Console.WriteLine($"Extracting XRechnung {version} specification ({syntax})...");
            Console.WriteLine($"  XRechnung docs : {xrechnungDocPath}");
            Console.WriteLine($"  XSD path       : {xsdPath}");
            Console.WriteLine($"  Syntax         : {syntax}");
            Console.WriteLine($"  Output         : {outputJson}");

            SpecExtractor extractor = new SpecExtractor();
            XRechnungSpec spec;
            try
            {
                spec = extractor.Extract(xrechnungDocPath!, xsdPath!, syntax, version);
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine($"\nError: Schema file not found: {ex.FileName}");
                if (syntax == SyntaxBinding.UblInvoice || syntax == SyntaxBinding.UblCreditNote)
                {
                    string rootFile = syntax == SyntaxBinding.UblCreditNote
                        ? "UBL-CreditNote-2.1.xsd"
                        : "UBL-Invoice-2.1.xsd";
                    Console.Error.WriteLine($"\nThe OASIS UBL 2.1 XSD files are not included in this repository.");
                    Console.Error.WriteLine($"Please download the UBL 2.1 package and extract it:");
                    Console.Error.WriteLine($"  URL : http://docs.oasis-open.org/ubl/os-UBL-2.1/UBL-2.1.zip");
                    Console.Error.WriteLine($"Then point --schema to the directory containing '{rootFile}'");
                    Console.Error.WriteLine($"  (usually the 'xsd/maindoc' or 'xsdrt/maindoc' subfolder)");
                }
                Environment.Exit(2);
                return;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.Error.WriteLine($"\nError: XSD directory not found: {ex.Message}");
                Environment.Exit(2);
                return;
            }

            if (verbose)
            {
                List<SpecElement> flat = extractor.FlattenElements(spec);
                Console.WriteLine($"\nExtracted:");
                Console.WriteLine($"  Elements (total flattened) : {flat.Count}");
                Console.WriteLine($"  Rules                      : {spec.Rules.Count}");

                int fatalRules = 0, warningRules = 0;
                Dictionary<string, int> rulesByPattern = new Dictionary<string, int>(StringComparer.Ordinal);
                foreach (SpecRule r in spec.Rules)
                {
                    if (r.Flag == "fatal") fatalRules++;
                    else if (r.Flag == "warning") warningRules++;
                    rulesByPattern[r.Pattern] = rulesByPattern.TryGetValue(r.Pattern, out int c) ? c + 1 : 1;
                }
                Console.WriteLine($"    Fatal (MUST) rules       : {fatalRules}");
                Console.WriteLine($"    Warning (SHOULD) rules   : {warningRules}");
                Console.WriteLine();
                Console.WriteLine("  Rules per pattern:");
                foreach (KeyValuePair<string, int> kv in rulesByPattern)
                {
                    Console.WriteLine($"    {kv.Key,-45} : {kv.Value}");
                }
            }

            extractor.WriteJson(spec, outputJson);
            Console.WriteLine($"\nSpec written to: {outputJson}");
        }
    }
}
