/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateActivityResource implements Dto.IUpdateActivityResource {
        id: string = '';
        activityStatusId: string = '';
        marketAppraisalPrice: number = null;
        recommendedPrice: number = null;
        vendorEstimatedPrice: number = null;

        constructor(activity?: Business.Activity) {
            if (activity) {
                this.id = activity.id;
                this.activityStatusId = activity.activityStatusId;
                this.marketAppraisalPrice = activity.marketAppraisalPrice;
                this.recommendedPrice = activity.recommendedPrice;
                this.vendorEstimatedPrice = activity.vendorEstimatedPrice;
            }
        }
    }
}