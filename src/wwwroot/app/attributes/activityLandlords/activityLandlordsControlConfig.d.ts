/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IActivityLandlordsControlConfig extends Antares.Common.Models.Dto.IControlConfig  {
        landlords: Antares.Common.Models.Dto.IFieldConfig;
    }
}