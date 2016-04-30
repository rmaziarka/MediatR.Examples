/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Viewing implements Dto.IViewing {
        id: string = null;
        activityId: string = null;
        requirementId: string = null;
        negotiatorId: string = null;
        startDate: Date = null;
        endDate: Date = null;
        invitationText: string;
        postviewingComment: string;
        attendeesIds: string[];

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