using System;
using System.Collections.Generic;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// An included Item referenced from this trade product.
    /// </summary>
    public class IncludedItem
    {
        /// <summary>
        /// Name of Included Item
        /// 
        /// BT-X-18
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Included quantity
        /// 
        /// BT-X-20
        /// </summary>
        public decimal? UnitQuantity { get; set; }

        /// <summary>
        /// Item Base Quantity Unit Code
        /// 
        /// BT-X-20-1
        /// </summary>
        public QuantityCodes UnitCode { get; set; }
    }
}
