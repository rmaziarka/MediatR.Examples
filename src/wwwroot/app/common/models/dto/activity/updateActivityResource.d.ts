declare module Antares.Common.Models.Dto {
    interface IUpdateActivityResource {
        id: string;
        activityStatusId: string;
        marketAppraisalPrice?: number;
        recommendedPrice?: number;
        vendorEstimatedPrice?: number;
        leadNegotiatorId: string;
        secondaryNegotiatorIds: string[];
    }
}