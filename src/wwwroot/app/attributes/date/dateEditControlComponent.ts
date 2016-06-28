/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('dateEditControl', {
        templateUrl:'app/attributes/date/dateEditControl.html',
            controllerAs: 'vm',
            bindings: {
                ngModel: '=',
				config:'<',
                schema: '<',
                greaterThan: '<',
                lowerThan: '<'
            }
    });
}