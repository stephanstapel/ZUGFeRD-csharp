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

using System.ComponentModel;


namespace s2industries.ZUGFeRD
{
    /// <summary>
    /// Reason codes according to UNCL5189 code list
    /// </summary>	    
    public enum ChargeReasonCodes
    {
        /// Advertising
        [EnumStringValue("AA")]
        Advertising,

        /// Telecommunication
        [EnumStringValue("AAA")]
        Telecommunication,

        /// Technical modification
        [EnumStringValue("AAC")]
        TechnicalModification,

        /// Job-order production
        [EnumStringValue("AAD")]
        JobOrderProduction,

        /// Outlays
        [EnumStringValue("AAE")]
        Outlays,

        /// Off-premises
        [EnumStringValue("AAF")]
        OffPremises,

        /// Additional processing
        [EnumStringValue("AAH")]
        AdditionalProcessing,

        /// Attesting
        [EnumStringValue("AAI")]
        Attesting,

        /// Acceptance
        [EnumStringValue("AAS")]
        Acceptance,

        /// Rush delivery
        [EnumStringValue("AAT")]
        RushDelivery,

        /// Special construction
        [EnumStringValue("AAV")]
        SpecialConstruction,

        /// Airport facilities
        [EnumStringValue("AAY")]
        AirportFacilities,

        /// Concession
        [EnumStringValue("AAZ")]
        Concession,

        /// Compulsory storage
        [EnumStringValue("ABA")]
        CompulsoryStorage,

        /// Fuel removal
        [EnumStringValue("ABB")]
        FuelRemoval,

        /// Into plane
        [EnumStringValue("ABC")]
        IntoPlane,

        /// Overtime
        [EnumStringValue("ABD")]
        Overtime,

        /// Tooling
        [EnumStringValue("ABF")]
        Tooling,

        /// Miscellaneous
        [EnumStringValue("ABK")]
        Miscellaneous,

        /// Additional packaging
        [EnumStringValue("ABL")]
        AdditionalPackaging,

        /// Dunnage
        [EnumStringValue("ABN")]
        Dunnage,

        /// Containerisation
        [EnumStringValue("ABR")]
        Containerisation,

        /// Carton packing
        [EnumStringValue("ABS")]
        CartonPacking,

        /// Hessian wrapped
        [EnumStringValue("ABT")]
        HessianWrapped,

        /// Polyethylene wrap packing
        [EnumStringValue("ABU")]
        PolyethyleneWrapPacking,

        /// Miscellaneous treatment
        [EnumStringValue("ACF")]
        MiscellaneousTreatment,

        /// Enamelling treatment
        [EnumStringValue("ACG")]
        EnamellingTreatment,

        /// Heat treatment
        [EnumStringValue("ACH")]
        HeatTreatment,

        /// Plating treatment
        [EnumStringValue("ACI")]
        PlatingTreatment,

        /// Painting
        [EnumStringValue("ACJ")]
        Painting,

        /// Polishing
        [EnumStringValue("ACK")]
        Polishing,

        /// Priming
        [EnumStringValue("ACL")]
        Priming,

        /// Preservation treatment
        [EnumStringValue("ACM")]
        PreservationTreatment,

        /// Fitting
        [EnumStringValue("ACS")]
        Fitting,

        /// Consolidation
        [EnumStringValue("ADC")]
        Consolidation,

        /// Bill of lading
        [EnumStringValue("ADE")]
        BillOfLading,

        /// Airbag
        [EnumStringValue("ADJ")]
        Airbag,

        /// Transfer
        [EnumStringValue("ADK")]
        Transfer,

        /// Slipsheet
        [EnumStringValue("ADL")]
        Slipsheet,

        /// Binding
        [EnumStringValue("ADM")]
        Binding,

        /// Repair or replacement of broken returnable package
        [EnumStringValue("ADN")]
        RepairOrReplacementOfBrokenReturnablePackage,

        /// Efficient logistics
        [EnumStringValue("ADO")]
        EfficientLogistics,

        /// Merchandising
        [EnumStringValue("ADP")]
        Merchandising,

        /// Product mix
        [EnumStringValue("ADQ")]
        ProductMix,

