/// <reference path="../../../typings/_all.d.ts" />
/// <reference path="../../../common/models/dto/property.ts" />
/// <reference path="../../../common/models/dto/address.ts" />
/// <reference path="../../../common/models/dto/addressform.ts" />

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

        describe('when property is beeing loaded', () => {
            beforeEach(angular.mock.module('app'));
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                $http = $httpBackend;

                // http backend
                $http.whenGET(/\/api\/addressform\/countries\?entityType=Property/).respond(() => {
                    return [200, countriesMock];
                });
                $http.whenGET(/\/api\/property/).respond(() => {
                    return [200, {}];
                });

                // compile
                element = compile('<property-edit></property-edit>')(scope);
                scope.$apply();
                controller = element.controller('propertyEdit');
            }));

            it('then loading message appears', () => {
                expect(controller.isLoading).toBeTruthy();
            });
        });

        describe('when proper property is loaded', () =>{
            var countryMockId = countriesMock[0].country.id,
                addressFormMock: Dto.AddressForm = new Dto.AddressForm('adrfrmId1', countryMockId, []),
                propertyMock: Dto.Property = new Dto.Property('propMockId1',
                    new Dto.Address('adrId1', countryMockId, 'adrfrmId1', 'test prop name', '123456'));

            beforeEach(angular.mock.module('app'));
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) =>{

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                 // http backend
                $http.whenGET(/\/api\/addressform\/countries\?entityType=Property/).respond(() => {
                    return [200, countriesMock];
                });
                $http.whenGET(/\/api\/property/).respond(() => {
                    return [200, propertyMock];
                });
                $http.whenGET(/\/api\/addressForm\/\?entityType=Property&countryCode=GB/).respond(() => {
                    return [200, addressFormMock];
                });

                // compile
                element = compile('<property-edit></property-edit>')(scope);
                scope.$apply();
                controller = element.controller('propertyEdit');

                $http.flush();
            }));

            it('then loading message disappears', () => {
                expect(controller.isLoading).toBeFalsy();
            });

            it('then property details are loaded', () => {
                for (var attr in propertyMock) {
                    if (propertyMock.hasOwnProperty(attr)) {
                        var value = propertyMock[attr];
                        if (angular.isArray(value)) {
                            expect(controller.property[attr].length).toBe(value.length);
                        }
                        else {
                            expect(controller.property[attr]).toEqual(value);
                        }
                    }
                }
            });

            it('then page displays address form component', () => {
                var addressFormComponent = element.find('address-form-edit');
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
                    $http.expectPUT(/\/api\/property/, propertyMock).respond(() => {
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