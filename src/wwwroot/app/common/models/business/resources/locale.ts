module Antares.Common.Models.Business {
    export class Locale implements Dto.ILocale {

        id: string;
        isoCode: string;

        constructor(locale?: Dto.ILocale) {
            angular.extend(this, locale);
        }
    }
}