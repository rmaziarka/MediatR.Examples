/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer {
    angular.module('app').component('offerChainsControl', {
        templateUrl: 'app/attributes/offer/offerChains/offerChainsControl.html',
        controllerAs: 'vm',
        controller: 'OfferChainsControlController',
        bindings: {
            isPanelVisible: '<',
            chainCommand: '<',
            chainType: '<',
            config:'<'
        }
    });
}