/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Characteristic {
    angular.module('app').component('characteristicList', {
        templateUrl: 'app/common/components/chracteristic/list/characteristicList.html',
        controllerAs : 'clvm',
        controller: 'characteristicListController',
        bindings: {
            componentId: '<',
            property: '=',
            mode: '@'
        }
    });
}