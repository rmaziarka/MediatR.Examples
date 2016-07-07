/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('offerViewControl', {
        templateUrl: 'app/attributes/offer/offerViewControl.html',
        controllerAs: 'vm',
        controller: 'OfferViewControlController',
        bindings: {
            offers: '<',            
            isPanelVisible: '<',
            config: '<'
        }
    });
}