declare module Antares.Common.Models.Dto {
    interface IEditCompanyResource {  
        id: string;     
        name: string;        
        contacts: { id: string; }[];     
        websiteUrl: string;
        clientCarePageUrl: string;
        clientCareStatusId: string;
        description: string;
        companyCategoryId: string;
        companyTypeId: string;
        valid: boolean;
        relationshipManagerId: string;
      }
}