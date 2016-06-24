/// <reference path="../typings/_all.d.ts" />

module Antares.Property.Command {
    export class PropertyAttachmentSaveCommand {
        attachment: Antares.Common.Component.Attachment.AttachmentSaveCommand;
		propertyId : string;

        constructor(propertyId: string, attachment: Antares.Common.Component.Attachment.AttachmentUploadCardModel){
            this.attachment = new Antares.Common.Component.Attachment.AttachmentSaveCommand(attachment);
            this.propertyId = propertyId;
		}
	}
}