/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    export class AttachmentSaveCommand {
        fileName: string = '';
        documentTypeId: string = '';
        size: number = null;
        externalDocumentId: string = '';

        constructor(attachment: AttachmentUploadCardModel) {
            this.fileName = attachment.fileName;
            this.documentTypeId = attachment.documentTypeId;
            this.size = attachment.size;
            this.externalDocumentId = attachment.externalDocumentId;
        }
    }
}