/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Company implements Dto.ICompany {
        id: string = '';
        name: string = '';        
        contacts: Contact[] = [];

        constructor(company?: Dto.ICompany) {
            if (company) {
                angular.extend(this, company);
                
                this.contacts = company.contacts.map((contact: Dto.IContact) => { return new Contact(contact) });
            }
        }
    }
}