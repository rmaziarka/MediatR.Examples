/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class CompanyGenerator {
        public static generateDto(): Dto.ICompany {

            var company: Dto.ICompany = {
                id: CompanyGenerator.makeRandom('id'),
                name: CompanyGenerator.makeRandom('name'),
                websiteUrl: CompanyGenerator.makeRandom('website'),
                clientCarePageUrl: CompanyGenerator.makeRandom('clientCarePageUrl'),
                clientCareStatusId: CompanyGenerator.makeRandom('clientCareStatusId'),
                clientCareStatus: <Dto.IEnumTypeItem>{},
                companiesContacts: [],
                contacts: ContactGenerator.generateMany(3)
            }

            return company;
        }

        public static generateManyDtos(n: number): Dto.ICompany[] {
            return _.times(n, CompanyGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Company[] {
            return _.map<Dto.ICompany, Business.Company>(CompanyGenerator.generateManyDtos(n), (company: Dto.ICompany) => new Business.Company(company));
        }

        public static generate(): Business.Company {
            return new Business.Company(CompanyGenerator.generateDto());
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}