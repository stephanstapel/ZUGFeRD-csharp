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
    public class PaymentMeans
    {
        /// <summary>
        /// The means expressed as code, for how a payment is expected to be or has been settled.
        /// </summary>
        public PaymentMeansTypeCodes TypeCode { get; set; }

        /// <summary>
        /// The means expressed as code, for how a payment is expected to be or has been settled.
        /// </summary>
        public string Information { get; set; }

        /// <summary>
        /// Gläubiger-Identifikationsnummer
        /// 
        /// https://de.wikipedia.org/wiki/Gl%C3%A4ubiger-Identifikationsnummer
        /// </summary>
        public string SEPACreditorIdentifier { get; set; }

        /// <summary>
        /// Mandatsreferenz
        /// 
        /// https://de.wikipedia.org/wiki/Mandatsreferenz
        /// </summary>
        public string SEPAMandateReference { get; set; }

        /// <summary>
        /// Payment card information.
        /// </summary>
        public FinancialCard FinancialCard { get; set; }
    }
}
