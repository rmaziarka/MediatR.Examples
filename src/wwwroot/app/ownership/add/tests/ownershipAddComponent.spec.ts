/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OwnershipAddController = Antares.Property.OwnershipAddController;
    describe('Given ownership is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: Antares.TestHelpers.AssertValidators;

        var pageObjectSelectors = {
        };

        var controller: OwnershipAddController;

        beforeEach(angular.mock.module('app'));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
            element = $compile('<ownership-add></ownership-add>')(scope);
            scope.$apply();

            controller = element.controller('ownershipAdd');
            scope.$apply();

            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }));
    });
}