using System;
using System.Collections.Generic;
using System.Text;

namespace s2industries.ZUGFeRD
{
    internal class UBLTaxRegistrationSchemeIDMapper
    {
        internal static TaxRegistrationSchemeID Map(string value)
        {
            if (value.Trim().Equals("VAT", StringComparison.OrdinalIgnoreCase))
            {
                return TaxRegistrationSchemeID.VA;
            }
            else if (value.Trim().Equals("ID", StringComparison.OrdinalIgnoreCase))
            {
                return TaxRegistrationSchemeID.FC;
            }
            else
            {
                return default(TaxRegistrationSchemeID).FromString(value);
            }
        } // !Map()


        internal static string Map(TaxRegistrationSchemeID type)
        {
            if (type == TaxRegistrationSchemeID.VA)
            {
                return "VAT";
            }
            else if (type == TaxRegistrationSchemeID.FC)
            {
                return "ID";
            }
            else
            {
                return type.EnumToString(); 
            }
        } // !Map()
    }
}
