/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateCompanyResource implements Dto.ICreateCompanyResource {        
        name: string = '';    
        websiteUrl: string = '';
        clientCarePageUrl:  string = '';    
        clientCareStatusId:string = '';
        contactIds: string[] = [];

        constructor(company?: Dto.ICompany) {
            if (company) {
                this.name = company.name;
                this.websiteUrl = company.websiteUrl;
                this.clientCarePageUrl = company.clientCarePageUrl;
                this.clientCareStatusId = company.clientCareStatusId;
                this.contactIds = company.contacts.map((contact: Dto.IContact) => { return contact.id });
            }
        }

       
    }
}