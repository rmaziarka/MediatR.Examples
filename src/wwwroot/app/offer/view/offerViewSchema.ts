///<reference path="../../typings/_all.d.ts"/>

module Antares.Component {
    import Dto = Common.Models.Dto;

    export class OfferViewSchema {

        static activitySolicitor: Attributes.ICompanyContactViewControlSchema = <Attributes.ICompanyContactViewControlSchema>{
            controlId: 'vendorSolicitor',
            translationKey: 'OFFER.VIEW.SOLICITOR',
            emptyTranslationKey: 'OFFER.VIEW.NO_SOLICITOR'
        };
        static requirementSolicitor: Attributes.ICompanyContactViewControlSchema = <Attributes.ICompanyContactViewControlSchema>{
            controlId: 'applicantSolicitor',
            translationKey: 'OFFER.VIEW.SOLICITOR',
            emptyTranslationKey: 'OFFER.VIEW.NO_SOLICITOR'
        };
        static broker: Attributes.ICompanyContactViewControlSchema = <Attributes.ICompanyContactViewControlSchema>{
            controlId: 'broker',
            translationKey: 'OFFER.EDIT.BROKER',
            emptyTranslationKey: 'OFFER.EDIT.NO_BROKER'
        };
        static lender: Attributes.ICompanyContactViewControlSchema = <Attributes.ICompanyContactViewControlSchema>{
            controlId: 'lender',
            translationKey: 'OFFER.EDIT.LENDER',
            emptyTranslationKey: 'OFFER.EDIT.NO_LENDER'
        };
        static surveyor: Attributes.ICompanyContactViewControlSchema = <Attributes.ICompanyContactViewControlSchema>{
            controlId: 'surveyor',
            translationKey: 'OFFER.EDIT.SURVEYOR',
            emptyTranslationKey: 'OFFER.EDIT.NO_SURVEYOR'
        };
        static additionalSurveyor: Attributes.ICompanyContactViewControlSchema = <Attributes.ICompanyContactViewControlSchema>{
            controlId: 'additionalSurveyor',
            translationKey: 'OFFER.EDIT.SURVEYOR',
            emptyTranslationKey: 'OFFER.EDIT.NO_SURVEYOR'
        };
        static mortgageLoanToValue: Attributes.IPercentNumberControlSchema = <Attributes.IPercentNumberControlSchema>{
            controlId: "mortgage-loan-to-value",
            translationKey: "OFFER.VIEW.MORTGAGE_LOAN_TO_VALUE",
            fieldName: "mortgageLoanToValue"
        };
        static contractApproved: Attributes.IRadioButtonsViewControlSchema = <Attributes.IRadioButtonsViewControlSchema>{
            fieldName: "offerContractApproved",
            translationKey: "OFFER.VIEW.CONTRACT_APPROVED",
            templateUrl: "app/attributes/radioButtons/templates/radioButtonsViewBoolean.html"
        };
        static status: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId: "offer-status",
            translationKey: "OFFER.VIEW.STATUS",
            enumTypeCode: Dto.EnumTypeCode.OfferStatus
        };
        static price: Attributes.IPriceControlSchema = <Attributes.IPriceControlSchema>{
            controlId: "offer-price",
            translationKey: "OFFER.VIEW.OFFER",
            fieldName: "price"
        };
        static pricePerWeek: Attributes.IPriceControlSchema = <Attributes.IPriceControlSchema>{
            controlId: "offer-price-per-week",
            translationKey: "OFFER.VIEW.OFFER",
            fieldName: "pricePerWeek",
            suffix: "OFFER.EDIT.OFFER_PER_WEEK"
        };
        static offerDate: Attributes.IDateControlSchema = <Attributes.IDateControlSchema>{
            controlId: "offer-date",
            translationKey: "OFFER.VIEW.OFFER_DATE",
            fieldName: "offerDate"
        };
        static exchangeDate: Attributes.IDateControlSchema = <Attributes.IDateControlSchema>{
            controlId: "offer-exchange-date",
            translationKey: "OFFER.VIEW.EXCHANGE_DATE",
            fieldName: "exchangeDate"
        };
        static completionDate: Attributes.IDateControlSchema = <Attributes.IDateControlSchema>{
            controlId: "offer-completion-date",
            translationKey: "OFFER.VIEW.COMPLETION_DATE",
            fieldName: "completionDate"
        };
        static specialConditions: Attributes.ITextControlSchema = <Attributes.ITextControlSchema>{
            controlId: "offer-special-conditions",
            translationKey: "OFFER.VIEW.SPECIAL_CONDITIONS",
            fieldName: "specialConditions"
        };
        static mortgageStatus: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId: "offer-mortgage-status",
            translationKey: "OFFER.VIEW.MORTGAGE_STATUS",
            enumTypeCode: Dto.EnumTypeCode.MortgageStatus
        };
        static mortgageSurveyStatus: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId: "offer-mortgage-survey-status",
            translationKey: "OFFER.VIEW.MORTGAGE_SURVEY_STATUS",
            enumTypeCode: Dto.EnumTypeCode.MortgageSurveyStatus
        };
        static additionalSurveyStatus: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId: "offer-additional-survey-status",
            translationKey: "OFFER.VIEW.ADDITIONAL_SURVEY_STATUS",
            enumTypeCode: Dto.EnumTypeCode.AdditionalSurveyStatus
        };
        static searchStatus: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId: "offer-search-status",
            translationKey: "OFFER.VIEW.SEARCH_STATUS",
            enumTypeCode: Dto.EnumTypeCode.SearchStatus
        };
        static enquiries: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId: "offer-enquiries",
            translationKey: "OFFER.VIEW.ENQUIRIES",
            enumTypeCode: Dto.EnumTypeCode.Enquiries
        };
        static mortgageSurveyDate: Attributes.IDateControlSchema = <Attributes.IDateControlSchema>{
            controlId: "mortgage-survey-date",
            translationKey: "OFFER.VIEW.MORTGAGE_SURVEY_DATE",
            fieldName: "mortgageSurveyDate"
        };
        static additionalSurveyDate: Attributes.IDateControlSchema = <Attributes.IDateControlSchema>{
            controlId: "additional-survey-date",
            translationKey: "OFFER.VIEW.ADDITIONAL_SURVEY_DATE",
            fieldName: "additionalSurveyDate"
        };
        static progressComment: Attributes.ITextControlSchema = <Attributes.ITextControlSchema>{
            controlId: "offer-progress-comment",
            translationKey: "OFFER.VIEW.COMMENT",
            fieldName: "progressComment"
        }
    };
}