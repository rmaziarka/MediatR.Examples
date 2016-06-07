/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    export class EnumTypeItemGenerator {
        public static generateDto(): Dto.IEnumTypeItem {
            var enumTypeItem: Dto.IEnumTypeItem = {
                id: EnumTypeItemGenerator.makeRandom('enumTypeItemId'),
                code: EnumTypeItemGenerator.makeRandom('code'),
                enumTypeId: EnumTypeItemGenerator.makeRandom('enumTypeId')
            }
            return enumTypeItem;
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}