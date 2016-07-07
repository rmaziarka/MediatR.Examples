/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    declare var filesize: Filesize.IFilesize;

    export class Attachment {
        id: string = null;
        fileName: string = null;
        documentTypeId: string = null;
        size: number = null;
        externalDocumentId: string = null;
        createdDate: Date = null;
        user: User = null;

        constructor(attachment?: Dto.IAttachment) {
            if (attachment) {
                angular.extend(this, attachment);

                this.createdDate = new Date(<string>attachment.createdDate);
                this.user = new User(attachment.user);
            }
        }
    }
}