using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptorReader
    {
        public static InvoiceDescriptor Load(string filename)
        {
            if (!System.IO.File.Exists(filename))
            {
                /***
                 * @todo throw exception
                 */
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(filename);

            InvoiceDescriptor retval = new InvoiceDescriptor();
            retval.Profile = Profile.FromString(_nodeAsString(doc, "/GuidelineSpecifiedDocumentContextParameter/ID"));
            retval.Type = InvoiceType.FromString(_nodeAsString(doc, "/rsm:HeaderExchangedDocument/TypeCode"));
        } // !Load()


        private string _nodeAsString(XmlDocument document, string xpath, string defaultValue = "")
        {
            try
            {
                XmlNode node = document.SelectSingleNode(xpath);
                return node.InnerText;
            }
            catch (XPathException ex)
            {
                return defaultValue;
            }
            catch (Exception ex)
            {
                throw ex;
            };
        } // _nodeAsString()
    }
}
