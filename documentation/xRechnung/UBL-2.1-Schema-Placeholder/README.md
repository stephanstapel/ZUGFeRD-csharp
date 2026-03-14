# OASIS UBL 2.1 Schema Files

The OASIS UBL 2.1 XSD schema files are **not included** in this repository due to licensing considerations. You must download them separately from OASIS.

## Download

1. Go to: http://docs.oasis-open.org/ubl/os-UBL-2.1/UBL-2.1.zip
2. Extract the ZIP archive
3. The XSD files are located in the `xsd/` or `xsdrt/` subfolder:
   - `xsd/maindoc/` — canonical XSD files (with comments/annotations)
   - `xsdrt/maindoc/` — runtime XSD files (stripped, faster to load)

## Required files

For the `ZUGFeRD.Spec` tool with `--syntax ubl`:

| File | Purpose |
|------|---------|
| `UBL-Invoice-2.1.xsd` | Root schema for UBL Invoice |
| `UBL-CreditNote-2.1.xsd` | Root schema for UBL CreditNote |
| `UBL-CommonAggregateComponents-2.1.xsd` | `cac:` element type definitions |
| `UBL-CommonBasicComponents-2.1.xsd` | `cbc:` leaf element definitions |
| `UBL-QualifiedDataTypes-2.1.xsd` | Qualified data types |
| `UBL-UnqualifiedDataTypes-2.1.xsd` | Unqualified data types |

## Usage

Once you have downloaded and extracted the UBL 2.1 XSDs, run the tool:

```bash
# Point --schema to the maindoc folder containing UBL-Invoice-2.1.xsd
dotnet run --project ZUGFeRD.Spec.Test -- \
  -x "documentation/xRechnung/XRechnung 3.0.1" \
  -s "<path-to-ubl-2.1>/xsd/maindoc" \
  --syntax ubl \
  -o xrechnung-3.0.1-ubl-spec.json \
  --verbose
```

## License

The OASIS UBL 2.1 specification is licensed under the
[OASIS IPR Policy](https://www.oasis-open.org/policies-guidelines/ipr/).
See the OASIS website for details.
