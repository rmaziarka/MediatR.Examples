/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    angular.module('app').component('offerAddCard', {
        templateUrl: 'app/offer/addCard/offerAddCard.html',
        controllerAs: 'vm',
        controller: 'OfferAddCardController',
        bindings: {
            config:'<',
            onSave: '&',
            onCancel: '&',
            onReloadConfig: '&',
            pristineFlag: '<',
            activity: '<'
        }
    });
}