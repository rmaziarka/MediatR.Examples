/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Viewing implements Dto.IViewing {
        id: string = null;
        activityId: string = null;
        requirementId: string = null;
        negotiatorId: string = null;
        negotiator: User;
        startDate: Date | string = null;
        endDate: Date | string = null;
        invitationText: string;
        postviewingComment: string;
        attendeesIds: string[];
        attendees: Contact[];
        day: string;
        activity: Dto.IActivity;   

        constructor(viewing?: Dto.IViewing) {
            if (viewing) {
                angular.extend(this, viewing);
            this.startDate = Core.DateTimeUtils.convertDateToUtc(viewing.startDate);
            this.endDate = Core.DateTimeUtils.convertDateToUtc(viewing.endDate);
            this.day = (<string>viewing.startDate).substr(0, 10);
            }
        }
    }
}