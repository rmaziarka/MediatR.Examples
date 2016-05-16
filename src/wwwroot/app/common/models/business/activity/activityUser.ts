/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityUser {
        activityId: string = "";
        activity: Activity = null;
        userId: string = "";
        user: User = null;
        userType: Enums.NegotiatorTypeEnum = null;
        
        constructor(activityUser: Dto.IActivityUser) {
            if(activityUser) {
	            angular.extend(this, activityUser);
            }
        }
    }
}