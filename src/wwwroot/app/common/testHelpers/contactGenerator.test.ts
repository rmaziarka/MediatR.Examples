/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Antares.Common.Models.Dto;

    export class ContactGenerator {
        public static Generate(): Dto.IContact {

            var contact: Dto.IContact = {
                firstName: ContactGenerator.makeRandom('firstName'),
                surname: ContactGenerator.makeRandom('surname'),
                id: ContactGenerator.makeRandom('id'),
                title: ContactGenerator.makeRandom('title')
            }

            return contact;
        }

        public static GenerateMany(n: number): Dto.IContact[] {
            return _.times(n, ContactGenerator.Generate);
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000);
        }
    }
}