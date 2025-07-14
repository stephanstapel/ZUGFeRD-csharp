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
    public sealed class InvoiceOptionsBuilder
    {
        private readonly InvoiceFormatOptions _options;

        private InvoiceOptionsBuilder()
        {
            _options = new InvoiceFormatOptions();
        } // !InvoiceOptionsBuilder()


        private InvoiceOptionsBuilder(InvoiceFormatOptions baseOptions)
        {
            _options = baseOptions?.Clone() ?? throw new ArgumentNullException(nameof(baseOptions));
        } // !InvoiceOptionsBuilder()


        public static InvoiceOptionsBuilder Create()
        {
            return new InvoiceOptionsBuilder();
        } // !Create()


        public static InvoiceOptionsBuilder From(InvoiceFormatOptions options)
        {
            return new InvoiceOptionsBuilder(options);
        } // !From()


        public static InvoiceOptionsBuilder CreateDefault()
        {
            return Create().UseRecommendedDefaults();
        } // !CreateDefault()


        public InvoiceFormatOptions Build()
        {
            return _options;
        } // !Build()


        public InvoiceOptionsBuilder EnableXmlComments(bool enable = true)
        {
            _options.IncludeXmlComments = enable;
            return this;
        } // !EnableXmlComments()


        public InvoiceOptionsBuilder AddHeaderXmlComment(List<string> comments)
        {
            _options.XmlHeaderComments.AddRange(comments);
            return this;
        } // !AddHeaderXmlComment()


        public InvoiceOptionsBuilder AddHeaderXmlComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return this;
            }
            _options.XmlHeaderComments.Add(comment);
            return this;
        } // !AddHeaderXmlComment()


        public InvoiceOptionsBuilder UseRecommendedDefaults()
        {
            return EnableXmlComments(false);
        } // !UseRecommendedDefaults()


        public InvoiceOptionsBuilder AutomaticallyCleanInvalidCharacters()
        {
            _options.AutomaticallyCleanInvalidCharacters = true;
            return this;
        } // !AutomaticallyCleanInvalidCharacters()
    }
}
