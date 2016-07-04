/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class UserGenerator {
        public static generateDto(specificData?: any): Dto.IUser {

            var department: Dto.IDepartment = TestHelpers.DepartmentGenerator.generateDto();
            var user: Dto.IUser = {
                id: StringGenerator.generate(),
                firstName: StringGenerator.generate(),
                lastName: StringGenerator.generate(),
                departmentId: department.id,
                department: department
            }

            return angular.extend(user, specificData || {});
        }

        public static generateUserDataDto(): Dto.IUserData {
            var department: Dto.IDepartment = TestHelpers.DepartmentGenerator.generateDto();

            var userData: Dto.IUserData = {
                id: StringGenerator.generate(),
                firstName: StringGenerator.generate(),
                lastName: StringGenerator.generate(),
                name: StringGenerator.generate(),
                email: StringGenerator.generate(),
                country: StringGenerator.generate(),
                division: null,
                roles: [],
                department: department
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