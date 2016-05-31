declare module Antares.Common.Models.Dto {
    interface IUpdateActivityResource {
        id: string;
        activityStatusId: string;
        marketAppraisalPrice?: number;
        recommendedPrice?: number;
        vendorEstimatedPrice?: number;

        leadNegotiator: IUpdateActivityUserResource;
        secondaryNegotiators: IUpdateActivityUserResource[];
    }
}