declare module Antares.Common.Models.Dto {
    interface ITenancyContact {
        id: string;
        userId: string;
        user: Dto.IUser;
        userType: Dto.IEnumTypeItem;
    }
}