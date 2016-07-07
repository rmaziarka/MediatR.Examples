/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IActivityAppraisalMeetingDateControlConfig extends Antares.Common.Models.Dto.IControlConfig {
        appraisalMeetingStart: Antares.Common.Models.Dto.IFieldConfig;
        appraisalMeetingEnd: Antares.Common.Models.Dto.IFieldConfig;
    }
}