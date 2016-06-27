/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    export class EnumDictionaryGenerator {
        public static generateDictionary(): Dto.IEnumDictionary {
            var enumDictionary: Dto.IEnumDictionary = {
                activityDepartmentType: EnumDictionaryGenerator.generateEnums(),
                activityDocumentType: EnumDictionaryGenerator.generateEnums(),
                activityStatus: EnumDictionaryGenerator.generateEnums(),
                activityUserType: EnumDictionaryGenerator.generateEnums(),
                division: EnumDictionaryGenerator.generateEnums(),
                entityType: EnumDictionaryGenerator.generateEnums(),
                offerStatus: EnumDictionaryGenerator.generateEnums(),
                ownershipType: EnumDictionaryGenerator.generateEnums()
            }
            return enumDictionary;
        }

        public static generateEnums(): Dto.IEnumItem[]{
            var enums: Dto.IEnumItem[] = [];
            for (var i = 0; i < 4; i++) {
                enums.push({
                    id: EnumDictionaryGenerator.makeRandom('id'),
                    code: EnumDictionaryGenerator.makeRandom('code')
                });
            }
            return enums;
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}