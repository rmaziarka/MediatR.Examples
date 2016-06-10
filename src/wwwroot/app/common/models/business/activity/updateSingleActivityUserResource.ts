/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateSingleActivityUserResource implements Dto.IUpdateSingleActivityUserResource {
        id: string = '';
        callDate: Date = null;

        constructor(activityUser?: Business.ActivityUser) {
            if (activityUser) {
                this.id = activityUser.id;
                this.callDate = Core.DateTimeUtils.createDateAsUtc(activityUser.callDate);
            }
        }
    }
}