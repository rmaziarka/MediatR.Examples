/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityUserGenerator {
        public static generateDto(userType: Common.Models.Enums.NegotiatorTypeEnum): Dto.IActivityUser{
            var enumUserTypeItem: Dto.IEnumTypeItem = {
                id: ActivityUserGenerator.makeRandom('enumId'),
                code: Enums.NegotiatorTypeEnum[userType],
                enumTypeId: ActivityUserGenerator.makeRandom('enumTypeId')
            };

            var activityUser: Dto.IActivityUser = {
                id: ActivityUserGenerator.makeRandom('id'),
                user: UserGenerator.generateDto(),
                activityId: ActivityUserGenerator.makeRandom('activityId'),
                userId: ActivityUserGenerator.makeRandom('userId'),
                userType: enumUserTypeItem
            }

            return activityUser;
        }

        public static generateManyDtos(n: number, userType: Common.Models.Enums.NegotiatorTypeEnum): Dto.IActivityUser[] {
            return _.times(n, () => {return ActivityUserGenerator.generateDto(userType)});
        }

        public static generateMany(n: number, userType: Common.Models.Enums.NegotiatorTypeEnum): Business.ActivityUser[] {
            return _.map<Dto.IActivityUser, Business.ActivityUser>(ActivityUserGenerator.generateManyDtos(n, userType), (user: Dto.IActivityUser) => { return new Business.ActivityUser(user); });
        }

        public static generate(userType: Common.Models.Enums.NegotiatorTypeEnum): Business.ActivityUser {
            return new Business.ActivityUser(ActivityUserGenerator.generateDto(userType));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}