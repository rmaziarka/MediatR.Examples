/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class EditCompanyResource implements Dto.IEditCompanyResource { 
        id: string = '';       
        name: string = '';    
        websiteUrl: string = '';
        clientCarePageUrl:  string = '';    
        clientCareStatusId:string = '';
        contactIds: string[] = [];

        constructor(company?: Dto.ICompany) {
            if (company) {
                this.id = company.id;
                this.name = company.name;
                this.websiteUrl = company.websiteUrl;
                this.clientCarePageUrl = company.clientCarePageUrl;
                this.clientCareStatusId = company.clientCareStatusId;
                this.contactIds = company.contacts.map((contact: Dto.IContact) => { return contact.id });
            }
        }

       
    }
}