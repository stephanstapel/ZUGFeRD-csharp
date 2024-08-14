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
using System.Xml;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Common reader for both qdt and udt data types
    /// </summary>
    internal class DataTypeReader
    {
        internal static DateTime? ReadFormattedIssueDateTime(XmlNode node, string xpath, XmlNamespaceManager nsmgr, DateTime? defaultValue = null)
        {
            XmlNode selectedNode = node.SelectSingleNode(xpath, nsmgr);
            if (selectedNode == null)
            {
                return defaultValue;
            }

            if (selectedNode.InnerXml.Contains("<qdt:"))
            {
                return XmlUtils.NodeAsDateTime(selectedNode, "./qdt:DateTimeString", nsmgr);
            }
            else if (selectedNode.InnerXml.Contains("<udt:"))
            {
                return XmlUtils.NodeAsDateTime(selectedNode, "./udt:DateTimeString", nsmgr);
            }

            return defaultValue;
        } // !ReadFormattedIssueDateTime()
    }
}
