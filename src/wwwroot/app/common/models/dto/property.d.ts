declare module Antares.Common.Models.Dto {
    interface IProperty {
        id: string;
        propertyTypeId: string,
        address: Dto.IAddress;
        ownerships: Dto.IOwnership[];
        activities: Dto.IActivity[];
    }
}