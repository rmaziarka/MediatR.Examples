﻿module Antares.Common.Models.Dto {
    export class AddressFormFieldDefinition {
        constructor(
            public name: string,
            public labelKey: string,
            public required: boolean,
            public regEx: string,
            public rowOrder: number,
            public columnOrder: number,
            public columnSize: number
        ){}
    }

    export interface IAddressForm {
        id: string;
        countryId: string;
        addressFieldDefinitions: AddressFormFieldDefinition[];
    }

    export class AddressForm implements IAddressForm{
        public addressFieldRows: AddressFormFieldDefinition[][];

        constructor(
            public id: string = "",
            public countryId: string = "",
            public addressFieldDefinitions: AddressFormFieldDefinition[] = []
        ){
            this.updateAddressFieldRows();
        }
        
        updateAddressFieldRows(){
            this.addressFieldRows = _.toArray<AddressFormFieldDefinition[]>(_.groupBy<AddressFormFieldDefinition>(this.addressFieldDefinitions, 'rowOrder'));
        }
    }
}