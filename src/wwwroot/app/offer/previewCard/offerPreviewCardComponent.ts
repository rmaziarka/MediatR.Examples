/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    angular.module('app').component('offerPreviewCard', {
        templateUrl: 'app/offer/previewCard/offerPreviewCard.html',
        controllerAs: 'vm',
        controller: 'OfferPreviewCardController',
        bindings: {
            config: '<',
            offer: '<',
            canEdit: '<',
            onEdit: '&'
        }
    });
}