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

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// http://www.stylusstudio.com/edifact/D02A/4451.htm
    /// </summary>
    public enum SubjectCodes
    {
        /// <summary>
        /// Goods description
        /// 
        /// [7002] Plain language description of the nature of the goods 
        /// sufficient to identify them at the level required for banking, 
        /// Customs, statistical or transport purposes, avoiding unnecessary 
        /// detail (Generic term).
        /// </summary>
        AAA,

        /// <summary>
        /// Terms of payments
        /// 
        /// [4276] Conditions of payment between the parties to a transaction (generic term).
        /// </summary>
        AAB,

        /// <summary>
        /// Dangerous goods additional information
        /// 
        /// Additional information concerning dangerous goods.
        /// </summary>
        AAC,

        /// <summary>
        /// Price conditions
        /// 
        /// Information on the price conditions that are expected or given.
        /// </summary>
        AAK,

        Unknown
    }



    internal static class SubjectCodesExtensions
    {
        public static SubjectCodes FromString(this SubjectCodes _c, string s)
        {
            try
            {
                return (SubjectCodes)Enum.Parse(typeof(SubjectCodes), s);
            }
            catch
            {
                return SubjectCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this SubjectCodes codes)
        {
            return codes.ToString("g");
        } // !ToString()
    }
}
