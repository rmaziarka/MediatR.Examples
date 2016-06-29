/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    angular.module('app').component('offerEditCard', {
        templateUrl: 'app/offer/editCard/offerEditCard.html',
        controllerAs: 'vm',
        controller: 'OfferEditCardController',
        bindings: {
            config:'<',
            onSave: '&',
            onCancel: '&',
            pristineFlag: '<',
            offer: '<',
            backToPreview: '<'
        }
    });
}