﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import PropertyViewController = Property.View.PropertyViewController;
    import ContactListController = Component.ContactListController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    describe('Given view property page is loaded', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            filter: ng.IFilterService,
            $http: ng.IHttpBackendService;

        var pageObjectSelectors = {
            activity: {
                createdDate : '#activity-preview-created-date',
                status : '#activity-preview-status',
                vendors: '#activity-preview-vendors [id^=activity-preview-vendor-item-]'
            }
        }

        var userMock = { name: "user", email: "user@gmail.com", country: "GB", division: { id: "0acc9d28-51fa-e511-828b-8cdcd42baca7", value: "Commercial", code: "Commercial" }, divisionCode: <any>null, roles: ["admin", "superuser"] };

        var controller: PropertyViewController;

        describe('and property is loaded', () =>{
            var propertyMock: Dto.IProperty = {
                id: '1',
                propertyTypeId: 'propType1',
                divisionId: '',
                division: null,
                address : Antares.Mock.AddressForm.FullAddress,
                ownerships : [],
                activities : [
                    new Business.Activity(<Dto.IActivity>{ id: 'It1', propertyId: '1', activityStatusId: '123', contacts:[] }),
                    new Business.Activity(<Dto.IActivity>{ id: 'It2', propertyId: '1', activityStatusId: '1234', contacts: [] })
                ],
                attributeValues: {}
            };

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
                    return [200, { attributes: []}];
                });

                scope = $rootScope.$new();
                scope['userData'] = userMock;
                scope['property'] = propertyMock;
                element = $compile('<property-view property="property" user-data="userData"></property-view>')(scope);

                scope.$apply();
                $http.flush();

                controller = element.controller('propertyView');
            }));

            it('card-list component for activities is set up', () => {
                // assert
                var cardListElement = element.find('card-list#card-list-activities'),
                    cardListHeaderElement = cardListElement.find('card-list-header'),
                    cardListHeaderElementContent = cardListHeaderElement.find('[translate="ACTIVITY.LIST.ACTIVITIES"]'),
                    cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                    cardListNoItemsElementContent = cardListNoItemsElement.find('[translate="ACTIVITY.LIST.NO_ITEMS"]');

                expect(cardListElement.length).toBe(1);
                expect(cardListElement[0].getAttribute('show-item-add')).toBe("vm.showActivityAdd");
                expect(cardListHeaderElement.length).toBe(1);
                expect(cardListHeaderElementContent.length).toBe(1);
                expect(cardListNoItemsElement.length).toBe(1);
                expect(cardListNoItemsElementContent.length).toBe(1);
            });

            it('card components for activities are set up', () => {
                // assert
                var cardListElement = element.find('card-list#card-list-activities'),
                    cardListItemElement = cardListElement.find('card-list-item'),
                    cardListItemCardElement = cardListItemElement.find('card');

                expect(cardListItemElement.length).toBe(2);
                expect(cardListItemCardElement.length).toBe(2);
                expect(cardListItemCardElement[0].getAttribute('card-template-url')).toBe("'app/activity/templates/activityCard.html'");
                expect(cardListItemCardElement[0].getAttribute('show-item-details')).toBe("vm.showActivityPreview");
            });
        });

        describe('and activities are loaded', () => {
            var propertyMock: Dto.IProperty = {
                id: '1',
                propertyTypeId: '1',
                divisionId: '',
                division: null,
                address: Antares.Mock.AddressForm.FullAddress,
                ownerships: [],
                activities: [],
                attributeValues: []
            };

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

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
                    return [200, { attributes: [] }];
                });

                scope = $rootScope.$new();
                scope['userData'] = userMock;
                compile = $compile;
            }));

            it('when no activities then "no items" element should be visible', () => {
                // arrange
                propertyMock.activities = [];
                scope['property'] = propertyMock;

                // act
                element = compile('<property-view property="property" user-data="userData"></property-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var noItemsElement = element.find('card-list-no-items');
                var cardElements = element.find('card');
                expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(cardElements.length).toBe(0);
            });

            it('when existing activities then card components should be visible', () => {
                // arrange
                propertyMock.activities = [
                    new Business.Activity(<Dto.IActivity>{ id: 'It1', propertyId: '1', activityStatusId: '123', contacts: [] }),
                    new Business.Activity(<Dto.IActivity>{ id: 'It2', propertyId: '1', activityStatusId: '1234', contacts: [] })];
                scope['property'] = propertyMock;

                // act
                element = compile('<property-view property="property" user-data="userData"></property-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var noItemsElement = element.find('card-list-no-items');
                var cardElements = element.find('card');
                expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(cardElements.length).toBe(2);
            });

            it('when existing activities then card components should have proper data', () => {
                // arrange
                var date1Mock = new Date('2011-01-01');
                propertyMock.activities = [
                    new Business.Activity(<Dto.IActivity>{
                        id : 'It1',
                        propertyId : '1',
                        activityStatusId : '123',
                        createdDate : date1Mock,
                        contacts : [
                            <Business.Contact>{ id : 'Contact1', firstName : 'John', surname : 'Test1', title : 'Mr' },
                            <Business.Contact>{ id : 'Contact2', firstName : 'Amy', surname : 'Test2', title : 'Mrs' }
                        ]
                    })
                ];

                scope['property'] = propertyMock;

                // act
                element = compile('<property-view property="property" user-data="userData"></property-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var cardListElement = element.find('card-list');
                var cardElement = cardListElement.find('card#activity-card-It1');
                var activityDataElement = cardElement.find('[id="activity-data-It1"]');
                var activityStatusElement = cardElement.find('[id="activity-status-It1"]');

                var formattedDate = filter('date')(date1Mock, 'dd-MM-yyyy');
                expect(activityDataElement.text()).toBe(formattedDate + ' John Test1, Amy Test2');
                expect(activityStatusElement.text()).toBe('DYNAMICTRANSLATIONS.123');
            });
        });

        describe('and activity details is clicked', () => {
            var date1Mock = new Date('2011-01-01');
            var date2Mock = new Date('2011-01-05');
            var propertyMock: Dto.IProperty = {
                id : '1',
                propertyTypeId : '1',
                address: Antares.Mock.AddressForm.FullAddress,
                divisionId: '',
                division: null,
                ownerships : [],
                activities : [
                    new Business.Activity(<Dto.IActivity>{ id: 'It1', propertyId: '1', activityStatusId: '123', createdDate: date1Mock, contacts: [] }),
                    new Business.Activity(<Dto.IActivity>{
                        id : 'It2',
                        propertyId : '1',
                        activityStatusId: '456',
                        createdDate: date2Mock,
                        contacts : [
                            <Business.Contact>{ id : 'Contact1', firstName : 'John', surname : 'Test1', title : 'Mr' },
                            <Business.Contact>{ id : 'Contact2', firstName : 'Amy', surname : 'Test2', title : 'Mrs' }
                        ]
                    })
                ],
                attributeValues: []
            };

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
                    return [200, { attributes: [] }];
                });

                scope = $rootScope.$new();
                scope['property'] = propertyMock;
                scope['userData'] = userMock;
                element = $compile('<property-view property="property" user-data="userData"></property-view>')(scope);

                scope.$apply();
                $httpBackend.flush();
            }));

            it('then activity details are visible on activity preview panel', () => {
                // act
                var cardListElement = element.find('card-list');
                var cardElement = cardListElement.find('card#activity-card-It2');

                cardElement.find('a').click();

                // assert
                var activityPreviewPanel = element.find('activity-preview');
                var statusElement = activityPreviewPanel.find(pageObjectSelectors.activity.status);
                var formattedDate = filter('date')(date2Mock, 'dd-MM-yyyy');
                var dateElement = activityPreviewPanel.find(pageObjectSelectors.activity.createdDate);
                var vendorsItemsElement = activityPreviewPanel.find(pageObjectSelectors.activity.vendors);

                expect(statusElement.text()).toBe('DYNAMICTRANSLATIONS.456');
                expect(dateElement.text()).toBe(formattedDate);
                expect(vendorsItemsElement.length).toBe(2);
                expect(vendorsItemsElement[0].innerText).toBe('John Test1');
                expect(vendorsItemsElement[1].innerText).toBe('Amy Test2');
            });
        });

        describe('and contact list is opened', () => {
            var propertyMock: Dto.IProperty = {
                id: '1',
                propertyTypeId: '1',
                divisionId: '',
                division: null,
                address: Antares.Mock.AddressForm.FullAddress,
                ownerships: [],
                activities: [],
                attributeValues: []
            };

            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $httpBackend.whenGET("").respond(() => { return {}; });

                scope = $rootScope.$new();
                scope['property'] = propertyMock;
                scope['userData'] = userMock;
                element = $compile('<property-view property="property" user-data="userData"></property-view>')(scope);

                scope.$apply();
                $httpBackend.flush();
            }));

            it('when contacts are selected and Configure button is clicked then contacts are visible on ownership add panel', () => {
                // arrange
                var contactListController: ContactListController = element.find('contact-list').controller('contactList');
                contactListController.isLoading = false;
                contactListController.contacts = [
                    new Business.Contact(<Dto.IContact>{ firstName: 'Vernon', surname: 'Vaughn', title: 'Mr' }),
                    new Business.Contact(<Dto.IContact>{ firstName: 'Julie', surname: 'Lerman', title: 'Mrs' }),
                    new Business.Contact(<Dto.IContact>{ firstName: 'Mark', surname: 'Rendle', title: 'Mr' })
                ];

                scope.$apply();

                // act
                var checkboxes = element.find('contact-list').find('input');
                checkboxes[0].click();
                checkboxes[2].click();

                element.find('#ownership-add-button').click();

                // assert
                var addOwnershipPanel = element.find('ownership-add');
                var names = addOwnershipPanel.find('.full-name');
                expect(names.length).toBe(2);
                expect(names[0].innerHTML).toBe('Vernon Vaughn');
                expect(names[1].innerHTML).toBe('Mark Rendle');
            });
        });

        describe('and contact list is opened', () => {
            var createDateAsUtc = Antares.Core.DateTimeUtils.createDateAsUtc;

            var contacts1: Dto.IContact[] = [
                { id: 'db9b4d6b-178a-41ce-8d29-8182b8a533c6', firstName: 'John', surname: 'Papa', title: 'Mr' },
                { id: '78fbd602-5cd6-456e-a7e5-996d5ae2bbe5', firstName: 'Mark', surname: 'Rendle', title: 'Mr' }
            ];

            var contacts2: Dto.IContact[] = [
                { id: 'db9b4d6b-178a-41ce-8d29-8182b8a533c6', firstName: 'Julie', surname: 'Lerman', title: 'Mrs' },
                { id: '78fbd602-5cd6-456e-a7e5-996d5ae2bbe5', firstName: 'Ian', surname: 'Cooper', title: 'Mr' }
            ];

            var ownerships: Dto.IOwnership[] = [
                {
                    id: 'ba0390a3-4ce3-4a66-962b-197d10e5df7a', createDate: new Date(), ownershipTypeId: 'leaseholderId',
                    ownershipType: { code: Common.Models.Enums.OwnershipTypeEnum.Leaseholder }, contacts: contacts1, buyPrice: 1000, sellPrice: 2000,
                    sellDate: createDateAsUtc(new Date('2016-02-01')), purchaseDate: createDateAsUtc(new Date('2016-01-01'))
                },
                {
                    id: 'ed9107c2-83b9-4723-9547-9b399d5907b2', createDate: new Date(), ownershipTypeId: 'freeholderId',
                    ownershipType: { code: Common.Models.Enums.OwnershipTypeEnum.Freeholder }, contacts: contacts2, buyPrice: 4000, sellPrice: 8000,
                    sellDate: createDateAsUtc(new Date('2016-04-01')), purchaseDate: createDateAsUtc(new Date('2016-03-01'))
                }
            ];

            var propertyMock: Dto.IProperty = {
                id: '1',
                address: Antares.Mock.AddressForm.FullAddress,
                divisionId: '',
                division: null,
                ownerships: ownerships,
                activities: [],
                propertyTypeId: 'typeId',
                attributeValues: []
            };

            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $httpBackend.whenGET("").respond(() => { return {}; });

                scope = $rootScope.$new();
                scope['property'] = new Business.Property(propertyMock);
                scope['userData'] = userMock;
                element = $compile('<property-view property="property" user-data="userData"></property-view>')(scope);

                scope.$apply();
                $httpBackend.flush();
            }));

            it('when ownership details is clicked then panel is shown and data is showed', () => {
                // act
                var secondownershipCard = element.find('#ownership-list card-list-item:nth-child(2)');
                secondownershipCard.find('a').click();

                // assert
                var ownershipPanel = element.find('#ownership-panel .side-panel');
                var panelIsOpened = ownershipPanel.hasClass('slide-in');
                expect(panelIsOpened).toBeTruthy();

                var contacts = ownershipPanel.find('[data-type="ownership-list-contact"]');
                var contactNames = contacts.toArray().map((el: HTMLElement) => el.innerHTML);
                expect(contactNames).toEqual(["John Papa, ", "Mark Rendle"]);

                var ownershipTypeCode = getValueFromElement(ownershipPanel, 'ownership-list-ownership-type');
                expect(ownershipTypeCode).toBe('DYNAMICTRANSLATIONS.leaseholderId');

                var purchaseDate = getValueFromElement(ownershipPanel, 'ownership-list-purchase-date');
                expect(purchaseDate).toBe('01-01-2016');

                var sellDate = getValueFromElement(ownershipPanel, 'ownership-list-sell-date');
                expect(sellDate).toBe('01-02-2016');

                var purchasePrice = getValueFromElement(ownershipPanel, 'ownership-list-purchase-price');
                expect(purchasePrice).toBe('1000');

                var sellPrice = getValueFromElement(ownershipPanel, 'ownership-list-sell-price');
                expect(sellPrice).toBe('2000');
            });

            function getValueFromElement(ownershipPanel: ng.IAugmentedJQuery, fieldId: string): string{
                return ownershipPanel.find('#' + fieldId).html();
            }
        });
    });
}