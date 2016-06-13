/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityViewController = Activity.View.ActivityViewController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    xdescribe('Given activity view controller', () => {
        var $scope: ng.IScope,
            q: ng.IQService,
            controller: ActivityViewController;

        describe('when new attachment is being added', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();
            var $http: ng.IHttpBackendService;

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any,
                $httpBackend: ng.IHttpBackendService ) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                var bindings = { activity: activityMock };
                controller = <ActivityViewController>$controller('activityViewController', { $scope: $scope }, bindings);
            }));

            it('when saveAttachment is called then POST request is sent with attachment data', () =>{
                var requestData: Dto.ICreateActivityAttachmentResource;
                var attachment: Business.Attachment = TestHelpers.AttachmentGenerator.generate();

                $http.expectPOST(/\/api\/activities\/[0-9a-zA-Z]*\/attachments/, (data: string) => {
                    requestData = JSON.parse(data);
                    return true;
                }).respond(201, {});

                // act
                controller.saveAttachment(attachment);
                $http.flush();

                // assert
                expect(requestData).toBeDefined();
                expect(requestData.activityId).toBe(activityMock.id);
                expect(requestData.attachment).toBeDefined();
                expect(requestData.attachment.documentTypeId).toBe(attachment.documentTypeId);
                expect(requestData.attachment.fileName).toBe(attachment.fileName);
                expect(requestData.attachment.size).toBe(attachment.size);
                expect(requestData.attachment.externalDocumentId).toBe(attachment.externalDocumentId);
            });

            it('when addSavedAttachmentToList is called then new attachment should be added to the list', () => {
                var attachment: Dto.IAttachment = TestHelpers.AttachmentGenerator.generateDto();
                spyOn(controller, 'hidePanels');

                // act
                controller.addSavedAttachmentToList(attachment);

                // assert
                var attachments: Array<Business.Attachment> = controller.activity.attachments.filter((a: Business.Attachment) => { return a.id === attachment.id });
                expect(attachments.length).toBe(1);

                var attachmentAddedToList: Business.Attachment = attachments[0];
                expect(attachmentAddedToList).toBeDefined();
                expect(attachmentAddedToList.documentTypeId).toBe(attachment.documentTypeId);
                expect(attachmentAddedToList.fileName).toBe(attachment.fileName);
                expect(attachmentAddedToList.size).toBe(attachment.size);
                expect(attachmentAddedToList.externalDocumentId).toBe(attachment.externalDocumentId);
                expect(attachment.user).toBeDefined();
                expect(attachment.user.id).toBe(attachment.user.id);

            });
        });

        describe('when saveActivityAttachment is called', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();

            var uploadAttachmentDefferedMock: ng.IDeferred<Business.Attachment>;
            var saveAttachmentDefferedMock: ng.IDeferred<Dto.IAttachment>;

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any,
                $q: ng.IQService) => {

                // init
                $scope = $rootScope.$new();
                q = $q;

                uploadAttachmentDefferedMock = q.defer();
                saveAttachmentDefferedMock = q.defer();

                var bindings = { activity : activityMock };
                controller = <ActivityViewController>$controller('activityViewController', { $scope: $scope }, bindings);
            }));

            it('then uploadAttachment is called with activity id', () => {
                // arrange
                var activityIdFromUpload: string;
                spyOn(controller.components, 'activityAttachmentAdd').and.returnValue({
                    uploadAttachment: (activityId: string) => {
                        activityIdFromUpload = activityId;
                        return uploadAttachmentDefferedMock.promise;
                    }
                });

                //act
                controller.saveActivityAttachment();

                // assert
                expect(activityIdFromUpload).toBe(activityMock.id);
            });

            it('and uploadAttachment fails then saveAttachment and addSavedAttachmentToList are not called but finally statement is called', () => {
                // arrange
                spyOn(controller.components, 'activityAttachmentAdd').and.returnValue({
                    uploadAttachment: (activityId: string) => {
                        return uploadAttachmentDefferedMock.promise;
                    }
                });
                spyOn(controller, 'saveAttachment').and.returnValue(saveAttachmentDefferedMock.promise);
                spyOn(controller, 'addSavedAttachmentToList');

                //act
                var savedAttachmentMock = TestHelpers.AttachmentGenerator.generateDto();

                controller.saveActivityAttachmentBusy = true;
                controller.saveActivityAttachment();

                uploadAttachmentDefferedMock.reject();
                saveAttachmentDefferedMock.resolve(savedAttachmentMock);

                $scope.$apply();

                // assert
                expect(controller.saveAttachment).not.toHaveBeenCalled();
                expect(controller.addSavedAttachmentToList).not.toHaveBeenCalled();
                expect(controller.saveActivityAttachmentBusy).toBe(false);
            });

            it('and saveAttachment fails then addSavedAttachmentToList are not called but finally statement is called', () => {
                // arrange
                spyOn(controller.components, 'activityAttachmentAdd').and.returnValue({
                    uploadAttachment: (activityId: string) => {
                        return uploadAttachmentDefferedMock.promise;
                    }
                });
                spyOn(controller, 'saveAttachment').and.returnValue(saveAttachmentDefferedMock.promise);
                spyOn(controller, 'addSavedAttachmentToList');

                //act
                var createdAttachmentMock = TestHelpers.AttachmentGenerator.generate();

                controller.saveActivityAttachmentBusy = true;
                controller.saveActivityAttachment();

                uploadAttachmentDefferedMock.resolve(createdAttachmentMock);
                saveAttachmentDefferedMock.reject();

                $scope.$apply();

                // assert
                expect(controller.saveAttachment).toHaveBeenCalledWith(createdAttachmentMock);
                expect(controller.addSavedAttachmentToList).not.toHaveBeenCalled();
                expect(controller.saveActivityAttachmentBusy).toBe(false);
            });

            it('and all promises resolve successfully then addSavedAttachmentToList is called and finally statement is called', () => {
                // arrange
                spyOn(controller.components, 'activityAttachmentAdd').and.returnValue({
                    uploadAttachment: (activityId: string) => {
                        return uploadAttachmentDefferedMock.promise;
                    }
                });
                spyOn(controller, 'saveAttachment').and.returnValue(saveAttachmentDefferedMock.promise);
                spyOn(controller, 'addSavedAttachmentToList');

                //act
                var createdAttachmentMock = TestHelpers.AttachmentGenerator.generate();
                var savedAttachmentMock = TestHelpers.AttachmentGenerator.generateDto();

                controller.saveActivityAttachmentBusy = true;
                controller.saveActivityAttachment();

                uploadAttachmentDefferedMock.resolve(createdAttachmentMock);
                saveAttachmentDefferedMock.resolve(savedAttachmentMock);

                $scope.$apply();

                // assert
                expect(controller.saveAttachment).toHaveBeenCalledWith(createdAttachmentMock);
                expect(controller.addSavedAttachmentToList).toHaveBeenCalledWith(savedAttachmentMock);
                expect(controller.saveActivityAttachmentBusy).toBe(false);
            });
        });
	});
}