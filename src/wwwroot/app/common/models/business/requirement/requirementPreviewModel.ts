/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import Dto = Models.Dto;

    export class RequirementPreviewModel {
        contacts: Contact[];
        id: string;

        constructor(requirement: Dto.IRequirement ) {
            this.contacts = requirement.contacts.map((user: Dto.IContact) => {
                return new Contact(user);
            });
            
            this.id = requirement.id;
        }

        public getContactNames() {
            return this.contacts.map((c: Contact) => { return c.getName() }).join(", ");
        }
    }
}