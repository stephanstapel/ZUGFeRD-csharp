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
using System.Text;

namespace s2industries.ZUGFeRD
{
    public sealed class InvoiceFormatOptions
    {
        public List<string> XmlHeaderComments { get; set; } = new List<string>();
        public bool IncludeXmlComments { get; internal set; } = false;
        public bool AutomaticallyCleanInvalidCharacters { get; internal set; } = false;


        internal InvoiceFormatOptions Clone()
        {
            return new InvoiceFormatOptions
            {
                XmlHeaderComments = new List<string>(this.XmlHeaderComments),
                IncludeXmlComments = this.IncludeXmlComments,
                AutomaticallyCleanInvalidCharacters = this.AutomaticallyCleanInvalidCharacters
            };
        } // !Clone()
    }
}
