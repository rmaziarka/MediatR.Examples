/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AttachmentUploadCardController = Common.Component.Attachment.AttachmentUploadCardController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    describe('Given attachment upload card controller', () => {
        var $scope: ng.IScope,
            q: ng.IQService,
            evtAggregator: Antares.Core.EventAggregator,
            controller: AttachmentUploadCardController;

        var getAzureUploadUrlDefferedMock: ng.IDeferred<Dto.IAzureUploadUrlContainer>;
        var uploadFileDefferedMock: ng.IDeferred<any>;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: ng.IControllerService,
            $q: ng.IQService,
            eventAggregator: Antares.Core.EventAggregator) => {

            // init
            $scope = $rootScope.$new();
            q = $q;
            evtAggregator = eventAggregator;

            getAzureUploadUrlDefferedMock = q.defer();
            uploadFileDefferedMock = q.defer();

            var bindings: any = { onUploadFinished: () => { }, onUploadFailed: () => { }, onUploadStarted: () => { } };

            controller = <AttachmentUploadCardController>$controller('AttachmentUploadCardController', { $scope: $scope }, bindings);
        }));

        describe('when cancel is called', () => {
            it('then BusySidePanelEvent event should be published', () => {
                // arrange
                var isClosedCalled = false;
                evtAggregator.with(controller)
                    .subscribe(Common.Component.CloseSidePanelEvent, (event: Common.Component.CloseSidePanelEvent) => {
                        isClosedCalled = true;
                    });

                //act
                controller.cancel();
                $scope.$apply();

                // assert
                expect(isClosedCalled).toBe(true);
            });
        });

        describe('when uploadAttachment is called', () => {
            it('and getAzureUploadUrl fails then onUploadFinished is not called and onUploadFailed is called', () => {
                // arrange
                spyOn(controller, 'getAzureUploadUrl').and.returnValue(getAzureUploadUrlDefferedMock.promise);
                spyOn(controller, 'uploadFile').and.returnValue(uploadFileDefferedMock.promise);
                spyOn(controller, 'onUploadStarted');
                spyOn(controller, 'onUploadFinished');
                spyOn(controller, 'onUploadFailed');

                //act
                controller.uploadAttachment();

                getAzureUploadUrlDefferedMock.reject();
                uploadFileDefferedMock.resolve();

                $scope.$apply();

                // assert
                expect(controller.onUploadStarted).toHaveBeenCalled();
                expect(controller.onUploadFailed).toHaveBeenCalled();
                expect(controller.onUploadFinished).not.toHaveBeenCalled();
            });

            it('and uploadFile fails then onUploadFinished is not called and onUploadFailed is called', () => {
                // arrange
                spyOn(controller, 'getAzureUploadUrl').and.returnValue(getAzureUploadUrlDefferedMock.promise);
                spyOn(controller, 'uploadFile').and.returnValue(uploadFileDefferedMock.promise);
                spyOn(controller, 'onUploadStarted');
                spyOn(controller, 'onUploadFinished');
                spyOn(controller, 'onUploadFailed');

                //act
                controller.uploadAttachment();

                var azureUrlContainerMock = { externalDocumentId: 'testExternalDocId', url: 'http:\\test.com' };
                getAzureUploadUrlDefferedMock.resolve(azureUrlContainerMock);
                uploadFileDefferedMock.reject();

                $scope.$apply();

                // assert
                expect(controller.onUploadStarted).toHaveBeenCalled();
                expect(controller.onUploadFailed).toHaveBeenCalled();
                expect(controller.onUploadFinished).not.toHaveBeenCalled();
            });

            it('and all promises resolve successfully then onUploadFinished is called with proper attachment and onUploadFailed is not called', () => {
                // arrange
                var attachment: Common.Component.Attachment.AttachmentUploadCardModel;
                var fileMock = { name: 'testFileName', size: 111 };
                var documentTypeIdMock = 'testDocumentTypeId';

                controller.file = <File>fileMock;
                controller.documentTypeId = documentTypeIdMock;

                spyOn(controller, 'getAzureUploadUrl').and.returnValue(getAzureUploadUrlDefferedMock.promise);
                spyOn(controller, 'uploadFile').and.returnValue(uploadFileDefferedMock.promise);
                spyOn(controller, 'onUploadStarted');
                spyOn(controller, 'onUploadFinished').and.callFake((obj: { attachment: Common.Component.Attachment.AttachmentUploadCardModel }) => {
                    attachment = obj.attachment;
                });
                spyOn(controller, 'onUploadFailed');

                //act
                controller.uploadAttachment();

                var azureUrlContainerMock = { externalDocumentId: 'testExternalDocId', url: 'http:\\test.com' };
                getAzureUploadUrlDefferedMock.resolve(azureUrlContainerMock);
                uploadFileDefferedMock.resolve();

                $scope.$apply();

                // assert
                expect(controller.onUploadStarted).toHaveBeenCalled();
                expect(controller.onUploadFailed).not.toHaveBeenCalled();
                expect(controller.onUploadFinished).toHaveBeenCalled();

                expect(attachment.externalDocumentId).toBe(azureUrlContainerMock.externalDocumentId);
                expect(attachment.fileName).toBe(fileMock.name);
                expect(attachment.size).toBe(fileMock.size);
                expect(attachment.documentTypeId).toBe(documentTypeIdMock);
            });
        });

    });
}