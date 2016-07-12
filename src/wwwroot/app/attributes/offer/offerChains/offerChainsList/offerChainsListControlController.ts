/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    import Enums = Common.Models.Enums;
    import RequirementService = Requirement.RequirementService;
    import KfModalService = Services.KfModalService;
    import ActivityService = Antares.Services.ActivityService;
    import ChainTransaction = Common.Models.Business.ChainTransaction;
    import Property = Common.Models.Dto.IProperty;

    export class OfferChainsListControlController {
        // bindings
        chains: ChainTransaction[];
        chainType: Enums.OfferChainsType;
        property: Property;
        onChainEdit: (obj: { chain: ChainTransaction }) => void;
        onChainPreview: (obj: { chain: ChainTransaction }) => void;
        onChainRemove: (obj: { chain: ChainTransaction }) => void;

        constructor(private eventAggregator: Core.EventAggregator,
            private activityService: ActivityService,
            private requirementService: RequirementService,
            private kfModalService: KfModalService) { }

        deleteChain = (chain: ChainTransaction) => {
            this.onChainRemove({ chain: chain });
        }

        editChain = (chain: ChainTransaction) => {
            this.onChainEdit({ chain: chain });
        }

        previewChain = (chain: ChainTransaction) => {
            this.onChainPreview({ chain: chain });
        }
    }

    angular.module('app').controller('OfferChainsListControlController', OfferChainsListControlController);
}