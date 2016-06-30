/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    export class ContactGenerator {
        public static generateDto(): Dto.IContact {

            var contact: Dto.IContact = {
                firstName: ContactGenerator.makeRandom('firstName'),
                lastName: ContactGenerator.makeRandom('lastName'),
                id: ContactGenerator.makeRandom('id'),
                title: ContactGenerator.makeRandom('title'),
                mailingFormalSalutation: ContactGenerator.makeRandom('mailingFormalSalutation'),
                mailingSemiformalSalutation: ContactGenerator.makeRandom('mailingSemiformalSalutation'),
                mailingInformalSalutation: ContactGenerator.makeRandom('mailingInformalSalutation'),
                mailingPersonalSalutation: ContactGenerator.makeRandom('mailingPersonalSalutation'),
                mailingEnvelopeSalutation: ContactGenerator.makeRandom('mailingEnvelopeSalutation'),
                defaultMailingSalutationId: ContactGenerator.makeRandom('defaultMailingSalutationId'),
                eventInviteSalutation: ContactGenerator.makeRandom('eventInviteSalutation'),
                eventSemiformalSalutation: ContactGenerator.makeRandom('eventSemiformalSalutation'),
                eventInformalSalutation: ContactGenerator.makeRandom('eventInformalSalutation'),
                eventPersonalSalutation: ContactGenerator.makeRandom('eventPersonalSalutation'),
                eventEnvelopeSalutation: ContactGenerator.makeRandom('eventEnvelopeSalutation'),
                defaultEventSalutationId: ContactGenerator.makeRandom('defaultEventSalutationId'),
                leadNegotiator: ContactUserGenerator.generateDto(Enums.NegotiatorTypeEnum.LeadNegotiator),
                secondaryNegotiators:[]
            }

            return contact;
        }

        public static generateManyDtos(n: number): Dto.IContact[] {
            return _.times(n, ContactGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Contact[] {
            return _.map(ContactGenerator.generateManyDtos(n), (contact: Dto.IContact) => { return new Business.Contact(contact); });
        }

        public static generate(): Business.Contact {
            return new Business.Contact(ContactGenerator.generateDto());
        }

     private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}