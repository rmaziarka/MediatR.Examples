module Antares.Common.Models.Business {

    export class PreviewProperty implements Dto.IPreviewProperty {
        id: string = null;
        address: Business.Address = new Business.Address();
        divisionId: string = null;

        constructor(property?: Dto.IPreviewProperty)
        {
            if (property) {
                this.id = property.id;
                this.address = new Business.Address(property.address);

                this.divisionId = property.divisionId;
            }
        }
    }
}