/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IOfferSpecialConditionsControlConfig extends Common.Models.Dto.IControlConfig {
        specialConditions: Common.Models.Dto.IFieldConfig;
    }
}