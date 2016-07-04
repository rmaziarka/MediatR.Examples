/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('percentNumberEditControl', {
        templateUrl: 'app/attributes/percentNumber/percentNumberEditControl.html',
        controllerAs: 'vm',
        transclude: {
            'overrideErrors': '?overrideErrors'
        },
        bindings: {
            config: '<',
            schema: '<',
            value: '=',
            maxValue: '<'
        }
    });
}