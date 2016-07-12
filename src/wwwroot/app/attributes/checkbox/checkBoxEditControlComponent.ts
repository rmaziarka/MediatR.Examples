/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('checkboxEditControl', {
        templateUrl: 'app/attributes/checkbox/checkboxEditControl.html',
        controllerAs: 'vm',
        transclude: {
            'overrideErrors': '?overrideErrors'
        },
        bindings: {
            config: '<',
            ngModel: '=',
            schema: '<'
        }
    });
}