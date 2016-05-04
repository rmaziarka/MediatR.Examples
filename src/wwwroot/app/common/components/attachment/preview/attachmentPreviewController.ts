/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;

    export class AttachmentPreviewController {
        componentId: string;
        attachment: Business.Attachment = <Business.Attachment>{};

        constructor(
            private componentRegistry: Core.Service.ComponentRegistry) {

            componentRegistry.register(this, this.componentId);
        }

        setAttachment = (attachment: Business.Attachment) => {
            this.attachment = attachment;
        }
    }

    angular.module('app').controller('AttachmentPreviewController', AttachmentPreviewController);
}