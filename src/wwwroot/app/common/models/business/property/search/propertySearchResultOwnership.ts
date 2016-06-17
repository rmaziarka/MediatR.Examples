module Antares.Common.Models.Business {
    export class PropertySearchResultOwnership {
        id: string = null;
        sellDate: Date = null;
        ownershipTypeId: string = null;
        contacts: Business.PropertySearchResultContact[] = [];

        isCurrentOwner: boolean;

        constructor(propertySearchResultOwnership?: Dto.IPropertySearchResultOwnership) {
            if (propertySearchResultOwnership) {
                this.id = propertySearchResultOwnership.id;
                this.ownershipTypeId = propertySearchResultOwnership.ownershipTypeId;
                this.contacts = propertySearchResultOwnership.contacts.map((c: Dto.IPropertySearchResultContact) => { return new PropertySearchResultContact(c) });

                this.isCurrentOwner = !this.sellDate;
            }
        }
    }
}
