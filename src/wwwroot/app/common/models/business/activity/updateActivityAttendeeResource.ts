/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateActivityAttendeeResource {
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