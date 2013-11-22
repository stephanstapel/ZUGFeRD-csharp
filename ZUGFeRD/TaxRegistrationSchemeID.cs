using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// For a reference see:
    /// http://www.unece.org/trade/untdid/d00a/tred/tred1153.htm
    /// </summary>
    public enum TaxRegistrationSchemeID
    {
        /// <summary>
        /// Fiscal number
        /// 
        /// Tax payer's number. Number assigned to individual
        /// persons as well as to corporates by a public
        /// institution; this number is different from the VAT
        /// registration number.
        /// </summary>
        FC,

        /// <summary>
        /// VAT registration number
        /// 
        /// Unique number assigned by the relevant tax authority to
        /// identify a party for use in relation to Value Added Tax
        /// (VAT).
        /// </summary>
        VA,
        Unknown
    }
}
