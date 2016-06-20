declare module Antares.Common.Models.Dto {
    interface IPropertySearchResultOwnership {
        id: string;
        ownershipTypeId: string;
        sellDate: Date;
        contacts: Dto.IPropertySearchResultContact[];
    }
}