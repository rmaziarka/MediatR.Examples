/// <reference path="../../typings/_all.d.ts" />

module Antares.Viewing {
    angular.module('app').component('viewingPreviewPanel', {
        templateUrl: 'app/viewing/previewPanel/viewingPreviewPanel.html',
        controller:'ViewingPreviewPanelController',
        controllerAs: 'vm',
        bindings: {
            viewing: '<',
            isVisible: '<'
        }
    });
}