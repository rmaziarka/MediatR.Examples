/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class UserGenerator {
        public static generateDto(specificData?: any): Dto.IUser{
            var departmentId = UserGenerator.makeRandom('departmentId');
            var user: Dto.IUser = {
                id: UserGenerator.makeRandom('id'),
                firstName: UserGenerator.makeRandom('firstName'),
                lastName: UserGenerator.makeRandom('lastName'),
                departmentId: departmentId,
                //TODO use generator
                department: { id: departmentId, name: 'name', countryId: '1'}
            }

            return angular.extend(user, specificData || {});
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

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}