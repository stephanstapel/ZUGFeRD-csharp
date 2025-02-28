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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Scriban;

namespace s2industries.ZUGFeRD.Render
{
    public class InvoiceDescriptorHtmlRenderer
    {
        public static async Task<string> RenderAsync(InvoiceDescriptor invoice)
        {
            string templateContent = _LoadEmbeddedResource("Simple.scriban");
            var template = Template.Parse(templateContent);
            var model = new
            {
                Invoice = invoice
            };

            string result = await template.RenderAsync(model, memberRenamer: member => member.Name);
            return result;
        } // !RenderAsync()


        private static string _LoadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = assembly.GetManifestResourceNames()
                                       .FirstOrDefault(r => r.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

            if (resourcePath == null)
            {
                throw new FileNotFoundException($"The embedded resource '{resourceName}' was not found.");
            }

            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        } // !_LoadEmbeddedResource()
    }
}
