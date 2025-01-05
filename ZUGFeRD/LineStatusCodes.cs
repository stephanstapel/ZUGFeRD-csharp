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
using System.Text;

namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Codelist UNTDID 1229
    /// https://service.unece.org/trade/untdid/d14a/tred/tred1229.htm
    ///
    /// Used in BT-X-7
    /// </summary>
    public enum LineStatusCodes
    {
        /// <summary>
        /// Unknown/ invalid line status code
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The information is to be or has been added.
        /// </summary>
        Added = 1,

        /// <summary>
        /// The information is to be or has been deleted.
        /// </summary>
        Deleted = 2,

        /// <summary>
        /// The information is to be or has been changed.
        /// </summary>
        Changed = 3,

        /// <summary>
        /// No action
        ///
        /// This line item is not affected by the actual message.
        /// </summary>
        NoAction = 4,

        /// <summary>
        /// Accepted without amendment
        ///
        /// This line item is entirely accepted by the seller.
        /// </summary>
        AcceptedWithoutAmendment = 5,

        /// <summary>
        /// Accepted with amendment
        ///
        /// This line item is accepted but amended by the seller.
        /// </summary>
        AcceptedWithAmendment = 6,

        /// <summary>
        /// Not accepted
        ///
        /// This line item is not accepted by the seller.
        /// </summary>
        NotAccepted = 7,

        /// <summary>
        /// Schedule only
        ///
        /// Code specifying that the message is a schedule only.
        /// </summary>
        ScheduleOnly = 8,

        /// <summary>
        /// Code specifying that amendments are requested/notified.
        /// </summary>
        Amendments = 9,

        /// <summary>
        /// Not found
        ///
        /// This line item is not found in the referenced message.
        /// </summary>
        NotFound = 10,

        /// <summary>
        /// Not amended
        ///
        /// This line is not amended by the buyer.
        /// </summary>
        NotAmended = 11,

        /// <summary>
        /// Line item numbers changed
        ///
        /// Code specifying that the line item numbers have changed.
        /// </summary>
        LineItemNumbersChanged = 12,

        /// <summary>
        /// Buyer has deducted amount
        ///
        /// Buyer has deducted amount from payment.
        /// </summary>
        BuyerHasDeductedAmount = 13,

        /// <summary>
        /// Buyer claims against invoice
        ///
        /// Buyer has a claim against an outstanding invoice.
        /// </summary>
        BuyerClaimsAgainstInvoice = 14,

        /// <summary>
        /// Charge back by seller
        ///
        /// Factor has been requested to charge back the outstanding item.
        /// </summary>
        ChargeBackBySeller = 15,

        /// <summary>
        /// Seller will issue credit note
        ///
        /// Seller agrees to issue a credit note.
        /// </summary>
        SellerWillIssueCreditNote = 16,

        /// <summary>
        /// Terms changed for new terms
        ///
        /// New settlement terms have been agreed.
        /// </summary>
        TermsChangedForNewTerms = 17,

        /// <summary>
        /// Abide outcome of negotiations
        ///
        /// Factor agrees to abide by the outcome of negotiations between seller and buyer.
        /// </summary>
        AbideOutcomeOfNegotiations = 18,

        /// <summary>
        /// Seller rejects dispute
        ///
        /// Seller does not accept validity of dispute.
        /// </summary>
        SellerRejectsDispute = 19,

        /// <summary>
        /// The reported situation is settled.
        /// </summary>
        Settlement = 20,

        /// <summary>
        /// No delivery
        ///
        /// Code indicating that no delivery will be required.
        /// </summary>
        NoDelivery = 21,

        /// <summary>
        /// Call-off delivery
        ///
        /// A request for delivery of a particular quantity of goods to be delivered on a particular date (or within a particular period).
        /// </summary>
        CallOffDelivery = 22,

        /// <summary>
        /// Proposed amendment
        ///
        /// A code used to indicate an amendment suggested by the sender.
        /// </summary>
        ProposedAmendment = 23,

        /// <summary>
        /// Accepted with amendment, no confirmation required
        ///
        /// Accepted with changes which require no confirmation.
        /// </summary>
        AcceptedWithAmendmentNoConfirmationRequired = 24,

        /// <summary>
        /// Equipment provisionally repaired
        ///
        /// The equipment or component has been provisionally repaired.
        /// </summary>
        EquipmentProvisionallyRepaired = 25,

        /// <summary>
        /// Code indicating that the entity is included.
        /// </summary>
        Included = 26,

        /// <summary>
        /// Request for information.
        /// </summary>
        Inquiry = 34,

        /// <summary>
        /// Checked.
        /// </summary>
        Checked = 35,

        /// <summary>
        /// Not checked.
        /// </summary>
        NotChecked = 36,

        /// <summary>
        /// Discontinued.
        /// </summary>
        Cancelled = 37,

        /// <summary>
        /// Provide a replacement.
        /// </summary>
        Replaced = 38,

        /// <summary>
        /// Not existing before.
        /// </summary>
        New = 39,

        /// <summary>
        /// Consent.
        /// </summary>
        Agreed = 40,

        /// <summary>
        /// Put forward for consideration.
        /// </summary>
        Proposed = 41,

        /// <summary>
        /// Already delivered
        ///
        /// Delivery has taken place.
        /// </summary>
        AlreadyDelivered = 42,

        /// <summary>
        /// Additional subordinate structures will follow
        ///
        /// Additional subordinate structures will follow the current hierarchy level.
        /// </summary>
        AdditionalSubordinateStructuresWillFollow = 43,

        /// <summary>
        /// Additional subordinate structures will not follow
        ///
        /// No additional subordinate structures will follow the current hierarchy level.
        /// </summary>
        AdditionalSubordinateStructuresWillNotFollow = 44,

        /// <summary>
        /// Result opposed
        ///
        /// A notification that the result is opposed.
        /// </summary>
        ResultOpposed = 45,

        /// <summary>
        /// Auction held
        ///
        /// A notification that an auction was held.
        /// </summary>
        AuctionHeld = 46,

        /// <summary>
        /// Legal action pursued
        ///
        /// A notification that legal action has been pursued.
        /// </summary>
        LegalActionPursued = 47,

        /// <summary>
        /// Meeting held
        ///
        /// A notification that a meeting was held.
        /// </summary>
        MeetingHeld = 48,

        /// <summary>
        /// Result set aside
        ///
        /// A notification that the result has been set aside.
        /// </summary>
        ResultSetAside = 49,

        /// <summary>
        /// Result disputed
        ///
        /// A notification that the result has been disputed.
        /// </summary>
        ResultDisputed = 50,

        /// <summary>
        /// A notification that a countersuit has been filed.
        /// </summary>
        Countersued = 51,

        /// <summary>
        /// A notification that an action is awaiting settlement.
        /// </summary>
        Pending = 52,

        /// <summary>
        /// Court action dismissed
        ///
        /// A notification that a court action will no longer be heard.
        /// </summary>
        CourtActionDismissed = 53,

        /// <summary>
        /// Referred item, accepted
        ///
        /// The item being referred to has been accepted.
        /// </summary>
        ReferredItemAccepted = 54,

        /// <summary>
        /// Referred item, rejected
        ///
        /// The item being referred to has been rejected.
        /// </summary>
        ReferredItemRejected = 55,

        /// <summary>
        /// Debit advice statement line
        ///
        /// Notification that the statement line is a debit advice.
        /// </summary>
        DebitAdviceStatementLine = 56,

        /// <summary>
        /// Credit advice statement line
        ///
        /// Notification that the statement line is a credit advice.
        /// </summary>
        CreditAdviceStatementLine = 57,

        /// <summary>
        /// Grouped credit advices
        ///
        /// Notification that the credit advices are grouped.
        /// </summary>
        GroupedCreditAdvices = 58,

        /// <summary>
        /// Grouped debit advices
        ///
        /// Notification that the debit advices are grouped.
        /// </summary>
        GroupedDebitAdvices = 59,

        /// <summary>
        /// The name is registered.
        /// </summary>
        Registered = 60,

        /// <summary>
        /// Payment denied
        ///
        /// The payment has been denied.
        /// </summary>
        PaymentDenied = 61,

        /// <summary>
        /// Approved as amended
        ///
        /// Approved with modifications.
        /// </summary>
        ApprovedAsAmended = 62,

        /// <summary>
        /// Approved as submitted
        ///
        /// The request has been approved as submitted.
        /// </summary>
        ApprovedAsSubmitted = 63,

        /// <summary>
        /// Cancelled, no activity
        ///
        /// Cancelled due to the lack of activity.
        /// </summary>
        CancelledNoActivity = 64,

        /// <summary>
        /// Under investigation
        ///
        /// Investigation is being done.
        /// </summary>
        UnderInvestigation = 65,

        /// <summary>
        /// Initial claim received
        ///
        /// Notification that the initial claim was received.
        /// </summary>
        InitialClaimReceived = 66,

        /// <summary>
        /// Not in process.
        /// </summary>
        NotInProcess = 67,

        /// <summary>
        /// Rejected, duplicate
        ///
        /// Rejected because it is a duplicate.
        /// </summary>
        RejectedDuplicate = 68,

        /// <summary>
        /// Rejected, resubmit with corrections
        ///
        /// Rejected but may be resubmitted when corrected.
        /// </summary>
        RejectedResubmitWithCorrections = 69,

        /// <summary>
        /// Pending, incomplete
        ///
        /// Pending because of incomplete information.
        /// </summary>
        PendingIncomplete = 70,

        /// <summary>
        /// Under field office investigation
        ///
        /// Investigation by the field is being done.
        /// </summary>
        UnderFieldOfficeInvestigation = 71,

        /// <summary>
        /// Pending, awaiting additional material
        ///
        /// Pending awaiting receipt of additional material.
        /// </summary>
        PendingAwaitingAdditionalMaterial = 72,

        /// <summary>
        /// Pending, awaiting review
        ///
        /// Pending while awaiting review.
        /// </summary>
        PendingAwaitingReview = 73,

        /// <summary>
        /// Opened again.
        /// </summary>
        Reopened = 74,

        /// <summary>
        /// Processed by primary, forwarded to additional payer(s)
        ///
        /// This request has been processed by the primary payer and sent to additional payer(s).
        /// </summary>
        ProcessedByPrimaryForwardedToAdditionalPayers = 75,

        /// <summary>
        /// Processed by secondary, forwarded to additional payer(s)
        ///
        /// This request has been processed by the secondary payer and sent to additional payer(s).
        /// </summary>
        ProcessedBySecondaryForwardedToAdditionalPayers = 76,

        /// <summary>
        /// Processed by tertiary, forwarded to additional payer(s)
        ///
        /// This request has been processed by the tertiary payer and sent to additional payer(s).
        /// </summary>
        ProcessedByTertiaryForwardedToAdditionalPayers = 77,

        /// <summary>
        /// Previous payment decision reversed
        ///
        /// A previous payment decision has been reversed.
        /// </summary>
        PreviousPaymentDecisionReversed = 78,

        /// <summary>
        /// Not our claim, forwarded to another payer(s)
        ///
        /// A request does not belong to this payer but has been forwarded to another payer(s).
        /// </summary>
        NotOurClaimForwardedToAnotherPayers = 79,

        /// <summary>
        /// Transferred to correct insurance carrier
        ///
        /// The request has been transferred to the correct insurance carrier for processing.
        /// </summary>
        TransferredToCorrectInsuranceCarrier = 80,

        /// <summary>
        /// Not paid, predetermination pricing only
        ///
        /// Payment has not been made and the enclosed response is predetermination pricing only.
        /// </summary>
        NotPaidPredeterminationPricingOnly = 81,

        /// <summary>
        /// Documentation claim
        ///
        /// The claim is for documentation purposes only, no payment required.
        /// </summary>
        DocumentationClaim = 82,

        /// <summary>
        /// Assessed.
        /// </summary>
        Reviewed = 83,

        /// <summary>
        /// This price was changed.
        /// </summary>
        Repriced = 84,

        /// <summary>
        /// An official examination has occurred.
        /// </summary>
        Audited = 85,

        /// <summary>
        /// Conditionally paid
        ///
        /// Payment has been conditionally made.
        /// </summary>
        ConditionallyPaid = 86,

        /// <summary>
        /// On appeal
        ///
        /// Reconsideration of the decision has been applied for.
        /// </summary>
        OnAppeal = 87,

        /// <summary>
        /// Shut.
        /// </summary>
        Closed = 88,

        /// <summary>
        /// A subsequent official examination has occurred.
        /// </summary>
        Reaudited = 89,

        /// <summary>
        /// Issued again.
        /// </summary>
        Reissued = 90,

        /// <summary>
        /// Closed after reopening
        ///
        /// Reopened and then closed.
        /// </summary>
        ClosedAfterReopening = 91,

        /// <summary>
        /// Determined again or differently.
        /// </summary>
        Redetermined = 92,

        /// <summary>
        /// Processed as primary
        ///
        /// Processed as the first.
        /// </summary>
        ProcessedAsPrimary = 93,

        /// <summary>
        /// Processed as secondary
        ///
        /// Processed as the second.
        /// </summary>
        ProcessedAsSecondary = 94,

        /// <summary>
        ///  Processed as tertiary
        ///
        /// Processed as the third.
        /// </summary>
        ProcessedAsTertiary = 95,

        /// <summary>
        /// Correction of error
        ///
        /// A correction to information previously communicated which contained an error.
        /// </summary>
        CorrectionOfError = 96,

        /// <summary>
        /// Single credit item of a group
        ///
        /// Notification that the credit item is a single credit item of a group of credit items.
        /// </summary>
        SingleCreditItemOfAGroup = 97,

        /// <summary>
        /// Single debit item of a group
        ///
        /// Notification that the debit item is a single debit item of a group of debit items.
        /// </summary>
        SingleDebitItemOfAGroup = 98,

        /// <summary>
        /// Interim response
        ///
        /// The response is an interim one.
        /// </summary>
        InterimResponse = 99,

        /// <summary>
        /// Final response
        ///
        /// The response is a final one.
        /// </summary>
        FinalResponse = 100,

        /// <summary>
        /// Debit advice requested
        ///
        /// A debit advice is requested for the transaction.
        /// </summary>
        DebitAdviceRequested = 101,

        /// <summary>
        /// Transaction not impacted
        ///
        /// Advice that the transaction is not impacted.
        /// </summary>
        TransactionNotImpacted = 102,

        /// <summary>
        /// Patient to be notified
        ///
        /// The action to take is to notify the patient.
        /// </summary>
        PatientToBeNotified = 103,

        /// <summary>
        /// Healthcare provider to be notified
        ///
        /// The action to take is to notify the healthcare provider.
        /// </summary>
        HealthcareProviderToBeNotified = 104,

        /// <summary>
        /// Usual general practitioner to be notified
        ///
        /// The action to take is to notify the usual general practitioner.
        /// </summary>
        UsualGeneralPractitionerToBeNotified = 105,

        /// <summary>
        /// Advice without details
        ///
        /// An advice without details is requested or notified.
        /// </summary>
        AdviceWithoutDetails = 106,

        /// <summary>
        /// Advice with details
        ///
        /// An advice with details is requested or notified.
        /// </summary>
        AdviceWithDetails = 107,

        /// <summary>
        /// Amendment requested
        ///
        /// An amendment is requested.
        /// </summary>
        AmendmentRequested = 108,

        /// <summary>
        /// For information
        ///
        /// Included for information only.
        /// </summary>
        ForInformation = 109,

        /// <summary>
        /// A code indicating discontinuance or retraction.
        /// </summary>
        Withdraw = 110,

        /// <summary>
        /// Delivery date change
        ///
        /// The action / notification is a change of the delivery date.
        /// </summary>
        DeliveryDateChange = 111,

        /// <summary>
        /// Quantity change
        ///
        /// The action / notification is a change of quantity.
        /// </summary>
        QuantityChange = 112,

        /// <summary>
        /// Resale and claim
        ///
        /// The identified items have been sold by the distributor to the end customer, and compensation for the loss of inventory value is claimed.
        /// </summary>
        ResaleAndClaim = 113,

        /// <summary>
        /// Resale
        ///
        /// The identified items have been sold by the distributor to the end customer.
        /// </summary>
        Resale = 114,

        /// <summary>
        /// Prior addition
        ///
        /// This existing line item becomes available at an earlier date.
        /// </summary>
        PriorAddition = 115,

        /// <summary>
        /// This line has expired.
        /// </summary>
        Expired = 116,

        /// <summary>
        /// This line is on Hold.
        /// </summary>
        Hold = 117,

        /// <summary>
        /// This line is open.
        /// </summary>
        Open = 118
    }

    internal static class LineStatusCodesExtensions
    {
        public static LineStatusCodes? FromString(this LineStatusCodes _, string s)
        {
            if (s == null)
                return null;
            return EnumExtensions.StringToEnum<LineStatusCodes>(s);
        } // !FromString()

        public static string EnumValueToString(this LineStatusCodes t)
        {
            return EnumExtensions.EnumToInt(t).ToString();
        } // !ToString()

        public static string EnumToString(this LineStatusCodes c)
        {
            return EnumExtensions.EnumToString<LineStatusCodes>(c);
        } // !ToString()
    }
}
