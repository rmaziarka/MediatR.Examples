/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class PropertySearchResultContactGenerator {
        public static generateDto(): Dto.IPropertySearchResultContact {

            var contact: Dto.IPropertySearchResultContact = {
                id: PropertySearchResultContactGenerator.makeRandom('id'),
                firstName: PropertySearchResultContactGenerator.makeRandom('firstName'),
                surname: PropertySearchResultContactGenerator.makeRandom('surname'),
                title: PropertySearchResultContactGenerator.makeRandom('title')
            }

            return contact;
        }

        public static generateManyDtos(n: number): Dto.IPropertySearchResultContact[] {
            return _.times(n, PropertySearchResultContactGenerator.generateDto);
        }

        public static generateMany(n: number): Business.PropertySearchResultContact[] {
            return _.map(PropertySearchResultContactGenerator.generateManyDtos(n), (contact: Dto.IPropertySearchResultContact) => { return new Business.PropertySearchResultContact(contact); });
        }

        public static generate(): Business.PropertySearchResultContact {
            return new Business.PropertySearchResultContact(ContactGenerator.generateDto());
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}