using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    internal class Party
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string StreetNo { get; set; }
        public string Street { get; set; }
        public GlobalID GlobalID { get; set; }
    }
}
