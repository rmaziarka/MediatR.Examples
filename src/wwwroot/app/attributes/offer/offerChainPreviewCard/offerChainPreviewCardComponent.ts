/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer.OfferChain {
    angular.module('app').component('offerChainPreviewCard', {
        templateUrl: 'app/attributes/offer/offerChainPreviewCard/offerChainPreviewCard.html',
        controllerAs: 'vm',
        controller: 'offerChainPreviewCardController',
        bindings: {
            chain: '<',
            onEdit: '&'
        }
    });
}