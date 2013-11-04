using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s2industries.ZUGFeRD;

namespace ZUGFeRD_Test
{
    class Application
    {
        internal void run()
        {
            InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2013, 6, 5), CurrencyCodes.EUR, "GE2020211-471102");
            desc.Profile = Profile.Comfort;
            desc.ReferenceOrderNo = "AB-312";
            desc.AddNote("Rechnung gemäß Bestellung Nr. 2013-471331 vom 01.03.2013.");
            desc.AddNote("Es bestehen Rabatt- und Bonusvereinbarungen.", "AAK");
            desc.SetBuyer("Kunden Mitte AG", "69876", "Frankfurt", "Kundenstraße", "15", "DE", "88", "4000001987658");
            desc.AddBuyerTaxRegistration("DE234567890", "VA");
            desc.SetBuyerContact("Hans Muster");
            desc.SetSeller("Lieferant GmbH", "80333", "München", "Lieferantenstraße", "20", "DE", "88", "4000001123452");
            desc.AddSellerTaxRegistration("201/113/40209", "FC");
            desc.AddSellerTaxRegistration("DE123456789", "VA");
            desc.SetBuyerOrderReferenceDocument("2013-471331", new DateTime(2013, 03, 01));
            desc.SetDeliveryNoteReferenceDocument("2013-51111", new DateTime(2013, 6, 3));
            desc.ActualDeliveryDate = new DateTime(2013, 6, 3);
            desc.SetTotals(202.76m, 5.80m, 14.73m, 193.83m, 21.31m, 215.14m, 50.0m, 165.14m);
            desc.AddApplicableTradeTax(9.06m, 129.37m, 7m, "VAT", "S");
            desc.AddApplicableTradeTax(12.25m, 64.46m, 19m, "VAT", "S");
            desc.SetLogisticsServiceCharge(5.80m, "Versandkosten", "VAT", "S", 7m);
            desc.setTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.07.2013, 3% Skonto innerhalb 10 Tagen bis 15.06.2013", new DateTime(2013, 07, 04));

            desc.Save("output.xml");
        }
    }
}
