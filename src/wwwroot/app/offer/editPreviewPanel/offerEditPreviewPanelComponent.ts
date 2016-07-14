/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('offerEditPreviewPanel', {
        controller: 'offerEditPreviewPanelController',
        controllerAs : 'vm',
        templateUrl: 'app/offer/editPreviewPanel/offerEditPreviewPanel.html',
        transclude : true,
        bindings: {
            isVisible: '<',
            offer: '<',
            requirement: '<',
            mode: '=',
            canEdit: '<',
            showActivity: '<',
            showRequirement: '<'
        }
    });
}