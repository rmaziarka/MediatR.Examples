declare module Antares.Common.Models.Dto {
    interface IAddressForm {
        id: string;
        countryId: string;
        addressFieldDefinitions: IAddressFormFieldDefinition[];
    }
}