declare module Antares.Common.Models.Dto {
    interface IAddressFormFieldDefinition {
        name: string;
        labelKey: string;
        required: boolean;
        regEx: string;
        rowOrder: number;
        columnOrder: number;
        columnSize: number;
    }
}