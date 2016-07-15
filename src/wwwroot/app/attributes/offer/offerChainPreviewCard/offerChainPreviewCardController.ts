/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer.OfferChain {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class OfferChainPreviewCardController {
        // bindings
        chain: Business.ChainTransaction;
        config: any;
        onEdit: () => void;

        controlSchemas: OfferChainPreviewSchema = OfferChainPreviewSchema;

        constructor(
            private $state: ng.ui.IStateService) {
        }

        edit = () => {
            this.onEdit();
        }

        navigateToProperty = (property: Dto.IProperty) => {
            this.$state.go('app.property-view', { id: property.id });
        }
    }

    angular.module('app').controller('offerChainPreviewCardController', OfferChainPreviewCardController);
}