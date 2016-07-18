///<reference path="../../../typings/_all.d.ts"/>

module Antares.Attributes.Offer.OfferChain {
    import Dto = Common.Models.Dto;
    import ISearchUserControlSchema = Attributes.ISearchUserControlSchema;

    export class OfferChainAddEditSchema {

        static searchUser: ISearchUserControlSchema = <ISearchUserControlSchema>{
            formName : "searchUserControlForm",
            emptyTranslationKey : "OFFER.CHAIN.EDIT.NO_AGENT",
            controlId : "offer-chain-search-user",
            searchPlaceholderTranslationKey : "OFFER.CHAIN.EDIT.FIND_NEGOTIATOR",
            fieldName : "agentUserId",
            itemTemplateUrl : "app/attributes/activityNegotiators/userSearchTemplate.html",
            usersSearchMaxCount : 100
        };
        static isEnd = <any>{
            formName : "isEndControlForm",
            controlId : "offer-chain-edit-is-end",
            translationKey : "OFFER.CHAIN.EDIT.IS_END",
            fieldName : "isEnd"
        };
        static vendor = <any>{
            formName : "vendorControlForm",
            controlId : "offer-chain-edit-vendor",
            translationKey : "OFFER.CHAIN.EDIT.VENDOR",
            fieldName : "vendor",
            placeholder: "OFFER.CHAIN.EDIT.NAME_AND_SURNAME",
            maxLength: 128
        };
        static agentUser= <any>{
            formName : "agentUserForm",
            controlId : "offer-chain-edit-agent-user",
            translationKey : "OFFER.CHAIN.EDIT.AGENT",
            fieldName : "agentUserId"
        };
        static agentCompanyContact= <any>{
            formName : "agentCompanyContactForm",
            controlId : "offer-chain-edit-agent-company-contact",
            translationKey : "",
            emptyTranslationKey : "OFFER.CHAIN.EDIT.NO_AGENT",
            fieldName : "agentContactId"
        };
        static solicitorCompanyContact: Attributes.ICompanyContactViewControlSchema = <Attributes.ICompanyContactViewControlSchema>{
            formName : "solicitorCompanyContactControlForm",
            controlId : "offer-chain-edit-solicitor",
            translationKey : "OFFER.CHAIN.EDIT.SOLICITOR",
            emptyTranslationKey : "OFFER.CHAIN.EDIT.NO_SOLICITOR",
            fieldName : "solicitorCompanyContact"
        };
        static mortgage: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            formName : "mortgageControlForm",
            controlId : "offer-chain-edit-mortgage",
            translationKey : "OFFER.CHAIN.EDIT.MORTGAGE",
            fieldName : "mortgageId",
            enumTypeCode : Dto.EnumTypeCode.ChainMortgageStatus
        };
        static survey: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            formName : "surveyControlForm",
            controlId : "offer-chain-edit-survey",
            translationKey : "OFFER.CHAIN.EDIT.SURVEY",
            fieldName : "surveyId",
            enumTypeCode : Dto.EnumTypeCode.ChainMortgageSurveyStatus
        };
        static searches: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            formName : "searchesControlForm",
            controlId : "offer-chain-edit-searches",
            translationKey : "OFFER.CHAIN.EDIT.SEARCHES",
            fieldName : "searchesId",
            enumTypeCode : Dto.EnumTypeCode.ChainSearchStatus
        };
        static enquiries: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            formName : "enquiriesControlForm",
            controlId : "offer-chain-edit-enquiries",
            translationKey : "OFFER.CHAIN.EDIT.ENQUIRIES",
            fieldName : "enquiriesId",
            enumTypeCode : Dto.EnumTypeCode.ChainEnquiries
        };
        static contractAgreed: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            formName : "contractAgreedeControlForm",
            controlId : "offer-chain-edit-contract-agreed",
            translationKey : "OFFER.CHAIN.EDIT.CONTRACT_AGREED",
            fieldName : "contractAgreedId",
            enumTypeCode : Dto.EnumTypeCode.ChainContractAgreedStatus
        };
        static surveyDate: Attributes.IDateEditControlSchema = <Attributes.IDateEditControlSchema>{
            formName : "surveyDateControlForm",
            controlId : "offer-chain-edit-survey-date",
            translationKey : "OFFER.CHAIN.EDIT.SURVEY_DATE",
            fieldName : "surveyDate"
        };
        static agentType: Attributes.IRadioButtonsEditControlSchema = <Attributes.IRadioButtonsEditControlSchema>{
            formName : "chainEditAgentTypeForm",
            fieldName : "chainEditAgentType",
            translationKey : "OFFER.CHAIN.EDIT.AGENT",
            radioButtons : [
                { value : true, translationKey : "OFFER.CHAIN.EDIT.KNIGHT_FRANK" },
                { value : false, translationKey : "OFFER.CHAIN.EDIT.THIRD_PARTY" }
            ]
        };
    };
}