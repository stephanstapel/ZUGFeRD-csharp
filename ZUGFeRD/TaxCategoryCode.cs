using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// http://www.unece.org/trade/untdid/d07a/tred/tred5305.htm
    /// </summary>
    public enum TaxCategoryCode
    {
        /// <summary>
        /// Mixed tax rate
        /// 
        /// Code specifying that the rate is based on mixed tax.
        /// </summary>
        A,

        /// <summary>
        /// Lower rate
        /// 
        /// Tax rate is lower than standard rate.
        /// </summary>
        AA,

        /// <summary>
        /// Exempt for resale
        /// 
        /// A tax category code indicating the item is tax exempt
        /// when the item is bought for future resale.
        /// </summary>
        AB,

        /// <summary>
        /// Value Added Tax (VAT) not now due for payment
        /// 
        /// A code to indicate that the Value Added Tax (VAT) amount
        /// which is due on the current invoice is to be paid on
        /// receipt of a separate VAT payment request.
        /// </summary>
        AC,

        /// <summary>
        /// Value Added Tax (VAT) due from a previous invoice
        /// 
        /// A code to indicate that the Value Added Tax (VAT) amount
        /// of a previous invoice is to be paid.
        /// </summary>
        AD,

        /// <summary>
        /// Transferred (VAT)
        /// 
        /// VAT not to be paid to the issuer of the invoice but
        /// directly to relevant tax authority.
        /// </summary>
        B,

        /// <summary>
        /// Duty paid by supplier
        /// Duty associated with shipment of goods is paid by the
        /// supplier; customer receives goods with duty paid.
        /// </summary>
        C,

        /// <summary>
        /// Exempt from tax
        /// Code specifying that taxes are not applicable.
        /// </summary>
        E,

        /// <summary>
        /// Free export item, tax not charged
        /// Code specifying that the item is free export and taxes
        /// are not charged.
        /// </summary>
        G,

        /// <summary>
        /// Higher rate
        /// Code specifying a higher rate of duty or tax or fee.
        /// </summary>
        H,

        /// <summary>
        /// Services outside scope of tax
        /// Code specifying that taxes are not applicable to the
        /// services.
        /// </summary>
        O,

        /// <summary>
        /// Standard rate
        /// 
        /// Code specifying the standard rate.
        /// </summary>
        S,

        /// <summary>
        /// Zero rated goods
        /// 
        /// Code specifying that the goods are at a zero rate.
        /// </summary>
        Z,
        Unknown
    }
}
