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
using OfficeOpenXml;
using s2industries.ZUGFeRD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZUGFeRDToExcel
{
    public class InvoiceConverter
    {
        internal class PosColumns
        {
            public static readonly string DESCRIPTION = "A";
            public static readonly string SELLER_ASSIGNED_ID = "B";
            public static readonly string NET_UNIT_PRICE = "C";
            public static readonly string GROSS_UNIT_PRICE = "D";
            public static readonly string QUANTITY = "E";
            public static readonly string LINE_TOTAL_AMOUNT = "F";
            public static readonly string TRADE_ALLOWANCE_CHARGE_0 = "G";
            public static readonly string TRADE_ALLOWANCE_CHARGE_1 = "H";
            public static readonly string TRADE_ALLOWANCE_CHARGE_2 = "I";
            public static readonly string TRADE_ALLOWANCE_CHARGE_TOTAL = "J";
            public static readonly string ADDITIONAL_REFERENCE = "K";
        }

        internal class HeadColumns
        {
            public static readonly string DESCRIPTION = "A";
            public static readonly string VALUE = "B";
            public static readonly string ANALYSIS = "C";
            public static readonly string EXPLANATION = "D";
        }        



        public static void ConvertZUGFeRDToExcel(string inputPath, string outputPath)
        {
            InvoiceDescriptor desc = InvoiceDescriptor.Load(inputPath);
            ConvertZUGFeRDToExcel(desc, outputPath);
        } // !ConvertZUGFeRDToExcel()


        public static void ConvertZUGFeRDToExcel(InvoiceDescriptor descriptor, string outputPath)
        {

            // create/ open  Excel file
            ExcelPackage pck = new ExcelPackage(new FileInfo(outputPath));

            // make sure that our Excel file is clean
            for (int j = 1; j <= pck.Workbook.Worksheets.Count;)
            {
                if (pck.Workbook.Worksheets[j].Name.Equals("Positions"))
                {
                    pck.Workbook.Worksheets.Delete(j);
                }
                else if (pck.Workbook.Worksheets[j].Name.Equals("Head"))
                {
                    pck.Workbook.Worksheets.Delete(j);
                }
                else
                {
                    j++;
                }
            }

            ExcelWorksheet positionWorksheet = pck.Workbook.Worksheets.Add("Positions");

            new ExcelCell(positionWorksheet, PosColumns.DESCRIPTION, 1).setText("Name").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.SELLER_ASSIGNED_ID, 1).setText("Seller assigned Id").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.NET_UNIT_PRICE, 1).setText("Net Unit Price").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.GROSS_UNIT_PRICE, 1).setText("Gross Unit Price").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.QUANTITY, 1).setText("Billed Quantity").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.LINE_TOTAL_AMOUNT, 1).setText("Line Total Amount").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_0, 1).setText("Trade Allowance Charge per Unit (0)").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_1, 1).setText("Trade Allowance Charge per Unit (1)").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_2, 1).setText("Trade Allowance Charge per Unit (2)").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_TOTAL, 1).setText("Trade Allowance Charge Total").setBold().setBorderBottom();
            new ExcelCell(positionWorksheet, PosColumns.ADDITIONAL_REFERENCE, 1).setText("Reference").setBold().setBorderBottom();

            string cellForLineTotal = "";

            int i = 2;
            foreach (TradeLineItem lineItem in descriptor.TradeLineItems)
            {
                if (!String.IsNullOrEmpty(lineItem.Name))
                {
                    new ExcelCell(positionWorksheet, PosColumns.DESCRIPTION, i).setText(lineItem.Name);
                }
                else if ((lineItem.AssociatedDocument != null) && (lineItem.AssociatedDocument.Notes.Count > 0))
                {
                    new ExcelCell(positionWorksheet, PosColumns.DESCRIPTION, i).setText(lineItem.AssociatedDocument.Notes[0].Content);
                }
                new ExcelCell(positionWorksheet, PosColumns.SELLER_ASSIGNED_ID, i).setText(lineItem.SellerAssignedID);
                new ExcelCell(positionWorksheet, PosColumns.NET_UNIT_PRICE, i).setValue(lineItem.NetUnitPrice, "0.00");
                new ExcelCell(positionWorksheet, PosColumns.GROSS_UNIT_PRICE, i).setValue(lineItem.GrossUnitPrice, "0.00");
                new ExcelCell(positionWorksheet, PosColumns.QUANTITY, i).setValue(lineItem.BilledQuantity, "0.00");
                if (lineItem.LineTotalAmount.HasValue)
                {
                    new ExcelCell(positionWorksheet, PosColumns.LINE_TOTAL_AMOUNT, i).setValue(lineItem.LineTotalAmount.Value, "0.00");
                }

                foreach (AdditionalReferencedDocument ard in lineItem.AdditionalReferencedDocuments)
                {
                    if (ard.ReferenceTypeCode == ReferenceTypeCodes.IV)
                    {
                        new ExcelCell(positionWorksheet, PosColumns.ADDITIONAL_REFERENCE, i).setText(ard.ID);
                    }
                }


                new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_0, i).setValue(0m, "0.00").formatWithDecimals();
                new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_1, i).setValue(0m, "0.00").formatWithDecimals();
                new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_2, i).setValue(0m, "0.00").formatWithDecimals();
                for (int j = 0; j < lineItem.TradeAllowanceCharges.Count; j++)
                {
                    TradeAllowanceCharge tac = lineItem.TradeAllowanceCharges[j];
                    string column = PosColumns.TRADE_ALLOWANCE_CHARGE_0;
                    if (j == 1)
                    {
                        column = PosColumns.TRADE_ALLOWANCE_CHARGE_1;
                    }
                    else if (j == 2)
                    {
                        column = PosColumns.TRADE_ALLOWANCE_CHARGE_2;
                    }
                    if (tac.ChargeIndicator == false) // Allowance
                    {
                        new ExcelCell(positionWorksheet, column, i).setValue(-tac.ActualAmount, "0.00");
                    }
                    else
                    {
                        new ExcelCell(positionWorksheet, column, i).setValue(tac.ActualAmount, "0.00");
                    }
                }

                new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_TOTAL, i).setFormula(String.Format("=SUM({0}{3}:{1}{3})*{2}{3}", PosColumns.TRADE_ALLOWANCE_CHARGE_0, PosColumns.TRADE_ALLOWANCE_CHARGE_2, PosColumns.QUANTITY, i))
                    .formatWithDecimals()
                    .setColor(ExcelColors.Green);

                i += 1;
            }

            i += 2;
            ExcelCell cell = new ExcelCell(positionWorksheet, PosColumns.LINE_TOTAL_AMOUNT, i).setFormula(String.Format("=sum({0}2:{0}{1})", PosColumns.LINE_TOTAL_AMOUNT, i - 3)).setColor(ExcelColors.Green).formatWithDecimals();
            cellForLineTotal = cell.getCellAddress();

            new ExcelCell(positionWorksheet, PosColumns.TRADE_ALLOWANCE_CHARGE_TOTAL, i + 2).setFormula(String.Format("=sum({0}{1}:{0}{2})", PosColumns.TRADE_ALLOWANCE_CHARGE_TOTAL, 2, i)).setColor(ExcelColors.Green).formatWithDecimals();


            positionWorksheet.Cells[positionWorksheet.Dimension.Address].AutoFilter = true;
            positionWorksheet.View.FreezePanes(2, 1);
            positionWorksheet.Cells[positionWorksheet.Dimension.Address].AutoFitColumns();

            // ---- head area ----
            ExcelWorksheet headWorksheet = pck.Workbook.Worksheets.Add("Head");


            i = 1;

            // output allowance charges
            List<string> cellsForAllowanceChargesPerVAT = new List<string>();
            string cellForAllowanceAnalysis = "";
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Allowances and charges").setBold().joinColumns("B").setAlignCenter();

            i += 1;
            foreach (TradeAllowanceCharge tac in descriptor.TradeAllowanceCharges)
            {
                new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Base amount");
                new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(tac.BasisAmount, "0.00");

                i += 1;
                if (tac.ChargeIndicator == false)
                {
                    new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Allowance amount");
                    new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(-tac.ActualAmount, "0.00");
                }
                else
                {
                    new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Charge amount");
                    new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(tac.ActualAmount, "0.00");
                }
                new ExcelCell(headWorksheet, HeadColumns.EXPLANATION, i).setText("Negative value indicates allowance (discount), position value indicates (sur)charge").setItalic();

                cellForAllowanceAnalysis = String.Format("{0}{1}", HeadColumns.ANALYSIS, i); // placeholder for later
                cellsForAllowanceChargesPerVAT.Add(String.Format("{0}{1}", HeadColumns.VALUE, i));

                i += 1;

                new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Tax percent");
                new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(tac.Tax.Percent, "0.00");

                i += 1;
            }

            // analysis of allowance charges
            string _calculation = "=";
            foreach(string _cell in cellsForAllowanceChargesPerVAT) { _calculation += _cell + "+"; }
            _calculation = _calculation.Substring(0, _calculation.Length - 1);
            new ExcelCell(headWorksheet, cellForAllowanceAnalysis).setFormula(_calculation).formatWithDecimals().setColor(ExcelColors.Green);

            i += 1;

            // output taxes
            List<string> cellsForTaxes = new List<string>();
            string cellForTaxAnalysis = "";
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Tax").setBold().joinColumns("B").setAlignCenter();
            i += 1;

            foreach (Tax tax in descriptor.Taxes)
            {
                new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Base Amount");
                new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(tax.BasisAmount, "0.00");

                i += 1;
                new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Tax percent");
                new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(tax.Percent, "0.00");

                i += 1;
                new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Tax amount");
                new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(tax.TaxAmount, "0.00");

                cellsForTaxes.Add(String.Format("{0}{1}", HeadColumns.VALUE, i));
                cellForTaxAnalysis = String.Format("{0}{1}", HeadColumns.ANALYSIS, i);

                i += 1;
            }

            // analysis of taxes
            _calculation = "=";
            foreach(string _cell in cellsForTaxes)
            {
                _calculation += _cell + "+";
            }
            _calculation = _calculation.Substring(0, _calculation.Length - 1);
            new ExcelCell(headWorksheet, cellForTaxAnalysis).setFormula(_calculation).formatWithDecimals().setColor(ExcelColors.Green);

            i += 1;
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Totals").setBold().joinColumns("B").setAlignCenter();

            i += 1;
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Line total amount");
            new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(descriptor.LineTotalAmount, "0.00");
            string cellForLineTotalAmount = new ExcelCell(headWorksheet, HeadColumns.ANALYSIS, i).setFormula(String.Format("={0}", cellForLineTotal)).formatWithDecimals().setColor(ExcelColors.Green).getCellAddress();

            i += 1;
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Charge total amount");
            new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(descriptor.ChargeTotalAmount.Value, "0.00");

            i += 1;
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Allowance total amount");
            new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(descriptor.AllowanceTotalAmount.Value, "0.00");
            string cellForAllowanceTotal = new ExcelCell(headWorksheet, HeadColumns.ANALYSIS, i).setFormula(String.Format("={0}", cellForAllowanceAnalysis)).formatWithDecimals().setColor(ExcelColors.Green).getCellAddress();

            i += 1;
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Tax basis amount");
            new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(descriptor.TaxBasisAmount.Value, "0.00");
            string cellForTaxBasisAmount = new ExcelCell(headWorksheet, HeadColumns.ANALYSIS, i).setFormula(String.Format("={0}+{1}", cellForLineTotalAmount, cellForAllowanceTotal)).formatWithDecimals().setColor(ExcelColors.Green).getCellAddress();

            i += 1;
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Tax total amount");
            new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(descriptor.TaxTotalAmount, "0.00");
            string cellForTaxTotalAmount = new ExcelCell(headWorksheet, HeadColumns.ANALYSIS, i).setFormula(String.Format("={0}", cellForTaxAnalysis)).formatWithDecimals().setColor(ExcelColors.Green).getCellAddress();

            i += 1;
            new ExcelCell(headWorksheet, HeadColumns.DESCRIPTION, i).setText("Grand total amount");
            new ExcelCell(headWorksheet, HeadColumns.VALUE, i).setValue(descriptor.GrandTotalAmount, "0.00");
            new ExcelCell(headWorksheet, HeadColumns.ANALYSIS, i).setFormula(String.Format("={0}+{1}", cellForTaxBasisAmount, cellForTaxTotalAmount)).formatWithDecimals().setColor(ExcelColors.Green).getCellAddress();

            headWorksheet.Cells[headWorksheet.Dimension.Address].AutoFitColumns();

            pck.Save();
        } // !ConvertZUGFeRDToExcel()
    }
}
