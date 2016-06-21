/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;

    describe('propertySearchController', () => {
        var controller: Property.PropertySearchController;
        var q: ng.IQService;
        var scope: ng.IScope;

        beforeEach(inject(($rootScope: ng.IRootScopeService, $componentController: any, $q: ng.IQService) => {
            q = $q;
            scope = $rootScope.$new();

            controller = $componentController('propertySearch', { $scope: scope });
        }));

        describe('when search is invoked', () => {
            it('then current page should be set to one and searching should be invoked', () => {
                controller.currentPage = 100;
                spyOn(controller, 'onPageChanged');

                controller.search();

                expect(controller.currentPage).toBe(1);
                expect(controller.onPageChanged).toHaveBeenCalled();
            });
        });

        describe('when onPageChanged is invoked', () => {
            var getSearchResultDefer: ng.IDeferred<Business.PropertySearchResult>;
            beforeEach(() => {
                getSearchResultDefer = q.defer();

                spyOn(controller.propertyService, 'getSearchResult').and.returnValue(getSearchResultDefer.promise);
            });

            it('then correct parametrs are passed to service', () => {
                controller.searchQuery = 'foo';
                controller.currentPage = 20;
                controller.resultsPerPage = 25;

                controller.onPageChanged();

                expect(controller.propertyService.getSearchResult).toHaveBeenCalledWith('foo', 19, 25);
            });

            it('then controller properties are set after promise is resolved', () => {
                var returnedData: Business.PropertySearchResultItem[] = [
                    new Business.PropertySearchResultItem(),
                    new Business.PropertySearchResultItem()
                ];

                controller.onPageChanged();

                getSearchResultDefer.resolve(<Business.PropertySearchResult>{ total: 2, data: returnedData });
                scope.$apply();

                expect(controller.showSearchOptions).toBe(true);
                expect(controller.totalResults).toEqual(2);
                expect(controller.searchResult.length).toBe(2);
                expect(controller.searchResult).toEqual(returnedData);
            });
        });
    });
}