using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s2industries.ZUGFeRD
{
    public class Profile : Enumeration
    {
        public static int readonly Basic = 0;
        public static int readonly Comfort = 1;
        public static int readonly Extended = 2;

        public static Profile FromString(string s)
        {
            switch (s)
            {
                case "urn:ferd:invoice:1.0:basic": return Profile.Basic;
                case "urn:ferd:invoice:1.0:comfort": return Profile.Comfort;
                case "urn:ferd:invoice:1.0:extended": return Profile.Extended;
            }
        } // !FromString()


        public static string ToString(Profile profile)
        {
            switch (profile)
            {
                case Profile.Basic: return "urn:ferd:invoice:1.0:basic";
                case Profile.Comfort: return "urn:ferd:invoice:1.0:comfort";
                case Profile.Extended: return "urn:ferd:invoice:1.0:extended";
                default: return "";
            }
        } // !ToString()
    }
}
