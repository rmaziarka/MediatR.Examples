/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityVendorEstimatedPriceEditControl', {
        templateUrl:'app/attributes/activityVendorEstimatedPrice/activityVendorEstimatedPriceControl.html',
            controllerAs: 'vm',
            bindings: {
                ngModel: '='
            }
    });
}