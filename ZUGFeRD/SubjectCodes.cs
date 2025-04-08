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
    /// http://www.stylusstudio.com/edifact/D02A/4451.htm
	/// https://www.xrepository.de/details/urn:xoev-de:kosit:codeliste:untdid.4451_4#version
    /// </summary>
    public enum SubjectCodes
    {
        /// <summary>
        /// Unknon/ invalid subject code
        /// </summary>
        Unknown,
		
        /// <summary>
        /// Goods item description
        /// [7002] Plain language description of the nature of a goods item sufficient to identify it for customs, statistical or transport purposes.
        /// </summary>
        AAA,

        /// <summary>
        /// Payment term
        /// [4276] Free form description of the conditions of payment between the parties to a transaction.
        /// </summary>
        AAB,

        /// <summary>
        /// Dangerous goods additional information
        /// [7488] Additional information concerning dangerous substances and/or article in a consignment.
        /// </summary>
        AAC,

        /// <summary>
        /// Dangerous goods technical name
        /// [7254] Proper shipping name, supplemented as necessary with the correct technical name, by which a dangerous substance or article may be correctly identified, or which is sufficiently informative to permit identification by reference to generally available literature.
        /// </summary>
        AAD,

        /// <summary>
        /// Acknowledgement description
        /// The content of an acknowledgement.
        /// </summary>
        AAE,

        /// <summary>
        /// Rate additional information
        /// Specific details applying to rates.
        /// </summary>
        AAF,

        /// <summary>
        /// Party instructions
        /// Indicates that the segment contains instructions to be passed on to the identified party.
        /// </summary>
        AAG,

        /// <summary>
        /// General information
        /// The text contains general information.
        /// </summary>
        AAI,

        /// <summary>
        /// Additional conditions of sale/purchase
        /// Additional conditions specific to this order or project.
        /// </summary>
        AAJ,

        /// <summary>
        /// Price conditions
        /// Information on the price conditions that are expected or given.
        /// </summary>
        AAK,

        /// <summary>
        /// Goods dimensions in characters
        /// Expression of a number in characters as length of ten meters.
        /// </summary>
        AAL,

        /// <summary>
        /// Equipment re-usage restrictions
        /// Technical or commercial reasons why a piece of equipment may not be re-used after the current transport terminates.
        /// </summary>
        AAM,

        /// <summary>
        /// Handling restriction
        /// Restrictions in handling depending on the technical characteristics of the piece of equipment or on the nature of the goods.
        /// </summary>
        AAN,

        /// <summary>
        /// Error description (free text)
        /// Error described by a free text.
        /// </summary>
        AAO,

        /// <summary>
        /// Response (free text)
        /// Free text of the response to a communication.
        /// </summary>
        AAP,

        /// <summary>
        /// Package content's description
        /// A description of the contents of a package.
        /// </summary>
        AAQ,

        /// <summary>
        /// Terms of delivery
        /// (4053) Free text of the non Incoterms terms of delivery. For Incoterms, use: 4053.
        /// </summary>
        AAR,

        /// <summary>
        /// Bill of lading remarks
        /// The remarks printed or to be printed on a bill of lading.
        /// </summary>
        AAS,

        /// <summary>
        /// Mode of settlement information
        /// Free text information on an IATA Air Waybill to indicate means by which account is to be settled.
        /// </summary>
        AAT,

        /// <summary>
        /// Consignment invoice information
        /// Information pertaining to the invoice covering the consignment.
        /// </summary>
        AAU,

        /// <summary>
        /// Clearance invoice information
        /// Information pertaining to the invoice covering clearance of the cargo.
        /// </summary>
        AAV,

        /// <summary>
        /// Letter of credit information
        /// Information pertaining to the letter of credit.
        /// </summary>
        AAW,

        /// <summary>
        /// License information
        /// Information pertaining to a license.
        /// </summary>
        AAX,

        /// <summary>
        /// Certification statements
        /// The text contains certification statements.
        /// </summary>
        AAY,

        /// <summary>
        /// Additional export information
        /// The text contains additional export information.
        /// </summary>
        AAZ,

        /// <summary>
        /// Tariff statements
        /// Description of parameters relating to a tariff.
        /// </summary>
        ABA,

        /// <summary>
        /// Medical history
        /// Historical details of a patients medical events.
        /// </summary>
        ABB,

        /// <summary>
        /// Conditions of sale or purchase
        /// (4490) (4372) Additional information regarding terms and conditions which apply to the transaction.
        /// </summary>
        ABC,

        /// <summary>
        /// Contract document type
        /// [4422] Textual representation of the type of contract.
        /// </summary>
        ABD,

        /// <summary>
        /// Additional terms and/or conditions (documentary credit)
        /// (4260) Additional terms and/or conditions to the documentary credit.
        /// </summary>
        ABE,

        /// <summary>
        /// Instructions or information about standby documentary credit
        /// Instruction or information about a standby documentary credit.
        /// </summary>
        ABF,

        /// <summary>
        /// Instructions or information about partial shipment(s)
        /// Instructions or information about partial shipment(s).
        /// </summary>
        ABG,

        /// <summary>
        /// Instructions or information about transhipment(s)
        /// Instructions or information about transhipment(s).
        /// </summary>
        ABH,

        /// <summary>
        /// Additional handling instructions documentary credit
        /// Additional handling instructions for a documentary credit.
        /// </summary>
        ABI,

        /// <summary>
        /// Domestic routing information
        /// Information regarding the domestic routing.
        /// </summary>
        ABJ,

        /// <summary>
        /// Chargeable category of equipment
        /// Equipment types are coded by category for financial purposes.
        /// </summary>
        ABK,

        /// <summary>
        /// Government information
        /// Information pertaining to government.
        /// </summary>
        ABL,

        /// <summary>
        /// Onward routing information
        /// The text contains onward routing information.
        /// </summary>
        ABM,

        /// <summary>
        /// Accounting information
        /// [4410] The text contains information related to accounting.
        /// </summary>
        ABN,

        /// <summary>
        /// Discrepancy information
        /// Free text or coded information to indicate a specific discrepancy.
        /// </summary>
        ABO,

        /// <summary>
        /// Confirmation instructions
        /// Documentary credit confirmation instructions.
        /// </summary>
        ABP,

        /// <summary>
        /// Method of issuance
        /// Method of issuance of documentary credit.
        /// </summary>
        ABQ,

        /// <summary>
        /// Documents delivery instructions
        /// Delivery instructions for documents required under a documentary credit.
        /// </summary>
        ABR,

        /// <summary>
        /// Additional conditions
        /// Additional conditions to the issuance of a documentary credit.
        /// </summary>
        ABS,

        /// <summary>
        /// Information/instructions about additional amounts covered
        /// Additional amounts information/instruction.
        /// </summary>
        ABT,

        /// <summary>
        /// Deferred payment termed additional
        /// Additional terms concerning deferred payment.
        /// </summary>
        ABU,

        /// <summary>
        /// Acceptance terms additional
        /// Additional terms concerning acceptance.
        /// </summary>
        ABV,

        /// <summary>
        /// Negotiation terms additional
        /// Additional terms concerning negotiation.
        /// </summary>
        ABW,

        /// <summary>
        /// Document name and documentary requirements
        /// Document name and documentary requirements.
        /// </summary>
        ABX,

        /// <summary>
        /// Instructions/information about revolving documentary credit
        /// Instructions/information about a revolving documentary credit.
        /// </summary>
        ABZ,

        /// <summary>
        /// Documentary requirements
        /// Specification of the documentary requirements.
        /// </summary>
        ACA,

        /// <summary>
        /// Additional information
        /// (4270) The text contains additional information.
        /// </summary>
        ACB,

        /// <summary>
        /// Factor assignment clause
        /// Assignment based on an agreement between seller and factor.
        /// </summary>
        ACC,

        /// <summary>
        /// Reason
        /// Reason for a request or response.
        /// </summary>
        ACD,

        /// <summary>
        /// Dispute
        /// A notice, usually from buyer to seller, that something was found wrong with goods delivered or the services rendered, or with the related invoice.
        /// </summary>
        ACE,

        /// <summary>
        /// Additional attribute information
        /// The text refers to information about an additional attribute not otherwise specified.
        /// </summary>
        ACF,

        /// <summary>
        /// Absence declaration
        /// A declaration on the reason of the absence.
        /// </summary>
        ACG,

        /// <summary>
        /// Aggregation statement
        /// A statement on the way a specific variable or set of variables has been aggregated.
        /// </summary>
        ACH,

        /// <summary>
        /// Compilation statement
        /// A statement on the compilation status of an array or other set of figures or calculations.
        /// </summary>
        ACI,

        /// <summary>
        /// Definitional exception
        /// An exception to the agreed definition of a term, concept, formula or other object.
        /// </summary>
        ACJ,

        /// <summary>
        /// Privacy statement
        /// A statement on the privacy or confidential nature of an object.
        /// </summary>
        ACK,

        /// <summary>
        /// Quality statement
        /// A statement on the quality of an object.
        /// </summary>
        ACL,

        /// <summary>
        /// Statistical description
        /// The description of a statistical object such as a value list, concept, or structure definition.
        /// </summary>
        ACM,

        /// <summary>
        /// Statistical definition
        /// The definition of a statistical object such as a value list, concept, or structure definition.
        /// </summary>
        ACN,

        /// <summary>
        /// Statistical name
        /// The name of a statistical object such as a value list, concept or structure definition.
        /// </summary>
        ACO,

        /// <summary>
        /// Statistical title
        /// The title of a statistical object such as a value list, concept, or structure definition.
        /// </summary>
        ACP,

        /// <summary>
        /// Off-dimension information
        /// Information relating to differences between the actual transport dimensions and the normally applicable dimensions.
        /// </summary>
        ACQ,

        /// <summary>
        /// Unexpected stops information
        /// Information relating to unexpected stops during a conveyance.
        /// </summary>
        ACR,

        /// <summary>
        /// Principles
        /// Text subject is principles section of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ACS,

        /// <summary>
        /// Terms and definition
        /// Text subject is terms and definition section of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ACT,

        /// <summary>
        /// Segment name
        /// Text subject is segment name.
        /// </summary>
        ACU,

        /// <summary>
        /// Simple data element name
        /// Text subject is name of simple data element.
        /// </summary>
        ACV,

        /// <summary>
        /// Scope
        /// Text subject is scope section of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ACW,

        /// <summary>
        /// Message type name
        /// Text subject is name of message type.
        /// </summary>
        ACX,

        /// <summary>
        /// Introduction
        /// Text subject is introduction section of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ACY,

        /// <summary>
        /// Glossary
        /// Text subject is glossary section of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ACZ,

        /// <summary>
        /// Functional definition
        /// Text subject is functional definition section of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ADA,

        /// <summary>
        /// Examples
        /// Text subject is examples as given in the example(s) section of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ADB,

        /// <summary>
        /// Cover page
        /// Text subject is cover page of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ADC,

        /// <summary>
        /// Dependency (syntax) notes
        /// Denotes that the associated text is a dependency (syntax) note.
        /// </summary>
        ADD,

        /// <summary>
        /// Code value name
        /// Text subject is name of code value.
        /// </summary>
        ADE,

        /// <summary>
        /// Code list name
        /// Text subject is name of code list.
        /// </summary>
        ADF,

        /// <summary>
        /// Clarification of usage
        /// Text subject is an explanation of the intended usage of a segment or segment group.
        /// </summary>
        ADG,

        /// <summary>
        /// Composite data element name
        /// Text subject is name of composite data element.
        /// </summary>
        ADH,

        /// <summary>
        /// Field of application
        /// Text subject is field of application of the UN/EDIFACT rules for presentation of standardized message and directories documentation.
        /// </summary>
        ADI,

        /// <summary>
        /// Type of assets and liabilities
        /// Information describing the type of assets and liabilities.
        /// </summary>
        ADJ,

        /// <summary>
        /// Promotion information
        /// The text contains information about a promotion.
        /// </summary>
        ADK,

        /// <summary>
        /// Meter condition
        /// Description of the condition of a meter.
        /// </summary>
        ADL,

        /// <summary>
        /// Meter reading information
        /// Information related to a particular reading of a meter.
        /// </summary>
        ADM,

        /// <summary>
        /// Type of transaction reason
        /// Information describing the type of the reason of transaction.
        /// </summary>
        ADN,

        /// <summary>
        /// Type of survey question
        /// Type of survey question.
        /// </summary>
        ADO,

        /// <summary>
        /// Carrier's agent counter information
        /// Information for use at the counter of the carrier's agent.
        /// </summary>
        ADP,

        /// <summary>
        /// Description of work item on equipment
        /// Description or code for the operation to be executed on the equipment.
        /// </summary>
        ADQ,

        /// <summary>
        /// Message definition
        /// Text subject is message definition.
        /// </summary>
        ADR,

        /// <summary>
        /// Booked item information
        /// Information pertaining to a booked item.
        /// </summary>
        ADS,

        /// <summary>
        /// Source of document
        /// Text subject is source of document.
        /// </summary>
        ADT,

        /// <summary>
        /// Note
        /// Text subject is note.
        /// </summary>
        ADU,

        /// <summary>
        /// Fixed part of segment clarification text
        /// Text subject is fixed part of segment clarification text.
        /// </summary>
        ADV,

        /// <summary>
        /// Characteristics of goods
        /// Description of the characteristic of goods in addition to the description of the goods.
        /// </summary>
        ADW,

        /// <summary>
        /// Additional discharge instructions
        /// Special discharge instructions concerning the goods.
        /// </summary>
        ADX,

        /// <summary>
        /// Container stripping instructions
        /// Instructions regarding the stripping of container(s).
        /// </summary>
        ADY,

        /// <summary>
        /// CSC (Container Safety Convention) plate information
        /// Information on the CSC (Container Safety Convention) plate that is attached to the container.
        /// </summary>
        ADZ,

        /// <summary>
        /// Cargo remarks
        /// Additional remarks concerning the cargo.
        /// </summary>
        AEA,

        /// <summary>
        /// Temperature control instructions
        /// Instruction regarding the temperature control of the cargo.
        /// </summary>
        AEB,

        /// <summary>
        /// Text refers to expected data
        /// Remarks refer to data that was expected.
        /// </summary>
        AEC,

        /// <summary>
        /// Text refers to received data
        /// Remarks refer to data that was received.
        /// </summary>
        AED,

        /// <summary>
        /// Section clarification text
        /// Text subject is section clarification text.
        /// </summary>
        AEE,

        /// <summary>
        /// Information to the beneficiary
        /// Information given to the beneficiary.
        /// </summary>
        AEF,

        /// <summary>
        /// Information to the applicant
        /// Information given to the applicant.
        /// </summary>
        AEG,

        /// <summary>
        /// Instructions to the beneficiary
        /// Instructions made to the beneficiary.
        /// </summary>
        AEH,

        /// <summary>
        /// Instructions to the applicant
        /// Instructions given to the applicant.
        /// </summary>
        AEI,

        /// <summary>
        /// Controlled atmosphere
        /// Information about the controlled atmosphere.
        /// </summary>
        AEJ,

        /// <summary>
        /// Take off annotation
        /// Additional information in plain text to support a take off annotation. Taking off is the process of assessing the quantity work from extracting the measurement from construction documentation.
        /// </summary>
        AEK,

        /// <summary>
        /// Price variation narrative
        /// Additional information in plain language to support a price variation.
        /// </summary>
        AEL,

        /// <summary>
        /// Documentary credit amendment instructions
        /// Documentary credit amendment instructions.
        /// </summary>
        AEM,

        /// <summary>
        /// Standard method narrative
        /// Additional information in plain language to support a standard method.
        /// </summary>
        AEN,

        /// <summary>
        /// Project narrative
        /// Additional information in plain language to support the project.
        /// </summary>
        AEO,

        /// <summary>
        /// Radioactive goods, additional information
        /// Additional information related to radioactive goods.
        /// </summary>
        AEP,

        /// <summary>
        /// Bank-to-bank information
        /// Information given from one bank to another.
        /// </summary>
        AEQ,

        /// <summary>
        /// Reimbursement instructions
        /// Instructions given for reimbursement purposes.
        /// </summary>
        AER,

        /// <summary>
        /// Reason for amending a message
        /// Identification of the reason for amending a message.
        /// </summary>
        AES,

        /// <summary>
        /// Instructions to the paying and/or accepting and/or negotiating bank
        /// Instructions to the paying and/or accepting and/or negotiating bank.
        /// </summary>
        AET,

        /// <summary>
        /// Interest instructions
        /// Instructions given about the interest.
        /// </summary>
        AEU,

        /// <summary>
        /// Agent commission
        /// Instructions about agent commission.
        /// </summary>
        AEV,

        /// <summary>
        /// Remitting bank instructions
        /// Instructions to the remitting bank.
        /// </summary>
        AEW,

        /// <summary>
        /// Instructions to the collecting bank
        /// Instructions to the bank, other than the remitting bank, involved in processing the collection.
        /// </summary>
        AEX,

        /// <summary>
        /// Collection amount instructions
        /// Instructions about the collection amount.
        /// </summary>
        AEY,

        /// <summary>
        /// Internal auditing information
        /// Text relating to internal auditing information.
        /// </summary>
        AEZ,

        /// <summary>
        /// Constraint
        /// Denotes that the associated text is a constraint.
        /// </summary>
        AFA,

        /// <summary>
        /// Comment
        /// Denotes that the associated text is a comment.
        /// </summary>
        AFB,

        /// <summary>
        /// Semantic note
        /// Denotes that the associated text is a semantic note.
        /// </summary>
        AFC,

        /// <summary>
        /// Help text
        /// Denotes that the associated text is an item of help text.
        /// </summary>
        AFD,

        /// <summary>
        /// Legend
        /// Denotes that the associated text is a legend.
        /// </summary>
        AFE,

        /// <summary>
        /// Batch code structure
        /// A description of the structure of a batch code.
        /// </summary>
        AFF,

        /// <summary>
        /// Product application
        /// A general description of the application of a product.
        /// </summary>
        AFG,

        /// <summary>
        /// Customer complaint
        /// Complaint of customer.
        /// </summary>
        AFH,

        /// <summary>
        /// Probable cause of fault
        /// The probable cause of fault.
        /// </summary>
        AFI,

        /// <summary>
        /// Defect description
        /// Description of the defect.
        /// </summary>
        AFJ,

        /// <summary>
        /// Repair description
        /// The description of the work performed during the repair.
        /// </summary>
        AFK,

        /// <summary>
        /// Review comments
        /// Comments relevant to a review.
        /// </summary>
        AFL,

        /// <summary>
        /// Title
        /// Denotes that the associated text is a title.
        /// </summary>
        AFM,

        /// <summary>
        /// Description of amount
        /// An amount description in clear text.
        /// </summary>
        AFN,

        /// <summary>
        /// Responsibilities
        /// Information describing the responsibilities.
        /// </summary>
        AFO,

        /// <summary>
        /// Supplier
        /// Information concerning suppliers.
        /// </summary>
        AFP,

        /// <summary>
        /// Purchase region
        /// Information concerning the region(s) where purchases are made.
        /// </summary>
        AFQ,

        /// <summary>
        /// Affiliation
        /// Information concerning an association of one party with another party(ies).
        /// </summary>
        AFR,

        /// <summary>
        /// Borrower
        /// Information concerning the borrower.
        /// </summary>
        AFS,

        /// <summary>
        /// Line of business
        /// Information concerning an entity's line of business.
        /// </summary>
        AFT,

        /// <summary>
        /// Financial institution
        /// Description of financial institution(s) used by an entity.
        /// </summary>
        AFU,

        /// <summary>
        /// Business founder
        /// Information about the business founder.
        /// </summary>
        AFV,

        /// <summary>
        /// Business history
        /// Description of the business history.
        /// </summary>
        AFW,

        /// <summary>
        /// Banking arrangements
        /// Information concerning the general banking arrangements.
        /// </summary>
        AFX,

        /// <summary>
        /// Business origin
        /// Description of the business origin.
        /// </summary>
        AFY,

        /// <summary>
        /// Brand names' description
        /// Description of the entity's brands.
        /// </summary>
        AFZ,

        /// <summary>
        /// Business financing details
        /// Details about the financing of the business.
        /// </summary>
        AGA,

        /// <summary>
        /// Competition
        /// Information concerning an entity's competition.
        /// </summary>
        AGB,

        /// <summary>
        /// Construction process details
        /// Details about the construction process.
        /// </summary>
        AGC,

        /// <summary>
        /// Construction specialty
        /// Information concerning the line of business of a construction entity.
        /// </summary>
        AGD,

        /// <summary>
        /// Contract information
        /// Details about contract(s).
        /// </summary>
        AGE,

        /// <summary>
        /// Corporate filing
        /// Details about a corporate filing.
        /// </summary>
        AGF,

        /// <summary>
        /// Customer information
        /// Description of customers.
        /// </summary>
        AGG,

        /// <summary>
        /// Copyright notice
        /// Information concerning the copyright notice.
        /// </summary>
        AGH,

        /// <summary>
        /// Contingent debt
        /// Details about the contingent debt.
        /// </summary>
        AGI,

        /// <summary>
        /// Conviction details
        /// Details about the law or penal codes that resulted in conviction.
        /// </summary>
        AGJ,

        /// <summary>
        /// Equipment
        /// Description of equipment.
        /// </summary>
        AGK,

        /// <summary>
        /// Workforce description
        /// Comments about the workforce.
        /// </summary>
        AGL,

        /// <summary>
        /// Exemption
        /// Description about exemptions.
        /// </summary>
        AGM,

        /// <summary>
        /// Future plans
        /// Information on future plans.
        /// </summary>
        AGN,

        /// <summary>
        /// Interviewee conversation information
        /// Information concerning the interviewee conversation.
        /// </summary>
        AGO,

        /// <summary>
        /// Intangible asset
        /// Description of intangible asset(s).
        /// </summary>
        AGP,

        /// <summary>
        /// Inventory
        /// Description of the inventory.
        /// </summary>
        AGQ,

        /// <summary>
        /// Investment
        /// Description of the investments.
        /// </summary>
        AGR,

        /// <summary>
        /// Intercompany relations information
        /// Description of the intercompany relations.
        /// </summary>
        AGS,

        /// <summary>
        /// Joint venture
        /// Description of the joint venture.
        /// </summary>
        AGT,

        /// <summary>
        /// Loan
        /// Description of a loan.
        /// </summary>
        AGU,

        /// <summary>
        /// Long term debt
        /// Description of the long term debt.
        /// </summary>
        AGV,

        /// <summary>
        /// Location
        /// Description of a location.
        /// </summary>
        AGW,

        /// <summary>
        /// Current legal structure
        /// Details on the current legal structure.
        /// </summary>
        AGX,

        /// <summary>
        /// Marital contract
        /// Details on a marital contract.
        /// </summary>
        AGY,

        /// <summary>
        /// Marketing activities
        /// Information concerning marketing activities.
        /// </summary>
        AGZ,

        /// <summary>
        /// Merger
        /// Description of a merger.
        /// </summary>
        AHA,

        /// <summary>
        /// Marketable securities
        /// Description of the marketable securities.
        /// </summary>
        AHB,

        /// <summary>
        /// Business debt
        /// Description of the business debt(s).
        /// </summary>
        AHC,

        /// <summary>
        /// Original legal structure
        /// Information concerning the original legal structure.
        /// </summary>
        AHD,

        /// <summary>
        /// Employee sharing arrangements
        /// Information describing how a company uses employees from another company.
        /// </summary>
        AHE,

        /// <summary>
        /// Organization details
        /// Description about the organization of a company.
        /// </summary>
        AHF,

        /// <summary>
        /// Public record details
        /// Information concerning public records.
        /// </summary>
        AHG,

        /// <summary>
        /// Price range
        /// Information concerning the price range of products made or sold.
        /// </summary>
        AHH,

        /// <summary>
        /// Qualifications
        /// Information on the accomplishments fitting a party for a position.
        /// </summary>
        AHI,

        /// <summary>
        /// Registered activity
        /// Information concerning the registered activity.
        /// </summary>
        AHJ,

        /// <summary>
        /// Criminal sentence
        /// Description of the sentence imposed in a criminal proceeding.
        /// </summary>
        AHK,

        /// <summary>
        /// Sales method
        /// Description of the selling means.
        /// </summary>
        AHL,

        /// <summary>
        /// Educational institution information
        /// Free form description relating to the school(s) attended.
        /// </summary>
        AHM,

        /// <summary>
        /// Status details
        /// Describes the status details.
        /// </summary>
        AHN,

        /// <summary>
        /// Sales
        /// Description of the sales.
        /// </summary>
        AHO,

        /// <summary>
        /// Spouse information
        /// Information about the spouse.
        /// </summary>
        AHP,

        /// <summary>
        /// Educational degree information
        /// Details about the educational degree received from a school.
        /// </summary>
        AHQ,

        /// <summary>
        /// Shareholding information
        /// General description of shareholding.
        /// </summary>
        AHR,

        /// <summary>
        /// Sales territory
        /// Information on the sales territory.
        /// </summary>
        AHS,

        /// <summary>
        /// Accountant's comments
        /// Comments made by an accountant regarding a financial statement.
        /// </summary>
        AHT,

        /// <summary>
        /// Exemption law location
        /// Description of the exemption provided to a location by a law.
        /// </summary>
        AHU,

        /// <summary>
        /// Share classifications
        /// Information about the classes or categories of shares.
        /// </summary>
        AHV,

        /// <summary>
        /// Forecast
        /// Description of a prediction.
        /// </summary>
        AHW,

        /// <summary>
        /// Event location
        /// Description of the location of an event.
        /// </summary>
        AHX,

        /// <summary>
        /// Facility occupancy
        /// Information related to occupancy of a facility.
        /// </summary>
        AHY,

        /// <summary>
        /// Import and export details
        /// Specific information provided about the importation and exportation of goods.
        /// </summary>
        AHZ,

        /// <summary>
        /// Additional facility information
        /// Additional information about a facility.
        /// </summary>
        AIA,

        /// <summary>
        /// Inventory value
        /// Description of the value of inventory.
        /// </summary>
        AIB,

        /// <summary>
        /// Education
        /// Description of the education of a person.
        /// </summary>
        AIC,

        /// <summary>
        /// Event
        /// Description of a thing that happens or takes place.
        /// </summary>
        AID,

        /// <summary>
        /// Agent
        /// Information about agents the entity uses.
        /// </summary>
        AIE,

        /// <summary>
        /// Domestically agreed financial statement details
        /// Details of domestically agreed financial statement.
        /// </summary>
        AIF,

        /// <summary>
        /// Other current asset description
        /// Description of other current asset.
        /// </summary>
        AIG,

        /// <summary>
        /// Other current liability description
        /// Description of other current liability.
        /// </summary>
        AIH,

        /// <summary>
        /// Former business activity
        /// Description of the former line of business.
        /// </summary>
        AII,

        /// <summary>
        /// Trade name use
        /// Description of how a trading name is used.
        /// </summary>
        AIJ,

        /// <summary>
        /// Signing authority
        /// Description of the authorized signatory.
        /// </summary>
        AIK,

        /// <summary>
        /// Guarantee
        /// [4376] Description of guarantee.
        /// </summary>
        AIL,

        /// <summary>
        /// Holding company operation
        /// Description of the operation of a holding company.
        /// </summary>
        AIM,

        /// <summary>
        /// Consignment routing
        /// Information on routing of the consignment.
        /// </summary>
        AIN,

        /// <summary>
        /// Letter of protest
        /// A letter citing any condition in dispute.
        /// </summary>
        AIO,

        /// <summary>
        /// Question
        /// A free text question.
        /// </summary>
        AIP,

        /// <summary>
        /// Party information
        /// Free text information related to a party.
        /// </summary>
        AIQ,

        /// <summary>
        /// Area boundaries description
        /// Description of the boundaries of a geographical area.
        /// </summary>
        AIR,

        /// <summary>
        /// Advertisement information
        /// The free text contains advertisement information.
        /// </summary>
        AIS,

        /// <summary>
        /// Financial statement details
        /// Details regarding the financial statement in free text.
        /// </summary>
        AIT,

        /// <summary>
        /// Access instructions
        /// Description of how to access an entity.
        /// </summary>
        AIU,

        /// <summary>
        /// Liquidity
        /// Description of an entity's liquidity.
        /// </summary>
        AIV,

        /// <summary>
        /// Credit line
        /// Description of the line of credit available to an entity.
        /// </summary>
        AIW,

        /// <summary>
        /// Warranty terms
        /// Text describing the terms of warranty which apply to a product or service.
        /// </summary>
        AIX,

        /// <summary>
        /// Division description
        /// Plain language description of a division of an entity.
        /// </summary>
        AIY,

        /// <summary>
        /// Reporting instruction
        /// Instruction on how to report.
        /// </summary>
        AIZ,

        /// <summary>
        /// Examination result
        /// The result of an examination.
        /// </summary>
        AJA,

        /// <summary>
        /// Laboratory result
        /// The result of a laboratory investigation.
        /// </summary>
        AJB,

        /// <summary>
        /// Allowance/charge information
        /// Information referring to allowance/charge.
        /// </summary>
        ALC,

        /// <summary>
        /// X-ray result
        /// The result of an X-ray examination.
        /// </summary>
        ALD,

        /// <summary>
        /// Pathology result
        /// The result of a pathology investigation.
        /// </summary>
        ALE,

        /// <summary>
        /// Intervention description
        /// Details of an intervention.
        /// </summary>
        ALF,

        /// <summary>
        /// Summary of admittance
        /// Summary description of admittance.
        /// </summary>
        ALG,

        /// <summary>
        /// Medical treatment course detail
        /// Details of a course of medical treatment.
        /// </summary>
        ALH,

        /// <summary>
        /// Prognosis
        /// Details of a prognosis.
        /// </summary>
        ALI,

        /// <summary>
        /// Instruction to patient
        /// Instruction given to a patient.
        /// </summary>
        ALJ,

        /// <summary>
        /// Instruction to physician
        /// Instruction given to a physician.
        /// </summary>
        ALK,

        /// <summary>
        /// All documents
        /// The note implies to all documents.
        /// </summary>
        ALL,

        /// <summary>
        /// Medicine treatment
        /// Details of medicine treatment.
        /// </summary>
        ALM,

        /// <summary>
        /// Medicine dosage and administration
        /// Details of medicine dosage and method of administration.
        /// </summary>
        ALN,

        /// <summary>
        /// Availability of patient
        /// Details of when and/or where the patient is available.
        /// </summary>
        ALO,

        /// <summary>
        /// Reason for service request
        /// Details of the reason for a requested service.
        /// </summary>
        ALP,

        /// <summary>
        /// Purpose of service
        /// Details of the purpose of a service.
        /// </summary>
        ALQ,

        /// <summary>
        /// Arrival conditions
        /// Conditions under which arrival takes place.
        /// </summary>
        ARR,

        /// <summary>
        /// Service requester's comment
        /// Comment by the requester of a service.
        /// </summary>
        ARS,

        /// <summary>
        /// Authentication
        /// (4130) (4136) (4426) Name, code, password etc. given for authentication purposes.
        /// </summary>
        AUT,

        /// <summary>
        /// Requested location description
        /// The description of the location requested.
        /// </summary>
        AUU,

        /// <summary>
        /// Medicine administration condition
        /// The event or condition that initiates the administration of a single dose of medicine or a period of treatment.
        /// </summary>
        AUV,

        /// <summary>
        /// Patient information
        /// Information concerning a patient.
        /// </summary>
        AUW,

        /// <summary>
        /// Precautionary measure
        /// Action to be taken to avert possible harmful affects.
        /// </summary>
        AUX,

        /// <summary>
        /// Service characteristic
        /// Free text description is related to a service characteristic.
        /// </summary>
        AUY,

        /// <summary>
        /// Planned event comment
        /// Comment about an event that is planned.
        /// </summary>
        AUZ,

        /// <summary>
        /// Expected delay comment
        /// Comment about the expected delay.
        /// </summary>
        AVA,

        /// <summary>
        /// Transport requirements comment
        /// Comment about the requirements for transport.
        /// </summary>
        AVB,

        /// <summary>
        /// Temporary approval condition
        /// The condition under which the approval is considered.
        /// </summary>
        AVC,

        /// <summary>
        /// Customs Valuation Information
        /// Information provided in this category will be used by the trader to make certain declarations in relation to Customs Valuation.
        /// </summary>
        AVD,

        /// <summary>
        /// Value Added Tax (VAT) margin scheme
        /// Description of the VAT margin scheme applied.
        /// </summary>
        AVE,

        /// <summary>
        /// Maritime Declaration of Health
        /// Information about Maritime Declaration of Health.
        /// </summary>
        AVF,

        /// <summary>
        /// Passenger baggage information
        /// Information related to baggage tendered by a passenger, such as odd size indication, tag.
        /// </summary>
        BAG,

        /// <summary>
        /// Maritime Declaration of Health
        /// Information about Maritime Declaration of Health.
        /// </summary>
        BAH,

        /// <summary>
        /// Additional product information address
        /// Address at which additional information on the product can be found.
        /// </summary>
        BAI,

        /// <summary>
        /// Information to be printed on despatch advice
        /// Specification of free text information which is to be printed on a despatch advice.
        /// </summary>
        BAJ,

        /// <summary>
        /// Missing goods remarks
        /// Remarks concerning missing goods.
        /// </summary>
        BAK,

        /// <summary>
        /// Non-acceptance information
        /// Information related to the non-acceptance of an order, goods or a consignment.
        /// </summary>
        BAL,

        /// <summary>
        /// Returns information
        /// Information related to the return of items.
        /// </summary>
        BAM,

        /// <summary>
        /// Sub-line item information
        /// Note contains information related to sub-line item data.
        /// </summary>
        BAN,

        /// <summary>
        /// Test information
        /// Information related to a test.
        /// </summary>
        BAO,

        /// <summary>
        /// External link
        /// The external link to a digital document (e.g.: URL).
        /// </summary>
        BAP,

        /// <summary>
        /// VAT exemption reason
        /// Reason for Value Added Tax (VAT) exemption.
        /// </summary>
        BAQ,

        /// <summary>
        /// Processing Instructions
        /// Instructions for processing.
        /// </summary>
        BAR,

        /// <summary>
        /// Relay Instructions
        /// Instructions for relaying.
        /// </summary>
        BAS,

        /// <summary>
        /// SIMA applicable
        /// Identifies that Special Import Measures Act applies
        /// </summary>
        BAT,

        /// <summary>
        /// Appeals program code
        /// Identifies information related to an appeals program.
        /// </summary>
        BAU,

        /// <summary>
        /// SIMA subject
        /// Identifies if the goods are subject to a Special Import Measures Act measure.
        /// </summary>
        BAV,

        /// <summary>
        /// Surtax applicable
        /// Identifies that surtax applies
        /// </summary>
        BAW,

        /// <summary>
        /// SIMA security bond
        /// Identifies that there is a security bond in hand that could theoretically be used to cover Special Import Measures Act charges
        /// </summary>
        BAX,

        /// <summary>
        /// Surtax subject
        /// Identifies if the goods are subject to a surtax measure
        /// </summary>
        BAY,

        /// <summary>
        /// Safeguard applicable
        /// Identifies safeguard applies
        /// </summary>
        BAZ,

        /// <summary>
        /// Safeguard applicable
        /// Identifies safeguard applies
        /// </summary>
        BBA,

        /// <summary>
        /// Safeguard subject
        /// Identifies if the goods are subject to a safeguard measure
        /// </summary>
        BBB,

        /// <summary>
        /// Transport contract document clause
        /// [4180] Clause on a transport document regarding the cargo being consigned. Synonym: Bill of Lading clause.
        /// </summary>
        BLC,

        /// <summary>
        /// Instruction to prepare the patient
        /// Instruction with the purpose of preparing the patient.
        /// </summary>
        BLD,

        /// <summary>
        /// Medicine treatment comment
        /// Comment about treatment with medicine.
        /// </summary>
        BLE,

        /// <summary>
        /// Examination result comment
        /// Comment about the result of an examination.
        /// </summary>
        BLF,

        /// <summary>
        /// Service request comment
        /// Comment about the requested service.
        /// </summary>
        BLG,

        /// <summary>
        /// Prescription reason
        /// Details of the reason for a prescription.
        /// </summary>
        BLH,

        /// <summary>
        /// Prescription comment
        /// Comment concerning a specified prescription.
        /// </summary>
        BLI,

        /// <summary>
        /// Clinical investigation comment
        /// Comment concerning a clinical investigation.
        /// </summary>
        BLJ,

        /// <summary>
        /// Medicinal specification comment
        /// Comment concerning the specification of a medicinal product.
        /// </summary>
        BLK,

        /// <summary>
        /// Economic contribution comment
        /// Comment concerning economic contribution.
        /// </summary>
        BLL,

        /// <summary>
        /// Status of a plan
        /// Comment about the status of a plan.
        /// </summary>
        BLM,

        /// <summary>
        /// Random sample test information
        /// Information regarding a random sample test.
        /// </summary>
        BLN,

        /// <summary>
        /// Period of time
        /// Text subject is a period of time.
        /// </summary>
        BLO,

        /// <summary>
        /// Legislation
        /// Information about legislation.
        /// </summary>
        BLP,

        /// <summary>
        /// Security measures requested
        /// Text describing security measures that are requested to be executed (e.g. access controls, supervision of ship's stores).
        /// </summary>
        BLQ,

        /// <summary>
        /// Transport contract document remark
        /// [4244] Remarks concerning the complete consignment to be printed on the transport document. Synonym: Bill of Lading remark.
        /// </summary>
        BLR,

        /// <summary>
        /// Previous port of call security information
        /// Text describing the security information as applicable at the port facility in the previous port where a ship/port interface was conducted.
        /// </summary>
        BLS,

        /// <summary>
        /// Security information
        /// Text describing security related information (e.g security measures currently in force on a vessel).
        /// </summary>
        BLT,

        /// <summary>
        /// Waste information
        /// Text describing waste related information.
        /// </summary>
        BLU,

        /// <summary>
        /// B2C marketing information, short description
        /// Consumer marketing information, short description.
        /// </summary>
        BLV,

        /// <summary>
        /// B2B marketing information, long description
        /// Trading partner marketing information, long description.
        /// </summary>
        BLW,

        /// <summary>
        /// B2C marketing information, long description
        /// Consumer marketing information, long description.
        /// </summary>
        BLX,

        /// <summary>
        /// Product ingredients
        /// Information on the ingredient make up of the product.
        /// </summary>
        BLY,

        /// <summary>
        /// Location short name
        /// Short name of a location e.g. for display or printing purposes.
        /// </summary>
        BLZ,

        /// <summary>
        /// Packaging material information
        /// The text contains a description of the material used for packaging.
        /// </summary>
        BMA,

        /// <summary>
        /// Filler material information
        /// Text contains information on the material used for stuffing.
        /// </summary>
        BMB,

        /// <summary>
        /// Ship-to-ship activity information
        /// Text contains information on ship-to-ship activities.
        /// </summary>
        BMC,

        /// <summary>
        /// Package material description
        /// A description of the type of material for packaging beyond the level covered by standards such as UN Recommendation 21.
        /// </summary>
        BMD,

        /// <summary>
        /// Consumer level package marking
        /// Textual representation of the markings on a consumer level package.
        /// </summary>
        BME,

        /// <summary>
        /// SIMA measure in force
        /// Identifies the specific Special Import Measures Act measure related to the goods
        /// </summary>
        BMF,

        /// <summary>
        /// Pre-CARM
        /// Identifiication of how the transmission should be processed regarding submissions transmitted prior to implementation of Canada Border Services Agency’s Assessment and Revenue Management (CARM) project
        /// </summary>
        BMG,

        /// <summary>
        /// SIMA measure type
        /// Identification of the type of Special Import Measures Act measure
        /// </summary>
        BMH,

        /// <summary>
        /// Customs clearance instructions
        /// Any coded or clear instruction agreed by customer and carrier regarding the declaration of the goods.
        /// </summary>
        CCI,

        /// <summary>
        /// Sub Type Code
        /// Code which identifies a secondary form type
        /// </summary>
        CCJ,

        /// <summary>
        /// SIMA information
        /// Additional information detailing Special Import Measures Act information
        /// </summary>
        CCK,

        /// <summary>
        /// Time limit end
        /// The date the goods exited the economy or warehouse
        /// </summary>
        CCL,

        /// <summary>
        /// Time limit start
        /// The date the goods entered the economy or warehouse
        /// </summary>
        CCM,

        /// <summary>
        /// Warehouse time limit
        /// The amount of time goods may remain in the warehouse
        /// </summary>
        CCN,

        /// <summary>
        /// Value for duty information
        /// Additional information detailing the basis on which the value for duty was determined
        /// </summary>
        CCO,

        /// <summary>
        /// Customs clearance instructions export
        /// Any coded or clear instruction agreed by customer and carrier regarding the export declaration of the goods.
        /// </summary>
        CEX,

        /// <summary>
        /// Change information
        /// Note contains change information.
        /// </summary>
        CHG,

        /// <summary>
        /// Customs clearance instruction import
        /// Any coded or clear instruction agreed by customer and carrier regarding the import declaration of the goods.
        /// </summary>
        CIP,

        /// <summary>
        /// Clearance place requested
        /// Name of the place where Customs clearance is asked to be executed as requested by the consignee/consignor.
        /// </summary>
        CLP,

        /// <summary>
        /// Loading remarks
        /// Instructions concerning the loading of the container.
        /// </summary>
        CLR,

        /// <summary>
        /// Order information
        /// Additional information related to an order.
        /// </summary>
        COI,

        /// <summary>
        /// Customer remarks
        /// Remarks from or for a supplier of goods or services.
        /// </summary>
        CUR,

        /// <summary>
        /// Customs declaration information
        /// (4034) Note contains customs declaration information.
        /// </summary>
        CUS,

        /// <summary>
        /// Damage remarks
        /// Remarks concerning damage on the cargo.
        /// </summary>
        DAR,

        /// <summary>
        /// Document issuer declaration
        /// [4020] Text of a declaration made by the issuer of a document.
        /// </summary>
        DCL,

        /// <summary>
        /// Delivery information
        /// Information about delivery.
        /// </summary>
        DEL,

        /// <summary>
        /// Delivery instructions
        /// [4492] Instructions regarding the delivery of the cargo.
        /// </summary>
        DIN,

        /// <summary>
        /// Documentation instructions
        /// Instructions pertaining to the documentation.
        /// </summary>
        DOC,

        /// <summary>
        /// Duty declaration
        /// The text contains a statement constituting a duty declaration.
        /// </summary>
        DUT,

        /// <summary>
        /// Effective used routing
        /// Physical route effectively used for the movement of the means of transport.
        /// </summary>
        EUR,

        /// <summary>
        /// First block to be printed on the transport contract
        /// The first block of text to be printed on the transport contract.
        /// </summary>
        FBC,

        /// <summary>
        /// Government bill of lading information
        /// Free text information on a transport document to indicate payment information by Government Bill of Lading.
        /// </summary>
        GBL,

        /// <summary>
        /// Entire transaction set
        /// Note is general in nature, applies to entire transaction segment.
        /// </summary>
        GEN,

        /// <summary>
        /// Further information concerning GGVS par. 7
        /// Special permission for road transport of certain goods in the German dangerous goods regulation for road transport.
        /// </summary>
        GS7,

        /// <summary>
        /// Consignment handling instruction
        /// [4078] Free form description of a set of handling instructions. For example how specified goods, packages or transport equipment (container) should be handled.
        /// </summary>
        HAN,

        /// <summary>
        /// Hazard information
        /// Information pertaining to a hazard.
        /// </summary>
        HAZ,

        /// <summary>
        /// Consignment information for consignee
        /// [4070] Any remarks given for the information of the consignee.
        /// </summary>
        ICN,

        /// <summary>
        /// Insurance instructions
        /// (4112) Instructions regarding the cargo insurance.
        /// </summary>
        IIN,

        /// <summary>
        /// Invoice mailing instructions
        /// Instructions as to which freight and charges components have to be mailed to whom.
        /// </summary>
        IMI,

        /// <summary>
        /// Commercial invoice item description
        /// Free text describing goods on a commercial invoice line.
        /// </summary>
        IND,

        /// <summary>
        /// Insurance information
        /// Specific note contains insurance information.
        /// </summary>
        INS,

        /// <summary>
        /// Invoice instruction
        /// Note contains invoice instructions.
        /// </summary>
        INV,

        /// <summary>
        /// Information for railway purpose
        /// Data entered by railway stations when required, e.g. specified trains, additional sheets for freight calculations, special measures, etc.
        /// </summary>
        IRP,

        /// <summary>
        /// Inland transport details
        /// Information concerning the pre-carriage to the port of discharge if by other means than a vessel.
        /// </summary>
        ITR,

        /// <summary>
        /// Testing instructions
        /// Instructions regarding the testing that is required to be carried out on the items in the transaction.
        /// </summary>
        ITS,

        /// <summary>
        /// Location Alias
        /// Alternative name for a location.
        /// </summary>
        LAN,

        /// <summary>
        /// Line item
        /// Note contains line item information.
        /// </summary>
        LIN,

        /// <summary>
        /// Loading instruction
        /// [4080] Instructions where specified packages or containers are to be loaded on a means of transport.
        /// </summary>
        LOI,

        /// <summary>
        /// Miscellaneous charge order
        /// Free text accounting information on an IATA Air Waybill to indicate payment information by Miscellaneous charge order.
        /// </summary>
        MCO,

        /// <summary>
        /// Maritime Declaration of Health
        /// Information about Maritime Declaration of Health.
        /// </summary>
        MDH,

        /// <summary>
        /// Additional marks/numbers information
        /// Additional information regarding the marks and numbers.
        /// </summary>
        MKS,

        /// <summary>
        /// Order instruction
        /// Free text contains order instructions.
        /// </summary>
        ORI,

        /// <summary>
        /// Other service information
        /// General information created by the sender of general or specific value.
        /// </summary>
        OSI,

        /// <summary>
        /// Packing/marking information
        /// Information regarding the packaging and/or marking of goods.
        /// </summary>
        PAC,

        /// <summary>
        /// Payment instructions information
        /// The free text contains payment instructions information relevant to the message.
        /// </summary>
        PAI,

        /// <summary>
        /// Payables information
        /// Note contains payables information.
        /// </summary>
        PAY,

        /// <summary>
        /// Packaging information
        /// Note contains packaging information.
        /// </summary>
        PKG,

        /// <summary>
        /// Packaging terms information
        /// The text contains packaging terms information.
        /// </summary>
        PKT,

        /// <summary>
        /// Payment detail/remittance information
        /// The free text contains payment details.
        /// </summary>
        PMD,

        /// <summary>
        /// Payment information
        /// (4438) Note contains payments information.
        /// </summary>
        PMT,

        /// <summary>
        /// Product information
        /// The text contains product information.
        /// </summary>
        PRD,

        /// <summary>
        /// Price calculation formula
        /// Additional information regarding the price formula used for calculating the item price.
        /// </summary>
        PRF,

        /// <summary>
        /// Priority information
        /// (4218) Note contains priority information.
        /// </summary>
        PRI,

        /// <summary>
        /// Purchasing information
        /// Note contains purchasing information.
        /// </summary>
        PUR,

        /// <summary>
        /// Quarantine instructions
        /// Instructions regarding quarantine, i.e. the period during which an arriving vessel, including its equipment, cargo, crew or passengers, suspected to carry or carrying a contagious disease is detained in strict isolation to prevent the spread of such a disease.
        /// </summary>
        QIN,

        /// <summary>
        /// Quality demands/requirements
        /// Specification of the quality/performance expectations or standards to which the items must conform.
        /// </summary>
        QQD,

        /// <summary>
        /// Quotation instruction/information
        /// Note contains quotation information.
        /// </summary>
        QUT,

        /// <summary>
        /// Risk and handling information
        /// Information concerning risks induced by the goods and/or handling instruction.
        /// </summary>
        RAH,

        /// <summary>
        /// Regulatory information
        /// The free text contains information for regulatory authority.
        /// </summary>
        REG,

        /// <summary>
        /// Return to origin information
        /// Free text information on an IATA Air Waybill to indicate consignment returned because of non delivery.
        /// </summary>
        RET,

        /// <summary>
        /// Receivables
        /// The text contains receivables information.
        /// </summary>
        REV,

        /// <summary>
        /// Consignment route
        /// [3050] Description of a route to be used for the transport of goods.
        /// </summary>
        RQR,

        /// <summary>
        /// Safety information
        /// The text contains safety information.
        /// </summary>
        SAF,

        /// <summary>
        /// Consignment documentary instruction
        /// [4284] Instructions given and declarations made by the sender to the carrier concerning Customs, insurance, and other formalities.
        /// </summary>
        SIC,

        /// <summary>
        /// Special instructions
        /// Special instructions like licence no, high value, handle with care, glass.
        /// </summary>
        SIN,

        /// <summary>
        /// Ship line requested
        /// Shipping line requested to be used for traffic between European continent and U.K. for Ireland.
        /// </summary>
        SLR,

        /// <summary>
        /// Special permission for transport, generally
        /// Statement that a special permission has been obtained for the transport (and/or routing) in general, and reference to such permission.
        /// </summary>
        SPA,

        /// <summary>
        /// Special permission concerning the goods to be transported
        /// Statement that a special permission has been obtained for the transport (and/or routing) of the goods specified, and reference to such permission.
        /// </summary>
        SPG,

        /// <summary>
        /// Special handling
        /// Note contains special handling information.
        /// </summary>
        SPH,

        /// <summary>
        /// Special permission concerning package
        /// Statement that a special permission has been obtained for the packaging, and reference to such permission.
        /// </summary>
        SPP,

        /// <summary>
        /// Special permission concerning transport means
        /// Statement that a special permission has been obtained for the use of the means transport, and reference to such permission.
        /// </summary>
        SPT,

        /// <summary>
        /// Subsidiary risk number (IATA/DGR)
        /// Number(s) of subsidiary risks, induced by the goods, according to the valid classification.
        /// </summary>
        SRN,

        /// <summary>
        /// Special service request
        /// Request for a special service concerning the transport of the goods.
        /// </summary>
        SSR,

        /// <summary>
        /// Supplier remarks
        /// Remarks from or for a supplier of goods or services.
        /// </summary>
        SUR,

        /// <summary>
        /// Consignment tariff
        /// [5430] Free text specification of tariff applied to a consignment.
        /// </summary>
        TCA,

        /// <summary>
        /// Consignment transport
        /// [8012] Transport information for commercial purposes (generic term).
        /// </summary>
        TDT,

        /// <summary>
        /// Transportation information
        /// General information regarding the transport of the cargo.
        /// </summary>
        TRA,

        /// <summary>
        /// Requested tariff
        /// Stipulation of the tariffs to be applied showing, where applicable, special agreement numbers or references.
        /// </summary>
        TRR,

        /// <summary>
        /// Tax declaration
        /// The text contains a statement constituting a tax declaration.
        /// </summary>
        TXD,

        /// <summary>
        /// Warehouse instruction/information
        /// Note contains warehouse information.
        /// </summary>
        WHI,

        /// <summary>
        /// Mutually defined
        /// Note contains information mutually defined by trading partners.
        /// </summary>
        ZZZ
    }
}
