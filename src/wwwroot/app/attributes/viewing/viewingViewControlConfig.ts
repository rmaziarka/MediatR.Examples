/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IViewingViewControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        viewings: Antares.Common.Models.Dto.IFieldConfig;
    }
}