declare module Antares.Common.Models.Dto {
    interface IUpdateActivityResource {
        id: string;
        activityStatusId: string;
        marketAppraisalPrice?: number;
        recommendedPrice?: number;
        vendorEstimatedPrice?: number;
		askingPrice?: number;
		shortLetPricePerWeek?: number;

        leadNegotiator: IUpdateActivityUserResource;
        secondaryNegotiators: IUpdateActivityUserResource[];

        departments: IUpdateActivityDepartmentResource[];
    }
}