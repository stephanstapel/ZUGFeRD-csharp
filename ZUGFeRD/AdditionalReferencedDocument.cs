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
    /// Reference documents are supposed to hold additional data you might want to show on item level.
    ///
    /// Reference documents are used e.g. for commissions on item level
    /// </summary>
    public class AdditionalReferencedDocument : BaseReferencedDocument
    {
        /// <summary>
        /// External document location
        /// BT-124, BT-X-28
        /// </summary>
        public string URIID { get; set; }

        /// <summary>
        /// Referenced position
        /// BT-X-29
        /// </summary>
        public string LineID { get; set; }

        /// <summary>
        /// Type of the reference document
        /// BT-17-0, BT-18-0, BT-122-0, BT-X-30
        /// </summary>
        public AdditionalReferencedDocumentTypeCode? TypeCode { get; set; }

        /// <summary>
        /// Reference documents are strongly typed, specify ReferenceTypeCode to allow easy processing by invoicee
        /// BT-18-1
        /// </summary>
        public ReferenceTypeCodes? ReferenceTypeCode { get; set; }

        /// <summary>
        /// Description of document
        /// BT-123, BT-X-299
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An attached document embedded as binary object or sent together with the invoice.
        /// BT-125
        /// </summary>
        public byte[] AttachmentBinaryObject { get; set; } = null;

        /// <summary>
        /// Filename of attachment
        /// BT-125-2
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// MimeType of the attached document embedded as binary object.
        /// BT-125-1
        /// </summary>
        public string MimeType => MimeTypeMapper.GetMimeType(Filename);
    }
}
