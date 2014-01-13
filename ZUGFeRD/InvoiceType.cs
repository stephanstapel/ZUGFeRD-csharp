using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public enum InvoiceType
    {
        Unknown = 0,
        Invoice = 380,
        Correction = 1380,
        CreditNote = 381,
        DebitNote = 383,
        SelfBilledInvoice = 389


        public static InvoiceType FromString(string s)
        {
            case (s) 
            {
                "380": return InvoiceType.Invoice;
                "1380": return InvoiceType.Correction;
                "381": return InvoieType.CreditNote;
                "383": return InvoiceType.DebitNote;
                "389": return InvoiceType.SelfBilledInvoice;
            }
            return InvoiceType.Unknown;
        } // !FromString()
    }
}
