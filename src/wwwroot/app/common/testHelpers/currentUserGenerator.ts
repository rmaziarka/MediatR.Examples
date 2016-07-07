/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    function makeRandom(text: string): string {
        return text + _.random(1, 1000000);
    }

    export class CurrentUserGenerator {
        public static generateDto(specificData?: any): Dto.ICurrentUser {
            var currentUser: Dto.ICurrentUser = {
                id: makeRandom('currentUserId'),
                firstName: makeRandom('currentUserId'),
                lastName: makeRandom('currentUserId'),
                email: makeRandom('currentUserId'),
                salutationFormatId: null,
                country: null,
                division: null,
                department: <Dto.IDepartment>null,
                roles: [],
                locale: LocaleGenerator.generateDto('en')
            };

            return angular.extend(currentUser, specificData || {});
        }
        
        public static generate(specificData?: any): Business.CurrentUser {
            return new Business.CurrentUser(CurrentUserGenerator.generateDto(specificData));
        }
    }
}