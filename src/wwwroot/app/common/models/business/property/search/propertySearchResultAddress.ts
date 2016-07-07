module Antares.Common.Models.Business {
    export class PropertySearchResultAddress {
        id: string = null;
        countryId: string = null;
        addressFormId: string = null;
        propertyNumber: string = null;
        propertyName: string = null;
        line1: string = null;
        line2: string = null;
        line3: string = null;
        postcode: string = null;
        city: string = null;
        county: string = null;

        constructor(propertySearchResultAddress?: Dto.IPropertySearchResultAddress) {
            if (propertySearchResultAddress) {
                this.id = propertySearchResultAddress.id;
                this.countryId = propertySearchResultAddress.countryId;
                this.addressFormId = propertySearchResultAddress.addressFormId;
                this.propertyNumber = propertySearchResultAddress.propertyNumber;
                this.propertyName = propertySearchResultAddress.propertyName;
                this.line1 = propertySearchResultAddress.line1;
                this.line2 = propertySearchResultAddress.line2;
                this.line3 = propertySearchResultAddress.line3;
                this.postcode = propertySearchResultAddress.postcode;
                this.city = propertySearchResultAddress.city;
                this.county = propertySearchResultAddress.county;
            }
        }
    }
}