/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component
{
    angular.module('app').component('departmentsEdit', {
        templateUrl: 'app/common/components/departments/edit/departmentsEdit.html',
        controllerAs : 'dvm',
        controller: 'DepartmentsController',
        require: {
            activityEdit: '^^activityEdit'  
        },
        bindings: {
            activityId: '<',
            departments: '=',
            leadNegotiator: '=',
            secondaryNegotiators: '='
        }
    });
}