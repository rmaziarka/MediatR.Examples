declare module Antares.Common.Models.Dto {
    interface IProperty {
        id: string;
        propertyTypeId: string,
        address: Dto.Address;
        ownerships: Dto.IOwnership[];
        activities: Dto.IActivity[];
    }
}