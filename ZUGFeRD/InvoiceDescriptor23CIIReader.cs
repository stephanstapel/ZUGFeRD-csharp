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
				string _subjectCode = XmlUtils.NodeAsString(node, ".//ram:SubjectCode", nsmgr);
				SubjectCodes subjectCode = default(SubjectCodes).FromString(_subjectCode);
				string _contentCode = XmlUtils.NodeAsString(node, ".//ram:ContentCode", nsmgr);
				ContentCodes contentCode = default(ContentCodes).FromString(_contentCode);
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


			//Get all referenced and embedded documents (BG-24)
			XmlNodeList referencedDocNodes = doc.SelectNodes(".//ram:ApplicableHeaderTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
			foreach (XmlNode referenceNode in referencedDocNodes)
			{
				retval.AdditionalReferencedDocuments.Add(_readAdditionalReferencedDocument(referenceNode, nsmgr));
			}

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


			retval.ShipTo = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ShipToTradeParty", nsmgr);
			retval.UltimateShipTo = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:UltimateShipToTradeParty", nsmgr);
			retval.ShipFrom = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ShipFromTradeParty", nsmgr);
			retval.ActualDeliveryDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);

			string _despatchAdviceNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DespatchAdviceReferencedDocument/ram:IssuerAssignedID", nsmgr);
			DateTime? _despatchAdviceDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DespatchAdviceReferencedDocument/ram:FormattedIssueDateTime/udt:DateTimeString", nsmgr);

			if (!_despatchAdviceDate.HasValue)
			{
				_despatchAdviceDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DespatchAdviceReferencedDocument/ram:FormattedIssueDateTime", nsmgr);
			}

			if (_despatchAdviceDate.HasValue || !String.IsNullOrWhiteSpace(_despatchAdviceNo))
			{
				retval.DespatchAdviceReferencedDocument = new DespatchAdviceReferencedDocument()
				{
					ID = _despatchAdviceNo,
					IssueDateTime = _despatchAdviceDate
				};
			}

			string _deliveryNoteNo = XmlUtils.NodeAsString(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssuerAssignedID", nsmgr);
			DateTime? _deliveryNoteDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:FormattedIssueDateTime/udt:DateTimeString", nsmgr);

			if (!_deliveryNoteDate.HasValue)
			{
				_deliveryNoteDate = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:FormattedIssueDateTime", nsmgr);
			}

			if (_deliveryNoteDate.HasValue || !String.IsNullOrWhiteSpace(_deliveryNoteNo))
			{
				retval.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
				{
					ID = _deliveryNoteNo,
					IssueDateTime = _deliveryNoteDate
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
			PaymentMeans _tempPaymentMeans = new PaymentMeans()
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
				_tempPaymentMeans.FinancialCard = new FinancialCard()
				{
					Id = financialCardId,
					CardholderName = financialCardCardholderName
				};
			}

			retval.PaymentMeans = _tempPaymentMeans;

			retval.BillingPeriodStart = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:BillingSpecifiedPeriod/ram:StartDateTime", nsmgr);
			retval.BillingPeriodEnd = XmlUtils.NodeAsDateTime(doc.DocumentElement, "//ram:ApplicableHeaderTradeSettlement/ram:BillingSpecifiedPeriod/ram:EndDateTime", nsmgr);

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
				retval.CreditorBankAccounts[i].ID = XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:ProprietaryID", nsmgr);
				retval.CreditorBankAccounts[i].IBAN = XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:IBANID", nsmgr);
				retval.CreditorBankAccounts[i].Name = XmlUtils.NodeAsString(creditorFinancialAccountNodes[i], ".//ram:AccountName", nsmgr);
			}

			for (int i = 0; i < creditorFinancialInstitutions.Count; i++)
			{
				retval.CreditorBankAccounts[i].BIC = XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:BICID", nsmgr);
				retval.CreditorBankAccounts[i].Bankleitzahl = XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:GermanBankleitzahlID", nsmgr);
				retval.CreditorBankAccounts[i].BankName = XmlUtils.NodeAsString(creditorFinancialInstitutions[i], ".//ram:Name", nsmgr);
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
					ID = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:ProprietaryID", nsmgr),
					IBAN = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:IBANID", nsmgr),
					Bankleitzahl = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:GermanBankleitzahlID", nsmgr),
					BankName = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:Name", nsmgr),
				};

				var payerSpecifiedDebtorFinancialInstitutionNode = specifiedTradeSettlementPaymentMeansNode.SelectSingleNode("ram:PayerSpecifiedDebtorFinancialInstitution", nsmgr);
				if (payerSpecifiedDebtorFinancialInstitutionNode != null)
					_account.BIC = XmlUtils.NodeAsString(payerPartyDebtorFinancialAccountNode, ".//ram:BICID", nsmgr);

				retval.DebitorBankAccounts.Add(_account);
			}

			foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:ApplicableTradeTax", nsmgr))
			{
				retval.AddApplicableTradeTax(XmlUtils.NodeAsDecimal(node, ".//ram:BasisAmount", nsmgr, 0).Value,
											 XmlUtils.NodeAsDecimal(node, ".//ram:RateApplicablePercent", nsmgr, 0).Value,
											 default(TaxTypes).FromString(XmlUtils.NodeAsString(node, ".//ram:TypeCode", nsmgr)),
											 default(TaxCategoryCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:CategoryCode", nsmgr)),
											 0,
											 default(TaxExemptionReasonCodes).FromString(XmlUtils.NodeAsString(node, ".//ram:ExemptionReasonCode", nsmgr)),
											 XmlUtils.NodeAsString(node, ".//ram:ExemptionReason", nsmgr));
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
											   XmlUtils.NodeAsDecimal(node, ".//ram:CategoryTradeTax/ram:RateApplicablePercent", nsmgr, 0).Value);
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
				decimal? discountAmount = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentDiscountTerms/ram:BasisAmount", nsmgr, null);
				decimal? penaltyPercent = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentPenaltyTerms/ram:CalculationPercent", nsmgr, null);
				int? penaltyDueDays = null; // XmlUtils.NodeAsInt(node, ".//ram:ApplicableTradePaymentPenaltyTerms/ram:BasisPeriodMeasure", nsmgr);
				decimal? penaltyAmount = XmlUtils.NodeAsDecimal(node, ".//ram:ApplicableTradePaymentPenaltyTerms/ram:BasisAmount", nsmgr, null);
				PaymentTermsType? paymentTermsType = discountPercent.HasValue ? PaymentTermsType.Skonto :
					penaltyPercent.HasValue ? PaymentTermsType.Verzug :
					(PaymentTermsType?)null;

				retval.AddTradePaymentTerms(XmlUtils.NodeAsString(node, ".//ram:Description", nsmgr),
											XmlUtils.NodeAsDateTime(node, ".//ram:DueDateDateTime", nsmgr),
											paymentTermsType,
											discountDueDays ?? penaltyDueDays,
											discountPercent ?? penaltyPercent,
											discountAmount ?? penaltyAmount);
			}

			retval.LineTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:LineTotalAmount", nsmgr, 0).Value;
			retval.ChargeTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:ChargeTotalAmount", nsmgr, null);
			retval.AllowanceTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:AllowanceTotalAmount", nsmgr, null);
			retval.TaxBasisAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxBasisTotalAmount", nsmgr, null);
			retval.TaxTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TaxTotalAmount", nsmgr, 0).Value;
			retval.GrandTotalAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:GrandTotalAmount", nsmgr, 0).Value;
			retval.RoundingAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:RoundingAmount", nsmgr, 0).Value;
			retval.TotalPrepaidAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:TotalPrepaidAmount", nsmgr, null);
			retval.DuePayableAmount = XmlUtils.NodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementHeaderMonetarySummation/ram:DuePayableAmount", nsmgr, 0).Value;

			foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableHeaderTradeSettlement/ram:ReceivableSpecifiedTradeAccountingAccount", nsmgr))
			{
				retval.ReceivableSpecifiedTradeAccountingAccounts.Add(new ReceivableSpecifiedTradeAccountingAccount()
				{
					TradeAccountID = XmlUtils.NodeAsString(node, ".//ram:ID", nsmgr),
					TradeAccountTypeCode = XmlUtils.NodeAsEnum<AccountingAccountTypeCodes>(node, ".//ram:TypeCode", nsmgr),
				});
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

			string _lineId = _lineId = XmlUtils.NodeAsString(tradeLineItem, ".//ram:AssociatedDocumentLineDocument/ram:LineID", nsmgr, String.Empty);
			TradeLineItem item = new TradeLineItem(_lineId)
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
							bool chargeIndicator = XmlUtils.NodeAsBool(LineTradeSettlementNode, "./ram:ChargeIndicator/udt:Indicator", nsmgr);
							decimal? basisAmount = XmlUtils.NodeAsDecimal(LineTradeSettlementNode, "./ram:BasisAmount", nsmgr, null);
							string basisAmountCurrency = XmlUtils.NodeAsString(LineTradeSettlementNode, "./ram:BasisAmount/@currencyID", nsmgr);
							decimal actualAmount = XmlUtils.NodeAsDecimal(LineTradeSettlementNode, "./ram:ActualAmount", nsmgr, 0).Value;
							string actualAmountCurrency = XmlUtils.NodeAsString(LineTradeSettlementNode, "./ram:ActualAmount/@currencyID", nsmgr);
							string reason = XmlUtils.NodeAsString(LineTradeSettlementNode, "./ram:Reason", nsmgr);
							decimal? chargePercentage = XmlUtils.NodeAsDecimal(LineTradeSettlementNode, "./ram:CalculationPercent", nsmgr, null);

							item.AddSpecifiedTradeAllowanceCharge(!chargeIndicator, // wichtig: das not (!) beachten
														default(CurrencyCodes).FromString(basisAmountCurrency),
														basisAmount,
														actualAmount,
														chargePercentage,
														reason);
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
								TradeAccountID = XmlUtils.NodeAsString(LineTradeSettlementNode, "./ram:ID", nsmgr),
								TradeAccountTypeCode = XmlUtils.NodeAsEnum<AccountingAccountTypeCodes>(LineTradeSettlementNode, ".//ram:TypeCode", nsmgr)
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
				string actualAmountCurrency = XmlUtils.NodeAsString(appliedTradeAllowanceChargeNode, "./ram:ActualAmount/@currencyID", nsmgr);
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

			//Get all referenced AND embedded documents
			XmlNodeList referenceNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedLineTradeAgreement/ram:AdditionalReferencedDocument", nsmgr);
			foreach (XmlNode referenceNode in referenceNodes)
			{
				item._AdditionalReferencedDocuments.Add(_readAdditionalReferencedDocument(referenceNode, nsmgr));
			}

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
				ReferenceTypeCode = default(ReferenceTypeCodes).FromString(XmlUtils.NodeAsString(node, "ram:ReferenceTypeCode", nsmgr))
			};
		} // !_readAdditionalReferencedDocument()

	}
}
