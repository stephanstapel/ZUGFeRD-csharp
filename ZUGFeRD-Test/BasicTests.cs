using Microsoft.VisualStudio.TestTools.UnitTesting;
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZUGFeRD_Test
{
    [TestClass]
    public class BasicTests
    {
        InvoiceProvider InvoiceProvider = new InvoiceProvider();


        [TestMethod]
        public void TestAutomaticLineIds()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();

            desc.AddTradeLineItem("Item1");
            desc.AddTradeLineItem("Item2");

            Assert.AreEqual(desc.TradeLineItems[0].AssociatedDocument.LineID, "1");
            Assert.AreEqual(desc.TradeLineItems[1].AssociatedDocument.LineID, "2");
        } // !TestAutomaticLineIds()



        [TestMethod]
        void TestManualLineIds()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            desc.TradeLineItems.Clear();
            desc.AddTradeLineItem(lineID: "item-01", "Item1");
            desc.AddTradeLineItem(lineID: "item-02", "Item2");

            Assert.AreEqual(desc.TradeLineItems[0].AssociatedDocument.LineID, "item-01");
            Assert.AreEqual(desc.TradeLineItems[1].AssociatedDocument.LineID, "item-02");
        } // !TestManualLineIds()
    }
}
