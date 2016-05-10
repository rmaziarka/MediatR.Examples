/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AttachmentUploadController = Common.Component.AttachmentUploadController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    describe('Given attachment upload controller', () => {
        var $scope: ng.IScope,
            q: ng.IQService,
            controller: AttachmentUploadController;

        describe('when createAttachment is called', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: ng.IControllerService) => {

                // init
                $scope = $rootScope.$new();

                controller = <AttachmentUploadController>$controller('AttachmentUploadController', { $scope: $scope });
            }));

            it('then attachment is returned with  proper data set', () => {
                // arrange
                var fileMock = { name: 'testFileName', size: 111 };
                var documentTypeIdMock = 'testDocumentTypeId';

                controller.file = <File>fileMock;
                controller.documentTypeId = documentTypeIdMock;

                //act
                var externalDocumentIdMock = 'testExternalDocumentIdMock';

                var createdAttachment = controller.createAttachment(externalDocumentIdMock);

                // assert
                expect(createdAttachment.externalDocumentId).toBe(externalDocumentIdMock);
                expect(createdAttachment.fileName).toBe(fileMock.name);
                expect(createdAttachment.size).toBe(fileMock.size);
                expect(createdAttachment.documentTypeId).toBe(documentTypeIdMock);
            });
        });

        describe('when uploadAttachment is called', () => {
            var activityIdMock: string = 'testActivityId';
            var getAzureUploadUrlDefferedMock: ng.IDeferred<Dto.IAzureUploadUrlContainer>;
            var uploadFileDefferedMock: ng.IDeferred<any>;

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: ng.IControllerService,
                $q: ng.IQService) => {

                // init
                $scope = $rootScope.$new();
                q = $q;

                getAzureUploadUrlDefferedMock = q.defer();
                uploadFileDefferedMock = q.defer();

                controller = <AttachmentUploadController>$controller('AttachmentUploadController', { $scope: $scope });
            }));

            it('and data is invalid then nothing is called and rejected promise is returned', () => {
                // arrange
                spyOn(controller, 'isDataValid').and.returnValue(false);
                spyOn(controller, 'getAzureUploadUrl').and.returnValue(getAzureUploadUrlDefferedMock.promise);
                spyOn(controller, 'uploadFile').and.returnValue(uploadFileDefferedMock.promise);
                spyOn(controller, 'createAttachment');

                //act
                var resultIsSucces: boolean;
                var azureUrlContainerMock = { externalDocumentId: 'testExternalDocId', url: 'http:\\test.com' };

                controller.uploadAttachment(activityIdMock)
                    .then(() => { resultIsSucces = true; }, () => { resultIsSucces = false; });

                getAzureUploadUrlDefferedMock.resolve(azureUrlContainerMock);
                uploadFileDefferedMock.resolve(azureUrlContainerMock.externalDocumentId);

                $scope.$apply();

                // assert
                expect(resultIsSucces).toBeFalsy();
                expect(controller.getAzureUploadUrl).not.toHaveBeenCalled();
                expect(controller.uploadFile).not.toHaveBeenCalled();
                expect(controller.createAttachment).not.toHaveBeenCalled();
            });

            it('and getAzureUploadUrl fails then uploadFile and createAttachment are not called and rejected promise is returned', () => {
                // arrange
                spyOn(controller, 'isDataValid').and.returnValue(true);
                spyOn(controller, 'getAzureUploadUrl').and.returnValue(getAzureUploadUrlDefferedMock.promise);
                spyOn(controller, 'uploadFile').and.returnValue(uploadFileDefferedMock.promise);
                spyOn(controller, 'createAttachment');

                //act
                var resultIsSucces: boolean;
                var azureUrlContainerMock = { externalDocumentId: 'testExternalDocId', url: 'http:\\test.com' };

                controller.uploadAttachment(activityIdMock)
                    .then(() => { resultIsSucces = true; }, () => { resultIsSucces = false; });

                getAzureUploadUrlDefferedMock.reject();
                uploadFileDefferedMock.resolve(azureUrlContainerMock.externalDocumentId);

                $scope.$apply();

                // assert
                expect(resultIsSucces).toBeFalsy();
                expect(controller.uploadFile).not.toHaveBeenCalled();
                expect(controller.createAttachment).not.toHaveBeenCalled();
            });

            it('and uploadFile fails then createAttachment is not called and rejected promise is returned', () => {
                // arrange
                spyOn(controller, 'isDataValid').and.returnValue(true);
                spyOn(controller, 'getAzureUploadUrl').and.returnValue(getAzureUploadUrlDefferedMock.promise);
                spyOn(controller, 'uploadFile').and.returnValue(uploadFileDefferedMock.promise);
                spyOn(controller, 'createAttachment');

                //act
                var resultIsSucces: boolean;
                var azureUrlContainerMock = { externalDocumentId: 'testExternalDocId', url: 'http:\\test.com' };

                controller.uploadAttachment(activityIdMock)
                    .then(() => { resultIsSucces = true; }, () => { resultIsSucces = false; });

                getAzureUploadUrlDefferedMock.resolve(azureUrlContainerMock);
                uploadFileDefferedMock.reject();

                $scope.$apply();

                // assert
                expect(resultIsSucces).toBeFalsy();
                expect(controller.createAttachment).not.toHaveBeenCalled();
            });

            it('and all promises resolve successfully then createAttachment is called and promise resolved with data is returned', () => {
                // arrange
                spyOn(controller, 'isDataValid').and.returnValue(true);
                spyOn(controller, 'getAzureUploadUrl').and.returnValue(getAzureUploadUrlDefferedMock.promise);
                spyOn(controller, 'uploadFile').and.returnValue(uploadFileDefferedMock.promise);
                controller.file = <File>{};

                var createdAttachmentMock = TestHelpers.AttachmentGenerator.generate();
                spyOn(controller, 'createAttachment').and.returnValue(createdAttachmentMock);

                //act
                var resultIsSucces: boolean;
                var result: Business.Attachment;
                var azureUrlContainerMock = { externalDocumentId: 'testExternalDocId', url: 'http:\\test.com' };

                controller.uploadAttachment(activityIdMock)
                    .then((uploadAttachmentResult) => {
                        resultIsSucces = true;
                        result = uploadAttachmentResult;
                    }, () => {
                        resultIsSucces = false;
                    });

                getAzureUploadUrlDefferedMock.resolve(azureUrlContainerMock);
                uploadFileDefferedMock.resolve(azureUrlContainerMock.externalDocumentId);

                $scope.$apply();

                // assert
                expect(resultIsSucces).toBeTruthy();
                expect(result).toBe(createdAttachmentMock);
                expect(controller.createAttachment).toHaveBeenCalledWith(azureUrlContainerMock.externalDocumentId);
            });
        });

    });
}