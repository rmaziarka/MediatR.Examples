/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CompanyContact implements Dto.ICompanyContact {
        id: string;
        contactId: string = '';
        contact: Contact = new Contact();
        companyId: string = '';
        company: Company = new Company();

        constructor(contactCompany: Dto.ICompanyContact) {
            angular.extend(this, contactCompany);
            if (contactCompany.company)
                this.company = new Company(contactCompany.company);
            if (contactCompany.contact)
                this.contact = new Contact(contactCompany.contact);
        }
    }
}