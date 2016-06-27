/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityVendorsPreviewControl', {
        templateUrl:'app/attributes/activityVendors/activityVendorsPreviewControl.html',
            controllerAs: 'vm',
            bindings: {
                contacts: '<',
                config:'<'
            }
    });
}