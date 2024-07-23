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
        /// Document/message for providing debit information related to financial adjustments to the relevant party.
        /// </summary>
        DebitnoteRelatedToFinancialAdjustments = 84,

        /// <summary>
        /// Self billed credit note (261)
        /// is a Credit Note
        ///
        /// A document which indicates that the customer
        /// is claiming credit in a self billing environment
        /// </summary>
        SelfBilledCreditNote = 261,

        /// <summary>
        /// Partial invoice (326)
        /// is an Invoice
        /// 
        /// Teilrechnung
        /// </summary>
        PartialInvoice = 326,

        /// <summary>
        /// Commercial invoice (380)
        /// is an Invoice
        /// 
        /// This is the main invoice type
        /// 
        /// Handelsrechnung
        /// </summary>
        Invoice = 380,

        /// <summary>
        /// Credit note (381)
        /// is a Credit Note
        /// 
        /// This is the main credit note type
        /// 
        /// Gutschriftanzeige
        /// </summary>
        CreditNote = 381,

        /// <summary>
        /// Debit note (383)
        /// is an Invoice
        /// 
        /// Belastungsanzeige
        /// </summary>
        DebitNote = 383,

        /// <summary>
        /// Corrected invoice (384)
        /// is an Invoice
        /// 
        /// Rechnungskorrektur
        /// </summary>
        Correction = 384,

        /// <summary>
        /// Prepayment invoice (386)
        /// is an Invoice
        /// 
        /// Vorauszahlungsrechnung
        /// Eine Rechnung, die Vorauszahlung für Produkte anfordert.
        /// Die darin enthaltenen Beträge werden in der Schlussrechnung abgezogen.
        /// </summary>
        PrepaymentInvoice = 386,

        /// <summary>
        /// Self-billed invoice (389)
        /// is an Invoice
        /// 
        /// Gutschrift (Selbst ausgestellte Rechnung)
        /// Gutschrift im Gutschriftverfahren
        /// Eine Rechnung, die der Zahlungspflichtige selbst ausstellt
        /// anstelle des Verkäufers.
        /// </summary>
        SelfBilledInvoice = 389,

        /// <summary>
        /// Invoice information for accounting purposes (751)
        /// is an Invoice
        /// 
        /// Buchungshilfe - KEINE Rechnung
        /// 
        /// Für die Profile BASIC WL und MINIMUM darf ausschließlich dieser
        /// Code 751 "Buchungshilfe - KEINE Rechnung" verwendet werden,
        /// da diese Profile in DE steuerrechtlich keine Rechnungen darstellen!
        /// </summary>
        InvoiceInformation = 751,

        /// <summary>
        /// Corrected invoice (1380)
        /// is an Invoice
        /// Old ZUGFeRD variant, use
        /// Corrected Invoice (384) instead
        /// </summary>
        CorrectionOld = 1380,
        
        /// <summary>
        /// Cancellation (457)
        ///
        /// Storno
        /// </summary>
        Cancellation = 457,

        /// <summary>
        /// Appears to be only valid in Germany for XRechnung (BR-DE-17)
        /// </summary>
        PartialConstructionInvoice = 875,

        /// <summary>
        /// Appears to be only valid in Germany for XRechnung (BR-DE-17)
        /// </summary>
        PartialFinalConstructionInvoice = 876,

        /// <summary>
        /// Appears to be only valid in Germany for XRechnung (BR-DE-17)
        /// </summary>
        FinalConstructionInvoice = 877,

        /// <summary>
        /// Unknown (0)
        /// is a fall back for all other cases
        /// (not all UNTDID 1001 codes are allowed, but there are several more)
        /// </summary>
        Unknown = 0

    }


    internal static class InvoiceTypeExtensions
    {

        public static InvoiceType FromString(this InvoiceType _, string s)
        {
            try
            {
                return (InvoiceType) Enum.Parse(typeof(InvoiceType), s);
            }
            catch
            {
                return InvoiceType.Unknown;
            }
        } // !FromString()


        public static string EnumToString(this InvoiceType i)
        {
            return i.ToString("g");
        } // !ToString()
    }
}
