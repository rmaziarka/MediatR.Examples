/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityAppraisalMeetingGenerator {
        public static generateMany(n: number): Business.ActivityAppraisalMeeting[] {
            return _.times(n, () => { return ActivityAppraisalMeetingGenerator.generate() });
        }

        public static generate(): Business.ActivityAppraisalMeeting {
            return new Business.ActivityAppraisalMeeting(moment().toString(), moment().toString(), StringGenerator.generate());
        }
    }
}