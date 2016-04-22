/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityAddController = Activity.ActivityAddController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    describe('Given activity is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            controller: ActivityAddController;

        var pageObjectSelectors = {
            activityStatusSelector: 'select#status',
            vendors: '#activity-add-vendors [id^=list-item-]'
        };

        var activityStatuses: any = {
            items:
            [
                { id: "1", value: "Pre-appraisal", code: "PreAppraisal" },
                { id: "2", value: "Market appraisal", code: "MarketAppraisal" },
                { id: "3", value: "Not selling", code: "NotSelling" }
            ]
        };

        var createVendors = (count: number) => {
            return _.map(TestHelpers.ContactGenerator.generateMany(count), (dtoContact: Dto.IContact) => {
                return new Business.Contact(dtoContact);
            });
        }

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            compile = $compile;

            $http.expectGET(/\/api\/enums\/ActivityStatus\/items/).respond(() => {
                return [200, activityStatuses];
            });

            scope = $rootScope.$new();
            element = compile('<activity-add></activity-add>')(scope);
            scope.$apply();
            $http.flush();

            controller = element.controller('activityAdd');

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }
        ));

        describe('when component is being loaded', () => {
            it('then default Pre-appraisal status is selected', () => {
                var expectedPreAppraisalStatus = _.find(activityStatuses.items, { 'code': 'PreAppraisal' });
                expect(controller.selectedActivityStatus).toEqual(expectedPreAppraisalStatus);
            });
        });

        describe('when vendors are passed to component setVendors method', () => {
            it('and are null then empty vendors array should be set ', () =>{
                controller.setVendors(null);

                expect(controller.vendors.length).toBe(0);
            });

            it('and are array then vendors array should be set ', () => {
                var vendorsArray = createVendors(3);
                controller.setVendors(vendorsArray);

                expect(controller.vendors).toBe(vendorsArray);
            });
        });

        describe('when vendors are set in component', () => {
            var setVendors = (vendorsCount: number): Business.Contact[] => {
                var vendors = createVendors(vendorsCount);

                controller.setVendors(vendors);
                scope.$apply();

                return vendors;
            }

            it('then all should be displayed', () => {
                var vendorsCount: number = 3;
                setVendors(vendorsCount);

                var elements: ng.IAugmentedJQuery = element.find(pageObjectSelectors.vendors);
                expect(elements.length).toBe(vendorsCount);
            });

            it('then his name should be displayed correctly', () => {
                var vendorsCount: number = 1;
                var vendor: Business.Contact = setVendors(vendorsCount)[0];

                var elements: ng.IAugmentedJQuery = element.find(pageObjectSelectors.vendors);
                expect(elements.length).toBe(1);

                var vendorElement: HTMLElement = elements[0];
                expect(vendorElement.innerText).toBe(vendor.getName());
            });
        });

        describe('when activity status value is ', () => {
            it('missing then required message should be displayed', () => {
                assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.activityStatusSelector);
            });

            it('not missing then required message should not be displayed', () => {
                assertValidator.assertRequiredValidator('2', true, pageObjectSelectors.activityStatusSelector);
            });
        });

        describe('when "Save button" is clicked ', () => {
            var propertyMock: Dto.IProperty;

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                $httpBackend.whenGET("").respond(() => { return {}; });

                scope = $rootScope.$new();
                propertyMock = TestHelpers.PropertyGenerator.generateDto();
                scope['property'] = propertyMock;
                element = $compile('<property-view property="property"></property-view>')(scope);

                scope.$apply();
                $http.flush();
            }
            ));

            it('then proper data should be send to API', () => {
                var vendors = createVendors(4);

                var requestData: Business.CreateActivityResource;

                var activityAddController: Activity.ActivityAddController = element.find('activity-add').controller('activityAdd');
                activityAddController.activityStatuses = activityStatuses.items;
                activityAddController.selectedActivityStatus = _.find(activityStatuses.items, { 'code': 'PreAppraisal' });
                activityAddController.setVendors(vendors);

                var expectedRequest = new Business.CreateActivityResource();
                expectedRequest.propertyId = propertyMock.id;
                expectedRequest.activityStatusId = activityAddController.selectedActivityStatus.id;
                expectedRequest.contactIds = vendors.map((vendor: Dto.IContact) => { return vendor.id });

                scope.$apply();

                $http.expectPOST(/\/api\/activities/, (data: string) => {
                    requestData = JSON.parse(data);

                    return true;
                }).respond(201, new Business.Activity());

                element.find('#activity-add-button').click();
                $http.flush();

                expect(requestData.activityStatusId).toEqual(expectedRequest.activityStatusId);
                expect(requestData.propertyId).toEqual(expectedRequest.propertyId);
                expect(requestData.contactIds.length).toEqual(expectedRequest.contactIds.length);
                expect(angular.equals(requestData.contactIds, expectedRequest.contactIds)).toBe(true);
            });

            it('then new activity should be added to property activity list', () => {
                var vendors = createVendors(2);

                var activityAddController: Activity.ActivityAddController = element.find('activity-add').controller('activityAdd');
                activityAddController.activityStatuses = activityStatuses.items;
                activityAddController.selectedActivityStatus = _.find(activityStatuses.items, { 'code': 'PreAppraisal' });
                activityAddController.setVendors(vendors);

                var expectedResponse = new Business.Activity();
                expectedResponse.propertyId = propertyMock.id;
                expectedResponse.activityStatusId = activityAddController.selectedActivityStatus.id;
                expectedResponse.contacts = vendors;
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
}