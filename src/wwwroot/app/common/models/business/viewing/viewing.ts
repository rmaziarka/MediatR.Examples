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
            }
        }
    }
}