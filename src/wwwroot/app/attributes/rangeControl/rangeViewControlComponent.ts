/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('rangeViewControl', {
        templateUrl: 'app/attributes/rangeControl/rangeViewControl.html',
        controllerAs: 'vm',
        controller: 'RangeViewControlController',
        bindings: {
            min: '<',
            max: '<',
            config: '<',
            schema: '<'
        }
    });
}