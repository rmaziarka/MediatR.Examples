/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('priceViewControl', {
        templateUrl:'app/attributes/price/priceViewControl.html',
            controllerAs: 'vm',
            bindings: {
                price: '=',
				config:'<',
				schema:'<'
            }
    });
}