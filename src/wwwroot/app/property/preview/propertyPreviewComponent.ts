/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('propertyPreview', {
        templateUrl: 'app/property/preview/propertyPreview.html',
        controllerAs: 'ppvm',
        controller: 'PropertyPreviewController',
        bindings: {
            componentId: '<'
        }
    });
}