/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityAskingPriceViewControl', {
        templateUrl:'app/attributes/activityAskingPrice/activityAskingPriceViewControl.html',
            controllerAs: 'vm',
            bindings: {
                price: '=',
				config:'<'
            }
    });
}