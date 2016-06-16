module Antares.Common.Models.Business {
    export class PropertySearchResultOwnership {
        id: string = null;
        ownershipTypeId: string = null;
        contacts: Business.PropertySearchResultContact[] = [];

        constructor(propertySearchResultOwnership?: Dto.IPropertySearchResultOwnership) {
            if (propertySearchResultOwnership) {
                this.id = propertySearchResultOwnership.id;
                this.ownershipTypeId = propertySearchResultOwnership.ownershipTypeId;
                this.contacts = propertySearchResultOwnership.contacts.map((c: Dto.IPropertySearchResultContact) => { return new PropertySearchResultContact(c) });
            }
        }
    }
}
