using Microsoft.VisualStudio.TestTools.UnitTesting;
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZUGFeRD_Test
{
    [TestClass]
    public class XRechnungUBLTests : TestBase
    {
        InvoiceProvider InvoiceProvider = new InvoiceProvider();


        [TestMethod]
        public void TestInvoiceCreation()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();
            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version22, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.AreEqual(loadedInvoice.Invoicee, null);
            Assert.AreNotEqual(loadedInvoice.Seller, null);
            Assert.AreEqual(loadedInvoice.Taxes.Count, 2);
            Assert.AreEqual(loadedInvoice.SellerContact.Name, "Max Mustermann");
            Assert.IsNull(loadedInvoice.BuyerContact);
        } // !TestInvoiceCreation()


        [TestMethod]
        public void TestTradelineitemProductCharacterstics()
        {
            InvoiceDescriptor desc = this.InvoiceProvider.CreateInvoice();

            desc.TradeLineItems[0].ApplicableProductCharacteristics = new ApplicableProductCharacteristic[]
                    {
                        new ApplicableProductCharacteristic()
                        {
                            Description = "Test Description",
                            Value = "1.5 kg"
                        },
                        new ApplicableProductCharacteristic()
                        {
                            Description = "UBL Characterstics 2",
                            Value = "3 kg"
                        },
                    }.ToList();

            MemoryStream ms = new MemoryStream();

            desc.Save(ms, ZUGFeRDVersion.Version22, Profile.XRechnung, ZUGFeRDFormats.UBL);
            ms.Seek(0, SeekOrigin.Begin);

            InvoiceDescriptor loadedInvoice = InvoiceDescriptor.Load(ms);

            Assert.IsNotNull(loadedInvoice.TradeLineItems);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics.Count, 2);
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics[0].Description, "Test Description");
            Assert.AreEqual(loadedInvoice.TradeLineItems[0].ApplicableProductCharacteristics[1].Value, "3 kg");
        } // !TestTradelineitemProductCharacterstics()
    }
}
