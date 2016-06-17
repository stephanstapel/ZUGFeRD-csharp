using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public enum ContentCodes
    {
        /// <summary>
        ///  Die Ware bleibt bis zur vollständigen Bezahlung
        ///  unser Eigentum. 
        /// </summary>
        EEV,

        /// <summary>
        /// Die Ware bleibt bis zur vollständigen Bezahlung
        /// aller Forderungen unser Eigentum. 
        /// </summary>
        WEV,

        /// <summary>
        /// Die Ware bleibt bis zur vollständigen Bezahlung
        /// unser Eigentum. Dies gilt auch im Falle der
        /// Weiterveräußerung oder -verarbeitung der Ware.
        /// </summary>
        VEV,

        /// <summary>
        /// Es ergeben sich Entgeltminderungen auf Grund von
        /// Rabatt- und Bonusvereinbarungen. 
        /// </summary>
        ST1,

        /// <summary>
        /// Entgeltminderungen ergeben sich aus unseren
        /// aktuellen Rahmen- und Konditionsvereinbarungen. 
        /// </summary>
        ST2,

        /// <summary>
        /// Es bestehen Rabatt- oder Bonusvereinbarungen.
        /// </summary>
        ST3,

        Unknown
    }



    internal static class ContentCodesExtensions
    {
        public static ContentCodes FromString(this ContentCodes _c, string s)
        {
            try
            {
                return (ContentCodes)Enum.Parse(typeof(ContentCodes), s);
            }
            catch
            {
                return ContentCodes.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this ContentCodes c)
        {
            return c.ToString("g");
        } // !ToString()
    }
}
