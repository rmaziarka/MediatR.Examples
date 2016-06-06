/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateActivityDepartmentResource implements Dto.IUpdateActivityDepartmentResource {
        departmentId: string = '';
        departmentTypeId: string = '';

        constructor(activityDepartment?: Business.ActivityDepartment) {
            if (activityDepartment) {
                this.departmentId = activityDepartment.department.id;
                this.departmentTypeId = activityDepartment.departmentTypeId;
            }
        }
    }
}