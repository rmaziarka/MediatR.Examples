/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityLandlordsPreviewControl', {
        templateUrl:'app/attributes/activityLandlords/activityLandlordsPreviewControl.html',
            controllerAs: 'vm',
            bindings: {
                contacts: '<',
                config:'<'
            }
    });
}