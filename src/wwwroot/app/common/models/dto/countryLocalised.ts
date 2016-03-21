module Antares.Common.Models.Dto {
    export class Country {
        id: string;
        isoCode: string;
    }

    export class CountryLocalised {
        country: Country;
        locale: Dto.Locale;
        value: string;
    }
}