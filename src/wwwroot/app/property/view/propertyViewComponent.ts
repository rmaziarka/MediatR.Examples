/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('propertyView', {
        templateUrl: 'app/property/view/propertyView.html',
        controllerAs: 'vm',
        controller: 'propertyViewController',
        bindings: {
            property: '<',
            userData: '<'
        }
    });
}