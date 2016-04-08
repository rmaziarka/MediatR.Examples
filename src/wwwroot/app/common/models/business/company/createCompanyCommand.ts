/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateCompanyCommand implements Dto.ICreateCompanyCommand {        
        name: string = '';        
        contactIds: string[] = [];

        constructor(company?: Dto.ICompany) {
            if (company) {
                this.name = company.name;
                this.contactIds = company.contacts.map((contact: Dto.IContact) => { return contact.id });
            }
        }
    }
}