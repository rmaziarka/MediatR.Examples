/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    import Business = Antares.Common.Models.Business;

    export class AttachmentUploadCardModel {
        id: string = '';
        fileName: string = '';
        documentTypeId: string = '';
        size: number = null;
        externalDocumentId: string = '';
        createdDate: Date = null;
        user: Business.User = null;
    }

}