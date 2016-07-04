/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ActivityAppraisalMeetingViewControlController {
        // binding
        appraisalMeeting: Business.ActivityAppraisalMeeting;
        attendees: Dto.IActivityAttendee[];
        config: IActivityAppraisalMeetingViewControlConfig;

        attendeesModel: Models.ViewActivityAttendeeModel[];

        constructor() {
            this.attendeesModel = [];
            if (this.attendees) {
                this.attendeesModel = this.attendees.map((a: Dto.IActivityAttendee) => new Antares.Attributes.Models.ViewActivityAttendeeModel(a));
            }
        }

        public getAttendees = () : string => {
            return _.map(this.attendeesModel, (a: Models.ViewActivityAttendeeModel) => a.getName()).join(', ');
        }
    }

    angular.module('app').controller('ActivityAppraisalMeetingViewControlController', ActivityAppraisalMeetingViewControlController);
};