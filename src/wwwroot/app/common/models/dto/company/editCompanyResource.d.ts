declare module Antares.Common.Models.Dto {
    interface IEditCompanyResource {  
        id: string;     
        name: string;        
        contactIds: string[];     
        websiteUrl: string;
        clientCarePageUrl: string;
        clientCareStatusId: string;
      }
}