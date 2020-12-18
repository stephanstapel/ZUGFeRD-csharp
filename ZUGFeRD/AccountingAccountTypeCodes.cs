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
    //based on https://www.unece.org/fileadmin/DAM/uncefact/codelist/standard/EDIFICASEU_AccountingAccountType_D11A.xsd

    /// <summary>Account Types (EDIFICAS-EU Code List)</summary>
    public enum AccountingAccountTypeCodes
    {
        /// <summary>
        /// TypeCode not set
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The code indicates a financial account
        /// </summary>
        Financial = 1,
        /// <summary>
        /// The code indicates a subsidiary account
        /// </summary>
        Subsidiary = 2,
        /// <summary>
        /// The code indicates a budget account
        /// </summary>
        Budget = 3,
        /// <summary>
        /// The code indicates a cost accounting account
        /// </summary>
        Cost_Accounting = 4,
        /// <summary>
        /// The code indicates a receivable account
        /// </summary>
        Receivable = 5,
        /// <summary>
        /// The code indicates a payable account
        /// </summary>
        Payable = 6,
        /// <summary>
        /// The code indicates a job cost accounting
        /// </summary>
        Job_Cost_Accounting = 7
    }


    internal static class AccountingAccountTypeCodesExtensions
    {
        public static AccountingAccountTypeCodes FromString(this AccountingAccountTypeCodes _, string s)
        {
            try
            {
                return (AccountingAccountTypeCodes)Enum.Parse(typeof(AccountingAccountTypeCodes), s);
            }
            catch
            {
                return AccountingAccountTypeCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this AccountingAccountTypeCodes c)
        {
            return c.ToString("g");
        } // !ToString()
    }
}
