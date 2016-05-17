/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    declare var moment: any;

    export class Viewing implements Dto.IViewing {
        id: string = null;
        activityId: string = null;
        requirementId: string = null;
        negotiatorId: string = null;
        negotiator: User;
        startDate: Date | string = null;
        endDate: Date | string = null;
        invitationText: string;
        postViewingComment: string;
        attendeesIds: string[];
        attendees: Contact[];
        day: string;
        activity: Business.Activity;
        requirement: Business.Requirement;

        constructor(viewing?: Dto.IViewing) {
            if (viewing) {
                angular.extend(this, viewing);
                this.day = Core.DateTimeUtils.getDatePart(viewing.startDate);
                if (viewing.requirement) {
                    this.requirement = new Requirement(viewing.requirement);
                }
                if (viewing.activity) {
                    this.activity = new Business.Activity(viewing.activity);
                }
                this.startDate = moment(viewing.startDate).toDate();
                this.endDate = moment(viewing.endDate).toDate();
            }
        }
    }
}