/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import PropertyViewController = Property.View.PropertyViewController;
    import IProperty = Antares.Common.Models.Dto.IProperty;
    import ContactListController = Antares.Component.ContactListController;
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    describe('Given view activity page is loaded', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            filter: ng.IFilterService,
            $http: ng.IHttpBackendService;

        var controller: PropertyViewController;

        describe('and activity is loaded', () => {
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
                    <Dto.Contact>{ id : 'Contact1', firstName : 'John', surname : 'Test1', title : 'Mr' },
                    <Dto.Contact>{ id : 'Contact2', firstName : 'Amy', surname : 'Test2', title : 'Mrs' }
                ]
            });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) =>{

                $http = $httpBackend;

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

            it('card component for property is set up', () => {
                // assert
                var cardElement = element.find('card#card-property');

                expect(cardElement.length).toBe(1);
                expect(cardElement[0].getAttribute('card-template-url')).toBe("'app/property/templates/propertyCard.html'");
                expect(cardElement[0].getAttribute('item')).toBe("avvm.activity.property");
                expect(cardElement[0].getAttribute('show-item-details')).toBe("avvm.showPropertyPreview");
            });

            it('list component for vendors is set up', () => {
                // assert
                var listElement = element.find('list#list-vendors'),
                    listHeaderElement = listElement.find('list-header'),
                    listHeaderElementContent = listHeaderElement.find('[translate="ACTIVITY.VIEW.VENDORS"]'),
                    listNoItemsElement = listElement.find('list-no-items'),
                    listNoItemsElementContent = listNoItemsElement.find('[translate="ACTIVITY.VIEW.NO_VENDORS"]');

                expect(listElement.length).toBe(1);
                expect(listHeaderElement.length).toBe(1);
                expect(listHeaderElementContent.length).toBe(1);
                expect(listNoItemsElement.length).toBe(1);
                expect(listNoItemsElementContent.length).toBe(1);
            });
        });

        describe('and vendors are loaded', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;
                filter = $filter;

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
                var listElement = element.find('list#list-vendors');
                var noItemsElement = listElement.find('list-no-items');
                var listItemElements = listElement.find('list-item');

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
                        <Dto.Contact>{ id: 'Contact1', firstName: 'John', surname: 'Test1', title: 'Mr' },
                        <Dto.Contact>{ id: 'Contact2', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }
                    ]
                });

                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var listElement = element.find('list#list-vendors');
                var noItemsElement = listElement.find('list-no-items');
                var listItemElements = listElement.find('list-item');

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
                        <Dto.Contact>{ id: 'Contact1', firstName: 'John', surname: 'Test1', title: 'Mr' },
                        <Dto.Contact>{ id: 'Contact2', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }
                    ]
                });

                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var listElement = element.find('list#list-vendors');
                var listItemElement1 = listElement.find('list-item#list-item-vendor-Contact1');
                var listItemElement2 = listElement.find('list-item#list-item-vendor-Contact2');

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
                    <Dto.Contact>{ id : 'Contact1', firstName : 'John', surname : 'Test1', title : 'Mr' },
                    <Dto.Contact>{ id : 'Contact2', firstName : 'Amy', surname : 'Test2', title : 'Mrs' }
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
                var cardElement = element.find('card#card-property');
                var addressElement = cardElement.find('address-form-view');

                expect(addressElement.length).toBe(1);
                expect(addressElement[0].getAttribute('address')).toBe("cvm.item.address");
            });
        });
    });
}