/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CharacteristicSelectGenerator {
        public static generate(specificData?: any): Business.CharacteristicSelect{

            var characteristicSelect: Business.CharacteristicSelect = new Business.CharacteristicSelect({
                characteristicId : CharacteristicSelectGenerator.makeRandom('characteristiSelectId'),
                text : CharacteristicSelectGenerator.makeRandom('characteristiSelectText')
            });

            return angular.extend(characteristicSelect, specificData || {});
        }

        public static generateMany(n: number): Business.CharacteristicGroup[] {
            return _.map<Dto.ICharacteristicGroupUsage, Business.CharacteristicGroup>(CharacteristicGroupUsageGenerator.generateManyDtos(n), (characteristicGroupUsage: Dto.ICharacteristicGroupUsage) => { return new Business.CharacteristicGroup(characteristicGroupUsage); });
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000);
        }
    }
}