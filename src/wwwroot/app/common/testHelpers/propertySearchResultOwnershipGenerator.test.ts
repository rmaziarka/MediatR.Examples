/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class PropertySearchResultOwnershipGenerator {
        public static generateDto(specificData?:any): Dto.IPropertySearchResultOwnership {

            var ownership: Dto.IPropertySearchResultOwnership = {
                id: PropertySearchResultOwnershipGenerator.makeRandom('id'),
                ownershipTypeId: PropertySearchResultOwnershipGenerator.makeRandom('ownershipTypeId'),
                sellDate: new Date(),
                contacts: PropertySearchResultContactGenerator.generateManyDtos(3)
            }

            return angular.extend(ownership, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IPropertySearchResultOwnership[] {
            return _.times(n, PropertySearchResultOwnershipGenerator.generateDto);
        }

        public static generateMany(n: number): Business.PropertySearchResultOwnership[] {
            return _.map(PropertySearchResultOwnershipGenerator.generateManyDtos(n), (ownership: Dto.IPropertySearchResultOwnership) => { return new Business.PropertySearchResultOwnership(ownership); });
        }

        public static generate(specificData?: any): Business.PropertySearchResultOwnership {
            return new Business.PropertySearchResultOwnership(PropertySearchResultOwnershipGenerator.generateDto(specificData));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}