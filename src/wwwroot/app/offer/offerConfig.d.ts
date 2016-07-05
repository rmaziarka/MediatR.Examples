/// <reference path="../typings/_all.d.ts" />

declare module Antares.Offer {
    interface IOfferConfig {
        [key: string]: any;
        statusId: Attributes.IOfferStatusControlConfig;
        offerDate: Attributes.IOfferOfferDateControlConfig;
        completionDate: Attributes.IOfferCompletionDateControlConfig;
        exchangeDate: Attributes.IOfferExchangeDateControlConfig;
        pricePerWeek: Attributes.IOfferPricePerWeekControlConfig;
        price: Attributes.IOfferPriceControlConfig;
        specialConditions: Attributes.IOfferSpecialConditionsControlConfig;
    }
}