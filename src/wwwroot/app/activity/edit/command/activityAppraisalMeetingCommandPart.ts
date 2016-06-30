/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;
    
    export class ActivityAppraisalMeetingCommandPart {
        activityAppraisalMeetingStart: string;
        activityAppraisalMeetingEnd: string;
        invitationText: string;

        constructor(appraisalMeeting?: Business.ActivityAppraisalMeeting) {
            if (appraisalMeeting) {
                this.activityAppraisalMeetingStart = moment(appraisalMeeting.activityAppraisalMeetingStart).toDate().toUTCString();
                this.activityAppraisalMeetingEnd = moment(appraisalMeeting.activityAppraisalMeetingEnd).toDate().toUTCString();
                this.invitationText = appraisalMeeting.invitationText;
            }
        }
    }
}