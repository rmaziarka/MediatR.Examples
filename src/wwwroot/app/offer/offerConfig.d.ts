/// <reference path="../typings/_all.d.ts" />

declare module Antares.Offer {
    interface IOfferConfig {
        [key: string]: any;
        offer_StatusId: Attributes.IOfferStatusControlConfig;
        offer_OfferDate: Attributes.IOfferOfferDateControlConfig;
        offer_CompletionDate: Attributes.IOfferCompletionDateControlConfig;
        offer_ExchangeDate: Attributes.IOfferExchangeDateControlConfig;
        offer_PricePerWeek: Attributes.IOfferPricePerWeekControlConfig;
        offer_Price: Attributes.IOfferPriceControlConfig;
        offer_SpecialConditions: Attributes.IOfferSpecialConditionsControlConfig;
    }
}