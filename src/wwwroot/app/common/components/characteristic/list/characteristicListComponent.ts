/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Characteristic {
    angular.module('app').component('characteristicList', {
        templateUrl: 'app/common/components/characteristic/list/characteristicList.html',
        controllerAs : 'clvm',
        controller: 'CharacteristicListController',
        bindings: {
            componentId: '<',
            property: '='
        }
    });
}