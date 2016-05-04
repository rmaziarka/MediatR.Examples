/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Attachment {
        fileName: string = '';
        fileTypeId: string = '';
        fileType: Dto.IEnumItem = null;
        size: number = null;
        externalDocumentId: string = '';
        createdDate: Date = null;
        user: User = null;
    }
}