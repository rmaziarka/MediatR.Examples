/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityViewController = Activity.View.ActivityViewController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    describe('Given activity view controller', () => {
        var scope: ng.IScope,
            q: ng.IQService,
            evtAggregator: Antares.Core.EventAggregator,
            controller: ActivityViewController,
            $http: ng.IHttpBackendService;

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
    });
}