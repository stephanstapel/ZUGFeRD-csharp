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
using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace s2industries.ZUGFeRD.PDF
{
    /// <summary>
    /// Cross-platform font resolver that uses installed system fonts
    /// discovered by <see cref="FontInfoProvider"/>.
    /// </summary>
    internal class SystemFontResolver : IFontResolver
    {
        private readonly Dictionary<string, InstalledFont> _fontsByName;

        internal SystemFontResolver()
        {
            List<InstalledFont> installedFonts = FontInfoProvider.GetInstalledFonts();
            _fontsByName = new Dictionary<string, InstalledFont>(StringComparer.OrdinalIgnoreCase);
            foreach (InstalledFont font in installedFonts)
            {
                // first entry wins for duplicate family names
                if (!_fontsByName.ContainsKey(font.FamilyName))
                {
                    _fontsByName[font.FamilyName] = font;
                }
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Try exact match first, then build style-specific candidates
            string[] candidates = _BuildCandidates(familyName, isBold, isItalic);

            foreach (string candidate in candidates)
            {
                if (_fontsByName.TryGetValue(candidate, out InstalledFont font))
                {
                    return new FontResolverInfo(font.FilePath, isBold, isItalic);
                }
            }

            // fallback: return the base family without style suffix
            if (_fontsByName.TryGetValue(familyName, out InstalledFont fallback))
            {
                return new FontResolverInfo(fallback.FilePath, isBold, isItalic);
            }

            return null;
        }

        public byte[] GetFont(string faceName)
        {
            // faceName is the file path set in ResolveTypeface
            if (File.Exists(faceName))
            {
                return File.ReadAllBytes(faceName);
            }

            return null;
        }

        private static string[] _BuildCandidates(string familyName, bool isBold, bool isItalic)
        {
            if (isBold && isItalic)
            {
                return new[]
                {
                    familyName + " Bold Italic",
                    familyName + " Bold Oblique",
                    familyName + " BoldItalic",
                    familyName + " Bold",
                    familyName
                };
            }

            if (isBold)
            {
                return new[]
                {
                    familyName + " Bold",
                    familyName
                };
            }

            if (isItalic)
            {
                return new[]
                {
                    familyName + " Italic",
                    familyName + " Oblique",
                    familyName
                };
            }

            return new[] { familyName };
        }
    }
}
