/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class AddressFormFieldDefinition implements Dto.IAddressFormFieldDefinition {
        constructor(
            public name: string,
            public labelKey: string,
            public required: boolean,
            public regEx: string,
            public rowOrder: number,
            public columnOrder: number,
            public columnSize: number
        ) { }
    }
}