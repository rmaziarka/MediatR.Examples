/// <reference path="../../typings/_all.d.ts" />

module Antares.Viewing {
    angular.module('app').component('offerPreviewCard', {
        templateUrl: 'app/offer/previewCard/offerPreviewCard.html',
        controller: 'OfferPreviewCardController',
        controllerAs: 'vm',
        bindings: {
            offer: '<'
        }
    });
}