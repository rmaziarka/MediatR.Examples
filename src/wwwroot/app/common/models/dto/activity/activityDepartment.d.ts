declare module Antares.Common.Models.Dto {
    interface IActivityDepartment {
        id: string;
        activityId: string;
        departmentId: string;
        department: IDepartment;
        departmentType: Dto.IEnumItem;
    }
}