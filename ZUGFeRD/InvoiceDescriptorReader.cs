using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptorReader
    {
        public static InvoiceDescriptor Load(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new IllegalStreamException("Cannot read from stream");
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.DocumentElement.OwnerDocument.NameTable);
            nsmgr.AddNamespace("rsm", "urn:ferd:CrossIndustryDocument:invoice:1p0");
            nsmgr.AddNamespace("ram", "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:12");
            nsmgr.AddNamespace("udt", "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:15");

            InvoiceDescriptor retval = new InvoiceDescriptor();
            retval.IsTest = _nodeAsBool(doc.DocumentElement, "//rsm:SpecifiedExchangedDocumentContext/ram:TestIndicator", nsmgr);
            retval.Profile = default(Profile).FromString(_nodeAsString(doc.DocumentElement, "//ram:GuidelineSpecifiedDocumentContextParameter/ram:ID", nsmgr));
            retval.Type = default(InvoiceType).FromString(_nodeAsString(doc.DocumentElement, "//rsm:HeaderExchangedDocument/ram:TypeCode", nsmgr));
            retval.InvoiceNo = _nodeAsString(doc.DocumentElement, "//rsm:HeaderExchangedDocument/ram:ID", nsmgr);
            retval.InvoiceDate = _nodeAsDateTime(doc.DocumentElement, "//rsm:HeaderExchangedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);

            foreach (XmlNode node in doc.SelectNodes("//rsm:HeaderExchangedDocument/ram:IncludedNote", nsmgr))
            {
                string content = _nodeAsString(node, ".//ram:Content", nsmgr);
                string _subjectCode = _nodeAsString(node, ".//ram:SubjectCode", nsmgr);
                SubjectCodes subjectCode = default(SubjectCodes).FromString(_subjectCode);
                retval.AddNote(content, subjectCode);
            }

            retval.ReferenceOrderNo = _nodeAsString(doc, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerReference", nsmgr);

            retval.Seller = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:SellerTradeParty", nsmgr);
            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableSupplyChainTradeAgreement/ram:SellerTradeParty/ram:SpecifiedTaxRegistration", nsmgr))
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
                    EmailAddress = _nodeAsString(doc.DocumentElement, "//ram:SellerTradeParty/ram:DefinedTradeContact/ram:EmailURIUniversalCommunication/ram:CompleteNumber", nsmgr)
                };
            }

            retval.Buyer = _nodeAsParty(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerTradeParty", nsmgr);
            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerTradeParty/ram:SpecifiedTaxRegistration", nsmgr))
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
                    EmailAddress = _nodeAsString(doc.DocumentElement, "//ram:BuyerTradeParty/ram:DefinedTradeContact/ram:EmailURIUniversalCommunication/ram:CompleteNumber", nsmgr)
                };
            }

            retval.ActualDeliveryDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:ActualDeliverySupplyChainEvent/ram:OccurrenceDateTime/udt:DateTimeString", nsmgr);

            string _deliveryNoteNo = _nodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ID", nsmgr);
            DateTime? _deliveryNoteDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);

            if (_deliveryNoteDate.HasValue || !String.IsNullOrEmpty(_deliveryNoteNo))
            {
                retval.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
                {
                    ID = _deliveryNoteNo,
                    IssueDateTime = _deliveryNoteDate
                };
            }

            retval.InvoiceNoAsReference = _nodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:PaymentReference", nsmgr);
            retval.Currency = default(CurrencyCodes).FromString(_nodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:InvoiceCurrencyCode", nsmgr));

            PaymentMeans _tempPaymentMeans= new PaymentMeans()
            {
                TypeCode = default(PaymentMeansTypeCodes).FromString(_nodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:TypeCode", nsmgr)),
                Information = _nodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:Information", nsmgr)
            };
            retval.PaymentMeans = _tempPaymentMeans;
            
            XmlNodeList financialAccountNodes = doc.SelectNodes("//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeePartyCreditorFinancialAccount", nsmgr);
            XmlNodeList financialInstitutions = doc.SelectNodes("//ram:ApplicableSupplyChainTradeSettlement/ram:SpecifiedTradeSettlementPaymentMeans/ram:PayeeSpecifiedCreditorFinancialInstitution", nsmgr);

            if (financialAccountNodes.Count == financialInstitutions.Count)
            {
                for (int i = 0; i < financialAccountNodes.Count; i++)
                {
                    BankAccount _account = new BankAccount() 
                    {
                        ID = _nodeAsString(financialAccountNodes[0], ".//ram:ProprietaryID", nsmgr),
                        IBAN = _nodeAsString(financialAccountNodes[0], ".//ram:IBANID", nsmgr),
                        BIC = _nodeAsString(financialInstitutions[0], ".//ram:BICID", nsmgr),
                        Bankleitzahl = _nodeAsString(financialInstitutions[0], ".//ram:GermanBankleitzahlID", nsmgr),
                        BankName = _nodeAsString(financialInstitutions[0], ".//ram:Name", nsmgr),
                    };

                    retval.CreditorBankAccounts.Add(_account);
                } // !for(i)
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:ApplicableSupplyChainTradeSettlement/ram:ApplicableTradeTax", nsmgr))
            {
                retval.AddApplicableTradeTax(_nodeAsDecimal(node, ".//ram:BasisAmount", nsmgr, 0).Value,
                                             _nodeAsDecimal(node, ".//ram:ApplicablePercent", nsmgr, 0).Value,
                                             default(TaxTypes).FromString(_nodeAsString(node, ".//ram:TypeCode", nsmgr)),
                                             default(TaxCategoryCodes).FromString(_nodeAsString(node, ".//ram:CategoryCode", nsmgr)));
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
                                               _nodeAsDecimal(node, ".//ram:CategoryTradeTax/ram:ApplicablePercent", nsmgr, 0).Value);
            }

            foreach (XmlNode node in doc.SelectNodes("//ram:SpecifiedLogisticsServiceCharge", nsmgr))
            {
                retval.AddLogisticsServiceCharge(_nodeAsDecimal(node, ".//ram:AppliedAmount", nsmgr, 0).Value,
                                                 _nodeAsString(node, ".//ram:Description", nsmgr),
                                                 default(TaxTypes).FromString(_nodeAsString(node, ".//ram:AppliedTradeTax/ram:TypeCode", nsmgr)),
                                                 default(TaxCategoryCodes).FromString(_nodeAsString(node, ".//ram:AppliedTradeTax/ram:CategoryCode", nsmgr)),
                                                 _nodeAsDecimal(node, ".//ram:AppliedTradeTax/ram:ApplicablePercent", nsmgr, 0).Value);
            }

            retval.PaymentTerms = new PaymentTerms()
            {
                Description = _nodeAsString(doc.DocumentElement, "//ram:SpecifiedTradePaymentTerms/ram:Description", nsmgr),
                DueDate = _nodeAsDateTime(doc.DocumentElement, "//ram:SpecifiedTradePaymentTerms/ram:DueDateDateTime", nsmgr)
            };

            retval.LineTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:LineTotalAmount", nsmgr, 0).Value;
            retval.ChargeTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:ChargeTotalAmount", nsmgr, 0).Value;
            retval.AllowanceTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:AllowanceTotalAmount", nsmgr, 0).Value;
            retval.TaxBasisAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:TaxBasisTotalAmount", nsmgr, 0).Value;
            retval.TaxTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:TaxTotalAmount", nsmgr, 0).Value;
            retval.GrandTotalAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:GrandTotalAmount", nsmgr, 0).Value;
            retval.TotalPrepaidAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:TotalPrepaidAmount", nsmgr, 0).Value;
            retval.DuePayableAmount = _nodeAsDecimal(doc.DocumentElement, "//ram:SpecifiedTradeSettlementMonetarySummation/ram:DuePayableAmount", nsmgr, 0).Value;

            retval.OrderDate = _nodeAsDateTime(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime/udt:DateTimeString", nsmgr);
            retval.OrderNo = _nodeAsString(doc.DocumentElement, "//ram:ApplicableSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:ID", nsmgr);
            
            
            foreach(XmlNode node in doc.SelectNodes("//ram:IncludedSupplyChainTradeLineItem", nsmgr))
            {
                retval.TradeLineItems.Add(_parseTradeLineItem(node, nsmgr));
            }
            return retval;
        } // !Load()


        public static InvoiceDescriptor Load(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                throw new FileNotFoundException();
            }

            return Load(new FileStream(filename, FileMode.Open, FileAccess.Read));
        } // !Load()


        private static TradeLineItem _parseTradeLineItem(XmlNode tradeLineItem, XmlNamespaceManager nsmgr = null)
        {
            if (tradeLineItem == null)
            {
                return null;
            }

            if (tradeLineItem.SelectSingleNode(".//ram:AssociatedDocumentLineDocument/ram:IncludedNote/ram:Content", nsmgr) != null)
            {
                return new TradeLineItem()
                {
                    Comment = _nodeAsString(tradeLineItem, ".//ram:AssociatedDocumentLineDocument/ram:IncludedNote/ram:Content", nsmgr)
                };
            }
            else
            {
                TradeLineItem item = new TradeLineItem()
                {
                    GlobalID = new GlobalID(_nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:GlobalID/@schemeID", nsmgr),
                                            _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:GlobalID", nsmgr)),
                    SellerAssignedID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:SellerAssignedID", nsmgr),
                    BuyerAssignedID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:BuyerAssignedID", nsmgr),
                    Name = _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:Name", nsmgr),
                    Description = _nodeAsString(tradeLineItem, ".//ram:SpecifiedTradeProduct/ram:Description", nsmgr),
                    UnitQuantity = _nodeAsDecimal(tradeLineItem, ".//ram:BasisQuantity", nsmgr, 1),
                    BilledQuantity = _nodeAsDecimal(tradeLineItem, ".//ram:BilledQuantity", nsmgr, 1).Value,
                    LineTotalAmount = _nodeAsDecimal(tradeLineItem, ".//ram:LineTotalAmount", nsmgr, 1),
                    TaxCategoryCode = default(TaxCategoryCodes).FromString(_nodeAsString(tradeLineItem, ".//ram:ApplicableTradeTax/ram:CategoryCode", nsmgr)),
                    TaxType = default(TaxTypes).FromString(_nodeAsString(tradeLineItem, ".//ram:ApplicableTradeTax/ram:TypeCode", nsmgr)),
                    TaxPercent = _nodeAsDecimal(tradeLineItem, ".//ram:ApplicableTradeTax/ram:ApplicablePercent", nsmgr, 0).Value,
                    NetUnitPrice = _nodeAsDecimal(tradeLineItem, ".//ram:NetPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                    GrossUnitPrice = _nodeAsDecimal(tradeLineItem, ".//ram:GrossPriceProductTradePrice/ram:ChargeAmount", nsmgr, 0).Value,
                    UnitCode = default(QuantityCodes).FromString(_nodeAsString(tradeLineItem, ".//ram:BasisQuantity/@unitCode", nsmgr))
                };

                XmlNodeList appliedTradeAllowanceChargeNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedSupplyChainTradeAgreement/ram:GrossPriceProductTradePrice/ram:AppliedTradeAllowanceCharge", nsmgr);
                foreach(XmlNode appliedTradeAllowanceChargeNode in appliedTradeAllowanceChargeNodes)
                {
                    bool chargeIndicator = _nodeAsBool(appliedTradeAllowanceChargeNode, "./ram:ChargeIndicator/udt:Indicator", nsmgr);
                    decimal basisAmount = _nodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:BasisAmount", nsmgr, 0).Value;
                    string basisAmountCurrency = _nodeAsString(appliedTradeAllowanceChargeNode, "./ram:BasisAmount/@currencyID", nsmgr);
                    decimal actualAmount = _nodeAsDecimal(appliedTradeAllowanceChargeNode, "./ram:ActualAmount", nsmgr, 0).Value;
                    string actualAmountCurrency = _nodeAsString(appliedTradeAllowanceChargeNode, "./ram:ActualAmount/@currencyID", nsmgr);
                    string reason = _nodeAsString(appliedTradeAllowanceChargeNode, "./ram:Reason", nsmgr);

                    item.addTradeAllowanceCharge(!chargeIndicator, // wichtig: das not (!) beachten
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

                if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:ID", nsmgr) != null)
                {
                    item.BuyerOrderReferencedDocument = new BuyerOrderReferencedDocument()
                    {
                        ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:ID", nsmgr),
                        IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:BuyerOrderReferencedDocument/ram:IssueDateTime", nsmgr),
                    };
                }

                if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:ID", nsmgr) != null)
                {
                    item.DeliveryNoteReferencedDocument = new DeliveryNoteReferencedDocument()
                    {
                        ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:ID", nsmgr),
                        IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeDelivery/ram:DeliveryNoteReferencedDocument/ram:IssueDateTime", nsmgr),
                    };
                }

                if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:ID", nsmgr) != null)
                {
                    item.ContractReferencedDocument = new ContractReferencedDocument()
                    {
                        ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:ID", nsmgr),
                        IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:IssueDateTime", nsmgr),
                    };
                }

                XmlNodeList referenceNodes = tradeLineItem.SelectNodes(".//ram:SpecifiedSupplyChainTradeAgreement/ram:AdditionalReferenceDocument", nsmgr);
                foreach(XmlNode referenceNode in referenceNodes)
                {
                    string _code = _nodeAsString(referenceNode, "ram:ReferenceTypeCode", nsmgr);

                    item.addAdditionalReferencedDocument(
                        id : _nodeAsString(referenceNode, "ram:ID", nsmgr),
                        date : _nodeAsDateTime(referenceNode, "ram:IssueDateTim", nsmgr),
                        code : default(ReferenceTypeCodes).FromString(_code)
                    );
                }

                if (tradeLineItem.SelectSingleNode(".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:ID", nsmgr) != null)
                {
                    item.ContractReferencedDocument = new ContractReferencedDocument()
                    {
                        ID = _nodeAsString(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:ID", nsmgr),
                        IssueDateTime = _nodeAsDateTime(tradeLineItem, ".//ram:SpecifiedSupplyChainTradeAgreement/ram:ContractReferencedDocument/ram:IssueDateTime", nsmgr),
                    };
                }


                return item;
            }
        } // !_parseTradeLineItem()


        private static bool _nodeAsBool(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, bool defaultValue = true)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string value = _nodeAsString(node, xpath, nsmgr);
            if (value == "")
            {
                return defaultValue;
            }
            else
            {
                if ((value.Trim().ToLower() == "true") || (value.Trim() == "1"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        } // !_nodeAsBool()


        private static string _nodeAsString(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, string defaultValue = "")
        {
            if (node == null)
            {
                return defaultValue;
            }

            try
            {
                XmlNode _node = node.SelectSingleNode(xpath, nsmgr);
                if (_node == null)
                {
                    return defaultValue;
                }
                else
                {
                    return _node.InnerText;
                }
            }
            catch (XPathException)
            {
                return defaultValue;
            }
            catch (Exception ex)
            {
                throw ex;
            };
        } // _nodeAsString()


        private static int _nodeAsInt(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, int defaultValue = 0)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string temp = _nodeAsString(node, xpath, nsmgr, "");
            int retval;
            if (Int32.TryParse(temp, out retval))
            {
                return retval;
            }
            else
            {
                return defaultValue;
            }
        } // !_nodeAsInt()
            

        private static decimal? _nodeAsDecimal(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, decimal? defaultValue = null)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string temp = _nodeAsString(node, xpath, nsmgr, "");
            decimal retval;
            if (decimal.TryParse(temp, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out retval))
            {
                return retval;
            }
            else
            {
                return defaultValue;
            }
        } // !_nodeAsDecimal()


        private static DateTime? _nodeAsDateTime(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, DateTime? defaultValue = null)
        {
            if (node == null)
            {
                return defaultValue;
            }

            string format = "";
            XmlNode dateNode = node.SelectSingleNode(xpath, nsmgr);
            if (dateNode == null)
            {
                if (defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                else
                {
                    return null;
                }
            }

            if (((XmlElement)dateNode).HasAttribute("format"))
            {
                format = dateNode.Attributes["format"].InnerText;
            }

            string rawValue = dateNode.InnerText;
            if ((format == "102") || (rawValue.Length == 8))
            {
                if (rawValue.Length != 8)
                {
                    throw new Exception("Wrong length of datetime element");
                }

                string year = rawValue.Substring(0, 4);
                string month = rawValue.Substring(4, 2);
                string day = rawValue.Substring(6, 2);

                return new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));
            }
            else if (rawValue.Length == 19)
            {
                string year = rawValue.Substring(0, 4);
                string month = rawValue.Substring(5, 2);
                string day = rawValue.Substring(8, 2);

                string hour = rawValue.Substring(11, 2);
                string minute = rawValue.Substring(14, 2);
                string second = rawValue.Substring(17, 2);

                return new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day), Int32.Parse(hour), Int32.Parse(minute), Int32.Parse(second));
            }
            else
            { 
                throw new UnsupportedException();
            }
            
        } // !_nodeAsDateTime()


        private static Party _nodeAsParty(XmlNode baseNode, string xpath, XmlNamespaceManager nsmgr = null)
        {
            if (baseNode == null)
            {
                return null;
            }

            XmlNode node = baseNode.SelectSingleNode(xpath, nsmgr);
            return new Party()
            {
                ID = _nodeAsString(node, "ram:ID", nsmgr),
                GlobalID = new GlobalID(_nodeAsString(node, "ram:GlobalID/@schemeID", nsmgr),
                                        _nodeAsString(node, "ram:GlobalID", nsmgr)),
                Name = _nodeAsString(node, "ram:Name", nsmgr),
                Street = _nodeAsString(node, "ram:PostalTradeAddress/ram:LineOne", nsmgr),
                Postcode = _nodeAsString(node, "ram:PostalTradeAddress/ram:PostcodeCode", nsmgr),
                City = _nodeAsString(node, "ram:PostalTradeAddress/ram:CityName", nsmgr),
                Country = default(CountryCodes).FromString(_nodeAsString(node, "ram:PostalTradeAddress/ram:CountryID", nsmgr))
            };
        } // !_nodeAsParty()
    }
}
