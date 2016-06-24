/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityStatusPreviewControl', {
        templateUrl:'app/attributes/activityStatus/activityStatusPreviewControl.html',
            controllerAs: 'vm',
            bindings: {
                statusId: '<',
                config:'<'
            }
    });
}