declare module Antares.Common.Models.Dto {
    interface IActivityUser {
        id: string;
        activityId: string;
        userId: string;
        user: IUser;
        userType: Dto.IEnumTypeItem;
    }
}