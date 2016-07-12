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
                isEnd: <Dto.IControlConfig>{ required: true, active: true },
                property: <Dto.IControlConfig>{ required: true, active: true },
                vendor: <Dto.IControlConfig>{ required: true, active: true },
                agentUser: chain != null && chain.agentUser != null ? <Dto.IControlConfig>{ required: true, active: true } : null,
                agentCompanyContact: chain == null || chain.agentUser != null ? null : <Dto.IControlConfig>{ required: true, active: true },
                solicitorCompanyContact: <Dto.IControlConfig>{ required: true, active: true },
                mortgage: <Dto.IControlConfig>{ required: true, active: true },
                survey: <Dto.IControlConfig>{ required: true, active: true },
                searches: <Dto.IControlConfig>{ required: true, active: true },
                enquiries: <Dto.IControlConfig>{ required: true, active: true },
                contractAgreed: <Dto.IControlConfig>{ required: true, active: true },
                surveyDate: <Dto.IControlConfig>{ required: true, active: true }
            }
        }
    }

    angular.module('app').controller('offerChainPanelController', OfferChainPanelController);
}