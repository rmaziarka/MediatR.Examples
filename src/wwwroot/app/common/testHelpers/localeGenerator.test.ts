/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class LocaleGenerator {
        public static generateDto(isoCode? : any): Dto.ILocale {

            var locale: Dto.ILocale = {

                id: LocaleGenerator.makeRandom('id'),
                isoCode: isoCode || LocaleGenerator.makeRandom('isoCode')
            }

            return locale;
        }

        public static generateManyDtos(n: number): Dto.ILocale[] {
            return _.times(n, LocaleGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Locale[] {
            return _.map(LocaleGenerator.generateManyDtos(n), (locale: Dto.ILocale) => { return new Business.Locale(locale); });
        }

        public static generate(isoCode? : string): Business.Locale {
            return new Business.Locale(LocaleGenerator.generateDto(isoCode));
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}