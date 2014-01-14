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
        public static InvoiceDescriptor Load(FileStream stream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.DocumentElement.OwnerDocument.NameTable);
            nsmgr.AddNamespace("rsm", doc.DocumentElement.OwnerDocument.DocumentElement.NamespaceURI);

            InvoiceDescriptor retval = new InvoiceDescriptor();
            retval.Profile = default(Profile).FromString(_nodeAsString(doc, "//GuidelineSpecifiedDocumentContextParameter/ID", nsmgr));
            retval.Type = default(InvoiceType).FromString(_nodeAsString(doc, "//rsm:HeaderExchangedDocument/TypeCode", nsmgr));

            foreach(XmlNode node in doc.SelectNodes("//IncludedSupplyChainTradeLineItem"))
            {
                retval.TradeLineItems.Add(_parseTradeLineItem(node));
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
            return new TradeLineItem()
                {
                    GlobalID = new GlobalID(_nodeAsString(tradeLineItem, "//SpecifiedTradeProduct/GlobalID/@schemeID", nsmgr),
                                            _nodeAsString(tradeLineItem, "//SpecifiedTradeProduct/GlobalID", nsmgr)),
                    SellerAssignedID = _nodeAsString(tradeLineItem, "//SpecifiedTradeProduct/SellerAssignedID", nsmgr),
                    BuyerAssignedID = _nodeAsString(tradeLineItem, "//SpecifiedTradeProduct/BuyerAssignedID", nsmgr),
                    Name = _nodeAsString(tradeLineItem, "//SpecifiedTradeProduct/Name", nsmgr),
                    Description = _nodeAsString(tradeLineItem, "//SpecifiedTradeProduct/Description", nsmgr),
                    UnitQuantity = _nodeAsInt(tradeLineItem, "//BasisQuantity", nsmgr, 1),
                    BilledQuantity = _nodeAsInt(tradeLineItem, "//BilledQuantity", nsmgr, 1),
                    TaxCategoryCode = default(TaxCategoryCodes).FromString(_nodeAsString(tradeLineItem, "//ApplicableTradeTax/CategoryCode", nsmgr)),
                    TaxType = default(TaxTypes).FromString(_nodeAsString(tradeLineItem, "//ApplicableTradeTax/TypeCode", nsmgr)),
                    TaxPercent = _nodeAsDecimal(tradeLineItem, "//ApplicableTradeTax/ApplicablePercent", nsmgr),
                    NetUnitPrice = _nodeAsDecimal(tradeLineItem, "//GrossPriceProductTradePrice/ChargeAmount", nsmgr),
                    GrossUnitPrice = _nodeAsDecimal(tradeLineItem, "//NetPriceProductTradePrice/ChargeAmount", nsmgr),
                    UnitCode = default(QuantityCodes).FromString(_nodeAsString(tradeLineItem, "//BasisQuantity/@unitCode", nsmgr))
                };
        } // !_parseTradeLineItem()


        private static string _nodeAsString(XmlNode node , string xpath, XmlNamespaceManager nsmgr = null, string defaultValue = "")
        {
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


        private static string _nodeAsString(XmlDocument document, string xpath, XmlNamespaceManager nsmgr = null, string defaultValue = "")
        {
            return _nodeAsString(document.DocumentElement, xpath, nsmgr, defaultValue);
        } // _nodeAsString()


        private static int _nodeAsInt(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, int defaultValue = 0)
        {
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


        private static decimal _nodeAsDecimal(XmlNode node, string xpath, XmlNamespaceManager nsmgr = null, decimal defaultValue = 0)
        {
            string temp = _nodeAsString(node, xpath, nsmgr, "");
            decimal retval;
            if (decimal.TryParse(temp, out retval))
            {
                return retval;
            }
            else
            {
                return defaultValue;
            }
        } // !_nodeAsDecimal()
    }
}
