/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import PropertyAddController = Property.PropertyAddController;

    describe('Given add property page is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            controller: PropertyAddController,
            assertValidator: Antares.TestHelpers.AssertValidators;

        var pageObjectSelectors = {
            propertyTypeSelector: 'select#type'
        };


        var countriesMock = [{ country: { id: "countryId1", isoCode: "GB" }, locale: {}, value: "United Kingdom" }];
        var propertyTypesMock: any = {
            propertyTypes: [{ id: "8b152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "House", children: [], order: 22 },
                { id: "8c152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "Flat", children: [], order: 23 },
                { id: "90152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "Development Plot", children: [], order: 27 }]
        };

        var propertyAttributesMock = { "attributes": [{ "order": 2, "nameKey": "GuestRooms", "labelKey": "PROPERTY.GUESTROOMS" }] }
        var propertyAttributesForHouseMock = { "attributes": [{ "order": 0, "nameKey": "Area", "labelKey": "PROPERTY.AREA" }, { "order": 1, "nameKey": "LandArea", "labelKey": "PROPERTY.LANDAREA" }, { "order": 2, "nameKey": "GuestRooms", "labelKey": "PROPERTY.GUESTROOMS" }] }
        var propertyAttributesForFlatMock = { "attributes": [{ "order": 0, "nameKey": "Bedrooms", "labelKey": "PROPERTY.BEDROOM" }, { "order": 1, "nameKey": "Receptions", "labelKey": "PROPERTY.RECEPTIONS" }, { "order": 2, "nameKey": "GuestRooms", "labelKey": "PROPERTY.GUESTROOMS" }] }


        describe('when page is loaded', () => {
            var countryMockId = countriesMock[0].country.id,
                propertyTypesMockId = propertyTypesMock.propertyTypes[0].id,
                newPropertyMock: Business.Property = new Business.Property();

            newPropertyMock.id = 'propId1';
            newPropertyMock.address = new Business.Address();
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

                $http.whenGET(/\/api\/addressForms/).respond(() => {
                    return [200, {}];
                });

                $http.whenGET(/\/api\/properties\/attributes\?countryCode=GB&propertyTypeId=8b152e4f-f505-e611-828c-8cdcd42baca7/).respond(() => {
                    return [200, propertyAttributesForHouseMock];
                });

                $http.whenGET(/\/api\/properties\/attributes\?countryCode=GB&propertyTypeId=8c152e4f-f505-e611-828c-8cdcd42baca7/).respond(() => {
                    return [200, propertyAttributesForFlatMock];
                });

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
                    return [200, propertyAttributesMock];
                });

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
                    return [200, {}];
                });

                $http.whenGET(/\/api\/enums\/Division\/items/).respond(() => {
                    return [200, {}];
                });

                // compile
                scope['property'] = newPropertyMock;
                scope['userData'] = usermock;
                element = compile('<property-add property="property" user-data="userData"></property-add>')(scope);
                $httpBackend.flush();
                scope.$apply();
                controller = element.controller('propertyAdd');

                controller.property = newPropertyMock;

                assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
            }));

            it('then page displays address form component', () => {
                var addressFormComponent = element.find('address-form-edit');
                expect(addressFormComponent.length).toBe(1);
            });

            it('then save button is disabled if country is not selected', () => {
                newPropertyMock.address.countryId = '';
                scope.$apply();

                var button = element.find('button#saveBtn');
                expect(button[0].getAttribute('disabled')).toBeTruthy();
            });

            it('then save button is enabled if country is selected', () => {
                newPropertyMock.address.countryId = countryMockId;
                scope.$apply();

                var button = element.find('button#saveBtn');
                expect(button[0].getAttribute('disabled')).toBeFalsy();
            });

            describe('when property type is changed', () => {
                it('then proper attributes are shown', () => {
                    controller.property.propertyTypeId = "8b152e4f-f505-e611-828c-8cdcd42baca7";
                    scope.$apply();
                    controller.loadAttributes();
                    $http.flush();
                    scope.$apply();

                    var htmlAttributes = element.find('input#minArea, input#maxArea, input#minLandArea, input#maxLandArea, input#minGuestRooms, input#maxGuestRooms');
                    expect(htmlAttributes.length).toEqual(6);

                    controller.property.propertyTypeId = "8c152e4f-f505-e611-828c-8cdcd42baca7";
                    scope.$apply();
                    controller.loadAttributes();
                    $http.flush();
                    scope.$apply();

                    htmlAttributes = element.find('input#minBedrooms, input#maxBedrooms, input#minReceptions, input#maxReceptions, input#minGuestRooms, input#maxGuestRooms');
                    expect(htmlAttributes.length).toEqual(6);
                });

                it('then attribute values retains even if they are hidden', () => {
                    var attributeMock = {
                        minArea: 11,
                        maxArea: 22,
                        minLandArea: 33,
                        maxLandArea: 44,
                        minGuestRooms: 33,
                        maxGuestRooms: 44
                    };

                    controller.property.propertyTypeId = "8b152e4f-f505-e611-828c-8cdcd42baca7";
                    scope.$apply();
                    controller.loadAttributes();
                    $http.flush();
                    controller.property.attributeValues = attributeMock;
                    scope.$apply();

                    controller.property.propertyTypeId = "8c152e4f-f505-e611-828c-8cdcd42baca7";
                    scope.$apply();
                    controller.loadAttributes();
                    $http.flush();
                    scope.$apply();

                    expect(controller.property.attributeValues).toEqual(attributeMock);
                });
            });

            describe('when valid data and save button is clicked', () => {
                it('then save method is called', () => {
                    spyOn(controller, 'save');
                    newPropertyMock.address.countryId = countryMockId;
                    newPropertyMock.propertyTypeId = propertyTypesMockId;
                    scope.$apply();

                    var button = element.find('button#saveBtn');
                    button.click();

                    expect(controller.save).toHaveBeenCalled();
                });

                it('then put request is is called and redirect to view page', () => {
                    var addressFormMock: Business.AddressForm = new Business.AddressForm('adrfrmId1', countryMockId, []);
                    $http.whenGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() => {
                        return [200, addressFormMock];
                    });

                    var propertyFromServerMock: Business.Property = new Business.Property();
                    propertyFromServerMock.id = 'propFromServerId1';
                    propertyFromServerMock.address = new Business.Address();

                    $http.expectPOST(/\/api\/properties/, newPropertyMock).respond(() => {
                        return [200, propertyFromServerMock];
                    });

                    var propertyId: string;
                    spyOn(state, 'go').and.callFake((routeName: string, property: Business.Property) => {
                        propertyId = property.id;
                    });

                    newPropertyMock.address = new Business.Address();

                    newPropertyMock.address.id = 'adrId1';
                    newPropertyMock.address.countryId = countryMockId;
                    newPropertyMock.address.addressFormId = 'adrfrmId1';
                    newPropertyMock.address.propertyName = 'test prop name';
                    newPropertyMock.address.propertyNumber = '123456';

                    scope.$apply();

                    var button = element.find('button#saveBtn');
                    button.click();
                    $http.flush();

                    expect(state.go).toHaveBeenCalled();
                    expect(propertyId).toEqual(propertyFromServerMock.id);
                });

                it('then hidden attribte values are removed', () => {
                    $http.whenPOST(/\/api\/properties/).respond(() => {
                        return [200, {}];
                    });
                    var attributeMock = {
                        minArea: 11,
                        maxArea: 22,
                        minLandArea: 33,
                        maxLandArea: 44,
                        minGuestRooms: 33,
                        maxGuestRooms: 44,
                        minReceptions: 33,
                        maxReceptions: 44
                    };

                    controller.property.propertyTypeId = "8b152e4f-f505-e611-828c-8cdcd42baca7";
                    scope.$apply();
                    controller.loadAttributes();
                    controller.property.attributeValues = attributeMock;
                    $http.flush();
                    scope.$apply();

                    var button = element.find('button#saveBtn');
                    button.click();

                    expect(controller.property.attributeValues.minArea).not.toBeNull();
                    expect(controller.property.attributeValues.maxArea).not.toBeNull();
                    expect(controller.property.attributeValues.minLandArea).not.toBeNull();
                    expect(controller.property.attributeValues.maxLandArea).not.toBeNull();
                    expect(controller.property.attributeValues.minGuestRooms).not.toBeNull();
                    expect(controller.property.attributeValues.maxGuestRooms).not.toBeNull();
                    expect(controller.property.attributeValues.minReceptions).toBeNull();
                    expect(controller.property.attributeValues.maxReceptions).toBeNull();
                });
            });

            describe('when', () => {
                describe('property type value is ', () => {
                    it('missing then required message should be displayed', () => {
                        assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.propertyTypeSelector);
                    });

                    it('not missing then required message should not be displayed', () => {
                        assertValidator.assertRequiredValidator('8b152e4f-f505-e611-828c-8cdcd42baca7', true, pageObjectSelectors.propertyTypeSelector);
                    });
                });
            });
        });
    });
}