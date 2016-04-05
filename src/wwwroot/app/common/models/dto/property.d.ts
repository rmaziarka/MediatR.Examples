declare module Antares.Common.Models.Dto {
    interface IProperty {
        id: string;
        address: Dto.Address;
        ownerships: Dto.IOwnership[];
        activities: Dto.IActivity[];
    }
}