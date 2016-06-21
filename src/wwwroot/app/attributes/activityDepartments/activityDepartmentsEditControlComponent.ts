/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes
{
    angular.module('app').component('activityDepartmentsEditControl', {
        templateUrl: 'app/attributes/activityDepartments/activityDepartmentsEditControl.html',
        controllerAs : 'vm',
        controller: 'ActivityDepartmentsEditControlController',
        bindings: {
            activityId: '<',
            departments: '=',
            departmentIsRelatedWithNegotiator: '&',
            config: '<'
        }
    });
}