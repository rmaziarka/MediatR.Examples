///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Business = Common.Models.Business;

        export class OfferPreviewController {
            componentId: string;
            offer: Business.Offer;

            constructor(
                private componentRegistry: Antares.Core.Service.ComponentRegistry,
                private $state: ng.ui.IStateService) {
                componentRegistry.register(this, this.componentId);
            }

            getOffer = (): Business.Offer =>{
                return this.offer;
            }

            setOffer = (offer: Business.Offer) => {
                this.offer = offer;
            }

            clearOfferPreview = () => {
                this.offer = new Business.Offer();
            }

            goToActivityView = () => {
                this.$state.go('app.activity-view', { id: this.offer.activity.id });
            }
        }

        angular.module('app').controller('offerPreviewController', OfferPreviewController);
    }
}