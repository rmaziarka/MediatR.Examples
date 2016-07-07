declare module Antares.Common.Models.Dto {
    interface ICompany {
        id: string;
        name: string;   
        websiteUrl: string;
        clientCarePageUrl: string;   
        clientCareStatusId: string;
        clientCareStatus: Dto.IEnumTypeItem;
        contacts: IContact[];   
        companiesContacts: ICompanyContact[];     
    }
}