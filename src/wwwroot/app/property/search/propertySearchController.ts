/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Business = Common.Models.Business;

    export class PropertySearchController {
        showSearchOptions: boolean;
        searchQuery: string;
        resultsPerPageOptions = [25, 50, 100];
        resultsPerPage: number = 25;
        totalResults: number = 0;
        
        searchResult: Business.PropertySearchResultItem[] = [];

        constructor(private propertyService: Services.PropertyService) {

        }

        search = () => {
            this.propertyService.getSearchResult(this.searchQuery, 0, this.resultsPerPage)
                .then((result: Business.PropertySearchResult) => {
                    this.showSearchOptions = true;
                    this.searchResult = [];
                    this.totalResults = result.total;

                    return result.data;
                })
                .then((items: Business.PropertySearchResultItem[]) => {
                    _.each(items, (item: Business.PropertySearchResultItem) => this.searchResult.push(item));
                });
        }
    }
    angular.module('app').controller('PropertySearchController', PropertySearchController);
}
