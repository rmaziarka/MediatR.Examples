/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Business = Common.Models.Business;

    export class PropertySearchController {
        showSearchOptions: boolean;
        searchQuery: string;
        resultsPerPageOptions = [25, 50, 100];
        resultsPerPage: number = this.resultsPerPageOptions[0];
        currentPage: number = 1;
        totalResults: number = 0;

        searchResult: Business.PropertySearchResultItem[] = [];

        constructor(public propertyService: Services.PropertyService) {
        }

        search = () => {
            this.currentPage = 1;
            this.onPageChanged();
        }

        onPageChanged = () =>{
            this.propertyService.getSearchResult(this.searchQuery, this.currentPage - 1, this.resultsPerPage)
                .then((result: Business.PropertySearchResult) =>{
                    this.showSearchOptions = true;
                    this.searchResult = result.data;
                    this.totalResults = result.total;
                });
        }
    }
    angular.module('app').controller('PropertySearchController', PropertySearchController);
}
