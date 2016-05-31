/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Company implements Dto.ICompany {
        id: string = '';
        name: string = '';    
        websiteUrl: string = null;
        clientCarePageUrl: string = null;
        clientCareStatusId: string = null;
        clientCareStatus: Business.EnumTypeItem;
        contacts: Contact[] = [];

        constructor(company?: Dto.ICompany) {

            if (company) {
                angular.extend(this, company);
                this.websiteUrl = company.websiteUrl;
                this.clientCarePageUrl = company.clientCarePageUrl;
                this.clientCareStatusId = company.clientCareStatusId;
                this.contacts = company.contacts.map((contact: Dto.IContact) => { return new Contact(contact) });
            }
        }
    }
}