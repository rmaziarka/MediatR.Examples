/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('enumItemEditControl', {
        templateUrl: 'app/attributes/enumItem/enumItemEditControl.html',
        controllerAs: 'vm',
        controller: 'EnumItemEditControlController',
        bindings: {
            config: '<',
            ngModel: '=',
            onSelectedItemChanged: '&?',
            schema: '<',
            hideEmptyValue: '<'
        }
    });
}