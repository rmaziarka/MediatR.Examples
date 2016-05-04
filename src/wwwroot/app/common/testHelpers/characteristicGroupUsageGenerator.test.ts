/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CharacteristicGroupUsageGenerator {
        public static generateDto(specificData?: any): Dto.ICharacteristicGroupUsage {

            var characteristicGroupUsage: Dto.ICharacteristicGroupUsage = {
                id: CharacteristicGroupUsageGenerator.makeRandom('characteristicGroupUsageId'),
                propertyTypeId: CharacteristicGroupUsageGenerator.makeRandom('propertyId'),
                countryId: CharacteristicGroupUsageGenerator.makeRandom('countryId'),
                characteristicGroupId: CharacteristicGroupUsageGenerator.makeRandom('characteristicGroupId'),
                characteristicGroupItems: [],
                order: 1,
                isDisplayLabel: true
            }

            return angular.extend(characteristicGroupUsage, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.ICharacteristicGroupUsage[] {
            return _.times(n, CharacteristicGroupUsageGenerator.generateDto);
        }

        public static generate(specificData?: any): Business.CharacteristicGroup {
            var characteristicGroup = new Business.CharacteristicGroup(CharacteristicGroupUsageGenerator.generateDto());
            return angular.extend(characteristicGroup, specificData || {});
        }

        public static generateMany(n: number): Business.CharacteristicGroup[] {
            return _.map<Dto.ICharacteristicGroupUsage, Business.CharacteristicGroup>(CharacteristicGroupUsageGenerator.generateManyDtos(n), (characteristicGroupUsage: Dto.ICharacteristicGroupUsage) => { return new Business.CharacteristicGroup(characteristicGroupUsage); });
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}