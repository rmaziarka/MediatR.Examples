/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    import Dto = Common.Models.Dto;
    import CartListOrder = Common.Component.ListOrder;

    export class AttachmentsManagerController {
        // bindings
        data: IAttachmentsManagerData;
        onSaveAttachmentForEntity: (obj: { attachment: AttachmentUploadCardModel }) => ng.IPromise<Dto.IAttachment>;

        // controller
        attachmentsCartListOrder: CartListOrder = new CartListOrder('createdDate', true);

        isAttachmentPreviewPanelVisible: boolean;
        isAttachmentUploadPanelVisible: boolean;
        isAttachmentUploadPanelBusy: boolean = false;
        title: string;
        filesNumberLimit: number;

        selectedAttachment: Common.Models.Business.Attachment = <Common.Models.Business.Attachment>{};

        constructor(
            private dataAccessService: Services.DataAccessService,
            private eventAggregator: Antares.Core.EventAggregator) {
            
            eventAggregator.with(this)
                .subscribe(Common.Component.BusySidePanelEvent, (event: Common.Component.BusySidePanelEvent) => {
                    this.isAttachmentUploadPanelBusy = event.isBusy;
                });
        }

        saveAttachment = (attachment: AttachmentUploadCardModel) => {
            return this.onSaveAttachmentForEntity({ attachment: attachment });
        }

        showAttachmentAdd = () =>{
            this.eventAggregator.publish(new OpenAttachmentUploadPanelEvent());
        }

        showAttachmentPreview = (attachment: Common.Models.Business.Attachment) => {
            this.selectedAttachment = attachment;
            this.eventAggregator.publish(new OpenAttachmentPreviewPanelEvent());
        }
    }

    angular.module('app').controller('AttachmentsManagerController', AttachmentsManagerController);
}