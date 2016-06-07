/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    export class DepartmentGenerator {
        public static generateDto(): Dto.IDepartment {
            var department: Dto.IDepartment = {
                id: DepartmentGenerator.makeRandom('departmentId'),
                name: DepartmentGenerator.makeRandom('departmentName'),
                countryId: DepartmentGenerator.makeRandom('countryId')
            };
            return department;
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}