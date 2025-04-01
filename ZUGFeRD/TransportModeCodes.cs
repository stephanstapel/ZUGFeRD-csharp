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

namespace s2industries.ZUGFeRD
{
    /// <summary>
    ///     Eine Konsignation auf Kopfebene, die mit dieser Handelslieferung zusammenhängt.
    ///     A logistics transport movement specified for this supply chain consignment.
    ///     BT-X-152
    /// </summary>
    public enum TransportModeCodes
    {
        /// <summary>
        /// Transport mode not specified
        /// </summary>
        [EnumStringValue("0")]
        TransportModeNotSpecified,

        /// <summary>
        /// Maritime transport
        /// </summary>
        [EnumStringValue("1")]
        Maritime,

        /// <summary>
        /// Rail Transport
        /// </summary>
        [EnumStringValue("2")]
        Rail,

        /// <summary>
        /// Road Transport
        /// </summary>
        [EnumStringValue("3")]
        Road,

        /// <summary>
        /// Air Transport
        /// </summary>
        [EnumStringValue("4")]
        Air,

        /// <summary>
        /// Via Mail
        /// </summary>
        [EnumStringValue("5")]
        Mail,

        /// <summary>
        /// Multimodal transport
        /// </summary>
        [EnumStringValue("6")]
        MultiMode,

        /// <summary>
        /// Fixed transport installation
        /// </summary>
        [EnumStringValue("7")]
        FixedTransport,

        /// <summary>
        /// Inland water transport
        /// </summary>
        [EnumStringValue("8")]
        InlandWater,

        /// <summary>
        /// Transport mode not applicable
        /// </summary>
        [EnumStringValue("9")]
        NotApplicable
    }
}
