declare module Antares.Common.Models.Dto {
    interface ICompany {
        id: string;
        name: string;
        websiteUrl: string;
        clientCarePageUrl: string;   
        clientCareStatusId: string;
        contacts: IContact[];   
        companiesContacts: ICompanyContact[];
        description: string;
        companyCategoryId: string;
        companyTypeId: string;
        valid: boolean;
        relationshipManager: IUser;
    }
}