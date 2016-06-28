declare module Antares.Common.Models.Dto {
    interface ICurrentUser {
        id: string;
        firstName: string;
        lastName: string;
        email: string;
        country: ICountry;
        division: IEnumTypeItem;
        roles: string[];
        salutationFormatId: string;
        locale: ILocale;
    }
}