///<reference path="../../../typings/_all.d.ts"/>

module Antares.Attributes.Offer.OfferChain {
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import Business = Common.Models.Business;

    export class OfferChainPanelController extends Common.Component.BaseSidePanelController {
        // bindings
        isPreviewMode: boolean;
        chain: Business.ChainTransaction;
        isLastChain: boolean;
        
        // properties
        config: any;

        constructor(
            private eventAggregator: Core.EventAggregator) {
            super();
        }

        panelShown = () => {
        }

        public edit = (chain: Business.ChainTransaction) => {
        }

        cancel = () => {
            this.isBusy = false;
            this.eventAggregator.publish(new Common.Component.CloseSidePanelEvent());
        }

        public $onChanges(changesObj: any) {
            this.config = this.defineControlConfig(changesObj.chain.currentValue);
            super.$onChanges(changesObj);
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
                surveyDate: { surveyDate: { required: true, active: true } }
            }
        }
    }

    angular.module('app').controller('offerChainPanelController', OfferChainPanelController);
}