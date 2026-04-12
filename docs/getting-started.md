\# Introduction

The ZUGFeRD library allows to create XML files as required by German electronic invoice initiative ZUGFeRD as well invoices in the successor Factur-X. One special profile of Factur-X is the German XRechnung format.

The library is meant to be as simple as possible, however it is not straight forward to use as the resulting XML file contains a complete invoice in XML format. Please take a look at the ZUGFeRD-Test project to find sample creation code. This code creates the same XML file as shipped with the ZUGFeRD information package.



\# Consulting/ Commercial Support

\*Auf Deutsch:\* Falls Sie kommerzielle Unterstützung bei der Umsetzung von ZUGFeRD/ XRechnung in Ihrem Unternehmen benötigen, schreiben Sie mir gerne an stephan@s2-industries.com



\*In English\* In case you need consulting or commercial support for implementing ZUGFeRD/ XRechnung/ Factur-X in your company, please send an email to stephan@s2-industries.com



\# Relationship between the different standards

Since there are a lot of terms and standards around electronic invoices, I'd like to lay out my understanding:



\- ZUGFeRD was developed by a German initiative as a standard for electronic invoices (https://www.ferd-net.de/).

\- ZUGFeRD 2.1 is identical to the German/French cooperation Factur-X (ZUGFeRD 2.1 = Factur-X 1.0) (https://www.ferd-net.de/en/standards/zugferd/factur-x).

\- The standard Factur-X 1.0 (respectively ZUGFeRD 2.1) is conform with the European norm EN 16931.

\- EN 16931 in turn is based on worldwide UN/CEFACT standard 'Cross Industry Invoice' (CII).

\- XRechnung as another German standard is a subset of EN 16931. It is defined by another party called KoSIT (https://www.xoev.de/). It comes with its own validation rules (https://xeinkauf.de/dokumente/).

\- This means that both Factur-X 1.0 (respectively ZUGFeRD 2.1) and XRechnung are conform with EN 16931. This does not automatically result that those invoices are per se identical.

\- To achieve compatibility, ZUGFeRD 2.1.1 introduced a XRechnung reference profile to guarantee compatibility between the two sister formats.



\# License

Subject to the Apache license http://www.apache.org/licenses/LICENSE-2.0.html



\# Installation

Just use nuget or Visual Studio Package Manager and download 'ZUGFeRD-csharp'.



You can find more information about the nuget package here:



\[!\[NuGet](https://img.shields.io/nuget/v/ZUGFeRD-csharp?color=blue)](https://www.nuget.org/packages/ZUGFeRD-csharp/)



https://www.nuget.org/packages/ZUGFeRD-csharp/



\# Building on your own

Prerequisites:

\* Visual Studio >= 2017

\* .net Framework >= 4.6.1 (for .net Standard 2.0 support)



Open ZUGFeRD/ZUGFeRD.sln solution file. Choose Release or Debug mode and hit 'Build'. That's it.



For running the tests, open ZUGFeRD-Test/ZUGFeRD-Test.sln and run the unit tests. The tests show good cases on how to use the library.



\# Step-by-step guide for creating invoices

Central class for users is class `InvoiceDescriptor`.

This class does not only allow to read and set all ZUGFeRD attributes and structures but also allows to load and save ZUGFeRD files.



However, the standard has become quite large during the recent years. So it is worthwhile to go through the creation process step by step.



\## Creating an invoice



```csharp

InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2013, 6, 5), CurrencyCodes.EUR, "GE2020211-471102");

desc.Name = "WARENRECHNUNG";

desc.ReferenceOrderNo = "AB-312";

desc.AddNote("Rechnung gemäß Bestellung Nr. 2013-471331 vom 01.03.2013.");

desc.AddNote("Es bestehen Rabatt- und Bonusvereinbarungen.", SubjectCodes.AAK);

desc.SetBuyer("Kunden Mitte AG", "69876", "Frankfurt", "Kundenstraße 15", CountryCodes.DE, "88", new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"));

desc.AddBuyerTaxRegistration("DE234567890", TaxRegistrationSchemeID.VA);

desc.SetBuyerContact("Hans Muster");

desc.SetSeller("Lieferant GmbH", "80333", "München", "Lieferantenstraße 20", CountryCodes.DE, "88", new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"));

desc.AddSellerTaxRegistration("201/113/40209", TaxRegistrationSchemeID.FC);

desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);

desc.SetBuyerOrderReferenceDocument("2013-471331", new DateTime(2013, 03, 01));

desc.SetDeliveryNoteReferenceDocument("2013-51111", new DateTime(2013, 6, 3));

desc.ActualDeliveryDate = new DateTime(2013, 6, 3);

desc.SetTotals(202.76m, 5.80m, 14.73m, 193.83m, 21.31m, 215.14m, 50.0m, 165.14m);

desc.AddApplicableTradeTax(129.37m, 7m, TaxTypes.VAT, TaxCategoryCodes.S);

desc.AddApplicableTradeTax(64.46m, 19m, TaxTypes.VAT, TaxCategoryCodes.S);

desc.AddLogisticsServiceCharge(5.80m, "Versandkosten", TaxTypes.VAT, TaxCategoryCodes.S, 7m);

desc.AddTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018", new DateTime(2018, 4, 4));

desc.AddTradePaymentTerms("3% Skonto innerhalb 10 Tagen bis 15.03.2018", new DateTime(2018, 3, 15), PaymentTermsType.Skonto, 30, 3m);

```



Optionally, to support Peppol, an electronic address can be passed:



```csharp

desc.SetSellerElectronicAddress("DE123456789", ElectronicAddressSchemeIdentifiers.GermanyVatNumber);

desc.SetBuyerElectronicAddress("LU987654321", ElectronicAddressSchemeIdentifiers.LuxemburgVatNumber);

```



The fields are only necessary if you want to send the XRechnung via the Peppol network.

A description of the fields can be found in the following documents:



\[https://docs.peppol.eu/edelivery/policies/PEPPOL-EDN-Policy-for-use-of-identifiers-4.3.0-2024-10-03.pdf](https://docs.peppol.eu/edelivery/policies/PEPPOL-EDN-Policy-for-use-of-identifiers-4.3.0-2024-10-03.pdf)





In Luxembourg, it has been mandatory since this year to process all invoices via Peppol:



https://gouvernement.lu/de/dossiers.gouv\_digitalisation%2Bde%2Bdossiers%2B2021%2Bfacturation-electronique.html



In Germany, this has so far only been necessary for invoices in the course of a public contract from the federal government:



https://www.e-rechnung-bund.de/ubertragungskanale/peppol/



\## Adding line items

\### Handling of line ids

The library allows to operate in two modes: you can either let the library generate the line ids automatically or you can alternatively pass distinct line ids. This is helpful if you want to convert existing invoices, e.g. from ERP systems, to ZUGFeRD/ Factur-X.



To let the library create line ids, you can use:



```csharp

InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2013, 6, 5), CurrencyCodes.EUR, "GE2020211-471102");

desc.AddTradeLineItem("Item name", 23.99m, QuantityCodes.H87, "Detail description", ....);

```



This will generate an invoice with trade line item numbered as '1'.



To pass pre-defined line ids, this is the way to go:



```csharp

InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2013, 6, 5), CurrencyCodes.EUR, "GE2020211-471102");

desc.AddTradeLineItem(lineID: "0001", 23.99m, QuantityCodes.H87, "Item name", "Detail description", ....);

desc.AddTradeLineItem(lineID: "0002", 49.99m, QuantityCodes.H87, "Item name", "Detail description", ....);

```

which will generate an invoice with two trade line items, with the first one as number '0001' and the second one as number '0002'.


## Storing the invoice
```csharp
FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
desc.Save(stream, ZUGFeRDVersion.Version23, Profile.XRechnung);
stream.Flush();
stream.Close();    
```



# Working with ZUGFeRD PDF files
The ZUGFeRD-csharp component has a sister component which relies on [PDFSharp](https://github.com/empira/PDFsharp) to read and write PDF files.

Download the package here:

[![NuGet](https://img.shields.io/nuget/v/ZUGFeRD.PDF-csharp?color=blue)](https://www.nuget.org/packages/ZUGFeRD.PDF-csharp/)

The component makes loading the invoice from a pdf as easy as this:

```csharp
InvoiceDescriptor desc = await InvoicePdfProcessor.LoadFromPdfAsync("invoice.pdf");

// alternatively, you can invoke the function in synchronous manner:
InvoiceDescriptor desc = InvoicePdfProcessor.LoadFromPdf("invoice.pdf");
```

Converting a PDF file to a ZUGFeRD PDF/A is almost as simple:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2018, 03, 05), CurrencyCodes.EUR);
// ... fill the descriptor

await InvoicePdfProcessor.SaveToPdfAsync("zugferd-invoice.pdf", ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, "input-invoice.pdf", descriptor);

// alternatively, you can invoke the function in synchronous manner:
InvoicePdfProcessor.SaveToPdf("zugferd-invoice.pdf", ZUGFeRDVersion.Version23, Profile.Comfort, ZUGFeRDFormats.CII, "input-invoice.pdf", descriptor);
```

