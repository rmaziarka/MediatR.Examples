/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CharacteristicGroupItemGenerator {
        public static generateDto(specificData?: any): Dto.ICharacteristicGroupItem {

            var characteristicGroupUsage: Dto.ICharacteristicGroupItem = {
                id: CharacteristicGroupItemGenerator.makeRandom('characteristicGroupItemId'),
                characteristic: <Dto.ICharacteristic>{
                    id: CharacteristicGroupItemGenerator.makeRandom('characteristicId'),
                    code: CharacteristicGroupItemGenerator.makeRandom('code'),
                    isDisplayText: true,
                    isEnabled: true
                }
            }

            return angular.extend(characteristicGroupUsage, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.ICharacteristicGroupItem[] {
            return _.times(n, CharacteristicGroupItemGenerator.generateDto);
        }

        public static generate(specificData?: any): Business.Characteristic {
            return new Business.Characteristic(CharacteristicGroupItemGenerator.generateDto(specificData));
        }

        public static generateMany(n: number): Business.Characteristic[] {
            return _.map<Dto.ICharacteristicGroupItem, Business.Characteristic>(CharacteristicGroupItemGenerator.generateManyDtos(n), (characteristicGroupItem: Dto.ICharacteristicGroupItem) => { return new Business.Characteristic(characteristicGroupItem); });
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}