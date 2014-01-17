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
    public enum InvoiceType
    {
        Unknown = 0,
        Invoice = 380,
        Correction = 1380,
        CreditNote = 381,
        DebitNote = 383,
        SelfBilledInvoice = 389
    }


    internal static class InvoiceTypeExtensions
    {
        public static InvoiceType FromString(this InvoiceType _type, string s)
        {
            switch (s) 
            {
                case "380": return InvoiceType.Invoice;
                case "1380": return InvoiceType.Correction;
                case "381": return InvoiceType.CreditNote;
                case "383": return InvoiceType.DebitNote;
                case "389": return InvoiceType.SelfBilledInvoice;
            }
            return InvoiceType.Unknown;
        } // !FromString()
    }
}
