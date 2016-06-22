/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityDepartment {
        id: string ="";
        activityId: string = "";
        departmentId: string = "";
        department: Department = null;
        departmentTypeId: string;
        departmentType: Dto.IEnumItem;

        constructor(activityDepartment?: Dto.IActivityDepartment) {
            if (activityDepartment) {
                angular.extend(this, activityDepartment);
            }
        }

        public isManaging(): boolean {
            return this.departmentType.code === Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Managing];
        }
    }
}