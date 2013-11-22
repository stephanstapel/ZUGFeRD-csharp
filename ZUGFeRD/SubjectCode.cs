using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// http://www.stylusstudio.com/edifact/D02A/4451.htm
    /// </summary>
    public enum SubjectCode
    {
        /// <summary>
        /// Goods description
        /// 
        /// [7002] Plain language description of the nature of the goods 
        /// sufficient to identify them at the level required for banking, 
        /// Customs, statistical or transport purposes, avoiding unnecessary 
        /// detail (Generic term).
        /// </summary>
        AAA,

        /// <summary>
        /// Terms of payments
        /// 
        /// [4276] Conditions of payment between the parties to a transaction (generic term).
        /// </summary>
        AAB,

        /// <summary>
        /// Dangerous goods additional information
        /// 
        /// Additional information concerning dangerous goods.
        /// </summary>
        AAC,
        Unknown
    }
}
