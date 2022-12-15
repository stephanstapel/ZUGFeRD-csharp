﻿/*
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
using System.Xml;
using System.Xml.XPath;
using System.IO;
using ZUGFeRD;
using System.Xml.Linq;

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor21Reader : IInvoiceDescriptorReader
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
            nsmgr.AddNamespace("qdt", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            nsmgr.AddNamespace("a", "urn:un:unece:uncefact:data:standard:QualifiedDataType:100");
            nsmgr.AddNamespace("rsm", "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100");
            nsmgr.AddNamespace("ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100");
            nsmgr.AddNamespace("udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100");

            InvoiceDescriptor retval = new InvoiceDescriptor
            {
                IsTest = _nodeAsBool(doc.DocumentElement, "//rsm:ExchangedDocumentContext/ram:TestIndicator/udt:Indicator", nsmgr),
                Profile = default(Profile).FromString(_nodeAsString(doc.DocumentElement, "//ram:GuidelineSpecifiedDocumentContextParameter/ram:ID", nsmgr)),
                Type = default(InvoiceType).FromString(_nodeAsString(doc.DocumentElement, "//rsm:ExchangedDocument/ram:TypeCode", nsmgr)),
                InvoiceNo = _nodeAsString(doc.DocumentElement, "//rsm:ExchangedDocument/ram:ID", nsmgr),
                InvoiceDate = _nodeAsDateTime(doc.DocumentElement, "//rsm:ExchangedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr)
            };

            foreach (XmlNode node in doc.SelectNodes("//rsm:ExchangedDocument/ram:IncludedNote", nsmgr))
            {
                string content = _nodeAsString(node, ".//ram:Content", nsmgr);
                string _subjectCode = _nodeAsString(node, ".//ram:SubjectCode", nsmgr);
                SubjectCodes subjectCode = default(SubjectCodes).FromString(_subjectCode);
                retval.AddNote(content, subjectCode);
            }

            retval.ReferenceOrderNo = _nodeAsString(doc, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerReference", nsmgr);

            retval.Seller = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty", nsmgr);
            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration", nsmgr))
            {
                string id = _nodeAsString(node, ".//ram:ID", nsmgr);
                string schemeID = _nodeAsString(node, ".//ram:ID/@schemeID", nsmgr);

                retval.AddSellerTaxRegistration(id, default(TaxRegistrationSchemeID).FromString(schemeID));
            }

            if (doc.SelectSingleNode("//ram:SellerTradeParty/ram:DefinedTradeContact", nsmgr) != null)
            {
                retval.SellerContact = new Contact()
                {
                    Name = _nodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:PersonName", nsmgr),
                    OrgUnit = _nodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:DepartmentName", nsmgr),
                    PhoneNo = _nodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:TelephoneUniversalCommunication/ram:CompleteNumber", nsmgr),
                    FaxNo = _nodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:FaxUniversalCommunication/ram:CompleteNumber", nsmgr),
                    EmailAddress = _nodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:EmailURIUniversalCommunication/ram:URIID", nsmgr)
                };
            }

            retval.Buyer = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty", nsmgr);
            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedTaxRegistration", nsmgr))
            {
                string id = _nodeAsString(node, ".//ram:ID", nsmgr);
                string schemeID = _nodeAsString(node, ".//ram:ID/@schemeID", nsmgr);

                retval.AddBuyerTaxRegistration(id, default(TaxRegistrationSchemeID).FromString(schemeID));
            }

            if (doc.SelectSingleNode("//ram:BuyerTradeParty/ram:DefinedTradeContact", nsmgr) != null)
            {
                retval.BuyerContact = new Contact()
                {
                    Name = _nodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:PersonName", nsmgr),
                    OrgUnit = _nodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/DefinedTradeContact/ram:DepartmentName", nsmgr),
                    PhoneNo = _nodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:TelephoneUniversalCommunication/ram:CompleteNumber", nsmgr),
                    FaxNo = _nodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:FaxUniversalCommunication/ram:CompleteNumber", nsmgr),
                    EmailAddress = _nodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:EmailURIUniversalCommunication/ram:URIID", nsmgr)
                };
            }

            // TODO: Read SellerOrderReferencedDocument
            // TODO: Read BuyerOrderReferencedDocument
            // TODO: Read ContractReferencedDocument

            if (doc.SelectSingleNode("//ram:AdditionalReferencedDocument", nsmgr) != null)
            {
                string _issuerAssignedID = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:IssuerAssignedID", nsmgr);
                string _typeCode = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:TypeCode", nsmgr);
                string _referenceTypeCode = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:ReferenceTypeCode", nsmgr);
                string _name = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:Name", nsmgr);
                DateTime? _date = _nodeAsDateTime(doc.DocumentElement, "//ram:AdditionalReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr);

                if (doc.SelectSingleNode("//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject", nsmgr) != null)
                {
                    string _filename = _nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject/@filename", nsmgr);
                    byte[] data = Convert.FromBase64String(_nodeAsString(doc, "//ram:AdditionalReferencedDocument/ram:AttachmentBinaryObject", nsmgr));

                    retval.AddAdditionalReferencedDocument(id: _issuerAssignedID,
                                                           typeCode: default(AdditionalReferencedDocumentTypeCode).FromString(_typeCode),
                                                           issueDateTime: _date,                                                           
                                                           referenceTypeCode: default(ReferenceTypeCodes).FromString(_referenceTypeCode),
                                                           name: _name,
                                                           attachmentBinaryObject: data,
                                                           filename: _filename);
                }
                else
                {
                    retval.AddAdditionalReferencedDocument(id: _issuerAssignedID,
                                                           typeCode: default(AdditionalReferencedDocumentTypeCode).FromString(_typeCode),
                                                           issueDateTime: _date,                                                           
                                                           referenceTypeCode: default(ReferenceTypeCodes).FromString(_referenceTypeCode),
                                                           name: _name);
                }
            }


            retval.ShipTo = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ShipToTradeParty", nsmgr);
            retval.ShipFrom = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ShipFromTradeParty", nsmgr);
            retval.ActualDeliveryDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);

            string _deliveryNoteNo = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr);
            DateTime? _deliveryNoteDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);

            if (!_deliveryNoteDate.HasValue)
            {
                _deliveryNoteDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime", nsmgr);
            }

            if (_deliveryNoteDate.HasValue || !String.IsNullOrEmpty(_deliveryNoteNo))
            {
                retval.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
                {
                    ID = _deliveryNoteNo,
                    IssueDateTime = _deliveryNoteDate
                };
            }

            retval.Invoicee = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceeTradeParty", nsmgr);
            retval.Payee = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:PayeeTradeParty", nsmgr);

            retval.PaymentReference = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:PaymentReference", nsmgr);
            retval.Currency = default(CurrencyCodes).FromString(_nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceCurrencyCode", nsmgr));

            // TODO: Multiple SpecifiedTradeSettlementPaymentMeans can exist for each account/institution (with different SEPA?)
            PaymentMeans _tempPaymentMeans = new PaymentMeans()
            {
                TypeCode = default(PaymentMeansTypeCodes).FromString(_nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:TypeCode", nsmgr)),
                Information = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:Information", nsmgr),
                SEPACreditorIdentifier = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:CreditorReferenceID", nsmgr),
                SEPAMandateReference = _nodeAsString(doc.DocumentElement, "//ram:SpecifiedTradePaymentTerms/ram:DirectDebitMandateID", nsmgr)
            };

            var financialCardId = _nodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:ApplicableTradeSettlementFinancialCard/ram:ID", nsmgr);
            var financialCardCardholderName = _nodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:ApplicableTradeSettlementFinancialCard/ram:CardholderName", nsmgr);

            if (!string.IsNullOrEmpty(financialCardId) || !string.IsNullOrEmpty(financialCardCardholderName))
            {
                _tempPaymentMeans.FinancialCard = new FinancialCard()
                {
                    Id = financialCardId,
                    CardholderName = financialCardCardholderName
                };
            }

            retval.PaymentMeans = _tempPaymentMeans;

            retval.BillingPeriodStart = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:BillingSpecifiedPeriod/ram:StartDateTime", nsmgr);
            retval.BillingPeriodEnd = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:BillingSpecifiedPeriod/ram:EndDateTime", nsmgr);

            XmlNodeList creditorFinancialAccountNodes = doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeePartyCreditorFinancialAccount", nsmgr);
            XmlNodeList creditorFinancialInstitutions = doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeeSpecifiedCreditorFinancialInstitution", nsmgr);

            int numberOfAccounts = creditorFinancialAccountNodes.Count > creditorFinancialInstitutions.Count ? creditorFinancialAccountNodes.Count : creditorFinancialInstitutions.Count;
            for (int i = 0; i < numberOfAccounts; i++)
            {
                BankAccount _account = new BankAccount();
                retval.CreditorBankAccounts.Add(_account);
            }

            for (int i = 0; i < creditorFinancialAccountNodes.Count; i++)
            {
                retval.CreditorBankAccounts[i].ID = _nodeAsString(creditorFinancialAccountNodes[i], ".//ram:ProprietaryID", nsmgr);
                retval.CreditorBankAccounts[i].IBAN = _nodeAsString(creditorFinancialAccountNodes[i], ".//ram:IBANID", nsmgr);
            }

            for (int i = 0; i < creditorFinancialInstitutions.Count; i++)
            {
                retval.CreditorBankAccounts[i].BIC = _nodeAsString(creditorFinancialInstitutions[i], ".//ram:BICID", nsmgr);
                retval.CreditorBankAccounts[i].Bankleitzahl = _nodeAsString(creditorFinancialInstitutions[i], ".//ram:GermanBankleitzahlID", nsmgr);
                retval.CreditorBankAccounts[i].BankName = _nodeAsString(creditorFinancialInstitutions[i], ".//ram:Name", nsmgr);
                retval.CreditorBankAccounts[i].Name = _nodeAsString(creditorFinancialInstitutions[i], ".//ram:AccountName", nsmgr);
            }            

            var specifiedTradeSettlementPaymentMeansNodes = doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans", nsmgr);

            foreach (var specifiedTradeSettlementPaymentMeansNode in specifiedTradeSettlementPaymentMeansNodes.OfType<XmlNode>())
            {
                var payerPartyDebtorFinancialAccountNode = specifiedTradeSettlementPaymentMeansNode.SelectSingleNode("ram:PayerPartyDebtorFinancialAccount", nsmgr);
                
                if (payerPartyDebtorFinancialAccountNode == null)
                {
                    continue;
                }
                
                var _account = new BankAccount()
                {
                    ID = _nodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:ProprietaryID", nsmgr),
                    IBAN = _nodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:IBANID", nsmgr),
                    Bankleitzahl = _nodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:GermanBankleitzahlID", nsmgr),
                    BankName = _nodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:Name", nsmgr),
                };

                var payerSpecifiedDebtorFinancialInstitutionNode = specifiedTradeSettlementPaymentMeansNode.SelectSingleNode("ram:PayerSpecifiedDebtorFinancialInstitution", nsmgr);
                if (payerSpecifiedDebtorFinancialInstitutionNode != null)
                    _account.BIC = _nodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:BICID", nsmgr);

                retval.DebitorBankAccounts.Add(_account);
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:ApplicableTradeTax", nsmgr))
            {
                retval.AddApplicableTradeTax(_nodeAsDecimal(node, ".//ram:BasisAmount", nsmgr, 0).Value,
                                             _nodeAsDecimal(node, ".//ram:RateApplicablePercent", nsmgr, 0).Value,
                                             default(TaxTypes).FromString(_nodeAsString(node, ".//ram:TypeCode", nsmgr)),
                                             default(TaxCategoryCodes).FromString(_nodeAsString(node, ".//ram:CategoryCode", nsmgr)),
                                             0,
                                             default(TaxExemptionReasonCodes).FromString(_nodeAsString(node, ".//ram:ExemptionReasonCode", nsmgr)),
                                             _nodeAsString(node, ".//ram:ExemptionReason", nsmgr));
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedTradeAllowanceCharge", nsmgr))
            {
                retval.AddTradeAllowanceCharge(!_nodeAsBool(node, ".//ram:ChargeIndicator", nsmgr), // wichtig: das not (!) beachten
                                               _nodeAsDecimal(node, ".//ram:BasisAmount", nsmgr, 0).Value,
                                               retval.Currency,
                                               _nodeAsDecimal(node, ".//ram:ActualAmount", nsmgr, 0).Value,
                                               _nodeAsString(node, ".//ram:Reason", nsmgr),
                                               default(TaxTypes).FromString(_nodeAsString(node, ".//ram:CategoryTradeTax/ram:TypeCode", nsmgr)),
                                               default(TaxCategoryCodes).FromString(_nodeAsString(node, ".//ram:CategoryTradeTax/ram:CategoryCode", nsmgr)),
                                               _nodeAsDecimal(node, ".//ram:CategoryTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value);
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedLogisticsServiceCharge", nsmgr))
            {
                retval.AddLogisticsServiceCharge(_nodeAsDecimal(node, ".//ram:AppliedAmount", nsmgr, 0).Value,
                                                 _nodeAsString(node, ".//ram:Description", nsmgr),
                                                 default(TaxTypes).FromString(_nodeAsString(node, ".//ram:AppliedTradeTax/ram:TypeCode", nsmgr)),
                                                 default(TaxCategoryCodes).FromString(_nodeAsString(node, ".//ram:AppliedTradeTax/ram:CategoryCode", nsmgr)),
                                                 _nodeAsDecimal(node, ".//ram:AppliedTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value);
            }

            retval.InvoiceReferencedDocument = new InvoiceReferencedDocument()
            {
                ID = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceReferencedDocument/ram:IssuerAssignedID", nsmgr),
                IssueDateTime = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceReferencedDocument/ram:FormattedIssueDateTime", nsmgr)
            };

            retval.PaymentTerms = new PaymentTerms()
            {
                Description = _nodeAsString(doc.DocumentElement, "//ram:SpecifiedTradePaymentTerms/ram:Description", nsmgr),
                DueDate = _nodeAsDateTime(doc.DocumentElement, "//ram:SpecifiedTradePaymentTerms/ram:DueDateDateTime", nsmgr)
            };

            retval.LineTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:LineTotalAmount", nsmgr, 0).Value;
            retval.ChargeTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:ChargeTotalAmount", nsmgr, null);
            retval.AllowanceTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:AllowanceTotalAmount", nsmgr, null);
            retval.TaxBasisAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxBasisTotalAmount", nsmgr, null);
            retval.TaxTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount", nsmgr, 0).Value;
            retval.GrandTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:GrandTotalAmount", nsmgr, 0).Value;
            retval.RoundingAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:RoundingAmount", nsmgr, 0).Value;
            retval.TotalPrepaidAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TotalPrepaidAmount", nsmgr, null);
            retval.DuePayableAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:DuePayableAmount", nsmgr, 0).Value;

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:ReceivableSpecifiedTradeAccountingAccount", nsmgr))
            {
                retval.ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
                {
                    TradeAccountID = _nodeAsString(node, ".//ram:ID", nsmgr),
                    TradeAccountTypeCode = (AccountingAccountTypeCodes)_nodeAsInt(node, ".//ram:TypeCode", nsmgr),
                });
            }

            retval.OrderDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);
            if (!retval.OrderDate.HasValue)
            {
                retval.OrderDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime", nsmgr);
            }
            retval.OrderNo = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID", nsmgr);

            retval.ContractReferencedDocument = new ContractReferencedDocument
            {
                ID = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
                IssueDateTime = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime", nsmgr)
            };

            retval.SpecifiedProcuringProject = new SpecifiedProcuringProject
            {
                ID = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument/ram:ID", nsmgr),
                Name = _nodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument/ram:Name", nsmgr)
            };

            foreach (XmlNode node in doc.SelectNodes("//ram:IncludedSupplyChainTradeLineItem", nsmgr))
            {
                retval.TradeLineItems.Add(_parseTradeLineItem(node, nsmgr));
            }
            return retval;
        } // !Load()        


        public override bool IsReadableByThisReaderVersion(Stream stream)
        {
            List<string> validURIs = new List<string>()
                {
                    "urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", // Factur-X 1.03 EXTENDED
                    "urn:cen.eu:en16931:2017",  // Profil EN 16931 (COMFORT)
                    "urn:cen.eu:en16931:2017#compliant#urn:factur-x.eu:1p0:basic", // BASIC
                    "urn:factur-x.eu:1p0:basicwl", // BASIC WL
                    "urn:factur-x.eu:1p0:minimum", // MINIMUM
                    "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_1.2", // XRechnung 1.2
                    "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.1", // XRechnung 2.1
                    "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.2" // XRechnung 2.2
                };

            return _IsReadableByThisReaderVersion(stream, validURIs);
        } // !IsReadableByThisReaderVersion()


        private static TradeLineItem _parseTradeLineItem(XmlNode tradeLineItem, XmlNamespaceManager nsmgr = null)
        {
            if (tradeLineItem == null)
            {
                return null;
            }

            TradeLineItem item = new TradeLineItem()
            {
                LineID = _nodeAsString(tradeLineItem, ".//ram:AssociatedDocumentLineDocument/ram:LineID", nsmgr),
                GlobalID = new GlobalID(_nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:GlobalID/@schemeID", nsmgr),
                                        _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:GlobalID", nsmgr)),
                SellerAssignedID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:SellerAssignedID", nsmgr),
                BuyerAssignedID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:BuyerAssignedID", nsmgr),
                Name = _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:Name", nsmgr),
                Description = _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:Description", nsmgr),
                UnitQuantity = _nodeAsDecimal(tradeLineItem, ".//ram:BasisQuantity", nsmgr, 1),
                BilledQuantity = _nodeAsDecimal(tradeLineItem, ".//ram:BilledQuantity", nsmgr, 0).Value,
                LineTotalAmount = _nodeAsDecimal(tradeLineItem, ".//ram:LineTotalAmount", nsmgr, 0),
                TaxCategoryCode = default(TaxCategoryCodes).FromString(_nodeAsString(tradeLineItem, ".//ram:ApplicableTradeTax/ram:CategoryCode", nsmgr)),
                TaxType = default(TaxTypes).FromString(_nodeAsString(tradeLineItem, ".//ram:ApplicableTradeTax/ram:TypeCode", nsmgr)),
                TaxPercent = _nodeAsDecimal(tradeLineItem, ".//ram:ApplicableTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value,
                NetUnitPrice = _nodeAsDecimal(tradeLineItem, ".//ram:NetPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                GrossUnitPrice = _nodeAsDecimal(tradeLineItem, ".//ram:GrossPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                UnitCode = default(QuantityCodes).FromString(_nodeAsString(tradeLineItem, ".//ram:BasisQuantity/@unitCode", nsmgr)),
                BillingPeriodStart = _nodeAsDateTime(tradeLineItem, ".//ram:BillingSpecifiedPeriod/ram:StartDateTime/udt:DateTimeString", nsmgr),
                BillingPeriodEnd = _nodeAsDateTime(tradeLineItem, ".//ram:BillingSpecifiedPeriod/ram:EndDateTime/udt:DateTimeString", nsmgr),
            };

            if (tradeLineItem.SelectNodes(".//ram:SpecifiedTradeProduct/ram:ApplicableProductCharacteristic", nsmgr) != null)
            {
                foreach (XmlNode applicableProductCharacteristic in tradeLineItem.SelectNodes(".//ram:SpecifiedTradeProduct/ram:ApplicableProductCharacteristic", nsmgr))
                {
                    item.ApplicableProductCharacteristics.Add(new ApplicableProductCharacteristic()
                    {
                        Description = _nodeAsString(applicableProductCharacteristic, ".//ram:Description", nsmgr),
                        Value = _nodeAsString(applicableProductCharacteristic, ".//ram:Value", nsmgr),
                    });
                }
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument", nsmgr) != null)
            {
                item.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
                {
                    ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime/qdt:DateTimeString", nsmgr)
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument", nsmgr) != null)
            {
                item.ContractReferencedDocument = new ContractReferencedDocument()
                {
                    ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssueDateTime/qdt:DateTimeString", nsmgr)
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeSettlement", nsmgr) != null)
            {
                XmlNodeList LineTradeSettlementNodes = tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeSettlement", nsmgr).ChildNodes;
                foreach (XmlNode LineTradeSettlementNode in LineTradeSettlementNodes)
                {
                    switch (LineTradeSettlementNode.Name)
                    {
                        case "ram:ApplicableTradeTax":
                            //TODO
                            break;
                        case "ram:BillingSpecifiedPeriod":
                            //TODO
                            break;
                        case "ram:SpecifiedTradeAllowanceCharge":
                            //TODO
                            break;
                        case "ram:SpecifiedTradeSettlementLineMonetarySummation":
                            //TODO
                            break;
                        case "ram:AdditionalReferencedDocument":
                            //TODO
                            break;
                        case "ram:ReceivableSpecifiedTradeAccountingAccount":
                            item.ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
                            {
                                TradeAccountID = _nodeAsString(LineTradeSettlementNode, "./ram:ID", nsmgr),
                                TradeAccountTypeCode = (AccountingAccountTypeCodes)_nodeAsInt(LineTradeSettlementNode, ".//ram:TypeCode", nsmgr)
                            });
                            break;
                    }
                }
            }

            if (tradeLineItem.SelectSingleNode(".//ram:AssociatedDocumentLineDocument", nsmgr) != null)
            {
                item.AssociatedDocument = new AssociatedDocument(_nodeAsString(tradeLineItem, ".//ram:AssociatedDocumentLineDocument/ram:LineID", nsmgr));

                XmlNodeList noteNodes = tradeLineItem.SelectNodes(".//ram:AssociatedDocumentLineDocument/ram:IncludedNote", nsmgr);
                foreach (XmlNode noteNode in noteNodes)
                {
                    item.AssociatedDocument.Notes.Add(new Note(
                                content: _nodeAsString(noteNode, ".//ram:Content", nsmgr),
                                subjectCode: default(SubjectCodes).FromString(_nodeAsString(noteNode, ".//ram:SubjectCode", nsmgr)),
                                contentCode: ContentCodes.Unknown
                    ));
                }

            }

            XmlNodeList appliedTradeAllowanceChargeNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedSupplyChainTradeAgreement/ram:GrossPriceProductTradePrice/ram:AppliedTradeAllowanceCharge", nsmgr);
            foreach (XmlNode appliedTradeAllowanceChargeNode in appliedTradeAllowanceChargeNodes)
            {
                bool chargeIndicator = _nodeAsBool(appliedTradeAllowanceChargeNode, "./ram:ChargeIndicator/udt:Indicator", nsmgr);
                decimal basisAmount = _nodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:BasisAmount", nsmgr, 0).Value;
                string basisAmountCurrency = _nodeAsString(appliedTradeAllowanceChargeNode, "./ram:BasisAmount/@currencyID", nsmgr);
                decimal actualAmount = _nodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:ActualAmount", nsmgr, 0).Value;
                string actualAmountCurrency = _nodeAsString(appliedTradeAllowanceChargeNode, "./ram:ActualAmount/@currencyID", nsmgr);
                string reason = _nodeAsString(appliedTradeAllowanceChargeNode, "./ram:Reason", nsmgr);

                item.AddTradeAllowanceCharge(!chargeIndicator, // wichtig: das not (!) beachten
                                                default(CurrencyCodes).FromString(basisAmountCurrency),
                                                basisAmount,
                                                actualAmount,
                                                reason);
            }

            if (item.UnitCode == QuantityCodes.Unknown)
            {
                // UnitCode alternativ aus BilledQuantity extrahieren
                item.UnitCode = default(QuantityCodes).FromString(_nodeAsString(tradeLineItem, ".//ram:BilledQuantity/@unitCode", nsmgr));
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            {
                item.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
                {
                    ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime", nsmgr),
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            {
                item.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
                {
                    ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr),
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime", nsmgr) != null)
            {
                item.ActualDeliveryDate = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            {
                item.ContractReferencedDocument = new ContractReferencedDocument()
                {
                    ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr),
                };
            }

            XmlNodeList referenceNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedSupplyChainTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
            foreach (XmlNode referenceNode in referenceNodes)
            {
                string _code = _nodeAsString(referenceNode, "ram:ReferenceTypeCode", nsmgr);

                item.AddAdditionalReferencedDocument(
                    id: _nodeAsString(referenceNode, "ram:IssuerAssignedID", nsmgr),
                    date: _nodeAsDateTime(referenceNode, "ram:IssueDateTime/udt:DateTimeString", nsmgr),
                    code: default(ReferenceTypeCodes).FromString(_code)
                );
            }

            return item;
        } // !_parseTradeLineItem()        


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

            Party retval = new Party()
            {
                ID = _nodeAsString(node, "ram:ID", nsmgr),
                GlobalID = new GlobalID(_nodeAsString(node, "ram:GlobalID/@schemeID", nsmgr),
                                        _nodeAsString(node, "ram:GlobalID", nsmgr)),
                Name = _nodeAsString(node, "ram:Name", nsmgr),
                Postcode = _nodeAsString(node, "ram:PostalTradeAddress/ram:PostcodeCode", nsmgr),
                City = _nodeAsString(node, "ram:PostalTradeAddress/ram:CityName", nsmgr),
                Country = default(CountryCodes).FromString(_nodeAsString(node, "ram:PostalTradeAddress/ram:CountryID", nsmgr))
            };

            string lineOne = _nodeAsString(node, "ram:PostalTradeAddress/ram:LineOne", nsmgr);
            string lineTwo = _nodeAsString(node, "ram:PostalTradeAddress/ram:LineTwo", nsmgr);

            if (!String.IsNullOrEmpty(lineTwo))
            {
                retval.ContactName = lineOne;
                retval.Street = lineTwo;
            }
            else
            {
                retval.Street = lineOne;
                retval.ContactName = null;
            }

            return retval;
        } // !_nodeAsParty()
    }
}
