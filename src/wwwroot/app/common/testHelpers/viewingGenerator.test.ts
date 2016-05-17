/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ViewingGenerator {
        public static generateDto(specificData?: any): Dto.IViewing {

            var viewing: Business.Viewing = {
                startDate: new Date(),
                endDate: new Date(),
                id: ViewingGenerator.makeRandom('id'),
                negotiatorId: ViewingGenerator.makeRandom('negotiatorId'),
                negotiator: new Business.User({ id: ViewingGenerator.makeRandom('id'), firstName: ViewingGenerator.makeRandom('firstName'), lastName: ViewingGenerator.makeRandom('lastName') }),
                requirementId: ViewingGenerator.makeRandom('requirementId'),
                activityId: ViewingGenerator.makeRandom('activityId'),
                invitationText: ViewingGenerator.makeRandom('invitationText'),
                postViewingComment: ViewingGenerator.makeRandom('postViewingComment'),
                attendeesIds: [],
                attendees: [],
                activity: ActivityGenerator.generate(),
                day: "",
                requirement: RequirementGenerator.generate()
            }

            return angular.extend(viewing, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IViewing[] {
            return _.times(n, ViewingGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Viewing[] {
            return _.map<Dto.IViewing, Business.Viewing>(ViewingGenerator.generateManyDtos(n), (viewing: Dto.IViewing) => { return new Business.Viewing(viewing); });
        }

        public static generate(specificData?: any): Business.Viewing {
            return new Business.Viewing(ViewingGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}