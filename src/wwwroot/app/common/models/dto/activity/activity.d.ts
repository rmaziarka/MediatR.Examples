declare module Antares.Common.Models.Dto {
    interface IActivity {
        id: string;
        propertyId: string;
        activityStatusId: string;
        activityTypeId: string;
        contacts: IContact[];
        property?: IProperty;
        createdDate?: Date;
        marketAppraisalPrice?: number;
        recommendedPrice?: number;
        vendorEstimatedPrice?: number;
    }
}