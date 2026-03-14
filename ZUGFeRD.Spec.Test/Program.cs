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
            string? en16931XsdPath = null;
            string? outputJson = null;
            string version = "3.0.1";
            bool showHelp = false;
            bool verbose = false;

            OptionSet? options = null;
            try
            {
                options = new OptionSet
                {
                    { "?|help|h", "Print help and exit", _ => showHelp = true },
                    { "x|xrechnung=", "Path to the XRechnung version folder\n(e.g. documentation/xRechnung/XRechnung 3.0.1)", v => xrechnungDocPath = v },
                    { "s|schema=", "Path to the EN16931 CII XSD directory\n(e.g. documentation/zugferd211en/Schema/EN16931)", v => en16931XsdPath = v },
                    { "o|output=", "Output JSON file path", v => outputJson = v },
                    { "ver|version=", "XRechnung version label (default: 3.0.1)", v => version = v },
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
                Console.WriteLine("from XRechnung schematron and EN16931 CII XSD files.");
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

            if (string.IsNullOrEmpty(en16931XsdPath))
            {
                Console.Error.WriteLine("Error: -s/--schema is required.");
                options!.WriteOptionDescriptions(Console.Error);
                Environment.Exit(1);
            }

            if (string.IsNullOrEmpty(outputJson))
            {
                outputJson = Path.Combine(
                    Path.GetDirectoryName(xrechnungDocPath) ?? ".",
                    $"xrechnung-{version}-spec.json");
            }

            Console.WriteLine($"Extracting XRechnung {version} specification...");
            Console.WriteLine($"  XRechnung docs : {xrechnungDocPath}");
            Console.WriteLine($"  EN16931 XSDs   : {en16931XsdPath}");
            Console.WriteLine($"  Output         : {outputJson}");

            SpecExtractor extractor = new SpecExtractor();
            XRechnungSpec spec = extractor.Extract(xrechnungDocPath, en16931XsdPath, version);

            if (verbose)
            {
                List<SpecElement> flat = extractor.FlattenElements(spec);
                Console.WriteLine($"\nExtracted:");
                Console.WriteLine($"  Elements (total flattened) : {flat.Count}");
                Console.WriteLine($"  Rules (CII + UBL)          : {spec.Rules.Count}");

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
