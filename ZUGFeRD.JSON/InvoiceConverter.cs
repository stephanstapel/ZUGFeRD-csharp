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
using System.IO;
using System.Text;
using System.Text.Json;

namespace s2industries.ZUGFeRD.JSON
{
    /// <summary>
    /// Converts ZUGFeRD / Factur-X / XRechnung invoice descriptors to JSON format.
    /// </summary>
    public class InvoiceConverter
    {
        /// <summary>
        /// Converts an invoice XML file to a JSON file.
        /// </summary>
        /// <param name="inputPath">Path to the source XRechnung/ZUGFeRD XML file.</param>
        /// <param name="outputPath">Path to the target JSON output file.</param>
        public static void ToJson(string inputPath, string outputPath)
        {
            InvoiceDescriptor descriptor = InvoiceDescriptor.Load(inputPath);
            ToJson(descriptor, outputPath);
        } // !ToJson()


        /// <summary>
        /// Converts an <see cref="InvoiceDescriptor"/> to a JSON file.
        /// </summary>
        /// <param name="descriptor">The invoice descriptor to convert.</param>
        /// <param name="outputPath">Path to the target JSON output file.</param>
        public static void ToJson(InvoiceDescriptor descriptor, string outputPath)
        {
            string json = ToJsonString(descriptor);
            File.WriteAllText(outputPath, json, new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        } // !ToJson()


        /// <summary>
        /// Converts an <see cref="InvoiceDescriptor"/> to a JSON string.
        /// </summary>
        /// <param name="descriptor">The invoice descriptor to convert.</param>
        /// <returns>A formatted JSON string representing the invoice.</returns>
        public static string ToJsonString(InvoiceDescriptor descriptor)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                ToJsonStream(descriptor, stream);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        } // !ToJsonString()


        /// <summary>
        /// Converts an <see cref="InvoiceDescriptor"/> and writes the JSON representation to the given stream.
        /// </summary>
        /// <param name="descriptor">The invoice descriptor to convert.</param>
        /// <param name="outputStream">The stream to write the JSON output to.</param>
        public static void ToJsonStream(InvoiceDescriptor descriptor, Stream outputStream)
        {
            JsonWriterOptions options = new JsonWriterOptions { Indented = true };
            using (Utf8JsonWriter writer = new Utf8JsonWriter(outputStream, options))
            {
                writer.WriteStartObject();

                _writeInvoiceHeader(writer, descriptor);
                _writeParty(writer, "seller", descriptor.Seller, descriptor.SellerContact, descriptor.SellerTaxRegistration);
                _writeParty(writer, "buyer", descriptor.Buyer, descriptor.BuyerContact, descriptor.BuyerTaxRegistration);
                _writeLineItems(writer, descriptor);
                _writeAllowanceCharges(writer, descriptor);
                _writeTaxes(writer, descriptor);
                _writeTotals(writer, descriptor);
                _writePaymentMeans(writer, descriptor);

                writer.WriteEndObject();
                writer.Flush();
            }
        } // !ToJsonStream()


