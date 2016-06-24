/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('propertyViewControl', {
        templateUrl: 'app/attributes/property/propertyViewControl.html',
        controllerAs: 'vm',
        controller: 'PropertyViewControlController',
        bindings: {
            property: '<',
            config: '<',
            isPanelVisible: '<'
        }
    });
}