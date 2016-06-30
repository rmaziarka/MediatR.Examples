/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('enumItemViewControl', {
        templateUrl: 'app/attributes/enumItem/enumItemViewControl.html',
        controllerAs: 'vm',
        controller: 'EnumItemViewControlController',
        bindings: {
            schema: '<',
            config: '<',
            ngModel: '='
        }
    });
}