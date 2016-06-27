/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('offerStatusControl', {
        templateUrl: 'app/attributes/offerStatus/offerStatusEditControl.html',
        controllerAs: 'vm',
        controller: 'OfferStatusEditControlController',
        bindings: {
            config: '<',
            ngModel: '=',
            onOfferStatusChanged: '&'
        }
    });
}