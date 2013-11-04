using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public class ServiceCharge
    {
        public Tax Tax { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
