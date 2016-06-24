/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import ActivityDepartment = Common.Models.Business.ActivityDepartment;

    export class ActivityDepartmentsViewControlController {
        // binding
        activityDepartments: ActivityDepartment[];
        config: IActivityDepartmentsViewControlConfig;
    }

    angular.module('app').controller('ActivityDepartmentsViewControlController', ActivityDepartmentsViewControlController);
};