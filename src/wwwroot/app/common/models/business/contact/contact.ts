/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Contact {

        id: string;
        title: string;
        firstName: string;
        lastName: string;
        mailingFormalSalutation: string;
        mailingSemiformalSalutation: string;
        mailingInformalSalutation: string;
        mailingPersonalSalutation: string;
        mailingEnvelopeSalutation: string;
        defaultMailingSalutationId: string;
        eventInviteSalutation: string;
        eventSemiformalSalutation: string;
        eventInformalSalutation: string;
        eventPersonalSalutation: string;
        eventEnvelopeSalutation: string;
        defaultEventSalutationId: string;
        company: Business.Company = null;
        leadNegotiator: ContactUser;
        secondaryNegotiators: ContactUser[];

        constructor(contact?: Dto.IContact, company?: Dto.ICompany) {
            angular.extend(this, contact);
			if (company) {
				this.company = new Business.Company(company);
			}
        }

        public getName() {
            return this.firstName + ' ' + this.lastName;
        }
    }
}