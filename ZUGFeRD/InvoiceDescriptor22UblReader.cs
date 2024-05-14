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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;


namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor22UblReader : IInvoiceDescriptorReader
    {
        /// <summary>
        /// Parses the ZUGFeRD invoice from the given stream.
        /// 
        /// Make sure that the stream is open, otherwise an IllegalStreamException exception is thrown.
        /// Important: the stream will not be closed by this function.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>The parsed ZUGFeRD invoice</returns>
        public override InvoiceDescriptor Load(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new IllegalStreamException("Cannot read from stream");
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.DocumentElement.OwnerDocument.NameTable);
            nsmgr.AddNamespace("ubl", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            InvoiceDescriptor retval = new InvoiceDescriptor
            {
                IsTest = false, // TODO: Find value //IsTest = _nodeAsBool(doc.DocumentElement, "//rsm:ExchangedDocumentContext/ram:TestIndicator/udt:Indicator", nsmgr),
                BusinessProcess = _nodeAsString(doc.DocumentElement, "//cbc:ProfileID", nsmgr),
                Profile = Profile.XRechnung, //default(Profile).FromString(_nodeAsString(doc.DocumentElement, "//ram:GuidelineSpecifiedDocumentContextParameter/ram:ID", nsmgr)),
                Type = default(InvoiceType).FromString(_nodeAsString(doc.DocumentElement, "//cbc:InvoiceTypeCode", nsmgr)),
                InvoiceNo = _nodeAsString(doc.DocumentElement, "//cbc:ID", nsmgr),
                InvoiceDate = _nodeAsDateTime(doc.DocumentElement, "//cbc:IssueDate", nsmgr)
            };

            foreach (XmlNode node in doc.SelectNodes("/ubl:Invoice/cbc:Note", nsmgr))
            {
                string content = _nodeAsString(node, ".", nsmgr);
                if (string.IsNullOrWhiteSpace(content)) continue;
                var contentParts = content.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                string _subjectCode = "";
                if (contentParts.Length > 1 && contentParts[0].Length == 3)
                {
                    _subjectCode = contentParts[0];
                    content = contentParts[1];
                }
                SubjectCodes subjectCode = default(SubjectCodes).FromString(_subjectCode);
                retval.AddNote(content, subjectCode);
            }

            retval.ReferenceOrderNo = _nodeAsString(doc, "//cbc:BuyerReference", nsmgr);

            retval.Seller = _nodeAsParty(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party", nsmgr);

            if (doc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party/cbc:EndpointID", nsmgr) != null)
            {
                string id = _nodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cbc:EndpointID", nsmgr);
                string schemeID = _nodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cbc:EndpointID/@schemeID", nsmgr);

                var eas = default(ElectronicAddressSchemeIdentifiers).FromString(schemeID);

                if (eas.HasValue)
                {
                    retval.SetSellerElectronicAddress(id, eas.Value);
                }
            }

            foreach (XmlNode node in doc.SelectNodes("//cac:AccountingSupplierParty/cac:Party/cac:PartyTaxScheme", nsmgr))
            {
                string id = _nodeAsString(node, ".//cbc:CompanyID", nsmgr);
                TaxRegistrationSchemeID schemeID = _getUncefactTaxSchemeID(_nodeAsString(node, ".//cac:TaxScheme/cbc:ID", nsmgr));

                retval.AddSellerTaxRegistration(id, schemeID);
            }

            if (doc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party/cac:Contact", nsmgr) != null)
            {
                retval.SellerContact = new Contact()
                {
                    Name = _nodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:Contact/cbc:Name", nsmgr),
                    OrgUnit = "", // TODO: Find value //OrgUnit = _nodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/ram:DefinedTradeContact/ram:DepartmentName", nsmgr),
                    PhoneNo = _nodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:Contact/cbc:Telephone", nsmgr),
                    FaxNo = "", // TODO: Find value //FaxNo = _nodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:Contact/", nsmgr),
                    EmailAddress = _nodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:Contact/cbc:ElectronicMail", nsmgr)
                };
            }

            retval.Buyer = _nodeAsParty(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party", nsmgr);

            if (doc.SelectSingleNode("//cac:AccountingCustomerParty/cac:Party/cbc:EndpointID", nsmgr) != null)
            {
                string id = _nodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cbc:EndpointID", nsmgr);
                string schemeID = _nodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cbc:EndpointID/@schemeID", nsmgr);

                var eas = default(ElectronicAddressSchemeIdentifiers).FromString(schemeID);

                if (eas.HasValue)
                {
                    retval.SetBuyerElectronicAddress(id, eas.Value);
                }
            }

            foreach (XmlNode node in doc.SelectNodes("//cac:AccountingCustomerParty/cac:Party/cac:PartyTaxScheme", nsmgr))
            {
                string id = _nodeAsString(node, ".//cbc:CompanyID", nsmgr);
                TaxRegistrationSchemeID schemeID = _getUncefactTaxSchemeID(_nodeAsString(node, ".//cac:TaxScheme/cbc:ID", nsmgr));

                retval.AddBuyerTaxRegistration(id, schemeID);
            }

            if (doc.SelectSingleNode("//cac:AccountingCustomerParty/cac:Party/cac:Contact", nsmgr) != null)
            {
                retval.BuyerContact = new Contact()
                {
                    Name = _nodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:Name", nsmgr),
                    OrgUnit = "", // TODO: Find value //OrgUnit = _nodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/ram:DefinedTradeContact/ram:DepartmentName", nsmgr),
                    PhoneNo = _nodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:Telephone", nsmgr),
                    FaxNo = "", // TODO: Find value //FaxNo = _nodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cac:Contact/", nsmgr),
                    EmailAddress = _nodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:ElectronicMail", nsmgr)
                };
            }

            //Get all referenced and embedded documents (BG-24)
            // TODO //XmlNodeList referencedDocNodes = doc.SelectNodes(".//ram:ApplicableHeaderTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
            //foreach (XmlNode referenceNode in referencedDocNodes)
            //{
            //  retval.AdditionalReferencedDocuments.Add(_getAdditionalReferencedDocument(referenceNode, nsmgr));
            //}

            //-------------------------------------------------
            // hzi: With old implementation only the first document has been read instead of all documents
            //-------------------------------------------------
            //if (doc.SelectSingleNode("//ram:AdditionalReferencedDocument", nsmgr) != null)
            //{
            //    string _issuerAssignedID = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:IssuerAssignedID", nsmgr);
            //    string _typeCode = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:TypeCode", nsmgr);
            //    string _referenceTypeCode = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:ReferenceTypeCode", nsmgr);
            //    string _name = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:Name", nsmgr);
            //    DateTime? _date = _nodeAsDateTime(doc.DocumentElement, "//ram:AdditionalReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr);

            //    if (doc.SelectSingleNode("//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject", nsmgr) != null)
            //    {
            //        string _filename = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject/@filename", nsmgr);
            //        byte[] data = Convert.FromBase64String(_nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject", nsmgr));

            //        retval.AddAdditionalReferencedDocument(id: _issuerAssignedID,
            //                                               typeCode: default(AdditionalReferencedDocumentTypeCode).FromString(_typeCode),
            //                                               issueDateTime: _date,                                                           
            //                                               referenceTypeCode: default(ReferenceTypeCodes).FromString(_referenceTypeCode),
            //                                               name: _name,
            //                                               attachmentBinaryObject: data,
            //                                               filename: _filename);
            //    }
            //    else
            //    {
            //        retval.AddAdditionalReferencedDocument(id: _issuerAssignedID,
            //                                               typeCode: default(AdditionalReferencedDocumentTypeCode).FromString(_typeCode),
            //                                               issueDateTime: _date,                                                           
            //                                               referenceTypeCode: default(ReferenceTypeCodes).FromString(_referenceTypeCode),
            //                                               name: _name);
            //    }
            //}
            //-------------------------------------------------

            XmlNode deliveryNode = doc.SelectSingleNode("//cac:Delivery", nsmgr);
            if (deliveryNode != null)
            {
                XmlNode deliveryLocationNode = deliveryNode.SelectSingleNode("//cac:DeliveryLocation", nsmgr);
                if (deliveryLocationNode != null)
                {
                    retval.ShipTo = _nodeAsAddressParty(deliveryNode, ".//cac:Address", nsmgr) ?? new Party();
                    retval.ShipTo.GlobalID = new GlobalID();
                    retval.ShipTo.ID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(_nodeAsString(deliveryLocationNode, ".//cbc:ID/@schemeID", nsmgr)), _nodeAsString(deliveryLocationNode, ".//cbc:ID", nsmgr));
                    retval.ShipTo.Name = _nodeAsString(deliveryNode, ".//cac:DeliveryParty/cac:PartyName/cbc:Name", nsmgr);
                }
                retval.ActualDeliveryDate = _nodeAsDateTime(doc.DocumentElement, "//cac:Delivery/cbc:ActualDeliveryDate", nsmgr);
            }
            // TODO: Find value //retval.ShipFrom = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ShipFromTradeParty", nsmgr);

            // TODO: Find value //string _deliveryNoteNo = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr);
            // TODO: Find value //DateTime? _deliveryNoteDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);

            //if (!_deliveryNoteDate.HasValue)
            //{
            //  _deliveryNoteDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime", nsmgr);
            //}

            //if (_deliveryNoteDate.HasValue || !String.IsNullOrWhiteSpace(_deliveryNoteNo))
            //{
            //  retval.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            //  {
            //    ID = _deliveryNoteNo,
            //    IssueDateTime = _deliveryNoteDate
            //  };
            //}

            string _despatchAdviceNo = _nodeAsString(doc.DocumentElement, "//cac:ApplicableHeaderTradeDelivery/cac:DespatchAdviceReferencedDocument/cbc:Id", nsmgr);
            DateTime? _despatchAdviceDate = _nodeAsDateTime(doc.DocumentElement, "//cac:ApplicableHeaderTradeDelivery/cac:DespatchAdviceReferencedDocument/cbc:IssueDate", nsmgr);

            if (_despatchAdviceDate.HasValue || !String.IsNullOrWhiteSpace(_despatchAdviceNo))
            {
                retval.DespatchAdviceReferencedDocument = new DespatchAdviceReferencedDocument()
                {
                    ID = _despatchAdviceNo,
                    IssueDateTime = _despatchAdviceDate
                };
            }

            // TODO: Find value //retval.Invoicee = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceeTradeParty", nsmgr);
            retval.Payee = _nodeAsParty(doc.DocumentElement, "//cac:PayeeParty", nsmgr);

            retval.PaymentReference = _nodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cbc:PaymentID", nsmgr);
            retval.Currency = default(CurrencyCodes).FromString(_nodeAsString(doc.DocumentElement, "//cbc:DocumentCurrencyCode", nsmgr));

            // TODO: Multiple SpecifiedTradeSettlementPaymentMeans can exist for each account/institution (with different SEPA?)
            PaymentMeans _tempPaymentMeans = new PaymentMeans()
            {
                TypeCode = default(PaymentMeansTypeCodes).FromString(_nodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cbc:PaymentMeansCode", nsmgr)),
                Information = _nodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cbc:PaymentMeansCode/@name", nsmgr),
                SEPACreditorIdentifier = _nodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID='SEPA']", nsmgr),
                SEPAMandateReference = _nodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cac:PaymentMandate/cbc:ID", nsmgr)
            };

            var financialCardId = _nodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cac:CardAccount/cbc:PrimaryAccountNumberID", nsmgr);
            var financialCardCardholderName = _nodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cac:CardAccount/cbc:HolderName", nsmgr);

            if (!string.IsNullOrWhiteSpace(financialCardId) || !string.IsNullOrWhiteSpace(financialCardCardholderName))
            {
                _tempPaymentMeans.FinancialCard = new FinancialCard()
                {
                    Id = financialCardId,
                    CardholderName = financialCardCardholderName
                };
            }

            retval.PaymentMeans = _tempPaymentMeans;

            retval.BillingPeriodStart = _nodeAsDateTime(doc.DocumentElement, "//cac:InvoicePeriod/cbc:StartDate", nsmgr);
            retval.BillingPeriodEnd = _nodeAsDateTime(doc.DocumentElement, "//cac:InvoicePeriod/cbc:EndDate", nsmgr);

            XmlNodeList creditorFinancialAccountNodes = doc.SelectNodes("//cac:PaymentMeans/cac:PayeeFinancialAccount", nsmgr);
            foreach (XmlNode node in creditorFinancialAccountNodes)
            {
                retval.CreditorBankAccounts.Add(_nodeAsBankAccount(node, ".", nsmgr));
            }

            XmlNodeList debitorFinancialAccountNodes = doc.SelectNodes("//cac:PaymentMeans/cac:PaymentMandate/cac:PayerFinancialAccount", nsmgr);
            foreach (XmlNode node in debitorFinancialAccountNodes)
            {
                retval.DebitorBankAccounts.Add(_nodeAsBankAccount(node, ".", nsmgr));
            }

            foreach (XmlNode node in doc.SelectNodes("//cac:TaxTotal/cac:TaxSubtotal", nsmgr))
            {
                retval.AddApplicableTradeTax(_nodeAsDecimal(node, "cbc:TaxableAmount", nsmgr, 0).Value,
                                             _nodeAsDecimal(node, "cac:TaxCategory/cbc:Percent", nsmgr, 0).Value,
                                             default(TaxTypes).FromString(_nodeAsString(node, "cac:TaxCategory/cac:TaxScheme/cbc:ID", nsmgr)),
                                             default(TaxCategoryCodes).FromString(_nodeAsString(node, "cac:TaxCategory/cbc:ID", nsmgr)),
                                             0,
                                             default(TaxExemptionReasonCodes).FromString(_nodeAsString(node, "cac:TaxCategory/cbc:TaxExemptionReasonCode", nsmgr)),
                                             _nodeAsString(node, "cac:TaxCategory/cbc:TaxExemptionReason", nsmgr)
                                             );
            }

            foreach (XmlNode node in doc.SelectNodes("//cac:AllowanceCharge", nsmgr))
            {
                retval.AddTradeAllowanceCharge(!_nodeAsBool(node, ".//cbc:ChargeIndicator", nsmgr), // wichtig: das not (!) beachten
                                               _nodeAsDecimal(node, ".//cbc:BaseAmount", nsmgr, 0).Value,
                                               retval.Currency,
                                               _nodeAsDecimal(node, ".//cbc:Amount", nsmgr, 0).Value,
                                               _nodeAsString(node, ".//cbc:AllowanceChargeReason", nsmgr),
                                               default(TaxTypes).FromString(_nodeAsString(node, ".//cac:TaxCategory/cac:TaxScheme/cbc:ID", nsmgr)),
                                               default(TaxCategoryCodes).FromString(_nodeAsString(node, ".//cac:TaxCategory/cbc:ID", nsmgr)),
                                               _nodeAsDecimal(node, ".//cac:TaxCategory/cbc:Percent", nsmgr, 0).Value);
            }

            // TODO: Find value //foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedLogisticsServiceCharge", nsmgr))
            //{
            //  retval.AddLogisticsServiceCharge(_nodeAsDecimal(node, ".//ram:AppliedAmount", nsmgr, 0).Value,
            //                                   _nodeAsString(node, ".//ram:Description", nsmgr),
            //                                   default(TaxTypes).FromString(_nodeAsString(node, ".//ram:AppliedTradeTax/ram:TypeCode", nsmgr)),
            //                                   default(TaxCategoryCodes).FromString(_nodeAsString(node, ".//ram:AppliedTradeTax/ram:CategoryCode", nsmgr)),
            //                                   _nodeAsDecimal(node, ".//ram:AppliedTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value);
            //}

            retval.InvoiceReferencedDocument = new InvoiceReferencedDocument()
            {
                ID = _nodeAsString(doc.DocumentElement, "//cac:BillingReference/cac:InvoiceDocumentReference/cbc:ID", nsmgr),
                IssueDateTime = _nodeAsDateTime(doc.DocumentElement, "//cac:BillingReference/cac:InvoiceDocumentReference/cbc:IssueDate", nsmgr)
            };

            retval.PaymentTerms = new PaymentTerms()
            {
                Description = _nodeAsString(doc.DocumentElement, "//cac:PaymentTerms/cbc:Note", nsmgr),
                DueDate = _nodeAsDateTime(doc.DocumentElement, "//cbc:DueDate", nsmgr)
            };

            retval.LineTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:LineExtensionAmount", nsmgr, 0).Value;
            retval.ChargeTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:ChargeTotalAmount", nsmgr, null);
            retval.AllowanceTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:AllowanceTotalAmount", nsmgr, null);
            retval.TaxBasisAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:TaxExclusiveAmount", nsmgr, null);
            retval.TaxTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:TaxTotal/cbc:TaxAmount", nsmgr, 0).Value;
            retval.GrandTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount", nsmgr, 0).Value; ;
            retval.RoundingAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:PayableRoundingAmount", nsmgr, 0).Value;
            retval.TotalPrepaidAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:PrepaidAmount", nsmgr, null);
            retval.DuePayableAmount = _nodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:PayableAmount", nsmgr, 0).Value;

            // TODO: Find value //foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:ReceivableSpecifiedTradeAccountingAccount", nsmgr))
            //{
            //  retval.ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
            //  {
            //    TradeAccountID = _nodeAsString(node, ".//ram:ID", nsmgr),
            //    TradeAccountTypeCode = (AccountingAccountTypeCodes)_nodeAsInt(node, ".//ram:TypeCode", nsmgr),
            //  });
            //}

            // TODO: Find value //retval.OrderDate = _nodeAsDateTime(doc.DocumentElement, "//cac:OrderReference/ram:BuyerOrderReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr);
            retval.OrderNo = _nodeAsString(doc.DocumentElement, "//cac:OrderReference/cbc:ID", nsmgr);

            // Read SellerOrderReferencedDocument
            if (doc.SelectSingleNode("//cac:OrderReference/cbc:SalesOrderID", nsmgr) != null)
            {
                retval.SellerOrderReferencedDocument = new SellerOrderReferencedDocument()
                {
                    ID = _nodeAsString(doc.DocumentElement, "//cac:OrderReference/cbc:SalesOrderID", nsmgr),
                    // TODO: Find value //IssueDateTime = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SellerOrderReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr)
                };
            }

            // Read ContractReferencedDocument
            if (doc.SelectSingleNode("//cac:ContractDocumentReference/cbc:ID", nsmgr) != null)
            {
                retval.ContractReferencedDocument = new ContractReferencedDocument
                {
                    ID = _nodeAsString(doc.DocumentElement, "//cac:ContractDocumentReference/cbc:ID", nsmgr),
                    // TODO: Find value //IssueDateTime = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime", nsmgr)
                };
            }

            retval.SpecifiedProcuringProject = new SpecifiedProcuringProject
            {
                ID = _nodeAsString(doc.DocumentElement, "//cac:ProjectReference/cbc:ID", nsmgr),
                Name = "" // TODO: Find value //Name = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SpecifiedProcuringProject/ram:Name", nsmgr)
            };

            foreach (XmlNode node in doc.SelectNodes("//cac:InvoiceLine", nsmgr))
            {
                retval.TradeLineItems.Add(_parseTradeLineItem(node, nsmgr));
            }

            return retval;
        } // !Load()        


        public override bool IsReadableByThisReaderVersion(Stream stream)
        {
            List<string> validURIs = new List<string>()
                {
                  "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2",
                  "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2",
                  "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2",
                };

            return _IsReadableByThisReaderVersion(stream, validURIs);
        } // !IsReadableByThisReaderVersion()

        private new bool _IsReadableByThisReaderVersion(Stream stream, IList<string> validURIs)
        {
            long _oldStreamPosition = stream.Position;
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                string data = reader.ReadToEnd().Replace(" ", "").ToLower();
                foreach (string validURI in validURIs)
                {
                    if (data.Contains(string.Format("=\"{0}\"", validURI.ToLower())))
                    {
                        stream.Position = _oldStreamPosition;
                        return true;
                    }
                }
            }

            stream.Position = _oldStreamPosition;
            return false;
        }

        private static TradeLineItem _parseTradeLineItem(XmlNode tradeLineItem, XmlNamespaceManager nsmgr = null)
        {
            if (tradeLineItem == null)
            {
                return null;
            }

            TradeLineItem item = new TradeLineItem()
            {
                // TODO: Find value //GlobalID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(_nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:GlobalID/@schemeID", nsmgr)),
                //                          _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:GlobalID", nsmgr)),
                SellerAssignedID = _nodeAsString(tradeLineItem, ".//cac:Item/cac:SellersItemIdentification/cbc:ID", nsmgr),
                BuyerAssignedID = _nodeAsString(tradeLineItem, ".//cac:Item/cac:BuyersItemIdentification/cbc:ID", nsmgr),
                Name = _nodeAsString(tradeLineItem, ".//cac:Item/cbc:Name", nsmgr),
                Description = _nodeAsString(tradeLineItem, ".//cac:Item/cbc:Description", nsmgr),
                UnitQuantity = _nodeAsDecimal(tradeLineItem, ".//cac:Price/cbc:BaseQuantity", nsmgr, 1),
                BilledQuantity = _nodeAsDecimal(tradeLineItem, ".//cbc:InvoicedQuantity", nsmgr, 0).Value,
                LineTotalAmount = _nodeAsDecimal(tradeLineItem, ".//cbc:LineExtensionAmount", nsmgr, 0),
                TaxCategoryCode = default(TaxCategoryCodes).FromString(_nodeAsString(tradeLineItem, ".//cac:Item/cac:ClassifiedTaxCategory/cbc:ID", nsmgr)),
                TaxType = default(TaxTypes).FromString(_nodeAsString(tradeLineItem, ".//cac:Item/cac:ClassifiedTaxCategory/cac:TaxScheme/cbc:ID", nsmgr)),
                TaxPercent = _nodeAsDecimal(tradeLineItem, ".//cac:Item/cac:ClassifiedTaxCategory/cbc:Percent", nsmgr, 0).Value,
                NetUnitPrice = _nodeAsDecimal(tradeLineItem, ".//cac:Price/cbc:PriceAmount", nsmgr, 0).Value,
                GrossUnitPrice = 0, // TODO: Find value //GrossUnitPrice = _nodeAsDecimal(tradeLineItem, ".//ram:GrossPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                UnitCode = default(QuantityCodes).FromString(_nodeAsString(tradeLineItem, ".//cbc:InvoicedQuantity/@unitCode", nsmgr)),
                BillingPeriodStart = _nodeAsDateTime(tradeLineItem, ".//cac:InvoicePeriod/cbc:StartDate", nsmgr),
                BillingPeriodEnd = _nodeAsDateTime(tradeLineItem, ".//cac:InvoicePeriod/cbc:EndDate", nsmgr),
            };

            // TODO: Find value //if (tradeLineItem.SelectNodes(".//cac:Item/ram:ApplicableProductCharacteristic", nsmgr) != null)
            //{
            //  foreach (XmlNode applicableProductCharacteristic in tradeLineItem.SelectNodes(".//cac:Item/ram:ApplicableProductCharacteristic", nsmgr))
            //  {
            //    item.ApplicableProductCharacteristics.Add(new ApplicableProductCharacteristic()
            //    {
            //      Description = _nodeAsString(applicableProductCharacteristic, ".//ram:Description", nsmgr),
            //      Value = _nodeAsString(applicableProductCharacteristic, ".//ram:Value", nsmgr),
            //    });
            //  }
            //}

            // TODO: Find value //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument", nsmgr) != null)
            //{
            //  item.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
            //  {
            //    ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID", nsmgr),
            //    IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr)
            //  };
            //}

            // TODO: Find value //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument", nsmgr) != null)
            //{
            //  item.ContractReferencedDocument = new ContractReferencedDocument()
            //  {
            //    ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
            //    IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr)
            //  };
            //}

            // TODO: Find value //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeSettlement", nsmgr) != null)
            //{
            //  XmlNodeList LineTradeSettlementNodes = tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeSettlement", nsmgr).ChildNodes;
            //  foreach (XmlNode LineTradeSettlementNode in LineTradeSettlementNodes)
            //  {
            //    switch (LineTradeSettlementNode.Name)
            //    {
            //      case "ram:ApplicableTradeTax":
            //        //TODO
            //        break;
            //      case "ram:BillingSpecifiedPeriod":
            //        //TODO
            //        break;
            //      case "ram:SpecifiedTradeAllowanceCharge":
            //        //TODO
            //        break;
            //      case "ram:SpecifiedTradeSettlementLineMonetarySummation":
            //        //TODO
            //        break;
            //      case "ram:AdditionalReferencedDocument":
            //        //TODO
            //        break;
            //      case "ram:ReceivableSpecifiedTradeAccountingAccount":
            //        // TODO: Find value //item.ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
            //        //{
            //        //  TradeAccountID = _nodeAsString(LineTradeSettlementNode, "./ram:ID", nsmgr),
            //        //  TradeAccountTypeCode = (AccountingAccountTypeCodes)_nodeAsInt(LineTradeSettlementNode, ".//ram:TypeCode", nsmgr)
            //        //});
            //        break;
            //    }
            //  }
            //}

            item.AssociatedDocument = new AssociatedDocument(_nodeAsString(tradeLineItem, ".//cbc:ID", nsmgr));

            XmlNodeList noteNodes = tradeLineItem.SelectNodes(".//cbc:Note", nsmgr);
            foreach (XmlNode noteNode in noteNodes)
            {
                item.AssociatedDocument.Notes.Add(new Note(
                            content: noteNode.InnerText,
                            subjectCode: SubjectCodes.Unknown,
                            contentCode: ContentCodes.Unknown
                ));
            }

            XmlNodeList appliedTradeAllowanceChargeNodes = tradeLineItem.SelectNodes(".//cac:AllowanceCharge", nsmgr);
            foreach (XmlNode appliedTradeAllowanceChargeNode in appliedTradeAllowanceChargeNodes)
            {
                bool chargeIndicator = _nodeAsBool(appliedTradeAllowanceChargeNode, "./cbc:ChargeIndicator", nsmgr);
                decimal basisAmount = _nodeAsDecimal(appliedTradeAllowanceChargeNode, "./cbc:BaseAmount", nsmgr, 0).Value;
                string basisAmountCurrency = _nodeAsString(appliedTradeAllowanceChargeNode, "./cbc:BaseAmount/@currencyID", nsmgr);
                decimal actualAmount = _nodeAsDecimal(appliedTradeAllowanceChargeNode, "./cbc:Amount", nsmgr, 0).Value;
                string actualAmountCurrency = _nodeAsString(appliedTradeAllowanceChargeNode, "./cbc:Amount/@currencyID", nsmgr);
                string reason = _nodeAsString(appliedTradeAllowanceChargeNode, "./cbc:AllowanceChargeReason", nsmgr);

                item.AddTradeAllowanceCharge(!chargeIndicator, // wichtig: das not (!) beachten
                                                default(CurrencyCodes).FromString(basisAmountCurrency),
                                                basisAmount,
                                                actualAmount,
                                                reason);
            }

            if (item.UnitCode == QuantityCodes.Unknown)
            {
                // UnitCode alternativ aus BilledQuantity extrahieren
                item.UnitCode = default(QuantityCodes).FromString(_nodeAsString(tradeLineItem, ".//cbc:InvoicedQuantity/@unitCode", nsmgr));
            }

            // TODO: Find value //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            //{
            //  item.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            //  {
            //    ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr),
            //    IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr),
            //  };
            //}

            // TODO: Find value //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime", nsmgr) != null)
            //{
            //  item.ActualDeliveryDate = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);
            //}

            //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            //{
            //    item.ContractReferencedDocument = new ContractReferencedDocument()
            //    {
            //        ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
            //        IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr),
            //    };
            //}

            //Get all referenced AND embedded documents
            // TODO: Find value //XmlNodeList referenceNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedLineTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
            //foreach (XmlNode referenceNode in referenceNodes)
            //{
            //  item.AdditionalReferencedDocuments.Add(_getAdditionalReferencedDocument(referenceNode, nsmgr));
            //}

            return item;
        } // !_parseTradeLineItem()        


        private static LegalOrganization _nodeAsLegalOrganization(XmlNode baseNode, string xpath, XmlNamespaceManager nsmgr = null)
        {
            if (baseNode == null)
                return null;
            XmlNode node = baseNode.SelectSingleNode(xpath, nsmgr);
            if (node == null)
                return null;
            var retval = new LegalOrganization()
            {
                ID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(_nodeAsString(node, "cbc:CompanyID/@schemeID", nsmgr)),
                                        _nodeAsString(node, "cbc:CompanyID", nsmgr)),
                TradingBusinessName = _nodeAsString(node, "cbc:RegistrationName", nsmgr),
            };
            return retval;
        }
        private static Party _nodeAsParty(XmlNode baseNode, string xpath, XmlNamespaceManager nsmgr = null)
        {
            if (baseNode == null)
            {
                return null;
            }

            XmlNode node = baseNode.SelectSingleNode(xpath, nsmgr);
            if (node == null)
            {
                return null;
            }

            Party retval = _nodeAsAddressParty(node, $"{xpath}/cac:PostalAddress", nsmgr) ?? new Party();
            var id = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(_nodeAsString(node, "cac:PartyIdentification/cbc:ID/@schemeID", nsmgr)), _nodeAsString(node, "cac:PartyIdentification/cbc:ID", nsmgr));
            if (id.SchemeID == GlobalIDSchemeIdentifiers.GLN)
            {
                retval.ID = new GlobalID();
                retval.GlobalID = id;
            }
            else
            {
                retval.ID = id;
                retval.GlobalID = new GlobalID();
            }
            retval.Name = _nodeAsString(node, "cac:PartyName/cbc:Name", nsmgr);
            retval.SpecifiedLegalOrganization = _nodeAsLegalOrganization(node, "cac:PartyLegalEntity", nsmgr);

            if (string.IsNullOrWhiteSpace(retval.Name))
            {
                retval.Name = _nodeAsString(node, "cac:PartyLegalEntity/cbc:RegistrationName", nsmgr);
            }

            if (string.IsNullOrWhiteSpace(retval.ContactName))
            {
                retval.ContactName = null;
            }

            if (string.IsNullOrEmpty(retval.AddressLine3))
            {
                retval.AddressLine3 = "";
            }

            if (string.IsNullOrEmpty(retval.CountrySubdivisionName))
            {
                retval.CountrySubdivisionName = "";
            }

            if (string.IsNullOrEmpty(retval.City))
            {
                retval.City = "";
            }

            if (string.IsNullOrEmpty(retval.Postcode))
            {
                retval.Postcode = "";
            }

            if (string.IsNullOrEmpty(retval.Street))
            {
                retval.Street = "";
            }

            return retval;
        } // !_nodeAsParty()
        private static Party _nodeAsAddressParty(XmlNode baseNode, string xpath, XmlNamespaceManager nsmgr = null)
        {
            if (baseNode == null)
            {
                return null;
            }

            XmlNode node = baseNode.SelectSingleNode(xpath, nsmgr);
            if (node == null)
            {
                return null;
            }

            Party retval = new Party()
            {
                Street = _nodeAsString(node, "cbc:StreetName", nsmgr),
                AddressLine3 = _nodeAsString(node, "cbc:AdditionalStreetName", nsmgr),
                City = _nodeAsString(node, "cbc:CityName", nsmgr),
                Postcode = _nodeAsString(node, "cbc:PostalZone", nsmgr),
                CountrySubdivisionName = _nodeAsString(node, "cbc:CountrySubentity", nsmgr),
                Country = default(CountryCodes).FromString(_nodeAsString(node, "cac:Country/cbc:IdentificationCode", nsmgr)),
            };
            string addressLine2 = _nodeAsString(node, "cac:AddressLine/cbc:Line", nsmgr);
            if (!string.IsNullOrWhiteSpace(addressLine2))
            {
                if (string.IsNullOrWhiteSpace(retval.AddressLine3))
                {
                    retval.AddressLine3 = addressLine2;
                }
                else if (!string.IsNullOrWhiteSpace(addressLine2) && string.IsNullOrWhiteSpace(retval.ContactName))
                {
                    retval.ContactName = addressLine2;
                }
            }

            return retval;
        } // !_nodeAsAddressParty()
        private static BankAccount _nodeAsBankAccount(XmlNode baseNode, string xpath, XmlNamespaceManager nsmgr = null)
        {
            if (baseNode == null)
            {
                return null;
            }

            XmlNode node = baseNode.SelectSingleNode(xpath, nsmgr);
            if (node == null)
            {
                return null;
            }

            BankAccount retval = new BankAccount()
            {
                Name = _nodeAsString(node, "cbc:Name", nsmgr),
                IBAN = _nodeAsString(node, "cbc:ID", nsmgr),
                BIC = _nodeAsString(node, "cac:FinancialInstitutionBranch/cbc:ID", nsmgr, null),
                ID = ""
            };

            return retval;
        } // !_nodeAsAddressParty()

        private static AdditionalReferencedDocument _getAdditionalReferencedDocument(XmlNode a_oXmlNode, XmlNamespaceManager a_nsmgr)
        {
            string strBase64BinaryData = _nodeAsString(a_oXmlNode, "ram:AttachmentBinaryObject", a_nsmgr);
            return new AdditionalReferencedDocument
            {
                ID = _nodeAsString(a_oXmlNode, "ram:IssuerAssignedID", a_nsmgr),
                TypeCode = default(AdditionalReferencedDocumentTypeCode).FromString(_nodeAsString(a_oXmlNode, "ram:TypeCode", a_nsmgr)),
                Name = _nodeAsString(a_oXmlNode, "ram:Name", a_nsmgr),
                IssueDateTime = _nodeAsDateTime(a_oXmlNode, "ram:FormattedIssueDateTime/qdt:DateTimeString", a_nsmgr),
                AttachmentBinaryObject = !string.IsNullOrWhiteSpace(strBase64BinaryData) ? Convert.FromBase64String(strBase64BinaryData) : null,
                Filename = _nodeAsString(a_oXmlNode, "ram:AttachmentBinaryObject/@filename", a_nsmgr),
                ReferenceTypeCode = default(ReferenceTypeCodes).FromString(_nodeAsString(a_oXmlNode, "ram:ReferenceTypeCode", a_nsmgr))
            };
        }
        private static TaxRegistrationSchemeID _getUncefactTaxSchemeID(string schemeID)
        {
            if (string.IsNullOrWhiteSpace(schemeID)) return TaxRegistrationSchemeID.Unknown;
            switch (schemeID.ToUpper())
            {
                case "ID":
                    return TaxRegistrationSchemeID.FC;
                case "VAT":
                    return TaxRegistrationSchemeID.VA;
            }
            return default(TaxRegistrationSchemeID).FromString(schemeID);
        }
    }
}
