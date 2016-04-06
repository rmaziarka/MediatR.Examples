/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import PropertyViewController = Property.View.PropertyViewController;
    import ContactListController = Antares.Component.ContactListController;
    import Dto = Antares.Common.Models.Dto;
    import Models = Antares.Common.Models;

    describe('Given view property page is loaded', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService;

        var controller: PropertyViewController;

        describe('and proper property id is provided', () => {
            var propertyMock: Dto.IProperty = {
                id: '1',
                address: Antares.Mock.AddressForm.FullAddress,
                ownerships: [],
                activities: [
                    new Dto.Activity(<Dto.IActivity>{ id: 'It1', propertyId: '1', activityStatusId: '123', contacts: [] }),
                    new Dto.Activity(<Dto.IActivity>{ id: 'It2', propertyId: '1', activityStatusId: '1234', contacts: [] })
                ]
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

                scope = $rootScope.$new();
                scope['property'] = propertyMock;
                element = $compile('<property-view property="property"></property-view>')(scope);

                scope.$apply();
                $http.flush();

                controller = element.controller('propertyView');
            }));

            it('card-list component for activities is set up', () => {
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
                address: Antares.Mock.AddressForm.FullAddress,
                ownerships: [],
                activities: []
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

                scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when no activities then "no items" element should be visible', () => {
                propertyMock.activities = [];
                scope['property'] = propertyMock;
                element = compile('<property-view property="property"></property-view>')(scope);

                scope.$apply();
                $http.flush();

                var noItemsElement = element.find('card-list-no-items');
                var cardElements = element.find('card');
                expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(cardElements.length).toBe(0);
            });

            it('when existing activities then card components should be visible', () => {
                propertyMock.activities = [
                    new Dto.Activity(<Dto.IActivity>{ id: 'It1', propertyId: '1', activityStatusId: '123', contacts: [] }),
                    new Dto.Activity(<Dto.IActivity>{ id: 'It2', propertyId: '1', activityStatusId: '1234', contacts: [] })];
                scope['property'] = propertyMock;
                element = compile('<property-view property="property"></property-view>')(scope);

                scope.$apply();
                $http.flush();

                var noItemsElement = element.find('card-list-no-items');
                var cardElements = element.find('card');
                expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(cardElements.length).toBe(2);
            });
        });

        describe('and contact list is opened', () => {
            var propertyMock: Dto.IProperty = {
                id: '1',
                address: Antares.Mock.AddressForm.FullAddress,
                ownerships: [],
                activities: []
            };

            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $httpBackend.whenGET("").respond(() => { return {}; });

                scope = $rootScope.$new();
                scope['property'] = propertyMock;
                element = $compile('<property-view property="property"></property-view>')(scope);

                scope.$apply();
                $httpBackend.flush();
            }));

            it('when contacts are selected and Configure button is clicked then contacts are visible on ownership add panel', () => {
                // arrange
                var contactListController: ContactListController = element.find('contact-list').controller('contactList');
                contactListController.isLoading = false;
                contactListController.contacts = [
                    new Dto.Contact(<Dto.IContact>{ firstName: 'Vernon', surname: 'Vaughn', title: 'Mr' }),
                    new Dto.Contact(<Dto.IContact>{ firstName: 'Julie', surname: 'Lerman', title: 'Mrs' }),
                    new Dto.Contact(<Dto.IContact>{ firstName: 'Mark', surname: 'Rendle', title: 'Mr' })
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
                    id: 'ba0390a3-4ce3-4a66-962b-197d10e5df7a', createDate: new Date(),
                    ownershipType: { code: Models.Enums.OwnershipTypeEnum.Freeholder }, contacts: contacts1, buyPrice: 1000, sellPrice: 2000,
                    sellDate: createDateAsUtc(new Date('2016-02-01')), purchaseDate: createDateAsUtc(new Date('2016-01-01'))
                },
                {
                    id: 'ed9107c2-83b9-4723-9547-9b399d5907b2', createDate: new Date(),
                    ownershipType: { code: Models.Enums.OwnershipTypeEnum.Leaseholder }, contacts: contacts2, buyPrice: 4000, sellPrice: 8000,
                    sellDate: createDateAsUtc(new Date('2016-04-01')), purchaseDate: createDateAsUtc(new Date('2016-03-01'))
                }
            ];

            var propertyMock: Dto.IProperty = {
                id: '1',
                address: Antares.Mock.AddressForm.FullAddress,
                ownerships: ownerships,
                activities: []
            };

            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $httpBackend.whenGET("").respond(() => { return {}; });

                scope = $rootScope.$new();
                scope['property'] = new Models.Business.Property(propertyMock);
                element = $compile('<property-view property="property"></property-view>')(scope);

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
                expect(contactNames).toEqual(["Julie Lerman", "Ian Cooper"]);

                var ownershipTypeCode = getValueFromElement(ownershipPanel, 'ownership-list-ownership-type-code');
                expect(ownershipTypeCode).toBe(Models.Enums.OwnershipTypeEnum.Leaseholder);

                var purchaseDate = getValueFromElement(ownershipPanel, 'ownership-list-purchase-date');
                expect(purchaseDate).toBe('01-03-2016');

                var sellDate = getValueFromElement(ownershipPanel, 'ownership-list-sell-date');
                expect(sellDate).toBe('01-04-2016');

                var purchasePrice = getValueFromElement(ownershipPanel, 'ownership-list-purchase-price');
                expect(purchasePrice).toBe('4000');

                var sellPrice = getValueFromElement(ownershipPanel, 'ownership-list-sell-price');
                expect(sellPrice).toBe('8000');
            });

            function getValueFromElement(ownershipPanel: ng.IAugmentedJQuery, fieldId: string): string{
                return ownershipPanel.find('#' + fieldId).html();
            }
        });



    });
}