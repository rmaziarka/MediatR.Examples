/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import ActivityEditController = Activity.ActivityEditController;
    import Enums = Common.Models.Enums;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given edit activity controller', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            $state: ng.ui.IStateService,
            eventAggregator: Antares.Core.EventAggregator,
            assertValidator: TestHelpers.AssertValidators,
            controller: ActivityEditController,
            $q: ng.IQService,
            $scope: ng.IScope,
            activityService: Activity.ActivityService,
            latestViewsProvider: Providers.LatestViewsProvider;

        var leadNegotiatorMock = TestHelpers.ActivityUserGenerator.generate(Enums.NegotiatorTypeEnum.LeadNegotiator);
        var secondaryNegotiatorsMock = TestHelpers.ActivityUserGenerator.generateMany(3, Enums.NegotiatorTypeEnum.SecondaryNegotiator);

        beforeEach(inject((
            _eventAggregator_: Antares.Core.EventAggregator,
            $rootScope: ng.IRootScopeService,
            $controller: ng.IControllerService,
            _$state_: ng.ui.IStateService,
            $httpBackend: ng.IHttpBackendService,
            _$q_: ng.IQService,
            _activityService_: Activity.ActivityService,
            _latestViewsProvider_: Providers.LatestViewsProvider) => {

            // init
            eventAggregator = _eventAggregator_;
            $scope = $rootScope.$new();
            $http = $httpBackend;
            $state = _$state_;
            activityService = _activityService_;
            $q = _$q_;
            latestViewsProvider = _latestViewsProvider_;

            $scope = $rootScope.$new();
            controller = <ActivityEditController>$controller('ActivityEditController', { $scope: $scope });
            controller.activity = new Business.Activity();
            controller.activity.leadNegotiator = leadNegotiatorMock;
            controller.activity.secondaryNegotiator = secondaryNegotiatorsMock;
        }));

        describe('when departmentIsRelatedWithNegotiator is called', () => {
            type TestCaseForDepartmentsCheck = [Business.ActivityDepartment, number, Business.ActivityDepartment, string, boolean]; //[leadNegotiatorDepartment, secondaryNegotiatorIndex, secondaryNegotiatorDepartment, message]
            var departmentToCheck = TestHelpers.ActivityDepartmentGenerator.generate();

            runDescribe('and when depertment is related to ')
                .data<TestCaseForDepartmentsCheck>([
                    [TestHelpers.ActivityDepartmentGenerator.generate(), 0, TestHelpers.ActivityDepartmentGenerator.generate(), 'no negotiator', false],
                    [departmentToCheck, 0, TestHelpers.ActivityDepartmentGenerator.generate(), 'lead negotiator', true],
                    [TestHelpers.ActivityDepartmentGenerator.generate(), 0, departmentToCheck, 'secondary negotiator 0', true],
                    [TestHelpers.ActivityDepartmentGenerator.generate(), 1, departmentToCheck, 'secondary negotiator 1', true]])
                .dataIt((data: TestCaseForDepartmentsCheck) =>
                    `${data[3]} then it should return ${data[4]}`)
                .run((data: TestCaseForDepartmentsCheck) => {
                    // arrange
                    controller.activity.activityDepartments.push(departmentToCheck);

                    controller.activity.leadNegotiator.user.departmentId = data[0].departmentId;
                    controller.activity.leadNegotiator.user.department = data[0].department;

                    controller.activity.secondaryNegotiator[data[1]].user.departmentId = data[2].departmentId;
                    controller.activity.secondaryNegotiator[data[1]].user.department = data[2].department;

                    // act / assert
                    expect(controller.departmentIsRelatedWithNegotiator(departmentToCheck.department)).toBe(data[4]);
                });
        });

        describe('when cancel', () => {
            it('then redrection to activity view page should be triggered', () => {
                // arrange
                controller.activity.id = TestHelpers.StringGenerator.generate();

                spyOn($state, 'go');

                // act
                controller.cancel();

                // assert
                expect($state.go).toHaveBeenCalledWith('app.activity-view', { id: controller.activity.id });
            });
        });

        describe('when save', () => {
            describe('and one new department is related with negotiator', () => {
                var newActivityDepartment1: Business.ActivityDepartment;
                var newActivityDepartment2: Business.ActivityDepartment;
                var newDepartment1 = TestHelpers.DepartmentGenerator.generate();
                var newDepartment2 = TestHelpers.DepartmentGenerator.generate();

                beforeEach(() => {
                    newActivityDepartment1 = TestHelpers.ActivityDepartmentGenerator.generate({ id: null, department: newDepartment1 });
                    newActivityDepartment2 = TestHelpers.ActivityDepartmentGenerator.generate({ id: null, department: newDepartment2 });

                    controller.activity.activityDepartments.push(newActivityDepartment1);
                    controller.activity.activityDepartments.push(newActivityDepartment2);

                });

                it('then error message should be displayed', () => {
                    spyOn(controller, 'departmentIsRelatedWithNegotiator').and.callFake((department: Business.Department) => {
                        if (department === newDepartment1) {
                            return true;
                        }

                        return false;
                    });
                    spyOn(controller.kfMessageService, 'showErrorByCode');
                    
                    // act
                    controller.save();

                    // assert
                    expect(controller.kfMessageService.showErrorByCode).toHaveBeenCalled();
                });
            });

            describe('with valid data', () => {
                beforeEach(() => {
                    spyOn(controller, 'departmentIsRelatedWithNegotiator').and.returnValue(false);
                    spyOn($state, 'go').and.callFake(() => { });

                })

                describe('for edit mode', () => {
                    var deferred: ng.IDeferred<any>;
                    var activityFromService: Dto.IActivity;
                    var requestData: Antares.Activity.Commands.IActivityEditCommand;
                    var activity: Business.Activity;

                    beforeEach(() => {
                        // arrange
                        deferred = $q.defer();
                        activity = TestHelpers.ActivityGenerator.generate();
                        controller.activity = activity;

                        activityFromService = TestHelpers.ActivityGenerator.generateDto();

                        spyOn(activityService, 'updateActivity').and.callFake((activityCommand: Antares.Activity.Commands.IActivityEditCommand) => {
                            requestData = activityCommand;
                            return deferred.promise;
                        });
                    })

                    it('then correct data is sent to API', () => {
                        // act
                        controller.save();
                        deferred.resolve(activityFromService);
                        $scope.$apply();

                        // assert
                        expect(requestData.id).toEqual(activity.id);
                        expectCorrectRequest(requestData, activity);
                    });

                    it('then user is redirected to activity view page', () => {
                        // act
                        controller.save();
                        deferred.resolve(activityFromService);
                        $scope.$apply();

                        // assert
                        expect($state.go).toHaveBeenCalledWith('app.activity-view', { id: activityFromService.id });
                    });
                })

                describe('for add mode', () => {
                    var deferred: ng.IDeferred<any>;
                    var activityFromService: Dto.IActivity;
                    var requestData: Antares.Activity.Commands.IActivityAddCommand;
                    var activity: Business.Activity;

                    beforeEach(() => {
                        // arrange
                        deferred = $q.defer();
                        activity = TestHelpers.ActivityGenerator.generate();
                        activity.id = null;
                        controller.activity = activity;

                        activityFromService = TestHelpers.ActivityGenerator.generateDto();

                        spyOn(activityService, 'addActivity').and.callFake((activityCommand: Antares.Activity.Commands.IActivityAddCommand) => {
                            requestData = activityCommand;
                            return deferred.promise;
                        });

                        spyOn(latestViewsProvider, 'addView').and.callFake(() => { });
                    })

                    it('then correct data is sent to API', () => {
                        // act
                        controller.save();
                        deferred.resolve(activityFromService);
                        $scope.$apply();

                        // assert
                        expectCorrectRequest(requestData, activity);
                    });

                    it('then user is redirected to activity view page', () => {
                        // act
                        controller.save();
                        deferred.resolve(activityFromService);
                        $scope.$apply();

                        // assert
                        expect($state.go).toHaveBeenCalledWith('app.activity-view', { id: activityFromService.id });
                    });

                    it('then activity should be addded to latest view', () => {
                        // act
                        controller.save();
                        deferred.resolve(activityFromService);
                        $scope.$apply();

                        // assert
                        expect(latestViewsProvider.addView).toHaveBeenCalled();
                    });
                })

                var expectCorrectRequest = (requestData: Antares.Activity.Commands.IActivityBaseCommand, activity: Business.Activity) => {
                    expect(requestData.activityStatusId).toEqual(activity.activityStatusId);
                    expect(requestData.marketAppraisalPrice).toEqual(activity.marketAppraisalPrice);
                    expect(requestData.recommendedPrice).toEqual(activity.recommendedPrice);
                    expect(requestData.vendorEstimatedPrice).toEqual(activity.vendorEstimatedPrice);
                    expect(requestData.leadNegotiator.userId).toEqual(activity.leadNegotiator.userId);
                    expect(requestData.sellingReasonId).toEqual(activity.sellingReasonId);
                    expect(requestData.sourceId).toEqual(activity.sourceId);
                    expect(requestData.pitchningThreats).toEqual(activity.pitchningThreats);
                    expect(requestData.keyNumber).toEqual(activity.keyNumber);
                    expect(requestData.accessArrangements).toEqual(activity.accessArrangements);

                    expect(requestData.secondaryNegotiators.map((negotiator) => negotiator.userId)).toEqual(activity.secondaryNegotiator.map((negotiator) => negotiator.userId));
                }
            });
        });

        describe('when onNegotiatorAdded with new user', () => {

            it('then their department should NOT be added if already added', () => {
                // arrange
                var user: Dto.IUser = TestHelpers.UserGenerator.generateDto();
                var activityDepartment: Business.ActivityDepartment = TestHelpers.ActivityDepartmentGenerator.generate({ departmentId: user.department.id });

                controller.activity.activityDepartments = [activityDepartment];

                // act
                controller.onNegotiatorAdded(user);

                // assert
                expect(controller.activity.activityDepartments.length).toBe(1);
            });

            it('then their department should be added if NOT already added', () => {
                // arrange
                var user: Dto.IUser = TestHelpers.UserGenerator.generateDto();
                controller.activity.activityDepartments = [TestHelpers.ActivityDepartmentGenerator.generate()];
                controller.standardDepartmentType = TestHelpers.EnumTypeItemGenerator.generateDto();

                // act
                controller.onNegotiatorAdded(user);

                // assert
                expect(controller.activity.activityDepartments.length).toBe(2);

                var addedActivityDepartment = controller.activity.activityDepartments[1];

                expect(addedActivityDepartment.departmentId).toBe(user.department.id);
                expect(addedActivityDepartment.activityId).toBe(controller.activity.id);
                expect(addedActivityDepartment.departmentType).toBe(controller.standardDepartmentType);
                expect(addedActivityDepartment.departmentTypeId).toBe(controller.standardDepartmentType.id);
            });
        });

        describe('should subscribe to events', () => {
            it('when close side panel event then property view panel should be closed', () => {
                // arrange
                controller.isPropertyPreviewPanelVisible = true;

                // act
                eventAggregator.publish(new Common.Component.CloseSidePanelEvent());

                // assert
                expect(controller.isPropertyPreviewPanelVisible).toBeFalsy();
            });

            it('when open property prewiew panel event then property view panel should be opened', () => {
                // arrange
                controller.isPropertyPreviewPanelVisible = false;

                // act
                eventAggregator.publish(new Attributes.OpenPropertyPrewiewPanelEvent());

                // assert
                expect(controller.isPropertyPreviewPanelVisible).toBeTruthy();
            });
        });
    });
}