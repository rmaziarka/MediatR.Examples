/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('activityStatusControl', {
        templateUrl: 'app/attributes/activityStatus/activityStatusControl.html',
        controllerAs: 'vm',
        controller: 'ActivityStatusControlController',
        bindings: {
            ngModel: '='
        }
    });
}