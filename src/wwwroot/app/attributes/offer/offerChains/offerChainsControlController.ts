/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import KfModalService = Services.KfModalService;
    import KfMessageService = Services.KfMessageService;
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
        chainToRemove: ChainTransaction;
        panelInPreviewMode: boolean = false;
        chains: Dto.IChainTransaction[];
        titleCode: string = "OFFER.VIEW.DELETE_CHAIN_TITLE";
        messageCode: string = "OFFER.VIEW.DELETE_CHAIN_MESSAGE";
        confirmCode: string = "OFFER.VIEW.DELETE_CHAIN_CONFIRM";
        successCode: string = "OFFER.VIEW.DELETE_CHAIN_SUCCESS";

        constructor(private eventAggregator: Core.EventAggregator,
            private chainTransationsService: Services.ChainTransationsService,
            private kfModalService: KfModalService,
            private kfMessageService: KfMessageService) { }

        private $onChanges = () => {
            var tempCommand = angular.copy(this.chainCommand);
            this.chains = tempCommand.chainTransactions;
        };

        public removeCurrentChainIfNotAvailable = () => {
            var chainFromList = this.chains.filter(c => c.id == this.currentChain.id)[0];

            if(chainFromList){
                return;
            }
        }

        public addChain = () => {
            this.currentChain = new ChainTransaction();
            this.panelInPreviewMode = false;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        };
        public editChain = (chain: ChainTransaction) => {
            this.currentChain = chain;
            this.panelInPreviewMode = false;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        };
        public previewChain = (chain: ChainTransaction) => {

            if (chain.parentId) {
                var parentChain = this.chains.filter((parentChain: ChainTransaction) => { return parentChain.id === chain.parentId; })[0];
                chain.parent = new Business.ChainTransaction(parentChain);
            }

            this.currentChain = chain;
            this.panelInPreviewMode = true;

            this.eventAggregator.publish(new OpenChainPanelEvent());
        };
        public removeChain = (chain: ChainTransaction) => {
            this.chainToRemove = chain;
            var promise = this.kfModalService.showModal(this.titleCode, this.messageCode, this.confirmCode);
            promise.then(this.onRemoveConfirm);
        };

        private onRemoveConfirm = () => {
            this.chainTransationsService
                .removeChain(this.chainToRemove, this.chainCommand, this.chainType)
                .then(this.chainRemoved);
        };

        private chainRemoved = (model: Dto.IActivity | Dto.IRequirement) => {
            if(this.currentChain && this.currentChain.id == this.chainToRemove.id){
                this.currentChain = null;

                if(this.isPanelVisible){
                    this.eventAggregator.publish(new Common.Component.CloseSidePanelEvent());
                }
            }
            
            if (this.chainType === Enums.OfferChainsType.Activity) {
                var activity = <Dto.IActivity>model;
                this.eventAggregator.publish(new ActivityUpdatedOfferChainsEvent(activity));
            }
            else {
                var requirement = <Dto.IRequirement>model;
                this.eventAggregator.publish(new RequirementUpdatedOfferChainsEvent(requirement));
            }

            this.kfMessageService.showSuccessByCode(this.successCode);
        }

        public isAddChainButtonVisible = () => {
            if (!this.chains || this.chains.length === 0) {
                return true;
            }

            return !this.chains[this.chains.length - 1].isEnd;
        }
    }

    angular.module('app').controller('OfferChainsControlController', OfferChainsControlController);
}