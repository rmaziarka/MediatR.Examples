module Antares.Common.Models.Business {
    export class CountryLocalised implements Dto.ICountryLocalised{
        public country: Country;
        public locale: Locale;
        public value: string;
    }
}