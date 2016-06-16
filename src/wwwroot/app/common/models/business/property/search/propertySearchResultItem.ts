module Antares.Common.Models.Business {
    export class PropertySearchResultItem {
        id: string = null;
        addressId: string = null;
        address: Business.PropertySearchResultAddress = new Business.PropertySearchResultAddress();
        ownerships: Business.PropertySearchResultOwnership[] = [];

        constructor(propertySearchResultItem?: Dto.IPropertySearchResultItem) {
            if (propertySearchResultItem) {
                this.id = propertySearchResultItem.id;
                this.addressId = propertySearchResultItem.addressId;
                this.address = new Business.PropertySearchResultAddress(propertySearchResultItem.address);
                this.ownerships = propertySearchResultItem.ownerships.map((o: Dto.IPropertySearchResultOwnership) => { return new PropertySearchResultOwnership(o) });
            }
        }
    }
}