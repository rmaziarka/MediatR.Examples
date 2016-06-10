/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    angular.module('app').component('offerEdit', {
        templateUrl: 'app/offer/edit/offerEdit.html',
        controllerAs : 'vm',
        controller: 'OfferEditController',
        bindings: {
            offer: '='
        }
    });
}