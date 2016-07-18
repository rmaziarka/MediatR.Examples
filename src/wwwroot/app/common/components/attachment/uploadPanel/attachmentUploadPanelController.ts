/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;

    export class AttachmentUploadPanelController extends Component.BaseSidePanelController {
        // bindings
        entityId: string;
        entityType: Enums.EntityTypeEnum;
        enumDocumentType: Dto.EnumTypeCode;
        onSaveAttachment: (obj: { attachment: AttachmentUploadCardModel }) => ng.IPromise<Dto.IAttachment>;

        attachmentClear: any;

        constructor(private eventAggregator: Core.EventAggregator) {
            super();
        }

        panelShown = () =>{
            this.attachmentClear = {};
        };

        startUpload = () => {
            this.eventAggregator.publish(new BusySidePanelEvent(true));
        }

        endUpload = () => {
            this.eventAggregator.publish(new BusySidePanelEvent(false));
        }

        endUploadAndSave = (attachment: AttachmentUploadCardModel) => {
            this.onSaveAttachment({ attachment: attachment })
                .then((attachmentDto: Dto.IAttachment) => {
                    this.eventAggregator.publish(new AttachmentSavedEvent(attachmentDto));
                    this.eventAggregator.publish(new Component.CloseSidePanelEvent());
                }).finally(() => {
                    this.eventAggregator.publish(new BusySidePanelEvent(false));
                });
        }
    }

    angular.module('app').controller('AttachmentUploadPanelController', AttachmentUploadPanelController);
};