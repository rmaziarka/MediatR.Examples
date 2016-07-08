/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityDepartmentGenerator {
        public static generateDto(specificData?: any): Dto.IActivityDepartment {
            var departmentGenerator = new TestHelpers.DepartmentGenerator();
            var department = departmentGenerator.generateDto();
            var departmentType =  EnumTypeItemGenerator.generateDto();

            var activityDepartment: Dto.IActivityDepartment = {
                id: ActivityDepartmentGenerator.makeRandom("id"),
                activityId: ActivityDepartmentGenerator.makeRandom('activityId'),
                departmentId: department.id,
                department: department,
                departmentType:departmentType
            };
            return angular.extend(activityDepartment, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IActivityDepartment[] {
            return _.times(n, ActivityDepartmentGenerator.generateDto);
        }

        public static generate(specificData?: any) {
            return new Business.ActivityDepartment(ActivityDepartmentGenerator.generateDto(specificData));
        }

        public static generateMany(n: number): Business.ActivityDepartment[] {
            return _.map<Dto.IActivityDepartment, Business.ActivityDepartment>(ActivityDepartmentGenerator.generateManyDtos(n), (activityDepartment: Dto.IActivityDepartment) => { return new Business.ActivityDepartment(activityDepartment); });
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}