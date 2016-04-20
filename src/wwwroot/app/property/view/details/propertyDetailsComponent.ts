/// <reference path="../../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('propertyDetails', {
        templateUrl: 'app/property/view/details/propertyDetails.html',
        controllerAs: 'vm',
        controller: 'propertyDetailsController',
        bindings: {
            property: '=',
            userData: '<'
        }
    });
}