/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ContactTitle implements Dto.IContactTitle {

        id: string;
        title: string;
        locale: Locale;
        priority: number;
       
        constructor(contactTitle?: Dto.IContactTitle) {
            angular.extend(this, contactTitle);
        }
    }
}