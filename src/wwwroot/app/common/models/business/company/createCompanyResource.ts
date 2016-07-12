/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateCompanyResource implements Dto.ICreateCompanyResource {
        name: string = '';    
        websiteUrl: string = '';
        clientCarePageUrl:  string = '';    
        clientCareStatusId: string = '';
        description: string = '';
        categoryId: string = '';
        typeId: string = '';            
        isValid: boolean = false;
        //relationshipManageruserId: string = null;      //todo! 

        contactIds: string[] = [];

        constructor(company?: Dto.ICompany) {
            if (company) {
                this.name = company.name;
                this.websiteUrl = company.websiteUrl;
                this.clientCarePageUrl = company.clientCarePageUrl;
                this.clientCareStatusId = (company.clientCareStatus ? company.clientCareStatus.id : null);
                this.categoryId = (company.category ? company.category.id : null);
                this.typeId = (company.type ? company.type.id : null);
                this.description = company.description;
                this.isValid = company.isValid;

                this.contactIds = company.contacts.map((contact: Dto.IContact) => { return contact.id });
            }
        }

       
    }
}