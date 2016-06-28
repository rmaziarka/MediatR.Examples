/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import TypeItem = Antares.Common.Models.Dto.IEnumTypeItem;

    export class CurrentUser implements Dto.ICurrentUser {
        id: string;
        firstName: string;
        lastName: string;
        email: string;
        country: Country;
        division: EnumTypeItem;
        roles: string[];
        salutationFormatId: string;
        locale: Locale;

        constructor(user?: Dto.ICurrentUser){
            if (user) {
                angular.extend(this, user);
            }

        }

        public getName() {
            return this.firstName + ' ' + this.lastName;
        }
    }
}