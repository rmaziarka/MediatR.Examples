/// <reference path="../../../../../typings/_all.d.ts" />
/// <reference path="addressFormViewServices.mock.ts" />
module Antares {

    describe("Given address form controller", () => {
        var scope: ng.IScope,
            controller: Antares.Common.Component.AddressFormViewController,
            address: Antares.Common.Models.Dto.Address;

        beforeEach(angular.mock.module('app'));
        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService) => {

            Antares.Mock.AddressForm.mockHttpResponce($httpBackend, "a1", [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);

            address = Antares.Mock.AddressForm.FullAddress;
            scope = $rootScope.$new();
            controller = $controller("addressFormViewController", {}, { addressFormId: "a1", address: address });
            $httpBackend.flush();
        }));

        xit('render one line of address', () => {
            var addressLine = controller.getAddressRowText(0);
            expect(addressLine).toBe("");
        });
    });
}