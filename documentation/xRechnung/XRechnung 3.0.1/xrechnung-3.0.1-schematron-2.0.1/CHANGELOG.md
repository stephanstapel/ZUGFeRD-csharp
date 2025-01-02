# Changelog

All notable changes to the Schematron Rules and this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## v2.0.1 on 2023-09-22

This release is compatible with XRechnung 3.0.x

### Fixed

* Bug in PEPPOL-EN16931-R053 in CII
* CII tests for PEPPOL-EN16931-R053

## v2.0.0 on 2023-07-31

This release is compatible with XRechnung 3.0.x

### Added

* PEPPOL-EN16931-R001 Business process MUST be provided
* PEPPOL-EN16931-R005 VAT accounting currency code MUST be different from invoice currency code when provided
* PEPPOL-EN16931-R008 Document MUST not contain empty elements (disabled in CII for element ram:ApplicableHeaderTradeDelivery)
* PEPPOL-EN16931-R010 Buyer electronic address MUST be provided
* PEPPOL-EN16931-R020 Seller electronic address MUST be provided
* PEPPOL-EN16931-R040 Allowance/charge amount must equal base amount * percentage/100 if base amount and percentage exists
* PEPPOL-EN16931-R041 Allowance/charge base amount MUST be provided when allowance/charge percentage is provided
* PEPPOL-EN16931-R042 Allowance/charge percentage MUST be provided when allowance/charge base amount is provided
* PEPPOL-EN16931-R043 Allowance/charge ChargeIndicator value MUST equal 'true' or 'false'
* PEPPOL-EN16931-R044 Charge on price level is NOT allowed. Only value 'false' allowed
* PEPPOL-EN16931-R046 Item net price MUST equal (Gross price - Allowance amount) when gross price is provided
* PEPPOL-EN16931-R053 Only one tax total with tax subtotals MUST be provided
* PEPPOL-EN16931-R054 Only one tax total without tax subtotals MUST be provided when tax currency code is provided
* PEPPOL-EN16931-R055 Invoice total VAT amount and Invoice total VAT amount in accounting currency MUST have the same operational sign
* PEPPOL-EN16931-R061 Mandate reference MUST be provided for direct debit (Replaces BR-DE-29)
* PEPPOL-EN16931-R101 Element Document reference can only be used for Invoice line object
* PEPPOL-EN16931-R110 Start date of line period MUST be within invoice period
* PEPPOL-EN16931-R111 End date of line period MUST be within invoice period
* PEPPOL-EN16931-R120 Invoice line net amount MUST equal (Invoiced quantity * (Item net price/item price base quantity) + Sum of invoice line charge amount - sum of invoice line allowance amount (disabled in CII due to syntax binding ambiguities)
* PEPPOL-EN16931-R121 Base quantity MUST be a positive number above zero
* PEPPOL-EN16931-R130 Unit code of price base quantity MUST be same as invoiced quantity
* Unless otherwise indicated, all PEPPOL EN16931-XXX rules are switched to severity level "warning" in CII during introduction phase

### Changed

* ISO 6523 ICD and CEF EAS codelist values in extension validation rules that override CEN rules contain newest codelist values.
* Schematron Rules
    * temporary BR-TMP-1 to add warning on prohibited multiple BG-30 LINE VAT INFORMATION within BG-25 INVOICE LINE (see https://github.com/ConnectingEurope/eInvoicing-EN16931/issues/349).
    * Ant target `retrieve-peppol-bis-billing-rules`
* Ant target `merge-peppol-rules-into-xr-rules` 
* Add xsl script for merging peppol bis billing rules into xrechnung rules
* BR-DE-16 adapted to CEN schematron: 'VAT' no longer permitted as BT-31 value
* "VERZUG" removed from BR-DE-18 SKONTO Regex
* removed reference to "Verzugszinsen" from Schematron and Test files
* removed BR-DE-29 (is replaced by PEPPOL-EN16931-R061)
* removed tests for BR-DE-29
* removed UBL CreditNote tests for BR-DEX-XXX rules

## v1.8.2 on 2023-05-12

This release is compatible with XRechnung 2.3.x

### Added

* Schematron Rules
    * BR-DEX-13 to validate maximum number of two allowed fraction digits in BT-DEX-002 ("Third party payment amount").
    * BR-DEX-14 to validate currency of BT-DEX-002 is the same as "Invoice Currency Code" (BT-5).
    * BR-DEX-15 to detect possible use of unsupported Sub Invoice Lines in CII.
* Tests 
    * for BR-DE-16 with VAT code "O"
    * `cii-br-dex-15-test-on-sub-invoice-lines.xml`

### Changed

* Schematron Rules
  * merged ubl invoice and ubl creditnote rules
* References to Schematron files in mutator tests
* BR-DE-19 and BR-DE-20 IBAN validation is now more robust

### Fixed

* BR-DE-16 description now includes restriction to all tax codes except "O" (as implemented with XR version 2.0.1 / Schematron 1.5.0).
* Incorrect ChargeIndicator in BR-DE-16 tests with BT-95.
* Incorrect ChargeIndicator in BR-DE-16 tests with BT-102.


## v1.8.1 on 2023-02-17

This release is compatible with XRechnung 2.3.x

### Fixed

* Validation of BR-DEX-10, BR-DEX-11 and BR-DEX-12 now restricted to extension 

## v1.8.0 on 2023-01-31

This release is compatible with XRechnung 2.3.x

### Added
* Schematron Rules
  * Missing Rules BR-DE-29, BR-DE-30, BR-DE-31 for mandatory elements of BG-19.
  * Rules BR-DEX-09, BR-DEX-10, BR-DEX-11, BR-DEX-12 for third party payment

### Changed

* ISO 6523 ICD and CEF EAS codelist values in extension validation rules that override CEN rules contain newest codelist values

## v1.7.3 on 2022-11-15

This release is compatible with XRechnung 2.2.0

### Changed

* Schematron Rules
  * BR-DE-21 in CII to allow for Extension specification identifier 

### Fixed
* Removed superfluous duplicate unit test from ubl-inv-contact-tests.xml
* Changed some IDs to German IDs

## v1.7.2 on 2022-05-31

This version is compatible with XRechnung 2.2.0

### Added
* Schematron Rules
  * BR-DE-26 to UBL CreditNote to allow for "Corrected CreditNotes"

### Changed

* Schematron Rules
  * BR-DE-20, that testing is more robust, because type conversion is now explicitly to decimal and thus more compatible with xpath 1.0

## v1.7.1 on 2022-02-07

This version is compatible with XRechnung 2.2.0

### Fixed

* Schematron Rules
  * BR-DE-28, that - (minus), \_ (underscore), and other special characters are allowed in emails 

## v1.7.0 on 2022-01-31

This version is compatible with XRechnung 2.2.0

### Added

* Schematron Rules
  * BR-DE-27, that a telephone number must have at least three digits
  * BR-DE-28, that an email address must have exactly one @ sign, does not start or end with a dot, the @ sign must not be flanked by a whitespace or a dot and must be preceded and followed by at least two characters.
  * BR-DE-18-a, CII cardinality for BT-20 

* Schematron Rules for XRechnung Extension to include DIGA Codes (XR01, XR02, XR03)
  * BR-DEX-04 replaces CEN rule BR-CL-10 (ISO 6523 ICD Codelist)
  * BR-DEX-05 replaces CEN rule BR-CL-11 (ISO 6523 ICD Codelist)
  * BR-DEX-06 replaces CEN rule BR-CL-21 (ISO 6523 ICD Codelist)
  * BR-DEX-07 replaces CEN rule BR-CL-25 (EAS Codelist)
  * BR-DEX-08 replaces CEN rule BR-CL-26 (ISO 6523 ICD Codelist)

* numerous tests for BR-DEX-04 to BR-DEX-08 rules 

### Changed

* BR-DE-18 more robust checking of Skonto rules

### Fixed

* Corrected UBL invoice test for IBAN, checking for BR-DE-19 instead of BR-DE-21

## v1.6.1 on 2021-11-15

This version is compatible with XRechnung 2.1.1

### Fixed

* Schematron Rules
  * BR-DE-18 fixed bug, that no newline was not allowed at last not skonto note
* Tests
  * fixed wrong CII syntax in cii-br-de-23-test-bg-17-with-bg-18.xml

## v1.6.0 on 2021-07-31

This version is compatible with XRechnung 2.1.1

### Added

* Schematron Rules
  * BR-DE-23 replaces BR-DE-13
  * BR-DE-24 replaces BR-DE-13
  * BR-DE-25 replaces BR-DE-13
  * BR-DE-26 (for UBL Invoice and CII only)

### Changed

* Schematron Rules
  * BR-DE-13 removed
* Now only one Schematron file per Syntax
* All test source instances valid to EN16931 and XRechnung 2.1 

## v1.5.0 on 2020-12-31

This version is compatible with XRechnung 2.0.1

### Added

* Schematron Rules
  * BR-DEX-03 to check existence of BG-DEX-06 in a BG-DEX-01

### Changed

* This version is compatible with XRechnung 2.0.1
* Bump version to 1.5.0 for next release
* Schematron Rules
  * BR-DE-16 is now only relevant, if bt-95, bt-102 or bt-151 exist

### Fixed

* Schematron Rules
  * BR-DEX-02 rewrote rule to not give false negative
  * BR-DE-18 now checks last newline and allows negative Basisbetrag
  * BR-DE-19 flag is set to warning for CII

## v1.4.0 on 2020-07-31

### Added

* Schematron Rules
  * BR-DE-22 to check for unique file names
  * BR-DEX-01 to allow mime type 'application/xml' in XRechnung Extension
  * BR-DEX-02 on checking the sum of prices for UBL sub invoice lines in XRechnung extension

### Changed

* Schematron Rules
  * BR-DE-19 and BR-DE-20 refactoring IBAN rules
  * BR-DE-21 to check specification identifier for extension

### Fixed

* Schematron Rules
  * BR-DE-19 and BR-DE-20 fixed CII IBAN rules

## v1.3.0 on 2019-12-30

### Added

* Schematron Rules
  * BR-DE-19 und BR-DE-20 on IBAN test
  * BR-DE-21 on correct CustomizationID independent of validation scenarios
* Unit tests on Contact rules and on IBAN using xmute instructions  

### Changed

* Removed superflous files 
* Build.xml
  * Version string in Schematron files is configured by build script
  * Overwrite of Schematron compilation files is configurable

## v1.2.1 on 2019-06-24

### Added

- License.md
- Apache 2.0 License to this work
- CEN License statement
- OpenPeppol BIS 3.0 material incl.
  - [specific test instances](test/instances/bis)
  - [BIS Version of Schematron rules](src/bis)

### Changed

- README.md includes List of XRechnung business rules covered by schematron rules


## v1.2.0 on 2018-12-19

This release is compatible to XRechnung 1.2.0.

This version is only by chance the same version as XRechnung!

Because of #19 and #12 it might break your validation and business workflow. Please evaluate impact!

### Added

- Semantic Versioning (see [README](README.md)) #9
- Codelist term 389 (Self-billed invoice) for BT-3 #19

### Changed

- Made descriptions of rules `BR-DE-10` (#18), `BR-DE-11` (#18), `BR-DE-13` (#17), `BR-DE-16` (#16), and `BR-DE-18` (#22)  more precise

### Fixed

- Workaround to inconsistency in CEN Norm and Rules concerning syntax binding as described in [CEN issue #57](https://github.com/CenPC434/validation/issues/57)
- For now we allow for both 'VA' and 'VAT' (see #12)
