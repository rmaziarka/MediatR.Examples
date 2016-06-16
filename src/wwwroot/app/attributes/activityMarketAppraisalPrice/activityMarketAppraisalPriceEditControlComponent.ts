/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityMarketAppraisalPriceEditControl', {
        templateUrl:'app/attributes/activityMarketAppraisalPrice/activityMarketAppraisalPriceEditControl.html',
            controllerAs: 'vm',
            bindings: {
                ngModel: '='
            }
    });
}