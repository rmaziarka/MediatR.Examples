declare module Antares.Common.Models.Dto {
    interface IContactUser {
        id: string;
        contactId: string;
        userId: string;
        user: IUser;
        userType: Dto.IEnumTypeItem     
    }
}