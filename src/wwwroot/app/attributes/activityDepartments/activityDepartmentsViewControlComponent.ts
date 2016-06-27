/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityDepartmentsViewControl', {
        templateUrl: 'app/attributes/activityDepartments/activityDepartmentsViewControl.html',
        controllerAs: 'vm',
        controller: 'ActivityDepartmentsViewControlController',
        bindings: {
            activityDepartments: '<',
            config: '<'
        }
    });
}