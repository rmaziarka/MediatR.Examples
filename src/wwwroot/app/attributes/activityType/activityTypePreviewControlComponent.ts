/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityTypePreviewControl', {
        templateUrl:'app/attributes/activityType/activityTypePreviewControl.html',
            controllerAs: 'vm',
            bindings: {
                typeId: '<',
                config: '<'
            }
    });
}