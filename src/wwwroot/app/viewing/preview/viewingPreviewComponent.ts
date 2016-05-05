/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('viewingPreview', {
        controller: 'viewingPreviewController',
        controllerAs : 'vm',
        templateUrl: 'app/viewing/preview/viewingPreview.html',
        transclude : true,
        bindings : {
            componentId: '<'
        }
    });
}