/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AttachmentUploadPanelController = Common.Component.Attachment.AttachmentUploadPanelController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    describe('Given attachment upload panel controller', () => {
        var $scope: ng.IScope,
            q: ng.IQService,
            evtAggregator: Antares.Core.EventAggregator,
            controller: AttachmentUploadPanelController;

        var onSaveAttachmentDeffered: ng.IDeferred<Dto.IAttachment>;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $q: ng.IQService,
            $controller: ng.IControllerService,
            eventAggregator: Antares.Core.EventAggregator) => {

            // init
            $scope = $rootScope.$new();
            evtAggregator = eventAggregator;

            onSaveAttachmentDeffered = $q.defer();

            var bindings: any = { onSaveAttachment: () => onSaveAttachmentDeffered.promise};
            controller = <AttachmentUploadPanelController>$controller('AttachmentUploadPanelController', {}, bindings);
        }));

        describe('when endUploadAndSave is called', () => {
            it('and onSaveAttachment is resolved as success then proper events should be published', () => {
                // arrange
                var attachmentModel = TestHelpers.AttachmentUploadCardModelGenerator.generate();
                var attachmentDto = TestHelpers.AttachmentGenerator.generateDto();

                var isClosedCalled = false;
                var busyCalledValue: boolean = null;
                var attachmentSavedEventCalledValue: Dto.IAttachment = null;

                evtAggregator.with(controller)
                    .subscribe(Common.Component.Attachment.AttachmentSavedEvent, (event: Common.Component.Attachment.AttachmentSavedEvent) => {
                        attachmentSavedEventCalledValue = event.attachmentSaved;
                    });

                evtAggregator.with(controller)
                    .subscribe(Common.Component.CloseSidePanelEvent, (event: Common.Component.CloseSidePanelEvent) => {
                        isClosedCalled = true;
                    });

                evtAggregator.with(controller)
                    .subscribe(Common.Component.BusySidePanelEvent, (event: Common.Component.BusySidePanelEvent) => {
                        busyCalledValue = event.isBusy;
                    });

                //act
                controller.endUploadAndSave(attachmentModel);
                onSaveAttachmentDeffered.resolve(attachmentDto);
                $scope.$apply();

                // assert
                expect(isClosedCalled).toBe(true);
                expect(attachmentSavedEventCalledValue).toBe(attachmentDto);
                expect(busyCalledValue).toBe(false);
            });

            it('and onSaveAttachment is resolved as error then proper events should be published', () => {
                // arrange
                var attachmentModel = TestHelpers.AttachmentUploadCardModelGenerator.generate();
                var attachmentDto = TestHelpers.AttachmentGenerator.generateDto();

                var isClosedCalled = false;
                var busyCalledValue: boolean = null;
                var attachmentSavedEventCalledValue: Dto.IAttachment = null;

                evtAggregator.with(controller)
                    .subscribe(Common.Component.Attachment.AttachmentSavedEvent, (event: Common.Component.Attachment.AttachmentSavedEvent) => {
                        attachmentSavedEventCalledValue = event.attachmentSaved;
                    });

                evtAggregator.with(controller)
                    .subscribe(Common.Component.CloseSidePanelEvent, (event: Common.Component.CloseSidePanelEvent) => {
                        isClosedCalled = true;
                    });

                evtAggregator.with(controller)
                    .subscribe(Common.Component.BusySidePanelEvent, (event: Common.Component.BusySidePanelEvent) => {
                        busyCalledValue = event.isBusy;
                    });

                //act
                controller.endUploadAndSave(attachmentModel);
                onSaveAttachmentDeffered.reject();
                $scope.$apply();

                // assert
                expect(isClosedCalled).toBe(false);
                expect(attachmentSavedEventCalledValue).toBe(null);
                expect(busyCalledValue).toBe(false);
            });
        });

        describe('when panelShown is called', () => {
            it('then attachmentClear is set to new object', () => {
                // arrange
                controller.attachmentClear = {};

                //act
                controller.panelShown();

                // assert
                expect(controller.attachmentClear).toBe(controller.attachmentClear);
            });
        });

        describe('when startUpload is called', () => {
            it('then BusySidePanelEvent event should be published', () => {
                // arrange
                var busyCalledValue: boolean = null;
                evtAggregator.with(controller)
                    .subscribe(Common.Component.BusySidePanelEvent, (event: Common.Component.BusySidePanelEvent) => {
                        busyCalledValue = event.isBusy;
                    });

                //act
                controller.startUpload();
                $scope.$apply();

                // assert
                expect(busyCalledValue).toBe(true);
            });
        });

        describe('when endUpload is called', () => {
            it('then BusySidePanelEvent event should be published', () => {
                // arrange
                var busyCalledValue: boolean = null;
                evtAggregator.with(controller)
                    .subscribe(Common.Component.BusySidePanelEvent, (event: Common.Component.BusySidePanelEvent) => {
                        busyCalledValue = event.isBusy;
                    });

                //act
                controller.endUpload();
                $scope.$apply();

                // assert
                expect(busyCalledValue).toBe(false);
            });
        });
    });
}