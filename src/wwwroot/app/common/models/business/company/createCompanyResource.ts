/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateCompanyResource implements Dto.ICreateCompanyResource {
        name: string = '';    
        websiteUrl: string = '';
        clientCarePageUrl:  string = '';    
        clientCareStatusId: string = '';
        description: string = '';
        companyCategoryId: string = '';
        companyTypeId: string = '';            
        valid: boolean = false;
        relationshipManageruserId: string = null;
        contactIds: string[] = [];

        constructor(company?: Dto.ICompany) {
            if (company) {
                this.name = company.name;
                this.websiteUrl = company.websiteUrl;
                this.clientCarePageUrl = company.clientCarePageUrl;
                this.clientCareStatusId = company.clientCareStatusId;
                this.companyCategoryId = company.companyCategoryId;
                this.companyTypeId = company.companyTypeId;
                this.description = company.description;
                this.valid = company.valid;
                this.relationshipManageruserId = (company.relationshipManager ? company.relationshipManager.id : null);
                this.contactIds = company.contacts.map((contact: Dto.IContact) => { return contact.id });
            }
        }

       
    }
}