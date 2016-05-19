/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {

    export class Offer implements Dto.IOffer {
        id: string = null;
        statusId: string = null;
        activityId: string = null;
        requirementId: string = null;
        price: number;
        exchangeDate: Date;
        completionDate: Date;
        specialConditions: string;
        negotiatorId: string;
        negotiator: Business.User;
        activity: Business.Activity;
        requirement: Business.Requirement;
        status: Business.EnumTypeItem;
        offerDate: Date = null;

        constructor(offer?: Dto.IOffer) {
            angular.extend(this, offer);
            if (offer) {
                this.activity = new Activity(offer.activity);
                this.negotiator = new User(offer.negotiator);
            }
        }
    }
}