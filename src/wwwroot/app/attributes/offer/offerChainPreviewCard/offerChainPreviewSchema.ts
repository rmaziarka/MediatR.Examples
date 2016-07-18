///<reference path="../../../typings/_all.d.ts"/>

module Antares.Attributes.Offer.OfferChain{
    export class OfferChainPreviewSchema {

        //TODO - specify control scheme type
        static isEnd = <any>{
            controlId : "offer-chain-preview-is-end",
            translationKey : "OFFER.CHAIN.PREVIEW.IS_END"
        };
        //TODO - specify control scheme type
        static property = <any>{
            controlId : "offer-chain-preview-property",
            translationKey : "OFFER.CHAIN.PREVIEW.PROPERTY"
        };
        //TODO - specify control scheme type
        static vendor = <any>{
            controlId : "offer-chain-preview-vendor",
            translationKey : "OFFER.CHAIN.PREVIEW.VENDOR"
        };
        //TODO - specify control scheme type
        static agentUser = <any>{
            controlId : "offer-chain-preview-agent-user",
            translationKey : "OFFER.CHAIN.PREVIEW.AGENT"
        };
        static agentCompanyContact: Attributes.ICompanyContactViewControlSchema= <Attributes.ICompanyContactViewControlSchema>{
            controlId : "offer-chain-preview-agent-contact",
            translationKey : "OFFER.CHAIN.PREVIEW.AGENT",
            emptyTranslationKey : "OFFER.CHAIN.PREVIEW.NO_AGENT"
        };
        static solicitorCompanyContact: Attributes.ICompanyContactViewControlSchema= <Attributes.ICompanyContactViewControlSchema>{
            controlId : "offer-chain-preview-solicitor",
            translationKey : "OFFER.CHAIN.PREVIEW.SOLICITOR",
            emptyTranslationKey : "OFFER.CHAIN.PREVIEW.NO_SOLICITOR"
        };
        static mortgage: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId : "offer-chain-preview-mortgage",
            translationKey : "OFFER.CHAIN.PREVIEW.MORTGAGE"
        };
        static survey: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema> {
            controlId : "offer-chain-preview-survey",
            translationKey : "OFFER.CHAIN.PREVIEW.SURVEY"
        };
        static searches: Attributes.IEnumItemControlSchema= <Attributes.IEnumItemControlSchema>{
            controlId : "offer-chain-preview-searches",
            translationKey : "OFFER.CHAIN.PREVIEW.SEARCHES"
        };
        static enquiries: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId : "offer-chain-preview-enquiries",
            translationKey : "OFFER.CHAIN.PREVIEW.ENQUIRIES"
        };
        static contractAgreed: Attributes.IEnumItemControlSchema = <Attributes.IEnumItemControlSchema>{
            controlId : "offer-chain-preview-contract-agreed",
            translationKey : "OFFER.CHAIN.PREVIEW.CONTRACT_AGREED"
        };
        static surveyDate: Attributes.IDateEditControlSchema = <Attributes.IDateEditControlSchema>{
            controlId : "offer-chain-preview-survey-date",
            translationKey : "OFFER.CHAIN.PREVIEW.SURVEY_DATE"
        };
    };
}