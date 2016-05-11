declare module Antares.Common.Models.Dto {
    interface ICreateActivityAttachmentResource {
        attachment: Dto.ICreateAttachmentResource;
        activityId: string;
    }
}