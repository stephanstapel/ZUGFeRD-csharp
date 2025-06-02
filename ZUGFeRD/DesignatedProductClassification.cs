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
    /// <summary>
    /// Detailed information on the item classification
    /// </summary>
    public class DesignatedProductClassification
    {
        /// <summary>
        /// A code for the classification of an item according to type or kind or nature.
        /// 
        /// Classification codes are used for the aggregation of similar products, which might be useful for various
        /// purposes, for instance like public procurement, in accordance with the Common Vocabulary for Public Procurement
        /// [CPV]), e-Commerce(UNSPSC) etc.
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// Product classification name
        /// </summary>
        public DesignatedProductClassificationClassCodes? ListID { get; set; }

        /// <summary>
        /// Version of product classification
        /// </summary>
        public string ListVersionID { get; set; }

        /// <summary>
        /// Classification name
        /// </summary>
        public string ClassName { get; set; }

    }
}
