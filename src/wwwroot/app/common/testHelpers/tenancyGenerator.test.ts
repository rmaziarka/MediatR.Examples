/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class TenancyGenerator {
        public static generateDto(specificData?: any): Dto.ITenancy {
            var activity: Dto.IActivity = ActivityGenerator.generateDto();
            var requirement: Dto.IRequirement = RequirementGenerator.generateDto();
            var tenancyType: Dto.IEnumTypeItem = EnumTypeItemGenerator.generateDto(Enums.TenancyType[Enums.TenancyType.ResidentialLetting]);

            var landlords = TenancyContactGenerator.generateManyDtos(2, Enums.TenancyContactType.Landlord);
            var tenants = TenancyContactGenerator.generateManyDtos(2, Enums.TenancyContactType.Tenant);

            var term: Dto.ITenancyTerm = {
                id: StringGenerator.generate(),
                tenancyId: StringGenerator.generate(),
                agreedRent: 200,
                startDate: moment().toISOString(),
                endDate: moment().toISOString()
            };

            var tenancy: Dto.ITenancy = {
                id: StringGenerator.generate(),
                activity: activity,
                activityId: activity.id,
                requirement: requirement,
                requirementId: requirement.id,
                tenancyType: tenancyType,
                tenancyTypeId: tenancyType.id,
                contacts: [].concat(landlords, tenants),
                terms: [term]
            }

            return angular.extend(tenancy, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.ITenancy[] {
            return _.times(n, TenancyGenerator.generateDto);
        }

        public static generateManyEditModel(n: number): Business.TenancyEditModel[] {
            return _.map<Dto.ITenancy, Business.TenancyEditModel>(TenancyGenerator.generateManyDtos(n), (tenancy: Dto.ITenancy) => { return new Business.TenancyEditModel(tenancy); });
        }

        public static generateEditModel(specificData?: any): Business.TenancyEditModel {
            return new Business.TenancyEditModel(TenancyGenerator.generateDto(specificData));
        }
    }
}