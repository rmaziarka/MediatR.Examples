/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;
    
    export class ActivityAppraisalMeetingCommandPart {
        start: string;
        end: string;
        invitationText: string;

        constructor(appraisalMeeting?: Business.ActivityAppraisalMeeting) {
            if (appraisalMeeting) {
                this.start = moment(appraisalMeeting.appraisalMeetingStart).toDate().toUTCString();
                this.end = moment(appraisalMeeting.appraisalMeetingEnd).toDate().toUTCString();
                this.invitationText = appraisalMeeting.appraisalMeetingInvitationText;
            }
        }
    }
}