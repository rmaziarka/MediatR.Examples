declare module Antares.Common.Models.Dto {
    interface ICreateActivityCommand {
        propertyId: string;
        activityStatusId: string;
        contactIds: string[];
    }
}