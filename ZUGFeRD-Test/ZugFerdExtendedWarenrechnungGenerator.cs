using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZUGFeRD_Test
{
    class ZugFerdExtendedWarenrechnungGenerator
    {
        public void generate()
        {
            InvoiceDescriptor desc = _generateDescriptor();
            desc.Save("ZUGFeRD_1p0_EXTENDED_Warenrechnung.xml");
        } // !generate()


        private InvoiceDescriptor _generateDescriptor()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("R87654321012345", new DateTime(2013, 8, 6), CurrencyCodes.EUR);
            desc.IsTest = true;
            desc.Profile = Profile.Extended;

            desc.AddNote("Es bestehen Rabatt- oder Bonusvereinbarungen.", SubjectCodes.AAK, ContentCodes.ST3);
            desc.AddNote("Der Verkäufer bleibt Eigentümer der Waren bis zu vollständigen Erfüllung der Kaufpreisforderung.", SubjectCodes.AAJ, ContentCodes.EEV);
            desc.AddNote("MUSTERLIEFERANT GMBH BAHNHOFSTRASSE 99 99199 MUSTERHAUSEN Geschäftsführung: Max Mustermann USt-IdNr: DE123456789 Telefon: +49 932 431 0 www.musterlieferant.de HRB Nr. 372876 Amtsgericht Musterstadt GLN 4304171000002 WEEE-Reg-Nr.: DE87654321", 
                         SubjectCodes.REG);
            desc.AddNote("Leergutwert: 46,50");

            desc.SetSeller("MUSTERLIEFERANT GMBH",
                           "99199",
                           "MUSTERHAUSEN",
                           "BAHNHOFSTRASSE 99",
                           CountryCodes.DE,
                           "",
                           new GlobalID(GlobalID.SchemeID_GLN, "4333741000005"));
            desc.SetSellerContact("", emailAddress: "max.mustermann@musterlieferant.de", phoneno: "+49 932 431 500");
            desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);

            desc.SetBuyer("MUSTER-KUNDE GMBH",
                          "40235",
                          "DUESSELDORF",
                          "KUNDENWEG 88",
                          CountryCodes.DE,
                          "009420",
                          new GlobalID(GlobalID.SchemeID_GLN, "4304171000002"));
            desc.SetBuyerOrderReferenceDocument("B123456789", new DateTime(2013, 08, 01));
            
            /**
             * @todo AdditionalReferenceDocument
             */
             

            return desc;
        } // !_generateDescriptor()


        public void read()
        {
            InvoiceDescriptor tempDesc = _generateDescriptor();
            MemoryStream ms = new MemoryStream();
            tempDesc.Save(ms);
            string s = Encoding.ASCII.GetString(ms.ToArray());
            InvoiceDescriptor desc = InvoiceDescriptor.Load(ms);

            bool equals = tempDesc.Equals(desc);
        } // !read()
    }
}
