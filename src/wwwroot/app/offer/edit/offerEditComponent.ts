/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    angular.module('app').component('offerEdit', {
        templateUrl: 'app/offer/edit/offerEdit.html',
        controllerAs : 'aevm',
        controller: 'OfferEditController',
        bindings: {
            offer: '='
        }
    });
}