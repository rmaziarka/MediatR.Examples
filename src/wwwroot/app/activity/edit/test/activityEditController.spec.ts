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
            enumProvider: Providers.EnumProvider,
            latestViewsProvider: Providers.LatestViewsProvider;

        var leadNegotiatorMock = TestHelpers.ActivityUserGenerator.generate(Enums.NegotiatorTypeEnum.LeadNegotiator);
        var secondaryNegotiatorsMock = TestHelpers.ActivityUserGenerator.generateMany(3, Enums.NegotiatorTypeEnum.SecondaryNegotiator);

        beforeEach(inject((
            _eventAggregator_: Antares.Core.EventAggregator,
            $rootScope: ng.IRootScopeService,
            $controller: ng.IControllerService,
            _$state_: ng.ui.IStateService,
            $httpBackend: ng.IHttpBackendService,
            _enumProvider_: Providers.EnumProvider,
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
            enumProvider = _enumProvider_;

            $scope = $rootScope.$new();
            controller = <ActivityEditController>$controller('ActivityEditController', { $scope: $scope });
            controller.activity = new Activity.ActivityEditModel();
            controller.activity.leadNegotiator = leadNegotiatorMock;
            controller.activity.secondaryNegotiator = secondaryNegotiatorsMock;
            controller.config = TestHelpers.ConfigGenerator.generateActivityEditConfig();
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

                });

                describe('for edit mode', () => {
                    var deferred: ng.IDeferred<any>;
                    var activityFromService: Dto.IActivity;
                    var requestData: Antares.Activity.Commands.IActivityEditCommand;
                    var activity: Activity.ActivityEditModel;

                    beforeEach(() => {
                        // arrange
                        deferred = $q.defer();
                        activity = TestHelpers.ActivityGenerator.generateActivityEdit();
                        controller.activity = activity;

                        activityFromService = TestHelpers.ActivityGenerator.generateDto();

                        spyOn(activityService, 'updateActivity').and.callFake((activityCommand: Antares.Activity.Commands.IActivityEditCommand) => {
                            requestData = activityCommand;
                            return deferred.promise;
                        });
                    });

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
                });

                describe('for add mode', () => {
                    var deferred: ng.IDeferred<any>;
                    var activityFromService: Dto.IActivity;
                    var requestData: Antares.Activity.Commands.IActivityAddCommand;
                    var activity: Activity.ActivityEditModel;

                    beforeEach(() => {
                        // arrange
                        deferred = $q.defer();
                        activity = TestHelpers.ActivityGenerator.generateActivityEdit();
                        activity.id = null;
                        controller.activity = activity;

                        activityFromService = TestHelpers.ActivityGenerator.generateDto();

                        spyOn(activityService, 'addActivity').and.callFake((activityCommand: Antares.Activity.Commands.IActivityAddCommand) => {
                            requestData = activityCommand;
                            return deferred.promise;
                        });

                        spyOn(latestViewsProvider, 'addView').and.callFake(() => { });
                    });

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
                });

                var expectCorrectRequest = (requestData: Antares.Activity.Commands.IActivityBaseCommand, activity: Activity.ActivityEditModel) => {
                    expect(requestData.activityStatusId).toEqual(activity.activityStatusId);
                    expect(requestData.leadNegotiator.userId).toEqual(activity.leadNegotiator.userId);
                    expect(requestData.sellingReasonId).toEqual(activity.sellingReasonId);
                    expect(requestData.sourceId).toEqual(activity.sourceId);
                    expect(requestData.pitchingThreats).toEqual(activity.pitchingThreats);
                    expect(requestData.keyNumber).toEqual(activity.accessDetails.keyNumber);
                    expect(requestData.accessArrangements).toEqual(activity.accessDetails.accessArrangements);
                    expect(requestData.appraisalMeetingStart).toEqual(Core.DateTimeUtils.createDateAsUtc(activity.appraisalMeeting.appraisalMeetingStart));
                    expect(requestData.appraisalMeetingEnd).toEqual(Core.DateTimeUtils.createDateAsUtc(activity.appraisalMeeting.appraisalMeetingEnd));
                    expect(requestData.appraisalMeetingInvitationText).toEqual(activity.appraisalMeeting.appraisalMeetingInvitationText);
                    expect(requestData.appraisalMeetingAttendeesList.map((attendee: Antares.Activity.Commands.ActivityAttendeeCommandPart) => attendee.userId)).toEqual(activity.appraisalMeetingAttendees.map((attendee: Dto.IActivityAttendee) => attendee.userId));
                    expect(requestData.appraisalMeetingAttendeesList.map((attendee: Antares.Activity.Commands.ActivityAttendeeCommandPart) => attendee.contactId)).toEqual(activity.appraisalMeetingAttendees.map((attendee: Dto.IActivityAttendee) => attendee.contactId));
                    expect(requestData.secondaryNegotiators.map((negotiator) => negotiator.userId)).toEqual(activity.secondaryNegotiator.map((negotiator) => negotiator.userId));

                    expect(requestData.kfValuationPrice).toEqual(activity.kfValuationPrice);
                    expect(requestData.agreedInitialMarketingPrice).toEqual(activity.agreedInitialMarketingPrice);
                    expect(requestData.vendorValuationPrice).toEqual(activity.vendorValuationPrice);
                    expect(requestData.shortKfValuationPrice).toEqual(activity.shortKfValuationPrice);
                    expect(requestData.shortVendorValuationPrice).toEqual(activity.shortVendorValuationPrice);
                    expect(requestData.shortAgreedInitialMarketingPrice).toEqual(activity.shortAgreedInitialMarketingPrice);
                    expect(requestData.longKfValuationPrice).toEqual(activity.longKfValuationPrice);
                    expect(requestData.longVendorValuationPrice).toEqual(activity.longVendorValuationPrice);
                    expect(requestData.longAgreedInitialMarketingPrice).toEqual(activity.longAgreedInitialMarketingPrice);
                    expect(requestData.disposalTypeId).toEqual(activity.disposalTypeId);
                    expect(requestData.decorationId).toEqual(activity.decorationId);
                    expect(requestData.serviceChargeAmount).toEqual(activity.serviceChargeAmount);
                    expect(requestData.serviceChargeNote).toEqual(activity.serviceChargeNote);
                    expect(requestData.groundRentAmount).toEqual(activity.groundRentAmount);
                    expect(requestData.groundRentNote).toEqual(activity.groundRentNote);
                    expect(requestData.otherCondition).toEqual(activity.otherCondition);
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

        describe('when negotiator list has changed', () => {
            type TestCase = [Common.Models.Enums.NegotiatorTypeEnum]; //[NegotiatorTypeEnum]
            runDescribe('and new ')
                .data<TestCase>([
                    [Common.Models.Enums.NegotiatorTypeEnum.LeadNegotiator],
                    [Common.Models.Enums.NegotiatorTypeEnum.SecondaryNegotiator]
                ])
                .dataIt((data: TestCase) =>
                    `${data[0]} is added then they should be added to available attendees user list`)
                .run((data: TestCase) => {
                    // arrange
                    var activityUser: Business.ActivityUser = TestHelpers.ActivityUserGenerator.generate(data[0]);

                    controller.activity.activityDepartments = [TestHelpers.ActivityDepartmentGenerator.generate()];
                    controller.standardDepartmentType = TestHelpers.EnumTypeItemGenerator.generateDto();

                    // act
                    if (data[0] === Common.Models.Enums.NegotiatorTypeEnum.SecondaryNegotiator) {
                        controller.activity.secondaryNegotiator.push(activityUser);
                    }
                    else if (data[0] === Common.Models.Enums.NegotiatorTypeEnum.LeadNegotiator) {
                        controller.activity.leadNegotiator = activityUser;
                    }
                    controller.onNegotiatorAdded(activityUser.user);

                    // assert
                    var addedUser = _.find(controller.availableAttendeeUsers, (attendee: Business.User) => { return attendee.id === activityUser.user.id });

                    expect(addedUser).toBeTruthy();
                });

            runDescribe('and existing ')
                .data<TestCase>([
                    [Common.Models.Enums.NegotiatorTypeEnum.LeadNegotiator],
                    [Common.Models.Enums.NegotiatorTypeEnum.SecondaryNegotiator]
                ])
                .dataIt((data: TestCase) =>
                    `${data[0]} is removed then they should be removed from available attendees user list`)
                .run((data: TestCase) => {
                    // arrange
                    var activityUser: Business.ActivityUser = TestHelpers.ActivityUserGenerator.generate(data[0]);

                    controller.activity.activityDepartments = [TestHelpers.ActivityDepartmentGenerator.generate()];
                    controller.standardDepartmentType = TestHelpers.EnumTypeItemGenerator.generateDto();
                    controller.availableAttendeeUsers = [activityUser.user];

                    // act
                    controller.onNegotiatorRemoved();

                    // assert
                    var addedUser = _.find(controller.availableAttendeeUsers, (attendee: Business.User) => { return attendee.id === activityUser.user.id });

                    expect(addedUser).toBeFalsy();
                });
        });

        describe('should subscribe to events', () => {
            it('when close side panel event then property view panel should be closed', () => {
                // arrange
                controller.isPropertyPreviewPanelVisible = Enums.SidePanelState.Opened;

                // act
                eventAggregator.publish(new Common.Component.CloseSidePanelEvent());

                // assert
                expect(controller.isPropertyPreviewPanelVisible).toBe(Enums.SidePanelState.Closed);
            });

            it('when open property prewiew panel event then property view panel should be opened', () => {
                // arrange
                controller.isPropertyPreviewPanelVisible = Enums.SidePanelState.Closed;

                // act
                eventAggregator.publish(new Attributes.OpenPropertyPrewiewPanelEvent());

                // assert
                expect(controller.isPropertyPreviewPanelVisible).toBe(Enums.SidePanelState.Opened);
            });
        });

        describe('when in edit mode', () => {
            var deferred: ng.IDeferred<any>;
            var activityFromService: Dto.IActivity;
            var activity: Activity.ActivityEditModel;
            var userData: Dto.ICurrentUser;

            beforeEach(() => {
                // arrange
                activity = TestHelpers.ActivityGenerator.generateActivityEdit();
                controller.activity = activity;
                controller.userData = userData;

                enumProvider.enums = <Dto.IEnumDictionary>{
                    activityDepartmentType: [
                        { id: TestHelpers.StringGenerator.generate(), code: Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard] },
                        { id: TestHelpers.StringGenerator.generate(), code: Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Managing] }
                    ]
                };
            });

            describe('when initializing', () => {
                it('then correct lead negotiator should be set', () => {
                    // act
                    controller.$onInit();

                    expect(controller.activity.leadNegotiator).not.toBeNull();
                    expect(controller.activity.leadNegotiator.user.firstName).toBe(activity.leadNegotiator.user.firstName);
                    expect(controller.activity.leadNegotiator.user.lastName).toBe(activity.leadNegotiator.user.lastName);
                    expect(controller.activity.leadNegotiator.user.id).toBe(activity.leadNegotiator.user.id);
                    expect(controller.activity.leadNegotiator.user.departmentId).toBe(activity.leadNegotiator.user.departmentId);
                });

                it('then correct call date should be set', () => {
                    // act
                    controller.$onInit();

                    var callDate = moment(controller.activity.leadNegotiator.callDate);
                    var expectedCallDate = moment(activity.leadNegotiator.callDate);

                    expect(callDate.day).toBe(expectedCallDate.day);
                    expect(callDate.month).toBe(expectedCallDate.month);
                    expect(callDate.year).toBe(expectedCallDate.year);
                });
            });

        });

        describe('when in add mode', () => {
            var deferred: ng.IDeferred<any>;
            var activityFromService: Dto.IActivity;
            var activity: Activity.ActivityEditModel;
            var userData: Dto.ICurrentUser;

            beforeEach(() => {
                // arrange
                userData = TestHelpers.UserGenerator.generateUserDataDto();
                activity = TestHelpers.ActivityGenerator.generateActivityEdit();

                activity.id = null;
                controller.activity = activity;
                controller.userData = userData;

                enumProvider.enums = <Dto.IEnumDictionary>{
                    activityDepartmentType: [
                        { id: TestHelpers.StringGenerator.generate(), code: Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Standard] },
                        { id: TestHelpers.StringGenerator.generate(), code: Enums.DepartmentTypeEnum[Enums.DepartmentTypeEnum.Managing] }
                    ]
                };
            });

            describe('when initializing', () => {
                it('then current user should be set as lead negotiator', () => {
                    // act
                    controller.$onInit();

                    expect(controller.activity.leadNegotiator).not.toBeNull();
                    expect(controller.activity.leadNegotiator.user.firstName).toBe(userData.firstName);
                    expect(controller.activity.leadNegotiator.user.lastName).toBe(userData.lastName);
                    expect(controller.activity.leadNegotiator.user.lastName).toBe(userData.lastName);
                    expect(controller.activity.leadNegotiator.user.id).toBe(userData.id);
                    expect(controller.activity.leadNegotiator.user.departmentId).toBe(userData.department.id);
                });

                it('then call date for lead negotiator should be set 2 weeks from today', () => {
                    // act
                    controller.$onInit();

                    var twoWeeksFromToday = moment().add(2, 'week');
                    var callDate = moment(controller.activity.leadNegotiator.callDate);

                    expect(callDate.day).toBe(twoWeeksFromToday.day);
                    expect(callDate.month).toBe(twoWeeksFromToday.month);
                    expect(callDate.year).toBe(twoWeeksFromToday.year);
                });
            });

        });

        describe('when isOtherSectionVisible is called', () =>{
            type TestCase = [any, any, boolean];
            runDescribe('with specific config')
                .data<TestCase>([
                    [{}, {}, true],
                    [{}, null, false],
                    [null, {}, false],
                    [null, null, false]])
                .dataIt((data: TestCase) =>
                    `where decoration is ${data[0]} and otherCondition is ${data[1]} then isOtherSectionVisible must return ${data[2]}`)
                .run((data: TestCase) => {
                    controller.config.decoration = data[0];
                    controller.config.otherCondition = data[1];

                    // act & assert
                    expect(controller.isOtherSectionVisible()).toBe(data[2]);
                });
        });
    });
}


