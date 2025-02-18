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
    /// Reason codes according to UN/EDIFACT UNTDID 7161 code list
    /// </summary>
	public enum AllowanceReasonCodes
	{
		/// <summary>
		/// Unknown
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Advertising
		/// </summary>
		[Description("AA")]
		Advertising = 1,

		/// <summary>
		/// Off-premises discount
		/// </summary>
		[Description("ABL")]
		OffPremisesDiscount = 2,

		/// <summary>
		/// Customer discount
		/// </summary>
		[Description("ADR")]
		CustomerDiscount = 3,

		/// <summary>
		/// Damaged goods
		/// </summary>
		[Description("ADT")]
		DamagedGoods = 4,

		/// <summary>
		/// Early payment allowance
		/// </summary>
		[Description("FC")]
		EarlyPaymentAllowance = 66,

		/// <summary>
		/// Discount
		/// </summary>
		[Description("95")]
		Discount = 95,

		/// <summary>
		/// Volume discount
		/// </summary>
		[Description("100")]
		VolumeDiscount = 100,

		/// <summary>
		/// Special agreement
		/// </summary>
		[Description("102")]
		SpecialAgreement = 102,

		/// <summary>
		/// Freight charge
		/// </summary>
		[Description("FC")]
		FreightCharge = 30, // FC

		/// <summary>
		/// Insurance
		/// </summary>
		[Description("FI")]
		Insurance = 31, // INS

		/// <summary>
		/// Packaging
		/// </summary>
		[Description("PAC")]
		Packaging = 32, // PAC

		/// <summary>
		/// Pallet charge
		/// </summary>
		[Description("PC")]
		PalletCharge = 33, // PC

		/// <summary>
		/// Handling service
		/// </summary>
		[Description("SH")]
		HandlingService = 34, // SH

		/// <summary>
		/// Transport costs
		/// </summary>
		[Description("TC")]
		TransportCosts = 35, // TC

        /// <summary>
        /// Testing service
        /// </summary>
        [Description("TAC")]
        TestingService = 36, // TAC

        /// <summary>
        /// Miscellaneous service
        /// </summary>
        [Description("ZZZ")]
		MiscellaneousService = 99 // ZZZ
	}
}
