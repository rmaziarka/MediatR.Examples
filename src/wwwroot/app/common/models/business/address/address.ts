module Antares.Common.Models.Business {
    export class Address implements Dto.IAddress {
        [key: string]: any;

        id: string = null;
        countryId: string = null;
        addressFormId: string = null;
        propertyName: string = null;
        propertyNumber: string = null;
        line1: string = null;
        line2: string = null;
        line3: string = null;
        postcode: string = null;
        city: string = null;
        county: string = null;

        constructor(address?: Dto.IAddress){
            if (address) {
                angular.extend(this, address);
            }
        }

        public clear = (): void =>{
            this.countryId = null;
            this.addressFormId = null;
            this.propertyName = null;
            this.propertyNumber = null;
            this.line1 = null;
            this.line2 = null;
            this.line3 = null;
            this.postcode = null;
            this.city = null;
            this.county = null;
        }

        public getAddressText = (): string =>{
            var addressSpaceSeparatedElements: string[] = [];
            var addressCommaSeparatedElements: string[] = [];

            if (this.propertyNumber) {
                addressSpaceSeparatedElements.push(this.propertyNumber);
            }
            if (this.line2) {
                addressSpaceSeparatedElements.push(this.line2);
            }
            if (this.propertyName) {
                addressCommaSeparatedElements.push(this.propertyName);
            }
            if (addressSpaceSeparatedElements.length > 0) {
                addressCommaSeparatedElements.push(addressSpaceSeparatedElements.join(" "));
            }

            return addressCommaSeparatedElements.join(", ");
        }
    }
}