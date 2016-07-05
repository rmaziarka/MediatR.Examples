/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityAppraisalMeeting {
        appraisalMeetingStart: Date | string = null;
        appraisalMeetingEnd: Date | string = null;
        appraisalMeetingInvitationText: string;

        constructor(activityAppraisalMeetingStart?: Date | string, activityAppraisalMeetingEnd?: Date | string, invitationText?: string) {
            this.appraisalMeetingStart = activityAppraisalMeetingStart ? Antares.Core.DateTimeUtils.convertDateToUtc(activityAppraisalMeetingStart) : '';
            this.appraisalMeetingEnd = activityAppraisalMeetingEnd ? Antares.Core.DateTimeUtils.convertDateToUtc(activityAppraisalMeetingEnd) : '';
            this.appraisalMeetingInvitationText = invitationText;
        }
    }
}