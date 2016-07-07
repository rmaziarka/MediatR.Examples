/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;

    export class OfferChainsControlController {
        // bindings
        isPanelVisible: Enums.SidePanelState;
        chainCommand: any;
        config: IOfferChainsControlConfig;
        chainType: Enums.OfferChainsType;

        //fields
        currentChain: any;
        panelInPreviewMode: boolean = false;

        constructor(private eventAggregator: Core.EventAggregator) {}

        addChain = () =>{
            this.currentChain = {};
            this.panelInPreviewMode = false;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        editChain = (chain: any) => {
            this.currentChain = chain;
            this.panelInPreviewMode = false;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        previewChain = (chain: any) => {
            this.currentChain = chain;
            this.panelInPreviewMode = true;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        removeChain = (chain: any) =>{
            // remove chain
            // publish event
        }
    }

    angular.module('app').controller('OfferChainsControlController', OfferChainsControlController);
}