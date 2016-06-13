/// <reference path="../typings/_all.d.ts" />

module Antares.Common.Component.Attachment {
    export class ActivityAttachmentSaveCommand {
        attachment: Antares.Common.Component.Attachment.AttachmentSaveCommand;
		activityId : string;

        constructor(activityId: string, attachment: Antares.Common.Component.Attachment.AttachmentUploadCardModel){
            this.attachment = new Antares.Common.Component.Attachment.AttachmentSaveCommand(attachment);
			this.activityId = activityId;
		}
	}
}