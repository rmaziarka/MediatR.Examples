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

        constructor(contact?: Dto.IContact) {
            angular.extend(this, contact);
        }

        public getName() {
            return this.firstName + ' ' + this.lastName;
        }
    }
}