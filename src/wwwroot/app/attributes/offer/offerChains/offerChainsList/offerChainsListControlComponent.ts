/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    angular.module('app').component('offerChainsList', {
        templateUrl: 'app/attributes/offer/offerChains/offerChainsList/offerChainsListControl.html',
        controllerAs: 'vm',
        controller: 'OfferChainsListControlController',
        bindings: {
            chains: '<',
            property: '<',
            chainType: '<',
            onChainEdit: '&',
            onChainPreview: '&',
            onChainRemove: '&'
        }
    });
}