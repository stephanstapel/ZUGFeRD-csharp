using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public class PaymentTerms
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

        public PaymentTerms()
        {
            this.DueDate = DateTime.MinValue;
        }
    }
}
