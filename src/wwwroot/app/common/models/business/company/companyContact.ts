/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CompanyContact implements Dto.ICompanyContact {
        id: string;
        contactId: string = '';
        contact: Contact = new Contact();
        companyId: string = '';
        company: Company = new Company();
        
        constructor(contactCompany?: Dto.ICompanyContact, contact?: Dto.IContact, company?: Dto.ICompany) {
            if (contactCompany) {
                angular.extend(this, contactCompany);
                if (contactCompany.company) this.company = new Company(contactCompany.company);
                if (contactCompany.contact) this.contact = new Contact(contactCompany.contact);
            }
            else {
                this.company = new Company(company);
                this.companyId = this.company.id;
                this.contact = new Contact(contact);
                this.contactId = this.contact.id;
            }
        }
    }
}