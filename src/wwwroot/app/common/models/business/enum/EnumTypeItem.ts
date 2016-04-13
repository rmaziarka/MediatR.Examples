module Antares.Common.Models.Business {

    export class EnumTypeItem implements Dto.IEnumTypeItem {
        id: string = null;
        enumTypeId: string = null;
        code: string = null;

        constructor(property?: Dto.IEnumTypeItem)
        {
            if (property) {
                this.id = property.id;
                this.enumTypeId = property.enumTypeId;
                this.code = property.code;
            }
        }
    }
}