/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Offer {
    interface IOfferEditConfig extends IOfferConfig {
        offer_MortgageSurveyDate: Attributes.IOfferMortgageSurveyDateControlConfig;
        offer_AdditionalSurveyDate: Attributes.IOfferAdditionalSurveyDateControlConfig;
        offer_Broker: Attributes.IOfferBrokerControlConfig;
        offer_Lender: Attributes.IOfferLenderControlConfig;
        offer_Surveyor: Attributes.IOfferSurveyorControlConfig;
        offer_MortgageStatus: Attributes.IOfferMortgageStatusControlConfig;
        offer_MortgageSurveyStatus: Attributes.IOfferMortgageSurveyDateControlConfig;
        offer_SearchStatus: Attributes.IOfferSearchStatusControlConfig;
        offer_Enquiries: Attributes.IOfferEnquiriesControlConfig;
        offer_ContractApproved: Attributes.IOfferContractApprovedControlConfig;
        offer_AdditionalSurveyor: Attributes.IOfferAdditionalSurveyorControlConfig;
        offer_ProgressComment: Attributes.IOfferProgressCommentControlConfig;
    }
}