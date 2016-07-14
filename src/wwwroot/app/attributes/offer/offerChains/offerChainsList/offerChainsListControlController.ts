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
            this.onChainRemove({ chain: angular.copy(chain) });
        }

        editChain = (chain: ChainTransaction) => {
            this.onChainEdit({ chain: angular.copy(chain) });
        }

        previewChain = (chain: ChainTransaction) => {
            this.onChainPreview({ chain: angular.copy(chain) });
        }

        $onChanges = (obj: any) => {
            if (obj.chains && obj.chains.currentValue !== obj.chains.previousValue) {
                this.markLastChainElement();
            }
        }

        markLastChainElement = () => {
            this.chains.forEach((chain: ChainTransaction) => {
                var isParentToOther = this.chains.filter((innerChain: ChainTransaction) => {
                    return innerChain.parentId === chain.id;
                });
                if (isParentToOther.length > 0) {
                    chain.lastElementInChainLink = false;
                }
                else {
                    chain.lastElementInChainLink = true;
                }
            });
        }
    }

    angular.module('app').controller('OfferChainsListControlController', OfferChainsListControlController);
}