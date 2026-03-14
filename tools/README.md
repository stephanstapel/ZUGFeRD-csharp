# Tools

## extract_zugferd10_schema.py

Parses the ZUGFeRD 1.0 schema files in `documentation/zugferd10/Schema/` and
produces a structured representation of the XML elements with their data types,
cardinality, BT/BG numbers, and EN16931 descriptions.

The JSON output structure mirrors the C# `Element` class in
`ZUGFeRD/SchemaDocumentation/Element.cs`.

### Parseable source files

| File | Content |
|------|---------|
| `ZUGFeRD1p0.xsd` | Root schema – defines the top-level `CrossIndustryDocument` element |
| `ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_ReusableAggregateBusinessInformationEntity_12.xsd` | Business entity complex types (RABIE) |
| `ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_QualifiedDataType_12.xsd` | Qualified data types |
| `ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_UnqualifiedDataType_15.xsd` | Primitive data types (Amount, ID, Text, …) |
| `ZUGFeRD_1p0.scmt` | Schematron rules – profile-specific cardinality for BASIC / COMFORT / EXTENDED, business rules |
| `documentation/zugferd211en/Schema/EN16931/Schematron/FACTUR-X_EN16931.sch` | ZUGFeRD 2.x EN16931 Schematron – source for BT/BG number cross-reference and English descriptions |

> **Note:** ZUGFeRD 1.0 predates the EN16931 standard and contains no BT/BG numbers.
> The script cross-references element local names from the ZUGFeRD 2.x EN16931
> Schematron to provide best-effort BT/BG annotations.

### Extracted information (JSON / Excel columns)

| Field | Source |
|-------|--------|
| `name` | XSD element name with namespace prefix (e.g. `ram:ID`) |
| `typeName` | XSD type reference with prefix (e.g. `udt:IDType`) |
| `xsdCardinality` | XSD `minOccurs..maxOccurs` (e.g. `0..1`) |
| `ciiCardinality` | Cardinality derived from SCMT assert messages (stricter than XSD where applicable) |
| `xpath` | Absolute namespace-qualified XPath from document root |
| `businessTerm` | EN16931 BT number(s) (e.g. `BT-116`) – cross-referenced from ZUGFeRD 2.x |
| `id` | EN16931 BG number(s) (e.g. `BG-23`) |
| `description` | English description from EN16931 Schematron assertion message |
| `profileSupport` | ZUGFeRD 1.0 profiles that include this element (`BASIC`, `COMFORT`, `EXTENDED`) |
| `businessRule` | SCMT assert message(s) – mandatory count / presence rules |
| `additionalData` | Extra context (e.g. circular reference notes) |

### Output files

| File | Description |
|------|-------------|
| `documentation/zugferd10/schema_structure.json` | Hierarchical JSON tree of all 849 elements |
| `documentation/zugferd10/schema_structure.xlsx` | Flat Excel table with 11 columns, colour-coded by depth |

### Usage

```bash
# Default – reads Schema/ relative to this repo, writes output to documentation/zugferd10/
python3 tools/extract_zugferd10_schema.py

# Custom paths
python3 tools/extract_zugferd10_schema.py \
    --schema-dir /path/to/Schema \
    --output-dir /path/to/output \
    --en16931-sch /path/to/FACTUR-X_EN16931.sch

# JSON only (no Excel)
python3 tools/extract_zugferd10_schema.py --no-excel
```

**Requirement:** `openpyxl` for Excel output (`pip install openpyxl`).
The script runs without it and produces JSON only.

---

## ZUGFeRD/SchemaDocumentation/Element.cs and SchemaReader.cs

C# classes for reading ZUGFeRD schema files programmatically.

### Element class

```csharp
var element = new Element
{
    Name           = "ram:BasisAmount",   // qualified name with prefix
    TypeName       = "udt:AmountType",
    XsdCardinality = "0..1",
    XPath          = "/rsm:CrossIndustryDocument/.../ram:BasisAmount",
    BusinessTerm   = "BT-116",
    Id             = "BG-23",
    Description    = "Each VAT breakdown shall have a VAT category taxable amount.",
    CiiCardinality = "1..1",
    ProfileSupport = new List<string> { "BASIC", "COMFORT", "EXTENDED" },
    BusinessRule   = "Element 'ram:BasisAmount' must occur exactly 1 times.",
};
```

### SchemaReader usage

```csharp
var reader = new SchemaReader(
    xsdDirectory:       @"documentation\zugferd10\Schema",
    scmtFilePath:       @"documentation\zugferd10\Schema\ZUGFeRD_1p0.scmt",
    en16931SchFilePath: @"documentation\zugferd211en\Schema\EN16931\Schematron\FACTUR-X_EN16931.sch");

Element root = reader.ReadSchema();
// root.Name == "rsm:CrossIndustryDocument"
// root.Children contains SpecifiedExchangedDocumentContext, HeaderExchangedDocument, …

// Serialize to JSON with System.Text.Json:
var json = System.Text.Json.JsonSerializer.Serialize(root,
    new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
```

All constructor parameters are paths; pass `null` or omit to skip the
optional enrichment sources (SCMT and EN16931 Schematron).
