/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityShortLetPricePerWeekEditControl', {
        templateUrl:'app/attributes/activityShortLetPricePerWeek/activityShortLetPricePerWeekEditControl.html',
            controllerAs: 'vm',
            bindings: {
                ngModel: '=',
				config:'<'
            }
    });
}