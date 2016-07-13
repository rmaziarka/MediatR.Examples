/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class EditCompanyResource implements Dto.IEditCompanyResource { 
        id: string = '';       
        name: string = '';    
        websiteUrl: string = '';
        clientCarePageUrl: string = '';
        clientCareStatusId: string = '';
        contacts: { id: string; }[] = [];
        description: string = '';
        companyCategoryId: string = '';
        companyTypeId: string = '';
        valid: boolean = false;
        relationshipManageruserId: string = null;

        constructor(company?: Dto.ICompany) {
            if (company) {
                this.id = company.id;
                this.name = company.name;
                this.websiteUrl = company.websiteUrl;
                this.clientCarePageUrl = company.clientCarePageUrl;
                this.clientCareStatusId = company.clientCareStatusId;
                this.companyCategoryId = company.companyCategoryId;
                this.companyTypeId = company.companyTypeId;
                this.description = company.description;
                this.valid = company.valid;
                this.relationshipManageruserId = (company.relationshipManager ? company.relationshipManager.id : null);
                this.contacts = company.contacts.map((contact: Dto.IContact) =>{ return { id : contact.id }; });
            }
        }

       
    }
}