/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IOfferViewControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        offers: Antares.Common.Models.Dto.IFieldConfig;
    }
}