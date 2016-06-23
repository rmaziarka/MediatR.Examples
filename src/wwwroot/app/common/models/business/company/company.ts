/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Company implements Dto.ICompany {
        id: string = '';
        name: string = '';    
        websiteUrl: string = '';
        clientCarePageUrl: string = '';
        clientCareStatusId: string = '';
        clientCareStatus: Business.EnumTypeItem;
        contacts: Contact[] = [];

        constructor(company?: Dto.ICompany) {

            if (company) {
                angular.extend(this, company);

                if (company.contacts) {
                    this.contacts = company.contacts.map((contact: Dto.IContact) =>{ return new Contact(contact) });
                }
            }
        }
    }
}