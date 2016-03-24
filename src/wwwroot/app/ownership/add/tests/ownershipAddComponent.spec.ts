/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OwnershipAddController = Antares.Property.OwnershipAddController;
    describe('Given ownership is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: Antares.TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService;

        var pageObjectSelectors = {
            ownershipTypeSelector: 'select#type'
        };

        var controller: OwnershipAddController;

        beforeEach(angular.mock.module('app'));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            $http.whenGET(/\/api\/enums\/OwnershipType\/items/).respond(() =>{
                return [200, { "items" : [{ "id" : "1", "value" : "Freeholder" }, { "id" : "2", "value" : "Leaseholder" }] }];
            });

            scope = $rootScope.$new();
            element = $compile('<ownership-add></ownership-add>')(scope);
            $httpBackend.flush();
            scope.$apply();

            controller = element.controller('ownershipAdd');

            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }));

        it('when ownership type value is missing then required message should be displayed', () => {
            assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.ownershipTypeSelector);
        });

        it('when ownership type value is not missing then required message should not be displayed', () => {
            assertValidator.assertRequiredValidator('1', true, pageObjectSelectors.ownershipTypeSelector);
        });
    });
}