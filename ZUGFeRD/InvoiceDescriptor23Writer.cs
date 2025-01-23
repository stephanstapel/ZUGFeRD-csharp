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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;


namespace s2industries.ZUGFeRD
{
    internal class InvoiceDescriptor23Writer : IInvoiceDescriptorWriter
    {
        private readonly Profile ALL_PROFILES = Profile.Minimum | Profile.BasicWL | Profile.Basic | Profile.Comfort | Profile.Extended | Profile.XRechnung1 | Profile.XRechnung;


        /// <summary>
        /// Saves the given invoice to the given stream.
        /// Make sure that the stream is open and writeable. Otherwise, an IllegalStreamException will be thron.
        /// </summary>
        /// <param name="descriptor">The invoice object that should be saved</param>
        /// <param name="stream">The target stream for saving the invoice</param>
        /// <param name="format">Format of the target file</param>
        public override void Save(InvoiceDescriptor descriptor, Stream stream, ZUGFeRDFormats format = ZUGFeRDFormats.CII)
        {
            IInvoiceDescriptorWriter writer = null;

            if (format == ZUGFeRDFormats.CII)
            {
                writer = new InvoiceDescriptor23CIIWriter();
            }
            else if ((format == ZUGFeRDFormats.UBL) && (descriptor.Profile == Profile.XRechnung))
            {
                writer = new InvoiceDescriptor22UBLWriter();
            }

            if (writer == null)
            {
                throw new UnsupportedException($"Profile {descriptor.Profile.EnumToString()} and format {format.EnumToString()} is not supported.");
            }

            writer.Save(descriptor, stream, format);
        } // !Save()


        internal override bool Validate(InvoiceDescriptor descriptor, bool throwExceptions = true)
        {
            if (descriptor.Profile == Profile.BasicWL)
            {
                if (throwExceptions) { throw new UnsupportedException("Invalid profile used for ZUGFeRD 2.x invoice."); }
                return false;
            }

            if (descriptor.Profile != Profile.Extended) // check tax types, only extended profile allows tax types other than vat
            {
                if (!descriptor.GetTradeLineItems().All(l => l.TaxType.Equals(TaxTypes.VAT) || l.TaxType.Equals(TaxTypes.Unknown)))
                {
                    if (throwExceptions) { throw new UnsupportedException("Tax types other than VAT only possible with extended profile."); }
                    return false;
                }
            }

            if ((descriptor.Profile == Profile.XRechnung) || (descriptor.Profile == Profile.XRechnung1))
            {
                if (descriptor.Seller != null)
                {
                    if (descriptor.SellerContact == null)
                    {
                        if (throwExceptions) { throw new MissingDataException("Seller contact (BG-6) required when seller is set (BR-DE-2)."); }
                        return false;
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(descriptor.SellerContact.EmailAddress))
                        {
                            if (throwExceptions) { throw new MissingDataException("Seller contact email address (BT-43) is required (BR-DE-7)."); }
                            return false;
                        }
                        if (String.IsNullOrWhiteSpace(descriptor.SellerContact.PhoneNo))
                        {
                            if (throwExceptions) { throw new MissingDataException("Seller contact phone no (BT-42) is required (BR-DE-6)."); }
                            return false;
                        }
                        if (String.IsNullOrWhiteSpace(descriptor.SellerContact.Name) && String.IsNullOrWhiteSpace(descriptor.SellerContact.OrgUnit))
                        {
                            if (throwExceptions) { throw new MissingDataException("Seller contact point (name or org unit) no (BT-41) is required (BR-DE-5)."); }
                            return false;
                        }
                    }
                }


                // BR-DE-17
                if (!new[] { InvoiceType.PartialInvoice, InvoiceType.Invoice, InvoiceType.Correction, InvoiceType.SelfBilledInvoice, InvoiceType.CreditNote,
                             InvoiceType.PartialConstructionInvoice, InvoiceType.PartialFinalConstructionInvoice, InvoiceType.FinalConstructionInvoice}.Contains(descriptor.Type))
                {
                    throw new UnsupportedException("Invoice type (BT-3) does not match requirements of BR-DE-17");
                }
            }

            return true;
        } // !Validate()
    }
}
