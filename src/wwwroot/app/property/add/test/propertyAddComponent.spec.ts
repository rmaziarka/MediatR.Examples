/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import PropertyAddController = Property.PropertyAddController;
    import KfMessageService = Services.KfMessageService;

    describe('Given add property page is loaded', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            compile: ng.ICompileService,
            state: ng.ui.IStateService,
            controller: PropertyAddController,
            assertValidator: TestHelpers.AssertValidators,
            q: ng.IQService,
            messageService: KfMessageService;

        var pageObjectSelectors = {
            propertyTypeSelector: 'select#type'
        };

        var propertyTypes: any = {
            House: { id: "8b152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "House", children: [], order: 22 },
            Flat: { id: "8c152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "Flat", children: [], order: 23 },
            DevelopmentPlot: { id: "90152e4f-f505-e611-828c-8cdcd42baca7", parentId: null, name: "Development Plot", children: [], order: 27 }
        }
        var countries: any = {
            GB: { id: 'countryGB', isoCode: 'GB' }
        };

        var countriesMock = [{ country: countries.GB, locale: {}, value: "United Kingdom" }];
        var propertyTypesMock: any = { propertyTypes: [propertyTypes.House, propertyTypes.Flat, propertyTypes.DevelopmentPlot] };
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
                $httpBackend: ng.IHttpBackendService,
                $q: ng.IQService,
                kfMessageService: KfMessageService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;
                q = $q;
                messageService = kfMessageService;

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

                $http.whenGET(new RegExp('\\/api\\/properties\\/attributes\\?countryId=' + countries.GB.id + '&propertyTypeId=' + propertyTypes.House.id)).respond(() => {
                    return [200, propertyAttributesForHouseMock];
                });

                $http.whenGET(new RegExp('\\/api\\/properties\\/attributes\\?countryId=' + countries.GB.id + '&propertyTypeId=' + propertyTypes.Flat.id)).respond(() => {
                    return [200, propertyAttributesForFlatMock];
                });

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
                    return [200, propertyAttributesMock];
                });

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
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

                assertValidator = new TestHelpers.AssertValidators(element, scope);
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
                    controller.property.propertyTypeId = propertyTypes.House.id;
                    scope.$apply();
                    controller.loadAttributes();
                    $http.flush();
                    scope.$apply();

                    var htmlAttributes = element.find('input#minArea, input#maxArea, input#minLandArea, input#maxLandArea, input#minGuestRooms, input#maxGuestRooms');
                    expect(htmlAttributes.length).toEqual(6);

                    controller.property.propertyTypeId = propertyTypes.Flat.id;
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

                    controller.property.propertyTypeId = propertyTypes.House.id;
                    scope.$apply();
                    controller.loadAttributes();
                    $http.flush();
                    controller.property.attributeValues = attributeMock;
                    scope.$apply();

                    controller.property.propertyTypeId = propertyTypes.Flat.id;
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

                it('then put request is called and redirect to view page with success message', () => {
                    var addressFormMock: Business.AddressForm = new Business.AddressForm('adrfrmId1', countryMockId, []);
                    $http.whenGET(/\/api\/addressForms\/\?entityType=Property&countryCode=GB/).respond(() => {
                        return [200, addressFormMock];
                    });

                    var requestData: Business.CreateOrUpdatePropertyResource;
                    var propertyFromServerMock: Business.Property = new Business.Property();
                    propertyFromServerMock.id = 'propFromServerId1';
                    propertyFromServerMock.address = new Business.Address();

                    $http.expectPOST(/\/api\/properties/, (data: string) => {
                        requestData = JSON.parse(data);

                        return true;
                    }).respond(() => {
                        return [200, propertyFromServerMock];
                    });

                    var stateDeffered = q.defer();
                    var propertyId: string;
                    spyOn(state, 'go').and.callFake((routeName: string, property: Business.Property) =>{
                        propertyId = property.id;
                        return stateDeffered.promise;
                    });

                    spyOn(messageService, 'showSuccessByCode');

                    newPropertyMock.address = new Business.Address();

                    newPropertyMock.address.id = 'adrId1';
                    newPropertyMock.address.countryId = countryMockId;
                    newPropertyMock.address.addressFormId = 'adrfrmId1';
                    newPropertyMock.address.propertyName = 'test prop name';
                    newPropertyMock.address.propertyNumber = '123456';

                    stateDeffered.resolve();
                    scope.$apply();

                    var button = element.find('button#saveBtn');
                    button.click();
                    $http.flush();

                    expect(state.go).toHaveBeenCalled();
                    expect(messageService.showSuccessByCode).toHaveBeenCalledWith('PROPERTY.ADD.PROPERTY_ADD_SUCCESS');

                    expect(requestData.id).toEqual(newPropertyMock.id);
                    expect(propertyId).toEqual(propertyFromServerMock.id);
                });

                it('then hidden attribute values are removed', () => {
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

                    controller.property.propertyTypeId = propertyTypes.House.id;
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

                it('when server error occurs then error message is displayed and no redirect occurs', () =>{
                    var
                        requestData: Business.CreateOrUpdatePropertyResource,
                        response: any= {};

                    $http.expectPOST(/\/api\/properties/, (data: string) =>{
                        requestData = JSON.parse(<string>data);

                        return true;
                    }).respond(() =>{
                        return [500, response];
                    });

                    spyOn(state, 'go');
                    spyOn(messageService, 'showErrors');

                    scope.$apply();

                    var button = element.find('button#saveBtn');
                    button.click();
                    $http.flush();

                    expect(messageService.showErrors).toHaveBeenCalled();
                    var showErrorsCallArgs = (<jasmine.Spy>messageService.showErrors).calls.mostRecent().args;
                    expect(showErrorsCallArgs.length).toEqual(1);
                    expect(showErrorsCallArgs[0].data).toBeDefined();
                    expect(showErrorsCallArgs[0].data).toEqual(response);
                    expect(state.go).not.toHaveBeenCalled();
                });
            });

            describe('when', () => {
                describe('property type value is ', () => {
                    it('missing then required message should be displayed', () => {
                        assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.propertyTypeSelector);
                    });

                    it('not missing then required message should not be displayed', () => {
                        $http.whenGET(/\/api\/characteristicGroups/).respond(() => {
                            return [200, []];
                        });

                        assertValidator.assertRequiredValidator(propertyTypes.House.id, true, pageObjectSelectors.propertyTypeSelector);
                    });
                });
            });
        });
    });
}