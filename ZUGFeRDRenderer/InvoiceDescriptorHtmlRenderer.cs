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
using RazorEngine;
using RazorEngine.Templating;
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
using System.Xml;

namespace s2industries.ZUGFeRDRenderer
{
    public class InvoiceDescriptorHtmlRenderer
    {
        public static string render(InvoiceDescriptor desc)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "s2industries.ZUGFeRDRenderer.test.cshtml";

            string template = "";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                template = reader.ReadToEnd();
            }


            Engine.Razor.AddTemplate(new FullPathTemplateKey("ad", "ad", ResolveType.Global, null),
                                     new LoadedTemplateSource(template));

            string result = Engine.Razor.RunCompile(new FullPathTemplateKey("ad", "ad", ResolveType.Global, null));




            /*
            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteDocType("html", "-//W3C//DTD XHTML 1.0 Transitional//EN", "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", null);
            writer.WriteStartElement("html");
            writer.WriteStartElement("head");
            writer.WriteRaw("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css\" integrity=\"sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7\" crossorigin=\"anonymous\">");
            writer.WriteEndElement(); // !head
            writer.WriteStartElement("body");

            // Absender
            writer.WriteRaw("<div class=\"row\">");
            writer.WriteRaw("<div class=\"col-md-3\">");
            writer.WriteRaw("<table class=\"table table-bordered\">");
            writer.WriteStartElement("tr");
            writer.WriteStartElement("td");
            writer.WriteRaw("<small>Rechnungsersteller</small><br />");
            _writeParty(writer, desc.Seller);
            writer.WriteEndElement(); // !td
            writer.WriteEndElement(); // !tr

            writer.WriteStartElement("tr");
            writer.WriteStartElement("td");
            writer.WriteRaw("<small>Rechnungsempfänger</small><br />");
            _writeParty(writer, desc.Buyer);
            writer.WriteEndElement(); // !td
            writer.WriteEndElement(); // !tr
            writer.WriteRaw("</table>");
            writer.WriteRaw("</div>");
            writer.WriteRaw("</div>");

            // Empfänger
            writer.WriteRaw("<div class=\"row\">");
            writer.WriteRaw("<div class=\"col-md-3\">");
            
            writer.WriteRaw("</div>");
            writer.WriteRaw("</div>");

            // Kopfdaten
            writer.WriteRaw("<div class=\"row\">");
            writer.WriteRaw("<div class=\"col-md-6\">");
            writer.WriteRaw(String.Format("Rechnungsnummer: {0}", desc.InvoiceNo));
            writer.WriteElementString("br", "");
            writer.WriteRaw(String.Format("Rechnungsdatum: {0}", desc.InvoiceDate));
            writer.WriteRaw("</div>");
            writer.WriteRaw("</div>");


            writer.WriteEndElement(); // !body
            writer.WriteEndElement(); // !html
            writer.WriteEndDocument();
            writer.Flush();

            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string retval = sr.ReadToEnd();
            return retval;
            */
            return "";
        } // !render()


        public static void render(InvoiceDescriptor desc, string filename)
        {
            string output = render(desc);
            StreamWriter writer = File.CreateText(filename);
            writer.WriteLine(output);
            writer.Close();
        } // !render()


        private static void _writeParty(XmlTextWriter writer, Party p)
        {
            writer.WriteRaw(p.Name);
            writer.WriteElementString("br", "");
            writer.WriteRaw(p.Street);
            writer.WriteElementString("br", "");
            writer.WriteRaw(String.Format("{0} {1} {2}", p.Country.EnumToString(), p.Postcode, p.City));
            writer.WriteElementString("br", "");
            writer.WriteRaw(String.Format("ID: ", p.ID));
            writer.WriteElementString("br", "");
            writer.WriteRaw(String.Format("GlobalID: ", p.GlobalID.ID));
        } // !_writeParty()
    }
}
