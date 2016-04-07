/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    describe('Given address form view is rendered', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: Antares.Common.Component.AddressFormViewController,
            addressFormId = 'a1';

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