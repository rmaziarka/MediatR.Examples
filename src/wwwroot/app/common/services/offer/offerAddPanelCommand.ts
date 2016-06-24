/// <reference path="../../../typings/_all.d.ts" />

module Antares.Services.Commands {
    import Dto = Common.Models.Dto;

    export class OfferAddPanelCommand implements IOfferAddPanelCommand {
        activityId: string;
        requirementId: string;
        statusId: string;
        price: number;
        pricePerWeek: number;
        offerDate: Date;
        exchangeDate: Date;
        completionDate: Date;
        specialConditions: string;

        constructor(model: Dto.IOffer) {
            this.activityId = model.activityId;
            this.requirementId = model.requirementId;
            this.statusId = model.statusId;
            this.price = model.price;
            this.pricePerWeek = model.pricePerWeek;
            this.offerDate = model.offerDate;
            this.exchangeDate = model.exchangeDate;
            this.completionDate = model.completionDate;
            this.specialConditions = model.specialConditions;
        }
    }

    export interface IOfferAddPanelCommand {
        activityId: string;
        requirementId: string;
        statusId: string;
        price: number;
        pricePerWeek: number;
        offerDate: Date;
        exchangeDate: Date;
        completionDate: Date;
        specialConditions: string;
    }
}