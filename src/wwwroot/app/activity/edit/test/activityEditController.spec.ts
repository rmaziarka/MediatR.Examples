/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import ActivityEditController = Activity.ActivityEditController;
    import Enums = Common.Models.Enums;
    import Commands = Common.Models.Commands;
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
            activityService: Services.ActivityService,
            enumProvider: Providers.EnumProvider,
            latestViewsProvider: Providers.LatestViewsProvider,
            obj = 'an object';

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
            _activityService_: Services.ActivityService,
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
            controller.activity = new Business.ActivityEditModel();
            controller.activity.leadNegotiator = leadNegotiatorMock;
            controller.activity.secondaryNegotiator = secondaryNegotiatorsMock;
            controller.config = <Activity.IActivityEditViewConfig>{
                editConfig: TestHelpers.ConfigGenerator.generateActivityEditConfig(),
                viewConfig: {}
            };
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
                    var requestData: Commands.Activity.IActivityEditCommand;
                    var activity: Business.ActivityEditModel;

                    beforeEach(() => {
                        // arrange
                        deferred = $q.defer();
                        activity = TestHelpers.ActivityGenerator.generateActivityEdit();
                        controller.activity = activity;

                        activityFromService = TestHelpers.ActivityGenerator.generateDto();

                        spyOn(activityService, 'updateActivity').and.callFake((activityCommand: Commands.Activity.IActivityEditCommand) => {
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
                    var requestData: Commands.Activity.IActivityAddCommand;
                    var activity: Business.ActivityEditModel;

                    beforeEach(() => {
                        // arrange
                        deferred = $q.defer();
                        activity = TestHelpers.ActivityGenerator.generateActivityEdit();
                        activity.id = null;
                        controller.activity = activity;

                        activityFromService = TestHelpers.ActivityGenerator.generateDto();

                        spyOn(activityService, 'addActivity').and.callFake((activityCommand: Commands.Activity.IActivityAddCommand) => {
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

                var expectCorrectRequest = (requestData: Commands.Activity.IActivityBaseCommand, activity: Business.ActivityEditModel) => {
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
                    expect(requestData.appraisalMeetingAttendeesList.map((attendee: Commands.Activity.ActivityAttendeeCommandPart) => attendee.userId)).toEqual(activity.appraisalMeetingAttendees.map((attendee: Dto.IActivityAttendee) => attendee.userId));
                    expect(requestData.appraisalMeetingAttendeesList.map((attendee: Commands.Activity.ActivityAttendeeCommandPart) => attendee.contactId)).toEqual(activity.appraisalMeetingAttendees.map((attendee: Dto.IActivityAttendee) => attendee.contactId));
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
            var activity: Business.ActivityEditModel;
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
            var activity: Business.ActivityEditModel;
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

        describe('when isValuationPricesSectionVisible is called', () => {
            type TestCase = [any, any, boolean];
            runDescribe('with specific config')
                .data<TestCase>([
                    [{}, {}, true],
                    [null, {}, true],
                    [{}, null, true],
                    [null, null, false]])
                .dataIt((data: TestCase) =>
                    `where askingPrice is ${data[0]} and shortLetPricePerWeek is ${data[1]} then isValuationPricesSectionVisible must return ${data[2]}`)
                .run((data: TestCase) => {
                    controller.config.editConfig.askingPrice = data[0];
                    controller.config.editConfig.shortLetPricePerWeek = data[1];

                    // act & assert
                    expect(controller.isValuationPricesSectionVisible()).toBe(data[2]);
                });
        });

        describe('when isBasicInformationSectionVisible is called', () => {
            type TestCase = [any, any, any, any, any, any, boolean];
            runDescribe('with specific config')
                .data<TestCase>([
                    [{}, {}, {}, {}, {}, {}, true],
                    [null, {}, {}, {}, {}, {}, true],
                    [{}, null, {}, {}, {}, {}, true],
                    [{}, {}, null, {}, {}, {}, true],
                    [{}, {}, {}, null, {}, {}, true],
                    [{}, {}, {}, {}, null, {}, true],
                    [{}, {}, {}, {}, {}, null, true],
                    [null, null, null, null, null, null, false]])
                .dataIt((data: TestCase) =>
                    `where property is ${data[0]} and source is ${data[1]} and sourceDescription is ${data[2]} and sellingReason is ${data[3]} and pitchingThreats is ${data[4]} and disposalType is ${data[5]} then isBasicInformationSectionVisible must return ${data[6]}`)
                .run((data: TestCase) => {
                    controller.config.editConfig.property = data[0];
                    controller.config.editConfig.source = data[1];
                    controller.config.editConfig.sourceDescription = data[2];
                    controller.config.editConfig.sellingReason = data[3];
                    controller.config.editConfig.pitchingThreats = data[4];
                    controller.config.editConfig.disposalType = data[5];
                    
                    // act & assert
                    expect(controller.isBasicInformationSectionVisible()).toBe(data[6]);
                });
        });

        describe('when isAdditionalInformationSectionVisible is called', () => {
            type TestCase = [any, any, boolean];
            runDescribe('with specific config')
                .data<TestCase>([
                    [{}, {}, true],
                    [null, {}, true],
                    [{}, null, true],
                    [null, null, false]])
                .dataIt((data: TestCase) =>
                    `where keyNumber is ${data[0]} and accessArrangements is ${data[1]} then isAdditionalInformationSectionVisible must return ${data[2]}`)
                .run((data: TestCase) => {
                    controller.config.editConfig.keyNumber = data[0];
                    controller.config.editConfig.accessArrangements = data[1];

                    // act & assert
                    expect(controller.isAdditionalInformationSectionVisible()).toBe(data[2]);
                });
        });

        describe('when isAppraisalMeetingSectionVisible is called', () => {
            type TestCase = [any, any, any, boolean];
            runDescribe('with specific config')
                .data<TestCase>([
                    [{}, {}, {}, true],
                    [null, {}, {}, true],
                    [{}, null, {}, true],
                    [{}, {}, null, true],
                    [null, null, null, false]])
                .dataIt((data: TestCase) =>
                    `where appraisalMeetingDate is ${data[0]} and appraisalMeetingAttendees is ${data[1]} and appraisalMeetingInvitation is ${data[2]} then isAppraisalMeetingSectionVisible must return ${data[3]}`)
                .run((data: TestCase) => {
                    controller.config.editConfig.appraisalMeetingDate = data[0];
                    controller.config.editConfig.appraisalMeetingAttendees = data[1];
                    controller.config.editConfig.appraisalMeetingInvitation = data[2];

                    // act & assert
                    expect(controller.isAppraisalMeetingSectionVisible()).toBe(data[3]);
                });
        });

        describe('when isOtherSectionVisible is called', () => {
            type TestCase = [any, any, boolean];
            runDescribe('with specific config and the following parameters')
                .data<TestCase>([
                    [obj, obj, true],
                    [obj, null, false],
                    [null, obj, false],
                    [null, null, false]])
                .dataIt((data: TestCase) =>
                    `where 1st is ${data[0]} and 2nd is ${data[1]} then isOtherSectionVisible must return ${data[2]}`)
                .run((data: TestCase) => {
                    controller.config.editConfig.decoration = data[0];
                    controller.config.editConfig.otherCondition = data[1];

                    // act & assert
                    expect(controller.isOtherSectionVisible()).toBe(data[2]);
                });
        });

        describe('when isValuationInfoSectionVisible is called', () => {
            type TestCase = [any, any, any, boolean];
            runDescribe('with specific config and the following parameters')
                .data<TestCase>([
                    [obj, obj, obj, true],
                    [null, obj, obj, false],
                    [obj, null, obj, false],
                    [obj, obj, null, false],
                    [null, null, null, false]])
                .dataIt((data: TestCase) =>
                    `where 1st is ${data[0]} and 2nd is ${data[1]} and 3rd is ${data[2]} then isValuationInfoSectionVisible must return ${data[3]}`)
                .run((data: TestCase) => {
                    controller.config.editConfig.kfValuationPrice = data[0];
                    controller.config.editConfig.vendorValuationPrice = data[1];
                    controller.config.editConfig.agreedInitialMarketingPrice = data[2];

                    // act & assert
                    expect(controller.isValuationInfoSectionVisible()).toBe(data[3]);
                });
        });

        describe('when isValuationInfoShortLongSectionVisible is called', () => {
            type TestCase = [any, any, any, any, any, any, boolean];
            runDescribe('with specific config and the following parameters')
                .data<TestCase>([
                    [obj, obj, obj, obj, obj, obj, true],
                    [null, obj, obj, obj, obj, obj, false],
                    [obj, null, obj, obj, obj, obj, false],
                    [obj, obj, null, obj, obj, obj, false],
                    [obj, obj, obj, null, obj, obj, false],
                    [obj, obj, obj, obj, null, obj, false],
                    [obj, obj, obj, obj, obj, null, false],
                    [null, null, null, null, null, null, false]])
                .dataIt((data: TestCase) =>
                    `where 1st is ${data[0]} and 2nd is ${data[1]} and 3rd is ${data[2]} and 4th is ${data[3]} and 5th is ${data[4]} and 6th is ${data[5]} then isValuationInfoShortLongSectionVisible must return ${data[6]}`)
                .run((data: TestCase) => {
                    controller.config.editConfig.shortKfValuationPrice = data[0];
                    controller.config.editConfig.longKfValuationPrice = data[1];
                    controller.config.editConfig.shortVendorValuationPrice = data[2];
                    controller.config.editConfig.longVendorValuationPrice = data[3];
                    controller.config.editConfig.shortAgreedInitialMarketingPrice = data[4];
                    controller.config.editConfig.longAgreedInitialMarketingPrice = data[5];

                    // act & assert
                    expect(controller.isValuationInfoShortLongSectionVisible()).toBe(data[6]);
                });
        });

        describe('when isChargesSectionVisible is called', () => {
            type TestCase = [any, any, any, boolean];
            runDescribe('with specific config and the following parameters')
                .data<TestCase>([
                    [obj, obj, obj, true],
                    [null, obj, obj, false],
                    [obj, null, obj, false],
                    [obj, obj, null, false],
                    [null, null, null, false]])
                .dataIt((data: TestCase) =>
                    `where 1st is ${data[0]} and 2nd is ${data[1]} and 3rd is ${data[2]} then isChargesSectionVisible must return ${data[3]}`)
                .run((data: TestCase) => {
                    controller.config.editConfig.serviceChargeAmount = data[0];
                    controller.config.editConfig.groundRentAmount = data[1];
                    controller.config.editConfig.groundRentNote = data[2];

                    // act & assert
                    expect(controller.isChargesSectionVisible()).toBe(data[3]);
                });
        });
    });
}


