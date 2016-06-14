/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityViewController = Activity.View.ActivityViewController;
    import AttachmentPreviewController = Common.Component.AttachmentPreviewController;
    import PropertyPreviewController = Property.Preview.PropertyPreviewController;
    import Business = Common.Models.Business;

    declare var moment: any;

    describe('Given view activity page is loaded', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            filter: ng.IFilterService,
            $http: ng.IHttpBackendService,
            controller: ActivityViewController;

        var pageObjectSelectors = {
            common: {
                address: 'address-form-view',
                detailsLink: 'a'
            },
            well: {
                main: '#activity-view-well',
                createdDate: '#activity-view-well #createdDate',
                status: '#activity-view-well #activityStatus',
                type: '#activity-view-well #activityType'
            },
            prices: {
                main: '#activity-view-prices',
                marketAppraisalPrice: '#activity-view-prices #marketAppraisalPrice',
                recommendedPrice: '#activity-view-prices #recommendedPrice',
                vendorEstimatedPrice: '#activity-view-prices #vendorEstimatedPrice'
            },
            property: {
                card: '#activity-view-property card#card-property'
            },
            vendorList: {
                list: '#activity-view-vendors list#list-vendors',
                header: '#activity-view-vendors list#list-vendors list-header',
                noItems: '#activity-view-vendors list#list-vendors list-no-items',
                items: '#activity-view-vendors list#list-vendors list-item',
                item: '#activity-view-vendors list#list-vendors list-item#list-item-'
            },
            attachments: {
                list: 'card-list#card-list-attachments'
            },
            propertyPreview: {
                main: 'property-preview',
                addressSection: '#property-preview-address'
            },
            viewings: {
                list: '#activity-view-viewings card-list#viewings-list',
                noItems: '#activity-view-viewings card-list#viewings-list card-list-no-items',
                group: '#activity-view-viewings card-list-group.viewing-group',
                groupTitle: '#activity-view-viewings card-list-group.viewing-group card-list-group-header h5 time',
                item: '#activity-view-viewings card-list-group.viewing-group card-list-group-item card.viewing-item',
            },
            departments: {
                departmentsSection: '#departments-section',
                departmentItem: '.department-item'
            }
        };

        describe('when activity is loaded', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();

            activityMock.activityDepartments = TestHelpers.ActivityDepartmentGenerator.generateMany(3);
            activityMock.activityDepartments.forEach((activityDepartment, index) => {

                switch (index) {
                    case 1:
                        activityDepartment.departmentTypeId = '1';
                        activityDepartment.departmentType.id = '1';
                        activityDepartment.departmentType.code = 'Managing';
                        return;
                    default:
                        activityDepartment.departmentTypeId = '2';
                        activityDepartment.departmentType.id = '2';
                        activityDepartment.departmentType.code = 'Standard'
                }

                activityDepartment
            });

            beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) => {
                $provide.service('enumService', Antares.Mock.EnumServiceMock);
            }));

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService,
                enumService: Antares.Mock.EnumServiceMock) => {

                $http = $httpBackend;
                filter = $filter;
                var enumItems = [
                    { id: '1', code: 'Managing' },
                    { id: '2', code: 'Standard' }
                ];

                enumService.setEnum('ActivityDepartmentType',enumItems);

                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);
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

            it('then card component for property is set up', () => {
                // assert
                var cardElement = element.find(pageObjectSelectors.property.card);

                expect(cardElement.length).toBe(1);
                expect(cardElement[0].getAttribute('item')).toBe("avvm.activity.property");
                expect(cardElement[0].getAttribute('show-item-details')).toBe("avvm.showPropertyPreview");
            });

            it('then list component for vendors is set up', () => {
                // assert
                var listElement = element.find(pageObjectSelectors.vendorList.list),
                    listHeaderElement = element.find(pageObjectSelectors.vendorList.header),
                    listHeaderElementContent = listHeaderElement.find('[translate="ACTIVITY.VIEW.VENDORS"]'),
                    listNoItemsElement = element.find(pageObjectSelectors.vendorList.noItems),
                    listNoItemsElementContent = listNoItemsElement.find('[translate="ACTIVITY.VIEW.NO_VENDORS"]');

                expect(listElement.length).toBe(1);
                expect(listHeaderElement.length).toBe(1);
                expect(listHeaderElementContent.length).toBe(1);
                expect(listNoItemsElement.length).toBe(1);
                expect(listNoItemsElementContent.length).toBe(1);
            });

            xit('card-list component for attachments is set up', () => {
                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListHeaderElement = cardListElement.find('card-list-header'),
                    cardListHeaderElementContent = cardListHeaderElement.find('[translate="ACTIVITY.VIEW.ATTACHMENTS"]'),
                    cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                    cardListNoItemsElementContent = cardListNoItemsElement.find('[translate="ACTIVITY.VIEW.NO_ATTACHMENTS"]');

                expect(cardListElement.length).toBe(1);
                expect(cardListElement[0].getAttribute('show-item-add')).toBe("avvm.showActivityAttachmentAdd");
                expect(cardListHeaderElement.length).toBe(1);
                expect(cardListHeaderElementContent.length).toBe(1);
                expect(cardListNoItemsElement.length).toBe(1);
                expect(cardListNoItemsElementContent.length).toBe(1);
            });

            it('then activity summary component for activity is displayed and should have proper data', () => {
                // assert
                var wellElement = element.find(pageObjectSelectors.well.main);
                var createdDateElement = element.find(pageObjectSelectors.well.createdDate);
                var activityStatusElement = element.find(pageObjectSelectors.well.status);
                var activityTypeElement = element.find(pageObjectSelectors.well.type);

                var formattedDate = filter('date')(activityMock.createdDate, 'dd-MM-yyyy');
                expect(wellElement.length).toBe(1);
                expect(createdDateElement.length).toBe(1);
                expect(createdDateElement.text()).toBe(formattedDate);
                expect(activityStatusElement.length).toBe(1);
                expect(activityStatusElement.text()).toBe('DYNAMICTRANSLATIONS.' + activityMock.activityStatusId);
                expect(activityTypeElement.length).toBe(1);
                expect(activityTypeElement.text()).toBe('DYNAMICTRANSLATIONS.' + activityMock.activityTypeId);
            });

            it('and valuation prices are null then valuation prices for activity are not displayed', () => {
                // arrange / act
                controller.activity.marketAppraisalPrice = null;
                controller.activity.recommendedPrice = null;
                controller.activity.vendorEstimatedPrice = null;

                // assert
                var pricesElement = element.find(pageObjectSelectors.prices.main);
                var marketAppraisalPriceElement = element.find(pageObjectSelectors.prices.marketAppraisalPrice);
                var recomendedPriceElement = element.find(pageObjectSelectors.prices.recommendedPrice);
                var vendorEstimatedPriceElement = element.find(pageObjectSelectors.prices.vendorEstimatedPrice);

                expect(pricesElement.length).toBe(1);
                expect(marketAppraisalPriceElement.length).toBe(0);
                expect(recomendedPriceElement.length).toBe(0);
                expect(vendorEstimatedPriceElement.length).toBe(0);
            });

            it('and valuation prices are not null then valuation prices for activity are displayed and should have proper data', () => {
                // arrange / act
                controller.activity.marketAppraisalPrice = 99;
                controller.activity.recommendedPrice = 1.1;
                controller.activity.vendorEstimatedPrice = 55.05;
                scope.$apply();

                // assert
                var pricesElement = element.find(pageObjectSelectors.prices.main);
                var marketAppraisalPriceElement = element.find(pageObjectSelectors.prices.marketAppraisalPrice);
                var recomendedPriceElement = element.find(pageObjectSelectors.prices.recommendedPrice);
                var vendorEstimatedPriceElement = element.find(pageObjectSelectors.prices.vendorEstimatedPrice);

                expect(pricesElement.length).toBe(1);
                expect(marketAppraisalPriceElement.length).toBe(1);
                expect(marketAppraisalPriceElement.text()).toBe('99.00 GBP');
                expect(recomendedPriceElement.length).toBe(1);
                expect(recomendedPriceElement.text()).toBe('1.10 GBP');
                expect(vendorEstimatedPriceElement.length).toBe(1);
                expect(vendorEstimatedPriceElement.text()).toBe('55.05 GBP');
            });

            it('and activity departments list is displayed and sorted', () => {
                // assert
                var departmentsSectionElement = element.find(pageObjectSelectors.departments.departmentsSection);
                var departmentItems = departmentsSectionElement.find(pageObjectSelectors.departments.departmentItem);

                expect(departmentItems.length).toBe(3);
                expect(departmentItems.first().find('.department-status').length).toBe(1);
            });
        });

        describe('and vendors are loaded', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when no vendors then "no items" element should be visible', () => {
                // arrange
                var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate({ contacts: [] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var noItemsElement = element.find(pageObjectSelectors.vendorList.noItems);
                var listItemElements = element.find(pageObjectSelectors.vendorList.items);

                expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(listItemElements.length).toBe(0);
            });

            it('when existing vendors then list item components should be visible', () => {
                // arrange
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();
                var activityMock = TestHelpers.ActivityGenerator.generate({ contacts: [contact1Mock, contact2Mock] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var noItemsElement = element.find(pageObjectSelectors.vendorList.noItems);
                var listItemElements = element.find(pageObjectSelectors.vendorList.items);

                expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(listItemElements.length).toBe(2);
            });

            it('when existing vendors then list item components should have proper data', () => {
                // arrange
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();
                var activityMock = TestHelpers.ActivityGenerator.generate({ contacts: [contact1Mock, contact2Mock] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var vendorsItemsElement1 = element.find(pageObjectSelectors.vendorList.item + contact1Mock.id);
                var vendorsItemsElement2 = element.find(pageObjectSelectors.vendorList.item + contact2Mock.id);

                expect(vendorsItemsElement1[0].innerText.trim()).toBe(contact1Mock.getName());
                expect(vendorsItemsElement2[0].innerText.trim()).toBe(contact2Mock.getName());
            });
        });

        describe('and property is loaded', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate({ contacts: [] });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);
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

            it('when existing property then property card should have address element visible', () => {
                // assert
                var cardElement = element.find(pageObjectSelectors.property.card);
                var addressElement = cardElement.find(pageObjectSelectors.common.address);

                expect(addressElement.length).toBe(1);
                expect(addressElement[0].getAttribute('address')).toBe("cvm.item.address");
            });

            xdescribe('and property details is clicked', () => {
                it('then property details are set in property preview component', () => {
                    // arrange
                    activityMock.property = TestHelpers.PropertyGenerator.generate();
                    scope.$apply();

                    var propertyPreviewController: PropertyPreviewController = element.find('property-preview').controller('propertyPreview');

                    spyOn(propertyPreviewController, 'setProperty');

                    $http.expectPOST(/\/api\/latestviews/, () => {
                        return true;
                    }).respond(200, []);

                    // act
                    var cardElement = element.find(pageObjectSelectors.property.card);
                    cardElement.find(pageObjectSelectors.common.detailsLink).click();

                    // assert
                    expect(propertyPreviewController.setProperty).toHaveBeenCalledWith(activityMock.property);
                });
            });
        });

        xdescribe('and attachments are loaded', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();

            var fileSizeFilter = Mock.FileSize.generate();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService) => {

                filter = $filter;
                $http = $httpBackend;

                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);

                scope = $rootScope.$new();
                compile = $compile;

                scope['activity'] = activityMock;
                element = compile('<activity-view activity="activity"></activity-view>')(scope);

                scope.$apply();
                $http.flush();
            }));

            it('when no attachments then "no items" element should be visible', () => {
                // arrange / act
                activityMock.attachments = [];
                scope.$apply();

                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                    cardListNoItemsElementContent = cardListNoItemsElement.find('[translate="ACTIVITY.VIEW.NO_ATTACHMENTS"]'),
                    cardListItemElement = cardListElement.find('card-list-item');

                expect(cardListNoItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(cardListNoItemsElementContent.length).toBe(1);
                expect(cardListItemElement.length).toBe(0);
            });

            it('when existing attachments then card components should be visible', () => {
                // arrange / act
                activityMock.attachments = TestHelpers.AttachmentGenerator.generateMany(2);
                scope.$apply();

                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListNoItemsElement = cardListElement.find('card-list-no-items'),
                    cardListItemElement = cardListElement.find('card-list-item'),
                    cardListItemCardElement = cardListItemElement.find('card');

                expect(cardListNoItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(cardListItemElement.length).toBe(2);
                expect(cardListItemCardElement.length).toBe(2);

            });

            it('when existing attachments then card components should have proper order', () => {
                // arrange / act
                var date1Mock = new Date('2016-01-12');
                var date2Mock = new Date('2016-01-18');
                activityMock.attachments = TestHelpers.AttachmentGenerator.generateMany(2);
                activityMock.attachments[0].createdDate = date1Mock;
                activityMock.attachments[1].createdDate = date2Mock;
                scope.$apply();

                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListItemCardElements = cardListElement.find('card-list-item card'),
                    cardListItem1CardElement = cardListElement.find('card[id="attachment-card-' + activityMock.attachments[0].id + '"]'),
                    cardListItem2CardElement = cardListElement.find('card[id="attachment-card-' + activityMock.attachments[1].id + '"]');

                expect(cardListItemCardElements[0]).toBe(cardListItem2CardElement[0]);
                expect(cardListItemCardElements[1]).toBe(cardListItem1CardElement[0]);
            });

            it('when existing attachments then card components should have proper data', () => {
                // arrange / act
                var dateMock = new Date('2011-01-01');
                var attachmentMock = TestHelpers.AttachmentGenerator.generate({ user: new Business.User({ id: 'us1', firstName: 'firstName1', lastName: 'lastName1', departmentId: 'depId', department: null }) });
                attachmentMock.createdDate = dateMock;

                activityMock.attachments = [attachmentMock];
                scope.$apply();

                // assert
                var cardListElement = element.find(pageObjectSelectors.attachments.list),
                    cardListItemElement = cardListElement.find('card-list-item'),
                    cardListItemCardElement = cardListItemElement.find('card[id="attachment-card-' + attachmentMock.id + '"]');

                var attachmentDataElement = cardListItemCardElement.find('[id="attachment-data-' + attachmentMock.id + '"]');
                var attachmentDateElement = cardListItemCardElement.find('[id="attachment-created-date-' + attachmentMock.id + '"]');
                var attachmentTypeElement = cardListItemCardElement.find('[id="attachment-type-' + attachmentMock.id + '"]');
                var attachmentFileSizeElement = cardListItemCardElement.find('[id="attachment-file-size-' + attachmentMock.id + '"]');

                expect(attachmentDataElement.text()).toBe(attachmentMock.fileName);
                var formattedDate = filter('date')(dateMock, 'dd-MM-yyyy');
                expect(attachmentDateElement.text()).toBe(formattedDate);
                expect(attachmentTypeElement.text()).toBe('DYNAMICTRANSLATIONS.' + attachmentMock.documentTypeId);
                expect(fileSizeFilter).toHaveBeenCalledWith(attachmentMock.size);
            });

            xdescribe('and attachment details is clicked', () => {
                it('then attachment details are set in attachment preview component', () => {
                    // arrange
                    activityMock.attachments = TestHelpers.AttachmentGenerator.generateMany(4);
                    scope.$apply();

                    var attachmentForDetailsClick = activityMock.attachments[2];
                    var attachmentPreviewController: AttachmentPreviewController = element.find('attachment-preview').controller('attachmentPreview');

                    spyOn(attachmentPreviewController, 'setAttachment');

                    // act
                    var cardElement = element.find('card[id="attachment-card-' + attachmentForDetailsClick.id + '"]');
                    cardElement.find(pageObjectSelectors.common.detailsLink).click();

                    // assert
                    expect(attachmentPreviewController.setAttachment).toHaveBeenCalledWith(attachmentForDetailsClick, activityMock.id);
                });
            });
        });

        describe('and viewings are loaded', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when no viewings then "no items" element should be visible', () => {
                // arrange
                var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate({ viewings: [] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var noItemsElement = element.find(pageObjectSelectors.viewings.noItems);
                var groupElements = element.find(pageObjectSelectors.viewings.group);

                expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(groupElements.length).toBe(0);
            });

            it('when viewings exist then card list components should be visible', () => {
                // arrange
                var viewingsMock = TestHelpers.ViewingGenerator.generateMany(4);
                var activityMock = TestHelpers.ActivityGenerator.generate({ viewings: viewingsMock });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var noItemsElement = element.find(pageObjectSelectors.viewings.noItems);
                var listItemElements = element.find(pageObjectSelectors.viewings.item);

                expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(listItemElements.length).toBe(4);
            });

            it('viewing list is displayed and grouped correctly', () => {
                // arrange
                var viewingsMock = [
                    {
                        id: '1',
                        startDate: "2016-01-01T10:00:00Z",
                        endDate: "2016-01-01T11:00:00Z"
                    },
                    {
                        id: '2',
                        startDate: "2016-01-01T13:00:00Z",
                        endDate: "2016-01-01T14:00:00Z"
                    },
                    {
                        id: '3',
                        startDate: "2016-01-02T10:00:00Z",
                        endDate: "2016-01-02T11:00:00Z"
                    },
                    {
                        id: '4',
                        startDate: "2016-01-03T00:00:00Z",
                        endDate: "2016-01-03T01:00:00Z"
                    }
                ];
                var activityMock = TestHelpers.ActivityGenerator.generate({ viewings: viewingsMock });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var viewingGroups = element.find(pageObjectSelectors.viewings.group);
                var viewingGroupTitles = element.find(pageObjectSelectors.viewings.groupTitle);
                var viewingItems = element.find(pageObjectSelectors.viewings.item);

                expect(viewingGroups.length).toBe(activityMock.viewingsByDay.length);
                expect(viewingItems.length).toBe(activityMock.viewings.length);

                activityMock.viewingsByDay.sort((a, b) => new Date(b.day).getTime() - new Date(a.day).getTime()).forEach((g: Business.ViewingGroup, i: number) => {
                    expect(viewingGroupTitles[i].textContent).toBe(moment(g.day, 'YYYY-MM-DD').format('DD-MM-YYYY'));
                });
            });

            it('viewing list item contains all applicants', () => {
                // arrange
                var contactsMock = TestHelpers.ContactGenerator.generateMany(3);
                var requirementMock = TestHelpers.RequirementGenerator.generate({ contacts: contactsMock });
                var viewingsMock = TestHelpers.ViewingGenerator.generate({ requirement: requirementMock });
                var activityMock = TestHelpers.ActivityGenerator.generate({ viewings: [viewingsMock] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                var viewingItem = element.find(pageObjectSelectors.viewings.item).first();

                contactsMock.forEach((c: Business.Contact) => {
                    expect(viewingItem.text()).toContain(c.getName());
                });
            });
        });
    });
}