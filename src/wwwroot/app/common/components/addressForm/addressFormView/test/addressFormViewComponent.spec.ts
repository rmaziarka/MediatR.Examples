/// <reference path="../../../../../typings/_all.d.ts" />
/// <reference path="../addressFormViewController.ts" />
/// <reference path="../../../../../common/models/dto/addressForm.ts" />
/// <reference path="addressFormViewServices.mock.ts" />

module Antares {
    import AddressFormFieldDefinition = Antares.Common.Models.Dto.AddressFormFieldDefinition;
    describe('Given address form view is rendered', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: Antares.Common.Component.AddressFormViewController,
            addressFormId = 'a1';

        beforeEach(angular.mock.module('app'));
        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {
            
            //Antares.Mock.AddressForm.mockHttpResponce($httpBackend,addressFormId,[200,Antares.Mock.AddressForm.AddressFormWithOneLine]);
            
            scope = $rootScope.$new();
            element = $compile(`<address-form-view address-form-id="'${addressFormId}'"></address-form-view>`)(scope);

            //$httpBackend.flush();
            scope.$apply();
            controller = element.controller('addressFormView');
        }));

        xit('and render address labels', () => {
            expect(controller.addressForm.id).toBe(addressFormId);
            expect(element.find('label').length).toBe(1);
        })
    });
}