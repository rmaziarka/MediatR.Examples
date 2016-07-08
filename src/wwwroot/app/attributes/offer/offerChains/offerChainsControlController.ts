/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;
    import RequirementService = Antares.Requirement.RequirementService;
    import KfModalService = Antares.Services.KfModalService;
    import ActivityService = Antares.Activity.ActivityService;

    export class OfferChainsControlController {
        // bindings
        isPanelVisible: Enums.SidePanelState;
        chainCommand: any;
        config: IOfferChainsControlConfig;
        chainType: Enums.OfferChainsType;

        //fields
        currentChain: any;
        panelInPreviewMode: boolean = false;

        constructor(private eventAggregator: Core.EventAggregator,
            private activityService: ActivityService,
            private requirementService: RequirementService,
            private kfModalService: KfModalService) { }

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
            var titleCode = "OFFER.VIEW.DELETE_CHAIN_TITLE";
            var messageCode = "OFFER.VIEW.DELETE_CHAIN_MESSAGE";
            var confirmCode = "OFFER.VIEW.DELETE_CHAIN_CONFIRM";
            var promise = this.kfModalService.showModal(titleCode, messageCode, confirmCode);
            // publish event
        }

        updateChain = (chain: any) =>{
            
        }

        isEndOfChainVisibleInPanel = () =>{
            return false;
        }
    }

    angular.module('app').controller('OfferChainsControlController', OfferChainsControlController);
}