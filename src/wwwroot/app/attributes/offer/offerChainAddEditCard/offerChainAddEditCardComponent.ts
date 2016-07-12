/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer.OfferChain {
    angular.module('app').component('offerChainAddEditCard', {
        templateUrl: 'app/attributes/offer/offerChainAddEditCard/offerChainAddEditCard.html',
        controllerAs: 'vm',
        controller: 'offerChainAddEditCardController',
        bindings: {
            chain: '<',
            onEdit: '&'
        }
    });
}