/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateActivityResource implements Dto.IUpdateActivityResource {
        id: string = '';
        activityTypeId: string = '';
        activityStatusId: string = '';
        marketAppraisalPrice: number = null;
        recommendedPrice: number = null;
        vendorEstimatedPrice: number = null;
		askingPrice: number = null;
		shortLetPricePerWeek: number = null;

        leadNegotiator: UpdateActivityUserResource = null;
        secondaryNegotiators: UpdateActivityUserResource[] = [];

        departments: UpdateActivityDepartmentResource[];

        attendees: UpdateActivityAttendeeResource[] = [];

        constructor(activity?: Business.Activity) {
            if (activity) {
                this.id = activity.id;
                this.activityTypeId = activity.activityTypeId;
                this.activityStatusId = activity.activityStatusId;
                this.marketAppraisalPrice = activity.marketAppraisalPrice;
                this.recommendedPrice = activity.recommendedPrice;
                this.vendorEstimatedPrice = activity.vendorEstimatedPrice;
                //this.leadNegotiator = new UpdateActivityUserResource(activity.leadNegotiator);
                this.secondaryNegotiators = _.map(activity.secondaryNegotiator, (activityUser: Business.ActivityUser) => new UpdateActivityUserResource(activityUser));
                this.departments = _.map(activity.activityDepartments, (department: Business.ActivityDepartment) => new UpdateActivityDepartmentResource(department));
	            this.askingPrice = activity.askingPrice;
                this.shortLetPricePerWeek = activity.shortLetPricePerWeek;
                this.attendees = activity.attendees.map((attendee: Dto.IActivityAttendee) => new UpdateActivityAttendeeResource(attendee));
            }
        }
    }
}