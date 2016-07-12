/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Company implements Dto.ICompany {
        id: string = null;
        name: string = null;    
        websiteUrl: string = null;
        clientCarePageUrl: string = null;
        clientCareStatus: Business.EnumTypeItem = null;
        contacts: Contact[] = [];
        companiesContacts: Models.Dto.ICompanyContact[] = [];
        description: string = null;
        category: Dto.IEnumTypeItem = null;        //todo! check
        type: Dto.IEnumTypeItem = null;            //todo! check
        isValid: boolean = false;
        //relationshipManager: Contact = null;      //todo! check

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

                //this.relationshipManager = new Contact(company.relationshipManager);//todo! 
            }
        }

        getCategoryId(): string{
            return (this.category ? this.category.id : null);
        }

        setCategoryById(id: string){
            this.category = new EnumTypeItem({ id: id, code: null, enumTypeId: null });
        }

        getTypeId(): string {
            return (this.type ? this.type.id : null);
        }

        setTypeById(id: string) {
            this.type = new EnumTypeItem({ id: id, code: null, enumTypeId: null });
        }

        getClientCareStatus(id: string) {
            return (this.clientCareStatus ? this.clientCareStatus.id : null);
        }

        setClientCareStatus(id: string){
            this.clientCareStatus = new EnumTypeItem({ id: id, code: null, enumTypeId: null });
        }
    }
}