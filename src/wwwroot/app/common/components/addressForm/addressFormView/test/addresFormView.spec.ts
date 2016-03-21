/// <reference path="../../../../../typings/_all.d.ts" />
/// <reference path="../addressFormViewController.ts" />

describe('Given address form view is rendered', () => {
    var scope: ng.IScope,
        element: ng.IAugmentedJQuery,
        controller: Antares.Common.Component.AddressFormViewController;

    beforeEach(angular.mock.module('app'));
    beforeEach(inject((
        $rootScope: ng.IRootScopeService,
        $compile: ng.ICompileService) => {

        scope = $rootScope.$new();
        element = $compile('<address-form-view></address-form-view>')(scope);
        scope.$apply();

        controller = element.controller('adressFormView');
    }));

    it('should be available', () => {
        var message = element.find('div').text();
        expect(message).toBe('Hello address form');
    });

});