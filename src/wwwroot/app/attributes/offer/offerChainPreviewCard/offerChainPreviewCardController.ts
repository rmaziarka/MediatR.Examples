/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer.OfferChain {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class OfferChainPreviewCardController {
        // bindings
        chain: Business.ChainTransaction;
        onEdit: () => void;

        // controls
        controlConfig: Dto.IControlConfig = {
            // not specified, all controls should be displayed
        };

        controlSchemas: any = {
            //TODO - specify control scheme type
            isEnd: <any>{
                controlId: "offer-chain-preview-is-end",
                translationKey: "OFFER.CHAIN.PREVIEW.IS_END",
            },
            //TODO - specify control scheme type
            property: <any>{
                controlId: "offer-chain-preview-property",
                translationKey: "OFFER.CHAIN.PREVIEW.PROPERTY",
            },
            //TODO - specify control scheme type
            vendor: <any>{
                controlId: "offer-chain-preview-vendor",
                translationKey: "OFFER.CHAIN.PREVIEW.VENDOR",
            },
            //TODO - specify control scheme type
            agentUser: <any>{
                controlId: "offer-chain-preview-agent-user",
                translationKey: "OFFER.CHAIN.PREVIEW.AGENT",
            },
            agentCompanyContact: <Attributes.ICompanyContactViewControlSchema>{
                controlId: "offer-chain-preview-agent-contact",
                translationKey: "OFFER.CHAIN.PREVIEW.AGENT",
                emptyTranslationKey: "OFFER.CHAIN.PREVIEW.NO_AGENT"
            },
            solicitorCompanyContact: <Attributes.ICompanyContactViewControlSchema>{
                controlId: "offer-chain-preview-solicitor",
                translationKey: "OFFER.CHAIN.PREVIEW.SOLICITOR",
                emptyTranslationKey: "OFFER.CHAIN.PREVIEW.NO_SOLICITOR"
            },
            mortgage: <Attributes.IEnumItemControlSchema>{
                controlId: "offer-chain-preview-mortgage",
                translationKey: "OFFER.CHAIN.PREVIEW.MORTGAGE",
            },
            survey: <Attributes.IEnumItemControlSchema>{
                controlId: "offer-chain-preview-survey",
                translationKey: "OFFER.CHAIN.PREVIEW.SURVEY",
            },
            searches: <Attributes.IEnumItemControlSchema>{
                controlId: "offer-chain-preview-searches",
                translationKey: "OFFER.CHAIN.PREVIEW.SEARCHES",
            },
            enquiries: <Attributes.IEnumItemControlSchema>{
                controlId: "offer-chain-preview-enquiries",
                translationKey: "OFFER.CHAIN.PREVIEW.ENQUIRIES",
            },
            contractAgreed: <Attributes.IEnumItemControlSchema>{
                controlId: "offer-chain-preview-contract-agreed",
                translationKey: "OFFER.CHAIN.PREVIEW.CONTRACT_AGREED",
            },
            surveyDate: <Attributes.IDateEditControlSchema>{
                controlId: "offer-chain-preview-survey-date",
                translationKey: "OFFER.CHAIN.PREVIEW.SURVEY_DATE",
            }
        }

        constructor(
            private $state: ng.ui.IStateService) {
        }

        edit = () => {
            this.onEdit();
        }

        navigateToProperty = (property: Dto.IProperty) => {
            this.$state.go('app.property-view', { id: property.id });
        }
    }

    angular.module('app').controller('offerChainPreviewCardController', OfferChainPreviewCardController);
}