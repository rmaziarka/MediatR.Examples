declare module Antares.Common.Models.Dto {
    interface IPropertySearchResultOwnership {
        id: string;
        ownershipTypeId: string;
        contacts: Dto.IPropertySearchResultContact[];
    }
}