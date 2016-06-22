/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    describe('addressFormViewController', () => {
        var componentController: any;
        var q: ng.IQService;
        var scope: ng.IScope;

        var createController = (addressFormDefinitions: Business.AddressFormFieldDefinition[], address: Dto.IAddress): Common.Component.AddressFormViewController => {
            var providerMock = {
                property: {
                    'country1': new Business.AddressForm('a1', 'country1', addressFormDefinitions)
                }
            }

            var bindings = {
                addressType: 'property',
                address: address
            }

            return componentController('addressFormView', { $scope: scope, addressFormsProvider: providerMock }, bindings);
        }

        beforeEach(inject(($rootScope: ng.IRootScopeService, $componentController: any, $q: ng.IQService) => {
            q = $q;
            scope = $rootScope.$new();
            componentController = $componentController;
        }));

        describe('when constructor is invoked', () => {
            it('then address field definitions should be filtered based on address values', () => {
                var addressFormDefintions = [
                    new Business.AddressFormFieldDefinition("propertyName", "propertyName", true, "[a-zA-Z]", 0, 0, 1),
                    new Business.AddressFormFieldDefinition("propertyNumber", "propertyNumber", true, "[a-zA-Z]", 0, 1, 1),
                    new Business.AddressFormFieldDefinition("City", "cityKey", true, "[a-zA-Z]", 2, 0, 1),
                    new Business.AddressFormFieldDefinition("Line3", "label3Key", true, "[a-zA-Z]", 1, 0, 1)
                ];

                var address = <Dto.IAddress>{
                    id: '1',
                    propertyName: 'Castle',
                    propertyNumber: '1',
                    city: 'London',
                    countryId: 'country1'
                }

                var expectedAddressFormDefinitions = [
                    addressFormDefintions.filter((x: Business.AddressFormFieldDefinition) => x.name === 'propertyName')[0],
                    addressFormDefintions.filter((x: Business.AddressFormFieldDefinition) => x.name === 'propertyNumber')[0],
                    addressFormDefintions.filter((x: Business.AddressFormFieldDefinition) => x.name === 'City')[0]
                ];

                var controller = createController(addressFormDefintions, address);

                expect(controller.addressForm.addressFieldDefinitions).toEqual(expectedAddressFormDefinitions);
            });
        });
    });
}