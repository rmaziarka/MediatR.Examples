declare module Antares.Common.Models.Dto {
    interface ICreateCompanyResource {
        name: string;
        contactIds: string[];
        websiteUrl: string;
        clientCarePageUrl: string;
        clientCareStatusId: string;
        description: string;
        companyCategoryId: string;
        companyTypeId: string;
        valid: boolean;
        relationshipManageruserId: string;
      }
}