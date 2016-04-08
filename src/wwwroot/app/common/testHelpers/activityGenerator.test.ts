/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityGenerator {
        public static generateDto(): Dto.IActivity {

            var activity: Dto.IActivity = {
                activityStatusId: ActivityGenerator.makeRandom('activityStatusId'),
                contacts: [],
                createdDate: new Date(),
                id: ActivityGenerator.makeRandom('id'),
                property: PropertyGenerator.generate(),
                propertyId: ActivityGenerator.makeRandom('propertyId')
            }

            return activity;
        }

        public static generateManyDtos(n: number): Dto.IActivity[] {
            return _.times(n, ActivityGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Activity[] {
            return _.map<Dto.IActivity, Business.Activity>(ActivityGenerator.generateManyDtos(n), (activity: Dto.IActivity) => { return new Business.Activity(activity); });
        }

        public static generate(): Business.Activity {
            return new Business.Activity(ActivityGenerator.generateDto());
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000);
        }
    }
}