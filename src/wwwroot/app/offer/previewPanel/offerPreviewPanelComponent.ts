/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    angular.module('app').component('offerPreviewPanel', {
        templateUrl: 'app/offer/previewPanel/offerPreviewPanel.html',
        controller:'OfferPreviewPanelController',
        controllerAs: 'vm',
        bindings: {
            offer: '<',
            isVisible: '<'
        }
    });
}