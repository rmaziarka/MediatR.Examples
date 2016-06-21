declare module Antares.Common.Models.Dto {
    interface IPropertySearchResult {
        total: number;
        size: number;
        page: number;
        data: Dto.IPropertySearchResultItem[];
    }
}