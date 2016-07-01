declare module Antares.Common.Models.Dto {
    interface IContact {
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
		contactUsers: IContactUser[];
 
    }
}