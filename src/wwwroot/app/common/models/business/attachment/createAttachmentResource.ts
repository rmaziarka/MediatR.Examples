/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateAttachmentResource implements Dto.ICreateAttachmentResource  {
        fileName: string = '';
        documentTypeId: string = '';
        size: number = null;
        externalDocumentId: string = '';

        constructor(attachment: Attachment) {
            this.fileName = attachment.fileName;
            this.documentTypeId = attachment.documentTypeId;
            this.size = attachment.size;
            this.externalDocumentId = attachment.externalDocumentId;
        }
    }
}