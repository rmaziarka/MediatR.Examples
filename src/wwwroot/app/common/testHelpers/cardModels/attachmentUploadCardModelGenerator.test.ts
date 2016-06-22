/// <reference path="../../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import AttachmentUploadCardModel = Common.Component.Attachment.AttachmentUploadCardModel;

    export class AttachmentUploadCardModelGenerator {
        public static generate(specificData?: any): AttachmentUploadCardModel {

            var attachment = new AttachmentUploadCardModel();
            attachment.externalDocumentId = AttachmentUploadCardModelGenerator.makeRandom('attachmentExternalDocumentId');
            attachment.fileName = AttachmentUploadCardModelGenerator.makeRandom('attachmentFileName');
            attachment.size = _.random(1, 100000);
            attachment.documentTypeId = AttachmentUploadCardModelGenerator.makeRandom('attachmentFileTypeId');
            //createdDate: new Date(),
            //user: new Business.User()

            return angular.extend(attachment, specificData || {});
        }

        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}