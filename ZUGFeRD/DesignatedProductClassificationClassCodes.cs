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
    /// A code for the classification of an item according to type or kind or nature.
    ///
    /// Classification codes are used for the aggregation of similar products, which might be useful for various
    /// purposes,
    /// for instance like public procurement, in accordance with the Common Vocabulary for Public Procurement
    /// [CPV]), e-Commerce(UNSPSC) etc.
    ///
    /// Source: UNTDID 7143
    /// Business rule: BR-65
    /// </summary>
    public enum DesignatedProductClassificationClassCodes
    {
        /// <summary>
        /// Product version number
        /// Number assigned by manufacturer or seller to identify the release of a product.
        /// </summary>
        AA,

        /// <summary>
        /// Harmonised system
        /// The item number is part of, or is generated in the context of the Harmonised Commodity Description and Coding System (Harmonised System), as developed and maintained by the World Customs Organization (WCO).
        /// </summary>
        HS,

        /// <summary>
        ///  In bond number
        /// </summary>
        IB,

        /// <summary>
        /// Mutually defined
        /// Item type identification mutually agreed between interchanging parties.
        /// </summary>
        ZZZ
    }
}
