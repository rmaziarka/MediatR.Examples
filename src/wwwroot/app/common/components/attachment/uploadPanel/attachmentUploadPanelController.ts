/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class AttachmentUploadPanelController extends Common.Component.BaseSidePanelController {
        // bindings
        entityType: Enums.EntityTypeEnum;
        enumDocumentType: Dto.EnumTypeCode;
        entityId: string;
        saveAttachmentForEntity: Function;

        constructor(private eventAggregator: Antares.Core.EventAggregator) {
            super();
        }

        save = (attachment: AttachmentUploadCardModel) => {
            this.saveAttachmentForEntity(attachment).then((attachmentDto: Dto.IAttachment) => {
                this.eventAggregator.publish(new AttachmentSavedEvent(attachmentDto));
                this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
            });
        }
    }

    angular.module('app').controller('AttachmentUploadPanelController', AttachmentUploadPanelController);
};