module Antares.Common.Models.Business {

    export class PreviewProperty implements Dto.IPreviewProperty {
        id: string = null;
        address: Business.Address = new Business.Address();
        divisionId: string = null;
        propertyTypeId: string;
        ownerships: Business.Ownership[] = [];

        constructor(property?: Dto.IPreviewProperty)
        {
            if (property) {
                this.id = property.id;
                this.address = new Business.Address(property.address);
                this.propertyTypeId = property.propertyTypeId;
                this.divisionId = property.divisionId;
                this.ownerships = property.ownerships.map((ownership: Dto.IOwnership) => { return new Business.Ownership(ownership) });
            }
        }
    }
}