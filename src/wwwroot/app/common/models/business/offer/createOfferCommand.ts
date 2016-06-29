/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateOfferCommand {
        activityId: string;
        requirementId: string;
        offerTypeId: string;
        statusId: string;
        price: number;
        pricePerWeek: number;
        offerDate: Date;
        exchangeDate: Date;
        completionDate: Date;
        specialConditions: string;

        constructor(model?: CreateOfferCommand){
            model = model || <CreateOfferCommand>{};
            this.activityId = model.activityId;
            this.requirementId = model.requirementId;
            this.offerTypeId = model.offerTypeId;
            this.statusId = model.statusId;
            this.price = model.price;
            this.pricePerWeek = model.pricePerWeek;
            this.offerDate = model.offerDate;
            this.exchangeDate = model.exchangeDate;
            this.completionDate = model.completionDate;
            this.specialConditions = model.specialConditions;
        }
    }
}