/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class AttachmentGenerator {
        public static generateDto(specificData?: any): Dto.IAttachment {

            var attachment: Dto.IAttachment = {
                fileName: AttachmentGenerator.makeRandom('attachmentFileName'),
                documentTypeId: AttachmentGenerator.makeRandom('attachmentFileTypeId'),
                size: _.random(1, 100000),
                externalDocumentId: AttachmentGenerator.makeRandom('attachmentExternalDocumentId'),
                createdDate: new Date(),
                user: new Business.User()
            }

            return angular.extend(attachment, specificData || {});
        }

        public static generateManyDtos(n: number): Dto.IAttachment[] {
            return _.times(n, AttachmentGenerator.generateDto);
        }

        public static generateMany(n: number): Business.Attachment[] {
            return _.map<Dto.IAttachment, Business.Attachment>(AttachmentGenerator.generateManyDtos(n), (attachment: Dto.IAttachment) => { return new Business.Attachment(attachment); });
        }

        public static generate(specificData?: any): Business.Attachment {
            var attachment = new Business.Attachment(AttachmentGenerator.generateDto());
            return angular.extend(attachment, specificData || {});
        }


        private static makeRandom(text: string): string {
            return text + _.random(1, 1000000);
        }
    }
}