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
            $http.whenGET(/\/api\/enums\/OwnershipType\/items/).respond(() => {
                return [200, { "items": [{ "id": "1", "value": "Freeholder" }, { "id": "2", "value": "Leaseholder" }] }];
            });

            scope = $rootScope.$new();
            scope["ownerships"] = [];
            element = $compile('<ownership-add ownerships="ownerships"></ownership-add>')(scope);
            $httpBackend.flush();
            scope.$apply();

            controller = element.controller('ownershipAdd');

            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }));

        describe('when', () => {
            describe('ownership type value is ', () => {
                it('missing then required message should be displayed', () => {
                    assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.ownershipTypeSelector);
                });

                it('not missing then required message should not be displayed', () => {
                    assertValidator.assertRequiredValidator('1', true, pageObjectSelectors.ownershipTypeSelector);
                });
            });
            describe('ownership purchasing date is ', () => {
                it('invalid then required message should be displayed', () => {
                    assertValidator.assertRequiredValidator('invalid date', false, '[name=purchaseDate]');
                });

                it('vaild then required message should not be displayed', () => {
                    assertValidator.assertRequiredValidator('21-12-1984', true, '[name=purchaseDate]');
                });
            });
            describe('ownership selling date is ', () => {

                it('ownership selling date is invalid then required message should be displayed', () => {
                    assertValidator.assertRequiredValidator('invalid date', false, '[name=sellDate]');
                });

                it('ownership selling date is vaild then required message should not be displayed', () => {
                    assertValidator.assertRequiredValidator('21-12-1984', true, '[name=sellDate]');
                });
            });
            describe('ownership buying price is ', () => {
                it('lower then zero', () => {
                    assertValidator.assertMinValueValidator(-10, false, "[name=buyPrice]");
                });
            });
        });
    });
}