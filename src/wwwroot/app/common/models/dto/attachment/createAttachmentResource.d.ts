declare module Antares.Common.Models.Dto {
    interface ICreateAttachmentResource {
        fileName: string;
        documentTypeId: string;
        size: number;
        externalDocumentId: string;
    }
}