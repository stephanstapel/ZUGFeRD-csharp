using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public class Tax
    {
        public decimal TaxAmount { get; set; }
        public decimal BasisAmount { get; set; }
        public decimal Percent { get; set; }
        public TaxType TypeCode { get; set; }
        public TaxCategoryCode CategoryCode { get; set; }


        public Tax()
        {
            this.CategoryCode = TaxCategoryCode.S;
        }
    }
}
