using System;
using System.Collections.Generic;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public sealed class InvoiceFormatOptions
    {
        public List<string> XmlHeaderComments { get; set; } = new List<string>();
        public bool IncludeXmlComments { get; internal set; } = false;


        internal InvoiceFormatOptions Clone()
        {
            return new InvoiceFormatOptions
            {
                XmlHeaderComments = new List<string>(this.XmlHeaderComments),
                IncludeXmlComments = this.IncludeXmlComments
            };
        } // !Clone()
    }
}
