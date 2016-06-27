/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class AttachmentPreviewPanelController extends Common.Component.BaseSidePanelController {
        // bindings
        entityId: string;
        entityType: Enums.EntityTypeEnum;
        attachment: Common.Models.Business.Attachment;
    }

    angular.module('app').controller('AttachmentPreviewPanelController', AttachmentPreviewPanelController);
};