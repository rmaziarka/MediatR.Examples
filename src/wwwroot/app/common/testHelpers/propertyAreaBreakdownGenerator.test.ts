/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class PropertyAreaBreakdownGenerator {
        public static generateDto(specificData?: any): Dto.IPropertyAreaBreakdown {

            var propertyMock: Dto.IPropertyAreaBreakdown = {
                id: StringGenerator.generate(),
                name: StringGenerator.generate(),
                order: 1,
                propertyId: StringGenerator.generate(),
                size: 1
            };

            return angular.extend(propertyMock, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IPropertyAreaBreakdown[] {
            return _.times(n, PropertyAreaBreakdownGenerator.generateDto);
        }

        public static generateMany(n: number): Business.PropertyAreaBreakdown[] {
            return _.map<Dto.IPropertyAreaBreakdown, Business.PropertyAreaBreakdown>(
                PropertyAreaBreakdownGenerator.generateManyDtos(n),
                (area: Dto.IPropertyAreaBreakdown) => { return new Business.PropertyAreaBreakdown(area); });
        }

        public static generate(specificData?: any): Business.PropertyAreaBreakdown {
            return new Business.PropertyAreaBreakdown(PropertyAreaBreakdownGenerator.generateDto(specificData));
        }
    }
}