        /// Other services
        [EnumStringValue("ADR")]
        OtherServices,

        /// Pick-up
        [EnumStringValue("ADT")]
        PickUp,

        /// Chronic illness
        [EnumStringValue("ADW")]
        ChronicIllness,

        /// New product introduction
        [EnumStringValue("ADY")]
        NewProductIntroduction,

        /// Direct delivery
        [EnumStringValue("ADZ")]
        DirectDelivery,

        /// Diversion
        [EnumStringValue("AEA")]
        Diversion,

        /// Disconnect
        [EnumStringValue("AEB")]
        Disconnect,

        /// Distribution
        [EnumStringValue("AEC")]
        Distribution,

        /// Handling of hazardous cargo
        [EnumStringValue("AED")]
        HandlingOfHazardousCargo,

        /// Rents and leases
        [EnumStringValue("AEF")]
        RentsAndLeases,

        /// Location differential
        [EnumStringValue("AEH")]
        LocationDifferential,

        /// Aircraft refueling
        [EnumStringValue("AEI")]
        AircraftRefueling,

        /// Fuel shipped into storage
        [EnumStringValue("AEJ")]
        FuelShippedIntoStorage,

        /// Cash on delivery
        [EnumStringValue("AEK")]
        CashOnDelivery,

        /// Small order processing service
        [EnumStringValue("AEL")]
        SmallOrderProcessingService,

        /// Clerical or administrative services
        [EnumStringValue("AEM")]
        ClericalOrAdministrativeServices,

        /// Guarantee
        [EnumStringValue("AEN")]
        Guarantee,

        /// Collection and recycling
        [EnumStringValue("AEO")]
        CollectionAndRecycling,

        /// Copyright fee collection
        [EnumStringValue("AEP")]
        CopyrightFeeCollection,

        /// Veterinary inspection service
        [EnumStringValue("AES")]
        VeterinaryInspectionService,

        /// Pensioner service
        [EnumStringValue("AET")]
        PensionerService,

        /// Medicine free pass holder
        [EnumStringValue("AEU")]
        MedicineFreePassHolder,

        /// Environmental protection service
        [EnumStringValue("AEV")]
        EnvironmentalProtectionService,

        /// Environmental clean-up service
        [EnumStringValue("AEW")]
        EnvironmentalCleanUpService,

        /// National cheque processing service outside account area
        [EnumStringValue("AEX")]
        NationalChequeProcessingServiceOutsideAccountArea,

        /// National payment service outside account area
        [EnumStringValue("AEY")]
        NationalPaymentServiceOutsideAccountArea,

        /// National payment service within account area
        [EnumStringValue("AEZ")]
        NationalPaymentServiceWithinAccountArea,

        /// Adjustments
        [EnumStringValue("AJ")]
        Adjustments,

        /// Authentication
        [EnumStringValue("AU")]
        Authentication,

        /// Cataloguing
        [EnumStringValue("CA")]
        Cataloguing,

        /// Cartage
        [EnumStringValue("CAB")]
        Cartage,

        /// Certification
        [EnumStringValue("CAD")]
        Certification,

        /// Certificate of conformance
        [EnumStringValue("CAE")]
        CertificateOfConformance,

        /// Certificate of origin
        [EnumStringValue("CAF")]
        CertificateOfOrigin,

        /// Cutting
        [EnumStringValue("CAI")]
        Cutting,

        /// Consular service
        [EnumStringValue("CAJ")]
        ConsularService,

        /// Customer collection
        [EnumStringValue("CAK")]
        CustomerCollection,

        /// Payroll payment service
        [EnumStringValue("CAL")]
        PayrollPaymentService,

        /// Cash transportation
        [EnumStringValue("CAM")]
        CashTransportation,

        /// Home banking service
        [EnumStringValue("CAN")]
        HomeBankingService,

        /// Bilateral agreement service
        [EnumStringValue("CAO")]
        BilateralAgreementService,

        /// Insurance brokerage service
        [EnumStringValue("CAP")]
        InsuranceBrokerageService,

        /// Cheque generation
        [EnumStringValue("CAQ")]
        ChequeGeneration,

