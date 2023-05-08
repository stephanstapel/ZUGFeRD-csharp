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
  /// Detailed information about a party that has a certain role within an invoice
  /// </summary>
  public class Party
  {
    /// <summary>
    /// Party identifier
    /// </summary>
    public GlobalID ID { get; set; }

    /// <summary>
    /// Party name, e.g. company name
    /// </summary>      
    public string Name { get; set; }

    /// <summary>
    /// Name of the contact at the party
    /// </summary>
    public string ContactName { get; set; }

    /// <summary>
    /// City, not including postcode (separate property)
    /// </summary>   
    public string City { get; set; }

    /// <summary>
    /// Party postcode, represented in the respective country format
    /// </summary>      
    public string Postcode { get; set; }

    /// <summary>
    /// Party country
    /// </summary>      
    public CountryCodes Country { get; set; }

    /// <summary>
    /// Street name and number
    /// </summary>
    public string Street { get; set; }
    /// <summary>
    /// Global identifier
    /// </summary>
    public GlobalID GlobalID { get; set; }

    /// <summary>
    /// Address line 3
    /// It's an additional line to give more details to the address.
    /// This field is purely optional.
    /// e.g. used for BT-162, BT-164, BT-165
    /// </summary>
    public string AddressLine3 { get; set; }

    /// <summary>
    /// Country subdivision (e.g. Niedersachsen, Bayern)
    /// This field is purely optional.
    /// e.g. used for BT-39, BT-54, BT-68, BT-79
    /// </summary>
    public string CountrySubdivisionName { get; set; }
    /// <summary>
    /// Legal organization
    /// </summary>
    public LegalOrganization SpecifiedLegalOrganization { get; set; }
  }
}
