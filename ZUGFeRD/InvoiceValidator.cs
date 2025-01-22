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
        public static void ValidateAndPrint(InvoiceDescriptor descriptor, string filename = null)
        {
            List<string> output = Validate(descriptor);

            if (!String.IsNullOrWhiteSpace(filename))
            {
                System.IO.File.WriteAllText(filename, string.Join("\n", output));
            }

            foreach (string line in output)
            {
                System.Console.WriteLine(line);
            }
        } // !ValidateAndPrint()


        public static List<string> Validate(InvoiceDescriptor descriptor)
        {
            List<string> retval = new List<string>();
            if (descriptor == null)
            {
                retval.Add("Invalid invoice descriptor");
                return retval;
            }

            // line item summation
            retval.Add("Validating invoice monetary summation");
            retval.Add(String.Format("Starting recalculating line total from {0} items...", descriptor.GetTradeLineItems().Count));
            int lineCounter = 0;

            decimal lineTotal = 0m;
            Dictionary<decimal, decimal> lineTotalPerTax = new Dictionary<decimal, decimal>();
            foreach (TradeLineItem item in descriptor.GetTradeLineItems())
            {
                decimal total = 0m;
                if (item.NetUnitPrice.HasValue)
                {
                    total = decimal.Multiply(item.NetUnitPrice.Value, item.BilledQuantity);
                    lineTotal += total;
                }

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

                retval.Add(String.Format("{0};{1};{2}", ++lineCounter, item.Name, total));
            }

            retval.Add("==> DONE!");
            retval.Add("Finished recalculating monetarySummation.lineTotal...");
            retval.Add("Adding tax amounts from invoice allowance charge...");

            decimal allowanceTotal = 0.0m;
            decimal chargeTotal = 0.0m;
            foreach (TradeAllowanceCharge charge in descriptor.GetTradeAllowanceCharges())
            {
                retval.Add(String.Format("==> added {0:0.00} to {1:0.00}%", -charge.Amount, charge.Tax.Percent));

                if (!lineTotalPerTax.ContainsKey(charge.Tax.Percent))
                {
                    lineTotalPerTax.Add(charge.Tax.Percent, new decimal());
                }
                lineTotalPerTax[charge.Tax.Percent] -= charge.Amount;

                if (charge.ChargeIndicator == false)
                {
                    allowanceTotal += charge.Amount;
                }
                else
                {
                    chargeTotal += charge.Amount;
                }
            }

            retval.Add("Adding tax amounts from invoice service charge...");
            // TODO

            // TODO ausgeben: Recalculating tax basis for tax percentages: [Key{percentage=7.00, code=[VAT] Value added tax, category=[S] Standard rate}, Key{percentage=19.00, code=[VAT] Value added tax, category=[S] Standard rate}]
            retval.Add(String.Format("Recalculated tax basis = {0:0.0000}", lineTotal - allowanceTotal));
            retval.Add("Calculating tax total...");

            decimal taxTotal = 0.0m;
            foreach (KeyValuePair<decimal, decimal> kv in lineTotalPerTax)
            {
                decimal taxTotalForLine = Decimal.Divide(Decimal.Multiply(kv.Value, kv.Key), 100.0m);
                taxTotal += taxTotalForLine;
                retval.Add(String.Format("===> {0:0.0000} x {1:0.00}% = {2:0.00}", kv.Value, kv.Key, taxTotalForLine));
            }

            decimal grandTotal = lineTotal - allowanceTotal + taxTotal + chargeTotal;
            decimal prepaid = 0m; // TODO: calculcate

            retval.Add(String.Format("Recalculated tax total = {0:0.00}", taxTotal));
            retval.Add(String.Format("Recalculated grand total = {0:0.0000} EUR(tax basis total + tax total)", grandTotal));
            retval.Add("Recalculating invoice monetary summation DONE!");
            retval.Add(String.Format("==> result: MonetarySummation[lineTotal = {0:0.0000} EUR, chargeTotal = {1:0.0000} EUR, allowanceTotal = {2:0.0000} EUR, taxBasisTotal = {3:0.0000} EUR, taxTotal = {4:0.0000} EUR, grandTotal = {5:0.0000} EUR, totalPrepaid = {6:0.0000} EUR, duePayable = {7:0.0000} EUR]",
                                     lineTotal,
                                     chargeTotal,
                                     allowanceTotal,
                                     lineTotal - allowanceTotal + chargeTotal, // tax basis total
                                     taxTotal,
                                     grandTotal,
                                     prepaid,
                                     lineTotal - allowanceTotal + taxTotal + chargeTotal + prepaid
                                     ));


            decimal taxBasisTotal = 0m;
            foreach (Tax tax in descriptor.GetApplicableTradeTaxes())
            {
                taxBasisTotal += tax.BasisAmount;
            }

            decimal allowanceTotalSummedPerTradeAllowanceCharge = 0m;
            decimal chargesTotalSummedPerTradeAllowanceCharge = 0m;
            foreach (TradeAllowanceCharge allowance in descriptor.GetTradeAllowanceCharges())
            {
                if (allowance.ChargeIndicator == true)
                {
                    chargesTotalSummedPerTradeAllowanceCharge += allowance.ActualAmount;
                }
                else
                {
                    allowanceTotalSummedPerTradeAllowanceCharge += allowance.ActualAmount;
                }
            }

            if (!descriptor.TaxTotalAmount.HasValue)
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.taxTotal Message: Kein TaxTotalAmount vorhanden"));
            }
            else if (Math.Abs(taxTotal - descriptor.TaxTotalAmount.Value) < 0.01m)
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.taxTotal Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", taxTotal));
            }
            else
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.taxTotal Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", taxTotal, descriptor.TaxTotalAmount));
            }

            if (Math.Abs(lineTotal - descriptor.LineTotalAmount.Value) < 0.01m)
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.lineTotal Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", lineTotal));
            }
            else
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.lineTotal Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", lineTotal, descriptor.LineTotalAmount));
            }

            if (!descriptor.GrandTotalAmount.HasValue)
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.grandTotal Message: Kein GrandTotalAmount vorhanden"));
            }
            else if (Math.Abs(grandTotal - descriptor.GrandTotalAmount.Value) < 0.01m)
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.grandTotal Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", grandTotal));
            }
            else
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.grandTotal Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", grandTotal, descriptor.GrandTotalAmount));
            }

            /*
             * @todo Richtige Validierung implementieren
             */
            if (Math.Abs(taxBasisTotal - taxBasisTotal) < 0.01m)
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.taxBasisTotal Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", taxBasisTotal));
            }
            else
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.taxBasisTotal Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", taxBasisTotal, taxBasisTotal));
            }

            if (Math.Abs(allowanceTotalSummedPerTradeAllowanceCharge - allowanceTotal) < 0.01m)
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.allowanceTotal  Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", allowanceTotalSummedPerTradeAllowanceCharge));
            }
            else
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.allowanceTotal  Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", allowanceTotalSummedPerTradeAllowanceCharge, allowanceTotal));
            }

            if (Math.Abs(chargesTotalSummedPerTradeAllowanceCharge - chargeTotal) < 0.01m)
            { 
                retval.Add(String.Format("trade.settlement.monetarySummation.chargeTotal  Message: Berechneter Wert ist wie vorhanden:[{0:0.0000}]", chargesTotalSummedPerTradeAllowanceCharge));
            }
            else
            {
                retval.Add(String.Format("trade.settlement.monetarySummation.chargeTotal  Message: Berechneter Wert ist[{0:0.0000}] aber tatsächliche vorhander Wert ist[{1:0.0000}] | Actual value: {1:0.0000})", chargesTotalSummedPerTradeAllowanceCharge, chargeTotal));
            }

            return retval;
        } // !Validate()
    }
}
