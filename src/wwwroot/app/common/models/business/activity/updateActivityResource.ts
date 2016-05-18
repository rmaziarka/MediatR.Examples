/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateActivityResource implements Dto.IUpdateActivityResource {
        id: string = '';
        activityTypeId: string = '';
        activityStatusId: string = '';
        marketAppraisalPrice: number = null;
        recommendedPrice: number = null;
        vendorEstimatedPrice: number = null;        
        leadNegotiatorId: string = '';
        secondaryNegotiatorIds: string[] = [];
        
        constructor(activity?: Business.Activity) {
            if (activity) {
                this.id = activity.id;
                this.activityTypeId = activity.activityTypeId;
                this.activityStatusId = activity.activityStatusId;
                this.marketAppraisalPrice = activity.marketAppraisalPrice;
                this.recommendedPrice = activity.recommendedPrice;
                this.vendorEstimatedPrice = activity.vendorEstimatedPrice;
                this.leadNegotiatorId = activity.leadNegotiator.userId;
                this.secondaryNegotiatorIds = _.map(activity.secondaryNegotiator, (activityUser: Business.ActivityUser) => activityUser.userId);
            }
        }
    }
}