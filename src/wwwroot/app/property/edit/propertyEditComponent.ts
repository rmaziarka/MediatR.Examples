/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('propertyEdit', {
        templateUrl: 'app/property/edit/propertyEdit.html',
        controllerAs: 'vm',
        controller: 'PropertyEditController',
        bindings: {
            property: '=',
            userData: '<'
        }
    });
}