/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityAskingPriceEditControl', {
        templateUrl:'app/attributes/activityAskingPrice/activityAskingPriceEditControl.html',
            controllerAs: 'vm',
            bindings: {
                ngModel: '=',
				config:'<'
            }
    });
}