declare module Antares.Common.Models.Dto {
    interface IAttachment {
        fileName: string;
        fileTypeId: string;
        size: number;
        externalDocumentId: string;
        createdDate?: Date;
        user?: IUser;
    }
}