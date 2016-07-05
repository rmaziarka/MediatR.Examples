/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    
    export class ActivityBaseCommand implements IActivityBaseCommand {
        activityStatusId: string = '';
        marketAppraisalPrice: number;
        recommendedPrice: number;
        vendorEstimatedPrice: number;
        shortLetPricePerWeek: number;
        askingPrice: number;
        activityTypeId: string ='';
        leadNegotiator: ActivityUserCommandPart;
        secondaryNegotiators: ActivityUserCommandPart[];
        departments: IActivityDepartmentCommandPart[];
        contactIds: string[];
        sourceId: string = null;
        sourceDescription: string = '';
        sellingReasonId: string = '';
        appraisalMeetingAttendeesList: Business.UpdateActivityAttendeeResource[];
        pitchingThreats: string = '';
        appraisalMeetingStart: Date = null;
        appraisalMeetingEnd: Date = null;
        appraisalMeetingInvitationText: string = '';
        keyNumber: string = '';
        accessArrangements: string = '';
        kfValuationPrice: number = null;
        agreedInitialMarketingPrice: number = null;
        vendorValuationPrice: number = null;
        shortKfValuationPrice: number = null;
        shortAgreedInitialMarketingPrice: number = null;
        shortVendorValuationPrice: number = null;
        longKfValuationPrice: number = null;
        longAgreedInitialMarketingPrice: number = null;
        longVendorValuationPrice: number = null;
        disposalTypeId: string = '';
        decorationId: string = '';
        serviceChargeAmount: number = null;
        serviceChargeNote: string = '';
        groundRentAmount: number = null;
        groundRentNote: string = '';
        otherCondition: string = '';

        constructor(activity: Activity.ActivityEditModel) {
            this.activityStatusId = activity.activityStatusId;
            this.shortLetPricePerWeek = activity.shortLetPricePerWeek;
            this.askingPrice = activity.askingPrice;
            this.activityTypeId = activity.activityTypeId;
            this.sourceId = activity.sourceId;
            this.sourceDescription = activity.sourceDescription;
            this.sellingReasonId = activity.sellingReasonId;
            this.pitchingThreats = activity.pitchingThreats;
            this.leadNegotiator = new ActivityUserCommandPart(activity.leadNegotiator);
            this.secondaryNegotiators = activity.secondaryNegotiator.map((n: Business.ActivityUser) => new ActivityUserCommandPart(n));
            this.departments = activity.activityDepartments.map((d: Business.ActivityDepartment) => new ActivityDepartmentCommandPart(d));
            this.contactIds = activity.contacts.map((c: Business.Contact) => c.id);
            this.appraisalMeetingAttendeesList = activity.appraisalMeetingAttendees.map((a: Dto.IActivityAttendee) => new Business.UpdateActivityAttendeeResource(a));

            this.appraisalMeetingStart = activity.appraisalMeeting.appraisalMeetingStart ? Core.DateTimeUtils.createDateAsUtc(activity.appraisalMeeting.appraisalMeetingStart) : null;
            this.appraisalMeetingEnd = activity.appraisalMeeting.appraisalMeetingEnd ? Core.DateTimeUtils.createDateAsUtc(activity.appraisalMeeting.appraisalMeetingEnd) : null;
            this.appraisalMeetingInvitationText = activity.appraisalMeeting.appraisalMeetingInvitationText;
            this.keyNumber = activity.accessDetails.keyNumber;
            this.accessArrangements = activity.accessDetails.accessArrangements;
            this.kfValuationPrice = activity.kfValuationPrice;
            this.agreedInitialMarketingPrice = activity.agreedInitialMarketingPrice;
            this.vendorValuationPrice = activity.vendorValuationPrice;
            this.shortKfValuationPrice = activity.shortKfValuationPrice;
            this.shortAgreedInitialMarketingPrice = activity.shortAgreedInitialMarketingPrice;
            this.shortVendorValuationPrice = activity.shortVendorValuationPrice;
            this.longKfValuationPrice = activity.longKfValuationPrice;
            this.longAgreedInitialMarketingPrice = activity.longAgreedInitialMarketingPrice;
            this.longVendorValuationPrice = activity.longVendorValuationPrice;
            this.disposalTypeId = activity.disposalTypeId;
            this.decorationId = activity.decorationId;
            this.serviceChargeAmount = activity.serviceChargeAmount;
            this.serviceChargeNote = activity.serviceChargeNote;
            this.groundRentAmount = activity.groundRentAmount;
            this.groundRentNote = activity.groundRentNote;
            this.otherCondition = activity.otherCondition;

        }
    }
    
    export interface IActivityBaseCommand {
        activityStatusId: string;
        shortLetPricePerWeek: number;
        askingPrice: number;
        activityTypeId: string;
        leadNegotiator: ActivityUserCommandPart;
        secondaryNegotiators: ActivityUserCommandPart[];
        departments: IActivityDepartmentCommandPart[];
        appraisalMeetingAttendeesList: Business.UpdateActivityAttendeeResource[];
        contactIds: string[];
        sourceId: string;
        sourceDescription: string;
        sellingReasonId: string;
        appraisalMeetingStart: Date;
        appraisalMeetingEnd: Date;
        appraisalMeetingInvitationText: string;
        keyNumber: string;
        accessArrangements: string;
        pitchingThreats: string;
        kfValuationPrice: number;
        agreedInitialMarketingPrice: number;
        vendorValuationPrice: number;
        shortKfValuationPrice: number;
        shortAgreedInitialMarketingPrice: number;
        shortVendorValuationPrice: number;
        longKfValuationPrice: number;
        longAgreedInitialMarketingPrice: number;
        longVendorValuationPrice: number;
        disposalTypeId: string;
        decorationId: string;
        serviceChargeAmount: number;
        serviceChargeNote: string;
        groundRentAmount: number;
        groundRentNote: string;
        otherCondition: string;
    }
}