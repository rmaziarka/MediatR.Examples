/// <reference path="../typings/_all.d.ts" />

module Antares.Requirement.Command {
    export class RequirementAttachmentSaveCommand {
        attachment: Common.Component.Attachment.AttachmentSaveCommand;
        requirementId: string;

        constructor(requirementId: string, attachment: Common.Component.Attachment.AttachmentUploadCardModel) {
            this.attachment = new Common.Component.Attachment.AttachmentSaveCommand(attachment);
            this.requirementId = requirementId;
        }
    }
}