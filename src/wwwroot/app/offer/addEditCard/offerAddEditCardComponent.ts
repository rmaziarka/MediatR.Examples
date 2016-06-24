/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    angular.module('app').component('offerAddEditCard', {
        templateUrl: 'app/offer/addEditCard/offerAddEditCard.html',
        controllerAs: 'vm',
        controller: 'OfferAddEditCardController',
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