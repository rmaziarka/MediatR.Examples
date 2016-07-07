/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Company implements Dto.ICompany {
        id: string = null;
        name: string = null;    
        websiteUrl: string = null;
        clientCarePageUrl: string = null;
        clientCareStatusId: string = null;
        clientCareStatus: Business.EnumTypeItem;
        contacts: Contact[] = [];
        companiesContacts: Models.Dto.ICompanyContact[] = [];

        constructor(company?: Dto.ICompany) {

            if (company) {
                angular.extend(this, company);

                //TODO: check if can be removed
                if (company.contacts && !company.companiesContacts) {
                    this.contacts = company.contacts.map((contact: Dto.IContact) =>{ return new Contact(contact) });
                }

                if (company.companiesContacts && !company.contacts) {
                    this.contacts = company.companiesContacts.map((item) =>{ return new Contact(item.contact) });
                }
            }
        }
    }
}