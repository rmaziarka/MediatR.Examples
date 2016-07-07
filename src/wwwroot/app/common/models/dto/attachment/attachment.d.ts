declare module Antares.Common.Models.Dto {
    interface IAttachment {
        id: string,
        fileName: string;
        documentTypeId: string;
        size: number;
        externalDocumentId: string;
        createdDate?: Date | string;
        user?: IUser;
    }
}