/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    export class EnumTypeItemGenerator {
        public static generateDto(code?: string): Dto.IEnumTypeItem {
            code = code || StringGenerator.generate();

            var enumTypeItem: Dto.IEnumTypeItem = {
                id: StringGenerator.generate(),
                code: code,
                enumTypeId: StringGenerator.generate()
            }
            return enumTypeItem;
        }
    }
}