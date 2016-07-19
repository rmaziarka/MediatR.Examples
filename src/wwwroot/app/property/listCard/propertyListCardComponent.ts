/// <reference path="../../typings/_all.d.ts" />

module Antares.Component.Property.ListCard {
    angular.module('app').component('propertyListCard', {
        controller: 'propertyListCardController',
        controllerAs: 'vm',
        templateUrl: 'app/property/listCard/propertyListCard.html',
        transclude: true,
        bindings: {
            properties: '<',
            isLoading: '<',
            allowMultipleSelect: '<',
            onSave: '&',
            onCancel: '&'
        }
    });
}