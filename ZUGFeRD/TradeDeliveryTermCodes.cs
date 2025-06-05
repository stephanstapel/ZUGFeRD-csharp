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

using System.ComponentModel;


namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Delivery term codes according to UNTDID 4053 + INCOTERMS code lists
	/// Lieferbedingung (Code), BT-X-145
    /// </summary>
	public enum TradeDeliveryTermCodes
	{
		/// <summary>
		/// Lieferung durch den Lieferanten organisiert
		/// Delivery arranged by the supplier
		///
		/// Indicates that the supplier will arrange delivery of the goods.
		/// </summary>
		[EnumStringValue("1")]
		_1,

		/// <summary>
		/// Lieferung durch Logistikdienstleister organisiert
		/// Delivery arranged by logistic service provider
		///
		/// Code indicating that the logistic service provider has
		/// arranged the delivery of goods.
		/// </summary>
		[EnumStringValue("2")]
		_2,

        /// <summary>
        /// CFR
        /// Kosten und Fracht
        /// </summary>
        [EnumStringValue("CFR")]
        CFR,

        /// <summary>
        /// CIF
        /// Kosten, Versicherung und Fracht
        /// </summary>
        [EnumStringValue("CIF")]
        CIF,

        /// <summary>
        /// CIP
        /// Transport und Versicherung bezahlt nach (benannten Bestimmungsort einfügen)
        /// </summary>
        [EnumStringValue("CIP")]
        CIP,

        /// <summary>
        /// CPT
        /// Frachtfrei nach (benannten Bestimmungsort einfügen)
        /// </summary>
        [EnumStringValue("CPT")]
        CPT,

        /// <summary>
        /// DAP
        /// Geliefert am Ort (benannten Bestimmungsort einfügen)
        /// </summary>
        [EnumStringValue("DAP")]
        DAP,

        /// <summary>
        /// DDP
        /// Geliefert verzollt (benannten Bestimmungsort einfügen)
        /// </summary>
        [EnumStringValue("DDP")]
        DDP,

        /// <summary>
        /// DPU
        /// Geliefert am Ort der Entladung (benannten Ort der Entladung einfügen)
        /// </summary>
        [EnumStringValue("DPU")]
        DPU,

        /// <summary>
        /// EXW
        /// Ab Werk (benannten Ort der Lieferung einfügen)
        /// </summary>
        [EnumStringValue("EXW")]
        EXW,

        /// <summary>
        /// FAS
        /// Frei Längsseite Schiff (benannten Verschiffungshafen einfügen)
        /// </summary>
        [EnumStringValue("FAS")]
        FAS,

        /// <summary>
        /// FCA
        /// Frei Frachtführer (benannten Ort der Zustellung einfügen)
        /// </summary>
        [EnumStringValue("FCA")]
        FCA,

        /// <summary>
        /// FOB
        /// Frei an Bord (benannten Verschiffungshafen einfügen)
        /// </summary>
        [Description("FOB")]
        FOB
	}
}
