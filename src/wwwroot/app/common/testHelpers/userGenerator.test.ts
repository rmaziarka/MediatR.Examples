/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class UserGenerator {
        public static generateDto(specificData?: any): Dto.IUser {
            var departmentGenerator = new TestHelpers.DepartmentGenerator();

            var department: Dto.IDepartment = departmentGenerator.generateDto();
            var user: Dto.IUser = {
                id: StringGenerator.generate(),
                firstName: StringGenerator.generate(),
                lastName: StringGenerator.generate(),
                departmentId: department.id,
                department: department
            }

            return angular.extend(user, specificData || {});
        }

        public static generateUserDataDto(): Dto.ICurrentUser {
            var departmentGenerator = new TestHelpers.DepartmentGenerator();
            var department: Dto.IDepartment = departmentGenerator.generateDto();

            var userData: Dto.ICurrentUser = {
                id: StringGenerator.generate(),
                firstName: StringGenerator.generate(),
                lastName: StringGenerator.generate(),
                email: StringGenerator.generate(),
                country: null,
                division: null,
                roles: [],
                salutationFormatId: null,
                department: department,
                locale: LocaleGenerator.generateDto('en')
            }

            return userData;
        }

        public static generateManyDtos(n: number): Dto.IUser[] {
            return _.times(n, UserGenerator.generateDto);
        }

        public static generateMany(n: number): Business.User[] {
            return _.map<Dto.IUser, Business.User>(UserGenerator.generateManyDtos(n), (user: Dto.IUser) => { return new Business.User(user); });
        }

        public static generate(specificData?: any): Business.User {
            return new Business.User(UserGenerator.generateDto(specificData));
        }
    }
}