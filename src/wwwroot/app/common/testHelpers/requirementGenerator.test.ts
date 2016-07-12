/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class RequirementGenerator {
        public static generateDto(specificData?: any): Dto.IRequirement{
            var requirement: Dto.IRequirement = {
                id: RequirementGenerator.makeRandom('id'),
                requirementTypeId: RequirementGenerator.makeRandom('requirementTypeid'),
                contacts : [],
                address : null,
                createDate : new Date(),
                requirementNotes : [],
                viewings : [],
                offers : [],
                attachments: [],
                solicitor: <Dto.IContact>{},
                solicitorCompany: <Dto.ICompany>{},
                tenancy: null
        }

            return angular.extend(requirement, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IRequirement[] {
            return _.times(n, RequirementGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Requirement[]{
            return _.map<Dto.IRequirement, Business.Requirement>(RequirementGenerator.generateManyDtos(n), (requirement: Dto.IRequirement) =>{ return new Business.Requirement(requirement); });
        }

        public static generate(specificData?: any): Business.Requirement {
            return new Business.Requirement(RequirementGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}