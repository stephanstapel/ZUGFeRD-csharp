Part of the ZUGFeRD community:
https://github.com/zugferd

# Introduction
The ZUGFeRD library allows to create XML files as required by German electronic invoice initiative ZUGFeRD.
The library is meant to be as simple as possible, however it is not straight forward to use as the resulting XML file contains a complete invoice in XML format. Please take a look at the ZUGFeRD-Test project to find sample creation code. This code creates the same XML file as shipped with the ZUGFeRD information package.

A description of the library can be found here:

http://www.s2-industries.com/wordpress/2013/11/creating-zugferd-descriptors-with-c/

# Installation
Just use nuget or Visual Studio Package Manager and download 'ZUGFeRD-chsarp'.

You can find more information about the nuget package here:

https://www.nuget.org/packages/ZUGFeRD-csharp/

# Creating invoices
Central class for users is class InvoiceDescriptor.
This class does not only allow to read and set all ZUGFeRD attributes and structures but also allows to load and save ZUGFeRD files.

Using InvoiceDescriptor to create invoices is straight forward:

```csharp
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
```

# Using ZUGFeRD 1.x and ZUGFeRD 2.x
In order to load ZUGFeRD files, you call InvoiceDescriptor.Load(), passing a file path like this:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.Load("zugferd.xml");
```

alternatively, you can pass an open stream object:

```csharp
Stream stream = new FileStream("zugferd.xml", FileMode.Open, FileAccess.Read);
InvoiceDescriptor descriptor = InvoiceDescriptor.Load(stream);
```

The library will automatically detect the ZUGFeRD version of the file and parse accordingly. As of today (2020-06-21), parsing ZUGFeRD 2.x is not yet finished.
For saving ZUGFeRD files, use InvoiceDescriptor.Save(). Here, you can also pass a stream object:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.CreateInvoice(......);

// fill attributes and structures

FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
descriptor.Save(stream);
stream.Flush();
stream.Close();            
```

Alternatively, you can pass a file path:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.CreateInvoice(......);

// fill attributes and structures

descriptor.Save("zugferd.xml");          
```

Optionally, you can pass the ZUGFeRD version to use, default currently is version 1.x, e.g.:

```csharp
InvoiceDescriptor descriptor = InvoiceDescriptor.CreateInvoice(......);

// fill attributes and structures


descriptor.Save("zugferd-v1.xml", ZUGFeRDVersion.Version1); // save as version 1.x
descriptor.Save("zugferd-v2.xml", ZUGFeRDVersion.Version2); // save as version 2.0
descriptor.Save("zugferd-v2.xml", ZUGFeRDVersion.Version21); // save as version 2.1
```


# Thanks
* The solution is used in CKS.DMS and supported by CKSolution: 
  http://www.cksolution.de
* https://github.com/cGiesen for starting with ZUGFeRD 2.1 implementation

# Links
You can find more information about ZUGFeRD here:
http://www.ferd-net.de/

