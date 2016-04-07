declare module Antares.Common.Models.Dto {
    interface IAddress {
        id?: string;
        countryId?: string;
        addressFormId?: string;
        propertyName?: string;
        propertyNumber?: string;
        line1?: string;
        line2?: string;
        line3?: string;
        postcode?: string;
        city?: string;
        county?: string;
    }
}