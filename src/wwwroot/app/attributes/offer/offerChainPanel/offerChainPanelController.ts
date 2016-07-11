///<reference path="../../../typings/_all.d.ts"/>

module Antares.Attributes.Offer.OfferChain {
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import Business = Common.Models.Business;

    export class OfferChainPanelController extends Common.Component.BaseSidePanelController {
        // bindings
        isVisible: Enums.SidePanelState;
        isPreviewMode: boolean;
        chain: Business.ChainTransaction;
        isLastChain: boolean;

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

    }

    angular.module('app').controller('offerChainPanelController', OfferChainPanelController);
}