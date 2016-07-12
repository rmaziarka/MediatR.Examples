/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ContactTitleGenerator {
        public static generateDto(title?: any, isoCode?: any): Dto.IContactTitle {

            var contactTitle: Dto.IContactTitle = {

                id: ContactTitleGenerator.makeRandom('id'),
                title: title || ContactTitleGenerator.makeRandom('title'),
                locale: LocaleGenerator.generate(isoCode),
                priority: ContactTitleGenerator.makeRandomNumber(10)
            }

            return contactTitle;
        }

        public static generateManyDtos(n: number): Dto.IContactTitle[] {
            return _.times(n, ContactTitleGenerator.generateDto);
        }

        public static generateMany(n: number): Business.ContactTitle[] {
            return _.map(ContactTitleGenerator.generateManyDtos(n), (contactTitle: Dto.IContactTitle) => { return new Business.ContactTitle(contactTitle); });
        }

        public static generate(title?: any, isoCode?: any): Business.ContactTitle {
            return new Business.ContactTitle(ContactTitleGenerator.generateDto(title, isoCode));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }

        private static makeRandomNumber(max: number): number {
            return _.random(1, max);
        }
    }
}