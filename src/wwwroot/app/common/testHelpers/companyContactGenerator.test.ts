/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class CompanyContactGenerator {
        public static generateDto(): Dto.ICompanyContact {

            var companyContact = <Dto.ICompanyContact> {
                id: CompanyContactGenerator.makeRandom('id'),
                contactId: CompanyContactGenerator.makeRandom('contact_id'),
                contact: ContactGenerator.generate(),
                companyId: CompanyContactGenerator.makeRandom('company_id'),
                company: CompanyGenerator.generate()
            }

            return companyContact;
        }

        public static generateManyDtos(n: number): Dto.ICompanyContact[] {
            return _.times(n, CompanyContactGenerator.generateDto);
        }

        public static generateMany(n: number): Business.CompanyContact[] {
            return _.map(CompanyContactGenerator.generateManyDtos(n), (companyContact: Dto.ICompanyContact) => { return new Business.CompanyContact(companyContact); });
        }

        public static generate(): Business.CompanyContact {
            return new Business.CompanyContact(CompanyContactGenerator.generateDto());
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}