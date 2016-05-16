declare module Antares.Common.Models.Dto {
    interface IActivityUser {
        activityId: string;
        activity: IActivity;
        userId: string;
        user: IUser;
        userType: Enums.NegotiatorTypeEnum;
    }
}