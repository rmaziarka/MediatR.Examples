declare module Antares.Common.Models.Dto {
    interface IPropertySearchResultItem {
        id: string;
        addressId: string;
        address: Dto.IPropertySearchResultAddress;
        ownerships: Dto.IPropertySearchResultOwnership[];
    }
}