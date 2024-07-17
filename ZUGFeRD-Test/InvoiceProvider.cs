﻿/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using s2industries.ZUGFeRD;

namespace ZUGFeRD_Test
{
  internal class InvoiceProvider
  {
    internal InvoiceDescriptor CreateInvoice()
    {
      InvoiceDescriptor desc = InvoiceDescriptor.CreateInvoice("471102", new DateTime(2018, 03, 05), CurrencyCodes.EUR);
      desc.AddNote("Rechnung gemäß Bestellung vom 01.03.2018.");
      desc.AddNote(note: "Lieferant GmbH\r\nLieferantenstraße 20\r\n80333 München\r\nDeutschland\r\nGeschäftsführer: Hans Muster\r\nHandelsregisternummer: H A 123\r\n",
                   subjectCode: SubjectCodes.REG
                  );

      desc.AddTradeLineItem(name: "Trennblätter A4",
                            unitCode: QuantityCodes.H87,
                            sellerAssignedID: "TB100A4",
                            id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4012345001235"),
                            grossUnitPrice: 9.9m,
                            netUnitPrice: 9.9m,
                            billedQuantity: 20m,
                            taxType: TaxTypes.VAT,
                            categoryCode: TaxCategoryCodes.S,
                            taxPercent: 19m
                           );

      desc.AddTradeLineItem(name: "Joghurt Banane",
          unitCode: QuantityCodes.H87,
          sellerAssignedID: "ARNR2",
          id: new GlobalID(GlobalIDSchemeIdentifiers.EAN, "4000050986428"),
          grossUnitPrice: 5.5m,
          netUnitPrice: 5.5m,
          billedQuantity: 50,
          taxType: TaxTypes.VAT,
          categoryCode: TaxCategoryCodes.S,
          taxPercent: 7
          );

      desc.ReferenceOrderNo = "04011000-12345-34";
      desc.SetSeller(name: "Lieferant GmbH",
                     postcode: "80333",
                     city: "München",
                     street: "Lieferantenstraße 20",
                     country: CountryCodes.DE,
                     id: "",
                     globalID: new GlobalID(GlobalIDSchemeIdentifiers.GLN, "4000001123452"),
                     legalOrganization: new LegalOrganization(GlobalIDSchemeIdentifiers.GLN, "4000001123452", "Lieferant GmbH")
                     );
      desc.SetSellerContact(name: "Max Mustermann",
                            orgunit: "Muster-Einkauf",
                            emailAddress: "Max@Mustermann.de",
                            phoneno: "+49891234567"
                           );
      desc.AddSellerTaxRegistration("201/113/40209", TaxRegistrationSchemeID.FC);
      desc.AddSellerTaxRegistration("DE123456789", TaxRegistrationSchemeID.VA);

      desc.SetBuyer(name: "Kunden AG Mitte",
                    postcode: "69876",
                    city: "Frankfurt",
                    street: "Kundenstraße 15",
                    country: CountryCodes.DE,
                    id: "GE2020211"
                    );

      desc.ActualDeliveryDate = new DateTime(2018, 03, 05);
      desc.SetPaymentMeans(PaymentMeansTypeCodes.SEPACreditTransfer, "Zahlung per SEPA Überweisung.");
      desc.AddCreditorFinancialAccount(iban: "DE02120300000000202051", bic: "BYLADEM1001", name: "Kunden AG");
      desc.AddDebitorFinancialAccount(iban: "DB02120300000000202051", bic: "DBBYLADEM1001", bankName: "KundenDB AG");
            desc.AddApplicableTradeTax(basisAmount: 275.0m,
                                 percent: 7m,
                                 typeCode: TaxTypes.VAT,
                                 categoryCode: TaxCategoryCodes.S
                                 );

      desc.AddApplicableTradeTax(basisAmount: 198.0m,
                                 percent: 19m,
                                 typeCode: TaxTypes.VAT,
                                 categoryCode: TaxCategoryCodes.S
                                 );

      desc.SetTradePaymentTerms("Zahlbar innerhalb 30 Tagen netto bis 04.04.2018, 3% Skonto innerhalb 10 Tagen bis 15.03.2018");
      desc.SetTotals(lineTotalAmount: 473.0m,
                     taxBasisAmount: 473.0m,
                     taxTotalAmount: 56.87m,
                     grandTotalAmount: 529.87m,
                     duePayableAmount: 529.87m
                    );

      return desc;
    } // !CreateInvoice()
  }
}
