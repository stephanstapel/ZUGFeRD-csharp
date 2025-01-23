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
    internal class InvoiceDescriptor22UBLReader : IInvoiceDescriptorReader
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

            byte[] firstPartOfDocumentBuffer = new byte[1024];
            stream.Read(firstPartOfDocumentBuffer, 0, 1024);
            stream.Position = 0;
            string firstPartOfDocument = System.Text.Encoding.UTF8.GetString(firstPartOfDocumentBuffer);
            bool isInvoice = true;

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.DocumentElement.OwnerDocument.NameTable);

            if ((firstPartOfDocument.IndexOf("<CreditNote", StringComparison.OrdinalIgnoreCase) > -1) ||
                (firstPartOfDocument.IndexOf("<ubl:CreditNote", StringComparison.OrdinalIgnoreCase) > -1))
            {
                isInvoice = false;
            }

            if (isInvoice)
            {
                nsmgr.AddNamespace("ubl", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2");
            }
            else
            {
                nsmgr.AddNamespace("ubl", "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2");
            }

            nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            string typeSelector = "//cbc:InvoiceTypeCode";
            string tradeLineItemSelector = "//cac:InvoiceLine";
            XmlNode baseNode = null;
            if (isInvoice)
            {
                baseNode = doc.SelectSingleNode("/ubl:Invoice", nsmgr);
                if (baseNode == null)
                {
                    baseNode = doc.SelectSingleNode("/Invoice", nsmgr);
                }
            }
            else
            {
                typeSelector = "//cbc:CreditNoteTypeCode";
                tradeLineItemSelector = "//cac:CreditNoteLine";
                baseNode = doc.SelectSingleNode("/ubl:CreditNote", nsmgr);
                if (baseNode == null)
                {
                    baseNode = doc.SelectSingleNode("/CreditNote", nsmgr);
                }
            }

            InvoiceDescriptor retval = new InvoiceDescriptor
            {
                IsTest = XmlUtils.NodeAsBool(doc.DocumentElement, "//cbc:TestIndicator", nsmgr, false),
                BusinessProcess = XmlUtils.NodeAsString(doc.DocumentElement, "//cbc:ProfileID", nsmgr),
                Profile = Profile.XRechnung, //default(Profile).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//ram:GuidelineSpecifiedDocumentContextParameter/ram:ID", nsmgr)),
                InvoiceNo = XmlUtils.NodeAsString(doc.DocumentElement, "//cbc:ID", nsmgr),
                InvoiceDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//cbc:IssueDate", nsmgr),
                Type = default(InvoiceType).FromString(XmlUtils.NodeAsString(doc.DocumentElement, typeSelector, nsmgr))
            };

            foreach (XmlNode node in baseNode.SelectNodes("./cbc:Note", nsmgr))
            {
                string content = XmlUtils.NodeAsString(node, ".", nsmgr);
                if (string.IsNullOrWhiteSpace(content)) continue;
                var contentParts = content.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                string subjectCodeAsString = String.Empty;
                if (contentParts.Length > 1 && contentParts[0].Length == 3)
                {
                    subjectCodeAsString = contentParts[0];
                    content = contentParts[1];
                }
                SubjectCodes subjectCode = default(SubjectCodes).FromString(subjectCodeAsString);
                retval.AddNote(content, subjectCode);
            }

            retval.ReferenceOrderNo = XmlUtils.NodeAsString(doc, "//cbc:BuyerReference", nsmgr);

            retval.Seller = _nodeAsParty(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party", nsmgr);

            if (doc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party/cbc:EndpointID", nsmgr) != null)
            {
                string id = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cbc:EndpointID", nsmgr);
                string schemeID = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cbc:EndpointID/@schemeID", nsmgr);

                var eas = default(ElectronicAddressSchemeIdentifiers).FromString(schemeID);

                if (eas.HasValue)
                {
                    retval.SetSellerElectronicAddress(id, eas.Value);
                }
            }

            foreach (XmlNode node in doc.SelectNodes("//cac:AccountingSupplierParty/cac:Party/cac:PartyTaxScheme", nsmgr))
            {
                string id = XmlUtils.NodeAsString(node, ".//cbc:CompanyID", nsmgr);
                TaxRegistrationSchemeID schemeID = UBLTaxRegistrationSchemeIDMapper.Map(XmlUtils.NodeAsString(node, ".//cac:TaxScheme/cbc:ID", nsmgr));

                retval.AddSellerTaxRegistration(id, schemeID);
            }

            if (doc.SelectSingleNode("//cac:AccountingSupplierParty/cac:Party/cac:Contact", nsmgr) != null)
            {
                retval.SellerContact = new Contact()
                {
                    Name = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:Contact/cbc:Name", nsmgr),
                    OrgUnit = String.Empty, // TODO: Find value //OrgUnit = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/ram:DefinedTradeContact/ram:DepartmentName", nsmgr),
                    PhoneNo = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:Contact/cbc:Telephone", nsmgr),
                    FaxNo = String.Empty, // TODO: Find value //FaxNo = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:Contact/", nsmgr),
                    EmailAddress = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:Contact/cbc:ElectronicMail", nsmgr)
                };
            }

            retval.Buyer = _nodeAsParty(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party", nsmgr);

            if (doc.SelectSingleNode("//cac:AccountingCustomerParty/cac:Party/cbc:EndpointID", nsmgr) != null)
            {
                string id = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cbc:EndpointID", nsmgr);
                string schemeID = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cbc:EndpointID/@schemeID", nsmgr);

                var eas = default(ElectronicAddressSchemeIdentifiers).FromString(schemeID);

                if (eas.HasValue)
                {
                    retval.SetBuyerElectronicAddress(id, eas.Value);
                }
            }

            foreach (XmlNode node in doc.SelectNodes("//cac:AccountingCustomerParty/cac:Party/cac:PartyTaxScheme", nsmgr))
            {
                string id = XmlUtils.NodeAsString(node, ".//cbc:CompanyID", nsmgr);
                TaxRegistrationSchemeID schemeID = UBLTaxRegistrationSchemeIDMapper.Map(XmlUtils.NodeAsString(node, ".//cac:TaxScheme/cbc:ID", nsmgr));

                retval.AddBuyerTaxRegistration(id, schemeID);
            }

            if (doc.SelectSingleNode("//cac:AccountingCustomerParty/cac:Party/cac:Contact", nsmgr) != null)
            {
                retval.BuyerContact = new Contact()
                {
                    Name = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:Name", nsmgr),
                    OrgUnit = String.Empty, // TODO: Find value //OrgUnit = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/ram:DefinedTradeContact/ram:DepartmentName", nsmgr),
                    PhoneNo = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:Telephone", nsmgr),
                    FaxNo = String.Empty, // TODO: Find value //FaxNo = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cac:Contact/", nsmgr),
                    EmailAddress = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:ElectronicMail", nsmgr)
                };
            }

            //Get all referenced and embedded documents (BG-24)
            // TODO //XmlNodeList referencedDocNodes = doc.SelectNodes(".//ram:ApplicableHeaderTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
            //foreach (XmlNode referenceNode in referencedDocNodes)
            //{
            //  retval._AdditionalReferencedDocuments.Add(_readAdditionalReferencedDocument(referenceNode, nsmgr));
            //}

            //-------------------------------------------------
            // hzi: With old implementation only the first document has been read instead of all documents
            //-------------------------------------------------
            //if (doc.SelectSingleNode("//ram:AdditionalReferencedDocument", nsmgr) != null)
            //{
            //    string _issuerAssignedID = XmlUtils.NodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:IssuerAssignedID", nsmgr);
            //    string _typeCode = XmlUtils.NodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:TypeCode", nsmgr);
            //    string _referenceTypeCode = XmlUtils.NodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:ReferenceTypeCode", nsmgr);
            //    string _name = XmlUtils.NodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:Name", nsmgr);
            //    DateTime? _date = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:AdditionalReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr);

            //    if (doc.SelectSingleNode("//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject", nsmgr) != null)
            //    {
            //        string _filename = XmlUtils.NodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject/@filename", nsmgr);
            //        byte[] data = Convert.FromBase64String(XmlUtils.NodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject", nsmgr));

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
                    retval.ShipTo.ID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(deliveryLocationNode, ".//cbc:ID/@schemeID", nsmgr)), XmlUtils.NodeAsString(deliveryLocationNode, ".//cbc:ID", nsmgr));
                    retval.ShipTo.Name = XmlUtils.NodeAsString(deliveryNode, ".//cac:DeliveryParty/cac:PartyName/cbc:Name", nsmgr);
                }
                retval.ActualDeliveryDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//cac:Delivery/cbc:ActualDeliveryDate", nsmgr);
            }
            // TODO: Find value //retval.ShipFrom = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ShipFromTradeParty", nsmgr);

            // TODO: Find value //string _deliveryNoteNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr);
            // TODO: Find value //DateTime? _deliveryNoteDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);

            //if (!_deliveryNoteDate.HasValue)
            //{
            //  _deliveryNoteDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime", nsmgr);
            //}

            //if (_deliveryNoteDate.HasValue || !String.IsNullOrWhiteSpace(_deliveryNoteNo))
            //{
            //  retval.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            //  {
            //    ID = _deliveryNoteNo,
            //    IssueDateTime = _deliveryNoteDate
            //  };
            //}

            // TODO: Find value //retval.Invoicee = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceeTradeParty", nsmgr);
            retval.Payee = _nodeAsParty(doc.DocumentElement, "//cac:PayeeParty", nsmgr);

            retval.PaymentReference = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cbc:PaymentID", nsmgr);
            retval.Currency = default(CurrencyCodes).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//cbc:DocumentCurrencyCode", nsmgr));

            CurrencyCodes optionalTaxCurrency = default(CurrencyCodes).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//cbc:TaxCurrencyCode", nsmgr)); // BT-6
            if (optionalTaxCurrency != CurrencyCodes.Unknown)
            {
                retval.TaxCurrency = optionalTaxCurrency;
            }

            // TODO: Multiple SpecifiedTradeSettlementPaymentMeans can exist for each account/institution (with different SEPA?)
            PaymentMeans tempPaymentMeans = new PaymentMeans()
            {
                TypeCode = default(PaymentMeansTypeCodes).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cbc:PaymentMeansCode", nsmgr)),
                Information = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cbc:PaymentMeansCode/@name", nsmgr),
                SEPACreditorIdentifier = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:AccountingSupplierParty/cac:Party/cac:PartyIdentification/cbc:ID[@schemeID='SEPA']", nsmgr),
                SEPAMandateReference = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cac:PaymentMandate/cbc:ID", nsmgr)
            };

            var financialCardId = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cac:CardAccount/cbc:PrimaryAccountNumberID", nsmgr);
            var financialCardCardholderName = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:PaymentMeans/cac:CardAccount/cbc:HolderName", nsmgr);

            if (!string.IsNullOrWhiteSpace(financialCardId) || !string.IsNullOrWhiteSpace(financialCardCardholderName))
            {
                tempPaymentMeans.FinancialCard = new FinancialCard()
                {
                    Id = financialCardId,
                    CardholderName = financialCardCardholderName
                };
            }

            retval.PaymentMeans = tempPaymentMeans;

            retval.BillingPeriodStart = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//cac:InvoicePeriod/cbc:StartDate", nsmgr);
            retval.BillingPeriodEnd = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//cac:InvoicePeriod/cbc:EndDate", nsmgr);

            XmlNodeList creditorFinancialAccountNodes = doc.SelectNodes("//cac:PaymentMeans/cac:PayeeFinancialAccount", nsmgr);
            foreach (XmlNode node in creditorFinancialAccountNodes)
            {
                retval._AddCreditorFinancialAccount(_nodeAsBankAccount(node, ".", nsmgr));
            }

            XmlNodeList debitorFinancialAccountNodes = doc.SelectNodes("//cac:PaymentMeans/cac:PaymentMandate/cac:PayerFinancialAccount", nsmgr);
            foreach (XmlNode node in debitorFinancialAccountNodes)
            {
                retval._AddDebitorFinancialAccount(_nodeAsBankAccount(node, ".", nsmgr));
            }

            foreach (XmlNode node in doc.SelectNodes("//cac:TaxTotal/cac:TaxSubtotal", nsmgr))
            {
                retval.AddApplicableTradeTax(XmlUtils.NodeAsDecimal(node, "cbc:TaxableAmount", nsmgr, 0).Value,
                                             XmlUtils.NodeAsDecimal(node, "cac:TaxCategory/cbc:Percent", nsmgr, 0).Value,
                                             XmlUtils.NodeAsDecimal(node, "cbc:TaxAmount", nsmgr, 0).Value,
                                             default(TaxTypes).FromString(XmlUtils.NodeAsString(node, "cac:TaxCategory/cac:TaxScheme/cbc:ID", nsmgr)),
                                             default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, "cac:TaxCategory/cbc:ID", nsmgr)),
                                             null,
                                             default(TaxExemptionReasonCodes).FromString(XmlUtils.NodeAsString(node, "cac:TaxCategory/cbc:TaxExemptionReasonCode", nsmgr)),
                                             XmlUtils.NodeAsString(node, "cac:TaxCategory/cbc:TaxExemptionReason", nsmgr)
                                             );
            }

            // As both document level and document item level element have same name
            // only using the //cac:AllowanceCharge gives all the allowances from the file
            // so we specify the node with the parent node
            foreach (XmlNode node in baseNode.SelectNodes("./cac:AllowanceCharge", nsmgr))
            {
                retval.AddTradeAllowanceCharge(!XmlUtils.NodeAsBool(node, ".//cbc:ChargeIndicator", nsmgr), // wichtig: das not (!) beachten
                                               XmlUtils.NodeAsDecimal(node, ".//cbc:BaseAmount", nsmgr, 0).Value,
                                               retval.Currency,
                                               XmlUtils.NodeAsDecimal(node, ".//cbc:Amount", nsmgr, 0).Value,
                                               XmlUtils.NodeAsString(node, ".//cbc:AllowanceChargeReason", nsmgr),
                                               default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//cac:TaxCategory/cac:TaxScheme/cbc:ID", nsmgr)),
                                               default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//cac:TaxCategory/cbc:ID", nsmgr)),
                                               XmlUtils.NodeAsDecimal(node, ".//cac:TaxCategory/cbc:Percent", nsmgr, 0).Value,
                                               EnumExtensions.FromDescription<AllowanceReasonCodes>(XmlUtils.NodeAsString(node, "./cbc:AllowanceChargeReasonCode", nsmgr)));
            }

            // TODO: Find value //foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedLogisticsServiceCharge", nsmgr))
            //{
            //  retval.AddLogisticsServiceCharge(XmlUtils.NodeAsDecimal(node, ".//ram:AppliedAmount", nsmgr, 0).Value,
            //                                   XmlUtils.NodeAsString(node, ".//ram:Description", nsmgr),
            //                                   default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//ram:AppliedTradeTax/ram:TypeCode", nsmgr)),
            //                                   default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:AppliedTradeTax/ram:CategoryCode", nsmgr)),
            //                                   XmlUtils.NodeAsDecimal(node, ".//ram:AppliedTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value);
            //

            foreach (XmlNode invoiceReferencedDocumentNodes in doc.DocumentElement.SelectNodes("//cac:BillingReference/cac:InvoiceDocumentReference", nsmgr))
            {
                retval.AddInvoiceReferencedDocument(
                    XmlUtils.NodeAsString(invoiceReferencedDocumentNodes, "./cbc:ID", nsmgr),
                    XmlUtils.NodeAsDateTime(invoiceReferencedDocumentNodes, "./cbc:IssueDate", nsmgr)
                );
                break; // only one occurrence allowed in UBL
            }

            XmlNode despatchDocumentReferenceIdNode = baseNode.SelectSingleNode("./cac:DespatchDocumentReference/cbc:ID", nsmgr);
            if (despatchDocumentReferenceIdNode != null)
            {
                retval.SetDespatchAdviceReferencedDocument(despatchDocumentReferenceIdNode.InnerText);
            }

            retval.AddTradePaymentTerms(
                description: XmlUtils.NodeAsString(doc.DocumentElement, "//cac:PaymentTerms/cbc:Note", nsmgr),
                dueDate: XmlUtils.NodeAsDateTime(doc.DocumentElement, "//cbc:DueDate", nsmgr)
            );

            retval.LineTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:LineExtensionAmount", nsmgr);
            retval.ChargeTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:ChargeTotalAmount", nsmgr);
            retval.AllowanceTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:AllowanceTotalAmount", nsmgr);
            retval.TaxBasisAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:TaxExclusiveAmount", nsmgr);
            retval.TaxTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:TaxTotal/cbc:TaxAmount", nsmgr);
            retval.GrandTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount", nsmgr);
            retval.RoundingAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:PayableRoundingAmount", nsmgr);
            retval.TotalPrepaidAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:PrepaidAmount", nsmgr);
            retval.DuePayableAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//cac:LegalMonetaryTotal/cbc:PayableAmount", nsmgr);

            // TODO: Find value //foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:ReceivableSpecifiedTradeAccountingAccount", nsmgr))
            //{
            //  retval._ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
            //  {
            //    TradeAccountID = XmlUtils.NodeAsString(node, ".//ram:ID", nsmgr),
            //    TradeAccountTypeCode = (AccountingAccountTypeCodes)_nodeAsInt(node, ".//ram:TypeCode", nsmgr),
            //  });
            //}

            // TODO: Find value //retval.OrderDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//cac:OrderReference/ram:BuyerOrderReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr);
            retval.OrderNo = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:OrderReference/cbc:ID", nsmgr);

            // Read SellerOrderReferencedDocument
            if (doc.SelectSingleNode("//cac:OrderReference/cbc:SalesOrderID", nsmgr) != null)
            {
                retval.SellerOrderReferencedDocument = new SellerOrderReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:OrderReference/cbc:SalesOrderID", nsmgr),
                    // unclear how to map
                    //    IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr),
                    //    LineID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:LineID", nsmgr),
                };
            }

            // Read ContractReferencedDocument
            if (doc.SelectSingleNode("//cac:ContractDocumentReference/cbc:ID", nsmgr) != null)
            {
                retval.ContractReferencedDocument = new ContractReferencedDocument
                {
                    ID = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:ContractDocumentReference/cbc:ID", nsmgr),
                    // TODO: Find value //IssueDateTime = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime", nsmgr)
                };
            }

            // Read AdditionalReferencedDocument
            foreach (XmlNode node in doc.SelectNodes("//cac:AdditionalDocumentReference", nsmgr))
            {
                AdditionalReferencedDocument document = new AdditionalReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(node, ".//cbc:ID", nsmgr),
                    ReferenceTypeCode = default(ReferenceTypeCodes).FromString(XmlUtils.NodeAsString(node, ".//cbc:ID/@schemeID", nsmgr)),
                    TypeCode = default(AdditionalReferencedDocumentTypeCode).FromString(XmlUtils.NodeAsString(node, ".//cbc:DocumentTypeCode", nsmgr)),
                    Name = XmlUtils.NodeAsString(node, ".//cbc:DocumentDescription", nsmgr)
                };

                XmlNode binaryObjectNode = node.SelectSingleNode(".//cac:Attachment/cbc:EmbeddedDocumentBinaryObject", nsmgr);
                if (binaryObjectNode != null)
                {
                    document.Filename = binaryObjectNode.Attributes["filename"]?.InnerText;
                    document.AttachmentBinaryObject = Convert.FromBase64String(binaryObjectNode.InnerText);
                }

                retval.AdditionalReferencedDocuments.Add(document);
            }

            retval.SpecifiedProcuringProject = new SpecifiedProcuringProject
            {
                ID = XmlUtils.NodeAsString(doc.DocumentElement, "//cac:ProjectReference/cbc:ID", nsmgr),
                Name = String.Empty // TODO: Find value //Name = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SpecifiedProcuringProject/ram:Name", nsmgr)
            };

            foreach (XmlNode node in doc.SelectNodes(tradeLineItemSelector, nsmgr))
            {
                retval._AddTradeLineItems(_parseTradeLineItem(node, nsmgr));
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
            long oldStreamPosition = stream.Position;
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                string data = reader.ReadToEnd().Replace(" ", String.Empty).ToLower();
                foreach (string validURI in validURIs)
                {
                    if (data.Contains(string.Format("=\"{0}\"", validURI.ToLower())))
                    {
                        stream.Position = oldStreamPosition;
                        return true;
                    }
                }
            }

            stream.Position = oldStreamPosition;
            return false;
        }

        private static List<TradeLineItem> _parseTradeLineItem(XmlNode tradeLineItem, XmlNamespaceManager nsmgr = null, string parentLineId = null)
        {
            if (tradeLineItem == null)
            {
                return null;
            }

            List<TradeLineItem> resultList = new List<TradeLineItem>();

            string lineId = XmlUtils.NodeAsString(tradeLineItem, ".//cbc:ID", nsmgr);
            TradeLineItem item = new TradeLineItem(lineId)
            {
                GlobalID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(tradeLineItem, "./cac:Item/cac:StandardItemIdentification/cbc:ID/@schemeID", nsmgr)),
                                        XmlUtils.NodeAsString(tradeLineItem, "./cac:Item/cac:StandardItemIdentification/cbc:ID", nsmgr)),
                SellerAssignedID = XmlUtils.NodeAsString(tradeLineItem, "./cac:Item/cac:SellersItemIdentification/cbc:ID", nsmgr),
                BuyerAssignedID = XmlUtils.NodeAsString(tradeLineItem, "./cac:Item/cac:BuyersItemIdentification/cbc:ID", nsmgr),
                Name = XmlUtils.NodeAsString(tradeLineItem, "./cac:Item/cbc:Name", nsmgr),
                Description = XmlUtils.NodeAsString(tradeLineItem, ".//cac:Item/cbc:Description", nsmgr),
                UnitQuantity = XmlUtils.NodeAsDecimal(tradeLineItem, ".//cac:Price/cbc:BaseQuantity", nsmgr, 1),
                BilledQuantity = XmlUtils.NodeAsDecimal(tradeLineItem, ".//cbc:InvoicedQuantity", nsmgr, 0).Value,
                LineTotalAmount = XmlUtils.NodeAsDecimal(tradeLineItem, ".//cbc:LineExtensionAmount", nsmgr, 0),
                TaxCategoryCode = default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//cac:Item/cac:ClassifiedTaxCategory/cbc:ID", nsmgr)),
                TaxType = default(TaxTypes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//cac:Item/cac:ClassifiedTaxCategory/cac:TaxScheme/cbc:ID", nsmgr)),
                TaxPercent = XmlUtils.NodeAsDecimal(tradeLineItem, ".//cac:Item/cac:ClassifiedTaxCategory/cbc:Percent", nsmgr, 0).Value,
                NetUnitPrice = XmlUtils.NodeAsDecimal(tradeLineItem, ".//cac:Price/cbc:PriceAmount", nsmgr, 0).Value,
                GrossUnitPrice = 0, // TODO: Find value //GrossUnitPrice = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:GrossPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                UnitCode = default(QuantityCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//cbc:InvoicedQuantity/@unitCode", nsmgr)),
                BillingPeriodStart = XmlUtils.NodeAsDateTime(tradeLineItem, ".//cac:InvoicePeriod/cbc:StartDate", nsmgr),
                BillingPeriodEnd = XmlUtils.NodeAsDateTime(tradeLineItem, ".//cac:InvoicePeriod/cbc:EndDate", nsmgr),
            };

            if (!String.IsNullOrWhiteSpace(parentLineId))
            {
                item.SetParentLineId(parentLineId);
            }

            // Read ApplicableProductCharacteristic
            if (tradeLineItem.SelectNodes(".//cac:Item/cac:CommodityClassification", nsmgr) != null)
            {
                foreach (XmlNode commodityClassification in tradeLineItem.SelectNodes(".//cac:Item/cac:CommodityClassification/cbc:ItemClassificationCode", nsmgr))
                {
                    DesignatedProductClassificationClassCodes listID = default(DesignatedProductClassificationClassCodes).FromString(XmlUtils.NodeAsString(commodityClassification, "./@listID", nsmgr));
                    item.AddDesignatedProductClassification(
                        listID,
                        XmlUtils.NodeAsString(commodityClassification, "./@listVersionID", nsmgr),
                        commodityClassification.InnerText,
                        String.Empty // no name in Peppol Billing!
                        );
                }
            }

            // Read ApplicableProductCharacteristic
            if (tradeLineItem.SelectNodes(".//cac:Item/cac:AdditionalItemProperty", nsmgr) != null)
            {
                foreach (XmlNode applicableProductCharacteristic in tradeLineItem.SelectNodes(".//cac:Item/cac:AdditionalItemProperty", nsmgr))
                {
                    item.ApplicableProductCharacteristics.Add(new ApplicableProductCharacteristic()
                    {
                        Description = XmlUtils.NodeAsString(applicableProductCharacteristic, ".//cbc:Name", nsmgr),
                        Value = XmlUtils.NodeAsString(applicableProductCharacteristic, ".//cbc:Value", nsmgr),
                    });
                }
            }

            // Read BuyerOrderReferencedDocument
            if (tradeLineItem.SelectSingleNode("cac:OrderLineReference", nsmgr) != null)
            {
                item.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(tradeLineItem, ".//cbc:IssuerAssignedID", nsmgr),
                    IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//cac:FormattedIssueDateTime/cbc:DateTimeString", nsmgr),
                    LineID = XmlUtils.NodeAsString(tradeLineItem, ".//cbc:LineID", nsmgr),
                };
            }

            // Read AdditionalReferencedDocument
            foreach (XmlNode node in tradeLineItem.SelectNodes("//cac:DocumentReference", nsmgr))
            {
                AdditionalReferencedDocument document = new AdditionalReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(node, ".//cbc:ID", nsmgr),
                    ReferenceTypeCode = default(ReferenceTypeCodes).FromString(XmlUtils.NodeAsString(node, ".//cbc:ID/@schemeID", nsmgr)),
                    TypeCode = default(AdditionalReferencedDocumentTypeCode).FromString(XmlUtils.NodeAsString(node, ".//cbc:DocumentTypeCode", nsmgr)),
                    Name = XmlUtils.NodeAsString(node, ".//cbc:DocumentDescription", nsmgr)
                };

                XmlNode binaryObjectNode = node.SelectSingleNode(".//cac:Attachment/cbc:EmbeddedDocumentBinaryObject", nsmgr);
                if (binaryObjectNode != null)
                {
                    document.Filename = binaryObjectNode.Attributes["filename"]?.InnerText;
                    document.AttachmentBinaryObject = Convert.FromBase64String(binaryObjectNode.InnerText);
                }

                item._AdditionalReferencedDocuments.Add(document);
            }

            //Read IncludedReferencedProducts
            //TODO:

            // TODO: Find value //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument", nsmgr) != null)
            //{
            //  item.ContractReferencedDocument = new ContractReferencedDocument()
            //  {
            //    ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
            //    IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr)
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
            //        // TODO: Find value //item._ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
            //        //{
            //        //  TradeAccountID = XmlUtils.NodeAsString(LineTradeSettlementNode, "./ram:ID", nsmgr),
            //        //  TradeAccountTypeCode = (AccountingAccountTypeCodes)_nodeAsInt(LineTradeSettlementNode, ".//ram:TypeCode", nsmgr)
            //        //});
            //        break;
            //    }
            //  }
            //}

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
                bool chargeIndicator = XmlUtils.NodeAsBool(appliedTradeAllowanceChargeNode, "./cbc:ChargeIndicator", nsmgr);
                decimal basisAmount = XmlUtils.NodeAsDecimal(appliedTradeAllowanceChargeNode, "./cbc:BaseAmount", nsmgr, 0).Value;
                string basisAmountCurrency = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./cbc:BaseAmount/@currencyID", nsmgr);
                decimal actualAmount = XmlUtils.NodeAsDecimal(appliedTradeAllowanceChargeNode, "./cbc:Amount", nsmgr, 0).Value;
                string actualAmountCurrency = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./cbc:Amount/@currencyID", nsmgr);
                string reason = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./cbc:AllowanceChargeReason", nsmgr);
                string reasonCode = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./cbc:AllowanceChargeReasonCode", nsmgr);

                item.AddTradeAllowanceCharge(!chargeIndicator, // wichtig: das not (!) beachten
                                                default(CurrencyCodes).FromString(basisAmountCurrency),
                                                basisAmount,
                                                actualAmount,
                                                reason,
                                                EnumExtensions.FromDescription<AllowanceReasonCodes>(reasonCode));
            }

            if (item.UnitCode == QuantityCodes.Unknown)
            {
                // UnitCode alternativ aus BilledQuantity extrahieren
                item.UnitCode = default(QuantityCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//cbc:InvoicedQuantity/@unitCode", nsmgr));
            }

            // TODO: Find value //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            //{
            //  item.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
            //  {
            //    ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr),
            //    IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr),
            //  };
            //}

            // TODO: Find value //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime", nsmgr) != null)
            //{
            //  item.ActualDeliveryDate = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);
            //}

            //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            //{
            //    item.ContractReferencedDocument = new ContractReferencedDocument()
            //    {
            //        ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
            //        IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr),
            //    };
            //}

            //Get all referenced AND embedded documents
            // TODO: Find value //XmlNodeList referenceNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedLineTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
            //foreach (XmlNode referenceNode in referenceNodes)
            //{
            //  item._AdditionalReferencedDocuments.Add(_readAdditionalReferencedDocument(referenceNode, nsmgr));
            //}

            //Add main item to result list
            resultList.Add(item);

            //Find sub invoice lines recursively
            //Note that selectnodes also select the sub invoice line from other nodes
            XmlNodeList subInvoiceLineNodes = tradeLineItem.SelectNodes(".//cac:SubInvoiceLine", nsmgr);
            foreach (XmlNode subInvoiceLineNode in subInvoiceLineNodes)
            {
                List<TradeLineItem> parseResultList = _parseTradeLineItem(subInvoiceLineNode, nsmgr, item.AssociatedDocument.LineID);
                foreach (TradeLineItem resultItem in parseResultList)
                {
                    //Don't add nodes that are already in the resultList
                    if (!resultList.Any(t => t.AssociatedDocument.LineID == resultItem.AssociatedDocument.LineID))
                    {
                        resultList.Add(resultItem);
                    }
                }
            }

            return resultList;
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
                ID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(node, "cbc:CompanyID/@schemeID", nsmgr)),
                                        XmlUtils.NodeAsString(node, "cbc:CompanyID", nsmgr)),
                TradingBusinessName = XmlUtils.NodeAsString(node, "cbc:RegistrationName", nsmgr),
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
            var id = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(node, "cac:PartyIdentification/cbc:ID/@schemeID", nsmgr)), XmlUtils.NodeAsString(node, "cac:PartyIdentification/cbc:ID", nsmgr));
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
            retval.Name = XmlUtils.NodeAsString(node, "cac:PartyName/cbc:Name", nsmgr);
            retval.SpecifiedLegalOrganization = _nodeAsLegalOrganization(node, "cac:PartyLegalEntity", nsmgr);

            if (string.IsNullOrWhiteSpace(retval.Name))
            {
                retval.Name = XmlUtils.NodeAsString(node, "cac:PartyLegalEntity/cbc:RegistrationName", nsmgr);
            }

            if (string.IsNullOrWhiteSpace(retval.ContactName))
            {
                retval.ContactName = null;
            }

            if (string.IsNullOrWhiteSpace(retval.AddressLine3))
            {
                retval.AddressLine3 = String.Empty;
            }

            if (string.IsNullOrWhiteSpace(retval.CountrySubdivisionName))
            {
                retval.CountrySubdivisionName = String.Empty;
            }

            if (string.IsNullOrWhiteSpace(retval.City))
            {
                retval.City = String.Empty;
            }

            if (string.IsNullOrWhiteSpace(retval.Postcode))
            {
                retval.Postcode = String.Empty;
            }

            if (string.IsNullOrWhiteSpace(retval.Street))
            {
                retval.Street = String.Empty;
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
                Street = XmlUtils.NodeAsString(node, "cbc:StreetName", nsmgr),
                AddressLine3 = XmlUtils.NodeAsString(node, "cbc:AdditionalStreetName", nsmgr),
                City = XmlUtils.NodeAsString(node, "cbc:CityName", nsmgr),
                Postcode = XmlUtils.NodeAsString(node, "cbc:PostalZone", nsmgr),
                CountrySubdivisionName = XmlUtils.NodeAsString(node, "cbc:CountrySubentity", nsmgr),
                Country = default(CountryCodes).FromString(XmlUtils.NodeAsString(node, "cac:Country/cbc:IdentificationCode", nsmgr)),
            };
            string addressLine2 = XmlUtils.NodeAsString(node, "cac:AddressLine/cbc:Line", nsmgr);
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
                Name = XmlUtils.NodeAsString(node, "cbc:Name", nsmgr),
                IBAN = XmlUtils.NodeAsString(node, "cbc:ID", nsmgr),
                BIC = XmlUtils.NodeAsString(node, "cac:FinancialInstitutionBranch/cbc:ID", nsmgr, null),
                BankName = XmlUtils.NodeAsString(node, "cac:FinancialInstitutionBranch/cbc:Name", nsmgr, null),
                ID = String.Empty
            };

            return retval;
        } // !_nodeAsAddressParty()

        private static AdditionalReferencedDocument _getAdditionalReferencedDocument(XmlNode node, XmlNamespaceManager nsmgr)
        {
            string strBase64BinaryData = XmlUtils.NodeAsString(node, "ram:AttachmentBinaryObject", nsmgr);
            return new AdditionalReferencedDocument
            {
                ID = XmlUtils.NodeAsString(node, "ram:IssuerAssignedID", nsmgr),
                TypeCode = default(AdditionalReferencedDocumentTypeCode).FromString(XmlUtils.NodeAsString(node, "ram:TypeCode", nsmgr)),
                Name = XmlUtils.NodeAsString(node, "ram:Name", nsmgr),
                IssueDateTime = XmlUtils.NodeAsDateTime(node, "ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr),
                AttachmentBinaryObject = !string.IsNullOrWhiteSpace(strBase64BinaryData) ? Convert.FromBase64String(strBase64BinaryData) : null,
                Filename = XmlUtils.NodeAsString(node, "ram:AttachmentBinaryObject/@filename", nsmgr),
                ReferenceTypeCode = default(ReferenceTypeCodes).FromString(XmlUtils.NodeAsString(node, "ram:ReferenceTypeCode", nsmgr))
            };
        }
    }
}
