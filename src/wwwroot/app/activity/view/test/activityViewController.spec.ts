/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityViewController = Activity.View.ActivityViewController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    describe('Given activity view controller', () => {
        var $scope: ng.IScope,
            q: ng.IQService,
            controller: ActivityViewController;

        describe('when saveAttachment is called', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any) => {

                // init
                $scope = $rootScope.$new();

                var bindings = { activity: activityMock };
                controller = <ActivityViewController>$controller('activityViewController', { $scope: $scope }, bindings);
            }));

            //it('then POST request ius sent with attachmant data', () => {
            //});
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