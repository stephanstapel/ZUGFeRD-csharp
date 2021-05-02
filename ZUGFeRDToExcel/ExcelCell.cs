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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZUGFeRDToExcel
{
    public class ExcelCell
    {
        private ExcelWorksheet _worksheet;
        private string _cell;
        private string _column;
        private int _row;


        public ExcelCell(ExcelWorksheet worksheet, string column, int row)
        {
            this._worksheet = worksheet;
            this._column = column;
            this._row = row;
            this._cell = String.Format("{0}{1}", column, row);
        }


        public ExcelCell(ExcelWorksheet worksheet, string cell)
        {
            this._worksheet = worksheet;
            string _rowValue = Regex.Match(cell, @"\d+").Value;
            this._row = Int32.Parse(_rowValue);
            this._column = cell.Replace(_rowValue, "");
            this._cell = cell;
        }


        public ExcelCell setText(string text)
        {
            if (text == null)
            {
                text = "";
            }

            // see if we need to convert the input text            
            if (text.Contains(","))
            {
                float f;
                if (float.TryParse(text, out f))
                {
                    this._worksheet.Cells[this._cell].Value = f;
                    return this;
                }
            }
            else
            {
                int i;
                if (Int32.TryParse(text, out i))
                {
                    this._worksheet.Cells[this._cell].Value = i;
                    return this;
                }
            }

            this._worksheet.Cells[this._cell].Value = text;
            return this;
        } // !setText()


        public ExcelCell setFormula(string formula, bool isArrayFormula = false)
        {
            if (isArrayFormula)
            {
                this._worksheet.Cells[this._cell].CreateArrayFormula(formula);
            }
            else
            {
                this._worksheet.Cells[this._cell].Formula = formula;
            }
            return this;
        } // !setFormula()


        internal ExcelCell setValue(decimal? value, string formatString)
        {
            if (!value.HasValue)
            {
                ExcelCell retval = this.setText("");
                return retval;
            }
            else
            {
                ExcelCell retval = this.setText(value.Value.ToString(formatString));
                if (formatString.Contains(".#") || formatString.Contains(".0"))
                {
                    retval.formatWithDecimals();
                }
                return retval;
            }
        } // !setValue()


        public ExcelCell setIndent(int indent = 5)
        {
            this._worksheet.Cells[this._cell].Style.Indent = indent;
            return this;
        }


        public ExcelCell formatWithDecimals()
        {
            this._worksheet.Cells[this._cell].Style.Numberformat.Format = "#,##0.00";
            return this;
        }


        public ExcelCell setBold(bool isBold = true)
        {
            this._worksheet.Cells[this._cell].Style.Font.Bold = isBold;
            return this;
        }


        public ExcelCell setItalic(bool isItalic = true)
        {
            this._worksheet.Cells[this._cell].Style.Font.Italic = isItalic;
            return this;
        }        


        public ExcelCell setFontSize(int fontSize = 10)
        {
            this._worksheet.Cells[this._cell].Style.Font.Size = fontSize;
            return this;
        }


        public ExcelCell setAlignRight()
        {
            this._worksheet.Cells[this._cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
            return this;
        }


        public ExcelCell setAlignCenter()
        {
            this._worksheet.Cells[this._cell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            return this;
        } // !setAlignCenter()


        public ExcelCell setBorderBottom()
        {
            return setBorder(top: false, right: false, bottom: true, left: false);
        }


        public ExcelCell setBorderTop()
        {
            return setBorder(top: true, right: false, bottom: false, left: false);
        }


        public ExcelCell setBorder(bool top = true, bool right = true, bool bottom = true, bool left = true)
        {
            if (top)
            {
                this._worksheet.Cells[this._cell].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
            if (right)
            {
                this._worksheet.Cells[this._cell].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
            if (bottom)
            {
                this._worksheet.Cells[this._cell].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
            if (left)
            {
                this._worksheet.Cells[this._cell].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }
            return this;
        } // !setBorder()


        public string getCellAddress()
        {
            return String.Format("{0}!{1}", this._worksheet.Name, this._worksheet.Cells[this._cell].Address);
        } // !getCellAddress()


        public ExcelCell setColor(ExcelColors color)
        {
            this._worksheet.Cells[this._cell].Style.Font.Color.SetColor(_translateColor(color));
            return this;
        }


        public ExcelCell setBackground(ExcelColors color)
        {
            this._worksheet.Cells[this._cell].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            this._worksheet.Cells[this._cell].Style.Fill.BackgroundColor.SetColor(_translateColor(color));
            return this;
        }


        public ExcelCell joinColumns(string untilColumn)
        {
            this._worksheet.Cells[this._cell + ":" + String.Format("{0}{1}", untilColumn, this._row)].Merge = true;
            return this;
        } // !joinColumns()


        private Color _translateColor(ExcelColors color)
        {
            switch (color)
            {
                case ExcelColors.Yellow:
                    {
                        return System.Drawing.Color.FromArgb(255, 192, 0);
                    }
                case ExcelColors.Green:
                    {
                        return System.Drawing.Color.FromArgb(0, 128, 0);
                    }
            }

            return System.Drawing.Color.FromArgb(255, 255, 255);
        } // !_translateColor()
    }
}
