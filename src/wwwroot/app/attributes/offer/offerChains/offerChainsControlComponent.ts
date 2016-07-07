/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    angular.module('app').component('offerChainsControl', {
        templateUrl: 'app/attributes/offer/attachment/offerChains/offerChainsControl.html',
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