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
using System.Xml;

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor1Reader : IInvoiceDescriptorReader
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
                IsTest = XmlUtils.NodeAsBool(doc.DocumentElement, "//rsm:SpecifiedExchangedDocumentContext/ram:TestIndicator", nsmgr, false),
                BusinessProcess = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:BusinessProcessSpecifiedDocumentContextParameter/ram:ID", nsmgr),
                Profile = default(Profile).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//ram:GuidelineSpecifiedDocumentContextParameter/ram:ID", nsmgr)),
                Name = XmlUtils.NodeAsString(doc.DocumentElement, "//rsm:HeaderExchangedDocument/ram:Name", nsmgr),
                Type = default(InvoiceType).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//rsm:HeaderExchangedDocument/ram:TypeCode", nsmgr)),
                InvoiceNo = XmlUtils.NodeAsString(doc.DocumentElement, "//rsm:HeaderExchangedDocument/ram:ID", nsmgr),
                InvoiceDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//rsm:HeaderExchangedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr)
            };

            foreach (XmlNode node in doc.SelectNodes("//rsm:HeaderExchangedDocument/ram:IncludedNote", nsmgr))
            {
                string content = XmlUtils.NodeAsString(node, ".//ram:Content", nsmgr);
                string subjectCodeAsString = XmlUtils.NodeAsString(node, ".//ram:SubjectCode", nsmgr);
                SubjectCodes subjectCode = default(SubjectCodes).FromString(subjectCodeAsString);
                string contentCodeAsString = XmlUtils.NodeAsString(node, ".//ram:ContentCode", nsmgr);
                ContentCodes contentCode = default(ContentCodes).FromString(contentCodeAsString);
                retval.AddNote(content, subjectCode, contentCode);
            }

            retval.ReferenceOrderNo = XmlUtils.NodeAsString(doc, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerReference", nsmgr);

            retval.Seller = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:SellerTradeParty", nsmgr);
            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableSupplyChainTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration", nsmgr))
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

            retval.Buyer = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerTradeParty", nsmgr);
            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedTaxRegistration", nsmgr))
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

            retval.ShipTo = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:ShipToTradeParty", nsmgr);
            retval.ShipFrom = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:ShipFromTradeParty", nsmgr);
            retval.ActualDeliveryDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);

            string deliveryNoteNoAsString = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:ID", nsmgr);
            DateTime? deliveryNoteDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);

            if (!deliveryNoteDate.HasValue)
            {
                deliveryNoteDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime", nsmgr);
            }

            if (deliveryNoteDate.HasValue || !String.IsNullOrWhiteSpace(deliveryNoteNoAsString))
            {
                retval.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
                {
                    ID = deliveryNoteNoAsString,
                    IssueDateTime = deliveryNoteDate
                };
            }

            retval.Invoicee = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:InvoiceeTradeParty", nsmgr);
            retval.Payee = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:PayeeTradeParty", nsmgr);

            retval.PaymentReference = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:PaymentReference", nsmgr);
            retval.Currency = default(CurrencyCodes).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:InvoiceCurrencyCode", nsmgr));

            // TODO: Multiple SpecifiedTradeSettlementPaymentMeans can exist for each account/institution (with different SEPA?)
            PaymentMeans paymentMeans = new PaymentMeans()
            {
                TypeCode = default(PaymentMeansTypeCodes).FromString(XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:TypeCode", nsmgr)),
                Information = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:Information", nsmgr),
                SEPACreditorIdentifier = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:ID", nsmgr),
                SEPAMandateReference = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:ID/@schemeAgencyID", nsmgr)
            };
            retval.PaymentMeans = paymentMeans;

            retval.BillingPeriodStart = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:BillingSpecifiedPeriod/ram:StartDateTime", nsmgr);
            retval.BillingPeriodEnd = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:BillingSpecifiedPeriod/ram:EndDateTime", nsmgr);

            XmlNodeList creditorFinancialAccountNodes = doc.SelectNodes("//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeePartyCreditorFinancialAccount", nsmgr);
            XmlNodeList creditorFinancialInstitutions = doc.SelectNodes("//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeeSpecifiedCreditorFinancialInstitution", nsmgr);

            if (creditorFinancialAccountNodes.Count == creditorFinancialInstitutions.Count)
            {
                for (int i = 0; i < creditorFinancialAccountNodes.Count; i++)
                {
                    retval.AddCreditorFinancialAccount(iban: XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:IBANID", nsmgr),
                                                       bic: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:BICID", nsmgr),
                                                       id: XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:ProprietaryID", nsmgr),
                                                       bankleitzahl: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:GermanBankleitzahlID", nsmgr),
                                                       bankName: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:Name", nsmgr),
                                                       name: XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:AccountName", nsmgr));
                } // !for(i)
            }

            XmlNodeList debitorFinancialAccountNodes = doc.SelectNodes("//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayerPartyDebtorFinancialAccount", nsmgr);
            XmlNodeList debitorFinancialInstitutions = doc.SelectNodes("//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayerSpecifiedDebtorFinancialInstitution", nsmgr);

            if (debitorFinancialAccountNodes.Count == debitorFinancialInstitutions.Count)
            {
                for (int i = 0; i < debitorFinancialAccountNodes.Count; i++)
                {
                    retval.AddDebitorFinancialAccount(iban: XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:IBANID", nsmgr),
                                                      bic: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:BICID", nsmgr),
                                                      id: XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:ProprietaryID", nsmgr),
                                                      bankleitzahl: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:GermanBankleitzahlID", nsmgr),
                                                      bankName: XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:Name", nsmgr));
                } // !for(i)
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableSupplyChainTradeSettlement/ram:ApplicableTradeTax", nsmgr))
            {
                retval.AddApplicableTradeTax(XmlUtils.NodeAsDecimal(node, ".//ram:BasisAmount", nsmgr, 0).Value,
                                             XmlUtils.NodeAsDecimal(node, ".//ram:ApplicablePercent", nsmgr, 0).Value,
                                             XmlUtils.NodeAsDecimal(node, ".//ram:CalculatedAmount", nsmgr, 0).Value,
                                             default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//ram:TypeCode", nsmgr)),
                                             default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:CategoryCode", nsmgr)),
                                             XmlUtils.NodeAsDecimal(node, ".//ram:AllowanceChargeBasisAmount", nsmgr),
                                             lineTotalBasisAmount: XmlUtils.NodeAsDecimal(node, ".//ram:LineTotalBasisAmount", nsmgr));
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedTradeAllowanceCharge", nsmgr))
            {
                retval.AddTradeAllowanceCharge(!XmlUtils.NodeAsBool(node, ".//ram:ChargeIndicator", nsmgr), // wichtig: das not (!) beachten
                                               XmlUtils.NodeAsDecimal(node, ".//ram:BasisAmount", nsmgr, 0).Value,
                                               retval.Currency,
                                               XmlUtils.NodeAsDecimal(node, ".//ram:ActualAmount", nsmgr, 0).Value,
                                               XmlUtils.NodeAsString(node, ".//ram:Reason", nsmgr),
                                               default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//ram:CategoryTradeTax/ram:TypeCode", nsmgr)),
                                               default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:CategoryTradeTax/ram:CategoryCode", nsmgr)),
                                               XmlUtils.NodeAsDecimal(node, ".//ram:CategoryTradeTax/ram:ApplicablePercent", nsmgr, 0).Value,
                                               EnumExtensions.FromDescription<AllowanceReasonCodes>(XmlUtils.NodeAsString(node, "./ram:ReasonCode", nsmgr)));
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedLogisticsServiceCharge", nsmgr))
            {
                retval.AddLogisticsServiceCharge(XmlUtils.NodeAsDecimal(node, ".//ram:AppliedAmount", nsmgr, 0).Value,
                                                 XmlUtils.NodeAsString(node, ".//ram:Description", nsmgr),
                                                 default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//ram:AppliedTradeTax/ram:TypeCode", nsmgr)),
                                                 default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:AppliedTradeTax/ram:CategoryCode", nsmgr)),
                                                 XmlUtils.NodeAsDecimal(node, ".//ram:AppliedTradeTax/ram:ApplicablePercent", nsmgr, 0).Value);
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

            retval.LineTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:LineTotalAmount", nsmgr, 0).Value;
            retval.ChargeTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:ChargeTotalAmount", nsmgr, null);
            retval.AllowanceTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:AllowanceTotalAmount", nsmgr, null);
            retval.TaxBasisAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:TaxBasisTotalAmount", nsmgr, null);
            retval.TaxTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:TaxTotalAmount", nsmgr, 0).Value;
            retval.GrandTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:GrandTotalAmount", nsmgr, 0).Value;
            retval.TotalPrepaidAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:TotalPrepaidAmount", nsmgr, null);
            retval.DuePayableAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:DuePayableAmount", nsmgr, 0).Value;

            retval.OrderDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);
            if (!retval.OrderDate.HasValue)
            {
                retval.OrderDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime", nsmgr);
            }
            retval.OrderNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:ID", nsmgr);


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
                "urn:ferd:invoice:1.0:basic",
                "urn:ferd:invoice:1.0:comfort",
                "urn:ferd:invoice:1.0:extended",
                "urn:ferd:CrossIndustryDocument:invoice:1p0:basic",
                "urn:ferd:CrossIndustryDocument:invoice:1p0:comfort",
                "urn:ferd:CrossIndustryDocument:invoice:1p0:extended"
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
                LineTotalAmount = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:LineTotalAmount", nsmgr, 0),
                TaxCategoryCode = default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:ApplicableTradeTax/ram:CategoryCode", nsmgr)),
                TaxType = default(TaxTypes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:ApplicableTradeTax/ram:TypeCode", nsmgr)),
                TaxPercent = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:ApplicableTradeTax/ram:ApplicablePercent", nsmgr, 0).Value,
                NetUnitPrice = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:NetPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                GrossUnitPrice = XmlUtils.NodeAsDecimal(tradeLineItem, ".//ram:GrossPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                UnitCode = default(QuantityCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:BilledQuantity/@unitCode", nsmgr)),
                BillingPeriodStart = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:BillingSpecifiedPeriod/ram:StartDateTime/udt:DateTimeString", nsmgr),
                BillingPeriodEnd = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:BillingSpecifiedPeriod/ram:EndDateTime/udt:DateTimeString", nsmgr),
            };

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

            XmlNodeList appliedTradeAllowanceChargeNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedSupplyChainTradeAgreement/ram:GrossPriceProductTradePrice/ram:AppliedTradeAllowanceCharge", nsmgr);
            foreach (XmlNode appliedTradeAllowanceChargeNode in appliedTradeAllowanceChargeNodes)
            {
                bool chargeIndicator = XmlUtils.NodeAsBool(appliedTradeAllowanceChargeNode, "./ram:ChargeIndicator/udt:Indicator", nsmgr);
                decimal basisAmount = XmlUtils.NodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:BasisAmount", nsmgr, 0).Value;
                string basisAmountCurrency = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./ram:BasisAmount/@currencyID", nsmgr);
                decimal actualAmount = XmlUtils.NodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:ActualAmount", nsmgr, 0).Value;                
                string reason = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./ram:Reason", nsmgr);

                item.AddTradeAllowanceCharge(!chargeIndicator, // wichtig: das not (!) beachten
                                                default(CurrencyCodes).FromString(basisAmountCurrency),
                                                basisAmount,
                                                actualAmount,
                                                reason);
            }

            if (item.UnitCode == QuantityCodes.Unknown)
            {
                // UnitCode alternativ aus BilledQuantity extrahieren
                item.UnitCode = default(QuantityCodes).FromString(XmlUtils.NodeAsString(tradeLineItem, ".//ram:BilledQuantity/@unitCode", nsmgr));
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:ID", nsmgr) != null)
            {
                item.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:ID", nsmgr),
                    IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime", nsmgr),
                    LineID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:LineID", nsmgr)
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:ID", nsmgr) != null)
            {
                item.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:ID", nsmgr),
                    IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime", nsmgr),
                    LineID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:LineID", nsmgr)
                };
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime", nsmgr) != null)
            {
                item.ActualDeliveryDate = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);
            }

            if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:ID", nsmgr) != null)
            {
                item.ContractReferencedDocument = new ContractReferencedDocument()
                {
                    ID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:ID", nsmgr),
                    IssueDateTime = XmlUtils.NodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:IssueDateTime", nsmgr),
                    LineID = XmlUtils.NodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:LineID", nsmgr)
                };
            }

            XmlNodeList referenceNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedSupplyChainTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
            foreach (XmlNode referenceNode in referenceNodes)
            {
                string typeCodeAsString = XmlUtils.NodeAsString(referenceNode, "ram:TypeCode", nsmgr);
                string codeAsString = XmlUtils.NodeAsString(referenceNode, "ram:ReferenceTypeCode", nsmgr);

                item.AddAdditionalReferencedDocument(
                    id: XmlUtils.NodeAsString(referenceNode, "ram:ID", nsmgr),
                    typeCode: default(AdditionalReferencedDocumentTypeCode).FromString(typeCodeAsString),
                    code: default(ReferenceTypeCodes).FromString(codeAsString),
                    issueDateTime: XmlUtils.NodeAsDateTime(referenceNode, "ram:IssueDateTime", nsmgr)
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
                ID = new GlobalID(GlobalIDSchemeIdentifiers.Unknown, XmlUtils.NodeAsString(node, "ram:ID", nsmgr)),
                GlobalID = new GlobalID(default(GlobalIDSchemeIdentifiers).FromString(XmlUtils.NodeAsString(node, "ram:GlobalID/@schemeID", nsmgr)),
                                        XmlUtils.NodeAsString(node, "ram:GlobalID", nsmgr)),
                Name = XmlUtils.NodeAsString(node, "ram:Name", nsmgr),
                Description = XmlUtils.NodeAsString(node, "ram:Description", nsmgr), // BT-33 Seller only
                Postcode = XmlUtils.NodeAsString(node, "ram:PostalTradeAddress/ram:PostcodeCode", nsmgr),
                City = XmlUtils.NodeAsString(node, "ram:PostalTradeAddress/ram:CityName", nsmgr),
                Country = default(CountryCodes).FromString(XmlUtils.NodeAsString(node, "ram:PostalTradeAddress/ram:CountryID", nsmgr))
            };

            string lineOne = XmlUtils.NodeAsString(node, "ram:PostalTradeAddress/ram:LineOne", nsmgr);
            string lineTwo = XmlUtils.NodeAsString(node, "ram:PostalTradeAddress/ram:LineTwo", nsmgr);

            if (!String.IsNullOrWhiteSpace(lineTwo))
            {
                retval.ContactName = lineOne;
                retval.Street = lineOne;
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
