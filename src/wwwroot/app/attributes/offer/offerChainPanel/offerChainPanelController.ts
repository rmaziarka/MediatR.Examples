///<reference path="../../../typings/_all.d.ts"/>

module Antares.Attributes.Offer.OfferChain {
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import Business = Common.Models.Business;

    export class OfferChainPanelController extends Common.Component.BaseSidePanelController {
        // bindings
        inPreviewMode: boolean;
        chain: Business.ChainTransaction;
        isLastChain: boolean;

        // properties
        config: any;
        cardPristine: any;
        isBusy: boolean = false;
        panelMode: OfferChainPanelMode;
        offerChainPanelMode: any = OfferChainPanelMode;

        constructor(
            private eventAggregator: Core.EventAggregator,
            private activityService: Services.ActivityService) {
            super();
        }

        public panelShown = () => {
            if (this.inPreviewMode) {
                this.panelMode = OfferChainPanelMode.Preview;
            } else {
                this.panelMode = OfferChainPanelMode.AddEdit;
            }
        }

        public save = (chain: Business.ChainTransaction) => {
            //TODO service updateChain
        }

        public edit = (chain: Business.ChainTransaction) => {
        }

        public cancel = () => {
            this.isBusy = false;
            this.eventAggregator.publish(new Common.Component.CloseSidePanelEvent());
        }

        protected onChanges = (changesObj: any) => {
            this.loadConfig(changesObj.chain.currentValue);
            this.resetState();
        }

        private resetState = () => {
            this.cardPristine = new Object();
        }

        private loadConfig(chain: Business.ChainTransaction) {
            this.config = this.defineControlConfig(chain);
        }

        private defineControlConfig = (chain: Business.ChainTransaction) => {
            return {
                isEnd: { isEnd: { required: false, active: true } },
                property: { propertyId: { required: true, active: true } },
                vendor: { vendor: { required: true, active: true } },
                agentUser: chain != null && chain.agentUser != null ? { agentUserId: { required: true, active: true } } : null,
                agentCompanyContact: chain == null || chain.agentUser != null ? null : { agentContactId: { required: true, active: true }, agentCompanyId: { required: true, active: true } },
                solicitorCompanyContact: { solicitorContactId: { required: true, active: true }, solicitorCompanyId: { required: true, active: true } },
                mortgage: { mortgageId: { required: true, active: true } },
                survey: { surveyId: { required: true, active: true } },
                searches: { searchesId: { required: true, active: true } },
                enquiries: { enquiriesId: { required: true, active: true } },
                contractAgreed: { contractAgreedId: { required: true, active: true } },
                surveyDate: { surveyDate: { required: false, active: true } }
            }
        }
    }

    angular.module('app').controller('offerChainPanelController', OfferChainPanelController);
}