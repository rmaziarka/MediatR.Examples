/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('rangeEditControl', {
        templateUrl: 'app/attributes/rangeControl/rangeEditControl.html',
        controllerAs: 'vm',
        bindings: {
            min: '=',
            max: '=',
            config: '<',
            schema: '<'
        }
    });
}