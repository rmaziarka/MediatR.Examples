/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityLandlordsEditControl', {
        templateUrl: 'app/attributes/activityLandlords/activityLandlordsEditControl.html',
        controllerAs: 'vm',
        controller: 'ActivityLandlordsEditControlController',
        bindings: {
            contacts: '<',
            config: '<'
        }
    });
}