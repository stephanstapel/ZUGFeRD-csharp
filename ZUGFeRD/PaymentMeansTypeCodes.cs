namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Adopted to ZUGFeRD 1.0, German description from ZUGFeRD specification
    /// </summary>
    public enum PaymentMeansTypeCodes
    {
        Unknown = 0,

        /// <summary>
        /// Keine Zahlungsart definiert
        /// Available in: Extended
        /// </summary>
        NotDefined = 1,

        /// <summary>
        /// Belastung durch automatisierte Clearingstelle, Z.B. bei Abwicklung durch Zahlungsdienstleister wie Online-Bezahlsysteme
        /// </summary>
        AutomatedClearingHouseDebit = 3,

        /// <summary>
        /// Bar
        /// Available in: Basic, Extended
        /// </summary>
        InCash = 10,

        /// <summary>
        /// Scheck
        /// Available in: Basic, Extended
        /// </summary>
        Cheque = 20,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>
        CreditTransfer = 30,

        /// <summary>
        /// Lastschriftübermittlung:
        /// Zahlung durch Belastung eines Geldbetrages eines
        /// Kontos zugunsten eines anderen.
        /// Überweisung international und nationale SEPA-Überweisung
        /// 
        /// Available in: Extended
        /// </summary>
        DebitTransfer = 31,

        /// <summary>
        /// Zahlung an Bankkonto
        /// Überweisung national, vor SEPA-Umstellung
        /// Available in: Basic, Extended
        /// </summary>
        PaymentToBankAccount = 42,

        /// <summary>
        /// Bankkkarte, Kreditkarte
        /// Available in: Basic, Extended
        /// </summary>
        BankCard = 48,

        /// <summary>
        /// Lastschriftverfahren
        /// 
        /// Available in: Basic, Extended
        /// /// </summary>
        DirectDebit = 49,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>        
        StandingAgreement = 57,


        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>
        SEPACreditTransfer = 58,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>        
        SEPADirectDebit = 59,

        /// <summary>
        /// Ausgleich zwischen Partnern.
        /// Beträge, die zwei Partner sich gegenseitig schulden werden ausgeglichen um unnütze Zahlungen zu vermeiden.
        /// Available in: Basic, Extended
        /// </summary>
        ClearingBetweenPartners = 97
    }


    internal static class PaymentMeansTypeCodesExtensions
    {
        public static PaymentMeansTypeCodes FromString(this PaymentMeansTypeCodes _type, string s)
        {
            switch (s)
            {
                case "1": return PaymentMeansTypeCodes.NotDefined;
                case "3": return PaymentMeansTypeCodes.AutomatedClearingHouseDebit;
                case "10": return PaymentMeansTypeCodes.InCash;
                case "20": return PaymentMeansTypeCodes.Cheque;
                case "30": return PaymentMeansTypeCodes.CreditTransfer;
                case "31": return PaymentMeansTypeCodes.DebitTransfer;
                case "42": return PaymentMeansTypeCodes.PaymentToBankAccount;
                case "48": return PaymentMeansTypeCodes.BankCard;
                case "49": return PaymentMeansTypeCodes.DirectDebit;
                case "57": return PaymentMeansTypeCodes.StandingAgreement;
                case "58": return PaymentMeansTypeCodes.SEPACreditTransfer;
                case "59": return PaymentMeansTypeCodes.SEPADirectDebit;
                case "97": return PaymentMeansTypeCodes.ClearingBetweenPartners;
            }
            return PaymentMeansTypeCodes.Unknown;
        } // !FromString()


        public static string EnumToString(this PaymentMeansTypeCodes c)
        {
            return ((int)c).ToString();
        } // !ToString()
    }
}