/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IOfferLenderControlConfig extends Common.Models.Dto.IControlConfig {
        lenderId: Common.Models.Dto.IFieldConfig;
    }
}