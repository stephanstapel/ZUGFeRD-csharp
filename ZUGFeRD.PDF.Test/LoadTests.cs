using s2industries.ZUGFeRD;
using s2industries.ZUGFeRD.PDF;

namespace ZUGFeRD.PDF.Test
{
    [TestClass]
    public sealed class LoadTests
    {
        [TestMethod]
        public async Task BasicLoadExampleFile()
        {
            string path = @"..\..\..\..\documentation\zugferd23en\Examples\4. EXTENDED\EXTENDED_Warenrechnung\EXTENDED_Warenrechnung.pdf";
            InvoiceDescriptor desc = await InvoicePdfProcessor.LoadFromPdfAsync(path);

            Assert.IsNotNull(desc);
            Assert.AreEqual("R87654321012345", desc.InvoiceNo);
        } // !BasicLoadExampleFile()


        [TestMethod]
        public async Task BasicLoadNonExistingFile()
        {
            string path = @"doesnotexist.pdf";            
            await Assert.ThrowsExceptionAsync<FileNotFoundException>(() => InvoicePdfProcessor.LoadFromPdfAsync(path));
        } // !BasicLoadNonExistingFile()
    }
}
