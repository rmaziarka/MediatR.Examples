/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;
    import RequirementService = Requirement.RequirementService;
    import KfModalService = Services.KfModalService;
    import ActivityService = Antares.Services.ActivityService;
    import ChainTransaction = Common.Models.Dto.IChainTransaction;
    import Property = Common.Models.Dto.IProperty;

    export class OfferChainsListControlController {
        // bindings
        chains: ChainTransaction[];
        chainType: Enums.OfferChainsType;
        property: Property;
        onChainEdit: (chain: ChainTransaction) => void;
        onChainPreview: (chain: ChainTransaction) => void;
        onChainRemove: (chain: ChainTransaction) => void;

        constructor(private eventAggregator: Core.EventAggregator,
            private activityService: ActivityService,
            private requirementService: RequirementService,
            private kfModalService: KfModalService) { }

        deleteChain = (chain: ChainTransaction) => {
            this.onChainRemove(chain);
        }

        editChain = (chain: ChainTransaction) => {
            this.onChainEdit(chain);
        }

        previewChain = (chain: ChainTransaction) => {
            this.onChainPreview(chain);
        }
    }

    angular.module('app').controller('OfferChainsListControlController', OfferChainsListControlController);
}