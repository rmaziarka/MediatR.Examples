/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityStatusControl', {
        templateUrl: 'app/attributes/activityStatus/activityStatusEditControl.html',
        controllerAs: 'vm',
        controller: 'ActivityStatusEditControlController',
        bindings: {
            config: '<',
            ngModel: '=',
            onActivityStatusChanged: '&'
        }
    });
}