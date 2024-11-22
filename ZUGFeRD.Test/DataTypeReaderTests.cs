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
using System.Reflection;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using s2industries.ZUGFeRD;

namespace s2industries.ZUGFeRD.Test
{
    [TestClass]
    public class DataTypeReaderTests
    {
        private readonly Type? _dataTypeReader = typeof(InvoiceDescriptor).Assembly.GetType("s2industries.ZUGFeRD.DataTypeReader");

        [TestMethod]
        public void ReadFormattedIssueDateTime_ReturnsCorrectDateTime_WhenNodeContainsQdt()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.LoadXml("<root xmlns=\"http://www.sample.com/file\" xmlns:qdt=\"http://example.com/1\">" +
                        "<dateTime>" +
                        "<qdt:DateTimeString format=\"102\">20230101</qdt:DateTimeString>" +
                        "</dateTime></root>");
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("qdt", "http://example.com/1");
            nsmgr.AddNamespace("def", "http://www.sample.com/file");
            var node = doc.SelectSingleNode("//def:dateTime", nsmgr);

            // Act
            var result = InvokeReadFormattedIssueDateTime(node, ".", nsmgr, null);

            // Assert
            Assert.AreEqual(new DateTime(2023, 1, 1), result);
        }

        [TestMethod]
        public void ReadFormattedIssueDateTime_ReturnsCorrectDateTime_WhenNodeContainsUdt()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.LoadXml("<root xmlns=\"http://www.sample.com/file\" xmlns:udt=\"http://example.com/1\">" +
                        "<dateTime>" +
                        "<udt:DateTimeString format=\"102\">20230101</udt:DateTimeString>" +
                        "</dateTime></root>");
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("udt", "http://example.com/1");
            nsmgr.AddNamespace("def", "http://www.sample.com/file");
            var node = doc.SelectSingleNode("//def:dateTime", nsmgr);

            // Act
            var result = InvokeReadFormattedIssueDateTime(node, ".", nsmgr, null);

            // Assert
            Assert.AreEqual(new DateTime(2023, 1, 1), result);
        }


        [TestMethod]
        public void ReadFormattedIssueDateTime_ReturnsDefaultValue_WhenNodeIsEmpty()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.LoadXml("<root><dateTime></dateTime></root>");
            var node = doc.SelectSingleNode("//dateTime");
            var nsmgr = new XmlNamespaceManager(new NameTable());
            DateTime? expected = new DateTime(2023, 1, 1);

            // Act
            var result = InvokeReadFormattedIssueDateTime(node, ".", nsmgr, expected);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ReadFormattedIssueDateTime_ReturnsDefaultValue_WhenNodeDoesNotContainQdtOrUdt()
        {
            // Arrange
            var doc = new XmlDocument();
            doc.LoadXml("<root><dateTime>NoSpecialTag</dateTime></root>");
            var node = doc.SelectSingleNode("//dateTime");
            var nsmgr = new XmlNamespaceManager(new NameTable());
            var defaultValue = new DateTime(2023, 1, 1);

            // Act
            var result = InvokeReadFormattedIssueDateTime(node, ".", nsmgr, defaultValue);

            // Assert
            Assert.AreEqual(defaultValue, result);
        }


        private DateTime? InvokeReadFormattedIssueDateTime(params object?[] methodParams)
        {
            ArgumentNullException.ThrowIfNull(_dataTypeReader);

            var method = _dataTypeReader.GetMethod("ReadFormattedIssueDateTime", BindingFlags.NonPublic | BindingFlags.Static);

            ArgumentNullException.ThrowIfNull(method);

            return (DateTime?)method.Invoke(null, methodParams);
        }
    }
}
