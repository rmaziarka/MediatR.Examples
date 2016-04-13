/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Contact {

        id: string;
        firstName: string;
        surname: string;
        title: string;

        constructor(contact?: Dto.IContact) {
            angular.extend(this, contact);
        }

        public getName() {
            return this.firstName + ' ' + this.surname;
        }
    }
}