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
using s2industries.ZUGFeRD.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD.Render.Demo
{
    internal class Application
    {
        internal async Task RunAsync()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.Load("../../../../demodata/zugferd22/zugferd_2p2_EXTENDED_Fremdwaehrung-factur-x.xml");
            desc.GetTradeLineItems().First().AddDesignatedProductClassification(DesignatedProductClassificationClassCodes.ZZZ, "Version Id", "Class Code", "Class Name");
            string html = await InvoiceDescriptorHtmlRenderer.RenderAsync(desc);
            System.IO.File.WriteAllText("output.html", html);
        } // !RunAsync()
    }
}
