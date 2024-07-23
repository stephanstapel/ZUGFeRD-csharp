<pattern xmlns="http://purl.oclc.org/dsdl/schematron" is-a="model"
  id="UBL-model">
  <param name="BR-DE-01" value="cac:PaymentMeans" />
  <param name="BR-DE-02" value="cac:Party/cac:Contact" />
  <param name="BR-DE-03" value="cbc:CityName[boolean(normalize-space(.))]" />
  <param name="BR-DE-04" value="cbc:PostalZone[boolean(normalize-space(.))]" />
  <param name="BR-DE-05" value="cbc:Name[boolean(normalize-space(.))]" />
  <param name="BR-DE-06" value="cbc:Telephone[boolean(normalize-space(.))]" />
  <param name="BR-DE-07" value="cbc:ElectronicMail[boolean(normalize-space(.))]" />
  <param name="BR-DE-08" value="cbc:CityName[boolean(normalize-space(.))]" />
  <param name="BR-DE-09" value="cbc:PostalZone[boolean(normalize-space(.))]" />
  <param name="BR-DE-10" value="cbc:CityName[boolean(normalize-space(.))]" />
  <param name="BR-DE-11" value="cbc:PostalZone[boolean(normalize-space(.))]" />
  <param name="BR-DE-13"
    value="count((cac:PaymentMeans/cac:PayeeFinancialAccount)[1]) + count(cac:PaymentMeans/cac:CardAccount) + count(cac:PaymentMeans/cac:PaymentMandate) = 1" />
  <param name="BR-DE-14"
    value="cac:TaxCategory/cbc:Percent[boolean(normalize-space(.))]" />
  <param name="BR-DE-15" value="cbc:BuyerReference[boolean(normalize-space(.))]" />
  <param name="BR-DE-16"
    value="(cac:TaxRepresentativeParty, cac:AccountingSupplierParty/cac:Party/cac:PartyTaxScheme/cbc:CompanyID[boolean(normalize-space(.))])" />
  <param name="BR-DE-17"
    value="cbc:CreditNoteTypeCode = ('326', '380', '384', '389', '381', '875', '876', '877')" />
  <param name="BR-DE-18"
    value="every $line in cac:PaymentTerms/cbc:Note/tokenize(.,'(\r\n|\r|\n)') satisfies if(count(tokenize($line,'#')) &gt; 1) then tokenize($line,'#')[1]='' and (tokenize($line,'#')[2]='SKONTO' or tokenize($line,'#')[2]='VERZUG') and string-length(replace(tokenize($line,'#')[3],'TAGE=[0-9]+',''))=0 and string-length(replace(tokenize($line,'#')[4],'PROZENT=[0-9]+\.[0-9]{2}',''))=0 and (tokenize($line,'#')[5]='' and empty(tokenize($line,'#')[6]) or string-length(replace(tokenize($line,'#')[5],'BASISBETRAG=[0-9]+\.[0-9]{2}',''))=0 and tokenize($line,'#')[6]='' and empty(tokenize($line,'#')[7])) else true()" />
  <param name="BR-DE-19"
    value="not(cbc:PaymentMeansCode = '58') or  matches(normalize-space(replace(cac:PayeeFinancialAccount/cbc:ID, '([ \n\r\t\s])', '')), '^[A-Z]{2}[0-9]{2}[a-zA-Z0-9]{0,30}$') and xs:integer(string-join(for $cp in string-to-codepoints(concat(substring(normalize-space(replace(cac:PayeeFinancialAccount/cbc:ID, '([ \n\r\t\s])', '')),5),upper-case(substring(normalize-space(replace(cac:PayeeFinancialAccount/cbc:ID, '([ \n\r\t\s])', '')),1,2)),substring(normalize-space(replace(cac:PayeeFinancialAccount/cbc:ID, '([ \n\r\t\s])', '')),3,2))) return  (if($cp > 64) then $cp - 55 else  $cp - 48),'')) mod 97 = 1" />
  <param name="BR-DE-20"
    value="not(cbc:PaymentMeansCode = '59') or  matches(normalize-space(replace(cac:PaymentMandate/cac:PayerFinancialAccount/cbc:ID, '([ \n\r\t\s])', '')), '^[A-Z]{2}[0-9]{2}[a-zA-Z0-9]{0,30}$') and xs:integer(string-join(for $cp in string-to-codepoints(concat(substring(normalize-space(replace(cac:PaymentMandate/cac:PayerFinancialAccount/cbc:ID, '([ \n\r\t\s])', '')),5),upper-case(substring(normalize-space(replace(cac:PaymentMandate/cac:PayerFinancialAccount/cbc:ID, '([ \n\r\t\s])', '')),1,2)),substring(normalize-space(replace(cac:PaymentMandate/cac:PayerFinancialAccount/cbc:ID, '([ \n\r\t\s])', '')),3,2))) return  (if($cp > 64) then $cp - 55 else  $cp - 48),'')) mod 97 = 1" />
  <param name="BR-DE-21"
    value="cbc:CustomizationID = 'urn:cen.eu:en16931:2017#compliant#urn:xoev-de:kosit:standard:xrechnung_1.2'" />

  <param name="CREDIT_NOTE" value="//ubl:CreditNote" />
  <param name="BG-4_SELLER" value="//ubl:CreditNote/cac:AccountingSupplierParty" />
  <param name="BG-5_SELLER_POSTAL_ADDRESS"
    value="//ubl:CreditNote/cac:AccountingSupplierParty/cac:Party/cac:PostalAddress" />
  <param name="BG-6_SELLER_CONTACT"
    value="//ubl:CreditNote/cac:AccountingSupplierParty/cac:Party/cac:Contact" />

  <param name="BG-8_BUYER_POSTAL_ADDRESS"
    value="//ubl:CreditNote/cac:AccountingCustomerParty/cac:Party/cac:PostalAddress" />

  <param name="BG-15_DELIVER_TO_ADDRESS"
    value="//ubl:CreditNote/cac:Delivery/cac:DeliveryLocation/cac:Address" />

  <param name="BG-16_PAYMENT_INSTRUCTIONS"
    value="//ubl:CreditNote/cac:PaymentMeans" />

  <param name="BG-23_VAT_BREAKDOWN"
    value="//ubl:CreditNote/cac:TaxTotal/cac:TaxSubtotal" />

</pattern>
