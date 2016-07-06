/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('percentNumberViewControl', {
        templateUrl: 'app/attributes/percentNumber/percentNumberViewControl.html',
        controllerAs: 'vm',
        bindings: {
            config: '<',
            schema: '<',
            value: '<'
        }
    });
}