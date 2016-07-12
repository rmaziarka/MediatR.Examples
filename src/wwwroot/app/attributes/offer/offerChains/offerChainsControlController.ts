/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;
    import RequirementService = Antares.Requirement.RequirementService;
    import KfModalService = Antares.Services.KfModalService;
    import ActivityService = Services.ActivityService;
    import ChainTransaction = Common.Models.Business.ChainTransaction;

    export class OfferChainsControlController {
        // bindings
        isPanelVisible: Enums.SidePanelState;
        chainCommand: Common.Models.Commands.IChainTransactionCommand;
        property: Common.Models.Business.Property;
        config: IOfferChainsControlConfig;
        chainType: Enums.OfferChainsType;

        //fields
        currentChain: ChainTransaction;
        panelInPreviewMode: boolean = false;
        titleCode: string = "OFFER.VIEW.DELETE_CHAIN_TITLE";
        messageCode: string = "OFFER.VIEW.DELETE_CHAIN_MESSAGE";
        confirmCode: string = "OFFER.VIEW.DELETE_CHAIN_CONFIRM";

        constructor(private eventAggregator: Core.EventAggregator,
            private activityService: ActivityService,
            private requirementService: RequirementService,
            private kfModalService: KfModalService) { }

        addChain = () =>{
            this.currentChain = new ChainTransaction();
            this.panelInPreviewMode = false;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        editChain = (chain: ChainTransaction) => {
            this.currentChain = chain;
            this.panelInPreviewMode = false;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        previewChain = (chain: ChainTransaction) => {
            this.currentChain = chain
            this.panelInPreviewMode = true;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        }

        removeChain = (chain: ChainTransaction) =>{
            var promise = this.kfModalService.showModal(this.titleCode, this.messageCode, this.confirmCode);
            // publish event
        }

        updateChain = (chain: ChainTransaction) =>{
            
        }

        isEndOfChainVisibleInPanel = () =>{
            return false;
        }
    }

    angular.module('app').controller('OfferChainsControlController', OfferChainsControlController);
}