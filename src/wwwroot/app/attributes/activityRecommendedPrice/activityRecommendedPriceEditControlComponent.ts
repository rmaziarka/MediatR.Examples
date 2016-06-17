/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityRecommendedPriceEditControl', {
        templateUrl:'app/attributes/activityRecommendedPrice/activityRecommendedPriceEditControl.html',
            controllerAs: 'vm',
            bindings: {
                ngModel: '=',
				config:'<'
            }
    });
}