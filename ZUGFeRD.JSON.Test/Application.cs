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
using System.IO;

namespace s2industries.ZUGFeRD.JSON.Test
{
    internal class Application
    {
        internal void Run(Options options)
        {
            if (String.IsNullOrEmpty(options.InputFile))
            {
                Console.WriteLine("Error: No input file specified. Use -i to specify an input file.");
                return;
            }

            if (options.InputFile.Contains("*"))
            {
                string baseDirectory = Path.GetDirectoryName(options.InputFile) ?? ".";
                string searchPattern = Path.GetFileName(options.InputFile);
                foreach (string inputPath in Directory.GetFiles(baseDirectory, searchPattern, options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                {
                    string outputPath = Path.Combine(Path.GetDirectoryName(inputPath) ?? ".", String.Format("{0}.{1}", Path.GetFileNameWithoutExtension(inputPath), "json"));
                    InvoiceConverter.ToJson(inputPath, outputPath);
                }
            }
            else
            {
                string? outputFile = options.OutputFile;
                if (String.IsNullOrEmpty(outputFile))
                {
                    outputFile = Path.Combine(Path.GetDirectoryName(options.InputFile) ?? ".", String.Format("{0}.{1}", Path.GetFileNameWithoutExtension(options.InputFile), "json"));
                }

                InvoiceConverter.ToJson(options.InputFile, outputFile);
            }
        }
    }
}
