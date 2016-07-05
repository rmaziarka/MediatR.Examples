/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Dto = Common.Models.Dto;

    export class ActivityAttendeeCommandPart {
        userId: string = '';
        contactId: string = '';

        constructor(activityAttendee?: Dto.IActivityAttendee) {
            if (activityAttendee) {
                this.contactId = activityAttendee.contactId;
                this.userId = activityAttendee.userId;
            }
        }
    }
}