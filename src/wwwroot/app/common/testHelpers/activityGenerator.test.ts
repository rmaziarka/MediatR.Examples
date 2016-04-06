/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    export class ActivityGenerator {
        public static GenerateDto(): Dto.IActivity {

            var activity: Dto.IActivity = {
                activityStatusId: ActivityGenerator.makeRandom('activityStatusId'),
                contacts: [],
                createdDate: new Date(),
                id: ActivityGenerator.makeRandom('id'),
                property: new Dto.Property(),
                propertyId: ActivityGenerator.makeRandom('propertyId')
            }

            return activity;
        }

        public static GenerateManyDtos(n: number): Dto.IActivity[] {
            return _.times(n, ActivityGenerator.GenerateDto);
        }

        public static GenerateMany(n: number): Business.Activity[] {
            return _.map(ActivityGenerator.GenerateManyDtos(n), (activity: Dto.IActivity) => { return new Business.Activity(activity); });
        }

        public static Generate(): Business.Activity {
            return new Business.Activity(ActivityGenerator.GenerateDto());
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000);
        }
    }
}