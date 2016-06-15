/// <reference path="../../../typings/_all.d.ts" />

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
                createdDate: '#activity-preview-created-date',
                status: '#activity-preview-status',
                vendors: '#activity-preview-vendors [id^=activity-preview-vendor-item-]',
                cardListActivities: '#card-list-activities'
            },
            areaBreakdown: {
                addBtn: '#card-list-areas button#addItemBtn',
                itemCard: (id: string) => { return '#card-list-areas #area-card-' + id; },
                editBtn: (id: string) => { return '#card-list-areas #area-card-' + id + ' context-menu-item[type="edit"] a'; },
                areaBreakdownSection: 'section#areaBreakdownSection',
                noItems: 'card-list#card-list-areas card-list-no-items',
                areaBreakdownItems: 'section#areaBreakdownSection card-list-items card[data-type-area-breakdown]'
            }
        }

        var userMock = { name: "user", email: "user@gmail.com", country: "GB", division: { id: "0acc9d28-51fa-e511-828b-8cdcd42baca7", value: "Commercial", code: "Commercial" }, divisionCode: <any>null, roles: ["admin", "superuser"] };

        var controller: PropertyViewController;

        describe('and property is loaded', () => {
            var propertyMock = TestHelpers.PropertyGenerator.generatePropertyView({
                id: '1',
                propertyTypeId: 'propType1',
                activities: TestHelpers.ActivityGenerator.generateManyDtos(2)
            });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                setUpBaseHttpMocks($http);

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
            var propertyMock = TestHelpers.PropertyGenerator.generatePropertyView();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;
                filter = $filter;

                setUpBaseHttpMocks($http);

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
                var cardListElement = element.find(pageObjectSelectors.activity.cardListActivities);

                var noItemsElement = cardListElement.find('card-list-no-items');
                var cardElements = cardListElement.find('card');
                expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(cardElements.length).toBe(0);
            });

            it('when existing activities then card components should be visible', () => {
                // arrange
                propertyMock.activities = TestHelpers.ActivityGenerator.generateMany(2);
                scope['property'] = propertyMock;

                // act
                element = compile('<property-view property="property" user-data="userData"></property-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var cardListElement = element.find(pageObjectSelectors.activity.cardListActivities);

                var noItemsElement = cardListElement.find('card-list-no-items');
                var cardElements = cardListElement.find('card');
                expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(cardElements.length).toBe(2);
            });

            it('when existing activities then card components should have proper data', () => {
                // arrange
                var date1Mock = new Date('2011-01-01');
                var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate({
                    id: 'It1',
                    propertyId: '1',
                    activityStatusId: '123',
                    activityTypeId: '123',
                    contacts: [
                        <Dto.IContact>{ id: 'Contact1', firstName: 'John', surname: 'Test1', title: 'Mr' },
                        <Dto.IContact>{ id: 'Contact2', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }
                    ]
                });
                activityMock.createdDate = date1Mock;
                propertyMock.activities = [activityMock];

                scope['property'] = propertyMock;

                // act
                element = compile('<property-view property="property" user-data="userData"></property-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var cardListElement = element.find(pageObjectSelectors.activity.cardListActivities);

                var cardElement = cardListElement.find('card#activity-card-It1');
                var activityDataElement = cardElement.find('[id="activity-data-It1"]');
                var activityStatusElement = cardElement.find('[id="activity-status-It1"]');

                var formattedDate = filter('date')(date1Mock, 'dd-MM-yyyy');
                expect(activityDataElement.text()).toBe(formattedDate + ' John Test1, Amy Test2');
                expect(activityStatusElement.text()).toBe('DYNAMICTRANSLATIONS.123');
            });
        });

        describe('and add activity button is clicked', () => {
            var activityTypes: any =
                [
                    { id: "1", order: 1 },
                    { id: "2", order: 2 },
                    { id: "3", order: 3 }
                ];

            var activityStatuses = [
                { id: "111", code: "PreAppraisal" },
                { id: "testStatus222", code: "MarketAppraisal" },
                { id: "333", code: "NotSelling" }
            ];

            var propertyMock = TestHelpers.PropertyGenerator.generatePropertyView();
            var defaultActivityStatus = _.find(activityStatuses, { 'code': 'PreAppraisal' });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                enumService: Mock.EnumServiceMock,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                // enums
                enumService.setEnum(Dto.EnumTypeCode.ActivityStatus.toString(), activityStatuses);

                // activity type http mock
                var query = '/api/activities/types?countryCode=GB&propertyTypeId=' + propertyMock.propertyTypeId;
                $http.whenGET(query).respond(() => {
                    return [200, [
                        <Dto.IActivityTypeQueryResult>{ id: '1', order: 1 },
                        <Dto.IActivityTypeQueryResult>{ id: '2', order: 2 },
                        <Dto.IActivityTypeQueryResult>{ id: '3', order: 3 }
                    ]
                    ];
                });

                setUpBaseHttpMocks($http);

                scope = $rootScope.$new();
                scope['userData'] = userMock;
                scope['property'] = propertyMock;
                compile = $compile;

                // act
                element = compile('<property-view property="property" user-data="userData"></property-view>')(scope);
                scope.$apply();
                $http.flush();

                var addActivityButton = element.find('#card-list-activities #addItemBtn');
                addActivityButton.click();
            }));

            it('default values are set', () => {
                // assert
                var activityAddController = <Activity.ActivityAddController>element.find('activity-add').controller('activityAdd');

                expect(activityAddController.selectedActivityStatusId).toBe(defaultActivityStatus.id);
                expect(activityAddController.selectedActivityType).toBe(null);
            });

            describe('when values are changed and activity add window opened again', () => {
                beforeEach(() => {
                    var addActivityPanel = element.find('activity-add');
                    var activityStatusSelect = addActivityPanel.find('#addActivityForm select[name="status"]');
                    var activityTypeSelect = addActivityPanel.find('#addActivityForm select[name="type"]');
                    activityStatusSelect.val(1)
                    activityTypeSelect.val(1);

                    var closePanelButton = addActivityPanel.find('.close-side-panel');
                    closePanelButton.click();

                    var addActivityButton = element.find('#card-list-activities #addItemBtn');
                    addActivityButton.click();
                })

                it('default values are set', () => {
                    // assert
                    var activityAddController = <Activity.ActivityAddController>element.find('activity-add').controller('activityAdd');

                    expect(activityAddController.selectedActivityStatusId).toBe(defaultActivityStatus.id);
                    expect(activityAddController.selectedActivityType).toBe(null);
                });
            });

            describe('and "Save button" is clicked ', () => {
                it('then new activity should be added to property activity list', () => {
                    $http.expectPOST(/\/api\/latestviews/, () => {
                        return true;
                    }).respond(200, []);

                    var activityAddController: Activity.ActivityAddController = element.find('activity-add').controller('activityAdd');
                    activityAddController.activityStatuses = activityStatuses;
                    activityAddController.activityTypes = activityTypes;
                    activityAddController.selectedActivityStatusId = _.find(activityStatuses, { 'code': 'PreAppraisal' }).id;
                    activityAddController.selectedActivityType = _.find(activityTypes, { id: "1" });

                    var expectedResponse = new Business.Activity();
                    expectedResponse.propertyId = propertyMock.id;
                    expectedResponse.activityStatusId = activityAddController.selectedActivityStatusId;
                    expectedResponse.activityTypeId = activityAddController.selectedActivityType.id;
                    expectedResponse.contacts = [];
                    expectedResponse.createdDate = new Date('2016-02-03');
                    expectedResponse.id = '123';

                    scope.$apply();

                    $http.whenPOST(/\/api\/activities/).respond(201, expectedResponse);

                    element.find('#activity-add-button').click();
                    $http.flush();

                    var activitiesList = element.find('#card-list-activities');
                    var activityListItems = activitiesList.find('card');

                    expect(propertyMock.activities.filter((item) => { return item.id === '123' }).length).toBe(1);
                    expect(activityListItems.length).toBe(1);
                });
            });
        });

        describe('and activity details is clicked', () => {
            var date1Mock = new Date('2011-01-01');
            var date2Mock = new Date('2011-01-05');
            var activity1Mock: Business.Activity = TestHelpers.ActivityGenerator.generate({
                id: 'It1', propertyId: '1', activityStatusId: '123', activityTypeId: '123'
            });
            activity1Mock.createdDate = date1Mock;

            var activity2Mock: Business.Activity = TestHelpers.ActivityGenerator.generate({
                id: 'It2',
                propertyId: '1',
                activityStatusId: '456',
                activityTypeId: '456',
                contacts: [
                    <Dto.IContact>{ id: 'Contact1', firstName: 'John', surname: 'Test1', title: 'Mr' },
                    <Dto.IContact>{ id: 'Contact2', firstName: 'Amy', surname: 'Test2', title: 'Mrs' }
                ]
            });
            activity2Mock.createdDate = date2Mock;

            var propertyMock = TestHelpers.PropertyGenerator.generateDto({
                id: '1',
                propertyTypeId: '1',
                activities: [activity1Mock, activity2Mock]
            }
            );

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                setUpBaseHttpMocks($http);

                scope = $rootScope.$new();
                scope['property'] = propertyMock;
                scope['userData'] = userMock;
                element = $compile('<property-view property="property" user-data="userData"></property-view>')(scope);

                scope.$apply();
                $httpBackend.flush();
            }));

            it('then activity details are visible on activity preview panel', () => {
                // arrange
                $http.expectPOST(/\/api\/latestviews/, () =>{
                    return true;
                }).respond(200, []);

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
            var propertyMock = TestHelpers.PropertyGenerator.generateDto(
                {
                    id: '1',
                    propertyTypeId: '1',
                    divisionId: '',
                    division: null,
                    address: Mock.AddressForm.FullAddress,
                    ownerships: [],
                    activities: [],
                    attributeValues: []
                });

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
            var createDateAsUtc = Core.DateTimeUtils.createDateAsUtc;

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

            var propertyMock = TestHelpers.PropertyGenerator.generateDto({
                ownerships: ownerships
            });

            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $httpBackend.whenGET("").respond(() => { return {}; });

                scope = $rootScope.$new();
                scope['property'] = new Business.PropertyView(propertyMock);
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

            function getValueFromElement(ownershipPanel: ng.IAugmentedJQuery, fieldId: string): string {
                return ownershipPanel.find('#' + fieldId).html();
            }
        });

        describe('and area breakdown list is loaded', () => {
            var propertyMock = TestHelpers.PropertyGenerator.generateDto();
            var assertValidator: TestHelpers.AssertValidators;

            beforeEach(inject(($rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $httpBackend.whenGET("").respond(() => { return {}; });

                compile = $compile;
                scope = $rootScope.$new();
                scope['property'] = new Business.PropertyView(propertyMock);
                scope['userData'] = userMock;
                element = $compile('<property-view property="property" user-data="userData"></property-view>')(scope);

                scope.$apply();
                $httpBackend.flush();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('when property is not commercial then area breakdown section is not rendered', () => {
                propertyMock = TestHelpers.PropertyGenerator.generateDto();
                scope['property'] = new Business.PropertyView(propertyMock);
                element = compile('<property-view property="property" user-data="userData"></property-view>')(scope);
                scope.$apply();

                var areaBreakdownSection = element.find(pageObjectSelectors.areaBreakdown.areaBreakdownSection);

                expect(areaBreakdownSection.length).toBe(0);
            });

            describe('when property is commercial', () => {
                beforeEach(() => {
                    propertyMock = TestHelpers.PropertyGenerator.generateDto({ division: { code: Dto.DivisionEnumTypeCode.Commercial } });
                    scope['property'] = new Business.PropertyView(propertyMock);
                    element = compile('<property-view property="property" user-data="userData"></property-view>')(scope);
                    scope.$apply();

                    controller = element.controller('propertyView');
                });

                it('when area breakdown list is empty then no items message is visible', () => {
                    assertValidator.assertElementHasHideClass(false, pageObjectSelectors.areaBreakdown.noItems);
                });

                it('then area breakdown list should be ordered by order property', () => {
                    var areaBreakdownList = TestHelpers.PropertyAreaBreakdownGenerator.generateMany(5);
                    for (var i = 0; i < areaBreakdownList.length; i++) {
                        areaBreakdownList[i].order = i;
                    }

                    scope['property'].propertyAreaBreakdowns = areaBreakdownList;
                    scope.$apply();
                    var cards:any = element.find(pageObjectSelectors.areaBreakdown.areaBreakdownItems);

                    var isOrdered: boolean = _.every(cards, (item: any, index: number): boolean => {
                        var sourceElement: string = pageObjectSelectors.areaBreakdown.itemCard(areaBreakdownList[index].id);

                        return item.getAttribute('id') === sourceElement;
                    });

                    expect(isOrdered).toBeTruthy();
                });

                it('when add area breakdown button is clicked then showAreaAdd should be called', () => {
                    // arrange
                    spyOn(controller, 'showAreaAdd');
                    scope.$apply();

                    // act
                    var button = element.find(pageObjectSelectors.areaBreakdown.addBtn);
                    button.click();
                    scope.$apply();

                    // assert
                    expect(controller.showAreaAdd).toHaveBeenCalled();
                });

                it('when edit area breakdown button is clicked then showAreaEdit should be called', () => {
                    // arrange
                    var areas: Business.PropertyAreaBreakdown[] = TestHelpers.PropertyAreaBreakdownGenerator.generateMany(3);

                    spyOn(controller, 'showAreaEdit');

                    controller.property.propertyAreaBreakdowns = areas;
                    scope.$apply();

                    // act
                    var button = element.find(pageObjectSelectors.areaBreakdown.editBtn(areas[1].id));
                    button.click();
                    scope.$apply();

                    // assert
                    expect(controller.showAreaEdit).toHaveBeenCalledWith(areas[1]);
                });
            });
        });
    });

    function setUpBaseHttpMocks($http: ng.IHttpBackendService): void {
        Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);
        $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
            return [];
        });

        $http.whenGET(/\/api\/properties\/attributes/).respond(() => {
            return [200, { attributes: [] }];
        });

        $http.whenGET(/\/api\/activities\/types/).respond(() => {
            return [200, []];
        });

        $http.whenGET(/\/api\/characteristicGroups/).respond(() => {
            return [200, []];
        });
    }
}