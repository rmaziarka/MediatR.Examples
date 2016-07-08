/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    export class DepartmentGenerator extends TestDataGenerator<Dto.IDepartment, Business.Department> {

        public generateDto(specificData?: any): Dto.IDepartment {
            var department: Dto.IDepartment = {
                id: super.makeRandom('departmentId'),
                name: super.makeRandom('departmentName'),
                countryId: super.makeRandom('countryId')
            };

            return angular.extend(department, specificData || {});
        }
    }
}