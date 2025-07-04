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
using System.Runtime.CompilerServices;
using System.Text;


namespace s2industries.ZUGFeRD
{    
    internal class InvoiceCommentConstants
    {
        internal static string IncludedSupplyChainTradeLineItemComment = "Artikelposition";
        internal static string NetPriceProductTradePriceComment = "Nettopreis";
        internal static string SpecifiedTradeSettlementLineMonetarySummationComment = "Gesamtsummierung pro Position";
        internal static string ApplicableHeaderTradeAgreementComment = "Formulardaten wie Käufer, Verkäufer etc.";
        internal static string BuyerReferenceComment = "Ihr Zeichen bzw. Leitweg-Id";
        internal static string SellerTradePartyComment = "Verkäufer";
        internal static string BuyerTradePartyComment = "Käufer";
        internal static string BuyerOrderReferencedDocumentComment = "Bestelldokument";
        internal static string ApplicableHeaderTradeDeliveryComment = "Lieferdaten samt abw. Lieferadresse";
        internal static string DespatchAdviceReferencedDocumentComment = "Lieferschein";
        internal static string ApplicableHeaderTradeSettlementComment = "Dokumentdaten";
        internal static string SpecifiedTradeSettlementPaymentMeansComment = "Zahlungsart mit Zahlungsinfo";
        internal static string ApplicableTradeTaxComment = "Steuerposition pro Steuersatz für Dokument";
        internal static string SpecifiedTradeSettlementHeaderMonetarySummationComment = "Gesamtsummierung des Dokumentes";
    }
}
