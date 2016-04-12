/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import PropertyEditController = Property.PropertyEditController;

    describe('Given edit property page is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            controller: PropertyEditController;

        var countriesMock = [{ country: { id: "1111", isoCode: "GB" }, locale: {}, value: "United Kingdom" }];
        var propertyTypesMock = { propertyTypes: [{ "id": "45cc9d28-51fa-e511-828b-8cdcd42baca7", "parentId": '45cc9d28-51fa-e511-828b-8cdcd42baca7', "name": "Office" }] };

        describe('when proper property is loaded', () => {
            var countryMockId = countriesMock[0].country.id,
                propertyTypesMockId = propertyTypesMock.propertyTypes[0].id,
                addressFormMock: Business.AddressForm = new Business.AddressForm('adrfrmId1', countryMockId, []),
                propertyMock: Business.Property = new Business.Property();

            propertyMock.id = 'propMockId1';
            propertyMock.address = new Business.Address();
            propertyMock.address.id = 'adrId1';
            propertyMock.address.countryId = countryMockId;
            propertyMock.address.addressFormId = 'adrfrmId1';
            propertyMock.address.propertyName = 'test prop name';
            propertyMock.address.propertyNumber = '123456';
            propertyMock.propertyTypeId = propertyTypesMockId;
            var usermock = { name: "user", email: "user@gmail.com", country: "GB", division: { id: "0acc9d28-51fa-e511-828b-8cdcd42baca7", value: "Commercial", code: "Commercial" }, divisionCode: <any>null, roles: ["admin", "superuser"] };

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
                $http.whenGET(/\/api\/resources\/countries\/addressform\?entityTypeItemCode=Property/).respond(() => {
                    return [200, countriesMock];
                });

                $http.whenGET(/\/api\/properties\/types/).respond(() => {
                    return [200, propertyTypesMock];
                });

                $http.whenGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() => {
                    return [200, addressFormMock];
                });

                // compile
                scope['property'] = propertyMock;
                scope['userData'] = usermock;
                element = compile('<property-edit property="property" user-data="userData"></property-edit>')(scope);
                $httpBackend.flush();
                scope.$apply();
                controller = element.controller('propertyEdit');
            }));

            it('then property details are loaded', () => {
                controller.property.address = propertyMock.address;
                expect(controller.property.address).toEqual(propertyMock.address);
            });

            it('then page displays address form component', () => {
                var addressFormComponent: ng.IAugmentedJQuery = element.find('address-form-edit');
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
                    spyOn(state, 'go').and.callFake((routeName: string, property: Business.Property) => {
                        propertyId = property.id;
                    });

                    var propertyFromServerMock: Business.Property = new Business.Property();
                    propertyFromServerMock.id = 'propFromServerId1';
                    propertyFromServerMock.address = new Business.Address();

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