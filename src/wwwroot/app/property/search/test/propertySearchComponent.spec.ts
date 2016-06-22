/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import runDescribe = TestHelpers.runDescribe;

    describe('Given property search component', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            assertValidator: TestHelpers.AssertValidators,
            controller: Antares.Property.PropertySearchController;


        var pageObjectSelectors = {
            searchInput: '#search-query',
            searchOptions: '#search-options'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            // init
            scope = $rootScope.$new();
            compile = $compile;
            $http = $httpBackend;

            // compile
            element = compile('<property-search></property-search>')(scope);
            scope.$apply();
            controller = element.controller('propertySearch');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        it('then should validate if search phrase empty', () => {
            assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.searchInput);
            assertValidator.assertRequiredValidator('search me', true, pageObjectSelectors.searchInput);
        });


        describe('then search option', () => {
            it('should be not be displayed when `showSearchOptions` is set to false', () => {
                controller.showSearchOptions = false;
                scope.$apply();

                var searchOptionsElement = element.find(pageObjectSelectors.searchOptions);
                expect(searchOptionsElement.length).toBe(0);
            });

            it('should be be displayed when `showSearchOptions` is set to true', () => {
                controller.showSearchOptions = true;
                scope.$apply();

                var searchOptionsElement = element.find(pageObjectSelectors.searchOptions);
                expect(searchOptionsElement.length).toBe(1);
            });
        });
    });
}