/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
	export class CreateActivityAttachmentResource  {
		attachment: CreateAttachmentResource;
		activityId : string;

		constructor(activityId: string, attachment: Attachment){
			this.attachment = new CreateAttachmentResource(attachment);
			this.activityId = activityId;
		}
	}
}