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


namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// UNTDID 4461 Payment means code
    ///
    /// This enum contains payment methods organized into the following logical blocks:
    ///
    /// 1. Basic Cash and Check Payments - Traditional retail, local businesses, small transactions
    /// 2. Modern Electronic Transfers - Business-to-business, international commerce, recurring payments(most common)
    /// 3. Card-Based Payments - E-commerce, retail, consumer transactions
    /// 4. Digital/Online Payment Services - E-commerce, fintech, modern applications
    /// 5. Automated Clearing House(ACH) Transactions - US-based automated payments, payroll, recurring bills
    /// 6. Specialized Banking Operations - Internal bank operations, interbank transfers
    /// 7. Bank Drafts and Certified Instruments - High-value transactions, international trade
    /// 8. Bills of Exchange and Trade Finance - International trade, commercial transactions
    /// 9. Promissory Notes - Formal debt instruments, commercial lending
    /// 10. Regional Payment Systems - Country-specific payment networks
    /// 11. Priority and Special Handling - Time-sensitive payments, treasury operations
    /// 12. Agreement and Settlement Types - Business relationships, partner transactions
    /// 13. Special Cases - Error handling, custom implementations
    ///
    /// Most Common Payment Methods
    /// - SEPA Credit Transfer(58) - European countries, businesses and government bodies use the SEPA network to make payments by direct debit and credit transfer, making SEPA one of the most widely used global payment methods.SEPA currently processes around 50 billion transactions every year
    /// - SEPA Direct Debit (59) - As of August 2014, 99.4% of credit transfers, 99.9% of direct debit, and 79.2% of card payments have been migrated to SEPA in the euro area
    /// - Credit Card(54) - Credit cards represented 20% of global e-commerce transactions in 2023
    /// - Credit Transfer(30) - Traditional bank transfers
    /// - Direct Debit(49) - Automated account debiting
    /// - Online Payment Service(68) - The buyer can also make payments through online payment service providers like PayPal and Stripe
    /// </summary>
    public enum PaymentMeansTypeCodes
    {
        /// <summary>
        /// Instrument not defined
        ///
        /// Not defined legally enforceable agreement between two or
        /// more parties (expressing a contractual right or a right
        /// to the payment of money).
        /// 
        /// Available in: Extended
        /// </summary>
        [EnumStringValue("1")]
        NotDefined = 1,

        /// <summary>
        /// Automated clearing house debit
        /// 
        ///  A credit transaction made through the automated clearing
        /// house system.
        /// 
        /// </summary>
        [EnumStringValue("2")]
        AutomatedClearingHouseCredit = 2,

        /// <summary>
        /// Automated clearing house debit
        /// 
        /// A debit transaction made through the automated clearing
        /// house system.
        /// 
        /// </summary>
        [EnumStringValue("3")]
        AutomatedClearingHouseDebit = 3,

        /// <summary>
        /// ACH demand debit reversal
        ///
        /// A request to reverse an ACH debit transaction to a
        /// demand deposit account.
        /// 
        /// </summary>
        [EnumStringValue("4")]
        ACHDemandDebitReversal = 4,


        /// <summary>
        /// ACH demand credit reversal
        ///
        /// A request to reverse a credit transaction to a demand
        /// deposit account.
        /// </summary>        
        [EnumStringValue("5")]
        ACHDemandCreditReversal = 5,

        /// <summary>
        /// ACH demand credit
        ///
        /// A credit transaction made through the ACH system to a
        /// demand deposit account.
        /// </summary>
        [EnumStringValue("6")]
        ACHDemandCredit = 6,

        /// <summary>
        /// ACH demand debit
        ///
        /// A debit transaction made through the ACH system to a
        /// demand deposit account.
        /// </summary>
        [EnumStringValue("7")]
        ACHDemandDebit = 7,

        /// <summary>
        /// Hold
        ///
        /// Indicates that the bank should hold the payment for
        /// collection by the beneficiary or other instructions.
        /// </summary>
        [EnumStringValue("8")]
        Hold = 8,

        /// <summary>
        /// National or regional clearing
        ///
        /// Indicates that the payment should be made using the
        /// national or regional clearing.
        /// </summary>
        [EnumStringValue("9")]
        NationalOrRegionalClearing = 9,

        /// <summary>
        /// Bar
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("10")]
        InCash = 10,

        /// <summary>
        /// ACH savings credit reversal
        ///
        /// A request to reverse an ACH credit transaction to a
        /// savings account.
        /// </summary>
        [EnumStringValue("11")]
        ACHSavingsCreditReversal = 11,

        /// <summary>
        /// ACH savings debit reversal
        ///
        /// A request to reverse an ACH debit transaction to a
        /// savings account.
        /// </summary>
        [EnumStringValue("12")]
        ACHSavingsDebitReversal = 12,

        /// <summary>
        /// ACH savings credit
        ///
        /// A credit transaction made through the ACH system to a
        /// savings account.
        /// </summary>
        [EnumStringValue("13")]
        ACHSavingsCredit = 13,

        /// <summary>
        /// ACH savings debit
        ///
        /// A debit transaction made through the ACH system to a
        /// savings account.
        /// </summary>
        [EnumStringValue("14")]
        ACHSavingsDebit = 14,

        /// <summary>
        /// Bookentry credit
        ///
        /// A credit entry between two accounts at the same bank
        /// branch. Synonym: house credit.
        /// </summary>
        [EnumStringValue("15")]
        BookentryCredit = 15,

        /// <summary>
        /// Bookentry debit
        ///
        /// A debit entry between two accounts at the same bank
        /// branch. Synonym: house debit.
        /// </summary>
        [EnumStringValue("16")]
        BookentryDebit = 16,

        /// <summary>
        /// ACH demand cash concentration/disbursement (CCD) credit
        ///
        /// A credit transaction made through the ACH system to a
        /// demand deposit account using the CCD payment format.
        /// </summary>
        [EnumStringValue("17")]
        ACHDemandCCDCredit = 17,

        /// <summary>
        /// ACH demand cash concentration/disbursement (CCD) debit
        ///
        /// A debit transaction made through the ACH system to a
        /// demand deposit account using the CCD payment format.
        /// </summary>
        [EnumStringValue("18")]
        ACHDemandCCDDebit = 18,

        /// <summary>
        /// ACH demand corporate trade payment (CTP) credit
        ///
        /// A credit transaction made through the ACH system to a
        /// demand deposit account using the CTP payment format.
        /// </summary>
        [EnumStringValue("19")]
        ACHDemandCTPCredit = 19,

        /// <summary>
        /// Scheck
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("20")]
        Cheque = 20,

        /// <summary>
        /// Banker's draft
        ///
        /// Issue of a banker's draft in payment of the funds.
        /// </summary>
        [EnumStringValue("21")]
        BankersDraft = 21,

        /// <summary>
        /// Certified banker's draft
        ///
        /// Cheque drawn by a bank on itself or its agent. A person
        /// who owes money to another buys the draft from a bank for
        /// cash and hands it to the creditor who need have no fear
        /// that it might be dishonoured.
        /// </summary>
        [EnumStringValue("22")]
        CertifiedBankersDraft = 22,

        /// <summary>
        /// Bank cheque (issued by a banking or similar establishment)
        ///
        /// Payment by a pre-printed form, which has been completed
        /// by a financial institution, on which instructions are
        /// given to an account holder (a bank or building society)
        /// to pay a stated sum to a named recipient.
        /// </summary>
        [EnumStringValue("23")]
        BankCheque = 23,

        /// <summary>
        /// Bill of exchange awaiting acceptance
        ///
        /// Bill drawn by the creditor on the debtor but not yet
        /// accepted by the debtor.
        /// </summary>
        [EnumStringValue("24")]
        BillOfExchangeAwaitingAcceptance = 24,

        /// <summary>
        /// Certified cheque
        ///
        /// Payment by a pre-printed form stamped with the paying
        /// bank's certification on which instructions are given to
        /// an account holder (a bank or building society) to pay a
        /// stated sum to a named recipient.
        /// </summary>
        [EnumStringValue("25")]
        CertifiedCheque = 25,

        /// <summary>
        /// Local cheque
        ///
        /// Indicates that the cheque is given local to the
        /// recipient.
        /// </summary>
        [EnumStringValue("26")]
        LocalCheque = 26,

        /// <summary>
        /// ACH demand corporate trade payment (CTP) debit
        ///
        /// A debit transaction made through the ACH system to a
        /// demand deposit account using the CTP payment format.
        /// </summary>
        [EnumStringValue("27")]
        ACHDemandCTPDebit = 27,

        /// <summary>
        /// ACH demand corporate trade exchange (CTX) credit
        ///
        /// A credit transaction made through the ACH system to a
        /// demand deposit account using the CTX payment format.
        /// </summary>
        [EnumStringValue("28")]
        ACHDemandCTXCredit = 28,

        /// <summary>
        /// ACH demand corporate trade exchange (CTX) debit
        ///
        /// A debit transaction made through the ACH system to a
        /// demand account using the CTX payment format.
        /// </summary>
        [EnumStringValue("29")]
        ACHDemandCTXDebit = 29,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("30")]
        CreditTransfer = 30,

        /// <summary>
        /// Lastschriftübermittlung:
        /// Zahlung durch Belastung eines Geldbetrages eines
        /// Kontos zugunsten eines anderen.
        /// Überweisung international und nationale SEPA-Überweisung
        /// 
        /// Available in: Extended
        /// </summary>
        [EnumStringValue("31")]
        DebitTransfer = 31,

        /// <summary>
        /// ACH demand cash concentration/disbursement plus (CCD+) credit
        ///
        /// A credit transaction made through the ACH system to a
        /// demand deposit account using the CCD+ payment format.
        /// </summary>
        [EnumStringValue("32")]
        ACHDemandCCDPlusCredit = 32,

        /// <summary>
        /// ACH demand cash concentration/disbursement plus (CCD+) debit
        ///
        /// A debit transaction made through the ACH system to a
        /// demand deposit account using the CCD+ payment format.
        /// </summary>
        [EnumStringValue("33")]
        ACHDemandCCDPlusDebit = 33,

        /// <summary>
        /// ACH prearranged payment and deposit (PPD)
        ///
        /// A consumer credit transaction made through the ACH
        /// system to a demand deposit or savings account.
        /// </summary>
        [EnumStringValue("34")]
        ACHPrearrangedPaymentAndDeposit = 34,

        /// <summary>
        /// ACH savings cash concentration/disbursement (CCD) credit
        ///
        /// A credit transaction made through the ACH system to a
        /// demand deposit or savings account.
        /// </summary>
        [EnumStringValue("35")]
        ACHSavingsCCDCredit = 35,

        /// <summary>
        /// ACH savings cash concentration/disbursement (CCD) debit
        ///
        /// A debit transaction made through the ACH system to a
        /// savings account using the CCD payment format.
        /// </summary>
        [EnumStringValue("36")]
        ACHSavingsCCDDebit = 36,

        /// <summary>
        /// ACH savings corporate trade payment (CTP) credit
        ///
        /// A credit transaction made through the ACH system to a
        /// savings account using the CTP payment format.
        /// </summary>
        [EnumStringValue("37")]
        ACHSavingsCTPCredit = 37,

        /// <summary>
        /// ACH savings corporate trade payment (CTP) debit
        ///
        /// A debit transaction made through the ACH system to a
        /// savings account using the CTP payment format.
        /// </summary>
        [EnumStringValue("38")]
        ACHSavingsCTPDebit = 38,

        /// <summary>
        /// ACH savings corporate trade exchange (CTX) credit
        ///
        /// A credit transaction made through the ACH system to a
        /// savings account using the CTX payment format.
        /// </summary>
        [EnumStringValue("39")]
        ACHSavingsCTXCredit = 39,

        /// <summary>
        /// ACH savings corporate trade exchange (CTX) debit
        ///
        /// A debit transaction made through the ACH system to a
        /// savings account using the CTX payment format.
        /// </summary>
        [EnumStringValue("40")]
        ACHSavingsCTXDebit = 40,

        /// <summary>
        /// ACH savings cash concentration/disbursement plus (CCD+) credit
        ///
        /// A credit transaction made through the ACH system to a
        /// savings account using the CCD+ payment format.
        /// </summary>
        [EnumStringValue("41")]
        ACHSavingsCCDPlusCredit = 41,

        /// <summary>
        /// Zahlung an Bankkonto
        /// Überweisung national, vor SEPA-Umstellung
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("42")]
        PaymentToBankAccount = 42,

        /// <summary>
        /// ACH savings cash concentration/disbursement plus (CCD+) debit
        ///
        /// A debit transaction made through the ACH system to a
        /// savings account using the CCD+ payment format.
        /// </summary>
        [EnumStringValue("43")]
        ACHSavingsCCDPlusDebit = 43,

        /// <summary>
        /// Accepted bill of exchange
        ///
        /// Bill drawn by the creditor on the debtor and accepted by
        /// the debtor.
        /// </summary>
        [EnumStringValue("44")]
        AcceptedBillOfExchange = 44,

        /// <summary>
        /// Referenced home-banking credit transfer
        ///
        /// A referenced credit transfer initiated through home-
        /// banking.
        /// </summary>
        [EnumStringValue("45")]
        ReferencedHomeBankingCreditTransfer = 45,

        /// <summary>
        /// Interbank debit transfer
        ///
        /// A debit transfer via interbank means.
        /// </summary>
        [EnumStringValue("46")]
        InterbankDebitTransfer = 46,

        /// <summary>
        /// Home-banking debit transfer
        ///
        /// A debit transfer initiated through home-banking.
        /// </summary>
        [EnumStringValue("47")]
        HomeBankingDebitTransfer = 47,

        /// <summary>
        /// Bankkkarte, Kreditkarte
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("48")]
        BankCard = 48,

        /// <summary>
        /// Lastschriftverfahren
        /// 
        /// Available in: Basic, Extended
        /// /// </summary>
        [EnumStringValue("49")]
        DirectDebit = 49,

        /// <summary>
        /// Payment by postgiro
        ///
        /// A method for the transmission of funds through the
        /// postal system rather than through the banking system.
        /// </summary>
        [EnumStringValue("50")]
        PaymentByPostgiro = 50,

        /// <summary>
        /// FR, norme 6 97-Telereglement CFONB (French Organisation for Banking Standards) - Option A
        ///
        /// A French standard procedure that allows a debtor to pay
        /// an amount due to a creditor. The creditor will forward
        /// it to its bank, which will collect the money on the bank
        /// account of the debtor.
        /// </summary>
        [EnumStringValue("51")]
        FrenchTelerglementCFONB = 51,

        /// <summary>
        /// Urgent commercial payment
        ///
        /// Payment order which requires guaranteed processing by
        /// the most appropriate means to ensure it occurs on the
        /// requested execution date, provided that it is issued to
        /// the ordered bank before the agreed cut-off time.
        /// </summary>
        [EnumStringValue("52")]
        UrgentCommercialPayment = 52,

        /// <summary>
        /// Urgent Treasury Payment
        ///
        /// Payment order or transfer which must be executed, by the
        /// most appropriate means, as urgently as possible and
        /// before urgent commercial payments.
        /// </summary>
        [EnumStringValue("53")]
        UrgentTreasuryPayment = 53,

        /// <summary>
        /// Credit card
        ///
        /// Payment made by means of credit card.
        /// </summary>
        [EnumStringValue("54")]
        CreditCard = 54,

        /// <summary>
        /// Debit card
        ///
        /// Payment made by means of debit card.
        /// </summary>
        [EnumStringValue("55")]
        DebitCard = 55,

        /// <summary>
        /// Bankgiro
        ///
        /// Payment will be, or has been, made by bankgiro.
        /// </summary>
        [EnumStringValue("56")]
        Bankgiro = 56,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>        
        [EnumStringValue("57")]
        StandingAgreement = 57,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("58")]
        SEPACreditTransfer = 58,

        /// <summary>
        /// Available in: Basic, Extended
        /// </summary>        
        [EnumStringValue("59")]
        SEPADirectDebit = 59,

        /// pay on demand or at a fixed or determinable future time
        /// a sum certain in money, to order or to bearer.
        /// </summary>
        [EnumStringValue("60")]
        PromissoryNote = 60,

        /// <summary>
        /// Promissory note signed by the debtor
        ///
        /// Payment by an unconditional promise in writing made by
        /// the debtor to another person, signed by the debtor,
        /// engaging to pay on demand or at a fixed or determinable
        /// future time a sum certain in money, to order or to bearer.
        /// </summary>
        [EnumStringValue("61")]
        PromissoryNoteSignedByDebtor = 61,

        /// <summary>
        /// Promissory note signed by the debtor and endorsed by a bank
        ///
        /// Payment by an unconditional promise in writing made by
        /// the debtor to another person, signed by the debtor and
        /// endorsed by a bank, engaging to pay on demand or at a
        /// fixed or determinable future time a sum certain in
        /// money, to order or to bearer.
        /// </summary>
        [EnumStringValue("62")]
        PromissoryNoteSignedByDebtorEndorsedByBank = 62,

        /// <summary>
        /// Promissory note signed by the debtor and endorsed by a third party
        ///
        /// Payment by an unconditional promise in writing made by
        /// the debtor to another person, signed by the debtor and
        /// endorsed by a third party, engaging to pay on demand or
        /// at a fixed or determinable future time a sum certain in
        /// money, to order or to bearer.
        /// </summary>
        [EnumStringValue("63")]
        PromissoryNoteSignedByDebtorEndorsedByThirdParty = 63,

        /// <summary>
        /// Promissory note signed by a bank
        ///
        /// Payment by an unconditional promise in writing made by
        /// the bank to another person, signed by the bank, engaging
        /// to pay on demand or at a fixed or determinable future
        /// time a sum certain in money, to order or to bearer.
        /// </summary>
        [EnumStringValue("64")]
        PromissoryNoteSignedByBank = 64,

        /// <summary>
        /// Promissory note signed by a bank and endorsed by another bank
        ///
        /// Payment by an unconditional promise in writing made by
        /// the bank to another person, signed by the bank and
        /// endorsed by another bank, engaging to pay on demand or
        /// at a fixed or determinable future time a sum certain in
        /// money, to order or to bearer.
        /// </summary>
        [EnumStringValue("65")]
        PromissoryNoteSignedByBankEndorsedByAnotherBank = 65,

        /// <summary>
        /// Promissory note signed by a third party
        ///
        /// Payment by an unconditional promise in writing made by a
        /// third party to another person, signed by the third
        /// party, engaging to pay on demand or at a fixed or
        /// determinable future time a sum certain in money, to
        /// order or to bearer.
        /// </summary>
        [EnumStringValue("66")]
        PromissoryNoteSignedByThirdParty = 66,

        /// <summary>
        /// Promissory note signed by a third party and endorsed by a bank
        ///
        /// Payment by an unconditional promise in writing made by a
        /// third party to another person, signed by the third party
        /// and endorsed by a bank, engaging to pay on demand or at
        /// a fixed or determinable future time a sum certain in
        /// money, to order or to bearer.
        /// </summary>
        [EnumStringValue("67")]
        PromissoryNoteSignedByThirdPartyEndorsedByBank = 67,

        /// <summary>
        /// Payment will be made or has been made by an online payment service like Paypal, Stripe etc.
        /// </summary>
        [EnumStringValue("68")]
        OnlinePaymentService = 68,

        /// Bill drawn by the creditor on the debtor
        ///
        /// Bill drawn by the creditor on the debtor.
        /// </summary>
        [EnumStringValue("70")]
        BillDrawnByCreditorOnDebtor = 70,

        /// <summary>
        /// Bill drawn by the creditor on a bank
        ///
        /// Bill drawn by the creditor on a bank.
        /// </summary>
        [EnumStringValue("74")]
        BillDrawnByCreditorOnBank = 74,

        /// <summary>
        /// Bill drawn by the creditor, endorsed by another bank
        ///
        /// Bill drawn by the creditor, endorsed by another bank.
        /// </summary>
        [EnumStringValue("75")]
        BillDrawnByCreditorEndorsedByAnotherBank = 75,

        /// <summary>
        /// Bill drawn by the creditor on a bank and endorsed by a third party
        ///
        /// Bill drawn by the creditor on a bank and endorsed by a
        /// third party.
        /// </summary>
        [EnumStringValue("76")]
        BillDrawnByCreditorOnBankEndorsedByThirdParty = 76,

        /// <summary>
        /// Bill drawn by the creditor on a third party
        ///
        /// Bill drawn by the creditor on a third party.
        /// </summary>
        [EnumStringValue("77")]
        BillDrawnByCreditorOnThirdParty = 77,

        /// <summary>
        /// Bill drawn by creditor on third party, accepted and endorsed by bank
        ///
        /// Bill drawn by creditor on third party, accepted and
        /// endorsed by bank.
        /// </summary>
        [EnumStringValue("78")]
        BillDrawnByCreditorOnThirdPartyAcceptedAndEndorsedByBank = 78,

        /// <summary>
        /// Not transferable banker's draft
        ///
        /// Issue a bankers draft not endorsable.
        /// </summary>
        [EnumStringValue("91")]
        NotTransferableBankersDraft = 91,

        /// <summary>
        /// Not transferable local cheque
        ///
        /// Issue a cheque not endorsable in payment of the funds.
        /// </summary>
        [EnumStringValue("92")]
        NotTransferableLocalCheque = 92,

        /// <summary>
        /// Reference giro
        ///
        /// Ordering customer tells the bank to use the payment
        /// system 'Reference giro'. Used in the Finnish national
        /// banking system.
        /// </summary>
        [EnumStringValue("93")]
        ReferenceGiro = 93,

        /// <summary>
        /// Urgent giro
        ///
        /// Ordering customer tells the bank to use the bank service
        /// 'Urgent Giro' when transferring the payment. Used in
        /// Finnish national banking system.
        /// </summary>
        [EnumStringValue("94")]
        UrgentGiro = 94,

        /// <summary>
        /// Free format giro
        ///
        /// Ordering customer tells the ordering bank to use the
        /// bank service 'Free Format Giro' when transferring the
        /// payment. Used in Finnish national banking system.
        /// </summary>
        [EnumStringValue("95")]
        FreeFormatGiro = 95,

        /// <summary>
        /// Requested method for payment was not used
        ///
        /// If the requested method for payment was or could not be
        /// used, this code indicates that.
        /// </summary>
        [EnumStringValue("96")]
        RequestedMethodForPaymentWasNotUsed = 96,

        /// <summary>
        /// Ausgleich zwischen Partnern.
        /// Beträge, die zwei Partner sich gegenseitig schulden werden ausgeglichen um unnütze Zahlungen zu vermeiden.
        /// Available in: Basic, Extended
        /// </summary>
        [EnumStringValue("97")]
        ClearingBetweenPartners = 97,

        /// Mutually defined
        ///
        /// A code assigned within a code list to be used on an
        /// interim basis and as defined among trading partners
        /// until a precise code can be assigned to the code list.
        /// </summary>
        [EnumStringValue("ZZZ")]
        MutuallyDefined = 999
    }
}