        /// Preferential merchandising location
        [EnumStringValue("CAR")]
        PreferentialMerchandisingLocation,

        /// Crane
        [EnumStringValue("CAS")]
        Crane,

        /// Special colour service
        [EnumStringValue("CAT")]
        SpecialColourService,

        /// Sorting
        [EnumStringValue("CAU")]
        Sorting,

        /// Battery collection and recycling
        [EnumStringValue("CAV")]
        BatteryCollectionAndRecycling,

        /// Product take back fee
        [EnumStringValue("CAW")]
        ProductTakeBackFee,

        /// Quality control released
        [EnumStringValue("CAX")]
        QualityControlReleased,

        /// Quality control held
        [EnumStringValue("CAY")]
        QualityControlHeld,

        /// Quality control embargo
        [EnumStringValue("CAZ")]
        QualityControlEmbargo,

        /// Car loading
        [EnumStringValue("CD")]
        CarLoading,

        /// Cleaning
        [EnumStringValue("CG")]
        Cleaning,

        /// Cigarette stamping
        [EnumStringValue("CS")]
        CigaretteStamping,

        /// Count and recount
        [EnumStringValue("CT")]
        CountAndRecount,

        /// Layout/design
        [EnumStringValue("DAB")]
        LayoutDesign,

        /// Assortment allowance
        [EnumStringValue("DAC")]
        AssortmentAllowance,

        /// Driver assigned unloading
        [EnumStringValue("DAD")]
        DriverAssignedUnloading,

        /// Debtor bound
        [EnumStringValue("DAF")]
        DebtorBound,

        /// Dealer allowance
        [EnumStringValue("DAG")]
        DealerAllowance,

        /// Allowance transferable to the consumer
        [EnumStringValue("DAH")]
        AllowanceTransferableToTheConsumer,

        /// Growth of business
        [EnumStringValue("DAI")]
        GrowthOfBusiness,

        /// Introduction allowance
        [EnumStringValue("DAJ")]
        IntroductionAllowance,

        /// Multi-buy promotion
        [EnumStringValue("DAK")]
        MultiBuyPromotion,

        /// Partnership
        [EnumStringValue("DAL")]
        Partnership,

        /// Return handling
        [EnumStringValue("DAM")]
        ReturnHandling,

        /// Minimum order not fulfilled charge
        [EnumStringValue("DAN")]
        MinimumOrderNotFulfilledCharge,

        /// Point of sales threshold allowance
        [EnumStringValue("DAO")]
        PointOfSalesThresholdAllowance,

        /// Wholesaling discount
        [EnumStringValue("DAP")]
        WholesalingDiscount,

        /// Documentary credits transfer commission
        [EnumStringValue("DAQ")]
        DocumentaryCreditsTransferCommission,

        /// Delivery
        [EnumStringValue("DL")]
        Delivery,

        /// Engraving
        [EnumStringValue("EG")]
        Engraving,

        /// Expediting
        [EnumStringValue("EP")]
        Expediting,

        /// Exchange rate guarantee
        [EnumStringValue("ER")]
        ExchangeRateGuarantee,

        /// Fabrication
        [EnumStringValue("FAA")]
        Fabrication,

        /// Freight equalization
        [EnumStringValue("FAB")]
        FreightEqualization,

        /// Freight extraordinary handling
        [EnumStringValue("FAC")]
        FreightExtraordinaryHandling,

        /// Freight service
        [EnumStringValue("FC")]
        FreightService,

        /// Filling/handling
        [EnumStringValue("FH")]
        FillingHandling,

        /// Financing
        [EnumStringValue("FI")]
        Financing,

        /// Grinding
        [EnumStringValue("GAA")]
        Grinding,

        /// Hose
        [EnumStringValue("HAA")]
        Hose,

        /// Handling
        [EnumStringValue("HD")]
        Handling,

        /// Hoisting and hauling
        [EnumStringValue("HH")]
        HoistingAndHauling,

        /// Installation
        [EnumStringValue("IAA")]
        Installation,

        /// Installation and warranty
        [EnumStringValue("IAB")]
        InstallationAndWarranty,

