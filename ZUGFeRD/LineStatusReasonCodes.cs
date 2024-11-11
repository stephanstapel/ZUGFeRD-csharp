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
    /// Used in BT-X-8
    /// </summary>
    public enum LineStatusReasonCodes
    {
        /// <summary>
        /// Unknown/ invalid line status code
        /// </summary>
        Unknown,

        /// <summary>
        /// Detail
        /// 
        /// Regular item position (standard case)
        /// </summary>
        DETAIL,

        /// <summary>
        /// Subtotal
        /// </summary>
        GROUP,

        /// <summary>
        /// Solely information
        /// 
        /// For information only
        /// </summary>
        INFORMATION
    }

    internal static class LineStatusReasonCodesExtensions
    {
        public static LineStatusReasonCodes? FromString(this LineStatusReasonCodes _, string s)
        {
            if (s == null)
                return null;
            return EnumExtensions.StringToEnum<LineStatusReasonCodes>(s);
        } // !FromString()

        public static string EnumValueToString(this LineStatusReasonCodes t)
        {
            return EnumExtensions.EnumToInt(t).ToString();
        } // !ToString()

        public static string EnumToString(this LineStatusReasonCodes c)
        {
            return EnumExtensions.EnumToString<LineStatusReasonCodes>(c);
        } // !ToString()
    }
}
