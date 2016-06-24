/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ContactWithSelection {

        id: string;
        firstName: string;
        lastName: string;
        title: string;
        selected: boolean = false;

        constructor(contact?: Dto.IContact) {
            angular.extend(this, contact);
        }

        public getName() {
            return this.firstName + ' ' + this.lastName;
        }
    }
}