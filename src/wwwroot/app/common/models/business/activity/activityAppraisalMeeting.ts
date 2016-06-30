/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityAppraisalMeeting {
        activityAppraisalMeetingStart: Date | string;
        activityAppraisalMeetingEnd: Date | string;
        invitationText: string;

        constructor(appraisalMeeting?: Dto.IActivityAppraisalMeeting) {
            if (appraisalMeeting) {
                this.activityAppraisalMeetingStart = moment(appraisalMeeting.activityAppraisalMeetingStart).toDate();
                this.activityAppraisalMeetingEnd = moment(appraisalMeeting.activityAppraisalMeetingEnd).toDate();
                this.invitationText = appraisalMeeting.invitationText;
            }
        }
    }
}