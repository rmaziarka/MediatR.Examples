/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('areaBreakdownEdit', {
        templateUrl: 'app/property/view/areaBreakdown/edit/areaBreakdownEdit.html',
        controllerAs: 'vm',
        controller: 'AreaBreakdownEditController',
        bindings: {
            componentId: '<'
        }
    });
}