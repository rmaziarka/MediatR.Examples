/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    export class ContactGenerator {
        public static GenerateDto(): Dto.IContact {

            var contact: Dto.IContact = {
                firstName: ContactGenerator.makeRandom('firstName'),
                surname: ContactGenerator.makeRandom('surname'),
                id: ContactGenerator.makeRandom('id'),
                title: ContactGenerator.makeRandom('title')
            }

            return contact;
        }

        public static GenerateManyDtos(n: number): Dto.IContact[] {
            return _.times(n, ContactGenerator.GenerateDto);
        }

        public static GenerateMany(n: number): Business.Contact[] {
            return _.map(ContactGenerator.GenerateManyDtos(n), (activity: Dto.IContact) => { return new Business.Contact(activity); });
        }

        public static Generate(): Business.Contact {
            return new Business.Contact(ContactGenerator.GenerateDto());
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000);
        }
    }
}