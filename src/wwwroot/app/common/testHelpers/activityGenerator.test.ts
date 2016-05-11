/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityGenerator {
        public static generateDto(specificData?: any): Dto.IActivity {

            var activity: Dto.IActivity = {
                activityStatusId: ActivityGenerator.makeRandom('activityStatusId'),
                activityTypeId: ActivityGenerator.makeRandom('activityTypeId'),
                contacts: [],
                attachments: [],
                createdDate: new Date(),
                id: ActivityGenerator.makeRandom('id'),
                property: PropertyGenerator.generateDto(),
                propertyId: ActivityGenerator.makeRandom('propertyId'),
            }

            return angular.extend(activity, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IActivity[] {
            return _.times(n, ActivityGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Activity[] {
            return _.map<Dto.IActivity, Business.Activity>(ActivityGenerator.generateManyDtos(n), (activity: Dto.IActivity) => { return new Business.Activity(activity); });
        }

        public static generate(specificData?: any): Business.Activity {
            return new Business.Activity(ActivityGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}