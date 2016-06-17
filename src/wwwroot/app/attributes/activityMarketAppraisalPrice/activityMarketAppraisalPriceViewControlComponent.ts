/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityMarketAppraisalPriceViewControl', {
        templateUrl:'app/attributes/activityMarketAppraisalPrice/activityMarketAppraisalPriceViewControl.html',
            controllerAs: 'vm',
            bindings: {
                price: '=',
				config:'<'
            }
    });
}