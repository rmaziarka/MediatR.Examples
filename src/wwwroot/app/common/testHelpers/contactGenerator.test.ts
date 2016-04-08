/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ContactGenerator {
        public static generateDto(): Dto.IContact {

            var contact: Dto.IContact = {
                firstName: ContactGenerator.makeRandom('firstName'),
                surname: ContactGenerator.makeRandom('surname'),
                id: ContactGenerator.makeRandom('id'),
                title: ContactGenerator.makeRandom('title')
            }

            return contact;
        }

        public static generateManyDtos(n: number): Dto.IContact[] {
            return _.times(n, ContactGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Contact[] {
            return _.map(ContactGenerator.generateManyDtos(n), (activity: Dto.IContact) => { return new Business.Contact(activity); });
        }

        public static generate(): Business.Contact {
            return new Business.Contact(ContactGenerator.generateDto());
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000);
        }
    }
}