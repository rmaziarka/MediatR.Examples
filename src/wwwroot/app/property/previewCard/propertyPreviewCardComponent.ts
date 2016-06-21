/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('propertyPreviewCard', {
        templateUrl: 'app/property/previewCard/propertyPreviewCard.html',
        controller: 'PropertyPreviewCardController',
        controllerAs: 'vm',
        bindings: {
            property: '<'
        }
    });
}