/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('offerPreview', {
        controller: 'offerPreviewController',
        controllerAs : 'vm',
        templateUrl: 'app/offer/preview/offerPreview.html',
        transclude : {
            customItem: "?offerPreviewCustomItem"
        },
        bindings : {
            componentId: '<',
            offer: '<'
        }
    });
}