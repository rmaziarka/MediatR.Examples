/// <reference path="../../../../typings/_all.d.ts" />

declare module Antares.Common.Component.Attachment {
    import Business = Common.Models.Business;
    import Enums = Antares.Common.Models.Enums;

    interface IAttachmentsManagerData {
        entityId: string;
        enumDocumentType: Antares.Common.Models.Dto.EnumTypeCode;
        entityType: Antares.Common.Models.Enums.EntityTypeEnum;
        attachments: Business.Attachment[];
        isUploadPanelVisible: Enums.SidePanelState;
        isPreviewPanelVisible: Enums.SidePanelState;
    }
}