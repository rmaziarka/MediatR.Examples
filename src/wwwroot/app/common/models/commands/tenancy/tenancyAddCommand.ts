/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Commands.Tenancy {
    import Business = Antares.Common.Models.Business;

    export class TenancyAddCommand implements ITenancyAddCommand {
        activityId: string;
        requirementId: string;
        term: TenancyTermCommandPart;
        landlordContacts: string[];
        tenantContacts: string[];

        constructor(tenancy?: Business.TenancyEditModel) {
            if (tenancy) {
                this.activityId = tenancy.activity.id;
                this.requirementId = tenancy.requirement.id;
                this.term = new TenancyTermCommandPart(tenancy);
                this.landlordContacts = this.getContactIds(tenancy.landlords);
                this.tenantContacts = this.getContactIds(tenancy.tenants);
            }
        }

        private getContactIds = (contacts: Business.Contact[]): string[] => {
            return contacts ? contacts.map((contact: Business.Contact) => { return contact.id; }) : [];
        }
    }

    export interface ITenancyAddCommand {
        activityId: string;
        requirementId: string;
    }
}