/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import PropertyViewController = Property.View.PropertyViewController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    describe('Given view activity page is loaded', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            filter: ng.IFilterService,
            $http: ng.IHttpBackendService,
            controller: PropertyViewController;

        var pageObjectSelectors = {
            propertyCard: 'card#activity-view-card-property',
            vendorList: {
                main: 'list#list-vendors',
                header: 'list-header',
                noItems: 'list-no-items',
                items: 'list-item'
            },
            well: {
                main: '#activity-view-well',
                createdDate: '#activity-view-well-createdDate',
                status: '#activity-view-well-activityStatus'
            },
            address: 'address-form-view',
            propertyPreview: {
                main: 'property-preview',
                address: '#property-preview-address'
            }
        };

        describe('when activity is loaded', () => {
            var createdDateMock = new Date('2011-01-01');
            var activityMock: Dto.IActivity = new Business.Activity(<Dto.IActivity>{
                id : 'It1',
                propertyId: '1',
                property: {
                    id: '1',
                    propertyTypeId: '1',
                    address: Antares.Mock.AddressForm.FullAddress,
                    ownerships: [],
                    activities: []
                },
                activityStatusId : '123',
                createdDate : createdDateMock,
                contacts : [
                    <Business.Contact>{ id : 'Contact1', firstName : 'John', surname : 'Test1', title : 'Mr' },
                    <Business.Contact>{ id : 'Contact2', firstName : 'Amy', surname : 'Test2', title : 'Mrs' }
                ]
            });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService) =>{

                $http = $httpBackend;
                filter = $filter;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                scope['activity'] = activityMock;
                element = $compile('<activity-view activity="activity"></activity-view>')(scope);

                scope.$apply();
                $http.flush();

                controller = element.controller('activityView');
            }));

            it('then card component for activity is set up', () => {
                // assert
                var cardElement = element.find(pageObjectSelectors.propertyCard);

                expect(cardElement.length).toBe(1);
                expect(cardElement[0].getAttribute('card-template-url')).toBe("'app/property/templates/propertyCard.html'");
                expect(cardElement[0].getAttribute('item')).toBe("avvm.activity.property");
                expect(cardElement[0].getAttribute('show-item-details')).toBe("avvm.showPropertyPreview");
            });

            it('then list component for vendors is set up', () => {
                // assert
                var listElement = element.find(pageObjectSelectors.vendorList.main),
                    listHeaderElement = listElement.find(pageObjectSelectors.vendorList.header),
                    listHeaderElementContent = listHeaderElement.find('[translate="ACTIVITY.VIEW.VENDORS"]'),
                    listNoItemsElement = listElement.find(pageObjectSelectors.vendorList.noItems),
                    listNoItemsElementContent = listNoItemsElement.find('[translate="ACTIVITY.VIEW.NO_VENDORS"]');

                expect(listElement.length).toBe(1);
                expect(listHeaderElement.length).toBe(1);
                expect(listHeaderElementContent.length).toBe(1);
                expect(listNoItemsElement.length).toBe(1);
                expect(listNoItemsElementContent.length).toBe(1);
            });

            it('then well component for activity is displayed and should have proper data', () => {
                // assert
                var wellElement = element.find(pageObjectSelectors.well.main);
                var createdDateElement = wellElement.find(pageObjectSelectors.well.createdDate);
                var activityStatusElement = wellElement.find(pageObjectSelectors.well.status);

                var formattedDate = filter('date')(createdDateMock, 'dd-MM-yyyy');
                expect(wellElement.length).toBe(1);
                expect(createdDateElement.length).toBe(1);
                expect(createdDateElement.text()).toBe(formattedDate);
                expect(activityStatusElement.length).toBe(1);
                expect(activityStatusElement.text()).toBe('ENUMS.123');
            });
        });

        describe('and vendors are loaded', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when no vendors then "no items" element should be visible', () => {
                // arrange
                var createdDateMock = new Date('2011-01-01');
                var activityMock: Dto.IActivity = new Business.Activity(<Dto.IActivity>{
                    id: 'It1',
                    propertyId: '1',
                    property: {
                        id: '1',
                        propertyTypeId: '1',
                        address: Antares.Mock.AddressForm.FullAddress,
                        ownerships: [],
                        activities: []
                    },
                    activityStatusId: '123',
                    createdDate: createdDateMock,
                    contacts: []
                });

                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var listElement = element.find(pageObjectSelectors.vendorList.main);
                var noItemsElement = listElement.find(pageObjectSelectors.vendorList.noItems);
                var listItemElements = listElement.find(pageObjectSelectors.vendorList.items);

                expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(listItemElements.length).toBe(0);
            });

            it('when existing vendors then list item components should be visible', () => {
                // arrange
                var createdDateMock = new Date('2011-01-01');
                var activityMock: Dto.IActivity = new Business.Activity(<Dto.IActivity>{
                    id: 'It1',
                    propertyId: '1',
                    property: {
                        id: '1',
                        propertyTypeId: '1',
                        address: Antares.Mock.AddressForm.FullAddress,
                        ownerships: [],
                        activities: []
                    },
                    activityStatusId: '123',
                    createdDate: createdDateMock,
                    contacts: [
                        <Business.Contact>{ id: 'Contact1', firstName: 'John', surname: 'Test1', title: 'Mr' },
                        <Business.Contact>{ id: 'Contact2', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }
                    ]
                });

                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var listElement = element.find(pageObjectSelectors.vendorList.main);
                var noItemsElement = listElement.find(pageObjectSelectors.vendorList.noItems);
                var listItemElements = listElement.find(pageObjectSelectors.vendorList.items);

                expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(listItemElements.length).toBe(2);
            });

            it('when existing vendors then list item components should have proper data', () => {
                // arrange
                var createdDateMock = new Date('2011-01-01');
                var activityMock: Dto.IActivity = new Business.Activity(<Dto.IActivity>{
                    id: 'It1',
                    propertyId: '1',
                    property: {
                        id: '1',
                        propertyTypeId: '1',
                        address: Antares.Mock.AddressForm.FullAddress,
                        ownerships: [],
                        activities: []
                    },
                    activityStatusId: '123',
                    createdDate: createdDateMock,
                    contacts: [
                        <Business.Contact>{ id: 'Contact1', firstName: 'John', surname: 'Test1', title: 'Mr' },
                        <Business.Contact>{ id: 'Contact2', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }
                    ]
                });

                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var listElement = element.find(pageObjectSelectors.vendorList.main);
                var listItemElement1 = listElement.find(pageObjectSelectors.vendorList.items + '#list-item-vendor-Contact1');
                var listItemElement2 = listElement.find(pageObjectSelectors.vendorList.items + '#list-item-vendor-Contact2');

                expect(listItemElement1.text()).toBe('John Test1');
                expect(listItemElement2.text()).toBe('Amy Test2');

            });
        });

        describe('and property is loaded', () =>{
            var createdDateMock = new Date('2011-01-01');
            var activityMock: Dto.IActivity = new Business.Activity(<Dto.IActivity>{
                id : 'It1',
                propertyId : '1',
                property : {
                    id : '1',
                    propertyTypeId : '1',
                    address : Antares.Mock.AddressForm.FullAddress,
                    ownerships : [],
                    activities : []
                },
                activityStatusId : '123',
                createdDate : createdDateMock,
                contacts : [
                    <Business.Contact>{ id : 'Contact1', firstName : 'John', surname : 'Test1', title : 'Mr' },
                    <Business.Contact>{ id : 'Contact2', firstName : 'Amy', surname : 'Test2', title : 'Mrs' }
                ]
            });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) =>{

                $http = $httpBackend;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() =>{
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;

                scope['activity'] = activityMock;
                element = compile('<activity-view activity="activity"></activity-view>')(scope);

                scope.$apply();
                $http.flush();
            }));

            it('when existing property then card component should have proper data', () => {
                // assert
                var cardElement = element.find(pageObjectSelectors.propertyCard);
                var addressElement = cardElement.find(pageObjectSelectors.address);

                expect(addressElement.length).toBe(1);
                expect(addressElement[0].getAttribute('address')).toBe("cvm.item.address");
            });
        });

        describe('and property details is clicked', () => {
            var createdDateMock = new Date('2011-01-05');
            var activityMock: Dto.IActivity = new Business.Activity(<Dto.IActivity>{
                id: 'It1',
                propertyId: '1',
                property: {
                    id: '1',
                    propertyTypeId: '1',
                    address: Antares.Mock.AddressForm.FullAddress,
                    ownerships: [],
                    activities: []
                },
                activityStatusId: '123',
                createdDate: createdDateMock,
                contacts: [
                    <Business.Contact>{ id: 'Contact1', firstName: 'John', surname: 'Test1', title: 'Mr' },
                    <Business.Contact>{ id: 'Contact2', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }
                ]
            });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;

                scope['activity'] = activityMock;
                element = compile('<activity-view activity="activity"></activity-view>')(scope);

                scope.$apply();
                $http.flush();
            }));

            it('then property details are visible on property preview panel', () => {
                // act
                var cardElement = element.find(pageObjectSelectors.propertyCard);
                cardElement.find('a').click();

                // assert
                var propertyPreviewPanel = element.find(pageObjectSelectors.propertyPreview.main);
                var addressElement = propertyPreviewPanel.find(pageObjectSelectors.propertyPreview.address);
                var addressValueElement = addressElement.find(pageObjectSelectors.address);

                expect(addressElement.length).toBe(1);
                expect(addressValueElement.length).toBe(1);

                //TODO: test viewving property type when it is ready
                //var typeElement = propertyPreviewPanel.find('pageObjectSelectors.propertyPreview.type');
                //expect(typeElement.text()).toBe('ENUMS.456');
            });
        });
    });
}