        /// Inside delivery
        [EnumStringValue("ID")]
        InsideDelivery,

        /// Inspection
        [EnumStringValue("IF")]
        Inspection,

        /// Installation and training
        [EnumStringValue("IR")]
        InstallationAndTraining,

        /// Invoicing
        [EnumStringValue("IS")]
        Invoicing,

        /// Koshering
        [EnumStringValue("KO")]
        Koshering,

        /// Carrier count
        [EnumStringValue("L1")]
        CarrierCount,

        /// Labelling
        [EnumStringValue("LA")]
        Labelling,

        /// Labour
        [EnumStringValue("LAA")]
        Labour,

        /// Repair and return
        [EnumStringValue("LAB")]
        RepairAndReturn,

        /// Legalisation
        [EnumStringValue("LF")]
        Legalisation,

        /// Mounting
        [EnumStringValue("MAE")]
        Mounting,

        /// Mail invoice
        [EnumStringValue("MI")]
        MailInvoice,

        /// Mail invoice to each location
        [EnumStringValue("ML")]
        MailInvoiceToEachLocation,

        /// Non-returnable containers
        [EnumStringValue("NAA")]
        NonReturnableContainers,

        /// Outside cable connectors
        [EnumStringValue("OA")]
        OutsideCableConnectors,

        /// Invoice with shipment
        [EnumStringValue("PA")]
        InvoiceWithShipment,

        /// Phosphatizing (steel treatment)
        [EnumStringValue("PAA")]
        PhosphatizingSteelTreatment,

        /// Packing
        [EnumStringValue("PC")]
        Packing,

        /// Palletizing
        [EnumStringValue("PL")]
        Palletizing,

        /// Price variation
        [EnumStringValue("PRV")]
        PriceVariation,

        /// Repacking
        [EnumStringValue("RAB")]
        Repacking,

        /// Repair
        [EnumStringValue("RAC")]
        Repair,

        /// Returnable container
        [EnumStringValue("RAD")]
        ReturnableContainer,

        /// Restocking
        [EnumStringValue("RAF")]
        Restocking,

        /// Re-delivery
        [EnumStringValue("RE")]
        ReDelivery,

        /// Refurbishing
        [EnumStringValue("RF")]
        Refurbishing,

        /// Rail wagon hire
        [EnumStringValue("RH")]
        RailWagonHire,

        /// Loading
        [EnumStringValue("RV")]
        Loading,

        /// Salvaging
        [EnumStringValue("SA")]
        Salvaging,

        /// Shipping and handling
        [EnumStringValue("SAA")]
        ShippingAndHandling,

        /// Special packaging
        [EnumStringValue("SAD")]
        SpecialPackaging,

        /// Stamping
        [EnumStringValue("SAE")]
        Stamping,

        /// Consignee unload
        [EnumStringValue("SAI")]
        ConsigneeUnload,

        /// Shrink-wrap
        [EnumStringValue("SG")]
        ShrinkWrap,

        /// Special handling
        [EnumStringValue("SH")]
        SpecialHandling,

        /// Special finish
        [EnumStringValue("SM")]
        SpecialFinish,

        /// Set-up
        [EnumStringValue("SU")]
        SetUp,

        /// Tank renting
        [EnumStringValue("TAB")]
        TankRenting,

        /// Testing
        [EnumStringValue("TAC")]
        Testing,

        /// Transportation - third party billing
        [EnumStringValue("TT")]
        TransportationThirdPartyBilling,

        /// Transportation by vendor
        [EnumStringValue("TV")]
        TransportationByVendor,

        /// Drop yard
        [EnumStringValue("V1")]
        DropYard,

        /// Drop dock
        [EnumStringValue("V2")]
        DropDock,

        /// Warehousing
        [EnumStringValue("WH")]
        Warehousing,

        /// Combine all same day shipment
        [EnumStringValue("XAA")]
        CombineAllSameDayShipment,

        /// Split pick-up
        [EnumStringValue("YY")]
        SplitPickUp,

        /// Mutually defined
        [EnumStringValue("ZZZ")]
        MutuallyDefined
    }
}
