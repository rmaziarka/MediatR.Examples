/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateActivityUserResource implements Dto.IUpdateActivityUserResource {
        userId: string = '';
        callDate: Date = null;

        constructor(activityUser?: Business.ActivityUser) {
            if (activityUser) {
                this.userId = activityUser.userId;
                this.callDate = Core.DateTimeUtils.createDateAsUtc(activityUser.callDate);
            }
        }
    }
}