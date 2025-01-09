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
using RazorLight;
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace s2industries.ZUGFeRD.Render
{
    public class InvoiceDescriptorHtmlRenderer
    {
        public static string Render(InvoiceDescriptor desc)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = typeof(InvoiceDescriptorHtmlRenderer).Namespace + ".test.cshtml";

            string templateText = "";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                templateText = reader.ReadToEnd();
            }

            var engine = new RazorLightEngineBuilder()
                .UseOptions(new RazorLightOptions()
                {
                    EnableDebugMode = true
                })
                .SetOperatingAssembly(typeof(InvoiceDescriptorHtmlRenderer).GetTypeInfo().Assembly)
                .Build();
            Task<string> resultTask = engine.CompileRenderAsync("test", desc);
            resultTask.Wait();


            return resultTask.Result;
        } // !Render()


        public static void Render(InvoiceDescriptor desc, string filename)
        {
            string output = Render(desc);
            StreamWriter writer = File.CreateText(filename);
            writer.WriteLine(output);
            writer.Close();
        } // !Render()
    }
}
