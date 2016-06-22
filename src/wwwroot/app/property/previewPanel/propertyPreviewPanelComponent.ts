/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('propertyPreviewPanel', {
        templateUrl: 'app/property/previewPanel/propertyPreviewPanel.html',
        controller:'PropertyPreviewPanelController',
        controllerAs: 'vm',
        bindings: {
            property: '<',
            isVisible: '<'
        }
    });
}