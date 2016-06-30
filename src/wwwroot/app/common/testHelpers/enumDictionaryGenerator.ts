/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    export class EnumDictionaryGenerator {
        public static generateDictionary(): Dto.IEnumDictionary {
            var enumDictionary: Dto.IEnumDictionary = {
                activityDepartmentType: EnumDictionaryGenerator.generateEnums(Enums.ActivityDepartmentType),
                activityDocumentType: EnumDictionaryGenerator.generateEnums(Enums.ActivityDocumentType),
                activityStatus: EnumDictionaryGenerator.generateEnums(Enums.ActivityStatus),
                activityUserType: EnumDictionaryGenerator.generateEnums(Enums.ActivityUserType),
                division: EnumDictionaryGenerator.generateEnums(Enums.Division),
                entityType: EnumDictionaryGenerator.generateEnums(Enums.EntityType),
                offerStatus: EnumDictionaryGenerator.generateEnums(Enums.OfferStatus),
                ownershipType: EnumDictionaryGenerator.generateEnums(Enums.OwnershipType)
            }
            return enumDictionary;
        }

        public static generateEnums(enumType: any): Dto.IEnumItem[]{
            var enums: Dto.IEnumItem[] = [];
            for (var enumMember in enumType) {
                if (enumType.hasOwnProperty(enumMember)) {
                    enums.push({
                        id : EnumDictionaryGenerator.makeRandom('id'),
                        code : enumType[enumMember]
                    });
                }
            }
            return enums;
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}