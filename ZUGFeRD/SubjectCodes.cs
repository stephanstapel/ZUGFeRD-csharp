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

    /// <summary>
    /// UNTDID 4451 — Text subject qualifier
    /// Version 24A
    /// https://service.unece.org/trade/untdid/d23a/tred/tred4451.htm
    /// </summary>
    public enum SubjectCodes
    {
        /// <summary>
        /// Goods item description
        /// </summary>
        AAA,

        /// <summary>
        /// Terms of payments
        /// </summary>
        /// [4276] Conditions of payment between the parties to a transaction(generic term).
        AAB,

        /// <summary>
        /// Dangerous goods additional information
        /// </summary>
        /// Additional information concerning dangerous goods.
        AAC,

        /// <summary>
        /// Dangerous goods technical name
        /// </summary>
        AAD,

        /// <summary>
        /// Acknowledgement description
        /// </summary>
        AAE,

        /// <summary>
        /// Rate additional information
        /// </summary>
        AAF,

        /// <summary>
        /// Party instructions
        /// </summary>
        AAG,

        /// <summary>
        /// General information
        ///
        /// Generelle Informationen
        /// </summary>
        /// Generelle Informationen zu diesem Kauf
        AAI,

        /// <summary>
        /// Zusätzliche Konditionen zu diesem Kauf
        ///
        /// Angaben zum Eigentumsvorbehalt
        /// </summary>
        AAJ,

        /// <summary>
        /// Price conditions
        /// 
        /// Preiskonditionen
        ///
        /// Angaben zu Entgeltminderungen
        /// </summary>
        AAK,

        /// <summary>
        /// Goods dimensions in characters
        /// </summary>
        AAL,

        /// <summary>
        /// Equipment re-usage restrictions
        /// </summary>
        AAM,

        /// <summary>
        /// Handling restriction
        /// </summary>
        AAN,

        /// <summary>
        /// Error description (free text)
        /// </summary>
        AAO,

        /// <summary>
        /// Response (free text)
        /// </summary>
        AAP,

        /// <summary>
        /// Package content's description
        /// </summary>
        AAQ,

        /// <summary>
        /// Terms of delivery
        /// </summary>
        AAR,

        /// <summary>
        /// Bill of lading remarks
        /// </summary>
        AAS,

        /// <summary>
        /// Mode of settlement information
        /// </summary>
        AAT,

        /// <summary>
        /// Consignment invoice information
        /// </summary>
        AAU,

        /// <summary>
        /// Clearance invoice information
        /// </summary>
        AAV,

        /// <summary>
        /// Letter of credit information
        /// </summary>
        AAW,

        /// <summary>
        /// License information
        /// </summary>
        AAX,

        /// <summary>
        /// Certification statements
        /// </summary>
        AAY,

        /// <summary>
        /// Additional export information
        /// </summary>
        AAZ,

        /// <summary>
        /// Tariff statements
        /// </summary>
        ABA,

        /// <summary>
        /// Medical history
        /// </summary>
        ABB,

        /// <summary>
        /// Conditions of sale or purchase
        /// </summary>
        ABC,

        /// <summary>
        /// Contract document type
        /// </summary>
        ABD,

        /// <summary>
        /// Additional terms and/or conditions (documentary credit)
        /// </summary>
        ABE,

        /// <summary>
        /// Instructions or information about standby documentary
        /// </summary>
        ABF,

        /// <summary>
        /// Instructions or information about partial shipment(s)
        /// </summary>
        ABG,

        /// <summary>
        /// Instructions or information about transhipment(s)
        /// </summary>
        ABH,

        /// <summary>
        /// Additional handling instructions documentary credit
        /// </summary>
        ABI,

        /// <summary>
        /// Domestic routing information
        /// </summary>
        ABJ,

        /// <summary>
        /// Chargeable category of equipment
        /// </summary>
        ABK,

        /// <summary>
        /// Government information
        /// </summary>
        /// Self explanatory.
        ABL,

        /// <summary>
        /// Onward routing information
        /// </summary>
        ABM,

        /// <summary>
        /// Accounting information
        ///
        /// Buchhaltungsinformationen
        /// </summary>
        /// Informationen für die Buchaltung zu diesem Kauf
        ABN,

        /// <summary>
        /// Discrepancy information
        /// </summary>
        ABO,

        /// <summary>
        /// Confirmation instructions
        /// </summary>
        ABP,

        /// <summary>
        /// Method of issuance
        /// </summary>
        ABQ,

        /// <summary>
        /// Documents delivery instructions
        /// </summary>
        ABR,

        /// <summary>
        /// Additional conditions
        /// </summary>
        ABS,

        /// <summary>
        /// Information/instructions about additional amounts covered
        /// </summary>
        ABT,

        /// <summary>
        /// Deferred payment termed additional
        /// </summary>
        ABU,

        /// <summary>
        /// Acceptance terms additional
        /// </summary>
        ABV,

        /// <summary>
        /// Negotiation terms additional
        /// </summary>
        ABW,

        /// <summary>
        /// Document name and documentary requirements
        /// </summary>
        ABX,

        /// <summary>
        /// Instructions/information about revolving documentary credit
        /// </summary>
        ABZ,

        /// <summary>
        /// Documentary requirements
        /// </summary>
        ACA,

        /// <summary>
        /// Factor assignment clause
        /// </summary>
        /// Assignment based on an agreement between seller and factor.
        ACC,

        /// <summary>
        /// Additional information
        ///
        /// Zusätzliche Angaben
        /// </summary>
        /// Zusaätzliche Angaben zu diesem Kauf
        ACB,

        /// <summary>
        /// Reason
        /// </summary>
        ACD,

        /// <summary>
        /// Dispute
        /// </summary>
        ACE,

        /// <summary>
        /// Additional attribute information
        /// </summary>
        ACF,

        /// <summary>
        /// Absence declaration
        /// </summary>
        ACG,

        /// <summary>
        /// Aggregation statement
        /// </summary>
        ACH,

        /// <summary>
        /// Compilation statement
        /// </summary>
        ACI,

        /// <summary>
        /// Definitional exception
        /// </summary>
        ACJ,

        /// <summary>
        /// Privacy statement
        /// </summary>
        ACK,

        /// <summary>
        /// Quality statement
        /// </summary>
        ACL,

        /// <summary>
        /// Statistical description
        /// </summary>
        ACM,

        /// <summary>
        /// Statistical definition
        /// </summary>
        ACN,

        /// <summary>
        /// Statistical name
        /// </summary>
        ACO,

        /// <summary>
        /// Statistical title
        /// </summary>
        ACP,

        /// <summary>
        /// Off-dimension information
        /// </summary>
        ACQ,

        /// <summary>
        /// Unexpected stops information
        /// </summary>
        ACR,

        /// <summary>
        /// Principles
        /// </summary>
        ACS,

        /// <summary>
        /// Terms and definition
        /// </summary>
        ACT,

        /// <summary>
        /// Segment name
        /// </summary>
        ACU,

        /// <summary>
        /// Simple data element name
        /// </summary>
        ACV,

        /// <summary>
        /// Scope
        /// </summary>
        ACW,

        /// <summary>
        /// Message type name
        /// </summary>
        ACX,

        /// <summary>
        /// Introduction
        /// </summary>
        ACY,

        /// <summary>
        /// Glossary
        /// </summary>
        ACZ,

        /// <summary>
        /// Functional definition
        /// </summary>
        ADA,

        /// <summary>
        /// Examples
        /// </summary>
        ADB,

        /// <summary>
        /// Cover page
        /// </summary>
        ADC,

        /// <summary>
        /// Dependency (syntax) notes
        /// </summary>
        ADD,

        /// <summary>
        /// Code value name
        /// </summary>
        ADE,

        /// <summary>
        /// Code list name
        /// </summary>
        ADF,

        /// <summary>
        /// Clarification of usage
        /// </summary>
        ADG,

        /// <summary>
        /// Composite data element name
        /// </summary>
        ADH,

        /// <summary>
        /// Field of application
        /// </summary>
        ADI,

        /// <summary>
        /// Type of assets and liabilities
        /// </summary>
        ADJ,

        /// <summary>
        /// Promotion information
        /// </summary>
        ADK,

        /// <summary>
        /// Meter condition
        /// </summary>
        ADL,

        /// <summary>
        /// Meter reading information
        /// </summary>
        ADM,

        /// <summary>
        /// Type of transaction reason
        /// </summary>
        ADN,

        /// <summary>
        /// Type of survey question
        /// </summary>
        ADO,

        /// <summary>
        /// Carrier's agent counter information
        /// </summary>
        ADP,

        /// <summary>
        /// Description of work item on equipment
        /// </summary>
        ADQ,

        /// <summary>
        /// Message definition
        /// </summary>
        ADR,

        /// <summary>
        /// Booked item information
        /// </summary>
        ADS,

        /// <summary>
        /// Source of document
        /// </summary>
        ADT,

        /// <summary>
        /// Text subject is note.
        /// </summary>
        ADU,

        /// <summary>
        /// Fixed part of segment clarification text
        /// </summary>
        ADV,

        /// <summary>
        /// Characteristics of goods
        /// </summary>
        ADW,

        /// <summary>
        /// Additional discharge instructions
        /// </summary>
        ADX,

        /// <summary>
        /// Container stripping instructions
        /// </summary>
        ADY,

        /// <summary>
        /// CSC (Container Safety Convention) plate information
        /// </summary>
        ADZ,

        /// <summary>
        /// Cargo remarks
        /// </summary>
        AEA,

        /// <summary>
        /// Temperature control instructions
        /// </summary>
        AEB,

        /// <summary>
        /// Text refers to expected data
        /// </summary>
        AEC,

        /// <summary>
        /// Text refers to received data
        /// </summary>
        AED,

        /// <summary>
        /// Section clarification text
        /// </summary>
        AEE,

        /// <summary>
        /// Information to the beneficiary
        /// </summary>
        AEF,

        /// <summary>
        /// Information to the applicant
        /// </summary>
        AEG,

        /// <summary>
        /// Instructions to the beneficiary
        /// </summary>
        AEH,

        /// <summary>
        /// Instructions to the applicant
        /// </summary>
        AEI,

        /// <summary>
        /// Controlled atmosphere
        /// </summary>
        AEJ,

        /// <summary>
        /// Take off annotation
        /// </summary>
        AEK,

        /// <summary>
        /// Price variation narrative
        /// </summary>
        AEL,

        /// <summary>
        /// Documentary credit amendment instructions
        /// </summary>
        AEM,

        /// <summary>
        /// Standard method narrative
        /// </summary>
        AEN,

        /// <summary>
        /// Project narrative
        /// </summary>
        AEO,

        /// <summary>
        /// Radioactive goods, additional information
        /// </summary>
        AEP,

        /// <summary>
        /// Bank-to-bank information
        /// </summary>
        AEQ,

        /// <summary>
        /// Reimbursement instructions
        /// </summary>
        AER,

        /// <summary>
        /// Reason for amending a message
        /// </summary>
        AES,

        /// <summary>
        /// Instructions to the paying and/or accepting and/or negotiating bank
        /// </summary>
        AET,

        /// <summary>
        /// Interest instructions
        /// </summary>
        AEU,

        /// <summary>
        /// Agent commission
        /// </summary>
        AEV,

        /// <summary>
        /// Remitting bank instructions
        /// </summary>
        AEW,

        /// <summary>
        /// Instructions to the collecting bank
        /// </summary>
        AEX,

        /// <summary>
        /// Collection amount instructions
        /// </summary>
        AEY,

        /// <summary>
        /// Internal auditing information
        /// </summary>
        AEZ,

        /// <summary>
        /// Constraint
        /// </summary>
        AFA,

        /// <summary>
        /// Comment
        /// </summary>
        AFB,

        /// <summary>
        /// Semantic note
        /// </summary>
        AFC,

        /// <summary>
        /// Help text
        /// </summary>
        AFD,

        /// <summary>
        /// Legend
        /// </summary>
        AFE,

        /// <summary>
        /// Batch code structure
        /// </summary>
        AFF,

        /// <summary>
        /// Product application
        /// </summary>
        AFG,

        /// <summary>
        /// Customer complaint
        /// </summary>
        AFH,

        /// <summary>
        /// Probable cause of fault
        /// </summary>
        AFI,

        /// <summary>
        /// Defect description
        /// </summary>
        AFJ,

        /// <summary>
        /// Repair description
        /// </summary>
        AFK,

        /// <summary>
        /// Review comments
        /// </summary>
        AFL,

        /// <summary>
        /// Title
        /// </summary>
        AFM,

        /// <summary>
        /// Description of amount
        /// </summary>
        AFN,

        /// <summary>
        /// Responsibilities
        /// </summary>
        AFO,

        /// <summary>
        /// Supplier
        /// </summary>
        AFP,

        /// <summary>
        /// Purchase region
        /// </summary>
        AFQ,

        /// <summary>
        /// Affiliation
        /// </summary>
        AFR,

        /// <summary>
        /// Borrower
        /// </summary>
        AFS,

        /// <summary>
        /// Line of business
        /// </summary>
        AFT,

        /// <summary>
        /// Financial institution
        /// </summary>
        AFU,

        /// <summary>
        /// Business founder
        /// </summary>
        AFV,

        /// <summary>
        /// Business history
        /// </summary>
        AFW,

        /// <summary>
        /// Banking arrangements
        /// </summary>
        AFX,

        /// <summary>
        /// Business origin
        /// </summary>
        AFY,

        /// <summary>
        /// Brand names' description
        /// </summary>
        AFZ,

        /// <summary>
        /// Business financing details
        /// </summary>
        AGA,

        /// <summary>
        /// Competition
        /// </summary>
        AGB,

        /// <summary>
        /// Construction process details
        /// </summary>
        AGC,

        /// <summary>
        /// Construction specialty
        /// </summary>
        AGD,

        /// <summary>
        /// Contract information
        /// </summary>
        AGE,

        /// <summary>
        /// Corporate filing
        /// </summary>
        AGF,

        /// <summary>
        /// Customer information
        /// </summary>
        AGG,

        /// <summary>
        /// Copyright notice
        /// </summary>
        AGH,

        /// <summary>
        /// Contingent debt
        /// </summary>
        AGI,

        /// <summary>
        /// Conviction details
        /// </summary>
        AGJ,

        /// <summary>
        /// Equipment
        /// </summary>
        AGK,

        /// <summary>
        /// Workforce description
        /// </summary>
        AGL,

        /// <summary>
        /// Exemption
        /// </summary>
        AGM,

        /// <summary>
        /// Future plans
        /// </summary>
        AGN,

        /// <summary>
        /// Interviewee conversation information
        /// </summary>
        AGO,

        /// <summary>
        /// Intangible asset
        /// </summary>
        AGP,

        /// <summary>
        /// Inventory
        /// </summary>
        AGQ,

        /// <summary>
        /// Investment
        /// </summary>
        AGR,

        /// <summary>
        /// Intercompany relations information
        /// </summary>
        AGS,

        /// <summary>
        /// Joint venture
        /// </summary>
        AGT,

        /// <summary>
        /// Loan
        /// </summary>
        AGU,

        /// <summary>
        /// Long term debt
        /// </summary>
        AGV,

        /// <summary>
        /// Location
        /// </summary>
        AGW,

        /// <summary>
        /// Current legal structure
        /// </summary>
        AGX,

        /// <summary>
        /// Marital contract
        /// </summary>
        AGY,

        /// <summary>
        /// Marketing activities
        /// </summary>
        AGZ,

        /// <summary>
        /// Merger
        /// </summary>
        AHA,

        /// <summary>
        /// Marketable securities
        /// </summary>
        AHB,

        /// <summary>
        /// Business debt
        /// </summary>
        AHC,

        /// <summary>
        /// Original legal structure
        /// </summary>
        AHD,

        /// <summary>
        /// Employee sharing arrangements
        /// </summary>
        AHE,

        /// <summary>
        /// Organization details
        /// </summary>
        AHF,

        /// <summary>
        /// Public record details
        /// </summary>
        AHG,

        /// <summary>
        /// Price range
        /// </summary>
        AHH,

        /// <summary>
        /// Qualifications
        /// </summary>
        AHI,

        /// <summary>
        /// Registered activity
        /// </summary>
        AHJ,

        /// <summary>
        /// Criminal sentence
        /// </summary>
        AHK,

        /// <summary>
        /// Sales method
        /// </summary>
        AHL,

        /// <summary>
        /// Educational institution information
        /// </summary>
        AHM,

        /// <summary>
        /// Status details
        /// </summary>
        AHN,

        /// <summary>
        /// Sales
        /// </summary>
        AHO,

        /// <summary>
        /// Spouse information
        /// </summary>
        AHP,

        /// <summary>
        /// Educational degree information
        /// </summary>
        AHQ,

        /// <summary>
        /// Shareholding information
        /// </summary>
        AHR,

        /// <summary>
        /// Sales territory
        /// </summary>
        AHS,

        /// <summary>
        /// Accountant's comments
        /// </summary>
        AHT,

        /// <summary>
        /// Exemption law location
        /// </summary>
        AHU,

        /// <summary>
        /// Share classifications
        /// </summary>
        AHV,

        /// <summary>
        /// Forecast
        /// </summary>
        AHW,

        /// <summary>
        /// Event location
        /// </summary>
        AHX,

        /// <summary>
        /// Facility occupancy
        /// </summary>
        AHY,

        /// <summary>
        /// Import and export details
        /// </summary>
        AHZ,

        /// <summary>
        /// Additional facility information
        /// </summary>
        AIA,

        /// <summary>
        /// Inventory value
        /// </summary>
        AIB,

        /// <summary>
        /// Education
        /// </summary>
        AIC,

        /// <summary>
        /// Event
        /// </summary>
        AID,

        /// <summary>
        /// Agent
        /// </summary>
        AIE,

        /// <summary>
        /// Domestically agreed financial statement details
        /// </summary>
        AIF,

        /// <summary>
        /// Other current asset description
        /// </summary>
        AIG,

        /// <summary>
        /// Other current liability description
        /// </summary>
        AIH,

        /// <summary>
        /// Former business activity
        /// </summary>
        AII,

        /// <summary>
        /// Trade name use
        /// </summary>
        AIJ,

        /// <summary>
        /// Signing authority
        /// </summary>
        AIK,

        /// <summary>
        /// Guarantee
        /// </summary>
        AIL,

        /// <summary>
        /// Holding company operation
        /// </summary>
        AIM,

        /// <summary>
        /// Consignment routing
        /// </summary>
        AIN,

        /// <summary>
        /// Letter of protest
        /// </summary>
        AIO,

        /// <summary>
        /// Question
        /// </summary>
        AIP,

        /// <summary>
        /// Party information
        /// </summary>
        AIQ,

        /// <summary>
        /// Area boundaries description
        /// </summary>
        AIR,

        /// <summary>
        /// Advertisement information
        /// </summary>
        AIS,

        /// <summary>
        /// Financial statement details
        /// </summary>
        AIT,

        /// <summary>
        /// Access instructions
        /// </summary>
        AIU,

        /// <summary>
        /// Liquidity
        /// </summary>
        AIV,

        /// <summary>
        /// Credit line
        /// </summary>
        AIW,

        /// <summary>
        /// Warranty terms
        /// </summary>
        AIX,

        /// <summary>
        /// Division description
        /// </summary>
        AIY,

        /// <summary>
        /// Reporting instruction
        /// </summary>
        AIZ,

        /// <summary>
        /// Examination result
        /// </summary>
        AJA,

        /// <summary>
        /// Laboratory result
        /// </summary>
        AJB,

        /// <summary>
        /// Allowance/charge information
        /// </summary>
        ALC,

        /// <summary>
        /// X-ray result
        /// </summary>
        ALD,

        /// <summary>
        /// Pathology result
        /// </summary>
        ALE,

        /// <summary>
        /// Intervention description
        /// </summary>
        ALF,

        /// <summary>
        /// Summary of admittance
        /// </summary>
        ALG,

        /// <summary>
        /// Medical treatment course detail
        /// </summary>
        ALH,

        /// <summary>
        /// Prognosis
        /// </summary>
        ALI,

        /// <summary>
        /// Instruction to patient
        /// </summary>
        ALJ,

        /// <summary>
        /// Instruction to physician
        /// </summary>
        ALK,

        /// <summary>
        /// All documents
        /// </summary>
        ALL,

        /// <summary>
        /// Medicine treatment
        /// </summary>
        ALM,

        /// <summary>
        /// Medicine dosage and administration
        /// </summary>
        ALN,

        /// <summary>
        /// Availability of patient
        /// </summary>
        ALO,

        /// <summary>
        /// Reason for service request
        /// </summary>
        ALP,

        /// <summary>
        /// Purpose of service
        /// </summary>
        ALQ,

        /// <summary>
        /// Arrival conditions
        /// </summary>
        ARR,

        /// <summary>
        /// Service requester's comment
        /// </summary>
        ARS,

        /// <summary>
        /// Authentication
        /// </summary>
        AUT,

        /// <summary>
        /// Requested location description
        /// </summary>
        AUU,

        /// <summary>
        /// Medicine administration condition
        /// </summary>
        AUV,

        /// <summary>
        /// Patient information
        /// </summary>
        AUW,

        /// <summary>
        /// Precautionary measure
        /// </summary>
        AUX,

        /// <summary>
        /// Service characteristic
        /// </summary>
        AUY,

        /// <summary>
        /// Planned event comment
        /// </summary>
        AUZ,

        /// <summary>
        /// Expected delay comment
        /// </summary>
        AVA,

        /// <summary>
        /// Transport requirements comment
        /// </summary>
        AVB,

        /// <summary>
        /// Temporary approval condition
        /// </summary>
        AVC,

        /// <summary>
        /// Customs Valuation Information
        /// </summary>
        AVD,

        /// <summary>
        /// Value Added Tax (VAT) margin scheme
        /// </summary>
        AVE,

        /// <summary>
        /// Maritime Declaration of Health
        /// </summary>
        AVF,

        /// <summary>
        /// Passenger baggage information
        /// </summary>
        BAG,

        /// <summary>
        /// Maritime Declaration of Health
        /// </summary>
        BAH,

        /// <summary>
        /// Additional product information address
        /// </summary>
        BAI,

        /// <summary>
        /// Information to be printed on despatch advice
        /// </summary>
        BAJ,

        /// <summary>
        /// Missing goods remarks
        /// </summary>
        BAK,

        /// <summary>
        /// Non-acceptance information
        /// </summary>
        BAL,

        /// <summary>
        /// Returns information
        /// </summary>
        BAM,

        /// <summary>
        /// Sub-line item information
        /// </summary>
        BAN,

        /// <summary>
        /// Test information
        /// </summary>
        BAO,

        /// <summary>
        /// External link
        /// </summary>
        BAP,

        /// <summary>
        /// VAT exemption reason
        /// </summary>
        BAQ,

        /// <summary>
        /// Processing Instructions
        /// </summary>
        BAR,

        /// <summary>
        /// Relay Instructions
        /// </summary>
        BAS,

        /// <summary>
        /// SIMA applicable
        /// </summary>
        BAT,

        /// <summary>
        /// Appeals program code
        /// </summary>
        BAU,

        /// <summary>
        /// SIMA subject
        /// </summary>
        BAV,

        /// <summary>
        /// Surtax applicable
        /// </summary>
        BAW,

        /// <summary>
        /// SIMA security bond
        /// </summary>
        BAX,

        /// <summary>
        /// Surtax subject
        /// </summary>
        BAY,

        /// <summary>
        /// Safeguard applicable
        /// </summary>
        BAZ,

        /// <summary>
        /// Safeguard applicable
        /// </summary>
        BBA,

        /// <summary>
        /// Safeguard subject
        /// </summary>
        BBB,

        /// <summary>
        /// Transport contract document clause
        /// </summary>
        BLC,

        /// <summary>
        /// Instruction to prepare the patient
        /// </summary>
        BLD,

        /// <summary>
        /// Medicine treatment comment
        /// </summary>
        BLE,

        /// <summary>
        /// Examination result comment
        /// </summary>
        BLF,

        /// <summary>
        /// Service request comment
        /// </summary>
        BLG,

        /// <summary>
        /// Prescription reason
        /// </summary>
        BLH,

        /// <summary>
        /// Prescription comment
        /// </summary>
        BLI,

        /// <summary>
        /// Clinical investigation comment
        /// </summary>
        BLJ,

        /// <summary>
        /// Medicinal specification comment
        /// </summary>
        BLK,

        /// <summary>
        /// Economic contribution comment
        /// </summary>
        BLL,

        /// <summary>
        /// Status of a plan
        /// </summary>
        BLM,

        /// <summary>
        /// Random sample test information
        /// </summary>
        BLN,

        /// <summary>
        /// Period of time
        /// </summary>
        BLO,

        /// <summary>
        /// Legislation
        /// </summary>
        BLP,

        /// <summary>
        /// Security measures requested
        /// </summary>
        BLQ,

        /// <summary>
        /// Transport contract document remark
        /// </summary>
        BLR,

        /// <summary>
        /// Previous port of call security information
        /// </summary>
        BLS,

        /// <summary>
        /// Security information
        /// </summary>
        BLT,

        /// <summary>
        /// Waste information
        /// </summary>
        /// Text describing waste related information.
        BLU,

        /// <summary>
        /// B2C marketing information, short description
        /// </summary>
        BLV,

        /// <summary>
        /// B2B marketing information, long description
        /// </summary>
        BLW,

        /// <summary>
        /// B2C marketing information, long description
        /// </summary>
        BLX,

        /// <summary>
        /// Product ingredients
        /// </summary>
        BLY,

        /// <summary>
        /// Location short name
        /// </summary>
        BLZ,

        /// <summary>
        /// Packaging material information
        /// </summary>
        BMA,

        /// <summary>
        /// Filler material information
        /// </summary>
        BMB,

        /// <summary>
        /// Ship-to-ship activity information
        /// </summary>
        BMC,

        /// <summary>
        /// Package material description
        /// </summary>
        BMD,

        /// <summary>
        /// Consumer level package marking
        /// </summary>
        BME,

        /// <summary>
        /// SIMA measure in force
        /// </summary>
        BMF,

        /// <summary>
        /// Pre-CARM
        /// </summary>
        BMG,

        /// <summary>
        /// SIMA measure type
        /// </summary>
        BMH,

        /// <summary>
        /// Customs clearance instructions
        /// </summary>
        CCI,

        /// <summary>
        /// Sub Type Code
        /// </summary>
        CCJ,

        /// <summary>
        /// SIMA information
        /// </summary>
        CCK,

        /// <summary>
        /// Time limit end
        /// </summary>
        CCL,

        /// <summary>
        /// Time limit start
        /// </summary>
        CCM,

        /// <summary>
        /// Warehouse time limit
        /// </summary>
        CCN,

        /// <summary>
        /// Value for duty information
        /// </summary>
        CCO,

        /// <summary>
        /// Customs clearance instructions export
        /// </summary>
        CEX,

        /// <summary>
        /// Change information
        /// </summary>
        CHG,

        /// <summary>
        /// Customs clearance instruction import
        /// </summary>
        CIP,

        /// <summary>
        /// Clearance place requested
        /// </summary>
        CLP,

        /// <summary>
        /// Loading remarks
        /// </summary>
        CLR,

        /// <summary>
        /// Order information
        /// </summary>
        COI,

        /// <summary>
        /// Customer remarks
        /// </summary>
        CUR,

        /// <summary>
        /// Customs declaration information
        /// </summary>
        /// Note contains customs declaration information.
        CUS,

        /// <summary>
        /// Damage remarks
        /// </summary>
        DAR,

        /// <summary>
        /// Document issuer declaration
        /// </summary>
        DCL,

        /// <summary>
        /// Delivery information
        /// </summary>
        DEL,

        /// <summary>
        /// Delivery instructions
        /// </summary>
        /// The free text contains delivery instructions.
        DIN,

        /// <summary>
        /// Documentation instructions
        /// </summary>
        DOC,

        /// <summary>
        /// Duty declaration
        /// </summary>
        DUT,

        /// <summary>
        /// Effective used routing
        /// </summary>
        EUR,

        /// <summary>
        /// First block to be printed on the transport contract
        /// </summary>
        FBC,

        /// <summary>
        /// Government bill of lading information
        /// </summary>
        GBL,

        /// <summary>
        /// Entire transaction set
        /// </summary>
        GEN,

        /// <summary>
        /// Further information concerning GGVS par. 7
        /// </summary>
        GS7,

        /// <summary>
        /// Consignment handling instruction
        /// </summary>
        HAN,

        /// <summary>
        /// Hazard information
        /// </summary>
        HAZ,

        /// <summary>
        /// Consignment information for consignee
        /// </summary>
        ICN,

        /// <summary>
        /// Insurance instructions
        /// </summary>
        IIN,

        /// <summary>
        /// Invoice mailing instructions
        /// </summary>
        IMI,

        /// <summary>
        /// Commercial invoice item description
        /// </summary>
        IND,

        /// <summary>
        /// Insurance information
        /// </summary>
        INS,

        /// <summary>
        /// Invoice instruction
        /// </summary>
        INV,

        /// <summary>
        /// Information for railway purpose
        /// </summary>
        IRP,

        /// <summary>
        /// Inland transport details
        /// </summary>
        ITR,

        /// <summary>
        /// Testing instructions
        /// </summary>
        ITS,

        /// <summary>
        /// Location Alias
        /// </summary>
        LAN,

        /// <summary>
        /// Line item
        /// </summary>
        LIN,

        /// <summary>
        /// Loading instruction
        /// </summary>
        LOI,

        /// <summary>
        /// Miscellaneous charge order
        /// </summary>
        MCO,

        /// <summary>
        /// Maritime Declaration of Health
        /// </summary>
        MDH,

        /// <summary>
        /// Additional marks/numbers information
        /// </summary>
        MKS,

        /// <summary>
        /// Order instruction
        /// </summary>
        ORI,

        /// <summary>
        /// Other service information
        /// </summary>
        OSI,

        /// <summary>
        /// Packing/marking information
        /// </summary>
        PAC,

        /// <summary>
        /// Payment instructions information
        /// </summary>
        PAI,

        /// <summary>
        /// Payables information
        /// </summary>
        PAY,

        /// <summary>
        /// Packaging information
        /// </summary>
        PKG,

        /// <summary>
        /// Packaging terms information
        /// </summary>
        PKT,

        /// <summary>
        /// Payment detail/remittance information
        /// </summary>
        /// The free text contains payment details.
        PMD,

        /// <summary>
        /// Payment information
        ///
        /// Zahlungsinformation
        ///
        /// Bekanntgabe der Abtretung der
        /// Forderung (Zession)
        /// </summary>
        PMT,

        /// <summary>
        /// Product information
        /// </summary>
        PRD,

        /// <summary>
        /// Price calculation formula
        ///
        /// Preiskalkulationsschema
        ///
        /// Zum Beispiel Angabe Zählerstand,
        /// Zähler etc. oder andere Hinweise
        /// bezüglich Abrechnung.
        /// </summary>
        PRF,

        /// <summary>
        /// Priority information
        /// </summary>
        PRI,

        /// <summary>
        /// Purchasing information
        /// </summary>
        PUR,

        /// <summary>
        /// Quarantine instructions
        /// </summary>
        QIN,

        /// <summary>
        /// Quality demands/requirements
        /// </summary>
        QQD,

        /// <summary>
        /// Quotation instruction/information
        /// </summary>
        QUT,

        /// <summary>
        /// Risk and handling information
        /// </summary>
        RAH,

        /// <summary>
        /// Regulatory information
        ///
        /// Regulatorische Informationen
        ///
        /// Angaben zum leistenden Unternehmen
        /// (Angabe Geschäftsführer, HR-Nummer
        /// etc.)
        /// </summary>
        REG,

        /// <summary>
        /// Return to origin information
        /// </summary>
        RET,

        /// <summary>
        /// Receivables
        /// </summary>
        REV,

        /// <summary>
        /// Consignment route
        /// </summary>
        RQR,

        /// <summary>
        /// Safety information
        /// </summary>
        SAF,

        /// <summary>
        /// Consignment documentary instruction
        /// </summary>
        SIC,

        /// <summary>
        /// Special instructions
        /// </summary>
        SIN,

        /// <summary>
        /// Ship line requested
        /// </summary>
        SLR,

        /// <summary>
        /// Special permission for transport, generally
        /// </summary>
        SPA,

        /// <summary>
        /// Special permission concerning the goods to be transported
        /// </summary>
        SPG,

        /// <summary>
        /// Special handling
        /// </summary>
        SPH,

        /// <summary>
        /// Special permission concerning package
        /// </summary>
        SPP,

        /// <summary>
        /// Special permission concerning transport means
        /// </summary>
        SPT,

        /// <summary>
        /// Subsidiary risk number (IATA/DGR)
        /// </summary>
        SRN,

        /// <summary>
        /// Special service request
        /// </summary>
        SSR,

        /// <summary>
        /// Supplier remarks
        /// Remarks from or for a supplier of goods or services.
        /// </summary>
        SUR,

        /// <summary>
        /// Consignment tariff
        /// </summary>
        TCA,

        /// <summary>
        /// Consignment transport
        /// </summary>
        TDT,

        /// <summary>
        /// Transportation information
        /// </summary>
        TRA,

        /// <summary>
        /// Requested tariff
        /// </summary>
        TRR,

        /// <summary>
        /// Tax declaration
        ///
        /// Grund der Steuerbefreiung
        /// </summary>
        TXD,

        /// <summary>
        /// Warehouse instruction/information
        ///
        /// Note contains warehouse information.
        /// </summary>
        WHI,

        /// <summary>
        /// Mutually defined
        ///
        /// Note contains information mutually defined by trading partners.
        /// </summary>
        ZZZ
    }
}
