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
        [EnumStringValue("0")]
        Unknown = 0,

        /// <summary>
        /// The information is to be or has been added.
        /// </summary>
        [EnumStringValue("1")]
        Added = 1,

        /// <summary>
        /// The information is to be or has been deleted.
        /// </summary>
        [EnumStringValue("2")]
        Deleted = 2,

        /// <summary>
        /// The information is to be or has been changed.
        /// </summary>
        [EnumStringValue("3")]
        Changed = 3,

        /// <summary>
        /// No action
        /// 
        /// This line item is not affected by the actual message.
        /// </summary>
        [EnumStringValue("4")]
        NoAction = 4,

        /// <summary>
        /// Accepted without amendment
        /// 
        /// This line item is entirely accepted by the seller.
        /// </summary>
        [EnumStringValue("5")]
        AcceptedWithoutAmendment = 5,

        /// <summary>
        /// Accepted with amendment
        /// 
        /// This line item is accepted but amended by the seller.
        /// </summary>
        [EnumStringValue("6")]
        AcceptedWithAmendment = 6,

        /// <summary>
        /// Not accepted
        /// 
        /// This line item is not accepted by the seller.
        /// </summary>
        [EnumStringValue("7")]
        NotAccepted = 7,

        /// <summary>
        /// Schedule only
        /// 
        /// Code specifying that the message is a schedule only.
        /// </summary>
        [EnumStringValue("8")]
        ScheduleOnly = 8,

        /// <summary>
        /// Code specifying that amendments are requested/notified.
        /// </summary>
        [EnumStringValue("9")]
        Amendments = 9,

        /// <summary>
        /// Not found
        /// 
        /// This line item is not found in the referenced message.
        /// </summary>
        [EnumStringValue("10")]
        NotFound = 10,

        /// <summary>
        /// Not amended
        /// 
        /// This line is not amended by the buyer.
        /// </summary>
        [EnumStringValue("11")]
        NotAmended = 11,

        /// <summary>
        /// Line item numbers changed
        /// 
        /// Code specifying that the line item numbers have changed.
        /// </summary>
        [EnumStringValue("12")]
        LineItemNumbersChanged = 12,

        /// <summary>
        /// Buyer has deducted amount
        /// 
        /// Buyer has deducted amount from payment.
        /// </summary>
        [EnumStringValue("13")]
        BuyerHasDeductedAmount = 13,

        /// <summary>
        /// Buyer claims against invoice
        /// 
        /// Buyer has a claim against an outstanding invoice.
        /// </summary>
        [EnumStringValue("14")]
        BuyerClaimsAgainstInvoice = 14,

        /// <summary>
        /// Charge back by seller
        /// 
        /// Factor has been requested to charge back the outstanding item.
        /// </summary>
        [EnumStringValue("15")]
        ChargeBackBySeller = 15,

        /// <summary>
        /// Seller will issue credit note
        /// 
        /// Seller agrees to issue a credit note.
        /// </summary>
        [EnumStringValue("16")]
        SellerWillIssueCreditNote = 16,

        /// <summary>
        /// Terms changed for new terms
        /// 
        /// New settlement terms have been agreed.
        /// </summary>
        [EnumStringValue("17")]
        TermsChangedForNewTerms = 17,

        /// <summary>
        /// Abide outcome of negotiations
        /// 
        /// Factor agrees to abide by the outcome of negotiations between seller and buyer.
        /// </summary>
        [EnumStringValue("18")]
        AbideOutcomeOfNegotiations = 18,

        /// <summary>
        /// Seller rejects dispute
        /// 
        /// Seller does not accept validity of dispute.
        /// </summary>
        [EnumStringValue("19")]
        SellerRejectsDispute = 19,

        /// <summary>
        /// The reported situation is settled.
        /// </summary>
        [EnumStringValue("20")]
        Settlement = 20,

        /// <summary>
        /// No delivery
        /// 
        /// Code indicating that no delivery will be required.
        /// </summary>
        [EnumStringValue("21")]
        NoDelivery = 21,

        /// <summary>
        /// Call-off delivery
        /// 
        /// A request for delivery of a particular quantity of goods to be delivered on a particular date (or within a particular period).
        /// </summary>
        [EnumStringValue("22")]
        CallOffDelivery = 22,

        /// <summary>
        /// Proposed amendment
        /// 
        /// A code used to indicate an amendment suggested by the sender.
        /// </summary>
        [EnumStringValue("23")]
        ProposedAmendment = 23,

        /// <summary>
        /// Accepted with amendment, no confirmation required
        /// 
        /// Accepted with changes which require no confirmation.
        /// </summary>
        [EnumStringValue("24")]
        AcceptedWithAmendmentNoConfirmationRequired = 24,

        /// <summary>
        /// Equipment provisionally repaired
        /// 
        /// The equipment or component has been provisionally repaired.
        /// </summary>
        [EnumStringValue("25")]
        EquipmentProvisionallyRepaired = 25,

        /// <summary>
        /// Code indicating that the entity is included.
        /// </summary>
        [EnumStringValue("26")]
        Included = 26,

        /// <summary>
        /// Request for information.
        /// </summary>
        [EnumStringValue("34")]
        Inquiry = 34,

        /// <summary>
        /// Checked.
        /// </summary>
        [EnumStringValue("35")]
        Checked = 35,

        /// <summary>
        /// Not checked.
        /// </summary>
        [EnumStringValue("36")]
        NotChecked = 36,

        /// <summary>
        /// Discontinued.
        /// </summary>
        [EnumStringValue("37")]
        Cancelled = 37,

        /// <summary>
        /// Provide a replacement.
        /// </summary>
        [EnumStringValue("38")]
        Replaced = 38,

        /// <summary>
        /// Not existing before.
        /// </summary>
        [EnumStringValue("39")]
        New = 39,

        /// <summary>
        /// Consent.
        /// </summary>
        [EnumStringValue("40")]
        Agreed = 40,

        /// <summary>
        /// Put forward for consideration.
        /// </summary>
        [EnumStringValue("41")]
        Proposed = 41,

        /// <summary>
        /// Already delivered
        /// 
        /// Delivery has taken place.
        /// </summary>
        [EnumStringValue("42")]
        AlreadyDelivered = 42,

        /// <summary>
        /// Additional subordinate structures will follow
        /// 
        /// Additional subordinate structures will follow the current hierarchy level.
        /// </summary>
        [EnumStringValue("43")]
        AdditionalSubordinateStructuresWillFollow = 43,

        /// <summary>
        /// Additional subordinate structures will not follow
        /// 
        /// No additional subordinate structures will follow the current hierarchy level.
        /// </summary>
        [EnumStringValue("44")]
        AdditionalSubordinateStructuresWillNotFollow = 44,

        /// <summary>
        /// Result opposed
        /// 
        /// A notification that the result is opposed.
        /// </summary>
        [EnumStringValue("45")]
        ResultOpposed = 45,

        /// <summary>
        /// Auction held
        /// 
        /// A notification that an auction was held.
        /// </summary>
        [EnumStringValue("46")]
        AuctionHeld = 46,

        /// <summary>
        /// Legal action pursued
        /// 
        /// A notification that legal action has been pursued.
        /// </summary>
        [EnumStringValue("47")]
        LegalActionPursued = 47,

        /// <summary>
        /// Meeting held
        /// 
        /// A notification that a meeting was held.
        /// </summary>
        [EnumStringValue("48")]
        MeetingHeld = 48,

        /// <summary>
        /// Result set aside
        /// 
        /// A notification that the result has been set aside.
        /// </summary>
        [EnumStringValue("49")]
        ResultSetAside = 49,

        /// <summary>
        /// Result disputed
        /// 
        /// A notification that the result has been disputed.
        /// </summary>
        [EnumStringValue("50")]
        ResultDisputed = 50,

        /// <summary>
        /// A notification that a countersuit has been filed.
        /// </summary>
        [EnumStringValue("51")]
        Countersued = 51,

        /// <summary>
        /// A notification that an action is awaiting settlement.
        /// </summary>
        [EnumStringValue("52")]
        Pending = 52,

        /// <summary>
        /// Court action dismissed
        /// 
        /// A notification that a court action will no longer be heard.
        /// </summary>
        [EnumStringValue("53")]
        CourtActionDismissed = 53,

        /// <summary>
        /// Referred item, accepted
        /// 
        /// The item being referred to has been accepted.
        /// </summary>
        [EnumStringValue("54")]
        ReferredItemAccepted = 54,

        /// <summary>
        /// Referred item, rejected
        /// 
        /// The item being referred to has been rejected.
        /// </summary>
        [EnumStringValue("55")]
        ReferredItemRejected = 55,

        /// <summary>
        /// Debit advice statement line
        /// 
        /// Notification that the statement line is a debit advice.
        /// </summary>
        [EnumStringValue("56")]
        DebitAdviceStatementLine = 56,

        /// <summary>
        /// Credit advice statement line
        /// 
        /// Notification that the statement line is a credit advice.
        /// </summary>
        [EnumStringValue("57")]
        CreditAdviceStatementLine = 57,

        /// <summary>
        /// Grouped credit advices
        /// 
        /// Notification that the credit advices are grouped.
        /// </summary>
        [EnumStringValue("58")]
        GroupedCreditAdvices = 58,

        /// <summary>
        /// Grouped debit advices
        /// 
        /// Notification that the debit advices are grouped.
        /// </summary>
        [EnumStringValue("59")]
        GroupedDebitAdvices = 59,

        /// <summary>
        /// The name is registered.
        /// </summary>
        [EnumStringValue("60")]
        Registered = 60,

        /// <summary>
        /// Payment denied
        /// 
        /// The payment has been denied.
        /// </summary>
        [EnumStringValue("61")]
        PaymentDenied = 61,

        /// <summary>
        /// Approved as amended
        /// 
        /// Approved with modifications.
        /// </summary>
        [EnumStringValue("62")]
        ApprovedAsAmended = 62,

        /// <summary>
        /// Approved as submitted
        /// 
        /// The request has been approved as submitted.
        /// </summary>
        [EnumStringValue("63")]
        ApprovedAsSubmitted = 63,

        /// <summary>
        /// Cancelled, no activity
        /// 
        /// Cancelled due to the lack of activity.
        /// </summary>
        [EnumStringValue("64")]
        CancelledNoActivity = 64,

        /// <summary>
        /// Under investigation
        /// 
        /// Investigation is being done.
        /// </summary>
        [EnumStringValue("65")]
        UnderInvestigation = 65,

        /// <summary>
        /// Initial claim received
        /// 
        /// Notification that the initial claim was received.
        /// </summary>
        [EnumStringValue("66")]
        InitialClaimReceived = 66,

        /// <summary>
        /// Not in process.
        /// </summary>
        [EnumStringValue("67")]
        NotInProcess = 67,

        /// <summary>
        /// Rejected, duplicate
        /// 
        /// Rejected because it is a duplicate.
        /// </summary>
        [EnumStringValue("68")]
        RejectedDuplicate = 68,

        /// <summary>
        /// Rejected, resubmit with corrections
        /// 
        /// Rejected but may be resubmitted when corrected.
        /// </summary>
        [EnumStringValue("69")]
        RejectedResubmitWithCorrections = 69,

        /// <summary>
        /// Pending, incomplete
        /// 
        /// Pending because of incomplete information.
        /// </summary>
        [EnumStringValue("70")]
        PendingIncomplete = 70,

        /// <summary>
        /// Under field office investigation
        /// 
        /// Investigation by the field is being done.
        /// </summary>
        [EnumStringValue("71")]
        UnderFieldOfficeInvestigation = 71,

        /// <summary>
        /// Pending, awaiting additional material
        /// 
        /// Pending awaiting receipt of additional material.
        /// </summary>
        [EnumStringValue("72")]
        PendingAwaitingAdditionalMaterial = 72,

        /// <summary>
        /// Pending, awaiting review
        /// 
        /// Pending while awaiting review.
        /// </summary>
        [EnumStringValue("73")]
        PendingAwaitingReview = 73,

        /// <summary>
        /// Opened again.
        /// </summary>
        [EnumStringValue("74")]
        Reopened = 74,

        /// <summary>
        /// Processed by primary, forwarded to additional payer(s)
        /// 
        /// This request has been processed by the primary payer and sent to additional payer(s).
        /// </summary>
        [EnumStringValue("75")]
        ProcessedByPrimaryForwardedToAdditionalPayers = 75,

        /// <summary>
        /// Processed by secondary, forwarded to additional payer(s)
        /// 
        /// This request has been processed by the secondary payer and sent to additional payer(s).
        /// </summary>
        [EnumStringValue("76")]
        ProcessedBySecondaryForwardedToAdditionalPayers = 76,

        /// <summary>
        /// Processed by tertiary, forwarded to additional payer(s)
        /// 
        /// This request has been processed by the tertiary payer and sent to additional payer(s).
        /// </summary>
        [EnumStringValue("77")]
        ProcessedByTertiaryForwardedToAdditionalPayers = 77,

        /// <summary>
        /// Previous payment decision reversed
        /// 
        /// A previous payment decision has been reversed.
        /// </summary>
        [EnumStringValue("78")]
        PreviousPaymentDecisionReversed = 78,

        /// <summary>
        /// Not our claim, forwarded to another payer(s)
        /// 
        /// A request does not belong to this payer but has been forwarded to another payer(s).
        /// </summary>
        [EnumStringValue("79")]
        NotOurClaimForwardedToAnotherPayers = 79,

        /// <summary>
        /// Transferred to correct insurance carrier
        /// 
        /// The request has been transferred to the correct insurance carrier for processing.
        /// </summary>
        [EnumStringValue("80")]
        TransferredToCorrectInsuranceCarrier = 80,

        /// <summary>
        /// Not paid, predetermination pricing only
        /// 
        /// Payment has not been made and the enclosed response is predetermination pricing only.
        /// </summary>
        [EnumStringValue("81")]
        NotPaidPredeterminationPricingOnly = 81,

        /// <summary>
        /// Documentation claim
        /// 
        /// The claim is for documentation purposes only, no payment required.
        /// </summary>
        [EnumStringValue("82")]
        DocumentationClaim = 82,

        /// <summary>
        /// Assessed.
        /// </summary>
        [EnumStringValue("83")]
        Reviewed = 83,

        /// <summary>
        /// This price was changed.
        /// </summary>
        [EnumStringValue("84")]
        Repriced = 84,

        /// <summary>
        /// An official examination has occurred.
        /// </summary>
        [EnumStringValue("85")]
        Audited = 85,

        /// <summary>
        /// Conditionally paid
        /// 
        /// Payment has been conditionally made.
        /// </summary>
        [EnumStringValue("86")]
        ConditionallyPaid = 86,

        /// <summary>
        /// On appeal
        /// 
        /// Reconsideration of the decision has been applied for.
        /// </summary>
        [EnumStringValue("87")]
        OnAppeal = 87,

        /// <summary>
        /// Shut.
        /// </summary>
        [EnumStringValue("88")]
        Closed = 88,

        /// <summary>
        /// A subsequent official examination has occurred.
        /// </summary>
        [EnumStringValue("89")]
        Reaudited = 89,

        /// <summary>
        /// Issued again.
        /// </summary>
        [EnumStringValue("90")]
        Reissued = 90,

        /// <summary>
        /// Closed after reopening
        /// 
        /// Reopened and then closed.
        /// </summary>
        [EnumStringValue("91")]
        ClosedAfterReopening = 91,

        /// <summary>
        /// Determined again or differently.
        /// </summary>
        [EnumStringValue("92")]
        Redetermined = 92,

        /// <summary>
        /// Processed as primary
        /// 
        /// Processed as the first.
        /// </summary>
        [EnumStringValue("93")]
        ProcessedAsPrimary = 93,

        /// <summary>
        /// Processed as secondary
        /// 
        /// Processed as the second.
        /// </summary>
        [EnumStringValue("94")]
        ProcessedAsSecondary = 94,

        /// <summary>
        /// Processed as tertiary
        /// 
        /// Processed as the third.
        /// </summary>
        [EnumStringValue("95")]
        ProcessedAsTertiary = 95,

        /// <summary>
        /// Correction of error
        /// 
        /// A correction to information previously communicated which contained an error.
        /// </summary>
        [EnumStringValue("96")]
        CorrectionOfError = 96,

        /// <summary>
        /// Single credit item of a group
        /// 
        /// Notification that the credit item is a single credit item of a group of credit items.
        /// </summary>
        [EnumStringValue("97")]
        SingleCreditItemOfAGroup = 97,

        /// <summary>
        /// Single debit item of a group
        /// 
        /// Notification that the debit item is a single debit item of a group of debit items.
        /// </summary>
        [EnumStringValue("98")]
        SingleDebitItemOfAGroup = 98,

        /// <summary>
        /// Interim response
        /// 
        /// The response is an interim one.
        /// </summary>
        [EnumStringValue("99")]
        InterimResponse = 99,

        /// <summary>
        /// Final response
        /// 
        /// The response is a final one.
        /// </summary>
        [EnumStringValue("100")]
        FinalResponse = 100,

        /// <summary>
        /// Debit advice requested
        /// 
        /// A debit advice is requested for the transaction.
        /// </summary>
        [EnumStringValue("101")]
        DebitAdviceRequested = 101,

        /// <summary>
        /// Transaction not impacted
        /// 
        /// Advice that the transaction is not impacted.
        /// </summary>
        [EnumStringValue("102")]
        TransactionNotImpacted = 102,

        /// <summary>
        /// Patient to be notified
        /// 
        /// The action to take is to notify the patient.
        /// </summary>
        [EnumStringValue("103")]
        PatientToBeNotified = 103,

        /// <summary>
        /// Healthcare provider to be notified
        /// 
        /// The action to take is to notify the healthcare provider.
        /// </summary>
        [EnumStringValue("104")]
        HealthcareProviderToBeNotified = 104,

        /// <summary>
        /// Usual general practitioner to be notified
        /// 
        /// The action to take is to notify the usual general practitioner.
        /// </summary>
        [EnumStringValue("105")]
        UsualGeneralPractitionerToBeNotified = 105,

        /// <summary>
        /// Advice without details
        /// 
        /// An advice without details is requested or notified.
        /// </summary>
        [EnumStringValue("106")]
        AdviceWithoutDetails = 106,

        /// <summary>
        /// Advice with details
        /// 
        /// An advice with details is requested or notified.
        /// </summary>
        [EnumStringValue("107")]
        AdviceWithDetails = 107,

        /// <summary>
        /// Amendment requested
        /// 
        /// An amendment is requested.
        /// </summary>
        [EnumStringValue("108")]
        AmendmentRequested = 108,

        /// <summary>
        /// For information
        /// 
        /// Included for information only.
        /// </summary>
        [EnumStringValue("109")]
        ForInformation = 109,

        /// <summary>
        /// A code indicating discontinuance or retraction.
        /// </summary>
        [EnumStringValue("110")]
        Withdraw = 110,

        /// <summary>
        /// Delivery date change
        /// 
        /// The action / notification is a change of the delivery date.
        /// </summary>
        [EnumStringValue("111")]
        DeliveryDateChange = 111,

        /// <summary>
        /// Quantity change
        /// 
        /// The action / notification is a change of quantity.
        /// </summary>
        [EnumStringValue("112")]
        QuantityChange = 112,

        /// <summary>
        /// Resale and claim
        /// 
        /// The identified items have been sold by the distributor to the end customer, and compensation for the loss of inventory value is claimed.
        /// </summary>
        [EnumStringValue("113")]
        ResaleAndClaim = 113,

        /// <summary>
        /// Resale
        /// 
        /// The identified items have been sold by the distributor to the end customer.
        /// </summary>
        [EnumStringValue("114")]
        Resale = 114,

        /// <summary>
        /// Prior addition
        /// 
        /// This existing line item becomes available at an earlier date.
        /// </summary>
        [EnumStringValue("115")]
        PriorAddition = 115,

        /// <summary>
        /// This line has expired.
        /// </summary>
        [EnumStringValue("116")]
        Expired = 116,

        /// <summary>
        /// This line is on Hold.
        /// </summary>
        [EnumStringValue("117")]
        Hold = 117,

        /// <summary>
        /// This line is open.
        /// </summary>
        [EnumStringValue("118")]
        Open = 118
    }
}
