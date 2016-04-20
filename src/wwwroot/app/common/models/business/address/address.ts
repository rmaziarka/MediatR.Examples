﻿module Antares.Common.Models.Business {
    export class Address implements Dto.IAddress {
        [key: string]: any;

        id: string = '';
        countryId: string = '';
        addressFormId: string = '';
        propertyName: string = '';
        propertyNumber: string = '';
        line1: string = '';
        line2: string = '';
        line3: string = '';
        postcode: string = '';
        city: string = '';
        county: string = '';

        constructor(address?: Dto.IAddress) {
            if (address) {
                angular.extend(this, address);
            }
        }

        public clear = (): void => {
            this.countryId = '';
            this.addressFormId = '';
            this.propertyName = '';
            this.propertyNumber = '';
            this.line1 = '';
            this.line2 = '';
            this.line3 = '';
            this.postcode = '';
            this.city = '';
            this.county = '';
        }
    }
}