using s2industries.ZUGFeRD;
using s2industries.ZUGFeRDRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZUGFeRDRendererDemo
{
    internal class Application
    {
        internal async Task RunAsync()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.Load("../../../../demodata/zugferd22/zugferd_2p2_EXTENDED_Fremdwaehrung-factur-x.xml");
            string html = InvoiceDescriptorHtmlRenderer.render(desc);
            System.IO.File.WriteAllText("output.html", html);
        } // !RunAsync()
    }
}
