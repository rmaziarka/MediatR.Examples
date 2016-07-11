﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityViewController = Activity.View.ActivityViewController;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given activity view controller', () => {
        var scope: ng.IScope,
            evtAggregator: Antares.Core.EventAggregator,
            controller: ActivityViewController,
            $http: ng.IHttpBackendService,
            obj = 'an object';

        var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            eventAggregator: Antares.Core.EventAggregator,
            $httpBackend: ng.IHttpBackendService) => {

            // init
            scope = $rootScope.$new();
            $http = $httpBackend;
            evtAggregator = eventAggregator;

            var bindings = { activity: activityMock };
            controller = <ActivityViewController>$controller('activityViewController', { $scope: scope }, bindings);
            controller.config = TestHelpers.ConfigGenerator.generateActivityViewConfig();
        }));

        beforeAll(() => {
            jasmine.addMatchers(TestHelpers.CustomMatchers.AttachmentCustomMatchersGenerator.generate());
        });

        describe('when AttachmentSavedEvent event is triggered', () => {
            it('then addSavedAttachmentToList should be called', () => {
                // arrange
                spyOn(controller, 'addSavedAttachmentToList')

                var attachmentDto = TestHelpers.AttachmentGenerator.generateDto();
                var command = new Common.Component.Attachment.AttachmentSavedEvent(attachmentDto);

                // act
                evtAggregator.publish(command);

                // assert
                expect(controller.addSavedAttachmentToList).toHaveBeenCalledWith(command.attachmentSaved);
            });
        });

        describe('when addSavedAttachmentToList is called', () => {
            it('then attachment should be added to list', () => {
                // arrange
                var attachmentDto = TestHelpers.AttachmentGenerator.generateDto();
                controller.activity.attachments = [];

                // act
                controller.addSavedAttachmentToList(attachmentDto);

                // assert
                var expectedAttachment = new Business.Attachment(attachmentDto);
                expect(controller.activity.attachments[0]).toBeSameAsAttachment(expectedAttachment);
            });
        });

        describe('when saveAttachment is called', () => {
            it('then proper API request should be sent', () => {
                // arrange
                var attachmentModel = TestHelpers.AttachmentUploadCardModelGenerator.generate();
                var requestData: Activity.Command.ActivityAttachmentSaveCommand;

                var expectedUrl = `/api/activities/${controller.activity.id}/attachments`;
                $http.expectPOST(expectedUrl, (data: string) => {
                    requestData = JSON.parse(data);
                    return true;
                }).respond(201, {});

                // act
                controller.saveAttachment(attachmentModel);
                $http.flush();

                // assert
                expect(requestData).not.toBe(null);
                expect(requestData.activityId).toBe(controller.activity.id);
                expect(requestData.attachment).toBeSameAsAttachmentModel(attachmentModel);

            });
        });

        describe("and selected tab is 'Overview'", () => {
            beforeEach(() => {
                controller.selectedTabIndex = 0;
            });

            it("then 'isOverviewTabSelected' must be true",
                () => {
                    // act & assert
                    expect(controller.isOverviewTabSelected()).toBe(true);
                });

            it("then 'isDetailsTabSelected' must be false",
                () => {
                    // act & assert
                    expect(controller.isDetailsTabSelected()).toBe(false);
                });
        });

        describe("and selected tab is 'Details'", () => {
            beforeEach(() => {
                controller.selectedTabIndex = 1;
            });

            it("then 'isOverviewTabSelected' must be false",
                () => {
                    // act & assert
                    expect(controller.isOverviewTabSelected()).toBe(false);
                });

            it("then 'isDetailsTabSelected' must be true",
                () => {
                    // act & assert
                    expect(controller.isDetailsTabSelected()).toBe(true);
                });
        });

        describe('when selected tab is changed', () => {
            beforeEach(() => {                
                controller.isOfferPreviewPanelVisible = Enums.SidePanelState.Opened;
                controller.isPropertyPreviewPanelVisible = Enums.SidePanelState.Closed;
                controller.isAttachmentsUploadPanelVisible = Enums.SidePanelState.Opened
                controller.isAttachmentsPreviewPanelVisible = Enums.SidePanelState.Opened;
                controller.isViewingPreviewPanelVisible = Enums.SidePanelState.Opened;                
            });

            it("when 'Overview' tab is selected then selected tab index must be 0 and all side panels must have state 'Untouched'", () => {
                // act
                controller.setActiveTabIndex(0);
                
                // assert  
                expect(controller.selectedTabIndex).toBe(0);                              
                assertPanelState(controller, Enums.SidePanelState.Untouched);
            });

            it("when 'Details' tab is selected then selected tab index must be 1 and all side panels must have state 'Untouched'", () => {
                // act
                controller.setActiveTabIndex(1);

                // assert
                expect(controller.selectedTabIndex).toBe(1);
                assertPanelState(controller, Enums.SidePanelState.Untouched);
            });

            var assertPanelState = (controller: ActivityViewController, state: Enums.SidePanelState) => {
                expect(controller.isAttachmentsUploadPanelVisible).toBe(state);
                expect(controller.isAttachmentsPreviewPanelVisible).toBe(state);
                expect(controller.isPropertyPreviewPanelVisible).toBe(state);
                expect(controller.isViewingPreviewPanelVisible).toBe(state);
                expect(controller.isOfferPreviewPanelVisible).toBe(state);
            };
        });

        describe('when isRentSectionVisibleVisible is called', () => {
            type TestCase = [any, any, any, any, any, any, any, any, any, any, boolean];
            runDescribe('with specific config and the following parameters')
                .data<TestCase>([
                    [obj, obj, null, null, obj, obj, obj, obj, obj, obj, true],
                    [obj, obj, obj, obj, null, obj, obj, obj, obj, obj, true],
                    [obj, obj, null, null, null, obj, obj, obj, obj, obj, false],
                    [obj, obj, obj, obj, obj, obj, obj, obj, obj, null, true],
                    [obj, obj, obj, obj, obj, obj, obj, null, null, obj, true],
                    [obj, obj, obj, obj, obj, obj, obj, null, null, null, false],
                    [null, obj, obj, obj, obj, obj, obj, obj, obj, obj, false],
                    [obj, null, obj, obj, obj, obj, obj, obj, obj, obj, false],
                    [obj, obj, obj, obj, obj, null, obj, obj, obj, obj, false],
                    [obj, obj, obj, obj, obj, obj, null, obj, obj, obj, false]])
                .dataIt((data: TestCase) =>
                    `where shortAskingMonthRent is ${data[0]} and shortAskingWeekRent is ${data[1]} and shortMatchFlexMonthValue is ${data[2]} and shortMatchFlexWeekValue is ${data[3]} and shortMatchFlexPercentage is ${data[4]} and longAskingMonthRent is ${data[5]} and longAskingWeekRent is ${data[6]} and longMatchFlexMonthValue is ${data[7]} and longMatchFlexWeekValue is ${data[8]} and longMatchFlexPercentage is ${data[9]} then isValuationInfoShortLongSectionVisible must return ${data[10]}`)
                .run((data: TestCase) => {
                    controller.config.shortAskingMonthRent = data[0];
                    controller.config.shortAskingWeekRent = data[1];
                    controller.config.shortMatchFlexMonthValue = data[2];
                    controller.config.shortMatchFlexWeekValue = data[3];
                    controller.config.shortMatchFlexPercentage = data[4];
                    controller.config.longAskingMonthRent = data[5];
                    controller.config.longAskingWeekRent = data[6];
                    controller.config.longMatchFlexMonthValue = data[7];
                    controller.config.longMatchFlexWeekValue = data[8];
                    controller.config.longMatchFlexPercentage = data[9];

                    // act & assert
                    expect(controller.isRentSectionVisible()).toBe(data[10]);
                });
        });

    });
}