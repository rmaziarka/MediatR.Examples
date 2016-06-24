/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityLandlordsViewControl', {
        templateUrl:'app/attributes/activityLandlords/activityLandlordsViewControl.html',
            controllerAs: 'vm',
            bindings: {
                contacts: '<',
                config:'<'
            }
    });
}