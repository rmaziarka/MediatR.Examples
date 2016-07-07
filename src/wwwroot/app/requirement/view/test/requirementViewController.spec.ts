/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import RequirementViewController = Requirement.View.RequirementViewController;
    import Business = Common.Models.Business;

    describe('Given requirement view controller', () =>{
        var scope: ng.IScope,
            evtAggregator: Core.EventAggregator,
            controller: RequirementViewController,
            $http: ng.IHttpBackendService;

        var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            eventAggregator: Core.EventAggregator,
            $httpBackend: ng.IHttpBackendService) =>{
            scope = $rootScope.$new();
            $http = $httpBackend;
            evtAggregator = eventAggregator;

            var bindings = { requirement : requirementMock };
            controller = <RequirementViewController>$controller("requirementViewController", { $scope : scope }, bindings);
        }));

        beforeAll(() =>{
            jasmine.addMatchers(TestHelpers.CustomMatchers.AttachmentCustomMatchersGenerator.generate());
        });

        describe('when AttachmentSavedEvent event is triggered', () =>{
            it('then addSavedAttachmentToList should be called', () =>{
                // arrange
                spyOn(controller, 'addSavedAttachmentToList');

                var attachmentDto = TestHelpers.AttachmentGenerator.generateDto();
                var command = new Common.Component.Attachment.AttachmentSavedEvent(attachmentDto);

                // act
                evtAggregator.publish(command);

                // assert
                expect(controller.addSavedAttachmentToList).toHaveBeenCalledWith(command.attachmentSaved);
            });
        });

        describe('when addSavedAttachmentToList is called', () =>{
            it('then attachment should be added to list', () =>{
                // arrange
                var attachmentDto = TestHelpers.AttachmentGenerator.generateDto();
                controller.requirement.attachments = [];

                // act
                controller.addSavedAttachmentToList(attachmentDto);

                // assert
                var expectedAttachment = new Business.Attachment(attachmentDto);
                expect(controller.requirement.attachments[0]).toBeSameAsAttachment(expectedAttachment);
            });
        });

        describe('when saveAttachment is called', () =>{
            it('then proper API request should be sent', () =>{
                // arrange
                var attachmentModel = TestHelpers.AttachmentUploadCardModelGenerator.generate();
                var requestData: Requirement.Command.RequirementAttachmentSaveCommand;

                var expectedUrl = `/api/requirements/${controller.requirement.id}/attachments/`;
                $http.expectPOST(expectedUrl, (data: string) =>{
                    requestData = JSON.parse(data);
                    return true;
                }).respond(201, {});

                // act
                controller.saveAttachment(attachmentModel);
                $http.flush();

                // assert
                expect(requestData).not.toBe(null);
                expect(requestData.requirementId).toBe(controller.requirement.id);
                expect(requestData.attachment).toBeSameAsAttachmentModel(attachmentModel);
            });
        });
    });
}