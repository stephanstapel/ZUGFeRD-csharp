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
using System.Text;

namespace s2industries.ZUGFeRD.Test
{
    [TestClass]
    public class Format616DateTests
    {
        private static InvoiceDescriptor LoadWeek(string coded)
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<rsm:CrossIndustryInvoice xmlns:rsm=""urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100"" xmlns:ram=""urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"" xmlns:udt=""urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100"">
  <rsm:ExchangedDocumentContext>
    <ram:GuidelineSpecifiedDocumentContextParameter>
      <ram:ID>urn:factur-x.eu:1p0:minimum</ram:ID>
    </ram:GuidelineSpecifiedDocumentContextParameter>
  </rsm:ExchangedDocumentContext>
  <rsm:ExchangedDocument>
    <ram:IssueDateTime>
      <udt:DateTimeString format=""616"">" + coded + @"</udt:DateTimeString>
    </ram:IssueDateTime>
  </rsm:ExchangedDocument>
</rsm:CrossIndustryInvoice>";
            using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            return InvoiceDescriptor.Load(stream);
        }

        [TestMethod]
        public void Format616Week1Of2024IsTheMonday()
        {
            DateTime? invoiceDate = LoadWeek("202401").InvoiceDate;
            Assert.AreEqual(new DateTime(2024, 1, 1), invoiceDate);
            Assert.AreEqual(DateTimeKind.Unspecified, invoiceDate.Value.Kind);
        }

        [TestMethod]
        public void Format616Week53Of2020IsTheMonday()
        {
            Assert.AreEqual(new DateTime(2020, 12, 28), LoadWeek("202053").InvoiceDate);
        }

        [TestMethod]
        public void Format616Week1CanStartInThePreviousYear()
        {
            Assert.AreEqual(new DateTime(2014, 12, 29), LoadWeek("201501").InvoiceDate);
        }

        [TestMethod]
        public void Format616DoesNotTurnWeek99IntoAnInvoiceDate()
        {
            Assert.IsNull(LoadWeek("202499").InvoiceDate);
        }

        [TestMethod]
        public void Format616DoesNotInventWeek53()
        {
            Assert.IsNull(LoadWeek("202453").InvoiceDate);
        }
    }
}
