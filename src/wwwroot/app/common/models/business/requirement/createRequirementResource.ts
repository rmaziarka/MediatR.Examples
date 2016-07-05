/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {

    export class CreateRequirementResource implements Dto.ICreateRequirementResource {
        requirementTypeId: string;
        contactIds: string[] = [];
        address: Dto.IAddress = null;

        rentMin: number = null;
        rentMax: number = null;

        description: string = null;

        constructor(requirement?: Dto.IRequirement) {
            if (requirement) {
                this.requirementTypeId = requirement.requirementTypeId;
                this.contactIds = requirement.contacts.map((contact: Dto.IContact) => contact.id);
                this.address = requirement.address;

                this.rentMin = requirement.rentMin;
                this.rentMax = requirement.rentMax;

                this.description = requirement.description;
            }
        }
    }
}