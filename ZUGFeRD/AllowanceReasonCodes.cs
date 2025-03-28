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
    /// Reason codes according to UN/EDIFACT UNCL5189 code list
    /// </summary>
	public enum AllowanceReasonCodes
	{
        /// <summary>
        /// Bonus for works ahead of schedule
        /// </summary>
        [EnumStringValue("41")]
        BonusForWorksAheadOfSchedule,

        /// <summary>
        /// Other bonus
        /// </summary>
        [EnumStringValue("42")]
        OtherBonus,

        /// <summary>
        /// Manufacturer’s consumer discount
        /// </summary>
        [EnumStringValue("60")]
        ManufacturersConsumerDiscount,

        /// <summary>
        /// Due to military status
        /// </summary>
        [EnumStringValue("62")]
        DueToMilitaryStatus,

        /// <summary>
        /// Due to work accident
        /// </summary>
        [EnumStringValue("63")]
        DueToWorkAccident,

        /// <summary>
        /// Special agreement
        /// </summary>
        [EnumStringValue("64")]
        SpecialAgreement,

        /// <summary>
        /// Production error discount
        /// </summary>
        [EnumStringValue("65")]
        ProductionErrorDiscount,

        /// <summary>
        /// New outlet discount
        /// </summary>
        [EnumStringValue("66")]
        NewOutletDiscount,

        /// <summary>
        /// Sample discount
        /// </summary>
        [EnumStringValue("67")]
        SampleDiscount,

        /// <summary>
        /// End-of-range discount
        /// </summary>
        [EnumStringValue("68")]
        EndOfRangeDiscount,

        /// <summary>
        /// Incoterm discount
        /// </summary>
        [EnumStringValue("70")]
        IncotermDiscount,

        /// <summary>
        /// Point of sales threshold allowance
        /// </summary>
        [EnumStringValue("71")]
        PointOfSalesThresholdAllowance,

        /// <summary>
        /// Material surcharge/deduction
        /// </summary>
        [EnumStringValue("88")]
        MaterialSurchargeOrDeduction,

        /// <summary>
        /// Discount
        /// </summary>
        [EnumStringValue("95")]
        Discount,

        /// <summary>
        /// Special rebate
        /// </summary>
        [EnumStringValue("100")]
        SpecialRebate,

        /// <summary>
        /// Fixed long term
        /// </summary>
        [EnumStringValue("102")]
        FixedLongTerm,

        /// <summary>
        /// Temporary
        /// </summary>
        [EnumStringValue("103")]
        Temporary,

        /// <summary>
        /// Standard
        /// </summary>
        [EnumStringValue("104")]
        Standard,

        /// <summary>
        /// Yearly turnover
        /// </summary>
        [EnumStringValue("105")]
        YearlyTurnover
    }
}
