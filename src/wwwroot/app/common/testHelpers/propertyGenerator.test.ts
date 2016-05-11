/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class PropertyGenerator {
        public static generateDto(specificData?: any): Dto.IProperty {

            var propertyMock: Dto.IProperty = {
                id: PropertyGenerator.makeRandom('id'),
                propertyTypeId: PropertyGenerator.makeRandom('propertyTypeId'),
                address: Mock.AddressForm.FullAddress,
                ownerships: [],
                activities: [],
                divisionId: PropertyGenerator.makeRandom('divisionId'),
                division: null,
                attributeValues: {},
                propertyCharacteristics: []
            };

            return angular.extend(propertyMock, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IProperty[] {
            return _.times(n, PropertyGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Property[] {
            return _.map<Dto.IProperty, Business.Property>(PropertyGenerator.generateManyDtos(n), (property: Dto.IProperty) => { return new Business.Property(property); });
        }

        public static generate(specificData?: any): Business.Property {
            return new Business.Property(PropertyGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}