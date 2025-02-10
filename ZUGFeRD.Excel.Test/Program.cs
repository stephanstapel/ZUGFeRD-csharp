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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Options;

namespace s2industries.ZUGFeRD.Excel.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Options options = new Options();

            OptionSet set = null;
            try
            {
                set = new OptionSet
                {
                    { "?|help|h", "Outputs help", m => options.Help = true },
                    { "i|inputfile=", "ZUGFeRD input file. Might also be a file pattern", i => options.InputFile = i },
                    { "o|outputfile=", "Excel output file", o => options.OutputFile = o },
                    { "r|recursive", "In case you pass a pattern as inputfile, recursive determines if sub directories are recursively iterated", c => options.Recursive = true },
                };
            }
            catch (FormatException)
            {
                System.Console.WriteLine("Error parsing arguments.");
                Environment.Exit(-1);
            }

            try
            {
                set.Parse(args);
            }
            catch (OptionException)
            {
                set.WriteOptionDescriptions(Console.Out);
            }


            if (options.Help)
            {
                set.WriteOptionDescriptions(Console.Out);
                Environment.Exit(0);
            }

            Application app = new Application();
            app.Run(options);
        }
    }
}
