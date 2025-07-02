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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD.PDF
{
    public static class FontInfoProvider
    {
        private const int MacOsPlatformId = 1;
        private const int MacRomanEncodingId = 0;
        private const int Ucs4EncodingId = 10;
        private const int UnicodePlatformId = 0;
        private const int Utf16BigEndianEncodingId = 1;
        private const int WindowsPlatformId = 3;

        private static readonly Encoding _iso88591;
        private static readonly Encoding _macRoman;
        private static readonly Encoding _utf16Be = Encoding.BigEndianUnicode;

        static FontInfoProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _iso88591 = Encoding.GetEncoding("ISO-8859-1");
            _macRoman = Encoding.GetEncoding("macintosh");
        }

        internal static List<InstalledFont> GetInstalledFonts()
        {
            IEnumerable<string> fontDirectories = GetDefaultFontDirectories()
                .Where(Directory.Exists);

            List<string> fontPaths = fontDirectories
                .AsParallel()
                .SelectMany(directory =>
                    Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                        .Where(file =>
                            file.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) ||
                            file.EndsWith(".otf", StringComparison.OrdinalIgnoreCase)))
                .Distinct()
                .ToList();

            ConcurrentBag<InstalledFont> installedFonts =
                new ConcurrentBag<InstalledFont>();

            Parallel.ForEach(fontPaths, fontPath =>
            {
                string fontName = TryGetFullFontName(fontPath);

                if (!string.IsNullOrEmpty(fontName))
                {
                    installedFonts.Add(new InstalledFont
                    {
                        FamilyName = fontName,
                        FilePath = fontPath
                    });
                }
            });

            return installedFonts.ToList();
        }

        internal static IEnumerable<string> GetDefaultFontDirectories()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string fontDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

                if (!string.IsNullOrEmpty(fontDirectory))
                {
                    yield return fontDirectory;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                yield return "/System/Library/Fonts";
                yield return "/Library/Fonts";

                string fontDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Library", "Fonts");

                yield return fontDirectory;
            }
            else
            {
                yield return "/usr/share/fonts";
                yield return "/usr/local/share/fonts";

                string fontDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    ".fonts");

                yield return fontDirectory;
            }
        }

        private static Encoding GetEncoding(ushort platformId, ushort encodingId)
        {
            switch (platformId)
            {
                case UnicodePlatformId:
                case WindowsPlatformId when encodingId == Utf16BigEndianEncodingId || encodingId == Ucs4EncodingId:
                    return _utf16Be;

                case MacOsPlatformId when encodingId == MacRomanEncodingId:
                    return _macRoman;

                default:

                    return _iso88591;
            }
        }

        private static ushort ReadUInt16BigEndian(BinaryReader reader)
        {
            byte[] b = reader.ReadBytes(2);

            return (ushort) ((b[0] << 8) | b[1]);
        }

        private static uint ReadUInt32BigEndian(BinaryReader reader)
        {
            byte[] b = reader.ReadBytes(4);

            return (uint) ((b[0] << 24) | (b[1] << 16) | (b[2] << 8) | b[3]);
        }

        private static string TryGetFullFontName(string path)
        {
            if (!File.Exists(path))
            {
                return string.Empty;
            }

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    //	Header
                    //	------
                    //	Bytes: 
                    //  0-3: sfntVersion
                    //  4-5: numberOfTables
                    //  6-7: searchRange
                    //  8-9: entrySelector
                    //  10-11: rangeShift
                    //
                    //	TableRecords
                    //	------------
                    //	Bytes:
                    //	12-n: tableRecord n (16 Bytes)
                    //

                    const int numberOfTablesOffset = 4;
                    const int tableRecordsOffset = 12;
                    const int tableRecordEntryLength = 4;
                    const string nameTag = "name";

                    if (fileStream.Length < 12)
                    {
                        return string.Empty;
                    }

                    fileStream.Seek(numberOfTablesOffset, SeekOrigin.Begin);

                    ushort numberOfTables = ReadUInt16BigEndian(binaryReader);

                    fileStream.Seek(tableRecordsOffset, SeekOrigin.Begin);
                    int nameTableRecordOffset = -1;
                    int nameTableRecordLength = 0;

                    for (int i = 0; i < numberOfTables; i++)
                    {
                        //	TableRecord
                        //	-----------
                        //	Bytes:
                        //  0-3: tag
                        //	4-7: checksum
                        //	8-11: offset
                        //	12-15: length

                        string tag = Encoding.ASCII.GetString(binaryReader.ReadBytes(tableRecordEntryLength));
                        binaryReader.ReadUInt32(); // checksum
                        uint offset = ReadUInt32BigEndian(binaryReader);
                        uint length = ReadUInt32BigEndian(binaryReader);

                        if (tag != nameTag)
                        {
                            continue;
                        }

                        nameTableRecordOffset = (int) offset;
                        nameTableRecordLength = (int) length;

                        break;
                    }

                    if (nameTableRecordOffset < 0 || nameTableRecordLength <= 0)
                    {
                        return string.Empty;
                    }

                    //	NameTableRecord
                    //	---------------
                    //	Bytes:
                    //	0-1: format
                    //	2-3: count
                    //	4-5: stringOffset
                    //	6-n: count * NameRecord
                    //
                    //	NameRecord
                    //	----------
                    //	0-1: platformId
                    //	2-3: encodingId
                    //	4-5: languageId
                    //	6-7: nameId
                    //	8-9: length
                    //	10-11: offset

                    const int nameTableRecordCountOffset = 2;

                    fileStream.Seek(nameTableRecordOffset + nameTableRecordCountOffset, SeekOrigin.Begin);

                    ushort count = ReadUInt16BigEndian(binaryReader);
                    ushort stringOffset = ReadUInt16BigEndian(binaryReader);

                    for (int i = 0; i < count; i++)
                    {
                        const int fullFontNameId = 4;

                        ushort platformId = ReadUInt16BigEndian(binaryReader);
                        ushort encodingId = ReadUInt16BigEndian(binaryReader);
                        binaryReader.BaseStream.Seek(2, SeekOrigin.Current); // languageId
                        ushort nameId = ReadUInt16BigEndian(binaryReader);
                        ushort length = ReadUInt16BigEndian(binaryReader);
                        ushort offset = ReadUInt16BigEndian(binaryReader);

                        if (nameId != fullFontNameId)
                        {
                            continue;
                        }

                        long absolutePos = nameTableRecordOffset + stringOffset + offset;

                        if (absolutePos + length > fileStream.Length)
                        {
                            continue;
                        }

                        fileStream.Seek(absolutePos, SeekOrigin.Begin);
                        byte[] bytes = binaryReader.ReadBytes(length);

                        Encoding encoding = GetEncoding(platformId, encodingId);

                        return encoding.GetString(bytes);
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }
    }
}