        private static void _writeInvoiceHeader(Utf8JsonWriter writer, InvoiceDescriptor descriptor)
        {
            writer.WriteStartObject("invoice");

            _writeStringIfNotEmpty(writer, "invoiceNo", descriptor.InvoiceNo);
            _writeDateIfSet(writer, "invoiceDate", descriptor.InvoiceDate);
            writer.WriteString("type", descriptor.Type.ToString());
            writer.WriteString("currency", descriptor.Currency.ToString());
            writer.WriteString("profile", descriptor.Profile.ToString());
            _writeStringIfNotEmpty(writer, "businessProcess", descriptor.BusinessProcess);
            _writeStringIfNotEmpty(writer, "name", descriptor.Name);
            _writeStringIfNotEmpty(writer, "orderNo", descriptor.OrderNo);
            _writeDateIfSet(writer, "orderDate", descriptor.OrderDate);
            _writeStringIfNotEmpty(writer, "referenceOrderNo", descriptor.ReferenceOrderNo);
            _writeDateIfSet(writer, "actualDeliveryDate", descriptor.ActualDeliveryDate);
            _writeStringIfNotEmpty(writer, "paymentReference", descriptor.PaymentReference);

            if (descriptor.ContractReferencedDocument != null &&
                (!String.IsNullOrEmpty(descriptor.ContractReferencedDocument.ID) || descriptor.ContractReferencedDocument.IssueDateTime.HasValue))
            {
                writer.WriteStartObject("contractReference");
                _writeStringIfNotEmpty(writer, "id", descriptor.ContractReferencedDocument.ID);
                _writeDateIfSet(writer, "date", descriptor.ContractReferencedDocument.IssueDateTime);
                writer.WriteEndObject();
            }

            if (descriptor.Notes != null && descriptor.Notes.Count > 0)
            {
                writer.WriteStartArray("notes");
                foreach (Note note in descriptor.Notes)
                {
                    writer.WriteStartObject();
                    _writeStringIfNotEmpty(writer, "content", note.Content);
                    _writeStringIfNotEmpty(writer, "subjectCode", note.SubjectCode.ToString());
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        } // !_writeInvoiceHeader()


        private static void _writeParty(Utf8JsonWriter writer, string propertyName, Party party, Contact contact, List<TaxRegistration> taxRegistrations)
        {
            if (party == null)
            {
                return;
            }

            writer.WriteStartObject(propertyName);

            _writeStringIfNotEmpty(writer, "name", party.Name);
            _writeStringIfNotEmpty(writer, "description", party.Description);
            _writeStringIfNotEmpty(writer, "contactName", party.ContactName);

            writer.WriteStartObject("address");
            _writeStringIfNotEmpty(writer, "street", party.Street);
            _writeStringIfNotEmpty(writer, "street2", party.Street2);
            _writeStringIfNotEmpty(writer, "addressLine3", party.AddressLine3);
            _writeStringIfNotEmpty(writer, "postcode", party.Postcode);
            _writeStringIfNotEmpty(writer, "city", party.City);
            if (party.Country.HasValue)
            {
                writer.WriteString("country", party.Country.ToString());
            }
            _writeStringIfNotEmpty(writer, "countrySubdivision", party.CountrySubdivisionName);
            writer.WriteEndObject();

            if (contact != null)
            {
                writer.WriteStartObject("contact");
                _writeStringIfNotEmpty(writer, "name", contact.Name);
                _writeStringIfNotEmpty(writer, "orgUnit", contact.OrgUnit);
                _writeStringIfNotEmpty(writer, "emailAddress", contact.EmailAddress);
                _writeStringIfNotEmpty(writer, "phoneNo", contact.PhoneNo);
                _writeStringIfNotEmpty(writer, "faxNo", contact.FaxNo);
                writer.WriteEndObject();
            }

            if (taxRegistrations != null && taxRegistrations.Count > 0)
            {
                writer.WriteStartArray("taxRegistrations");
                foreach (TaxRegistration tr in taxRegistrations)
                {
                    writer.WriteStartObject();
                    _writeStringIfNotEmpty(writer, "no", tr.No);
                    writer.WriteString("schemeId", tr.SchemeID.ToString());
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            if (party.SpecifiedLegalOrganization != null &&
                (!String.IsNullOrEmpty(party.SpecifiedLegalOrganization.ID?.ID) || !String.IsNullOrEmpty(party.SpecifiedLegalOrganization.TradingBusinessName)))
            {
                writer.WriteStartObject("legalOrganization");
                _writeStringIfNotEmpty(writer, "id", party.SpecifiedLegalOrganization.ID?.ID);
                _writeStringIfNotEmpty(writer, "tradingBusinessName", party.SpecifiedLegalOrganization.TradingBusinessName);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        } // !_writeParty()


        private static void _writeLineItems(Utf8JsonWriter writer, InvoiceDescriptor descriptor)
        {
            IList<TradeLineItem> lineItems = descriptor.GetTradeLineItems();
            if (lineItems == null || lineItems.Count == 0)
            {
                return;
            }

            writer.WriteStartArray("lineItems");
            foreach (TradeLineItem lineItem in lineItems)
            {
                writer.WriteStartObject();

                _writeStringIfNotEmpty(writer, "lineId", lineItem.AssociatedDocument?.LineID);
                _writeStringIfNotEmpty(writer, "name", lineItem.Name);
                _writeStringIfNotEmpty(writer, "description", lineItem.Description);
                _writeStringIfNotEmpty(writer, "sellerAssignedId", lineItem.SellerAssignedID);
                _writeStringIfNotEmpty(writer, "buyerAssignedId", lineItem.BuyerAssignedID);

                writer.WriteNumber("billedQuantity", lineItem.BilledQuantity);
                if (lineItem.UnitCode.HasValue)
                {
                    writer.WriteString("unitCode", lineItem.UnitCode.ToString());
                }
                writer.WriteNumber("netUnitPrice", lineItem.NetUnitPrice);
                if (lineItem.GrossUnitPrice.HasValue)
                {
                    writer.WriteNumber("grossUnitPrice", lineItem.GrossUnitPrice.Value);
                }
                if (lineItem.LineTotalAmount.HasValue)
                {
                    writer.WriteNumber("lineTotalAmount", lineItem.LineTotalAmount.Value);
                }

                writer.WriteStartObject("tax");
                if (lineItem.TaxType.HasValue)
                {
                    writer.WriteString("typeCode", lineItem.TaxType.ToString());
                }
                if (lineItem.TaxCategoryCode.HasValue)
                {
                    writer.WriteString("categoryCode", lineItem.TaxCategoryCode.ToString());
                }
                writer.WriteNumber("percent", lineItem.TaxPercent);
                writer.WriteEndObject();

                _writeDateIfSet(writer, "billingPeriodStart", lineItem.BillingPeriodStart);
                _writeDateIfSet(writer, "billingPeriodEnd", lineItem.BillingPeriodEnd);

                IList<AbstractTradeAllowanceCharge> allowanceCharges = lineItem.GetTradeAllowanceCharges();
                if (allowanceCharges != null && allowanceCharges.Count > 0)
                {
                    writer.WriteStartArray("allowanceCharges");
                    foreach (AbstractTradeAllowanceCharge tac in allowanceCharges)
                    {
                        writer.WriteStartObject();
                        writer.WriteBoolean("isCharge", tac.ChargeIndicator);
                        if (tac.BasisAmount.HasValue)
                        {
                            writer.WriteNumber("basisAmount", tac.BasisAmount.Value);
                        }
                        writer.WriteNumber("actualAmount", tac.ActualAmount);
                        if (tac.Tax != null && tac.Tax.Percent != 0)
                        {
                            writer.WriteNumber("taxPercent", tac.Tax.Percent);
                        }
                        _writeStringIfNotEmpty(writer, "reason", tac.Reason);
                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                }

                if (lineItem.AdditionalReferencedDocuments != null && lineItem.AdditionalReferencedDocuments.Count > 0)
                {
                    writer.WriteStartArray("additionalReferences");
                    foreach (AdditionalReferencedDocument ard in lineItem.AdditionalReferencedDocuments)
                    {
                        writer.WriteStartObject();
                        _writeStringIfNotEmpty(writer, "id", ard.ID);
                        if (ard.ReferenceTypeCode.HasValue)
                        {
                            writer.WriteString("referenceTypeCode", ard.ReferenceTypeCode.ToString());
                        }
                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                }

                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        } // !_writeLineItems()


        private static void _writeAllowanceCharges(Utf8JsonWriter writer, InvoiceDescriptor descriptor)
        {
            IList<AbstractTradeAllowanceCharge> allowanceCharges = descriptor.TradeAllowanceCharges;
            if (allowanceCharges == null || allowanceCharges.Count == 0)
            {
                return;
            }

            writer.WriteStartArray("allowanceCharges");
            foreach (AbstractTradeAllowanceCharge tac in allowanceCharges)
            {
                writer.WriteStartObject();
                writer.WriteBoolean("isCharge", tac.ChargeIndicator);
                if (tac.BasisAmount.HasValue)
                {
                    writer.WriteNumber("basisAmount", tac.BasisAmount.Value);
                }
                writer.WriteNumber("actualAmount", tac.ActualAmount);
                if (tac.Tax != null)
                {
                    writer.WriteStartObject("tax");
                    if (tac.Tax.TypeCode.HasValue)
                    {
                        writer.WriteString("typeCode", tac.Tax.TypeCode.ToString());
                    }
                    if (tac.Tax.CategoryCode.HasValue)
                    {
                        writer.WriteString("categoryCode", tac.Tax.CategoryCode.ToString());
                    }
                    writer.WriteNumber("percent", tac.Tax.Percent);
                    writer.WriteEndObject();
                }
                _writeStringIfNotEmpty(writer, "reason", tac.Reason);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        } // !_writeAllowanceCharges()


        private static void _writeTaxes(Utf8JsonWriter writer, InvoiceDescriptor descriptor)
        {
            IList<Tax> taxes = descriptor.GetApplicableTradeTaxes();
            if (taxes == null || taxes.Count == 0)
            {
                return;
            }

            writer.WriteStartArray("taxes");
            foreach (Tax tax in taxes)
            {
                writer.WriteStartObject();
                if (tax.TypeCode.HasValue)
                {
                    writer.WriteString("typeCode", tax.TypeCode.ToString());
                }
                if (tax.CategoryCode.HasValue)
                {
                    writer.WriteString("categoryCode", tax.CategoryCode.ToString());
                }
                writer.WriteNumber("percent", tax.Percent);
                writer.WriteNumber("basisAmount", tax.BasisAmount);
                writer.WriteNumber("taxAmount", tax.TaxAmount);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        } // !_writeTaxes()


        private static void _writeTotals(Utf8JsonWriter writer, InvoiceDescriptor descriptor)
        {
            writer.WriteStartObject("totals");

            if (descriptor.LineTotalAmount.HasValue)
            {
                writer.WriteNumber("lineTotalAmount", descriptor.LineTotalAmount.Value);
            }
            if (descriptor.ChargeTotalAmount.HasValue)
            {
                writer.WriteNumber("chargeTotalAmount", descriptor.ChargeTotalAmount.Value);
            }
            if (descriptor.AllowanceTotalAmount.HasValue)
            {
                writer.WriteNumber("allowanceTotalAmount", descriptor.AllowanceTotalAmount.Value);
            }
            if (descriptor.TaxBasisAmount.HasValue)
            {
                writer.WriteNumber("taxBasisAmount", descriptor.TaxBasisAmount.Value);
            }
            if (descriptor.TaxTotalAmount.HasValue)
            {
                writer.WriteNumber("taxTotalAmount", descriptor.TaxTotalAmount.Value);
            }
            if (descriptor.GrandTotalAmount.HasValue)
            {
                writer.WriteNumber("grandTotalAmount", descriptor.GrandTotalAmount.Value);
            }
            if (descriptor.TotalPrepaidAmount.HasValue)
            {
                writer.WriteNumber("totalPrepaidAmount", descriptor.TotalPrepaidAmount.Value);
            }
            if (descriptor.RoundingAmount.HasValue)
            {
                writer.WriteNumber("roundingAmount", descriptor.RoundingAmount.Value);
            }
            if (descriptor.DuePayableAmount.HasValue)
            {
                writer.WriteNumber("duePayableAmount", descriptor.DuePayableAmount.Value);
            }

            writer.WriteEndObject();
        } // !_writeTotals()


        private static void _writePaymentMeans(Utf8JsonWriter writer, InvoiceDescriptor descriptor)
        {
            if (descriptor.PaymentMeans == null && descriptor.CreditorBankAccounts.Count == 0)
            {
                return;
            }

            writer.WriteStartObject("paymentMeans");

            if (descriptor.PaymentMeans != null)
            {
                writer.WriteString("typeCode", descriptor.PaymentMeans.TypeCode.ToString());
                _writeStringIfNotEmpty(writer, "information", descriptor.PaymentMeans.Information);
                _writeStringIfNotEmpty(writer, "sepaCreditorIdentifier", descriptor.PaymentMeans.SEPACreditorIdentifier);
                _writeStringIfNotEmpty(writer, "sepaMandateReference", descriptor.PaymentMeans.SEPAMandateReference);
            }

            if (descriptor.CreditorBankAccounts != null && descriptor.CreditorBankAccounts.Count > 0)
            {
                writer.WriteStartArray("creditorBankAccounts");
                foreach (BankAccount account in descriptor.CreditorBankAccounts)
                {
                    writer.WriteStartObject();
                    _writeStringIfNotEmpty(writer, "iban", account.IBAN);
                    _writeStringIfNotEmpty(writer, "bic", account.BIC);
                    _writeStringIfNotEmpty(writer, "name", account.Name);
                    _writeStringIfNotEmpty(writer, "bankleitzahl", account.Bankleitzahl);
                    _writeStringIfNotEmpty(writer, "bankName", account.BankName);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            if (descriptor.PaymentTerms != null && descriptor.PaymentTerms.Count > 0)
            {
                writer.WriteStartArray("paymentTerms");
                foreach (PaymentTerms terms in descriptor.PaymentTerms)
                {
                    writer.WriteStartObject();
                    _writeStringIfNotEmpty(writer, "description", terms.Description);
                    _writeDateIfSet(writer, "dueDate", terms.DueDate);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        } // !_writePaymentMeans()


        private static void _writeStringIfNotEmpty(Utf8JsonWriter writer, string propertyName, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                writer.WriteString(propertyName, value);
            }
        } // !_writeStringIfNotEmpty()


        private static void _writeDateIfSet(Utf8JsonWriter writer, string propertyName, DateTime? date)
        {
            if (date.HasValue)
            {
                writer.WriteString(propertyName, date.Value.ToString("yyyy-MM-dd"));
            }
        } // !_writeDateIfSet()
    }
}
