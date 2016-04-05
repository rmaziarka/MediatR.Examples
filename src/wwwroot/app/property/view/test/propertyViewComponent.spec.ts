/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import PropertyViewController = Property.View.PropertyViewController;
    import IProperty = Antares.Common.Models.Dto.IProperty;
    import ContactListController = Antares.Component.ContactListController;
    import Dto = Antares.Common.Models.Dto;
    describe('Given view property page is loaded', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService;

        var controller: PropertyViewController;

        describe('and proper property id is provided', () =>{
            var propertyMock: IProperty = {
                id : '1',
                address : Antares.Mock.AddressForm.FullAddress,
                ownerships : [],
                activities : [
                    new Dto.Activity(<Dto.IActivity>{ id: 'It1', propertyId: '1', activityStatusId: '123', contacts:[] }),
                    new Dto.Activity(<Dto.IActivity>{ id: 'It2', propertyId: '1', activityStatusId: '1234', contacts: [] })
                ]
            };

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
                scope['property'] = propertyMock;
                element = $compile('<property-view property="property"></property-view>')(scope);

                scope.$apply();
                $http.flush();

                controller = element.controller('propertyView');
            }));

            it('card-list component for activities is set up', () =>{
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

            it('card components for activities are set up', () =>{
                var cardListElement = element.find('card-list#card-list-activities'),
                    cardListItemElement = cardListElement.find('card-list-item'),
                    cardListItemCardElement = cardListItemElement.find('card');

                expect(cardListItemElement.length).toBe(2);
                expect(cardListItemCardElement.length).toBe(2);
                expect(cardListItemCardElement[0].getAttribute('card-template-url')).toBe("'app/activity/templates/activityCard.html'");
                expect(cardListItemCardElement[0].getAttribute('show-item-details')).toBe("vm.showActivityPreview");
            });
        });

        describe('and activities are loaded', () =>{
            var propertyMock: IProperty = {
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

            it('when no activities then "no items" element should be visible', () =>{
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
            var propertyMock: IProperty = {
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
                    new Dto.Contact(<Dto.IContact>{firstName: 'Vernon', surname:'Vaughn',title: 'Mr'}),
                    new Dto.Contact(<Dto.IContact>{firstName: 'Julie', surname:'Lerman',title: 'Mrs'}),
                    new Dto.Contact(<Dto.IContact>{firstName: 'Mark', surname:'Rendle',title: 'Mr'})
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

    });
}