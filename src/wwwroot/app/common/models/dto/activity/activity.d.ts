declare module Antares.Common.Models.Dto {
    interface IActivity {
        id: string;
        propertyId: string;
        activityStatusId: string;
        activityTypeId: string;
        contacts: IContact[];
        attachments?: IAttachment[];
        property?: IProperty;
        createdDate?: Date;
        marketAppraisalPrice?: number;
        recommendedPrice?: number;
        vendorEstimatedPrice?: number;
        viewings?: IViewing[];
    }
}