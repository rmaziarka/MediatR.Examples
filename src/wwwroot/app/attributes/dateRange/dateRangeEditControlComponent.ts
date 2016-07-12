/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('dateRangeEditControl', {
        templateUrl: 'app/attributes/dateRange/dateRangeEditControl.html',
        controller: 'DateRangeEditControlController',
        controllerAs: 'vm',
        bindings: {
            dateFrom: '=',
            dateTo: '=',
            config: '<',
            schema: '<'
        }
    });
}