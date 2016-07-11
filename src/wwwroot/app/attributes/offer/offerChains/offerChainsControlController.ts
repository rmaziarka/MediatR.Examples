﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;
    import RequirementService = Antares.Requirement.RequirementService;
    import KfModalService = Antares.Services.KfModalService;
    import ActivityService = Services.ActivityService;

    export class OfferChainsControlController {
        // bindings
        isPanelVisible: Enums.SidePanelState;
        chainCommand: Common.Models.Commands.IChainTransactionCommand;
        property: Common.Models.Business.Property;
        config: IOfferChainsControlConfig;
        chainType: Enums.OfferChainsType;

        //fields
        currentChain: any;
        panelInPreviewMode: boolean = false;
        titleCode: string = "OFFER.VIEW.DELETE_CHAIN_TITLE";
        messageCode: string = "OFFER.VIEW.DELETE_CHAIN_MESSAGE";
        confirmCode: string = "OFFER.VIEW.DELETE_CHAIN_CONFIRM";

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
            var promise = this.kfModalService.showModal(this.titleCode, this.messageCode, this.confirmCode);
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