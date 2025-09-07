/*
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

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Type codes for the various kinds of documents that can be represented using ZUGFeRD.
    /// </summary>
    public enum InvoiceType
    {
        /// <summary>
        /// Request for payment
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("71")]
        RequestForPayment,

        /// <summary>
        /// Debit note related to goods or services
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("80")]
        DebitNoteRelatedToGoodsOrServices,

        /// <summary>
        /// Credit note related to goods or services
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("81")]
        CreditNoteRelatedToGoodsOrServices,

        /// <summary>
        /// Metered services invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("82")]
        MeteredServicesInvoice,

        /// <summary>
        /// Credit note related to financial adjustments
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("83")]
        CreditNoteRelatedToFinancialAdjustments,

        /// <summary>
        /// Debit note related to financial adjustments
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("84")]
        DebitnoteRelatedToFinancialAdjustments,

        /// <summary>
        /// Tax notification
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("102")]
        TaxNotification,

        /// <summary>
        /// Invoicing data sheet
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("130")]
        InvoicingDataSheet,

        /// <summary>
        /// Direct payment valuation
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("202")]
        DirectPaymentValuation,

        /// <summary>
        /// Provisional payment valuation
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("203")]
        ProvisionalPaymentValuation,

        /// <summary>
        /// Payment valuation
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("204")]
        PaymentValuation,

        /// <summary>
        /// Interim application for payment
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("211")]
        InterimApplicationForPayment,

        /// <summary>
        /// Final payment request based on completion of work
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("218")]
        FinalPaymentRequestBasedOnCompletionOfWork,

        /// <summary>
        /// Payment request for completed units
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("219")]
        PaymentRequestForCompletedUnits,

        /// <summary>
        /// Self billed credit note
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("261")]
        SelfBilledCreditNote,

        /// <summary>
        /// Consolidated credit note - goods and services
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("262")]
        ConsolidatedCreditNoteGoodsAndServices,

        /// <summary>
        /// Price variation invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("295")]
        PriceVariationInvoice,

        /// <summary>
        /// Credit note for price variation
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("296")]
        CreditNoteForPriceVariation,

        /// <summary>
        /// Delcredere credit note
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("308")]
        DelcredereCreditNote,

        /// <summary>
        /// Proforma invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("325")]
        ProformaInvoice,

        /// <summary>
        /// Partial invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("326")]
        PartialInvoice,

        /// <summary>
        /// Commercial invoice which includes a packing list
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("331")]
        CommercialInvoiceWithPackingList,

        /// <summary>
        /// Commercial invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("380")]
        Invoice,

        /// <summary>
        /// Credit note
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("381")]
        CreditNote,

        /// <summary>
        /// Commission note
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("382")]
        CommissionNote,

        /// <summary>
        /// Debit note
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("383")]
        DebitNote,

        /// <summary>
        /// Corrected invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("384")]
        Correction,

        /// <summary>
        /// Consolidated invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("385")]
        ConsolidatedInvoice,

        /// <summary>
        /// Prepayment invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("386")]
        PrepaymentInvoice,

        /// <summary>
        /// Hire invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("387")]
        HireInvoice,

        /// <summary>
        /// Tax invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("388")]
        TaxInvoice,

        /// <summary>
        /// Self-billed invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("389")]
        SelfBilledInvoice,

        /// <summary>
        /// Delcredere invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("390")]
        DelcredereInvoice,

        /// <summary>
        /// Factored invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("393")]
        FactoredInvoice,

        /// <summary>
        /// Lease invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("394")]
        LeaseInvoice,

        /// <summary>
        /// Consignment invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("395")]
        ConsignmentInvoice,

        /// <summary>
        /// Factored credit note
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("396")]
        FactoredCreditNote,

        /// <summary>
        /// Optical Character Reading (OCR) payment credit note
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("420")]
        OcrPaymentCreditNote,

        /// <summary>
        /// Debit advice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("456")]
        DebitAdvice,

        /// <summary>
        /// Reversal of debit
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("457")]
        ReversalOfDebit,

        /// <summary>
        /// Reversal of credit
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("458")]
        ReversalOfCredit,

        /// <summary>
        /// Self-billed corrective invoice, invoice type, Corrected
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("471")]
        SelfBilledCorrectiveInvoice,

        /// <summary>
        /// Factored Corrective Invoice, invoice type, Corrected
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("472")]
        FactoredCorrectiveInvoice,

        /// <summary>
        /// Self billed Factored corrective invoice, invoice type, Corrected
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("473")]
        SelfBilledFactoredCorrectiveInvoice,

        /// <summary>
        /// Self Prepayment invoice, invoice type, Original
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("500")]
        SelfPrepaymentInvoice,

        /// <summary>
        /// Self billed factored invoice, invoice type, Original
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("501")]
        SelfBilledFactoredInvoice,

        /// <summary>
        /// Self billed factored Credit Note, Credit note type, Corrected
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("502")]
        SelfBilledFactoredCreditNote,

        /// <summary>
        /// Prepayment credit note, credit note type, Corrected
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("503")]
        PrepaymentCreditNoteCorrected,

        /// <summary>
        /// Self billed debit note
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("527")]
        SelfBilledDebitNote,

        /// <summary>
        /// Forwarder's credit note
        /// EN16931 interpretation: Credit Note
        /// </summary>
        [EnumStringValue("532")]
        ForwardersCreditNote,

        /// <summary>
        /// Forwarder's invoice discrepancy report
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("553")]
        ForwardersInvoiceDiscrepancyReport,

        /// <summary>
        /// Insurer's invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("575")]
        InsurersInvoice,

        /// <summary>
        /// Forwarder's invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("623")]
        ForwardersInvoice,

        /// <summary>
        /// Port charges documents
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("633")]
        PortChargesDocuments,

        /// <summary>
        /// Invoice information for accounting purposes
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("751")]
        InvoiceInformation,

        /// <summary>
        /// Freight invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("780")]
        FreightInvoice,

        /// <summary>
        /// Claim notification
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("817")]
        ClaimNotification,

        /// <summary>
        /// Consular invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("870")]
        ConsularInvoice,

        /// <summary>
        /// Partial construction invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("875")]
        PartialConstructionInvoice,

        /// <summary>
        /// Partial final construction invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("876")]
        PartialFinalConstructionInvoice,

        /// <summary>
        /// Final construction invoice
        /// EN16931 interpretation: Invoice
        /// </summary>
        [EnumStringValue("877")]
        FinalConstructionInvoice
    }
}
