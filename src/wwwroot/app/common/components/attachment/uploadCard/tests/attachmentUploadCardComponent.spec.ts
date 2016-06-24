/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AttachmentUploadCardController = Common.Component.Attachment.AttachmentUploadCardController;
    import Business = Common.Models.Business;

    interface IAttachmentUploadCardScope extends ng.IScope {
        entityId: string;
        entityType: string;
        attachmentClear: boolean;
    }

    describe('Given attachment upload card component', () => {
        var scope: IAttachmentUploadCardScope,
            element: ng.IAugmentedJQuery,
            controller: AttachmentUploadCardController;

        var pageObjectSelectors = {
            fileNameLink: '#attachment-preview-fileName a',
            fileNameSpan: '#attachment-preview-fileName span',
            fileTypeId: '#attachment-preview-type',
            size: '#attachment-preview-size',
            createdDate: '#attachment-preview-created-date',
            user: '#attachment-preview-user'
        }

        describe('when attachment is set', () =>{
            var entityId = 'testEntityId',
                entityType = 'TestEntity'

            var attachment = TestHelpers.AttachmentGenerator.generate({ user: new Business.User({ id: 'us1', firstName: 'firstName1', lastName: 'lastName1', departmentId: 'depId', department: null }) });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = <IAttachmentUploadCardScope>$rootScope.$new();
                scope.entityId = entityId;
                scope.entityType = entityType;
                scope.attachmentClear = false;

                element = $compile('<attachment-upload-card entity-id="entityId" entity-type="{{entityType}}" attachment-clear="attachmentClear"></attachment-upload-card>')(scope);
                scope.$apply();

                controller = element.controller('attachmentUploadCard');
            }));

            it('when attachmentClear is changed to true then clearAttachmentForm should be called', () => {
                // arrange / act
                scope.attachmentClear = false;
                spyOn(controller, 'clearAttachmentForm');

                // act
                scope.attachmentClear = true;
                scope.$apply();

                // assert
                expect(controller.clearAttachmentForm).toHaveBeenCalled();
            });

            it('when attachmentClear is changed to false then clearAttachmentForm should not be called', () => {
                // arrange / act
                scope.attachmentClear = true;
                spyOn(controller, 'clearAttachmentForm');

                // act
                scope.attachmentClear = false;
                scope.$apply();

                // assert
                expect(controller.clearAttachmentForm).not.toHaveBeenCalled();
            });
        });
    });
}