Core: [![NuGet](https://img.shields.io/nuget/v/ZUGFeRD-csharp?color=blue)](https://www.nuget.org/packages/ZUGFeRD-csharp/)

PDF: [![NuGet](https://img.shields.io/nuget/v/ZUGFeRD.PDF-csharp?color=blue)](https://www.nuget.org/packages/ZUGFeRD.PDF-csharp/)

Part of the ZUGFeRD community:
https://github.com/zugferd

# Sponsoring
Implementing and maintaining this library is a lot of hard work. I'm doing this in my spare time, there is no company behind developing ZUGFeRD-csharp. Support me in this work and help making this library better:

[:heart: Sponsor me on GitHub](https://github.com/sponsors/stephanstapel)

In particular, I am searching for sponsors for:

* Validation using the standard XSL
* Invoice visualization

# Introduction
The ZUGFeRD library allows to create XML files as required by German electronic invoice initiative ZUGFeRD as well invoices in the successor Factur-X. One special profile of Factur-X is the German XRechnung format.
The library is meant to be as simple as possible, however it is not straight forward to use as the resulting XML file contains a complete invoice in XML format. Please take a look at the ZUGFeRD-Test project to find sample creation code. This code creates the same XML file as shipped with the ZUGFeRD information package.

# Consulting/ Commercial Support
*Auf Deutsch:* Falls Sie kommerzielle Unterstützung bei der Umsetzung von ZUGFeRD/ XRechnung in Ihrem Unternehmen benötigen, schreiben Sie mir gerne an stephan@s2-industries.com

*In English* In case you need consulting or commercial support for implementing ZUGFeRD/ XRechnung/ Factur-X in your company, please send an email to stephan@s2-industries.com

# Relationship between the different standards
Since there are a lot of terms and standards around electronic invoices, I'd like to lay out my understanding:

- ZUGFeRD was developed by a German initiative as a standard for electronic invoices (https://www.ferd-net.de/).
- ZUGFeRD 2.1 is identical to the German/French cooperation Factur-X (ZUGFeRD 2.1 = Factur-X 1.0) (https://www.ferd-net.de/en/standards/zugferd/factur-x).
- The standard Factur-X 1.0 (respectively ZUGFeRD 2.1) is conform with the European norm EN 16931.
- EN 16931 in turn is based on worldwide UN/CEFACT standard 'Cross Industry Invoice' (CII).
- XRechnung as another German standard is a subset of EN 16931. It is defined by another party called KoSIT (https://www.xoev.de/). It comes with its own validation rules (https://xeinkauf.de/dokumente/).
- This means that both Factur-X 1.0 (respectively ZUGFeRD 2.1) and XRechnung are conform with EN 16931. This does not automatically result that those invoices are per se identical.
- To achieve compatibility, ZUGFeRD 2.1.1 introduced a XRechnung reference profile to guarantee compatibility between the two sister formats.

# License
Subject to the Apache license http://www.apache.org/licenses/LICENSE-2.0.html

# Installation
Just use nuget or Visual Studio Package Manager and download 'ZUGFeRD-csharp'.

You can find more information about the nuget package here:

[![NuGet](https://img.shields.io/nuget/v/ZUGFeRD-csharp?color=blue)](https://www.nuget.org/packages/ZUGFeRD-csharp/)

https://www.nuget.org/packages/ZUGFeRD-csharp/

# Building on your own
Prerequisites:
* Visual Studio >= 2017
* .net Framework >= 4.6.1 (for .net Standard 2.0 support)

Open ZUGFeRD/ZUGFeRD.sln solution file. Choose Release or Debug mode and hit 'Build'. That's it.

For running the tests, open ZUGFeRD-Test/ZUGFeRD-Test.sln and run the unit tests. The tests show good cases on how to use the library.

# Step-by-step guide for creating invoices
Central class for users is class `InvoiceDescriptor`.
This class does not only allow to read and set all ZUGFeRD attributes and structures but also allows to load and save ZUGFeRD files.

However, the standard has become quite large during the recent years. So it is worthwhile to go through the creation process step by step.

## Creating an invoice

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

[https://docs.peppol.eu/edelivery/policies/PEPPOL-EDN-Policy-for-use-of-identifiers-4.3.0-2024-10-03.pdf](https://docs.peppol.eu/edelivery/policies/PEPPOL-EDN-Policy-for-use-of-identifiers-4.3.0-2024-10-03.pdf)


In Luxembourg, it has been mandatory since this year to process all invoices via Peppol:

https://gouvernement.lu/de/dossiers.gouv_digitalisation%2Bde%2Bdossiers%2B2021%2Bfacturation-electronique.html

In Germany, this has so far only been necessary for invoices in the course of a public contract from the federal government:

https://www.e-rechnung-bund.de/ubertragungskanale/peppol/

## Adding line items
### Handling of line ids
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

### Working with product characteristics
Product characteristics are used to add information for the specified trade product in the 'ApplicableProductCharacteristic' section.
One trade product can have one or more product characteristics, which can contain description, value, typecode and value measurand elements.

 ```csharp
// you can optionally add product characteristics:
 desc.TradeLineItems.Add(new TradeLineItem("0003")
{
    ApplicableProductCharacteristics = new List<ApplicableProductCharacteristic>
    {
        new ApplicableProductCharacteristic()
        {
            Description = "Description",
            Value = "Value"
        }
    }
});
```

## Document references
The library allows to add special references to an invoice which are pretty rare but nevertheless supported:

```csharp
// you can optionally add a reference to a procuring project:
desc.SpecifiedProcuringProject = new SpecifiedProcuringProject {Name = "Projekt AB-312", ID = "AB-312"};

// you can optionally reference a contract:
desc.ContractReferencedDocument = new ContractReferencedDocument {ID = "AB-312-1", IssueDateTime = new DateTime(2013,1,1)};
```

## Invoice Line Status
The library supports setting a status code and a reason for each line item. This feature helps clarifying whether a line item contributes to the invoice total or is for information only. It can be useful for marking items as informational, subtotal lines, or fully processed items, adding context to each invoiced item.
```csharp
// Example: Adding a trade line item with a specific status and reason
TradeLineItem tradeLineItem3 = desc.AddTradeLineItem(
    name: "Abschlagsrechnung vom 01.01.2024",
    netUnitPrice: 500,
    unitCode: QuantityCodes.H87,
    billedQuantity: -1m,
    categoryCode: TaxCategoryCodes.S,
    taxPercent: 19.0m,
    taxType: TaxTypes.VAT
);

// Set a line status code and reason code to indicate that this line is for documentation only
tradeLineItem3.SetLineStatus(LineStatusCodes.DocumentationClaim, LineStatusReasonCodes.INFORMATION);
```

## Storing the invoice
```csharp
FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
desc.Save(stream, ZUGFeRDVersion.Version23, Profile.XRechnung);
stream.Flush();
stream.Close();    
```

# Support for ZUGFeRD 1.x, ZUGFeRD 2.x
In order to load ZUGFeRD files, you call `InvoiceDescriptor.Load()`, passing a file path like this:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.Load("zugferd.xml");
```

alternatively, you can pass an open stream object:

```csharp
Stream stream = new FileStream("zugferd.xml", FileMode.Open, FileAccess.Read);
InvoiceDescriptor descriptor = InvoiceDescriptor.Load(stream);
```

The library will automatically detect the ZUGFeRD version of the file and parse accordingly. It will automatically be chosen which XRechnung version to use depending on the current date.
The lifecycle of the stream is not influenced by the ZUGFeRD library, i.e. the library expects an open stream and will not close if after reading from it.

For saving ZUGFeRD files, use `InvoiceDescriptor.Save()`. Here, you can also pass a stream object:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.CreateInvoice(......);

// fill attributes and structures

FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
descriptor.Save(stream, ZUGFeRDVersion.Version1, Profile.Basic);
stream.Flush();
stream.Close();            
```

As you see, the library does not influence the lifecycle of the stream, i.e. it is not automatically closed by the library. Just as opening the stream, flushing and closing is the duty of the calling function.

Alternatively, you can pass a file path:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.CreateInvoice(......);

// fill attributes and structures

descriptor.Save("zugferd.xml", ZUGFeRDVersion.Version1, Profile.Basic);          
```

Optionally, you can pass the ZUGFeRD version to use. Currently, the default version is 1.x, e.g.:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.CreateInvoice(......);

// fill attributes and structures


descriptor.Save("zugferd-v1.xml", ZUGFeRDVersion.Version1, Profile.Basic); // save as version 1.x
descriptor.Save("zugferd-v20.xml", ZUGFeRDVersion.Version20, Profile.Basic); // save as version 2.0
descriptor.Save("zugferd-v23.xml", ZUGFeRDVersion.Version23, Profile.Basic); // save as version 2.3
```

For reading and writing XRechnung invoices, please see below.

# Support for XRechnung
In general, creating XRechnung files is straight forward and just like creating any other ZUGFeRD version and profile:

```csharp
descriptor.Save("xrechnung.xml", ZUGFeRDVersion.Version23, Profile.XRechnung);
```

This will save the invoice as XRechnung 3.0.1 as valid from 2024/02/01.

Make sure to also add a business process which is required starting with XRechnung 3.0.1:

```csharp
desc.BusinessProcess = "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0";
```

Furthermore, XRechnung comes with some special features. One of these features is the ability to embed binary files as attachments to the `xrechnung.xml` document:

```csharp
InvoiceDescriptor desc = _createInvoice();
byte[] data = System.IO.File.ReadAllBytes("my-calculation.xlsx");
desc.AddAdditionalReferencedDocument(
    id: "calculation-sheet",
    typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,
    name: "Calculation as the basis for the invoice",
    attachmentBinaryObject: data,
    filename: "my-calculation.xlsx");

desc.Save("xrechnung.xml", ZUGFeRDVersion.Version23, Profile.XRechnung);            
```

The resulting `xrechnung.xml` file will then contain the calculation file content. As this is not standardized, the decision was to encode the attachments in base64.
Please note that there are only few mime-types supported by the XRechnung standard. The supported mime-types are defined in BG-24 and BT-125. At the time of writing this tutorial, those are also listed in the discussion you find over here: https://projekte.kosit.org/xrechnung/xrechnung/-/issues/59

- application/pdf
- image/png
- image/jpeg
- text/csv
- application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
- application/vnd.oasis.opendocument.spreadsheet
- application/xml

# Support for E-Reporting
For french companies, a dedicated profile exists called E-Reporting. This profile is implemented on top of XRechnung/ Factur-X. It is used when transactions are done to customers who don't make use of VAT. One example are private customers (in B2C scenarios). The other example is when selling to entities beyond France.
More information can be found here: https://www.impots.gouv.fr/e-reporting-la-transmission-de-donnees-de-transaction-ladministration (French)
And here: https://www.roedl.de/themen/frankreich-e-invoice-reporting-umsatzsteuer-digital (german)

Thanks to [@Athilla](https://github.com/Athilla), this profile is also supported by ZUGFeRD-csharp.

The information that is written into the invoice descriptor is identical to standard XRechnung/ Factur-X invoices, you just need to adjust the profile:

```csharp
descriptor.Save("factur-x.xml", ZUGFeRDVersion.Version23, Profile.EReporting);
```

This information needs to be sent to the tax authorities. Different due dates apply for implementation for different sizes of companies.

# Support for profiles
The library contains support for all profiles that are supported by the ZUGFeRD formats:

| Profile         	 | Version1 	 | Version20	 | Version23 	 |
|-------------------|------------|------------|-------------|
| MINIMUM         	 | 	          | X        	 | X         	 |
| BASIC WL        	 | 	          | X        	 | X         	 |
| BASIC           	 | X        	 | X        	 | X         	 |
| COMFORT/EN16391 	 | X        	 | X        	 | X         	 |
| XRECHNUNG       	 | 	          | 	          | X         	 |
| EXTENDED        	 | X        	 | X        	 | X         	 |

Please note that version 1 implementation of the library is not strict, i.e. it will output all information available into the invoice xml, regardless of the profiles that is used. Reading various files with different profiles will generate the correct output.

If you want to write the invoice xml with a certain ZUGFeRD version and a certain profile, make sure to use the parameters of the Save method:

```csharp
descriptor.Save("zugferd-v1.xml", ZUGFeRDVersion.Version1, Profile.Basic); // save as version 1.x, profile Basic
descriptor.Save("zugferd-v20.xml", ZUGFeRDVersion.Version20, Profile.Basic); // save as version 2.0, profile Basic
descriptor.Save("zugferd-v23.xml", ZUGFeRDVersion.Version23, Profile.Basic); // save as version 2.3, profile Basic
descriptor.Save("zugferd-v23-xrechnung.xml", ZUGFeRDVersion.Version23, Profile.XRechnung); // save as version 2.3, profile XRechnung
```

# Configuration
In order to make the xml more readable for humans, you can optionally add comments to the xml file (currently available only in German).
If you like to enable these comments, you can use the options builder and pass the resulting options to the Save() command:

```csharp
InvoiceFormatOptions options = InvoiceOptionsBuilder
.Create()
.EnableXmlComments(true)
.Build();
desc.Save(ms, version, profile, format, options);
```

This will work with all ZUGFeRD versions and also with XRechnung UBL files.

Another new feature in version 18.0.0 of the library is cleaning of invalid xml characters or throwing an exception when invalid xml characters are found.
Can you enable automatic cleaning of xml files using:

```csharp
InvoiceFormatOptions options = InvoiceOptionsBuilder
.Create()
.AutomaticallyCleanInvalidCharacters()
.Build();
desc.Save(ms, version, profile, format, options);
```

Otherweise, an exception will be thrown once an invalid character is written to the xml file.

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


# Thanks

* First of all I'd like to thank the numerous contributors working on new features and removing bugs
* The solution is used in CKS.DMS and supported by CKSolution: 
  https://www.cksolution.de
* ZUGFeRD 2.1 implementation was done by https://netco-solution.de and used in netCo.Butler

# Links

You can find more information about ZUGFeRD here:
https://www.ferd-net.de/
