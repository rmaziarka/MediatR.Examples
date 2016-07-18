/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('checkboxListEditControl', {
        templateUrl: 'app/attributes/checkboxList/checkboxListEditControl.html',
        controllerAs: 'vm',
        controller:'CheckboxListEditControlController',
        bindings: {
            config: '<',
            ngModel: '=',
            schema: '<'
        }
    });
}