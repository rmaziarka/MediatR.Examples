/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityDepartmentGenerator {
        public static generateDto(specificData?: any): Dto.IActivityDepartment {
            var activityDepartment: Dto.IActivityDepartment = {
                id: ActivityDepartmentGenerator.makeRandom("id"),
                activityId: ActivityDepartmentGenerator.makeRandom('activityId'),
                departmentId: ActivityDepartmentGenerator.makeRandom('departmentId'),
                department: DepartmentGenerator.generateDto(),
                departmentType: EnumTypeItemGenerator.generateDto()
            };
            return activityDepartment;
        }

        public static generate(specificData?: any) {
            return new Business.ActivityDepartment(ActivityDepartmentGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}