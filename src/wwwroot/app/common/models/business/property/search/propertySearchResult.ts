module Antares.Common.Models.Business {
    export class PropertySearchResult {
        total: number = 0;
        size: number = 0;
        page: number = 0;
        data: Business.PropertySearchResultItem[] = [];

        constructor(propertySearchResult?: Dto.IPropertySearchResult) {
            if (propertySearchResult) {
                this.total = propertySearchResult.total;
                this.size = propertySearchResult.size;
                this.page = propertySearchResult.page;
                this.data = propertySearchResult.data.map((d: Dto.IPropertySearchResultItem) => { return new PropertySearchResultItem(d) });
            }
        }
    }
}