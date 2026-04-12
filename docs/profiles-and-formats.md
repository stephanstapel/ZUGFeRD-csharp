



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

&#x20;   id: "calculation-sheet",

&#x20;   typeCode: AdditionalReferencedDocumentTypeCode.ReferenceDocument,

&#x20;   name: "Calculation as the basis for the invoice",

&#x20;   attachmentBinaryObject: data,

&#x20;   filename: "my-calculation.xlsx");



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

