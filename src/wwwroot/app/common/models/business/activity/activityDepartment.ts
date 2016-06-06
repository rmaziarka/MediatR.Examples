/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityDepartment {
        id: string ="";
        activityId: string = "";
        departmentId: string = "";
        department: Department = null;
        departmentType: Dto.IEnumTypeItem;

        constructor(activityDepartment?: Dto.IActivityDepartment) {
            if (activityDepartment) {
                angular.extend(this, activityDepartment);
            }
        }
    }
}