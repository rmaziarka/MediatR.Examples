/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Viewing = Common.Models.Business.Viewing;
    import Enums = Common.Models.Enums;

    export class OfferViewControlController {
        // bindings
        offers: Business.Offer[];
        config: IOfferViewControlConfig;
        isPanelVisible: Enums.SidePanelState;
        
        //fields
        selectedOffer: Dto.IOffer;

        constructor(private eventAggregator: Core.EventAggregator,
            private $state: ng.ui.IStateService) {
        }

        showOfferPreview = (offer: Dto.IOffer) => {
            this.selectedOffer = offer;
            this.publishOpenEvent();
        };

        private publishOpenEvent = () =>{
            this.eventAggregator.publish(new OpenOfferPreviewPanelEvent());
        }

        navigateToOfferView = (offer: Common.Models.Dto.IOffer) => {
            this.$state.go('app.offer-view', { id: offer.id });
        };

        navigateToEditTenancy = (offer: Common.Models.Dto.IOffer) => {
            this.$state.go('app.tenancy-edit', { activityId: offer.activityId, requirementId: offer.requirementId });
        };
    }

    angular.module('app').controller('OfferViewControlController', OfferViewControlController);
};