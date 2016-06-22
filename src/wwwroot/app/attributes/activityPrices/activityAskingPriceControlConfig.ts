/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IActivityAskingPriceControlConfig extends Antares.Common.Models.Dto.IControlConfig {
		askingPrice: Antares.Common.Models.Dto.IFieldConfig;
    }
}