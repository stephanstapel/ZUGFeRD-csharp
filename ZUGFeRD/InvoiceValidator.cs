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
using System.Threading.Tasks;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Validator for ZUGFeRD invoice descriptor.
    ///
    /// Currently limited to summarizing line totals
    ///
    /// Output syntax copied from Konik library (https://konik.io/)
    /// </summary>
    public class InvoiceValidator
    {
        public static void ValidateAndPrint(InvoiceDescriptor descriptor, ZUGFeRDVersion version, string filename = null)
        {
            ValidationResult validationResult = Validate(descriptor, version);

            if (!String.IsNullOrWhiteSpace(filename))
            {
                System.IO.File.WriteAllText(filename, string.Join("\n", validationResult.Messages));
            }

            foreach (string line in validationResult.Messages)
            {
                System.Console.WriteLine(line);
            }
        } // !ValidateAndPrint()

        public static ValidationResult Validate(InvoiceDescriptor descriptor, ZUGFeRDVersion version)
        {
            ValidationResult retval = new ValidationResult()
            {
                IsValid = true
            };

            if (descriptor == null)
            {
                retval.Messages.Add("Invalid invoice descriptor");
                retval.IsValid = false;
                return retval;
            }

            // line item summation
            retval.Messages.Add("Validating invoice monetary summation");
            retval.Messages.Add(String.Format("Starting recalculating line total from {0} items...", descriptor.GetTradeLineItems().Count));
            int lineCounter = 0;

            decimal lineTotal = 0m;
            Dictionary<decimal, decimal> lineTotalPerTax = new Dictionary<decimal, decimal>();
            foreach (TradeLineItem item in descriptor.GetTradeLineItems())
            {
                decimal total = decimal.Multiply(item.NetUnitPrice, item.BilledQuantity);
                lineTotal += total;

                if (!lineTotalPerTax.ContainsKey(item.TaxPercent))
                {
                    lineTotalPerTax.Add(item.TaxPercent, new decimal());
                }
                lineTotalPerTax[item.TaxPercent] += total;

                /*
                retval.Add(String.Format("==> {0}:", ++lineCounter));
                retval.Add(String.Format("Recalculating item: [{0}]", item.Name));
                retval.Add(String.Format("Line total formula: {0:0.0000} EUR (net price) x {1:0.0000} (quantity)", item.NetUnitPrice, item.BilledQuantity));

                retval.Add(String.Format("Recalculated item line total = {0:0.0000} EUR", total));
                retval.Add(String.Format("Recalculated item tax = {0:0.00} %", item.TaxPercent));
                retval.Add(String.Format("Current monetarySummation.lineTotal = {0:0.0000} EUR(the sum of all line totals)", lineTotal));
                */

                retval.Messages.Add(String.Format("{0};{1};{2}", ++lineCounter, item.Name, total));
            }

            retval.Messages.Add("==> DONE!");
            retval.Messages.Add("Finished recalculating monetarySummation.lineTotal...");
            retval.Messages.Add("Adding tax amounts from invoice allowance charge...");
            
            decimal chargeTotal = 0.0m;
            foreach (TradeCharge charge in descriptor.GetTradeCharges())
            {
                retval.Messages.Add(String.Format("==> added {0:0.00} to {1:0.00}%", -charge.Amount, charge.Tax.Percent));

                if (!lineTotalPerTax.ContainsKey(charge.Tax.Percent))
                {
                    lineTotalPerTax.Add(charge.Tax.Percent, new decimal());
                }
                lineTotalPerTax[charge.Tax.Percent] -= charge.Amount;
                chargeTotal += charge.Amount;
            }

            decimal allowanceTotal = 0.0m;
            foreach (TradeAllowance allowance in descriptor.GetTradeAllowances())
            {
                retval.Messages.Add(String.Format("==> added {0:0.00} to {1:0.00}%", -allowance.Amount, allowance.Tax.Percent));

                if (!lineTotalPerTax.ContainsKey(allowance.Tax.Percent))
                {
                    lineTotalPerTax.Add(allowance.Tax.Percent, new decimal());
                }
                lineTotalPerTax[allowance.Tax.Percent] -= allowance.Amount;
                allowanceTotal += allowance.Amount;
            }

            retval.Messages.Add("Adding tax amounts from invoice service charge...");
            // TODO

            // TODO ausgeben: Recalculating tax basis for tax percentages: [Key{percentage=7.00, code=[VAT] Value added tax, category=[S] Standard rate}, Key{percentage=19.00, code=[VAT] Value added tax, category=[S] Standard rate}]
            retval.Messages.Add(String.Format("Recalculated tax basis = {0:0.0000}", lineTotal - allowanceTotal));
            retval.Messages.Add("Calculating tax total...");

            decimal taxTotal = 0.0m;
            foreach (KeyValuePair<decimal, decimal> kv in lineTotalPerTax)
            {
                decimal taxTotalForLine = Decimal.Divide(Decimal.Multiply(kv.Value, kv.Key), 100.0m);
                taxTotal += taxTotalForLine;
                retval.Messages.Add(String.Format("===> {0:0.0000} x {1:0.00}% = {2:0.00}", kv.Value, kv.Key, taxTotalForLine));
            }

            decimal grandTotal = lineTotal - allowanceTotal + taxTotal + chargeTotal;
            decimal prepaid = 0m; // TODO: calculcate

            retval.Messages.Add(String.Format("Recalculated tax total = {0:0.00}", taxTotal));
            retval.Messages.Add(String.Format("Recalculated grand total = {0:0.0000} EUR(tax basis total + tax total)", grandTotal));
            retval.Messages.Add("Recalculating invoice monetary summation DONE!");
            retval.Messages.Add(String.Format("==> result: MonetarySummation[lineTotal = {0:0.0000} EUR, chargeTotal = {1:0.0000} EUR, allowanceTotal = {2:0.0000} EUR, taxBasisTotal = {3:0.0000} EUR, taxTotal = {4:0.0000} EUR, grandTotal = {5:0.0000} EUR, totalPrepaid = {6:0.0000} EUR, duePayable = {7:0.0000} EUR]",
                                     lineTotal,
                                     chargeTotal,
                                     allowanceTotal,
                                     lineTotal - allowanceTotal + chargeTotal, // tax basis total
                                     taxTotal,
                                     grandTotal,
                                     prepaid,
                                     lineTotal - allowanceTotal + taxTotal + chargeTotal + prepaid
                                     ));


            decimal taxBasisTotal = descriptor.GetApplicableTradeTaxes().Sum(t => t.BasisAmount);
            decimal allowanceTotalSummedPerTradeAllowanceCharge = descriptor.GetTradeAllowances().Sum(a => a.ActualAmount);
            decimal chargesTotalSummedPerTradeAllowanceCharge = descriptor.GetTradeCharges().Sum(c => c.ActualAmount);

            if (!descriptor.TaxTotalAmount.HasValue)
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.taxTotal Message: Kein TaxTotalAmount vorhanden"));
                retval.IsValid = false;
            }
            else if (Math.Abs(taxTotal - descriptor.TaxTotalAmount.Value) < 0.01m)
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.taxTotal Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", taxTotal));
            }
            else
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.taxTotal Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", taxTotal, descriptor.TaxTotalAmount));
                retval.IsValid = false;
            }

            if (Math.Abs(lineTotal - descriptor.LineTotalAmount.Value) < 0.01m)
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.lineTotal Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", lineTotal));
            }
            else
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.lineTotal Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", lineTotal, descriptor.LineTotalAmount));
                retval.IsValid = false;
            }

            if (!descriptor.GrandTotalAmount.HasValue)
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.grandTotal Message: Kein GrandTotalAmount vorhanden"));
                retval.IsValid = false;
            }
            else if (Math.Abs(grandTotal - descriptor.GrandTotalAmount.Value) < 0.01m)
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.grandTotal Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", grandTotal));
            }
            else
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.grandTotal Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", grandTotal, descriptor.GrandTotalAmount));
                retval.IsValid = false;
            }

            /*
             * @todo Richtige Validierung implementieren
             */
            if (Math.Abs(taxBasisTotal - taxBasisTotal) < 0.01m)
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.taxBasisTotal Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", taxBasisTotal));
            }
            else
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.taxBasisTotal Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", taxBasisTotal, taxBasisTotal));
                retval.IsValid = false;
            }

            if (Math.Abs(allowanceTotalSummedPerTradeAllowanceCharge - allowanceTotal) < 0.01m)
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.allowanceTotal  Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", allowanceTotalSummedPerTradeAllowanceCharge));
            }
            else
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.allowanceTotal  Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", allowanceTotalSummedPerTradeAllowanceCharge, allowanceTotal));
                retval.IsValid = false;
            }

            if (Math.Abs(chargesTotalSummedPerTradeAllowanceCharge - chargeTotal) < 0.01m)
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.chargeTotal  Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", chargesTotalSummedPerTradeAllowanceCharge));
            }
            else
            {
                retval.Messages.Add(String.Format("trade.settlement.monetarySummation.chargeTotal  Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", chargesTotalSummedPerTradeAllowanceCharge, chargeTotal));
                retval.IsValid = false;
            }

            // version-specific validation
            ValidationResult versionSpecificResults;
            switch (version)
            {
                case ZUGFeRDVersion.Version1:
                {
                    versionSpecificResults = _ValidateAccordingToVersion1(descriptor);
                    break;
                }
                default:
                {
                    versionSpecificResults = new ValidationResult { IsValid = true };
                    break;
                }
            }

            retval.IsValid = retval.IsValid && versionSpecificResults.IsValid;
            retval.Messages.AddRange(versionSpecificResults.Messages);

            return retval;
        } // !Validate()

        private static ValidationResult _ValidateAccordingToVersion1(InvoiceDescriptor descriptor)
        {
            ValidationResult retval = new ValidationResult()
            {
                IsValid = true
            };

            if (!EnumExtensions.In<GlobalIDSchemeIdentifiers>(descriptor.Buyer?.GlobalID?.SchemeID, GlobalIDSchemeIdentifiers.Swift, GlobalIDSchemeIdentifiers.DUNS, GlobalIDSchemeIdentifiers.GLN, GlobalIDSchemeIdentifiers.EAN, GlobalIDSchemeIdentifiers.Odette))
            {
                retval.IsValid = false;
                retval.Messages.Add($"Global identifier scheme {descriptor.Buyer?.GlobalID?.SchemeID} is not supported for buyers in ZUGFeRD 1.0");
            }

            if (!EnumExtensions.In<GlobalIDSchemeIdentifiers>(descriptor.Seller?.GlobalID?.SchemeID, GlobalIDSchemeIdentifiers.Swift, GlobalIDSchemeIdentifiers.DUNS, GlobalIDSchemeIdentifiers.GLN, GlobalIDSchemeIdentifiers.EAN, GlobalIDSchemeIdentifiers.Odette))
            {
                retval.IsValid = false;
                retval.Messages.Add($"Global identifier scheme {descriptor.Buyer?.GlobalID?.SchemeID} is not supported for sellers in ZUGFeRD 1.0");
            }

            if (!EnumExtensions.In<GlobalIDSchemeIdentifiers>(descriptor.ShipFrom?.GlobalID?.SchemeID, GlobalIDSchemeIdentifiers.Swift, GlobalIDSchemeIdentifiers.DUNS, GlobalIDSchemeIdentifiers.GLN, GlobalIDSchemeIdentifiers.EAN, GlobalIDSchemeIdentifiers.Odette))
            {
                retval.IsValid = false;
                retval.Messages.Add($"Global identifier scheme {descriptor.Buyer?.GlobalID?.SchemeID} is not supported for senders (ShipFrom) in ZUGFeRD 1.0");
            }

            if (!EnumExtensions.In<GlobalIDSchemeIdentifiers>(descriptor.ShipTo?.GlobalID?.SchemeID, GlobalIDSchemeIdentifiers.Swift, GlobalIDSchemeIdentifiers.DUNS, GlobalIDSchemeIdentifiers.GLN, GlobalIDSchemeIdentifiers.EAN, GlobalIDSchemeIdentifiers.Odette))
            {
                retval.IsValid = false;
                retval.Messages.Add($"Global identifier scheme {descriptor.Buyer?.GlobalID?.SchemeID} is not supported for recipients (ShipTo) in ZUGFeRD 1.0");
            }

            return retval;
        } // !_ValidateAccordingToVersion1()
    }
}
