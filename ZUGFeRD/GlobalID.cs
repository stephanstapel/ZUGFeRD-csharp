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
    /// Global ID definition according to ISO 6523:
    /// http://en.wikipedia.org/wiki/ISO_6523
    /// 
    /// For a full list of identifiers, see here:
    /// http://www.oid-info.com/doc/ICD-list.pdf
    /// 
    /// Especially important for ZUGFeRD:
    /// Code 0160 - GTIN - Global Trade Item Number for articles
    /// Code 0088 - EAN Location Code
    /// </summary>
    public class GlobalID
    {
        public string ID { get; set; }
        public string SchemeID { get; set; }

        public const string SchemeID_GTIN = "0160";
        public const string SchemeID_EANLocationCode = "0088";

        public GlobalID()
        {
            this.ID = "";
            this.SchemeID = "";
        } // !GlobalID()

        public GlobalID(string schemeID, string ID)
        {
            this.ID = ID;
            this.SchemeID = schemeID;
        } // !GlobalID()
    }
}
