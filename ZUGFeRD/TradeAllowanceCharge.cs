using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Zu- und Abschlag
    /// 
    /// Beispiel:
    /// <SpecifiedTradeAllowanceCharge>
	///   <ChargeIndicator>false</ChargeIndicator>
    ///      <BasisAmount currencyID="EUR">137.30</BasisAmount>
    ///      <ActualAmount>13.73</ActualAmount>
    ///      <Reason>Sondernachlass</Reason>
    ///      <CategoryTradeTax>
    ///        <TypeCode>VAT</TypeCode>
    ///        <CategoryCode>S</CategoryCode>
    ///        <ApplicablePercent>7</ApplicablePercent>
    ///      </CategoryTradeTax>
    ///    </SpecifiedTradeAllowanceCharge>
    /// </summary>
    public class TradeAllowanceCharge : Charge
    {
        /// <summary>
        /// Schalter für Zu- und Abschlag
        /// 
        /// false: Abschlag
        /// true: Zuschlag
        /// </summary>
        public bool ChargeIndicator { get; set; }
        public string Reason { get; set; }
        public decimal BasisAmount { get; set; }
        public CurrencyCodes Currency { get; set; }
    }
}
