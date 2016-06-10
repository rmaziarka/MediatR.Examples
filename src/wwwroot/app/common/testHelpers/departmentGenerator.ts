/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    export class DepartmentGenerator {
        public static generateDto(specificData?: any): Dto.IDepartment {
            var department: Dto.IDepartment = {
                id: DepartmentGenerator.makeRandom('departmentId'),
                name: DepartmentGenerator.makeRandom('departmentName'),
                countryId: DepartmentGenerator.makeRandom('countryId')
            };

            return angular.extend(department, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IDepartment[] {
            return _.times(n, DepartmentGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Department[] {
            return _.map<Dto.IDepartment, Business.Department>(DepartmentGenerator.generateManyDtos(n), (department: Dto.IDepartment) => { return new Business.Department(department); });
        }

        public static generate(specificData?: any): Business.Department {
            return new Business.Department(DepartmentGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}