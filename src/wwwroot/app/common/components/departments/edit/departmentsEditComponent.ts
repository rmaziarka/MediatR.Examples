/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component
{
    angular.module('app').component('departmentsEdit', {
        templateUrl: 'app/common/components/departments/edit/departmentsEdit.html',
        controllerAs : 'dvm',
        controller: 'DepartmentsController',
        bindings: {
            activityId: '<',
            departments: '=',
            leadNegotiator: '=',
            secondaryNegotiators: '='
        }
    });
}