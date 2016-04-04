declare module Antares.Common.Models.Dto {
    interface IProperty {
        id: string;
        address: Dto.Address;
        ownerships: Business.Ownership[];
        activities: Dto.Activity[];
    }
}