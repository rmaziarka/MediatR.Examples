declare module Antares.Common.Models.Dto {
    interface ICountryLocalised {
        country: ICountry;
        locale: ILocale;
        value: string;
    }
}