/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityViewController = Activity.View.ActivityViewController;
    import PropertyPreviewController = Property.Preview.PropertyPreviewController;
    import Business = Common.Models.Business;
    import IActivityViewConfig = Activity.IActivityViewConfig;
    declare var moment: any;

    describe('Given view activity page is loaded', () => {
        beforeEach(() => {
            angular.mock.module(($provide: any) => {
                $provide.service('addressFormsProvider', Mock.AddressFormsProviderMock);
            });
        });

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

        xdescribe('when activity is loaded', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();
            var configMock = { vendors: {}, departments: {} } as IActivityViewConfig;

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

                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                scope['activity'] = activityMock;
                scope['config'] = configMock;
                element = $compile('<activity-view activity="activity" config="config"></activity-view>')(scope);

                scope.$apply();

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

            it('and activity departments list is displayed and sorted', () => {
                // assert
                var departmentsSectionElement = element.find(pageObjectSelectors.departments.departmentsSection);
                var departmentItems = departmentsSectionElement.find(pageObjectSelectors.departments.departmentItem);

                expect(departmentItems.length).toBe(3);
                expect(departmentItems.first().find('.department-status').length).toBe(1);
            });
        });

        xdescribe('and property is loaded', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate({ contacts: [] });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;

                scope['activity'] = activityMock;
                element = compile('<activity-view activity="activity"></activity-view>')(scope);

                scope.$apply();
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

        describe('and viewings are loaded', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

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

                var viewingItem = element.find(pageObjectSelectors.viewings.item).first();

                contactsMock.forEach((c: Business.Contact) => {
                    expect(viewingItem.text()).toContain(c.getName());
                });
            });
        });
    });
}