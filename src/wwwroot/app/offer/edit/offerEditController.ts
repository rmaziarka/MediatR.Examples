/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class OfferEditController {
        public offer: Business.Offer;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService) {
        }

        public save() {
        }

        cancel() {
            this.$state.go('app.offer-view', { id: this.offer.id });
        }
    }

    angular.module('app').controller('OfferEditController', OfferEditController);
};