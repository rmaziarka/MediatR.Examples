/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityLandlordsControl', {
        templateUrl: 'app/attributes/activityLandlords/activityLandlordsControl.html',
        controllerAs: 'vm',
        controller: 'ActivityLandlordsControlController',
        bindings: {
            landlordsContacts: '<',
            config: '<'
        }
    });
}