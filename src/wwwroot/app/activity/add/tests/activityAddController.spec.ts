/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityAddController = Activity.ActivityAddController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    import runDescribe = TestHelpers.runDescribe;

    describe('Given activity controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: ActivityAddController;

        var activitiesMock: Common.Models.Business.Activity[] = [];

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

        var createVendors = (count: number) => {
            return _.map(TestHelpers.ContactGenerator.generateMany(count), (dtoContact: Dto.IContact) => {
                return new Business.Contact(dtoContact);
            });
        }

        describe('when vendors are passed to component setVendors method', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: ng.IControllerService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                $http.expectGET(/\/api\/activities\/types/).respond(() => {
                    return [200, activityTypes];
                });

                controller = <ActivityAddController>$controller('ActivityAddController', { $scope: $scope });
                $http.flush();
            }));

            it('and are null then empty vendors array should be set ', () => {
                controller.setVendors(null);

                expect(controller.vendors.length).toBe(0);
            });

            it('and are array then vendors array should be set ', () => {
                var vendorsArray = createVendors(3);
                controller.setVendors(vendorsArray);

                expect(controller.vendors).toBe(vendorsArray);
            });
        });


        describe('when saveActivity is called', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                // 'any' must be used instead of 'ng.IControllerService' because there is invalid typing for this service function,
                // that sais that 3rd argument is bool, but in fact it is an object containing bindings for controller (for comonents and directives)
                $controller: any,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                $http.expectGET(/\/api\/activities\/types/).respond(() => {
                    return [200, activityTypes];
                });

                var bindings = { activities: activitiesMock };
                controller = <ActivityAddController>$controller('ActivityAddController', { $scope: $scope }, bindings);
                $http.flush();
            }));

            it('then proper data should be send to API', () => {
                // arrange
                spyOn(controller, 'isDataValid').and.returnValue(true);

                var propertyMockId = '123456';
                var vendors = createVendors(4);

                var requestData: Business.CreateActivityResource;

                controller.activityStatuses = activityStatuses;
                controller.activityTypes = activityTypes;
                controller.selectedActivityStatusId = _.find(activityStatuses, { 'code': 'PreAppraisal' }).id;
                controller.selectedActivityType = _.find(activityTypes, { id: "1" });
                controller.setVendors(vendors);

                var expectedRequest = new Business.CreateActivityResource();
                expectedRequest.propertyId = propertyMockId;
                expectedRequest.activityStatusId = controller.selectedActivityStatusId;
                expectedRequest.activityTypeId = controller.selectedActivityType.id;
                expectedRequest.contactIds = vendors.map((vendor: Dto.IContact) => { return vendor.id });

                $http.expectPOST(/\/api\/activities/, (data: string) => {
                    requestData = JSON.parse(data);

                    return true;
                }).respond(200, new Business.Activity());

                //act
                controller.saveActivity(propertyMockId);
                $http.flush();

                // assert
                expect(requestData.activityStatusId).toEqual(expectedRequest.activityStatusId);
                expect(requestData.propertyId).toEqual(expectedRequest.propertyId);
                expect(requestData.contactIds.length).toEqual(expectedRequest.contactIds.length);
                expect(angular.equals(requestData.contactIds, expectedRequest.contactIds)).toBe(true);
            });
        });
	});
}