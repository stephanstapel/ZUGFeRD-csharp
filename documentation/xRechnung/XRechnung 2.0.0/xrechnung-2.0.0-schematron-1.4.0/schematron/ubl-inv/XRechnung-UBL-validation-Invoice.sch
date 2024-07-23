<?xml version="1.0" encoding="UTF-8"?>
<schema xmlns="http://purl.oclc.org/dsdl/schematron"
  xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"
  xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"
  queryBinding="xslt2" defaultPhase="xrechnung-model">
  <title>Schematron Version 1.4.0 - XRechnung 2.0.0 compatible - UBL - Invoice</title>
  <ns prefix="cbc"
    uri="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" />
  <ns prefix="cac"
    uri="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2" />
  <ns prefix="ext"
    uri="urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2" />
  <ns prefix="ubl" uri="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2" />
  <ns prefix="xs" uri="http://www.w3.org/2001/XMLSchema" />

  <phase id="xrechnung-model">
    <active pattern="model-pattern" />
    <active pattern="ubl-extension-pattern" />
  </phase>
  <!-- Abstract patterns -->
  <!-- ========================= -->
  <include href="abstract/XRechnung-model.sch" />

  <!-- Data Binding parameters -->
  <!-- ======================= -->
  <include href="UBL/XRechnung-UBL-model.sch" />
  <pattern xmlns="http://purl.oclc.org/dsdl/schematron"
    id="ubl-extension-pattern">

    <!-- robust version of testing extension https://stackoverflow.com/questions/3206975/xpath-selecting-elements-that-equal-a-value  -->
    <let name="isExtension"
      value="exists(/ubl:Invoice/cbc:CustomizationID[text() = 'urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_2.0#conformant#urn:xoev-de:kosit:extension:xrechnung_2.0'])" />

    <rule context="cbc:EmbeddedDocumentBinaryObject[$isExtension]">
      <!-- BR-DEX-01
        checks whether an EmbeddedCocumentBinaryObject has a valid mimeCode (incl. XML)
        -->
      <assert
        test=".[@mimeCode = 'application/pdf' or @mimeCode = 'image/png' or @mimeCode = 'image/jpeg' or @mimeCode = 'text/csv' or @mimeCode = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' or @mimeCode = 'application/vnd.oasis.opendocument.spreadsheet' or @mimeCode = 'application/xml']"

        id="BR-DEX-01" flag="fatal">[BR-DEX-01] Das Element <name /> "Attached Document" (BT-125) benutzt einen nicht zulässigen MIME-Code: <value-of
             select="@mimeCode" />. Im Falle einer Extension darf zusätzlich zu der Liste der mime codes (definiert in Abschnitt 8.2, "Binary Object") der MIME-Code application/xml genutzt werden.</assert>
    </rule> 

    <rule
      context="/ubl:Invoice[$isExtension]">
      <!-- BR-DEX-02
         this rule consists of two parts:
         part one proofs in every invoiceline whether the lineextensionamount of it is equal to the sum of lineExtensionAmount of the ancillary subinvoicelines
         part two proofs whether the count of invoice lines with correct lineextensionamounts according to part one is equal to the count of subinvoicelines with including subinvoicelines
         every amount has to be cast to decimal cause of floating point problems -->
      <assert
        test="count(//cac:SubInvoiceLine) = 0 or (sum(./cac:InvoiceLine/xs:decimal(cbc:LineExtensionAmount)) = sum(child::cac:InvoiceLine/cac:SubInvoiceLine/xs:decimal(cbc:LineExtensionAmount))) and (count(//cac:SubInvoiceLine[xs:decimal(cbc:LineExtensionAmount) = sum(child::cac:SubInvoiceLine/xs:decimal(cbc:LineExtensionAmount))]) = count(//cac:SubInvoiceLine[count(cac:SubInvoiceLine) > 0]))"

        flag="warning" id="BR-DEX-02"
        >[BR-DEX-02] Der Wert von "Invoice line net amount" (BT-131) einer "INVOICE LINE"
        (BG-25) oder einer "SUB INVOICE LINE" (BG-DEX-01) soll der Summe
        der "Invoice line net amount" (BT-131) der direkt darunterliegenden "SUB
        INVOICE LINE" (BG-DEX-01) entsprechen.</assert>

    </rule>
  </pattern>
</schema>
