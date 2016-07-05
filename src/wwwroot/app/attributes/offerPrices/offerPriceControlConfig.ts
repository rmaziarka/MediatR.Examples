/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IOfferPriceControlConfig extends Common.Models.Dto.IControlConfig {
        price: Common.Models.Dto.IFieldConfig;
    }
}