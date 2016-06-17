/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityRecommendedPriceViewControl', {
        templateUrl:'app/attributes/activityRecommendedPrice/activityRecommendedPriceViewControl.html',
            controllerAs: 'vm',
            bindings: {
                price: '=',
				config:'<'
            }
    });
}