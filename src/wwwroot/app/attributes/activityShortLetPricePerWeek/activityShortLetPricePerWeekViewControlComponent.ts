/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityShortLetPricePerWeekViewControl', {
        templateUrl:'app/attributes/activityShortLetPricePerWeek/activityShortLetPricePerWeekViewControl.html',
            controllerAs: 'vm',
            bindings: {
                price: '=',
				config:'<'
            }
    });
}