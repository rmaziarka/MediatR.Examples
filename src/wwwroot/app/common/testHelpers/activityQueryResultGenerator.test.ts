/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityQueryResultGenerator {
        public static generateDto(specificData?: any): Dto.IActivityQueryResult {

            var activity: Dto.IActivityQueryResult = {                
                id: ActivityQueryResultGenerator.makeRandom('id'),
                propertyName: ActivityQueryResultGenerator.makeRandom('propertyName'),
                propertyNumber: ActivityQueryResultGenerator.makeRandom('propertyNumber'),
                line2: ActivityQueryResultGenerator.makeRandom('line2'),                                
            }

            return angular.extend(activity, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IActivityQueryResult[] {
            return _.times(n, ActivityQueryResultGenerator.generateDto);
        }

        public static generateMany(n: number): Business.ActivityQueryResult[] {
            return _.map<Dto.IActivityQueryResult, Business.ActivityQueryResult>(ActivityQueryResultGenerator.generateManyDtos(n), (activity: Dto.IActivityQueryResult) => { return new Business.ActivityQueryResult(activity); });
        }

        public static generate(specificData?: any): Business.ActivityQueryResult {
            return new Business.ActivityQueryResult(ActivityQueryResultGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}