declare module Antares.Common.Models.Dto {
    interface IOffer {
        id: string;
        activityId: string;
        requirementId: string;
        statusId: string;
        price: number;
        offerDate: Date;
        exchangeDate?: Date;
        completionDate?: Date;
        specialConditions: string;
        negotiatorId: string;
        negotiator: Dto.IUser;
        activity: Dto.IActivity;
        requirement: Dto.IRequirement;
        status: Dto.IEnumTypeItem;
        createdDate?:Date;
    }
}