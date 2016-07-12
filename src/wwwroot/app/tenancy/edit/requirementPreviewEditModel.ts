/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy  {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    export class RequirementPreviewEditModel {
        contacts: Business.Contact[];
        id: string;

        constructor(requirement: Dto.IRequirement ) {
            this.contacts = requirement.contacts.map((user: Dto.IContact) => {
                return new Business.Contact(user);
            });
            
            this.id = requirement.id;
        }

        public getContactNames() {
            return this.contacts.map((c: Business.Contact) => { return c.getName() }).join(", ");
        }
    }
}