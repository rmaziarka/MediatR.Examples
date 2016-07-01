/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityAppraisalMeetingGenerator {
        public static generateDto(): Dto.IActivityAppraisalMeeting{

            var activityAppraisalMeeting: Dto.IActivityAppraisalMeeting = {
                activityAppraisalMeetingStart: moment().toString(),
                activityAppraisalMeetingEnd: moment().toString(),
                invitationText: StringGenerator.generate()
        }

            return activityAppraisalMeeting;
        }

        public static generateManyDtos(n: number): Dto.IActivityAppraisalMeeting[] {
            return _.times(n, () => { return ActivityAppraisalMeetingGenerator.generateDto()});
        }

        public static generateMany(n: number): Business.ActivityAppraisalMeeting[] {
            return _.map<Dto.IActivityAppraisalMeeting, Business.ActivityAppraisalMeeting>(ActivityAppraisalMeetingGenerator.generateManyDtos(n), (aam: Dto.IActivityAppraisalMeeting) => { return new Business.ActivityAppraisalMeeting(aam); });
        }

        public static generate(): Business.ActivityAppraisalMeeting {
            return new Business.ActivityAppraisalMeeting(ActivityAppraisalMeetingGenerator.generateDto());
        }
    }
}