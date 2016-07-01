/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given appraisal meeting date edit control controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: ActivityAppraisalMeetingDateEditControlController,
            startDate = '2016-06-30 10:00',
            endDate = '2016-06-30 12:00',
            config: IActivityAppraisalMeetingDateControlConfig;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService) => {

                // init
                $scope = $rootScope.$new();
                config = {
                    start: TestHelpers.ConfigGenerator.generateFieldConfig(),
                    end: TestHelpers.ConfigGenerator.generateFieldConfig()
                };
                $scope['config'] = config;
                $http = $httpBackend;

                var bindings = { appraisalMeetingStart: startDate, appraisalMeetingEnd: endDate, config: config };
                controller = <ActivityAppraisalMeetingDateEditControlController>$controller('ActivityAppraisalMeetingDateEditControlController', { $scope: $scope }, bindings);
            }
        ));

        describe('when start and end are set and onChange is called', () => {
            var dateTimeFormat = 'YYYY-MM-DD HH:mm';
            beforeEach(() => {
                controller.meetingDate = moment('2016-06-30', 'YYYY-MM-DD').toDate();
                controller.meetingStartTime = moment('2000-01-01 15:00', dateTimeFormat);
                controller.meetingEndTime = moment('2000-01-01 18:00', dateTimeFormat);

                controller.onChange();
            });

            it('then value of appraisalMeetingStart must be update to correct date', () => {
                expect(controller.appraisalMeetingStart).toEqual(moment('2016-06-30 15:00', dateTimeFormat).toDate());
            });

            it('then value of appraisalMeetingEnd must be update to correct date', () => {
                expect(controller.appraisalMeetingEnd).toEqual(moment('2016-06-30 18:00', dateTimeFormat).toDate());
            });
        });
       
        describe('when isRequired is called', () => {
            type TestCase = [boolean, boolean, boolean]; // start required, end required, expected result           
            runDescribe('with specific config')
                .data<TestCase>([
                    [true, true, true],
                    [true, false, true],
                    [false, true, true],
                    [false, false, false]])
                .dataIt((data: TestCase) =>
                    `where startRequire is ${data[0]} and endRequire is ${data[1]} then isRequire must return ${data[2]}`)
                .run((data: TestCase) => {
                    controller.config.start.required = data[0];
                    controller.config.end.required = data[1];

                    // act & assert
                    expect(controller.isRequired()).toBe(data[2]);
                });
        });
    });
}