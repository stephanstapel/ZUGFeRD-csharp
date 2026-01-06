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
    /// An included Item referenced from this trade product.
    /// </summary>
    public class IncludedReferencedProduct
    {
        /// <summary>
        /// Name of Included Item
        /// 
        /// BT-X-18
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Included quantity
        /// 
        /// BT-X-20
        /// </summary>
        public decimal? UnitQuantity { get; set; }

        /// <summary>
        /// Item Base Quantity Unit Code
        /// 
        /// BT-X-20-1
        /// </summary>
        public QuantityCodes? UnitCode { get; set; }

        /// <summary>
        /// The identification of articles based on a registered scheme
        ///
        /// GlobalID of Included Item
        /// </summary>
        public GlobalID GlobalID { get; set; } = new GlobalID();

        /// <summary>
        /// Included Item Seller's identifier
        /// </summary>
        public string SellerAssignedID { get; set; }

        /// <summary>
        /// Included Item Buyer's identifier
        /// </summary>
        public string BuyerAssignedID { get; set; }

        /// <summary>
        /// Industry AssignedID of Included Item
        /// </summary>
        public string IndustryAssignedID { get; set; }

        /// <summary>
        /// Description of Included Item
        /// </summary>
        public string Description { get; set; }

    }
}
