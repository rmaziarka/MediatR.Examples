/// <reference path='../../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('propertyCardEditControl', {
        templateUrl:'app/attributes/property/propertyCard/propertyCardEditControl.html',
            controllerAs: 'vm',
            controller:'propertyCardEditControlController',
            bindings: {
                property: '<',
                config:'<',
                onAddEdit: '&'
            }
    });
}