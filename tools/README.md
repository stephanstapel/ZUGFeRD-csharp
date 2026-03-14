# Tools

## extract_zugferd10_schema.py

Parses the ZUGFeRD 1.0 schema files in `documentation/zugferd10/Schema/` and
produces a structured representation of the XML elements with their data types,
cardinality, and Schematron validation rules.

### Parseable source files

| File | Content |
|------|---------|
| `ZUGFeRD1p0.xsd` | Root schema – defines the top-level `CrossIndustryDocument` element |
| `ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_ReusableAggregateBusinessInformationEntity_12.xsd` | Business entity complex types (RABIE) |
| `ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_QualifiedDataType_12.xsd` | Qualified data types |
| `ZUGFeRD1p0_urn_un_unece_uncefact_data_standard_UnqualifiedDataType_15.xsd` | Primitive data types (Amount, ID, Text, …) |
| `ZUGFeRD_1p0.scmt` | Schematron rules – profile-specific cardinality for BASIC / COMFORT / EXTENDED |

### Extracted information

For every XML element in the schema the script extracts:

* **XPath** – fully namespace-qualified path from the document root
* **Element name** – local XML element name
* **Data type** – qualified XSD type reference (e.g. `udt:AmountType`)
* **minOccurs / maxOccurs** – XSD cardinality
* **Schematron assert messages** – rules that *require* an element or attribute
* **Schematron report messages** – rules that mark an element as *not used* in a given profile/context

### Output files

| File | Description |
|------|-------------|
| `documentation/zugferd10/schema_structure.json` | Hierarchical JSON tree of all elements |
| `documentation/zugferd10/schema_structure.xlsx` | Flat Excel table of all 849 elements, colour-coded by depth |

### Usage

```bash
# Default – reads Schema/ relative to this repo, writes output to documentation/zugferd10/
python3 tools/extract_zugferd10_schema.py

# Custom paths
python3 tools/extract_zugferd10_schema.py \
    --schema-dir /path/to/Schema \
    --output-dir /path/to/output

# JSON only (no Excel)
python3 tools/extract_zugferd10_schema.py --no-excel
```

**Requirement:** `openpyxl` for Excel output (`pip install openpyxl`).
The script runs without it and produces JSON only.
