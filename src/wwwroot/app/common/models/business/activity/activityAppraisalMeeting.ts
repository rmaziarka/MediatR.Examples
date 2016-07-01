/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityAppraisalMeeting {
        appraisalMeetingStart: Date | string = null;
        appraisalMeetingEnd: Date | string = null;
        appraisalMeetingInvitationText: string;

        constructor(activityAppraisalMeetingStart?: Date | string, activityAppraisalMeetingEnd?: Date | string, invitationText?: string) {
            this.appraisalMeetingStart = activityAppraisalMeetingStart ? moment(activityAppraisalMeetingStart).toDate() : '';
            this.appraisalMeetingEnd = activityAppraisalMeetingEnd ? moment(activityAppraisalMeetingEnd).toDate() : '';
            this.appraisalMeetingInvitationText = invitationText;
        }
    }
}