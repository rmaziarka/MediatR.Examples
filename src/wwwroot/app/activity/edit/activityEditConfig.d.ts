/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Activity {
    import Attributes = Antares.Attributes;

    interface IActivityEditConfig extends IActivityConfig {
        activityStatus: Attributes.IActivityStatusEditControlConfig;
        departments: Attributes.IActivityDepartmentsViewControlConfig;
        negotiators: Attributes.IActivityNegotiatorsControlConfig;
        property: Attributes.IPropertyViewControlConfig;
        marketAppraisalPrice: any;
        recommendedPrice: any;
        vendorEstimatedPrice: any;
        askingPrice: any;
        shortLetPricePerWeek: any;
        source: any;
        sourceDescription: any;
        sellingReason: any;
        pitchingThreats: any;
        keyNumber: any;
        accessArrangements: any;
        appraisalMeetingDate: any;
        appraisalMeetingAttendees: any;
        appraisalMeetingInvitation: any;
    }
}