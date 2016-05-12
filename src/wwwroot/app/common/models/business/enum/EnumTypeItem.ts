module Antares.Common.Models.Business {

    export class EnumTypeItem implements Dto.IEnumTypeItem {
        id: string = null;
        enumTypeId: string = null;
        code: string = null;

        constructor(enumTypeItem?: Dto.IEnumTypeItem)
        {
            if (enumTypeItem) {
                angular.extend(this, enumTypeItem);
            }
        }
    }
}