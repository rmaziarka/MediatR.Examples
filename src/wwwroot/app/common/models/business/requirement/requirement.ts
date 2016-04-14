/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Requirement implements Dto.IRequirement {
        [index: string]: any;

        id: string = '';
        createDate: Date = null;
        contacts: Contact[] = [];
        address: Address = new Address();
        description: string;

        constructor(requirement?: Dto.IRequirement) {
            if (requirement) {
                angular.extend(this, requirement);

                this.createDate = Core.DateTimeUtils.convertDateToUtc(requirement.createDate);
                this.contacts = requirement.contacts.map((contact: Dto.IContact) => { return new Contact(contact) });
                this.address = new Address(requirement.address);
            }
        }
    }
}