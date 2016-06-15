/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityVendorsViewControl', {
        templateUrl:'app/attributes/activityVendors/activityVendorsViewControl.html',
            controllerAs: 'vm',
            bindings: {
                contacts: '<',
                config:'<'
            }
    });
}