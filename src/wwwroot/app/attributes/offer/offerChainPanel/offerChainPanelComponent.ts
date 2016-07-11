/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer.OfferChain {
    angular.module('app').component('offerChainPanel', {
        controller: 'offerChainPanelController',
        controllerAs : 'vm',
        templateUrl: 'app/attributes/offer/offerChainPanel/offerChainPanel.html',
        transclude : true,
        bindings: {
            isVisible: '<',
            isPreviewMode: '<',
            chain: '<',
            isLastChain: '<'
        }
    });
}