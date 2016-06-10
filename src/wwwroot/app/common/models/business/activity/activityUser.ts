/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityUser {
        id: string ="";
        activityId: string = "";
        userId: string = "";
        user: User = null;
        userType: Dto.IEnumTypeItem;
        callDate: Date = null;

        constructor(activityUser?: Dto.IActivityUser) {
            if(activityUser) {
                angular.extend(this, activityUser);

                if (activityUser.callDate) {
                    this.callDate = moment(activityUser.callDate).toDate();
                }
            }
        }
    }
}