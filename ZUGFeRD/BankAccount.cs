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
    /// This class holds information about a bank account. The class is used in different places,
    /// e.g. for holding supplier and customer bank information
    /// </summary>
    public class BankAccount
    {
        /// <summary>
        /// National account number (not SEPA) 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// IBAN identifier for the bank account. This information is not yet validated.
        /// </summary>
        public string IBAN { get; set; }

        /// <summary>
        /// Payment service provider identifier
        /// </summary>
        public string BIC { get; set; }

        /// <summary>
        /// Legacy bank identifier
        /// </summary>
        public string Bankleitzahl { get; set; }

        /// <summary>
        /// Clear text name of the bank
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Payment account name
        /// </summary>
        public string Name { get; set; }
    }
}
