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
        /// </summary>
        PaymentMeans_1 = 1,

        /// <summary>
        /// Belastung durch automatisierte Clearingstelle, Z.B. bei Abwicklung durch Zahlungsdienstleister wie Online-Bezahlsysteme
        /// </summary>
        PaymentMeans_3 = 3,

        /// <summary>
        /// Bar
        /// </summary>
        PaymentMeans_10 = 10,

        /// <summary>
        /// Scheck
        /// </summary>
        PaymentMeans_20 = 20,

        /// <summary>
        /// Lastschriftübermittlung:
        /// Zahlung durch Belastung eines Geldbetrages eines
        /// Kontos zugunsten eines anderen.
        /// Überweisung international und nationale SEPA-Überweisung
        /// </summary>
        PaymentMeans_31 = 31,

        /// <summary>
        /// Zahlung an Bankkonto
        /// Überweisung national, vor SEPA-Umstellung
        /// </summary>
        PaymentMeans_42 = 42,

        /// <summary>
        /// Bankkkarte, Kreditkarte
        /// </summary>
        PaymentMeans_48 = 48,

        /// <summary>
        /// Lastschriftverfahren
        /// </summary>
        PaymentMeans_49 = 49,

        /// <summary>
        /// Ausgleich zwischen Partnern.
        /// Beträge, die zwei Partner sich gegenseitig schulden werden ausgeglichen um unnütze Zahlungen zu vermeiden.
        /// </summary>
        PaymentMeans_97 = 97
    }


    internal static class PaymentMeansTypeCodesExtensions
    {
        public static PaymentMeansTypeCodes FromString(this PaymentMeansTypeCodes _type, string s)
        {
            switch (s)
            {
                case "1": return PaymentMeansTypeCodes.PaymentMeans_1;
                case "3": return PaymentMeansTypeCodes.PaymentMeans_3;
                case "10": return PaymentMeansTypeCodes.PaymentMeans_10;
                case "20": return PaymentMeansTypeCodes.PaymentMeans_20;
                case "31": return PaymentMeansTypeCodes.PaymentMeans_31;
                case "42": return PaymentMeansTypeCodes.PaymentMeans_42;
                case "48": return PaymentMeansTypeCodes.PaymentMeans_48;
                case "49": return PaymentMeansTypeCodes.PaymentMeans_49;
                case "97": return PaymentMeansTypeCodes.PaymentMeans_97;
            }
            return PaymentMeansTypeCodes.Unknown;
        } // !FromString()


        public static string EnumToString(this PaymentMeansTypeCodes c)
        {
            return ((int)c).ToString();
        } // !ToString()
    }
}