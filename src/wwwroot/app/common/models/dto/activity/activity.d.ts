declare module Antares.Common.Models.Dto {
    interface IActivity {
        id: string;
        propertyId: string;
        activityStatusId: string;
        contacts: IContact[];
        property?: Property;
        createdDate?: Date;
    }
}