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
using System.Linq.Expressions;
using System.Text;

// UNTID 2475 codes
//
namespace s2industries.ZUGFeRD
{
    public enum DateTypeCodes
    {
        /// <summary>
        /// Unknown means, we have a problem ...
        /// </summary>
        [EnumStringValue("")]
        Unknown = 0,

        /// <summary>
        /// Date of invoice
        /// </summary>
        [EnumStringValue("5")]
        InvoiceDate = 5,

        /// <summary>
        /// Date of delivery of goods to establishments/domicile/site
        /// </summary>
        [EnumStringValue("29")]
        DeliveryDate = 29,

        /// <summary>
        /// Payment date
        /// </summary>
        [EnumStringValue("72")]
        PaymentDate = 72

    }
}
