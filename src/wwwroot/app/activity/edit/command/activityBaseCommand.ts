/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;
    
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

        constructor(activity: Business.Activity){
            this.activityStatusId = activity.activityStatusId;
            this.marketAppraisalPrice = activity.marketAppraisalPrice;
            this.recommendedPrice = activity.recommendedPrice;
            this.vendorEstimatedPrice = activity.vendorEstimatedPrice;
            this.shortLetPricePerWeek = activity.shortLetPricePerWeek;
            this.askingPrice = activity.askingPrice;
            this.activityTypeId = activity.activityTypeId;
            this.leadNegotiator = new ActivityUserCommandPart(activity.leadNegotiator);
            this.secondaryNegotiators = activity.secondaryNegotiator.map((n: Business.ActivityUser) => new ActivityUserCommandPart(n));
            this.departments = activity.activityDepartments.map((d: Business.ActivityDepartment) => new ActivityDepartmentCommandPart(d));
            this.contactIds = activity.contacts.map((c: Business.Contact) => c.id);
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
    }
}