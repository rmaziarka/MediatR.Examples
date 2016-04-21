/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Chracteristic {
    angular.module('app').component('chracteristicList', {
        templateUrl: 'app/common/components/chracteristic/list/chracteristicList.html',
        controllerAs : 'clvm',
        controller: 'chracteristicListController',
        bindings: {
            componentId: '<',
            property: '=',
            mode: '@'
        }
    });
}