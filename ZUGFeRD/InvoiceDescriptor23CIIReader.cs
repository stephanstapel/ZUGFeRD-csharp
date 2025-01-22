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
using System.Xml;
using System.Xml.Linq;


namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor23CIIReader : IInvoiceDescriptorReader
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
            XmlNamespaceManager nsmgr = _GenerateNamespaceManagerFromNode(doc.DocumentElement);

            InvoiceDescriptor retval = new InvoiceDescriptor
            {
                IsTest = XmlUtils.NodeAsBool(doc.DocumentElement, "//rsm:ExchangedDocumentContext/ram:TestIndicator/udt:Indicator", nsmgr, false),
                BusinessProcess = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BusinessProcessSpecifiedDocumentContextParameter/ram:ID", nsmgr),
                Profile = default(Profile).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//ram:GuidelineSpecifiedDocumentContextParameter/ram:ID", nsmgr)),
                Name = XmlUtils.NodeAsString(doc.DocumentElement, "//rsm:ExchangedDocument/ram:Name", nsmgr),
                Type = default(InvoiceType).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//rsm:ExchangedDocument/ram:TypeCode", nsmgr)),
                InvoiceNo = XmlUtils.NodeAsString(doc.DocumentElement, "//rsm:ExchangedDocument/ram:ID", nsmgr),
                InvoiceDate = DataTypeReader.ReadFormattedIssueDateTime(doc.DocumentElement, "//rsm:ExchangedDocument/ram:IssueDateTime", nsmgr)
            };

            foreach (XmlNode node in doc.SelectNodes("//rsm:ExchangedDocument/ram:IncludedNote", nsmgr))
            {
                string content = XmlUtils.NodeAsString(node, ".//ram:Content", nsmgr);
                string parsedSubjectCode = XmlUtils.NodeAsString(node, ".//ram:SubjectCode", nsmgr);
                SubjectCodes subjectCode = default(SubjectCodes).FromString(parsedSubjectCode);
                string parsedContentCode = XmlUtils.NodeAsString(node, ".//ram:ContentCode", nsmgr);
                ContentCodes contentCode = default(ContentCodes).FromString(parsedContentCode);
                retval.AddNote(content, subjectCode, contentCode);
            }

            retval.ReferenceOrderNo = XmlUtils.NodeAsString(doc, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerReference", nsmgr);

            retval.Seller = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty", nsmgr);

            if (doc.SelectSingleNode("//ram:SellerTradeParty/ram:URIUniversalCommunication", nsmgr) != null)
            {
                string id = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:URIUniversalCommunication/ram:URIID", nsmgr);
                string schemeID = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:URIUniversalCommunication/ram:URIID/@schemeID", nsmgr);

                var eas = default(ElectronicAddressSchemeIdentifiers).FromString(schemeID);

                if (eas.HasValue)
                    retval.SetSellerElectronicAddress(id, eas.Value);
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration", nsmgr))
            {
                string id = XmlUtils.NodeAsString(node, ".//ram:ID", nsmgr);
                string schemeID = XmlUtils.NodeAsString(node, ".//ram:ID/@schemeID", nsmgr);

                retval.AddSellerTaxRegistration(id, default(TaxRegistrationSchemeID).FromString(schemeID));
            }

            if (doc.SelectSingleNode("//ram:SellerTradeParty/ram:DefinedTradeContact", nsmgr) != null)
            {
                retval.SellerContact = new Contact()
                {
                    Name = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:PersonName", nsmgr),
                    OrgUnit = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:DepartmentName", nsmgr),
                    PhoneNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:TelephoneUniversalCommunication/ram:CompleteNumber", nsmgr),
                    FaxNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:FaxUniversalCommunication/ram:CompleteNumber", nsmgr),
                    EmailAddress = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:EmailURIUniversalCommunication/ram:URIID", nsmgr)
                };
            }

            retval.Buyer = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty", nsmgr);

            if (doc.SelectSingleNode("//ram:BuyerTradeParty/ram:URIUniversalCommunication", nsmgr) != null)
            {
                string id = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:URIUniversalCommunication/ram:URIID", nsmgr);
                string schemeID = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:URIUniversalCommunication/ram:URIID/@schemeID", nsmgr);

                var eas = default(ElectronicAddressSchemeIdentifiers).FromString(schemeID);

                if (eas.HasValue)
                    retval.SetBuyerElectronicAddress(id, eas.Value);
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedTaxRegistration", nsmgr))
            {
                string id = XmlUtils.NodeAsString(node, ".//ram:ID", nsmgr);
                string schemeID = XmlUtils.NodeAsString(node, ".//ram:ID/@schemeID", nsmgr);

                retval.AddBuyerTaxRegistration(id, default(TaxRegistrationSchemeID).FromString(schemeID));
            }

            if (doc.SelectSingleNode("//ram:BuyerTradeParty/ram:DefinedTradeContact", nsmgr) != null)
            {
                retval.BuyerContact = new Contact()
                {
                    Name = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:PersonName", nsmgr),
                    OrgUnit = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:DepartmentName", nsmgr),
                    PhoneNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:TelephoneUniversalCommunication/ram:CompleteNumber", nsmgr),
                    FaxNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:FaxUniversalCommunication/ram:CompleteNumber", nsmgr),
                    EmailAddress = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:EmailURIUniversalCommunication/ram:URIID", nsmgr)
                };
            }

            //Get all referenced and embedded documents, BG-24
            XmlNodeList referencedDocNodes = doc.SelectNodes(".//ram:ApplicableHeaderTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
            foreach (XmlNode referenceNode in referencedDocNodes)
            {
                retval.AdditionalReferencedDocuments.Add(_readAdditionalReferencedDocument(referenceNode, nsmgr));
            }

            retval.ShipTo = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ShipToTradeParty", nsmgr);
            retval.UltimateShipTo = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:UltimateShipToTradeParty", nsmgr);
            retval.ShipFrom = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ShipFromTradeParty", nsmgr);
            retval.ActualDeliveryDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);

            string despatchAdviceNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DespatchAdviceReferencedDocument/ram:IssuerAssignedID", nsmgr);
            DateTime? despatchAdviceDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DespatchAdviceReferencedDocument/ram:FormattedIssueDateTime/udt:DateTimeString", nsmgr);

            if (!despatchAdviceDate.HasValue)
            {
                despatchAdviceDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DespatchAdviceReferencedDocument/ram:FormattedIssueDateTime", nsmgr);
            }

            if (despatchAdviceDate.HasValue || !String.IsNullOrWhiteSpace(despatchAdviceNo))
            {
                retval.DespatchAdviceReferencedDocument = new DespatchAdviceReferencedDocument()
                {
                    ID = despatchAdviceNo,
                    IssueDateTime = despatchAdviceDate
                };
            }

            string deliveryNoteNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr);
            DateTime? deliveryNoteDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:FormattedIssueDateTime/udt:DateTimeString", nsmgr);

            if (!deliveryNoteDate.HasValue)
            {
                deliveryNoteDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:FormattedIssueDateTime", nsmgr);
            }

            if (deliveryNoteDate.HasValue || !String.IsNullOrWhiteSpace(deliveryNoteNo))
            {
                retval.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
                {
                    ID = deliveryNoteNo,
                    IssueDateTime = deliveryNoteDate
                };
            }

            retval.Invoicee = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceeTradeParty", nsmgr);
            retval.Invoicer = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoicerTradeParty", nsmgr);
            retval.Payee = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:PayeeTradeParty", nsmgr);

            retval.PaymentReference = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:PaymentReference", nsmgr);
            retval.Currency = default(CurrencyCodes).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceCurrencyCode", nsmgr));
            retval.SellerReferenceNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:InvoiceIssuerReference", nsmgr);

            CurrencyCodes optionalTaxCurrency = default(CurrencyCodes).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:TaxCurrencyCode", nsmgr)); // BT-6
            if (optionalTaxCurrency != CurrencyCodes.Unknown)
            {
                retval.TaxCurrency = optionalTaxCurrency;
            }

            // TODO: Multiple SpecifiedTradeSettlementPaymentMeans can exist for each account/institution (with different SEPA?)
            PaymentMeans tempPaymentMeans = new PaymentMeans()
            {
                TypeCode = default(PaymentMeansTypeCodes).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:TypeCode", nsmgr)),
                Information = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:Information", nsmgr),
                SEPACreditorIdentifier = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:CreditorReferenceID", nsmgr),
                SEPAMandateReference = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:SpecifiedTradePaymentTerms/ram:DirectDebitMandateID", nsmgr)
            };

            var financialCardId = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:ApplicableTradeSettlementFinancialCard/ram:ID", nsmgr);
            var financialCardCardholderName = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:ApplicableTradeSettlementFinancialCard/ram:CardholderName", nsmgr);

            if (!string.IsNullOrWhiteSpace(financialCardId) || !string.IsNullOrWhiteSpace(financialCardCardholderName))
            {
                tempPaymentMeans.FinancialCard = new FinancialCard()
                {
                    Id = financialCardId,
                    CardholderName = financialCardCardholderName
                };
            }

            retval.PaymentMeans = tempPaymentMeans;

            retval.BillingPeriodStart = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:BillingSpecifiedPeriod/ram:StartDateTime", nsmgr);
            retval.BillingPeriodEnd = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:BillingSpecifiedPeriod/ram:EndDateTime", nsmgr);

            XmlNodeList creditorFinancialAccountNodes = doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeePartyCreditorFinancialAccount", nsmgr);
            XmlNodeList creditorFinancialInstitutions = doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeeSpecifiedCreditorFinancialInstitution", nsmgr);

            int numberOfAccounts = Math.Min(creditorFinancialAccountNodes.Count, creditorFinancialInstitutions.Count);

            for (int i = 0; i < numberOfAccounts; i++)
            {
                retval.AddCreditorFinancialAccount(
                    iban: XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:IBANID", nsmgr),
                    bic: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:BICID", nsmgr),
                    id: XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:ProprietaryID", nsmgr),
                    bankleitzahl: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:GermanBankleitzahlID", nsmgr),
                    bankName: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:Name", nsmgr),
                    name: XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:AccountName", nsmgr)
                );
            } // !for(i)

            var specifiedTradeSettlementPaymentMeansNodes = doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans", nsmgr);

            foreach (var specifiedTradeSettlementPaymentMeansNode in specifiedTradeSettlementPaymentMeansNodes.OfType<XmlNode>())
            {
                var payerPartyDebtorFinancialAccountNode = specifiedTradeSettlementPaymentMeansNode.SelectSingleNode("ram:PayerPartyDebtorFinancialAccount", nsmgr);

                if (payerPartyDebtorFinancialAccountNode == null)
                {
                    continue;
                }

                var account = new BankAccount()
                {
                    ID = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:ProprietaryID", nsmgr),
                    IBAN = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:IBANID", nsmgr),
                    Bankleitzahl = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:GermanBankleitzahlID", nsmgr),
                    BankName = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:Name", nsmgr),
                };

                var payerSpecifiedDebtorFinancialInstitutionNode = specifiedTradeSettlementPaymentMeansNode.SelectSingleNode("ram:PayerSpecifiedDebtorFinancialInstitution", nsmgr);
                if (payerSpecifiedDebtorFinancialInstitutionNode != null)
                    account.BIC = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:BICID", nsmgr);

                retval._AddDebitorFinancialAccount(account);
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:ApplicableTradeTax", nsmgr))
            {
                retval.AddApplicableTradeTax(XmlUtils.NodeAsDecimal(node, ".//ram:BasisAmount", nsmgr, 0).Value,
                                             XmlUtils.NodeAsDecimal(node, ".//ram:RateApplicablePercent", nsmgr, 0).Value,
                                             XmlUtils.NodeAsDecimal(node, ".//ram:CalculatedAmount", nsmgr, 0).Value,
                                             default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//ram:TypeCode", nsmgr)),
                                             default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:CategoryCode", nsmgr)),
                                             XmlUtils.NodeAsDecimal(node, ".//ram:AllowanceChargeBasisAmount", nsmgr),
                                             default(TaxExemptionReasonCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:ExemptionReasonCode", nsmgr)),
                                             XmlUtils.NodeAsString(node, ".//ram:ExemptionReason", nsmgr),
                                             XmlUtils.NodeAsDecimal(node, ".//ram:LineTotalBasisAmount", nsmgr));
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:SpecifiedTradeAllowanceCharge", nsmgr))
            {
                retval.AddTradeAllowanceCharge(!XmlUtils.NodeAsBool(node, ".//ram:ChargeIndicator", nsmgr), // wichtig: das not (!) beachten
                                               XmlUtils.NodeAsDecimal(node, ".//ram:BasisAmount", nsmgr),
                                               retval.Currency,
                                               XmlUtils.NodeAsDecimal(node, ".//ram:ActualAmount", nsmgr, 0).Value,
                                               XmlUtils.NodeAsDecimal(node, ".//ram:CalculationPercent", nsmgr),
                                               XmlUtils.NodeAsString(node, ".//ram:Reason", nsmgr),
                                               default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//ram:CategoryTradeTax/ram:TypeCode", nsmgr)),
                                               default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:CategoryTradeTax/ram:CategoryCode", nsmgr)),
                                               XmlUtils.NodeAsDecimal(node, ".//ram:CategoryTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value,
                                               EnumExtensions.FromDescription<AllowanceReasonCodes>(XmlUtils.NodeAsString(node, "./ram:ReasonCode", nsmgr)));
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedLogisticsServiceCharge", nsmgr))
            {
                retval.AddLogisticsServiceCharge(XmlUtils.NodeAsDecimal(node, ".//ram:AppliedAmount", nsmgr, 0).Value,
                                                 XmlUtils.NodeAsString(node, ".//ram:Description", nsmgr),
                                                 default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//ram:AppliedTradeTax/ram:TypeCode", nsmgr)),
                                                 default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:AppliedTradeTax/ram:CategoryCode", nsmgr)),
                                                 XmlUtils.NodeAsDecimal(node, ".//ram:AppliedTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value);
            }

            foreach (XmlNode invoiceReferencedDocumentNodes in doc.DocumentElement.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:InvoiceReferencedDocument", nsmgr))
            {
                retval.AddInvoiceReferencedDocument(
                    XmlUtils.NodeAsString(invoiceReferencedDocumentNodes, "./ram:IssuerAssignedID", nsmgr),
                     XmlUtils.NodeAsDateTime(invoiceReferencedDocumentNodes, "./ram:FormattedIssueDateTime", nsmgr)
                );
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedTradePaymentTerms", nsmgr))
            {
                decimal? discountPercent = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentDiscountTerms/ram:CalculationPercent", nsmgr, null);
                int? discountDueDays = null; // XmlUtils.NodeAsInt(node, ".//ram:ApplicableTradePaymentDiscountTerms/ram:BasisPeriodMeasure", nsmgr);
                decimal? discountBaseAmount = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentDiscountTerms/ram:BasisAmount", nsmgr, null);
                decimal? discountActualAmount = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentDiscountTerms/ram:ActualDiscountAmount", nsmgr, null);
                decimal? penaltyPercent = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentPenaltyTerms/ram:CalculationPercent", nsmgr, null);
                int? penaltyDueDays = null; // XmlUtils.NodeAsInt(node, ".//ram:ApplicableTradePaymentPenaltyTerms/ram:BasisPeriodMeasure", nsmgr);
                decimal? penaltyBaseAmount = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentPenaltyTerms/ram:BasisAmount", nsmgr, null);
                decimal? penaltyActualAmount = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentDiscountTerms/ram:ActualPenaltyAmount", nsmgr, null);
                PaymentTermsType? paymentTermsType = discountPercent.HasValue ? PaymentTermsType.Skonto :
                    penaltyPercent.HasValue ? PaymentTermsType.Verzug :
                    (PaymentTermsType?)null;

                retval.AddTradePaymentTerms(XmlUtils.NodeAsString(node, ".//ram:Description", nsmgr),
                                            XmlUtils.NodeAsDateTime(node, ".//ram:DueDateDateTime/udt:DateTimeString", nsmgr),
                                            paymentTermsType,
                                            discountDueDays ?? penaltyDueDays,
                                            discountPercent ?? penaltyPercent,
                                            discountBaseAmount ?? penaltyBaseAmount,
                                            discountActualAmount ?? penaltyActualAmount);
            }

            retval.LineTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:LineTotalAmount", nsmgr);
            retval.ChargeTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:ChargeTotalAmount", nsmgr);
            retval.AllowanceTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:AllowanceTotalAmount", nsmgr);
            retval.TaxBasisAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxBasisTotalAmount", nsmgr);
            retval.TaxTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount", nsmgr);
            retval.GrandTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:GrandTotalAmount", nsmgr);
            retval.RoundingAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:RoundingAmount", nsmgr);
            retval.TotalPrepaidAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TotalPrepaidAmount", nsmgr);
            retval.DuePayableAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:DuePayableAmount", nsmgr);

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:ReceivableSpecifiedTradeAccountingAccount", nsmgr))
            {
                retval.AddReceivableSpecifiedTradeAccountingAccount(
                    AccountID: XmlUtils.NodeAsString(node, ".//ram:ID", nsmgr),
                    AccountTypeCode: XmlUtils.NodeAsEnum<AccountingAccountTypeCodes>(node, ".//ram:TypeCode", nsmgr)
                );
            }

            retval.OrderDate = DataTypeReader.ReadFormattedIssueDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:FormattedIssueDateTime", nsmgr);
            retval.OrderNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID", nsmgr);

            // Read SellerOrderReferencedDocument
            if (doc.SelectSingleNode("//ram:ApplicableHeaderTradeAgreement/ram:SellerOrderReferencedDocument", nsmgr) != null)
            {
                retval.SellerOrderReferencedDocument = new SellerOrderReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SellerOrderReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = DataTypeReader.ReadFormattedIssueDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SellerOrderReferencedDocument/ram:FormattedIssueDateTime", nsmgr)
                };
            }

            // Read ContractReferencedDocument
            if (doc.SelectSingleNode("//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument", nsmgr) != null)
            {
                retval.ContractReferencedDocument = new ContractReferencedDocument
                {
                    ID = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime", nsmgr)
                };
            }

            retval.SpecifiedProcuringProject = new SpecifiedProcuringProject
            {
                ID = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SpecifiedProcuringProject/ram:ID", nsmgr),
                Name = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeAgreement/ram:SpecifiedProcuringProject/ram:Name", nsmgr)
            };

            foreach (XmlNode node in doc.SelectNodes("//ram:IncludedSupplyChainTradeLineItem", nsmgr))
            {
                retval._AddTradeLineItem(_parseTradeLineItem(node, nsmgr));
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
                    "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.0", // XRechnung 2.0
                    "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.1", // XRechnung 2.1
                    "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.2", // XRechnung 2.2
                    "urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.3", // XRechnung 2.3
                    "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0", // XRechnung 3.0
                    "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0#conformant#urn:xeinkauf.de:kosit:extension:xrechnung_3.0", // XRechnung 3.0
                    "urn.cpro.gouv.fr:1p0:ereporting" //Factur-X E-reporting
                };

            return _IsReadableByThisReaderVersion(stream, validURIs);
        } // !IsReadableByThisReaderVersion()


        private static TradeLineItem _parseTradeLineItem(XmlNode tradeLineItem, XmlNamespaceManager nsmgr = null)
        {
            if (tradeLineItem == null)
            {
                return null;
            }

            string lineId = XmlUtils.NodeAsString(tradeLineItem, ".//ram:AssociatedDocumentLineDocument/ram:LineID", nsmgr, String.Empty);
            string parentLineId = XmlUtils.NodeAsString(tradeLineItem, ".//ram:AssociatedDocumentLineDocument/ram:ParentLineID", nsmgr, null);
            LineStatusCodes? lineStatusCode = default(LineStatusCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:AssociatedDocumentLineDocument/ram:LineStatusCode", nsmgr, null));
            LineStatusReasonCodes? lineStatusReasonCode = default(LineStatusReasonCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:AssociatedDocumentLineDocument/ram:LineStatusReasonCode", nsmgr, null));

            TradeLineItem item = new TradeLineItem(lineId)
            {
                GlobalID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:GlobalID/@schemeID", nsmgr)),
                                        XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:GlobalID", nsmgr)),
                SellerAssignedID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:SellerAssignedID", nsmgr),
                BuyerAssignedID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:BuyerAssignedID", nsmgr),
                Name = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:Name", nsmgr),
                Description = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:Description", nsmgr),
                UnitQuantity = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:BasisQuantity", nsmgr, 1),
                BilledQuantity = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:BilledQuantity", nsmgr, 0).Value,
                ShipTo = _nodeAsParty(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:ShipToTradeParty", nsmgr),
                UltimateShipTo = _nodeAsParty(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:UltimateShipToTradeParty", nsmgr),
                LineTotalAmount = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:LineTotalAmount", nsmgr, 0),
                TaxCategoryCode = default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:ApplicableTradeTax/ram:CategoryCode", nsmgr)),
                TaxType = default(TaxTypes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:ApplicableTradeTax/ram:TypeCode", nsmgr)),
                TaxPercent = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:ApplicableTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value,
                NetUnitPrice = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:NetPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                GrossUnitPrice = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:GrossPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                UnitCode = default(QuantityCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:BilledQuantity/@unitCode", nsmgr)),
                BillingPeriodStart = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:BillingSpecifiedPeriod/ram:StartDateTime/udt:DateTimeString", nsmgr),
                BillingPeriodEnd = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:BillingSpecifiedPeriod/ram:EndDateTime/udt:DateTimeString", nsmgr),
            };


            if (!String.IsNullOrWhiteSpace(parentLineId))
            {
                item.SetParentLineId(parentLineId);
            }

            if (lineStatusCode.HasValue && lineStatusReasonCode.HasValue)
            {
                item.SetLineStatus(lineStatusCode.Value, lineStatusReasonCode.Value);
            }

            if (tradeLineItem.SelectNodes(".//ram:SpecifiedTradeProduct/ram:ApplicableProductCharacteristic", nsmgr) != null)
            {
                foreach (XmlNode applicableProductCharacteristic in tradeLineItem.SelectNodes(".//ram:SpecifiedTradeProduct/ram:ApplicableProductCharacteristic", nsmgr))
                {
                    item.ApplicableProductCharacteristics.Add(new ApplicableProductCharacteristic()
                    {
                        Description = XmlUtils.NodeAsString(applicableProductCharacteristic, ".//ram:Description", nsmgr),
                        Value = XmlUtils.NodeAsString(applicableProductCharacteristic, ".//ram:Value", nsmgr),
                    });
                }
            }

            foreach (XmlNode includedItem in tradeLineItem.SelectNodes(".//ram:SpecifiedTradeProduct/ram:IncludedReferencedProduct", nsmgr))
            {
                var unitCode = XmlUtils.NodeAsString(includedItem, ".//ram:UnitQuantity/@unitCode", nsmgr, null);

                item.IncludedReferencedProducts.Add(new IncludedReferencedProduct()
                {
                    Name = XmlUtils.NodeAsString(includedItem, ".//ram:Name", nsmgr),
                    UnitQuantity = XmlUtils.NodeAsDecimal(includedItem, ".//ram:UnitQuantity", nsmgr, null),
                    UnitCode = unitCode != null ? (QuantityCodes?)default(QuantityCodes).FromString(unitCode) : null
                });
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument", nsmgr) != null)
            {
                item.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = DataTypeReader.ReadFormattedIssueDateTime(tradeLineItem, "//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument/ram:FormattedIssueDateTime", nsmgr),
                    LineID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:BuyerOrderReferencedDocument/ram:LineID", nsmgr)
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument", nsmgr) != null)
            {
                item.ContractReferencedDocument = new ContractReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = DataTypeReader.ReadFormattedIssueDateTime(tradeLineItem, "//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime", nsmgr)
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeSettlement", nsmgr) != null)
            {
                XmlNodeList lineTradeSettlementNodes = tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeSettlement", nsmgr).ChildNodes;
                foreach (XmlNode lineTradeSettlementNode in lineTradeSettlementNodes)
                {
                    switch (lineTradeSettlementNode.Name)
                    {
                        case "ram:ApplicableTradeTax":
                            //TODO
                            break;
                        case "ram:BillingSpecifiedPeriod":
                            //TODO
                            break;
                        case "ram:SpecifiedTradeAllowanceCharge":
                            bool chargeIndicator = XmlUtils.NodeAsBool(lineTradeSettlementNode, "./ram:ChargeIndicator/udt:Indicator", nsmgr);
                            decimal? basisAmount = XmlUtils.NodeAsDecimal(lineTradeSettlementNode, "./ram:BasisAmount", nsmgr, null);
                            string basisAmountCurrency = XmlUtils.NodeAsString(lineTradeSettlementNode, "./ram:BasisAmount/@currencyID", nsmgr);
                            decimal actualAmount = XmlUtils.NodeAsDecimal(lineTradeSettlementNode, "./ram:ActualAmount", nsmgr, 0).Value;                            
                            string reason = XmlUtils.NodeAsString(lineTradeSettlementNode, "./ram:Reason", nsmgr);
                            decimal? chargePercentage = XmlUtils.NodeAsDecimal(lineTradeSettlementNode, "./ram:CalculationPercent", nsmgr, null);
                            string reasonCode = XmlUtils.NodeAsString(lineTradeSettlementNode, "./ram:ReasonCode", nsmgr);

                            item.AddSpecifiedTradeAllowanceCharge(!chargeIndicator, // wichtig: das not (!) beachten
                                                        default(CurrencyCodes).FromString(basisAmountCurrency),
                                                        basisAmount,
                                                        actualAmount,
                                                        chargePercentage,
                                                        reason,
                                                        EnumExtensions.FromDescription<AllowanceReasonCodes>(reasonCode));
                            break;
                        case "ram:SpecifiedTradeSettlementLineMonetarySummation":
                            //TODO
                            break;
                        case "ram:AdditionalReferencedDocument": // BT-128-00
                            item._AdditionalReferencedDocuments.Add(_readAdditionalReferencedDocument(lineTradeSettlementNode, nsmgr));
                            break;
                        case "ram:ReceivableSpecifiedTradeAccountingAccount":
                            item.ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
                            {
                                TradeAccountID = XmlUtils.NodeAsString(lineTradeSettlementNode, "./ram:ID", nsmgr),
                                TradeAccountTypeCode = XmlUtils.NodeAsEnum<AccountingAccountTypeCodes>(lineTradeSettlementNode, ".//ram:TypeCode", nsmgr)
                            });
                            break;
                    }
                }
            }

            if (tradeLineItem.SelectSingleNode(".//ram:AssociatedDocumentLineDocument", nsmgr) != null)
            {
                XmlNodeList noteNodes = tradeLineItem.SelectNodes(".//ram:AssociatedDocumentLineDocument/ram:IncludedNote", nsmgr);
                foreach (XmlNode noteNode in noteNodes)
                {
                    item.AssociatedDocument.Notes.Add(new Note(
                        content: XmlUtils.NodeAsString(noteNode, ".//ram:Content", nsmgr),
                        subjectCode: default(SubjectCodes).FromString(XmlUtils.NodeAsString(noteNode, ".//ram:SubjectCode", nsmgr)),
                        contentCode: default(ContentCodes).FromString(XmlUtils.NodeAsString(noteNode, ".//ram:ContentCode", nsmgr))
                    ));
                }
            }

            XmlNodeList appliedTradeAllowanceChargeNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedLineTradeAgreement/ram:GrossPriceProductTradePrice/ram:AppliedTradeAllowanceCharge", nsmgr);
            foreach (XmlNode appliedTradeAllowanceChargeNode in appliedTradeAllowanceChargeNodes)
            {
                bool chargeIndicator = XmlUtils.NodeAsBool(appliedTradeAllowanceChargeNode, "./ram:ChargeIndicator/udt:Indicator", nsmgr);
                decimal? basisAmount = XmlUtils.NodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:BasisAmount", nsmgr, null);
                string basisAmountCurrency = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./ram:BasisAmount/@currencyID", nsmgr);
                decimal actualAmount = XmlUtils.NodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:ActualAmount", nsmgr, 0).Value;                
                string reason = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./ram:Reason", nsmgr);
                decimal? chargePercentage = XmlUtils.NodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:CalculationPercent", nsmgr, null);

                item.AddTradeAllowanceCharge(!chargeIndicator, // wichtig: das not (!) beachten
                                            default(CurrencyCodes).FromString(basisAmountCurrency),
                                            basisAmount,
                                            actualAmount,
                                            chargePercentage,
                                            reason);
            }

            if (item.UnitCode == QuantityCodes.Unknown)
            {
                // UnitCode alternativ aus BilledQuantity extrahieren
                item.UnitCode = default(QuantityCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:BilledQuantity/@unitCode", nsmgr));
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            {
                item.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr),
                    IssueDateTime = DataTypeReader.ReadFormattedIssueDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:FormattedIssueDateTime", nsmgr)
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime", nsmgr) != null)
            {
                item.ActualDeliveryDate = DataTypeReader.ReadFormattedIssueDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime", nsmgr);
            }

            //if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr) != null)
            //{
            //    item.ContractReferencedDocument = new ContractReferencedDocument()
            //    {
            //        ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:IssuerAssignedID", nsmgr),
            //        IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedLineTradeAgreement/ram:ContractReferencedDocument/ram:FormattedIssueDateTime/qdt:DateTimeString", nsmgr),
            //    };
            //}

            foreach (XmlNode designatedProductClassificationNode in tradeLineItem.SelectNodes(".//ram:DesignatedProductClassification", nsmgr))
            {
                string className = XmlUtils.NodeAsString(designatedProductClassificationNode, "./ram:ClassName", nsmgr);
                string classCode = XmlUtils.NodeAsString(designatedProductClassificationNode, "./ram:ClassCode", nsmgr);
                DesignatedProductClassificationClassCodes listID = default(DesignatedProductClassificationClassCodes).FromString(XmlUtils.NodeAsString(designatedProductClassificationNode, "./ram:ClassCode/@listID", nsmgr));
                string listVersionID = XmlUtils.NodeAsString(designatedProductClassificationNode, "./ram:ClassCode/@listVersionID", nsmgr);

                item.AddDesignatedProductClassification(listID, listVersionID, classCode, className);
            } // !foreach(designatedProductClassificationNode))

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
                ID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(node, "ram:ID/@schemeID", nsmgr)),
                                        XmlUtils.NodeAsString(node, "ram:ID", nsmgr)),
                TradingBusinessName = XmlUtils.NodeAsString(node, "ram:TradingBusinessName", nsmgr),
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

            Party retval = new Party()
            {
                ID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(node, "./ram:ID/@schemeID", nsmgr)),
                                        XmlUtils.NodeAsString(node, "./ram:ID", nsmgr)),
                GlobalID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(node, "./ram:GlobalID/@schemeID", nsmgr)),
                                        XmlUtils.NodeAsString(node, "./ram:GlobalID", nsmgr)),
                Name = XmlUtils.NodeAsString(node, "./ram:Name", nsmgr),
                Description = XmlUtils.NodeAsString(node, "./ram:Description", nsmgr), // BT-33 Seller only
                Postcode = XmlUtils.NodeAsString(node, "./ram:PostalTradeAddress/ram:PostcodeCode", nsmgr),
                City = XmlUtils.NodeAsString(node, "./ram:PostalTradeAddress/ram:CityName", nsmgr),
                Country = default(CountryCodes).FromString(XmlUtils.NodeAsString(node, "./ram:PostalTradeAddress/ram:CountryID", nsmgr)),
                SpecifiedLegalOrganization = _nodeAsLegalOrganization(node, "./ram:SpecifiedLegalOrganization", nsmgr),
            };

            string lineOne = XmlUtils.NodeAsString(node, "./ram:PostalTradeAddress/ram:LineOne", nsmgr);
            string lineTwo = XmlUtils.NodeAsString(node, "./ram:PostalTradeAddress/ram:LineTwo", nsmgr);

            if (!String.IsNullOrWhiteSpace(lineTwo))
            {
                retval.ContactName = lineOne;
                retval.Street = lineTwo;
            }
            else
            {
                retval.Street = lineOne;
                retval.ContactName = null;
            }

            retval.AddressLine3 = XmlUtils.NodeAsString(node, "./ram:PostalTradeAddress/ram:LineThree", nsmgr);
            retval.CountrySubdivisionName = XmlUtils.NodeAsString(node, "./ram:PostalTradeAddress/ram:CountrySubDivisionName", nsmgr);

            return retval;
        } // !_nodeAsParty()


        private static AdditionalReferencedDocument _readAdditionalReferencedDocument(XmlNode node, XmlNamespaceManager nsmgr)
        {
            string strBase64BinaryData = XmlUtils.NodeAsString(node, "ram:AttachmentBinaryObject", nsmgr);
            return new AdditionalReferencedDocument
            {
                ID = XmlUtils.NodeAsString(node, "ram:IssuerAssignedID", nsmgr),
                TypeCode = default(AdditionalReferencedDocumentTypeCode).FromString(XmlUtils.NodeAsString(node, "ram:TypeCode", nsmgr)),
                Name = XmlUtils.NodeAsString(node, "ram:Name", nsmgr),
                IssueDateTime = DataTypeReader.ReadFormattedIssueDateTime(node, "ram:FormattedIssueDateTime", nsmgr),
                AttachmentBinaryObject = !string.IsNullOrWhiteSpace(strBase64BinaryData) ? Convert.FromBase64String(strBase64BinaryData) : null,
                Filename = XmlUtils.NodeAsString(node, "ram:AttachmentBinaryObject/@filename", nsmgr),
                ReferenceTypeCode = default(ReferenceTypeCodes).FromString(XmlUtils.NodeAsString(node, "ram:ReferenceTypeCode", nsmgr)),
                URIID = XmlUtils.NodeAsString(node, "ram:URIID", nsmgr, null),
                LineID = XmlUtils.NodeAsString(node, "ram:LineID", nsmgr, null)
			};
        } // !_readAdditionalReferencedDocument()

    }
}
