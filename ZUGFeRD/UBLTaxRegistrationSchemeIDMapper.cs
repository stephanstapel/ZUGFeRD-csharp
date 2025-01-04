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
    internal class UBLTaxRegistrationSchemeIDMapper
    {
        internal static TaxRegistrationSchemeID Map(string value)
        {
            if (value.Trim().Equals("VAT", StringComparison.OrdinalIgnoreCase))
            {
                return TaxRegistrationSchemeID.VA;
            }
            else if (value.Trim().Equals("ID", StringComparison.OrdinalIgnoreCase))
            {
                return TaxRegistrationSchemeID.FC;
            }
            else
            {
                return default(TaxRegistrationSchemeID).FromString(value);
            }
        } // !Map()


        internal static string Map(TaxRegistrationSchemeID type)
        {
            if (type == TaxRegistrationSchemeID.VA)
            {
                return "VAT";
            }
            else if (type == TaxRegistrationSchemeID.FC)
            {
                return "ID";
            }
            else
            {
                return type.EnumToString();
            }
        } // !Map()
    }
}
