/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateActivityUserResource implements Dto.IUpdateActivityUserResource {
        id: string = '';
        callDate: Date = null;

        constructor(activityUser?: Business.ActivityUser) {
            if (activityUser) {
                this.id = activityUser.userId;
                this.callDate = activityUser.callDate;
            }
        }
    }
}