/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityVendorEstimatedPriceViewControl', {
        templateUrl:'app/attributes/activityVendorEstimatedPrice/activityVendorEstimatedPriceViewControl.html',
            controllerAs: 'vm',
            bindings: {
                price: '=',
				config:'<'
            }
    });
}