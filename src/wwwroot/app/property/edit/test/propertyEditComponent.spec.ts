/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Dto = Antares.Common.Models.Dto;
    import PropertyEditController = Antares.Property.PropertyEditController;

    describe('Given edit property page is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            controller: PropertyEditController;

        var countriesMock = [{ country: { id: "1111", isoCode: "GB" }, locale: {}, value: "United Kingdom" }];

       describe('when proper property is loaded', () => {
            var countryMockId = countriesMock[0].country.id,
                addressFormMock: Dto.AddressForm = new Dto.AddressForm('adrfrmId1', countryMockId, []),
                propertyMock: Dto.Property = new Dto.Property('propMockId1',
                    new Dto.Address('adrId1', countryMockId, 'adrfrmId1', 'test prop name', '123456'));

            beforeEach(angular.mock.module('app'));
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                $http.whenGET(/\/api\/addressforms\/countries\?entityType=Property/).respond(() => {
                    return [200, countriesMock];
                });
                $http.whenGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() => {
                    return [200, addressFormMock];
                });

                // compile
                scope['property'] = propertyMock;
                element = compile('<property-edit property="property"></property-edit>')(scope);
                scope.$apply();
                controller = element.controller('propertyEdit');

                $http.flush();
            }));

            it('then property details are loaded', () =>{
                controller.property.address = propertyMock.address;
                expect(controller.property.address).toEqual(propertyMock.address);
            });

            it('then page displays address form component', () => {
                var addressFormComponent :ng.IAugmentedJQuery= element.find('address-form-edit');
                expect(addressFormComponent.length).toBe(1);
            });

            describe('when valid data and and save button is clicked', () => {
                it('then save method is called', () => {
                    spyOn(controller, 'save');

                    var button = element.find('button#saveBtn');
                    button.click();

                    expect(controller.save).toHaveBeenCalled();
                });

                it('then put request is is called and redirect to view page', () => {
                    var propertyId: string;
                    spyOn(state, 'go').and.callFake((routeName: string, property: Dto.Property) => {
                        propertyId = property.id;
                    });

                    var propertyFromServerMock: Dto.Property = new Dto.Property('propFromServerId1', new Dto.Address());
                    $http.expectPUT(/\/api\/properties/, propertyMock).respond(() => {
                        return [200, propertyFromServerMock];
                    });

                    var button = element.find('button#saveBtn');
                    button.click();
                    $http.flush();

                    expect(state.go).toHaveBeenCalled();
                    expect(propertyId).toEqual(propertyFromServerMock.id);
                });
            });
        });
    });
}