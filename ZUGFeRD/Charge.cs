using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public abstract class Charge
    {
        public Tax Tax { get; set; }
        public decimal Amount { get; set; }
    }
}
