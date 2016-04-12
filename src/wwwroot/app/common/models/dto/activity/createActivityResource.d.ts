declare module Antares.Common.Models.Dto {
    interface ICreateActivityResource {
        propertyId: string;
        activityStatusId: string;
        contactIds: string[];
    }
}