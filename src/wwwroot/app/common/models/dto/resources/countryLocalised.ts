module Antares.Common.Models.Dto {
    export class Country {
        public id: string;
        public isoCode: string;
    }

    export interface ICountryLocalised {
        country: Country;
        locale: Dto.Locale;
        value: string;
    }

    export class CountryLocalised implements ICountryLocalised{
        public country: Country;
        public locale: Dto.Locale;
        public value: string;
    }
}