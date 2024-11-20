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
using s2industries.ZUGFeRD;
using System;
using System.IO;

namespace s2industries.ZUGFeRD.Excel.Test
{
    internal class Application
    {
        internal void Run(Options options)
        {            
            if (options.InputFile.Contains("*"))
            {
                string baseDirectory = System.IO.Path.GetDirectoryName(options.InputFile);
                string searchPattern = System.IO.Path.GetFileName(options.InputFile);
                foreach (string _inputPath in System.IO.Directory.GetFiles(baseDirectory, searchPattern, options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                {
                    string _outputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_inputPath), String.Format("{0}.{1}", System.IO.Path.GetFileNameWithoutExtension(_inputPath), "xlsx"));
                    InvoiceConverter.ToExcel(_inputPath, _outputPath);
                }
            }
            else
            {
                string _outputFile = options.OutputFile;
                if (String.IsNullOrEmpty(_outputFile))
                {
                    _outputFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(options.InputFile), String.Format("{0}.{1}", System.IO.Path.GetFileNameWithoutExtension(options.InputFile), "xlsx"));
                }

                InvoiceConverter.ToExcel(options.InputFile, _outputFile);
            }
        }        
    }
}
