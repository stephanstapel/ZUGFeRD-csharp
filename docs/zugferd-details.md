# Working with product characteristics

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



# Document references

The library allows to add special references to an invoice which are pretty rare but nevertheless supported:



```csharp

// you can optionally add a reference to a procuring project:

desc.SpecifiedProcuringProject = new SpecifiedProcuringProject {Name = "Projekt AB-312", ID = "AB-312"};



// you can optionally reference a contract:

desc.ContractReferencedDocument = new ContractReferencedDocument {ID = "AB-312-1", IssueDateTime = new DateTime(2013,1,1)};



// you can optionally reference an order confirmation (seller's order) at invoice level

desc.SellerOrderReferencedDocument = new SellerOrderReferencedDocument {ID = "SO-2013-471331", IssueDateTime = new DateTime(2013, 3, 1)};

```



Order confirmations can also be referenced at the trade line item level:



```csharp
// reference an order confirmation at line item level:
TradeLineItem item = desc.AddTradeLineItem(...);
item.SellerOrderReferencedDocument = new SellerOrderReferencedDocument {ID = "SO-2013-471331-001", IssueDateTime = new DateTime(2013, 3, 1)};
```



# Invoice Line Status

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

## Cleaning
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