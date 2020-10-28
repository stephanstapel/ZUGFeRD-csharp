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

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// An aggregation of business terms to disclose free text which is invoice-relevant, as well as their qualification.
    /// </summary>
    public class Note
    {
        /// <summary>
        /// A free text containing unstructured information which is relevant for the invoice as a whole.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The qualification of the free text of an invoice of BT-22
        /// </summary>
        public SubjectCodes SubjectCode { get; set; } = SubjectCodes.Unknown;

        /// <summary>
        /// Bilaterally agreed text blocks which, here, are transferred as code.
        /// </summary>
        public ContentCodes ContentCode { get; set; } = ContentCodes.Unknown;

        /// <summary>
        /// Initialize a new node
        /// </summary>
        /// <param name="content"></param>
        /// <param name="subjectCode"></param>
        /// <param name="contentCode"></param>
        public Note(string content, SubjectCodes subjectCode, ContentCodes contentCode)
        {
            this.Content = content;
            this.SubjectCode = subjectCode;
            this.ContentCode = contentCode;
        }
    }
}
