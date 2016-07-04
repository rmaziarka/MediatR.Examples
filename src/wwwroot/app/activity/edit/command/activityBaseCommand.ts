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

        constructor(activity: Activity.ActivityEditModel) {
            this.activityStatusId = activity.activityStatusId;
            this.marketAppraisalPrice = activity.marketAppraisalPrice;
            this.recommendedPrice = activity.recommendedPrice;
            this.vendorEstimatedPrice = activity.vendorEstimatedPrice;
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
        }
    }
    
    export interface IActivityBaseCommand {
        activityStatusId: string;
        marketAppraisalPrice: number;
        recommendedPrice: number;
        vendorEstimatedPrice: number;
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
    }
}