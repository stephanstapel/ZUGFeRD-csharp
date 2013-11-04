using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public enum InvoiceType
    {
        Invoice = 380,
        Correction = 1380,
        CreditNote = 381,
        DebitNote = 383,
        SelfBilledInvoice = 389
    }
}
