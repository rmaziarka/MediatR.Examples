module Antares.Common.Models.Dto {
    export class Address {
        constructor(
            public id: string,
            public countryId: string,
            public addressFormId: string,
            public propertyName: string,
            public propertyNumber: string,
            public line1: string,
            public line2: string,
            public line3: string,
            public postcode: string,
            public city: string,
            public county: string
        ){}

        public clear = (): void =>{
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