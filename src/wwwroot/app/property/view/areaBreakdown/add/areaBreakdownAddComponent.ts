/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('areaBreakdownAdd', {
        templateUrl: 'app/property/view/areaBreakdown/add/areaBreakdownAdd.html',
        controllerAs: 'vm',
        controller: 'AreaBreakdownAddController',
        bindings: {
            componentId: '<'
        }
    });
}