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
            this.chain = new Business.ChainTransaction();
            this.chain.property = new Business.PreviewProperty();
            this.chain.property.address = new Business.Address();
            this.chain.property.address.countryId = 'countryId';
            this.chain.property.address.line2 = "line2";
            this.chain.surveyDate = new Date();
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