# XRechnung Schematron

Schematron Rules for XML validation of the [German CIUS (XRechnung)](https://xeinkauf.de/xrechnung/versionen-und-bundles/) business rules compliant with EN16931:2017.

## Changes and Versioning

Changes to each version are documented in our [CHANGELOG.md](CHANGELOG.md).

In December 2018 we introduced [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

Given a version number MAJOR.MINOR.PATCH, we increment the:

1. MAJOR version when we make incompatible changes:
    * That is we introduce changes to the rules which will **DEFINITELY** not validate previously generated XML which was valid to previous major version anymore
1. MINOR version when we add functionality
    * That is we introduce changes to the rules which widen the scope of valid content (e.g. we add new codelist terms or accept new optional XML elements).
    * **CAUTION**: This might break YOUR validation scenario and ability to accept XML content
1. PATCH version when we make backwards-compatible bug fixes.

## Contact

Existing issues can be found at [our issue tracker](https://projekte.kosit.org/xrechnung/xrechnung-schematron/-/issues).

If you think you have found a bug, please [contact us](https://xeinkauf.de/kontakt/#support).

You can find packaged releases on [our GitLab project](https://projekte.kosit.org/xrechnung/xrechnung-schematron/-/releases).

## Development

Further information on development may be found in our [Developer Documentation](./docs/development.md)
