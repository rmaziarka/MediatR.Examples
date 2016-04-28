/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Characteristic {
    angular.module('app').component('characteristicListView', {
        templateUrl: 'app/common/components/characteristic/list/characteristicListView.html',
        controllerAs : 'clvm',
        controller: 'CharacteristicListController',
        bindings: {
            componentId: '<',
            propertyTypeId: '<',
            countryId: '<',
            characteristicsMap: '<'
        }
    });
}