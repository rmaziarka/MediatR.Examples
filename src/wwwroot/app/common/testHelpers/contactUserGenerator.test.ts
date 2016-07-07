/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ContactUserGenerator {
        public static generateDto(userType: Common.Models.Enums.NegotiatorTypeEnum): Dto.IContactUser{
            var enumUserTypeItem: Dto.IEnumTypeItem = {
                id: ContactUserGenerator.makeRandom('enumId'),
                code: Enums.NegotiatorTypeEnum[userType],
                enumTypeId: ContactUserGenerator.makeRandom('enumTypeId')
            };
            
            var activityUser: Dto.IContactUser = {
                id: ContactUserGenerator.makeRandom('id'),
                user: UserGenerator.generateDto(),
                contactId: ContactUserGenerator.makeRandom('contactId'),
                userId: ContactUserGenerator.makeRandom('userId'),
                userType: enumUserTypeItem
            }

            return activityUser;
        }

        public static generateManyDtos(n: number, userType: Common.Models.Enums.NegotiatorTypeEnum): Dto.IContactUser[] {
            return _.times(n, () => { return ContactUserGenerator.generateDto(userType) });
        }

        public static generateMany(n: number, userType: Common.Models.Enums.NegotiatorTypeEnum): Business.ContactUser[] {
            return _.map<Dto.IContactUser, Business.ContactUser>(ContactUserGenerator.generateManyDtos(n, userType), (user: Dto.IContactUser) => { return new Business.ContactUser(user); });
        }

        public static generate(userType: Common.Models.Enums.NegotiatorTypeEnum): Business.ContactUser {
            return new Business.ContactUser(ContactUserGenerator.generateDto(userType));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}