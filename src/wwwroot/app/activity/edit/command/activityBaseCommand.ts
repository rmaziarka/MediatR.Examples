﻿/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    
    export class ActivityBaseCommand implements IActivityBaseCommand {
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
        contactIds: string[];
        sourceId: string = '';
        sourceDescription: string = '';
        sellingReasonId: string = '';
        attendees: Business.UpdateActivityAttendeeResource[];
        pitchingThreats: string = '';
        appraisalMeeting: ActivityAppraisalMeetingCommandPart = null;
        accessDetails: Dto.IActivityAccessDetails = null;

        constructor(activity: Business.Activity){
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
            this.attendees = activity.attendees.map((a: Dto.IActivityAttendee) => new Business.UpdateActivityAttendeeResource(a));
            this.appraisalMeeting = new ActivityAppraisalMeetingCommandPart(activity.appraisalMeeting);
            this.accessDetails = {
                accessArrangements: activity.accessDetails.accessArrangements,
                keyNumber: activity.accessDetails.keyNumber
            }
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
        contactIds: string[];
        sourceId: string;
        sourceDescription: string;
        sellingReasonId: string;
        accessDetails: Dto.IActivityAccessDetails;
        pitchingThreats: string;
        attendees: Business.UpdateActivityAttendeeResource[];
        appraisalMeeting: ActivityAppraisalMeetingCommandPart;
    }
}