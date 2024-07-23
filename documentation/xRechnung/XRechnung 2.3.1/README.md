# XRechnung Bundle

Ein integriertes Bundle mit dem Spezifikationsdokument für den Standard [XRechnung](https://xeinkauf.de/xrechnung/) und unterstützende Komponenten.

## Überblick Bestandteile

| Name                               | Version im Bundle | Kommentar |
|------------------------------------|-------------------|-----------|
| XRechnung Specification            | 2.3.1       | -         |
| XRechnung Syntax-Binding           | zu 2.3.1       | keine Änderungen, kompatibel zu XRechnung 2.3.x |
| Validator                          | 1.5.0           |           |
| XRechnung Validator Konfiguration  | 2023-05-12      |           |
| XRechnung Schematron               | 1.8.2           |           |
| XRechnung Visualization            | 2023-05-12           |           |
| XRechnung Testsuite                | 2023-05-12          |           |

## Änderungen zum letzten Release

Es ist zu beachten, dass dieses Release alle vorangegangenen Änderungen der XRechnung Bundle Version 2.3.1 zur Version 2.3.0 beinhaltet.

### Spezifikation

* Keine Änderungen gegenüber der am 03.02.2023 veröffentlichen Version XRechnung 2.3.1

### Syntax-Binding

* Keine Änderungen gegenüber der am 02.02.2023 veröffentlichten Version

### Validator

* keine Änderungen

Details siehe: https://github.com/itplr-kosit/validator/releases/tag/v1.5.0

### Validator Konfiguration XRechnung

* jetzt mit Testsuite v2023-05-12 und Schematron v1.8.2
* neue CEN Unit Tests wurden hinzugefügt, vorhandene Tests wurden überarbeitet

Details siehe: https://github.com/itplr-kosit/validator-configuration-xrechnung/releases/tag/release-2023-05-12

### XRechnung Schematron Regeln

* Zusammenführung von UBL Invoice und CreditNote Code
* neue Schematron Regeln BR-DEX-13, BR-DEX-14 und BR-DEX-15
* neue Tests wurden hinzugefügt, vorhandene Tests wurden überarbeitet

Hinweis: Die Prüfregeln BR-DEX-13, BR-DEX-14 und BR-DEX-15 können zu Fehlermeldungen führen, falls bereits bestehende normative Vorgaben nicht beachtet werden. BR-DEX-13, BR-DEX-14 und BR-DEX-15 werden mit dem Sommerrelease der Spezifikation als Information in die Tabelle 17.2 aufgenommen.

Details siehe: https://github.com/itplr-kosit/xrechnung-schematron/releases/tag/release-1.8.2

### XRechnung Visualisierung

* diverse Verbesserungen in den HTML und PDF Darstellungen

Details siehe: https://github.com/itplr-kosit/xrechnung-visualization/releases/tag/v2023-05-12

### XRechnung Testsuite

* XML Mutate wurde entfernt
* Korrekturen an Test `04.05a-INVOICE_uncefact.xml`

Details siehe: https://github.com/itplr-kosit/xrechnung-testsuite/releases/tag/release-2023-05-12

## Bundle Bestandteile Details

### Validator (Prüftool)

Das Prüftool ist ein Programm, welches XML-Dateien (Dokumente) in Abhängigkeit von ihren Dokumenttypen gegen verschiedene Validierungsregeln (XML Schema und Schematron) prüft und das Ergebnis zu einem Konformitätsbericht (Konformitätsstatus *valid* oder *invalid*) mit einer Empfehlung zur Weiterverarbeitung (*accept*) oder Ablehnung (*reject*) aggregiert. Mittels Konfiguration kann bestimmt werden, welche der Konformitätsregeln durch ein Dokument, das zur Weiterverarbeitung empfohlen (*accept*) wird, verletzt sein dürfen.

Das Prüftool selbst ist fachunabhängig und kennt weder spezifische Dokumentinhalte noch Validierungsregeln. Diese werden im Rahmen einer Prüftool-Konfiguration definiert, welche zur Anwendung des Prüftools erforderlich ist.

Weitere Details auf der [Validator Projektseite](https://github.com/itplr-kosit/validator).

### Validator Konfiguration XRechnung

Eine eigenständige Konfiguration für den Standard [XRechnung](https://xeinkauf.de/xrechnung/) wird ebenfalls auf [GitHub bereitgestellt](https://github.com/itplr-kosit/validator-configuration-xrechnung) ([Releases](https://github.com/itplr-kosit/validator-configuration-xrechnung/releases)). Diese enthält alle notwendigen Ressourcen zu der Norm EN16931 (XML-Schema und [Schematron Regeln](https://github.com/CenPC434/validation) u.a.) und die [XRechnung Schematron Regeln](https://github.com/itplr-kosit/xrechnung-schematron) in ihren aktuellen Versionen.

Weitere Details auf der [Validator Konfiguration XRechnung Projektseite](https://github.com/itplr-kosit/validator-configuration-xrechnung).

### XRechnung Schematron Regeln

Technische Implementierung der Geschäftsregeln des Standards [XRechnung](https://xeinkauf.de/xrechnung/) in Schematron Rules für XML Validierung.

Weitere Details auf der [XRechnung Schematron Regeln Projektseite](https://github.com/itplr-kosit/xrechnung-schematron).

### XRechnung Visualisierung

XSL Transformatoren für die Generierung von HTML Web-Seiten und PDF Dateien.

Diese zeigen den Inhalt von elektronischen Rechnungen an, die dem Standard [XRechnung](https://xeinkauf.de/xrechnung/) entsprechen.

Weitere Details auf der [XRechnung Visualisierung Projektseite](https://github.com/itplr-kosit/xrechnung-visualization).

### XRechnung Testsuite

Valide Testdokumente des Standards [XRechnung](https://xeinkauf.de/xrechnung/).

Diese dienen dazu, bei Organisationen, die IT-Fachverfahren herstellen und betreiben, das Verständnis der [XRechnung-Spezifikation](https://xeinkauf.de/xrechnung/versionen-und-bundles/) zu fördern, indem die umfangreichen und komplexen Vorgaben und Besonderheiten der Spezifikation durch valide Testdokumente veranschaulicht werden. Die Testdokumente stehen zur freien Verfügung für die Einbindung in eigene Testverfahren.

Weitere Details auf der [XRechnung Testsuite Projektseite](https://github.com/itplr-kosit/xrechnung-testsuite).
