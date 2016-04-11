declare module Antares.Common.Models.Dto {
    interface IProperty {
        id: string;
        propertyTypeId: string,
        divisionId: string,
        address: Dto.IAddress;
        ownerships: Dto.IOwnership[];
        activities: Dto.IActivity[];
        division: Dto.IEnumTypeItem;
    }
}