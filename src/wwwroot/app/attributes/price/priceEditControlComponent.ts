/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('priceEditControl', {
        templateUrl:'app/attributes/price/priceEditControl.html',
            controllerAs: 'vm',
            bindings: {
                ngModel: '=',
				config:'<',
				schema:'<'
            }
    });
}