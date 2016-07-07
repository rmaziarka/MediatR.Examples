declare module Antares.Common.Models.Dto {
    interface IActivityAttendee {
        userId: string;
        user: IUser;
        contactId: string;
        contact: IContact;
    }
}