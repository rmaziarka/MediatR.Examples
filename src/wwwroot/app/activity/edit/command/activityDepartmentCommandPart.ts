/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;
    
    export class ActivityDepartmentCommandPart implements IActivityDepartmentCommandPart {
        departmentId: string = '';
        departmentTypeId: string = '';

        constructor(activityDepartment?: Business.ActivityDepartment) {
            if (activityDepartment) {
                this.departmentId = activityDepartment.department.id;
                this.departmentTypeId = activityDepartment.departmentTypeId;
            }
        }
    }
    
    export interface IActivityDepartmentCommandPart {
        departmentId: string;
        departmentTypeId: string;
    }
}