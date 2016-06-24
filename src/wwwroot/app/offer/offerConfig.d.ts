/// <reference path="../typings/_all.d.ts" />

declare module Antares.Offer {
    interface IOfferConfig {
        [key: string]: any;
        offerStatus: Attributes.IOfferStatusEditFieldConfig;
    }
}