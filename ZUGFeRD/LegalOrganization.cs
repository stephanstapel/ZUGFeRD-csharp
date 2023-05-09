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
namespace s2industries.ZUGFeRD
{
  /// <summary>
  /// Details about a legal organization
  /// </summary>
  public class LegalOrganization
  {
    /// <summary>
    /// Create a new LegalOrganization instance
    /// </summary>
    public LegalOrganization()
    {
      ID = null;
      TradingBusinessName = "";
    }
    /// <summary>
    /// Create a new LegalOrganization instance
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schemeID"></param>
    /// <param name="tradingBusinessName"></param>
    public LegalOrganization(GlobalIDSchemeIdentifiers schemeID = GlobalIDSchemeIdentifiers.Unknown, string id = "", string tradingBusinessName = "")
    {
      ID = new GlobalID(schemeID, id);
      TradingBusinessName = tradingBusinessName;
    }
    /// <summary>
    /// Legal organization ID
    /// </summary>
    public GlobalID ID { get; set; }

    /// <summary>
    /// Trading business name
    /// </summary>
    public string TradingBusinessName { get; set; }
  }
}